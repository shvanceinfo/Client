using UnityEngine;
using System.Collections;

public class IdleState : State 
{
    public override void Enter(CharacterAI ai)
    {
        ai.SetCharacterState(CharacterAI.CHARACTER_STATE.CS_IDLE);
        ai.getOwner().ChangeAppear(ai.getOwner().m_kIdleAppear);
    }
}
