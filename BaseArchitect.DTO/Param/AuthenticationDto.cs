using BaseArchitect.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseArchitect.DTO.Param
{
    public class AuthenticationDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public Account Account { get; set; }

        public List<Role> Roles { get; set; }

        public AuthenticationFilter Filter { get; set; }
    }

    public class AuthenticationFilter
    {
        public int? AccountID { get; set; }
    }
}
