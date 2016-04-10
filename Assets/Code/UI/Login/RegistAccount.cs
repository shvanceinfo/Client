using UnityEngine;
using System.Collections;
using LitJson;
using System.Text.RegularExpressions;

public class RegistAccount : MonoBehaviour {
	
	UILabel m_Account;
	UILabel m_Pwd;
	UIInput m_PwdInput;
	
	string invalidAccount = "";
	string invalidPwd = "";
	public Transform maskObj;
	public UILabel m_AccountLabel;
    private UILabel _error;
	string m_szAccount;		//注册或者切换成功的帐号
	string m_szEncryPwd;	//注册或者切换成功的帐号
	
	GameObject cancelBtn;
	
	string m_RegistUrlPrefix = "http://122.226.109.67/gmop/api/RegeditAccount.php?";
	string m_ChangeAccountUrl = "http://122.226.109.67/gmop/api/AccountLogin.php?";

    private void Awake()
    {
        _error = transform.FindChild("Label").GetComponent<UILabel>();
        _error.text = "";
    }
	string CreateRegistUrl(string szPrefix, string szAccount, string szPwd, string szRoleID)
	{
		string url = szPrefix+"accountid="+szAccount+"&pw=";
		string encryPwd = Global.GetMd5Hash(szPwd);
		if (!string.IsNullOrEmpty(szRoleID))
		{
			url += encryPwd+"&roleid="+szRoleID;
		}
		else
		{
			url += encryPwd;
		}
		
		m_szAccount = szAccount;
		m_szEncryPwd = encryPwd;
		return url;
	}
	
	string CreateChangeAccUrl(string szPrefix, string szAccount, string szPwd)
	{
		string url = szPrefix+"account="+szAccount+"&pw=";
		string encryPwd = Global.GetMd5Hash(szPwd);
		url += encryPwd;
		
		m_szAccount = szAccount;
		m_szEncryPwd = encryPwd;
		return url;
	}
	
	public enum eAccountType
	{
		eRegistAccount,
		eChangeAccount,
	}
	
	public eAccountType m_type;
	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
		cancelBtn = transform.FindChild("cancelObj/cancleBtn").gameObject;
	}
	
	void OnEnable()
	{
		m_Account = transform.FindChild("account/accInput/Label").GetComponent<UILabel>();
		m_Pwd = transform.FindChild("passwd/pwdInput/Label").GetComponent<UILabel>();
		
		m_PwdInput =  transform.FindChild("passwd/pwdInput").GetComponent<UIInput>();
		
		/*
		if (m_type == eAccountType.eRegistAccount)
		{
			LanguageManager.SetText(ref m_Account, "account_input_desc");
		}
		else
		{
			LanguageManager.SetText(ref m_Account, "change_account_input_desc");
		}
		*/
		LanguageManager.SetText(ref m_Account, "account_input_desc");
		invalidAccount = m_Account.text;
		 
//		m_Pwd.password = false;
		m_PwdInput.inputType = UIInput.InputType.Password;
		
		
		LanguageManager.SetText(ref m_Pwd, "password_input_desc");
		invalidPwd = m_Pwd.text;
		maskObj.gameObject.SetActive(true);
		
		//m_Pwd.hasChange = false;
		//m_Pwd.
		//StartCoroutine("SetPasswd");
		//LanguageManager.SetText(ref m_Pwd, "password_input_desc");
	}
	
	void OnDisable()
	{
		maskObj.gameObject.SetActive(false);
	}
	
	IEnumerator SetPasswd()
	{
		yield return 2;
		//m_Pwd.showLastPasswordChar = true;
		//m_Pwd.password = true;
		m_PwdInput.inputType = UIInput.InputType.Password;
	}	
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnBtnClick()
	{
		if (m_type == eAccountType.eRegistAccount)
		{
			OnRegistAccount();
		}
		else
		{
			OnChangeAccount();
		}
	}
	
	void OnRegistAccount()
	{
		string szAccount = m_Account.text;
		string szPwd = m_Pwd.text;
		
		if (szAccount == invalidAccount || szPwd == invalidPwd)
		{
			//FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("error_msg_account_bad"), true, UIManager.Instance.getRootTrans());
            _error.text = LanguageManager.GetText("error_msg_account_bad");
			return;
		}
        if (szAccount.Length<Constant.MIN_SIZE||
            szAccount.Length>Constant.MAX_SIZE||
            szPwd.Length<Constant.MIN_SIZE||
            szPwd.Length>Constant.MAX_SIZE
            )
        {
            _error.text = LanguageManager.GetText("msg_account_lengt");
            return;
        }
        if (!Regex.IsMatch(szAccount,Constant.AccountRegex))
        {
            _error.text = LanguageManager.GetText("msg_account_char_error");
            return;
        }
        
		
		CacheUserInfo userInfo = CacheManager.GetInstance().GetCacheInfo();
		string registUrl = CreateRegistUrl(m_RegistUrlPrefix, szAccount, szPwd, userInfo.GUID);
		//Debug.Log("regist url: "+registUrl);
		UIManager.Instance.showWaitting(true);
		StartCoroutine(SendRegistData(registUrl));
	}
	
	IEnumerator SendRegistData(string szUrl)
	{
		//Debug.Log("SendRegistData !!!!!!!!!!!!!!");
		WWW reponse = new WWW(szUrl);
        yield return reponse;
		UIManager.Instance.closeWaitting();
        if (reponse.error != null)
        {
            //Debug.Log("[GET] http request error: "+reponse.error+" "+szUrl);
            _error.text = LanguageManager.GetText("msg_regist_fail");
        }
        else
        {
            //Debug.Log("[GET] http request ok: "+reponse.text);
			JsonData data = JsonMapper.ToObject(reponse.text);          
			int state = (int)data["state"];
			string errMsg = (string)data["data"];
			if (state == 1)
			{
				//注册成功，保存帐号信息
				CacheUserInfo userInfo = CacheManager.GetInstance().GetCacheInfo();
				if (userInfo.UserName.Length > 0)
				{
					userInfo.GUID = "";
				}
                userInfo.UserName = m_szAccount;
                userInfo.UserPassword = m_szEncryPwd;
                userInfo.isRememberPassword = true;
                CacheManager.GetInstance().SetUserLoginInfo(userInfo);
                CacheManager.GetInstance().SaveCache();

				//userInfo.GUID = "";
				m_AccountLabel.text = m_szAccount;

                _error.text = LanguageManager.GetText("msg_regist_success");
                _error.text = "";
				cancelBtn.SendMessage("OnChangeUIState");
			}
			else
			{
                _error.text = LanguageManager.GetText("msg_account_account_exist");
			}
        } 
	}
	
	void OnChangeAccount()
	{
		string szAccount = m_Account.text;
		string szPwd = m_Pwd.text;
		
		if (szAccount == invalidAccount || szPwd == invalidPwd)
		{
			FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("error_msg_account_bad"), true, UIManager.Instance.getRootTrans());
			return;
		}
		
		string url = CreateChangeAccUrl(m_ChangeAccountUrl, szAccount, szPwd);
		UIManager.Instance.showWaitting(true);
		StartCoroutine(QueryAccount(url));
	}
	
	IEnumerator	QueryAccount(string szUrl)
	{
		WWW reponse = new WWW(szUrl);
        yield return reponse;
		UIManager.Instance.closeWaitting();
        if (reponse.error != null)
        {
            //Debug.Log("[GET] http request error: "+reponse.error+" "+szUrl);
			FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_account_not_exist"), true, UIManager.Instance.getRootTrans());

        }
        else
        {
            //Debug.Log("[GET] http request ok: "+reponse.text);
			JsonData data = JsonMapper.ToObject(reponse.text);          
			int state = (int)data["state"];
			string errMsg = (string)data["data"];
			if (state == 1)
			{
				//切换帐号成功，保存帐号信息
				CacheUserInfo userInfo = CacheManager.GetInstance().GetCacheInfo();
                userInfo.UserName = m_szAccount;
                userInfo.UserPassword = m_szEncryPwd;
				userInfo.GUID = "";
                CacheManager.GetInstance().SetUserLoginInfo(userInfo);
                CacheManager.GetInstance().SaveCache();
				
				m_AccountLabel.text = m_szAccount;
				
				FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_account_change_success"), false, UIManager.Instance.getRootTrans());	
				cancelBtn.SendMessage("OnChangeUIState");
			}
			else
			{
				FloatMessage.GetInstance().PlayNewFloatMessage(errMsg, true, UIManager.Instance.getRootTrans());	
			}
        } 
	}
}
