using UnityEngine;
using System.Collections;
using model;

public class Task
{ 
	public uint taskID;				//任务ID
	public string taskName;			//任务名称
	public string description;		// 任务描述
	public int taskLine;		//任务线路   1-主线 2-支线
	public int startLevel;			//任务初始等级
	public int finishLevel;			//任务结束等级（支线任务使用，玩家等级超过该配置，自动放弃支线任务）
	public eTaskFinish finishType;	//任务完成类型  1-关卡通关任务  2-对话任务
	public BetterList<string> finishParams;		// 任务完成参数1,完成参数2,完成参数3....
	public uint acceptNPCID;		// 接取任务NPC的ID
	public uint submitNPCID;		// 提交任务NPC的ID
	public BetterList<string> startNPCDialog;	// 未领取状态NPC对话内容
	public BetterList<string> startBtn;			// 未领取状态对话按钮内容
	public BetterList<string> progressDialog;	// 未完成状态NPC对话内容
	public BetterList<string> progressBtn;		// 未完成状态对话按钮内容
	public BetterList<string> finishNPCDialog;	// 已完成状态NPC对话内容
	public BetterList<string> finishBtn;		// 已完成状态对话按钮内容
	public BetterList<uint> nextTasks; 	// 后续任务ID
	public uint taskOrder;      //任务顺序
	public int taskIndex;      //任务的顺序索引
	public eTaskJudgeType judgeType;	// 1-客户端  2-服务端
	public eTaskState taskState;		//任务的状态
	public uint prevTask;				//该任务的前置任务
	public bool notEnoughLevel;			//主线任务等级不足
	public int rewardNum;				//奖励的总共数目
	public int currentStep;  			//当前对话的步数
	public int finishGateNum;			//当前通关次数
	public BetterList<string> finish_Des;           //完成说明

	public BetterList<IdStruct> rewardRes;      //奖励金钱
	public BetterList<IdStruct> rewardItems;    //奖励物品

	public BetterList<IdStruct> curCondition;       //当前以完成条件
	public uint mapId;                           //关联地图
	private BetterList<IdStruct> complateCondition;  //完成条件

	public uint functionId;				//跟踪按钮用来判断的功能，如果是0则是打关卡，否则都是弹出页面

	public Task ()
	{
		finish_Des = new BetterList<string> ();
		curCondition = new BetterList<IdStruct> ();
		rewardRes = new BetterList<IdStruct> ();
		rewardItems = new BetterList<IdStruct> ();
		finishParams = new BetterList<string> ();
		nextTasks = new BetterList<uint> ();
		startNPCDialog = new BetterList<string> ();
		startBtn = new BetterList<string> ();
		progressDialog = new BetterList<string> ();
		progressBtn = new BetterList<string> ();
		finishNPCDialog = new BetterList<string> ();
		finishBtn = new BetterList<string> ();
		taskState = eTaskState.eInit;  //默认是不可见状态
		prevTask = 0;
		notEnoughLevel = false;
		rewardNum = 0;
		currentStep = 0;
		finishGateNum = 0;
	}
	
	//获取关联的NPC
	public NPC getRelateNPC ()
	{
		NPC npc = null;
		if (taskState == eTaskState.eCanAccept)
			npc = NPCManager.Instance.getNPCByID (acceptNPCID);
		else
			npc = NPCManager.Instance.getNPCByID (submitNPCID);
		return npc;
	}

	public BetterList<IdStruct> GetTaskCondition ()
	{
		if (complateCondition == null) {
			complateCondition = new BetterList<IdStruct> ();
			string[] sps;
			switch (finishType) {
			case eTaskFinish.finishGate:
				break;
			case eTaskFinish.npcDialog:
				break;
			case eTaskFinish.killEnemy:
				sps = finishParams [0].Split (',');
				int count = int.Parse (finishParams [1]);
				for (int i = 0; i < sps.Length; i++) {
					IdStruct id = new IdStruct (int.Parse (sps [i]), count);
					complateCondition.Add (id);
				}
				break;
			case eTaskFinish.getBindItem:
				sps = finishParams [0].Split (',');
				for (int i = 0; i < sps.Length; i+=2) {
					IdStruct id = new IdStruct (int.Parse (sps [i]), int.Parse (sps [i + 1]));
					complateCondition.Add (id);
				}

				break;
			case eTaskFinish.getItem:
				sps = finishParams [0].Split (',');
				for (int i = 0; i < sps.Length; i+=2) {
					IdStruct id = new IdStruct (int.Parse (sps [i]), int.Parse (sps [i + 1]));
					complateCondition.Add (id);
				}
				break;

			case eTaskFinish.levelUp: //人物等级提升

				break;
			case  eTaskFinish.vipUp:  //vip等级提升

				break;
			case 	 eTaskFinish.equipPlus:		//强化装备

				break;
			case 	 eTaskFinish.equipAdvance:	//进阶装备

				break;
			case 	 eTaskFinish.equipXiLian:	//洗练装备

				break;
			case	 eTaskFinish.wingUp:	//羽翼升级

				break;
			case 	 eTaskFinish.petUp:	//宠物升级

				break;
			case 	 eTaskFinish.medalUp:	//勋章升级

				break;
			case 	 eTaskFinish.addFriend:	//添加好友

				break;
			case	 eTaskFinish.joinArena: //参加竞技场
				break;
			case 	 eTaskFinish.eMoDongKu: //通关恶魔洞窟的层数
				break;
			case 	 eTaskFinish.goblin: //哥布林巢穴
				break;
			case	 eTaskFinish.rongLu:    //熔炉
				break;
			case	 eTaskFinish.chongZhiNum: //充值货币的数量
				break;
			case	 eTaskFinish.joinGuild:  //加入公会
				break;
			case	 eTaskFinish.jiNengLevel: //技能提升的级别
				break;
			case	 eTaskFinish.tianFuLevel: //天赋提升的级别
				break;
			case	 eTaskFinish.worldBoss:  //参与世界boss
				break;
			case	 eTaskFinish.shop:	//商城购买物品
				break;


			default:
				break;
			}
		}
		return complateCondition;
	}
}
