/**???????????
function: ??UI?texture,icon???
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections.Generic;

public class SourceManager
{
	private static SourceManager _instance;
	private Dictionary<string, Texture2D> _iconList; //ICON?????
	private SourceManager()
	{
		_iconList = new Dictionary<string, Texture2D>();
	}
  
    /// <summary>
    /// ??Commond????
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string getIconByType(eGoldType type)
    {
        string str = null;
        switch (type)
        {
            case eGoldType.gold:
                str = "common_gold";
                break;
            case eGoldType.zuanshi:
                str = "common_diamond";
                break;
            case eGoldType.rongyu:
                str = "common_rongyu";
                break;
            case eGoldType.fushi:
                str = "common_fushi";
                break;
            case eGoldType.exp:
                str = "common_exp";
                break;
            case eGoldType.tili:
                str = "common_diamond";
                break;
            case eGoldType.shuijing:
                str = "common_shuijing";
                break;
            default:
                break;
        }
        return str;
    }

    /// <summary>
    /// ??Iocn??????
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string getIocnStringByType(eGoldType type)
    {
        string str = null;
        switch (type)
        {
            case eGoldType.gold:
                str = "gold";
                break;
            case eGoldType.zuanshi:
                str = "diamond";
                break;
            case eGoldType.rongyu:
                str = "Resoure_rongyu";
                break;
            case eGoldType.fushi:
                str = "Resoure_fushi";
                break;
            case eGoldType.exp:
                str = "icon_exp";
                break;
            case eGoldType.tili:
                str = "Resoure_tili";
                break;
            case eGoldType.shuijing:
                str = "Resoure_shuijing";
                break;

            case eGoldType.gongxiandu:
                str = "Resoure_gongxian";
                break;
            case eGoldType.gonghuicaifu:
                str = "Resoure_gonghuicaifu";
                break;
            default:
                break;
        }
        return str;
    }


    
	
	//??icon?texture
    public Texture2D getTextByIconName(string iconName, string iconPath = PathConst.ICON_PATH)
    {
        if(!_iconList.ContainsKey(iconName))
        {
            string path = iconPath + iconName;
            Texture2D texture = BundleMemManager.Instance.loadResource(path, typeof(Texture2D)) as Texture2D;
            _iconList.Add(iconName, texture);
        }
        return _iconList[iconName];
    }
    
    //??icon?texture
    public void removeAllTexture()
    {
    	if(_iconList.Count > 0)
    	{
    		List<string> keyList = new List<string>(_iconList.Keys); 
    		foreach (string key in keyList) 
    		{
			    Texture2D texture = _iconList[key];
    			if(texture != null)
    				Resources.UnloadAsset(texture);
    			_iconList.Remove(key);
			}
    	}
    }

    public string GetLadderIconByNum(int num)
    {
        string str = string.Empty;
        switch (num)
        {
            case 1:
                str = "common_paihang1";
                break;
            case 2:
                str = "common_paihang2";
                break;
            case 3:
                str = "common_paihang3";
                break;

            default:
                str = "";
                break;
        }

        return str;

    }
    
    //getter and setter
	public static SourceManager Instance 
	{
		get 
		{ 
			if(_instance == null)
				_instance = new SourceManager();
			return _instance; 
		}
	}

    public Dictionary<string, Texture2D> IconList
    {
        get { return _iconList; }
    }
	
	public string GetCommonButton1SpriteNameByStatus (bool status)
	{
		if (status) {
			return "common_button1";
		}
		return "common_button1_hui";
	}
	
}
 
