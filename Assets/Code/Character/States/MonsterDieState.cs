using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterDieState : DieState
{
    public override void Enter(CharacterAI ai)
    {
        base.Enter(ai);

        if (MonsterManager.Instance.IsLastMonster(ai.getOwner() as CharacterMonster))
        {

            if (Global.m_bAutoFight)
            {
                // 自动战斗结束
                BattleAutomation.GetInstance().EndSceneData();
            }

           
            EffectManager.Instance.BeginCloseUpEffect(ai.getOwner().transform.position);
        }


        // 表现先放在各自机器上
        // 同步其他单位或者怪物的表现
        if (CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap())
        {
            MessageManager.Instance.sendObjectBehavior((uint)ai.getOwner().GetProperty().GetInstanceID(),
                (int)Appear.BATTLE_STATE.BS_DIE, Vector3.zero, ai.getOwner().getPosition());
            Debug.Log("主机怪被 搞死了");
        }



        BattleManager.Instance.onPlayerkillMonster(ai.getOwner() as CharacterMonster);
    }
}