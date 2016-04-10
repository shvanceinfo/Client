using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskAchievementList : NetHead
{
    private uint m_un32AchievementID;

    public GCAskAchievementList() : base()
    {
        _length = (UInt16)(Marshal.SizeOf(this) - 2);
        _assistantCmd = (UInt16)eC2GType.C2G_AskAchievementList;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        memStream.Close();
        return bytesData;
    }
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCNotifyAchievementChange : NetHead
{
    public uint m_ui32ID;
    public int m_n32Schedule;
    public byte m_bIfCompleted;
    public byte m_bIfReceived;


    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);

        int nMissionNum = (int)memRead.ReadUInt32();

        for (int i = 0; i < nMissionNum; ++i )
        {
            m_ui32ID = memRead.ReadUInt32();
            m_n32Schedule = memRead.ReadInt32();
            m_bIfCompleted = memRead.ReadByte();
            m_bIfReceived = memRead.ReadByte();
        }

        memRead.Close();
        memStream.Close();
    }
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCNotifyTitleChange : NetHead
{
    public uint m_un32CurTitleID;
    public uint m_un32PlayerID;

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);

        m_un32CurTitleID = memRead.ReadUInt32();
        m_un32PlayerID = memRead.ReadUInt32();

        memRead.Close();
        memStream.Close();
    }
}


[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskAchievementPrize : NetHead
{
    uint m_un32AchievementID;

    public GCAskAchievementPrize(uint id)
        : base()
    {
        _length = (UInt16)(Marshal.SizeOf(this) - 2);
        _assistantCmd = (UInt16)eC2GType.C2G_AskAchievementPrize;
        m_un32AchievementID = id;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(m_un32AchievementID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        memStream.Close();
        return bytesData;
    }
}




[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCReportAchievementSchedule : NetHead
{
    uint m_un32AchievementID;
    int m_n32Schedule;
    byte m_bIfComplete;

    public GCReportAchievementSchedule(uint id, int schedule, byte complete)
        : base()
    {
        _length = (UInt16)(Marshal.SizeOf(this) - 2);
        _assistantCmd = (UInt16)eC2GType.C2G_ReportAchievementSchedule;
        m_un32AchievementID = id;
        m_n32Schedule = schedule;
        m_bIfComplete = complete;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(m_un32AchievementID);
        memWrite.Write(m_n32Schedule);
        memWrite.Write(m_bIfComplete);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        memStream.Close();
        return bytesData;
    }
}


[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskUseTitle : NetHead
{
    uint m_un32ID;

    public GCAskUseTitle(uint id)
        : base()
    {
        _length = (UInt16)(Marshal.SizeOf(this) - 2);
        _assistantCmd = (UInt16)eC2GType.C2G_AskUseTitle;
        m_un32ID = id;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(m_un32ID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        memStream.Close();
        return bytesData;
    }
}




