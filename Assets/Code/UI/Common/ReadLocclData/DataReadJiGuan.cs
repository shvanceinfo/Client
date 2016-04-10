using UnityEngine;
using System.Collections;
using model;
using manager;

// 机关类 包括技能产生的机关和场景中的机关
public class JiGuanItem 
{
    public enum EJiGuanType
    {
        JGT_SKILL_HIT = 1,  // 技能触发的机关类 在技能命中时
        JGT_SKILL_BEGIN,  // 技能触发的机关类 在技能释放时产生
        JGT_SCENE, // 静态机关 场景中的固有的
    }

	public int nID;
    public EJiGuanType eJiGuanType;
    public DAMAGE_TIMES eDamageType;
	public float fDelay;
    public float fLifeTime;
    public float fIntervalTime;
    public string strDamageValue;
    public int nBuffID;
    public string strColliderPrefab;
	public string strEffect;
}

public class DataReadJiGuan : DataReadBase
{
    public override string getRootNodeName()
    {
        return "AIConfig";
    }

    public override void appendAttribute(int key, string name, string value)
    {
        JiGuanItem item;

        if (!data.ContainsKey(key))
        {
            item = new JiGuanItem();
            data.Add(key, item);
        }

        item = (JiGuanItem)data[key];

        switch (name)
        {
            case "ID":
                item.nID = int.Parse(value);
                break;
            case "Jiguan_type":
                item.eJiGuanType = (JiGuanItem.EJiGuanType)int.Parse(value);
                break;
            case "Delay":
                item.fDelay = float.Parse(value);
                break;
            case "LifeTime":
                item.fLifeTime = float.Parse(value);
                break;
            case "Dam_type":
                item.eDamageType = (DAMAGE_TIMES)int.Parse(value);
                break;
            case "Cycle":
                item.fIntervalTime = float.Parse(value);
                break;
            case "EffectID":
                item.nBuffID = int.Parse(value);
                break;
            case "Prefab":
                item.strColliderPrefab = value;
                break;
			case "jiguan_eff":
				item.strEffect = value;
				break;
        }
    }

    public JiGuanItem GetJiGuanData(int key)
    {
        if (!data.ContainsKey(key))
        {
            JiGuanItem mdi = new JiGuanItem();
            return mdi;
        }

        return (JiGuanItem)data[key];
    }
}