using UnityEngine;
using System.Collections;

public class SmoothPosAppear : Appear {
	
	float smooth_speed = 5.0f;
	float smooth_time = 0;
	Vector3 target_pos;

    public SmoothPosAppear()
    {
        battle_state = Appear.BATTLE_STATE.BS_SMOOTH_POS;
    }
	
	public override void active() 
    {
        owner.m_eAppearState = battle_state;

		on_active(smooth_time);
	}

	
	public override void update(float delta) {
		
		float t = updateTime(delta);
		
		float dist = Vector3.Distance(owner.getPosition(), target_pos);
		
		if (dist < smooth_speed * t) {
			owner.setPosition(target_pos);
		}
		else {
			Vector3 dir = target_pos - owner.getPosition();
			dir.Normalize();
			owner.movePosition(dir * smooth_speed * t);
		}
	}
	
	public void smoothToPos(Vector3 pos) {
		
		target_pos = pos;
		float dist = Vector3.Distance(owner.getPosition(), pos);
		smooth_time = dist / smooth_speed;
	}
}
