using System;
using WeChatRelatedSDK.Model;

namespace WeChatRelatedSDK.Merchant
{
    /// <summary>
    /// 证书操作类
    /// </summary>
    public class Certificate
    {
        /// <summary>
        /// 启用最新证书
        /// </summary>
        /// <param name="resultData"></param>
        /// <returns></returns>
        public static data LatestCer(CertificatesResponse resultData)
        {
            int index = 0;
            data data = new data();
            //加密请求消息中的敏感信息时，使用最新的平台证书（即：证书启用时间较晚的证书）
            if (resultData.data.Count > 1)
            {
                if (Convert.ToDateTime(resultData.data[0].effective_time) > Convert.ToDateTime(resultData.data[1].effective_time))
                {
                    index = 0;
                }
                else
                {
                    index = 1;
                }
            }
            data = resultData.data[index];
            return data;
        }

    }
}
