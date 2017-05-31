using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using LoginApp.Users;
using Microsoft.AspNet.Identity;

namespace LoginApp.Controllers
{
    [Authorize(Users = Consts.MasterUser)]
    public class MasterController : Controller
    {
        private readonly UserManager<User> _userManager = UserManagerFactory.Create();
        private readonly UserStore _userStore = new UserStore();
        
        [HttpGet]
        [Route("master")]
        public async Task<ActionResult> Get()
        {
            return View("Index", await GetUsernames());
        }

        [HttpPost]
        [Route("master")]
        public async Task<ActionResult> Post(string username)
        {
            await _userManager.UpdateSecurityStampAsync(username);
            if (username == User.Identity.Name)
            {
                return RedirectToAction("Get", "Login");
            }
            
            return View("Index", await GetUsernames());
        }
        
        private async Task<List<string>> GetUsernames()
        {
            var users = await _userStore.LoadAllUsersAsync();
            return users.Select(u => u.UserName).ToList();
        }
    }
}