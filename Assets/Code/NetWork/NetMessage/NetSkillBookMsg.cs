using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using model;
using manager;
using MVC.entrance.gate;
//请求Vip礼包
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class CGAskVipAward : NetHead
{
    public byte VipId { get; set; }

    public CGAskVipAward()
        : base()
    {
		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_AskVipAward;
    }
	
	public new byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
        memWrite.Write(VipId);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//请求激活武器进度 
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class CGAskMedalLevelUp : NetHead
{
	public UInt32 m_MedalId;
    public CGAskMedalLevelUp(UInt32 nScheduleID)
        : base()
    {
		m_MedalId = nScheduleID;
		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_AskWepon;
    }
	
	public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
		memWrite.Write(this.m_MedalId);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//请求装备武器技能 
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class CGAskEquipWeaponSkill : NetHead
{
	public UInt32 	m_nWeaponSkillID;	//技能id 
	public int 		m_nWeaponType;		//武器类型 
	public byte 	m_nIndex;			//索引 
	
    public CGAskEquipWeaponSkill(UInt32 nSkillID, int nType, byte nIndex)
        : base()
    {
		m_nWeaponSkillID = nSkillID;
		m_nWeaponType = nType;
		m_nIndex = nIndex;
		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
		this._assistantCmd = (UInt16)eC2GType.C2G_AskEquipWeaponSkill;
    }
	
	public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
		memWrite.Write(this.m_nWeaponSkillID);
		memWrite.Write(this.m_nWeaponType);
		memWrite.Write(this.m_nIndex);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class NetSkillBookSkills : NetHead
{
	public NetSkillBookSkills()
	{
	}
	
	public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        uint num = memRead.ReadUInt32();
        if (num == 0)
        {
            return;
        }
        for (int i = 0; i < num; i++)
        {
            uint id = memRead.ReadUInt32(); 
            byte lvl = memRead.ReadByte();
            byte type = memRead.ReadByte();

            if (type == 0x00)
            {
                SkillTalentManager.Instance.SetActiveSkill(id, lvl);
            }
            else {
                SkillTalentManager.Instance.SetActiveTalents(id, lvl);
            }
            
        }
        SkillTalentManager.Instance.SetLockSkills();
        SkillTalentManager.Instance.SetRerushTalentUI();
        memRead.Close();
        memStream.Close();
    }
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class NetSkillBookEquip : NetHead
{
	public NetSkillBookEquip()
	{
	}
	
	public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        uint num = memRead.ReadUInt32();
        if (num == 0)
        {
            return;
        }
        for (int i = 0; i < num; i++)
        {
//            SkillEquipData tempItem = new SkillEquipData();
//            tempItem.nWeaponType = memRead.ReadInt32();
//            tempItem.nScheduleID = memRead.ReadUInt32();
//            tempItem.nSkillID[0] = memRead.ReadUInt32();
//            tempItem.nSkillID[1] = memRead.ReadUInt32();
//            tempItem.nSkillID[2] = memRead.ReadUInt32();
//            SkillBookManager.GetInstance().AddItem(tempItem);

            

			//Debug.Log("shop id: "+tempItem.unShopItemID+"\tshop type: "+tempItem.nShopType+"\titem id: "+tempItem.nItemID);
		}
		
//		SkillEquipData data = null;
//		if (CharacterPlayer.character_property.getCareer() == CHARACTER_CAREER.CC_SWORD)
//		{
//			data = SkillBookManager.GetInstance().GetEquipItemByWeapon((int)eWeaponSkillType.eSword);
//		}
//		else if (CharacterPlayer.character_property.getCareer() == CHARACTER_CAREER.CC_ARCHER)
//		{
//			data = SkillBookManager.GetInstance().GetEquipItemByWeapon((int)eWeaponSkillType.eArcher);
//		}
//		else if (CharacterPlayer.character_property.getCareer() == CHARACTER_CAREER.CC_MAGICIAN)
//		{
//			data = SkillBookManager.GetInstance().GetEquipItemByWeapon((int)eWeaponSkillType.eMagicBook);
//		}
//		
//		uint[] skillArr = new uint[4]{201001,201002,201003,201004};
//		
//		SkillManager.GetInstance().skillList.Clear();
//		for (int i = 0; i < 3; ++i)
//		{
//			SkillDataItem skillItem = SkillManager.GetInstance().GetSkillTempById(data.nSkillID[i]);
//			sSkillData skill = new sSkillData();    
//			skill.skillId = (uint)skillItem.id;
//            skill.hurtValue = (int)skillItem.attack_coefficient;
//            skill.mpConsumeValue = skillItem.mp_cost;
//            skill.coolDownTime = (int)skillItem.cool_down;
//            skill.lastTime = (int)skillItem.lastTick;
//            skill.fpCate = 0;
//            skill.fpValue = 0;
//            skill.canUse = 1;
//        	SkillManager.GetInstance().ChangeSkill(skill);
//		}
//		SkillBookEvent.GetInstance().OnSkillEquipEvent();
		
        memRead.Close();
        memStream.Close();
    }
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class NetNotifyWeaponSkillSchedule : NetHead
{
	public NetNotifyWeaponSkillSchedule()
	{
	}
	
	public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        uint nScheduleID = memRead.ReadUInt32();
        
		int type = (int)nScheduleID/1000;
//		SkillEquipData data = SkillBookManager.GetInstance().GetEquipItemByWeapon(type);
//		data.nScheduleID = nScheduleID;
//		SkillBookEvent.GetInstance().OnSkillUpgradeEvent();
        memRead.Close();
        memStream.Close();
    }
}

