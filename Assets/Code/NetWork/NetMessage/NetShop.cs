using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using model;
using manager;
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class NetShopItems : NetHead
{
	public NetShopItems()
	{
	}
	
	public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        uint num = memRead.ReadUInt32();
        if (num == 0)
        {
            return;
        }

        for (int i = 0; i < num; i++)
        {
            ShopVo vo = new ShopVo();
            vo.Id  = (int)memRead.ReadUInt32();
            vo.Name = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(20)));
            vo.Table = (SellShopType)memRead.ReadInt32();
            vo.DisplayId = memRead.ReadInt32();
            vo.ItemId = memRead.ReadInt32();
            vo.GetItemCount = memRead.ReadInt32();
            vo.GetDiamonCount = memRead.ReadInt32();
            vo.SellMoneyType = (eGoldType)memRead.ReadInt32();
            vo.SellPrice = memRead.ReadInt32();
            vo.RmbPrice = memRead.ReadInt32();
            vo.SellState = (ShopStateType)memRead.ReadInt32();
            vo.StateDescription = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(40)));
            vo.Region = memRead.ReadInt32();
            ShopManager.Instance.AddShopItem(vo);
        }
        if (FeebManager.Instance.Request)
        {
            FeebManager.Instance.OpenWindow();
        }
        else {
            switch (FastOpenManager.Instance.WaitForOpenId)
            {
                case FunctionName.Shop_BuyDiamond:
                    ShopManager.Instance.OpenShopWindow(SellShopType.Diamon);
                    break;
                default:
                    ShopManager.Instance.OpenShopWindow();
                    break;
            }
            
        }
        memRead.Close();
        memStream.Close();
    }
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCAskShopTemp : NetHead
{
    public GCAskShopTemp() : base()
    {
		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_AskShopTemp;
    }
	
	public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCAskBuyShopItem : NetHead
{
	public UInt32 m_unShopItemID;
    public UInt16 u16ShopNum;
    public GCAskBuyShopItem() : base()
    {
		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_AskBuyShopItem;
    }
	
	public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
		memWrite.Write(this.m_unShopItemID);
        memWrite.Write(this.u16ShopNum);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}



[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GSNotifyGuideList : NetHead
{
    public GSNotifyGuideList()
    {
    }

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        uint num = memRead.ReadUInt16();
        if (num > 0)
        {
            byte[] bs = memRead.ReadBytes((int)num);
            for (int i = 0; i < bs.Length; i++)
            {
                GuideManager.Instance.ProcessServerData(i, Convert.ToBoolean(bs[i]));
            }
        }
        memRead.Close();
        memStream.Close();
    }
}

