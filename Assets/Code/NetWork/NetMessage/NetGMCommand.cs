using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCAskAddGoods : NetHead
{
	UInt32 m_un32TypeID;
    Int32 m_nNum;
    public GCAskAddGoods(UInt32 unTypeID, Int32 nNum)
        : base()
    {
        m_un32TypeID = unTypeID;
        m_nNum = nNum;

		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_AskAddGoods;
    }
	
	public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
        memWrite.Write(this.m_un32TypeID);
        memWrite.Write(this.m_nNum);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCAskGMPassGate : NetHead
{
	UInt32 m_un32GateID;
    public GCAskGMPassGate(UInt32 gateId)
        : base()
    {
        m_un32GateID = gateId;

		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_AskGMPassGate;
    }
	
	public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
        memWrite.Write(this.m_un32GateID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}




[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCAskGMChangeLevel : NetHead
{
    UInt16 u16Level;
    byte isChangeVIP;

    public GCAskGMChangeLevel(UInt16 level, UInt16 isVIP)
        : base()
    {
        u16Level = level;
        isChangeVIP = (byte) isVIP;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskGMChangeLevel;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
        memWrite.Write(this.u16Level);
        memWrite.Write(this.isChangeVIP);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//GM请求接受任务
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GC_GM_AcceptTask : NetHead
{
    UInt32 taskID; //任务ID
    
    public GC_GM_AcceptTask(uint taskID): base()
    {
        this.taskID = (UInt32)taskID;
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_GMAcceptTask;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.taskID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//GM请求发送公告
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GC_GM_SendAnnouncement : NetHead
{
    private byte channel; //频道
    private byte privilege; //公告优先级
    private UInt16 strLen;  //公告长度
    private byte loopNum;   //公告的循环次数
    private byte nameLen;   //私聊名称的长度（或者公会名称的长度）
    private byte[] talkMsg; //私聊名称（或者公会名称）
    private byte[] name;    //公告内容

    public GC_GM_SendAnnouncement(string msg)
        : base()
    {
        this.channel = (byte)5;
        this.privilege = (byte)100;
        this.talkMsg = StringToByte(msg);
        this.strLen = (UInt16)this.talkMsg.Length;
        this.loopNum = (byte)1;
        this.nameLen = (byte)0;
        NetHead head = new NetHead();
        int headLength = Marshal.SizeOf(head) - 2;
        this._assistantCmd = (UInt16)eC2GType.C2G_AskGMSendAnnounce;
        this._length = (UInt16)(headLength + 4 + 2 + this.strLen + this.nameLen);
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.channel);
		memWrite.Write(this.strLen);
        memWrite.Write(this.privilege);       
        memWrite.Write(this.loopNum);
        memWrite.Write(this.nameLen);
        memWrite.Write(this.talkMsg);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}


