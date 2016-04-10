using UnityEngine;
using System.Collections;

namespace model
{
    public enum EmailState
    { 
        /// <summary>
        /// 没阅读
        /// </summary>
        NotRead,

        Read,
        /// <summary>
        /// 以领取
        /// </summary>
        Receive,
        /// <summary>
        /// 未领取
        /// </summary>
        NotReceive,    
    }
    public class EmailVo
    {
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 奖励物品
        /// </summary>
        public BetterList<IdStruct> AwardItems { get; set; }

        /// <summary>
        /// 邮件状态
        /// </summary>
        public EmailState State { get; set; }

        public EmailState AwardState { get; set; }
        /// <summary>
        /// 是否有奖励，用于标示邮件状态
        /// </summary>
        public bool IsHaveAward {
            get {
                if (AwardItems.size>0)
                {
                    if (AwardState != EmailState.Receive)
                    {
                        for (int i = 0; i < AwardItems.size; i++)
                        {
                            if (AwardItems[i].Id==0)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
                return false; 
            }
        }

        public EmailVo()
        {
            AwardItems = new BetterList<IdStruct>();
        }
    }
}
