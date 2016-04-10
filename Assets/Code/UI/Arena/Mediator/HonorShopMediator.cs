/**该文件实现的基本功能等
function: 实现游戏币，金币，新邮件，新物品（服务器发送过来的相关数据变动时后通知UI层发生变化）
author:ljx
date:2013-11-09
**/

using System.Collections.Generic;
using MVC.entrance.gate;
using MVC.interfaces;
using model;
using manager;
using NetGame;

namespace mediator
{
	public class HonorShopMediator : ViewMediator
	{			
		public HonorShopMediator(HonorShopView view, uint id = MediatorName.HONOR_SHOP_MEDIATOR) : base(id, view)
		{
		}
		
		public override IList<uint> listReferNotification()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_HONOR_DIAMOND,
				MsgConstant.MSG_HONOR_EQUIP,
				MsgConstant.MSG_HONOR_TOOL,
				MsgConstant.MSG_HONOR_OTHER,
				MsgConstant.MSG_HONOR_BUY,
				MsgConstant.MSG_SURE_DIALOG
			};
		}
		
		public override void handleNotification(INotification notification)
		{
			if(view != null)
			{
				HonorShopView shopView = view;
				switch (notification.notifyId) 
				{
					case MsgConstant.MSG_HONOR_DIAMOND:
						shopView.switchTab(eHonorItemType.diamond);
						break;
					case MsgConstant.MSG_HONOR_EQUIP:
						shopView.switchTab(eHonorItemType.equip);
						break;
					case MsgConstant.MSG_HONOR_TOOL:
						shopView.switchTab(eHonorItemType.tools);
						break;
					case MsgConstant.MSG_HONOR_OTHER:
						shopView.switchTab(eHonorItemType.other);
						break;	
					case MsgConstant.MSG_HONOR_BUY:
						ArenaManager.Instance.showClickDialog(MsgConstant.MSG_HONOR_BUY, (uint)notification.body);
						break;
					case MsgConstant.MSG_SURE_DIALOG:
						eDialogSureType sureType = (eDialogSureType)notification.body;
						if(sureType == eDialogSureType.eSureBuyHonorItem)
						{
							GCAskBuyHonorItem buyItem = new GCAskBuyHonorItem();
							NetBase.GetInstance().Send(buyItem.ToBytes(ArenaManager.Instance.buyItemID), true);
						}
						break;
					default:					
						break;
				}
			}
		}
		
		public HonorShopView view
		{
			get 
			{
				if(_viewComponent != null && _viewComponent is HonorShopView)
					return _viewComponent as HonorShopView;
				return  null;					
			}
		}
	}
}
