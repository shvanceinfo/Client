using UnityEngine;
using System.Collections;
using MVC.entrance.gate;

public class BtnBossWinMsg : MonoBehaviour {

	 
	private const string BTN_RETCITY = "btnRet";
	
	void OnClick ()
	{
		switch (gameObject.name) {
		 	
		case BTN_RETCITY:
			Gate.instance.sendNotification (MsgConstant.MSG_BOSS_WIN_BACK_CITY);
			break;
		default:
			break;
		}
	}
}
