/**该文件实现的基本功能等
function: 实现关卡奖励的点击事件
author:zyl
date:2014-04-30
**/
using UnityEngine;
using MVC.entrance.gate;
using manager;

public class BtnChapterAward : MonoBehaviour {

	const string CLOSE = "close"; //关闭地图信息
	 
	const string AWARD = "award";
	
	const string BTN = "btn";		//领取奖励
	
	void OnClick ()
	{
		switch (gameObject.name) {
		case CLOSE:
			ChapterAwardManager.Instance.CloseChapterAwardUI();
			break;
		
		case AWARD:
			
			if (transform.Find("Label").gameObject.activeSelf) {
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("msg_have_take_award"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
			}else{
				ChapterAwardManager.Instance.ShowChapterAwardUI();
			}
			break;
			
		case BTN:
			Gate.instance.sendNotification (MsgConstant.MSG_CHAPTER_AWARD_ASK_AWARD);
			break;
								
		default:   
			break;
		}
	}
}
