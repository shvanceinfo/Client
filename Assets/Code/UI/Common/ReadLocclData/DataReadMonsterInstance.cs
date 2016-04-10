using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterInstanceDataItem {
	public int ID;
    public string pref;
    public string area;
    public int templateID;
    public int mapID;
}

public class DataReadMonsterInstance : DataReadBase {


    public Dictionary<int, List<int> > m_kTemplateIDToInstanceID = new Dictionary<int, List<int> >();
	
	//一张地图对应的怪物模板ID，ID不重复
	public Dictionary<int, List<int> > m_MapTemplateIDList = new Dictionary<int, List<int> >();

	public override string getRootNodeName() {
        return "RECORDS";
	}
	
	public override void appendAttribute(int key, string name, string value) {

        MonsterInstanceDataItem mdi;
		
		if (!data.ContainsKey(key))  
        {
            mdi = new MonsterInstanceDataItem();
			data.Add(key, mdi);
        }

        mdi = (MonsterInstanceDataItem)data[key];
		
		switch (name) {
		case "ID":
			mdi.ID = int.Parse(value);
			break;
		case "pref":
			mdi.pref = value;
			break;
        case "area":
            mdi.area = value;
            break;
        case "templateID":
			mdi.templateID = int.Parse(value);
			break;
        case "mapID":
            mdi.mapID = int.Parse(value);
            break;
		}
	}

    public MonsterInstanceDataItem getMonsterData(int key)
    {
		if (!data.ContainsKey(key))  
        {
            MonsterInstanceDataItem mdi = new MonsterInstanceDataItem();
			return mdi;
        }

        return (MonsterInstanceDataItem)data[key];
	}


    public void ParseData()
    {

        //foreach (DictionaryEntry item in data)
        //{
        //    fun((MissionItem)item.Value);
        //}


        foreach (DictionaryEntry item in data)
        {
            int nTemplateID = ((MonsterInstanceDataItem)item.Value).templateID;
            int nInstanceID = ((MonsterInstanceDataItem)item.Value).ID;

            if (!m_kTemplateIDToInstanceID.ContainsKey(nTemplateID))
            {
                List<int> instanceList = new List<int>();
                instanceList.Add(nInstanceID);
                m_kTemplateIDToInstanceID.Add(nTemplateID, instanceList);
            }
            else
            {
                m_kTemplateIDToInstanceID[nTemplateID].Add(nInstanceID);
            }
        }
    }
	
	public void ParseMapData()
	{
		foreach (DictionaryEntry item in data)
        {
            int nTemplateID = ((MonsterInstanceDataItem)item.Value).templateID;
			int nMapID = ((MonsterInstanceDataItem)item.Value).mapID;
			
            if (!m_MapTemplateIDList.ContainsKey(nMapID))
            {
                List<int> templateList = new List<int>();
                templateList.Add(nTemplateID);
                m_MapTemplateIDList.Add(nMapID, templateList);
            }
            else
            {
				int nCount = m_MapTemplateIDList[nMapID].Count;
				bool bContainID = false;
				for (int i = 0; i < nCount; ++i)
				{
					if (m_MapTemplateIDList[nMapID][i] == nTemplateID)
					{
						bContainID = true;
						break;
					}
				}
				if (!bContainID)
				{
					m_MapTemplateIDList[nMapID].Add(nTemplateID);
				}
            }
        }
	}

    public List<int> GetMonsterInstanceID(int key, int mapid, string area)
    {
        List<int> retData = new List<int>();

        if (m_kTemplateIDToInstanceID.ContainsKey(key))
        {
            for (int i = 0; i < m_kTemplateIDToInstanceID[key].Count; ++i )
            {
                MonsterInstanceDataItem keyData = (MonsterInstanceDataItem)data[m_kTemplateIDToInstanceID[key][i]];

                if (keyData.mapID == mapid && keyData.area == area)
                {
                    retData.Add(m_kTemplateIDToInstanceID[key][i]);
                }
            }
        }

        return retData;
    }


    public List<int> GetMonsterInstanceIDInTower(int key, int mapid)
    {
        List<int> retData = new List<int>();

        if (m_kTemplateIDToInstanceID.ContainsKey(key))
        {
            for (int i = 0; i < m_kTemplateIDToInstanceID[key].Count; ++i)
            {
                MonsterInstanceDataItem keyData = (MonsterInstanceDataItem)data[m_kTemplateIDToInstanceID[key][i]];

                if (keyData.mapID == mapid)
                {
                    retData.Add(m_kTemplateIDToInstanceID[key][i]);
                }
            }
        }

        return retData;
    }
}
