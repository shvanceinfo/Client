using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using MVC.entrance.gate;
using model;
using manager;

// 请求系统公告
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskNotice : NetHead
{
    UInt32 msgType; //1:系统公告;2:客服信息;3:运维公告

    public GCAskNotice(PostType type)
        : base()
    {
        this.msgType = (UInt32)type;
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskSystemNotice;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.msgType);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//服务器返回公告，客服信息，运维公告
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyService : NetHead
{
    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);

        uint msgID = memRead.ReadUInt32();
        PostType type = (PostType)memRead.ReadInt32();
        UInt32 time = memRead.ReadUInt32();
        string msgTime = ToolFunc.GetDateTime(time).ToString("yyyy年MM月dd日");
        uint titleLen = memRead.ReadUInt32();
        uint contentLen = memRead.ReadUInt32();
        uint authorLen = memRead.ReadUInt32();
        string title = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes((int)titleLen)));
        string content = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes((int)contentLen)));
        string author = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes((int)authorLen)));
        memRead.Close();
        memStream.Close();
        SettingManager.Instance.AddPost((int)msgID, type, msgTime, title, content, author);
    }
}

//汇报bug，建议以及反馈
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCReportBug : NetHead
{
    UInt32 reportType;
    UInt32 titleLen;
    UInt32 contentLen;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = TalkManager.MAX_MSG_LEN)]   
    byte[] title;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = TalkManager.MAX_MSG_LEN * 2)]
    byte[] content; //不超过80字符
    public GCReportBug(REPORT_TYPE type, string content) : base()
    {
        this.reportType = (UInt32)type;
        this.titleLen = 0;
        this.content = StringToByte(content);
        this.contentLen = (UInt32)this.content.Length;
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_ReportBug;
        this._length = (UInt16)(Marshal.SizeOf(head) - 2 + 4*3 + this.contentLen);
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.reportType);
        memWrite.Write(this.titleLen);
        memWrite.Write(this.contentLen);
        memWrite.Write(this.content);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//体力信息的回复
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GSNotifyEngery : NetHead
{
	public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        CharacterPlayer.character_property.currentEngery = memRead.ReadInt32();
        CharacterPlayer.character_property.buyEngeryNum = memRead.ReadInt32();
        memRead.Close();
        memStream.Close();
        Gate.instance.sendNotification(MsgConstant.MSG_INIT_POWER_ENGERY);
    }
}

//购买体力
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskBuyEngery : NetHead
{
    public GCAskBuyEngery() : base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskBuyPower;
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


