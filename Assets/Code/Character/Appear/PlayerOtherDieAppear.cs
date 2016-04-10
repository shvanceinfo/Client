using UnityEngine;
using System.Collections;

public class PlayerOtherDieAppear : DieAppear 
{
    public override void active()
    {
        animation_name = "die2a";
        on_active(int.MaxValue);

        BattleMultiPlay.GetInstance().OnPlayerDie(owner);
    }
}
