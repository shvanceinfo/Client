using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using model;

public class PlayerAI : CharacterAI 
{
    public override void init(Character c)
    {
        base.init(c);

        // new AIBehavious
        CharacterPlayer owner = c as CharacterPlayer;
        if (c)
        {
            CharacterProperty mp = (CharacterProperty)owner.GetProperty();
            if (mp != null)
            {
                List<AIDataItem> ailist = ConfigDataManager.GetInstance().getAIConfig().GetAIList(true, mp.getCareer());

                for (int i = 0; i < ailist.Count; ++i)
                {
                    foreach (SkillVo item in SkillTalentManager.Instance.ActiveSkills)
                    {
                        int nPreSkillID = item.XmlID / 1000;
                        if (nPreSkillID == ailist[i].AIValue)
                        {
                            CharacterAI.AISkillData aiData = new CharacterAI.AISkillData();
                            aiData.nAIID = ailist[i].id;
                            aiData.nSkillID = item.XmlID;
                            m_kAISkillBehavious.Add(aiData);
                        }
                    }
                }
            }
        }

        m_kMachine = new PlayerStateMachine();
        m_kMachine.m_kOwner = this;

        m_kGlobalState = new PlayerGlobalState();
        m_kGlobalState.SetStateMachine(m_kMachine);

        m_kAttackState = new PlayerAttackState();
        m_kAttackState.SetStateMachine(m_kMachine);

        m_kBeHitState = new PlayerBehitState();
        m_kBeHitState.SetStateMachine(m_kMachine);

        

        m_kDieState = new PlayerDieState();
        m_kDieState.SetStateMachine(m_kMachine);

        m_kIdleState = new PlayerIdleState();
        m_kIdleState.SetStateMachine(m_kMachine);

        m_kMoveState = new PlayerMoveState();
        m_kMoveState.SetStateMachine(m_kMachine);


        m_kSkillState = new PlayerSkillState();
        m_kSkillState.SetStateMachine(m_kMachine);

        m_kPursueState = new PlayerPursueState();
        m_kPursueState.SetStateMachine(m_kMachine);

        m_kDizzyState = new DizzyState();
        m_kDizzyState.SetStateMachine(m_kMachine);

        
        m_kMachine.SetGlobalState(m_kGlobalState);
        m_kGlobalState.Enter(this);

        m_kMachine.SetCurrentState(m_kIdleState);
        m_kIdleState.Enter(this);
    }

    

    




}



