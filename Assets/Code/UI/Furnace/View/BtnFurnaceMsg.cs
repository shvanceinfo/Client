using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using helper;

public class BtnFurnaceMsg : MonoBehaviour {

    const string Table1 = "Table1";
    const string Table2 = "Table2";

    const string Close = "Close";

    const string TabelFor = "TableFormula";
    const string TableGem = "TableGem";
    void OnClick()
    {
        switch (gameObject.name)
        {
            case Table1:
                Gate.instance.sendNotification(MsgConstant.MSG_FURNACE_SWING_TABLE, Table.Table1);
                break;
            case Table2:
                Gate.instance.sendNotification(MsgConstant.MSG_FURNACE_SWING_TABLE, Table.Table2);
                break;
            case Close:
                UIManager.Instance.closeWindow(UiNameConst.ui_furnace);
                UIManager.Instance.closeWindow(UiNameConst.ui_bag);
                UIManager.Instance.closeWindow(UiNameConst.ui_medal);
                break;
            case TabelFor:
                Gate.instance.sendNotification(MsgConstant.MSG_FURNACE_SWING_MERGE_TABLE, Table.Table2);
                break;
            case TableGem:
                Gate.instance.sendNotification(MsgConstant.MSG_FURNACE_SWING_MERGE_TABLE, Table.Table1);
                break;
            default:
                break;
        }
    }
}
