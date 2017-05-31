using System.Web.Mvc;
using LoginApp.Users;
using Microsoft.AspNet.Identity;

namespace LoginApp.Controllers
{
    [Authorize(Users = Consts.MasterUser)]
    public class PasswordController : Controller
    {
        private readonly UserManager<User> _userManager = UserManagerFactory.Create(); 

        [HttpGet]
        [Route("password")]
        public ActionResult Get()
        {
            return View("Index", null);
        }
        
        [HttpPost]
        [Route("password")]
        public ActionResult Post(string password)
        {
            return View("Index", model: _userManager.PasswordHasher.HashPassword(password));
        }
    }
}