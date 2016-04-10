/**该文件实现的基本功能等
function:
author:ljx
date:
**/
using UnityEngine;
using System.Collections;
using NetGame;

public class CreateRole : MonoBehaviour 
{
	private static bool _isMale; //人物选择的性别
	private static CHARACTER_CAREER _vocation;
	
	void OnClick()
    {    
		UILabel tipLable;
		if(this.name == "sureBtn") //点击了确认按钮
		{
			UIInput input = GameObject.Find("nameInput").GetComponent<UIInput>();
			if(input != null)
			{
				string nickName = input.text;
				nickName = nickName.Trim();
				tipLable = GameObject.Find("tipLable").GetComponent<UILabel>();
				tipLable.fontSize = 18;
//				tipLable.font.dynamicFontSize = 18;
				if(string.IsNullOrEmpty(nickName) || nickName.Equals(LanguageManager.GetText("nick_default")))
				{
					tipLable.text = LanguageManager.GetText("nick_empty");				
				}
				else if(nickName.Length > 8)
				{
					tipLable.text = LanguageManager.GetText("nick_outof");		
				}
				else if (NetBase.GetInstance().IsConnected)
				{				
					GCSendCreateRole sendMsg = new GCSendCreateRole((int)_vocation, _isMale, nickName, false, processResult);
		            NetBase.GetInstance().Send(sendMsg.ToBytes());
		            UIManager.Instance.showWaitting(true);
				}
				else
				{
					tipLable.text = "服务器还没有成功链接";
					//tipLable.text = LanguageManager.GetText("nick_not_connect");	
				}
			}
		}
		else if(this.name == "cancleBtn") //点击了取消按钮
		{
			GameObject obj = GameObject.Find("Main Camera");
			obj.SendMessage("rollBackAnimation");
			tipLable = GameObject.Find("tipLable").GetComponent<UILabel>();
			tipLable.text = "";
			GameObject.Find(Constant.SERVER_ROLE_TITLE).GetComponent<UILabel>().text = "选择角色";
			GameObject.Find("role_bottom").SetActive(false);
		}
		else 
		{
			string title="";
			if(this.name == Constant.SELECT_VOCATION1) //点击了角色1
			{
				_vocation = CHARACTER_CAREER.CC_ARCHER;
				_isMale = false;
				title = LanguageManager.GetText("vocation1");
			}
			else if(this.name == Constant.SELECT_VOCATION2)  //点击了角色2
			{
				_vocation = CHARACTER_CAREER.CC_SWORD;
				_isMale = true;
				title = LanguageManager.GetText("vocation2");
			}
			else if(this.name == Constant.SELECT_VOCATION3)  //点击了角色3
			{
				_vocation = CHARACTER_CAREER.CC_MAGICIAN;
				_isMale = false;
				title = LanguageManager.GetText("vocation3");
			}
			//GameObject.Find(Constant.SERVER_ROLE_TITLE).GetComponent<UILabel>().text = title;
			GameObject obj = GameObject.Find("Main Camera");
			obj.SendMessage("playNewAnimation", _vocation);
			GameObject.Find(Constant.SELECT_VOCATION1).SetActive(false);
			GameObject.Find(Constant.SELECT_VOCATION2).SetActive(false);
			GameObject.Find(Constant.SELECT_VOCATION3).SetActive(false);
			GameObject.Find("roleInfo").SetActive(false);
		}
    }
	
	//进入创角之后的创立角色处理，收到创角之后的回调
	void processResult(int result)
	{
		UIManager.Instance.closeWaitting();
		UILabel tipLable = GameObject.Find("tipLable").GetComponent<UILabel>();
//		tipLable.font.dynamicFontSize = 18;
		tipLable.fontSize = 18;
		if(result == 0) //创角成功
		{
			tipLable.text = LanguageManager.GetText("nick_success");
			MessageManager.Instance.sendMessageSelectRole();
			UIManager.Instance.showWaitting(true);
			//MainLogic.hasLoadCreateScene = false; //标记为创角结束，能够加载游戏场景
			tipLable.transform.parent.gameObject.SetActive(false); //
		}
		else if(result == 1) //角色昵称重复
		{
			tipLable.text = LanguageManager.GetText("nick_success");	
		}
		else //其他的服务器未知错误
		{
			Loger.Log("创建角色服务器返回错误码"+result);
			tipLable.text = "创建角色服务器返回错误码"+result;	
		}
	}
	
	//getter and setter
	public static CHARACTER_CAREER Vocation
	{
		get { return _vocation; }
	}
}
