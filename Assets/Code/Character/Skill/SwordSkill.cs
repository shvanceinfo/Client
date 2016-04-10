using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 技能容器类
public class SwordSkill : Skill {

    public SkillAttack1Cmd skill_common_cmd1;
    public SkillAttack2Cmd skill_common_cmd2;
    public SkillAttack3Cmd skill_common_cmd3;
    public SkillAttack4Cmd skill_common_cmd4;
    public SkillAttack5Cmd skill_common_cmd5;
    public SkillAttack6Cmd skill_common_cmd6;
    public SkillAttack7Cmd skill_common_cmd7;
    public SkillAttack8Cmd skill_common_cmd8;
    public SkillAttack9Cmd skill_common_cmd9;
    public SkillAttack10Cmd skill_common_cmd10;
    public SkillAttack11Cmd skill_common_cmd11;
    public SkillAttack12Cmd skill_common_cmd12;

	
	// Use this for initialization
	void Start () {

        // 平砍等使用频繁技能在初始的时候创建
        skill_common_cmd1 = new SkillAttack1Cmd(101001001);
        skill_common_cmd1.setOwner(GetComponent<Character>());
        skill_common_cmd1.init();
        skill_common_cmd2 = new SkillAttack2Cmd(101002001);
        skill_common_cmd2.setOwner(GetComponent<Character>());
        skill_common_cmd2.init();
        skill_common_cmd3 = new SkillAttack3Cmd(101003001);
        skill_common_cmd3.setOwner(GetComponent<Character>());
        skill_common_cmd3.init();
        skill_common_cmd4 = new SkillAttack4Cmd(101004001);
        skill_common_cmd4.setOwner(GetComponent<Character>());
        skill_common_cmd4.init();
        skill_common_cmd5 = new SkillAttack5Cmd(101005001);
        skill_common_cmd5.setOwner(GetComponent<Character>());
        skill_common_cmd5.init();
        skill_common_cmd6 = new SkillAttack6Cmd(101006001);
        skill_common_cmd6.setOwner(GetComponent<Character>());
        skill_common_cmd6.init();
        skill_common_cmd7 = new SkillAttack7Cmd(101007001);
        skill_common_cmd7.setOwner(GetComponent<Character>());
        skill_common_cmd7.init();
        skill_common_cmd8 = new SkillAttack8Cmd(101008001);
        skill_common_cmd8.setOwner(GetComponent<Character>());
        skill_common_cmd8.init();
        skill_common_cmd9 = new SkillAttack9Cmd(101009001);
        skill_common_cmd9.setOwner(GetComponent<Character>());
        skill_common_cmd9.init();
        skill_common_cmd10 = new SkillAttack10Cmd(101010001);
        skill_common_cmd10.setOwner(GetComponent<Character>());
        skill_common_cmd10.init();
        skill_common_cmd11 = new SkillAttack11Cmd(101011001);
        skill_common_cmd11.setOwner(GetComponent<Character>());
        skill_common_cmd11.init();
        skill_common_cmd12 = new SkillAttack12Cmd(101012001);
        skill_common_cmd12.setOwner(GetComponent<Character>());
        skill_common_cmd12.init();
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

    public override SkillAppear GetCommon10()
    {
        return skill_common_cmd10;
    }

    public override SkillAppear GetCommon11()
    {
        return skill_common_cmd11;
    }

    public override SkillAppear GetCommon12()
    {
        return skill_common_cmd12;
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
            skill_common_cmd9.isActive() || 
            skill_common_cmd10.isActive() || 
            skill_common_cmd11.isActive() ||
            skill_common_cmd12.isActive();
    }


    public override SkillAppear GetCommonAttack()
    {
        return skill_common_cmd1;
	}
}
