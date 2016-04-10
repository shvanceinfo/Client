/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-6-4
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnMonsterRewardMsg : MonoBehaviour
{
	
	Transform _trans;
	private const string CLOSE = "close";
	private const string NEXT = "next";
	private const string PREV = "prev";
	private const string BTNCLEAR = "btnclear";
	private const string BTNGO = "btngo";
	private const string BTNQUICK = "btnquick";
	
	void Awake ()
	{
		this._trans = this.transform;
	}
	
	void OnClick ()
	{
		switch (this.gameObject.name) {
		case CLOSE:
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_MONSTER_REWARD_UI);
			break;	
			
		case NEXT:
			
			MonsterRewardManager.Instance.NextPage ();
			break;
		case PREV:
			MonsterRewardManager.Instance.PrevPage ();
			break;
		case BTNCLEAR:
			if (MonsterRewardManager.Instance.CurrentMonsterRewardVo.CurrentClearNum == MonsterRewardManager.Instance.CurrentMonsterRewardVo.MaxClearNum) {
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("monster_reward_no_zhuiji"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				return;
			}
			 
			Gate.instance.sendNotification (MsgConstant.MSG_MONSTER_REWARD_ASK_ZHUIJI, ZhuiJiType.OneClear);
			break;	
			
		case BTNGO:
			if (MonsterRewardManager.Instance.CurrentMonsterRewardVo.CurrentClearNum == MonsterRewardManager.Instance.CurrentMonsterRewardVo.MaxClearNum) {
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("monster_reward_no_zhuiji"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				return;
			}
			Gate.instance.sendNotification (MsgConstant.MSG_MONSTER_REWARD_ASK_ZHUIJI, ZhuiJiType.Normal);
			break;
			
		case BTNQUICK:
			if (MonsterRewardManager.Instance.CurrentMonsterRewardVo.CurrentClearNum == MonsterRewardManager.Instance.CurrentMonsterRewardVo.MaxClearNum) {
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("monster_reward_no_zhuiji"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				return;
			}
			Gate.instance.sendNotification (MsgConstant.MSG_MONSTER_REWARD_ASK_ZHUIJI, ZhuiJiType.Quick);
			break;
			
		default:
			break;
		}
		
	}
	
}
