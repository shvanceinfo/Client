using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using helper;
using manager;

public class BtnRefineMsg : MonoBehaviour {

    const string Table1 = "Table1";
    const string Table2 = "Table2";
    const string Center = "Center";
    const string Button_Reset = "Button_Reset";
    const string Close = "Close";
    const string BTN_OK = "Button_Ok";
    void OnClick()
    {
        
        switch (gameObject.name)
        {
            case Table1:
                Gate.instance.sendNotification(MsgConstant.MSG_REFINE_DISPLAY_LIST_TABLE, Table.Table1);
                RefineManager.Instance.SelectRefineItem = 0;
                break;
            case Table2:
                Gate.instance.sendNotification(MsgConstant.MSG_REFINE_DISPLAY_LIST_TABLE, Table.Table2);
                RefineManager.Instance.SelectRefineItem = 0;
                break;
            case Button_Reset:
                Gate.instance.sendNotification(MsgConstant.MSG_REFINE_RESET,true);
                break;
            case Close:
                Gate.instance.sendNotification(MsgConstant.MSG_REFINE_RESET, false);
                break;
            case BTN_OK:
                Gate.instance.sendNotification(MsgConstant.MSG_REFINE_RESET_OK);
                break;
            default:
                if (gameObject.transform.parent.parent.name.Equals(Center))
                {
                    Gate.instance.sendNotification(MsgConstant.MSG_REFINE_SEND_REFINE, int.Parse(gameObject.transform.parent.name));
                }
                else {
                    XmlHelper.CallTry(() => { int.Parse(gameObject.name); 
                        RefineManager.Instance.SelectRefineItem = int.Parse(gameObject.name); });
                    Gate.instance.sendNotification(MsgConstant.MSG_REFINE_SELECT_ITEM, gameObject.GetComponent<StrengThenDisplayItem>().Id);
                }
                break;
        }
    }
}
