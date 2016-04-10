/**该文件实现的基本功能等
function: 实现弹出框的View控制
author:zyl
date:2014-4-5
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
	public class DialogMediator : ViewMediator
	{

		public DialogMediator (DialogView view, uint id = MediatorName.DIALOG_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public DialogView View {
			get {
				if (_viewComponent != null && _viewComponent is DialogView)
					return _viewComponent as DialogView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_DIALOG_SURE,	//确认对话框
				MsgConstant.MSG_DIALOG_CANCEL,	//取消对话框
				MsgConstant.MSG_DIALOG_SHOW		//显示对话框内容
			};
		}
		
 
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			DialogView dialogView = this.View;
			if (dialogView != null) {
				switch (notification.notifyId) {
				case  MsgConstant.MSG_DIALOG_SURE:
					DialogManager.Instance.Sure ();
					break;
				case MsgConstant.MSG_DIALOG_CANCEL:
					DialogManager.Instance.Cancel ();
					break;
				case MsgConstant.MSG_DIALOG_SHOW:
					string msg = (string)notification.body;
					dialogView.Show(msg);
					break;
					
				default:
					break;
				}
				
			}
			
		}
	}
}