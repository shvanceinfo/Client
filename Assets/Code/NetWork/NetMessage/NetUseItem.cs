using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using manager;
using model;


[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskUseItem : NetHead
{
    public UInt16 m_un16OperateID;
    public UInt32 m_u32MaterialID;

    public GCAskUseItem(UInt16 operateId, UInt32 materialId): base()
    {
        _length = (UInt16)(Marshal.SizeOf(this) - 2);
        _assistantCmd = (UInt16)eC2GType.C2G_AskUseItem;
        m_un16OperateID = operateId;
        m_u32MaterialID = materialId;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(m_un16OperateID);
        memWrite.Write(m_u32MaterialID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}