using System.Linq;
using UnityEngine;
using System.Collections;
using model;
using NetGame;
using System.Collections.Generic;
using MVC.entrance.gate;

namespace manager
{
    public class SettingManager
    {
        static SettingManager _instance;

        private Hashtable _displayHash;

        public Hashtable DisplayHash
        {
            get { return _displayHash; }
            set { _displayHash = value; }
        }

        /// <summary>
        /// 当前地图设置
        /// </summary>
        public SettingVo CurDisplayVo
        {
            get { return FindSettingVoByMapId(MessageManager.Instance.my_property.getServerMapID()); }
        }

        private Hashtable _posts;

        private BetterList<HelpInfo> _helps;

        private bool _hidePeople;
        private int _showPeople;


        private GCAskNotice _ask;

        private uint _checkBoxIndex;
        private SettingManager()
        {
            _showPeople = 10;
            _hidePeople = false;
            _posts = new Hashtable();
            _checkBoxIndex = 0;
            _displayHash = new Hashtable();
            _helps = new BetterList<HelpInfo>();
        }

        /// <summary>
        /// 添加公告信息
        /// </summary>
        public void AddPost(int id,PostType type,string time,string title,string content,string author)
        {
            PostVo vo = new PostVo();
            vo.Id = id;
            vo.Type = type;
            vo.MsgTime = title;
            vo.Title = title;
            vo.Content = content;
            vo.Author = author;
            _posts[vo.Id] = vo;
        }

        public void RequestPost()
        {
            _ask = new GCAskNotice(PostType.Post);
            NetBase.GetInstance().Send(_ask.ToBytes());
        }


        public void SetCheckBox(uint index)
        {
            _checkBoxIndex = index;
        }

        /// <summary>
        /// 提交bug
        /// </summary>
        public void SendQustion()
        { 
            
        }

        //设置隐藏/显示 开关
        public void SetPeopleOptionBtn()
        {
            if (CurDisplayVo.CurOption != 0)
            {
                OnChangePeopleOption(0);
            }
            else {
				if (CurDisplayVo.LastOption ==0) {                 //还原上次设置的值
					OnChangePeopleOption(CurDisplayVo.DefaultOption);
				}else{
					OnChangePeopleOption(CurDisplayVo.LastOption);
				}
            }
            Gate.instance.sendNotification(MsgConstant.MSG_SETTING_PEOPLE_SWITCH);
            SaveDisplayCache();
        }

        public void SetMusic()
        {
            AudioManager.Instance.MusicActive = !AudioManager.Instance.MusicActive;
            CacheManager.GetInstance().SetMusic(AudioManager.Instance.MusicActive);
            CacheManager.GetInstance().SaveCache();
        }
        public void SetAudio()
        {
            AudioManager.Instance.AudioActive = !AudioManager.Instance.AudioActive;
            CacheManager.GetInstance().SetAudio(AudioManager.Instance.AudioActive);
            CacheManager.GetInstance().SaveCache();
        }

        public PostVo GetSystemPostVo()
        {
            foreach (PostVo vo in _posts.Values)
            {
                if (vo.Type==PostType.System)
                {
                    return vo;
                }
            }
            return null;
        }

        //读取缓存
        public void ReadCache(List<KInt> cache)
        {
            foreach (KInt key in cache)
            {
				SettingVo vo = DisplayHash[key.Key] as SettingVo;
				vo.CurOption = key.Value;
				vo.LastOption = key.Value;
            }
        }

        /// <summary>
        /// 玩家当前人数设置发生改变
        /// </summary>
        /// <param name="opt"></param>
        public void OnChangePeopleOption(int opt)
        {
			PlayerManager.Instance.OnVisiblePlayerNumChanged(opt);
            CurDisplayVo.CurOption = opt;
			if (opt!=0) {
				CurDisplayVo.LastOption = opt;
			}
            Gate.instance.sendNotification(MsgConstant.MSG_SETTING_PEOPLE_SWITCH);
        }

        /// <summary>
        /// 保存当前人数显示记录
        /// </summary>
        public void SaveDisplayCache()
        {
            foreach (SettingVo vo in DisplayHash.Values)
            {
                CacheManager.GetInstance().SetDisplayOption(vo.Id, vo.CurOption);
            }
            CacheManager.GetInstance().SaveCache();
        }

        public void OpenWindow()
        {
            UIManager.Instance.openWindow(UiNameConst.ui_setting);

            _helps = ConfigDataManager.GetInstance().getHelpConfig().getHelpList();
        }


        public SettingVo FindSettingVoByMapId(int mapId)
        {
            foreach (SettingVo vo in DisplayHash.Values)
            {
                if (vo.MapId==mapId)
                {
                    return vo;
                }
            }
            return DisplayHash[0] as SettingVo;
        }
        public void CloseWindow()
        {
            UIManager.Instance.closeWindow(UiNameConst.ui_setting);
            SaveDisplayCache();
        }

        public static SettingManager Instance
        {
            get {
                if (_instance==null)
                {
                    _instance = new SettingManager();
                }
                return SettingManager._instance; 
            }
        }

        public BetterList<HelpInfo> Helps
        {
            get { return _helps; }
        }

        public HelpInfo GetHelpVoById(int id)
        {
            return ConfigDataManager.GetInstance().getHelpConfig().getHelpInfo(id);
        }

        /// <summary>
        /// 是否隐藏所有玩家
        /// </summary>
        public bool HidePeople
        {
            get { return _hidePeople; }
            set { _hidePeople = value; }
        }
        /// <summary>
        /// 显示玩家数量
        /// </summary>
        public int ShowPeople
        {
            get { return _showPeople; }
            set { _showPeople = value; }
        }
        /// <summary>
        /// 是否显示/隐藏
        /// </summary>
        public bool Hide_Display
        {
            get {
                return CurDisplayVo.CurOption == 0 ? false : true;
            }
        }

        public void ReLogin()
        {
            //TODE...需要添加账号绑定
            SaveDisplayCache();
            GuideManager.Instance.ReLogin();
            FriendManager.Instance.SaveLog();
            MainLogic.sMainLogic.isInGame = false;
            //MainLogic.hasLoadCreateScene = false;
            NetBase.GetInstance().clientDisconnect = false;
            MainLogic.sMainLogic.bRelogin = true;
            CharacterPlayer.character_property.setLevel(0);
            SceneManager.Instance.currentScene = SCENE_POS.NO_IN_SCENE;
            ScenarioManager.Instance.clearAllPlaying(); //清除剧情
            ArenaManager.Instance.ArenaVo.ResultList.Clear();

            SkillTalentManager.Instance.ActiveSkills.Clear();
            SkillTalentManager.Instance.ActiveTalents.Clear();
            SkillTalentManager.Instance.LockSkills.Clear();
            EmailManager.Instance.ClearEmail();
            Global.lastFightMap = new MapDataItem();
            ItemManager.GetInstance().Init(); //清除背包
			EquipmentManager.GetInstance ().Init ();	//清除装备
            UIManager.Instance.closeAllUI();
            UIManager.Instance.closeWindow(UiNameConst.ui_main); //清除主UI
            RaidManager.Instance.PassMapHash.Clear(); //清除通关历史记录
            RaidManager.Instance.resetAllMapVos();
            PetManager.Instance.init();     //清除宠物记录
            WingManager.Instance.init();    //清除翅膀记录
			FastOpenManager.Instance.Clear ();//清除状态记录
			TaskManager.Instance.Clear ();
            if (CameraFollow.sCameraFollow) //清除相机
            {
                GameObject.Destroy(CameraFollow.sCameraFollow.gameObject);
                CameraFollow.sCameraFollow = null;
            }
			BagManager.Instance.Reset ();
			if (CharacterPlayerUI.sPlayerMeUI)
			{
				CharacterPlayerUI.sPlayerMeUI.character_avatar = null;
				Object.Destroy(CharacterPlayerUI.sPlayerMeUI);                
				CharacterPlayerUI.sPlayerMeUI = null;
			}
            if (CharacterPlayer.sPlayerMe) //清除主角
            {
                MessageManager.Instance.my_property = new CharacterProperty();
                CharacterPlayer.sPlayerMe.character_avatar = null;
                Object.Destroy(CharacterPlayer.sPlayerMe.gameObject);              
                Object.Destroy(CharacterPlayer.sPlayerMe);
                CharacterPlayer.sPlayerMe = null;
            }
            
            MainLogic.sMainLogic.suspendGame();
            MainLogic.sMainLogic.activeName(false);
            PlayerManager.Instance.destroyAllPlayerOther();
            EasyTouchJoyStickProperty.ShowJoyTouch(false);
            GCAskLogout logout = new GCAskLogout();
            NetBase.GetInstance().Send(logout.ToBytes(), false);
            NetBase.GetInstance().Close(true);
            BundleItem mapBundle = BundleMemManager.Instance.getItemByModelName(Constant.CREATE_ROLE_SCENE);
            BundleMemManager.Instance.AllSceneBundles.Add(Constant.CREATE_ROLE_SCENE + BundleMemManager.BUNDLE_SUFFIX, mapBundle);
            BundleItem item = BundleMemManager.Instance.getBundleByType(EBundleType.eBundleSelectRole);
            BundleMemManager.Instance.AllSceneBundles.Add(item.bundleName + BundleMemManager.BUNDLE_SUFFIX, item);
            MainLogic.sMainLogic.StartCoroutine(loginEnumarator());
            //Application.LoadLevel(Constant.CREATE_ROLE_SCENE);
        }

        private IEnumerator loginEnumarator()
        {
            string[] keys = BundleMemManager.Instance.AllSceneBundles.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                string bundleName = keys[i];
                System.Object value = BundleMemManager.Instance.AllSceneBundles[bundleName];
                if (value is BundleItem) //就是assetBundle还没有加载
                {
                    BundleItem item = value as BundleItem;
                    if (item.bundleType == EBundleType.eBundleScene || item.bundleType == EBundleType.eBundleSelectRole)
                    {
                        string urlPath = BundleMemManager.Instance.getBundleUrl() + bundleName;
                        WWW www = WWW.LoadFromCacheOrDownload(urlPath, item.bundleVersion);
                        yield return www;
                        AssetBundle assetBundle = www.assetBundle;
                        BundleMemManager.Instance.addBundleToMem(bundleName, assetBundle, item.bundleType);
                    }                   
                }
                else if (value is AssetBundle)
                {                   
                    if (!BundleMemManager.Instance.needResidentInMem(bundleName))
                    {
                        AssetBundle bundle = value as AssetBundle;
                        bundle.Unload(false); //清除原来场景的内存
                        BundleMemManager.Instance.AllSceneBundles.Remove(bundleName);
                    }                    
                }
            }
            Application.LoadLevel(Constant.CREATE_ROLE_SCENE);
            MainLogic.sMainLogic.StopAllCoroutines();
        }
    }
}