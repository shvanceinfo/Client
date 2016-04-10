using UnityEngine;
using System.Collections;
using MVC.entrance.gate;

public class BtnEmailMsg : MonoBehaviour {

    const string button="Button";
    const string Close = "Close";
    void OnClick()
    {
        switch (gameObject.name)
        {
            case Close:
                Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_CLOSE);
                break;
            case button:
                Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_RECEIVE);
                break;
        }
    }
}
