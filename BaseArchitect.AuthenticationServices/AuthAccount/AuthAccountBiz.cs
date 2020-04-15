using BaseArchitect.DAL;
using BaseArchitect.Entities;
using System.Linq;

namespace BaseArchitect.AuthenticationServices.AuthAccount
{
    public class AuthAccountBiz
    {
        public Account GetAccountByID(int accountID)
        {
            return DAO.EF.Account.FirstOrDefault(e => e.AccountID == accountID);
        }

    }
}
