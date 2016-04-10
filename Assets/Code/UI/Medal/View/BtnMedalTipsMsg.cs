using UnityEngine;
using System.Collections;
using manager;
using model;
using mediator;
using MVC.entrance.gate;
using MVC.interfaces;
using helper;

public class BtnMedalTipsMsg : MonoBehaviour
{

	void OnClick ()
	{
		//ViewHelper.DisplayMessage("暂未开放,敬请期待!");
		//return;
		switch (transform.name) {
		case "MedalBtn":
			if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Medal)) {
				return;
			}
            //MedalTipsManager.Instance.ShowMedalUI ();
            NPCManager.Instance.createCamera(false);
            UIManager.Instance.openWindow(UiNameConst.ui_medal);
            UIManager.Instance.closeWindow(UiNameConst.ui_bag);
            UIManager.Instance.closeWindow(UiNameConst.ui_role);
            Gate.instance.sendNotification(MsgConstant.MSG_MEDAL_DISPLAY_VIEW, Table.Table2);
            EasyTouchJoyStickProperty.ShowJoyTouch(false); 
			break;
		case "WeaponBtn":
			 	
			ViewHelper.DisplayMessage ("暂未开放,敬请期待!");
			break;
		default:
			break;
		}
        
	}
}
