using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnSettingMsg : MonoBehaviour {

    const string Table1 = "Table1";
    const string Table2 = "Table2";
    const string Table3 = "Table3";
    const string Table4 = "Table4";

    const string CheckBox1 = "Checkbox1";
    const string CheckBox2 = "Checkbox2";
    const string CheckBox3 = "Checkbox3";
    const string CheckBox4 = "Checkbox4";

    const string Submit = "Submit";

    const string Music = "Button_Music";
    const string MusicEx = "Button_MusicEx";
    const string Close = "Close";
    const string Relogin = "Button_ReLogin";
    const string UP = "Button_Up";
    const string DOWN = "Button_Down";
    
    void OnClick()
    {
        switch (gameObject.name)
        {
            case Table1:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_SWTICHING_TABLE, Table.Table1);
                break;
            case Table2:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_SWTICHING_TABLE, Table.Table2);
                break;
            case Table3:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_SWTICHING_TABLE, Table.Table3);
                break;
            case Table4:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_SWTICHING_TABLE, Table.Table4);
                break;
            case CheckBox1:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_CHECK_BOX, 0);
                break;
            case CheckBox2:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_CHECK_BOX, 1);
                break;
            case CheckBox3:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_CHECK_BOX, 2);
                break;
            case CheckBox4:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_CHECK_BOX, 3);
                break;
            case Submit:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_SUBMIT);
                break;
            case Music:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_SETMUSIC);
                break;
            case MusicEx:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_SETAUDIO);
                break;
            case Close:
                SettingManager.Instance.CloseWindow();
                break;
            case Relogin:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_RELOGIN);
                break;
            case UP:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_MOVEHELP, false);
                break;
            case DOWN:
                Gate.instance.sendNotification(MsgConstant.MSG_SETTING_MOVEHELP, true);
                break;

            default:
                int option;
                if (int.TryParse(gameObject.name, out option))
                {
                    Gate.instance.sendNotification(MsgConstant.MSG_SETTING_PEOPLE_OPTION, option);
                }
                break;
        }
    }
}
