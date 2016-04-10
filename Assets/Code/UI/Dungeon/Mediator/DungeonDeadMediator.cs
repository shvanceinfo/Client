/**该文件实现的基本功能等
function: 实现副本死亡的View控制
author:zyl
date:2014-4-1
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
	public class DungeonDeadMediator : ViewMediator
	{

		public DungeonDeadMediator (DungeonDeadView view, uint id = MediatorName.DUNGEONDEAD_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public DungeonDeadView View {
			get {
				if (_viewComponent != null && _viewComponent is DungeonDeadView)
					return _viewComponent as DungeonDeadView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_OPEN_DUNGEONDEAD, 
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_DUNGEON_FAIL,
				MsgConstant.MSG_DUNGEON_BUY_REVIVE,
				MsgConstant.MSG_DUNGEONDEAD_ERROR,
				MsgConstant.MSG_DUNGEON_MAIN_PLAYER_DROP
			};
		}
		
		
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			DungeonDeadView dungeonDeadView = this.View;
			if (dungeonDeadView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_UI:
					UIManager.Instance.closeWindow (UiNameConst.ui_dungeondeadinfo);
					break;
				case MsgConstant.MSG_OPEN_DUNGEONDEAD:
					int price = (int)notification.body;
					dungeonDeadView.ShowDead (price);
					break;
				case MsgConstant.MSG_DUNGEON_FAIL:
					dungeonDeadView.ShowFail ();
					break;
				case MsgConstant.MSG_DUNGEON_BUY_REVIVE:
					DungeonManager.Instance.BuyRevive ();
					break;
				case MsgConstant.MSG_DUNGEONDEAD_ERROR:
					string msginfo = (string)notification.body;
					dungeonDeadView.ShowErr (msginfo);
					break;
				case MsgConstant.MSG_DUNGEON_MAIN_PLAYER_DROP:
					dungeonDeadView.ShowMainPlayerDrop();
					break;
				default:
					break;
				}
				
			}
		}
 
	}
}