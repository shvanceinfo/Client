/**该文件实现的基本功能等
function: 实现活动的View控制
author:zyl
date:2014-6-14
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
	public class NewFunctionMediator : ViewMediator
	{

		public NewFunctionMediator (NewFunctionView view, uint id = MediatorName.FUNCTION_EFFECT) : base(id, view)
		{
		
		}
	
		public NewFunctionView View {
			get {
				if (_viewComponent != null && _viewComponent is NewFunctionView)
					return _viewComponent as NewFunctionView;
				return  null;		
			}
		}
	
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
	 
			};
		}
	
	
	
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			 
			if (this.View != null) {
				switch (notification.notifyId) {
			 
		 
				default:
					break;
				}
			
			}
		
		}
	 
	}
}
