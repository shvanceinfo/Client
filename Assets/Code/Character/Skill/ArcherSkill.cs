using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcherSkill : Skill 
{
    public SkillArcherAttack1 skill_common_cmd1;
    public SkillArcherAttack2 skill_common_cmd2;
    public SkillArcherAttack3 skill_common_cmd3;
    public SkillArcherAttack4 skill_common_cmd4;
    public SkillArcherAttack5 skill_common_cmd5;
    //public SkillArcherAttack6 skill_common_cmd6;
    //public SkillArcherAttack7 skill_common_cmd7;
    //public SkillArcherAttack8 skill_common_cmd8;
    //public SkillArcherAttack9 skill_common_cmd9;



	// Use this for initialization
	void Start () {
	
        // 平砍等使用频繁技能在初始的时候创建
        skill_common_cmd1 = new SkillArcherAttack1(102001001);
        skill_common_cmd1.setOwner(GetComponent<Character>());
        skill_common_cmd1.init();
        skill_common_cmd2 = new SkillArcherAttack2(102002001);
        skill_common_cmd2.setOwner(GetComponent<Character>());
        skill_common_cmd2.init();
        skill_common_cmd3 = new SkillArcherAttack3(102003001);
        skill_common_cmd3.setOwner(GetComponent<Character>());
        skill_common_cmd3.init();
        skill_common_cmd4 = new SkillArcherAttack4(102004001);
        skill_common_cmd4.setOwner(GetComponent<Character>());
        skill_common_cmd4.init();
        skill_common_cmd5 = new SkillArcherAttack5(102005001);
        skill_common_cmd5.setOwner(GetComponent<Character>());
        skill_common_cmd5.init();
        //skill_common_cmd6 = new SkillArcherAttack6(101006);
        //skill_common_cmd6.setOwner(GetComponent<Character>());
        //skill_common_cmd6.init();
        //skill_common_cmd7 = new SkillArcherAttack7(101007);
        //skill_common_cmd7.setOwner(GetComponent<Character>());
        //skill_common_cmd7.init();
        //skill_common_cmd8 = new SkillArcherAttack8(101008);
        //skill_common_cmd8.setOwner(GetComponent<Character>());
        //skill_common_cmd8.init();
        //skill_common_cmd9 = new SkillArcherAttack9(101009);
        //skill_common_cmd9.setOwner(GetComponent<Character>());
        //skill_common_cmd9.init();  
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

    //public override SkillAppear GetCommon6()
    //{
    //    return skill_common_cmd6;
    //}

    //public override SkillAppear GetCommon7()
    //{
    //    return skill_common_cmd7;
    //}

    //public override SkillAppear GetCommon8()
    //{
    //    return skill_common_cmd8;
    //}

    //public override SkillAppear GetCommon9()
    //{
    //    return skill_common_cmd9;
    //}



    public override SkillAppear GetCommonAttack()
    {
        return skill_common_cmd1;
    }

    public override bool IsSkillCommonActive()
    {
        return skill_common_cmd1.isActive() ||
            skill_common_cmd2.isActive() ||
            skill_common_cmd3.isActive() ||
            skill_common_cmd4.isActive() ||
            skill_common_cmd5.isActive();
            //skill_common_cmd6.isActive() ||
            //skill_common_cmd7.isActive() ||
            //skill_common_cmd8.isActive() ||
            //skill_common_cmd9.isActive();
    }
}
