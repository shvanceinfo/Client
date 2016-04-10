using UnityEngine;
using System.Collections;

public class PlayerIdleState : IdleState
{
    public bool m_bArrived = false;

    public bool m_bShouldStay = false;

    public override void Execute(CharacterAI ai)
    {
        if (Global.m_bCameraCruise)
        {
            Debug.Log("玩家摄像机 迅游 ");
            return;
        }

        // 主城中 什么都不干
        if (Global.inCityMap())
        {
            return;
        }

        // 战斗中
        if (!Global.m_bAutoFight)
        {
            // 非自动战斗 模式
            if (ai.getOwner().GetComponent<FightProperty>().m_bEnterFight)
            {
                // 进入战斗状态
                FightProperty fightProperty = ai.getOwner().GetComponent<FightProperty>();

                if (fightProperty.m_kLockedEnemy)
                {
                    // 有锁定的目标
                    float dist = Vector3.Distance(ai.getOwner().transform.position,
                        PlayerPursueState.CalculatePursuePointByGameMode(ai, fightProperty.m_kLockedEnemy));
                    
                    if (dist < ai.getOwner().GetProperty().getAttackRange())
                    {
                        Debug.Log("待机切到攻击");
                        ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_ATTACK, fightProperty.m_kLockedEnemy);
                    }
                    else
                    {
                        Debug.Log("待机切到追*****");
                        ArrayList param = new ArrayList();
                        param.Add(fightProperty.m_kLockedEnemy);
                        param.Add(0);
                        ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, param);
                    }
                }
                else
                {
                    // 寻找目标锁定
                    Character enemy = BattleManager.Instance.GetViewRangeEnemy(ai.getOwner());
                    if (enemy)
                    {
                        fightProperty.SetLockedEnemy(enemy);

                        ArrayList param = new ArrayList();
                        param.Add(fightProperty.m_kLockedEnemy);
                        param.Add(0);
                        ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, param);
                    }
                }
            }
        }
        else
        {
            // 自动战斗 模式
            Character enemy = ai.GetNearestEnemy();
            if (enemy)
            {
                // 竞技场的自动战斗
                if (Global.InArena())
                {
                    if (BattleArena.GetInstance().m_bStartFight)
                    {
                        AIUtil.AIBehaviour(ai, enemy);
                    }
                }
                else
                {
                    // 不在竞技场的自动战斗 有敌人 就一定没有到达目的地
                    m_bArrived = false;
                    AIUtil.AIBehaviour(ai, enemy);
                }
            }
            else
            {
                // 自动战斗下 没有敌人
                if (!m_bShouldStay)
                {
                    // 没怪 并且没有撞门 就寻路
                    Vector3 purPt = BattleAutomation.GetInstance().GetNextMonsterArea();
                    if (purPt != Vector3.zero)
                    {
                        // 是否寻到路点
                        if (!m_bArrived)
                        {
                            ai.m_kPursueState.m_bForcePursue = false;
                            ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, purPt);
                        }
                        else
                        {
                            // 手动点地 寻路也到这里 这里比较特殊 当期位置和要寻路的地点距离超过一定大小表示 不是寻到路点
                            float distToMonsterArea = Vector3.Distance(purPt, ai.getOwner().getPosition());
                            if (distToMonsterArea > 1.0f)
                            {
                                ai.m_kPursueState.m_bForcePursue = false;
                                ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, purPt);
                            }
                        }
                    }
                }
            }
        }
    }

    // 一般状态下的打怪
    public void PursueMonster(CharacterAI ai, CharacterMonster monster)
    {
        Vector3 dir = monster.transform.position - ai.getOwner().transform.position;
        dir.Normalize();
        ai.getOwner().setFaceDir(dir);


        // 切到平砍
        float dist = Vector3.Distance(ai.getOwner().transform.position, monster.transform.position);

        if (dist > ai.getOwner().GetProperty().getAttackRange())
        {
            ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, monster);
        }
        else
        {
            m_kMachine.ChangeState(ai.m_kAttackState);
        }
    }
}
