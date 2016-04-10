/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-4-15
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnSaleMsg : MonoBehaviour
{
	private const  float _delayTime = .5f;		//长按多少时间启动
	private const  float _smoothTime = .1f;	//长按加减值时候的平滑时间
	private float _lastTime = 0;
	private bool _isPress = false;
	private const string BUTTON_MINUS = "Button_Minus";
	private const string BUTTON_ADD = "Button_Add";
	private const string BUTTON_MAX = "Button_Max";
	private const string BUTTON_SALEITEM = "Button_SaleItem";
	private const string BUTTON_CANCEL = "Button_Cancel";
	private const string BUTTON_OPENITEM = "Button_OpenItem";
	
	
	void Awake ()
	{
		
	}
	
	void OnClick ()
	{
		switch (gameObject.name) {
		case BUTTON_MINUS:
			Gate.instance.sendNotification (MsgConstant.MSG_SALE_MINUS);
			break;
		case BUTTON_ADD:
			Gate.instance.sendNotification (MsgConstant.MSG_SALE_ADD);
			break;
		case BUTTON_MAX:
			Gate.instance.sendNotification (MsgConstant.MSG_SALE_MAX);
			break;
		case BUTTON_SALEITEM:
			Gate.instance.sendNotification (MsgConstant.MSG_SALE_ITEM);
			
			UIManager.Instance.closeWindow (UiNameConst.ui_sale, true, true);
			//UIManager.Instance.closeWindow (UiNameConst.ui_common_tips, true, true);
			BagManager.Instance.ShowCareerModel ();
			break;
		case BUTTON_CANCEL:
			UIManager.Instance.closeWindow (UiNameConst.ui_sale, true, true);
			UIManager.Instance.closeWindow (UiNameConst.ui_open, true, true);
			BagManager.Instance.ShowCareerModel ();
			break;
		case BUTTON_OPENITEM:
			Gate.instance.sendNotification (MsgConstant.MSG_OPEN_ITEM);
			
			UIManager.Instance.closeWindow (UiNameConst.ui_open, true, true);
			BagManager.Instance.ShowCareerModel ();
			break;	
			
		default:
			break;
		}
	}
 
	void Update ()
	{
		if (!_isPress) {
			return ;
		}
		
		if (Time.time - _lastTime < _delayTime) {  
			//print (Time.time - _lastTime);
			return ;
		}									//如果长按则执行下面的操作
		//print (Time.time - _lastTime);
		
		switch (gameObject.name) {
		case BUTTON_MINUS:
			Gate.instance.sendNotification (MsgConstant.MSG_SALE_MINUS);
			break;
		case BUTTON_ADD:
			Gate.instance.sendNotification (MsgConstant.MSG_SALE_ADD);
			break;
		default:
			break;
		}
		_lastTime += _smoothTime;
	}
	
	void OnPress (bool isPress)
	{
		if (isPress) {			//按下的时候开始赋状态
			_lastTime = Time.time;
			this._isPress = true;
		} else {
			_lastTime = 0;		//放开的时候还原值
			this._isPress = false;
		}
	}
	
}
