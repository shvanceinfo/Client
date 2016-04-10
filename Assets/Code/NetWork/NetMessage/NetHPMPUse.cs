using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCReportHPVessel : NetHead
{
	UInt32 m_un32CurNum;
    public GCReportHPVessel(UInt32 unNum)
        : base()
    {
        m_un32CurNum = unNum;

		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_ReportHPVessel;
    }
	
	public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
        memWrite.Write(this.m_un32CurNum);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}



[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCReportMPVessel : NetHead
{
    UInt32 m_un32CurNum;
    public GCReportMPVessel(UInt32 unNum)
        : base()
    {
        m_un32CurNum = unNum;

        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_ReportMPVessel;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
        memWrite.Write(this.m_un32CurNum);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}