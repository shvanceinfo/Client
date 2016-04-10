using UnityEngine;
using System.Collections;

/// <summary>
///通用请求一般操作的ID，
///玩家身上资源enum类型，
///1代表喝药，
///2代表购买商品，
///3代表购买荣誉商品，
///4代表主动技能升级， 
/// 5代表被动技能升级，
/// 6代表主动技能解锁
/// </summary>
public enum ResourceEnum
{ 
    /// <summary>
    /// 喝药
    /// </summary>
    UseHealItem=1,
    /// <summary>
    /// 购买商品
    /// </summary>
    BuyItem,
    /// <summary>
    /// 购买荣誉商品
    /// </summary>
    BuyRongyuItem,
    /// <summary>
    /// 升级技能
    /// </summary>
    LevelSkill,
    /// <summary>
    /// 升级天赋
    /// </summary>
    LevelTalent,
    /// <summary>
    /// 解锁技能
    /// </summary>
    ReLockSkill
}
public enum eSkillType
{
    eNone = 0,
    eInitiative, //主动技能
    ePassive	//被动技能
}

public enum eGoldType
{
    none=0,
    /// <summary>
    /// 金币
    /// </summary>
    gold ,
    /// <summary>
    /// 钻石
    /// </summary>
    zuanshi,
    /// <summary>
    /// 荣誉
    /// </summary>
    rongyu,
    /// <summary>
    /// 符石
    /// </summary>
    fushi,
    /// <summary>
    /// 经验
    /// </summary>
    exp,
    /// <summary>
    /// 体力
    /// </summary>
    tili,
    /// <summary>
    /// 水晶
    /// </summary>
    shuijing,
    /// <summary>
    /// 贡献度
    /// </summary>
    gongxiandu,
    /// <summary>
    /// 公会财富
    /// </summary>
    gonghuicaifu

}

public enum Table
{ 
    None=0,
    Table1,
    Table2,
    Table3,
    Table4,
    Table5,
    Table6,
    TableMax
}

/// <summary>
/// 消耗物品类型
/// </summary>
public enum ConsumeType
{ 
    None=0,
    /// <summary>
    /// 消耗物品
    /// </summary>
    Item,

    /// <summary>
    /// 消耗金币
    /// </summary>
    Gold,
}

/// <summary>
/// 复活类型
/// </summary>
public enum ReliveType{
	Asset = 0,
	Time = 1
}