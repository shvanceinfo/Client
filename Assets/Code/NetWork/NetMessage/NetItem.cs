using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using MVC.entrance.gate;
using manager;
using model;
using UnityEngine;

/// <summary>
/// 使用物品的通信协议枚举状态
/// </summary>
public enum UseGoodsStatus
{
	Use = 0, 		//使用
	Sale = 1		//出售
}

public enum IsRefresh
{
	NotRefresh = 0,
	Refresh = 1
}


/// <summary>
/// 登陆，接收包裹数据
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class NetLoginItems : NetHead
{
	public NetLoginItems ()
	{
		//this._assistantCmd = (UInt16)eG2CType.G2C_BagInfo;        
	}

	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		byte refresh = memRead.ReadByte ();
		IsRefresh isRefresh = (IsRefresh)refresh;
		uint num = memRead.ReadUInt16 ();
		if (num == 0) {
			memRead.Close ();
			memStream.Close ();
			return;
		}
		
		for (int i = 0; i < num; i++) {
			ItemStruct tempItem = new ItemStruct ();
			tempItem.instanceId = memRead.ReadUInt16 ();
//			Debug.Log(tempItem.instanceId);
			tempItem.tempId = memRead.ReadUInt32 ();
//			Debug.Log(tempItem.tempId);
			tempItem.num = memRead.ReadUInt16 ();
//			Debug.Log(tempItem.num);
			tempItem.equipPos = 0;
			//tempItem.equipPos = (eEquipPart)memRead.ReadInt32();
			//tempItem.equipSource = 0;
			byte reason = memRead.ReadByte ();
			tempItem.changeReason = (ItemChangeReason)reason;
			ItemManager.GetInstance ().Change (tempItem, isRefresh);
		}
        GuideInfoManager.Instance.CheckItemTrigger();
		memRead.Close ();
		memStream.Close ();
		
		if (num > 1) {
			ItemManager.GetInstance ().SetLoginItem ();
		}
		
	}
}

/// <summary>
/// 登陆，接收装备数据
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class NetLoginEquipmentInfo : NetHead
{
	public NetLoginEquipmentInfo ()
	{

	}

	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		EquipmentStruct equipment = new EquipmentStruct ();
		equipment.instanceId = memRead.ReadUInt16 ();
		equipment.templateId = memRead.ReadUInt32 ();
		equipment.intensifyLevel = memRead.ReadUInt16 ();
		equipment.equipPart = (eEquipPart)memRead.ReadByte ();
		equipment.score = memRead.ReadInt32 (); //装备评分
		int num = memRead.ReadByte ();
		for (int j = 0; j < num; j++) {
			eFighintPropertyCate key = (eFighintPropertyCate)memRead.ReadByte ();
			int value = memRead.ReadInt32 ();
			eItemPropertySource type = (eItemPropertySource)memRead.ReadByte ();
			switch (type) {
			case eItemPropertySource.self:
				equipment.BaseAtrb.Add (new AttributeValue (){ Type=key,Value=value});
				break;
			case eItemPropertySource.intensify:
				if (equipment.StrengThenAtrb.ContainsKey (key)) {
					equipment.StrengThenAtrb [key] = new AttributeValue () { Type = key, Value = value };
				} else {
					equipment.StrengThenAtrb.Add (key, new AttributeValue () { Type = key, Value = value });
				}
                  
				break;
			case eItemPropertySource.None:
				break;
			case eItemPropertySource.badge:
				equipment.BadgeAtrb.Add (new AttributeValue () { Type = key, Value = value });
				break;

			case eItemPropertySource.Refine1:
			case eItemPropertySource.Refine2:
			case eItemPropertySource.Refine3:
			case eItemPropertySource.Refine4:
			case eItemPropertySource.Refine5:
			case eItemPropertySource.Refine6:
				int index = type - eItemPropertySource.Refine1;
				equipment.RefineAtrb.Add (index, new AttributeValue () { Type = key, Value = value });
				break;
			default:
				break;
			}

		}
//        equipment.source = memRead.ReadInt32();
		EquipmentManager.GetInstance ().Change (equipment);

		Gate.instance.sendNotification (MsgConstant.MSG_REFINE_DISPLAY_SELECT_VO, true);
		memRead.Close ();
		memStream.Close ();
	}
}
/// <summary>
/// 镶嵌
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class NetLoginEquipmentInlay : NetHead
{
	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		UInt16 count = memRead.ReadUInt16 ();

		//for (int i = 0; i < count; i++)
		//{
		//    byte part = memRead.ReadByte();
		//    byte index = memRead.ReadByte();
		//    UInt32 gemId = memRead.ReadUInt32();
		//    EquipmentStruct equip = BagManager.Instance.GetEquipByPart((eEquipPart)part);
		//    equip.gems[(int)index].Id = (int)gemId;
		//    equip.gems[(int)index].Value = true;    //解锁了
		//    EquipmentManager.GetInstance().Change(equip);
		//}

		for (int i = 0; i < count; i++) {
			byte part = memRead.ReadByte ();
			byte index = memRead.ReadByte ();
			UInt32 gemId = memRead.ReadUInt32 ();
			InlayManager.Instance.InlayInfo [(eEquipPart)part].Gems [index].Id = (int)gemId;
			InlayManager.Instance.InlayInfo [(eEquipPart)part].Gems [index].Value = true;
		}
		Gate.instance.sendNotification (MsgConstant.MSG_INLAY_DISPLAY_INFO);
		memRead.Close ();
		memStream.Close ();
	}
}

/// client-------------------->server
/// 购买物品
//[Serializable]
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//class GCAskBuyGoods : NetHead
//{
//	UInt32 itemTempId;
//	UInt32 goodsNum;
//
//	public GCAskBuyGoods (UInt32 tempId, UInt32 num)
//        : base()
//	{
//		itemTempId = tempId;
//		goodsNum = num;
//		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
//		this._assistantCmd = (UInt16)eC2GType.C2G_BuyGoods;
//	}
//
//	public byte[] ToBytes ()
//	{
//		MemoryStream memStream = new MemoryStream ();
//		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
//		base.ToBytes (ref memWrite);
//		memWrite.Write (this.itemTempId);
//		memWrite.Write (this.goodsNum);
//		byte[] bytesData = memStream.ToArray ();
//		memWrite.Close ();
//		return bytesData;
//	}
//}

///请求使用物品
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskUseGoods : NetHead
{
	byte  isSale;
	UInt16 saleNum;

	public GCAskUseGoods (UseGoodsStatus status, UInt16 num)
        : base()
	{
		this.isSale = (byte)status; //使用的状态
		this.saleNum = num; // 出售或者使用的物品的数量
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_UseGoods;
	}

	public byte[] ToBytes (IList<ItemStruct> itemlist, uint tempId=0)
	{
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
        #region 重新计算长度
		this._length += (ushort)(8 * itemlist.Count);
        #endregion
		
		base.ToBytes (ref memWrite);
		memWrite.Write (this.isSale);
		memWrite.Write (this.saleNum);
		//开始写数据
		for (int i = 0; i < itemlist.Count; i++) {
			memWrite.Write ((ushort)itemlist [i].instanceId);
			memWrite.Write ((ushort)itemlist [i].num);
			memWrite.Write(0);
		}
		
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
	
	public byte[] ToQuickUseOrSale (ItemInfo item)
	{
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
        #region 重新计算长度
		this._length += 8;
        #endregion
		
		base.ToBytes (ref memWrite);
		memWrite.Write (this.isSale);
		memWrite.Write (this.saleNum);
		//开始写数据
		memWrite.Write ((ushort)item.InstanceId);
		memWrite.Write ((ushort)item.Num);
		memWrite.Write (item.Id);

		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
	
	
	
	
	
}

///请求清理临时包裹
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskCleanTempBag : NetHead
{
	public GCAskCleanTempBag ()
            : base()
	{
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_Ping;
	}
		
	public byte[] ToBytes ()
	{
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		memStream.Close ();
		return bytesData;
	}
}
/// <summary>
/// 穿装备
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskEquipGoods : NetHead
{
	UInt16 itemInstanceId;

	public GCAskEquipGoods (UInt16 instanceId)
        : base()
	{
		itemInstanceId = instanceId;
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_EquipGoods;
	}

	public byte[] ToBytes ()
	{
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);
		memWrite.Write (this.itemInstanceId);
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}

/// <summary>
/// 出售物品
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskSaleGoods : NetHead
{
	UInt32 itemInstanceId;
	UInt32 num;

	public GCAskSaleGoods (UInt32 itemId, UInt32 saleNum)
        : base()
	{
		itemInstanceId = itemId;
		this.num = saleNum;
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_SaleGoods;
	}

	public byte[] ToBytes ()
	{
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);
		memWrite.Write (this.itemInstanceId);
		memWrite.Write (this.num);
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}

/// <summary>
/// 移动物品
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskMoveGoods : NetHead
{
	UInt32 oriPos;
	UInt32 tarPos;

	public GCAskMoveGoods (UInt32 ori, UInt32 tar)
        : base()
	{
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_AskMoveGoods;
		this.oriPos = ori;
		this.tarPos = tar;
	}

	public byte[] ToBytes ()
	{
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);
		memWrite.Write (this.oriPos);
		memWrite.Write (this.tarPos);
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}



/// <summary>
/// 物品增加的变化
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class NotifyItemChange : NetHead
{
	
	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		
		uint tempId = memRead.ReadUInt32 ();
		ushort num = memRead.ReadUInt16 ();
		ushort instanceId = memRead.ReadUInt16 ();
		
		ItemInfo item = new ItemInfo (tempId, instanceId, num);
		MainManager.Instance.NewItemList.Add (item);
		
//		if (item.Item.canTips) {
//			if (item.Item.packType == ePackageNavType.Other) {
//				MainManager.Instance.NewItemUseList.Add (item);
//			}else{
//				
//				if (item.Item.career == CharacterPlayer.character_property.career) {	//如果是同一个职业，则判断战斗力
//					if (BagManager.Instance.GetPowerCompareValue(item)>0) {
//						MainManager.Instance.NewItemUseList.Add (item);
//					}
//				}
//			}
//			
//		}//添加到使用列表里
		
		NewitemManager.Instance.NewItemShow ();
//		NewitemManager.Instance.NewItemUseShow ();
		
		
		memRead.Close ();
		memStream.Close ();

    }
}

/// <summary>
/// 勋章信息
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class NotifyMedalInfo : NetHead
{

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
		
		int medalID = (int)memRead.ReadUInt32();
        MedalManager.Instance.GetServerData(medalID);
        MedalTipsManager.Instance.UpdateInfo();
        memRead.Close();
        memStream.Close();
    }
}


