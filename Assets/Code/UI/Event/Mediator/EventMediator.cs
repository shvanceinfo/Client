/**该文件实现的基本功能等
function: 实现活动的View控制
author:zyl
date:2014-5-12
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
	public class EventMediator : ViewMediator
	{

		public EventMediator (EventView view, uint id = MediatorName.EVENT_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public EventView View {
			get {
				if (_viewComponent != null && _viewComponent is EventView)
					return _viewComponent as EventView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_EVENT_SHOW,
				MsgConstant.MSG_EVENT_UPDATEINFO,
				MsgConstant.MSG_EVENT_ASKJOIN
			};
		}
		
		
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			EventView eventView = this.View;
			if (eventView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_EVENT_SHOW:
					eventView.Show (EventManager.Instance.KeyList, EventManager.Instance.DictionaryEvent);
					break;
				case MsgConstant.MSG_EVENT_UPDATEINFO:
					EventVo vo = (EventVo)notification.body;
					eventView.UpdateInfo (vo);
					break;
				case MsgConstant.MSG_EVENT_ASKJOIN:
					EventManager.Instance.AskJoinActivity((int)notification.body);
					break;
				default:
					break;
				}
				
			}
			
		}
	}
	
}