using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LoginApp.Users;
using LoginApp.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace LoginApp.Controllers
 {
     public class LoginController : Controller
     {
         private readonly UserManager<User> _userManager = UserManagerFactory.Create();

         [HttpGet]
         [Route("login")]
         public ActionResult Get() => View("Index");
 
         [HttpPost]
         [Route("login")]
         public async Task<ActionResult> Post(LoginForm form, string returnUrl)
         {
             if (!ModelState.IsValid) return View("Index");

             var user = await _userManager.FindAsync(form.Username, form.Password);
             if (user == null)
             {
                 ModelState.AddModelError("username", "Username or password is invalid.");
                 return View("Index");
             }

             var owinContext = HttpContext.GetOwinContext();
             var authManager = owinContext.Authentication;
             var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
             identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
             
             authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
             return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Welcome") as ActionResult;
         }
     }
 }