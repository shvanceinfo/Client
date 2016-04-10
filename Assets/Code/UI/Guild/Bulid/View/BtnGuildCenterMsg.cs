using UnityEngine;
using System.Collections;
using manager;
using MVC.interfaces;
using MVC.entrance.gate;
using helper;
using model;

public class BtnGuildCenterMsg : MonoBehaviour {
    const string Table1 = "Table1";
    const string Btn_Close = "Close";
    const string Btn_Donate = "donate";
    const string Btn_LevelUp = "levelup";

    void OnClick()
    {
        switch (transform.name)
        {
            case Btn_Close:
                UIManager.Instance.closeWindow(UiNameConst.ui_guildcenter);
                break;
            case Table1:
                UIManager.Instance.openWindow(UiNameConst.ui_guildcenter);
                Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_CENTER_SHOW, GuildFlagType.GuildFlag);
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
