/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-3-18
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnDungeonMsg : MonoBehaviour
{
	
	private const string OPEN_DUNGEON = "open_dungeon";
	private const string CLOSE_DUNGEON = "close_dungeon";
	private const string CLOSE_TEAM = "close_Team";
	private const string CREATE_TEAM = "create_team";
	private const string QUICK_JOIN = "quick_join";
	private const string JOIN_TEAM = "join_team";
	private const string START_DUNGEON = "start_battle";
	private const string LEAVE_TEAM = "leave_team";
	private const string NEXT_DUNGEON = "next_dungeon";
	private const string PREV_DUNGEON = "prev_dungeon";
	private const string BTN_REVIVE = "btnRevive";
	private const string BTN_RETCITY = "btnRet";
	
	
	void OnClick ()
	{
		switch (gameObject.name) {
		case OPEN_DUNGEON:
			Gate.instance.sendNotification (MsgConstant.MSG_OPEN_DUNGEON);
			break;
		case CLOSE_DUNGEON:
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_UI);
			break;
		case CREATE_TEAM:
			Gate.instance.sendNotification (MsgConstant.MSG_CREATE_TEAM);
			break;
		case QUICK_JOIN:
			Gate.instance.sendNotification (MsgConstant.MSG_QUICK_JOIN);
			break;
		case JOIN_TEAM:
			uint teamid = uint.Parse (transform.Find ("Teamid").GetComponent<UILabel> ().text);	
			Gate.instance.sendNotification (MsgConstant.MSG_JOIN_TEAM, teamid);
			break;
		case START_DUNGEON:
			Gate.instance.sendNotification (MsgConstant.MSG_START_DUNGEON);
			break;
		case LEAVE_TEAM:
			Gate.instance.sendNotification (MsgConstant.MSG_LEAVE_TEAM);
			break;
		case NEXT_DUNGEON:
			Gate.instance.sendNotification (MsgConstant.MSG_DUNGEON_NEXT);
			break;	
		case PREV_DUNGEON :
			Gate.instance.sendNotification (MsgConstant.MSG_DUNGEON_PREV);
			break;	
		case CLOSE_TEAM:
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_TEAM);
			break;
		case BTN_REVIVE:
			Gate.instance.sendNotification (MsgConstant.MSG_DUNGEON_BUY_REVIVE);
			break;	
		case BTN_RETCITY:
			DungeonManager.Instance.BackCity();
			break;
		default:
			break;
		}
	}
 
	
}
