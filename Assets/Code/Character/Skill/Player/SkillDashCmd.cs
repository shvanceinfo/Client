/*
using UnityEngine;
using System.Collections;

public class SkillDashCmd : SkillAppear {
	
	float dash_time = 0.2f;
	
	float dash_speed = 30.0f;
	
	Vector3 dash_dir = Vector3.zero;

	public override void init() {
		//skill_type = SkillAppear.SkillType.ST_DASH;
	}
	
	public override void active() {
		
		owner.setAnimationTime("skill1a", dash_time);
		owner.crossFadeAnimation("skill1a", 0.2f);
		dash_dir = owner.getFaceDir();
		dash_dir.Normalize();
		owner.skill.setSkillHurting(true);
		on_active(dash_time);
	}
	
	public override void deActive() {
		
		owner.skill.setSkillHurting(false);
		on_deActive();
	}
	
	public override void update(float delta) {
		
		float t = updateTime(delta);
			
		owner.movePosition(dash_dir * t * dash_speed);
	}
}

public class SkillJumpCmd : SkillAppear {
	
	float jump_forward_speed = 0.0f;
	
	float jump_time = 2.4f;
	
	public override void init() {
		
	}
	
	public override void active() {
		
		owner.setAnimationTime("skill1b", jump_time);
		owner.playAnimation("skill1b");
		owner.showWeaponTrail();
		on_active(jump_time);
	}
	
	public override void deActive() {
		
		owner.hideWeaponTrail();
		on_deActive();
	}
	
	public override void update(float delta) {
			
		float t = updateTime(delta);
		
		owner.transform.Translate(owner.getFaceDir() * t * jump_forward_speed, Space.World);
	}
}

public class SkillBigCmd : SkillAppear {
	
	SkillDashCmd dash;
	SkillJumpCmd jump;
	SkillAppear cur_attack = null;
	
	public override void init() {
		
		//skill_type = SkillAppear.SkillType.ST_PLAYER_BIG;
			
		dash = new SkillDashCmd();
		dash.setOwner(owner);
		jump = new SkillJumpCmd();
		jump.setOwner(owner);
		
		dash.setNextCmd(jump);
	}
	
	public override void active() {
	
		cur_attack = dash;
		cur_attack.active();
		
		on_active(10000);
	}
	
	public override void deActive() {
		
		dash.deActive();
		jump.deActive();
		
		on_deActive();
	}
	
	public override void update(float delta) {
		
		if (cur_attack.isActive())
			cur_attack.update(delta);
		else {
			cur_attack.deActive();
			cur_attack = (SkillAppear)cur_attack.getNextCmd();
			if (cur_attack != null)
				cur_attack.active();
			else
				is_active = false;
		}
		
		//if (!cur_attack.isActive())
		//	is_active = false;
		
		time_since_begin += delta;
	}
	
	public override void compute() {
		
		MonsterManager.Instance.attackMonsterByRange(CharacterPlayer.sPlayerMe, this, 
			CharacterPlayer.sPlayerMe.transform.position,
			1.5f);
	}
}
*/