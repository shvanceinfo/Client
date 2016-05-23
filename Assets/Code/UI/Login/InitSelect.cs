/**该文件实现的基本功能等
function: 初始化服务器的区服
author:ljx
date:2013-10-23
**/
using UnityEngine;
using System.Collections;
using LitJson;
using manager;

public class InitSelect : MonoBehaviour 
{
	public UIAtlas uiAltlas;
	public UIFont uiFont;
	
//	private GameObject _selectFlag;	 	//选中标记
//	private GameObject _groupSelected; 	//区域选中框
//	private GameObject _serverSelected; //服务器选中框
	private int _realLoginNum = 0;	//服务器收到的登录服数目
	private int _realServerNum = 0;  //服务器收到的服务器数目
	private GameObject _lastServerClick; //上次高亮的服务器
	private GameObject _lastGroupClick;
	private UILabel _currentServerLbl; //当前所在服务器
	
	const float FLAG_OFFSET_X = -50f;
	const float FLAG_OFFSET_Y = 30f;
	const string LINK_FLAG = "-";
	const string LOGIN_NAME = "登录服";
	const string SERVER_NAME = "服务器";
	const string LOGIN_AREA = "login";
	const string GROUP_AREA = "group";
	const string SERVER_AREA = "server";
	const int LOGIN_NUM = 3;
	const int SERVER_NUM = 9; //至少显示的服务器数目
	const int GROUP_NUM = 30; //每一组的服务器数目
	
	string m_GetServerListUrl = "http://122.226.109.67/gmop/api/GetServerList.php?";
	
	void OnGetServerList()
	{
		Debug.Log (gameObject.name);
		CacheUserInfo userInfo = CacheManager.GetInstance().GetCacheInfo();
		if (userInfo.UserName.Length == 0)
		{
			//未创建帐号
			return;
		}
		UIManager.Instance.showWaitting(true);

        DataReadServer.Instance.DataInfo();
		StartCoroutine(GetServerList(m_GetServerListUrl));
	}
	
	IEnumerator	GetServerList(string szUrl)
	{
		WWW reponse = new WWW(szUrl);
        yield return reponse;
		UIManager.Instance.closeWaitting();
        if (reponse.error != null)
        {
            //Debug.Log("[GET] http request error: "+reponse.error+" "+szUrl);
			FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("取得服务器列表失败！"), true, UIManager.Instance.getRootTrans());
        }
        else
        {
            //Debug.Log("[GET] http request ok: "+reponse.text);
			JsonData data = JsonMapper.ToObject(reponse.text);          
			int state = (int)data["state"];
			string serverList = (string)data["data"];
			if (state == 1)
			{
				//成功
				Debug.Log(serverList);
			}
			else
			{
				FloatMessage.GetInstance().PlayNewFloatMessage(serverList, true, UIManager.Instance.getRootTrans());	
			}
        } 
	}
	
	void Awake()
	{
		_realLoginNum = 1;
		_realServerNum = 8;
		_lastServerClick = null;
		_lastGroupClick = null;
	}
	
	void Start () 
	{
        OnGetServerList();

		_currentServerLbl = GameObject.Find("current_server").GetComponent<UILabel>();
		int i = 0;
		for(i=1; i<=SERVER_NUM; i++)
	    {
	    	if(i <= _realServerNum)
				setBgLabel(SERVER_AREA, i);
			else
				disableCollision(SERVER_AREA, i);
	    }
		for(i=1; i<=LOGIN_NUM; i++)
		{
			if(i <= _realLoginNum)
				setBgLabel(LOGIN_AREA, i);
			else
				disableCollision(LOGIN_AREA, i);
		}
		int groupNum = (int)(_realServerNum/GROUP_NUM) + 1;		
		for(i=1; i<=GROUP_NUM; i++)
	    {
			if(i<=groupNum)
			{
				int begin = (i-1)*GROUP_NUM + 1;
				int end = i*GROUP_NUM;
				if(_realServerNum < end)
					end = _realServerNum;
				string groupName = generateGroupName(begin, end);
				setBgLabel(GROUP_AREA, i, groupName);
			}
			else
				disableCollision(GROUP_AREA, i);
	    }
		Camera.main.GetComponent<ControlUI>().SelectServerUI.active = false; //在这里隐藏对话框
	}
	
	//响应按钮的反应
	void responseClick(GameObject obj)
	{
		string name = obj.name;
		UISprite bgSprite = NGUITools.AddWidget<UISprite>(obj);
		bgSprite.type = UISprite.Type.Sliced;
		bgSprite.name = "click_" + obj.name;
		bgSprite.depth = 7;
		bgSprite.atlas = uiAltlas;
		if(name.IndexOf(SERVER_AREA) != -1)
		{
			bgSprite.spriteName = "big_sel";
			if(_lastServerClick != null)
				Destroy(_lastServerClick);
			_lastServerClick = bgSprite.gameObject;  
			_currentServerLbl.text = obj.GetComponentInChildren<UILabel>().text;
		}
		else if(name.IndexOf(LOGIN_AREA) != -1)
		{
			bgSprite.spriteName = "big_sel";
			if(_lastServerClick != null)
				Destroy(_lastServerClick);
			_lastServerClick = bgSprite.gameObject;
			_currentServerLbl.text = obj.GetComponentInChildren<UILabel>().text;
		}
		else
		{
			bgSprite.spriteName = "small_sel";
			if(_lastGroupClick != null)
				Destroy(_lastGroupClick);
			_lastGroupClick = bgSprite.gameObject;			
		}
		
		UISpriteData innerSp = uiAltlas.GetSprite(bgSprite.spriteName);
 		bgSprite.width = innerSp.width;
		bgSprite.height = innerSp.height;
		bgSprite.transform.localPosition = Vector3.zero;
		bgSprite.transform.localScale = new Vector3(innerSp.width, innerSp.height, 0);
		bgSprite.MakePixelPerfect();	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	//禁用没有服务器按钮的碰撞
	void disableCollision(string name, int index)
	{
		string btnName = name + index;
		GameObject obj = GameObject.Find(btnName);
		if(obj != null)
		{
			UIButton btn = obj.GetComponent<UIButton>();
			if(btn != null)
				btn.collider.active = false;
		}
	}
	
	//设置按钮的高亮背景跟名称
	void setBgLabel(string name, int index, string groupName = null)
	{
		string btnName = name + index;
		GameObject obj = GameObject.Find(btnName);
		if(obj != null)
		{
			UIEventListener.Get(obj).onClick = responseClick;  //增添按钮事件监听
			UISprite bgSprite = NGUITools.AddWidget<UISprite>(obj);
			bgSprite.type = UISprite.Type.Sliced;
			bgSprite.name = "select_" + name +index;
			bgSprite.depth = 6;
			bgSprite.atlas = uiAltlas;
			if(name == GROUP_AREA)
				bgSprite.spriteName = "ser_sel";
			else
				bgSprite.spriteName = "long_name";
			UISpriteData innerSp = uiAltlas.GetSprite(bgSprite.spriteName);
			bgSprite.transform.localPosition = Vector3.zero;
			bgSprite.transform.localScale = new Vector3(innerSp.width, innerSp.height, 0);
			bgSprite.width = innerSp.width;
			bgSprite.height = innerSp.height;
			bgSprite.MakePixelPerfect();	
			if(uiFont != null)
			{
	            UILabel label = NGUITools.AddWidget<UILabel>(obj);  
	            label.font = uiFont;  
				
				label.fontSize = 22;
//				label.font.dynamicFontSize = 22; //需要dynamic字体才能生效
				if(name == SERVER_AREA)
					label.text = SERVER_NAME+index;  
				else if(name == LOGIN_AREA)
					label.text = LOGIN_NAME+index;
				else
					label.text = groupName;
				//obj.name = label.text;
				label.transform.localScale = new Vector3(18f, 18f, 1f); //使用transform字体不生效，还需要是dynamic的字体
				label.transform.localPosition = Vector3.zero;                
				label.depth = 12;		
	            label.color = Color.white;  
	            label.MakePixelPerfect();  
			}
			if(index == 1)				
				responseClick(obj);
		}
	}
	
	//服务器组的生成
	string generateGroupName(int begin, int end)
	{
		string firstLabel = begin.ToString();
		string endLabel = end.ToString();
		if(begin < 10) //个位数前面加零
			firstLabel = "0" + firstLabel;
		if(end < 10)
			endLabel = "0" + endLabel;
		return firstLabel + LINK_FLAG + endLabel;
	}
	
//	//这里根据服务器的选中状态来打标记
//	public void tagBtn(string type, int index)
//	{
//		string name = type + index;
//		GameObject obj = GameObject.Find(name);
//		if(_selectFlag != null && obj != null)
//		{
//			Object newObj = BundleMemManager.Instance.instantiateObj(_selectFlag);  //屏蔽了复制对象方法
//			GameObject cloneFlag = newObj as GameObject;
//			cloneFlag.transform.parent = obj.transform; //只要parent设置好就添加到子对象中了
//			cloneFlag.transform.localPosition = new Vector3(FLAG_OFFSET_X, FLAG_OFFSET_Y, 0f);
//			cloneFlag.name = "flag"+type+index;
//			cloneFlag.SetActive(true);
//			//cloneFlag.transform.localRotation = Quaternion.identity；
//			cloneFlag.transform.localScale = _selectFlag.transform.localScale;
//			cloneFlag.GetComponent<UISprite>().pivot = UIWidget.Pivot.TopLeft;		
//		}
//	}


    
}
