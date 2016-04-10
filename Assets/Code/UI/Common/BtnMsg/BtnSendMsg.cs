/**该文件实现的基本功能等
function: 实现按钮点击的消息传送（服务器发送过来的相关数据变动时后通知UI层发生变化）
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;
using NetGame;

public class BtnSendMsg : MonoBehaviour
{
	//翅膀相关按钮
	const string OPEN_WING = "wing";
	const string CLOSE_WING = "closeWing";
//	const string CULTURE = "btnCulture";
//	const string AUTO_CULTURE = "btnAutoCulture";
	const string EVOLUTION = "btnEvolution";
	const string AUTO_EVOLUTION = "btnAuto";
	//竞技场相关按钮	
	const string OPEN_ARENA = "btnArena";
	const string OPEN_HERO_LIST = "heroList";
	const string CLOSE_ARENA = "closeArena";
	const string CLOSE_HERO_LIST = "closeHeroShop";
	const string CLEAR_CHALLENGE_CD = "clearCD";
	const string BUY_CHALLENGE_NUM = "buyChallenge";
	const string CLICK_CHALLENGE = "challenger";
	const string SEND_ARENA_RESULT = "sendResultBtn";
	const string OPEN_BATTLE_LOG = "battleLogBtn";
	const string CLOSE_BATTLE_LOG = "closeBattleLog";
    const string RESULT_CLOSE = "resultClose";
    const string RESULT_OPNE = "resultBtn";
    const string RESULT_RECEIVE = "btn_award";
	//荣誉商店相关按钮

	const string CLOSE_HONOR_SHOP = "closeHonor";
	const string OPEN_EQUIP_TAB = "equip";
	const string OPEN_TOOL_TAB = "tools";
	const string OPEN_DIAMOND_TAB = "diamond";
	const string OPEN_OTHER_TAB = "other";
	const string HONOR_SHOP_BUY_ITEM = "buyItem";
	const string BTN_CLICK_UP_ARROW = "upArrow";
	const string BTN_CLICK_DOWN_ARROW = "downArrow";
    const string BTN_CLICK_RETURN = "btnRet";
    const string BTN_RESULT = "resultBtn";
	void OnClick ()
	{
		bool clickChallenge = false;
		switch (gameObject.name) {
            case RESULT_RECEIVE:
                Gate.instance.sendNotification(MsgConstant.MSG_ARENA_SEND_RECEIVE_AWARD);
                break;
            case RESULT_OPNE:
                Gate.instance.sendNotification(MsgConstant.MSG_ARENA_OPEN_AWARD, true);
                break;
            case RESULT_CLOSE:
                Gate.instance.sendNotification(MsgConstant.MSG_ARENA_OPEN_AWARD, false);
                break;
		case OPEN_WING:
			UIManager.Instance.openWindow(UiNameConst.ui_wing);
			WingManager.Instance.askWingMsg ();
			break;
		case CLOSE_WING:
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_UI);
            SkillTalentManager.Instance.ClearEffect();
			break;
//		case CULTURE:
//			Gate.instance.sendNotification (MsgConstant.MSG_WING_CULTURE);
//			break;
//		case AUTO_CULTURE:
//			Gate.instance.sendNotification (MsgConstant.MSG_WING_AUTO_CULTURE);
//			break;
		case EVOLUTION:
			Gate.instance.sendNotification (MsgConstant.MSG_WING_EVOLUTION);
			break;
		case AUTO_EVOLUTION:
			Gate.instance.sendNotification (MsgConstant.MSG_WING_AUTO_EVOLUTION);
			break;
		case OPEN_ARENA:
			ArenaManager.Instance.askArenaInfo ();
			break;
        //case OPEN_HONOR_SHOP:
        //    GCAskHonorShopInfo askShop = new GCAskHonorShopInfo ();
        //    NetBase.GetInstance ().Send (askShop.ToBytes (), true);
        //    break;
		case OPEN_HERO_LIST:
			GCAskHeroRankList askRank = new GCAskHeroRankList ();
			NetBase.GetInstance ().Send (askRank.ToBytes (), true);
			break;
		case CLOSE_ARENA:
			Gate.instance.sendNotification (MsgConstant.MSG_DESTROY_MODEL_CAMERA); //发送删除模型相机
			UIManager.Instance.closeAllUI();
			break;
		case CLOSE_HERO_LIST:
			Gate.instance.sendNotification (MsgConstant.MSG_ENABLE_MODEL_CAMERA);
			UIManager.Instance.closeWindow(UiNameConst.ui_hero_list);
			break;
		case OPEN_BATTLE_LOG:
			
			//这里应该请求得到
			ArenaManager.Instance.showChallengeResult();//这句应该放在服务器读数据的代码中
			break;
		case CLOSE_BATTLE_LOG:
			Gate.instance.sendNotification (MsgConstant.MSG_ENABLE_MODEL_CAMERA);
			UIManager.Instance.closeWindow(UiNameConst.ui_battle_log);
			break;
		case CLOSE_HONOR_SHOP:
			Gate.instance.sendNotification (MsgConstant.MSG_ENABLE_MODEL_CAMERA);
			UIManager.Instance.closeWindow(UiNameConst.ui_honor_shop);
			break;
		case CLEAR_CHALLENGE_CD:
			Gate.instance.sendNotification (MsgConstant.MSG_ARENA_CLEAR_CD);
			break;
		case BUY_CHALLENGE_NUM:
			Gate.instance.sendNotification (MsgConstant.MSG_ARENA_BUY_NUM);
			break;
		case OPEN_EQUIP_TAB:
			Gate.instance.sendNotification (MsgConstant.MSG_HONOR_EQUIP);
			break;
		case OPEN_TOOL_TAB:
			Gate.instance.sendNotification (MsgConstant.MSG_HONOR_TOOL);
			break;
		case OPEN_DIAMOND_TAB:
			Gate.instance.sendNotification (MsgConstant.MSG_HONOR_DIAMOND);
			break;
		case OPEN_OTHER_TAB:
			Gate.instance.sendNotification (MsgConstant.MSG_HONOR_OTHER);
			break;
		case HONOR_SHOP_BUY_ITEM:
			uint needHonor = uint.Parse (transform.parent.Find ("consume/consumeNum").GetComponent<UILabel> ().text);
			ArenaManager.Instance.buyItemID = uint.Parse (transform.parent.name);
			Gate.instance.sendNotification (MsgConstant.MSG_HONOR_BUY, needHonor);
			break;
		case SEND_ARENA_RESULT:
			ArenaManager.Instance.sendArenaResult ();
			break;
		case BTN_CLICK_UP_ARROW:
			Gate.instance.sendNotification (MsgConstant.MSG_WING_DRAG_PREV);
			break;
		case BTN_CLICK_DOWN_ARROW:
			Gate.instance.sendNotification (MsgConstant.MSG_WING_DRAG_NEXT);
			break;
        case BTN_CLICK_RETURN:
            Gate.instance.sendNotification(MsgConstant.MSG_WING_SHOW_RETURN);
            break;

		default:	
			clickChallenge = true;	//都不是点击所有的按钮			
			break;
		}
		if (clickChallenge) {
			if (gameObject.name.ToLower().IndexOf (CLICK_CHALLENGE) != -1) { //点击的是挑战按钮
				string name = transform.parent.Find ("name").GetComponent<UILabel> ().text;
				uint rank = uint.Parse (transform.parent.Find ("rank/num1").GetComponent<UILabel> ().text);
				Gate.instance.sendNotification (MsgConstant.MSG_ARENA_CHALLENGE, rank, name);
			}
		}
	}
}
