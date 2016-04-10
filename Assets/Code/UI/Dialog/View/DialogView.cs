/**该文件实现的基本功能等
function: 实现弹出对话框的View控制
author:zyl
date:2014-4-5
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using mediator;
using MVC.entrance.gate;
using model;
using System.Text;

public class DialogView : MonoBehaviour
{
	public const int ROW = 42;	//定义多少字节换行 , UTF-8 1个字符4个字节
	UILabel lblMessage;     //显示的文字
	
	
	void Awake ()
	{
		lblMessage = transform.FindChild ("lbl_message").GetComponent<UILabel> ();
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new DialogMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.DIALOG_MEDIATOR);
		//NPCManager.Instance.createCamera (false); //消除3D相机
	}
	
	
	//显示文字
	public void Show (string msg)
	{
		gameObject.SetActive (true);
		//msg = "11111111111111111111111111111111111111111111111111111111111111";
		//print(Encoding.UTF8.GetBytes(msg).Length);
		if (Encoding.UTF8.GetByteCount (msg) > ROW) {     //如果超过换行数字则换行
			lblMessage.pivot = UIWidget.Pivot.TopLeft;
		}
		lblMessage.text = msg;
		
	}
 
	
}
