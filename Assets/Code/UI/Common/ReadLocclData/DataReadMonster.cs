using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterDataItem {
	public int id;
	public string name = "";
    public string desName = "";
	public int level;
    public MonsterProperty.MONSTER_SURFACE_LIGHT surfaceLight;
	public int type;
	public int hp;
	public int attack_power;
	public float attack_range;
	public int defence;
	public float move_speed;
	
	public float attack_interval_upper;
    public float attack_interval_low;
	public int attack_type;
	public float repel_speed;
	public float fly_speed;
	public int broken;
	public string broken_prefab;
	public int exp;
	public int gold;
	
	public int FPC_Precise;
	public int FPC_Dodge;
	public int FPC_BlastAttack;
	public int FPC_BlastAttackAdd;
	public int FPC_BlastAttackReduce;
	public int FPC_Tenacity;
	public int FPC_FightBreak;
	public int FPC_AntiFightBreak;
	public int FPC_IceAttack;
	public int FPC_AntiIceAttack;
	public int FPC_FireAttack;
	public int FPC_AntiFireAttack;
	public int FPC_PoisonAttack;
	public int FPC_AntiPoisonAttack;
	public int FPC_ThunderAttack;
	public int FPC_AntiThunderAttack;
	public string pszDisplayName="";
	public string pszDisplayIcon="";

    public bool isTeXie;

    public bool isShouJi;
	
	public string ai;

    public bool swoon;
}

public class PreLoadMonsterData
{
    public int nTmpID;
    public string strModelName;
    public string strBrokenName;
}

public class DataReadMonster : DataReadBase {
	
	public override string getRootNodeName() {
		return "MonsterConfig";
	}
	
	public override void appendAttribute(int key, string name, string value) {
		
		MonsterDataItem mdi;
		
		if (!data.ContainsKey(key))  
        {  
            mdi = new MonsterDataItem();
			data.Add(key, mdi);
        }
		
		mdi = (MonsterDataItem)data[key];
		
		switch (name) {
		case "ID":
			mdi.id = int.Parse(value);
			break;
		case "name":
			mdi.name = value;
			break;
        case "mon_name":
            mdi.desName = value;
            break;
        case "level":
			mdi.level = int.Parse(value);
			break;
        case "surfaceLight":
            mdi.surfaceLight = (MonsterProperty.MONSTER_SURFACE_LIGHT)int.Parse(value);
            break;
		case "monsterType":
			mdi.type = int.Parse(value);
			break;
		case "hp":
			mdi.hp = int.Parse(value);
			break;
		case "attack_power":
			mdi.attack_power = int.Parse(value);
			break;
		case "attack_range":
			mdi.attack_range = float.Parse(value);
			break;
		case "defence":
			mdi.defence = int.Parse(value);
			break;
		case "move_speed":
			mdi.move_speed = float.Parse(value);
			break;
        case "attack_interval":
            {
                string[] intervals = value.Split(',');

                if (intervals.Length == 2)
                {
                    mdi.attack_interval_low = float.Parse(intervals[0]);
                    mdi.attack_interval_upper = float.Parse(intervals[1]);
                }
            }
            break;
		case "attack_type":
			mdi.attack_type = int.Parse(value);
			break;
		case "repelSpeed":
			mdi.repel_speed = float.Parse(value);
			break;
		case "broken":
			mdi.broken = int.Parse(value);
			break;
		case "brokenPrefab":
			mdi.broken_prefab = value;
			break;
		case "dropExp":
			mdi.exp = int.Parse(value);
			break;
		case "dropSiliver":
			mdi.gold = int.Parse(value);
			break;
		case "eFPC_Precise":
			mdi.FPC_Precise = int.Parse(value);
			break;
		case "eFPC_Dodge":
			mdi.FPC_Dodge = int.Parse(value);
			break;
		case "eFPC_BlastAttack":
			mdi.FPC_BlastAttack = int.Parse(value);
			break;
		case "eFPC_BlastAttackAdd":
			mdi.FPC_BlastAttackAdd = int.Parse(value);
			break;
		case "eFPC_BlastAttackReduce":
			mdi.FPC_BlastAttackReduce = int.Parse(value);
			break;
		case "eFPC_Tenacity":
			mdi.FPC_Tenacity = int.Parse(value);
			break;
		case "eFPC_FightBreak":
			mdi.FPC_FightBreak = int.Parse(value);
			break;
		case "eFPC_AntiFightBreak":
			mdi.FPC_AntiFightBreak = int.Parse(value);
			break;
		case "eFPC_IceAttack":
			mdi.FPC_IceAttack = int.Parse(value);
			break;
		case "eFPC_AntiIceAttack":
			mdi.FPC_AntiIceAttack = int.Parse(value);
			break;
		case "eFPC_FireAttack":
			mdi.FPC_FireAttack = int.Parse(value);
			break;
		case "eFPC_AntiFireAttack":
			mdi.FPC_AntiFireAttack = int.Parse(value);
			break;
		case "eFPC_PoisonAttack":
			mdi.FPC_PoisonAttack = int.Parse(value);
			break;
		case "eFPC_AntiPoisonAttack":
			mdi.FPC_AntiPoisonAttack = int.Parse(value);
			break;
		case "eFPC_ThunderZAttack":
			mdi.FPC_ThunderAttack = int.Parse(value);
			break;
		case "eFPC_AntiThunderZAttack":
			mdi.FPC_AntiThunderAttack = int.Parse(value);
			break;
		case "AI":
			mdi.ai = value;
			break;
//		case "UIname":
//			mdi.pszDisplayIcon = value;
//			break;
		case "mon_image":
			mdi.pszDisplayIcon = value;
            break;
        case "isTeXie":
            mdi.isTeXie = (int.Parse(value) == 0 ? false : true);
			break;
        case "isShouJi":
            mdi.isShouJi = (int.Parse(value) == 0 ? false : true);
			break;
        case "swoon":
            mdi.swoon = (int.Parse(value) == 0 ? false : true);
            break;
		}
	}
	
	public MonsterDataItem getMonsterData(int key) {
		
		if (!data.ContainsKey(key))  
        {  
            MonsterDataItem mdi = new MonsterDataItem();
			return mdi;
        }
		
		return (MonsterDataItem)data[key];
	}

    public List<PreLoadMonsterData> GetMonsterModels()
    {
        List<PreLoadMonsterData> ret = new List<PreLoadMonsterData>();

        foreach (DictionaryEntry item in data)
        {
            PreLoadMonsterData subData = new PreLoadMonsterData();

            MonsterDataItem itemdata = item.Value as MonsterDataItem;
            subData.nTmpID = itemdata.id;
            subData.strModelName = itemdata.name;
            subData.strBrokenName = itemdata.broken_prefab;
            ret.Add(subData);
        }

        return ret;
    }

}