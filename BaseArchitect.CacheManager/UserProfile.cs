using BaseArchitect.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseArchitect.CacheManager
{
    public class UserProfile
    {
        public int AccountID { get; set; }              

        public List<Role> Roles { get; set; }     
    }
}
