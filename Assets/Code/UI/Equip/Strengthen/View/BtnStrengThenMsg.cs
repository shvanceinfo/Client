using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using helper;
using manager;

public class BtnStrengThenMsg : MonoBehaviour {

    const string Table1 = "Table1";
    const string Table2 = "Table2";
    const string LuckStone = "Button_LuckStone";
    const string Send = "Button_Ok";
    void OnClick()
    {
        switch (gameObject.name)
        {
            case Table1:
                Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_DISPLAY_LIST_TABLE, Table.Table1);
                StrengThenManager.Instance.ChooseItem = 0;
                break;
            case Table2:
                Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_DISPLAY_LIST_TABLE, Table.Table2);
                StrengThenManager.Instance.ChooseItem = 0;
                break;
            case LuckStone:
                LuckStoneManager.Instance.OpenWindow();
                break;
            case Send:
                Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_ASK_ST);
                break;
            default:
                XmlHelper.CallTry(() => { int.Parse(gameObject.name); });
                Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_SELECT_ITEM, gameObject.GetComponent<StrengThenDisplayItem>().Id);
                StrengThenManager.Instance.ChooseItem = int.Parse(gameObject.name); 
                break;
        }
    }
}
