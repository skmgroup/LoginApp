using System;
using System.IO;
using System.Security.Cryptography;
using System.Web.Mvc;
using LoginApp.Cryptography;
using LoginApp.ViewModel;

namespace LoginApp.Controllers
{
    [Authorize]
    public class WelcomeController : Controller
    {
        [HttpGet]
        public ActionResult Index() => View("Index", new EncryptForm());

        [HttpPost]
        public ActionResult Encrypt(EncryptForm form)
        {
            var encryptor = new Encryptor();
            form.Encrypted = encryptor.AesEncrypt(form.Message);
            form.Decrypted = encryptor.AesDecrypt(form.Encrypted);
            return View("Index", form);
        }
    }
}