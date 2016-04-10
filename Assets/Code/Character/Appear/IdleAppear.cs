using UnityEngine;
using System.Collections;

public class IdleAppear : Appear 
{
    public IdleAppear()
    {
        battle_state = BATTLE_STATE.BS_IDLE;
    }

	public override void active() 
    {
        if (Global.inFightMap() || Global.inTowerMap() || 
			Global.inGoldenGoblin() || Global.InArena() || 
			Global.inMultiFightMap() || Global.InWorldBossMap()||Global.InAwardMap())
        {
            animation_name = "wait";
        }
        else
        {
            if (owner.getType() != CharacterType.CT_MONSTER)
            {
                animation_name = "idle";
            }
            else
                animation_name = "wait";
        }

        on_active(int.MaxValue);
	}
	
	public override void update(float delta) 
    {
        base.update(delta);

        owner.playAnimation(animation_name);
	}
}
