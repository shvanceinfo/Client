using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using manager;
using model;
using System.Threading;


public enum UseItemInWorldBossType{
	Rongyu=1,
	Zhuanshi = 2
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCAskUseItemInWorldBoss : NetHead
{
    int m_n32Item;

    public GCAskUseItemInWorldBoss()
        : base()
    {
		this.m_n32Item = 0;
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskUseItemInWorldBoss;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes(int item)
    {
		this.m_n32Item = item;
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(m_n32Item);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}


[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCReportAttackedTarget : NetHead
{
    public UInt32 m_un32TargetID;
    public UInt32 m_un32SkillID;

    public GCReportAttackedTarget()
        : base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_ReportAttackedTarget;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(m_un32TargetID);
        memWrite.Write(m_un32SkillID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}


//通知世界boss buff使用次数
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyWorldBossUseItemResult:NetHead
{
	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		
		byte count = memRead.ReadByte(); //buff的次数
		
		memRead.Close ();
		memStream.Close ();

		BossManager.Instance.UpdateBuffInfo(count);
 
	}
} 




