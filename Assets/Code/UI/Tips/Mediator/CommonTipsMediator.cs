/**该文件实现的基本功能等
function: 实现tips的View控制
author:zyl
date:2014-4-14
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
	public class CommonTipsMediator : ViewMediator
	{

		public CommonTipsMediator (CommonTipsView view, uint id = MediatorName.COMMON_TIPS_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public CommonTipsView View {
			get {
				if (_viewComponent != null && _viewComponent is CommonTipsView)
					return _viewComponent as CommonTipsView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_COMMON_TIPS_SHOWINFO
			};
		}
 
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			CommonTipsView commonTipsView = this.View;
			if (commonTipsView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_UI:
					UIManager.Instance.closeAllUI ();
					break;
				case MsgConstant.MSG_COMMON_TIPS_SHOWINFO:
					TipsCommand cmd = (TipsCommand)notification.body;
					commonTipsView.ShowInfo(TipsManager.Instance.Iteminfo.Item,cmd);
					break;
					
				default:
					break;
				}
				
			}
			
		}
	}
}