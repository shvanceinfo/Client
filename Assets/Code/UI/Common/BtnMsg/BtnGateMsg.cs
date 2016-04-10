/**该文件实现的基本功能等
function: 实现关卡按钮点击，扫荡点击的功能
author:ljx
date:2014-03-09
**/
using UnityEngine;
using MVC.entrance.gate;
using model;
using manager;
using System;

public class BtnGateMsg : MonoBehaviour
{
	//副本按钮的相关常量
	const string CLOSE_MAP_INFO = "closeMapInfo"; //关闭地图信息
	const string ENTER_RAID = "enter"; //进入副本
	const string OPEN_SWEEP_WINDOW = "sweep"; //打开扫荡按钮
	//扫荡的按钮
	const string CLOSE_SWEEP_WINDOW = "closeSweep";
	const string BTN_BEGIN_SWEEP = "startBtn";
	const string BTN_STOP_SWEEP = "stopBtn";
	const string BTN_ACCELERATE_SWEEP = "accelerateBtn";
	const string BTN_ADD_SWEEP_TIME = "addBtn";
	const string BTN_SUBSTRACT_SWEEP_TIME = "subtractBtn";
	const string BTN_PROMOTE_VIP = "VIPBtn"; //提升VIP
	const string BTN_MAX = "zuida";
	private float lastClickTime = 0;
	
	void OnClick ()
	{ 
		
		switch (gameObject.name) {
		case CLOSE_MAP_INFO:
			UIManager.Instance.closeWindow (UiNameConst.ui_map_info);
			break;
		case ENTER_RAID:
			Gate.instance.sendNotification (MsgConstant.MSG_ENTER_RAID);
			break;
		case OPEN_SWEEP_WINDOW:
			Gate.instance.sendNotification (MsgConstant.MSG_OPEN_SWEEP);
			break;
		case CLOSE_SWEEP_WINDOW:
			if (SweepManager.Instance.IsAccelerate) {   //加速必看
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("sweep_result"),
				                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				return;
			}

			if (SweepManager.Instance.IsSweeping && VipManager.Instance.SweepJiaSu<=1) { //时间短也需要强行看
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("sweep_result"),
				                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				return;
			}


			SweepManager.Instance.IsShowResult = false;
			UIManager.Instance.closeWindow (UiNameConst.ui_sweep);
			break;
		case BTN_BEGIN_SWEEP:
			if (SweepManager.Instance.IsShowResult) {
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("sweep_result"),
				                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				return;
			}

			Gate.instance.sendNotification (MsgConstant.MSG_SWEEP_START);
			break;
		case BTN_STOP_SWEEP:
			Gate.instance.sendNotification (MsgConstant.MSG_SWEEP_STOP, eStopSweep.ePlayerStop);
			break;
		case BTN_ACCELERATE_SWEEP:
			if (RealTime.time - lastClickTime < 1) {
				return;
			}
			this.lastClickTime = RealTime.time;


			if (!SweepManager.Instance.CheckIsCanSweep) {
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("sweep_diamond_not_enough"),
				                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				return;
			}
 
			Gate.instance.sendNotification (MsgConstant.MSG_SWEEP_ACCELERATE);
			break;
		case BTN_ADD_SWEEP_TIME:
			{
				if (SweepManager.Instance.IsShowResult) {
					FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("sweep_result"),
				                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
					return;
				}

				UILabel lbl = transform.parent.Find ("Label").GetComponent<UILabel> ();
				int currentNum = int.Parse (lbl.text);
				Gate.instance.sendNotification (MsgConstant.MSG_SWEEP_ADD_NUM, currentNum);
			}
			break;
		case BTN_SUBSTRACT_SWEEP_TIME:
			{
				if (SweepManager.Instance.IsShowResult) {
					FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("sweep_result"),
				                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
					return;
				}
				UILabel lbl = transform.parent.Find ("Label").GetComponent<UILabel> ();
				int currentNum = int.Parse (lbl.text);
				Gate.instance.sendNotification (MsgConstant.MSG_SWEEP_SUBSTRACT_NUM, currentNum);
			}
			break;
		case BTN_PROMOTE_VIP:
			FastOpenManager.Instance.OpenWindow (FunctionName.VIP);
			break;
		case BTN_MAX:
			if (SweepManager.Instance.IsShowResult) {
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("sweep_result"),
				                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				return;
			}
			Gate.instance.sendNotification (MsgConstant.MSG_SWEEP_MAX);
			break;
		default:		
			break;
		}
	}
}
