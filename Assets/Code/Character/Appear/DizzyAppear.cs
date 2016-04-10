using UnityEngine;
using System.Collections;

public class DizzyAppear : Appear
{
    float dizzy_time = 0.0f;

    public DizzyAppear()
    {
        battle_state = BATTLE_STATE.BS_DIZZY;
    }

    public void SetDuration(float time)
    {
        dizzy_time = time;
    }

    public override void active()
    {
        //if (owner.getType() != CharacterType.CT_MONSTER)
        //{
        //    animation_name = "idle";
        //}
        //else
        animation_name = "wait";

        on_active(dizzy_time);


        if (CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap())
        {
            MessageManager.Instance.sendObjectBehavior((uint)owner.GetProperty().GetInstanceID(),
                (int)battle_state, Vector3.zero, owner.getPosition());
        }
    }

	protected override bool IsLoopAnimation()
    {
        return true;
    }
}
