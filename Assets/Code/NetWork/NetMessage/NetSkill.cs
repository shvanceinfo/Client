using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using manager;

/// <summary>
/// 技能变更
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyCurSkillInfo : NetHead
{
    public uint num;
    public GSNotifyCurSkillInfo()
    {
    }

    public void ToObject(byte[] byteData)
    {
        //MemoryStream memStream = new MemoryStream(byteData);
        //BinaryReader memRead = new BinaryReader(memStream);
        //base.ToObject(ref memRead);

        //uint selectedWeaponSkillID = memRead.ReadUInt32();

        //num = memRead.ReadUInt32();
       
        //if (num == 0)
        //{
        //    return;
        //}

        //for (int i = 0; i < num; i++)
        //{
        //    sSkillData skill = new sSkillData();    
        //    skill.skillId = memRead.ReadUInt32();
        //    skill.hurtValue = memRead.ReadInt32();
        //    skill.mpConsumeValue = memRead.ReadInt32();
        //    skill.coolDownTime = memRead.ReadInt32();
        //    skill.lastTime = memRead.ReadInt32();
        //    skill.fpCate = (eFighintPropertyCate)memRead.ReadInt32();
        //    skill.fpValue = memRead.ReadInt32();
        //    skill.canUse = memRead.ReadByte();
        //    SkillManager.GetInstance().ChangeSkill(skill);
        //}
        //memRead.Close();
        //memStream.Close();
        //SkillEvent.GetInstance().OnSkillChange();
    }
}

/// <summary>
/// 技能卡列表
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyCanActiveSkillCardList : NetHead
{
    public uint num;

    public GSNotifyCanActiveSkillCardList()
    {
    }

    public void ToObject(byte[] byteData)
    {
        
    }
}

/// <summary>
/// 技能点变更
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifySkillPointChange : NetHead
{
    public uint num;
    public GSNotifySkillPointChange()
    {
    }

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        checked{
            SkillTalentManager.Instance.SkillPoint = (int)memRead.ReadUInt32();
        }
        memRead.Close();
        memStream.Close();
    }
}

/// <summary>
/// 请求技能卡列表
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskActiveSkillCard : NetHead
{
    private uint skillCardId;

    public GCAskActiveSkillCard(uint cardId) : base()
    {
        skillCardId = cardId;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskActiveSkillCard;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(skillCardId);
        byte[] bytesData = memStream.ToArray();        
        memWrite.Close();
        memStream.Close();
        return bytesData;
    }
}

/// <summary>
/// 刷新技能卡列表
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskRefreshActiveSkillCard : NetHead
{

    public GCAskRefreshActiveSkillCard()
        : base()
    {
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskRefreshActiveSkillCard;
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

/// <summary>
/// 请求购买技能点
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskBuySkillPoint : NetHead
{
    private uint buyNum;
    public GCAskBuySkillPoint(uint num)
        : base()
    {
        this.buyNum = num;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskBuySkillPoint;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(buyNum);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        memStream.Close();
        return bytesData;
    }
}

/// <summary>
/// 选择技能
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskSelectWeaponSkill : NetHead
{
    //uint skillId;
    //public GCAskSelectWeaponSkill(uint skillId)
    //    : base()
    //{
    //    this.skillId = skillId;
    //    this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    //    this._assistantCmd = (UInt16)eC2GType.C2G_AskSelectWeaponSkill;
    //}

    //public byte[] ToBytes()
    //{
    //    MemoryStream memStream = new MemoryStream();
    //    BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
    //    base.ToBytes(ref memWrite);
    //    memWrite.Write(skillId);
    //    byte[] bytesData = memStream.ToArray();
    //    memWrite.Close();
    //    memStream.Close();
    //    return bytesData;
    //}
}