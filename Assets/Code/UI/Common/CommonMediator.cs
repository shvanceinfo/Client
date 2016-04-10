/**该文件实现的基本功能等
function: 实现一些原来非MVC框架的视图分离
author:ljx
date:2013-11-09
**/
using manager;
using MVC.entrance.gate;
using MVC.interfaces;
using System.Collections;
using System.Collections.Generic;
using NetGame;

namespace mediator
{
	public class CommonMediator : ViewMediator
	{
		private ChapterView _chapterView;
		private MainView _change;
		
		public CommonMediator(uint id = MediatorName.COMMON_MEDIATOR) : base(id, null)
		{
			_chapterView = null;
		}
		
		public override IList<uint> listReferNotification()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_INIT_POWER_ENGERY,
				MsgConstant.MSG_SURE_DIALOG
			};
		}
		
		//处理相关的消息
		public override void handleNotification(INotification notification)
		{				
			switch (notification.notifyId) 
			{
				case MsgConstant.MSG_INIT_POWER_ENGERY:
				{
					if(_chapterView != null)
						_chapterView.updateEnergy(CharacterPlayer.character_property.currentEngery);
					if(_change != null)
						_change.updateEngery(CharacterPlayer.character_property.currentEngery);
				}
				break;
				case MsgConstant.MSG_SURE_DIALOG:
				{
					eDialogSureType sureType = (eDialogSureType)notification.body;
					if(sureType == eDialogSureType.eSureBuyEngery)
					{
						GCAskBuyEngery buyEngery = new GCAskBuyEngery();
						NetBase.GetInstance().Send(buyEngery.ToBytes(), false);
					}
				}
				break;
				default:					
					break;
			}
		}
		
		//getter and setter
		public ChapterView ChapterView
		{
			set { _chapterView = value; }
		}
		
		public MainView Change
		{
			set { _change = value; }
		}
	}
}
