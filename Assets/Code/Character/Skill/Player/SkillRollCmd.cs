using UnityEngine;
using System.Collections;

public class SkillRollCmd : SkillAppear
{
    //float roll_time = 0.2f;

    //Vector3 roll_dir;


    public SkillRollCmd(int skillid)
        : base(skillid)
    {
        battle_state = BATTLE_STATE.BS_ROLL;
    }

    //public void setDir(Vector3 dir) {
    //    if (dir.z/dir.x > -0.1f ) {
    //        int a = 0;
    //    }
    //    roll_dir = dir;
    //    owner.setFaceDir(roll_dir);
    //}
	
    //public override void init() {
		
    //    skill_type = SkillAppear.SkillType.ST_ROLL;
		
    //    animation_name = "tumble";
    //    loadConfig(skill_type);
    //}
	
    //public override void active() {
    //    roll_time = owner.animation[animation_name].length;
    //    on_active(roll_time);
    //    owner.skill.setCurrentSkill(this);
    //}

    //public override void deActive()  {
    //    on_deActive();
    //    owner.skill.setCurrentSkill(null);
    //}

    //public override void update(float delta) {
    //    float t = updateTime(delta);
    //    owner.movePosition(owner.getFaceDir() * t * skill_move_speed);
    //}
}
