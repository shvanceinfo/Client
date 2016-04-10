using UnityEngine;
using System.Collections;

public class SkillMagicAttackCommonBase : SkillAppear 
{
    public PING_KAN m_ePingKanState;

    public SkillMagicAttackCommonBase(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_1;
    }

    public PING_KAN GetNextAttackAppear()
    {
        switch (m_ePingKanState)
        {
            case PING_KAN.PK_1:
            case PING_KAN.PK_2:
                return PING_KAN.PK_4;
            case PING_KAN.PK_4:
            case PING_KAN.PK_5:
                return PING_KAN.PK_7;
            case PING_KAN.PK_7:
            case PING_KAN.PK_8:
            case PING_KAN.PK_9:
                return PING_KAN.PK_1;
            default:
                return PING_KAN.PK_NONE;
        }
    }


    public void ChangeAppearInOrder()
    {
        switch (m_ePingKanState)
        {
            case PING_KAN.PK_1:
                owner.ChangeAppear(owner.skill.GetCommon2());
                break;
            case PING_KAN.PK_2:
                owner.ChangeAppear(owner.skill.GetCommon4());
                break;
            case PING_KAN.PK_4:
                owner.ChangeAppear(owner.skill.GetCommon5());
                break;
            case PING_KAN.PK_5:
                owner.ChangeAppear(owner.skill.GetCommon7());
                break;
            case PING_KAN.PK_7:
                owner.ChangeAppear(owner.skill.GetCommon8());
                break;
            case PING_KAN.PK_8:
                owner.ChangeAppear(owner.skill.GetCommon9());
                break;
            case PING_KAN.PK_3:
            case PING_KAN.PK_6:
            case PING_KAN.PK_9:
                owner.ChangeAppear(owner.skill.GetCommon1());
                break;
        }
    }

    public override void active()
    {
        animation_name = m_kSkillInfo.skillAction;

        time_length = owner.animation[animation_name].length;
        on_active(time_length);
        m_fUpdateTime = 0.0f;

        owner.skill.setCurrentSkill(this);

        if (owner.skill.getSkillTarget() != null)
        {
            Vector3 dir = owner.skill.getSkillTarget().transform.position - owner.transform.position;
            dir.Normalize();
            owner.setFaceDir(dir);
        }

		m_bSkillTriggedJiGuan = false;
    }

    public override void update(float delta)
    {
        if (m_fUpdateTime > time_length)
        {
			if (owner.GetComponent<FightProperty>())
			{
				if (owner.GetComponent<FightProperty>().m_bForceAttack)
	            {
	                switch (m_ePingKanState)
	                {
	                    case PING_KAN.PK_2:
	                    case PING_KAN.PK_5:
	                    case PING_KAN.PK_9:
	                        {
	                            // 由摇杆带来的朝向变换
	                            if (owner.GetComponent<InputProperty>().m_kPlayerNeedRot != Quaternion.identity)
	                            {
	                                owner.transform.rotation = owner.GetComponent<InputProperty>().m_kPlayerNeedRot;
	                            }
	                        }
	                        break;
	                }
	                //ChangeAppearInOrder();
	                //return;
	            }	
			}
            


            CharacterAI kAI = owner.GetAI() as CharacterAI;

            if (kAI.m_kPursueState.m_kPursurPoint != Vector3.zero || 
                kAI.m_kPursueState.m_kTmpSavedTarget != null)
            {
                switch (m_ePingKanState)
                {
                    case PING_KAN.PK_1:
                        owner.ChangeAppear(owner.skill.GetCommon2());
                        break;
                    case PING_KAN.PK_2:
                        owner.ChangeAppear(owner.skill.GetCommon3());
                        break;
                    case PING_KAN.PK_4:
                        owner.ChangeAppear(owner.skill.GetCommon5());
                        break;
                    case PING_KAN.PK_5:
                        owner.ChangeAppear(owner.skill.GetCommon6());
                        break;
                    case PING_KAN.PK_7:
                        owner.ChangeAppear(owner.skill.GetCommon8());
                        break;
                    case PING_KAN.PK_8:
                        owner.ChangeAppear(owner.skill.GetCommon9());
                        break;
                    case PING_KAN.PK_3:
                    case PING_KAN.PK_6:
                    case PING_KAN.PK_9:
                        {
                            kAI.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE);
                        }
                        break;
                }

                return;
            }

			if (owner.GetComponent<InputProperty>())
			{
				if (owner.GetComponent<InputProperty>().m_bNeedLeaveFight)
            {
                switch (m_ePingKanState)
                {
                    case PING_KAN.PK_1:
                        owner.ChangeAppear(owner.skill.GetCommon2());
                        break;
                    case PING_KAN.PK_2:
                        owner.ChangeAppear(owner.skill.GetCommon3());
                        break;
                    case PING_KAN.PK_4:
                        owner.ChangeAppear(owner.skill.GetCommon5());
                        break;
                    case PING_KAN.PK_5:
                        owner.ChangeAppear(owner.skill.GetCommon6());
                        break;
                    case PING_KAN.PK_7:
                        owner.ChangeAppear(owner.skill.GetCommon8());
                        break;
                    case PING_KAN.PK_8:
                        owner.ChangeAppear(owner.skill.GetCommon9());
                        break;
                    case PING_KAN.PK_3:
                    case PING_KAN.PK_6:
                    case PING_KAN.PK_9:
                        {
                            kAI.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
                            // 状态重置
                            owner.GetComponent<InputProperty>().m_bNeedLeaveFight = false;
                            owner.GetComponent<FightProperty>().m_bEnterFight = false;
                        }
                        break;
                }

                return;
            }
			}


            Character kTarget = BattleManager.Instance.GetViewRangeEnemy(owner);
            if (!kTarget)
            {
                // 有按键消息 可以平放
                // 需要收尾动作
                switch (m_ePingKanState)
                {
                    case PING_KAN.PK_1:
                        owner.ChangeAppear(owner.skill.GetCommon2());
                        break;
                    case PING_KAN.PK_2:
                        owner.ChangeAppear(owner.skill.GetCommon3());
                        break;
                    case PING_KAN.PK_4:
                        owner.ChangeAppear(owner.skill.GetCommon5());
                        break;
                    case PING_KAN.PK_5:
                        owner.ChangeAppear(owner.skill.GetCommon6());
                        break;
                    case PING_KAN.PK_7:
                        owner.ChangeAppear(owner.skill.GetCommon8());
                        break;
                    case PING_KAN.PK_8:
                        owner.ChangeAppear(owner.skill.GetCommon9());
                        break;
                    default:
                        kAI.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
                        break;
                }
            }
            else
            {
				// 前面视野范围内有怪
				if (m_ePingKanState == PING_KAN.PK_3 ||
				    m_ePingKanState == PING_KAN.PK_6 ||
				    m_ePingKanState == PING_KAN.PK_9)
				{
					// 如果当前为收刀 判断距离来 是否追击敌人
					float dist = owner.GetFightDist(kTarget);
					if (dist > owner.GetProperty().getAttackRange())
					{
						ArrayList param = new ArrayList();
						param.Add(kTarget);
						param.Add(0);
						owner.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, param);
					}
					else
					{
                        // 当前是收刀 然后在竞技场中需要AI思考
                        if (Global.InArena())
                        {
                            Debug.Log("法师放技能");
                            AIUtil.AIBehaviour(owner.GetAI(), kTarget);
                            return;
                        }
                        else
                        {
                            owner.skill.setSkillTarget(kTarget);
                            if (owner.GetComponent<FightProperty>() != null)
                                owner.GetComponent<FightProperty>().SetLockedEnemy(kTarget);
                            ChangeAppearInOrder();
                        }
					}
				}
				else
				{
					// 当前不为收刀 在平砍中进入视野范围
					owner.skill.setSkillTarget(kTarget);
					if (owner.GetComponent<FightProperty>() != null)
						owner.GetComponent<FightProperty>().SetLockedEnemy(kTarget);
					ChangeAppearInOrder();
				}
			}
		}
		
		
		owner.movePosition(owner.getFaceDir() * delta * m_kSkillInfo.moveSpeed);
		
		if (Global.inMultiFightMap() && owner.skill.IsCurSkillCommon() && owner.getType() == CharacterType.CT_PLAYER)
		{
			// 因为普通攻击也带位移需要同步给其他人
			// 用 Vector3.z = 99999 表示是普通攻击
			MessageManager.Instance.sendMessageAskMove(owner.GetProperty().GetInstanceID(), new Vector3(0.0f, 0.0f, 99999.0f), owner.getPosition());
        }


        m_fUpdateTime += delta;
        
    }

    public int skill_type = 0;
	public bool command = false;
    public Vector3 turn_dir;
}

public class SkillMagicAttack1 : SkillMagicAttackCommonBase 
{

    public SkillMagicAttack1(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_1;
    }

	public override void active() 
    {
        m_ePingKanState = PING_KAN.PK_1;

        base.active();

		command = false;
	}
}

public class SkillMagicAttack2 : SkillMagicAttackCommonBase 
{

    public SkillMagicAttack2(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_2;
    }
	
	
	public override void active() 
    {
        m_ePingKanState = PING_KAN.PK_2;

        base.active();

		command = false;
	}
}

public class SkillMagicAttack3 : SkillMagicAttackCommonBase 
{

    public SkillMagicAttack3(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_3;
    }
	
	
	public override void active() 
    {
        m_ePingKanState = PING_KAN.PK_3;

        base.active();
	
		command = false;

	}
	
}

public class SkillMagicAttack4 : SkillMagicAttackCommonBase
{

    public SkillMagicAttack4(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_4;
    }


    public override void active()
    {
        m_ePingKanState = PING_KAN.PK_4;

        base.active();

        command = false;

        
    }

}

public class SkillMagicAttack5 : SkillMagicAttackCommonBase
{

    public SkillMagicAttack5(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_5;
    }


    public override void active()
    {
        m_ePingKanState = PING_KAN.PK_5;
        base.active();
        command = false;
    }

}


public class SkillMagicAttack6 : SkillMagicAttackCommonBase
{

    public SkillMagicAttack6(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_6;
    }


    public override void active()
    {
        m_ePingKanState = PING_KAN.PK_6;

        base.active();

        command = false;
    }

}

public class SkillMagicAttack7 : SkillMagicAttackCommonBase
{

    public SkillMagicAttack7(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_7;
    }


    public override void active()
    {
        m_ePingKanState = PING_KAN.PK_7;

        base.active();

        command = false;

        
    }

}


public class SkillMagicAttack8 : SkillMagicAttackCommonBase
{

    public SkillMagicAttack8(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_8;
    }


    public override void active()
    {
        m_ePingKanState = PING_KAN.PK_8;

        base.active();

        command = false;

        
    }

}


public class SkillMagicAttack9 : SkillMagicAttackCommonBase
{

    public SkillMagicAttack9(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_9;
    }


    public override void active()
    {
        m_ePingKanState = PING_KAN.PK_9;

        base.active();

        command = false;

        
    }

}