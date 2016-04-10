using manager;
using NetGame;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// 请求线路表
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskChnnelList : NetHead
{

    public GCAskChnnelList()
        : base()
    {
        int headLength = Marshal.SizeOf(this) - 2;
        this._assistantCmd = (UInt16)eC2GType.C2G_AskChannelList;
        this._length = (UInt16)(headLength);

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

/// <summary>
/// 请求改变线路
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskChnnelChange : NetHead
{
    public byte channel;
    public GCAskChnnelChange()
        : base()
    {
        channel = 0;
        int headLength = Marshal.SizeOf(this) - 2;
        this._assistantCmd = (UInt16)eC2GType.C2G_AskChannelChangel;
        this._length = (UInt16)(headLength);
    }


    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(channel);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}


/// <summary>
/// 当前线路
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyChannelCur : NetHead
{
    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        ChannelManager.Instance.UpdateCurLine(memRead.ReadByte());
        memRead.Close();
        memStream.Close();
        DungeonManager.Instance.InitDungeon(); //初始化副本信息
    }
}
/// <summary>
/// 游戏线路表
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyChannelList: NetHead
{
    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);

        uint count = memRead.ReadUInt32();
        if (count<=0)
        {
            return;
        }
        ChannelManager.Instance.Lines.Clear();
        for (int i = 0; i < count; i++)
        {
            byte line = memRead.ReadByte();
            ushort people = memRead.ReadUInt16();
            ChannelManager.Instance.UpdateChannel(line, people);
        }
        ChannelManager.Instance.OpenWindow();
        memRead.Close();
        memStream.Close();
    }
}