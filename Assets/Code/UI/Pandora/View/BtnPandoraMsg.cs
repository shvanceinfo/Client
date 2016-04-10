/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-6-10
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnPandoraMsg : MonoBehaviour
{

	Transform _trans;
	private const string CLOSE = "close";
	private const string CHALLENGE = "btnchallenge";

	void Awake ()
	{
		this._trans = this.transform;
	}
	
	void OnClick ()
	{
		switch (this.gameObject.name) {
		case CLOSE:
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_PANDORA_UI);
			break;	
			
		case CHALLENGE:
//			Gate.instance.sendNotification (MsgConstant.MSG_PANDORA_CHALLENGE_PANDORA);
			break;
			
		default:
			break;
		}
		
	}
}
