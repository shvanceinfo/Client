using UnityEngine;
using System.Collections;

public class SetAccountName : MonoBehaviour {
	
	public UIInput _AccountInput;
	public UIInput _PasswordInput;
    public bool isAccount;
    public bool isRemeber;
    CacheUserInfo userCache;
    UIToggle to;
    private void Awake()
    {
        userCache = CacheManager.GetInstance().GetCacheInfo();
        if (isRemeber)
        {
            to = transform.parent.FindChild("GetPassword/CheckBox").GetComponent<UIToggle>();
        }
        

    }
	// Use this for initialization
	void Start () {
        if (!isRemeber)
        {
            if (isAccount)
            {
                if (userCache.UserName != "")
                {
                    UIInput account = GetComponent<UIInput>();
                    account.value = userCache.UserName;
                }
            }
        }
        else {
            if (userCache.isRememberPassword)
            {
                if (userCache.UserPassword != "")
                {
                    UIInput password = GetComponent<UIInput>();
                    password.value = userCache.UserPassword;
                }
            }
            else {
                userCache.UserPassword = "";
            }
            to.value = userCache.isRememberPassword;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void CheckBoxOnClick()
    {
        if (isRemeber)
        {
            CacheManager.GetInstance().SetRememberPassword(to.value);
            CacheManager.GetInstance().SaveCache();
        }
        
    }

    
    public void DisplayRegist()
    {
        if (userCache.isRememberPassword)
        {
            if (userCache.UserPassword != "")
            {
                UIInput account = GetComponent<UIInput>();
                account.value = userCache.UserPassword;
                to.value = userCache.isRememberPassword;
            }
        }
        
    }

    public void AccountSubmit()
    {
        if (isAccount)
        {
            if (userCache.UserName !=GetComponent<UIInput>().value) {
            	userCache.UserName=GetComponent<UIInput>().value;
				userCache.UserPassword="";
				_PasswordInput.value="";
            } ;
        }
    }
    public void PasswordSubmit()
    {
        if (isRemeber)
        {
            userCache.UserPassword = GetComponent<UIInput>().value;
        }
    }
}
