/**该文件实现的基本功能等
function: 实现出售的View控制
author:zyl
date:2014-4-15
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using MVC.interfaces;
using manager;
using model;
using NetGame;

namespace mediator
{
	public class SaleMediator : ViewMediator
	{

		public SaleMediator (BasicView view, uint id = MediatorName.SALE_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public BasicView View {
			get {
				if (_viewComponent != null && _viewComponent is BasicView)
					return _viewComponent as BasicView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_SALE_SHOWINFO,
				MsgConstant.MSG_SALE_MINUS,
				MsgConstant.MSG_SALE_ADD,
				MsgConstant.MSG_SALE_MAX,
				MsgConstant.MSG_SALE_UPDATEINFO,
				MsgConstant.MSG_SALE_ITEM,
				MsgConstant.MSG_OPEN_ITEM
			};
		}
 
 
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			BasicView basicView = this.View;
			if (basicView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_UI:
					UIManager.Instance.closeAllUI ();
					break;
				case MsgConstant.MSG_SALE_SHOWINFO:
					basicView.ShowSaleInfo (SaleManager.Instance.Iteminfo.Item, SaleManager.Instance.CurrentNum);
					break;
				case MsgConstant.MSG_SALE_MINUS:
					SaleManager.Instance.ItemMinus ();
					break;
				case  MsgConstant.MSG_SALE_ADD:
					SaleManager.Instance.ItemAdd ();
					break;
				case MsgConstant.MSG_SALE_MAX:
					SaleManager.Instance.ItemMax ();
					break;
				case MsgConstant.MSG_SALE_UPDATEINFO:
					basicView.UpdateSaleInfo (SaleManager.Instance.Iteminfo.Item, SaleManager.Instance.CurrentNum);
					break;	
				case MsgConstant.MSG_SALE_ITEM:
					SaleManager.Instance.SaleCurrentItem();
					break;
				case MsgConstant.MSG_OPEN_ITEM:
					SaleManager.Instance.OpenCurrentItem();
					break;
						
					
				default:
					break;
				}
				
			}
			
		}
	}
}
