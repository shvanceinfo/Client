/**该文件实现的基本功能等
function: 弹出对话框的数据存储管理
author:zyl
date:2014-4-5
**/
using UnityEngine;
using System.Collections;
using manager;
using MVC.entrance.gate;

public class BtnDialogMsg : MonoBehaviour
{
	private const string  BTN_CANCEL = "btn_cancel";
	private const string BTN_SURE = "btn_sure";
	
	void OnClick ()
	{
		switch (gameObject.name) {
		case BTN_CANCEL:
			Gate.instance.sendNotification(MsgConstant.MSG_DIALOG_CANCEL);
			break;
		case BTN_SURE:
			Gate.instance.sendNotification(MsgConstant.MSG_DIALOG_SURE);
			break;
			
		default:
			break;
		}
	}
}
