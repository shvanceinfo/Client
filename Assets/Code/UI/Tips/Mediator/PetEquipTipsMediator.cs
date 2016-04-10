/**该文件实现的基本功能等
function: 实现装备tips的View控制
author:zyl
date:2014-7-9
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
	public class PetEquipTipsMediator : ViewMediator
	{
		public PetEquipTipsMediator (PetEquipTipsView view, uint id = MediatorName.PET_EQUIP_TIPS_MEDIATOR) : base(id, view)
		{
			
		}
		
		public PetEquipTipsView View {
			get {
				if (_viewComponent != null && _viewComponent is PetEquipTipsView)
					return _viewComponent as PetEquipTipsView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_PET_EQUIP_TIPS_SHOWINFO
			};
		}
		
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			PetEquipTipsView petEquipTipsView = this.View;
			if (petEquipTipsView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_UI:
					UIManager.Instance.closeAllUI ();
					break;
				case MsgConstant.MSG_PET_EQUIP_TIPS_SHOWINFO:
					TipsCommand cmd = (TipsCommand)notification.body;
					petEquipTipsView.ShowInfo(TipsManager.Instance.EquipInfo,TipsManager.Instance.Iteminfo,cmd);
					break;
					
				default:
					break;
				}
				
			}
			
		}
	}

}