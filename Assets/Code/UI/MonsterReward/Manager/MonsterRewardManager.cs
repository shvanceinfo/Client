/**该文件实现的基本功能等
function: 魔物悬赏的数据存储管理
author:zyl
date:2014-06-04
**/
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using NetGame;
using UnityEngine;

namespace manager
{
	/// <summary>
	/// 操作的4种状态，不同状态处理不同的更新方式
	/// </summary>
	public enum MonsterRewardStatus
	{
		Open,
		Update,
		Next,
		Prev
	}

	public class MonsterRewardManager
	{
		private List<MonsterRewardVo> _monsterRewardList;				//队列数据结构
		private Dictionary<int,MonsterRewardVo> _dicMonsterReward ;		//字典数据结构
		private bool _isInit;											//是否已经初始化
		private int _currentIndex;			//当前定位的魔物悬赏索引ID
		private DataReadItem _item;			//道具阅读器
		private GCAskZhuiJi _askZhuiji;		//请求追缉
		private GCAskCurZhuiJiCount _askCurZhuiJiCount;   //请求得到当前的追缉信息
		private MonsterRewardStatus _currentStatus;		  //当前的操作状态
		private bool _isLock;		//是否被锁住了
		private static MonsterRewardManager _instance;
		
		private MonsterRewardManager ()
		{
			this.CurrentStatus = MonsterRewardStatus.Open;
			this._dicMonsterReward = new Dictionary<int, MonsterRewardVo> ();
			this._monsterRewardList = new List<MonsterRewardVo> ();
			this._item = ConfigDataManager.GetInstance ().getItemTemplate (); //得到道具的配置信息
			this._askZhuiji = new GCAskZhuiJi ();
			this._askCurZhuiJiCount = new GCAskCurZhuiJiCount ();
		}
		
		#region 属性

		public bool IsLock {
			get {
				return _isLock;
			}
			set {
				_isLock = value;
			}
		}

		public MonsterRewardStatus CurrentStatus {
			get {
				return _currentStatus;
			}
			set {
				_currentStatus = value;
			}
		}
 
		public bool IsShowBtnNext {
			get {
				if (this.CurrentIndex >= (this.MonsterRewardList.Count - 1)) {
					return false;
				}
				return true;
			}
		}
		
		public bool IsShowBtnPrev {
			get {
				if (this.CurrentIndex <= 0) {
					return false;
				}
				return true;
			}
		}
		
		private GCAskCurZhuiJiCount AskCurZhuiJiCount {
			get {
				return this._askCurZhuiJiCount;
			}
		}

		private GCAskZhuiJi AskZhuiji {
			get {
				return this._askZhuiji;
			}
		}

		public DataReadItem Item {
			get {
				return this._item;
			}
		}
		
		public MonsterRewardVo PrevMonsterRewardVo {
			get {
				if (this.CurrentIndex - 1 < 0) {
					return null;//溢出
				}
				return this.MonsterRewardList [this.CurrentIndex - 1];
			}
		}
		
		public MonsterRewardVo CurrentMonsterRewardVo {
			get {
				if (this.MonsterRewardList.Count == 0) {
					return null;//无
				}
				return this.MonsterRewardList [this.CurrentIndex];
			}
		}
		
		public MonsterRewardVo NextMonsterRewardVo {
			get {
				if (this.CurrentIndex + 1 >= this.MonsterRewardList.Count) {
					return null;//溢出
				}
				return this.MonsterRewardList [this.CurrentIndex + 1];
			}
		}
		
		public int CurrentIndex {
			get {
				return this._currentIndex;
			}
			private set {
				this._currentIndex = value;
			}
		}

		private bool IsInit {
			get {
				return this._isInit;
			}
			set {
				_isInit = value;
			}
		}

		public List<MonsterRewardVo> MonsterRewardList {
			get {
				return this._monsterRewardList;
			}
			set {
				_monsterRewardList = value;
			}
		}

		public Dictionary<int, MonsterRewardVo> DicMonsterReward {
			get {
				return this._dicMonsterReward;
			}
			 
		}

		public static MonsterRewardManager Instance {
			get { 
				if (_instance == null)
					_instance = new MonsterRewardManager ();
				return _instance; 
			}
		}
		#endregion
		 
		
		#region 窗体打开关闭
		public void OpenWindow ()
		{
			if (!this.IsInit) {
				this.Init ();
			}
			this.CurrentStatus = MonsterRewardStatus.Open;   //打开界面则认为是打开状态
			this.InitCurrentIndex (); 	//搜索当前可用的索引
			UIManager.Instance.openWindow (UiNameConst.ui_monster_reward);
			this.GCAskCurZhuiJiCount ();//得到当前所有的追缉次数
			
			 
		}
		
		public void CloseWindow ()
		{
			UIManager.Instance.closeAllUI ();
		}
		
		#endregion
		
		#region 控制器
		/// <summary>
		/// 刷新界面的方式
		/// </summary>
		public void MonsterRewardShow ()  
		{
			switch (this.CurrentStatus) {
			case MonsterRewardStatus.Open:  	 //打开状态，初始化所有追缉令
				this.MonsterRewardShowAll ();
				this.CurrentStatus = MonsterRewardStatus.Update;
				break;
			case MonsterRewardStatus.Update:
				this.MonsterRewardUpdateCurrent ();
				break;
			case MonsterRewardStatus.Next:
				this.MonsterRewardUpdateNext ();
				this.MonsterRewardNext ();
				this.CurrentStatus = MonsterRewardStatus.Update;
				break;
			case MonsterRewardStatus.Prev:
				this.MonsterRewardUpdatePrev ();
				this.MonsterRewardPrev ();
				this.CurrentStatus = MonsterRewardStatus.Update;
				break;
			default:
				break;
			}

			 

		}

		private void MonsterRewardShowAll ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_MONSTER_REWARD_SHOW);
		}

		private void MonsterRewardUpdateCurrent ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_MONSTER_REWARD_UPDATE_CURRENT);  //更新状态只更新当前的追缉令
		}

		private void MonsterRewardNext ()
		{ 
			Gate.instance.sendNotification (MsgConstant.MSG_MONSTER_REWARD_NEXT);
		}

		private void MonsterRewardPrev ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_MONSTER_REWARD_PREV);
		}
		
		private void MonsterRewardUpdateNext ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_MONSTER_REWARD_UPDATE_NEXT);   //更新右边的追缉令界面
		}

		private void MonsterRewardUpdatePrev ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_MONSTER_REWARD_UPDATE_PREV);   //更新左边的追缉令界面
		}
		 
	 
		#endregion
		
		
		
		#region 普通方法
		public void Init ()
		{
			this.IsInit = true;
			this.SortMonsterRewardList ();		
		}
		
		   
		/// <summary>
		/// Sorts the monster reward list.
		/// </summary>
		public void SortMonsterRewardList ()
		{
			this.MonsterRewardList.Sort ((x,y) => {
				return	x.Order.CompareTo (y.Order);
			});
		}
		
		
		
		/// <summary>
		///初始化当前可用的索引
		/// </summary>
		public void InitCurrentIndex ()
		{
			for (int i = this.MonsterRewardList.Count -1; i >=0; i--) {
				bool isTrue = this.MonsterRewardList [i].CheckCondition; //判断条件是否满足
				if (isTrue) {
					this.CurrentIndex = i;
					break;
				}
			}
		}
		
		public void NextPage ()
		{
//			Debug.LogWarning(this.IsLock);
			if (this.IsLock) {
				return;
			}
			if (this.IsShowBtnNext) {
				this.IsLock = true;
				this.CurrentIndex = Mathf.Min (++this.CurrentIndex, this.MonsterRewardList.Count - 1);
				this.CurrentStatus = MonsterRewardStatus.Next; 
				this.MonsterRewardShow();
			}
		}
		
		public void PrevPage ()
		{	
//			Debug.LogWarning(this.IsLock);
			if (this.IsLock) {
				return;
			}
			if (this.IsShowBtnPrev) {
				this.IsLock = true;
				this.CurrentIndex = Mathf.Max (--this.CurrentIndex, 0);
				this.CurrentStatus = MonsterRewardStatus.Prev;
				this.MonsterRewardShow();
			}
		}
		
		#endregion
		
		
		#region 网络通信
		public void GCAskZhuiJi (ZhuiJiType type)
		{
			if (!FeebManager.Instance.CheckIsHave (this.CurrentMonsterRewardVo.ItemId, this.CurrentMonsterRewardVo [MonsterRewardNumType.Item])) {
				return;
			}
			
			NetBase.GetInstance ().Send (this.AskZhuiji.ToBytes (type, (uint)this.CurrentMonsterRewardVo.Id), true);
		}
		
		public void GCAskCurZhuiJiCount ()
		{
			NetBase.GetInstance ().Send (this.AskCurZhuiJiCount.ToBytes (), true);
		}
		
		
		#endregion
		
		
		
		
	}
	
}