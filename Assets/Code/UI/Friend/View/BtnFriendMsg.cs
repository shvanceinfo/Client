using UnityEngine;
using System.Collections;
using manager;
using MVC.entrance.gate;
public class BtnFriendMsg : MonoBehaviour {

    const string OPEN_ADDFRIEND = "Button_Add_Friend";
    const string CLOSE_ADDFRIEND = "Button_Close_AddFriend";
    const string OK_ADDFRIEND = "Button_Add_Friend_True";
    const string OPEN_RECORD = "Button_Apply_Record";
    const string CLOSE_RECORD = "Shenqing_Close";
    const string CLOSE_WINDOW = "Button_Close";
    const string TABLE1 = "Table1";
    const string TABLE2 = "Table2";

    const string R_OK = "Record_Button_Agree";
    const string R_NO = "Record_Button_Refuse";

    const string DELETE_FRIEND = "Button_Delete_Friend";
    const string DELETE_SURE = "Butto_Delete_Agree";
    const string DELETE_CANSLE = "Button_Delete_Rufuse";
    const string DELETE_CLOSE = "Button_Close_DeleteFriend";

    const string SEND = "Button_Set";
    const string RECEIVE = "Button_Get";
    const string HANDINFO = "Sprite_Head";
    const string CLOSE_INFO = "Button_Close_Friend";
    const string INFO_SEND = "Button_Set_TiLi";
    const string INFO_DELETE = "Button_Delete_Friend1";

    const string BTN_SEND_ALL = "Button_Set_All";
    const string BTN_RECEIVE_ALL = "Button_Get_All";
    const string BTN_All_Agree = "All_Agree";
    const string BTN_All_Refuse = "All_Refuse";
    const string BTN_WHISPER = "Button_Say";
    const string INFO_WHISPER = "Button_Set_Say";
	void OnClick()
	{
        switch (gameObject.name)
        {
            case OPEN_ADDFRIEND:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_ADDFRIEND, true);
                break;
            case CLOSE_ADDFRIEND:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_ADDFRIEND, false);
                break;
            case OK_ADDFRIEND:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_ADD_TRUE);
                break;
            case OPEN_RECORD:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_RECORD_SHOW,Table.Table1);
                break;
            case CLOSE_RECORD:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_RECORD_CLOSE);
                break;
            case CLOSE_WINDOW:
                FriendManager.Instance.CloseWindow();
                break;
            case TABLE1:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_RECORD_SHOW, Table.Table1);
                break;
            case TABLE2:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_RECORD_SHOW, Table.Table2);
                break;
            case R_OK:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_RECORD_ADD_FRIENT,
                    int.Parse(gameObject.transform.parent.name));
                break;
            case R_NO:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_RECORD_RESULT_FRIENT,
                    int.Parse(gameObject.transform.parent.name));
                break;
            case DELETE_FRIEND:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DELETE, int.Parse(gameObject.transform.parent.name));
                break;
            case DELETE_SURE:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DELETE_TRUE);
                break;
            case DELETE_CANSLE:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DELETE_RUFUSE);
                break;
            case DELETE_CLOSE:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DELETE_RUFUSE);
                break;

            case SEND:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_SEND_TILI, int.Parse(gameObject.transform.parent.name));
                break;
            case RECEIVE:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_RECEIVE_TILI, int.Parse(gameObject.transform.parent.name));
                break;
            case HANDINFO:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_INFO,int.Parse(gameObject.transform.parent.name));
                break;
            case CLOSE_INFO:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_MESSAGE_CLOSE);
                break;
            case INFO_SEND:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_SEND_TILI,null);
                break;
            case INFO_DELETE:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DELETE,null);
                break;
            case BTN_SEND_ALL:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_OPT, 0);
                break;
            case BTN_RECEIVE_ALL:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_OPT, 1);
                break;
            case BTN_All_Agree:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_OPT, 2);
                break;
            case BTN_All_Refuse:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_OPT, 3);
                break;
            case BTN_WHISPER:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_WHISPER, int.Parse(gameObject.transform.parent.name));
                break;
            case INFO_WHISPER:
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_WHISPER,null);
                break;
            default:
                break;
        }
		
	}
	
	
}
