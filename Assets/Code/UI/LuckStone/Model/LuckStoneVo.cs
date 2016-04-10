using UnityEngine;
using System.Collections;

namespace model
{
    /// <summary>
    /// 强化石
    /// </summary>
    public class LuckStoneVo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 强化成功率1/10000
        /// </summary>
        public int Successrate { get; set; }

        /// <summary>
        /// 消耗物品
        /// </summary>
        public BetterList<IdStruct> ConsumeItem { get; set; }

        /// <summary>
        /// 消耗钻石
        /// </summary>
        public int ConsumeDiamond { get; set; }

        public LuckStoneVo()
        {
            ConsumeItem = new BetterList<IdStruct>();
        }
    }
}
