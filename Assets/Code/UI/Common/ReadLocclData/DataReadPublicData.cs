using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using model;
using System;
using helper;

public enum PublicDataType
{
	type1=1,
	type2=2,
	type3=3,
	type4=4,
	type5=5,
	type6=6,
	type7=7,
	type8=8,
}

public class PublicDataItem
{
	public int id;
	public string functionDesc;
	public PublicDataType pDataType;
	public List<int> type1List = new List<int> ();
	public int type2Data;
	public string type3Data;
	public List<string> type4Data = new List<string> ();
    public List<NumString> type5Data = new List<NumString>();
    public List<StringNum> type6Data = new List<StringNum>();
	public float type7Data;
	public List<float> type8List = new List<float>();
	 
	
}


public struct NumString
{
	public int num;
	public string str;
}

public struct StringNum
{
	public string str;
	public int num;
}

public class DataReadPublicData  : DataReadBase
{
	
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
	
	public override void appendAttribute (int key, string name, string value)
	{
		
		PublicDataItem item;
		
		if (!data.ContainsKey (key)) {
			item = new PublicDataItem ();
			data.Add (key, item);
		}

		item = (PublicDataItem)data [key];
		
		switch (name) {
		case "ID":
			item.id = int.Parse (value);
			break;
		case "FunctionDesc":
			item.functionDesc = value;
			break;
		case "Type":
			item.pDataType = (PublicDataType)int.Parse (value);
				
			break;
		case "Value":
			switch (item.pDataType) {
				case PublicDataType.type1:
					string [] type1Array = value.Split (',');
					for (int i = 0; i < type1Array.Length; ++i) {
						
						item.type1List.Add(int.Parse(type1Array[i]));
					}
					break;
				case PublicDataType.type2:
					item.type2Data = int.Parse (value);
					break;
				case PublicDataType.type3:
					item.type3Data = value;
					break;	
				case PublicDataType.type4:
					string [] type4Array = value.Split (',');
					for (int i = 0; i < type4Array.Length; ++i) {
                        item.type4Data.Add(type4Array[i]);
					}
					break;	
				case PublicDataType.type5:
					string [] type5Array = value.Split (',');
					for (int i = 0; i < type5Array.Length; i+=2) {
                        NumString t5 = new NumString();
						t5.num = int.Parse (type5Array [i]);
						t5.str = type5Array [i + 1];
						item.type5Data.Add (t5);
					}
					break;
				case PublicDataType.type6:
					string [] type6Array = value.Split (',');
					for (int i = 0; i < type6Array.Length; i+=2) {
                        StringNum t6 = new StringNum();
						t6.str = type6Array [i];
						t6.num = int.Parse(type6Array [i + 1]);
						item.type6Data.Add (t6);
					}
					break;
				case PublicDataType.type7:
					item.type7Data = float.Parse (value);
					break;
				case PublicDataType.type8:
					string [] type8Array = value.Split (',');
					for (int i = 0; i < type8Array.Length; ++i)
					{
						item.type8List.Add(float.Parse(type8Array[i]));
					}
					break;
				default:
					break;
			}
				
				 
			break;
		}
	}
	
	public PublicDataItem getPublicData (int key)
	{
		
		if (!data.ContainsKey (key)) {  
			PublicDataItem item = new PublicDataItem ();
			return item;
		}
		
		return (PublicDataItem)data [key];
	}
	
	 
}


public class DataReadSettingDisplay : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        SettingVo vo;
        if (SettingManager.Instance.DisplayHash.ContainsKey(key))
        {
            vo = SettingManager.Instance.DisplayHash[key] as SettingVo;
        }
        else {
            vo = new SettingVo();
            SettingManager.Instance.DisplayHash.Add(key, vo);
        }
        switch (name)
        {
            case "ID":
                vo.Id=XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "MapID":
                vo.MapId = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "Name":
                vo.Name = value;
                break;
            case "Option":
                vo.CurOption =vo.DefaultOption= XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "DisplayNum":
                XmlHelper.CallTry(() =>
                {
                    string[] sps = value.Split(',');
                    for (int i = 0; i < sps.Length; i++)
                    {
                        vo.Options.Add(int.Parse(sps[i]));
                    }
                });
                break;
            default:
                break;
        }
    }
}


public class DataReadChannel : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        ChannelVo vo;
        if (ChannelManager.Instance.ChannelHash.ContainsKey(key))
        {
            vo = ChannelManager.Instance.ChannelHash[key] as ChannelVo;
        }
        else {
            vo = new ChannelVo();
            ChannelManager.Instance.ChannelHash.Add(key, vo);
        }


        switch (name)
        {
            case "ID":
                vo.Id = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "MapID":
                vo.MapId = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "Name":
                vo.Name = value;
                break;
            case "HotNum":
                vo.MaxPeople = XmlHelper.CallTry(() => (int.Parse(value.Split(',')[1])));
                break;
            case "LiuChangNum":
                vo.NormalPeople = XmlHelper.CallTry(() => (int.Parse(value.Split(',')[1])));
                break;
            case "FreeNum":
                vo.FreePeople = XmlHelper.CallTry(() => (int.Parse(value.Split(',')[1])));
                break;
            default:
                break;
        }
    }
}
