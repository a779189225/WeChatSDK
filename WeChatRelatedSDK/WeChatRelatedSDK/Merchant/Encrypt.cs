using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WeChatRelatedSDK.Merchant
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    public class Encrypt
    {
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="associatedData">附加数据包（可能为空）</param>
        /// <param name="nonce">加密使用的随机串初始化向量</param>
        /// <param name="ciphertext">Base64编码后的密文</param>
        /// <param name="AES_KEY">支付V3密钥</param>
        /// <returns></returns>
        public static string AesGcmDecrypt(string associatedData, string nonce, string ciphertext, string AES_KEY)
        {
            GcmBlockCipher gcmBlockCipher = new(new AesEngine());
            AeadParameters aeadParameters = new(
                new KeyParameter(Encoding.UTF8.GetBytes(AES_KEY)),
                128,
                Encoding.UTF8.GetBytes(nonce),
                Encoding.UTF8.GetBytes(associatedData));
            gcmBlockCipher.Init(false, aeadParameters);

            byte[] data = Convert.FromBase64String(ciphertext);
            byte[] plaintext = new byte[gcmBlockCipher.GetOutputSize(data.Length)];
            int length = gcmBlockCipher.ProcessBytes(data, 0, data.Length, plaintext, 0);
            gcmBlockCipher.DoFinal(plaintext, length);
            return Encoding.UTF8.GetString(plaintext);
        }


        /// <summary>
        /// 最终提交请求时，需对敏感信息加密，如身份证、银行卡号。
        /// 加密算法是RSA，使用从接口下载到的公钥进行加密，非后台下载到的私钥。
        /// 感谢ISV(https://github.com/ndma-isv)提供示例
        /// 
        /// </summary>
        /// <param name="text">要加密的明文</param>
        /// <param name="publicKey"> -----BEGIN CERTIFICATE----- 开头的string，转为bytes </param>
        /// <returns></returns>
        public static string RSAEncrypt(string text, string publicKey)
        {

            byte[] pkey = Encoding.UTF8.GetBytes(publicKey);
            var x509 = new X509Certificate2(pkey);
            var rsaParam = x509.GetRSAPublicKey().ExportParameters(false);
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParam);
            var buff = rsa.Encrypt(Encoding.UTF8.GetBytes(text), true);
            return Convert.ToBase64String(buff);
            //using (var x509 = new X509Certificate2(publicKey))
            //{
            //    using (var rsa = (RSACryptoServiceProvider)x509.PublicKey.Key)
            //    {
            //        var buff = rsa.Encrypt(Encoding.UTF8.GetBytes(text), true);

            //        return Convert.ToBase64String(buff);
            //    }
            //}
        }
    }
}
