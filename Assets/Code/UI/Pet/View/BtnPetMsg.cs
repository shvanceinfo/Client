/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-6-3
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnPetMsg : MonoBehaviour
{
	private const string CLOSEPET = "closePet";
	private	const string EVOLUTION = "btnEvolution";
	private const string AUTO_EVOLUTION = "btnAuto";
	private const string BTN_CLICK_UP_ARROW = "upArrow";
	private const string BTN_CLICK_DOWN_ARROW = "downArrow";
	private const string BTN_RET = "btnRet";
	private const string BTN_FOLLOW = "btnFollow";
	private const string TABLE1 = "Table1";
	private const string TABLE2 = "Table2";
	private const string CLOSE_PET_SKILL = "closePetSkill";
	private const string OPEN_PET_SKILL = "skillbgbg";
	void OnClick ()
	{
		switch (gameObject.name) {
		case CLOSEPET:
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_PET_UI);
			break;
		case EVOLUTION:
			Gate.instance.sendNotification (MsgConstant.MSG_PET_EVOLUTION);
			break;
		case AUTO_EVOLUTION:
			Gate.instance.sendNotification (MsgConstant.MSG_PET_AUTO_EVOLUTION);
			break;	
		case BTN_CLICK_UP_ARROW:
			Gate.instance.sendNotification (MsgConstant.MSG_PET_DRAG_PREV);
			break;
		case BTN_CLICK_DOWN_ARROW:
			Gate.instance.sendNotification (MsgConstant.MSG_PET_DRAG_NEXT);
			break;
		case BTN_RET:
			Gate.instance.sendNotification (MsgConstant.MSG_PET_RET);
			break;
		case BTN_FOLLOW:
			Gate.instance.sendNotification (MsgConstant.MSG_PET_FOLLOW);
			break;
		case TABLE1:
			PetManager.Instance.ShowAttr ();
			break;
		case TABLE2:
			PetManager.Instance.ShowEquip ();
			break;
		case CLOSE_PET_SKILL:
			Gate.instance.sendNotification (MsgConstant.MSG_PET_CLOSE_PET_SKILL);
			break;
		case OPEN_PET_SKILL:
			Gate.instance.sendNotification (MsgConstant.MSG_PET_OPEN_PET_SKILL);
			break;

		default:
			break;
		}
		
	}
	 
}
