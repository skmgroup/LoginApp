using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using LoginApp.ViewModel;

namespace LoginApp.Controllers
{
    [Authorize]
    public class WelcomeController : Controller
    {
        private static byte[] AesKey = GenerateKey(32);
        private static byte[] AesIv = GenerateKey(16);

        private static byte[] GenerateKey(int length)
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var buff = new byte[length];
                provider.GetBytes(buff);
                return buff;
            }
        }

        [Route("welcome")]
        [HttpGet]
        public ActionResult Get() => View("Index", new EncryptForm());

        [Route("welcome")]
        [HttpPost]
        public ActionResult Post(EncryptForm form)
        {
            form.Encrypted = AesEncrypt(form.Message);
            form.Decrypted = AesDecrypt(form.Encrypted);
            return View("Index", form);
        }

        private string AesEncrypt(string message)
        {
            using (var aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.Key = AesKey;
                aes.IV = AesIv;
                aes.Padding = PaddingMode.ANSIX923;

                var encryptor = aes.CreateEncryptor();
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(message);
                        }

                        var bytes = memoryStream.ToArray();
                        return Convert.ToBase64String(bytes);
                    }
                }
            }
        }

        private string AesDecrypt(string encrypted)
        {
            var bytes = Convert.FromBase64String(encrypted);
            using (var aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.Key = AesKey;
                aes.IV = AesIv;
                aes.Padding = PaddingMode.ANSIX923;

                var encryptor = aes.CreateDecryptor();
                using (var memoryStream = new MemoryStream(bytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cryptoStream))
                        {
                            var decrypted = reader.ReadToEnd();
                            return decrypted;
                        }
                    }
                }
            }
        }
    }
}