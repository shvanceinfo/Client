using System.Collections;
using System.Text;
using System;
using UnityEngine;
using System.Security.Cryptography;
using System.Collections.Generic;

public class Global {	
	public static string enterScensBattlePref = "";
	
	public static bool firstPreload = true;
    /// <summary>
    /// 帧率
    /// </summary>
    public static readonly int FrameRate = 30;
    /// <summary>
    /// 刷新技能卡金额
    /// </summary>
    public static readonly int refreshSkillCardMoney = 10;

    /// <summary>
    /// 药品冷却时间(秒)
    /// </summary>
    public static readonly float drupCoolDownTime = 3f;
    /// <summary>
    /// 缓存文件名称
    /// </summary>
    public static readonly string CACHE_USER_INFO_FILE = "UserInfo.txt";
    /// <summary>
    /// 临时存放，用户加密过的密码
    /// </summary>
    public static string userInputPassword = "";
    /// <summary>
    /// 资源服务器地址
    /// </summary>
    public static string resourceUrl = "http://192.168.1.81/3d/";
	
	public static eFightLevel current_fight_level;
	public static uint cur_TowerId = 1;
    public static bool m_bAutoFight = false;

    public static bool m_bAutoFightSaved = false;

    

    public static float m_fCamDistCityH = 3.5f;
    public static float m_fCamDistCityV = 5.5f;
    public static float m_fCamDistFightH = 4.6f;
    public static float m_fCamDistFightV = 5.5f;

    public static bool m_bCameraCruise = false;

    public static bool m_bInGame = false;
    /// <summary>
    /// 物品类型
    /// </summary>
    public enum eGoodsType
    {
        Good_Equip = 1,     //装备
        Good_Drug = 2       //药品
    };

    /// <summary>
    /// 挑战等级
    /// </summary>
    public enum eFightLevel
    {
        Fight_Level1 = 0,	//在副本中的标记
        Fight_Level2 = 1,	//在恶魔洞窟设置这个标记
        Fight_Level3 = 2	//没有使用
    };
    

    public static MapDataItem lastFightMap = new MapDataItem();

    /// 编码转换-------开始
    public static string EncodingConvert(string fromString, Encoding fromEncoding, Encoding toEncoding)
    {
        byte[] fromBytes = fromEncoding.GetBytes(fromString);
        byte[] toBytes = Encoding.Convert(fromEncoding, toEncoding, fromBytes);

        string toString = toEncoding.GetString(toBytes);
        return toString;
    }

    public static string GB2312ToUtf8(string gb2312String)
    {
        Encoding Encoding = Encoding.GetEncoding("gb2312");
        Encoding toEncoding = Encoding.UTF8;
        return EncodingConvert(gb2312String, Encoding, toEncoding);
    }

    public static string Utf8ToGB2312(string utf8String)
    {
        Encoding Encoding = Encoding.UTF8;
        Encoding toEncoding = Encoding.GetEncoding("gb2312");
        return EncodingConvert(utf8String, Encoding, toEncoding);
    }
	
	public static string UnicodeToUtf8(string unicodeString)
    {
        Encoding Encoding = Encoding.GetEncoding("unicode");
        Encoding toEncoding = Encoding.UTF8;
        return EncodingConvert(unicodeString, Encoding, toEncoding);
    }

    public static string Utf8ToUnicode(string utf8String)
    {
        Encoding Encoding = Encoding.UTF8;
        Encoding toEncoding = Encoding.GetEncoding("unicode");
        return EncodingConvert(utf8String, Encoding, toEncoding);
    }

    ///编码转换------------------完

    /// <summary>
    /// 获取设备唯一标识
    /// </summary>
    /// <returns></returns>
    public static string GetDeviceIdentifier()
    {
		return "";
#if UNITY_IPHONE
        return iPhoneSettings.uniqueIdentifier;
#endif
        return SystemInfo.deviceUniqueIdentifier;
    }
    /// <summary>
    /// 获取MD5
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetMd5Hash(string str)
    {

        MD5 md5Hasher = MD5.Create();
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i ++ )
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }


    ////////////////////////////////////////////////战斗相关
    /// <summary>
    /// 是否处于战斗场景
    /// </summary>
    /// <returns></returns>
    /// // 单人关卡
    public static bool inFightMap()
    {
        int mapType = MessageManager.Instance.my_property.getServerMapID() / Constant.SCENE_JUDGE_PARAM;
        if (mapType != 2)
            return false;
        return true;
    }
	
	public static bool inCityMap()
    {
        int mapType = MessageManager.Instance.my_property.getServerMapID() / Constant.SCENE_JUDGE_PARAM;
        if (mapType != 1)
            return false;
        return true;
    }
	
	public static bool inTowerMap()
    {
        int mapId = MessageManager.Instance.my_property.getServerMapID();
        if (mapId != Constant.SCENE_DEVIL_WAVE)
            return false;
        return true;
    }

    public static bool inGoldenGoblin()
    {
        int mapId = MessageManager.Instance.my_property.getServerMapID();
        if (mapId != Constant.SCENE_GOLDEN_GOBLIN)
            return false;
        return true;
    }

    // 竞技场
    public static bool InArena()
    {
        if (MessageManager.Instance.my_property == null)
        {
            return false;
        }
        
        int mapId = MessageManager.Instance.my_property.getServerMapID();
        if (mapId != Constant.SCENE_ARENA)
        {
            return false;
        }

        return true;
    }

    // 多人副本
	public static bool inMultiFightMap()
    {
        int mapId = MessageManager.Instance.my_property.getServerMapID();

        MapDataItem mapData = ConfigDataManager.GetInstance().getMapConfig().getMapData(mapId);

		if (mapData.mapCate == MapCate.Dungeon) {
			return true; 
		}
        else {
        	return false;
        }
    }
    
    // 世界boss
    public static bool InWorldBossMap()
    {
        int mapId = MessageManager.Instance.my_property.getServerMapID();

        MapDataItem mapData = ConfigDataManager.GetInstance().getMapConfig().getMapData(mapId);

        if (mapData.mapCate == MapCate.WorldBoss)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
	
	//魔物悬赏
	public static bool InAwardMap(){
		int mapId = MessageManager.Instance.my_property.getServerMapID();
		
		MapDataItem mapData = ConfigDataManager.GetInstance().getMapConfig().getMapData(mapId);
		
		if (mapData.mapCate == MapCate.Award)
		{
			return true;
		}
		else
		{
			return false; 
		}
	}
	
    /// <summary>
    /// 最后一次进入场景金钱
    /// </summary>
    public static int fightLastMoney;
    /// <summary>
    /// 复活所需钻石
    /// </summary>
    public static int bornBaseDiamond = 10;
    /// <summary>
    /// 请求复活次数
    /// </summary>
    public static int requestBornNum = 1;
    /// <summary>
    /// 本次复活所需钻石
    /// </summary>
    /// <returns></returns>
    public static int GetBornDiamond()
    {
        int value = bornBaseDiamond + (requestBornNum - 1) * 5;
        return value;
    }
    /// <summary>
    /// 重置复活次数
    /// </summary>
    public static void ResetBornData()
    {
        requestBornNum = 1;
    }
    /// <summary>
    /// 最后一次选择的挑战等级
    /// </summary>
    public static eFightLevel LastSelectFightLevel = eFightLevel.Fight_Level1;
    //////////////////////////////////////////////////
	/// <summary>
	/// 传送点ID
	/// </summary>
	public static int transportId = 0;
	
	public static bool move_lock = false;
	public static bool pressOnUI() 
	{
		if (UICamera.hoveredObject != null || UICamera.selectedObject != null)
        {
            return true;
        }
		
		return false;
	}

    ///////////////////////////////////////unix时间戳////////////////////////////////////////////
    public static uint ToUnixTimeStamp(DateTime time)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970,1,1));
        DateTime dtNow = DateTime.Parse(time.ToString());
        TimeSpan toNow = dtNow.Subtract(dtStart);
        string timeStamp = toNow.Ticks.ToString();
        timeStamp = timeStamp.Substring(0,timeStamp.Length - 7);
        return uint.Parse(timeStamp);
    }

    public static DateTime FromUnixTimeStamp(string timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        DateTime dtResult = dtStart.Add(toNow);
        return dtResult;
    }

    public static void FormatTimeSec(uint timeSec, ref uint min, ref uint sec)
    {
        min = timeSec / 60;
        sec = timeSec % 60;
    }
    ////////////////////////////////////unix时间戳（完）/////////////////////////

    ////////////////////////字符串处理////////////////////
    public static string FormatStrimg(object str, params object[] value)
    {
        string msg = string.Format(str.ToString(), value);
        return msg;
    }

    public static string FromNetString(char[] chr)
    {
        string str = Utf8ToUnicode(new string(chr));
        str = str.Replace('\0', ' ');
        return str.Trim();
    }
}
