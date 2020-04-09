using IDO.Edu.Constants;
using IDO.Edu.DTO.Param;
using IDO.Edu.Entities;
using IDO.Edu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDO.Edu.Biz
{
    public class AuthenticationBiz
    {
        public Account Login(string email, string password)
        {
            Account accExists = DbProvider.EF.Account.Select(a => new Account()
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
    }
}
