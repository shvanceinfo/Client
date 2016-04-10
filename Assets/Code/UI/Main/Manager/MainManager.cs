/**该文件实现的基本功能等
function: 主界面的数据存储管理
author:zyl
date:2014-4-4
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using NetGame;

namespace manager
{
	public class MainManager
	{
		
		private List<ItemInfo> _newItemList = new List<ItemInfo> ();	  //新提示的道具
		private List<ItemInfo> _newItemUseList = new List<ItemInfo> ();//新的可以使用的道具
		
		private  static MainManager _instance;
		
		private MainManager ()
		{
			
		}

		public List<ItemInfo> NewItemUseList {
			get {
				return this._newItemUseList;
			}
			set {
				_newItemUseList = value;
			}
		}

		public List<ItemInfo> NewItemList {
			get {
				return this._newItemList;
			}
			set {
				_newItemList = value;
			}
		}

		public static MainManager Instance {
			get {
				if (_instance == null) {
					_instance = new MainManager ();
				}
				return _instance;
			}
		}
		
		#region 注册事件
		public void RegsiterEvent ()
		{
			EventDispatcher.GetInstance ().PlayerAsset += UpdatePlayerAsset;
			EventDispatcher.GetInstance ().PlayerProperty += UpdatePlayerProperty;
			DataChangeNotifyer.GetInstance ().EventChangeCurrency += UpdateCurrencyChange;
			DataChangeNotifyer.GetInstance ().EventChangeMoney += UpdateMoneyChange;
//			ItemEvent.GetInstance ().EventNewItemChange += NewItemChange;
		}
		#endregion
		
		#region 取消注册事件
		public void RemoveEvent ()
		{
			EventDispatcher.GetInstance ().PlayerAsset -= UpdatePlayerAsset;
			EventDispatcher.GetInstance ().PlayerProperty -= UpdatePlayerProperty;
			DataChangeNotifyer.GetInstance ().EventChangeCurrency -= UpdateCurrencyChange;
			DataChangeNotifyer.GetInstance ().EventChangeMoney -= UpdateMoneyChange;
//			ItemEvent.GetInstance ().EventNewItemChange -= NewItemChange;
		}
		#endregion
		
		
//		void NewItemChange (ItemInfo newItem, IsRefresh isRef)
//		{
//			if (isRef == IsRefresh.Refresh) {
//				NewitemManager.Instance.NewItemShow ();
//			}
//		}
		
		void UpdatePlayerAsset ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_MAIN_UPDATE_ASSET);
		}

		void UpdatePlayerProperty ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_MAIN_UPDATE_PROPERTY); 
		}
    
		void UpdateCurrencyChange (int changeNum)
		{
			Gate.instance.sendNotification (MsgConstant.MSG_MAIN_UPDATE_CURRENCY, changeNum); 
		}
    
		void UpdateMoneyChange (int changeNum)
		{
			Gate.instance.sendNotification (MsgConstant.MSG_MAIN_UPDATE_MONEY, changeNum); 
		}
		
		void UpdateEngery ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_MAIN_UPDATE_ENGERY); 
		}

		public void UpdateFunction(){
			Gate.instance.sendNotification (MsgConstant.MSG_MAIN_UPDATE_FUNCTION); 
		}

		void UpdatePeopleInfo ()
		{
			string spriteName = "";
			//初始化人物头像
			switch (CharacterPlayer.character_property.career) {
			case CHARACTER_CAREER.CC_SWORD:
				spriteName = Constant.Fight_WarriorHandIcon;
				
				break;
			case  CHARACTER_CAREER.CC_ARCHER:
				spriteName = Constant.Fight_ArcherHandIcon;
				break;
			case CHARACTER_CAREER.CC_MAGICIAN:
				spriteName = Constant.Fight_MagicHandIcon;
				break;
			default:
				break;
			}
			Gate.instance.sendNotification (MsgConstant.MSG_MAIN_PEOPLEINFO, spriteName); 
		}
		
		public void Init ()
		{
			this.UpdatePeopleInfo (); //人物信息
			this.UpdateEngery ();//初始化体力
			this.UpdatePlayerAsset ();  //初始化资源数据
			this.UpdatePlayerProperty ();//初始化角色属性数据
		}
		
		
		
	}
}