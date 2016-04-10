/**该文件实现的基本功能等
function: 实现UI关闭，打开，内存释放管理
author:ljx
date:2014-03-09
**/

using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using System.Collections;
using System.Xml;
using model;
using manager;
using MVC.entrance.gate;

public class UIManager
{
	private static UIManager _instance;
	
	private GameObject _prevOpenWindow; //上次打开的窗口
	private GameObject _currentOpenWindow; //当前打开的窗口
    private Hashtable _storeInMemoryHash; //存储在内存的UI
    private GameObject _packageCamera;  //背包的相机
    private Hashtable _uiSourceHash;  //存储UI路径的hash表
    private Hashtable _uiFuncOpenHash; //已经开发相应功能的UI
    private GameObject _camera2D;		//2D的相机
    private BetterList<GameObject> _visibleObjs; //可见的UI控件 
    private HashSet<string> _uicountHash;

	public GameObject m_kRenderTexCam;



	public Vector3 m_kRecordPos = Vector3.zero;
	

    private UIManager()
    {
    	_storeInMemoryHash = new Hashtable();
    	_uiSourceHash = new Hashtable();
    	_uiFuncOpenHash = new Hashtable();
        _visibleObjs = new BetterList<GameObject>();
    	_prevOpenWindow = null;
    	_currentOpenWindow = null;
    	loadSourceXml(); //初始化的时候加载位置XML文件

        _uicountHash = new HashSet<string> {
         UiNameConst.ui_arena,
            UiNameConst.ui_award,
            UiNameConst.ui_skill,
            UiNameConst.ui_role,
            UiNameConst.ui_demon,
            UiNameConst.ui_rank,
            UiNameConst.ui_shop,
            UiNameConst.ui_golden_goblin,
            UiNameConst.ui_task,
            UiNameConst.ui_task_dialog,
            UiNameConst.ui_wing,
            UiNameConst.ui_arena_result,
            UiNameConst.ui_map_info,
            UiNameConst.ui_sweep,
            UiNameConst.ui_battle_log,
            UiNameConst.ui_duoren,
            UiNameConst.ui_dungeoninfo,
            UiNameConst.ui_raid,
            UiNameConst.ui_bag,
            UiNameConst.ui_email,
            UiNameConst.ui_furnace,
            UiNameConst.ui_equip,
            UiNameConst.ui_vip,
            UiNameConst.ui_event,
            UiNameConst.ui_friend,
            UiNameConst.ui_guild,
            UiNameConst.ui_pet,
            UiNameConst.ui_monster_reward,
            UiNameConst.ui_guildcenter,
            UiNameConst.ui_guildflag,
            UiNameConst.ui_guildskill,
            UiNameConst.ui_guildshop,
             UiNameConst.ui_guilddonate,
             UiNameConst.ui_guildlist,
             UiNameConst.ui_guildcreate,
             UiNameConst.ui_pandora,
             UiNameConst.ui_medal,
             UiNameConst.ui_channel
        };
        
    }
    
    //根据Ui名称加载UI
	public void openWindow(string uiName)
	{
		// pre open window procedure
		// 1. get a render texture for the player camera

        if (CameraFollow.sCameraFollow && 
            isStuckNeededUI(uiName) && 
            CameraFollow.sCameraFollow.GetComponent<Camera>().gameObject.activeSelf &&
            CameraFollow.sCameraFollow.GetComponent<Camera>().transform.position != Vector3.zero) 
		{
			RenderTexture kRT = GraphicsUtil.CreatePlayerCamRT (CameraFollow.sCameraFollow.GetComponent<Camera> ());

			m_kRecordPos = CameraFollow.sCameraFollow.gameObject.transform.position;
			CameraFollow.sCameraFollow.SetBindTran (null);
			CameraFollow.sCameraFollow.gameObject.transform.position = new Vector3 (0.0f, 1000.0f, 0.0f);

			CameraFollow.sCameraFollow.gameObject.SetActive (false);

			GameObject kBGRenderTexture = GameObject.Find ("BGRenderTexture");


			// create camera
			if (m_kRenderTexCam == null)
			{
                GameObject camObj = BundleMemManager.Instance.getPrefabByName("Model/prefab/RenderTextureCamera", EBundleType.eBundleCommon);
				m_kRenderTexCam = BundleMemManager.Instance.instantiateObj(camObj, Vector3.zero, Quaternion.identity);
			}

			m_kRenderTexCam.SetActive(true);
			m_kRenderTexCam.transform.position = Vector3.zero;
			Camera kRTCam = m_kRenderTexCam.GetComponent<Camera>();

			if (kBGRenderTexture != null) 
			{
				ToolFunc.SetLayerRecursively(kBGRenderTexture, LayerMask.NameToLayer ("BGRenderTex"));

				MeshRenderer render = kBGRenderTexture.GetComponent<MeshRenderer>();
				render.material = new Material(Shader.Find("Diffuse"));
				render.material.SetTexture("_MainTex", kRT);

				kBGRenderTexture.transform.position = new Vector3(0.0f, 0.0f, 8.0f);
				kBGRenderTexture.transform.localEulerAngles = new Vector3(90, 180, 0);
				float ratio = (float)Screen.height / (float)Screen.width;
				kBGRenderTexture.transform.localScale = new Vector3(0.3f, 1.0f, ratio*0.3f);
			}
		}

		//		if(checkFuncHashOpen(uiName))
		//		{
		if (!checkWindowOnly(uiName)) //如果当前打开的窗口不唯一，那么不能打开
			return;
		if(checkUiInMemory(uiName))
		{
			GameObject obj = _uiSourceHash[uiName] as GameObject;
            if ((obj != null) && (!obj.activeSelf))
            {
                obj.SetActive(true);
                _storeInMemoryHash[uiName] = obj;
            }
            if(uiName.Equals(UiNameConst.ui_main) || uiName.Equals(UiNameConst.ui_fight)) //切换主场景
               	NPCManager.Instance.enableCollider(true);
            else
            	NPCManager.Instance.enableCollider(false);
		}
		else
		{
			loadUiResource(uiName);	
		}
		_currentOpenWindow = _storeInMemoryHash[uiName] as GameObject;
	    if (!isRootUI(uiName) && isEffectTouchUI(uiName))
	    {
//                MainLogic.sMainLogic.suspendGame();
	        EasyTouchJoyStickProperty.ShowJoyTouch(false);
	    }

        //发送UI打开新号
        Gate.instance.sendNotification(MsgConstant.MSG_GUIDE_SEND_TRIGGER,
                    new Trigger(TriggerType.UIOpen, uiName));
//		}
//		else //发送功能暂未开放的消息
//		{
//			
//		}
	}
	
	//关闭UI窗口,freeMemory是否资源来判断是否是否内存
	public void closeWindow(string uiName, bool needFree = true, bool changeScene = false)
	{
		if (CameraFollow.sCameraFollow && isStuckNeededUI (uiName))
		{
			if (m_kRenderTexCam) 
			{
				if (CameraFollow.sCameraFollow)
				{
					CameraFollow.sCameraFollow.gameObject.SetActive(true);
					CameraFollow.sCameraFollow.SetBindTran(CharacterPlayer.sPlayerMe.transform);
					CameraFollow.sCameraFollow.gameObject.transform.position = m_kRecordPos;
				}

				m_kRenderTexCam.transform.position = new Vector3(0.0f, 1000.0f, 0.0f);
			}
		}


		if(needFree)
		{
            GameObject destroyObj = _storeInMemoryHash[uiName] as GameObject;
            Object.Destroy(destroyObj);
            _storeInMemoryHash[uiName] = null;
            _storeInMemoryHash.Remove(uiName);
            if (isEffectTouchUI(uiName))
            {
               // freeMemory(destroyObj);
		    }
		}
		else
		{
			(_storeInMemoryHash[uiName] as GameObject).SetActive(false);
			_prevOpenWindow = _storeInMemoryHash[uiName] as GameObject;
		}
        switch (uiName)
        {
            case UiNameConst.ui_vip:
                EasyTouchJoyStickProperty.ShowJoyTouch(true);
                break;
            default:
                break;
        }
        //发送UI关闭新号
        Gate.instance.sendNotification(MsgConstant.MSG_GUIDE_SEND_TRIGGER,
                    new Trigger(TriggerType.UIClose, uiName));


		if (uiName!=UiNameConst.ui_common_tips&&uiName!=UiNameConst.ui_equip_tips&&uiName!=UiNameConst.ui_medal&&uiName!=UiNameConst.ui_pet_equip_tips) {
			Gate.instance.sendNotification(MsgConstant.MSG_MAIN_CLOSE_MENU);
		}
 
		_currentOpenWindow = null; //当前打开的窗口就设置为空了


		MainLogic.sMainLogic.resumeGame();
        NPCManager.Instance.enableCollider(true);
        if (!isRootUI(uiName) && isEffectTouchUI(uiName))
        {
			if (!changeScene)
			{
				if (funcMgr.isOpen == false &&
                    MenuFunView.IsOpen == false &&
                    !TalkManager.Instance.m_bOpen) 
                {    
                    //用来处理点击头像弹出的菜单
					EasyTouchJoyStickProperty.ShowJoyTouch(true);
				}
            }
            else
                EasyTouchJoyStickProperty.ShowJoyTouch(false);

            MainLogic.sMainLogic.resumeGame();
        }
        	
	}
	
	 //显示网络等待的mask，
    public void showWaitting(bool forceWait = false)
    {
    	GameObject waitObj = null;
    	if(SceneManager.Instance.currentScene == SCENE_POS.IN_CITY
    	  || SceneManager.Instance.currentScene == SCENE_POS.NO_IN_SCENE) //如果在主城,第一次进来使用ui_common
    	{
    		if(!_storeInMemoryHash.ContainsKey(UiNameConst.ui_waitting))
    		{
    			loadUiResource(UiNameConst.ui_waitting);
    		}
			waitObj = _storeInMemoryHash[UiNameConst.ui_waitting] as GameObject;
    	}
    	else if(forceWait)//如果在副本或者恶魔洞窟中，除了必须提示的其他都不用显示
    	{
    		if(!_storeInMemoryHash.ContainsKey(UiNameConst.ui_waitting_fight))
    		{
    			loadUiResource(UiNameConst.ui_waitting_fight);
    		}
			waitObj = _storeInMemoryHash[UiNameConst.ui_waitting_fight]  as GameObject;
    	}
    	if(waitObj != null)
    	{
    		UiWaitting waiting = waitObj.GetComponent<UiWaitting>();
    		waiting.showWaiting(true);
    	}
    }
    
    //根据窗口名称是否处于显示状态
    public bool isWindowOpen(string name)
    {
    	if(_currentOpenWindow == null)
    		return false;
    	return _currentOpenWindow.name.Equals(name);
    }

    
    
    //关闭所有的UI窗口
    public void closeAllUI(bool closeRoot = false, bool changeScene = false)
    {
    	BetterList<string> removeList = new BetterList<string>();
    	foreach (string name in _storeInMemoryHash.Keys)
    	{
    		if(closeRoot)
    		{
    			if(!name.Equals(UiNameConst.ui_root))
    				removeList.Add(name);
    		}
			else if(!name.Equals(UiNameConst.ui_main) && !name.Equals(UiNameConst.ui_fight) && !name.Equals(UiNameConst.ui_root) &&!name.Equals(UiNameConst.ui_common_notice))
    		{
    			removeList.Add(name);
    		}
    	}
    	foreach (string removeName in removeList) 
    	{
            closeWindow(removeName, true, changeScene);
    	}
        Gate.instance.sendNotification(MsgConstant.MSG_MAIN_CLOSE_MENU);
        MainLogic.sMainLogic.resumeGame();
    }

    //隐藏网络等待的mask
    public void closeWaitting()
    {
        GameObject waitObj = null;
    	if(SceneManager.Instance.currentScene == SCENE_POS.IN_CITY
			|| SceneManager.Instance.currentScene == SCENE_POS.NO_IN_SCENE) //如果在主城
    	{
    		if(_storeInMemoryHash.ContainsKey(UiNameConst.ui_waitting))
    			waitObj = _storeInMemoryHash[UiNameConst.ui_waitting]  as GameObject;	        
    	}
    	else
    	{
    		if(_storeInMemoryHash.ContainsKey(UiNameConst.ui_waitting_fight))
    			waitObj = _storeInMemoryHash[UiNameConst.ui_waitting_fight]  as GameObject;
    	}
    	if(waitObj != null)
    	{
    		UiWaitting waiting = waitObj.GetComponent<UiWaitting>();
    		waiting.showWaiting(false);
    	}
    }
    
    //显示对话框
    public void ShowDialog(eDialogSureType type, string msg)
    {
    	if(SceneManager.Instance.currentScene == SCENE_POS.IN_CITY)
    	{
	        if (!_storeInMemoryHash.ContainsKey(UiNameConst.ui_dialog))
	        {
	        	loadUiResource(UiNameConst.ui_dialog);
	        }
			DialogManager.Instance.Show(type, msg);//调用显示方法 
//	        (_storeInMemoryHash[UiNameConst.ui_dialog]  as GameObject).GetComponent<DialogView>().Show(type, msg);
    	}
    	else
    	{
    		if (!_storeInMemoryHash.ContainsKey(UiNameConst.ui_dialog_fight))
	        {
	            loadUiResource(UiNameConst.ui_dialog_fight);
	        }
			DialogManager.Instance.Show(type, msg);//调用显示方法
//    		(_storeInMemoryHash[UiNameConst.ui_dialog_fight]  as GameObject).GetComponent<DialogView>().Show(type, msg);
    	}
    }
	
	//显示信息对话框
	public void ShowMessageBox(string msg)
	{
        if (!_storeInMemoryHash.ContainsKey(UiNameConst.ui_dialog1))
        {
        	loadUiResource(UiNameConst.ui_dialog1);
        }
		DialogManager.Instance.SetDialogMsg(msg);
	}

    //显示信息对话框
    public void ShowMessageBoxNew(eDialogSureType type, string msg)
    {
        if (!_storeInMemoryHash.ContainsKey(UiNameConst.ui_dialog2))
        {
            loadUiResource(UiNameConst.ui_dialog2);
        }
        //DialogManager.Instance.SetDialogMsg(msg);
        DialogManager.Instance.Show(type, msg);//调用显示方法
    }

	//设置开启的UI功能
//	public void setOpenUiList(BetterList<string> openList)
//	{
//		foreach (string name in _uiSourceHash.Keys) 
//		{
//			_uiFuncOpenHash.Add(name, false);
//		}
//		if(openList.size > 0)
//		{
//			foreach(string name in openList) 
//			{
//				_uiFuncOpenHash[name] = true;
//			}
//		}
//	}
	
	//获取根资源
	public Transform getRootTrans()
	{
		GameObject obj = getUIFromMemory(UiNameConst.ui_root);
		if(obj != null)
		{
			return obj.transform;
		}
		else
		{
            obj = GameObject.Find("create_role_cam");
		    if (obj != null)
		        return obj.transform;
		}
		return null;
	}

    public void showHiddenUI(string uiName, bool isShow)
    {
        if (isShow)
        {
            foreach (GameObject obj in _visibleObjs)
            {
                if(!obj.name.Equals(uiName))
                    obj.SetActive(true);
            }
            _visibleObjs.Clear(); //清除可见列表
        }
        else
        {
            foreach (GameObject obj in _storeInMemoryHash.Values)
            {
				if (obj != null)
				{
					if (!obj.name.Equals(uiName) && !obj.name.Equals(UiNameConst.ui_root) && obj.activeSelf )
					{
						obj.SetActive(false);
						_visibleObjs.Add(obj);
					}
				}
            }
        }
    }
	
	//根据UI名称获取相应的Object
	public GameObject getUIFromMemory(string name)
    {
    	if (_storeInMemoryHash.ContainsKey(name))
            return _storeInMemoryHash[name] as GameObject;
    	return null;
    }
	
	//根据名称获取SourceVo
	public UiSourceVo getSourceVo(string name)
	{
		UiSourceVo vo = null;
		if(_uiSourceHash.ContainsKey(name))
			vo = _uiSourceHash[name] as UiSourceVo;
		return vo;
	}

    //public void freeAllUIMem()
    //{
    //    //释放从外部加载的icon texture
    //    foreach (Texture2D texture in SourceManager.Instance.IconList.Values)
    //    {
    //        Resources.UnloadAsset(texture);
    //    }
    //    SourceManager.Instance.IconList.Clear();
    //    //释放UI prefab的图集
    //    foreach (Texture texture in _uiTextureDic.Values)
    //    {
    //        Resources.UnloadAsset(texture);
    //    }
    //    _uiTextureDic.Clear();
    //}
	
	//释放单个UI占用资源,是否GC待定
	private void freeMemory(GameObject obj)
	{
        if (obj != null)
        {
            UISprite[] widgets = obj.GetComponentsInChildren<UISprite>(true);
            for (int i = 0, len = widgets.Length; i < len; i++)
            {
                UISprite w = widgets[i];
                if (w.mainTexture)
                {
                    if (!w.mainTexture.name.Equals(SpriteNameConst.COMMON_ATLAS))
                    {
                        try
                        {
                            Resources.UnloadAsset(w.mainTexture);
                        }
                        catch (System.Exception e)
                        {
                            Debug.Log("Removing texture: " + w.mainTexture.name);
                        }
                    }
                }
            }
        }
        //Resources.UnloadUnusedAssets();
	}
	
//	//检测UI相应功能是否开启
//	private bool checkFuncHashOpen(string uiName)
//	{
//		return true; //目前先开放功能
//		if(_uiFuncOpenHash.Contains(uiName))
//		{
//			return (bool)_uiFuncOpenHash[uiName];
//		}
//		return false;
//	}
	
	//检测UI是否存储在内存当中
	private bool checkUiInMemory(string uiName)
	{
		if(_storeInMemoryHash.Contains(uiName))
		{
			return true;
		}
		return false;
	}
	
	//判断当前关闭或开启的UI是否是 底部界面UI
	private bool isRootUI(string uiName)
	{
		return uiName.Equals(UiNameConst.ui_root) || uiName.Equals(UiNameConst.ui_main) || uiName.Equals(UiNameConst.ui_fight);
	}
	
	//判断当前打开的UI是否是不影响触摸板的UI
	private bool isEffectTouchUI(string uiName)
	{
		if(uiName.Equals(UiNameConst.ui_map_info)  || uiName.Equals(UiNameConst.ui_battle_log) || uiName.Equals(UiNameConst.ui_honor_shop) 
		   || uiName.Equals(UiNameConst.ui_hero_list )||uiName.Equals(UiNameConst.ui_luckstone)  || uiName.Equals(UiNameConst.ui_waitting)
		   || uiName.Equals(UiNameConst.ui_waitting_fight)||uiName.Equals(UiNameConst.ui_common_tips)||uiName.Equals(UiNameConst.ui_equip_tips)||uiName.Equals(UiNameConst.ui_pet_equip_tips)
            || uiName.Equals(UiNameConst.ui_sale) || uiName.Equals(UiNameConst.ui_open) || uiName.Equals(UiNameConst.ui_chapter_award) || uiName.Equals(UiNameConst.ui_common_notice)
		   || uiName.Equals(UiNameConst.ui_power)|| uiName.Equals(UiNameConst.ui_quickbuy)||uiName.Equals(UiNameConst.ui_medal)) //二级菜单都不对摇杆产生影响
			return false;
		else if(uiName.Equals(UiNameConst.ui_dialog) || uiName.Equals(UiNameConst.ui_dialog_fight)
			|| uiName.Equals(UiNameConst.ui_sweep) ||uiName.Equals(UiNameConst.ui_new_item))
		{
			foreach (string name in _storeInMemoryHash.Keys) 
			{
				if(!isRootUI(name)) //存在一个非根目录UI,就是二级菜单
					return false;
			}
			return true;
		} 
		
		return true;
	}

	private bool isStuckNeededUI(string uiName)
	{
		if(uiName.Equals(UiNameConst.ui_role) || 
		   uiName.Equals(UiNameConst.ui_bag) ||
		   uiName.Equals(UiNameConst.ui_task) ||
		   uiName.Equals(UiNameConst.ui_arena) ||
		   //uiName.Equals(UiNameConst.ui_talk) ||
		   uiName.Equals(UiNameConst.ui_email) ||
		   uiName.Equals(UiNameConst.ui_equip) ||
		   uiName.Equals(UiNameConst.ui_event) ||
		   uiName.Equals(UiNameConst.ui_friend) ||
		   uiName.Equals(UiNameConst.ui_furnace) ||
		   uiName.Equals(UiNameConst.ui_golden_goblin) ||
		   uiName.Equals(UiNameConst.ui_guild) ||
		   //uiName.Equals(UiNameConst.ui_guild_list) ||
		   //uiName.Equals(UiNameConst.ui_maze) ||
		   uiName.Equals(UiNameConst.ui_monster_reward) ||
		   uiName.Equals(UiNameConst.ui_duoren) ||
		   uiName.Equals(UiNameConst.ui_pandora) ||
		   uiName.Equals(UiNameConst.ui_pet) ||
		   uiName.Equals(UiNameConst.ui_raid) ||
		   uiName.Equals(UiNameConst.ui_setting) ||
		   uiName.Equals(UiNameConst.ui_shop) ||
		   uiName.Equals(UiNameConst.ui_skill) ||
		   uiName.Equals(UiNameConst.ui_demon) ||
		   uiName.Equals(UiNameConst.ui_setting) ||
		   uiName.Equals(UiNameConst.ui_vip) ||
		   uiName.Equals(UiNameConst.ui_wing)
		   )
			return true;

		return false;
	}
	
	//判断当前打开的UI是否唯一
    private bool checkWindowOnly(string uiName)
    {
 
        if (isRootUI(uiName) || uiName.Equals(UiNameConst.ui_waitting_fight) || uiName.Equals(UiNameConst.ui_waitting) || uiName.Equals(UiNameConst.ui_common_notice)
		    || uiName.Equals(UiNameConst.ui_power)||uiName.Equals(UiNameConst.ui_new_item) || uiName.Equals(UiNameConst.ui_quickbuy)|| uiName.Equals(UiNameConst.ui_new_function))
 
            return true;
        if (isEffectTouchUI(uiName)) //影响摇杠的界面就是单独界面
        {
            foreach (string name in _storeInMemoryHash.Keys)
            {
				if (name.Equals(UiNameConst.ui_waitting_fight) || name.Equals(UiNameConst.ui_waitting) || name.Equals(UiNameConst.ui_common_notice) || name.Equals(UiNameConst.ui_power)|| name.Equals(UiNameConst.ui_new_item)|| name.Equals(UiNameConst.ui_quickbuy)|| name.Equals(UiNameConst.ui_new_function))
                    continue;
				else if(!isRootUI(name))
                	return false; //存在一个非根目录UI,那么窗口就不是唯一打开得
            }
        }
        else
        {
            string parentName = null;
            switch (uiName)
            {
                case UiNameConst.ui_map_info:
                    parentName = UiNameConst.ui_raid;
                    break;
                 case UiNameConst.ui_battle_log:
                    parentName = UiNameConst.ui_arena;
                    break;
                 case UiNameConst.ui_honor_shop:
                    parentName = UiNameConst.ui_arena;
                    break;
                 case UiNameConst.ui_hero_list:
                    parentName = UiNameConst.ui_arena;
                    break;
                 case UiNameConst.ui_luckstone:
                    break;
                 default:
                    break;
            }
            if (parentName != null)
                return !checkOpenOther(parentName);
        }
        return true;
    }

    //检查二级UI菜单打开时，是否还有其它窗口打开
    private bool checkOpenOther(string parentName)
    {
        foreach (string name in _storeInMemoryHash.Keys)
        {
            if (isRootUI(name) || name.Equals(UiNameConst.ui_waitting) || name.Equals(UiNameConst.ui_waitting_fight) || name.Equals(UiNameConst.ui_common_notice) 
			    || name.Equals(UiNameConst.ui_power)||name.Equals(UiNameConst.ui_new_item)|| name.Equals(UiNameConst.ui_quickbuy)|| name.Equals(UiNameConst.ui_new_function) )
                continue;
            if (!name.Equals(parentName)) //存在不同于父级的UI，那么其它窗口就有打开
                return true;
        }
        return false;
    }
	
	//加载新的资源
	private void loadUiResource(string uiName)
    {
		UiSourceVo sourceVo = _uiSourceHash[uiName] as UiSourceVo;
		if(sourceVo != null)
		{
		    GameObject source = BundleMemManager.Instance.loadResource(sourceVo.loadPath) as GameObject;	    
            GameObject obj = null;
			if(uiName == UiNameConst.ui_root || uiName == UiNameConst.ui_common_notice) //根目录不消除
				obj = Object.Instantiate(source) as GameObject;
			else
				obj = BundleMemManager.Instance.instantiateObj(source);
            UISprite[] widgets = obj.GetComponentsInChildren<UISprite>(true);
            for (int i = 0, len = widgets.Length; i < len; i++)
            {
                UISprite w = widgets[i];
                if (w.mainTexture != null && !w.mainTexture.name.Equals(SpriteNameConst.COMMON_ATLAS))
                {
                    if (w.mainTexture && !BundleMemManager.Instance.UiTextures.ContainsKey(w.mainTexture.name))
                    {
                        BundleMemManager.Instance.UiTextures.Add(w.mainTexture.name, w.mainTexture);
                    }
                }
            }
			obj.name = sourceVo.name;
			if(sourceVo.parent != null)
			{
				GameObject parent = _storeInMemoryHash[sourceVo.parent] as GameObject; //存在parent,parent肯定要常驻内存
				if(parent == null && _storeInMemoryHash.Contains(UiNameConst.ui_root))
				{
					if(uiName.Equals(UiNameConst.ui_camera))
					{
						if(Camera2D != null)
							parent = Camera2D;
					}
					else
					{
						Transform trans = (_storeInMemoryHash[UiNameConst.ui_root] as GameObject).transform.Find(sourceVo.parent);
						if(trans != null)
							parent = trans.gameObject;
					}
				}
				if(parent != null)
				{
					obj.transform.parent = parent.transform;
				}
			}       
			obj.transform.localPosition = sourceVo.localPosition;
			obj.transform.localScale = sourceVo.localScale;
			obj.transform.eulerAngles = sourceVo.localRotation;  
			_storeInMemoryHash[uiName] = obj;
		}		
    }
	
	//加载UI的XML
	private void loadSourceXml()
	{
		TextAsset ta = BundleMemManager.Instance.loadResource(PathConst.UI_PREFAB_PATH) as TextAsset;
        if (ta)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ta.text);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("Root").ChildNodes;
            string[] splits = null;
			char[] charSeparators = new char[] {','};
            for (int k = 0; k < nodeList.Count; k++)
            {
                XmlElement xe = nodeList.Item(k) as XmlElement;
                if (xe == null)
                    continue;
				string name = xe.GetAttribute("Name");
				UiSourceVo uiVo;
				if(_uiSourceHash.ContainsKey(name))
					uiVo = _uiSourceHash[name] as UiSourceVo;
				else
				{
					uiVo = new UiSourceVo();
					_uiSourceHash[name] = uiVo;
				}
                for (int i = 0; i < xe.Attributes.Count; i++)
                {
                    XmlAttribute attr = xe.Attributes[i];
                    try
                    {
                    	switch (attr.Name)
                    	{
                    		case "Name":
                    			uiVo.name = attr.Value;
                    			break;
                    		case "Source":
                    			uiVo.loadPath = attr.Value;
                    			break;
                    		case "Parent":
                    			uiVo.parent = attr.Value;
                    			break;
                    		case "Position":
                    			splits = attr.Value.Split (charSeparators);
                    			uiVo.localPosition = new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2]));
                    			break;
                    		case "Scale":
                    			splits = attr.Value.Split (charSeparators);
                    			uiVo.localScale = new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2]));
                    			break;
                    		case "Rotation":
                    			splits = attr.Value.Split (charSeparators);
                    			uiVo.localRotation = new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2]));
                    			break;
                    		default:
                    			break;
                    	}
                    }
                    catch (System.Exception ex)
                    {
                        
                    }

                }               
            }
        }
	}
	
	//getter and setter
	public static UIManager Instance 
	{
		get 
		{ 
			if(_instance == null)
				_instance = new UIManager();
			return _instance; 
		}
	}
	
	public GameObject Camera2D 
	{
		get 
		{ 
			if(_camera2D == null)
			{
				if(_storeInMemoryHash.Contains(UiNameConst.ui_root))
				{
					Transform trans = (_storeInMemoryHash[UiNameConst.ui_root] as GameObject).transform.Find(UiNameConst.ui_camera);
					if(trans != null)
						_camera2D = trans.gameObject;
				}
			}
			return _camera2D;
		}
	}

    /// <summary>
    /// 是否有UI
    /// </summary>
    public bool IsHaveWindow
    {
        get {
            foreach (var ui in _uicountHash)
            {
				if (!checkWindowOnly(ui))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

//文件存储的VO
namespace model
{
	using UnityEngine;
	public class UiSourceVo
	{
		public string name;  //ui名称，跟程序中UI常量对应
	    public string loadPath; //ui的加载路径
	    public Vector3 localPosition;
	    public Vector3 localScale;
	    public Vector3 localRotation; 
	    public string parent;
	}
}
