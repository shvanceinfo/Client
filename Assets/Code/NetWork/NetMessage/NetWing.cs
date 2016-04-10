using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using manager;

//请求获得翅膀数据
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskWingInfo : NetHead
{    
    public GCAskWingInfo(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskWingInfo;
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

//请求翅膀进行培养
//[Serializable]
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//class GCAskWingCulture : NetHead
//{
//    public GCAskWingCulture(): base()
//    {
//        NetHead head = new NetHead();
//        this._assistantCmd = (UInt16)eC2GType.C2G_AskCultureWing;
//        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
//    }
//
//    public byte[] ToBytes()
//    {
//        MemoryStream memStream = new MemoryStream();
//        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
//        base.ToBytes(ref memWrite);
//        byte[] bytesData = memStream.ToArray();
//        memWrite.Close();
//        return bytesData;
//    }
//}

//翅膀进行升阶
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskWingEvolution : NetHead
{ 
    public GCAskWingEvolution(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskEvolutionWing;
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

//服务器通知客户端翅膀数据
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyWingInfo : NetHead
{
    public void ToObject(byte[] byteData)
    {
    	MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        uint wingID = (uint)memRead.ReadUInt32();
        uint curExp = (uint)memRead.ReadInt32();
        uint luckNum = (uint)memRead.ReadInt32();
        int doubleNum = (int)memRead.ReadInt32();
        WingManager.Instance.initWing(wingID, curExp, luckNum, doubleNum);
    }
}