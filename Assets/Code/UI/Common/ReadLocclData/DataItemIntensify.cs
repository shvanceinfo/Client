using UnityEngine;
using System.Collections;

public class IntensifyData {

    public IntensifyData()
    {
        level = 0;
        blastRate = 0;
        addRate = 0;
        goldRate = 0;
    }
    public int level;
    public int blastRate;
    public int addRate;
    public int goldRate;
    public string name;
}

public class DataReadIntensify : DataReadBase {
    
	public override string getRootNodeName() {
		return "RECORDS";
	}
	
	public override void appendAttribute(int key, string name, string value) {

        IntensifyData di;
		
		if (!data.ContainsKey(key))  
        {
            di = new IntensifyData();
			data.Add(key, di);
        }

        di = (IntensifyData)data[key];
		
		switch (name) {
		case "ID":
			di.level = int.Parse(value);
			break;
        case "intensifyBlastRate":
			di.blastRate = int.Parse(value);
            break;
        case "propertyAddRate":
            di.addRate = int.Parse(value);
            break;
        case "silverConsumeRate":
            di.goldRate = int.Parse(value);
            break;
        case "intensifyName":
            di.name = value;
            break;
		}
	}

    public IntensifyData getIntensifyData(int key)
    {
		
		if (!data.ContainsKey(key))  
        {
            IntensifyData di = new IntensifyData();
			return di;
        }

        return (IntensifyData)data[key];
	}

}
