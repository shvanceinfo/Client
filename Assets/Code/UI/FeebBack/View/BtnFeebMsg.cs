using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnFeebMsg : MonoBehaviour {

    const string CLOSE = "Close";
    const string Add = "Button_Add";
    const string Min = "Button_Minus";
    const string Max = "Button_Max";

    const string Table1 = "Table1";
    const string Table2 = "Table2";

    const string BTN_BUY = "Button_BuyItem";
    const string BTN_SHOP = "Button_Shop";

    const string RUN = "Button";
    void OnClick()
    {
        switch (gameObject.name)
        {
            case CLOSE:
                Gate.instance.sendNotification(MsgConstant.MSG_FEEB_CLOSE);
                break;
            case Add:
                Gate.instance.sendNotification(MsgConstant.MSG_FEEB_SET_COUNT, FeebManager.Instance.BuyCount + 1);
                break;
            case Min:
                Gate.instance.sendNotification(MsgConstant.MSG_FEEB_SET_COUNT, FeebManager.Instance.BuyCount - 1);
                break;
            case Max:
                Gate.instance.sendNotification(MsgConstant.MSG_FEEB_SET_COUNT, 99);
                break;

            case Table1:
                Gate.instance.sendNotification(MsgConstant.MSG_FEEB_SHOW_TABLE, Table.Table1);
                break;
            case Table2:
                Gate.instance.sendNotification(MsgConstant.MSG_FEEB_SHOW_TABLE, Table.Table2);
                break;
            case BTN_BUY:
                Gate.instance.sendNotification(MsgConstant.MSG_FEEB_BUY_ITEM);
                break;
            case BTN_SHOP:
                FastOpenManager.Instance.OpenWindow(FunctionName.Shop_BuyDiamond);
                break;
            case RUN:
                int res;
                if (int.TryParse(gameObject.transform.parent.name, out res))
                    Gate.instance.sendNotification(MsgConstant.MSG_FEEB_FAST_OPEN, res);
                    
                break;
            default:
                break;
        }
    }
}
