using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackState : State
{
    // 普通攻击锁定的目标对象
    public Character m_kTarget = null;


    public override void SetParam(ArrayList param)
    {
        if (param.Count == 1)
        {
            m_kTarget = param[0] as Character;
        }
    }


    public override void Enter(CharacterAI ai)
    {
        ai.SetCharacterState(CharacterAI.CHARACTER_STATE.CS_ATTACK);

        // 多人副本用的
        ai.getOwner().skill.setSkillTarget(m_kTarget);

        ai.getOwner().ChangeAppear(ai.getOwner().skill.GetCommonAttack());
    }

    public override void Execute(CharacterAI ai)
    {
        if (!ai.getOwner().skill.IsSkillCommonActive())
        {
            m_kMachine.ChangeState(ai.m_kIdleState);
            return;
        }

		if (CharacterAI.IsInState (ai.getOwner(), CharacterAI.CHARACTER_STATE.CS_DIE) && Global.InArena()) 
		{
			ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
		}
    }

    public override void Exit(CharacterAI ai)
    {

    }
}