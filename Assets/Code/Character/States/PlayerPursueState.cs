using UnityEngine;
using System.Collections;

public class PlayerPursueState : PursueState
{
    public override void Enter(CharacterAI ai)
    {
        base.Enter(ai);

        if (PursueState.EPursueReason.PR_PATH_FINDING == m_ePursueReason)
        {
            ai.getOwner().GetComponent<AISystem.AIPathFinding>().m_kTargetPositon = m_kPursurPoint;
        }
        else
        {
            PursueAI(ai);
        }
    }

    public override void Execute(CharacterAI ai)
    {
        PursueAI(ai);
    }

    public void PursueAI(CharacterAI ai)
    {
        switch (m_ePursueReason)
        {
            case PursueState.EPursueReason.PR_PURSUE_ONLY:
            case PursueState.EPursueReason.PR_PURSUE_WITH_AI:
                {
                    ai.getOwner().GetComponent<AISystem.AIPathFinding>().m_kTargetPositon = CalculatePursuePointByGameMode(ai, m_kPursurTarget);
                    //Debug.Log("寻路中...2");
                }
                break;
            case PursueState.EPursueReason.PR_PURSUE_WITH_ATTACK:
                {
                    float dist = Vector3.Distance(ai.getOwner().transform.position, CalculatePursuePointByGameMode(ai, m_kPursurTarget));

                    if (m_nNeedCastSkill == 0)
                    {
                        // 没有技能 那就是点击了普通攻击
                        if (dist < ai.getOwner().GetProperty().getAttackRange())
                        {
                            ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_ATTACK, m_kPursurTarget);
                        }
                        else
                        {
                            ai.getOwner().GetComponent<AISystem.AIPathFinding>().m_kTargetPositon = CalculatePursuePointByGameMode(ai, m_kPursurTarget);
                            //Debug.Log("寻路中...3");
                        }
                    }
                    else
                    {
                        // 有技能 
                        if (dist < ConfigDataManager.GetInstance().getSkillConfig().getSkillData(m_nNeedCastSkill).attack_range)
                        {
                            if (ai.getOwner().AICastSkill(m_kPursurTarget, m_nNeedCastSkill))
                            {
                                // 成功释放技能
                                //ai.getOwner().GetComponent<AISystem.AIPathFinding>().StopPathFinding();
                            }
                            else
                            {
                                // 有些限制 导致不能释放 进入待机
                                ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
                            }
                        }
                        else
                        {
                            ai.getOwner().GetComponent<AISystem.AIPathFinding>().m_kTargetPositon = CalculatePursuePointByGameMode(ai, m_kPursurTarget);
                            //Debug.Log("寻路中...4");
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    public static Vector3 CalculatePursuePointByGameMode(CharacterAI ai, Character kTarget)
    {
		return CalculatePursuePoint (ai.getOwner (), kTarget);
    }

	public static Vector3 CalculatePursuePoint(Character kAttacker, Character kTarget)
	{
		if (kTarget == null || kAttacker == null)
		{
			return Vector3.zero;
		}

		float scale = Mathf.Max(kTarget.transform.localScale.x, kTarget.transform.localScale.y, kTarget.transform.localScale.z); ;
		float fRealRadius = kTarget.GetComponent<CapsuleCollider>().radius * scale;
		
		if (Global.InWorldBossMap())
			fRealRadius += 1.0f;
		else
			fRealRadius += 0.5f;

		Vector3 pathDir = kAttacker.getPosition() - kTarget.getPosition();
		pathDir.Normalize();
		
		Vector3 kSearchPt = kTarget.getPosition() + pathDir * fRealRadius;
		
		return kSearchPt;
	}
}
