using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnVipMsg : MonoBehaviour {

    const string LEFT = "Left";
    const string RIGHT = "Right";
    const string CLOSE = "Close";
    const string TABLE1 = "Table1";
    const string TABLE2 = "Table2";
    const string TABLE3 = "Table3";
    const string REVE = "Button_Receive";
    const string ToSHOP = "Button_Add";
    void OnClick()
    {
        Gate.instance.sendNotification(MsgConstant.MSG_VIP_HID_TIP);
        switch (gameObject.name)
        {
            case ToSHOP:
                FastOpenManager.Instance.CleraModelCamera();
                FastOpenManager.Instance.OpenWindow(FunctionName.Shop_BuyDiamond);
                break;
            case LEFT:
                Gate.instance.sendNotification(MsgConstant.MSG_VIP_SWTING_VIP_SHOW, (int)VipManager.Instance.ShowVip.VipId - 1);
                break;
            case RIGHT:
                Gate.instance.sendNotification(MsgConstant.MSG_VIP_SWTING_VIP_SHOW, (int)VipManager.Instance.ShowVip.VipId + 1);
                break;
            case CLOSE:
                VipManager.Instance.CloseWindow();
                break;
            case TABLE1:
                Gate.instance.sendNotification(MsgConstant.MSG_VIP_SWTING_TABLES, Table.Table1);
                break;
            case TABLE2:
                Gate.instance.sendNotification(MsgConstant.MSG_VIP_SWTING_TABLES, Table.Table2);
                break;
            case TABLE3:
                Gate.instance.sendNotification(MsgConstant.MSG_VIP_SWTING_TABLES, Table.Table3);
                break;
            case REVE:
                Gate.instance.sendNotification(MsgConstant.MSG_VIP_RECEIVE_AWARD);
                break;
            default:
                int id;
                if (int.TryParse(gameObject.name,out id))
                {
                    Gate.instance.sendNotification(MsgConstant.MSG_VIP_SHOW_TIP,id);
                }
                break;
        }
        
    }
}
