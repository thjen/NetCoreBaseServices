using IDO.Edu.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace IDO.Edu.Biz
{
    public class AccountBiz
    {
        public List<Account> GetAllAccount()
        {

            return DbProvider.EF.Account.ToList();
        }
    }
}
