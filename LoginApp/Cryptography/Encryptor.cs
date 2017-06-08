using System;
using System.IO;
using System.Security.Cryptography;

namespace LoginApp.Cryptography
{
    public class Encryptor
    {
        private readonly RSACryptoServiceProvider _rsaParams;

        private readonly byte[] _aesKeyEncrypted;
        private readonly byte[] _aesIvEncrypted;

        public Encryptor()
        {
            _rsaParams = new RSACryptoServiceProvider(2048);
            var publicKey = _rsaParams.ExportParameters(false);
            _aesKeyEncrypted = RsaEncrypt(GenerateKey(32), publicKey);
            _aesIvEncrypted = RsaEncrypt(GenerateKey(16), publicKey);
        }
                     
        public string AesEncrypt(string message)
        {
            using (var aes = CreateAesManaged())
            {
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

        public string AesDecrypt(string encrypted)
        {
            var bytes = Convert.FromBase64String(encrypted);
            using (var aes = CreateAesManaged())
            {
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
        
        private static byte[] GenerateKey(int length)
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var buff = new byte[length];
                provider.GetBytes(buff);
                return buff;
            }
        }

        private static byte[] RsaEncrypt(byte[] message, RSAParameters publicKeyInfo)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKeyInfo);
                return rsa.Encrypt(message, true);         
            }
        }

        private static byte[] RsaDecrypt(byte[] message, RSAParameters privateKeyInfo)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKeyInfo);
                return rsa.Decrypt(message, true);
            }
        }

        private AesManaged CreateAesManaged()
        {
            return new AesManaged
            {
                KeySize = 256,
                Key = RsaDecrypt(_aesKeyEncrypted, _rsaParams.ExportParameters(true)),
                IV = RsaDecrypt(_aesIvEncrypted, _rsaParams.ExportParameters(true)),
                Padding = PaddingMode.ISO10126
            };
        }
    }
}