using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using helper;
using NetGame;
using model;

namespace manager
{
	public class FastOpenManager
	{
		private Hashtable _fastOpenHash;
		private List<FastOpenVo>  _fastOpenlist; //所有的功能信息队列
		private Dictionary<OpenType,int> _dirOpenTypeValue; //开启条件类型对应的参数值
		private List<FastOpenVo>  _functionEffectList;
		private HashSet<FastOpenVo> _prevLevelFunctionSet;	//上级所拥有的功能队列
		private bool isFastAddFriend;
		private bool _isLogin = true;
		private string _waitForAddName;
		private int _waitForOpenId;

		public int WaitForOpenId {
			get { return _waitForOpenId; }
			set { _waitForOpenId = value; }
		}
		/// <summary>
		/// 标志位有效的时候，不打开UI
		/// </summary>
		public bool IsFastAddFriend {
			get { return isFastAddFriend; }
			set { isFastAddFriend = value; }
		}

		private GCAskFriendRecord _ask110;

		private FastOpenManager ()
		{
			this._prevLevelFunctionSet = new HashSet<FastOpenVo> ();
			this._functionEffectList = new List<FastOpenVo> ();
			this._dirOpenTypeValue = new Dictionary<OpenType, int> ();
			this._fastOpenlist = new List<FastOpenVo> ();
			IsFastAddFriend = false;
			_fastOpenHash = new Hashtable ();
			_ask110 = new GCAskFriendRecord ();
		}

        #region 字段

		public HashSet<FastOpenVo> PrevLevelFunctionSet {
			get {
				return _prevLevelFunctionSet;
			}

		}

		public bool IsLogin {
			get {
				return _isLogin;
			}
			private set {
				_isLogin = value;
			}
		}

		public List<FastOpenVo> FunctionEffectList {
			get {
				return _functionEffectList;
			}
		}

		public Hashtable FastOpenHash {
			get { return _fastOpenHash; }
		}

		public List<FastOpenVo> FastOpenList {
			get {
				return _fastOpenlist;
			}
		}

		public Dictionary<OpenType, int> DirOpenTypeValue {
			get {
				return _dirOpenTypeValue;
			}
		}
        #endregion

        #region 单例
		private static FastOpenManager _instance;

		public static FastOpenManager Instance {
			get {
				if (_instance == null)
					_instance = new FastOpenManager ();
				return _instance;
			}
		}
        #endregion

		/// <summary>
		///  切场景需要还原的数据
		/// </summary>
		public void Reset ()
		{
			this.FunctionEffectList.Clear ();
			this.IsLogin = true;
		}

		/// <summary>
		/// 还原初始数据
		/// </summary>
		public void Clear ()
		{
			this.FunctionEffectList.Clear ();
			this.PrevLevelFunctionSet.Clear ();
			this.DirOpenTypeValue.Clear ();
			this.IsLogin = true;
		}

		public void Init ()
		{
			Sort (); //排序位置和位置的顺序
		}

		void Sort ()
		{
			this.FastOpenList.Sort ((x, y) => {
				if (x.Location < y.Location) {
					return -1;
				} else
					if (x.Location == y.Location) {
					return y.order.CompareTo (x.order);
				} else {
					return 1;
				}
			});
		}

		public void UpdateFunction ()
		{
			if (Global.inCityMap ()) {
				MainManager.Instance.UpdateFunction ();
			}
			if (Global.inFightMap ()) {
				FightManager.Instance.UpdateFunction ();
			}
 
		}

		public void AddFunctionEffect (FastOpenVo vo)
		{
			if (this.IsLogin) {
				this.PrevLevelFunctionSet.Add (vo);							//初始化上级的功能队列
				return;
			}
			if (vo.IsNotice) {
				if (!this.PrevLevelFunctionSet.Contains (vo)) {				//如果当前等级不存在的功能需要弹效果
					FastOpenManager.Instance.FunctionEffectList.Add (vo);
					this.PrevLevelFunctionSet.Add (vo);
				}
			}
		}


		/// <summary>
		/// 创建效果的界面
		/// </summary>
		public void OpenNewFunctionWindow ()
		{
			if (this.IsLogin) {
				this.IsLogin = false; //第一次登陆完以后置位
				return;
			}

			if (FastOpenManager.Instance.FunctionEffectList.Count == 0) {
				return;
			}

			UIManager.Instance.openWindow (UiNameConst.ui_new_function);
		}

		public void CloseNewFunctionWindow ()
		{
			UIManager.Instance.closeWindow (UiNameConst.ui_new_function);
		}

		/// <summary>
		/// Checks the function is open.
		/// </summary> 
		/// <returns><c>true</c>, if function is open was checked, <c>false</c> otherwise.</returns>
		/// <param name="key">int.</param>
		public bool this [int key] {
			get {
				FastOpenVo vo = FastOpenManager.Instance.FastOpenHash [key] as FastOpenVo; //得到当前功能的值
				if (FastOpenManager.Instance.DirOpenTypeValue.ContainsKey (vo.Type) == false) {
					return false;
				}
				if (vo.Param > FastOpenManager.Instance.DirOpenTypeValue [vo.Type]) {
					return false;
				} else {
					return true;
				}
			}
		}

		/// <summary>
		/// Checks the function is open.
		/// </summary>
		/// <returns><c>true</c>, if function is open was checked, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		public bool CheckFunctionIsOpen (int key, bool showMessage=true)
		{
			if (!FastOpenManager.Instance [key]) {
				if (showMessage) {
					FastOpenVo vo = FastOpenManager.Instance.FastOpenHash [key] as FastOpenVo; //得到当前功能的值
					FloatMessage.GetInstance ().PlayFloatMessage (string.Format (LanguageManager.GetText ("msg_function_need_level_open"), vo.Value),
					                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);	
				}
				return false;
			}
			return true;
		}

		public void CleraModelCamera ()
		{
			NPCManager.Instance.createCamera (false);
		}

		public void OpenWindow (int id, bool closeAllUi=true)
		{
			if (!Global.inCityMap ()) {
				ViewHelper.DisplayMessageLanguage ("fastopen_not_in_city");
				return;
			}

			if (!this.CheckFunctionIsOpen (id)) {  //确认功能是否开启
				return;
			}

			WaitForOpenId = id;
			if (closeAllUi) {
				UIManager.Instance.closeAllUI ();
			}
//			EasyTouchJoyStickProperty.ShowJoyTouch (true);
			switch (id) {
			case FunctionName.Role:
				RoleManager.Instance.OpenRole ();
				break;
			case FunctionName.BAG:
				BagManager.Instance.OpenBag ();
				break;
			case FunctionName.SKILL:
				UIManager.Instance.openWindow (UiNameConst.ui_skill);
				Gate.instance.sendNotification (MsgConstant.MSG_SKILL_TABLE_SWITCHING, 1);
				break;
			case FunctionName.Talent:
				UIManager.Instance.openWindow (UiNameConst.ui_skill);
				Gate.instance.sendNotification (MsgConstant.MSG_SKILL_TABLE_SWITCHING, 2);
				break;
			case FunctionName.Strengthen:
				UIManager.Instance.openWindow (UiNameConst.ui_equip);
				UIManager.Instance.getUIFromMemory (UiNameConst.ui_equip).transform.FindChild ("Table/Table1").GetComponent<UICheckBoxColor> ().isChecked = true;
				Gate.instance.sendNotification (MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table1);
				break;
			case FunctionName.Advanced:
				UIManager.Instance.openWindow (UiNameConst.ui_equip);
				UIManager.Instance.getUIFromMemory (UiNameConst.ui_equip).transform.FindChild ("Table/Table2").GetComponent<UICheckBoxColor> ().isChecked = true;
				Gate.instance.sendNotification (MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table2);
				break;
			case FunctionName.Refine:
				UIManager.Instance.openWindow (UiNameConst.ui_equip);
				UIManager.Instance.getUIFromMemory (UiNameConst.ui_equip).transform.FindChild ("Table/Table3").GetComponent<UICheckBoxColor> ().isChecked = true;
				Gate.instance.sendNotification (MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table3);
				break;
			case FunctionName.Inlay:
				UIManager.Instance.openWindow (UiNameConst.ui_equip);
				UIManager.Instance.getUIFromMemory (UiNameConst.ui_equip).transform.FindChild ("Table/Table4").GetComponent<UICheckBoxColor> ().isChecked = true;
				Gate.instance.sendNotification (MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table4);
				break;
			case FunctionName.Task:
				UIManager.Instance.openWindow (UiNameConst.ui_task);
				break;
			case FunctionName.Merge:
				FurnaceManager.OpenWindow ();
				UIManager.Instance.getUIFromMemory(UiNameConst.ui_furnace).transform.FindChild("Table/Table1").GetComponent<UICheckBoxColor>().isChecked = true;
				Gate.instance.sendNotification (MsgConstant.MSG_FURNACE_SWING_TABLE, Table.Table1);
				break;
			case FunctionName.Medal:
				UIManager.Instance.openWindow(UiNameConst.ui_medal);
				Gate.instance.sendNotification(MsgConstant.MSG_MEDAL_DISPLAY_VIEW, Table.Table2);
				break;
			case FunctionName.Wing:
				UIManager.Instance.openWindow (UiNameConst.ui_wing);
				WingManager.Instance.askWingMsg ();
				break;
			case FunctionName.Demon:
				DemonManager.Instance.RequestDemonInfo ();
				break;
			case FunctionName.Global:
				UIManager.Instance.openWindow (UiNameConst.ui_golden_goblin);
				MessageManager.Instance.SendAskGoldenGoblinTimes ();
				break;
			case FunctionName.Arena:
				ArenaManager.Instance.askArenaInfo ();
				break;

			case FunctionName.Rank:
				Debug.LogError ("Rank Not Edit!");
				break;
			case FunctionName.Setting:
				SettingManager.Instance.OpenWindow ();   
				break;
			case FunctionName.Shop:
				ShopManager.Instance.AskShopData ();
				break;
			case FunctionName.Emial:
				EmailManager.Instance.RequestEmailList ();
				break;
			case FunctionName.HotList:
				EventManager.Instance.OpenWindow ();
				break;
			case FunctionName.Group: 
				DungeonManager.Instance.ShowDungeonView ();
				break;
			case FunctionName.Aoding:
				EventManager.Instance.OpenWindow ();
				break;
			case FunctionName.Friend:
				FriendManager.Instance.OpenWindow ();
				break;
			case FunctionName.Grild:
				Debug.LogError ("Grild Not Edit!");
				break;
			case FunctionName.NormalDragon:
				RaidManager.Instance.initRaid ();
				break;
			case FunctionName.HardDragon:
				RaidManager.Instance.initRaid ();
				Gate.instance.sendNotification (MsgConstant.MSG_RAID_BTN_CLICK_HARD);
				break;
			case FunctionName.TaskDragon:
				Debug.LogError ("TaskDragon Not Edit!");
				break;
			case FunctionName.VIPDragon:
				Debug.LogError ("VIPDragon Not Edit!");
				break;
			case FunctionName.GuestShop:
				Debug.LogError ("GuestShop Not Edit!");
				break;
			case FunctionName.Award:
				Debug.LogError ("Award Not Edit!");
				break;
			case FunctionName.KingOfCosmos:
				Debug.LogError ("Solo Not Edit!");
				break;
			case FunctionName.PetLevelUp:
				PetManager.Instance.OpenWindow ();
				break;
			case FunctionName.PetEquip:
				Debug.LogError ("PetEquip Not Edit!");
				break;
			case FunctionName.PetSkill:
				Debug.LogError ("PetSkill Not Edit!");
				break;
			case FunctionName.VIP:
				VipManager.Instance.OpenWindow ();
				break;
			case FunctionName.Active:

				break;
			case FunctionName.Shop_BuyDiamond:
				ShopManager.Instance.AskShopData (SellShopType.Diamon);
				break;
			
			case FunctionName.MonsterReward:
				MonsterRewardManager.Instance.OpenWindow ();
				break;

			case FunctionName.SelectLine:

				break;

			}
		}


		/// <summary>
		/// 快速私聊
		/// </summary>
		/// <param name="name"></param>
		public void OpenWhisper (string name)
		{
			UIManager.Instance.closeAllUI ();
			TalkManager.Instance.OpenWindow ();
			TalkManager.Instance.SetWhisperName (name);
		}

		/// <summary>
		/// 快速添加好友
		/// </summary>
		/// <param name="name"></param>
		public void FastAddFriend (string name)
		{
			if (name.Equals (CharacterPlayer.character_property.getNickName ())) {
				ViewHelper.DisplayMessageLanguage ("friend_error_is_own");
				return;
			}
			if (!string.IsNullOrEmpty (name)) {
				_waitForAddName = name;
			}
			//查询数据
			IsFastAddFriend = true;
			NetBase.GetInstance ().Send (_ask110.ToBytes ());
		}

		//数据到达。进行查询操作
		public void FriendDataReceiveed ()
		{
			IsFastAddFriend = false;
			FriendManager.Instance.AddFriendEx (_waitForAddName);
		}


	}

	public class FunctionName
	{
		/// <summary>
		/// 角色面板
		/// </summary>
		public const int Role = 900101;

		/// <summary>
		/// 背包面板
		/// </summary>
		public const int BAG = 900201;

		/// <summary>
		/// 技能面板
		/// </summary>
		public const int SKILL = 900301;

		/// <summary>
		/// 天赋面板
		/// </summary>
		public const int Talent = 900302;

		/// <summary>
		/// 强化
		/// </summary>
		public const int Strengthen = 900401;
		/// <summary>
		/// 进阶
		/// </summary>
		public const int Advanced = 900402;
		/// <summary>
		/// 洗练
		/// </summary>
		public const int Refine = 900403;
		/// <summary>
		/// 镶嵌
		/// </summary>
		public const int Inlay = 900404;

		/// <summary>
		/// 任务
		/// </summary>
		public const int Task = 900501;

		/// <summary>
		/// 合成
		/// </summary>
		public const int Merge = 900601;

		/// <summary>
		/// 勋章
		/// </summary>
		public const int Medal = 900602;

		/// <summary>
		/// 羽翼
		/// </summary>
		public const int Wing = 900701;

		/// <summary>
		/// 恶魔洞窟
		/// </summary>
		public const int Demon = 900801;

		/// <summary>
		/// 哥布林
		/// </summary>
		public const int Global = 900901;

		/// <summary>
		/// 竞技场
		/// </summary>
		public const int Arena = 901001;

		/// <summary>
		/// 排行榜
		/// </summary>
		public const int Rank = 901101;
		/// <summary>
		/// 系统
		/// </summary>
		public const int Setting = 901201;
		/// <summary>
		/// 商城
		/// </summary>
		public const int Shop = 901301;
		/// <summary>
		/// 邮件
		/// </summary>
		public const int Emial = 901401;
		/// <summary>
		/// 活动列表
		/// </summary>
		public const int HotList = 901501;
		/// <summary>
		/// 组队副本
		/// </summary>
		public const int Group = 901502;
		/// <summary>
		/// 魔化奥丁
		/// </summary>
		public const int Aoding = 901503;
		/// <summary>
		/// 好友
		/// </summary>
		public const int Friend = 901601;
		/// <summary>
		/// 公会
		/// </summary>
		public const int Grild = 901701;

		
		/// <summary>
		/// 公会建设列表
		/// </summary>
		public const int GuildList = 901702;
		
		/// <summary>
		/// 公会建设-公会大厅升级
		/// </summary>
		public const int GuildHallLevelUp = 901703;
		
		/// <summary>
		/// 公会建设-公会旗帜升级
		/// </summary>
		public const int GuildFlagLevelUp = 901704;
		
		/// <summary>
		/// 公会建设-公会技能塔升级
		/// </summary>
		public const int GuildSkillLevelUp = 901705;
		
		/// <summary>
		/// 公会建设-公会商城升级
		/// </summary>
		public const int GuildShopLevelUp = 901706;
		
		/// <summary>
		/// 公会建筑-公会大厅
		/// </summary>
		public const int GuildHall = 901707;
		
		/// <summary>
		/// 公会建筑-公会旗帜
		/// </summary>
		public const int GuildFlag = 901708;
		
		/// <summary>
		/// 公会建筑-公会技能塔
		/// </summary>
		public const int GuildSkillTower = 901709;
		
		/// <summary>
		/// 公会建筑-公会商城
		/// </summary>
		public const int GuildShop = 901710;


		/// <summary>
		/// 普通副本
		/// </summary>
		public const int NormalDragon = 901801;
		/// <summary>
		/// 精英副本
		/// </summary>
		public const int HardDragon = 901802;
		/// <summary>
		/// 剧情副本
		/// </summary>
		public const int TaskDragon = 901901;
		/// <summary>
		/// VIP副本
		/// </summary>
		public const int VIPDragon = 902001;
		/// <summary>
		/// 神秘商店
		/// </summary>
		public const int GuestShop = 902101;
		/// <summary>
		/// 龙之宝藏
		/// </summary>
		public const int Award = 902201;
		/// <summary>
		/// 三界霸主
		/// </summary>
		public const int KingOfCosmos = 902301;
		/// <summary>
		/// 宠物升级
		/// </summary>
		public const int PetLevelUp = 902401;
		/// <summary>
		/// 宠物装备
		/// </summary>
		public const int PetEquip = 902402;
		/// <summary>
		/// 宠物技能
		/// </summary>
		public const int PetSkill = 902403;

		/// <summary>
		/// 潘多拉魔盒
		/// </summary>
		public const int Pandora = 902501;

		/// <summary>
		/// VIP
		/// </summary>
		public const int VIP = 902601;

		/// <summary>
		/// 活跃度
		/// </summary>
		public const int Active = 902701;

		/// <summary>
		/// 商城-购买钻石界面
		/// </summary>
		public const int Shop_BuyDiamond = 902801;

		/// <summary>
		/// 魔物悬赏
		/// </summary>
		public const int MonsterReward = 902901;

		/// <summary>
		/// 分线
		/// </summary>
		public const int SelectLine = 903001;

	 


	}

 
 	 
			 
 
 
  
}
