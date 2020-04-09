using BaseArchitect.CacheManager;
using BaseArchitect.Constants;
using BaseArchitect.Core.CustomException;
using BaseArchitect.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BaseArchitect.API.Config
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAsyncActionFilter
    {        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            HttpRequest request = context.HttpContext.Request;
            
            if (!request.Headers.TryGetValue("Authorization", out var userToken))
            {
                throw new AuthenticationException("No Authentication Header");
            }

            ClaimsPrincipal principal = null;
            SecurityToken token = ParseToken(userToken);
            if (token != null)
            {
                var jwtToken = (JwtSecurityToken)token;
         
                ClaimsIdentity identity = new ClaimsIdentity(jwtToken.Claims);
                principal = new ClaimsPrincipal(identity);
            }

            if (principal == null)
            {
                throw new AuthenticationException("SecurityToken Invalid");
            }
            else
            {
                var claimAccountID = principal.FindFirst(PCS.UserToken.Payload.AccountID);
                int? accountID = Converter.Obj2IntNull(claimAccountID.Value);

                // lấy profile từ cache trên hệ thống
                UserProfile profile = ProfileCaching.GetProfile(accountID.Value);

                // Nếu không có profile, server đã bị reset
                if (profile == null)
                {
                    throw new AuthenticationException("Phiên làm việc kết thúc, vui lòng đăng nhập lại.");
                }
                else
                {
                    await next();
                }
            }
        }

        private SecurityToken ParseToken(string tokenStr)
        {
            tokenStr = tokenStr.Split(' ')[1];
            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(PCS.UserToken.TokenInfo.TOKEN_SECRET_KEY)),
                ValidIssuer = PCS.UserToken.TokenInfo.TOKEN_ISSUER,
                ValidAudience = PCS.UserToken.TokenInfo.TOKEN_AUDIENCE,
                AuthenticationType = PCS.UserToken.TokenInfo.AUTHENTICATION_TYPE,
                ValidateLifetime = true,               
                LifetimeValidator = CustomLifetimeValidator,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,                
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            SecurityToken token = null;

            try
            {
                handler.ValidateToken(tokenStr, validationParameters, out token);                          
            }
            catch (TokenExpiredException authEx) {
                throw new TokenExpiredException("Token is expired!");
            }
            catch (Exception ex)
            {
                throw new AuthenticationException("Validate token fail");
            }

            return token;
        }

        private bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {
            if (expires != null)
            {
                if (expires > DateTime.UtcNow) return true;
                else throw new TokenExpiredException("Token is expired!");               
            }
            throw new TokenExpiredException("Token is expired!");
        }
    }
}
