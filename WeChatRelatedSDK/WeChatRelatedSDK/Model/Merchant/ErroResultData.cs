namespace WeChatRelatedSDK.Model.Merchant
{
    public class ErroResultData
    {
        /// <summary>
        /// 详细错误码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 错误描述，使用易理解的文字表示错误的原因。
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public detail detail { get; set; }
    }

    /// <summary>
    /// 错误详情
    /// </summary>
    public class detail
    {
        /// <summary>
        /// 
        /// </summary>
        public string field { get; set; }
        /// <summary>
        /// 错误的值
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 具体错误原因
        /// </summary>
        public string issue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string location { get; set; }
    }
}
