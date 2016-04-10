using UnityEngine;
using System.Collections;
using MVC.entrance.gate;

public class BtnLuckStoneMsg : MonoBehaviour {

    const string CHECK_BOX = "CheckBox";
    const string SURE = "Button_Ok";
    const string CLOSE = "Close";
    void OnClick()
    {
        switch (gameObject.name)
        {
            case CHECK_BOX:
                int id = transform.parent.GetComponent<LuckStoneDisplayItem>().Id;
                Gate.instance.sendNotification(MsgConstant.MSG_LUCKSTONE_SELECT_STONE, id);
                break;
            case SURE:
                Gate.instance.sendNotification(MsgConstant.MSG_LUCKSTONE_WINDOW_OPTION, true);
                break;
            case CLOSE:
                Gate.instance.sendNotification(MsgConstant.MSG_LUCKSTONE_WINDOW_OPTION, false);
                break;
            default:
                break;
        }
    }
}
