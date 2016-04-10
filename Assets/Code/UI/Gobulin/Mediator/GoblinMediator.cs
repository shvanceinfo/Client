/**该文件实现的基本功能等
function: 实现哥布林的View控制
author:zyl
date:2014-3-11
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
	public class GoblinMediator   : ViewMediator
	{

		public GoblinMediator (GoblinView view, uint id = MediatorName.GOBLIN_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public GoblinView View {
			get {
				if (_viewComponent != null && _viewComponent is GoblinView)
					return _viewComponent as GoblinView;
				return  null;		
			}
		}
		
		
		public override IList<uint> listReferNotification()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_OPEN_GOBLIN,
				MsgConstant.MSG_GOBLIN_INIT,
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_GOBLIN_UPDATE_ENTER_TIMES,
				MsgConstant.MSG_ENTER_GOBLIN,
				MsgConstant.MSG_GOBLIN_CAN_BUY_NUM,
				MsgConstant.MSG_GOBLIN_BUY_PRICE,
				MsgConstant.MSG_CLEAR_GOBLIN_BUY_PRICE,
				MsgConstant.MSG_GOBLIN_BUY_TIMES
			};
		}
		
		//处理相关的消息
		public override void handleNotification(INotification notification)
		{
			GoblinView goblinView = this.View;
			if (goblinView!=null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_UI:
					UIManager.Instance.closeAllUI();
					break;
				case MsgConstant.MSG_GOBLIN_INIT:
					goblinView.ShowGoblinModel(); //显示哥布林模型
					break;
				case MsgConstant.MSG_GOBLIN_UPDATE_ENTER_TIMES:  //得到进入次数
					IList<uint> list =  notification.body as List<uint>;
					if (list!=null) {
						goblinView.UpdateGoblinRemainTimes(list[0],list[1]);
					}
					break;
				case MsgConstant.MSG_ENTER_GOBLIN: //进入副本
					if (GoblinManager.Instance.IsCanEnterGoblin()) {
						goblinView.EnterGoblin();
					}else{
						goblinView.ShowErr("golden_goblin_times_not_enough");
					}
					break;
				case MsgConstant.MSG_GOBLIN_CAN_BUY_NUM:
					uint canBuyNum = (uint)notification.body;
					goblinView.UpdateGoblinCanBuyNum(canBuyNum);
					break;
				case MsgConstant.MSG_GOBLIN_BUY_PRICE:
					uint price = (uint)notification.body;
					goblinView.UpdateGoblinBuyPrice(price);
					break;
				case MsgConstant.MSG_CLEAR_GOBLIN_BUY_PRICE:
					goblinView.UpdateClearGoblinBuyPrice();
					break;
				case MsgConstant.MSG_GOBLIN_BUY_TIMES:
					if (GoblinManager.Instance.IsCanBuyTimes()) {
						if (GoblinManager.Instance.IsDiamondEnough()) {
							goblinView.ShowBuyTimesDialog(GoblinManager.Instance.Price);
						}else{
							goblinView.ShowErr("golden_goblin_double_benefit_diamond_not_enough");
						}
					}else{
						goblinView.ShowErr("golden_goblin_buy_ticket_not_enough");
					}
					
 
					break;
				default:
				break;
				}
				
			}
			
		}
		
		
	}

}
