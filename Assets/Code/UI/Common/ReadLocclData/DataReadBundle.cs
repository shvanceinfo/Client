using System.Collections.Generic;

public class BundleItem
{
    public uint		bundleID;       //bundle的ID索引
    public string 	modelName;      //bundle对应的model名称
    public string   bundleName;     
    public CHARACTER_CAREER career;
    public EBundleType bundleType;
    public int mapID;
    public List<string> subPrefabs;  //该bundle对应的所有子Prefab
    public int bundleVersion;

    public BundleItem()
	{
        subPrefabs = new List<string>();
	}
}

public class DataReadBundle : DataReadBase
{
    public bool useInGame = true;

    public override string getRootNodeName()
    {
        return "RECORDS";
    }

    public override void appendAttribute(int key, string name, string value)
    {
        BundleItem di;
        if (!data.ContainsKey(key))
        {
            di = new BundleItem();
            data.Add(key, di);
        }
        di = (BundleItem)data[key];

        switch (name)
        {
            case "ID":
                di.bundleID = uint.Parse(value);
                break;
            case "model":
                di.modelName = ToolFunc.TrimPath(value);
                break;
            case "prefabs":
                di.bundleName = ToolFunc.TrimPath(value);
				BundleMemManager.Instance.addBundleItem(di, false);
                break;
            case "type":
                di.bundleType = (EBundleType)int.Parse(value);
                BundleMemManager.Instance.addBundleItem(di, true);
                break;
            case "career":
                di.career = (CHARACTER_CAREER) int.Parse(value);
                break;
            case "version":
                di.bundleVersion = int.Parse(value);
                break;
            case "MapID":
                di.mapID = int.Parse(value);
                if (di.mapID > 0)
                {
                    BundleMemManager.Instance.addBundleItem(di, true, true);
                }
                break;
            case "PrefabUrl":
                string[] arrStr = value.Split(';');
                if (useInGame)
                {
                    foreach (string str in arrStr)
                    {
                        di.subPrefabs.Add(ToolFunc.TrimPath(str));
                    }                   
                }
                else
                {
                    foreach (string str in arrStr)
                    {
                        di.subPrefabs.Add(str);
                    }       
                }
                break;
        }
    }
}
