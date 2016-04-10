/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-5-15
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnBossDeadMsg : MonoBehaviour
{
	private const string BTN_REVIVE = "btnRevive";
	private const string BTN_RETCITY = "btnRet";
	
	void OnClick ()
	{
		switch (gameObject.name) {
		 
		case BTN_REVIVE:
			Gate.instance.sendNotification (MsgConstant.MSG_BOSS_DEAD_BUY_REVIVE);
			break;	
		case BTN_RETCITY:
			Gate.instance.sendNotification (MsgConstant.MSG_BOSS_DEAD_BACK_CITY);
			break;
		default:
			break;
		}
	}
 
	
}
