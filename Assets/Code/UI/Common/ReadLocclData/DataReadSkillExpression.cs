using UnityEngine;
using System.Collections;
using System;

public class SkillExpressionDataItem 
{
	public int id;
    public int skillEffectTarget;

    public string skillAction;

    public string skillHurtAction;
    public SKILL_DAMAGE_TYPE skillDamageType;
    public int skillRangeType;
    public int rangeFollow;
    public int skillAngle;
    public float skillRadius;
    public float collideSpeed;
    public float collideLife;
    public float moveSpeed;
    public float atkRepel;
    public float atkFly;
    public int skillEffect;
    public string skillRangePre;

    public int SpecialEffectLoop;
    public string SpecialEffect;

    public int unmatched;

    public float skillDamageInterval;

    public int nJiGuanID;
    public bool isPetAction;
}

public class DataReadSkillExpression : DataReadBase {
	
	public override string getRootNodeName() {
		return "SkillConfig";
	}
	
	public override void appendAttribute(int key, string name, string value) {

        SkillExpressionDataItem sdi;
		
		if (!data.ContainsKey(key))  
        {
            sdi = new SkillExpressionDataItem();
			data.Add(key, sdi);
        }

        sdi = (SkillExpressionDataItem)data[key];
		
		switch (name) {
		case "ID":
			sdi.id = int.Parse(value);
			break;
        case "skillEffectTarget":
			sdi.skillEffectTarget = int.Parse(value);
			break;
        case "skillAction":
            sdi.skillAction = value;
			break;
        case "skillHurtAction":
            sdi.skillHurtAction = value;
            break;
        case "skillDamageType":
            sdi.skillDamageType = (SKILL_DAMAGE_TYPE)int.Parse(value);
            break;
        case "skillRangeType":
            sdi.skillRangeType = int.Parse(value);
            break;
        case "rangeFollow":
            sdi.rangeFollow = int.Parse(value);
            break;
        case "skillAngle":
            sdi.skillAngle = int.Parse(value);
            break;
        case "Radius":
            sdi.skillRadius = float.Parse(value);
            break;
        case "collideSpeed":
            sdi.collideSpeed = float.Parse(value);
            break;
        case "collideLife":
            sdi.collideLife = float.Parse(value);
            break;
        case "moveSpeed":
            sdi.moveSpeed = float.Parse(value);
            break;
        case "atkRepel":
            sdi.atkRepel = float.Parse(value);
            break;
        case "atkFly":
            sdi.atkFly = float.Parse(value);
            break;
        case "skillEffect":
            sdi.skillEffect = int.Parse(value);
            break;
        case "skillRangePre":
            sdi.skillRangePre = value;
            break;
        case "SpecialEffectLoop":
            sdi.SpecialEffectLoop = int.Parse(value);
            break;
        case "SpecialEffect":
            sdi.SpecialEffect = value;
            break;
        case "unmatched":
            sdi.unmatched = int.Parse(value);
            break;
        case "skillDamageInterval":
            sdi.skillDamageInterval = float.Parse(value);
            break;
        case "JiguanID":
            sdi.nJiGuanID = int.Parse(value);
            break;
        case "IsPetAction":
            sdi.isPetAction = Convert.ToBoolean( int.Parse(value));
            break;
		}
	}

    public SkillExpressionDataItem getSkillExpressionData(int key)
    {
		
		if (!data.ContainsKey(key))  
        {
            SkillExpressionDataItem sdi = new SkillExpressionDataItem();
			return sdi;
        }

        return (SkillExpressionDataItem)data[key];
	}

}
