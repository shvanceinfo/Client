using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MAP_TIPS_TYPE
{
    MTT_SPAWN_MONSTER,      // 刷怪
    MTT_MONSTER_CAST_SKILL, // 怪放技能
};

public class MapTipsItem
{
    public int nMapId;
    public MAP_TIPS_TYPE eTipsType;
    public string strAreaName;
    public List<int> listMonsterID = new List<int>();
    public string strSoundRes;
    public List<string> listTips = new List<string>();
}




public class DataReadMapTips : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }

    public override void appendAttribute(int key, string name, string value)
    {
        MapTipsItem di;

        if (!data.ContainsKey(key))
        {
            di = new MapTipsItem();
            data.Add(key, di);
        }

        di = (MapTipsItem)data[key];

        switch (name)
        {
            case "ID":
                di.nMapId = int.Parse(value);
                break;
            case "tipsType":
                di.eTipsType = (MAP_TIPS_TYPE)int.Parse(value);
                break;
            case "MonsterArea":
                di.strAreaName = value;
                break;
            case "monsterID":
                {
                    string[] parsedValue = ParseMultiString(value);
                    foreach (string atom in parsedValue)
                    {
                        di.listMonsterID.Add(int.Parse(atom));
                    }
                }
                break;
            case "monsterSound":
                di.strSoundRes = value;
                break;
            case "tips":
                {
                    string[] parsedValue = ParseMultiString(value);
                    foreach (string atom in parsedValue)
                    {
                        di.listTips.Add(atom);
                    }
                }
                break;
        }
    }

    public MapTipsItem getMapTipsData(int key)
    {
        if (!data.ContainsKey(key))
        {
            MapTipsItem temp = new MapTipsItem();
            return temp;
        }

        return (MapTipsItem)data[key];
    }

    private string[] ParseMultiString(string rawString)
    {
        return rawString.Split('#');
    }

    public List<string> RealShowData(int key, List<int> listIndex)
    {
        List<string> retValue = new List<string>();

        MapTipsItem itemdata = (MapTipsItem)data[key];

        for (int i = 0; i < listIndex.Count; ++i )
        {
            retValue.Add(itemdata.listTips[listIndex[i]]);
        }

        return retValue;
    }
}