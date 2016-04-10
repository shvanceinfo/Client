using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class ChangeLoginUI : MonoBehaviour {
	
	public enum eLoginUIState
	{
		eLogin,
		eRegistAccount,
		eChangeAccount,
	};
	
	public eLoginUIState m_eState;
	
	private ControlUI _controlUI; //获取控制UI的控件

	public GameObject registBtn;	//注册帐号按钮 
	public GameObject selectBtn;	//选择服务器按钮  
	public GameObject loginBtn;		//登录游戏按钮 
	public GameObject m_registPanel;//注册帐号面板 
	
	// Use this for initialization
	void Start () 
	{
        _controlUI = Camera.main.GetComponent<ControlUI>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnClick()
    {
        OnChangeUIState();
    }
	public	void OnChangeUIState()
	{
		switch(m_eState)
		{
		case eLoginUIState.eLogin:
			{
				ShowLoginUI();
			}
			break;
		case eLoginUIState.eRegistAccount:
			{
				ShowRegistAccountUI();
			}
			break;
		case eLoginUIState.eChangeAccount:
			{
				ShowChangeAccountUI();
			}
			break;
		default:
			break;
		}

        switch (transform.name)
        {
            case "cancleBtn":
                //loginBtn.transform.parent.FindChild("current_server").GetComponent<UILabel>().text = ServerManager.Instance.CurrentVo.Name;
                break;
            case"sureBtn":
                break;
            default:
                break;
        }

        
	}
	
	void ShowLoginUI()
	{
		Debug.Log("ShowLoginUI is called");
		if (registBtn  && loginBtn)
		{
			registBtn.SetActive(true);
			loginBtn.SetActive(true);
			selectBtn.SetActive(true);
            _controlUI._loginFunction.SetActive(true);
            _controlUI._loginFunction.transform.FindChild("Password").GetComponent<SetAccountName>().DisplayRegist();
		}
		
		if (_controlUI)
		{
			_controlUI.ServerMask.SetActive(false); //隐藏遮罩
			_controlUI.SelectServerUI.SetActive(false);
			_controlUI.SerSelectBg.SetActive(false);
			//controlUI.ServerRoleUI.SetActive(false);
		}
		
		if (m_registPanel)
		{
			m_registPanel.SetActive(false);
		}

        //ServerManager.Instance.lastVo = ServerManager.Instance.CurrentVo;
        if (transform.name == "sureBtn")
        {
            ServerManager.Instance.CurrentVo = ServerManager.Instance.ChooseVo;
            if (ServerManager.Instance.CurrentVo != null)
            {
                //记录选择serverid
                CacheManager.GetInstance().SetServerId(ServerManager.Instance.CurrentVo.ServerId);
                loginBtn.transform.parent.FindChild("current_server").GetComponent<UILabel>().text = ServerManager.Instance.CurrentVo.Name;
            }
        }
	}
	
	void ShowRegistAccountUI()
	{
		Debug.Log("ShowRegistAccountUI is called");
		if (registBtn  && loginBtn)
		{
			registBtn.SetActive(false);
			loginBtn.SetActive(false);
			selectBtn.SetActive(false);
            _controlUI._loginFunction.SetActive(false);
		}

        if (m_registPanel)
        {
            RegistAccount obj = m_registPanel.GetComponent<RegistAccount>();
            obj.m_type = RegistAccount.eAccountType.eRegistAccount;
            m_registPanel.SetActive(true);
        }


	}
	
	void ShowChangeAccountUI()
	{
		Debug.Log("ShowRegistAccountUI is called");
		if (registBtn  && loginBtn)
		{
			registBtn.SetActive(false);
			loginBtn.SetActive(false);
			selectBtn.SetActive(false);
		}
		
		if (m_registPanel)
		{
			RegistAccount obj = m_registPanel.GetComponent<RegistAccount>();
			obj.m_type = RegistAccount.eAccountType.eChangeAccount;
			m_registPanel.SetActive(true);
		}
	}
}

