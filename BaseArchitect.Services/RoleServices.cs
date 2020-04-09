using BaseArchitect.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Linq;
using BaseArchitect.DAL;

namespace BaseArchitect.Services
{
    public class RoleServices
    {
        public List<Role> GetRolesByAccount(int accountID)
        {
            string query = @$"SELECT rol.RoleID, rol.Name
                            FROM Account acc
	                            JOIN AccountRole acr ON acr.AccountID = acc.AccountID
	                            JOIN Role rol on acr.RoleID = rol.RoleID
                            where acc.AccountID = {accountID}";
            return DAO.ADO.ExecuteQuery<Role>(query);
        }
    }
}
