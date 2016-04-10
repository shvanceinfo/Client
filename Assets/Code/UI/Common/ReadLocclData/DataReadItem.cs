using System;
using System.Collections;
using UnityEngine;
using model;
using manager;
using helper;

/// <summary>
/// 物品类型
/// </summary>
public enum eItemType
{
	/// <summary>
	/// 装备
	/// </summary>
	eEquip = 1, 
	///消耗品
	eExpend = 2,  
	///宝石
	eGem = 3,    
	///礼包
	ePacks = 4,   
	///材料
	eMaterial = 5, 
	///制作的东西
	eProduce = 6,
	/// <summary>
	/// 宠物
	/// </summary>
	ePet=7,
	/// <summary>
	/// 任务物品
	/// </summary>
	eTask=8,
	/// <summary>
	/// 宠物装备
	/// </summary>
	ePetEquip=9,	
}
/// <summary>
/// 物品品质
/// </summary>
public enum eItemQuality
{
	eWhite = 1,             ///白
	eGreen = 2,             ///绿
	eBlue = 3,                 ///蓝
	ePurple = 4,            ///紫
	eOrange = 5             ///橙
}
/// <summary>
/// 装备位置
/// </summary>

public enum eEquipPart
{
	eNone = 0,
	eSuit = 1,                      ///衣服
	eLeggings = 2,              ///护腿
	eShoes = 3,                 ///鞋子
	eNecklace = 4,                 ///项链
	eRing = 5,                      ///戒指
	eGreatSword = 6,            ///巨剑
	eArcher = 7,                    ///弓箭
	eDoublePole = 8,               ///双刀
	eEnd=9,
	eTooth = 61,			//牙齿
	eClaw = 62,				//爪子
	eEye = 63,				//瞳
	eJewelry = 64,			//珠宝
}
/// <summary>
/// 宝石类型
/// </summary>
[Obsolete("GemType")]
public enum eGemType
{
	eAttack = 1,                    ///攻击
	eDefense = 2,                   ///防御
	eCrit = 3,                          ///暴击
	eIceDamage = 4,             ///冰伤害
	eIceResistances = 5,        ///冰抗性
	eIceImmune = 6,             ///冰免疫
	eFireDamage = 7,                ///火
	eFireResistances = 8,
	eFireImmune = 9,
	ePoisonDamage = 10,         ///毒
	ePoisonResistances = 11,
	ePoisonImmune = 12
}

/// <summary>
/// 装备属性
/// </summary>
public enum eFighintPropertyCate
{
	eFPC_None = 0,
	eFPC_MaxHP = 1,
	eFPC_HPRecover,
	eFPC_MaxMP,
	eFPC_MPRecover,
	eFPC_Attack,
	eFPC_Defense,
	eFPC_Precise,
	eFPC_Dodge,
	eFPC_BlastAttack,
	eFPC_BlastAttackAdd,
	eFPC_BlastAttackReduce,
	eFPC_Tenacity, //韧性
	eFPC_FightBreak, //破击
	eFPC_AntiFightBreak, //招架
	eFPC_KnockDown, //击倒(击飞)
	eFPC_KnockBack, //击退
	eFPC_IceAttack, //冰伤害
	eFPC_AntiIceAttack, //冰抗性
	eFPC_IceImmunity, // 冰免疫
	eFPC_FireAttack,
	eFPC_AntiFireAttack,
	eFPC_FireImmunity,
	eFPC_PoisonAttack,
	eFPC_AntiPoisonAttack,
	eFPC_PoisonImmunity,
	eFPC_ThunderAttack,
	eFPC_AntiThunderAttack,
	eFPC_ThunderImmunity,
	eFpc_End
}
;
//属性来源:1自带属性，2强化属性，4徽章属性，
//5洗练属性1，6洗练属性2，7洗练属性3，8洗练属性4，9洗练属性5，10洗练属性6
public enum eItemPropertySource
{
	self =1, //自带属性
	intensify = 2,//强化
	None =3, //
	badge=4, //徽章
	Refine1=5,
	Refine2 = 6,
	Refine3 = 7,
	Refine4 = 8,
	Refine5 = 9,
	Refine6 = 10,
}

/// <summary>
/// 反馈类型
/// </summary>
public enum FeedBack
{ 
	/// <summary>
	/// 没有
	/// </summary>
	None=0,

	/// <summary>
	/// 文本提示
	/// </summary>
	Text,
	/// <summary>
	/// 快捷购买+获取提示
	/// </summary>
	DoubleDialog,

	/// <summary>
	/// 快捷购买
	/// </summary>
	QuickBuy,
	/// <summary>
	/// 获取途径
	/// </summary>
	FindInfo
}


/// <summary>
/// 物品模版数据
/// </summary>
public struct ItemTemplate
{
	public uint id;                      ///物品ID
	public string name;                     ///物品名称
	public eItemType itemType;      ///物品类型
	public ePackageNavType packType;   //格子类型
	public string icon;                     ///icon
	public string model;                    /// 3D model;
	public eItemQuality quality;       ///品质
	public uint usedLevel;              ///使用等级
	public uint overlapNum;         ///堆叠上线
	public uint silivePrice;                ///出售价格
	public uint saleProtect;         ///出售保护
	public uint usedCD;                 ///使用cd
	public uint usedEffect;         ///使用效果
	public uint itemloot;               ///掉了索引
	public string discription;          ///说明
	public string itemSource;          ///物品来源
	public uint typeRank; 			//物品排序ID
	public CHARACTER_CAREER career;	//物品对应的职业 
	public FeedBack feedType;       //物品不足提示类型
	public uint feedBackShopId;             //商品ID
	public BetterList<int> feedBackFunctionId; //便捷提示ID
	public bool canTips;			//是否有提示
}
/// <summary>
/// 装备模版
/// </summary>
public struct EquipmentTemplate
{
	public uint id;                         ///物品模版id      
	public eEquipPart part;           ///装备位置
//	public string texture_name;
	public string swordTexture; //剑士贴图
	public string archerTexture; //弓箭手贴图
	public string magicianTexture; //魔法师贴图
	public string model_name;
	public string effect_name;
	public string weapon_trail_texture;
	public eFighintPropertyCate bStateType1;         ///基础属性名1
	public eFighintPropertyCate bStateType2;
	public int bStateValue1;          ///基础属性值1
	public int bStateValue2;
	[Obsolete("don't use")]
	public eFighintPropertyCate
		aStateType1;
	[Obsolete("don't use")]
	public eFighintPropertyCate
		aStateType2;
	[Obsolete("don't use")]
	public eFighintPropertyCate
		aStateType3;
	[Obsolete("don't use")]
	public eFighintPropertyCate
		aStateType4;
	[Obsolete("don't use")]
	public int
		aStateValue1;
	[Obsolete("don't use")]
	public int
		aStateValue2;
	[Obsolete("don't use")]
	public int
		aStateValue3;
	[Obsolete("don't use")]
	public int
		aStateValue4;
	/// <summary>
	/// 装备进阶后新的物品ID
	/// </summary>
	public uint EquipmentUpId;

	/// <summary>
	/// 是否达到最大进阶
	/// </summary>
	public bool IsMaxAdvanced {
		get {
			if (EquipmentUpId <= 0) {
				return true;
			}
			return false;
		}
	}
       
	[Obsolete("don't use")]                          
	public uint
		equipmentUpMoney;
	public string skillFixed;
	public string skillRand;
	public uint maxForgeLevel;
	/// <summary>
	/// 装备进阶成功率
	/// </summary>
	public int EquipmentUpSuccessrate;


	/// <summary>
	/// 装备进阶替代消耗钻石
	/// </summary>
	public int EquipmentUpDiamond;

	/// <summary>
	/// 装备进阶消耗的物品
	/// </summary>
	public BetterList<TypeStruct> EquipmentUpItem;

	/// <summary>
	/// 是否可以使用幸运石
	/// </summary>
	public bool IsUsedLuckStone;
	[Obsolete("预留字段，等待以后开发")]
	public bool
		ItemResolved;

	/// <summary>
	/// 装备吞噬经验
	/// </summary>
	public int BadgeSwallowExp;

	/// <summary>
	/// 装备吞噬消耗多少金币
	/// </summary>
	public int BadgeSwallowGold;
	[Obsolete("预留字段，等待以后开发")]
	public int
		SkillFixed;
}
/// <summary>
/// 宝石模版
/// </summary>
[Obsolete("using GemVo")]
public struct GemTemplate
{
	public uint id;                             ///id
	public eGemType type;              ///宝石类型
	public eFighintPropertyCate stateType1;                  ///基础属性
	public eFighintPropertyCate stateType2;
	public int stateValue1;
	public int stateValue2;
	public string equipmentType;       ///可镶嵌装备类型（用“，”分割的字符串）
	public uint successRate;                ///升级成功率（1/1000）
	public uint upMoney;                    ///升级所需金币
	public uint upSuperDiamond;     ///超级强化所需钻石
	public uint gemUp;                      ///升级后的物品id
	public uint gemLevel;                   ///等级
}
/// <summary>
/// 强化模版
/// </summary>
[Obsolete("using EquipmentForgeVo")]
public struct EquipmentForgeTemplate
{
	public uint id;                                     ///id（第1位：物品类型，后3位：物品等级）
	public uint itemUsed;                        ///消耗物品     
	public uint itemCount;                      ///消耗数量
	public uint spendMoney;                  ///消耗金币
	public uint successRate;                   ///强化成功率（1/1000）
	public uint effectValue;                    ///强化效果值
	public uint superForgeDiamond;     ///超级强化消耗钻石
	public uint itemResolved;                ///分解获得道具
	public uint resolvedCount;              ///分解获得数量
}

/// <summary>
/// 读取物品模版
/// </summary>
public class DataReadItem : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{

		ItemTemplate di;

		if (!data.ContainsKey (key)) {
			di = new ItemTemplate ();
			data.Add (key, di);
		} else {
			di = (ItemTemplate)data [key];
		}      

		switch (name) {
		case "ID":
			di.id = uint.Parse (value);
			break;
		case "Name":
			di.name = value;
			break;
		case "ItemType":
			di.itemType = (eItemType)int.Parse (value);
			break;
		case "Icon":
			di.icon = value;
			break;
		case "itemPre":
			di.model = value;
			break;
		case "Quality":
			di.quality = (eItemQuality)int.Parse (value);
			break;
		case "UsedLevel":
			di.usedLevel = uint.Parse (value);
			break;
		case "OverlapNum":
			di.overlapNum = uint.Parse (value);
			break;
		case "SiliverPrice":
			di.silivePrice = uint.Parse (value);
			break;
		case "UsedCD":
			di.usedCD = uint.Parse (value);
			break;
		case "UsedEffect":
			di.usedEffect = uint.Parse (value);
			break;
		case "ItemLoot":
			di.itemloot = uint.Parse (value);
			break;
		case "Discription":
			di.discription = value;
			break;
		case "source":
			di.itemSource = value;
			break;
		case "ItemBagType":
			di.packType = (ePackageNavType)int.Parse (value);
			break;
		case "TypeRank":
			di.typeRank = uint.Parse (value);
			break;
		case "UsedZhiYe"://可以使用的职业
			di.career = (CHARACTER_CAREER)int.Parse (value);
			break;
		case "SaleProtect":
			di.saleProtect = uint.Parse (value);
			break;
		case "FeedBackType":
			di.feedType = (FeedBack)XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "FeedBackValue_shop":
			di.feedBackShopId = (uint)XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "FeedBackValue_function":
			XmlHelper.CallTry (() => {

				string[] sps = value.Split (',');
				if (di.feedBackFunctionId == null) {
					di.feedBackFunctionId = new BetterList<int> ();
				}
				for (int i = 0; i < sps.Length; i++) {
					di.feedBackFunctionId.Add (int.Parse (sps [i]));
				}
			});
			break;
		case "tishi":
			if (value == "1") {
				di.canTips = true;
			} else {
				di.canTips = false;
			}
			break;
		}
		
		data [key] = di;
	}

	public ItemTemplate getTemplateData (int key)
	{
		ItemTemplate temp = new ItemTemplate ();
		if (data.ContainsKey (key)) {
			temp = (ItemTemplate)data [key];
		}
        
		return  temp;
	}
}
/// <summary>
/// 装备模版
/// </summary>
public class DataReadEquipment : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{

		EquipmentTemplate di;

		if (!data.ContainsKey (key)) {
			di = new EquipmentTemplate ();
			di.EquipmentUpItem = new BetterList<TypeStruct> ();
			data.Add (key, di);
		}

		di = (EquipmentTemplate)data [key];
		string[] sps;
		switch (name) {
		case "ID":
			di.id = uint.Parse (value);
			break;
		case "Part":
			di.part = (eEquipPart)int.Parse (value);
			break;
		case "texture":
			di.swordTexture = value;
			break;
		case "texture1":
			di.archerTexture = value;
			break;
		case "texture2":
			di.magicianTexture = value;
			break;
		case "model":
			di.model_name = value;
			break;
		case "effect":
			di.effect_name = value;
			break;
		case "trail":
			di.weapon_trail_texture = value;
			break;
		case "StateValue":
			XmlHelper.CallTry (() => {
				sps = value.Split (',');
				di.bStateType1 = (eFighintPropertyCate)int.Parse (sps [0]);
				di.bStateValue1 = int.Parse (sps [1]);
				di.bStateType2 = eFighintPropertyCate.eFPC_None;
				di.bStateValue2 = 0;
			});
			break;
	
		case "EquipmentUp":
			di.EquipmentUpId = uint.Parse (value);
			break;

		case "SkillFixed":
			di.skillFixed = value.Trim ();
			break;
		case "SkillRand":
			di.skillRand = value.Trim ();
			break;
		case "MaxForgeLevel":
			di.maxForgeLevel = uint.Parse (value);
			break;
		case "EquipmentUpRate":
			di.EquipmentUpSuccessrate = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "EquipmentUpGold":
			XmlHelper.CallTry (() =>
			{
				di.EquipmentUpItem.Add (new TypeStruct ((int)eGoldType.gold,
                    ConsumeType.Gold, int.Parse (value)));
			});
			break;
		case "EquipmentUpDiamond":
			di.EquipmentUpDiamond = XmlHelper.CallTry (() => (int.Parse (value)));
			;
			break;
		case "EquipmentUpItem":
			XmlHelper.CallTry (() => {
				sps = value.Split (',');
				for (int i = 0; i < sps.Length; i+=2) {

					di.EquipmentUpItem.Add (new TypeStruct (int.Parse (sps [i]), ConsumeType.Item, int.Parse (sps [i + 1])));
				}
			});
			break;
		case "LuckStoneUsed":
			di.IsUsedLuckStone = Convert.ToBoolean (XmlHelper.CallTry (() => (int.Parse (value))));
			break;
		case "huizhangSwallowExp":
			di.BadgeSwallowExp = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "huizhangSwallowGold":
			di.BadgeSwallowGold = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		}
		data [key] = di;
	}

	public EquipmentTemplate getTemplateData (int key)
	{

		if (!data.ContainsKey (key)) {
			EquipmentTemplate di = new EquipmentTemplate ();
			return di;
		}

		return (EquipmentTemplate)data [key];
	}
}

/// <summary>
/// 读取宝石模版
/// </summary>
public class DataReadGem : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{
		GemVo vo;
		if (MergeManager.Instance.GemHash.ContainsKey (key)) {
			vo = MergeManager.Instance.GemHash [key] as GemVo;
		} else {
			vo = new GemVo ();
			MergeManager.Instance.GemHash.Add (key, vo);
		}
		string[] sps;
		char px = ',';
		switch (name) {
		case "ID":
			vo.Id = int.Parse (value);
			break;
		case "Name":
			vo.Name = value;
			break;
		case "GemType":
			vo.Type = (GemType)int.Parse (value);
			break;
		case "GemLevel":
			vo.Level = int.Parse (value);
			break;
		case "StateValue":
			sps = value.Split (px);
			for (int i = 0; i < sps.Length; i+=2) {
				vo.Attribute.Add (new AttributeValue () { Type = (eFighintPropertyCate)int.Parse(sps[i]), Value = int.Parse(sps[i+ 1] ) });
			}
			break;
		case "EquipmentType":
			sps = value.Split (px);
			for (int i = 0; i < sps.Length; i++) {
				vo.Equips.Add ((eEquipPart)int.Parse (sps [i]));
			}
			break;
		case "Successrate":
			vo.Successrate = int.Parse (value);
			break;
		case "GemUpNum":
			vo.MergeNum = int.Parse (value);
			break;
		case "GemUpMoney":
			vo.MergeGold = int.Parse (value);
			break;
		case "LuckStoneUsed":
			vo.IsUseLuckStone = Convert.ToBoolean (int.Parse (value));
			break;
		case "GemUp":
			if (value != "") {
				try {
					vo.MergeNextId = int.Parse (value);
				} catch (Exception ex) {
					throw ex;
				}
			}            
			break;
		case "GemResolvedMoney":
			vo.ResolvedConsumeGold = int.Parse (value);
			break;
		case "GemResolved":
			sps = value.Split (px);
			for (int i = 0; i < sps.Length; i+=2) {
				vo.ResolvedItems.Add (new IdStruct (int.Parse (sps [i]), int.Parse (sps [i + 1])));
			}
			break;
		case "GemTransformMoney":
			vo.GemTransformConsumeGold = int.Parse (value);
			break;
		case "GemTransform":
			sps = value.Split (px);
			for (int i = 0; i < sps.Length; i++) {
				vo.GemTransformItem.Add (int.Parse (sps [i]));
			}
			break;
		case "isMaxLV":
			vo.IsMergeGoing = Convert.ToBoolean (int.Parse (value));
			break;
		case "isResolved":
			vo.IsResolved = Convert.ToBoolean (int.Parse (value));
			break;
		default:
			break;
		}

	}

	public GemTemplate getTemplateData (int key)
	{

		if (!data.ContainsKey (key)) {
			GemTemplate di = new GemTemplate ();
			return di;
		}

		return (GemTemplate)data [key];
	}
}


/// <summary>
/// 读取道具模版
/// </summary>
public class DataReadFormula : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{
		FormulaVo vo;
		if (FormulaManager.Instance.FormulaHash.ContainsKey (key)) {
			vo = FormulaManager.Instance.FormulaHash [key] as FormulaVo;
		} else {
			vo = new FormulaVo ();
			FormulaManager.Instance.FormulaHash.Add (key, vo);
		}
        
		switch (name) {
		case "ID":
			vo.Id = int.Parse (value);
			break;
		case "Name":
			vo.Name = value;
			break;
		case "Type":
			vo.Type = (FormulaType)int.Parse (value);
			break;
		case "TypeRank":
			vo.OrderId = int.Parse (value);
			break;
		case "materialID":
			vo.MaterialID = int.Parse (value);
			break;
		case "materialNum":
			vo.MaterialNum = int.Parse (value);
			break;
		case "productPrice":
			int price = int.Parse (value);
			vo.ConsumeItem.Add (new IdStruct (vo.MaterialID, vo.MaterialNum, price));
			vo.ConsumeGold = price;
			break;
		case "Successrate":
			vo.Successrate = int.Parse (value);
			break;
		case "LuckStoneUsed":
			vo.IsUsedLuckStone = Convert.ToBoolean (int.Parse (value));
			break;
		case "productID":
			vo.MergeNextId = int.Parse (value);
			break;
		case "gonggaoID":
			vo.GonggaoID = int.Parse (value);
			break;
		default:
			break;
		}

	}

	public GemTemplate getTemplateData (int key)
	{

		if (!data.ContainsKey (key)) {
			GemTemplate di = new GemTemplate ();
			return di;
		}

		return (GemTemplate)data [key];
	}
}



/// <summary>
/// 强化模版
/// </summary>
public class DataReadEquipmentForge : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{
		EquipmentForgeVo vo;
		if (StrengThenManager.Instance.EquipForge.ContainsKey (key)) {
			vo = StrengThenManager.Instance.EquipForge [key] as EquipmentForgeVo;
		} else {
			vo = new EquipmentForgeVo ();
			StrengThenManager.Instance.EquipForge.Add (key, vo);
		}
		string[] sps;
		switch (name) {
		case "ID":
			vo.Id = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "StarLV":
			vo.StrengThenLevel = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "Part":
			vo.EquipType = (eEquipPart)XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "UsedZhiYe":
			vo.Career = (CHARACTER_CAREER)XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "ItemUsed":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (',');
				for (int i = 0; i < sps.Length; i += 2) {
					vo.ConsumeItem.Add (new TypeStruct (int.Parse (sps [i]), ConsumeType.Item, int.Parse (sps [i + 1])));
				}
			});
			break;
		case "SpendMoney":
			XmlHelper.CallTry (() =>
			{
				vo.ConsumeItem.Add (new TypeStruct ((int)eGoldType.gold, ConsumeType.Gold, int.Parse (value)));
                    
			});
			break;
		case "Successrate":
			vo.Successrate = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "EffectValue":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (',');
				for (int i = 0; i < sps.Length; i += 2) {
					vo.StrengThenValue.Add (new IdStruct (int.Parse (sps [i]), int.Parse (sps [i + 1])));
				}
			});
			break;
		case "LuckStoneUsed":
			vo.IsUsedLuckStone = Convert.ToBoolean (XmlHelper.CallTry (() => (int.Parse (value))));
			break;
		case "ItemResolved":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (',');
				for (int i = 0; i < sps.Length; i += 2) {
					vo.SplitItems.Add (new IdStruct (int.Parse (sps [i]), int.Parse (sps [i + 1])));
				}
			});
			break;
		case "ItemResolvedGold":
			vo.SplitConsumeGold = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		default:
			break;
		}

		
	}

	public EquipmentForgeTemplate getTemplateData (int key)
	{

		//if (!data.ContainsKey (key)) {
		//    EquipmentForgeTemplate di = new EquipmentForgeTemplate ();
		//    return di;
		//}

		return (EquipmentForgeTemplate)data [key];
	}
}

//镶嵌
public class DataReadEquipmentPart : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{
		EquipmentPartVo vo;
		if (StrengThenManager.Instance.EquipPart.ContainsKey (key)) {
			vo = StrengThenManager.Instance.EquipPart [key] as EquipmentPartVo;
		} else {
			vo = new EquipmentPartVo ();
			StrengThenManager.Instance.EquipPart.Add (key, vo);
		}
		string[] sps;
		switch (name) {
		case "ID":
			vo.Id = XmlHelper.CallTry (() => (int.Parse (value)));
			break;

		case "Part":
			vo.Part = (eEquipPart)XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "UsedZhiYe":
			vo.Career = (CHARACTER_CAREER)XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "Gem_kong":
			vo.MaxInlaySize = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "GemUnlockLV":
			XmlHelper.CallTry (() => 
			{
				sps = value.Split (',');
				for (int i = 0; i < sps.Length; i++) {
					vo.UnLockLevel.Add (XmlHelper.CallTry (() => (int.Parse (sps [i]))));
				}
			});
			break;
		case "GemUnlockVIP":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (',');
				for (int i = 0; i < sps.Length; i++) {
					vo.UnLockVip.Add (XmlHelper.CallTry (() => (int.Parse (sps [i]))));
				}
			});
			break;
		default:
			break;
		}


	}
}

public class DataReadEquipmentRefine : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{

		RefineVo vo;
		if (RefineManager.Instance.RefineHash.ContainsKey (key)) {
			vo = RefineManager.Instance.RefineHash [key] as RefineVo;
		} else {
			vo = new RefineVo ();
			RefineManager.Instance.RefineHash.Add (key, vo);
		}
		string[] sps;
		switch (name) {
		case "ID":
			vo.Id = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "StateValue":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (',');
				for (int i = 0; i < sps.Length; i+=5) {
					vo.Attribute.Add (new AttributeValue () 
                        { Type=(eFighintPropertyCate)XmlHelper.CallTry(() => (int.Parse(sps[i]))),
                         Value=XmlHelper.CallTry(() => (int.Parse(sps[i+4])))
                        });
				}
			});
			break;
		case "ItemUsed":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (',');
				for (int i = 0; i < sps.Length; i += 2) {
					vo.ConsumeItems.Add (new IdStruct ()
                        {
                            Id = XmlHelper.CallTry(() => (int.Parse(sps[i]))),
                            Value = XmlHelper.CallTry(() => (int.Parse(sps[i + 1])))
                        });
				}
			});
			break;
		case "ResetItem":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (',');
				for (int i = 0; i < sps.Length; i += 2) {
					vo.ResetConsume.Add (new IdStruct ()
                        {
                            Id = XmlHelper.CallTry(() => (int.Parse(sps[i]))),
                            Value = XmlHelper.CallTry(() => (int.Parse(sps[i + 1])))
                        });
				}
			});
			break;
		case "UnlockLv":
			vo.UnLockLevel = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "UnlockVip":
			vo.UnLockVip = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		default:
			break;
		}
	}
}

public class DataReadLoot : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{

		LootVo vo;
		if (LootManager.Instance.LootHash.ContainsKey (key)) {
			vo = LootManager.Instance.LootHash [key] as LootVo;
		} else {
			vo = new LootVo ();
			LootManager.Instance.LootHash.Add (key, vo);
		}
		switch (name) {
		case "ID":
			vo.Id = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "LootDesc":
			vo.Name = value;
			break;
		case "ItemID":
			XmlHelper.CallTry (() =>
			{
				string[] sps = value.Split (',');
				for (int i = 0; i < sps.Length; i++) {
					vo.AwardItems.Add (new ItemCount{ 
                         Item=ItemManager.GetInstance().GetTemplateByTempId(uint.Parse(sps[i]))
                        }); 
				}
			});
			break;
		case "ItemCount":
			XmlHelper.CallTry (() =>
			{
				string[] sps = value.Split (',');
				for (int i = 0; i < sps.Length; i++) {
					vo.AwardItems [i].Count = int.Parse (sps [i]);
				}
			});
			break;
		default:
			break;
		}
	}
}