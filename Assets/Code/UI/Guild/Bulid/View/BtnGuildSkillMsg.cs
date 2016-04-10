using UnityEngine;
using System.Collections;
using model;
using manager;
using MVC.entrance.gate;
using helper;

public class BtnGuildSkillMsg : MonoBehaviour {

    const string Table1 = "Table1";
    const string Table2 = "Table2";
    const string Btn_Close = "Close";
    const string Btn_Donate = "donate";
    const string Btn_LevelUp = "levelup";

    void OnClick()
    {
        switch (transform.name)
        {
            case Btn_Close:
                UIManager.Instance.closeWindow(UiNameConst.ui_guildskill);
                break;
            case Table1:
                UIManager.Instance.openWindow(UiNameConst.ui_guildskill);
                Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_SKILL_REFRESH, GuildSkillType.GuildSkillLearn);
                break;
            case Table2:
                UIManager.Instance.openWindow(UiNameConst.ui_guildskill);
                Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_SKILL_REFRESH, GuildSkillType.GuildSkillFocus);
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
