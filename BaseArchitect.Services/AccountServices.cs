using BaseArchitect.DAL;
using BaseArchitect.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace BaseArchitect.Services
{
    public class AccountServices
    {
        #region Methods

        public List<Account> GetAllAccount()
        {
            using (var scope = new TransactionScope())
            {
                var t = DAO.EF.Account.ToList();

                DAO.EF.Role.Add(new Role()
                {
                    Name = "test",
                    Deleted = false
                });

                DAO.EF.SaveChanges();

                var testwithado = new AccountRole()
                {
                    AccountID = 3,
                    RoleID = 1,
                    CreatedBy = "sgnfsdjgnfdjsgnfkjdsngjfdsnibvucxhurbguiebgurbsfdbhvcxjhdngsninugrengjksdgndfbhfsdhfdgbeihgurhegiuhsdjbnjknbjvncxkjbvcnxbjkdnfiusgheuriiseghsdbhjbfkvjnxjkvncjhfuighsfuigrensgbdshbjfhdkjxcnbjkcgskgheiusgheriughsuidbgfhbghsdgbehjrgbiusheieushgjdfbhfdsbghsdbguergjskdjgfdsngjkdfsngjkerneiusbuhrbhsdngkhsbgbgfhgjbfhgbfhsdgbeihgseiurghisudfhgiudfshgiuehruhsieuhgiushdguhdusigh",
                    Deleted = false
                };

                DAO.ADO.ExecuteNonQuery(@$"UPDATE AccountRole(AccountID,RoleID,CreatedBy,Deleted) VALUES({testwithado.AccountID},{testwithado.RoleID},'{testwithado.CreatedBy}',{testwithado.Deleted})");

                //Repo.EF.AccountRole.Add(new AccountRole()
                //{
                //    AccountID = 3,
                //    RoleID = 1,
                //    CreatedBy = "sgnfsdjgnfdjsgnfkjdsngjfdsnibvucxhurbguiebgurbsfdbhvcxjhdngsninugrengjksdgndfbhfsdhfdgbeihgurhegiuhsdjbnjknbjvncxkjbvcnxbjkdnfiusgheuriiseghsdbhjbfkvjnxjkvncjhfuighsfuigrensgbdshbjfhdkjxcnbjkcgskgheiusgheriughsuidbgfhbghsdgbehjrgbiusheieushgjdfbhfdsbghsdbguergjskdjgfdsngjkdfsngjkerneiusbuhrbhsdngkhsbgbgfhgjbfhgbfhsdgbeihgseiurghisudfhgiudfshgiuehruhsieuhgiushdguhdusigh",
                //    Deleted = false
                //});

                //Repo.EF.SaveChanges();

                scope.Complete();
                return t;
            }
        }

        public Account GetByID(int accountID)
        {
            return DAO.EF.Account.FirstOrDefault(e => e.AccountID == accountID);
        }

        #endregion
    }
}
