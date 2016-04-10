using UnityEngine;
using System.Collections.Generic;
using System;

namespace model
{
    public class VipVo
    {
        /// <summary>
        /// VIP等级
        /// </summary>
        public uint VipId { get; set; }

        /// <summary>
        /// 升级价格
        /// </summary>
        public uint Price { get; set; }

        /// <summary>
        /// 礼包ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 礼包物品
        /// </summary>
        public LootVo LootItem { get; set; }

        /// <summary>
        /// 模型路径
        /// </summary>
        public string ModelPath { get; set; }

        public Vector3 ModelPosition { get; set; }

        public Vector3 ModelRotation { get; set; }

        /// <summary>
        /// 增加的战斗力
        /// </summary>
        public int VipPower { get; set; }

        /// <summary>
        /// 显示VIP图片
        /// 键=显示的序列
        /// 值=显示方式
        /// </summary>
        public Dictionary<int,VipValue> VipPictures { get; set; }

        /// <summary>
        /// 显示VIP属性加成
        /// 键=序列
        /// 值=类型，值
        /// </summary>
        public Dictionary<int,AttributeValue> Attributes { get; set; }


        /// <summary>
        /// 特权集合
        /// Key=属性名
        /// 值
        /// </summary>
        public Dictionary<string,Privilege> Privileges { get; set; }

        /// <summary>
        /// 礼包是否以领取
        /// </summary>
        public VipState IsReceive { get; set; }

        public VipVo()
        {
            IsReceive =VipState.None;
            VipPictures = new Dictionary<int, VipValue>();
            for (int i = 0; i < Constant.VIP_PIC_LEN; i++)
            {
                VipPictures.Add(i, new VipValue());
            }
            Attributes = new Dictionary<int, AttributeValue>();
            Privileges = new Dictionary<string, Privilege>();

            ModelPosition = new Vector3();
            ModelRotation = new Vector3();
        }
    }

    public enum VipState : byte
    { 
        /// <summary>
        /// 不可领取，且未领取,单纯显示
        /// </summary>
        None=0,

        /// <summary>
        /// 已经领取了
        /// </summary>
        Receiveed,

        /// <summary>
        /// 未领取
        /// </summary>
        CanReveive,
        
    }

    /// <summary>
    /// 显示VIP图片类型
    /// </summary>
    public enum VipPicType:byte
    { 
        /// <summary>
        /// 不显示
        /// </summary>
        None,
        /// <summary>
        /// 显示图片
        /// </summary>
        Pic,
        /// <summary>
        /// 显示数字战斗力
        /// </summary>
        Number
    }

    /// <summary>
    ///  用于如何显示VIP图片
    /// </summary>
    public class VipValue
    {
        /// <summary>
        /// Tip类型
        /// </summary>
        public VipPicType TipType { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string TextureName { get; set; }

        /// <summary>
        /// Tip值
        /// </summary>
        public string TipValue { get; set; }
    }

    /// <summary>
    /// 后缀类型
    /// </summary>
    public enum SuffixType : byte
    { 
        /// <summary>
        /// 百分比 %
        /// </summary>
        Percent,
        /// <summary>
        /// 真/假
        /// </summary>
        Boolean,

        /// <summary>
        /// 加号 +
        /// </summary>
        Add,
        /// <summary>
        /// 天数 天
        /// </summary>
        Day,

        /// <summary>
        /// 次数，次
        /// </summary>
        Count,

        /// <summary>
        /// 秒
        /// </summary>
        Second,

        /// <summary>
        /// 显示数字
        /// </summary>
        Number,
    }

    /// <summary>
    /// 特权值
    /// </summary>
    public class Privilege
    {
        /// <summary>
        /// 后缀类型
        /// </summary>
        public SuffixType Type { get; set; }

        public string Value { get; set; }

        public int GetInt()
        {
            return int.Parse(Value);
        }
        public bool GetBoolean()
        {
            return Convert.ToBoolean(int.Parse(Value));
        }
    }

    /// <summary>
    /// 特权表
    /// </summary>
    public class PrivilegeVo
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 排序ID
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 特权描述
        /// </summary>
        public string Disction { get; set; }


        /// <summary>
        /// 特权对应的Key
        /// </summary>
        public string Key { get; set; }
    }

    public class AttributeKeyValue
    {
        public eFighintPropertyCate Type { get; set; }
        private BetterList<int> _values;

        public BetterList<int> Values
        {
            get { return _values; }
            set { _values = value; }
        }
        public AttributeKeyValue(int initSize)
        {
            _values = new BetterList<int>();
            for (int i = 0; i < initSize; i++)
                _values.Add(0);
        }
    }
}