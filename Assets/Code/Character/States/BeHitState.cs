using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeHitState : State
{
    protected float m_fHitBack;
    protected Vector3 m_vecHitDir;

    public override void SetParam(ArrayList param)
    {
        m_fHitBack = (float)param[0];
        m_vecHitDir = (Vector3)param[1];
    }

    public override void Enter(CharacterAI ai)
    {
        ai.SetCharacterState(CharacterAI.CHARACTER_STATE.CS_BE_HIT);

        if (m_fHitBack > 0.0f)
        {
            ai.getOwner().m_kHitBackAppear.setDir(m_vecHitDir);
            ai.getOwner().m_kHitBackAppear.setSpeedRate(m_fHitBack);
            ai.getOwner().ChangeAppear(ai.getOwner().m_kHitBackAppear);
        }
        else
            ai.getOwner().ChangeAppear(ai.getOwner().m_kHitAppear);
    }

    public override void Execute(CharacterAI ai)
    {
        if (!ai.getOwner().m_kHitAppear.isActive() && !ai.getOwner().m_kHitBackAppear.isActive())
        {
            ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
            return;
        }

        if (ai.getOwner().GetProperty().getHP() <= 0)
        {
            ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_DIE);
            return;
        }
    }

    public override void Exit(CharacterAI ai)
    {

    }
}