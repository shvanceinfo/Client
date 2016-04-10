using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreloadEffectItem
{
    public uint		unMonsterID;
    public string 	szMonsterModel;
    public List<string> MonsetEffectsList;
	
	public PreloadEffectItem()
	{
		MonsetEffectsList = new List<string>();
	}
}

public class DataReadPreloadEffects : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }

    public override void appendAttribute(int key, string name, string value)
    {
        PreloadEffectItem di;

        if (!data.ContainsKey(key))
        {
            di = new PreloadEffectItem();
            data.Add(key, di);
        }

        di = (PreloadEffectItem)data[key];

        switch (name)
        {
            case "ID":
                di.unMonsterID = uint.Parse(value);
                break;
            case "MonsterModel":
                di.szMonsterModel = value;
                break;
            case "EffectPrefabs":
				string[] effects = value.Split(';');
                for (int i = 0; i < effects.Length; ++i)
				{
					di.MonsetEffectsList.Add(effects[i]);
				}
                break;
        }
    }
	
	public PreloadEffectItem GetPreloadEffectDataByID(int key)
	{
		if (!data.ContainsKey(key))  
        {
            PreloadEffectItem mdi = new PreloadEffectItem();
			return mdi;
        }

        return (PreloadEffectItem)data[key];
	}

    public PreloadEffectItem GetPreloadEffectData(string szModelName)
    {
       	foreach(DictionaryEntry tmp in data)
		{
			PreloadEffectItem item = tmp.Value as PreloadEffectItem;
			if (item.szMonsterModel == szModelName)
			{
				return item;
			}
		}
        return null;
    }
	
	public List<string> GetPreloadEffectList()
	{
		List<string> effectList = new List<string>();
		Dictionary<string, string> tmpMap = new Dictionary<string, string>();
		foreach(DictionaryEntry tmp in data)
		{
			PreloadEffectItem item = tmp.Value as PreloadEffectItem;
			for(int i = 0; i < item.MonsetEffectsList.Count; ++i)
			{
				if (!tmpMap.ContainsKey(item.MonsetEffectsList[i]))
				{
					tmpMap.Add(item.MonsetEffectsList[i], "0");
					effectList.Add(item.MonsetEffectsList[i]);
				}
			}
		}
		
		return effectList;
	}
}
