using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WeaponSkill : SkillBase
{
    WeaponSkill(int nSkillId) : base(nSkillId)
    {
        m_eSkillGroupType = SKILL_GROUP_TYPE.WEAPON_SKILL;
    }
}