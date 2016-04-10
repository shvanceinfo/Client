using UnityEngine;
using System.Collections;
using manager;
using MVC.entrance.gate;
using helper;
public class BtnGuildMsg : MonoBehaviour {

    const string tab1 = "tab1";
    const string tab2 = "tab2";
    const string tab3 = "tab3";
    const string tab4 = "tab4";
    const string tab5 = "tab5";
    const string tab6 = "tab6";
    const string close = "close";
    const string toggle = "Toggle";
    const string closeShowInfo = "closeShowInfo";
    const string closeOffice = "closeOffice";

    const string enable = "enable";
    const string btn_office = "btn_office";

    const string memberGrid = "membergrid";
    void OnClick()
    {
        if (gameObject.transform.parent.parent.name.Equals(memberGrid))
        {
            Gate.instance.sendNotification(MsgConstant.MSG_GUILD_DISPLAY_MEMBER_INFO,
                gameObject.transform.parent.name.toInt32());
        }

        switch (gameObject.name)
        {

            case close:
                //GuildManager.Instance.CloseWindow();
                break;
            case tab1: Gate.instance.sendNotification(MsgConstant.MSG_GUILD_DISPLAY_TABLE, Table.Table1);
                break;
            case tab2: Gate.instance.sendNotification(MsgConstant.MSG_GUILD_DISPLAY_TABLE, Table.Table2);
                break;
            case tab3: Gate.instance.sendNotification(MsgConstant.MSG_GUILD_DISPLAY_TABLE, Table.Table3);
                break;
            case tab4: Gate.instance.sendNotification(MsgConstant.MSG_GUILD_DISPLAY_TABLE, Table.Table4);
                break;
            case tab5: Gate.instance.sendNotification(MsgConstant.MSG_GUILD_DISPLAY_TABLE, Table.Table5);
                break;
            case tab6: Gate.instance.sendNotification(MsgConstant.MSG_GUILD_DISPLAY_TABLE, Table.Table6);
                break;
            case toggle:
                Gate.instance.sendNotification(MsgConstant.MSG_GUILD_DISPLAY_MEMBER_TOGGLE,
                    transform.GetComponent<UIToggle>().value
                    );
                break;
            case closeShowInfo:
                //关闭
                Gate.instance.sendNotification(MsgConstant.MSG_GUILD_DISPLAY_MEMBER_INFO,
                -1);
                break;
            case closeOffice:
                Gate.instance.sendNotification(MsgConstant.MSG_GUILD_DISPLAY_MEMBER_OFFICE,false);
                break;
            case enable:
                switch (gameObject.transform.parent.name)
                {
                    case btn_office:
                        Gate.instance.sendNotification(MsgConstant.MSG_GUILD_DISPLAY_MEMBER_OFFICE, true);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
}
