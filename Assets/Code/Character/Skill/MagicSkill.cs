using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 技能容器类
public class MagicSkill : Skill {

    public SkillMagicAttack1 skill_common_cmd1;
    public SkillMagicAttack2 skill_common_cmd2;
    public SkillMagicAttack3 skill_common_cmd3;
    public SkillMagicAttack4 skill_common_cmd4;
    public SkillMagicAttack5 skill_common_cmd5;
    public SkillMagicAttack6 skill_common_cmd6;
    public SkillMagicAttack7 skill_common_cmd7;
    public SkillMagicAttack8 skill_common_cmd8;
    public SkillMagicAttack9 skill_common_cmd9;


	
	// Use this for initialization
	void Start () {

        // 平砍等使用频繁技能在初始的时候创建
        skill_common_cmd1 = new SkillMagicAttack1(103001001);
        skill_common_cmd1.setOwner(GetComponent<Character>());
        skill_common_cmd1.init();
        skill_common_cmd2 = new SkillMagicAttack2(103002001);
        skill_common_cmd2.setOwner(GetComponent<Character>());
        skill_common_cmd2.init();
        skill_common_cmd3 = new SkillMagicAttack3(103003001);
        skill_common_cmd3.setOwner(GetComponent<Character>());
        skill_common_cmd3.init();
        skill_common_cmd4 = new SkillMagicAttack4(103004001);
        skill_common_cmd4.setOwner(GetComponent<Character>());
        skill_common_cmd4.init();
        skill_common_cmd5 = new SkillMagicAttack5(103005001);
        skill_common_cmd5.setOwner(GetComponent<Character>());
        skill_common_cmd5.init();
        skill_common_cmd6 = new SkillMagicAttack6(103006001);
        skill_common_cmd6.setOwner(GetComponent<Character>());
        skill_common_cmd6.init();
        skill_common_cmd7 = new SkillMagicAttack7(103007001);
        skill_common_cmd7.setOwner(GetComponent<Character>());
        skill_common_cmd7.init();
        skill_common_cmd8 = new SkillMagicAttack8(103008001);
        skill_common_cmd8.setOwner(GetComponent<Character>());
        skill_common_cmd8.init();
        skill_common_cmd9 = new SkillMagicAttack9(103009001);
        skill_common_cmd9.setOwner(GetComponent<Character>());
        skill_common_cmd9.init();
	}

    public override SkillAppear GetCommon1()
    {
        return skill_common_cmd1;
    }

    public override SkillAppear GetCommon2()
    {
        return skill_common_cmd2;
    }

    public override SkillAppear GetCommon3()
    {
        return skill_common_cmd3;
    }

    public override SkillAppear GetCommon4()
    {
        return skill_common_cmd4;
    }

    public override SkillAppear GetCommon5()
    {
        return skill_common_cmd5;
    }

    public override SkillAppear GetCommon6()
    {
        return skill_common_cmd6;
    }

    public override SkillAppear GetCommon7()
    {
        return skill_common_cmd7;
    }

    public override SkillAppear GetCommon8()
    {
        return skill_common_cmd8;
    }

    public override SkillAppear GetCommon9()
    {
        return skill_common_cmd9;
    }

   

    public override bool IsSkillCommonActive()
    {
        return skill_common_cmd1.isActive() ||
            skill_common_cmd2.isActive() ||
            skill_common_cmd3.isActive() ||
            skill_common_cmd4.isActive() ||
            skill_common_cmd5.isActive() ||
            skill_common_cmd6.isActive() ||
            skill_common_cmd7.isActive() ||
            skill_common_cmd8.isActive() ||
            skill_common_cmd9.isActive();
    }


    public override SkillAppear GetCommonAttack()
    {
        return skill_common_cmd1;
	}
}
