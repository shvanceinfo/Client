using UnityEngine;
using System.Collections;

public class PlayerStateMachine : StateMachine
{
    public override void ProcessMessage(StateEvent stEvent)
    {
        if (stEvent.state != CharacterAI.CHARACTER_STATE.CS_PURSUE && m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_PURSUE)
        {
            // 当前状态是寻路 下一个状态不是寻路 需要停止寻路
            m_kOwner.getOwner().GetComponent<AISystem.AIPathFinding>().StopPathFinding();
        }

        if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_DIE)
        {
            if (stEvent.state == CharacterAI.CHARACTER_STATE.CS_RELIVE)
            {
                ChangeState(m_kOwner.m_kIdleState);
            }
                
            return;
        }

        switch (stEvent.state)
        {
            case CharacterAI.CHARACTER_STATE.CS_ATTACK:
                {
                    // 眩晕状态不能攻击
                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_DIZZY)
                    {
                        return;
                    }

                    m_kOwner.m_kAttackState.SetParam(stEvent.paramList);

                    ChangeState(m_kOwner.m_kAttackState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_MOVE:
                {
                    // 技能状态不能移动
                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_SKILL)
                    {
                        return;
                    }

                    // 受击状态不能移动
                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_BE_HIT)
                    {
                        return;
                    }

                    // 眩晕状态不能移动
                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_DIZZY)
                    {
                        return;
                    }

                    m_kOwner.m_kMoveState.SetParam(stEvent.paramList);

                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_ATTACK)
                    {
                        return;
                    }

                    ChangeState(m_kOwner.m_kMoveState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_PURSUE:
                {
                    // 眩晕状态不能追击
                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_DIZZY)
                    {
                        return;
                    }

                    // 技能状态不能追击
                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_SKILL)
                    {
                        return;
                    }

                    // 受击状态不能追击
                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_BE_HIT)
                    {
                        return;
                    }

                    m_kOwner.m_kPursueState.SetParam(stEvent.paramList);

                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_ATTACK)
                    {
                        //return;
                    }

                    
                    ChangeState(m_kOwner.m_kPursueState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_SKILL:
                {
                    // 眩晕状态不能放技能
                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_DIZZY)
                    {
                        return;
                    }

                    if (MainLogic.sMainLogic.isGameSuspended())
                    {
                        return;
                    }

                    m_kOwner.m_kSkillState.SetParam(stEvent.paramList);
                    ChangeState(m_kOwner.m_kSkillState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_IDLE:
            case CharacterAI.CHARACTER_STATE.CS_RELIVE:
                {
                    ChangeState(m_kOwner.m_kIdleState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_BE_HIT:
                {
                    // 眩晕状态不能受击
                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_DIZZY)
                    {
                        return;
                    }

                    // 不能持续进入受击状态
                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_BE_HIT)
                    {
                        return;
                    }


                    // 放技能不能进入受击
                    if (m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_SKILL)
                    {
                        return;
                    }

                    m_kOwner.m_kBeHitState.SetParam(stEvent.paramList);

                    ChangeState(m_kOwner.m_kBeHitState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_DIE:
                {
                    ChangeState(m_kOwner.m_kDieState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_DIZZY:
                {
                    m_kOwner.m_kDizzyState.SetParam(stEvent.paramList);
                    ChangeState(m_kOwner.m_kDizzyState);
                }
                break;
        }
    }
}
