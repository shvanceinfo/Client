using UnityEngine;
using System.Collections;

public class SkillAppearDataItem {
	public int id;
}

public class DataReadSkillAppear : DataReadBase {
	
	public override string getRootNodeName() {
        return "RECORDS";
	}
	
	public override void appendAttribute(int key, string name, string value) {

        SkillAppearDataItem rdi;
		
		if (!data.ContainsKey(key))  
        {
            rdi = new SkillAppearDataItem();
            data.Add(key, rdi);
        }

        rdi = (SkillAppearDataItem)data[key];
		
		switch (name) {
		    case "ID":
                rdi.id = int.Parse(value);
                break;
		}
	}

    public SkillAppearDataItem getSkillAppearData(int key)
    {
		
		if (!data.ContainsKey(key))  
        {
            SkillAppearDataItem rdi = new SkillAppearDataItem();
			return rdi;
        }

        return (SkillAppearDataItem)data[key];
	}

}
