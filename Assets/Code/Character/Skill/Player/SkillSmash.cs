using UnityEngine;
using System.Collections;

public class SkillSmash : SkillAppear
{
    float smash_time = 0.4f;

    public SkillSmash(int skillid)
        : base(skillid)
    {

    }
    //public override void init()
    //{
    //    skill_type = SkillAppear.SkillType.ST_SMASH;
		
    //    loadConfig(skill_type);
    //}

    //public override void active()
    //{
    //    smash_time = owner.animation["tiaozhan"].length;
    //    owner.playAnimation("tiaozhan");
    //    owner.skill.setSkillHurting(true);
    //    on_active(smash_time);
    //    owner.skill.setCurrentSkill(this);
    //    //MissleManager.Instance.createSkillArea("Model/prefab/TestSkillArea", Vector3.zero, owner.transform, this, dash_time, 0);
		
    //    if (owner.skill.getSkillTarget()) {
    //        Vector3 dir = owner.skill.getSkillTarget().getPosition() - owner.getPosition();
    //        dir.Normalize();
    //        owner.setFaceDir(dir);
    //    }
    //}

    //public override void deActive()
    //{
    //    owner.skill.setSkillHurting(false);
    //    on_deActive();
    //    owner.skill.setCurrentSkill(null);
    //}

    //public override void update(float delta)
    //{
    //    float t = updateTime(delta);
    //    if (owner.skill.getCurrentSkill() == null)
    //    {
    //        int a = 0;
    //    }
    //}
}
