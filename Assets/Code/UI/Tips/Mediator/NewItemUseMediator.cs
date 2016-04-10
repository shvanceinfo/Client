/**该文件实现的基本功能等
function: 实现新物品使用的View控制
author:zyl
date:2014-5-16
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
	public class NewItemUseMediator : ViewMediator
	{

		public NewItemUseMediator (NewItemUseView view, uint id = MediatorName.NEWITEM_USE_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public NewItemUseView View {
			get {
				if (_viewComponent != null && _viewComponent is NewItemUseView)
					return _viewComponent as NewItemUseView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_NEWITEM_USE_SHOW
			};
		}
 
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			NewItemUseView newItemUseView = this.View;
			if (newItemUseView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_NEWITEM_USE_SHOW:
					newItemUseView.ShowFirst(NewitemManager.Instance.Current);
					break;
				default:
					break;
				}
				
			}
			
		}
	}

}