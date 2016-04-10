using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 技能容器类
public class MonsterSkill : Skill 
{

	SkillMonsterCommonCmd skill_monster_common_cmd;

	
	

	// Use this for initialization
	void Start () 
    {
        skill_monster_common_cmd = new SkillMonsterCommonCmd(300001001);
		skill_monster_common_cmd.setOwner(GetComponent<Character>());
		skill_monster_common_cmd.init();
	}

    public override bool IsSkillCommonActive()
    {
        return skill_monster_common_cmd.isActive();
    }


    public override SkillAppear GetCommonAttack()
    {
        return skill_monster_common_cmd;
    }

    public override bool IsCurSkillCommon()
    {
        return true;
    }
}
