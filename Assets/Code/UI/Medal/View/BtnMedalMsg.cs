using UnityEngine;
using System.Collections;
using manager;
using model;
using mediator;
using MVC.entrance.gate;
using MVC.interfaces;

public class BtnMedalMsg : MonoBehaviour {
   
    void OnClick() 
    {
        switch (transform.name)
        {
            case "ShengjiButton":
                MedalManager.Instance.isMedalLevelUp = true;
                Gate.instance.sendNotification(MsgConstant.MSG_MEDAL_SWING_MEDAL_LEVELUP);
                break;
            case "ShengjiButtonTips":
                NPCManager.Instance.createCamera(false);
                FastOpenManager.Instance.OpenWindow(FunctionName.Medal, true);
				UIManager.Instance.getUIFromMemory(UiNameConst.ui_furnace).transform.FindChild("Table/Table2").GetComponent<UICheckBoxColor>().isChecked = true;
                break;
            default:
                break;
        }
    }
	
}
