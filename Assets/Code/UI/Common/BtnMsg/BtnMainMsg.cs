
#define DEVIL_CAVE
#define MONSTERREWARD
//#define LADDER
#define SHOP_BTN
#define EMailButton
#define ROLE
#define PACKAGE
#define EQUIP
#define SMELTER
#define WING
#define SETTING
#define VIP
#define DIVINE
//#define GUILD
using UnityEngine;
using System.Collections;
using manager;
using helper;
using MVC.entrance.gate;

public class BtnMainMsg : MonoBehaviour
{
	private const string DUNGEON = "dungeon";//副本
	private const string DEVIL_CAVE = "devilCave";		//恶魔洞窟
	private const string GOLDEN_GOBLIN = "golden_goblin";//黄金哥布林
	private const string DIVINE = "divine";			   //竞技场
	private const string LADDER = "ladder";			   //排行榜
	private const string SHOP_BTN = "shop_btn";        //商店按钮
	private const string EMAIL_BUTTON = "EMailButton"; //邮件按钮
	private const string ROLE = "role"; //角色按钮
	private const string PACKAGE = "package";	//背包
	private const string TASK = "task"; 	//任务
	private const string EQUIP = "equip";//装备
	private const string SMELTER = "smelter";
	private const string WING = "wing";	//翅膀
	private const string SETTING = "system"; //系统设置
	private const string SKILL = "skill";	//技能
	private const string PLAYER = "player"; //按钮
	private const string EVENT = "event";	//活动
	private const string CHAT = "chat";
	private const string VIP = "vip";
	private const string FRIEND = "friend";
	private const string PET = "pet";
	private const string GUILD = "guild";
	private const string BOTTOM_MENU = "menu";
	private const string PANDORA = "panduola";
	private const string SHENMI_SHOP = "shenmi_shop";
	private const string JUQINGFB = "juqingFB";
	private const string MONSTERREWARD = "xuanshang";
	private const string LONGZHIBAOZANG = "longzhibaozang";
	private const string CHANNLE = "Channel_Function";
	private const string HIDE = "Hide_Function";
    private const string GUIDETRIGGER = "ButtonTrigger";
	void OnClick ()
	{
		switch (gameObject.name) {
		case FRIEND:
			FriendManager.Instance.OpenWindow ();
			break;
		case DUNGEON:
			DungeonManager.Instance.ShowDungeonView ();
			break;
		case DEVIL_CAVE:
#if DEVIL_CAVE
			DemonManager.Instance.RequestDemonInfo ();
#else
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
#endif
			break;
		case GOLDEN_GOBLIN:
			UIManager.Instance.openWindow (UiNameConst.ui_golden_goblin);
			MessageManager.Instance.SendAskGoldenGoblinTimes ();
			break;
			
		case DIVINE:
#if DIVINE
			ArenaManager.Instance.askArenaInfo ();
#else
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
#endif
			
			break;	
		
		case MONSTERREWARD:
#if MONSTERREWARD
			MonsterRewardManager.Instance.OpenWindow ();
#else	
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
#endif
			break;
		case LADDER:
#if LADDER
			
#else
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
#endif			
			
			break;
		case SHOP_BTN:
#if SHOP_BTN
			ShopManager.Instance.AskShopData ();
#else
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
#endif
			
			break;	
		case EMAIL_BUTTON:
#if EMailButton
            EmailManager.Instance.OpenWindow();
            //EmailManager.Instance.RequestEmailList();
            EmailManager.Instance.RefreshWindow();
#else	
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
			
#endif
			
			break;
		case ROLE:
#if ROLE
			RoleManager.Instance.OpenRole ();
#else			
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);		
#endif			
			
			break;
		case PACKAGE:
#if PACKAGE
			//transform.parent.GetComponent<funcMgr>().ResetTime();
			BagManager.Instance.OpenBag ();
#else
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);	
#endif			
			
			break;
		case TASK:
			UIManager.Instance.openWindow (UiNameConst.ui_task);
			break;
		case EQUIP:
#if EQUIP
			UIManager.Instance.openWindow (UiNameConst.ui_equip);
			UIManager.Instance.getUIFromMemory (UiNameConst.ui_equip).transform.FindChild ("Table/Table1").GetComponent<UICheckBoxColor> ().isChecked = true;
			Gate.instance.sendNotification (MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table1);
#else
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);	
#endif			
			break;
		case SMELTER:
#if SMELTER
            
			FurnaceManager.OpenWindow ();
            UIManager.Instance.getUIFromMemory(UiNameConst.ui_furnace).transform.FindChild("Table/Table1").GetComponent<UICheckBoxColor>().isChecked = true;
			Gate.instance.sendNotification (MsgConstant.MSG_FURNACE_SWING_TABLE, Table.Table1);
#else
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);	
#endif
			break;
		case WING:
			UIManager.Instance.openWindow (UiNameConst.ui_wing);
			WingManager.Instance.askWingMsg ();
			break;
		
		case SETTING:
#if SETTING
			SettingManager.Instance.OpenWindow ();    
#else
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);	
#endif
			break;
		case SKILL:
			UIManager.Instance.openWindow (UiNameConst.ui_skill);
			Gate.instance.sendNotification (MsgConstant.MSG_SKILL_TABLE_SWITCHING, 1);
			break;
		
		case PLAYER:
			this.transform.parent.parent.parent.parent.FindChild("bottom/func").GetComponent<funcMgr>().OnOpenFunc();//执行打开或者关闭的操作
			break;
		case BOTTOM_MENU:
			this.transform.parent.FindChild("func").GetComponent<funcMgr>().OnOpenFunc();//执行打开或者关闭的操作
			break;	
		case EVENT:
			EventManager.Instance.OpenWindow ();
			break;
		case CHAT:
			TalkManager.Instance.OpenWindow ();
			break;
		case VIP:
#if VIP
			VipManager.Instance.OpenWindow ();
#else
            ViewHelper.DisplayMessage("暂未开放，敬请期待");
#endif

			break;
		case PET:
			PetManager.Instance.OpenWindow ();
		
			break;
		case GUILD:
#if GUILD
			GuildManager.Instance.OpenWindow();
#else
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);	
#endif
            
			break;
		case PANDORA:
			PandoraManager.Instance.OpenWindow ();
			break;
		case SHENMI_SHOP:
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);	
			break;
		case JUQINGFB:
            PlotManager.Instance.OpenWindow();
			break;
		case LONGZHIBAOZANG:
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("wait_open"),
			                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);	
			break;
            case CHANNLE:
            if (!Global.InWorldBossMap())
            {
                ChannelManager.Instance.AskLines();
            }
            else {
                ViewHelper.DisplayMessageLanguage("channel_cantdothat");
            }
            break;
            case HIDE:
            SettingManager.Instance.SetPeopleOptionBtn();
            break;
            case GUIDETRIGGER:
            GuideInfoManager.Instance.ProcessTrigger();
            break;
		default:
			break;
		}
	}
	
	
	
}
