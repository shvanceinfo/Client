using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using NetGame;
using System;
using manager;
using MVC.entrance.gate;
using model;
using ProtoBuf;
using NetPackage;

//public enum EPlatform
//{
//	ePlatform_Test = 0,
//	ePlatform_IOS_91,
//}
public class MainLogic : MonoBehaviour
{
	
	[DllImport("_Internal")]
	private static extern void IOS_91SDKConnector_SetDebugMode ();

	[DllImport("_Internal")]
	private static extern void IOS_91SDKConnector_Init ();

	[DllImport("_Internal")]
	private static extern void IOS_91SDKConnector_EnterPlatform ();
		
	public bool useLocalNet = false;
	public string localIP = "127.0.0.1";
	public int port = 9999;
	[HideInInspector]
	public bool suspend = false;
	[HideInInspector]
	public bool needReconnect = false;
	[HideInInspector]
	public bool isInGame = false;
	bool is_background = false;
	bool firstInGame = true; //第一次进入游戏
	
	[HideInInspector]
	public bool
		bRelogin = false;
	public static MainLogic sMainLogic;
	//public static bool hasLoadCreateScene = false; //是否加载过创角的场景
	
	void OnExit (eDialogSureType type)
	{
		if (type == eDialogSureType.eApplicationExit) {
			Application.Quit ();
		}

		if (type == eDialogSureType.eDisconnect) {
			//Application.Quit ();
            SettingManager.Instance.ReLogin();
		}
	}

	void Awake ()
	{

		//iPhoneUtils.PlayMovie("JiangLinWorld.mp4", Color.black, iPhoneMovieControlMode.CancelOnTouch);
        ///监听退出确认对话框
        EventDispatcher.GetInstance().DialogSure += OnExit;
        Application.targetFrameRate = Global.FrameRate;
		sMainLogic = this;
		SceneManager.Instance.setDontDestroyObj(gameObject);
		//DontDestroyOnLoad(gameObject);
        ConfigDataManager.GetInstance().init();       
        EventDispatcher.GetInstance().ConnectedServer += requestEnterGame;
        EventDispatcher.GetInstance().LostConnectServer += OnLostConnectServer;
        LanguageManager.LoadTxt();
        needReconnect = false;
        isInGame = false;
        firstInGame = true;
    	Application.LoadLevel(Constant.CREATE_ROLE_SCENE);
		Shader.WarmupAllShaders();
        Constant.LOAD_TIP_MAX = ConfigDataManager.GetInstance().getLoadingTipsConfig().getTipSize();
        
	}
	
	public void OnSDKNotify (string pszResult)
	{
		//UnityEngine.Debug.Log("OnSDKLoginResult pszResult = " + pszResult);
	}
	
	void Start ()
	{
		//iPhoneUtils.PlayMovie("JiangLinWorld.mp4", Color.black, iPhoneMovieControlMode.CancelOnTouch);
		///监听退出确认对话框
// 		EventDispatcher.GetInstance ().DialogSure += OnExit;
// 		EventDispatcher.GetInstance ().DialogCancel += OnExit;
// 
// 		Application.targetFrameRate = Global.FrameRate;
// 		sMainLogic = this;
// 		SceneManager.Instance.setDontDestroyObj (gameObject);
// 		//DontDestroyOnLoad(gameObject);
// 		ConfigDataManager.GetInstance ().init ();       
// 		EventDispatcher.GetInstance ().ConnectedServer += requestEnterGame;
// 		EventDispatcher.GetInstance ().LostConnectServer += OnLostConnectServer;
// 		LanguageManager.LoadTxt ();
// 		needReconnect = false;
// 		isInGame = false;
// 		firstInGame = true;
// 		Application.LoadLevel (Constant.CREATE_ROLE_SCENE);
// 		Shader.WarmupAllShaders ();
// 		Constant.LOAD_TIP_MAX = ConfigDataManager.GetInstance ().getLoadingTipsConfig ().getTipSize ();
//		if (Application.platform == RuntimePlatform.IPhonePlayer && m_ePlatform == EPlatform.ePlatform_IOS_91)
//		{
//			IOS_91SDKConnector_SetDebugMode();
//			IOS_91SDKConnector_Init();
//			IOS_91SDKConnector_EnterPlatform();
//		}

        if (!transform.gameObject.GetComponent<AskServerData>())
        {
            transform.gameObject.AddComponent<AskServerData>();
            transform.GetComponent<AskServerData>().AskData();
        }
        //DataReadServer.Instance.DataInfo();
	}
	
	void Update ()
	{	
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				UIManager.Instance.ShowDialog (eDialogSureType.eApplicationExit, LanguageManager.GetText ("msg_exit_application"));
			}
		}
		update_meassage ();
	}
	
	void OnApplicationFocus (bool focusStatus)
	{
		if (Application.platform == RuntimePlatform.Android) {
			if (focusStatus && is_background) {
				//NetBase.GetInstance().ReConnect();
				//is_background = false;
			} else {
				//NetBase.GetInstance().Close();
				//is_background = true;
			}
		}	
	}
	
	void OnApplicationQuit ()
	{
		GCAskLogout logout = new GCAskLogout ();
		NetBase.GetInstance ().Send (logout.ToBytes (), false);
		NetBase.GetInstance ().Close (true);
	}
	
	void requestEnterGame ()
	{		
		NetBase net = NetBase.GetInstance ();
		CacheUserInfo userCache = CacheManager.GetInstance ().GetCacheInfo ();
        CRequestLogin LoginMsg = new CRequestLogin { };
        LoginMsg._userGUID = "";
        LoginMsg._userName = userCache.UserName;
        LoginMsg._userPassword = userCache.UserPassword;
        LoginMsg._reConnect = isInGame;
        LoginMsg._deviceToken = Global.GetDeviceIdentifier();
		if ((userCache.GUID == "") || (string.IsNullOrEmpty (userCache.GUID))) {
			Loger.Notice ("Login, new user~~~");

			//LoginMsg = new RequestLogin (userCache.UserName, userCache.UserPassword, "", isInGame);
			Loger.Notice ("Login username: {0}, password: {1}, guid: {2}", userCache.UserName, userCache.UserPassword, userCache.GUID);
		} else {
			Loger.Notice ("Login from cache[{0}]", Application.persistentDataPath);
			bool reconnect = isInGame;
			if (Global.inMultiFightMap ())
				reconnect = false;
			//LoginMsg = new RequestLogin (userCache.UserName, userCache.UserPassword, userCache.GUID, reconnect);
            LoginMsg._userGUID = userCache.GUID;
			Loger.Notice ("new RequestLogin over");
			Loger.Notice ("Login username: {0}, password: {1}, guid: {2}", userCache.UserName, userCache.UserPassword, userCache.GUID);
		}        
		if (net.IsConnected) {
			//net.Send (LoginMsg.ToBytes ());
            net.Send<CRequestLogin>(LoginMsg, (UInt16)CeC2GType.C2G_Login);
		}
	}
	
	//登录游戏，从收到人物信息进行回调
	public void  enterGame ()
	{
		if (!isInGame) {			
			MessageManager.Instance.sendMessageSelectRole (); //第一次进入游戏需要发送选择角色
			GCAskNotice notice = new GCAskNotice (PostType.System); //请求服务器发送公告
			SendMesg (notice.ToBytes ());
			isInGame = true;
			InitUi ();
		}	
		if (CharacterPlayer.sPlayerMe == null) {
			PlayerManager.Instance.createPlayerMe (MessageManager.Instance.my_property);
			DontDestroyOnLoad (CharacterPlayer.sPlayerMe);
			PlayerManager.Instance.CreateEasyTouchJoyStick ();
		} else {
			CharacterPlayer.character_property = MessageManager.Instance.my_property;
			CharacterPlayer.sPlayerMe.applyProperty ();
		}
		if(CameraFollow.sCameraFollow == null)
		{
			PlayerManager.Instance.createPlayerCamera ();
			DontDestroyOnLoad (CameraFollow.sCameraFollow);
		}
	}
	
	//通过bLoginedIn来控制enterScene取消，正常流程进入游戏，进入场景后的回调
	public void enterScene (UInt32 id)
	{		
		MapDataItem di = ConfigDataManager.GetInstance ().getMapConfig ().getMapData ((int)id);
		if (di.clientNO <= 0) {
			return;
		}
		SceneManager.Instance.sceneID = di.clientNO;	
		SceneManager.Instance.nextMapID = MessageManager.Instance.my_property.getServerMapID ();
		if (!NetBase.GetInstance ().clientDisconnect) { //客户端断线重练，不关闭UI，不释放内存
            if (CameraFollow.sCameraFollow)
            {
                Destroy(CameraFollow.sCameraFollow.gameObject);
                CameraFollow.sCameraFollow = null;
            }	
			UIManager.Instance.closeWindow (UiNameConst.ui_main, true, true);
			UIManager.Instance.closeWindow (UiNameConst.ui_fight, true, true);
			if (CharacterPlayer.sPlayerMe) {
				MessageManager.Instance.my_property = CharacterPlayer.character_property;
                Destroy(CharacterPlayer.sPlayerMe.gameObject);
                CharacterPlayer.sPlayerMe = null;
			}
			PlayerManager.Instance.destroyAllPlayerOther ();
			//MonsterManager.Instance.onEnterLevel();  
		}
		if (di.battlePref != "") {
			Global.enterScensBattlePref = di.battlePref;
		}

		Global.m_bInGame = false;
			
		if (!NetBase.GetInstance ().clientDisconnect || bRelogin) 
		{
			bRelogin = false;
			UIManager.Instance.closeAllUI (false, true); //确保加载loading关闭所有的UI
			Application.LoadLevel (Constant.LOADING_SCENE); //清除之前场景	
		}
	}
	
	//加载场景结束后切换
	void OnLevelWasLoaded ()
	{
		if (!Constant.CREATE_ROLE_SCENE.Equals (Application.loadedLevelName)
			&& !Constant.LOADING_SCENE.Equals (Application.loadedLevelName)) { //只有加载不是进入服务器的场景才需要做后续流程
			suspend = false; //加载完场景不暂停			
			MonsterManager.Instance.onEnterLevel ();
			PlayerManager.Instance.createPlayerMe (MessageManager.Instance.my_property);
			PlayerManager.Instance.createPlayerCamera ();
			PlayerManager.Instance.CreateEasyTouchJoyStick ();
			NPCManager.Instance.initNPC (SceneManager.Instance.nextMapID);	
			//StartCoroutine ("reloadUI");
			reloadUI();
			BattleManager.Instance.m_bTimeOver = false;
			ArenaManager.Instance.prepareArena (); //如果是进入竞技场必须准备竞技场
			//BundleMemManager.Instance.freePreloadBundle(); //清除预加载的bundle
			CharacterPlayer.sPlayerMe.GetComponent<HUD> ().GenerateHeadUI (CharacterPlayer.character_property.nick_name, 1, HUD.HUD_CHARACTER_TYPE.HCT_PLAYER, false);
		}
	}
	
	//在清理内存完毕后才加载UI
	void reloadUI ()
	{	
		if (SceneManager.Instance.clearSceneOver) {
#if UNITY_IPHONE
//			UnityToXcodeMsgInterface.CheckHistoryReceipt();
#endif
			if (Global.inCityMap ()) {

				if (CharacterPlayer.sPlayerMe && Global.inCityMap ()) {
					CharacterPlayer.sPlayerMe.setPositionLikeGod (
                    CharacterPlayer.character_property.getEnterCityPos ());

					if (ArenaManager.Instance.challengerRank > 0) 
					{ //从竞技场返回得
                        //Debug.LogError("Load Open ArenaUI");
						ArenaManager.Instance.showArenaUI (); //请求打开竞技场界面
					}
					if (Global.firstPreload) {
						Global.firstPreload = false;
					}

					PlayerManager.Instance.CreateOthersByLoginData ();
				}

				// 多人副本数据重置
				BattleMultiPlay.GetInstance ().ResetData ();
				BattleWorldBoss.GetInstance ().ResetData ();


				UIManager.Instance.openWindow (UiNameConst.ui_main);
				UIManager.Instance.closeWindow (UiNameConst.ui_fight); //关闭战斗界面
				if (ScenarioManager.Instance.passGateSuccess) { //关卡通关显示通关的剧情
					ScenarioManager.Instance.showScenario (TaskManager.Instance.MainTask, null, eTriggerType.backToCity);
					ScenarioManager.Instance.passGateSuccess = true;
				}

				Global.m_bCameraCruise = false;
				BossManager.Instance.SetNotInWorldBossStatus ();//重置状态
				Global.requestBornNum = 1;
			} else {
				UIManager.Instance.openWindow (UiNameConst.ui_fight);
				UIManager.Instance.closeWindow (UiNameConst.ui_main);
				if (Global.inFightMap ()) {
					FightManager.Instance.ItemData.ReTime = 0;
					FightManager.Instance.ItemData.Time = 0;
					FightManager.Instance.ItemData.IsReTime = false;
					Global.current_fight_level = Global.eFightLevel.Fight_Level1;
					Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_DISPLAY_TIME);
				}
				if (Global.enterScensBattlePref != "") {
					GameObject asset = null;
					
					if (Global.InAwardMap ()) {
						asset = BundleMemManager.Instance.getPrefabByName (Global.enterScensBattlePref, EBundleType.eBundleReward);
					} else {
						asset = BundleMemManager.Instance.getPrefabByName (Global.enterScensBattlePref, EBundleType.eBundleRaid);
					}
					
					
					if (asset != null) {
						GameObject battlePref = BundleMemManager.Instance.instantiateObj (asset);
						battlePref.name = Global.enterScensBattlePref;
						Global.enterScensBattlePref = "";
						if (Global.inMultiFightMap ()) {
							BattleMultiPlay.GetInstance ().m_kBattlePrefab = battlePref.transform;
						}
					}
				}
				if (Global.inTowerMap ()) {
					FightManager.Instance.ItemData.IsReTime = false;
					FightManager.Instance.ItemData.Time = 0;
					//UIManager.Instance.getUIFromMemory(UiNameConst.ui_fight).GetComponent<UiFightMainMgr>().consTime = 60; //这时候初始化战斗资源
					Global.current_fight_level = Global.eFightLevel.Fight_Level2;
					BattleEmodongku.GetInstance ().waitTowerMonsterWave (Global.cur_TowerId);
					Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_DISPLAY_TIME);
				}
				if (Global.inGoldenGoblin ()) {
					//UIManager.Instance.getUIFromMemory(UiNameConst.ui_fight).GetComponent<UiFightMainMgr>().consTime = 120; //这时候初始化战斗资源
					FightManager.Instance.ItemData.IsReTime = true;
					FightManager.Instance.ItemData.ReTime = 120;
					Global.current_fight_level = Global.eFightLevel.Fight_Level2;
					BattleGoblin.GetInstance ().Init ();
					Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_DISPLAY_GOBLIN_FUCNTION);
					Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_DISPLAY_TIME);
				}
				if (Global.InArena ()) {
					Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_DISPLAY_TIME);
				}

				if (Global.inMultiFightMap ()) {
					PlayerManager.Instance.CreateOthersByLoginData ();
					CharacterPlayer.sPlayerMe.setPositionLikeGod (CharacterPlayer.character_property.getEnterCityPos ());
					FightManager.Instance.ItemData.ReTime = 0;
					FightManager.Instance.ItemData.Time = 0;
					FightManager.Instance.ItemData.IsReTime = false;
					Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_DISPLAY_TIME);
				}
				if (Global.InWorldBossMap ()) {
                    FightManager.Instance.ItemData.ReTime = BossManager.Instance.WorldBossTime() * -1; 
                    FightManager.Instance.ItemData.IsReTime = true;
					PlayerManager.Instance.CreateOthersByLoginData ();
					CharacterPlayer.sPlayerMe.setPositionLikeGod (CharacterPlayer.character_property.getEnterCityPos ());
					BossManager.Instance.SetInWorldBossStatus ();
				}
				if (Global.InAwardMap ()&&!Global.inGoldenGoblin ()&&!Global.inTowerMap ()) {
					FightManager.Instance.ItemData.ReTime = 0;
					FightManager.Instance.ItemData.Time = 0;
					FightManager.Instance.ItemData.IsReTime = false;
					Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_DISPLAY_TIME);
				}
				  
				
			}


			int nCurMapID = MessageManager.Instance.my_property.getServerMapID ();
			MapDataItem mapData = ConfigDataManager.GetInstance ().getMapConfig ().getMapData (nCurMapID);
			string gridname = mapData.mapGridName;
			//AISystem.AStarAlgorithm.GetInstance().ParseMapPathFileWithWrite(gridname);
			AISystem.AStarAlgorithm.GetInstance ().ParseMapPathFile (gridname);

			//PlayerManager.sPlayerManager.AddNavMeshComponent();

			activeName (true);		
			//StopCoroutine ("reloadUI");
			if (TaskManager.Instance.isTaskFollow) { //处于跨主城寻路状态, 必须在添加寻路组件后
				Vector3 moveToPos = TaskManager.Instance.FolllowPath.Pop ();
				CharacterPlayer.sPlayerMe.GetAI ().m_kPursueState.m_kPursurNPC = NPCManager.Instance.pathFollowNPC;
				CharacterPlayer.sPlayerMe.GetAI ().m_kPursueState.m_bGotoGate = false;
				NPCManager.Instance.markNPCTag (NPCManager.Instance.pathFollowNPC);
				CharacterPlayer.sPlayerMe.GetAI ().SendStateMessage (CharacterAI.CHARACTER_STATE.CS_PURSUE, moveToPos);
			}
			if (EasyTouchJoyStickProperty.sJoystickProperty) {
				EasyTouchJoyStickProperty.SetJoyStickEnable (true);
				PlayerManager.Instance.CreateEasyTouchJoyStick ();
				CharacterPlayer.sPlayerMe.gameObject.AddComponent<InputProperty> ();
				CharacterPlayer.sPlayerMe.GetComponent<InputProperty> ().ResetData ();
				if (!Global.InArena ()) { // 不在竞技场
					EasyTouchJoyStickProperty.ShowJoyTouch (true);  //加载完成,显示新场景的时候显示遥杠
					if (ArenaManager.Instance.challengerRank > 0) { //从竞技场返回得
						EasyTouchJoyStickProperty.ShowJoyTouch (false); //从竞技场返回隐藏摇杠
						ArenaManager.Instance.challengerRank = 0; //置位成零
					}
				}
				
				if (Global.inCityMap ()) {
					NewitemManager.Instance.NewItemShow ();
				}//显示得到的新道具
				
			}
			
			if (firstInGame) {
				firstInGame = false;
                if (GuideManager.Instance.CheckOpenNotice)
				showNotice ();	        	
			}

			Global.m_bInGame = true;
            Time.timeScale = 1.0f;
			#region 点击头像功能状态的还原
			funcMgr.isOpen = false;
			MenuFunView.IsOpen = false;
			#endregion

			#region 切场景的时候重置队列
			FastOpenManager.Instance.Reset();
			#endregion
		}
        //while (!SceneManager.Instance.clearSceneOver) {
        //    yield return new WaitForSeconds (0.1f);
        //}
	}

	public void suspendGame ()
	{
		suspend = true;
	}
	
	public void resumeGame ()
	{
		suspend = false;
	}
	
	public bool isGameSuspended ()
	{
		return suspend;
	}

	private void InitUi ()
	{
		UIManager.Instance.openWindow (UiNameConst.ui_root);
		TaskManager.Instance.EffectMask = UIManager.Instance.getRootTrans ().Find ("Camera/mask").gameObject;
		DontDestroyOnLoad (UIManager.Instance.getUIFromMemory (UiNameConst.ui_root));
		//先隐藏xCode的UI
		//GameObject obj = Instantiate(BundleMemManager.Instance.loadResource("XcodeToUnityMsg/XCodeToUnityMsgInterface")) as GameObject;
		//obj.name = "XCodeToUnityMsgInterface";
		//DontDestroyOnLoad(obj);
	}
	
	void update_meassage ()
	{		
		if (NetBase.GetInstance ().IsConnected) {
			ArrayList meassages = null;
			MessageManager.Instance.getMessage (out meassages);		
			if (meassages != null && meassages.Count > 0) {
				for (int i = 0; i < meassages.Count; i++) {		
					byte[] bytesData = (byte[])meassages [i];
					MessageManager.Instance.processMessage (bytesData);
				}
				UIManager.Instance.closeWaitting (); //收到了数据，关闭waiting
				meassages.Clear ();
			}
		} else if (needReconnect) { //开始断线重练
			if (NetBase.GetInstance ().serverDisconnect) { //如果是服务器断开连接那么走登录流程
				ConnectGameServer (); //链接服务器
				isInGame = false; //服务器断开需选择角色
			} else {
				NetBase.GetInstance ().ReConnect ();
			}
			needReconnect = false; //玩家在游戏状态
		}
	}

	public static void SendMesg (byte[] byteData, bool showWait=false)
	{
		NetBase.GetInstance ().Send (byteData, showWait);
	}

	public void ConnectGameServer (bool bCloseThread = false)
	{
		NetBase.GetInstance ().SetMessage (new GameMessage ());
		TaskManager.Instance.FirstRecv = true;
		if (useLocalNet)
			NetBase.GetInstance ().Connect (localIP, port, bCloseThread);
		else
			NetBase.GetInstance ().Connect ("118.192.89.218", port, bCloseThread);       
		InvokeRepeating ("Ping", Constant.PING_TIME, Constant.PING_TIME);
	}
    
	public void activeName (bool active)
	{
		for (int i=0; i<HUD.nameObjs.size; i++) {
			GameObject name = HUD.nameObjs [i];
			if (name != null)
				name.SetActive (active);
		}
	}
	
	public void reloadCurrentScene ()
	{
	}
	
//	public void stopPing()
//	{
//		CancelInvoke("Ping");
//	}
	
	#region 多人副本需要增加的功能 
	//改变为副本的ping
	public  void ChangeToDungeonPing ()
	{
		InvokeRepeating ("Ping", Constant.PING_DUNGEON_TIME, Constant.PING_DUNGEON_TIME);
	}

	public  void ChangeToNormalPing ()
	{
		InvokeRepeating ("Ping", Constant.PING_TIME, Constant.PING_TIME);
	}
	//取消ping
	public  void CancelPing ()
	{
		CancelInvoke ("Ping");
	}
	#endregion
	
	
	void Ping ()
	{
		RequestPing ping = new RequestPing ();
		SendMesg (ping.ToBytes (), false);
        //BundleMemManager.Instance.freeCacheMem ();
	}

	void OnLostConnectServer ()
	{
		NetBase.GetInstance ().ReConnect ();
	}
 
	//显示公告信息
	void showNotice ()
	{
		UIManager.Instance.openWindow (UiNameConst.ui_notice);
		GameObject obj = UIManager.Instance.getUIFromMemory (UiNameConst.ui_notice);
		UILabel title = obj.transform.Find ("noticeContent/title").GetComponent<UILabel> ();	
		title.text = ServiceManager.Instance.GameNotice.title;			
		UILabel content = obj.transform.Find ("noticeContent/contentView/TextList/content").GetComponent<UILabel> ();	
		content.lineWidth = 730;
		content.text = ServiceManager.Instance.GameNotice.content;
		UILabel author = obj.transform.Find ("noticeContent/author").GetComponent<UILabel> ();	
		author.text = ServiceManager.Instance.GameNotice.author;
		UILabel time = obj.transform.Find ("noticeContent/time").GetComponent<UILabel> ();	
		time.text = ServiceManager.Instance.GameNotice.msgTime;
	}
	
	
	 
}