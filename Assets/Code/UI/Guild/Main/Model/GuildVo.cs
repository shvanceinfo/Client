using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using helper;
using System.Timers;
using System;

namespace model
{
    /// <summary>
    /// 公会基本表
    /// </summary>
    public class GuildBaseVo
    {
        /// <summary>
        /// 公会ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 公会等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 最大成员上限
        /// </summary>
        public int MaxMember { get; set; }

        /// <summary>
        /// 公会每个职位的最大数量
        /// </summary>
        public BetterList<OfficeMember> Offices { get; set; }

        /// <summary>
        /// 公会旗帜等级上限
        /// </summary>
        public int MaxFlagLevel { get; set; }

        /// <summary>
        /// 公会商店上限
        /// </summary>
        public int MaxShopLevel { get; set; }

        /// <summary>
        /// 公会技能等级上限
        /// </summary>
        public int MaxSkillLevel { get; set; }

        /// <summary>
        /// 公会最大申请条目上限
        /// </summary>
        public int MaxAcceptCount { get; set; }

        /// <summary>
        /// 低，中，高，捐献物品集合
        /// </summary>
        public Dictionary<DonateLevel,TypeStruct> Donates { get; set; }

        /// <summary>
        /// 低，中，高，奖励物品集合
        /// </summary>
        public Dictionary<DonateLevel,BetterList<TypeStruct>> Awards { get; set; }

        /// <summary>
        /// 公会事件ID
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// 公告ID
        /// </summary>
        public int PostId { get; set; }


        public GuildBaseVo()
        {
            Offices = new BetterList<OfficeMember>();
            Donates = new Dictionary<DonateLevel, TypeStruct>();
            Awards = new Dictionary<DonateLevel, BetterList<TypeStruct>>();
            Awards.Add(DonateLevel.Low, new BetterList<TypeStruct>());
            Awards.Add(DonateLevel.Middle, new BetterList<TypeStruct>());
            Awards.Add(DonateLevel.High, new BetterList<TypeStruct>());
        }
    }


    /// <summary>
    /// 公会职位
    /// </summary>
    public class GuildOffcerVo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 官职名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限等级
        /// </summary>
        public GuildOffice OfficeLevel { get; set; }

        /// <summary>
        /// 成员审核权限True=拥有
        /// </summary>
        public bool CptCheckApply { get; set; }

        /// <summary>
        /// 修改公告权限
        /// </summary>
        public bool CptChangPost { get; set; }

        /// <summary>
        /// 修改官职权限 Size=0, 代表无任何权限
        /// </summary>
        public BetterList<GuildOffice> ChangOffices { get; set; }

        /// <summary>
        /// 发送招收成员信息权限
        /// </summary>
        public bool CptSendMsg { get; set; }

        /// <summary>
        /// 发送招收成员信息消耗道具
        /// </summary>
        public IdStruct SendMsgConsume { get; set; }

        /// <summary>
        /// 解散公会权限 否就是离开公会
        /// </summary>
        public bool CptDestroyGuild { get; set; }

        /// <summary>
        /// 踢人公会权限
        /// </summary>
        public bool CptKickingGuild { get; set; }

        /// <summary>
        /// 召唤恶魔权限
        /// </summary>
        public bool CptEventDemon { get; set; }

        /// <summary>
        /// 公会魔神改时间权限
        /// </summary>
        public bool CptBossTimeChang { get; set; }

        /// <summary>
        /// 公会大厅升级权限
        /// </summary>
        public bool CptCenterUp { get; set; }

        /// <summary>
        /// 公会旗帜加速权限
        /// </summary>
        public bool CptCenterSpeedUp { get; set; }

        /// <summary>
        /// 公会旗帜升级权限
        /// </summary>
        public bool CptFlagUp { get; set; }

        /// <summary>
        /// 公会旗帜加速权限
        /// </summary>
        public bool CptFlagSpeedUp { get; set; }

        /// <summary>
        /// 公会技能升级权限
        /// </summary>
        public bool CptSkillUp { get; set; }

        /// <summary>
        /// 公会技能加速权限
        /// </summary>
        public bool CptSkillSpeedUp { get; set; }

        /// <summary>
        /// 公会技能研究权限
        /// </summary>
        public bool CptSkillStudy { get; set; }

        /// <summary>
        /// 公会事件ID
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// 公告ID
        /// </summary>
        public int PostId { get; set; }

        public GuildOffcerVo()
        {
            ChangOffices = new BetterList<GuildOffice>();
        }
    }

    public enum GuildFlagType
    {
        None = 0,
        GuildFlag
    }

    /// <summary>
    /// 公会旗帜
    /// </summary>
    public class GuildFlagVo
    {
        public int Id { get; set; }

        /// <summary>
        /// 公会旗帜名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 旗帜图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 旗帜等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 旗帜基本属性
        /// </summary>
        public BetterList<AttributeValue> Attrubutes { get; set; }

        /// <summary>
        /// 公会各职位附加属性
        /// </summary>
        public Dictionary<GuildOffice,BetterList<AttributeValue>> OfficeAttrs { get; set; }

        /// <summary>
        /// 公会旗帜升级消耗,资源，道具
        /// </summary>
        public BetterList<TypeStruct> FlagLvlupConsume { get; set; }

        /// <summary>
        /// 升级冷却时间
        /// </summary>
        public int FlagUpTime { get; set; }

        /// <summary>
        /// 当前等级升级描述
        /// </summary>
        public string CurLevelDesc { get; set; }

        /// <summary>
        /// 下一级升级描述
        /// </summary>
        public string NextLevelDesc { get; set; }


        public GuildFlagVo()
        {
            Attrubutes = new BetterList<AttributeValue>();
            OfficeAttrs = new Dictionary<GuildOffice, BetterList<AttributeValue>>();
            FlagLvlupConsume = new BetterList<TypeStruct>();
        }
    }

    /// <summary>
    /// 公会大厅
    /// </summary>
    public class GuildCenterVo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        /// <summary>
        /// 公会大厅等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 升级消耗道具资源
        /// </summary>
        public BetterList<TypeStruct> CenterLvlupConsume { get; set; }

        /// <summary>
        /// 升级冷却时间
        /// </summary>
        public int CenterUpTime { get; set; }

        /// <summary>
        /// 当前等级描述
        /// </summary>
        public string CurLvlDesc { get; set; }

        /// <summary>
        /// 下一级描述
        /// </summary>
        public string NextLvlDesc { get; set; }

        public GuildCenterVo()
        {
            CenterLvlupConsume = new BetterList<TypeStruct>();
        }
    }


    /// <summary>
    /// 公会活动
    /// </summary>
    public class GuildEventVo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        /// <summary>
        /// 开放等级(和公会等级对比,如果公会等级大于当前，则开启)
        /// </summary>
        public int EnableLevel { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public int EventType { get; set; }

        /// <summary>
        /// 活动次数
        /// </summary>
        public int EventCount { get; set; }

        /// <summary>
        /// 活动持续时间，用于显示
        /// </summary>
        public string EventTime { get; set; }

        /// <summary>
        /// 自定义类型
        /// </summary>
        public int EventCustomType { get; set; }

        /// <summary>
        /// 自定义开战时间
        /// </summary>
        public string EventCustonTime { get; set; }

        /// <summary>
        /// 自定义按钮名称
        /// </summary>
        public string EventCustonButtonName1 { get; set; }

        /// <summary>
        /// 自定义按钮跳转ID
        /// </summary>
        public int EventFunction1 { get; set; }      
  
        /// <summary>
        /// 自定义按钮名称
        /// </summary>
        public string EventCustonButtonName2 { get; set; }

        public int EventFunction2 { get; set; }

        /// <summary>
        /// 活动描述
        /// </summary>
        public string EventDesc { get; set; }

    }

    /// <summary>
    /// 公会商店
    /// </summary>
    public class GuildShopVo
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 打开窗口类型
        /// </summary>
        public SellShopType ShopType { get; set; }

        /// <summary>
        /// 售卖类型（热卖类型）
        /// </summary>
        public ShopStateType SellState { get; set; }

        /// <summary>
        /// 商品资源类型
        /// </summary>
        public eGoldType SellMoneyType { get; set; }

        /// <summary>
        /// 商品的显示顺序
        /// </summary>
        public int DisplayId { get; set; }

        /// <summary>
        /// 售卖物品ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 售卖获得数量
        /// </summary>
        public int BuyCount { get; set; }

        /// <summary>
        /// 商品资源数量，即是销售价格
        /// </summary>
        public int ResourceNum { get; set; }


        /// <summary>
        /// 物品售卖上限个数
        /// </summary>
        public int SellLimit { get; set; }

        /// <summary>
        /// 今日购买个数
        /// </summary>
        public int CurBuyCount { get; set; }

        /// <summary>
        /// 售卖需要公会等级
        /// </summary>
        public int SellGuildLevel { get; set; }

        /// <summary>
        /// 购买价格
        /// </summary>
        public GoldValue ConsumePrice { get; set; }

        public GuildShopVo()
        {
            ConsumePrice = new GoldValue();
        }
        public int SellPrice { get; set; }

    }

    public class GuildLogVo
    {
        public string Log { get; set; }

        public DateTime Time { get; set; }

        /// <summary>
        /// 返回时间段
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            TimeSpan ts = DateTime.Now - Time;
            int year = DateTime.Now.Year - Time.Year;
            if (year != 0)
            {
                return ViewHelper.FormatLanguage("guild_time_year",year);
            }
            int month = DateTime.Now.Month - Time.Month;
            if (month!=0)
            {
                return ViewHelper.FormatLanguage("guild_time_month",month);
            }
            int day = DateTime.Now.Day - Time.Day;
            if (day != 0)
            {
                return ViewHelper.FormatLanguage("guild_time_day", day);
            }
            int hour = DateTime.Now.Hour - Time.Hour;
            if (hour!=0)
            {
                return ViewHelper.FormatLanguage("guild_time_hour", hour);
            }
            int minute = DateTime.Now.Minute - Time.Minute;
            if (minute!=0)
            {
                return ViewHelper.FormatLanguage("guild_time_minute", minute);
            }
            return ViewHelper.FormatLanguage("guild_time_minute", 1);
        }
    }

    public enum GuildSkillType
    {
        None = 0,
        GuildSkillLearn,
        GuildSkillFocus
    }

    public class GuildSkillVo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Type { get; set; }
        public int Level { get; set; }
        public BetterList<AttributeValue> Attributes { get; set; }
        public int BuildSkillLv { get; set; }

        public List<TypeStruct> SkillConsume { get; set; }
        public List<TypeStruct> SkillConsumeLearn { get; set; }

        public GuildSkillVo()
        {
            SkillConsume = new List<TypeStruct>();
            SkillConsumeLearn = new List<TypeStruct>();
        }

    }

    #region Enums
    /// <summary>
    /// 捐献等级
    /// </summary>
    public enum DonateLevel
    {
        None = 0,

        /// <summary>
        /// 低级捐献
        /// </summary>
        Low,

        /// <summary>
        /// 中级捐献
        /// </summary>
        Middle,

        /// <summary>
        /// 高级捐献
        /// </summary>
        High,
    }

    /// <summary>
    /// 官职
    /// </summary>
    public enum GuildOffice
    {
        None = 0,

        /// <summary>
        /// 会长
        /// </summary>
        Leader,

        /// <summary>
        /// 副会长
        /// </summary>
        DeputyLeader,

        /// <summary>
        /// 元老
        /// </summary>
        Statesman,

        /// <summary>
        /// 精英
        /// </summary>
        Elite,

        /// <summary>
        /// 群众
        /// </summary>
        Masses,
    }
    #endregion

    #region Structs
    /// <summary>
    /// 统计每个职位个数
    /// </summary>
    public struct OfficeMember
    {
        /// <summary>
        /// 职位
        /// </summary>
        public GuildOffice Type { get; set; }

        /// <summary>
        /// 当前职位最大人数
        /// </summary>
        public int MaxMember { get; set; }
    }
    #endregion    
}
