using UnityEngine;
using System.Collections;

public class UIBackToCity : MonoBehaviour {

    //public bool bPickAllTempGoods = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        OnClose();
    }
    void OnClose()
    {
        UIManager.Instance.closeWindow(UiNameConst.ui_award, true);
        MessageManager.Instance.SendAskGoblinMultiBenifit(1);
        UIManager.Instance.showWaitting(true); //强制显示loading界面
        Global.ResetBornData();        
    }    
}
