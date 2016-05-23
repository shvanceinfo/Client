/**该文件实现的基本功能等
function: 点击登录的按钮实现功能
author:ljx
date:2013-10-29
**/
using UnityEngine;
using System.Collections;
using helper;
using manager;
using MVC.entrance.gate;
using model;

public class SelectServer : MonoBehaviour 
{
	public static SelectServer sSelect;
	private bool _click;
	private ControlUI _controlUI; //获取控制UI的控件
	// Use this for initialization
	GameObject registBtn;	//注册帐号按钮 
	GameObject loginBtn;	//登录游戏按钮 
    private UILabel _accontLbl; //输入账户昵称
    private UIInput _inputAccount;
    private UIInput _inputPassword;
	void Awake () 
	{
		Debug.Log (gameObject.name);
		_controlUI = Camera.main.GetComponent<ControlUI>();
		sSelect = this;
		_click = false;
		registBtn = transform.parent.FindChild("registBtn").gameObject;
		loginBtn = transform.parent.FindChild("loginBtn").gameObject;
        _accontLbl = transform.parent.parent.FindChild("Account/accountName").GetComponent<UILabel>();
        _inputAccount = transform.parent.parent.FindChild("Account").GetComponent<UIInput>();
        _inputPassword = transform.parent.parent.FindChild("Password").GetComponent<UIInput>();

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
    public bool CheckAccount()
    {
        if (string.IsNullOrEmpty(_inputAccount.value))
        {
            ViewHelper.DisplayMessageLanguage("login_error_input_accountorpassword");
            return false;
        }
        if (string.IsNullOrEmpty(_inputPassword.value))
        {
            ViewHelper.DisplayMessageLanguage("login_error_input_accountorpassword");
            return false;
        }
        if (_inputAccount.value.Length <= 4 || _inputAccount.value.Length >= 16)
        {
            ViewHelper.DisplayMessageLanguage("login_error_input_length");
            return false;
        }
        if (_inputPassword.value.Length <= 4 || _inputPassword.value.Length >= 16)
        {
            ViewHelper.DisplayMessageLanguage("login_error_input_length");
            return false;
        }
        return true;
    }
	
	void OnClick()
	{
		if(gameObject.name.Equals("reselectBtn")) //出现选择服务器的界面
		{
			_controlUI.SerSelectBg.SetActive(true);
			_controlUI.SelectServerUI.SetActive(true);
			_controlUI.ServerMask.SetActive(true);

			gameObject.SetActive(false); //隐藏重新选服按钮
			if (registBtn  && loginBtn)
			{
				registBtn.SetActive(false);
				loginBtn.SetActive(false);
                _controlUI._loginFunction.SetActive(false);
			}
            Gate.instance.sendNotification(MsgConstant.MSG_SELECT_SERVER_SHOW);
            

		}
		else //登录游戏
		{
            if (!CheckAccount())
            {
                return;
            }
			MainLogic.sMainLogic.ConnectGameServer(false); //链接服务器
			_click = true;
			_controlUI.ServerSelectBtn.active = false;
			_controlUI.ServerMask.SetActive(false); //隐藏遮罩
			if (registBtn  && loginBtn)
			{
				registBtn.SetActive(true);
				loginBtn.SetActive(true);
			}

            //ServerManager.Instance.CurrentVo.ServerState = ServState.RECENT_SERVER;
		}		
	}

    public void DefaultServer()
    {
        if (ServerManager.Instance.CurrentVo != null)
        {
            //记录当前serverid
            CacheManager.GetInstance().SetServerId(ServerManager.Instance.CurrentVo.ServerId);
            if (transform.parent.FindChild("current_server"))
            {
                transform.parent.FindChild("current_server").GetComponent<UILabel>().text = ServerManager.Instance.CurrentVo.Name;
            }
        }
    }


	//只有选人才有回调
	public void createRole(bool hasCreate)
	{
		if(_click || !hasCreate) //确保只做一次创角
		{			
			if(!hasCreate)
			{
				//MainLogic.hasLoadCreateScene = true;				
				GameObject obj = GameObject.Find("Main Camera");
				if(obj != null)
					obj.SendMessage("realCreateRole");
			}
			_click = false;
		}
        _controlUI._loginFunction.SetActive(false);
	}
}
