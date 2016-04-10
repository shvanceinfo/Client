using UnityEngine;
using System.Collections;
using NetGame;
using model;
using MVC.entrance.gate;
using manager;

//public enum eTaskType
//{
//    mainType = 1,	//主线
//    lateralType = 2  //支线
//}

public enum eTaskFinish
{
	finishGate = 1, //关卡通关
	npcDialog = 2,	//对话任务
	killEnemy =3,      //击杀怪物
	getBindItem =4,    //获取绑定物品
	getItem =5,        //获取普通物品
	levelUp = 6, //人物等级提升
	vipUp = 7,    //vip等级提升
	equipPlus = 8,		//强化装备
	equipAdvance = 9,	//进阶装备
	equipXiLian = 10,	//洗练装备
	wingUp = 11,	//羽翼升级
	petUp = 12,	//宠物升级
	medalUp = 13,	//勋章升级
	addFriend = 14,	//添加好友
	joinArena = 15, //参加竞技场
	eMoDongKu = 16, //通关恶魔洞窟的层数
	goblin = 17,	//哥布林巢穴
	rongLu = 18,    //熔炉
	chongZhiNum = 19, //充值货币的数量
	joinGuild = 20,  //加入公会
	jiNengLevel = 21, //技能提升的级别
	tianFuLevel = 22, //天赋提升的级别
	worldBoss = 23,  //参与世界boss
	shop  =  24,	//商城购买物品

}

public enum EquipPlus
{
	PlusSuccessCount =1, //强化的成功次数
	PlusCount =2,        //强化的次数
	PlusToLevel =3,		 //强化提升至等级
}

public enum WingUpState
{
	LvUpCount =1,  //升级次数
	GetNewWing = 2 //新羽翼
}

public enum PetUpState
{
	LvUpCount =1,  //升级次数
	GetNewPet = 2, //新宠物
}

public enum RongLuState
{
	SuccessCount =1, //成功次数
	UseCount = 2,  //使用合成的次数
	Item = 3		//合成指定物品
}

public enum JiNengState
{
	Level = 1,      //提升至XX级
	Count = 2		//提升次数
}

public enum TianFuState
{
	Level = 1,		//天赋提升至XX级
	Count =2 		//提升次数
}

public enum eTaskState
{
	eInit = 0, 	//不显示
	eCanAccept, //可接取
	eInProgress, //进行中
	eFinish,	//已完成
	eAward		//已领取
}

public enum eTaskJudgeType
{
	eClient = 1, //客户端
	eServer = 2	 //服务端
}

public class TaskManager
{
	public const int MAIN_TASK_LINE = 1;
	public const string PLAY_EFFECT_NAME = "jsrw_ui";
	public const string FINISH_EFFECT_NAME = "wcrw_ui";
	const string ACCEPT_EFFECT = "Effect/Effect_Prefab/UI/UI_jieshourenwu";
	const string FINISH_EFFECT = "Effect/Effect_Prefab/UI/UI_renwuwancheng";
	public bool isTaskFollow; //点击了任务自动寻路
	public Task followTask; //当前追踪的任务
	
	private static TaskManager _instance = null;
	private uint _firstID;
	private bool _firstRecv; //第一次接收服务器任务
	private Task _mainTask;  //当前主线任务
	private Task _currentTask; //当前UI中显示的任务

	private Hashtable _taskHash;  //全部的任务列表
	private Hashtable _acceptTasks; //可接任务，客户端判断
	private Hashtable _progressingTasks; //进行中（未完成任务)
	private Hashtable _completeTasks; //已完成任务，根据judgeType来判断
	private Hashtable _rewardTasks; //已奖励的任务
	private Hashtable _rejectLaterals; //永远被丢弃的支线任务
	private Hashtable _taskOrderHash; //多个线路的排序任务
	private BetterList<Vector3> _followPath; //任务自动追踪的路线 
	
	//network message 
	private GCAcceptTask _reportAccept;
	private GCSubmitTask _reportSubmit;
	//private GCReportTaskProgress _reportProgress;
	
	//effect object
	private GameObject _finishEffect;
	private GameObject _acceptEffect;
	private GameObject _effectMask;
	
	public static TaskManager Instance {
		get { 
			if (_instance == null)
				_instance = new TaskManager ();
			return _instance;
		}
	}
	
	private TaskManager ()
	{
		_firstRecv = true;
		isTaskFollow = false;
		_taskHash = new Hashtable ();
		_acceptTasks = new Hashtable ();
		_progressingTasks = new Hashtable ();
		_completeTasks = new Hashtable ();
		_rewardTasks = new Hashtable ();
		_rejectLaterals = new Hashtable ();
		_reportAccept = new GCAcceptTask ();
		_reportSubmit = new GCSubmitTask ();
		_followPath = new BetterList<Vector3> ();
		_taskOrderHash = new Hashtable ();
		//_reportProgress = new GCReportTaskProgress();
		_acceptEffect = null;
		_finishEffect = null;
	}
	
	//处理服务器收到的后续任务
	public void processTask (uint len, uint[] taskIDs=null, int[] taskStates=null, BetterList<uint[]> conditions=null, BetterList<int[]> conditionNums=null, uint[] scenarioIDs=null)
	{
		Task task = null;
		if (_firstRecv) { //重新登录清除身上所有的任务
			_acceptTasks.Clear (); 
			_progressingTasks.Clear ();
			_completeTasks.Clear ();
			_rewardTasks.Clear ();
			_rejectLaterals.Clear ();
		}
		if (len == 0) { //处理第一个未接的任务
			task = _taskHash [_firstID] as Task;
			changeTaskState (task, eTaskState.eCanAccept); //设为可接的任务

		} else {
			//Hashtable taskHash = new Hashtable();
			for (int i=0; i<len; i++) {
				task = _taskHash [taskIDs [i]] as Task;
				ScenarioManager.Instance.setHadPlaying (taskIDs [i], scenarioIDs [i]);
				if (task.taskLine != MAIN_TASK_LINE) { //非主线任务剔除超过等级得
					if (task.finishLevel < CharacterPlayer.character_property.getLevel ()) {
						_rejectLaterals [task.taskLine] = true;
						continue;
					}
				}else{
					if (_firstRecv) {//只第一次登陆的时候加载
						this.LoadAllPrevTasksBranchTask(task);
					}
				}
//				if(param2s != null && param2s.Length >i)
//					task.finishGateNum = param2s[i];
				task.finishGateNum = 1;
				eTaskState state = (eTaskState)(taskStates [i] + 2); //跟服务器enum差2
				changeTaskState (task, state);


				#region 显示的状态信息
				UpdateConditionData (task,conditions, conditionNums,  i);
				#endregion
			}

		}
		if (_firstRecv)
			_firstRecv = false;
	}

	/// <summary>
	/// Loads all previous tasks branch task.
	/// </summary>
	/// <param name="task">Task.</param>
	void LoadAllPrevTasksBranchTask (Task task){
		BetterList<uint> taskOrders = _taskOrderHash [task.taskLine] as BetterList<uint>;
		if (taskOrders != null) {
			int i =0;
			if (task.taskState == eTaskState.eAward) {   //如果是已经领取奖励的状态，则需要从自己开始算支线任务
				i=  task.taskIndex;
			}else{										//否则从上个主线任务开始算
				i=  task.taskIndex -1;
			}
			 
			for (; i >=0; i--) {
				uint taskid = taskOrders.buffer[i];
				Task prevTask = _taskHash [taskid] as Task; //得到所有上级的主任务
				if (prevTask.nextTasks.size > 0) {
					foreach (uint nextID in prevTask.nextTasks) {
						Task nextTask = _taskHash [nextID] as Task;
						//							Task prevTask = getNeighborTask (task, false);
						//							if (prevTask != null)
						//								nextTask.prevTask = prevTask.taskID;
						changeTaskState (nextTask, eTaskState.eCanAccept); //设为可接
					}
				}
			}
		}
	}


	/// <summary>
	/// Updates the condition data.
	/// </summary>
	/// <param name="task">Task.</param>
	/// <param name="conditions">Conditions.</param>
	/// <param name="conditionNums">Condition nums.</param>
	/// <param name="i">The index.</param>
	void UpdateConditionData (Task task,BetterList<uint[]> conditions, BetterList<int[]> conditionNums,  int i)
	{
		switch (task.finishType) {
		case eTaskFinish.equipPlus: {
			if (((EquipPlus)int.Parse (task.finishParams [0])) == EquipPlus.PlusToLevel) {
				//强化装备的强化至等级需要特殊处理
				ConditionAddDataIfBetterValue (task, conditions, conditionNums, i);
			}
			else {
				CurConditionAddData (conditions, conditionNums, i, task);
			}
			break;
		}
		case eTaskFinish.jiNengLevel: {
			if (((JiNengState)int.Parse (task.finishParams [0])) == JiNengState.Level) {
				//强化技能的强化至等级需要特殊处理
				ConditionAddDataIfBetterValue (task, conditions, conditionNums, i);
			}
			else {
				CurConditionAddData (conditions, conditionNums, i, task);
			}
			break;
		}
		case eTaskFinish.tianFuLevel: {
			if (((TianFuState)int.Parse (task.finishParams [0])) == TianFuState.Level) {
				//强化天赋的强化至等级需要特殊处理
				ConditionAddDataIfBetterValue (task, conditions, conditionNums, i);
			}
			else {
				CurConditionAddData (conditions, conditionNums, i, task);
			}
			break;
		}
		default: {
			CurConditionAddData (conditions, conditionNums, i, task);
			break;
		}
		}
	}



	/// <summary>
	/// 为任务信息添加数据,如果有更大的值，则代替原始值
	/// </summary>
	/// <param name="task">Task.</param>
	/// <param name="conditions">Conditions.</param>
	/// <param name="conditionNums">Condition nums.</param>
	/// <param name="i">The index.</param>
	void  ConditionAddDataIfBetterValue (Task task,BetterList<uint[]> conditions, BetterList<int[]> conditionNums, int i)
	{
		if (task.curCondition.size > 0) {
			//如果有数据，需要和新来的数据进行判断
			IdStruct con = task.curCondition [0];
			//当前的最大强化值
			uint[] ids = conditions [i];
			int[] nums = conditionNums [i];
			for (int j = 0; j < ids.Length; j++) {
				if (con.Value < nums [j]) {
					//如果小于新的数据则替换
					con.Value = nums [j];
					break;
				}
			}
		}
		else {
			CurConditionAddData (conditions, conditionNums, i, task);
		}
	}

	/// <summary>
	/// 为任务信息添加数据
	/// </summary>
	/// <param name="conditions">Conditions.</param>
	/// <param name="conditionNums">Condition nums.</param>
	/// <param name="i">The index.</param>
	/// <param name="task">Task.</param>
	void CurConditionAddData (BetterList<uint[]> conditions, BetterList<int[]> conditionNums, int i, Task task)
	{
		uint[] ids = conditions [i];
		int[] nums = conditionNums [i];
		task.curCondition = new BetterList<IdStruct> ();
		for (int j = 0; j < ids.Length; j++) {
			task.curCondition.Add (new IdStruct ((int)ids [j], nums [j]));
		}
	}
	
	//改变任务的状态
	public void changeTaskState (Task task, eTaskState state)
	{
		task.taskState = state;
		if (task.taskLine == MAIN_TASK_LINE) {
			_mainTask = task;
			setNPCInfo ();
		}
		switch (state) {
		case eTaskState.eCanAccept:
			if (_rejectLaterals.Count == 0 || !_rejectLaterals.Contains (task.taskLine)) {   //判断是否需要丢弃任务
				if (CharacterPlayer.character_property.getLevel () >= task.startLevel) {
					task.notEnoughLevel = false;
					if (!_acceptTasks.Contains (task.taskID))
						_acceptTasks.Add (task.taskID, task);				
					if (task.prevTask > 0 && _rewardTasks.Contains (task.prevTask))
						_rewardTasks.Remove (task.prevTask);
				} else {
					task.notEnoughLevel = true;
					if (!_acceptTasks.Contains (task.taskID))
						_acceptTasks.Add (task.taskID, task);
					if (task.prevTask > 0 && _rewardTasks.Contains (task.prevTask))
						_rewardTasks.Remove (task.prevTask);
				}
			}
			break;
		case eTaskState.eInProgress:
			if (_acceptTasks.Contains (task.taskID))
				_acceptTasks.Remove (task.taskID);
			if (!_progressingTasks.Contains (task.taskID))
				_progressingTasks.Add (task.taskID, task);
			break;
		case eTaskState.eFinish:
			if (_acceptTasks.Contains (task.taskID)) //dialog task
				_acceptTasks.Remove (task.taskID);
			if (_progressingTasks.Contains (task.taskID))
				_progressingTasks.Remove (task.taskID);
			if (!_completeTasks.Contains (task.taskID))
				_completeTasks.Add (task.taskID, task);
			break;
		case eTaskState.eAward:
			if (_completeTasks.Contains (task.taskID))
				_completeTasks.Remove (task.taskID);
			if (!_rewardTasks.Contains (task.taskID))
				_rewardTasks.Add (task.taskID, task);
			if (task.nextTasks.size > 0) {
				foreach (uint nextID in task.nextTasks) {
					Task nextTask = _taskHash [nextID] as Task;
					Task prevTask = getNeighborTask (task, false);
					if (prevTask != null)
						nextTask.prevTask = prevTask.taskID;
					changeTaskState (nextTask, eTaskState.eCanAccept); //设为可接
				}
			}
			Task nextTask1 = getNeighborTask (task, true); //不管是否有下个任务都要把下个任务推进列表
			if (nextTask1 != null) {
				changeTaskState (nextTask1, eTaskState.eCanAccept); //设为可接
				nextTask1.prevTask = task.taskID;
			}
			break;
		default:
			break;
		}
		if (state != eTaskState.eAward) { //调整完任务状态后再改变
			NPCManager.Instance.markNPCTag (NPCManager.Instance.getNPCByID (task.acceptNPCID));
			NPCManager.Instance.markNPCTag (NPCManager.Instance.getNPCByID (task.submitNPCID));
		}

        switch (state)
        {
            case eTaskState.eInit:
                break;
            case eTaskState.eCanAccept:
                Gate.instance.sendNotification(MsgConstant.MSG_GUIDE_SEND_TRIGGER,
                    new Trigger(TriggerType.QuestionCanReceive, (int)task.taskID));
                break;
            case eTaskState.eInProgress:
                Gate.instance.sendNotification(MsgConstant.MSG_GUIDE_SEND_TRIGGER,
                    new Trigger(TriggerType.QuestInProgress, (int)task.taskID));
                break;
            case eTaskState.eFinish:
                Gate.instance.sendNotification(MsgConstant.MSG_GUIDE_SEND_TRIGGER,
                    new Trigger(TriggerType.QuestionComplate, (int)task.taskID));
                GuideInfoManager.Instance.AddGuideTriggerTask((int)task.taskID);
                break;
            case eTaskState.eAward:
                break;
            default:
                break;
        }
	}
	
	//判断任务状态是否完成，根据任务类型判断
	public void checkTaskComplete (eTaskFinish type, params object[] checkParams)
	{
		if (_progressingTasks.Count > 0) { //存在进行中的任务
			foreach (Task task in _progressingTasks.Values) {
				bool finish = true;
				if (task.finishType == type) {
					if (task.finishParams.size > 0 && checkParams.Length > 0) {
						int len = task.finishParams.size < checkParams.Length ? task.finishParams.size : checkParams.Length;
						for (int i=0; i<len; i++) {
							if (task.finishParams [i] != checkParams [i].ToString ()) {
								finish = false;
								break;  //有一个条件没完成就是没完成
							}
						}
					}
				} else if (finish) {
					if (type == eTaskFinish.finishGate && task.taskState == eTaskState.eInProgress)
						task.finishGateNum++;
					//changeTaskState(task, eTaskState.eFinish);				
					//NetBase.GetInstance().Send(_reportProgress.ToBytes(task.taskID, checkParams), false);
					//break;
				}
			}
		}
	}
	
	/// <summary>
	/// 获取挂在NPC身上的NPC
	/// </summary>
	/// <param name="npcID">npcID不为零就是获取NPC身上的任务，为零获取当前的任务列表</param>
	/// <returns></returns>
	public BetterList<Task> getTaskList (uint npcID = 0)
	{
		//排序主线，支线已完成，进行中，可接取
		BetterList<Task> tasks = new BetterList<Task> ();
		if (npcID == 0 || showInNPC (_mainTask, npcID))
			tasks.Add (_mainTask);
		if (_completeTasks.Count > 0) {
			foreach (Task task in _completeTasks.Values) {
				if (task.taskLine != MAIN_TASK_LINE) {
					if (npcID == 0)
						tasks .Add (task);
					else if (showInNPC (task, npcID))
						tasks.Add (task);
				}				
			}
		}
		if (_progressingTasks.Count > 0) {
			foreach (Task task in _progressingTasks.Values) {
				if (task.taskLine != MAIN_TASK_LINE) {
					if (npcID == 0)
						tasks .Add (task);
					else if (showInNPC (task, npcID))
						tasks.Add (task);
				}		
			}
		}
		if (_acceptTasks.Count > 0) {
			foreach (Task task in _acceptTasks.Values) {
				if (task.taskLine != MAIN_TASK_LINE) {
					if (npcID == 0)
						tasks .Add (task);
					else if (showInNPC (task, npcID))
						tasks.Add (task);
				}		
			}
		}
		return tasks;
	}

	/// <summary>
	/// 获取完成和正在进行中的任务
	/// </summary>
	/// <returns></returns>
	public BetterList<Task> getTaskCompleteAndPro ()
	{
		BetterList<Task> tasks = new BetterList<Task> ();

		if (_mainTask.taskState != eTaskState.eInit &&
			_mainTask.taskState != eTaskState.eCanAccept
            ) {
			tasks.Add (_mainTask);
		}

		if (_completeTasks.Count > 0) {
			foreach (Task task in _completeTasks.Values) {
				if (task.taskLine != MAIN_TASK_LINE) {
					tasks.Add (task);
				}
			}
		}
		if (_progressingTasks.Count > 0) {
			foreach (Task task in _progressingTasks.Values) {
				if (task.taskLine != MAIN_TASK_LINE) {
					tasks.Add (task);
				}
			}
		}
		return tasks;
	}

	/// <summary>
	/// 获取所有可以接的任务
	/// </summary>
	/// <returns></returns>
	public BetterList<Task> getTaslAcceptList ()
	{
		BetterList<Task> tasks = new BetterList<Task> ();
		if (_acceptTasks.Count > 0) {
			foreach (Task task in _acceptTasks.Values) {
				tasks.Add (task);
			}
		}
		return tasks;
	}

	//判断任务对话是否结束
	public bool isTaskDialogNotOver (Task task = null)
	{
		if (task != null) {
			_currentTask = task;
			_currentTask.currentStep = 0;
			if (_currentTask.notEnoughLevel)
				return false;
		}
		int len = 0;
		switch (_currentTask.taskState) {
		case eTaskState.eCanAccept:
			len = _currentTask.startNPCDialog.size;
			break;
		case eTaskState.eInProgress:
			len = _currentTask.progressDialog.size;
			break;
		case eTaskState.eFinish:
			len = _currentTask.finishNPCDialog.size;
			break;
		default:
			break;
		}
		if (_currentTask.currentStep >= len) {
			UIManager.Instance.closeAllUI ();
			NPCManager.Instance.openNPC.changeNPCLayer (false);  //重置NPC到场景中
			ScenarioManager.Instance.showScenario (_currentTask, submitTask);
			//submitTask(); //向服务器提交任务
			return false;
		}
		return true;
	}
	
	//获取对应的任务对白
	public string[] getTaskDialog ()
	{
		string[] dialogs = new string[2];	
		switch (_currentTask.taskState) {
		case eTaskState.eCanAccept:
			dialogs [0] = _currentTask.startNPCDialog [_currentTask.currentStep];
			dialogs [1] = _currentTask.startBtn [_currentTask.currentStep];
			break;
		case eTaskState.eInProgress:
			dialogs [0] = _currentTask.progressDialog [_currentTask.currentStep];
			dialogs [1] = _currentTask.progressBtn [_currentTask.currentStep];
			break;
		case eTaskState.eFinish:
			dialogs [0] = _currentTask.finishNPCDialog [_currentTask.currentStep];
			dialogs [1] = _currentTask.finishBtn [_currentTask.currentStep];
			break;
		default:
			break;
		}
		_currentTask.currentStep++;
		return dialogs;
	}
	
	//设置任务的标题，是否在任务列表还是任务对话中显示
	public string setTaskTitle (Task task, bool showInList)
	{
		string color = Constant.COLOR_GREEN;
		string prefix = "", suffix = Constant.COLOR_END;
		switch (task.taskState) {
		case eTaskState.eCanAccept:
			if (task.notEnoughLevel) {
				prefix = LanguageManager.GetText ("cant_accept_title_color") + task.taskName + Constant.COLOR_END;
				suffix = LanguageManager.GetText ("cant_accept_color") + Constant.LEFT_PARENTHESIS +
					LanguageManager.GetText ("task_state_cant_accept") + Constant.RIGHT_PARENTHESIS + Constant.COLOR_END;
			} else {
				prefix = LanguageManager.GetText ("accept_title_color") + task.taskName + Constant.COLOR_END;
				suffix = LanguageManager.GetText ("accept_color") + Constant.LEFT_PARENTHESIS +
					LanguageManager.GetText ("task_state_accept") + Constant.RIGHT_PARENTHESIS + Constant.COLOR_END;
			}
			break;
		case eTaskState.eInProgress:
			prefix = LanguageManager.GetText ("progressing_title_color") + task.taskName + Constant.COLOR_END;
			suffix = LanguageManager.GetText ("progressing_color") + Constant.LEFT_PARENTHESIS +
				LanguageManager.GetText ("task_state_progressing") + Constant.RIGHT_PARENTHESIS + Constant.COLOR_END;
			break;
		case eTaskState.eFinish:
			prefix = LanguageManager.GetText ("award_title_color") + task.taskName + Constant.COLOR_END;
			suffix = LanguageManager.GetText ("award_color") + Constant.LEFT_PARENTHESIS +
				LanguageManager.GetText ("task_state_award") + Constant.RIGHT_PARENTHESIS + Constant.COLOR_END;
			break;
		default:
			break;
		}
		if (showInList)
			return prefix;
		else
			return prefix + "  " + suffix;
	}
	
	//对话点击完毕设置各个NPC的状态，只在主线任务NPC上打标记
//	public void markNPCTag(Task mainTask = null)
//	{
//		if(SceneManager.Instance.currentScene == SceneManager.SCENE_POS.IN_CITY) //只有主城的NPC才打tag
//		{
//			if(mainTask == null)
//				mainTask = getMainTask();
//			if(NPCManager.Instance.hasInstantiate) //实例化过的才能mark
//			{
//				NPC currentNPC = null;
//				NPC prevNPC = null;
//				switch (mainTask.taskState)
//				{
//					case eTaskState.eCanAccept:
//						if(mainTask.prevTask > 0) //有前置任务，删除前置任务NPC的标记
//						{
//							Task prev = _taskHash[mainTask.prevTask] as Task;
//							if(prev != null && mainTask.acceptNPCID == prev.submitNPCID) 
//								currentNPC = NPCManager.Instance.getNPCByID(mainTask.acceptNPCID);		
//							else
//							{
//								currentNPC = NPCManager.Instance.getNPCByID(mainTask.acceptNPCID);
//								Task prevTask = _taskHash[mainTask.prevTask] as Task;
//								prevNPC = NPCManager.Instance.getNPCByID(prevTask.submitNPCID);
//							}
//						}
//						else
//							currentNPC = NPCManager.Instance.getNPCByID(mainTask.acceptNPCID);	
//						if(mainTask.notEnoughLevel)
//							currentNPC.tagTaskState(NPCManager.SIMBOL_ACCENT_FAIL); 
//						else
//							currentNPC.tagTaskState(NPCManager.SIMBOL_ACCENT);
//						break;
//					case eTaskState.eInProgress:
//						currentNPC = NPCManager.Instance.getNPCByID(mainTask.submitNPCID);
//						if(mainTask.acceptNPCID != mainTask.submitNPCID)	//只有NPC不一样才需要清除上一个NPC
//							prevNPC = NPCManager.Instance.getNPCByID(mainTask.acceptNPCID);
//						currentNPC.tagTaskState(NPCManager.SIMBOL_PROCESSING); 
//						break;
//					case eTaskState.eFinish:  //完成NPC跟进行在同一个NPC上标记
//						currentNPC = NPCManager.Instance.getNPCByID(mainTask.submitNPCID);
//						if(mainTask.acceptNPCID != mainTask.submitNPCID)	//只有NPC不一样才需要清除上一个NPC
//							prevNPC = NPCManager.Instance.getNPCByID(mainTask.acceptNPCID);
//						currentNPC.tagTaskState(NPCManager.SIMBOL_COMPLETE);
//						break;
//					default:
//						break;
//				}
//				if(currentNPC != null)
//				{
//					NPCManager.Instance.mainTaskNPC = currentNPC;
//					setNPCInfo();
//				}
//				if(prevNPC != null)
//					prevNPC.tagTaskState(null);
//			}
//		}
//	}
	
	//删除支线任务
	public void deleteLateralTasks (int level)
	{
		int len = _acceptTasks.Count;
		if (len > 0) {
			BetterList<uint> deleteKeys = new BetterList<uint> ();
			foreach (Task task in _acceptTasks.Values) {
				if (task.taskLine != MAIN_TASK_LINE && task.finishLevel < level) //非主线任务删除
					deleteKeys.Add (task.taskID);
				if (task.notEnoughLevel) {
					if (task.startLevel <= level)
						task.notEnoughLevel = false;
				}
			}
			if (deleteKeys.size > 0) {
				foreach (uint key in deleteKeys) 
					_acceptTasks.Remove (key);
			}
		}
	}
	
	//获取任务对应的标记
	public string getTagMark (Task task)
	{
		string tag = null;
		switch (task.taskState) {
		case eTaskState.eCanAccept:
			if (task.notEnoughLevel)
				tag = NPCManager.SIMBOL_ACCENT_FAIL;
			else
				tag = NPCManager.SIMBOL_ACCENT;
			break;
		case eTaskState.eInProgress:
			tag = NPCManager.SIMBOL_PROCESSING; 
			break;
		case eTaskState.eFinish:  //完成NPC跟进行在同一个NPC上标记
			tag = NPCManager.SIMBOL_COMPLETE;
			break;
		default:
			break;
		}	
		return tag;		
	}

	//根据任务支线添加排序任务
	public void addTaskByLine (Task task)
	{
		BetterList<uint> taskOrders = null;
		if (_taskOrderHash.ContainsKey (task.taskLine)) {
			taskOrders = _taskOrderHash [task.taskLine] as BetterList<uint>;
			taskOrders.Add (task.taskID);
		} else {
			taskOrders = new BetterList<uint> ();
			taskOrders.Add (task.taskID);
			_taskOrderHash.Add (task.taskLine, taskOrders);
		}
	}

	//排序任务列表，在读取任务列表时只排序一次
	public void sortTaskOrder ()
	{
		foreach (BetterList<uint> taskOrders in _taskOrderHash.Values) {
			int minIndex = 0;
			for (int i=0; i<taskOrders.size; i++) {
				Task task1 = _taskHash [taskOrders [i]] as Task;
				minIndex = i;
				for (int j = i; j < taskOrders.size; j++) {
					Task task2 = _taskHash [taskOrders [j]] as Task;
					if (task2.taskIndex < task1.taskIndex)
						minIndex = j;
				}
				if (minIndex != i) { //如果最小的索引不在当前的索引，必须交换两个位置
					taskOrders [i] = taskOrders [i] + taskOrders [minIndex];
					taskOrders [minIndex] = taskOrders [i] - taskOrders [minIndex];
					taskOrders [i] = taskOrders [i] - taskOrders [minIndex];
				}
				task1 = _taskHash [taskOrders [i]] as Task;
				task1.taskIndex = i;
			}
		}
	}

	//获取对应支线的相邻任务
	private Task getNeighborTask (Task task, bool getNext)
	{
		BetterList<uint> taskOrders = _taskOrderHash [task.taskLine] as BetterList<uint>;
		if (taskOrders != null) {
			if (getNext && task.taskIndex + 1 < taskOrders.size) {
				uint taskID = taskOrders [task.taskIndex + 1];
				return _taskHash [taskID] as Task;
			} else if (!getNext && task.taskIndex - 1 > 0) {
				uint taskID = taskOrders [task.taskIndex - 1];
				return _taskHash [taskID] as Task; 
			}
		}
		return null;
	}
	
	//该NPC是否显示该任务
	private bool showInNPC (Task task, uint npcID)
	{
		if (npcID == 0)
			return true;
	    if (task == null)
	        return false;
		if (task.taskState == eTaskState.eCanAccept) { //任务可接状态，显示
			if (npcID == task.acceptNPCID)//是接取NPC			
				return true;
			else
				return false;
		} else if (task.taskState == eTaskState.eInProgress || task.taskState == eTaskState.eFinish) { //显示进行中，已完成任务
			if (npcID == task.submitNPCID)
				return true;
			else
				return false;
		} else
			return false;
	}
	
	//获取下一个任务状态
	private void submitTask ()
	{
		if (_currentTask != null) {
			if (_currentTask.taskState == eTaskState.eCanAccept) {
				if (_currentTask.finishType == eTaskFinish.npcDialog) { //如果是NPC对话, 直接置为完成
					changeTaskState (_currentTask, eTaskState.eFinish); //对话任务直接完成
					playEffect (false);
					NetBase.GetInstance ().Send (_reportAccept.ToBytes (_currentTask.taskID), false);
				} else if (CharacterPlayer.character_property.getLevel () >= _currentTask.startLevel) {
					changeTaskState (_currentTask, eTaskState.eInProgress); //可以在网络返回前表现
					playEffect (false);
					NetBase.GetInstance ().Send (_reportAccept.ToBytes (_currentTask.taskID), false);
				} else
					FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("no_enough_level"), UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
			} else if (_currentTask.taskState == eTaskState.eFinish) {
				NetBase.GetInstance ().Send (_reportSubmit.ToBytes (_currentTask.taskID), false);
				playEffect (true);
			}
		}
	}
	
	//设置可以接取的任务
	private void setCanAcceptHash (BetterList<uint> laterTasks)
	{
		if (laterTasks.size > 0) {
			foreach (uint lateralID in laterTasks) {
				Task lateralTask = _taskHash [lateralID] as Task;
				Task nextTask = getNeighborTask (lateralTask, true);

			}
		}
	}
	//private void setCanAcceptHash(Hashtable existHash)
	//{
	//    uint taskID = _mainTask.taskID;
	//    foreach(Task task in _taskHash.Values)
	//    {
	//        if(task.taskID < taskID && task.taskLine == MAIN_TASK_LINE) //遍历主线任务
	//        {
	//            foreach (uint subTaskID in task.nextTasks) 
	//            {
	//                Task subTask = _taskHash[subTaskID] as Task;
	//                if (subTask.taskLine != MAIN_TASK_LINE)
	//                {
	//                    if (!existHash.Contains(subTask.taskLine)) //如果在主线中
	//                    {
	//                        if(subTask.taskState == eTaskState.eInit)
	//                        {
	//                            changeTaskState(subTask, eTaskState.eCanAccept); 
	//                        }
	//                        existHash.Add(subTask.taskLine, true);
	//                    }
	//                }
	//            }
	//        }
	//    }
	//}
	
	//设置UI右上角的信息
	private void setNPCInfo ()
	{
		GameObject obj = UIManager.Instance.getUIFromMemory (UiNameConst.ui_main);
		if (obj != null) {
			TaskFollow follow = obj.transform.Find ("top_right/taskFollowing").GetComponent<TaskFollow> ();
			follow.setNPCIcon (getTagMark (_mainTask));
		}
	}
	
	////获取关卡的支线ID
	//private string getTaskLateral(uint taskID)
	//{
	//    string idStr = taskID.ToString().Substring(1, 4);
	//    return idStr;
	//}
	
	//播放任务特效
	private void playEffect (bool isFinish)
	{

		if (isFinish) {
			if (_finishEffect == null) {
				if (BundleMemManager.Instance.isTypeInCache (EBundleType.eBundleUIEffect)) {
					BundleMemManager.Instance.loadPrefabViaWWW<GameObject> (EBundleType.eBundleUIEffect, FINISH_EFFECT,
                  	(asset) => 
					{
						_finishEffect = BundleMemManager.Instance.instantiateObj (asset, Vector3.zero, Quaternion.identity);
						setEffect (_finishEffect, FINISH_EFFECT_NAME);
					});
				} else {
					GameObject asset = BundleMemManager.Instance.getPrefabByName (FINISH_EFFECT, EBundleType.eBundleUIEffect);
					_finishEffect = BundleMemManager.Instance.instantiateObj (asset, Vector3.zero, Quaternion.identity);
					setEffect (_finishEffect, FINISH_EFFECT_NAME);
				}                
			} else {
				setEffect (_finishEffect, FINISH_EFFECT_NAME);
			}
		} else {
			if (_acceptEffect == null) {
				if (BundleMemManager.Instance.isTypeInCache (EBundleType.eBundleUIEffect)) {
					BundleMemManager.Instance.loadPrefabViaWWW<GameObject> (EBundleType.eBundleUIEffect, ACCEPT_EFFECT,
              		(asset) =>
					{
						_acceptEffect = BundleMemManager.Instance.instantiateObj (asset, Vector3.zero, Quaternion.identity);
						setEffect (_acceptEffect, PLAY_EFFECT_NAME);
					});
				} else {
					GameObject asset = BundleMemManager.Instance.getPrefabByName (ACCEPT_EFFECT, EBundleType.eBundleUIEffect);
					_acceptEffect = BundleMemManager.Instance.instantiateObj (asset, Vector3.zero, Quaternion.identity);
					setEffect (_acceptEffect, PLAY_EFFECT_NAME);
				}  
			} else {
				setEffect (_acceptEffect, PLAY_EFFECT_NAME);
			}

		}        
	}

	private void setEffect (GameObject effect, string effectName)
	{
		Transform selfParent = UIManager.Instance.getUIFromMemory (UiNameConst.ui_main).transform.parent;
		effect.name = effectName;
		effect.transform.parent = selfParent;
		effect.transform.localPosition = new Vector3 (0f, 0f, -10f);
		effect.transform.localEulerAngles = new Vector3 (0f, -180f, 0f);
		effect.transform.localScale = new Vector3 (200f, 200f, 1f);
		//_effectMask.SetActive(true);
		//_effectMask.GetComponent<BoxCollider>().enabled = true;
	}
	
	//清除任务特效
	public void desTroyEffect ()
	{
		if (_acceptEffect != null)
			Object.Destroy (_acceptEffect);
		else if (_finishEffect != null)
			Object.Destroy (_finishEffect);
		if (_effectMask != null) {
			_effectMask.SetActive (false);
			_effectMask.GetComponent<BoxCollider> ().enabled = false;
		}		
	}


	/// <summary>
	/// Opens the window by follow task fun identifier.
	/// </summary>
	public void OpenWindowByFollowTaskFunId(){
		FastOpenManager.Instance.OpenWindow ((int)this.followTask.functionId);
	}



	/// <summary>
	/// Clear 任务的当前状态
	/// </summary>
	public void Clear ()
	{
		foreach (var item in this._taskHash.Values) {
			Task task = item as Task;
			task.curCondition = new BetterList<IdStruct> ();
		}
	}

	//getter and setter
	public Hashtable TaskHash {
		get { return _taskHash; }
	}
	
	public uint FirstID {
		set { _firstID = value; }
	}
	
	public Task CurrentTask {
		get { return _currentTask; }
	}
	
	public Task MainTask {
		get { return _mainTask; }
	}

	public BetterList<Vector3> FolllowPath {
		get { return _followPath; }
	}
	
	public bool FirstRecv {
		set { _firstRecv = value; }
	}
	
	public GameObject EffectMask {
		set { 
			_effectMask = value; 
			_effectMask.SetActive (false);
			_effectMask.GetComponent<BoxCollider> ().enabled = false;
		}
	}
}
