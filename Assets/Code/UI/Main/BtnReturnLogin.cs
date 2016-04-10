using UnityEngine;
using System.Collections;
using NetGame;
using System;
using System.Collections.Generic;
using manager;
public class BtnReturnLogin : MonoBehaviour 
{
	void OnClick()
	{
		if(this.name.Equals("BtnReLoginGame"))
		{
			//NetBase.GetInstance().Close(true);
			
		}
	}
	
	void OnEnable()
    {
        EventDispatcher.GetInstance().DialogSure += OnReturnLogin;
    }

    void OnDisable()
    {
        EventDispatcher.GetInstance().DialogSure -= OnReturnLogin;
    }

	void OnReturnLogin(eDialogSureType type)
	{
		//Debug.Log("OnReturnLogin success!");
		UIManager.Instance.closeAllUI();
		Application.LoadLevel("CreateRole");
		//MainLogic.sMainLogic.OnApplicationQuit();
	}
	
	void OnShowDialog()
	{
		UIManager.Instance.ShowDialog(eDialogSureType.eReturnLogin, LanguageManager.GetText("msg_return_login"));
	}
}
