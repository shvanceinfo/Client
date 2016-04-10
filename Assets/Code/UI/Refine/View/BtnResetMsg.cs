using UnityEngine;
using System.Collections;
using MVC.entrance.gate;

public class BtnResetMsg : MonoBehaviour {

    const string CBK = "CheckBox";
    void OnClick()
    {
        switch (gameObject.name)
        {
            case CBK:
                Gate.instance.sendNotification(MsgConstant.MSG_REFINE_SELECT_RESET_ITEM,
                    int.Parse(gameObject.transform.parent.name));
                break;
            default:
                break;
        }
    }
}
