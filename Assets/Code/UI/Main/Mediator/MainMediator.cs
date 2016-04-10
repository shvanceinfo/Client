/**该文件实现的基本功能等
function: 实现主界面的View控制
author:zyl
date:2014-4-4
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
	public class MainMediator : ViewMediator
	{
		public MainMediator (MainView view, uint id = MediatorName.MAIN_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public MainView View {
			get {
				if (_viewComponent != null && _viewComponent is MainView)
					return _viewComponent as MainView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_MAIN_PEOPLEINFO,
				MsgConstant.MSG_MAIN_UPDATE_ENGERY,
				MsgConstant.MSG_MAIN_UPDATE_ASSET,
				MsgConstant.MSG_MAIN_UPDATE_PROPERTY,
				MsgConstant.MSG_MAIN_UPDATE_CURRENCY,
				MsgConstant.MSG_MAIN_UPDATE_MONEY,
                MsgConstant.MSG_MAIN_UPDATE_EMAIL,
                MsgConstant.MSG_MAIN_UPDATA_TALK,
                MsgConstant.MSG_MAIN_UPDATE_VIP,
                MsgConstant.MSG_FRIEND_NOTIFY,
				MsgConstant.MSG_MAIN_UPDATE_FUNCTION,
                MsgConstant.MSG_MIAN_UPDATE_CHANNEL,
                MsgConstant.MSG_SETTING_PEOPLE_SWITCH,
                MsgConstant.MSG_MAIN_CLOSE_MENU,
                MsgConstant.MSG_MAIN_PUSH_TRIGGER
			};
		}
		
  
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			MainView mainView = this.View;
			if (mainView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_UI:
					UIManager.Instance.closeAllUI ();
					break;
		 		
				case MsgConstant.MSG_MAIN_PEOPLEINFO:
					string spriteName = (string)notification.body;
					mainView.UpdatePeopleInfo (CharacterPlayer.character_property.nick_name, spriteName);
					break;	
				case MsgConstant.MSG_MAIN_UPDATE_ENGERY:
					mainView.updateEngery (CharacterPlayer.character_property.currentEngery);
					break;
				case MsgConstant.MSG_MAIN_UPDATE_ASSET:
					mainView.UpdatePlayerAsset (CharacterPlayer.character_asset.diamond, CharacterPlayer.character_asset.gold, CharacterPlayer.character_asset.Crystal);
					break;
				case MsgConstant.MSG_MAIN_UPDATE_PROPERTY:
					mainView.UpdatePlayerProperty (CharacterPlayer.character_property.getLevel ()
														, CharacterPlayer.character_property.getFightPower ());
					break;
				case MsgConstant.MSG_MAIN_UPDATE_CURRENCY:
					int currency = (int)notification.body;
					mainView.UpdatCurrencyChange (currency);
					break;
				case MsgConstant.MSG_MAIN_UPDATE_MONEY:
					int money = (int)notification.body;
					mainView.UpdatMoneyChange (money);
					break;
				case MsgConstant.MSG_MAIN_UPDATE_EMAIL:
					mainView.UpdateEmailNotify ((bool)notification.body);
                    break;
				case MsgConstant.MSG_MAIN_UPDATA_TALK:
					mainView.UpdateTalkMsg ();
					break;
				case MsgConstant.MSG_MAIN_UPDATE_VIP:
					mainView.UpdateVipLevel ();
					break;
				case MsgConstant.MSG_FRIEND_NOTIFY:
					mainView.UpdateFriendNotiy ((bool)notification.body);
					break;
				case MsgConstant.MSG_MAIN_UPDATE_FUNCTION:
					mainView.UpdateShowOrHideUi();
					break;
                    case MsgConstant.MSG_MIAN_UPDATE_CHANNEL:
                    mainView.UpdateChannel();
                    break;
                    case MsgConstant.MSG_SETTING_PEOPLE_SWITCH:
                    mainView.UpdateHidePeople();
                    break;
                    case MsgConstant.MSG_MAIN_CLOSE_MENU:
                    mainView.UpdateCloseMenu();
                    break;
                    case MsgConstant.MSG_MAIN_PUSH_TRIGGER:
                    GuideInfoManager.Instance.PushTrigger((GuideInfoData)notification.body);
                    break;
				default:
					break;
				}
				
			}
			
		}
	 
	}
	
}