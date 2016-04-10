using UnityEngine;
using System.Collections;
using manager;
using MVC.entrance.gate;
using helper;

public class BtnMergeMsg : MonoBehaviour {

    const string BTN_LUCKSTONE = "Button_Select";       //打开强化石面板
    const string ADD = "add";
    const string MINUS = "minus";
    const string MAX = "max";
    const string Merge = "Button";
    void OnClick()
    {
        switch (gameObject.name)
        {
            case BTN_LUCKSTONE:
                LuckStoneManager.Instance.OpenWindow();
                break;
            case ADD:
                Gate.instance.sendNotification(MsgConstant.MSG_MERGE_SET_MERGE_COUNT, MergeManager.Instance.SelectCount + 1);
                break;
            case MINUS:
                Gate.instance.sendNotification(MsgConstant.MSG_MERGE_SET_MERGE_COUNT, MergeManager.Instance.SelectCount - 1);
                break;
            case MAX:
                Gate.instance.sendNotification(MsgConstant.MSG_MERGE_SET_MERGE_COUNT, MergeManager.Instance.CanMergeCount);
                break;
            case Merge:
                Gate.instance.sendNotification(MsgConstant.MSG_MERGE_BUTTON_MERGE);
                break;
            default:
                XmlHelper.CallTry(() => (int.Parse(gameObject.name)));  //试试能不能转成数字，如果可以，则继续，错误，抛异常
                Gate.instance.sendNotification(MsgConstant.MSG_MERGE_SELECT_ITEM, transform.GetComponent<MergeDisplayItem>().Id);
                break;
                
        }
    }
}
