using UnityEngine;
using System.Collections;
using NetGame;

public enum eTriggerType
{
	notTrigger = 0,
	acceptTask = 1,	//任务接取触发，NPC对话结束触发
	finishTask = 2, //任务完成触发，NPC对话结束触发
	startGate = 3, 	//关卡开始触发，进入关卡，人物出生完毕触发
	finishGate = 4, //关卡通关触发，最后一个怪物杀完，宝箱掉落之前
	backToCity = 5	//关卡回城触发，回到主城站在关卡就触发
}

public enum eDisplayType
{
	useScenario = 1, //关卡通关
	blackSceen = 2	//对话任务
}

public class ScenarioManager
{	
	private static ScenarioManager _instance = null;
		
	public delegate void ScenarioOverHandler();	
	public bool passGateSuccess; //关卡成功通关
	
	private ScenarioOverHandler _scenarioOver; //点击对话结束后的回调
	private Hashtable _scenarioHash;
	private Hashtable _taskIndexHash;
	private GCReportScenario _reportScenario;
	
	public static ScenarioManager Instance
	{
		get 
		{ 
			if(_instance == null)
				_instance = new ScenarioManager();
			return _instance;
		}
	}
	
	private ScenarioManager()
	{
		_scenarioHash = new Hashtable();
		_taskIndexHash = new Hashtable();
		_reportScenario = new GCReportScenario();
		passGateSuccess = false;
	}
	
	//对话点击结束
	public void OnScenarioOver()
    {
        if (_scenarioOver != null)
        {
            _scenarioOver();
            UIManager.Instance.showHiddenUI(UiNameConst.ui_scenario, true);
            _scenarioOver = null;
        }
    }
	
	//将剧情保存为任务ID的索引
	public void addTaskIndex(uint taskID, Scenario scenario)
	{
		BetterList<Scenario> scenarios;
		if(_taskIndexHash.Contains(taskID))
			scenarios = _taskIndexHash[taskID] as BetterList<Scenario>;
		else
		{
			scenarios = new BetterList<Scenario>();
			_taskIndexHash.Add(taskID, scenarios);
		}
		scenarios.Add(scenario);
	}
	
	//send to server
	public void submitScenario(uint taskID, uint scenarioID, int step)
	{
		NetBase.GetInstance().Send(_reportScenario.ToBytes(taskID, scenarioID, step));
	}
	
	//reset all scenario
	public void clearAllPlaying()
	{
		foreach (Scenario scenario in _scenarioHash.Values) 
		{
			scenario.hadPlay = false;
			scenario.CurrentStep = 0;
		}
	}
	
	//根据设置否播放
	public void setHadPlaying(uint taskID, uint scenarioID)
	{
		if(scenarioID > 0)
		{
			if(_taskIndexHash.Contains(taskID))
			{
				BetterList<Scenario> scenarios = _taskIndexHash[taskID] as BetterList<Scenario>;
				foreach (Scenario scenario in scenarios) 
				{
					if(scenario.scenarioID == scenarioID)
						scenario.hadPlay = true;
				}
			}
		}
	}
	
	//显示剧情
	public void showScenario(Task task, ScenarioOverHandler scenarioOver, eTriggerType trigger=eTriggerType.notTrigger)
	{
		_scenarioOver = scenarioOver;
		if(existScenario(task, trigger))
		{
			if(trigger == eTriggerType.notTrigger)
			{
				if(task.taskState == eTaskState.eCanAccept)
					trigger = eTriggerType.acceptTask;
				else
					trigger = eTriggerType.finishTask;
			}
			Scenario scenario = getScenarioByTask(task.taskID, trigger);
			if(scenario != null)
			{
				UIManager.Instance.openWindow(UiNameConst.ui_scenario);
				MainLogic.sMainLogic.resumeGame(); //不暂停游戏
				GameObject obj = UIManager.Instance.getUIFromMemory(UiNameConst.ui_scenario);
				BtnClickScenario clickScenario = obj.transform.Find("scenarioBg").GetComponent<BtnClickScenario>();
				clickScenario.Scenario = scenario;
			    UIManager.Instance.showHiddenUI(UiNameConst.ui_scenario, false);
			}
			else
				OnScenarioOver();
		}
		else
			OnScenarioOver();
	}
	
	//判断相应任务是否存在对应的剧情
	private bool existScenario(Task task, eTriggerType trigger)
	{
		bool exist = false;
		if(_taskIndexHash.Contains(task.taskID))
		{
			switch (task.taskState) 
			{
				case eTaskState.eInit:
					break;
				case eTaskState.eCanAccept:
					if(!task.notEnoughLevel)
						exist = judgeScenarioByTrigger(task, eTriggerType.acceptTask);
					break;
				case eTaskState.eInProgress:
					if(trigger != eTriggerType.notTrigger)
						exist = judgeScenarioByTrigger(task, trigger);
					break;
				case eTaskState.eFinish:
					if(trigger == eTriggerType.backToCity)
						exist = judgeScenarioByTrigger(task, trigger);
					else
						exist = judgeScenarioByTrigger(task, eTriggerType.finishTask);
					break;
				case eTaskState.eAward:
					break;
				default:
					break;
			}
		}
		return exist;
	}
	
	//获取相应任务状态的剧情
	private Scenario getScenarioByTask(uint taskID, eTriggerType trigger)
	{
		BetterList<Scenario> scenarios = _taskIndexHash[taskID] as BetterList<Scenario>;
		foreach (Scenario scenario in scenarios) 
		{
			if(scenario.triggerType == trigger)
				return scenario;
		}
		return null;
	}
	
	//判断剧情列表中是否存在相应触发类型的剧情
	private bool judgeScenarioByTrigger(Task task, eTriggerType trigger)
	{
		BetterList<Scenario> scenarios = _taskIndexHash[task.taskID] as BetterList<Scenario>;
		foreach (Scenario scenario in scenarios) 
		{
			if(scenario.triggerType == trigger && !scenario.hadPlay) //并且没有播放过
			{
				if(trigger == eTriggerType.startGate || trigger == eTriggerType.finishGate)
				{
                    if (SceneManager.Instance.nextMapID == (int)task.mapId)
						return true;
					else
						return false;
				}
				else if(trigger == eTriggerType.backToCity)
				{
					if(task.taskState == eTaskState.eFinish || task.taskState == eTaskState.eAward)
						return true;
					else
						return false;
				}
				else	
					return true;
			}
		}
		return false;
	}
	
	//getter and setter
	public Hashtable ScenarioHash
	{
		get { return _scenarioHash; }
	}
}
