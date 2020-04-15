using BaseArchitect.DAL;
using BaseArchitect.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseArchitect.AuthenticationServices.AuthRole
{
    public class AuthRoleBiz
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
