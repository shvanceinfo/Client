using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerOtherAI : CharacterAI 
{
    public override void init(Character c)
    {
        base.init(c);

        m_kMachine = new PlayerStateMachine();
        m_kMachine.m_kOwner = this;

        m_kGlobalState = new PlayerGlobalState();
        m_kGlobalState.SetStateMachine(m_kMachine);

        m_kAttackState = new PlayerAttackState();
        m_kAttackState.SetStateMachine(m_kMachine);

        m_kBeHitState = new PlayerBehitState();
        m_kBeHitState.SetStateMachine(m_kMachine);

        

        m_kDieState = new PlayerOtherDieState();
        m_kDieState.SetStateMachine(m_kMachine);

        m_kIdleState = new PlayerOtherIdleState();
        m_kIdleState.SetStateMachine(m_kMachine);

        m_kMoveState = new PlayerMoveState();
        m_kMoveState.SetStateMachine(m_kMachine);


        m_kSkillState = new PlayerSkillState();
        m_kSkillState.SetStateMachine(m_kMachine);

        m_kPursueState = new PlayerOtherPursueState();
        m_kPursueState.SetStateMachine(m_kMachine);

        m_kDizzyState = new DizzyState();
        m_kDizzyState.SetStateMachine(m_kMachine);

        
        m_kMachine.SetGlobalState(m_kGlobalState);
        m_kGlobalState.Enter(this);

        m_kMachine.SetCurrentState(m_kIdleState);
        m_kIdleState.Enter(this);
    }

    

    




}



