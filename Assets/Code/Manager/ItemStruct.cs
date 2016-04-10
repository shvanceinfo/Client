using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using model;
 

/// <summary>
/// 物品改变事情的类型
/// </summary>
public enum ItemChangeReason     
{
	None =0,
	TakeChapterAward=1             //章节奖励
}


/// <summary>
/// 物品数据
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class ItemStruct : ICloneable
{
	public UInt16 instanceId;
	public UInt32 tempId;
	public UInt32 num;
	public eEquipPart equipPos;
	public int equipSource;
	public ItemChangeReason changeReason;
	public bool isNewItem;

	public object Clone ()
	{
		return MemberwiseClone ();
	}
}
[Serializable]
public struct sEquipProperty
{
	public int value;
	public eItemPropertySource source;
}
/// <summary>
/// 装备数据
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class EquipmentStruct:ICloneable
{

	public UInt16 instanceId;       //实例ID
	public uint templateId;         //物品ID
	public uint intensifyLevel;     //强化等级
	public eEquipPart equipPart;    //装备位置

    [Obsolete()]
	public Dictionary<eFighintPropertyCate, Dictionary<eItemPropertySource,sEquipProperty>> addProperty = new Dictionary<eFighintPropertyCate, Dictionary<eItemPropertySource,sEquipProperty>> ();
	public int score;               //装备评分

    /// <summary>
    /// 基本属性
    /// </summary>
    public BetterList<AttributeValue> BaseAtrb { get; set; }

    /// <summary>
    /// 强化属性
    /// </summary>
    public Dictionary<eFighintPropertyCate,AttributeValue> StrengThenAtrb { get; set; }


    /// <summary>
    /// 徽章属性
    /// </summary>
    public BetterList<AttributeValue> BadgeAtrb { get; set; }


    /// <summary>
    /// 洗练属性,Key=洗练条目下标,Value=属性，值
    /// </summary>
    public Dictionary<int,AttributeValue> RefineAtrb{ get; set; }


	public EquipmentStruct ()
	{
        BaseAtrb = new BetterList<AttributeValue>();
        StrengThenAtrb = new Dictionary<eFighintPropertyCate,AttributeValue>();
        BadgeAtrb = new BetterList<AttributeValue>();
        RefineAtrb = new Dictionary<int, AttributeValue>();
	}

	#region ICloneable implementation
	public object Clone ()
	{
		EquipmentStruct newES =new EquipmentStruct();
		newES.instanceId = this.instanceId;
		newES.templateId = this.templateId;
		newES.intensifyLevel  = this.intensifyLevel;
		newES.equipPart = this.equipPart;
		newES.score = this.score;
		
		foreach (var attr in BaseAtrb) {
			newES.BaseAtrb.Add(attr);
		}
		
		foreach (var item in StrengThenAtrb) {
			newES.StrengThenAtrb.Add(item.Key,item.Value);
		}
		
		foreach (var item in BadgeAtrb) {
			newES.BadgeAtrb.Add(item);	
		}
		
		foreach (var item in RefineAtrb) {
			newES.RefineAtrb.Add(item.Key,item.Value);
		}
		
		
		
		return newES;
		
//		MemoryStream ms = new MemoryStream ();
//		BinaryFormatter bf = new BinaryFormatter();
//		bf.Serialize(ms,this);
//		ms.Position =0;			//重新定位
//		return 	bf.Deserialize(ms);
	}
	#endregion
}


