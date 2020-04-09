using BaseArchitect.API.Config;
using BaseArchitect.CacheManager;
using BaseArchitect.DTO.Param;
using BaseArchitect.Services;
using Microsoft.AspNetCore.Mvc;

namespace BaseArchitect.API.Controllers
{
    [Route("api/account")]  
    [ApiController]
    [AuthorizeAttribute]
    public class AccountController : BaseController
    {
        private readonly AccountServices _AccountServices = new AccountServices();
       
        [HttpPost]
        [Route("GetAccounts")]      
        public AccountDto GetAccounts()
        {
            var userprofile = ProfileCaching.Profile();
            var res = new AccountDto();
            res.Accounts = _AccountServices.GetAllAccount();
            return res;
        }

        [HttpPost]
        [Route("GetAccountDetail")]
        public AccountDto GetAccountDetail()
        {
            var res = new AccountDto();
            //res.Accounts = accountSer.GetAccounts();
            return res;
        }
    }
}