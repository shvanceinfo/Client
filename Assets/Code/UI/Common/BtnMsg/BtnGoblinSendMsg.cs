/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-3-11
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;
using NetGame;

public class BtnGoblinSendMsg : MonoBehaviour
{
	
	//哥布林相关按钮
	const string OPEN_GOBLIN = "golden_goblin"; //打开哥布林界面
	const string CLOSE_GOBLIN = "closeGoblin";  //关闭哥布林界面
	
	const string ENTER_GOBLIN = "enterGoblin";  //进入哥布林副本
	const string BUY_TIMES = "buyTimes";        //购买挑战次数
	const string ONE_CLEAR = "oneClear";         //一键清剿
	
	void OnClick ()
	{
 
		switch (gameObject.name) {
		case  OPEN_GOBLIN:
			#region 打开哥布林界面
			UIManager.Instance.openWindow(UiNameConst.ui_golden_goblin);
			MessageManager.Instance.SendAskGoldenGoblinTimes (); //取得剩下的次数
			#endregion
			break;
		case CLOSE_GOBLIN:
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_UI);
			break;
		case ENTER_GOBLIN:
			Gate.instance.sendNotification (MsgConstant.MSG_ENTER_GOBLIN);
			break;
		case BUY_TIMES:
			Gate.instance.sendNotification(MsgConstant.MSG_GOBLIN_BUY_TIMES);
			break;
		default:
			 
			break;
		}
		
 
		
		
	}
	
}
