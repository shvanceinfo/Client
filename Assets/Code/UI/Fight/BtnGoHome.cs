using UnityEngine;
using System.Collections;

public class BtnGoHome : MonoBehaviour {


    void OnExit(eDialogSureType type)
    {
        if (type == eDialogSureType.eExitFight)
        {
            MessageManager.Instance.sendMessageReturnCity();
			UIManager.Instance.showWaitting(true); //回主城强制弹出对话框
            // 返回主城需要把自动战斗去掉
            Global.m_bAutoFightSaved = Global.m_bAutoFight;
            Global.m_bAutoFight = false;
        }
    }

    void OnEnable()
    {
        EventDispatcher.GetInstance().DialogSure += OnExit;
    }

    void OnDisable()
    {
        EventDispatcher.GetInstance().DialogSure -= OnExit;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        OnGoHome();
    }

    void OnGoHome()
    {
        UIManager.Instance.ShowDialog(eDialogSureType.eExitFight, LanguageManager.GetText("msg_exit_fight"));
    }
}
