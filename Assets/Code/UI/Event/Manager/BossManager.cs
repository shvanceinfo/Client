/**该文件实现的基本功能等
function: 世界BOSS胜利的数据存储管理
author:zyl
date:2014-5-13
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using NetGame;
using System.Text;

namespace manager
{
// 	<RECORD ID="1003001" ModelID="3" ModelDesc="世界BOSS" FunctionID="1" FunctionDesc="复活倒计时-（秒）" Type="2" Value="30"/>
//	<RECORD ID="1003002" ModelID="3" ModelDesc="世界BOSS" FunctionID="2" FunctionDesc="资源复活价格-（资源枚举，数量）" Type="1" Value="2,20"/>
//	<RECORD ID="1003006" ModelID="3" ModelDesc="世界BOSS" FunctionID="6" FunctionDesc="普通鼓舞消耗的资源类型及数值-（资源枚举，数量）" Type="1" Value="3,40"/>
//	<RECORD ID="1003007" ModelID="3" ModelDesc="世界BOSS" FunctionID="7" FunctionDesc="钻石鼓舞消耗的资源类型及数值-（资源枚举，数量）" Type="1" Value="2,20"/>
	public enum PublicBossData
	{
		Revive = 1003001,
		RevivePrice = 1003002,
		NormalBuff = 1003006,
		DiamondBuff = 1003007,
		BuffPerValue = 1003004		//鼓舞每次提升属性的千分比-（千分比）
	}
	
	public class BossManager
	{
		private uint _bossTempId;
		private MapDataItem _mapData;
		private MonsterDataItem _WorldBossData;
		private BossDamageVo _bossDamageVo;
		private BossWinVo _bossWinVo;
		private bool _isUpdateBossDamageUI = false;
		private PublicDataItem _reviveTimeData;
		private PublicDataItem _revivePriceData;
		private PublicDataItem _normalBuffData;
		private PublicDataItem _diamondBuffData;
		private PublicDataItem _buffPerValData;
		private eGoldType _goldType;
		private int _revivePrice;
		private int _cdTime;
		private bool _isInWorldBoss = false;
		private byte _buffCount;
		private GCAskUseItemInWorldBoss _askUseItemInWorldBoss;
		private static object uiUpdate = new object ();
		private static  BossManager _instance;
		
		private BossManager ()
		{
			_buffPerValData = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData ((int)PublicBossData.BuffPerValue); //得到每次鼓舞提升属性的千分比
			this._bossDamageVo = new BossDamageVo ();
			this._bossWinVo = new BossWinVo ();
			this._askUseItemInWorldBoss = new GCAskUseItemInWorldBoss ();
			this.Init ();
		}
		#region 属性
		/// <summary>
		/// buff提升的百分比值
		/// </summary>
		/// <value>The buff value.</value>
		public int BuffValue{
			get{
				  
				double buffVal = (double)_buffPerValData.type2Data / 1000; //得到提升的千分比值
				buffVal*=100;						//最终的百分比
				
				return  (int)buffVal*this.BuffCount;
			}
		}
		
		public byte BuffCount {
			get {
				return _buffCount;
			}
			private set {
				_buffCount = value;
			}
		}

		public GCAskUseItemInWorldBoss AskUseItemInWorldBoss {
			get {
				return this._askUseItemInWorldBoss;
			}
		}

		public bool IsInWorldBoss {
			get {
				return this._isInWorldBoss;
			}
			set {
				_isInWorldBoss = value;
			}
		}

		public MapDataItem MapData {
			get {
				if (this._mapData == null) {
					int mapId = MessageManager.Instance.my_property.getServerMapID ();
					this._mapData = ConfigDataManager.GetInstance ().getMapConfig ().getMapData (mapId);
				}
				return this._mapData;
			}
		}

        public int WorldBossTime()
        {
            
            if(EventManager.Instance.curActiveVo != null)
            {
                int nowTimeHour = System.DateTime.Now.Hour;
                int nowTimeMinute = System.DateTime.Now.Minute;
                int bossTime = 0;
                EventVo vo = EventManager.Instance.curActiveVo;
                for (int i = 0; i < vo.Schedule.Count; i++)
                {
                    if (nowTimeHour >= (int)vo.Schedule[i].kDayTime.u8BeginHour && nowTimeHour <= (int)vo.Schedule[i].kDayTime.u8EndHour)
                    {
                        bossTime = ((int)vo.Schedule[i].kDayTime.u8EndHour - nowTimeHour) * 3600 +
                            ((int)vo.Schedule[i].kDayTime.u8EndMinute - nowTimeMinute) * 60;

                        return bossTime;
                    }
                }
            }
            return 0;
        }

		public int CdTime {
			get {
				return this._cdTime;
			}
			private set {
				_cdTime = value;
			}
		}

		public eGoldType GoldType {
			get {
				return this._goldType;
			}
			private set {
				this._goldType = value;
			}
		}

		public PublicDataItem RevivePriceData {
			get {
				if (this._revivePriceData == null) {
					this._revivePriceData = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData ((int)PublicBossData.RevivePrice);
				}
				return this._revivePriceData;
			}
		}

		public PublicDataItem ReviveData {
			get {
				if (this._reviveTimeData == null) {
					this._reviveTimeData = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData ((int)PublicBossData.Revive);
				}
				return this._reviveTimeData;
			}
		}
		
		public PublicDataItem NormalBuffData {
			get {
				if (this._normalBuffData == null) {
					this._normalBuffData = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData ((int)PublicBossData.NormalBuff);
				}
				return this._normalBuffData;
			}
		}
		
		public PublicDataItem DiamondBuffData {
			get {
				if (this._diamondBuffData == null) {
					this._diamondBuffData = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData ((int)PublicBossData.DiamondBuff);
				}
				return this._diamondBuffData;
			}
		}

		public bool IsUpdateBossDamageUI {
			get {
				return this._isUpdateBossDamageUI;
			}
			set {
				_isUpdateBossDamageUI = value;
			}
		}

		public uint BossTempId {
			get {
				return this._bossTempId;
			}
			set {
				_bossTempId = value;
			}
		}

		public MonsterDataItem WorldBossData {
			get {
				if (this._WorldBossData == null) {
					this._WorldBossData = ConfigDataManager.GetInstance ().getMonsterConfig ().getMonsterData ((int)this._bossTempId);
				}
				
				return this._WorldBossData;
			}
		}

		public BossDamageVo BossDamageVo {
			get {
				return this._bossDamageVo;
			}
		}

		public BossWinVo BossWinVo {
			get {
				return this._bossWinVo;
			}
			set {
				_bossWinVo = value;
			}
		}

		public int RevivePrice {
			get {
				return this._revivePrice;
			}
			private set {
				this._revivePrice = value;
			}
		}

		public static  BossManager Instance {
			get {
				if (_instance == null) {
					_instance = new BossManager ();
				}
				return _instance;
			}
		}
		
		#endregion
		
 
		#region mediator方法
		public void WorldBossUpdateInfo ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_WORLD_BOSS_UPDATE_INFO);
		}
		
		public void SwitchBossDamageBtn ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_BOSS_BTN_SWITCH);
		}
		
		private void BossWinShow ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_BOSS_WIN_SHOW);
		}
		
		public void BossDeadShow ()
		{ 
			Gate.instance.sendNotification (MsgConstant.MSG_BOSS_DEAD_SHOW);
		}
		
		//钻石不够
		public void DiamondNotEnough ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_BOSS_DEAD_ERROR, "dungeon_diamond_not_enough");
		}
		
		public void GoldNotEnough ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_BOSS_DEAD_ERROR, "dungeon_gold_not_enough");
		}
		
		public void CrystalNotEnough ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_BOSS_DEAD_ERROR, "dungeon_crystal_not_enough");
		}
		
		public void UpdateBuffCountInfo(){
			Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_BUFF_COUNT);
		}
		
		#endregion
		
		
		#region 注册事件
		public void BossDeadRegisterEvent ()
		{
			EventDispatcher.GetInstance ().DialogSure += DialogSure;					 //弹出对话框的事件
			EventDispatcher.GetInstance ().DialogCancel += DialogCancel;				 //弹出对话框的事件
		}
		
		public void BossDeadCancelEvent ()
		{
			EventDispatcher.GetInstance ().DialogSure -= DialogSure;
			EventDispatcher.GetInstance ().DialogCancel -= DialogCancel;
		}
		
		#endregion
		
		#region 事件
		
		public void DialogSure (eDialogSureType type)
		{
			MessageManager.Instance.sendMessageReturnCity ();
			UIManager.Instance.showWaitting (true); //回主城强制弹出对话框
		}
		 
		public void DialogCancel (eDialogSureType type)
		{
			
			 
		}
		#endregion
		
 
		
		#region UI操作
		/// <summary>
		/// 更新BOSS伤害列表UI
		/// </summary>
		public void UpdateWorldBossInfoUI ()
		{
			if (!this.IsInWorldBoss) {
				return;
			}
			if (this.IsUpdateBossDamageUI == false) {
				
				lock (uiUpdate) {
					if (this.IsUpdateBossDamageUI == false) {
						this.IsUpdateBossDamageUI = true;
						this.WorldBossUpdateInfo ();
					}
				}
			}
		}
		
		public void ShowBossWinUI ()
		{
			UIManager.Instance.closeAllUI ();
			NPCManager.Instance.createCamera (false); //消除3D相机
			UIManager.Instance.openWindow (UiNameConst.ui_boss_win);
			this.BossWinShow ();

		}
		
		public void CloseBossWinUI ()
		{
			UIManager.Instance.closeWindow (UiNameConst.ui_boss_win);
		}
		
		public void ShowPeopleDeadUI ()
		{
			UIManager.Instance.openWindow (UiNameConst.ui_boss_dead);
			this.BossDeadShow ();
		}
		
		public void ClosePeopleDeadUI ()
		{
			UIManager.Instance.closeWindow (UiNameConst.ui_boss_dead);
		}
		
		
		#endregion
		
		
		
		#region 普通方法
		public void Init ()
		{
			if (this.RevivePriceData.type1List.Count > 0) {
				this.GoldType = (eGoldType)this.RevivePriceData.type1List [0];
				this.RevivePrice = this.RevivePriceData.type1List [1];
			}											//初始化消耗的资源和数量
			this.CdTime = this.ReviveData.type2Data; //初始化CD时间
		}
		
		
		
		//购买复活
		public void BuyRevive ()
		{
			switch (this.GoldType) {
			case eGoldType.zuanshi:
				{
					if (CharacterPlayer.character_asset.diamond >= this.RevivePrice) { //如果身上砖石足够则复活
						MessageManager.Instance.sendMessageBorn (ReliveType.Asset, eGoldType.zuanshi, (uint)this.RevivePrice);
						this.ClosePeopleDeadUI ();//关闭死亡界面
					} else {
						this.DiamondNotEnough ();
					}	
//					UIManager.Instance.ShowDialog (eDialogSureType.eSureBuyRevive,
//                    string.Format (LanguageManager.GetText ("msg_buy_golden_goblin_ticket"), price));
				}
				break;
			case eGoldType.gold:
				{
					if (CharacterPlayer.character_asset.gold >= this.RevivePrice) { //如果身上砖石足够则复活
						MessageManager.Instance.sendMessageBorn (ReliveType.Asset, eGoldType.gold, (uint)this.RevivePrice);
						this.ClosePeopleDeadUI ();//关闭死亡界面
					} else {
						this.GoldNotEnough ();
					}	
				}
				break; 
			case eGoldType.shuijing:
				{
					if (CharacterPlayer.character_asset.Crystal >= this.RevivePrice) { //如果身上砖石足够则复活
						MessageManager.Instance.sendMessageBorn (ReliveType.Asset, eGoldType.shuijing, (uint)this.RevivePrice);
						this.ClosePeopleDeadUI ();//关闭死亡界面
					} else {
						this.CrystalNotEnough ();
					}	
				}
				break;
			 
			default:
				break;
			}
 
		}
		
		
 
		public void TimeRevive ()
		{
			MessageManager.Instance.sendMessageBorn (); //时间复活
			this.ClosePeopleDeadUI ();//关闭死亡界面
		}
		
		
		
		
		//返回主城
		public void BackCity ()
		{
			UIManager.Instance.ShowDialog (eDialogSureType.eSureBackCity,
                    LanguageManager.GetText ("leave_dungeon"));
            
		}
		
		public void WinBackCity ()
		{
			MessageManager.Instance.sendMessageReturnCity ();
			UIManager.Instance.showWaitting (true); //回主城强制弹出对话框
		}
		
		public int GetLerpValue (int fromV, int toV, int meDamage)
		{
			return  Mathf.RoundToInt (Mathf.InverseLerp (fromV, toV, meDamage) * 100);
		}
	
		public string GetLerpValueString (int val, params string[] preStr)
		{
			StringBuilder sb = new StringBuilder ();
			for (int i = 0,max = preStr.Length; i < max; i++) {
				sb.Append (preStr [i]);
			}
			sb.Append ("[").Append (val).Append ("%]").ToString ();
			return sb.ToString ();
		}
		
		public void SetInWorldBossStatus ()
		{
			BossManager.Instance.IsInWorldBoss = true;
			BossManager.Instance.IsUpdateBossDamageUI = false;//重置世界BOSS UI刷新的状态
		}
		
		public void SetNotInWorldBossStatus ()
		{
			BossManager.Instance.IsInWorldBoss = false;
			BossManager.Instance.IsUpdateBossDamageUI = false;//重置世界BOSS UI刷新的状态
		}

		public void UpdateBuffInfo(){
			if (!this.IsInWorldBoss) {
				return;
			}

			this.BuffCount++;
			this.UpdateBuffCountInfo();
		}

		/// <summary>
		/// 更新buff的信息
		/// </summary>
		public void UpdateBuffInfo(byte count){

			this.BuffCount = count;
		}
		
		#endregion
		
		#region 网络通信
		public void UseItemInWorldBoss (UseItemInWorldBossType type)
		{
			switch (type) {
			case UseItemInWorldBossType.Rongyu:
				
				if (CharacterPlayer.character_asset.Honor<BossManager.Instance.NormalBuffData.type1List[1]) {
					FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("msg_world_boss_honor_not_enough"),
					                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
					return;	
				}
				
				break;
			case UseItemInWorldBossType.Zhuanshi:
				if (CharacterPlayer.character_asset.diamond<BossManager.Instance.DiamondBuffData.type1List[1]) {
					FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("msg_world_boss_diamond_not_enough"),
					                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
					return;
				}
				break;
				
			default:
				break;
			} 
 
			NetBase.GetInstance ().Send (this.AskUseItemInWorldBoss.ToBytes ((int)type), false);
		}
		
		
		#endregion

	}
	
}
