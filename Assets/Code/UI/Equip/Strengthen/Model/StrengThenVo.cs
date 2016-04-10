using UnityEngine;
using System.Collections;


namespace model
{
    
    /// <summary>
    /// 装备VO
    /// </summary>
    public class EquipmentVo
    {
        public int Id { get; set; }

        public int InstanceId { get; set; }

        public ItemTemplate Item { get; set; }


        /// <summary>
        /// 排序ID
        /// </summary>
        public int SortId { get; set; }
        /// <summary>
        /// 装备位置
        /// </summary>
        public eEquipPart EquipType
        {
            get
            {
                return EquipData.part;
            }
        
        }
        /// <summary>
        /// 装备类型
        /// </summary>
        public ItemInfo Info { get; set; }

        /// <summary>
        /// 装备表
        /// </summary>
        public EquipmentTemplate EquipData { get; set; }

        /// <summary>
        /// 实例装备
        /// </summary>
        public EquipmentStruct InstanceEquipData { get; set; }
        /// <summary>
        /// 战斗力
        /// </summary>
        public int AttackPower { get; set; }

        /// <summary>
        /// 强化等级
        /// </summary>
        public int StrengThenLevel {
            get {
                return (int)InstanceEquipData.intensifyLevel;
            }
        }
        /// <summary>
        /// 下一级的强化数据
        /// </summary>
        public EquipmentForgeVo NextStrengThen { get; set; }


        public EquipmentForgeVo CurStrengThen { get; set; }

        /// <summary>
        /// 进阶到下一级的数据
        /// </summary>
        public ItemTemplate NextAdvancedItem { get; set; }

        /// <summary>
        /// 进阶到下一级数据
        /// </summary>
        public EquipmentTemplate NextAdvanceEquip { get; set; }

        /// <summary>
        /// 装备位置孔数
        /// </summary>
        public EquipmentPartVo Part {
            get {
                foreach (EquipmentPartVo vo in manager.StrengThenManager.Instance.EquipPart.Values)
                {
                    if (vo.Part == EquipType)
                        return vo;
                }
                return null;
            }
        }

        public EquipmentVo()
        {

        }
    }
    /// <summary>
    /// 装备强化拆分表
    /// </summary>
    public class EquipmentForgeVo
    {
        public int Id { get; set; }

        /// <summary>
        /// 强化等级
        /// </summary>
        public int StrengThenLevel { get; set; }

        /// <summary>
        /// 装备位置
        /// </summary>
        public eEquipPart EquipType { get; set; }

        /// <summary>
        /// 职业限制
        /// </summary>
        public CHARACTER_CAREER Career { get; set; }

        /// <summary>
        /// 消耗物品
        /// </summary>
        public BetterList<TypeStruct> ConsumeItem { get; set; }

        /// <summary>
        /// 1/1000 概率
        /// </summary>
        public int Successrate { get; set; }

        /// <summary>
        /// 强化属性
        /// </summary>
        public BetterList<IdStruct> StrengThenValue { get; set; }

        /// <summary>
        /// 是否可以使用幸运石
        /// </summary>
        public bool IsUsedLuckStone { get; set; }

        /// <summary>
        /// 拆分获得的道具
        /// </summary>
        public BetterList<IdStruct> SplitItems { get; set; }

        /// <summary>
        /// 拆分消耗金币
        /// </summary>
        public int SplitConsumeGold { get; set; }

        public EquipmentForgeVo()
        {
            ConsumeItem = new BetterList<TypeStruct>();
            StrengThenValue = new BetterList<IdStruct>();
            SplitItems = new BetterList<IdStruct>();
        }
    }

    /// <summary>
    /// 装备位
    /// </summary>
    public class EquipmentPartVo
    {
        public int Id { get; set; }
        /// <summary>
        /// 装备位置
        /// </summary>
        public eEquipPart Part { get; set; }

        public CHARACTER_CAREER Career { get; set; }

        /// <summary>
        /// 宝石镶嵌数量
        /// </summary>
        public int MaxInlaySize { get; set; }

        /// <summary>
        /// 宝石孔解锁等级,下标为孔的索引
        /// </summary>
        public BetterList<int> UnLockLevel { get; set; }

        /// <summary>
        /// 宝石孔VIP解锁,下标为孔的索引
        /// </summary>
        public BetterList<int> UnLockVip { get; set; }

        public EquipmentPartVo()
        {
            Career = CHARACTER_CAREER.CC_BEGIN;
            UnLockLevel = new BetterList<int>();
            UnLockVip = new BetterList<int>();
        }
    }

}

    