using UnityEngine;
using System.Collections;

public class MonsterProperty : CharacterBaseProperty
{
    // 攻击效果分类
    public enum MONSTER_LEVEL_TYPE
    {
        MLT_COMMON = 0,
        MLT_ELITE,
        MLT_BOSS
    }

    // 上色分类
    public enum MONSTER_SURFACE_LIGHT
    {
        MSL_NONE = 0,
        MSL_ELITE,
        MSL_BOSS,
    }

	public int instance_id;
	public int template_id;
	public string name;
    public string strDesName;
	//public int level;
    public MONSTER_LEVEL_TYPE type; //0,1,2 小怪 精英 boss
    public MONSTER_SURFACE_LIGHT surface_type; // 0, 1，2 不上色 上精英色 上boss色
    //public int hp;
    //public int hp_max;
    //public int attack_power;
    //public float attack_range;
    //public int defence;
    //public float move_speed;
    public float attack_interval_upper;
	public float attack_interval_low;
	public int attack_type;
	public float repel_speed;
	public float fly_speed;
	public int broken;
	public string broken_prefab;
	public int exp;
	public int gold;
	public string pszDisplayName;
	public string pszDisplayIcon;
    //public CharacterFightProperty fightProperty = new CharacterFightProperty();
	
	public void setInstanceId(int id) {
		instance_id = id;
	}
	
	public int getInstanceId() {
		return instance_id;
	}
	
	public void setTemplateId(int id) {
		template_id = id;
	}
	
	public int getTemplateId() {
		return template_id;
	}
	
	public void setName(string n) {
		name = n;
	}
	
	public string getName() {
		return name;
	}
	
    //public void setLevel(int l) {
    //    level = l;
    //}
	
    //public int getLevel() {
    //    return level;	
    //}

    public void setType(MONSTER_LEVEL_TYPE t)
    {
		type = t;
	}

    public MONSTER_LEVEL_TYPE getType()
    {
		return type;	
	}

    public void SetSurfaceType(MONSTER_SURFACE_LIGHT t)
    {
        surface_type = t;
    }

    public MONSTER_SURFACE_LIGHT GetSurfaceType()
    {
        return surface_type;
    }
	
    // 移动基类中

    // 怪的血有变化 Ui也要更这边
    public override void setHP(int h)
    {
        hp = h;
        if (hp < 0)
            hp = 0;

        // UI change
        if (getType() == MONSTER_LEVEL_TYPE.MLT_BOSS)
        {
            BattleManager.Instance.setBossMonsterHPBar((CharacterMonster)property_owner);
        }
        else
        {
            //BattleManager.Instance.addHPBarOnMonster((CharacterMonster)property_owner);

            property_owner.GetComponent<HUD>().ChangeHPShow();
        }
    }
	
    //public int getHP() {
    //    return hp;	
    //}
	
    //public void setHPMax(int h) {
    //    hp_max = h;	
    //}
	
    //public int getHPMax() {
    //    return hp_max;	
    //}
	
    //public void setAttackPower(int ap) {
    //    attack_power = ap;
    //}
	
    //public int getAttackPower() {
    //    return attack_power;
    //}
	
    //public void setAttackRange(float ar) {
    //    attack_range = ar;
    //}
	
    //public float getAttackRange() {
    //    return attack_range;
    //}
	
    //public void setDefence(int def) {
    //    defence = def;
    //}
	
    //public int getDefence() {
    //    return defence;
    //}
	
    //public void setMoveSpeed(float ms) {
    //    move_speed = ms;
    //}
	
    //public float getMoveSpeed() {
    //    return move_speed;
    //}
	
	public void setAttackType(int at) {
		attack_type = at;
	}
	
	public int getAttackType() {
		return attack_type;
	}
	
	public void setRepelSpeed(float s) {
		repel_speed = s;
	}
	
	public float getRepelSpeed() {
		return repel_speed;
	}
	
	public void setBroken(int b) {
		broken = b;
	}
	
	public int getBroken() {
		return broken;
	}
	
	public void setExp(int e) {
		exp = e;
	}
	
	public int getExp() {
		return exp;
	}
	
	public void setGold(int g) {
		gold = g;
	}
	
	public int getGold() {
		return gold;
	}
	
	public void setBrokenPrefab(string bp) {
		broken_prefab = bp;
	}
	
	public string getBrokenPrefab() {
		return broken_prefab;
	}

    public override string GetName()
    {
        //return getName();
        return strDesName;
    }

    public override int GetInstanceID()
    {
        return getInstanceId();
    }

    public override void SetMP(int m)
    {
        
    }

    // 怪是没有蓝的 这里添加这个函数用来通用
    public override int GetMP()
    {
        // 这里永远都可以放
        return -1;
    }
}
