using UnityEngine;
using System.Collections;

public class MonsterStateMachine : StateMachine
{
    public override void ProcessMessage(StateEvent stEvent)
    {
        if (stEvent.state != CharacterAI.CHARACTER_STATE.CS_PURSUE && m_kOwner.m_ePlayerState == CharacterAI.CHARACTER_STATE.CS_PURSUE)
        {
            // 当前状态是寻路 下一个状态不是寻路 需要停止寻路
            m_kOwner.getOwner().GetComponent<AISystem.AIPathFinding>().StopPathFinding();
        }


        switch (stEvent.state)
        {
            case CharacterAI.CHARACTER_STATE.CS_BORN:
                {
                    ChangeState(m_kOwner.m_kBornState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_PURSUE:
                {
                    // 眩晕状态不能追击
                    if (m_kOwner.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_DIZZY)
                    {
                        return;
                    }

                    m_kOwner.m_kPursueState.SetParam(stEvent.paramList);
                    ChangeState(m_kOwner.m_kPursueState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_BE_HIT:
                {
                    if (m_kOwner.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_DIZZY)
                    {
                        return;
                    }

                    if (m_kOwner.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_SKILL)
                    {
                        return;
                    }


                    m_kOwner.m_kBeHitState.SetParam(stEvent.paramList);
                    ChangeState(m_kOwner.m_kBeHitState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_DIE:
                {
                    if (m_kOwner.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_DIE)
                    {
                        return;
                    }

                    m_kOwner.m_kDieState.SetParam(stEvent.paramList);
                    ChangeState(m_kOwner.m_kDieState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_ATTACK:
                {
                    // 眩晕状态不能攻击
                    if (m_kOwner.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_DIZZY)
                    {
                        return;
                    }

                    m_kOwner.m_kAttackState.SetParam(stEvent.paramList);
                    ChangeState(m_kOwner.m_kAttackState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_IDLE:
                {
                    ChangeState(m_kOwner.m_kIdleState);
                }
                break;
            case CharacterAI.CHARACTER_STATE.CS_SKILL:
                {
                    // 眩晕状态不能放技能
                    if (m_kOwner.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_DIZZY)
                    {
                        return;
                    }

                    m_kOwner.m_kSkillState.SetParam(stEvent.paramList);
                    ChangeState(m_kOwner.m_kSkillState);
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

    public override void Update()
    {
        if (CharacterPlayer.sPlayerMe == null)
        {
            return;
        }

        if (CharacterPlayer.sPlayerMe.GetAI() == null)
        {
            return;
        }


        if (!Global.inMultiFightMap())
        {
            // 只有在多人副本中怪物可以鞭尸
            if (CharacterPlayer.sPlayerMe.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_DIE)
            {
                m_kOwner.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
                return;
            }
        }
        

        base.Update();
    }
}
