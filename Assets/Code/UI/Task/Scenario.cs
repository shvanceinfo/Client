using UnityEngine;
using System.Collections;

public class SubScenario
{
	public eDisplayType display;	//1表示剧情对话、2表示黑屏
	public bool iconLeft;			//头像显示在左边,右边
	public uint iconID;				//NPC表示使用玩家的模型和姓名
	public int delaySecond;			//对话持续时间（单位：秒），超过时间不操作，自动执行下一步
	public string content;			//具体对话内容
}

public class Scenario
{
	public uint scenarioID;
	public eTriggerType triggerType; //剧情触发类型
	public uint relateGateID;		//关联的关卡ID
	public BetterList<SubScenario> subScenarios; //包含的剧情段落 
	public bool hadPlay;			//已经播放过了
	
	private int _currentStep;			//当前步数
	private uint _relateTaskID;		//关联的任务ID
	
	public Scenario()
	{
		subScenarios = new BetterList<SubScenario>();
		_currentStep = 0;
		hadPlay = false;
	}
	
	//剧情是否播放完毕
	public bool playScenarioOver()
	{
		if(_currentStep < subScenarios.size)
			return false;
		else
		{
			hadPlay = true;
			return true;
		}
	}
	
	//获取剧情步骤
	public SubScenario getSubScenario()
	{
		if(_currentStep < subScenarios.size)
			return subScenarios[_currentStep++];
		return null;
	}
	
	//提交服务器
	public void submitServer()
	{
		ScenarioManager.Instance.submitScenario(_relateTaskID, scenarioID, _currentStep);
	}
	
	//getter and setter
//	public uint RelateTaskID
//	{
//		get {return _relateTaskID;}
//	}
	
	public uint RelateTaskID
	{
		set 
		{
			_relateTaskID = value; 
			ScenarioManager.Instance.addTaskIndex(_relateTaskID, this);
		}
	}
	
	public int CurrentStep
	{
		set { _currentStep = value; }
	}
}
