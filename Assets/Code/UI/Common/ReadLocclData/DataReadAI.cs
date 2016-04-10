using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum AIBehaviouType
{
    AI_BT_NONE = 0,
    AI_BT_CAST_SKILL = 1,
    AI_BT_CAST_FILL_HP,
    AI_BT_CAST_FILL_MP,
    AI_BT_CAST_IDLE,
}

public class AIDataItem 
{
	public int id;
	public string name = "";
    public string desName = "";
    public AIBehaviouType type;
    public int AIValue;
    public float distanceConstraint;
    public float HPConstraintUp;
    public float HPConstraintDown;
    public float MPConstraintUp;
    public float MPConstraintDown;
}

public class DataReadAI : DataReadBase 
{
	
	public override string getRootNodeName() {
		return "AIConfig";
	}
	
	public override void appendAttribute(int key, string name, string value) {
		
		AIDataItem mdi;
		
		if (!data.ContainsKey(key))  
        {
            mdi = new AIDataItem();
			data.Add(key, mdi);
        }

        mdi = (AIDataItem)data[key];
		
		switch (name) 
        {
		case "ID":
			mdi.id = int.Parse(value);
			break;
		case "name":
			mdi.name = value;
			break;
        case "type":
            mdi.type = (AIBehaviouType)int.Parse(value);
            break;
        case "value":
            mdi.AIValue = int.Parse(value);
			break;
        case "distanceConstraint":
            mdi.distanceConstraint = float.Parse(value);
            break;
        case "HPConstraintUp":
            mdi.HPConstraintUp = float.Parse(value);
            break;
        case "HPConstraintDown":
            mdi.HPConstraintDown = float.Parse(value);
            break;
        case "MPConstraintUp":
            mdi.MPConstraintUp = float.Parse(value);
            break;
        case "MPConstraintDown":
            mdi.MPConstraintDown = float.Parse(value);
            break;
		}
	}

    public AIDataItem getAIData(int key)
    {
		
		if (!data.ContainsKey(key))  
        {
            AIDataItem mdi = new AIDataItem();
			return mdi;
        }

        return (AIDataItem)data[key];
	}

    public List<AIDataItem> GetAIList(bool IsPlayer, CHARACTER_CAREER career)
    {
        List<AIDataItem> retData = new List<AIDataItem>();

        foreach (DictionaryEntry item in data)
        {
            int nID = ((AIDataItem)item.Value).id;

            int nLength = nID.ToString().Length;

            int nModuleNum = 1;
            for (int i = 0; i < nLength - 1; ++i)
            {
                nModuleNum *= 10;
            }

            
            int nHightNum = nID / nModuleNum;
            if (nHightNum == 1)
            {
                // 怪
                if (!IsPlayer)
                {
                    retData.Add((AIDataItem)data[nID]);
                }
                
            }
            else if (nHightNum == 2)
            {
                // 角色
                if (IsPlayer)
                {
                    int nRemainNum = nID % nModuleNum;
                    nLength = nRemainNum.ToString().Length;

                    nModuleNum = 1;

                    for (int i = 0; i < nLength - 1; ++i)
                    {
                        nModuleNum *= 10;
                    }

                    nHightNum = nRemainNum / nModuleNum;

                    if (career == (CHARACTER_CAREER)nHightNum)
                    {
                        retData.Add((AIDataItem)data[nID]);
                    }
                }
            }
        }

        return retData;
    }
}