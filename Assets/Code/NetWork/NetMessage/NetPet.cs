using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using manager;



//请求获得宠物数据
using model;
using System.Diagnostics;


[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskPetInfo : NetHead
{    
    public GCAskPetInfo(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskPetData;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}



//宠物进行升阶
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskPetEvolution : NetHead
{ 
    public GCAskPetEvolution(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskPetLevelUp;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//服务器通知客户端宠物数据
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyPetInfo : NetHead
{
    public void ToObject(byte[] byteData)
    {
    	MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        uint petID = memRead.ReadUInt32();
        uint luckNum = (uint)memRead.ReadInt32();
		uint selectPetId =  memRead.ReadUInt32();
		memRead.Close();
		PetManager.Instance.InitPet(petID,luckNum,selectPetId);
    }
}


//请求选中宠物
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCSelectPet : NetHead
{   
	uint _selectPetId;
	public GCSelectPet(): base()
	{
		this._selectPetId = 0;
		NetHead head = new NetHead();
		this._assistantCmd = (UInt16)eC2GType.C2G_SelectPet;
		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
	}
	
	public byte[] ToBytes(uint petId)
	{
		MemoryStream memStream = new MemoryStream();
		BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
		base.ToBytes(ref memWrite);
		memWrite.Write (petId);
		byte[] bytesData = memStream.ToArray();
		memWrite.Close();
		return bytesData;
	}
}


////通知宠物身上的装备信息
//[Serializable]
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//class GSNotifyPetEquip : NetHead
//{
//	public void ToObject(byte[] byteData)
//	{
//		MemoryStream memStream = new MemoryStream(byteData);
//		BinaryReader memRead = new BinaryReader(memStream);
//		base.ToObject(ref memRead);
//
//		PetEquip pe = new PetEquip ();
//		pe.InstanceId =memRead.ReadUInt16 (); //实例ID(已经装备身上的从55000开始)
//		pe.TempId = memRead.ReadUInt32 ();	   //物品模板ID
//		pe.Score = memRead.ReadUInt32 ();			//装备评分
//		pe.EquipPart = (PetEquipPartEnum) memRead.ReadByte ();	   //装备位置
//	 
//		memRead.Close();
//
//		PetManager.Instance.UpdatePetEquip (pe);
//	}
//}


//请求装备宠物装备
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskEquipPet : NetHead
{    
	ushort _bagPos;
	public GCAskEquipPet(): base()
	{
		NetHead head = new NetHead();
		this._assistantCmd = (UInt16)eC2GType.C2G_AskEquipPet;
		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
	}
	
	public byte[] ToBytes(ushort bagPos)
	{
		MemoryStream memStream = new MemoryStream();
		BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
		base.ToBytes(ref memWrite);
		memWrite.Write (bagPos);
		byte[] bytesData = memStream.ToArray();
		memWrite.Close();
		return bytesData;
	}
}












