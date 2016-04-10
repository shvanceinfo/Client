using UnityEngine;
using System.Collections;

public class MoveState : State 
{
    public Vector3 m_vecMovePos;

    public override void SetParam(ArrayList param)
    {
        m_vecMovePos = (Vector3)param[0];
    }


    public override void Enter(CharacterAI ai)
    {
        ai.SetCharacterState(CharacterAI.CHARACTER_STATE.CS_MOVE);
        ai.getOwner().moveTo(m_vecMovePos);
        m_vecMovePos = Vector3.zero;

    }

    public override void Execute(CharacterAI ai)
    {
        if (!ai.getOwner().m_kMoveAppear.isActive())
        {
            m_kMachine.ChangeState(ai.m_kIdleState);
        }
    }

    public override void Exit(CharacterAI ai)
    {

    }
	
}
