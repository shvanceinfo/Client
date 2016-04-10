/**该文件实现的基本功能等
function: 实现关卡视图的点击事件
author:ljx
date:2014-04-05
**/
using UnityEngine;
using MVC.entrance.gate;
using manager;

public class BtnClickRaid : MonoBehaviour
{
	const string CLOSE_RAID_INFO = "closeRaid"; //关闭地图信息
	const string CLICK_NORMAL = "normalBtn"; //进入副本
	const string CLICK_HARD = "hardBtn"; //打开扫荡按钮
	const string CLICK_PREV = "left";
	const string DRAG_CHAPTER = "chapter1";
	const string CLICK_NEXT = "right";
	 
	
	void OnClick ()
	{
		switch (gameObject.name) {
		case CLOSE_RAID_INFO:
			Gate.instance.sendNotification (MsgConstant.MSG_RAID_BTN_CLICK_CLOSE);
			break;
		case CLICK_NORMAL:
			Gate.instance.sendNotification (MsgConstant.MSG_RAID_BTN_CLICK_NORMAL);
			break;
		case CLICK_HARD:
			Gate.instance.sendNotification (MsgConstant.MSG_RAID_BTN_CLICK_HARD);
			break;
		case CLICK_PREV:
			Gate.instance.sendNotification (MsgConstant.MSG_RAID_BTN_SHOW_PREV);
			break;
		case CLICK_NEXT:
			Gate.instance.sendNotification (MsgConstant.MSG_RAID_BTN_SHOW_NEXT);
			break;
		default:  //就是点击了关卡
			Gate.instance.sendNotification (MsgConstant.MSG_RAID_BTN_CLICK_MAP, uint.Parse (gameObject.name));
			break;
		}
	}
}
