/**该文件实现的基本功能等
function: 实现魔物悬赏的View控制
author:zyl
date:2014-06-04
**/
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using MVC.interfaces;
using manager;
using model;
using NetGame;

namespace mediator
{	
	public class MonsterRewardMediator : ViewMediator
	{

		public MonsterRewardMediator (MonsterRewardView view, uint id = MediatorName.MONSTER_REWARD_MEDIATOR) : base(id, view)
		{
			 
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_CLOSE_MONSTER_REWARD_UI,
				MsgConstant.MSG_MONSTER_REWARD_SHOW,
				MsgConstant.MSG_MONSTER_REWARD_ASK_ZHUIJI,
				MsgConstant.MSG_MONSTER_REWARD_NEXT,
				MsgConstant.MSG_MONSTER_REWARD_PREV,
				MsgConstant.MSG_MONSTER_REWARD_UPDATE_CURRENT,
				MsgConstant.MSG_MONSTER_REWARD_UPDATE_NEXT,
				MsgConstant.MSG_MONSTER_REWARD_UPDATE_PREV
			};
		}
 
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			if (this.view != null) {				
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_MONSTER_REWARD_UI:
					MonsterRewardManager.Instance.CloseWindow ();
					MonsterRewardManager.Instance.IsLock = false;
					break;
				case MsgConstant.MSG_MONSTER_REWARD_SHOW:
					this.view.Show ();
					break;
				case MsgConstant.MSG_MONSTER_REWARD_ASK_ZHUIJI:
					ZhuiJiType type = (ZhuiJiType)notification.body;
					MonsterRewardManager.Instance.GCAskZhuiJi (type);
					break;
				case MsgConstant.MSG_MONSTER_REWARD_NEXT:
					this.view.NextPage ();
					break;
				case MsgConstant.MSG_MONSTER_REWARD_PREV:
					this.view.PrevPage ();
					break;
				case MsgConstant.MSG_MONSTER_REWARD_UPDATE_CURRENT:
					this.view.UpdateCurrent ();
					break;	
				case MsgConstant.MSG_MONSTER_REWARD_UPDATE_NEXT:
					this.view.UpdateNext ();
					break;
				case MsgConstant.MSG_MONSTER_REWARD_UPDATE_PREV:
					this.view.UpdatePrev ();
					break;
				default:					
					break;
				}
			}
		}
		
	 
		
		
		//getter and setter
		public MonsterRewardView view {
			get {
				if (_viewComponent != null && _viewComponent is MonsterRewardView)
					return _viewComponent as MonsterRewardView;
				return  null;					
			}
		}
	}
}
