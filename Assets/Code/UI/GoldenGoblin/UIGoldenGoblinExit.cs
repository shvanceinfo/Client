using UnityEngine;
using System.Collections;

public class UIGoldenGoblinExit : MonoBehaviour {


    public void OnClick()
    {
        if (ServiceManager.Instance.submitOpen)
        {
            ServiceManager.Instance.OnCloseSendMsg();
            ServiceManager.Instance.submitOpen = false;
        }
        else
        {
            if (UIManager.Instance.getUIFromMemory(UiNameConst.ui_fight) != null)
            {
//            	UIManager.Instance.getUIFromMemory(UiNameConst.ui_fight).GetComponent<UiFightMainMgr>().ClosePauseDialog();
            }
           UIManager.Instance.closeAllUI();
        }
        
    }
}
