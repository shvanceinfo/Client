using UnityEngine;
using System.Collections;

public class PlayerOtherIdleState : IdleState
{
    public override void Execute(CharacterAI ai)
    {
        
        if (BattleArena.GetInstance().m_bStartFight)
        {
            if (ai.m_bAIAvailable)
            {
                AIUtil.AIBehaviour(ai, CharacterPlayer.sPlayerMe);
            }
        }
    }
}
