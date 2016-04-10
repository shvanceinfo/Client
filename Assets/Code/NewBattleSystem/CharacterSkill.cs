using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterSkill : SkillBase
{
    CharacterSkill(int nSkillId) : base(nSkillId)
    {
        m_eSkillGroupType = SKILL_GROUP_TYPE.CHARACTER_SKILL;
    }
}