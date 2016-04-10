using UnityEngine;
using System.Collections;

public class HitAppear : Appear 
{
    public HitAppear()
    {
        battle_state = BATTLE_STATE.BS_BE_HIT;
    }

	public override void active() 
    {
		//owner.crossFadeAnimation("hurt",0.1f);
        animation_name = "hurt";
		float hitTime = owner.animation["hurt"].length;
		on_active(hitTime);

        // 目前伤害后的表现放各自的机器上
        // 如果是主机上怪受击 或者其他人受击 需要同步到其他机器
        //if (Global.inMultiFightMap() && CharacterPlayer.character_property.getHostComputer())
        //{
        //    MessageManager.Instance.sendObjectBehavior((uint)owner.GetProperty().GetInstanceID(),
        //        (int)battle_state, owner.getFaceDir(), owner.getPosition());
        //}
	}
}
