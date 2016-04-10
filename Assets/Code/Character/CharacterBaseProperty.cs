using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using manager;
using model;

public class CharacterBaseProperty
{
    protected int hp;
    public float move_speed;
    public int hp_max;
    public float attack_range;
    public int defence;
    public int attack_power;

    public Character property_owner;


    public int level;

    public CharacterFightProperty fightProperty = new CharacterFightProperty();


    public int m_nFightPower;
    
    // 伤害致死原因
    public DAMAGE_TYPE m_eDamageReason = DAMAGE_TYPE.DT_COMMON_DAMAGE;

    public virtual void setHP(int h)
    {
        hp = h;
        if (hp < 0)
            hp = 0;

        EventDispatcher.GetInstance().OnPlayerProperty();
    }

    public virtual int getHP()
    {
        return hp;
    }

    public virtual float GetHpRatio()
    {
        if (hp == hp_max)
        {
            return 0.9999f;
        }
        else
            return (float)hp / (float)hp_max;
    }

    public virtual float GetMpRatio()
    {
        return 0.0f;
    }

    public virtual void SetMP(int m)
    {

    }

    public virtual int GetMP()
    {
        return 0;
    }

    public virtual void setHPMax(int h)
    {
        hp_max = h;
    }

    public virtual int getHPMax()
    {
        return hp_max;
    }

    public virtual void setAttackPower(int ap)
    {
        attack_power = ap;
    }

    public virtual int getAttackPower()
    {
        return attack_power;
    }

    public virtual void setAttackRange(float ar)
    {
        attack_range = ar;
    }

    public virtual float getAttackRange()
    {
        return attack_range;
    }

    public virtual void setDefence(int def)
    {
        defence = def;
    }

    public virtual int getDefence()
    {
        return defence;
    }

    public virtual void setMoveSpeed(float ms)
    {
        move_speed = ms;
    }

    public virtual float getMoveSpeed()
    {
        //return move_speed;
        return move_speed * fightProperty.GetBuffSpeed();
    }

    public virtual void setLevel(int l)
    {
        if (level>0 && l > level) //如果比原来等级高，那么播放升级特效
        {
            CharacterPlayer.sPlayerMe.OnUpgrade();
        }
        level = l;
        TaskManager.Instance.deleteLateralTasks(level);

        
    }

    public virtual int getLevel()
    {
        return level;
    }

    // 设置属性的拥有者
    public void SetPropertyOwner(Character character)
    {
        property_owner = character;
    }

    // 属性值变化
    //public virtual void ChangeFightPropertyValue(eFighintPropertyCate eType, int value)
    //{
    //    fightProperty.SetValue(eType, value);
    //}

    public virtual void GetFightPropertyValue(eFighintPropertyCate eType)
    {
        //if (fightProperty.)
        //{
        //}
    }

  
    public virtual CHARACTER_CAREER getCareer()
    {
        return CHARACTER_CAREER.CC_SWORD;
	}

    public virtual string GetName()
    {
        return null;
    }

    public virtual int GetInstanceID()
    {
        return 0;
    }

    public virtual int getWeapon()
    {
        return 0;
    }

    public virtual bool MPIsEnough()
    {
        return GetMP() == fightProperty.GetBaseValue(eFighintPropertyCate.eFPC_MaxMP);
    }

    public virtual bool HPIsEnough()
    {
        return getHP() == fightProperty.GetBaseValue(eFighintPropertyCate.eFPC_MaxHP);
    }

    public virtual bool HPMPIsEnough()
    {
        return HPIsEnough() && MPIsEnough();


    }

    //public virtual void FillHPMPFull()
    //{
    //    setHP(getHPMax());
    //    SetMP(fightProperty.GetBaseValue(eFighintPropertyCate.eFPC_MaxHP));
    //}
}
