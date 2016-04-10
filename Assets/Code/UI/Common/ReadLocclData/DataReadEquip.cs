using UnityEngine;
using System.Collections;

public class EquipDataItem {
	
	public EquipDataItem()
    {
		id = 0;
		name = "";
		part = 0;
		texture_name = "";
		texture_female_name = "";
		model_name = "";
		effect_name = "";
		weapon_trail_texture = "";
		font_color = "";
		att_addition = 0;
		def_addition = 0;
		icon = "";
        saleSiliver = 0;
        quality = 1;
        quality_name = "";
        equip_level = 0;
        skill_id = "";
    }
	
	public int id;
	public string name;
	public int part;
	public string texture_name;
	public string texture_female_name;
	public string model_name;
	public string effect_name;
	public string weapon_trail_texture;
    public string font_color;
    public int att_addition;
    public int def_addition;
    public string icon;
    public Global.eGoodsType good_type;
    public int saleSiliver;
    public int quality;
    public string quality_name;
    public int equip_level;
    public string skill_id;
}

public class DataReadEquip : DataReadBase {
	public override string getRootNodeName() {
		return "RECORDS";
	}
	
	public override void appendAttribute(int key, string name, string value) {
		
		EquipDataItem di;
		
		if (!data.ContainsKey(key))  
        {  
            di = new EquipDataItem();
			data.Add(key, di);
        }
		
		di = (EquipDataItem)data[key];
		
		switch (name) {
		case "id":
			di.id = int.Parse(value);
			break;
		case "name":
			di.name = value;
			break;
		case "part":
			di.part = int.Parse(value);
			break;
		case "texture":
			di.texture_name = value;
			break;
		case "texture_female":
			di.texture_female_name = value;
			break;
		case "model":
			di.model_name = value;
			break;
		case "effect":
			di.effect_name = value;
			break;
		case "trail":
			di.weapon_trail_texture = value;
			break;
        case "color":
            di.font_color = value;
            break;
        case "attack":
            di.att_addition = int.Parse(value);
            break;
        case "defense":
            di.def_addition = int.Parse(value);
            break;
        case "icon":
            di.icon = value;
            break;
         case "category":
            di.good_type = (Global.eGoodsType)int.Parse(value);
            break;
        case "saleSiliver":
            di.saleSiliver = int.Parse(value);
            break;
        case "quality":
            di.quality = int.Parse(value);
            break;
        case "qualityLabel":
            di.quality_name = value;
            break;
        case "equiplevel":
            di.equip_level = int.Parse(value);
            break;
        case "skillID":
            di.skill_id = value.Trim();
            break;
		}
	}
	
	public EquipDataItem getEquipData(int key) {
		
		if (!data.ContainsKey(key))  
        {
            EquipDataItem di = new EquipDataItem();
            return di;        
        }
		
		return (EquipDataItem)data[key];
	}

}

/// <summary>
/// 物品表
/// </summary>
public class DataReadGoods : DataReadBase
{

    public override string getRootNodeName()
    {
        return "RECORDS";
    }

    public override void appendAttribute(int key, string name, string value)
    {

        EquipDataItem di;

        if (!data.ContainsKey(key))
        {
            di = new EquipDataItem();
            data.Add(key, di);
        }

        di = (EquipDataItem)data[key];
        di.quality = 0;
        di.quality_name = "";
        di.skill_id = "";
        switch (name)
        {
            case "id":
                di.id = int.Parse(value);
                break;
            case "name":
                di.name = value;
                break;
            case "part":
                di.part = int.Parse(value);
                break;
            case "texture":
                di.texture_name = value;
                break;
            case "model":
                di.model_name = value;
                break;
            case "effect":
                di.effect_name = value;
                break;
            case "trail":
                di.weapon_trail_texture = value;
                break;
            case "color":
                di.font_color = value;
                break;
            case "attack":
                di.att_addition = int.Parse(value);
                break;
            case "defense":
                di.def_addition = int.Parse(value);
                break;
            case "icon":
                di.icon = value;
                break;
            case "category":
                di.good_type = (Global.eGoodsType)int.Parse(value);
                break;
            case "saleSiliver":
                di.saleSiliver = int.Parse(value);
                break;
        }
    }

    public EquipDataItem getEquipData(int key)
    {

        if (!data.ContainsKey(key))
        {
            EquipDataItem di = new EquipDataItem();
            return di;
        }

        return (EquipDataItem)data[key];
    }

}
