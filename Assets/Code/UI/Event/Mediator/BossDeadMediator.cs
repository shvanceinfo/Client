/**该文件实现的基本功能等
function: 实现世界boss死亡的View控制
author:zyl
date:2014-5-15
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
	public class BossDeadMediator : ViewMediator
	{
		public BossDeadMediator (BossDeadView view, uint id = MediatorName.BOSS_DEAD_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public BossDeadView View {
			get {
				if (_viewComponent != null && _viewComponent is BossDeadView)
					return _viewComponent as BossDeadView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_BOSS_DEAD_SHOW,
				MsgConstant.MSG_BOSS_DEAD_ERROR,
				MsgConstant.MSG_BOSS_DEAD_BUY_REVIVE,
				MsgConstant.MSG_BOSS_DEAD_BACK_CITY,
				MsgConstant.MSG_BOSS_DEAD_TIME_REVIVE
			};
		}
		
		
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			BossDeadView bossDeadView = this.View;
			if (bossDeadView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_BOSS_DEAD_SHOW:
					bossDeadView.ShowDead (BossManager.Instance.GoldType,BossManager.Instance.RevivePrice,BossManager.Instance.CdTime);
					break;
				case MsgConstant.MSG_BOSS_DEAD_ERROR:
					string msginfo = (string)notification.body;
					bossDeadView.ShowErr (msginfo);
					break;
				case MsgConstant.MSG_BOSS_DEAD_BUY_REVIVE:
					BossManager.Instance.BuyRevive();
					break;
				case MsgConstant.MSG_BOSS_DEAD_BACK_CITY:
					BossManager.Instance.BackCity();
					break;	
				case MsgConstant.MSG_BOSS_DEAD_TIME_REVIVE:
					BossManager.Instance.TimeRevive();
					break;
				default:
					break;
				}
				
			}
			
		}
	}
}