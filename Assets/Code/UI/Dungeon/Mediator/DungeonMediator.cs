/**该文件实现的基本功能等
function: 实现副本的View控制
author:zyl
date:2014-3-18
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
	public class DungeonMediator : ViewMediator
	{
		public DungeonMediator (DungeonView view, uint id = MediatorName.DUNGEON_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public DungeonView View {
			get {
				if (_viewComponent != null && _viewComponent is DungeonView)
					return _viewComponent as DungeonView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_OPEN_DUNGEON,
				MsgConstant.MSG_DUNGEON_INIT,
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_UPDATE_DUNGEON,
				MsgConstant.MSG_DUNGEON_NEXT,
				MsgConstant.MSG_DUNGEON_PREV,
				MsgConstant.MSG_UPDATE_TEAM_LIST,
				MsgConstant.MSG_CREATE_TEAM,
				MsgConstant.MSG_JOIN_TEAM,
				MsgConstant.MSG_QUICK_JOIN
			};
		}
		
		
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			DungeonView dungeonView = this.View;
			if (dungeonView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_UI:
					UIManager.Instance.closeAllUI ();
					break;
				case MsgConstant.MSG_OPEN_DUNGEON:
					DungeonManager.Instance.ShowDungeonView();

					break;
				case MsgConstant.MSG_DUNGEON_INIT:
					break;
				case MsgConstant.MSG_UPDATE_DUNGEON:
					ArrayList arraylist = notification.body as ArrayList;
					MapDataItem ddt = arraylist[0] as MapDataItem;
					IList<ItemTemplate> itemList =  arraylist[1] as IList<ItemTemplate>;
					ushort passNum  = (ushort)arraylist[2];
					dungeonView.UpdateDungeon(ddt,itemList,passNum,DungeonManager.Instance.CurrentDungeonBtnCanUse,
															DungeonManager.Instance.CanShowPrevBtn,DungeonManager.Instance.CanShowNextBtn);
					break;
				case MsgConstant.MSG_DUNGEON_NEXT:
					DungeonManager.Instance.NextDungeon();
					break;
				case MsgConstant.MSG_DUNGEON_PREV:
					DungeonManager.Instance.PrevDungeon();
					break;	
				case MsgConstant.MSG_UPDATE_TEAM_LIST:
					dungeonView.UpdateTeamList(DungeonManager.Instance.DungeonVo.DungeonTeamList,DungeonManager.Instance.CurrentDungeonBtnCanUse);
					break;
				case MsgConstant.MSG_CREATE_TEAM:
					//DungeonManager.Instance.ShowDungeonInfoView();
					DungeonManager.Instance.AskCreateTeam(); //请求创建队伍
					break;
				case MsgConstant.MSG_JOIN_TEAM:
					//DungeonManager.Instance.ShowDungeonInfoView();
					uint teamid = (uint)notification.body;
					DungeonManager.Instance.AskAddTeam(teamid);
					break;
				case MsgConstant.MSG_QUICK_JOIN:
					//DungeonManager.Instance.ShowDungeonInfoView();
					DungeonManager.Instance.QuickAddTeam();
					break;
	
					
				default:
					break;
				}
				
			}
			
		}
		
	 
	}
	
}