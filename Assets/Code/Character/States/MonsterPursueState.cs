using UnityEngine;
using System.Collections;

public class MonsterPursueState : PursueState
{
    public override void Execute(CharacterAI ai)
    {
        StraightAttack(ai);

        // 在追击中AI思考 怪物攻击最近的角色
        float fDist = 0.0f;

        Character kTargetCharacter = PlayerManager.Instance.getNearestPlayer(ai.getOwner().getPosition(), out fDist);

        AIUtil.TryExecuteAI(ai, kTargetCharacter);
    }

    public void StraightAttack(CharacterAI ai)
    {
        float dist = Vector3.Distance(ai.getOwner().transform.position, m_kPursurTarget.transform.position);

        CharacterMonster monster = ai.getOwner() as CharacterMonster;

        if (dist < ai.getOwner().GetProperty().getAttackRange())
        {
            // 如果处于攻击范围内了
            ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_ATTACK, m_kPursurTarget);
        }
        else
        {
            // 通过这个来得到格子中心点
            int nIndexXXX = 0;
            int nIndexZZZ = 0;

            // 通过增加随机的偏移，避免怪挤在一起
            float fRandomX = Random.Range(-2.0f, 2.0f);
            float fRandomZ = Random.Range(-2.0f, 2.0f);
            fRandomX = 0.0f;
            fRandomZ = 0.0f;


            AISystem.AStarAlgorithm.GetInstance().GamePointToIndex(m_kPursurTarget.getPosition() + new Vector3(fRandomX, 0.0f, fRandomZ), out nIndexXXX, out nIndexZZZ);

            int nID = AISystem.AStarAlgorithm.GetInstance().ConvertXZCoord2OneCoord(nIndexXXX, nIndexZZZ);

            AISystem.AStarAlgorithm.GetInstance().m_kNeighborIndex.Clear();
            AISystem.AStarAlgorithm.GetInstance().m_kNeighborIndex.Add(nID);
            int nFindNeighbor = AISystem.AStarAlgorithm.GetInstance().RecursiveFindWalkableNeighbor();

            if (nFindNeighbor != -1)
            {
                int nNeighX = 0;
                int nNeighZ = 0;

                AISystem.AStarAlgorithm.GetInstance().ConvertOneCoord2XZCoord(nFindNeighbor, out nNeighX, out nNeighZ);
                Vector3 ptr = AISystem.AStarAlgorithm.GetInstance().GetPositionByIndexCoord(nNeighX, nNeighZ);

                monster.GetComponent<AISystem.AIPathFinding>().m_kTargetPositon = ptr;
            }
            else
                monster.GetComponent<AISystem.AIPathFinding>().m_kTargetPositon = m_kPursurTarget.getPosition();
        }
    }
}
