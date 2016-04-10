using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using model;
using manager;

public class BtnTalkMsg : MonoBehaviour {

    const string TAB1 = "Table1";
    const string TAB2 = "Table2";
    const string TAB3 = "Table3";
    const string TAB4 = "Table4";
    const string TAB5 = "Table5";
    const string CURCHANNEL = "Type";
    const string WORLD = "0";
    const string GUILD = "1";
    const string WHISPER = "2";
    const string CLOSE = "Close";
    const string SENDMSG = "Button_Send";
    const string URL = "TextList";
    const string BTN_OK = "Button_Ok";
    const string BTN_FRIENDS_LIST = "Button_Firends";

    const string F0 = "F0";//添加好友 
    const string F1 = "F1";//私聊
    const string F2 = "F2";     

    void OnClick()
    {
        Gate.instance.sendNotification(MsgConstant.MSG_TALK_HIDDEN_PLAYER_TIP);

        switch (gameObject.name)
        {
            case F0:
                Gate.instance.sendNotification(MsgConstant.MSG_TALK_FAST_ADD_FRIEND);
                break;
            case F1:
                Gate.instance.sendNotification(MsgConstant.MSG_TALK_WHISPER_PLAYER);
                break;

            case TAB1: Gate.instance.sendNotification(MsgConstant.MSG_TALK_SWTING_TABLE, Table.Table1);
                break;
            case TAB2: Gate.instance.sendNotification(MsgConstant.MSG_TALK_SWTING_TABLE, Table.Table2);
                break;
            case TAB3: Gate.instance.sendNotification(MsgConstant.MSG_TALK_SWTING_TABLE, Table.Table3);
                break;
            case TAB4: Gate.instance.sendNotification(MsgConstant.MSG_TALK_SWTING_TABLE, Table.Table4);
                break;
            case TAB5: Gate.instance.sendNotification(MsgConstant.MSG_TALK_SWTING_TABLE, Table.Table5);
                break;
            case CURCHANNEL:
                Gate.instance.sendNotification(MsgConstant.MSG_TALK_DISPALY_CHANNELLIST, true);
                break;
            case WORLD:
                Gate.instance.sendNotification(MsgConstant.MSG_TALK_SELECT_CHANNEL, TalkType.World);
                break;
            case GUILD:
                Gate.instance.sendNotification(MsgConstant.MSG_TALK_SELECT_CHANNEL, TalkType.Guild);
                break;
            case WHISPER:
                Gate.instance.sendNotification(MsgConstant.MSG_TALK_SELECT_CHANNEL, TalkType.Whisper);
                break;
            case CLOSE:
                TalkManager.Instance.CloseWindow();
                break;
            case SENDMSG:
                Gate.instance.sendNotification(MsgConstant.MSG_TALK_SEND_MSG);
                break;
            case URL:
                Gate.instance.sendNotification(MsgConstant.MSG_TALK_CLICK_URL);
                break;
            case BTN_OK:
                Gate.instance.sendNotification(MsgConstant.MSG_TALK_ENTER_FRIEDN_NAME);
                break;
            case BTN_FRIENDS_LIST:
                Gate.instance.sendNotification(MsgConstant.MSG_TALK_DISPLAY_FRIEND_LIST,true);
                break;
            default:
                break;
        }
    }
}
