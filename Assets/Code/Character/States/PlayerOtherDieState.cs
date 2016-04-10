using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerOtherDieState : DieState
{
    public override void Enter(CharacterAI ai)
    {
        base.Enter(ai);

        if (Global.InArena())
        {
            Debug.Log("其他人死亡");
            BattleArena.GetInstance().ComputerDie();
        }
    }
}