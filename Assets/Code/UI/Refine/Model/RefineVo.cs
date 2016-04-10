using UnityEngine;
using System.Collections;

namespace model
{
    /// <summary>
    /// 洗练
    /// </summary>
    public class RefineVo
    {
        public int this[eFighintPropertyCate index]
        {
            get {
                foreach (AttributeValue a in Attribute)
                {
                    if (a.Type == index)
                        return a.Value;
                }
                return -1;
            }
        }
        /// <summary>
        /// 洗练条目序列
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 洗练属性
        /// </summary>
        public BetterList<AttributeValue> Attribute { get; set; }

        /// <summary>
        /// 洗练消耗
        /// </summary>
        public BetterList<IdStruct> ConsumeItems { get; set; }

        /// <summary>
        /// 重置消耗
        /// </summary>
        public BetterList<IdStruct> ResetConsume { get; set; }

        /// <summary>
        /// 洗练失败获得经验值
        /// </summary>
        public int Exp { get; set; }

        /// <summary>
        /// 解锁等级
        /// </summary>
        public int UnLockLevel { get; set; }

        /// <summary>
        /// 解锁vip
        /// </summary>
        public int UnLockVip { get; set; }

        public RefineVo()
        {
            Attribute = new BetterList<AttributeValue>();
            ConsumeItems = new BetterList<IdStruct>();
            ResetConsume = new BetterList<IdStruct>();
        }
    }

    /// <summary>
    /// 洗练属性详细数据
    /// </summary>
    public class RefineInfoVo
    {
        
        /// <summary>
        /// 属性名称
        /// </summary>
        public eFighintPropertyCate Type { get; set; }

        public int BaseValue { get; set; }

        public RefineVo Vo { get; set; }

        public RefineStatus Status { get; set; }


        public RefineInfoVo()
        {
            Status = RefineStatus.Lock;
        }
    }
    public enum RefineStatus:byte
    { 
        /// <summary>
        /// 未解锁
        /// </summary>
        Lock,
        /// <summary>
        /// 待命
        /// </summary>
        Standby,

        /// <summary>
        /// 解锁
        /// </summary>
        UnLock
    }
}