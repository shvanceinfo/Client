using UnityEngine;
using System.Collections;

public class SkillPointItem {
	public int nID;
	public int nPointType;
	public int nPreSkillPointID;
    public int nCostPoint;
	public int nLevelUpSkillID;
	public eFighintPropertyCate nStateType;
	public int nStateValue;
}

public class DataReadSkillPoint : DataReadBase {
	
	public override string getRootNodeName() {
        return "RECORDS";
	}
	
	public override void appendAttribute(int key, string name, string value) {
		
		SkillPointItem sdi;
		
		if (!data.ContainsKey(key))  
        {  
            sdi = new SkillPointItem();
			data.Add(key, sdi);
        }

        sdi = (SkillPointItem)data[key];
		
		switch (name) {
		case "ID":
			sdi.nID = int.Parse(value);
			break;
        case "pointType":
			sdi.nPointType = int.Parse(value);
			break;
        case "preSkillPoint":
			sdi.nPreSkillPointID = int.Parse(value);
			break;
        case "skillPointCost":
			sdi.nCostPoint = int.Parse(value);
			break;
        case "skillLevelUp":
			sdi.nLevelUpSkillID = int.Parse(value);
			break;	
		case "bStateType":
			sdi.nStateType = (eFighintPropertyCate)int.Parse(value);
			break;
		case "bStateValue":
			sdi.nStateValue = int.Parse(value);
			break;
		}
	}
	
	public SkillPointItem getSkillPointData(int key) {
		
		if (!data.ContainsKey(key))  
        {
            SkillPointItem sdi = new SkillPointItem();
			return sdi;
        }

        return (SkillPointItem)data[key];
	}
}

