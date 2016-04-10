//#define	Test

/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-5-12
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;
using model;

public class BtnEventMsg : MonoBehaviour
{
	const string CLOSE = "close";
	const string BTN = "btn";
	
	void OnClick ()
	{
		switch (gameObject.name) {
			
		case CLOSE:
			EventManager.Instance.CloseUI ();
			
			break;
		
		case BTN:
			
#if Test
			
#else
			int key = int.Parse (this.transform.parent.name);
			switch (EventManager.Instance.DictionaryEvent [key].Type) {
			case ActivityType.Boss:
				if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Aoding)) {
					return;
				}
				Gate.instance.sendNotification (MsgConstant.MSG_EVENT_ASKJOIN, key);//网络通信，加入活动
				break;
			case ActivityType.Dungeon:
				break;
			default:
				break;
			}
			

#endif	
			
			
			break;	
			
			
			
		default:
			break;
		}
	}
}
