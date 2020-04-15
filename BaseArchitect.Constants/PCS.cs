using System;
using System.Collections.Generic;
using System.Text;

namespace BaseArchitect.Constants
{
    public class PCS
    {
        public const int WebPageCount = 100;
        public const int MobilePageCount = 20;      

        public class MiddlewareContextItem
        {
            public const string AccountProfile = "AccountProfile ";
        }

        public class UserToken
        {
            public const int RefreshTokenExpiryTime = 5; // refresh token 600' = 10h làm việc
            public const int AccessTokenExpiryTime = 3; // access token 1'

            public class TokenInfo
            {
                public const string TOKEN_SECRET_KEY = "293uaf2834582kqIS0923SISF09234jlsk09234ljslflsoi234029aaf1ja2f4q1lafaasfajpw34230duadlfanaihiw3ru209347adfhzclzflqi3r20924hgqqad";
                public const string TOKEN_ISSUER = "IDO-EDU";
                public const string TOKEN_AUDIENCE = "IDO-EDU";
                public const string AUTHENTICATION_TYPE = "Bearer";
            }

            public class Payload
            {
                public const string AccountID = "AccountID";
                public const string Email = "Email";
            } 
        }     

        public class ActiveStatus
        {
            public const int Active = 1;
            public const int InActive = 0;
        }

        public class Status
        {
            public const int Pending = 1;
            public const int Checkout = 2;
            public const int Cancel = 3;

            public static readonly Dictionary<int, string> dicStatus = new Dictionary<int, string>()
            {
                { Pending,  "Đang chờ" },
                { Checkout, "Đã thanh toán" },
                { Cancel,   "Đã hủy" },
            };
        }
    }
}
