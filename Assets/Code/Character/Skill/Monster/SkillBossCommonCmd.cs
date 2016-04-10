using UnityEngine;
using System.Collections;

public class FireStorm : SkillAppear 
{
    public FireStorm(int skillid) : base(skillid)
    {

    }

    //public override void init() {
    //    skill_type = SkillAppear.SkillType.ST_MONSTER_COMMON;
    //    skill_life = 0;
    //    animation_name = "skill1";
    //    loadConfig(skill_type);
    //}

    public override void active()
    {
        base.active();
        GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.FIRE_RAIN_PREFAB, EBundleType.eBundleMonster);
        GameObject go = BundleMemManager.Instance.instantiateObj(asset, owner.getPosition(), Quaternion.identity);
        FireRain fr = go.GetComponent<FireRain>();
        if (fr)
        {
            fr.skill_cmd = this;
        }
    }
}
