using UnityEngine;
using System.Collections;

public enum BUFF_TYPE
{
    BT_NONE = 0,
    BT_ONE_KILL,        // 必杀
    BT_GREED,              // 贪婪
    BT_MEAN,            // 吝啬
    BT_RAMPAGE,         // 暴走
    BT_ATTACK,      // 加攻
    BT_DEFENCE,    // 加防
    BT_XUANYUN,     // 眩晕
    BT_FREEZE,          // 冰冻
    BT_SPEED_DOWN,  // 减速
    BT_FIRE_HURT,        // 残废
    BT_POISON_HURT,       // 中毒
    BT_THUNDER_HURT,         // 雷击
}

public enum BUFF_DAMAGE_TYPE
{
    BDT_NONE = 0,
    BDT_FIRE,               // 火伤害
    BDT_POISION,         // 毒伤害
    BDT_THUNDER,        // 雷伤害
}

public class SkillEffectItem 
{
	public int id;
    public string name;
    public float lastTime;
    public BUFF_TYPE buffType;
    public string buffIcon;
    public eFighintPropertyCate eType;
    public int baseData;
    public BUFF_DAMAGE_TYPE damageType;
    public int damageValue;
    public float speedRate;
    public int limitAttack;
    public string effectPre;
    public string triggerEffectPre;
    public int effPos;
}

public class DataReadSkillEffect : DataReadBase {
	
	public override string getRootNodeName() {
		return "SkillConfig";
	}
	
	public override void appendAttribute(int key, string name, string value) {

        SkillEffectItem sdi;
		
		if (!data.ContainsKey(key))  
        {
            sdi = new SkillEffectItem();
			data.Add(key, sdi);
        }

        sdi = (SkillEffectItem)data[key];
		
		switch (name) {
		case "ID":
			sdi.id = int.Parse(value);
			break;
		case "name":
			sdi.name = value;
			break;
        case "lastTime":
            sdi.lastTime = float.Parse(value);
            break;
        case "buffType":
            sdi.buffType = (BUFF_TYPE)int.Parse(value);
            break;
        case "buffIcon":
            sdi.buffIcon = value;
            break;
        case "AddStateType":
            sdi.eType = (eFighintPropertyCate)int.Parse(value);
			break;
        case "AddStateValue":
            sdi.baseData = int.Parse(value);
            break;
        case "AddDamageType":
            sdi.damageType = (BUFF_DAMAGE_TYPE)int.Parse(value);
            break;
        case "AddDamageValue":
            sdi.damageValue = int.Parse(value);
            break;
        case "speedRate":
            sdi.speedRate = float.Parse(value);
            break;
        case "limitAttack":
            sdi.limitAttack = int.Parse(value);
            break;
        case "effectPre":
            sdi.effectPre = value;
            break;
        case "triggerEffectPre":
            sdi.triggerEffectPre = value;
            break;
        case "effPos":
            sdi.effPos = int.Parse(value);
            break;
		}
	}

    public SkillEffectItem getSkillEffectData(int key)
    {
		
		if (!data.ContainsKey(key))  
        {
            SkillEffectItem sdi = new SkillEffectItem();
			return sdi;
        }

        return (SkillEffectItem)data[key];
	}

}
