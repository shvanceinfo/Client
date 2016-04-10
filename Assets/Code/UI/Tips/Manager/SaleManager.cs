/**该文件实现的基本功能等
function: 实现出售的管理
author:zyl
date:2014-04-15
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using NetGame;

namespace manager
{
	public class SaleManager
	{
		const int DEFAULT_NUM = 1;
		private ItemInfo _iteminfo;
		private int _currentNum;
		private static SaleManager _instance;
		
		private SaleManager ()
		{
			
		}
		
	

		public ItemInfo Iteminfo {
			get {
				return this._iteminfo;
			}
			private set {
				_iteminfo = value;
			}
		}

		public int CurrentNum {
			get {
				return this._currentNum;
			}
			private set {
				_currentNum = value;
			}
		}

		public static SaleManager Instance {
			get {
				if (_instance == null) {
					_instance = new SaleManager ();
				}
				return _instance;
			}
		}
		
		/// <summary>
		/// Shows the sale info.
		/// </summary>
		public void ShowSaleInfo ()
		{
			UIManager.Instance.openWindow (UiNameConst.ui_sale);
//			BagManager.Instance.HideCareerModel ();
			
			this.Iteminfo = TipsManager.Instance.Iteminfo;
			this.CurrentNum = DEFAULT_NUM;//默认的初始值

			ShowInfo ();
		}
		
		
		public void ShowOpenInfo(){
			
			
			if (CharacterPlayer.character_property.level < TipsManager.Instance.Iteminfo.Item.usedLevel) {
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("msg_use_not_enough_level"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				return ;
			}//验证等级


			this.Iteminfo = TipsManager.Instance.Iteminfo;
			this.CurrentNum = DEFAULT_NUM;//默认的初始值
			if (this.Iteminfo.Num>1) {
				UIManager.Instance.openWindow (UiNameConst.ui_open);
//				BagManager.Instance.HideCareerModel ();
				ShowInfo ();
			} else {    //只有1个的清空下，直接使用
				this.OpenCurrentItem();
			}

		}
		
		
		
		
		/// <summary>
		/// 物品减1
		/// </summary>
		public void ItemMinus ()
		{
			this.CurrentNum = Mathf.Max (DEFAULT_NUM, --this.CurrentNum);
			this.UpdateInfo ();
		}
		/// <summary>
		/// 物品加1
		/// </summary>
		public void ItemAdd ()
		{
			this.CurrentNum = Mathf.Min (++this.CurrentNum, (int)this.Iteminfo.Num);
			this.UpdateInfo ();
		}
		/// <summary>
		/// 物品设置为最大值
		/// </summary>
		public void ItemMax ()
		{
			this.CurrentNum = (int)this.Iteminfo.Num;
			this.UpdateInfo ();
		}
		
		/// <summary>
		/// Shows the info.
		/// </summary>
		public void ShowInfo ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_SALE_SHOWINFO); 
		}
		/// <summary>
		/// Updates the info.
		/// </summary>
		public void UpdateInfo ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_SALE_UPDATEINFO);
		}
		
		
		/// <summary>
		/// 出售当前的item
		/// </summary>
		public void SaleCurrentItem(){
			this.SaleOrOpenItem(FormatItemList (),UseGoodsStatus.Sale );
		}
		
		/// <summary>
		/// 开打当前的item
		/// </summary>
		public void OpenCurrentItem(){
			this.SaleOrOpenItem(FormatItemList (),UseGoodsStatus.Use);
		}
		
		/// <summary>
		/// 得到格式化的item结构
		/// </summary>
		/// <returns>
		/// The item list.
		/// </returns>
		public  List<ItemStruct> FormatItemList ()
		{
			List<ItemStruct> itemlist = new List<ItemStruct>();
			itemlist.Add(new ItemStruct(){
				instanceId = (ushort)this.Iteminfo.InstanceId,
				num = (uint)this.CurrentNum
			});
			return itemlist;
		}
		
		
		
		/// <summary>
		/// Sales the or open item.
		/// </summary>
		/// <param name='itemList'>
		/// Item list.
		/// </param>
		/// <param name='status'>
		/// Status.
		/// </param>
		public void SaleOrOpenItem (IList<ItemStruct> itemList,UseGoodsStatus status)
		{
			GCAskUseGoods useGoods = new GCAskUseGoods(status,(ushort)itemList.Count);
			NetBase.GetInstance ().Send (useGoods.ToBytes(itemList), true);
		}
		
		/// <summary>
		/// 快速使用
		/// </summary>
		/// <param name='item'>
		/// Item.
		/// </param>
		public void QuickOpenItem(ItemInfo item){
			GCAskUseGoods useGoods = new GCAskUseGoods(UseGoodsStatus.Use,1);
			NetBase.GetInstance ().Send (useGoods.ToQuickUseOrSale(item), true);
		}
		
	}
}


