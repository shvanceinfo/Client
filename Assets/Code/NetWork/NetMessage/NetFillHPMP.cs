using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskFillHP : NetHead
{
    uint m_nMount;

    public GCAskFillHP(uint mount)
        : base()
    {
        _length = (UInt16)(Marshal.SizeOf(this) - 2);
        _assistantCmd = (UInt16)eC2GType.C2G_AskBuyHPVessel;
        m_nMount = mount;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(m_nMount);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        memStream.Close();
        return bytesData;
    }
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskFillMP : NetHead
{
    uint m_nMount;

    public GCAskFillMP(uint mount)
        : base()
    {
        _length = (UInt16)(Marshal.SizeOf(this) - 2);
        _assistantCmd = (UInt16)eC2GType.C2G_AskBuyMPVessel;
        m_nMount = mount;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(m_nMount);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        memStream.Close();
        return bytesData;
    }
}