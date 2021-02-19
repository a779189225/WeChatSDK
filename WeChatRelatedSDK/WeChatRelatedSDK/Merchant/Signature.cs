using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WeChatRelatedSDK.Merchant
{
    /// <summary>
    /// 签名帮助类
    /// https://wechatpay-api.gitbook.io/wechatpay-api-v3/qian-ming-zhi-nan-1/qian-ming-sheng-cheng
    /// </summary>
    public class Signature
    {
        /// <summary>
        /// 返回最终签名
        /// </summary>
        /// <param name="nohttpsurl">获取请求的绝对URL，并去除域名部分得到参与签名的URL。如果请求中有查询参数，URL末尾应附加有'?'和对应的查询字符串。</param>
        /// <param name="method">请求方式</param>
        /// <param name="merchantId">商户号</param>
        /// <param name="serialNo">商户证书序列号</param>
        /// <param name="body">请求主体</param>
        /// <param name="privateKey">支付证书密钥</param>
        /// <returns></returns>
        public static string AuthorizationSign(string nohttpsurl, string method, string merchantId, string serialNo, string body, string privateKey)
        {
            string sign = StructureSign(nohttpsurl, method, merchantId, serialNo, body, privateKey);
            return $"WECHATPAY2-SHA256-RSA2048 {sign}";
        }

        /// <summary>
        /// 构造签名参数
        /// </summary>
        /// <param name="nohttpsurl">获取请求的绝对URL，并去除域名部分得到参与签名的URL。如果请求中有查询参数，URL末尾应附加有'?'和对应的查询字符串。</param>
        /// <param name="method">请求方式</param>
        /// <param name="merchantId">商户号</param>
        /// <param name="serialNo">商户证书序列号</param>
        /// <param name="body">请求主体</param>
        /// <param name="privateKey">支付证书密钥</param>
        /// <returns></returns>
        public static string StructureSign(string nohttpsurl, string method, string merchantId, string serialNo, string body, string privateKey)
        {
            string uri = nohttpsurl;
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            string nonce = Guid.NewGuid().ToString().Replace("-", "");
            string message = $"{method}\n{uri}\n{timestamp}\n{nonce}\n{body}\n";
            string signature = Sign(message, privateKey);
            return $"mchid=\"{merchantId}\",nonce_str=\"{nonce}\",timestamp=\"{timestamp}\",serial_no=\"{serialNo}\",signature=\"{signature}\"";
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="message"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string Sign(string message, string privateKey)
        {
            // NOTE： 私钥不包括私钥文件起始的-----BEGIN PRIVATE KEY-----
            //        亦不包括结尾的-----END PRIVATE KEY-----
            //string key = getpublickey.getkey();
            byte[] keyData = Convert.FromBase64String(privateKey);
            using (CngKey cngKey = CngKey.Import(keyData, CngKeyBlobFormat.Pkcs8PrivateBlob))
            using (RSACng rsa = new RSACng(cngKey))
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                return Convert.ToBase64String(rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            }
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="Signature">私钥加密串-Wechatpay-Signature</param>
        /// <param name="signSourceString">验签名串-应答时间戳\n应答随机串\n应答报文主体\n</param>
        /// <returns></returns>
        public static bool VerifySign(string Signature, string signSourceString, string publickey)
        {
            byte[] pkey = Encoding.UTF8.GetBytes(publickey);
            var x509 = new X509Certificate2(pkey);

            var rsaParam = x509.GetRSAPublicKey().ExportParameters(false);
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParam);

            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                var b = rsa.VerifyData(Encoding.UTF8.GetBytes(signSourceString), sha256, Convert.FromBase64String(Signature));
                return b;
            }
        }
    }
}
