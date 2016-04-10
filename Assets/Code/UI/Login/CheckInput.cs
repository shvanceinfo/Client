/**该文件实现的基本功能等
function:实现昵称输入的检查
author:ljx
date:2013-10-24
**/
using UnityEngine;
using System.Collections;
using System;
using NetGame;

public class CheckInput : MonoBehaviour {
	public static bool firstPress; //使用static使得点击随机名字生效
	UILabel _label;
	private string[] familyNames; //姓
	private string[] names; //名
	
	
	void Awake()
	{
		TextAsset txt = BundleMemManager.Instance.loadResource(PathConst.RAND_NAME_PATH, typeof(TextAsset)) as TextAsset;
        string[] names = txt.text.Split('=');
        if(names.Length == 2)
        {
        	char[] split = { '\r', '\n' };
        	this.names = names[0].Split(split, StringSplitOptions.RemoveEmptyEntries);
        	this.familyNames = names[1].Split(split, StringSplitOptions.RemoveEmptyEntries);
        }
	}
	
	// Use this for initialization
	void Start () 
	{
		firstPress = true;
		LanguageManager.LoadTxt();
		UILabel nickLabel = GameObject.Find("nickLabel").GetComponent<UILabel>();
		nickLabel.fontSize= 30;
		//nickLabel.font.dynamicFontSize = 30;
		nickLabel.text = LanguageManager.GetText("nick_label");
		_label = gameObject.GetComponentInChildren<UILabel>();
		_label.color = Color.white;
		_label.text = LanguageManager.GetText("nick_default");
		_label.alpha = 0.5f;
		GCSendCreateRole sendMsg = new GCSendCreateRole(1, true, randomName(), true, dealNick);
        NetBase.GetInstance().Send(sendMsg.ToBytes());
        UIManager.Instance.showWaitting(true);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void dealNick(int result)
	{
		if(result == 1) //角色昵称重复
		{
			GCSendCreateRole sendMsg = new GCSendCreateRole(1, true, randomName(), true, dealNick);
	        NetBase.GetInstance().Send(sendMsg.ToBytes());
	        UIManager.Instance.showWaitting(true);
		}
	}
	
	void OnClick()
	{
//		if(firstPress)
//		{
//			firstPress = false;
//			_label.text = "";
//			gameObject.GetComponent<UIInput>().text = " "; //必须修改空格
//		}
	}
	
	string randomName()
	{
		int familyIndex = UnityEngine.Random.Range(0, familyNames.Length);
		int nameIndex = UnityEngine.Random.Range(0, names.Length);
		string nickName = familyNames[familyIndex] +  names[nameIndex];
		UIInput input = gameObject.GetComponent<UIInput>();
		if(input != null)
		{
			UILabel tipLable = GameObject.Find("tipLable").GetComponent<UILabel>();
			//tipLable.font.dynamicFontSize = 18;
			tipLable.fontSize = 18;
			input.text = nickName;
		}
		return nickName;
	}
}
