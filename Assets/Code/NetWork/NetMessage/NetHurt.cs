using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

// 汇报伤害
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCReportHurt : NetHead
{
    UInt32 ownerID; //来源ID
    UInt32 targetID; //目标ID
    UInt32 skillID;
    Int32 hurtNum;
    Int32 hurtRemainHp;

    public GCReportHurt(UInt32 ownerid, UInt32 targetid, UInt32 skillid, Int32 hurt, Int32 remainhp)
        : base()
    {
        _length = (UInt16)(Marshal.SizeOf(this) - 2);
        _assistantCmd = (UInt16)eC2GType.C2G_ReportHurt;
        ownerID = ownerid;
        targetID = targetid;
        skillID = skillid;
        hurtNum = hurt;
        hurtRemainHp = remainhp;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(ownerID);
        memWrite.Write(targetID);
        memWrite.Write(skillID);
        memWrite.Write(hurtNum);
        memWrite.Write(hurtRemainHp);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        memStream.Close();
        return bytesData;
    }
}



// 汇报CD
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCReportCD : NetHead
{
    UInt32 m_un32SkillID;

    public GCReportCD(UInt32 skillid)
        : base()
    {
        _length = (UInt16)(Marshal.SizeOf(this) - 2);
        _assistantCmd = (UInt16)eC2GType.C2G_AskCheckSkill;
        m_un32SkillID = skillid;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(m_un32SkillID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        memStream.Close();
        return bytesData;
    }
}



