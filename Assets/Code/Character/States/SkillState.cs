using UnityEngine;
using System.Collections;



public class SkillState : State 
{
    public int skillId;

    public override void SetParam(ArrayList param)
    {
        skillId = (int)param[0];
    }

    public override void Enter(CharacterAI ai)
    {

        //if (ai.getOwner().getType() == CharacterType.CT_MONSTER)
        //{
        //    ai.getOwner().GetComponent<PathFindingMonster>().StopMove();
        //}
        //else if (ai.getOwner().getType() == CharacterType.CT_PLAYER)
        //{
        //    ai.getOwner().GetComponent<PathFinding>().StopMove();
        //}

        ai.SetCharacterState(CharacterAI.CHARACTER_STATE.CS_SKILL);
        //if (skillId == 201003)
        //{
        //    ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new SkillWhirlWind(201003)));
        //    return;
        //}
        //else if (skillId == 400001)
        //{
        //    ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new SkillRollCmd(400001)));
        //    return;
        //}
        //else if (skillId == 300002)
        //{
        //    ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new FireStorm(300002)));
        //    return;
        //}
        //else 
            if (skillId == 400002001)
        {
            ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new SkillFlash(400002001)));
            return;
        }
        //else if (skillId == 203202)
        //{
        //    ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new SkillFlashBack(203202)));
        //    return;
        //}
        else if (skillId == 400003001)
        {
            ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new SkillMagFlash(400003001)));
            return;
        }

        switch ((SKILL_APPEAR)skillId)
        {
            //case SKILL_APPEAR.SA_WHIRL_WIND:
            //    ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new SkillWhirlWind(201003)));
            //    break;
            //case SKILL_APPEAR.SA_ROLL:
            //    ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new SkillRollCmd(400001)));
            //    break;
            //case SKILL_APPEAR.SA_FIRE_RAIN:
            //    ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new FireStorm(300002)));
            //    break;
            case SKILL_APPEAR.SA_FLASH_AWAY:
                ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new SkillFlash(400002001)));
                break;
            case SKILL_APPEAR.SA_MAG_FLASH_AWAY:
                ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new SkillMagFlash(400003001)));
                break;
            //case SKILL_APPEAR.SA_MAG_FLASH_BACK:
            //    ai.getOwner().ChangeAppear(ai.getOwner().skill.InitSkill(new SkillFlashBack(203202)));
            //    break;
            default:
                bool isPetAction=ConfigDataManager.GetInstance().getSkillExpressionConfig().getSkillExpressionData(skillId/1000).isPetAction;
                if (isPetAction && ai.getOwner().m_kPet)
                {
                    ai.getOwner().m_kPet.PlaySkill();
                }
                ai.getOwner().ChangeAppear(ai.getOwner().skill.CreateSkill(skillId));
                break;
        }
    }

}
