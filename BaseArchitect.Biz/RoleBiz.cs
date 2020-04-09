
using IDO.Edu.Entities;
using System.Collections.Generic;

namespace IDO.Edu.Biz
{
    public class RoleBiz
    {      
        public List<Role> GetRolesByAccount(int accountID)
        {
            string query = @$"SELECT rol.RoleID, rol.Name
                            FROM Account acc
	                            JOIN AccountRole acr ON acr.AccountID = acc.AccountID
	                            JOIN Role rol on acr.RoleID = rol.RoleID
                            where acc.AccountID = {accountID}";
            return DbProvider.ADO.ExecuteQuery<Role>(query);           
        }

    }
}
