using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterBeHitState : BeHitState
{
 
    public override void Enter(CharacterAI ai)
    {
        base.Enter(ai);

        BattleManager.Instance.OnMonsterBeHit(ai.getOwner());
    }

    public override void Execute(CharacterAI ai)
    {
        if (!ai.getOwner().m_kHitAppear.isActive() && !ai.getOwner().m_kHitBackAppear.isActive())
        {
			ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_DIZZY, ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(1017001).type7Data);
			return;
        }
    }

    public override void Exit(CharacterAI ai)
    {

    }
}