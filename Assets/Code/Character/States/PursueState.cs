using UnityEngine;
using System.Collections;

public class PursueState : State
{
    // 追击对象
    public Character m_kTmpSavedTarget;
    public Character m_kPursurTarget;

    
    // 追击点
    public Vector3 m_kPursurPoint;


    // 是否强制寻路 (中间有怪不打怪)
    public bool m_bForcePursue = true;


    public bool m_bGotoGate = true;

    public NPC m_kPursurNPC = null;

    // 追击到了需要放这个技能
    public int m_nNeedCastSkill = 0;

    // 追击的原因
    public enum EPursueReason
    {
        PR_PURSUE_ONLY = 0, // 只是为了从其他状态切到追击状态 保持追击的数据不变 特殊处理用在主角的普通攻击的动作段中
        PR_PATH_FINDING = 1, // 寻路 一般用在做任务
        PR_PURSUE_WITH_AI = 2,  // 追到对象的位置 一般用于追击AI 后续动作不确定(由AI来管)
        PR_PURSUE_WITH_ATTACK = 3, // 追到对象的位置 一般用于玩家主动按了技能或者攻击 后续技能ID、攻击确定
    }

    public EPursueReason m_ePursueReason = EPursueReason.PR_PURSUE_ONLY;

    public override void SetParam(ArrayList param)
    {
        // 确定追击原因
        if (param.Count == 1)
        {
            m_kTmpSavedTarget = param[0] as Character;

            if (m_kTmpSavedTarget == null)
            {
                m_kPursurPoint = (Vector3)param[0];
                m_ePursueReason = EPursueReason.PR_PATH_FINDING;
            }
            else
            {
                m_ePursueReason = EPursueReason.PR_PURSUE_WITH_AI;
            }

            m_nNeedCastSkill = 0;            
        }
        else if (param.Count == 2)
        {
            m_kTmpSavedTarget = param[0] as Character;
            m_nNeedCastSkill = (int)param[1];

            m_ePursueReason = EPursueReason.PR_PURSUE_WITH_ATTACK;
        }
    }

    public void ResetData()
    {
        m_kTmpSavedTarget = null;
        m_kPursurTarget = null;
        m_kPursurPoint = Vector3.zero;
        m_bForcePursue = true;
        m_bGotoGate = false;
        m_kPursurNPC = null;
        m_ePursueReason = EPursueReason.PR_PURSUE_ONLY;
    }

    public override void Enter(CharacterAI ai)
    {
        ai.SetCharacterState(CharacterAI.CHARACTER_STATE.CS_PURSUE);
        m_kPursurTarget = m_kTmpSavedTarget;

        m_kTmpSavedTarget = null;
    }
}
