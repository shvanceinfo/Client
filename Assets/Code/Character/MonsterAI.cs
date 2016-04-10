using UnityEngine;
using System.Collections;

public class MonsterAI : CharacterAI 
{
	protected Vector3 hit_back_dir;
	protected float speed_rate;
	
	public float protect_time = 0;
	
	public void setSpeedRate(float s) {
		speed_rate = s;
	}
	
	public float getSpeedRate() {
		return speed_rate;
	}
	
	public void setHitBackDir(Vector3 dir) {
		hit_back_dir = dir;
        hit_back_dir.Normalize(); // 需要正交化
	}
	
	public Vector3 getHitBackDir() {
		return hit_back_dir;
	}
	
	public override void init(Character c) 
    {
        base.init(c);


        // new AIBehavious
        CharacterMonster owner = c as CharacterMonster;
        if (c)
        {
            MonsterProperty mp = (MonsterProperty)owner.GetProperty();
            if (mp != null)
            {
                string strAICollection = ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(mp.template_id).ai;

                string[] subAI = strAICollection.Split(',');

                for (int i = 0; i < subAI.Length; ++i)
                {
                    int aiId = int.Parse(subAI[i]);

                    if (ConfigDataManager.GetInstance().getAIConfig().getAIData(aiId).type == AIBehaviouType.AI_BT_CAST_SKILL)
                    {
                        CharacterAI.AISkillData aiData = new CharacterAI.AISkillData();
                        aiData.nAIID = aiId;
                        aiData.nSkillID = ConfigDataManager.GetInstance().getAIConfig().getAIData(aiId).AIValue;
                        m_kAISkillBehavious.Add(aiData);
                    }
                }
            }  
        }
        
        

		m_kMachine = new MonsterStateMachine();
		m_kMachine.m_kOwner  = this;
		
		m_kGlobalState = new MonsterGlobalState();
        m_kGlobalState.SetStateMachine(m_kMachine);
			
		m_kAttackState = new MonsterAttackState();
        m_kAttackState.SetStateMachine(m_kMachine);

        m_kBeHitState = new MonsterBeHitState();
        m_kBeHitState.SetStateMachine(m_kMachine);
		
		m_kBornState = new MonsterBornState();
        m_kBornState.SetStateMachine(m_kMachine);
		
		m_kDieState = new MonsterDieState();
        m_kDieState.SetStateMachine(m_kMachine);
		
		m_kIdleState = new MonsterIdleState();
        m_kIdleState.SetStateMachine(m_kMachine);
		
		m_kMoveState = new MonsterMoveState();
        m_kMoveState.SetStateMachine(m_kMachine);

        m_kGoblinState = new MonsterGoblinState();
        m_kGoblinState.SetStateMachine(m_kMachine);

        m_kPursueState = new MonsterPursueState();
        m_kPursueState.SetStateMachine(m_kMachine);

        m_kSkillState = new SkillState();
        m_kSkillState.SetStateMachine(m_kMachine);

        m_kDizzyState = new DizzyState();
        m_kDizzyState.SetStateMachine(m_kMachine);
		
		m_kMachine.SetGlobalState(m_kGlobalState);
        m_kGlobalState.Enter(this);

		m_kMachine.SetCurrentState(m_kBornState);
        m_kBornState.Enter(this);
	}
}
