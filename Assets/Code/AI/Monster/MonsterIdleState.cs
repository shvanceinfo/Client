using UnityEngine;
using System.Collections;

public class MonsterIdleState : IdleState 
{
    public override void Execute(CharacterAI ai)
    {
        if (Global.m_bCameraCruise)
        {
            Debug.Log("monster in came ");
            return;
        }

        // 怪物攻击最近的角色
        float fDist = 0.0f;

        Character kTargetCharacter = PlayerManager.Instance.getNearestPlayer(ai.getOwner().getPosition(), out fDist);

        if (kTargetCharacter)
        {
            AIUtil.AIBehaviour(ai, kTargetCharacter);
        }
    }
}
