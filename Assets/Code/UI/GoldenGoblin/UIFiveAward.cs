using UnityEngine;
using System.Collections;

public class UIFiveAward : MonoBehaviour {

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
        if (CharacterPlayer.character_asset.diamond < 180)
        {
            // 钻石不够
            FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("golden_goblin_double_benefit_diamond_not_enough"), true, UIManager.Instance.getRootTrans());
        }
        else
        {
            UIManager.Instance.closeWindow(UiNameConst.ui_golden_gain, true);
            MessageManager.Instance.SendAskGoblinMultiBenifit(5);
            UIManager.Instance.showWaitting(true); //强制显示loading界面
            Global.ResetBornData();
        }     
    }    
}
