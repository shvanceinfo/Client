using UnityEngine;
using System.Collections;

public class SkillAbsorb : SkillAppear
{

    public SkillAbsorb(int skillid)
        : base(skillid)
    {

    }
    //float absorb_time = 0.4f;

    //public override void init()
    //{
    //    skill_type = SkillAppear.SkillType.ST_ABSORB;
    //    loadConfig(skill_type);
    //}

    //public override void active()
    //{
    //    absorb_time = owner.animation["xiguaijiafang"].length;
    //    //owner.setAnimationTime("chongzhuang1", dash_time);
    //    owner.playAnimation("xiguaijiafang");
    //    on_active(absorb_time);
    //    owner.skill.setCurrentSkill(this);
    //    MissleManager.Instance.createSkillAreaAbsorb("Model/prefab/TestAbsorb", Vector3.zero, owner.transform, this, absorb_time);
    //}

    //public override void deActive()
    //{
    //    on_deActive();
    //    owner.skill.setCurrentSkill(null);
    //}

    //public override void update(float delta)
    //{
    //    float t = updateTime(delta);
    //}
}