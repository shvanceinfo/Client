/**该文件实现的基本功能等
function: 实现副本的View控制
author:zyl
date:2014-3-20
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
	public class DungeonInfoMediator : ViewMediator
	{

		public DungeonInfoMediator (DungeonInfoView view, uint id = MediatorName.DUNGEONINFO_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public DungeonInfoView View {
			get {
				if (_viewComponent != null && _viewComponent is DungeonInfoView)
					return _viewComponent as DungeonInfoView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_OPEN_DUNGEONINFO,
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_UPDATE_DUNGEON,
				MsgConstant.MSG_START_DUNGEON,
				MsgConstant.MSG_LEAVE_TEAM,
				MsgConstant.MSG_CLOSE_TEAM,
				MsgConstant.MSG_UPDATE_DUNGEON_PEOPLE_LIST,
				MsgConstant.MSG_SHOW_CD
			};
		}
		
		  
		
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			DungeonInfoView dungeonInfoView = this.View;
			if (dungeonInfoView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_UI:
					UIManager.Instance.closeAllUI ();
					break;
				case MsgConstant.MSG_OPEN_DUNGEONINFO:
					
					break;
 
				case MsgConstant.MSG_UPDATE_DUNGEON:
					ArrayList arraylist = notification.body as ArrayList;
					MapDataItem ddt = arraylist [0] as MapDataItem;
					IList<ItemTemplate> itemList = arraylist [1] as IList<ItemTemplate>;
					ushort passNum = (ushort)arraylist [2];
					dungeonInfoView.UpdateDungeon (ddt, itemList, passNum);
					break;
					
				case MsgConstant.MSG_UPDATE_DUNGEON_PEOPLE_LIST:
					dungeonInfoView.UpdatePeopleList (DungeonManager.Instance.DungeonVo.PeopleList);
					break;
					
				case MsgConstant.MSG_START_DUNGEON:
					DungeonManager.Instance.BeginBattle ();
					break; 
					
				case MsgConstant.MSG_LEAVE_TEAM:
					DungeonManager.Instance.AskLeaveTeam ();
					DungeonManager.Instance.ShowDungeonViewByView ();
					break;
					
				case MsgConstant.MSG_CLOSE_TEAM:
					DungeonManager.Instance.AskLeaveTeam ();
					UIManager.Instance.closeAllUI ();
					break;
					
				case MsgConstant.MSG_SHOW_CD:
					dungeonInfoView.ShowCD ();
					break;
					
					
				default:
					break;
				}
				
			}
			
		}
	}
	
}