using UnityEngine;
using System.Collections;
using System;

namespace model
{
    public class PlotVo {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        /// <summary>
        /// 关卡解锁等级
        /// </summary>
        public int UnLockLevel { get; set; }

        /// <summary>
        /// 扫荡一次时间
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 当前扫荡时间计时
        /// </summary>
        public DateTime CurTime { get; set; }

        /// <summary>
        /// 消耗体力
        /// </summary>
        public int ComsumeStrength { get; set; }


        public BetterList<int> Awards { get; set; }

        public PlotVo()
        {
            Awards = new BetterList<int>();
            IsReceive = false;
            IsRaids = false;
            IsKilled = false;
            IsCurKilled = false;
        }

        /// <summary>
        /// 是否领取奖励YES=可以领取奖励,NO=没有奖励
        /// </summary>
        public bool IsReceive { get; set; }

        /// <summary>
        /// 是否在扫荡
        /// </summary>
        public bool IsRaids { get; set; }

        /// <summary>
        /// 历史是否以通关过
        /// </summary>
        public bool IsKilled { get; set; }

        /// <summary>
        /// 是否解锁
        /// </summary>
        public bool IsUnLock {
            get {
                if (CharacterPlayer.character_property.getLevel()>=UnLockLevel)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 今日是否通关过
        /// </summary>
        public bool IsCurKilled { get; set; }
    }
    
}
