/**该文件实现的基本功能等
function: 实现关卡view的通知
author:ljx
date:2014-04-03
**/
using model;
using UnityEngine;
using System.Collections.Generic;
using manager;
using MVC.entrance.gate;
using MVC.interfaces;

namespace mediator
{
	public class RaidMediator : ViewMediator
	{	
		public RaidMediator (ChapterView view, uint id = MediatorName.RAID_MEDIATOR) : base(id, view)
		{
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_RAID_OPEN,
				MsgConstant.MSG_RAID_BTN_CLICK_NORMAL,
				MsgConstant.MSG_RAID_BTN_CLICK_HARD,
				MsgConstant.MSG_RAID_BTN_CLICK_MAP,
				MsgConstant.MSG_RAID_BTN_CLICK_ADD_ENERGY,
				MsgConstant.MSG_RAID_BTN_SHOW_PREV,
				MsgConstant.MSG_RAID_BTN_SHOW_NEXT,
				MsgConstant.MSG_RAID_BTN_CLICK_CLOSE,
                MsgConstant.MSG_INIT_POWER_ENGERY, //对于体力变化感兴趣
				MsgConstant.MSG_RAID_BTN_UPDATE_CHAPTER_AWARD
			};
		}
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			if (View != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_RAID_OPEN:
					View.initView ();
					break;
				case MsgConstant.MSG_RAID_BTN_CLICK_NORMAL:
					if (RaidManager.Instance.changeNormalHard (false))
						View.clickBtn (false);
					break;
				case MsgConstant.MSG_RAID_BTN_CLICK_HARD:
					if (RaidManager.Instance.changeNormalHard (true))
						View.clickBtn (true);
					break;
				case MsgConstant.MSG_RAID_BTN_CLICK_MAP:
					uint mapID = (uint)notification.body;
					if (mapID == RaidManager.Instance.CurrentRaid.mapID) { //点击了当前关卡
						if (!Gate.instance.hasMediator (MediatorName.SWEEP_MEDIATOR)) //先要注册扫荡的中介
							Gate.instance.registerMediator (new SweepMdiator (MediatorName.SWEEP_MEDIATOR));
						Gate.instance.sendNotification (MsgConstant.MSG_OPEN_SMALL_MAP_INFO, (int)mapID);
					} else {
						MapVo vo = RaidManager.Instance.getRaidVo (mapID);
						if (vo != null && vo.canEnter) {
							MapVo oldVo = RaidManager.Instance.changeCurentRaid (mapID);
							View.changeRaid (oldVo);
						} else {
							FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("raid_is_lock"), UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
						}
					}
					break;
				case MsgConstant.MSG_RAID_BTN_SHOW_PREV:
					if (RaidManager.Instance.previewChapter (false)) { //如果可以预览上一个窗口
						View.tweenView (false);
					}
					break;
				case MsgConstant.MSG_RAID_BTN_SHOW_NEXT:
					if (RaidManager.Instance.previewChapter (true)) { //如果可以预览下一个窗口
						View.tweenView (true);
					}
					break;
				case MsgConstant.MSG_RAID_BTN_CLICK_CLOSE:
					UIManager.Instance.closeWindow (UiNameConst.ui_raid);
					break;
				case MsgConstant.MSG_INIT_POWER_ENGERY:
					View.updateEnergy (CharacterPlayer.character_property.currentEngery);
					break;
				case MsgConstant.MSG_RAID_BTN_UPDATE_CHAPTER_AWARD:
					View.InitChapterAward((bool)notification.body);
					break;
					
				default:
					break;
				}
			}
		}
		
		//getter and setter
		public ChapterView View {
			get {
				if (_viewComponent != null && _viewComponent is ChapterView)
					return _viewComponent as ChapterView;
				return  null;					
			}
		}
	}
}
