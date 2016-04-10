using UnityEngine;
using System.Collections;

public class ServerPageMsg : MonoBehaviour {

    const string BTN_RIGHT = "Btn_Right";
    const string BTN_LEFT = "Btn_Left";
    private GameObject tableTrans;
    private GameObject leftBtn, rightBtn;

    void Awake()
    {
        tableTrans = transform.parent.parent.FindChild("ServerList/Grid/Table").gameObject;
        leftBtn = transform.parent.FindChild("Btn_Left").gameObject;
        rightBtn = transform.parent.FindChild("Btn_Right").gameObject;
        
    }

    void OnClick()
    {
        switch (transform.name)
        {
            case BTN_RIGHT:
                if (tableTrans.transform.GetChild(0).localPosition.x % 630 == 0)
                    RightAnim();
                break;
            case BTN_LEFT:
                if (tableTrans.transform.GetChild(0).localPosition.x % 630 == 0)
                    LeftAnim();
                break;
            default:
                break;
        }
    }

    private void LeftAnim()
    {
        for (int i = 0; i < tableTrans.transform.childCount; i++)
        {
            Transform transObj = tableTrans.transform.GetChild(i);
            transObj.GetComponent<TweenPosition>().from = transObj.localPosition;
            transObj.GetComponent<TweenPosition>().to = new Vector3(transObj.localPosition.x + 630, 0, 0);
            transObj.GetComponent<TweenPosition>().ResetToBeginning();
            transObj.GetComponent<TweenPosition>().PlayForward();
        }
        if (tableTrans.transform.childCount > 1 && tableTrans.transform.GetChild(0).localPosition.x >= -650 &&
            tableTrans.transform.GetChild(0).localPosition.x <= -600)
        {
            leftBtn.SetActive(false);
        }
        if (tableTrans.transform.childCount > 1 && tableTrans.transform.GetChild(tableTrans.transform.childCount - 1).localPosition.x <= 20 &&
            tableTrans.transform.GetChild(tableTrans.transform.childCount - 1).localPosition.x >= -30 )
        {
            rightBtn.SetActive(true);
        }
        
    }

    private void RightAnim()
    {
        for (int i = 0; i < tableTrans.transform.childCount; i++)
        {
            Transform transObj = tableTrans.transform.GetChild(i);
            transObj.GetComponent<TweenPosition>().from = transObj.localPosition;
            transObj.GetComponent<TweenPosition>().to = new Vector3(transObj.localPosition.x + -630,0,0);
            transObj.GetComponent<TweenPosition>().ResetToBeginning();
            transObj.GetComponent<TweenPosition>().PlayForward();
        }
        if (tableTrans.transform.childCount > 1 && tableTrans.transform.GetChild(tableTrans.transform.childCount - 1).localPosition.x <= 650
            && tableTrans.transform.GetChild(tableTrans.transform.childCount - 1).localPosition.x >= 600)
        {
            rightBtn.SetActive(false);
        }
        if (tableTrans.transform.childCount > 1 && tableTrans.transform.GetChild(0).localPosition.x <= 20
            && tableTrans.transform.GetChild(0).localPosition.x >= -30)
        {
            leftBtn.SetActive(true);
        }
        
    }

   
}
