using UnityEngine;
using System.Collections;
using System;

public class UiBtnClose : MonoBehaviour {
    private uint startTime;

    void OnEnable()
    {
        startTime = Global.ToUnixTimeStamp(DateTime.Now);
    }

    public void OnClose()
    {
    	if(ServiceManager.Instance.submitOpen)
    	{
    		ServiceManager.Instance.OnCloseSendMsg();
    		ServiceManager.Instance.submitOpen = false;
    	}
    	else
    	{
//	        funcMgr.isChildOpen = false; //还原子菜单为关闭状态
	        UIManager.Instance.closeAllUI();
	        if(this.name.Equals("closeTaskDialog")) //关闭任务对话框
            {
	        	NPCManager.Instance.openNPC.changeNPCLayer(false);
            }
	        NPCManager.Instance.createCamera(false); //清除UI相机
//	        UIManager.Instance.destroyCamera();
//	        BtnFunc.selectType = ePackageFuncType.eNone; //能再选择
    	}
    }
}
