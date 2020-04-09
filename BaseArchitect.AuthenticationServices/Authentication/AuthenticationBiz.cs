using BaseArchitect.Constants;
using BaseArchitect.DAL;
using BaseArchitect.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BaseArchitect.Utility;

namespace BaseArchitect.AuthenticationServices.Authentication
{
    public class AuthenticationBiz
    {
        #region Methods

        public RefreshToken UpdateRefreshToken(int accountID)
        {
            RefreshToken reToken = DAO.EF.RefreshToken.FirstOrDefault(rt => rt.AccountID == accountID);
            reToken.ReToken = Guid.NewGuid().ToString();
            reToken.ExpiredDate = DateTime.Now.AddMinutes(PCS.UserToken.RefreshTokenExpiryTime);
            DAO.EF.SaveChanges();
            return reToken;
        }

        public RefreshToken AddRefreshToken(int accountID)
        {
            RefreshToken reToken = new RefreshToken()
            {
                AccountID = accountID,
                ReToken = Guid.NewGuid().ToString(),
                ExpiredDate = DateTime.Now.AddMinutes(PCS.UserToken.RefreshTokenExpiryTime)
            };
            DAO.EF.SaveChanges();
            return reToken;
        }

        public RefreshToken GetRefreshToken(int accountID)
        {
            return DAO.EF
                .RefreshToken
                .FirstOrDefault(rt => rt.AccountID == accountID);
        }

        public Account Login(string email, string password)
        {
            Account accExists = DAO.EF.Account.Select(a => new Account()
            {
                AccountID = a.AccountID,
                Name = a.Name,
                Email = a.Email,
                Password = a.Password
            }).FirstOrDefault(a => a.Email == email);

            if (accExists == null)
            {
                throw new Exception("Tài khoản không tồn tại. Vui lòng thử lại.");
            }

            if (accExists.Status == PCS.ActiveStatus.InActive)
            {
                throw new Exception("Tài khoản đã bị khóa. Vui lòng liên hệ bộ phận quản trị để được giúp đỡ");
            }

            string passMD5Encrypt = password.ToMD5Hash();
            if (!passMD5Encrypt.Equals(accExists.Password))
            {
                throw new Exception("Mật khẩu không chính xác. Vui lòng thử lại");
            }

            return accExists;
        }

        #endregion
    }
}
