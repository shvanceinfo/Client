/**该文件实现的基本功能等
function: 副本的数据存储管理
author:zyl
date:2014-3-18
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using NetGame;
using helper;

namespace manager
{
	public enum DungeonStatus
	{
		Open,	//打开副本
		View	//浏览副本
	}
	
	public class DungeonManager
	{
		private static  DungeonManager _instance;
		private BetterList<MapDataItem> _dungeonList = new BetterList<MapDataItem> ();
		private MapDataItem _dungeonItem;
		private DataReadItem _item;
		private DataReadMap _map;
		
		private DataReadRelive _relive;
		
		private int _currentKeyIndex;//当前显示的副本主键索引
		private int _maxKeyIndex;    //可以显示的最大副本索引
		private DungeonVo _dungeonVo;
		private bool _currentDungeonBtnCanUse = false;//当前的副本是否可以启用加入队伍的按钮
		private BetterList<uint> _itemKeyList = new BetterList<uint> ();
		private DungeonStatus _currentStatus = DungeonStatus.Open;
		private GCPreviewMulti _gcPreviewMulti;
		private GCAskCreateTeam _gcAskCreateTeam;
		private GCQuickAddTeam _gcQuickAddTeam;
		private GCAskAddTeam _gcAskAddTeam;
		private GCAskLeaveTeam _gcAskLeaveTeam;
		private GCAskBeginFightMulti _gcBeginFight;
		
		private DungeonManager ()
		{
			#region 配置文件
			this._map = ConfigDataManager.GetInstance ().getMapConfig (); //得到地图的配置信息
			InitDungeonList ();
			this._item = ConfigDataManager.GetInstance ().getItemTemplate (); //得到道具的配置信息
			this._relive = ConfigDataManager.GetInstance ().GetReliveConfig (); //得到复活的配置信息
			#endregion
			
 			
			#region 网络通信
			this._gcPreviewMulti = new GCPreviewMulti ();
			this._gcAskCreateTeam = new GCAskCreateTeam ();
			this._gcQuickAddTeam = new GCQuickAddTeam ();
			this._gcAskAddTeam = new GCAskAddTeam ();
			this._gcAskLeaveTeam = new GCAskLeaveTeam ();
			this._gcBeginFight = new GCAskBeginFightMulti ();
			#endregion
			
			this._dungeonVo = new DungeonVo ();
			
		}
		
		#region 各种属性
		//复活
		public DataReadRelive Relive {
			get {
				return this._relive;
			}
			set {
				_relive = value;
			}
		}
		 	
		//物品KEY列表
		public BetterList<uint> ItemKeyList {
			get {
				return this._itemKeyList;
			}
		}
		
		//地图列表
		public DataReadMap Map {
			get {
				return this._map;
			}
		}

		//副本列表
		public BetterList<MapDataItem> DungeonList {
			get {
				return this._dungeonList;
			}
		}
		//当前显示的副本
		public MapDataItem CurrentDungeonItem {
			get {
				return this._dungeonItem;
			}
		}
		
		public DataReadItem Item {
			get {
				return this._item;
			}
		}

		public bool CurrentDungeonBtnCanUse {
			get {
				return this._currentDungeonBtnCanUse;
			}
			set {
				_currentDungeonBtnCanUse = value;
			}
		}
		
		public DungeonVo DungeonVo {
			get {
				return this._dungeonVo;
			}
		}
		
		//是否能显示下个按钮
		public bool CanShowNextBtn {
			get {
				if (this._currentKeyIndex == this._maxKeyIndex) {
					return false;
				}
				return true;
			}
		}
		
		//是否能显示前一个按钮
		public bool	CanShowPrevBtn {
			get {
				if (this._currentKeyIndex == 0) {
					return false;
				}
				return true;
			}
		}
		#endregion
		
 
		public static DungeonManager Instance {
			get {
				if (_instance == null) {
					_instance = new DungeonManager ();
				}
				return _instance;
				
			}
		}
		
		//初始化副本队列
		void InitDungeonList ()
		{
			foreach (int key in this._map.Keys) {
				var mapDate = this._map.getMapData (key);
				if (mapDate.mapCate == MapCate.Dungeon) { //验证是否是多人副本的地图
					this._dungeonList.Add (mapDate);
				}
			}
			//排序副本队列
			this._dungeonList.Sort (delegate	(MapDataItem x, MapDataItem y){
				if (x.nEnterLevel > y.nEnterLevel) {
					return 1;
				} else if (x.nEnterLevel == y.nEnterLevel) {
					return 0;
				} else {
					return -1;
				}
			});
		}
		
		
		//初始化主键队列和当前的副本信息
		void InitKeyListAndCurrentDungeon ()
		{
			for (int i = 0; i < _dungeonList.size; i++) {
				if (CharacterPlayer.character_property.level >= _dungeonList [i].nEnterLevel) {
					this._dungeonItem = _dungeonList [i]; //设置当前打的副本
					this._currentKeyIndex = i;
					this._maxKeyIndex = Mathf.Min (this._currentKeyIndex + 1, _dungeonList.size - 1);//保证最大值不能超过队列的最大值
				}	
			}//选中离等级最近的那个副本
			

			if (this._dungeonItem == null) {//等级低于第一个副本的情况
				this._dungeonItem = this._dungeonList [0];//得到第一条数据
				this._currentKeyIndex = 0;
				this._maxKeyIndex = 0;
			}
		
			this.InitItemList();//规格化道具
		}
		
		public void InitItemList(){
			#region 初始化道具主键队列 
			this._itemKeyList.Release ();
			var itemArray = this._dungeonItem.dropItem.Split (',');
			foreach (string i in itemArray) {
				this.ItemKeyList.Add (uint.Parse (i.Trim ()));
			}
			#endregion
		}
		
		
		
		#region 网络通信
		//请求得到副本的预览信息 
		public	void AskPreviewMulti ()
		{
			NetBase.GetInstance ().Send (this._gcPreviewMulti.ToBytes ((uint)this.CurrentDungeonItem.id), true);
		}
		
		//请求创建副本队伍
		public void AskCreateTeam ()
		{
			NetBase.GetInstance ().Send (this._gcAskCreateTeam.ToBytes ((uint)this.CurrentDungeonItem.id), true);
		}
		
		//请求快速进入副本
		public void QuickAddTeam ()
		{
			NetBase.GetInstance ().Send (this._gcQuickAddTeam.ToBytes (), true);
		}
		
		//请求加入队伍
		public void AskAddTeam (uint teamid)
		{
			NetBase.GetInstance ().Send (this._gcAskAddTeam.ToBytes (teamid), true);
		}
		
		//开始战斗
		public void BeginBattle ()
		{
			NetBase.GetInstance ().Send (this._gcBeginFight.ToBytes (), true);
		}
		
		//请求离开队伍
		public void AskLeaveTeam ()
		{
			NetBase.GetInstance ().Send (this._gcAskLeaveTeam.ToBytes (), true);
		}
		#endregion
		
		
		
		//初始化副本
		public void InitDungeon ()
		{
			//通过配置文件初始化副本界面
			if (this.CurrentDungeonItem != null) { //更新当前的界面
				this.RefreshDungeon ();
			}
		}
		
		//初始化副本详细信息
		public void InitDungeonInfo ()
		{
			//通过配置文件初始化副本界面
			if (this.CurrentDungeonItem != null) { //更新当前的界面
				this.UpdateDungeon ();		//更新副本信息
				this.UpdateDungeonPeopleList ();
			}
		}
		
		
		
		
		
		//刷新副本需要执行的方法
		private void RefreshDungeon ()
		{
			this.UpdateDungeon ();		//更新副本信息
			this.UpdateDungeonTeamList ();//更新副本对应的队伍信息
		}
		
		
		//更新副本
		public void UpdateDungeon ()
		{
			IList<ItemTemplate> list = new List<ItemTemplate> ();
			
			
			foreach (var itemid in this.ItemKeyList) {      //更新道具队列
				var itemModel = this.Item.getTemplateData ((int)itemid);
				if (!string.IsNullOrEmpty (itemModel.icon)) {
					list.Add (itemModel);
				}
			}
			ArrayList arrayList = new ArrayList (2);
			arrayList.Add (this.CurrentDungeonItem);
			arrayList.Add (list);
			arrayList.Add (this.DungeonVo.PassNum);//通关次数
			this.CheckCurrentDungeonBtnCanUse ();
			Gate.instance.sendNotification (MsgConstant.MSG_UPDATE_DUNGEON, arrayList);
		}
		
		//查看下个副本
		public void NextDungeon ()
		{
			this._currentKeyIndex = Mathf.Min (++this._currentKeyIndex, this._maxKeyIndex);
			this._dungeonItem = this._dungeonList [this._currentKeyIndex];
			this.InitItemList();
			this.ViewDungeonView ();
			//this.RefreshDungeon ();
		}
		
		//查看上个副本
		public void PrevDungeon ()
		{
			this._currentKeyIndex = Mathf.Max (--this._currentKeyIndex, 0);
			this._dungeonItem = this._dungeonList [this._currentKeyIndex];
			this.InitItemList();
			this.ViewDungeonView ();
			//this.RefreshDungeon ();
		}
		
		//更新副本队伍的信息 
		public void UpdateDungeonTeamList ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_UPDATE_TEAM_LIST);
		}
		
		//更新副本的人员信息
		public void UpdateDungeonPeopleList ()
		{
			
			#region 规格化人员信息队列
			SortPeopleList ();
			#endregion

			Gate.instance.sendNotification (MsgConstant.MSG_UPDATE_DUNGEON_PEOPLE_LIST);
		}
		
		//确认是否要显示按钮
		public void CheckCurrentDungeonBtnCanUse ()
		{
			
			if (CharacterPlayer.character_property.level >= this.CurrentDungeonItem.nEnterLevel) {
				this.CurrentDungeonBtnCanUse = true;
			} else {
				this.CurrentDungeonBtnCanUse = false;
			}
 
		}
		
		//显示副本界面并且请求信息
		public void ShowDungeonView ()
		{
			InitKeyListAndCurrentDungeon ();//初始化主键队列
			UIManager.Instance.openWindow (UiNameConst.ui_duoren);
			//请求服务器得到副本当前的队伍列表
			this.ViewDungeonView (); //打开副本界面
		}
		
		//显示副本界面并且请求信息
		public void ShowDungeonViewByView ()
		{
			UIManager.Instance.closeWindow (UiNameConst.ui_dungeoninfo,true,true);
			UIManager.Instance.openWindow (UiNameConst.ui_duoren);
			//请求服务器得到副本当前的队伍列表
			this.ViewDungeonView (); //打开副本界面
		}
		
		#region 打开界面的方式
		public void ViewDungeonView ()
		{
			this.AskPreviewMulti ();
		}
		#endregion
		
 
		//显示副本详细信息界面
		public void ShowDungeonInfoView ()
		{
			UIManager.Instance.openWindow (UiNameConst.ui_dungeoninfo); //打开副本详细信息界面
		}
		
		//关闭所有界面
		public void CloseAllView ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_UI);
		}
		
		/// <summary>
		/// Closes the dungeon U.
		/// </summary>
		public void CloseDungeonUI(){
			UIManager.Instance.closeWindow(UiNameConst.ui_duoren,true,true);
		}
		
		//根据队长排序队列
		public void SortPeopleList ()
		{
			int index = 0;
			for (int i = 0; i < this.DungeonVo.PeopleList.size; i++) {
				if (this.DungeonVo.PeopleList [i].leader) {
					index = i;
					break;
				}
			}//找出索引ID
			if (index != 0) {
				this.DungeonVo.PeopleList.Insert (0, this.DungeonVo.PeopleList [index]);
				this.DungeonVo.PeopleList.RemoveAt (index + 1);
			}
		}
		
		//开始倒计时
		public void ShowCD ()
		{
			#region 重新设置网络ping 的时间
			this.SetDungeonPing ();
			#endregion
			Gate.instance.sendNotification (MsgConstant.MSG_SHOW_CD);
		}
		
		//设置副本ping
		public void SetDungeonPing ()
		{
			MainLogic.sMainLogic.CancelPing ();
			MainLogic.sMainLogic.ChangeToDungeonPing ();
		}
		
		//设置正常值的ping
		public void SetNormalPing ()
		{
			MainLogic.sMainLogic.CancelPing ();
			MainLogic.sMainLogic.ChangeToNormalPing ();
		}
		
		//显示死亡界面
		public void ShowDead ()
		{
			UIManager.Instance.openWindow (UiNameConst.ui_dungeondeadinfo);
			Gate.instance.sendNotification (MsgConstant.MSG_OPEN_DUNGEONDEAD,this.Relive.GetReliveData (Global.requestBornNum).dia_price);
		}
		
		//显示全部死亡界面
		public void ShowFail ()
		{
			UIManager.Instance.openWindow (UiNameConst.ui_dungeondeadinfo);
			Gate.instance.sendNotification (MsgConstant.MSG_DUNGEON_FAIL);
		}

		//显示主机掉线界面
		public void ShowMainPlayerDrop(){
			UIManager.Instance.openWindow (UiNameConst.ui_dungeondeadinfo);
			Gate.instance.sendNotification (MsgConstant.MSG_DUNGEON_MAIN_PLAYER_DROP);
		}


		//摄像机跟随好友
		public void CameraFollowFriend ()
		{
			// 自己挂了需要走摄像机调节 随机选择其他玩家
            for (int i = 0; i < PlayerManager.Instance.player_other_list.Count; ++i)
            {
                CharacterPlayerOther other = PlayerManager.Instance.player_other_list[i];
                if (other != null)
                {
                    if (!CharacterAI.IsInState(other, CharacterAI.CHARACTER_STATE.CS_DIE))
                    {
                        CameraFollow.sCameraFollow.SetBindTran(other.transform);
                        return;
                    }
                }
            }
		}
		
		//跟随自己
		public void CameraFollowMe ()
		{

			CameraFollow.sCameraFollow.SetBindTran (CharacterPlayer.sPlayerMe.transform);
		}
		
		
		
		//购买复活
		public void BuyRevive ()
		{
            if (Global.requestBornNum > VipManager.Instance.BornCount)
            {
                ViewHelper.DisplayMessageLanguage("vip_broncount");
                return;
            }
			int needPrice = this.Relive.GetReliveData (Global.requestBornNum).dia_price;
			if (CharacterPlayer.character_asset.diamond >= needPrice) { //如果身上砖石足够则复活
                MessageManager.Instance.sendMessageBorn(ReliveType.Asset, eGoldType.zuanshi, (uint)needPrice);
				Global.requestBornNum += 1;
				UIManager.Instance.closeWindow (UiNameConst.ui_dungeondeadinfo, true);//关闭死亡界面
			} else {
				DungeonManager.Instance.DiamondNotEnough ();
			}	
//					UIManager.Instance.ShowDialog (eDialogSureType.eSureBuyRevive,
//                    string.Format (LanguageManager.GetText ("msg_buy_golden_goblin_ticket"), price));
		}
		
		//钻石不够
		public void DiamondNotEnough ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_DUNGEONDEAD_ERROR, "dungeon_diamond_not_enough");
 
		}
		
		//返回主城
		public void BackCity(){
            MessageManager.Instance.sendMessageReturnCity();
			UIManager.Instance.showWaitting(true); //回主城强制弹出对话框
		}
		
	}
}
