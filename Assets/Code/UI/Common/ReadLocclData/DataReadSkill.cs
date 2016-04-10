using UnityEngine;
using System.Collections;
using model;
using manager;
public class SkillDataItem {
	public int id;
	public int nWeaponType;
    public eSkillType type;
	public string name;
    public string szDesc;
    public float attack_range;
    public bool qishou;
    public int suo_target;
    public int no_target;
    public string max_active;   //最大数据，-1：无此项数据
    public string icon;
	
    public uint active_level;
	//public float life;
	public float attack_coefficient;
	public float damage_plus;
    public float lastTick;
    public int cool_down;
    public int mp_cost;
    public eFighintPropertyCate addStateType;
    //public int addStateValue;                   //old,not need
    //public int skillCost;                   //old,not need
    //public int influenceHurtRate;                       //old,not need           
    //public int influenceMPConsume;                   //old,not need
    //public int influenceCDTime;                   //old,not need
    //public int influenceLastTime;                   //old,not need
    //public int influenceEffectValue;                   //old,not need
    public int nextSkill;

    public string skillDescription;            //技能描述，UI用
    public eGoldType lvlComsumeType1;           //升级花费
    public int lvlComsumeValue1;
    public eGoldType lvlComsumeType2;
    public int lvlComsumeValue2;
    public string skillLevelDescription;        //升级描述
    public eGoldType reLockComsumeType;         //解锁消耗
    public int reLockComsumeValue;

    public bool isShow;                         //是否在UI上显示此技能
    /*<skill ID="201001001" 
    weaponType="1" 
     * SkillType="1"
     * name="野蛮冲刺"
     * skillDescribe="野蛮冲刺"
     * attack_range="4" suo_target="1" no_target="1" icon="冲锋" 
     * initAdd="1" activeLevel="1" attackCoefficient="1" 
     * damagePlus="0" lastTick="0" cooldown="8000" MPexpend="70"
     * AddStateType="0" skill_dingwei="位移，眩晕"
     * shengji_xiaohao="1,100,4,20" shengji_miaoshu="攻击力提升10,眩晕几率提升1%"
     * jiesuo_jiage="2,100" nextSkill="201201001"/>
    */
 

    //public int attack_repel;
    //public int attack_fly;
    //public int attack_swoon;
    
    
	
    //public float repel_scale;

    //public float move_speed;
    
    
    
    
    //public float range;
    //public int appear_id; // 表现表id

    //public string strCastAnim;
    //public string strBeHitAnim;
    //public float fDelayAction;
    //public float fDelaySpecialEffect;
    //public string strSpecialEffectName;
}

public class DataReadSkill : DataReadBase {

    //private Hashtable SkillTypeDatas;
    public DataReadSkill():base()
    {
        //SkillTypeDatas = new Hashtable();
        //SkillTypeDatas.Add((int)CHARACTER_CAREER.CC_SWORD, new Hashtable());
        //SkillTypeDatas.Add((int)CHARACTER_CAREER.CC_ARCHER, new Hashtable());
        //SkillTypeDatas.Add((int)(int)CHARACTER_CAREER.CC_MAGICIAN, new Hashtable());
    }
    
	public override string getRootNodeName() {
		return "SkillConfig";
	}
	
	public override void appendAttribute(int key, string name, string value) {
		
        //SkillDataItem sdi;
		
        //if (!data.ContainsKey(key))  
        //{  
        //    sdi = new SkillDataItem();
        //    data.Add(key, sdi);
        //}
		
        //sdi = (SkillDataItem)data[key];


        SkillVo sv;
        if (SkillTalentManager.Instance.SkillHash.ContainsKey(key))
        {
            sv = SkillTalentManager.Instance.SkillHash[key] as SkillVo;
        }
        else {
            sv = new SkillVo();
            SkillTalentManager.Instance.SkillHash.Add(key, sv);
        }

        string[] strs;
		switch (name) {

       
		case "ID":
			//sdi.id = int.Parse(value);
            sv.XmlID = int.Parse(value);
            sv.SID = sv.XmlID / 1000;
			break;
		case "name":
			//sdi.name = value;
            sv.Name = value;
			break;
        case "attack_range":
            //sdi.attack_range = float.Parse(value);
            sv.Attack_Range = float.Parse(value);
            break;
        case "qishou":
            //sdi.qishou = int.Parse(value) == 1 ? true : false;
            sv.Qishou = int.Parse(value) == 1 ? true : false;
            break;
        case "suo_target":
           // sdi.suo_target = int.Parse(value);
            sv.Sou_Target = int.Parse(value);
            break;
        case "no_target":
            //sdi.no_target = int.Parse(value);
            sv.No_Target = int.Parse(value);
            break;
        case "cooldown":
            //sdi.cool_down = int.Parse(value);
            sv.Cool_Down = int.Parse(value);
            break;
        case "icon":
            //sdi.icon = value;
            sv.Icon = value;
            break;
		case "MPexpend":
            //sdi.mp_cost = int.Parse(value);
            sv.Mp_Cost = int.Parse(value);
            break;
		case "attackCoefficient":
            //sdi.attack_coefficient = float.Parse(value);
            sv.Attack_Coefficient = float.Parse(value);
            break;
		case "damagePlus":
            //sdi.damage_plus = float.Parse(value);
            sv.Damage_Plus = float.Parse(value);
            break;
       case "MaxActive":
            //sdi.max_active = value.Trim();
            sv.Max_Active = value.Trim();
            break;
        case "SkillType":
            //sdi.type = (eSkillType)(int.Parse(value));
            sv.Type = (eSkillType)(int.Parse(value));
            break;
        case "activeLevel":
            //sdi.active_level = uint.Parse(value);
            sv.Active_Level = int.Parse(value);
            break;
        case "AddStateType":
            //sdi.addStateType = (eFighintPropertyCate)int.Parse(value);
            sv.AddStateType = (eFighintPropertyCate)int.Parse(value);
            break;
        case "lastTick":
            //sdi.lastTick = float.Parse(value) / 1000.0f;
            sv.LastTick = float.Parse(value) / 1000.0f;
            break;
        case "nextSkill":
            //sdi.nextSkill = int.Parse(value);
            sv.NextSkillID = int.Parse(value);
            break;
		case "weaponType":
			//sdi.nWeaponType = int.Parse(value);
            sv.WeaponType = int.Parse(value);
            if (SkillTalentManager.Instance.CareerSkillList.ContainsKey((CHARACTER_CAREER)sv.WeaponType))
            {
                Hashtable hash = SkillTalentManager.Instance.CareerSkillList[(CHARACTER_CAREER)sv.WeaponType] as Hashtable;
                hash[sv.XmlID] = sv;
            }
            
			break;
		case "skillDescribe":
			//sdi.szDesc = value;
            sv.SzDesc = value;
			break;


        case "jiesuo_jiage":
            string[] s = value.Split(',');
            //sdi.reLockComsumeType = (eGoldType)int.Parse(s[0]);
            //sdi.reLockComsumeValue = int.Parse(s[1]);
            sv.UnLockType = (eGoldType)int.Parse(s[0]);
            sv.UnLockValue = int.Parse(s[1]);
            break;
        case "shengji_miaoshu":
            //sdi.skillLevelDescription = value;
            sv.SkillLevelDescription = value;
            break;
        case "shengji_xiaohao":
            strs = value.Split(',');
            for (int i = 0; i < strs.Length; i+=2)
            {
                sv.Consume.Add(new TypeStruct
                {
                    Type = ConsumeType.Gold,
                    Id = int.Parse(strs[i]),
                    Value = int.Parse(strs[i+1])
                });
            }
            break;
        case "shengji_item":
            strs = value.Split(',');
            for (int i = 0; i < strs.Length; i += 2)
            {
                sv.Consume.Add(new TypeStruct
                {
                    Type = ConsumeType.Item,
                    Id = int.Parse(strs[i]),
                    Value = int.Parse(strs[i+1])
                });
            }
            break;
        case "skill_dingwei":
            //sdi.skillDescription = value;
            sv.SkillDescription = value;
            break;
        case "isshow":
            //sdi.isShow = int.Parse(value) == 0 ? false : true;
            sv.IsShow = int.Parse(value) == 0 ? false : true;
            break;
            
		}  
	}

    public SkillDataItem getSkillData(int key)
    {

        SkillDataItem sdi = new SkillDataItem(); ;
        if (!SkillTalentManager.Instance.SkillHash.ContainsKey(key))
        {
            return sdi;
        }
        SkillVo sv = SkillTalentManager.Instance.SkillHash[key] as SkillVo;
        sdi.id = sv.XmlID;
        sdi.name = sv.Name;
        sdi.attack_range = sv.Attack_Range;
        sdi.qishou = sv.Qishou;
        sdi.suo_target = sv.Sou_Target;
        sdi.no_target = sv.No_Target;
        sdi.cool_down = sv.Cool_Down;
        sdi.icon = sv.Icon;
        sdi.mp_cost = sv.Mp_Cost;
        sdi.attack_coefficient = sv.Attack_Coefficient;
        sdi.damage_plus = sv.Damage_Plus;
        sdi.max_active = sv.Max_Active;
        sdi.type = sv.Type;
        sdi.active_level = (uint)sv.Active_Level;
        sdi.addStateType = sv.AddStateType;
        sdi.lastTick = sv.LastTick;
        sdi.nextSkill = sv.NextSkillID;
        sdi.nWeaponType = sv.WeaponType;
        sdi.szDesc = sv.SzDesc;
        sdi.reLockComsumeType = sv.UnLockType;
        sdi.reLockComsumeValue = sv.UnLockValue;
        sdi.skillLevelDescription = sv.SkillLevelDescription;
        //sdi.lvlComsumeType1 = sv.MoneyType1;
        //sdi.lvlComsumeValue1 = sv.MoneyValue1;
        //sdi.lvlComsumeType2 = sv.MoneyType2;
        //sdi.lvlComsumeValue2 = sv.MoneyValue2;
        sdi.skillDescription = sv.SkillDescription;
        sdi.isShow = sv.IsShow;
        return sdi;
    }

    public Hashtable getHashtable()
    {
        return data;
    }
}



public class DataReadTalent : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        TalentVo td;

        if (!SkillTalentManager.Instance.TalentHash.ContainsKey(key))
        {
            td = new TalentVo();
            SkillTalentManager.Instance.TalentHash.Add(key, td);
        }

        td = (TalentVo)SkillTalentManager.Instance.TalentHash[key];

        switch (name)
        {
            case "ID":
                td.XmlId = int.Parse(value);
                td.SId = td.XmlId / 1000;
                break;
            case "Name":
                td.Name = value;
                break;
            case "icon":
                td.Icon = value;
                break;
            case "kaifang_LV":
                td.DisplayLevel = int.Parse(value);
                break;
            case "LV_shangxian":
                td.MaxLevel = int.Parse(value);
                break;
            case "shuxing_type":
                td.TalentType = int.Parse(value);
                break;
            case "shuxing_num":
                td.TalentValue = int.Parse(value);
                break;
            case "xiaohao_type":
                td.ComsumeType = int.Parse(value);
                break;
            case "xiaohao_num":
                td.ComsumeValue = int.Parse(value);
                break;
            case "miaoshu":
                td.Description = value;
                break;
            default:
                break;
        }
    }


    public TalentVo getTalentDataItem(int key)
    {
        if (!data.Contains(key))
        {
            return new TalentVo();
        }
        return (TalentVo)SkillTalentManager.Instance.TalentHash[key];
    }
}

