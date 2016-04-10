using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 技能容器类
public abstract class Skill : MonoBehaviour 
{
	protected SkillAppear cur_skill;
    protected Character skill_target = null;


    protected bool hurt_protecting = false;
    protected bool hurt_hide = false;

    // 技能的拥有者
    public Character m_kSkillOwner;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}


    abstract public SkillAppear GetCommonAttack();

    abstract public bool IsSkillCommonActive();


    public virtual SkillAppear GetCommon1() { return null; }
    public virtual SkillAppear GetCommon2() { return null; }
    public virtual SkillAppear GetCommon3() { return null; }
    public virtual SkillAppear GetCommon4() { return null; }
    public virtual SkillAppear GetCommon5() { return null; }
    public virtual SkillAppear GetCommon6() { return null; }
    public virtual SkillAppear GetCommon7() { return null; }
    public virtual SkillAppear GetCommon8() { return null; }
    public virtual SkillAppear GetCommon9() { return null; }
    public virtual SkillAppear GetCommon10() { return null; }
    public virtual SkillAppear GetCommon11() { return null; }
    public virtual SkillAppear GetCommon12() { return null; }



    public void SetSkillOwner(Character character)
    {
        m_kSkillOwner = character;
    }
	
	public void setCurrentSkill(SkillAppear skill) 
    {

		cur_skill = skill;

        if (skill == null)
        {
            return;
        }

        if (GetComponent<Character>().getType() == CharacterType.CT_PLAYER)
        {
            MessageManager.Instance.sendUseSkill(
                (uint)CharacterPlayer.character_property.getSeverID(),
                (uint)cur_skill.GetSkillId(),
                CharacterPlayer.sPlayerMe.getPosition(),
                CharacterPlayer.sPlayerMe.getFaceDir());
        }
        else if (GetComponent<Character>().getType() == CharacterType.CT_MONSTER &&
                CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap())
        {
            CharacterMonster theMonster = GetComponent<CharacterMonster>();

            Vector3 skillFace = theMonster.getFaceDir();

            if (theMonster.skill.getSkillTarget() != null)
            {
                skillFace = theMonster.skill.getSkillTarget().transform.position - theMonster.transform.position;
                skillFace.Normalize();
            }
            
            MessageManager.Instance.sendUseSkill(
                (uint)theMonster.monster_property.getInstanceId(),
                (uint)cur_skill.GetSkillId(),
                theMonster.getPosition(),
                skillFace);
        }
	}
	
	public SkillAppear getCurrentSkill() 
    {
		return cur_skill;
	}

    // 判断是否是普通攻击 
    public virtual bool IsCurSkillCommon()
    {  
        if (cur_skill != null)
        {
            return IsCurSkillCommon(cur_skill.skill_id);
        }

        return false;
    }

    public static bool IsCurSkillCommon(int skillId)
    {
        int ret = skillId / 100000000;
        if (ret == 1)
        {
            return true;
        }

        return false;
    }
	
	public void setHurtProtecting(bool b) {
		hurt_protecting = b;
	}
	
	public bool getHurtProtecting() {
		return hurt_protecting;
	}
	
	public void setHurtHide(bool b) {
		hurt_hide = b;
	}
	
	public bool getHurtHide() {
		return hurt_hide;
	}


	
	public void setSkillTarget(Character c) 
    {
		skill_target = c;

        if (skill_target == null)
        {
            return;
        }

        if (skill_target.getType() == CharacterType.CT_MONSTER)
        {
            BattleManager.Instance.Skill_Target_Flag.target = c;
            BattleManager.Instance.Skill_Target_Flag.playAnimation();
        }
	}
	
	public Character getSkillTarget() {
		return skill_target;
	}


    //////////////////////////////////////////////////////////////////////////
    // leidafu 创建通用技能
    public SkillAppear CreateSkill(int skillId)
    {
        SkillAppear skill = new SkillAppear(skillId);
        return InitSkill(skill);
    }

    public SkillAppear InitSkill(SkillAppear skill)
    {
        skill.setOwner(GetComponent<Character>());
        skill.init();

        if (m_kSkillOwner != null)
        {
            // 多人副本的副机不需要cd检测
            m_kSkillOwner.GetComponent<CoolDownProperty>().AddCDObj(skill.skill_id);
            //if (!Global.inMultiFightMap() || CharacterPlayer.character_property.getHostComputer() || m_kSkillOwner.getType() != CharacterType.CT_MONSTER)
            //{
                
            //}
        }
        
        //setCurrentSkill(skill);
        return skill;
    }
}
