/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-4-12
**/
#define VIP
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;
using helper;

public class BtnRoleMsg : MonoBehaviour
{
	private const string CLOSE_ROLE = "close_role";
	private const string ELEMENT = "element";
	private const string TITLE = "title";
	private const string VIP = "vip";
	
	void OnClick ()
	{
		switch (gameObject.name) {
		case CLOSE_ROLE:
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_UI);//关闭界面
			NPCManager.Instance.createCamera (false); //消除3D相机
			break;
		case ELEMENT:
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);	
			break;
		case TITLE:
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_UI);//关闭界面
			NPCManager.Instance.createCamera (false); //消除3D相机
			BagManager.Instance.OpenBag();
			break;	
		case VIP:
#if VIP
                FastOpenManager.Instance.CleraModelCamera();
            FastOpenManager.Instance.OpenWindow(FunctionName.VIP,true);
#else
            ViewHelper.DisplayMessage("暂未开放，敬请期待!");
#endif

            break;	
		default:
			break;
		}
	}
}
