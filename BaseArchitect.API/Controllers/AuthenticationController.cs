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
using BaseArchitect.API.Config;
using BaseArchitect.AuthenticationServices.AuthAccount;
using BaseArchitect.AuthenticationServices.AuthRole;

namespace BaseArchitect.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        #region Fields

        private readonly AuthAccountBiz _AccountServices = new AuthAccountBiz();

        private readonly AuthenticationBiz _AuthServices = new AuthenticationBiz();

        private readonly AuthRoleBiz _RoleServices = new AuthRoleBiz();

        #endregion

        #region Methods

        /// <summary>
        /// TODO: Đăng nhập
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public AuthenticationDto Login(AuthenticationDto request)
        {
            AuthenticationDto response = new AuthenticationDto();

            // get account
            Account account = _AuthServices.Login(request.Email, request.Password);                     

            // cache profile
            ProfileCaching.CacheProfile(new UserProfile() { AccountID = account.AccountID, Roles = _RoleServices.GetRolesByAccount(account.AccountID) });

            // generate token
            string token = GenerateToken(account);
            response.RefreshToken = _AuthServices.GetRefreshToken(account.AccountID).ReToken.Trim();
            response.AccessToken = token;

            return response;
        }

        [HttpPost]
        [Route("refresh")]
        [AuthorizeAttribute]
        public AuthenticationDto Refresh(AuthenticationDto param)
        {
            int accountID = (HttpContext.Items[PCS.MiddlewareContextItem.AccountProfile] as UserProfile).AccountID;
            Account account = _AccountServices.GetAccountByID(accountID);
            RefreshToken reToken = _AuthServices.GetRefreshToken(accountID);
            
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
                newRefreshToken = _AuthServices.UpdateRefreshToken(accountID);
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
