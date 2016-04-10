/**该文件实现的基本功能等
function: 实现装备tips的View控制
author:zyl
date:2014-4-17
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
	public class EquipTipsMediator : ViewMediator
	{
		public EquipTipsMediator (EquipTipsView view, uint id = MediatorName.EQUIP_TIPS_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public EquipTipsView View {
			get {
				if (_viewComponent != null && _viewComponent is EquipTipsView)
					return _viewComponent as EquipTipsView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_EQUIP_TIPS_SHOWINFO
			};
		}
 
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			EquipTipsView equipTipsView = this.View;
			if (equipTipsView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_UI:
					UIManager.Instance.closeAllUI ();
					break;
				case MsgConstant.MSG_EQUIP_TIPS_SHOWINFO:
					TipsCommand cmd = (TipsCommand)notification.body;
					equipTipsView.ShowInfo(TipsManager.Instance.EquipInfo,TipsManager.Instance.Iteminfo,cmd);
					break;
					
				default:
					break;
				}
				
			}
			
		}
	}
}
