using UnityEngine;
using System.Collections;

public class RoleDataItem {
	public int id;
    public int upgrade_exp;
    public int max_hp;
    public int max_mp;
    public int attack;
    public int defense;
}

public class DataReadRole : DataReadBase {
	
	public override string getRootNodeName() {
        return "RECORDS";
	}
	
	public override void appendAttribute(int key, string name, string value) {
		
		RoleDataItem rdi;
		
		if (!data.ContainsKey(key))  
        {
            rdi = new RoleDataItem();
            data.Add(key, rdi);
        }

        rdi = (RoleDataItem)data[key];
		
		switch (name) {
		    case "ID":
                rdi.id = int.Parse(value);
                break;
            case "upgradeExp":
                rdi.upgrade_exp = int.Parse(value);
                break;
            case "maxHP":
                rdi.max_hp = int.Parse(value);
                break;
            case "maxMP":
                rdi.max_mp = int.Parse(value);
                break;
            case "attack":
                rdi.attack = int.Parse(value);
                break;
            case "defense":
                rdi.defense = int.Parse(value);
                break;
		}
	}
	
	public RoleDataItem getRoleData(int key) {
		
		if (!data.ContainsKey(key))  
        {  
            RoleDataItem rdi = new RoleDataItem();
			return rdi;
        }
		
		return (RoleDataItem)data[key];
	}

}
