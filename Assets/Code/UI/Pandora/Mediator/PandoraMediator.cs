/**该文件实现的基本功能等
function: 实现潘多拉的View控制
author:zyl
date:2014-06-10
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
	public class PandoraMediator : ViewMediator
	{
		public PandoraMediator (PandoraView view, uint id = MediatorName.PANDORA_MEDIATOR) : base(id, view)
		{
			
		}

		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_CLOSE_PANDORA_UI,
				MsgConstant.MSG_PANDORA_SHOW,
				MsgConstant.MSG_PANDORA_CHALLENGE_PANDORA,
				MsgConstant.MSG_PANDORA_RESET_PANDORA_NUM,
				MsgConstant.MSG_PANDORA_CHALLENGE_ALL_PANDORA,
				MsgConstant.MSG_PANDORA_OPEN_PANDORA,
			};
		}

 
 

		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			if (this.view != null) {				
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_PANDORA_UI:
					PandoraManager.Instance.CloseWindow ();
					break;
				case MsgConstant.MSG_PANDORA_SHOW:
					this.view.Show (PandoraManager.Instance.CurrentPandora);
					break;
				
				case MsgConstant.MSG_PANDORA_CHALLENGE_PANDORA:
					PandoraManager.Instance.AskChallengePandora ();
					break;
				
				case MsgConstant.MSG_PANDORA_RESET_PANDORA_NUM:
					PandoraManager.Instance.AskResetPandoraNum (0);
					break;
				
				case MsgConstant.MSG_PANDORA_CHALLENGE_ALL_PANDORA:
					PandoraManager.Instance.AskChallengeAllPandora (0);
					break;
						
				case MsgConstant.MSG_PANDORA_OPEN_PANDORA:
					PandoraManager.Instance.AskOpenPandora ();
					break;

		 

				default:					
					break;
				}
			}
		}
		
		
		
		
		//getter and setter
		public PandoraView view {
			get {
				if (_viewComponent != null && _viewComponent is PandoraView)
					return _viewComponent as PandoraView;
				return  null;					
			}
		}
	}
}
