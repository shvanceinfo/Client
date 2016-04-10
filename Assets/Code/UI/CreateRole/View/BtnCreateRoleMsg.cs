using UnityEngine;
using System.Collections;
using MVC.entrance.gate;

public class BtnCreateRoleMsg : MonoBehaviour {

    const string createRole = "button_create";
    const string warrior_select = "Warrior";
    const string magic_select = "Magic";
    const string ancher_select = "Ancher";
    const string rollName = "Button_Roll";
    void OnClick()
    {
        switch (gameObject.name)
        {
            case createRole: 
                Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_CREATE);
                break;
            case warrior_select: 
                Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_SELECT_CAREER,CHARACTER_CAREER.CC_SWORD);
                break;
            case magic_select: 
                Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_SELECT_CAREER, CHARACTER_CAREER.CC_MAGICIAN);
                break;
            case ancher_select: 
                Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_SELECT_CAREER, CHARACTER_CAREER.CC_ARCHER);
                break;
            case rollName:
                Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_ROLL_NAME);
                break;
            default:
                break;
        }
    }
   
}
