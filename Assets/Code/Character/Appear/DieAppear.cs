using UnityEngine;
using System.Collections;

public class DieAppear : Appear 
{
    public DieAppear()
    {
        battle_state = Appear.BATTLE_STATE.BS_DIE;
    }

    public override bool isActive()
    {
        return is_active;
    }
}
