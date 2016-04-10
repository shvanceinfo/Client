using UnityEngine;
using System.Collections;

public class BtnMainSetting : MonoBehaviour 
{	
	void OnClickFinish()
    {
        transform.parent.GetComponent<funcMgr>().ResetTime();
        UIManager.Instance.openWindow(UiNameConst.ui_service);     
    }
}
