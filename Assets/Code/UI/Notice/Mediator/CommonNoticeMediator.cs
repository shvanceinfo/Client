/**该文件实现的基本功能等
function: 实现公共公告的View控制
author:zyl
date:2014-5-5
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
	public class CommonNoticeMediator : ViewMediator
	{

		public CommonNoticeMediator (CommonNoticeView view, uint id = MediatorName.COMMON_NOTICE_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public CommonNoticeView View {
			get {
				if (_viewComponent != null && _viewComponent is CommonNoticeView)
					return _viewComponent as CommonNoticeView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				  MsgConstant.MSG_COMMON_NOTICE_STOPTWEEN,
				  MsgConstant.MSG_COMMON_NOTICE_RESUMETWEEN,
				  MsgConstant.MSG_COMMON_NOTICE_HIDEUI
			};
		}
 
 
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			CommonNoticeView noticeView = this.View;
			if (noticeView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_COMMON_NOTICE_STOPTWEEN:
					noticeView.StopTween ();
					break;
				case MsgConstant.MSG_COMMON_NOTICE_RESUMETWEEN:
					noticeView.ResumeTween ();
					break;
				case MsgConstant.MSG_COMMON_NOTICE_HIDEUI:
					noticeView.HideUI ();
					break;
				default:
					break;
				}
				
			}
			
		}
	}
}