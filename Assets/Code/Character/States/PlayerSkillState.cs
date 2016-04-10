using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkillState : SkillState
{
    // 普通攻击锁定的目标对象
    public Character m_kTarget = null;

    public override void Enter(CharacterAI ai)
    {
        // 释放之前判断是否有目标被选定
        if (ai.getOwner().skill.getSkillTarget() == null)
        {
            Character enemy = ai.GetNearestEnemy();

            if (enemy)
            {
                Vector3 dir = enemy.transform.position - ai.getOwner().transform.position;
                dir.y = 0;
                dir.Normalize();
                ai.getOwner().setFaceDir(dir);
                ai.getOwner().skill.setSkillTarget(enemy);
            }
        }

        base.Enter(ai);
    }

    public override void Exit(CharacterAI ai)
    {
        if (ai.getOwner().GetComponent<InputProperty>())
        {
            if (ai.getOwner().GetComponent<InputProperty>().m_bNeedLeaveFight)
            {
                ai.getOwner().GetComponent<InputProperty>().m_bNeedLeaveFight = false;
                ai.getOwner().GetComponent<FightProperty>().m_bEnterFight = false;
            }
        }	
    }
}