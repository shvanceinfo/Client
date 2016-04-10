using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBehitState : BeHitState
{
    //float m_fHitBack;
    //Vector3 m_vecHitDir;

    //public override void SetParam(ArrayList param)
    //{
    //    m_fHitBack = (float)param[0];
    //    m_vecHitDir = (Vector3)param[2];
    //}

    public override void Enter(CharacterAI ai)
    {
        ai.SetCharacterState(CharacterAI.CHARACTER_STATE.CS_BE_HIT);

        ai.getOwner().PlayerBeHitAnimation(m_fHitBack, m_vecHitDir);

        BattleManager.Instance.OnPlayerBeHit();
    }

   

    public override void Exit(CharacterAI ai)
    {

    }
}