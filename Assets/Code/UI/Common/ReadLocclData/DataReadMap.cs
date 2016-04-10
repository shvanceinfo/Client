using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using model;

public enum MapCate
{
	City = 0,//主城
	Single,//单人关卡
	Award,//奖励关卡
	Arena,//竞技场
	Dungeon,//多人副本
    WorldBoss,  // 世界boss
    Plot,       //剧情副本
}

public class MapDataItem
{	
	public MapDataItem ()
	{
		id = 0;
		clientNO = 0;
		sceneName = "";
		sceneNO = "";
		name = "";
		childMapID = "";
		transPoint1 = "";
		battlePref = "";
        transferMapIDs = new int[6]{0,0,0,0,0,0};
        transferPos = new BetterList<Vector3>();
	}
	
	public int id;
	public int clientNO;
	public string sceneName;
	public string sceneNO;
	public int nBossMission;
	public int nEnterLevel;
	public int nXuePing;
	public string name;
	public string childMapID;
	public string icon;
	public string selectIcon;
	public string dropItem;
	public MapCate mapCate;
	public string transPoint1;
    public int[] transferMapIDs; //传送点ID
    public BetterList<Vector3> transferPos; //传送点位置
	public string battlePref;
	public int recommondPower; //推荐战力
	public int engeryConsume; //消耗体力 
	public int limitTime; //消耗体力
	public int starTime; //获得星星的时间
	public bool sweepSwitch; //扫荡开关
	public int sweepTime; //扫荡时间
	public int sweepExp; //扫荡1次获得的经验数量
	public int sweepGold; //扫荡1次获得的金币数量
    public string mapTip;   //地图后缀
    public string mapGridName;
    public string transferBack1;
    public string transferBack2;
    public string transferBack3;
    public int dropOutId;       //掉落宝箱ID

}

public class EnergyData
{
	public int addCount;
	public int needVIPLevel;
	public int needDiamond;
	public int addEngeryNum;
}

public class DataReadMap : DataReadBase
{
    
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
	
	public override void appendAttribute (int key, string name, string value)
	{		
		MapDataItem di;		
		if (!data.ContainsKey (key)) 
		{
			di = new MapDataItem ();
			data.Add (key, di);
		}	
		di = (MapDataItem)data [key];	
		switch (name) 
		{
		    case "ID":
		    {
			    di.id = int.Parse (value);
			    MapVo vo =RaidManager.Instance.getRaidVo((uint)key);
			    if(vo != null)
			    {
				    vo.mapID = uint.Parse (value);
				    if(vo.mapID % 10 == RaidManager.NORMAL_MAP_SUFFIX)
					    vo.isHard = false;
				    else
					    vo.isHard = true;
			    }
		    }
			break;
		    case "clientNO":
			    di.clientNO = int.Parse (value);
			break;
		    case "sceneName":
			    di.sceneName = value;
			break;
		    case "sceneNO":
			    di.sceneNO = value;
			break;
		    case "mapName":
		    {
			    di.name = value;
			    MapVo vo = RaidManager.Instance.getRaidVo((uint)key);
			    if(vo != null)
				    vo.gateName = value;
		    }   
			break;
		    case "childMapIDlist":
			    di.childMapID = value;
			break;
		    case "mapicon1":
			    di.icon = value;
			break;
            case "mapicon2":
            di.selectIcon = value;
			break;
            case "transferPoint1":
            di.transPoint1 = value;
			break;
		    case "transferPoint2":
			    di.transferMapIDs[0] = int.Parse(value);
			break;
		    case "transferPoint3":
                di.transferMapIDs[1] = int.Parse(value);
			break;
            case "transferPoint4":
                di.transferMapIDs[2] = int.Parse(value);
			break;
            case "transferPoint5":
                di.transferMapIDs[3] = int.Parse(value);
			break;
		    case "dropItem":
			    di.dropItem = value;
			break;
            case "battlePref":
                di.battlePref = value;
			break;
            case "BossMission":
                di.nBossMission = int.Parse(value);
			break;
            case "mapLevel":
                di.nEnterLevel = int.Parse(value);
			break;
            case "xueping":
                di.nXuePing = int.Parse(value);
			break;
            case "tuijian_ZL":
                di.recommondPower = int.Parse(value);
			break;
		    case "xiaohao_TL":
			    di.engeryConsume = int.Parse (value);
			break;
		    case "xianding_time":
			    di.limitTime = int.Parse (value);
			break;
		    case "star_time":
			    di.starTime = int.Parse (value);
			break;
		    case "saodang_switch":
			    int intValue = int .Parse (value);
			    if (intValue == 0)
				    di.sweepSwitch = false;
			    else
				    di.sweepSwitch = true;
			break;
		    case "saodang_time":
			    di.sweepTime = int.Parse (value);
			break;
		    case "SD_EXP":
			    di.sweepExp = int.Parse (value);
			break;
		    case "SD_gold":
			    di.sweepGold = int.Parse (value);
			break;
		    case "mapCate":
			    di.mapCate = (MapCate)int.Parse(value);
			break;
            case "moshimingcheng":
                di.mapTip = value;
            break;
            case "zhangjie":
            {
                MapVo vo = RaidManager.Instance.getRaidVo((uint)key);
                if (vo != null)
                    vo.whichChapter = int.Parse(value);
            }
			break;
		    case "guanka_icon":
		    {
			    MapVo vo = RaidManager.Instance.getRaidVo((uint)key);
			    if(vo != null)
				    vo.gateIcon = value;
		    }
			break;
		    case "xianshi_XYZ":
		    {
			    MapVo vo = RaidManager.Instance.getRaidVo((uint)key);
			    if(vo != null)
			    {
				    string[] splits = null;
				    char[] charSeparators = new char[] {','};
				    splits = value.Split (charSeparators);
				    vo.gatePos = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), 0f);
			    }
		    }
		    break;
            case "transferPos2":
		    {
                string[] splits = null;
                char[] charSeparators = new char[] { ',' };
                splits = value.Split(charSeparators);
                di.transferPos.Add(new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2])));
		    }
			break;
            case "transferPos3":
            {
                string[] splits = null;
                char[] charSeparators = new char[] { ',' };
                splits = value.Split(charSeparators);
                di.transferPos.Add(new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2])));
            }
            break;
            case "transferPos4":
            {
                string[] splits = null;
                char[] charSeparators = new char[] { ',' };
                splits = value.Split(charSeparators);
                di.transferPos.Add(new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2])));
            }
            break;
            case "transferPos5":
            {
                string[] splits = null;
                char[] charSeparators = new char[] { ',' };
                splits = value.Split(charSeparators);
                di.transferPos.Add(new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2])));
            }
            break;
            case "map_xunlu":
            {
                di.mapGridName = value;
            }
            break;
            case "transferBack1":
            {
                di.transferBack1 = value;
            }
            break;
            case "transferBack2":
            {
                di.transferBack2 = value;
            }
            break;
            case "transferBack3":
            {
                di.transferBack3 = value;
            }
            break;
            case "dropTreaureBox1ID":
            di.dropOutId = int.Parse(value);
            break;
		}
		
	}
	
	public MapDataItem getMapData (int key)
	{
		
		if (!data.ContainsKey (key)) {  
			MapDataItem di = new MapDataItem ();
			return di;
		}	

        MapDataItem item = (MapDataItem)data [key];
        return (MapDataItem)data[key];
	}

	public List<int> getPrevousMaps (int maxId)
	{
		List<int> preMaps = new List<int> ();
		foreach (DictionaryEntry item in data) {
			if (((MapDataItem)item.Value).id <= maxId) {
				preMaps.Add (((MapDataItem)item.Value).id);
			}
		}
		return preMaps;
	}
	
	public ICollection Keys 
	{
		get {
			return this.data.Keys;
		}
	} 
	
	//得到数量
	public int getCount ()
	{
		return this.data.Count;
	}
	
}

public class DataReadEngery : DataReadBase
{    
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
	
	public override void appendAttribute (int key, string name, string value)
	{		
		EnergyData dataEnergy;
		
		if (!SweepManager.Instance.EnergyHash.ContainsKey (key)) {  
			dataEnergy = new EnergyData ();
			SweepManager.Instance.EnergyHash.Add (key, dataEnergy);
		}	
		dataEnergy = (EnergyData)SweepManager.Instance.EnergyHash [key];	
		switch (name) {
		case "ID":
			dataEnergy.addCount = int.Parse (value);
			break;
		case "VIP":
			dataEnergy.needVIPLevel = int.Parse (value);
			break;
		case "dia_price":
			dataEnergy.needDiamond = int.Parse (value);
			break;
		case "tili_num":
			dataEnergy.addEngeryNum = int.Parse (value);
			break;
		default:
			break;
		}
	}
}

public class DataReadDropOut : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        DropOutVo vo;
        if (DropOutManager.Instance.DropHash.ContainsKey(key))
        {
            vo = DropOutManager.Instance.DropHash[key] as DropOutVo;
        }
        else {
            vo = new DropOutVo();
            DropOutManager.Instance.DropHash.Add(key, vo);
        }
        switch (name)
        {
            case "id":
                vo.Id = int.Parse(value);
                break;
            case "dropSiliver":
                vo.Gold = int.Parse(value);
                break;
            default:
                break;
        }
    }
}
