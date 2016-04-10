using UnityEngine;
using System.Collections;
using helper;
using MVC.entrance.gate;

public class BtnInlayGemMsg : MonoBehaviour {


    void OnClick()
    {
        XmlHelper.CallTry(() => { int.Parse(gameObject.name); });
        Gate.instance.sendNotification(MsgConstant.MSG_INLAY_SELECT_GEM,
            gameObject.GetComponent<ItemLabelEx>().Id);
    }
}
