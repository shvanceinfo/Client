using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SKILL_DAMAGE_TYPE
{
    SDT_NONE = 0,
    SDT_ONCE,
    SDT_MULTI,
}

public class SKILL_PROPERTY
{
    //public int costMp;
    //public int costHp;
    //public int damageHp; 
    //public int damageMp;

    //public float skill_life;
    //public float skill_move_speed;
    //public float attack_coefficient;
    //public float damage_plus;
    //public int attack_repel;
    //public int attack_fly;
    //public int attack_swoon;
    //public int mp_cost;
    //public float repel_scale;
    //public float range;
    //public int cool_down;
    //public int add_value;
    //public float last_tick;
}

public struct SkillBaseInfo
{
    public int m_nSkillId;
    public int m_nSkillLevel;
}


// 分组技能类型
public enum SKILL_GROUP_TYPE
{
    WEAPON_SKILL = 0,
    CHARACTER_SKILL
}

// 技能基类
// 技能的时间就要靠动作的时间来确定了
public class SkillBase : MonoBehaviour
{
    public int m_nSkillId { get; set; }                // 技能id
    private int m_bInCD { get; set; }                   // 是否在冷却中
    private int m_nbInUse { get; set; }                 // 是否在使用中 （有些技能是持续技能）
    private int m_nSkillLevel { get; set; }         // 技能级别

    public Character m_kCaster;


    protected SKILL_GROUP_TYPE m_eSkillGroupType;

    private List<Appear> m_kSkillAppear;

    private BaseAnimatorState m_kCastAnimator;
    private BaseAnimatorState m_kBeHitAnimator;




    public SkillBase(int nId)
    {
        m_nSkillId = nId;
        // 装载技能相关的资源
        m_kCastAnimator = new BaseAnimatorState();
        m_kBeHitAnimator = new BaseAnimatorState();     
    }

    public void SetCaster(Character caster)
    {
        m_kCaster = caster;
    }

    // 技能动画
    virtual public void PlayCastAnimator(BaseAnimatorState curState)
    {
        //m_kCastAnimator.Play();

    }

    //virtual public bool CanUse(CharacterBattleProperty prop)
    //{
    //    return prop.m_nMp >= m_kSkillItem.mp_cost;
    //}

    virtual public void Use(Character attacker, Character target)
    {
        //target.HurtAppear(m_kSkillItem.appear_id);
    }


    virtual public void CreateSkillArea()
    {
        //MissleManager.Instance.createSkillAreaMissle("Model/prefab/TestChongjibo",
        //    m_kCaster.m_kProp.m_vecPosition,
        //    m_kCaster.m_kProp.m_vecFaceDir,
            
        //    7.5f, 
        //    skillCmd, 
        //    0.5f, 
        //    0);


    }
}