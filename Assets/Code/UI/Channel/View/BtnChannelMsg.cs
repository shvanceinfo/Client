using UnityEngine;
using System.Collections;
using manager;
using MVC.entrance.gate;

public class BtnChannelMsg : MonoBehaviour {

    const string close = "close";
    const string submit = "button_ok";
    const string left = "left";
    const string right = "right";
    private int line = 0;
    void OnClick()
    {
        switch (gameObject.name)
        {
            case close:
                ChannelManager.Instance.CloseWindow();
                break;
            case submit:
                {
                    Gate.instance.sendNotification(MsgConstant.MSG_CHANNEL_CHANGE_SUBMIT);
                    PlayerManager.Instance.destroyAllPlayerOther();
                }
                break;
            case left:
                if (int.TryParse(gameObject.transform.parent.name,out line))
                {
                    Gate.instance.sendNotification(MsgConstant.MSG_CHANNEL_CHANGE_LINE, line);
                }
                break;
            case right:
                if (int.TryParse(gameObject.transform.parent.name, out line))
                {
                    Gate.instance.sendNotification(MsgConstant.MSG_CHANNEL_CHANGE_LINE, line+1);
                }
                break;
        }
    }
}
