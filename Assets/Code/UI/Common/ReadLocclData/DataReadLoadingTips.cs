using UnityEngine;
using System.Collections;

public class LoadingTipsItem
{
    public int id;
    public string tip;
}

public class DataReadLoadingTips : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }

    public override void appendAttribute(int key, string name, string value)
    {
        LoadingTipsItem di;

        if (!data.ContainsKey(key))
        {
            di = new LoadingTipsItem();
            data.Add(key, di);
        }

        di = (LoadingTipsItem)data[key];

        switch (name)
        {
            case "ID":
                di.id = int.Parse(value);
                break;
            case "Tips":
                di.tip = value;
                break;
        }
    }

    public LoadingTipsItem getTipData(int key)
    {
        if (!data.ContainsKey(key))
        {
            LoadingTipsItem temp = new LoadingTipsItem();
            return temp;
        }
        return (LoadingTipsItem)data[key];        
    }
    public int getTipSize()
    {
        return data.Count;
    }
}