using UnityEngine;
using System.Collections;

public enum SKILL_APPEAR
{
    SA_ROLL = 0, // 战士 弓箭手翻滚
    SA_MAG_FLASH_AWAY,      // 法师翻滚
    SA_MAG_FLASH_BACK,          // 法师后闪 技能中间一段
    SA_WHIRL_WIND,
    SA_FIRE_RAIN,
    //SA_ENERGE_WAVE,
    SA_FLASH_AWAY,      // 哥布林闪现
}

public class SkillAppear : Appear 
{
    public int skill_id;

    public SkillExpressionDataItem m_kSkillInfo;
    public SkillDataItem m_kData;

    protected float m_fUpdateTime = 0.0f;

    public float m_fSkillCD = 0.0f;

    // 持续伤害技能第一次的伤害置位判断
    public bool m_bDurationalSkillFirstClearFlag = true;

    // 持续伤害技能第一次的碰撞盒判断
    public bool m_bDurationalSkillFirstColliderFlag = true;


    // 第三方看自己普通攻击是否碰到敌人
    public int m_nColliderStopValue = 1;

    public bool m_bSkillIntrupted = false;


	public bool m_bSkillTriggedJiGuan = false;

    public SkillAppear(int skillid)
    {
        battle_state = Appear.BATTLE_STATE.BS_SKILL;
        skill_id = skillid;
    }

    public virtual void init()
    {
        if (skill_id != 0)
        {
            loadConfig();
        }
    }
	
	public virtual int GetSkillId() 
    {
		return skill_id;
	}
	
	public float getAttackRepel() 
    {
		return get_repel_inner();
	}
	
	public float getAttackFly() 
    {
		return get_fly_inner();
	}

	
	public int getMPCost() 
    {
        return m_kData.mp_cost;
	}

	
	public int getCoolDown() 
    {
        return m_kData.cool_down;
	}

    public float GetSkillLife()
    {
        if (m_kData.lastTick != 0)
        {
            return m_kData.lastTick ;
        }

        return time_length;
    }

    public float GetLeftTime()
    {
        return GetSkillLife() - m_fUpdateTime;
    }
	
    //public float getLife() {
    //    return m_kData.life;
    //}
	
	public float getLastTick() {
        return m_kData.lastTick;
	}
	
    //public int getAddValue() {
    //    return m_kData.addStateValue;
    //}
	
	public virtual float GetAttackCoefficient() 
    {
        return m_kData.attack_coefficient;
	}

    public virtual float GetDamagePlus()
    {
        return m_kData.damage_plus;
    }


    public static int ConvertLevelIDToSingleID(int levelID)
    {
        return levelID / 1000;
    }

	public void loadConfig() 
    {
       m_kData  = ConfigDataManager.GetInstance().getSkillConfig().getSkillData(skill_id);
       m_kSkillInfo = ConfigDataManager.GetInstance().getSkillExpressionConfig().getSkillExpressionData(ConvertLevelIDToSingleID(skill_id));
	}

    // 技能的攻击范围
    public float GetSkillRange()
    {
        return m_kData.attack_range;
    }

    // 技能是否锁定敌人
    public bool LockEnemy()
    {
        return m_kData.suo_target == 1;
    }

    // 技能是否可以平放
    public bool NoTargetCast()
    {
        return m_kData.no_target == 1;
    }

    public override void active()
    {
        // 单位时间内完成几个技能 (每秒可以施放的技能)
        animation_name = m_kSkillInfo.skillAction;
        //Debug.Log("SSSKKKIIILLL " + animation_name + " " + m_kSkillInfo.id);
        //owner.animation[animation_name].speed = 1.0f;

        m_fSkillCD = m_kData.cool_down;

        // 循环动画 
        if (m_kSkillInfo.SpecialEffectLoop != 0)
        {
            time_length = m_kData.lastTick;
        }
        else
            time_length = owner.animation[animation_name].length;


        PlayerSpecialEffect();

        on_active(time_length);

        owner.skill.setCurrentSkill(this);

        m_fUpdateTime = 0.0f;
        m_nColliderStopValue = 1;

		m_bSkillTriggedJiGuan = false;

        // 技能如果带有机关释放时触发类物件 需要在这里触发
        BattleJiGuan.Instance.OnJiGuanTrigger(JiGuanItem.EJiGuanType.JGT_SKILL_BEGIN, m_kSkillInfo.nJiGuanID, owner.transform.position, this);
        
        if (!Global.inMultiFightMap() || CharacterPlayer.character_property.getHostComputer() || owner.getType() != CharacterType.CT_MONSTER)
        {
            // 放技能之前把角色朝向指向性物件
            if (owner != null && owner.skill != null && owner.skill.getSkillTarget() != null && battle_state == BATTLE_STATE.BS_SKILL)
            {
                if (m_kData.qishou)
                {
                    Vector3 dir = owner.skill.getSkillTarget().transform.position - owner.transform.position;
                    dir.Normalize();
                    owner.setFaceDir(dir);

                    return;
                }
            }

            if (owner != null && owner.skill != null && owner.skill.getSkillTarget() != null && battle_state == BATTLE_STATE.BS_PING_KAN)
            {
                Vector3 dir = owner.skill.getSkillTarget().transform.position - owner.transform.position;
                dir.Normalize();
                owner.setFaceDir(dir);

                return;
            }
        }
    }

	protected override bool IsLoopAnimation()
    {
        return m_kSkillInfo.SpecialEffectLoop != 0;
    }

    protected override void on_deActive()
    {
        base.on_deActive();

        owner.skill.setCurrentSkill(null);  

        // 添加技能的buff

        // 每次清空第三方的旋风斩位移
        if (owner.getType() == CharacterType.CT_PLAYEROTHER)
        {
            CharacterPlayerOther other = owner as CharacterPlayerOther;

            other.m_kXuanFengZhanPos.Clear();
            other.m_kCommonAttackPos.Clear();
        }
    }

    public override void update(float delta)
    {
        m_fUpdateTime += delta;

        if (m_fUpdateTime > time_length)
        {
            if (m_kData.nextSkill != 0)
            {
                if (Global.inMultiFightMap() && !CharacterPlayer.character_property.getHostComputer() && owner.getType() == CharacterType.CT_MONSTER)
                {
                    owner.ChangeAppear(owner.skill.CreateSkill(m_kData.nextSkill));
                }
                else
                    owner.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_SKILL, m_kData.nextSkill);
            }
            else
            {
                GraphicsUtil.ResetData();
            }
        }
        else if (m_bSkillIntrupted)
        {
            if (m_kData.nextSkill != 0)
            {
                owner.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_SKILL, m_kData.nextSkill);
            }
            else
            {
                GraphicsUtil.ResetData();
            }

            m_bSkillIntrupted = false;
        }

        // 动画同时 人物的位移，朝向 等变化
        UpdateTranAndOrien(updateTime(delta));
    }

    
    public virtual void UpdateTranAndOrien(float updateTime)
    {
		// check skill move collision with enemy then stop it
		if ((owner.skill.getCurrentSkill().skill_id / 1000) == 201001)
		{
			// chongzhuang check dist with enemy
			if (Global.InArena())
			{
				Character target = owner.skill.getSkillTarget();

				if (target != null)
				{
					Vector3 hitPoint = PlayerPursueState.CalculatePursuePoint(owner, target);

					float dist = Vector3.Distance(owner.transform.position, hitPoint);

					if (hitPoint != Vector3.zero && dist < 0.4f)
					{
						// collisder with target
						m_bSkillIntrupted = true;
						Debug.Log("arena collider with enemy");
						return;
					}
				}
			}
			else
			{
				float fDistance = 0.0f;
				
				Character monster = MonsterManager.Instance.GetNearestMonster(out fDistance);
				Debug.Log("dist " + fDistance);
				if (monster != null && fDistance < 1.0f)
				{
					// collisder with target
					m_bSkillIntrupted = true;
					Debug.Log("collider with monster");
					return;
				}
			}
		}

        // 旋风斩 根据摇杆而动
        if ((owner.skill.getCurrentSkill().skill_id / 1000) == 201003)
        {
            if (owner.getType() == CharacterType.CT_PLAYER)
            {
                Vector3 dir = owner.GetComponent<InputProperty>().m_kXuanFengZhanDir;

                if (dir != Vector3.zero)
                {
                    // 旋风斩有位移
                    owner.movePosition(dir * updateTime * m_kSkillInfo.moveSpeed * m_nColliderStopValue);

                    // 通过发送朝向为Vector.x = 99999 来表示是旋风斩专用
                    MessageManager.Instance.sendMessageAskMove(CharacterPlayer.character_property.getSeverID(), new Vector3(99999.0f, 0.0f, 0.0f), owner.getPosition());
                }
            }
            else if (owner.getType() == CharacterType.CT_PLAYEROTHER)
            {
                // 其他人放旋风斩 通过位移信息来同步
                CharacterPlayerOther other = owner as CharacterPlayerOther;
                if (other.m_kXuanFengZhanPos.Count != 0)
                {
                    owner.transform.position = other.m_kXuanFengZhanPos[0];
                    other.m_kXuanFengZhanPos.RemoveAt(0);
                }
            }

            return;
        }

        if (Global.inMultiFightMap() && owner.skill.IsCurSkillCommon() && owner.getType() == CharacterType.CT_PLAYEROTHER)
        {
            CharacterPlayerOther other = owner as CharacterPlayerOther;
            if (other.m_kCommonAttackPos.Count != 0)
            {
                owner.transform.position = other.m_kCommonAttackPos[0];
                other.m_kCommonAttackPos.RemoveAt(0);
            }

            return;
        }

        if (Global.InWorldBossMap())
        {
            if (owner.skill.IsCurSkillCommon())
            {
                if (owner.getType() == CharacterType.CT_PLAYEROTHER)
                {
                    CharacterPlayerOther other = owner as CharacterPlayerOther;
                    if (other.m_kCommonAttackPos.Count != 0)
                    {
                        owner.transform.position = other.m_kCommonAttackPos[0];
                        other.m_kCommonAttackPos.RemoveAt(0);
                    }

                    return;
                }
                else if (owner.getType() == CharacterType.CT_MONSTER)
                {
                    // 如果是boss不需要位移
                    return;
                }
            }
        }

        // 位移变换
        owner.movePosition(owner.getFaceDir() * updateTime * m_kSkillInfo.moveSpeed * m_nColliderStopValue);
        // 朝向变换
    }

    public virtual void PlayerSpecialEffect()
    {
        if (m_kSkillInfo.SpecialEffect != "0")
        {
            EffectManager.Instance.createFX(m_kSkillInfo.SpecialEffect, owner.transform, m_kData.lastTick);
        }
    }

    public virtual void GenerateCollider()
    {
        if (m_kSkillInfo.skillRangePre != null)
        {
            MissleManager.Instance.createSkillArea("Model/prefab/" + m_kSkillInfo.skillRangePre,
            Vector3.zero, owner.transform, this, m_kData.lastTick, 1, 10000, 0.2f);
        }
    }

    public bool InCD()
    {
        if (m_kData.cool_down != 0)
        {
            return true;
        }
        return false;
    }

    public void setDir(Vector3 dir)
    {
        owner.setFaceDir(dir);
    }

    protected virtual int get_type_inner()
    {
        return skill_id;
    }
	
	protected virtual float get_repel_inner() {
        return m_kSkillInfo.atkRepel;
	}
	
	protected virtual float get_fly_inner() {
        return m_kSkillInfo.atkFly;
	}


    // 平砍中的自动战斗AI
    //protected virtual void AIAutoFight(CharacterAI ai)
    //{
    //    CharacterMonster monster = null;
    //    MonsterManager.Instance.GetNearestMonster(out monster);
    //    if (monster)
    //    {
    //        // 有怪就打怪
    //        IdleState.AIBehaviour(ai, monster);
    //    }
    //}

    protected virtual bool CanEnterAutoAIByAttackState()
    {
        //CharacterMonster monster = null;
        //MonsterManager.Instance.GetNearestMonster(out monster);
        //if (monster)
        //{
        //    return IdleState.TryExecuteAI(owner.GetAI(), monster);
        //}

        Character character = getOwner().GetAI().GetNearestEnemy();
        if (character)
        {
            return AIUtil.TryExecuteAI(owner.GetAI(), character);
        }
        

        return false;
    }
}
