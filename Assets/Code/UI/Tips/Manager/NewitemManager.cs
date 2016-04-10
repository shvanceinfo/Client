/**该文件实现的基本功能等
function: 实现新物品的管理
author:zyl
date:2014-5-9
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using NetGame;
using model;

namespace manager
{
	public class NewitemManager
	{
		
		private ItemInfo _current;
		private static NewitemManager _instance;
		
		private NewitemManager ()
		{
			 
		}

		public ItemInfo Current {
			get {
				return this._current;
			}
			set {
				_current = value;
			}
		}

		public static NewitemManager Instance {
			get {
				if (_instance == null) {
					_instance = new NewitemManager ();
				}
				return _instance;
			}
 
		}

	 
		/// <summary>
		/// 显示新添加的物品信息
		/// </summary>
		public void NewItemShow ()
		{
			if (!Global.inCityMap ()) {   //不在主城则跳出
				return;
			}

//			Debug.Log (MainManager.Instance.NewItemList.Count);
			if (MainManager.Instance.NewItemList.Count == 0) {  //数量为0则跳出
				return;
			}
			
			UIManager.Instance.openWindow (UiNameConst.ui_new_item);
			Gate.instance.sendNotification (MsgConstant.MSG_NEWITEM_SHOW);//显示新物品提示
		}
		
		public void NewItemUseShow ()
		{
			if (MainManager.Instance.NewItemUseList.Count == 0) {
				return;
			}
			
			if (this.Current == MainManager.Instance.NewItemUseList [0]) {
				return;
			} else {
				this.Current = MainManager.Instance.NewItemUseList [0];
			}
			
			UIManager.Instance.openWindow (UiNameConst.ui_new_item_use);
			Gate.instance.sendNotification (MsgConstant.MSG_NEWITEM_USE_SHOW);//显示新物品使用提示
		}
		
		
		public void NewItemUseCloseUI ()
		{
			MainManager.Instance.NewItemUseList.Clear();
			this.Current = null;
			UIManager.Instance.closeWindow (UiNameConst.ui_new_item_use, true, false);
		}
		
		
		public void QuickEquipItem(){
			ItemManager.GetInstance ().NetPutEquip ((ushort)this.Current.InstanceId);
		}
		
		
		/// <summary>
		/// 快速使用
		/// </summary>
		public void QuickUseItem ()
		{
			SaleManager.Instance.QuickOpenItem (this.Current);  //使用当前的物品
			MainManager.Instance.NewItemUseList.Remove (this.Current); //移除当前的物品
			this.Current = null;
			this.NewItemUseShow (); //显示下一个
		}
		
		
		
		
		
		
		
		
	}
}