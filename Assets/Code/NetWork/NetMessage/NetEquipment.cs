using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
/// client-------------------->server

/// <summary>
/// 强化
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskIntensifyEquip : NetHead
{
    /// <summary>
    /// public 
    /// </summary>
    public UInt16 u16bagPos;
    /// <summary>
    /// 使用的幸运石ID（表中的索引ID）(0代表不使用幸运石）
    /// </summary>
    public UInt32 u32LuckStoneId;
    /// <summary>
    /// 1代表进阶装备，0代表强化装备
    /// </summary>
    public byte option;

    public GCAskIntensifyEquip()
        : base()
    {
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskIntensifyEquip;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.u16bagPos);
        memWrite.Write(this.u32LuckStoneId);
        memWrite.Write(this.option);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

/// <summary>
/// 宝石合成
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskCombineGem : NetHead
{
    /// <summary>
    /// 要合成的装备（宝石）ID
    /// </summary>
    public UInt32 _un32Id;

    /// <summary>
    /// 需要的幸运石ID（0代表不使用幸运石)
    /// </summary>
    public UInt32 _un32LuckStoneId;
    /// <summary>
    /// 需要的合成的数量（1-99)
    /// </summary>
    public UInt16 _un16MergeCount;

    /// <summary>
    /// 合成装备还是宝石（１：装备合成，０：宝石合成）
    /// </summary>
    public byte _un8Option;

    public GCAskCombineGem()
        : base()
    {
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskCombineGem;
        
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(_un32Id);
        memWrite.Write(_un32LuckStoneId);
        memWrite.Write(_un16MergeCount);
        memWrite.Write(_un8Option);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

/// <summary>
/// 装备洗炼
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskWashEquip : NetHead
{
    public UInt16 instanceId;
    /// <summary>
    /// 是否重置属性（1：是，0：否（就是洗练属性）
    /// </summary>
    public byte isReset;

    /// <summary>
    /// 要洗练的索引（0,1,2三个位置（重置的时候使用1，2，4，8，16，32））
    /// </summary>
    public byte index;

    public GCAskWashEquip()
        : base()
    {
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskWashEquip;

    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.instanceId);
        memWrite.Write(this.isReset);
        memWrite.Write(this.index);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

/// <summary>
/// 宝石镶嵌
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskInlayGem : NetHead
{
    public byte part;
    public byte index;
    public UInt32 id;

    public GCAskInlayGem()
        : base()
    {
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskInlayGem;

    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.part);
        memWrite.Write(this.index);
        memWrite.Write(this.id);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

///// <summary>
///// 宝石移除
///// </summary>
//[Serializable]
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//class GCAskRemoveGem : NetHead
//{
//    UInt32 itemPos;
//    UInt32 gemHoleIndex;
//    /// <summary>
//    /// 宝石移除
//    /// </summary>
//    /// <param name="equipPos">装备ID</param>
//    /// <param name="index">镶嵌位置</param>
//    public GCAskRemoveGem(UInt32 equipPos, UInt32 index)
//        : base()
//    {
//        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
//        this._assistantCmd = (UInt16)eC2GType.C2G_AskRemoveGem;
//        this.itemPos = equipPos;
//        this.gemHoleIndex = index;
//    }

//    public byte[] ToBytes()
//    {
//        MemoryStream memStream = new MemoryStream();
//        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
//        base.ToBytes(ref memWrite);
//        memWrite.Write(this.itemPos);
//        memWrite.Write(this.gemHoleIndex);
//        byte[] bytesData = memStream.ToArray();
//        memWrite.Close();
//        return bytesData;
//    }
//}

///// <summary>
///// 装备进阶
///// </summary>
//[Serializable]
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//class GCAskUpgradeEquip : NetHead
//{
//    UInt32 itemPos;
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="itemId">强化物品ID</param>
//    public GCAskUpgradeEquip(UInt32 itemId)
//        : base()
//    {
//        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
//        this._assistantCmd = (UInt16)eC2GType.C2G_AskUpgradeEquip;
//        this.itemPos = itemId;
//    }

//    public byte[] ToBytes()
//    {
//        MemoryStream memStream = new MemoryStream();
//        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
//        base.ToBytes(ref memWrite);
//        memWrite.Write(this.itemPos);
//        byte[] bytesData = memStream.ToArray();
//        memWrite.Close();
//        return bytesData;
//    }
//}

///// <summary>
///// 分解
///// </summary>
//[Serializable]
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//class GCAskDecomposeEquip : NetHead
//{
//    UInt32 itemPos;
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="itemId">物品ID</param>
//    public GCAskDecomposeEquip(UInt32 itemId)
//        : base()
//    {
//        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
//        this._assistantCmd = (UInt16)eC2GType.C2G_AskDecomposeEquip;
//        this.itemPos = itemId;
//    }

//    public byte[] ToBytes()
//    {
//        MemoryStream memStream = new MemoryStream();
//        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
//        base.ToBytes(ref memWrite);
//        memWrite.Write(this.itemPos);
//        byte[] bytesData = memStream.ToArray();
//        memWrite.Close();
//        return bytesData;
//    }
//}

/// <summary>
/// 传承
/// </summary>
//[Serializable]
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//class GCAskTransmitEquipInstensify : NetHead
//{
//    UInt32 srcInstanceId;
//    UInt32 tagInstanceId;
//    public GCAskTransmitEquipInstensify(UInt32 srcId, UInt32 tagId)
//        : base()
//    {
//        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
//        this._assistantCmd = (UInt16)eC2GType.C2G_AskTransmitEquip;
//        this.srcInstanceId = srcId;
//        this.tagInstanceId = tagId;
//    }

//    public byte[] ToBytes()
//    {
//        MemoryStream memStream = new MemoryStream();
//        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
//        base.ToBytes(ref memWrite);
//        memWrite.Write(this.srcInstanceId);
//        memWrite.Write(this.tagInstanceId);
//        byte[] bytesData = memStream.ToArray();
//        memWrite.Close();
//        return bytesData;
//    }
//}