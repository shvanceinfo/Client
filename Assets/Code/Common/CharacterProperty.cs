using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using NetGame;
using MVC.entrance.gate;
using manager;

public enum CHARACTER_CAREER
{
    CC_BEGIN = 0,
    CC_SWORD = 1, //剑士
	CC_ARCHER = 2,   //弓箭手
	CC_MAGICIAN = 3,  //法师
    CC_END
}

/// 新加属性[2013/8/23]
public class CharacterFightProperty
{
    public Dictionary<eFighintPropertyCate, int> fightData = new Dictionary<eFighintPropertyCate,int>();

    public Dictionary<eFighintPropertyCate, int> buffData = new Dictionary<eFighintPropertyCate, int>();


    

    public float m_fBuffSpeed = 1.0f;

    public float m_fFrozenRate = 1.0f;

    public CharacterFightProperty()
    {
        m_fBuffSpeed = 1.0f;
        m_fFrozenRate = 1.0f;
    }

    // 需要有个缺省值
    public void ResetProperty()
    {
        if (fightData.Count == 0)
        {
            fightData.Add(eFighintPropertyCate.eFPC_MaxHP, 0);
            fightData.Add(eFighintPropertyCate.eFPC_HPRecover, 0);
            fightData.Add(eFighintPropertyCate.eFPC_MaxMP, 0);
            fightData.Add(eFighintPropertyCate.eFPC_MPRecover, 0);
            fightData.Add(eFighintPropertyCate.eFPC_Attack, 0);
            fightData.Add(eFighintPropertyCate.eFPC_Defense, 0);



            fightData.Add(eFighintPropertyCate.eFPC_Precise, 0);
            fightData.Add(eFighintPropertyCate.eFPC_Dodge, 0);
            fightData.Add(eFighintPropertyCate.eFPC_BlastAttack, 0);
            fightData.Add(eFighintPropertyCate.eFPC_BlastAttackAdd, 0);

            fightData.Add(eFighintPropertyCate.eFPC_BlastAttackReduce, 0);
            fightData.Add(eFighintPropertyCate.eFPC_Tenacity, 0);
            fightData.Add(eFighintPropertyCate.eFPC_FightBreak, 0);
            fightData.Add(eFighintPropertyCate.eFPC_AntiFightBreak, 0);

            fightData.Add(eFighintPropertyCate.eFPC_IceAttack, 0);
            fightData.Add(eFighintPropertyCate.eFPC_AntiIceAttack, 0);
            fightData.Add(eFighintPropertyCate.eFPC_FireAttack, 0);
            fightData.Add(eFighintPropertyCate.eFPC_AntiFireAttack, 0);

            fightData.Add(eFighintPropertyCate.eFPC_PoisonAttack, 0);
            fightData.Add(eFighintPropertyCate.eFPC_AntiPoisonAttack, 0);
            fightData.Add(eFighintPropertyCate.eFPC_ThunderAttack, 0);
            fightData.Add(eFighintPropertyCate.eFPC_AntiThunderAttack, 0);
        }
    }


    public void ResetBuffData()
    {
        foreach (KeyValuePair<eFighintPropertyCate, int> item in buffData)
        {
            buffData[item.Key] = 0;
        }

        m_fBuffSpeed = 1.0f;

        m_fFrozenRate = 1.0f;
    }

    //public void SetBaseSpeed(float speed)
    //{
    //    m_fBaseSpeed = speed;
    //}

    public void SetBuffSpeed(float speed)
    {
        m_fBuffSpeed = speed;
    }

    //public float GetBaseSpeed()
    //{
    //    return m_fBaseSpeed;
    //}

    public float GetBuffSpeed()
    {
        return m_fBuffSpeed;
    }

    //public float GetSpeed()
    //{
    //    return GetBaseSpeed() * GetBuffSpeed();
    //}

    public void SetBaseValue(eFighintPropertyCate type, int value)
    {
        if (fightData.ContainsKey(type))
        {
            if(PowerManager.Instance.IsFristGet)
                PowerManager.Instance.PushAttribute(type, value - fightData[type]);
            fightData[type] = value;
        }
    }

    public void SetBuffValue(eFighintPropertyCate type, int value)
    {
        if (buffData.ContainsKey(type))
        {
            buffData[type] += value;
        }
    }

    public int GetValue(eFighintPropertyCate type)
    {
        return GetBaseValue(type) + GetBuffValue(type);
    }

    public int GetBaseValue(eFighintPropertyCate type)
    {
        int nRet = 0;

        if (fightData.ContainsKey(type))
        {
            nRet += fightData[type];
        }

        return nRet;
    }

    public int GetBuffValue(eFighintPropertyCate type)
    {
        int nRet = 0;

        if (buffData.ContainsKey(type))
        {
            nRet += buffData[type];
        }

        return nRet;
    }
}

public class CharacterProperty : CharacterBaseProperty
{


	//public int hp;
	public int mp;
	//public int attack_power;
	//public float attack_range;
	//public int defence;
	//public float move_speed;
	public float action_speed;
	public int property_ice;
	public int property_fire;
	public int property_poison;
	public int property_elec;
	private int experience;
	//public int level;
	public float extrusion_range;
	
	public int server_id;
	public string nick_name;
    public CHARACTER_CAREER career;
	public int sex;  // 1 是男性， 0 是女性
	public int weapon;
	public int armor;
	public int hp_max;
	public int mp_max;
	public int property_ice_def;
	public int property_fire_def;
	public int property_poison_def;
	public int property_elec_def;
	
	public int server_scene_id = -1;
	public int server_map_id = -1;
	public int client_map_id = -1;
    public UInt32 cur_hp_vessel;
    public UInt32 cur_mp_vessel;
    public UInt32 max_hp_vessel;
    public UInt32 max_mp_vessel;
    public int currentEngery;	//当前体力
    public int buyEngeryNum; //当前购买体力次数
	
	public Vector3 enter_city_pos;
	
	public bool host_computer = true;


    /// 新加属性[2013/8/23]
    public uint coatTempId;
    public uint legGuardTempId;
    public uint shoeTempId;
    public uint necklaceTempId;
    public uint ringTempId;
    public uint greatSwordTempId;
    public uint bowTempId;
    public uint doubleSwordTempId;
    

    /// 新增可用包裹格子数
    public uint maxPackages;

	public override void setHP(int h) {
        base.setHP(h);
        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_REFRESH_HEALT_MAGIC);
        //EventDispatcher.GetInstance().OnPlayerProperty();
	}
	
	public override int getHP() {
		return hp;	
	}
	
	public override void setHPMax(int h) {
		hp_max = h;
        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_REFRESH_HEALT_MAGIC);
	}
	
	public override int getHPMax() {
		return hp_max;	
	}
	
	public override void SetMP(int m) 
    {
		mp = m;
		if (mp < 0)
			mp = 0;
		 EventDispatcher.GetInstance().OnPlayerProperty();
         Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_REFRESH_HEALT_MAGIC);
	}
	
	public override int GetMP() 
    {
		return mp;	
	}
	
	public void setMPMax(int h) {
		mp_max = h;
        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_REFRESH_HEALT_MAGIC);
	}
	
	public int getMPMax() {
		return mp_max;	
	}

    public override float GetHpRatio()
    {
        if (hp == hp_max)
        {
            return 0.9999f;
        }
        else
            return (float)hp / (float)hp_max;
    }

    public override float GetMpRatio()
    {
        if (mp == mp_max)
        {
            return 0.9999f;
        }
        else
            return (float)mp / (float)mp_max;
    }
	
	public override void setAttackPower(int ap) {
		attack_power = ap;
	}
	
	public override int getAttackPower() {
		return attack_power;
	}
	
	public override void setAttackRange(float ar) {
		attack_range = ar;
	}
	
	public override float getAttackRange() {
		return attack_range;
	}
	
	public override void setDefence(int def) {
		defence = def;
	}
	
	public override int getDefence() {
		return defence;
	}
	
    //public override void setMoveSpeed(float ms) {
    //    move_speed = ms;
    //}
	
    //public override float getMoveSpeed() {
    //    return move_speed;
    //}
	
	public void setActionSpeed(float acts) {
		action_speed = acts;
	}
	
	public float getActionSpeed() {
		return action_speed;
	}
	
	public void setPropertyIce(int pi) {
		property_ice = pi;
	}
	
	public int getPropertyIce() {
		return property_ice;
	}
	
	public void setPropertyFire(int pf) {
		property_fire = pf;
	}
	
	public int getPropertyFire() {
		return property_fire;
	}
	
	public void setPropertyPoison(int pp) {
		property_poison = pp;
	}
	
	public int getPropertyPoison() {
		return property_poison;
	}
	
	public void setPropertyElec(int pe) {
		property_elec = pe;
	}
	
	public int getPropertyElec() {
		return property_elec;
	}
	
	public void setExperience(int e) {
        try
        {
            experience = e;
            if (experience<0)
            {
                throw new Exception();
            }
            Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_EXP_CHANGE);
            EventDispatcher.GetInstance().OnPlayerProperty();
        }
        catch (Exception ea)
        {
            Debug.Log(ea);
            throw;
        }
		
	}
	
	public int getExperience() {
		return experience;
	}
	
    //public void setLevel(int l) {
    //    level = l;
    //}
	
    //public int getLevel() {
    //    return level;
    //}
	
	public void setExtrusionRange(float er) {
		extrusion_range = er;
	}
	
	public float getExtrusionRange() {
		return extrusion_range;
	}
	
	public void setSeverID(int s) {
		server_id = s;
	}
	
	public int getSeverID() {
		return server_id;
	}
	
	public void setNickName(string nn) {
		nick_name = nn;
	}
	
	public string getNickName() {
		return nick_name;
	}

    public void setCareer(CHARACTER_CAREER c)
    {
		career = c;
	}

    public override CHARACTER_CAREER getCareer()
    {
		return career;
	}
	
	public void setSex(int s) {
		sex = s;
	}
	
	public int getSex() {
		return sex;
	}
	
	public void setWeapon(int w) {
		weapon = w;
	}
	
	public override int getWeapon() {
		return weapon;
	}
	
	public void setArmor(int a) {
		armor = a;
	}
	
	public int getArmor() {
		return armor;
	}
	
	public void setPropertyIceDef(int pi) {
		property_ice_def = pi;
	}
	
	public int getPropertyIceDef() {
		return property_ice_def;
	}
	
	public void setPropertyFireDef(int pf) {
		property_fire_def = pf;
	}
	
	public int getPropertyFireDef() {
		return property_fire_def;
	}
	
	public void setPropertyPoisonDef(int pp) {
		property_poison_def = pp;
	}
	
	public int getPropertyPoisonDef() {
		return property_poison_def;
	}
	
	public void setPropertyElecDef(int pe) {
		property_elec_def = pe;
	}
	
	public int getPropertyElecDef() {
		return property_elec_def;
	}
	
	public void setServerSceneID(int id) {
		server_scene_id = id;
	}
	
	public int getServerSceneID() {
		return server_scene_id;
	}
	
	public void setServerMapID(int id) {
		server_map_id = id;
	}
	
	public int getServerMapID() {
		return server_map_id;
	}
	
	public void setClientMapID(int id) {
		client_map_id = id;
	}
	
	public int getClientMapID() {
		return client_map_id;
	}

    public void setCurHPVessel(UInt32 value)
    {
        cur_hp_vessel = value;

        EventDispatcher.GetInstance().OnPlayerProperty();
    }

    public UInt32 getCurHPVessel()
    {
        return cur_hp_vessel;
    }

    public void setCurMPVessel(UInt32 value)
    {
        cur_mp_vessel = value;

        EventDispatcher.GetInstance().OnPlayerProperty();
    }

    public UInt32 getCurMPVessel()
    {
        return cur_mp_vessel;
    }

    public void setMaxHPVessel(UInt32 value)
    {
        max_hp_vessel = value;
    }

    public UInt32 getMaxHPVessel()
    {
        return max_hp_vessel;
    }

    public void setMaxMPVessel(UInt32 value)
    {
        max_mp_vessel = value;
    }

    public UInt32 getMaxMPVessel()
    {
        return max_mp_vessel;
    }
	
	public void setEnterCityPos(Vector3 pos) {
		enter_city_pos = pos;
	}
	
	public Vector3 getEnterCityPos() {
		return enter_city_pos;
	}
	
	public void setHostComputer(bool h) {
		host_computer = h;
	}
	
	public bool getHostComputer() {
		return host_computer;
	}
    
    //生命上限/10+攻击+防御+命中*3+闪避*3+暴击*2+韧性*2+必杀*3+守护*3+招架*2+破击*2+冰伤害*3+冰抗性*3+冰免疫*3+火伤害*3+火抗性*3+火免疫*3+毒伤害*3+毒抗性*3+毒免疫*3+雷伤害*3+雷抗性*3+雷免疫*3
    public int getFightPower()
    {
        //float minzhong = fightProperty.GetValue(eFighintPropertyCate.eFPC_Precise);
        //float shanbi = fightProperty.GetValue(eFighintPropertyCate.eFPC_Dodge);
        //float baoji = fightProperty.GetValue(eFighintPropertyCate.eFPC_BlastAttack);
        //float renxing = fightProperty.GetValue(eFighintPropertyCate.eFPC_Tenacity);
        //float bisha = fightProperty.GetValue(eFighintPropertyCate.eFPC_BlastAttackAdd);
        //float shouhu = fightProperty.GetValue(eFighintPropertyCate.eFPC_BlastAttackReduce);
        //float poji = fightProperty.GetValue(eFighintPropertyCate.eFPC_FightBreak);
        //float zhaojia = fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiFightBreak);
        //float iceAttack = fightProperty.GetValue(eFighintPropertyCate.eFPC_IceAttack);
        //float antiIceAttack = fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiIceAttack);
        //float iceImmunity = fightProperty.GetValue(eFighintPropertyCate.eFPC_IceImmunity);
        //float fireAttack = fightProperty.GetValue(eFighintPropertyCate.eFPC_FireAttack);
        //float antiFireAttack = fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiFireAttack);
        //float fireImmunity = fightProperty.GetValue(eFighintPropertyCate.eFPC_FireImmunity);
        //float poisonAttack = fightProperty.GetValue(eFighintPropertyCate.eFPC_PoisonAttack);
        //float antiPoisonAttack = fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiPoisonAttack);
        //float poisonImmunity = fightProperty.GetValue(eFighintPropertyCate.eFPC_PoisonImmunity);
        //float thunderAttack = fightProperty.GetValue(eFighintPropertyCate.eFPC_ThunderAttack);
        //float antiThunderAttack = fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiThunderAttack);
        //float thunderImmunity = fightProperty.GetValue(eFighintPropertyCate.eFPC_ThunderImmunity);
        //int power =(int)(hp_max/10 + attack_power + defence + minzhong*3 + shanbi*3 + baoji*2 + renxing*2 + bisha*3 + shouhu*3 +zhaojia*2 + poji*2 + 
        //          (iceAttack + antiIceAttack +iceImmunity + fireAttack + antiFireAttack + fireImmunity + poisonAttack + antiPoisonAttack + poisonImmunity + 
        //                  thunderAttack + antiThunderAttack + thunderImmunity)*3);
        //return power;

        return m_nFightPower;
    }
	
    public bool UseHP()
    {
        bool canUse = false;

        int needHP = hp_max - hp;
        if (needHP > 0)
        {
            if (needHP < cur_hp_vessel)
            {
                hp += needHP;
                cur_hp_vessel -= (uint)needHP;

                NetBase.GetInstance().Send((new GCReportHPVessel((uint)cur_hp_vessel)).ToBytes());
                canUse = true;
            }
            else if (cur_hp_vessel > 0)            
            {
                hp += (int)cur_hp_vessel;
                cur_hp_vessel = 0;

                NetBase.GetInstance().Send((new GCReportHPVessel(cur_hp_vessel)).ToBytes());
                canUse = true;
            }
			else if (cur_hp_vessel == 0)
			{
				FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_blood_rest_not_enough"), true, UIManager.Instance.getRootTrans());
			}
        }
		else
		{
			FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_blood_full"), true, UIManager.Instance.getRootTrans());
		}
        if (canUse)
        {
            EventDispatcher.GetInstance().OnPlayerProperty();
        }
        return canUse;
    }

    public bool UseMP()
    {
        bool canUse = false;

        int needMP = mp_max - mp;
        if (needMP > 0)
        {
            if (needMP < cur_mp_vessel)
            {
                mp += needMP;
                cur_mp_vessel -= (uint)needMP;

                NetBase.GetInstance().Send((new GCReportMPVessel((uint)cur_mp_vessel)).ToBytes());
                canUse = true;
            }
            else if (cur_mp_vessel > 0)
            {
                mp += (int)cur_mp_vessel;
                cur_mp_vessel = 0;

                NetBase.GetInstance().Send((new GCReportMPVessel((uint)cur_mp_vessel)).ToBytes());
                canUse = true;
            }
			else if (cur_mp_vessel == 0)
			{
				FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_magic_rest_not_enough"), true, UIManager.Instance.getRootTrans());
			}
        }
		else
		{
			FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_magic_full"), true, UIManager.Instance.getRootTrans());
		}
        if (canUse)
        {
            EventDispatcher.GetInstance().OnPlayerProperty();
        }
        return canUse;
    }

    public override string GetName()
    {
        return getNickName();
    }

    public override int GetInstanceID()
    {
        return getSeverID();
    }
}

public class CharacterAsset
{
    public int diamond;
    public int gold;

    private int crystal;
    
    /// <summary>
    /// 水晶
    /// </summary>
    public int Crystal
    {
        get { return crystal; }
        set { crystal = value; }
    }
	private int honor;
	public int Honor
	{
		get { return honor; }
		set { honor = value; }
	}
    public void SetDiamond(int value)
    {
        diamond = value;
    }

    public void SetGold(int value)
    {
        gold = value;
		EventDispatcher.GetInstance().OnPlayerAsset();
    }
}
