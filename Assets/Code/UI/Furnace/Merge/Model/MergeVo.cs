using UnityEngine;
using System.Collections;
using System;

namespace model
{
    /// <summary>
    /// 宝石类型
    /// </summary>
    public enum GemType
    { 
        None=0,
        /// <summary>
        /// 攻击
        /// </summary>
        Attack,

        /// <summary>
        /// 防御
        /// </summary>
        Defense,

        /// <summary>
        /// 暴击
        /// </summary>
        Crit,

        /// <summary>
        /// 韧性
        /// </summary>
        CritResistance,

        /// <summary>
        /// 冰伤害
        /// </summary>
        IceAttack,

        /// <summary>
        /// 冰抗性
        /// </summary>
        IceResistance,

        /// <summary>
        /// 火伤害
        /// </summary>
        FireAttack,
        /// <summary>
        /// 火抗性
        /// </summary>
        FireResistance,

        /// <summary>
        /// 毒伤害
        /// </summary>
        PoisonAttack,

        /// <summary>
        /// 毒抗性
        /// </summary>
        PoisonResistance,

        /// <summary>
        /// 雷伤害
        /// </summary>
        ThunderAttack,

        /// <summary>
        /// 雷抗性
        /// </summary>
        ThunderResistance,

    }

    /// <summary>
    /// 属性：[类型][值]
    /// </summary>
    [Serializable]
    public struct AttributeValue
    {
        public eFighintPropertyCate Type { get; set; }
        public int Value { get; set; }
    }
    /// <summary>
    /// 宝石
    /// </summary>
    public class GemVo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 宝石类型
        /// </summary>
        public GemType Type { get; set; }

        /// <summary>
        /// 宝石等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 宝石属性
        /// </summary>
        public BetterList<AttributeValue> Attribute { get; set; }

        /// <summary>
        /// 可装备位置
        /// </summary>
        public BetterList<eEquipPart> Equips { get; set; }

        /// <summary>
        /// 合成成功率  1/1000
        /// </summary>
        public int Successrate { get; set; }

        /// <summary>
        /// 合成消耗数量
        /// </summary>
        public int MergeNum { get; set; }

        /// <summary>
        /// 合成消耗金币
        /// </summary>
        public int MergeGold { get; set; }

        /// <summary>
        /// 是否可以使用幸运石
        /// </summary>
        public bool IsUseLuckStone { get; set; }

        /// <summary>
        /// 合成出的宝石ID
        /// </summary>
        public int MergeNextId { get; set; }

        /// <summary>
        /// 是否可以继续合成
        /// </summary>
        public bool IsMergeGoing { get; set; }

        /// <summary>
        /// 分解消耗的金币
        /// </summary>
        public int ResolvedConsumeGold { get; set; }

        /// <summary>
        /// 是否可以分解
        /// </summary>
        public bool IsResolved { get; set; }

        /// <summary>
        /// 分解获得物品 id,value
        /// </summary>
        public BetterList<IdStruct> ResolvedItems { get; set; }

        /// <summary>
        /// 宝石转化消耗金币
        /// </summary>
        public int GemTransformConsumeGold { get; set; }

        /// <summary>
        /// 宝石转化获得物品ID
        /// </summary>
        public BetterList<int> GemTransformItem { get; set; }


        //自己用的字段

        /// <summary>
        /// 可以合成的数量
        /// </summary>
        public int CanMergeCount { get; set; }

        public GemVo()
        {
            Attribute = new BetterList<AttributeValue>();
            Equips = new BetterList<eEquipPart>();
            ResolvedItems = new BetterList<IdStruct>();
            GemTransformItem = new BetterList<int>();
            CanMergeCount = 0;
        }
    }

    
    
}

