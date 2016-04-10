using UnityEngine;
using System.Collections;
using model;
using manager;
using MVC.entrance.gate;
using helper;

public class BtnGuildFlagMsg : MonoBehaviour {
    const string Table1 = "Table1";
    const string Btn_Close = "Close";
    const string Btn_Donate = "donate";
    const string Btn_LevelUp = "levelup";

    void OnClick()
    {
        switch (transform.name)
        {
            case Btn_Close:
                UIManager.Instance.closeWindow(UiNameConst.ui_guildflag);
                break;
            case Table1:
                UIManager.Instance.openWindow(UiNameConst.ui_guildflag);
                Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_FLAG_SHOW,GuildFlagType.GuildFlag);
                break;
            case Btn_Donate:
                ViewHelper.DisplayMessage("暂未开放");
                break;
            case Btn_LevelUp:
                ViewHelper.DisplayMessage("暂未开放");
                break;
            default:
                break;
        }
    }
}
