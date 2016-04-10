/**该文件实现的基本功能等
function: 实现世界boss胜利的View控制
author:zyl
date:2014-5-13
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
	public class BossWinMediator : ViewMediator
	{
		public BossWinMediator (BossWinView view, uint id = MediatorName.BOSS_WIN_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public BossWinView View {
			get {
				if (_viewComponent != null && _viewComponent is BossWinView)
					return _viewComponent as BossWinView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_BOSS_WIN_SHOW,
				MsgConstant.MSG_BOSS_WIN_BACK_CITY
			};
		}
		
		
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			BossWinView bossWinView = this.View;
			if (bossWinView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_BOSS_WIN_SHOW:
					 bossWinView.Show(BossManager.Instance.BossWinVo);
					break;
				case MsgConstant.MSG_BOSS_WIN_BACK_CITY:
					BossManager.Instance.WinBackCity();
					break;
					
					
				default:
					break;
				}
				
			}
			
		}
	}
	
}