#define TOLINK
using UnityEngine;
using System.Collections;
using helper;
using MVC.entrance.gate;
using manager;

public class BtnInlayMsg : MonoBehaviour {

    const string BTN_REMOVE = "Button_ReMove";
    const string TO_MERGE = "Button_ToGem";     //转到宝石合成
    const string INLAY_GEM = "Button_Inlay";    //镶嵌宝石
    void OnClick()
    {
        switch (gameObject.name)
        {
            case TO_MERGE:
#if TOLINK
                FastOpenManager.Instance.OpenWindow(FunctionName.Merge);
#else
                ViewHelper.DisplayMessage("暂未开放，敬请期待");
#endif

                break;
            case INLAY_GEM:
                Gate.instance.sendNotification(MsgConstant.MSG_INLAY_GEM_INLAY);
                break;
            case BTN_REMOVE:
                int index=XmlHelper.CallTry(() => { return int.Parse(gameObject.transform.parent.name); });
                Gate.instance.sendNotification(MsgConstant.MSG_INLAY_REMOVE_GEM, index);
                break;
            default:
                XmlHelper.CallTry(() => { int.Parse(gameObject.name); });
                Gate.instance.sendNotification(MsgConstant.MSG_INLAY_SELECT_ITEM,
                    gameObject.GetComponent<StrengThenDisplayItem>().Id);
                break;
        }
    }
}
