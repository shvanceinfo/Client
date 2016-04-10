/**该文件实现的基本功能等
function:实现场景的管理切换，内存的释放与增添
author:ljx
date:
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum SCENE_POS
{
	NO_IN_SCENE = 0, //还不在场景中
	IN_CITY, //在主城
	IN_COMMON_RAID, //在普通副本
	IN_TIME_RAID,	//在计时副本
	IN_DEVIL,		//在恶魔洞窟
	IN_GEBULIN,		//在哥布林
	IN_ARENA		//在竞技场中
}

public class SceneManager
{		
	private static SceneManager _instance = null;
	public SCENE_POS currentScene = SCENE_POS.NO_IN_SCENE; //当前场景
	public GameObject mapPrefab;
	public bool clearSceneOver; //是否清除完脚本	
	public int sceneID = 0;
	public int nextMapID = 0; //下个场景的地图
	public int lastMapID = 0; //上个场景的地图
	
	private int _currentMapID = 0; 	//场景中的对应的地图
	private int _currentFreeSize; 	//当前场景要释放的长度
	private int _currentFreeCount;  //当前要释放的索引
	private int[] _widgetIndexs; 	//释放控件的索引数组
	private int[] _widgetLens; 	//释放控件的长度数组
	private BetterList<GameObject> dontDestroyObjs;
	
	
	public static SceneManager Instance
	{
		get 
		{ 
			if(_instance == null)
				_instance = new SceneManager();
			return _instance; 
		}
	}

    private SceneManager()
    {
        dontDestroyObjs = new BetterList<GameObject>();
        _widgetIndexs = new int[Constant.FREE_ATLAS_SIZE];
        _widgetLens = new int[Constant.FREE_ATLAS_SIZE];
        _currentFreeCount = 0;
        _currentFreeSize = 0;
        _instance = this;
    }
	
	//不删除资源
	public void setDontDestroyObj(GameObject obj)
	{
		if(!dontDestroyObjs.Contains(obj))
		{
			dontDestroyObjs.Add(obj);
		}
		GameObject.DontDestroyOnLoad(obj);
	}
	
	//删除dontDestroyOnload的资源
	public void destroyDontDestroy()
	{
		foreach (GameObject obj in dontDestroyObjs) 
		{
            GameObject.Destroy(obj);
		}
	}
	
	
	//切换场景
	public void changeScene()
	{
		if(nextMapID > 0)
		{
			if(_currentMapID != nextMapID) //切换场景地图必然出现loading条
			{
				if(currentScene == SCENE_POS.NO_IN_SCENE)
				{
					currentScene = judge_scene(nextMapID);
					if(currentScene == SCENE_POS.NO_IN_SCENE) //如果地图ID还是没有得到
						currentScene = SCENE_POS.IN_CITY; //默认在主城
					clearSceneOver = true;
				}
				else
				{
					SCENE_POS newScene = judge_scene(nextMapID);
					if(currentScene != newScene) //场景开始切换要进行资源销毁，
					{
						currentScene = newScene;				
						freeMemory();						
					}
					else 
						clearSceneOver = true;
					currentScene = newScene;					
				}					
			}
			else
			{
				clearSceneOver = true;
				if(currentScene == SCENE_POS.NO_IN_SCENE) //重新登陆
				{
					currentScene = judge_scene(nextMapID);
					if(currentScene == SCENE_POS.NO_IN_SCENE) //如果地图ID还是没有得到
						currentScene = SCENE_POS.IN_CITY; //默认在主城
				}
			}
			lastMapID = _currentMapID;
			_currentMapID = nextMapID;
		}
		else
			clearSceneOver = true;
	}
	
	//Destroy the scene
	private void freeMemory()
	{
		bool notRelease = true; //没有释放内存
		if(currentScene != SCENE_POS.IN_CITY)//由主城进入战斗，释放主城UI，目前无需清除UI资源
		{
            //_currentFreeCount = 0;
            //_currentFreeSize = 0;
            //int index = 0;
            //string[] removeList = new string[] { UiNameConst.ui_main, UiNameConst.ui_pack, UiNameConst.ui_demon, UiNameConst.ui_golden_goblin};
            //foreach (string removeName in removeList) 
            //{
            //    GameObject obj = UIManager.Instance.getUIFromMemory(removeName);
            //    if(obj != null)
            //    {
            //        notRelease = false; 
            //        UISprite[] widgets1 = obj.GetComponentsInChildren<UISprite>(true);
            //        _widgetIndexs[index] = 0;
            //        _widgetLens[index] = widgets1.Length;
            //        StartCoroutine(clearTexture(widgets1, index));
            //        index++; //累计加1,
            //        _currentFreeSize++; 
            //    }
            //}
            //if(mapPrefab != null)
            //{
            //    notRelease = false; 
            //    _currentFreeSize ++;  //要卸载地图卡图集必须再加上
            //    UISprite[] widgets = mapPrefab.GetComponentsInChildren<UISprite>(true);
            //    _widgetIndexs[index] = 0;
            //    _widgetLens[index] = widgets.Length;
            //    StartCoroutine(clearTexture(widgets, index));
            //}	
		}
		else //由战斗进入主城，释放战斗UI
		{
            //_currentFreeCount = 0;
            //_currentFreeSize = 1;
            //GameObject obj = UIManager.Instance.getUIFromMemory(UiNameConst.ui_fight);
            //if(obj != null)
            //{
            //    notRelease = false; 
            //    UISprite[] widgets = obj.GetComponentsInChildren<UISprite>(true);
            //    _widgetIndexs[0] = 0;
            //    _widgetLens[0] = widgets.Length;
            //    StartCoroutine(clearTexture(widgets, 0));
            //}
		}  	
        //BundleMemManager.Instance.freeAllBundle(); //退出场景的时候清除原来场景的Bundle
		if(notRelease)
			clearSceneOver = true;
	}
	
	//最后的Unload
	private void unLoadResource()
	{
		if(currentScene != SCENE_POS.IN_CITY) 
		{
//			for (int i = 0; i < UiNameConst.TagRoot.Length; i ++ )
//	        {
//	            string name = UiNameConst.TagRoot[i];
//	            if (!name.Equals(UiNameConst.ui_fight) && !name.Equals(UiNameConst.ui_pause)
//	                && !name.Equals(UiNameConst.ui_born) && !name.Equals(UiNameConst.ui_award))
//	            {
//	            	UiManager.GetInstance().Close(name, true); //Destroy主城的UI
//	            }   			            
//	        }
		}
		else //强制删除战斗的UI
		{	
			UIManager.Instance.closeWindow(UiNameConst.ui_dialog_fight, true);	//关闭暂停的UI
			UIManager.Instance.closeWindow(UiNameConst.ui_waitting_fight, true);
			UIManager.Instance.closeWindow(UiNameConst.ui_fight, true); //关闭战斗UI
			UIManager.Instance.closeWindow(UiNameConst.ui_born, true);
			UIManager.Instance.closeWindow(UiNameConst.ui_award, true);
			UIManager.Instance.closeWindow(UiNameConst.ui_pause, true);
            UIManager.Instance.closeWindow(UiNameConst.ui_golden_gain, true);		
		}  
        //StopAllCoroutines();
		Resources.UnloadUnusedAssets();
     	GC.Collect();  	
     	clearSceneOver = true;
	}
	
	//judge Scene
	private SCENE_POS judge_scene(int mapID)
	{
		//int mapType = MessageManager.Instance.my_property.getServerMapID() / 100000000;
		if(mapID == Constant.SCENE_DEVIL_WAVE)
			return SCENE_POS.IN_DEVIL;
        if (mapID == Constant.SCENE_GOLDEN_GOBLIN)
            return SCENE_POS.IN_GEBULIN;
		else if(mapID / Constant.SCENE_JUDGE_PARAM == 2)
			return SCENE_POS.IN_COMMON_RAID;
		else if(mapID/Constant.SCENE_JUDGE_PARAM == 1)
			return SCENE_POS.IN_CITY;
		else if(mapID == Constant.SCENE_ARENA)
			return SCENE_POS.IN_ARENA;
		return SCENE_POS.NO_IN_SCENE;
	}
	
	//clear texture
//    private IEnumerator clearTexture(UISprite[] widgets, int index)
//    {
//        while (_widgetIndexs[index] < _widgetLens[index])
//        {
//            yield return new WaitForSeconds(0.02f);
//            UISprite w = widgets[_widgetIndexs[index]];	
//            if (w.mainTexture)
//            {     	
//                if(canDeleteAtlta(w.mainTexture.name))
//                {
////					Debug.LogError(w.mainTexture.name);
//                    Resources.UnloadAsset(w.mainTexture);
//                }	        	
//            }
//            _widgetIndexs[index]++;        
//        }
//        if (_widgetIndexs[index] >= _widgetLens[index])
//        {
//            _currentFreeCount++;
//            if(_currentFreeCount >= _currentFreeSize)
//            {
//                unLoadResource();
//            }
//        }
//    }
	
	//判断图集能否删除
	bool canDeleteAtlta(string name)
	{
		if(currentScene == SCENE_POS.IN_CITY)
		{
			if(name.Equals(Constant.CHAT_ATLAS) || name.Equals(Constant.HP_ATLAS) || name.Equals(Constant.MP_ATLAS) || name.Equals(Constant.SKILL_ATLAS) 
	  			|| name.Equals(Constant.COMMON_ATLAS) || name.Equals(Constant.EQUIP_ATLAS) || name.Equals(Constant.DIAMOND_ATLAS))
				return false;
			else
				return true;
		}
		else if(currentScene == SCENE_POS.IN_DEVIL) //恶魔洞窟
		{
			if(name.Equals(Constant.CHAT_ATLAS) || name.Equals(Constant.HP_ATLAS) || name.Equals(Constant.MP_ATLAS)
	  			|| name.Equals(Constant.COMMON_ATLAS) || name.Equals(Constant.EQUIP_ATLAS) || name.Equals(Constant.SKILL_ATLAS))
				return false;
			return 
				true;
		}
		else //在副本
		{
			if(name.Equals(Constant.CHAT_ATLAS) || name.Equals(Constant.HP_ATLAS) || name.Equals(Constant.MP_ATLAS)
	  			|| name.Equals(Constant.COMMON_ATLAS) || name.Equals(Constant.DIAMOND_ATLAS) || name.Equals(Constant.SKILL_ATLAS))
				return false;
			return 
				true;
		}
		return false;
	}
	
//	private void clearTexture(GameObject obj)
//	{
//		if(obj != null)
//		{
//			UISprite[] widgets = obj.GetComponentsInChildren<UISprite>(true);
//			for (int i = 0, len = widgets.Length; i < len; i++)
//		    {
//		        UISprite w = widgets[i];	
//		        Debug.Log ("Removing GameObject: " + w.gameObject.name);
//		        if (w.mainTexture)
//		        {
//		            Debug.Log ("Removing texture: " + w.mainTexture.name);
//					try
//					{
//		            Resources.UnloadAsset(w.mainTexture);
//					}
//					catch(System.Exception e)
//					{
//						Debug.Log ("Removing texture: " + w.mainTexture.name);
//					}
//		        }
//		    }
//		}
//		Resources.UnloadUnusedAssets();
//        GC.Collect();    
//	}
}
