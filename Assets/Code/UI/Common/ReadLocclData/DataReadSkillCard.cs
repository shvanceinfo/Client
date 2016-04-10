using UnityEngine;
using System.Collections;

public class SkillCardDataItem {
	public int id;
	public string name;
    public string describe;
	public float cate;
    public int point_consume;
    public string icon;
}

public class DataReadSkillCard : DataReadBase {
	
	public override string getRootNodeName() {
        return "SkillCardConfig";
	}
	
	public override void appendAttribute(int key, string name, string value) {
		
		SkillCardDataItem sdi;
		
		if (!data.ContainsKey(key))  
        {  
            sdi = new SkillCardDataItem();
			data.Add(key, sdi);
        }

        sdi = (SkillCardDataItem)data[key];
		
		switch (name) {
		case "ID":
			sdi.id = int.Parse(value);
			break;
        case "szCardName":
			sdi.name = value;
			break;
        case "skillDescribe":
			sdi.describe = value;
			break;
        case "nCardCate":
			sdi.cate = int.Parse(value);
			break;
        case "nSkillPointConsume":
			sdi.point_consume = int.Parse(value);
			break;
        case "skillIcon":
            sdi.icon = value;
            break;	
		}
	}
	
	public SkillCardDataItem getSkillCardData(int key) {
		
		if (!data.ContainsKey(key))  
        {
            SkillCardDataItem sdi = new SkillCardDataItem();
			return sdi;
        }

        return (SkillCardDataItem)data[key];
	}

}
