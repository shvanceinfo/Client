using UnityEngine;
using System.Collections;

namespace model
{
    //聊天内容类型
    public enum TalkType
    { 
        Error=0,
        World,
        Guild,
        Whisper,
        System,
        Post,
        SystemAndPost,
    }

    /// <summary>
    /// 文本链接类型
    /// </summary>
    public enum LinkType : byte
    { 
        None,
        /// <summary>
        /// 玩家名称
        /// </summary>
        NameLink,

        /// <summary>
        /// 物品链接
        /// </summary>
        ItemLink,
        /// <summary>
        /// 功能链接
        /// </summary>
        FunctionLink,
    }

    public class TalkVo
    {
        /// <summary>
        /// 类型
        /// </summary>
        public TalkType Type { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
