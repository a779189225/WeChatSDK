using System.Collections.Generic;

namespace WeChatRelatedSDK.Model
{
    /// <summary>
    /// 返回成功数据
    /// </summary>
    public class CertificatesResponse
    {
        public List<data> data { get; set; }
    }
    /// <summary>
    /// 返回成功数据
    /// </summary>
    public class data
    {
        /// <summary>
        /// 证书启用时间
        /// </summary>
        public string effective_time { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public encrypt_certificateData encrypt_certificate { get; set; }
        /// <summary>
        /// 证书弃用时间
        /// </summary>
        public string expire_time { get; set; }
        /// <summary>
        /// 平台证书序列号
        /// </summary>
        public string serial_no { get; set; }
    }

    /// <summary>
    /// 返回成功数据详情
    /// </summary>
    public class encrypt_certificateData
    {
        /// <summary>
        /// 加密证书的算法
        /// </summary>
        public string algorithm { get; set; }
        /// <summary>
        /// 关联数据
        /// </summary>
        public string associated_data { get; set; }
        /// <summary>
        /// 加密后的证书内容
        /// </summary>
        public string ciphertext { get; set; }
        /// <summary>
        /// 加密后的随机串
        /// </summary>
        public string nonce { get; set; }
    }

}
