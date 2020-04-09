using BaseArchitect.CacheManager;
using BaseArchitect.Constants;
using BaseArchitect.Core.CustomException;
using BaseArchitect.DTO.Param;
using BaseArchitect.Entities;
using BaseArchitect.Services;
using BaseArchitect.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using BaseArchitect.AuthenticationServices.Authentication;

namespace BaseArchitect.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region Fields

        private readonly AccountServices _AccountServices = new AccountServices();

        private readonly AuthenticationBiz _AuthServices = new AuthenticationBiz();

        private readonly RoleServices _RoleServices = new RoleServices();

        #endregion

        #region Methods

        /// <summary>
        /// TODO: Đăng nhập
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public AuthenticationDto Login(AuthenticationDto param)
        {
            // get account
            Account account = _AuthServices.Login(param.Email, param.Password);
            RefreshToken reToken = _AuthServices.GetRefreshToken(account.AccountID);

            // cache profile
            ProfileCaching.CacheProfile(account, _RoleServices.GetRolesByAccount(account.AccountID));

            // generate token
            string token = GenerateToken(account);

            // return response
            AuthenticationDto response = new AuthenticationDto();
            response.AccessToken = token;
            response.RefreshToken = reToken.ReToken.Trim();
            //response.Account = account;

            return response;
        }

        [HttpPost]
        [Route("refresh")]
        public AuthenticationDto Refresh(AuthenticationDto param)
        {          
            Account account = _AccountServices.GetByID(ProfileCaching.Profile().AccountID);
            RefreshToken reToken = _AuthServices.GetRefreshToken(ProfileCaching.Profile().AccountID);
            
            if (param.RefreshToken.Trim() != reToken.ReToken.Trim())
            {
                throw new AuthenticationException("Refresh token is not valid!");
            }

            RefreshToken newRefreshToken = null;

            TimeSpan diff = DateTime.Now - reToken.ExpiredDate;
            if (diff.TotalMinutes >= 0)
            {
                // hết hạn refresh token
                //throw new TokenExpiredException("Refresh token is expired!");
                // cập nhật token
                newRefreshToken = _AuthServices.UpdateRefreshToken(ProfileCaching.Profile().AccountID);
            }

            // làm mới access token
            // generate token
            string token = GenerateToken(account);

            // return response
            AuthenticationDto response = new AuthenticationDto();
            response.RefreshToken = newRefreshToken.ReToken;
            response.AccessToken = token;

            return response;
        }

        private string GenerateToken(Account account)
        {
            // payload
            var claims = new Claim[]
            {
                new Claim(PCS.UserToken.Payload.AccountID, account.AccountID.ToString()),
                new Claim(PCS.UserToken.Payload.Email, account.Email)
            };

            // config
            JwtSecurityToken token = new JwtSecurityToken
            (
                issuer: PCS.UserToken.TokenInfo.TOKEN_ISSUER,
                audience: PCS.UserToken.TokenInfo.TOKEN_AUDIENCE,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(PCS.UserToken.AccessTokenExpiryTime),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.Default.GetBytes(PCS.UserToken.TokenInfo.TOKEN_SECRET_KEY)), SecurityAlgorithms.HmacSha256Signature)
            );

            // write token string
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string tokenString = handler.WriteToken(token);

            return tokenString;
        }

        #endregion
    }
}
