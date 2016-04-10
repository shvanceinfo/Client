/**该文件实现的基本功能等
function: 实现羽翼的View控制
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections.Generic;
using MVC.entrance.gate;
using MVC.interfaces;
using manager;
using model;
using System;

namespace mediator
{
	public class SweepMdiator : ViewMediator
	{
		private MapInfoView _mapView;
		private SweepView _sweepView;
		
		public SweepMdiator (uint id = MediatorName.SWEEP_MEDIATOR) : base(id, null)
		{
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_OPEN_SMALL_MAP_INFO,
				MsgConstant.MSG_ENTER_RAID,
				MsgConstant.MSG_OPEN_SWEEP,
				MsgConstant.MSG_SWEEP_START,
				MsgConstant.MSG_SWEEP_STOP,
				MsgConstant.MSG_SWEEP_ACCELERATE,
				MsgConstant.MSG_SWEEP_ADD_NUM,
				MsgConstant.MSG_SWEEP_SUBSTRACT_NUM,
				MsgConstant.MSG_SWEEP_SHOW_RESULT,
				MsgConstant.MSG_SWEEP_MAX,
				MsgConstant.MSG_SWEEP_SHOW_FINAL_RESULT
			};
		}
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			switch (notification.notifyId) {
			case MsgConstant.MSG_OPEN_SMALL_MAP_INFO:
				if (SweepManager.Instance.IsSweeping)
					Gate.instance.sendNotification (MsgConstant.MSG_OPEN_SWEEP); //打开扫荡界面
					else {
					UIManager.Instance.openWindow (UiNameConst.ui_map_info);
					_mapView = UIManager.Instance.getUIFromMemory (UiNameConst.ui_map_info).GetComponent<MapInfoView> ();
					SweepManager.Instance.setCurrentMap ((int)notification.body);
					_mapView.initView ();
				}
				break;
			case MsgConstant.MSG_ENTER_RAID:
				Global.lastFightMap = SweepManager.Instance.CurrentMap;
				Global.current_fight_level = Global.eFightLevel.Fight_Level1;
				MessageManager.Instance.sendMessageChangeScene (SweepManager.Instance.CurrentMap.id, false);
				EventDispatcher.GetInstance ().OnHUDNeedHideShow (false);
				break;
			case MsgConstant.MSG_OPEN_SWEEP:
				if (RaidManager.Instance.PassMapHash.ContainsKey ((uint)SweepManager.Instance.CurrentMap.id / 10)) {
					UIManager.Instance.closeWindow (UiNameConst.ui_map_info);
					UIManager.Instance.openWindow (UiNameConst.ui_sweep);
					_sweepView = UIManager.Instance.getUIFromMemory (UiNameConst.ui_sweep).GetComponent<SweepView> ();
					if (SweepManager.Instance.IsSweeping) { //正在扫荡中
						if (SweepManager.Instance.LastCloseTime > 0f) { //重新计时
							DateTime next = DateTime.Now.AddSeconds (SweepManager.Instance.LastCloseTime);
							SweepManager.Instance.CountDownSpan = next.Subtract (DateTime.Now);
						}
					}
					_sweepView.initView ();
				} else
					FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("not_pass_raid_before"),
						                                            UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				break;
			case MsgConstant.MSG_SWEEP_START:
				if (_sweepView != null) {
					int sweepTime = SweepManager.Instance.sweepTotalNum;
					if (checkEnoughEnergy (sweepTime)) {
						SweepManager.Instance.askServerSweep (notification.notifyId);
						_sweepView.showProcessInfo ();
					}
				}
				break;
			case MsgConstant.MSG_SWEEP_STOP:
				if (SweepManager.Instance.IsSweeping) {
					eStopSweep stopType = (eStopSweep)notification.body;
					if (stopType == eStopSweep.ePlayerStop) //玩家手动停止需要发送消息
						SweepManager.Instance.askServerSweep (notification.notifyId);
					SweepManager.Instance.IsShowResult = false;
					if (_sweepView != null)
						_sweepView.stopSweep (stopType);
				}
				break;
			case MsgConstant.MSG_SWEEP_ACCELERATE:
				if (SweepManager.Instance.IsSweeping) {
					SweepManager.Instance.askServerSweep (notification.notifyId);
					if (_sweepView != null)
						_sweepView.accelerateSweep ();
				}
				break;
			case MsgConstant.MSG_SWEEP_ADD_NUM:
				if (_sweepView != null) {
					int sweepTime = SweepManager.Instance.sweepTotalNum + 1;
					if (checkEnoughEnergy (sweepTime))
						_sweepView.showStartInfo (sweepTime);
				}
				break;
			case MsgConstant.MSG_SWEEP_SUBSTRACT_NUM:
				if (_sweepView != null) {
					int sweepTime = SweepManager.Instance.sweepTotalNum - 1;
					if (sweepTime > 0)
						_sweepView.showStartInfo (sweepTime);
				}
				break;
			case MsgConstant.MSG_SWEEP_SHOW_RESULT:
				if (_sweepView != null) {
					int currentSweepNum = (int)notification.body;
					_sweepView.SweepResult(currentSweepNum); 
				}
				break;
			case MsgConstant.MSG_SWEEP_MAX:
				int maxTimes;
				this.GetMaxSweepTimes (out maxTimes);
				_sweepView.showStartInfo (maxTimes);
				break;
			case MsgConstant.MSG_SWEEP_SHOW_FINAL_RESULT:
			{
				int currentSweepNum = (int)notification.body;
				_sweepView.SweepFinalResult(currentSweepNum);
				break;
			}

			default:					
				break;
			}
		}
		
		//检查体力是否足够扫荡
		private bool checkEnoughEnergy (int sweepTime)
		{
			if (SweepManager.Instance.CurrentMap != null) {
				int needEnergy = SweepManager.Instance.CurrentMap.engeryConsume * sweepTime;
				if (needEnergy > CharacterPlayer.character_property.currentEngery) {
					string msg = LanguageManager.GetText ("sweep_not_enough_energy");
					msg = msg.Replace (Constant.REPLACE_PARAMETER_1, sweepTime.ToString ());
					FloatMessage.GetInstance ().PlayFloatMessage (msg,
			                                            UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
					return false;
				} else 
					return true;
			}
			return false;
		}
		
		private void GetMaxSweepTimes (out int maxTimes)
		{
			maxTimes = CharacterPlayer.character_property.currentEngery / SweepManager.Instance.CurrentMap.engeryConsume;
			if (maxTimes <= 0) {
				maxTimes = 1;
			}
		}
		
		
		//getter and setter
		public SweepView view {
			get {
				if (_viewComponent != null && _viewComponent is SweepView)
					return _viewComponent as SweepView;
				return  null;					
			}
		}
		
//		public MapInfoView MapView
//		{
//			set {_mapView = value;}
//		}
		
//		public SweepView SweepView
//		{
//			set {_sweepView = value;}
//		}
	}
}
