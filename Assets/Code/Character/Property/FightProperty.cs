using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightProperty : MonoBehaviour
{
    public Character m_kOwner = null;


    // 普通攻击和技能锁定的单位
    public Character m_kLockedEnemy = null;

    // 是否强制攻击
    public bool m_bForceAttack = false;

    // 是否进入了战斗状态
    public bool m_bEnterFight = false;

    void Awake()
    {
        m_kOwner = gameObject.GetComponent<Character>();
    }


    public void SetLockedEnemy(Character enemy)
    {
        if (CharacterAI.IsInState(enemy, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            return;
        }

        if (m_kLockedEnemy != enemy)
        {
            m_kLockedEnemy = enemy;

            BattleManager.Instance.ShowCharacterLocked(enemy);

            Vector3 dir = enemy.transform.position - m_kOwner.transform.position;
            dir.Normalize();
            m_kOwner.setFaceDir(dir);
        }
    }
    

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (m_kLockedEnemy == null)
        {
            return;
        }

        if (CharacterAI.IsInState(m_kLockedEnemy, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            m_kLockedEnemy = null;
        }

		// position offset when attacking
		if (m_kOwner.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_ATTACK)
		{
			Vector3 kPos = PlayerPursueState.CalculatePursuePoint(m_kOwner, m_kLockedEnemy);
			
			float dist = Vector3.Distance(m_kOwner.transform.position, kPos);
			
			if (dist < 0.1)
			{
				SkillAttackBaseCmd kSkillBase = m_kOwner.skill.getCurrentSkill() as SkillAttackBaseCmd;
				if (kSkillBase != null)
				{
					kSkillBase.m_nColliderStopValue = 0;
				}
			}
		}
    }    
}
