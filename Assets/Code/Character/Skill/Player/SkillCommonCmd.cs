using UnityEngine;
using System.Collections;

public enum PING_KAN
{
    PK_NONE = 0,
    PK_1,
    PK_2,
    PK_3,
    PK_4,
    PK_5,
    PK_6,
    PK_7,
    PK_8,
    PK_9,
    PK_10,
    PK_11,
    PK_12
}

public class SkillAttackBaseCmd : SkillAppear 
{
    public PING_KAN m_ePingKanState;

    public int m_nColliderStopValue;

    public SkillAttackBaseCmd(int nSkillId) : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_1;

        init();
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
                return PING_KAN.PK_10;
            case PING_KAN.PK_10:
            case PING_KAN.PK_11:
            case PING_KAN.PK_12:
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
                owner.ChangeAppear(owner.skill.GetCommon10());
                break;
            case PING_KAN.PK_10:
                owner.ChangeAppear(owner.skill.GetCommon11());
                break;
            case PING_KAN.PK_11:
                owner.ChangeAppear(owner.skill.GetCommon12());
                break;
            case PING_KAN.PK_3:
            case PING_KAN.PK_6:
            case PING_KAN.PK_9:
            case PING_KAN.PK_12:
                owner.ChangeAppear(owner.skill.GetCommon1());
                break;

        }
    }

    public override void active()
    {
        animation_name = m_kSkillInfo.skillAction;


        //time_length = m_kData.life;
        time_length = owner.animation[animation_name].length;
        on_active(time_length);
        m_fUpdateTime = 0.0f;

        owner.skill.setCurrentSkill(this);

        // 每次发起平砍需要重置
        m_nColliderStopValue = 1;


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
            // 动作结束了
            // 有几种情况
            // 1. 在强制攻击中
			if (owner.GetComponent<FightProperty>())
			{
				if (owner.GetComponent<FightProperty>().m_bForceAttack)
	            {
	                switch (m_ePingKanState)
	                {
	                    case PING_KAN.PK_2:
	                    case PING_KAN.PK_5:
	                    case PING_KAN.PK_8:
	                    case PING_KAN.PK_12:
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
            

            // 2. 三个收刀动作结束后 看是否有移动需求 跳转状态 这个优先级最高
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
                    case PING_KAN.PK_10:
                        owner.ChangeAppear(owner.skill.GetCommon11());
                        break;
                    case PING_KAN.PK_11:
                        owner.ChangeAppear(owner.skill.GetCommon12());
                        break;
                    case PING_KAN.PK_3:
                    case PING_KAN.PK_6:
                    case PING_KAN.PK_9:
                    case PING_KAN.PK_12:
                        {
                            kAI.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE);
                        }
                        break;
                }

                return;
            }


            // 3. 如果是主角需要判断是否有摇杆移动打断
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
                    case PING_KAN.PK_10:
                        owner.ChangeAppear(owner.skill.GetCommon11());
                        break;
                    case PING_KAN.PK_11:
                        owner.ChangeAppear(owner.skill.GetCommon12());
                        break;
                    case PING_KAN.PK_3:
                    case PING_KAN.PK_6:
                    case PING_KAN.PK_9:
                    case PING_KAN.PK_12:
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
                    case PING_KAN.PK_10:
                        owner.ChangeAppear(owner.skill.GetCommon11());
                        break;
                    case PING_KAN.PK_11:
                        owner.ChangeAppear(owner.skill.GetCommon12());
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
                    m_ePingKanState == PING_KAN.PK_9 ||
                    m_ePingKanState == PING_KAN.PK_12)
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
                            AIUtil.AIBehaviour(owner.GetAI(), kTarget);
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
		
		
		owner.movePosition(owner.getFaceDir() * delta * m_kSkillInfo.moveSpeed * m_nColliderStopValue);

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



public class SkillAttack1Cmd : SkillAttackBaseCmd 
{

    public SkillAttack1Cmd(int nSkillId)
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
		
		Loger.Log("普通攻击"+animation_name);
	}
}

public class SkillAttack2Cmd : SkillAttackBaseCmd 
{

    public SkillAttack2Cmd(int nSkillId)
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

        owner.SetWeaponAvailable(true);
		
		Loger.Log("普通攻击"+animation_name);
	}

    public override void deActive()
    {
        base.deActive();

        owner.SetWeaponAvailable(false);
    }
}

public class SkillAttack3Cmd : SkillAttackBaseCmd 
{

    public SkillAttack3Cmd(int nSkillId)
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
		
		Loger.Log("普通攻击"+animation_name);
	}
	
}

public class SkillAttack4Cmd : SkillAttackBaseCmd
{

    public SkillAttack4Cmd(int nSkillId)
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

        Loger.Log("普通攻击" + animation_name);
    }

}

public class SkillAttack5Cmd : SkillAttackBaseCmd
{

    public SkillAttack5Cmd(int nSkillId)
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

        owner.SetWeaponAvailable(true);

        Loger.Log("普通攻击" + animation_name);
    }

    public override void deActive()
    {
        base.deActive();

        owner.SetWeaponAvailable(false);
    }
}


public class SkillAttack6Cmd : SkillAttackBaseCmd
{

    public SkillAttack6Cmd(int nSkillId)
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

        Loger.Log("普通攻击" + animation_name);
    }

}

public class SkillAttack7Cmd : SkillAttackBaseCmd
{

    public SkillAttack7Cmd(int nSkillId)
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

        Loger.Log("普通攻击" + animation_name);
    }

}


public class SkillAttack8Cmd : SkillAttackBaseCmd
{

    public SkillAttack8Cmd(int nSkillId)
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

        owner.SetWeaponAvailable(true);

        Loger.Log("普通攻击" + animation_name);
    }

    public override void deActive()
    {
        base.deActive();

        owner.SetWeaponAvailable(false);
    }

}


public class SkillAttack9Cmd : SkillAttackBaseCmd
{

    public SkillAttack9Cmd(int nSkillId)
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

        Loger.Log("普通攻击" + animation_name);
    }

}

public class SkillAttack10Cmd : SkillAttackBaseCmd
{

    public SkillAttack10Cmd(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_10;
    }


    public override void active()
    {
        m_ePingKanState = PING_KAN.PK_10;

        base.active();

        command = false;

        Loger.Log("普通攻击" + animation_name);
    }

}


public class SkillAttack11Cmd : SkillAttackBaseCmd
{

    public SkillAttack11Cmd(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_11;
    }


    public override void active()
    {
        m_ePingKanState = PING_KAN.PK_11;

        base.active();

        command = false;

        owner.SetWeaponAvailable(true);

        Loger.Log("普通攻击" + animation_name);
    }

    public override void deActive()
    {
        base.deActive();

        owner.SetWeaponAvailable(false);
    }
}


public class SkillAttack12Cmd : SkillAttackBaseCmd
{

    public SkillAttack12Cmd(int nSkillId)
        : base(nSkillId)
    {
        battle_state = BATTLE_STATE.BS_PING_KAN;
        m_ePingKanState = PING_KAN.PK_12;
    }


    public override void active()
    {
        m_ePingKanState = PING_KAN.PK_12;

        base.active();

        command = false;

        Loger.Log("普通攻击" + animation_name);
    }

}