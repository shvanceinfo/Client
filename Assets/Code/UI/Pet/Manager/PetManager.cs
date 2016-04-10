/**该文件实现的基本功能等
function: 宠物的数据存储管理
author:zyl
date:2014-06-03
**/
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using NetGame;
using System.Diagnostics;
using UnityEngine;

namespace manager
{
	/// <summary>
	/// Pet tab enum.
	/// </summary>
	public enum PetTabEnum
	{
		/// <summary>
		/// The attribute.
		/// </summary>
		Attribute = 1,  
		/// <summary>
		/// The equip.
		/// </summary>
		Equip = 2
	}

	public class PetManager
	{
		private uint _maxLadder; 	//当前最大的宠物
		private uint _previewLadder; //当前预览的宠物
		private uint _currentLuckNum; //当前幸运值
		private uint _currentLevel; //当前的宠物
		private PetVo _currentPet;  //当前跟随的宠物对象
		private PetVo _maxPet;		//最大的宠物队伍
		private float _currentTime; //当前时间
		
		//net message
		private GCAskPetEvolution _askEvolution;
		private GCAskPetInfo _askInfo;
		private GCSelectPet _selectPet;
		private GCAskEquipPet _askEquipPet;
		private bool _openPetAsk;
		private Dictionary<uint,PetVo> _dictionaryPet;//主键对应的键值对
		private PetTabEnum _currentTab;
		private static PetManager _instance;
		
		private PetManager ()
		{
			_dictionaryPet = new Dictionary<uint, PetVo> ();
			_askEvolution = new GCAskPetEvolution ();
			_askInfo = new GCAskPetInfo ();
			this._selectPet = new GCSelectPet ();
			this._askEquipPet = new GCAskEquipPet ();
			init ();
		}

		public void init ()
		{
			this._maxLadder = 0;
			this._maxPet = null;
			_openPetAsk = false;
			_currentTime = 0f;
			_currentLuckNum = 0;
			_previewLadder = 0;
			_currentLuckNum = 0;
			_currentPet = null;
			_currentLevel = 0;
		}
		
		#region 属性



		private GCAskEquipPet AskEquipPet {
			get {
				return _askEquipPet;
			}
		}

		public PetTabEnum CurrentTab {
			private set {
				this._currentTab = value;
			}
			get {
				return _currentTab;
			}
		}

		public PetVo MaxPet {
			get {
				return _maxPet;
			}
			set {
				_maxPet = value;
			}
		}

		public uint MaxLadder {
			get {
				return _maxLadder;
			}
			set {
				_maxLadder = value;
			}
		}

		private GCSelectPet GCSelectPet {
			get {
				return _selectPet;
			}
		}
		 
		public Dictionary<uint, PetVo> DictionaryPet {
			get {
				return this._dictionaryPet;
			}
		}

		private GCAskPetEvolution AskEvolution {
			get {
				return this._askEvolution;
			}
		}

		private GCAskPetInfo AskInfo {
			get {
				return this._askInfo;
			}
			
		}

		public uint CurrentLevel {
			get {
				return this._currentLevel;
			}
			private set {
				this._currentLevel = value;
			}
		}

		public uint CurrentLuckNum {
			get {
				return this._currentLuckNum;
			}
			set {
				_currentLuckNum = value;
			}
		}

		public PetVo CurrentPet {
			get {
				return this._currentPet;
			}
			private set {
				this._currentPet = value;
			}
		}

		public float CurrentTime {
			get {
				return this._currentTime;
			}
			set {
				_currentTime = value;
			}
		}

		public bool OpenPetAsk {
			get {
				return this._openPetAsk;
			}
			private set {
				_openPetAsk = value;
			}
		}

		public uint PreviewLadder {
			get {
				return this._previewLadder;
			}
			set {
				_previewLadder = value;
			}
		}
	
		public static PetManager Instance {
			get { 
				if (_instance == null)
					_instance = new PetManager ();
				return _instance; 
			}
		}
		#endregion
		
		
		
		#region 网络通信
		//请求宠物初始数据
		public void AskPetMsg ()
		{
			NetBase.GetInstance ().Send (_askInfo.ToBytes (), true);
			_openPetAsk = true;
		}

		public void AskEvolutionMsg ()
		{
			NetBase.GetInstance ().Send (_askEvolution.ToBytes (), false);
		}

		public void SelectPet (uint petId)
		{
			NetBase.GetInstance ().Send (this.GCSelectPet.ToBytes (petId), false);
		}

		public void GCAskEquipPet (ushort bagPos)
		{
			NetBase.GetInstance ().Send (this.AskEquipPet.ToBytes (bagPos), false);
		}
		#endregion
		
		
		
		#region 控制器模块
		public void UpdateModelLadder ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_UPDATE_MODEL_LADDER, new List<string>{this.CurrentPet.PetName,  this.CurrentPet.PetModle});
		}

		public void UpdateModelMaxLadder ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_UPDATE_MODEL_LADDER, new List<string>{this.MaxPet.PetName,  this.MaxPet.PetModle});
		}

		public void StopAuto ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_STOP_AUTO); //进阶成功停止自动进阶
		}

		public void ShowEffect (string num, string msg)
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_SHOW_EFFECT, new List<string>{num, LanguageManager.GetText (msg)});
		}
 
		public void PetInit ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_INIT);
		}
		
		
		//更新幸运石
		private void UpdateLuck ()
		{ 
			Gate.instance.sendNotification (MsgConstant.MSG_PET_UPDATE_LUCK, new ArrayList{this.CurrentLuckNum, this.MaxPet.HighLimit});
		}

		public  void ShowTime (ArrayList al)
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_SHOW_TIME, al);
		}

		public  void SetCamera ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_SET_CAMERA, false);
		}
		
		public void ShowEvolution ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_SHOW_EVOLUTION);
		}

		public  void NotEnoughMsg ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_NOT_ENOUGH_MSG, "pet_not_enough_money");
		}

		public void PetSwitchTab ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_SWITCH_TAB);
		}

		public void ShowPetEquip ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_SHOWPETEQUIP);
		}

		/// <summary>
		/// Shows the equiped data.
		/// </summary>
		public void ShowEquipedData ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_PET_SHOWPETEQUIPITEM);//显示装备数据
		}
		/// <summary>
		/// 更新属性信息
		/// </summary>
		public void UpdatePreviewAttr(){
			Gate.instance.sendNotification (MsgConstant.MSG_PET_UPDATE_PREVIEW_ATTR); 
		}


		#endregion
		
		#region 打开窗口
		public void OpenWindow (PetTabEnum tab = PetTabEnum.Attribute)
		{
			UIManager.Instance.openWindow (UiNameConst.ui_pet);
			this.TabSelect (tab);
			this.ShowEquipedData ();        //显示左边区域
			this.AskPetMsg ();
		}

		/// <summary>
		/// 选择开始进入的TAB
		/// </summary>
		/// <param name="tab">Tab.</param>
		private void TabSelect (PetTabEnum tab)
		{
			switch (tab) {
			case PetTabEnum.Attribute:
				this.ShowAttr();
				break;
			case PetTabEnum.Equip:
				this.ShowEquip();
				break;
			default:
				break;
			}
		}


		/// <summary>
		/// Switchs to attr.
		/// </summary>
		public void SwitchToAttr ()
		{
			this.CurrentTab = PetTabEnum.Attribute;
			this.PetSwitchTab ();
		}
		/// <summary>
		/// Switchs to equip.
		/// </summary>
		public void SwitchToEquip ()
		{
			this.CurrentTab = PetTabEnum.Equip;
			this.PetSwitchTab ();
		}

		/// <summary>
		/// Shows the attr.
		/// </summary>
		public void ShowAttr ()
		{
			this.SwitchToAttr ();
		}

		/// <summary>
		/// Shows the equip.
		/// </summary>
		public void ShowEquip ()
		{
			this.SwitchToEquip ();
			BagManager.Instance.ShowPetEquipItem (); //背包排序
			this.ShowPetEquip ();  //显示背包数据
		}

		/// <summary>
		/// 刷新宠物装备界面
		/// </summary>
		public void RefreshEquip ()
		{
			if (PetManager.Instance.CurrentTab == PetTabEnum.Equip) {
				PetManager.Instance.ShowEquip ();  //刷新界面
			}
		}

		#endregion
		
		
		//初始化设置当前跟随的宠物
		public void SetCurrentPet (uint petId)
		{
			if (this.DictionaryPet.ContainsKey (petId)) {
				this.CurrentPet = this.DictionaryPet [petId];
				Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_DISPLAY_PET_SKILL);
			}
		}

		//初始化设置当前最高等级的宠物
		public void SetMaxPet (uint petId)
		{
			if (this.DictionaryPet.ContainsKey (petId)) {
				this.MaxPet = this.DictionaryPet [petId];
			}
		}


		//初始化宠物的界面
		public void InitPet (uint petId, uint luckNum, uint selectPetId)
		{
			bool evoSuccess = false; //默认非进阶

			if (this.CurrentPet == null) {
				this.SetCurrentPet (selectPetId);
				this.SetMaxPet (petId);
				// 创建自己的宠物
//				CharacterPlayer.sPlayerMe.CreatePet(selectPetId);
				return;
			}

			if (this.CurrentPet.Id != selectPetId) {//如果选择的不同，则需要换宠物
				CharacterPlayer.sPlayerMe.CreatePet (selectPetId);
			}

			if (!_openPetAsk && petId != this.MaxPet.Id) //进阶成功
				evoSuccess = true;

		
			uint luckDelta = 0;
			if (luckNum > this.CurrentLuckNum)
				luckDelta = luckNum - this.CurrentLuckNum;  
			this.CurrentLuckNum = luckNum;   //设置当前的幸运值
 		
			this.MaxLadder = petId / 1000;
			this.CurrentLevel = selectPetId / 1000;
			this.SetCurrentPet (selectPetId);	 //设置当前宠物数据
			this.SetMaxPet (petId);

			if (this.OpenPetAsk) { //初始化界面需要重置模型以及阶数
				this.OpenPetAsk = false;
				this.PetInit ();   //初始化   
				UpdateModelLadder ();
			} else {
				if (evoSuccess) { //进阶成功换宠物模型跟名字
					StopAuto ();
					ShowEffect ("1", LanguageManager.GetText ("pet_evo_success"));
					UpdateModelMaxLadder ();
					Gate.instance.sendNotification (MsgConstant.MSG_PET_SUCCESS);
				} else if (luckDelta > 0) { //不满十阶不发生变化，直接提升
					ShowEffect ("1", LanguageManager.GetText ("pet_evo_per_time") + luckDelta);				
				}
				this.ShowEvolution ();
			}
			   
			this.UpdateLuck ();		//更新幸运石
//			this.ShowTime ();			//显示时间
		}
		
 
		
		//开始进阶，是否自动进阶
		public void BeginEvolute (bool isAuto=false)
		{
			if (CanEvolute ())
				this.AskEvolutionMsg ();
			else if (isAuto) //如果是自动，要停止相应线程
				this.StopAuto ();
		}
 
 
		
		//能否进行进阶操作
		private bool CanEvolute ()
		{
			if (!FeebManager.Instance.CheckIsHave ((uint)this.MaxPet.EvoCostItem, (int)this.MaxPet.EvoNum)) {
				SetCamera ();
				return false;
			}
            
			if (CharacterPlayer.character_asset.gold < this.MaxPet.CostGold) {
				NotEnoughMsg ();
				return false;
			} else			
				return true;
		}
		
		/// <summary>
		/// 显示时间 
		/// </summary>
		public void ShowTime ()
		{
			ArrayList al = new ArrayList ();
			al.Add (this.CurrentLuckNum);
			al.Add (this.MaxPet.IsLimit);
			ShowTime (al);
		}
		
		/// <summary>
		/// 设置宠物跟随
		/// </summary>
		public void SetFollowPet ()
		{
			uint petid = this.PreviewLadder * 1000 + 1;
			this.OpenPetAsk = true;
			this.SelectPet (petid);
		}

		 

	}
	
}