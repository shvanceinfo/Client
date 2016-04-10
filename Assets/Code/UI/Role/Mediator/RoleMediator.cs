/**该文件实现的基本功能等
function: 实现角色的View控制
author:zyl
date:2014-4-12
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
	public class RoleMediator : ViewMediator
	{

		public RoleMediator (RoleView view, uint id = MediatorName.ROLE_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public RoleView View {
			get {
				if (_viewComponent != null && _viewComponent is RoleView)
					return _viewComponent as RoleView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_ROLE_SHOWCAREERMODEL,
				MsgConstant.MSG_ROLE_HIDECAREERMODEL,
				MsgConstant.MSG_ROLE_SHOWEQUIPDATA,
				MsgConstant.MSG_ROLE_SHOWPEOPLEINFO
			};
		}
 
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			RoleView roleView = this.View;
			if (roleView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_UI:
					UIManager.Instance.closeAllUI ();
					break;
				case MsgConstant.MSG_ROLE_SHOWCAREERMODEL:
					roleView.ShowCareerModel (RoleManager.Instance.ModelName, RoleManager.Instance.ModelPos); //因为左边数据和背包数据一样，所以使用背包数据
					break;
				case MsgConstant.MSG_ROLE_HIDECAREERMODEL:
					roleView.HideCareerModel ();
					break;
				case MsgConstant.MSG_ROLE_SHOWEQUIPDATA:
					roleView.ShowEquip (RoleManager.Instance.EquipData);
                    roleView.ChangeVipLevel();
					break;
				case MsgConstant.MSG_ROLE_SHOWPEOPLEINFO:
					roleView.ShowPeopleInfo();
					break;
					
					
				default:
					break;
				}
				
			}
			
		}
	}

}

