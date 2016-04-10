using UnityEngine;
using System.Collections;


namespace model
{
    public enum FormulaType
    { 
        None=0,
        Type1=1,
    }
    public class FormulaVo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 合成类型ID，用于以后扩展页签
        /// </summary>
        public FormulaType Type { get; set; }

		/// <summary>
		/// 合成材料ID
		/// </summary>
		/// <value>The material I.</value>
		public int  MaterialID {set;get;}

		/// <summary>
		/// 合成材料数量
		/// </summary>
		/// <value>The material number.</value>
		public int MaterialNum { set;get;}

        /// <summary>
        /// 排序ID
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 消耗材料ID,value1=数量 value2=替代钻石价格
        /// </summary>
        public BetterList<IdStruct> ConsumeItem { get; set; }

        /// <summary>
        /// 消耗金币数量
        /// </summary>
        public int ConsumeGold { get; set; }

        /// <summary>
        /// 合成概率 1/1000
        /// </summary>
        public int Successrate { get; set; }

        /// <summary>
        /// 是否可以使用幸运石
        /// </summary>
        public bool IsUsedLuckStone { get; set; }

        /// <summary>
        /// 合成出物品的ID
        /// </summary>
        public int MergeNextId { get; set; }

        /// <summary>
        /// 能合成的数量
        /// </summary>
        public int CanMergeCount { get; set; }

        /// <summary>
        /// 公告ID
        /// </summary>
        public int GonggaoID { get; set; }

        public FormulaVo()
        {
            ConsumeItem = new BetterList<IdStruct>();
        }
    }

}

