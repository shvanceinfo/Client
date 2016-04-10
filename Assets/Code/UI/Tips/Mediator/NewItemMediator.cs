/**该文件实现的基本功能等
function: 实现新物品的View控制
author:zyl
date:2014-5-9
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
	public class NewItemMediator : ViewMediator
	{

		public NewItemMediator (NewItemView view, uint id = MediatorName.NEWITEM_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public NewItemView View {
			get {
				if (_viewComponent != null && _viewComponent is NewItemView)
					return _viewComponent as NewItemView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_NEWITEM_SHOW
			};
		}
 
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			NewItemView newItemView = this.View;
			if (newItemView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_NEWITEM_SHOW:
					newItemView.Show(MainManager.Instance.NewItemList);
					break;
				default:
					break;
				}
				
			}
			
		}
	}
}