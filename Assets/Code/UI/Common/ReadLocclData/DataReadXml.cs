using helper;
using manager;
using model;

/**该文件实现的基本功能等
function: 读取帮助，制作，NPC位置，任务，剧情的XML
author:ljx
date:2013-11-09
**/
using System;
using UnityEngine;

/// <summary>
/// 帮助的XML解析模板
/// </summary>
public struct HelpInfo
{
	public int helpID;                    	 //id（帮助的ID，对应相应的名称）
	public string helpTitle;                 //帮助标题
	public string helpIcon;                  //帮助的ICON
	public BetterList<String> helpContent;   //帮助的内容
}

/// 模型的位置解析模板
public struct ModelPos
{
	public int modelID;                    	 //id（帮助的ID，对应相应的名称）
	public Vector3 modelPos;                 //位置信息
	public Vector3 modelRolate;                  //旋转信息
	public Vector3 modelScale;                  //缩放大小
	public float cameraView;   //帮助的内容
	public Vector3 scenarioPos;
	public Vector3 scenarioScale;
	public Vector3 scenarioRotate;
}

/// <summary>
/// 制作的XML解析模板
/// </summary>
public struct ProduceInfo
{
	public int bookID;                    		//制作书ID
	public BetterList<uint> needMaterials;    	//所需要的材料ID
	public BetterList<uint> materialNums;        //所需要的材料的数量
	public BetterList<uint> materialPrices;   	//所需要的材料的钻石价格
	public uint needGold;						//所需要的货币
	public uint productID;						//生产出来产品的ID
}

public struct ArenaPos
{
	public int id;            //竞技场人物ID
	public Vector3 pos;       //位置
	public Vector3 rotate;    //旋转角度
	public Vector3 scale;     //缩放比
	public float view;        //摄像机范围
	
}

//帮助配置文件解析
public class DataReadHelp : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{	
		HelpInfo help;	
		if (!data.ContainsKey (key)) {
			help = new HelpInfo ();
			help.helpContent = new BetterList<string> ();
			data.Add (key, help);
		}
		help = (HelpInfo)data [key];	
		switch (name) {
		case "ID":
			help.helpID = int.Parse (value);
			break;
		case "help_name":
			help.helpTitle = value;
			break;
		case "help_icon":
			help.helpIcon = value;
			break;
		}
		data [key] = help;
	}
    
	public HelpInfo getHelpInfo (int key)
	{
		if (!data.ContainsKey (key)) {
			HelpInfo di = new HelpInfo ();
			return di;
		}
		return (HelpInfo)data [key];
	}

	public BetterList<HelpInfo> getHelpList ()
	{
		BetterList<HelpInfo> list = new BetterList<HelpInfo> ();
		foreach (HelpInfo info in data.Values) {
			list.Add (info);
		}
		return list;
	}
}
    
//任务配置文件解析
public class DataReadTask : DataReadBase
{
	bool first = true; //第一条任务就是第一个开始得
	
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{	
		Task task;
		uint hashKey = (uint)key;
		if (TaskManager.Instance.TaskHash.Contains (hashKey)) {
			task = TaskManager.Instance.TaskHash [hashKey] as Task;
		} else {
			task = new Task ();
			TaskManager.Instance.TaskHash.Add (hashKey, task);
		}
		string[] splits = null;
		char[] charSeparators = new char[] {','};
		switch (name) {
		case "ID":
			task.taskID = uint.Parse (value);
			break;
		case "task_name":
			task.taskName = value;
			break;
		case "task_discrip":
			task.description = value;
			break;
		case "task_type":
			task.taskLine = int.Parse (value);
			if (first && task.taskLine == TaskManager.MAIN_TASK_LINE) { //第一个主线任务 
				first = false;
				TaskManager.Instance.FirstID = (uint)key;
			}
			TaskManager.Instance.addTaskByLine (task);
			break;
		case "start_level":
			task.startLevel = int.Parse (value);
			break;
		case "finish_level":
			task.finishLevel = int.Parse (value);
			break;
		case "finish_type":
			task.finishType = (eTaskFinish)int.Parse (value);
			break;
		case "finish_Des":
			splits = value.Split (charSeparators);
			foreach (string every in splits) {
				task.finish_Des.Add (every);
			}
			break;
		case "finish_value1":
			task.finishParams.Add (value);
			break;
		case "finish_value2":
			task.finishParams.Add (value);
			break;
		case "finish_value3":
			task.finishParams.Add (value);
			break;
		case "start_NPC":
			task.acceptNPCID = uint.Parse (value);
			break;
		case "finish_NPC":
			task.submitNPCID = uint.Parse (value);
			break;
		case "start_NPC_word":
			splits = value.Split (charSeparators);
			foreach (string every in splits) {
				task.startNPCDialog.Add (every);
			}
			break;
		case "start_button_word":
			splits = value.Split (charSeparators);
			foreach (string every in splits) {
				task.startBtn.Add (every);
			}
			break;
		case "unfinish_NPC_word":
			splits = value.Split (charSeparators);
			foreach (string every in splits) {
				task.progressDialog.Add (every);
			}
			break;
		case "unfinish_button_word":
			splits = value.Split (charSeparators);
			foreach (string every in splits) {
				task.progressBtn.Add (every);
			}
			break;
		case "finish_NPC_word":
			splits = value.Split (charSeparators);
			foreach (string every in splits) {
				task.finishNPCDialog.Add (every);
			}
			break;
		case "finish_button_word":
			splits = value.Split (charSeparators);
			foreach (string every in splits) {
				task.finishBtn.Add (every);
			}
			break;
		case "reward_Resource":
			splits = value.Split (charSeparators);
			for (int i = 0; i < splits.Length; i+=2) {
				IdStruct id = new IdStruct (
                    int.Parse (splits [i]),
                    int.Parse (splits [i + 1])
				);
				task.rewardRes.Add (id);
				task.rewardNum++;
			}
			break;
		case "reward_Item":
			splits = value.Split (charSeparators);
			for (int i = 0; i < splits.Length; i+=2) {
				IdStruct id = new IdStruct (
                    int.Parse (splits [i]),
                    int.Parse (splits [i + 1])
				);
				task.rewardItems.Add (id);
				task.rewardNum++;
			}
			break;
		case "judgeType":
			task.judgeType = (eTaskJudgeType)int.Parse (value);
			break;
		case "nextID":
			splits = value.Split (charSeparators);
			foreach (string every in splits) {
				uint nextID = uint.Parse (every);
				if (nextID > 0)
					task.nextTasks.Add (uint.Parse (every));
			}
			break;
		case "task_typeNum":
			task.taskOrder = uint.Parse (value);
			break;               
		case "MapID":
			task.mapId = uint.Parse (value);
			break;
		case "FunctionID":
			{
				task.functionId = uint.Parse (value);
				break;
			
			}
		}



	}
}

//NPC配置文件解析
public class DataReadNPC : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{	
		NPC npc;
		string[] splits = null;
		char[] charSeparators = new char[] {','};
		uint hashKey = (uint)key;
		if (NPCManager.Instance.NpcHash.Contains (hashKey)) {
			npc = NPCManager.Instance.NpcHash [hashKey] as NPC;
		} else {
			npc = new NPC ();
			NPCManager.Instance.NpcHash.Add (hashKey, npc);
		}
		
		switch (name) {
		case "ID":
			npc.npcID = uint.Parse (value);
			break;
		case "NPC_name":
			npc.npcName = value;
			break;
		case "NPC_icon":
			npc.npcIcon = value;
			break;
		case "NPC_model":
			npc.npcModel = value;
			break;
		case "MapID":
			npc.mapID = uint.Parse (value);
			break;
		case "PosXZ":
			splits = value.Split (charSeparators);
			npc.npcPos = new Vector3 (float.Parse (splits [0]), 0f, float.Parse (splits [1]));
			break;
		case "RotationY":
			npc.rotatePosEuler = new Vector3 (0f, float.Parse (value), 0f);
			break;
		case "DefaultWord":
			npc.defaultWord = value;
			break;
		case "parentPos":
			splits = value.Split (charSeparators);
			npc.uiPos = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "parentRotate":
			splits = value.Split (charSeparators);
			npc.uiRotateEuler = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "parentScale":
			splits = value.Split (charSeparators);
			npc.uiScale = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "scenarioScale":
			splits = value.Split (charSeparators);
			npc.scenarioScale = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "scenarioPos":
			splits = value.Split (charSeparators);
			npc.scenarioPos = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "scenarioRotate":
			splits = value.Split (charSeparators);
			npc.scenarioRotate = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "cameraView":
			npc.caremeView = float.Parse (value);
			break;
		case "PathfindingXZ":
			splits = value.Split (charSeparators);
			npc.pathLocatePos = new Vector3 (float.Parse (splits [0]), 0f, float.Parse (splits [1]));
			break;
		default:
			break;
		}
	}
    
}

//剧情配置文件
public class DataReadScenario : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
    
	public void appendSubAttri (uint key, int step, string name, string value)
	{
		Scenario scenario = ScenarioManager.Instance.ScenarioHash [key] as Scenario;
		SubScenario subScenario;
		if (scenario.subScenarios.size >= step)
			subScenario = scenario.subScenarios [step - 1];
		else {
			subScenario = new SubScenario ();
			scenario.subScenarios.Add (subScenario);
		}
		switch (name) {
		case "plotType":
			subScenario.display = (eDisplayType)int.Parse (value);
			break;
		case "isLeft":
			subScenario.iconLeft = int.Parse (value) == 1;
			break;
		case "NPC":
			if (value.Equals (Constant.PLAYER_NAME))
				subScenario.iconID = 0;
			else
				subScenario.iconID = uint.Parse (value);
			break;
		case "delay":
			subScenario.delaySecond = int.Parse (value);
			break;
		case "diaCon":
			subScenario.content = value;
			break;
		default:
			break;
		}
	}

	public override void appendAttribute (int key, string name, string value)
	{	
		Scenario scenario;
		uint hashKey = (uint)key;
		if (ScenarioManager.Instance.ScenarioHash.Contains (hashKey)) {
			scenario = ScenarioManager.Instance.ScenarioHash [hashKey] as Scenario;
		} else {
			scenario = new Scenario ();
			ScenarioManager.Instance.ScenarioHash.Add (hashKey, scenario);
		}		
		switch (name) {
		case "ID":
			scenario.scenarioID = uint.Parse (value);
			break;
		case "TaskID":
			scenario.RelateTaskID = uint.Parse (value);
			break;
		case "triggerType":
			scenario.triggerType = (eTriggerType)int.Parse (value);
			break;
		default:
			break;
		}
	}
    
}

//旋转位置文件解析
public class DataModelPos : DataReadBase
{
	private string _prefix = "pos_";

	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{	
		string index = _prefix + key;
		ModelPos modelPos;	
		if (!data.ContainsKey (index)) {
			modelPos = new ModelPos ();
			data.Add (index, modelPos);
		}
		modelPos = (ModelPos)data [index];	
		string[] splits = null;
		char[] charSeparators = new char[] {','};
		switch (name) {
		case "ID":
			modelPos.modelID = int.Parse (value);
			break;
		case "pos":
			splits = value.Split (charSeparators);
			modelPos.modelPos = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "rotate":
			splits = value.Split (charSeparators);
			modelPos.modelRolate = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "Scale":
			splits = value.Split (charSeparators);
			modelPos.modelScale = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "scenarioPos":
			splits = value.Split (charSeparators);
			modelPos.scenarioPos = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "scenarioRotate":
			splits = value.Split (charSeparators);
			modelPos.scenarioRotate = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "scenarioScale":
			splits = value.Split (charSeparators);
			modelPos.scenarioScale = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "cameraView":
			modelPos.cameraView = float.Parse (value);
			break;
		default:
			break;
		}
		data [index] = modelPos;
	}
    
	public ModelPos getModelInfo (int key)
	{
		string index = _prefix + key;
		if (!data.ContainsKey (index)) {
			ModelPos di = new ModelPos ();
			return di;
		}
		return (ModelPos)data [index];
	}
}

//旋转位置文件解析
public class DataArenaPos : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{	
		ArenaPos arenaPos;	
		if (!data.ContainsKey (key)) {
			arenaPos = new ArenaPos ();
			data.Add (key, arenaPos);
		}
		arenaPos = (ArenaPos)data [key];	
		string[] splits = null;
		char[] charSeparators = new char[] {','};
		switch (name) {
		case "ID":
			arenaPos.id = int.Parse (value);
			break;
		case "pos":
			splits = value.Split (charSeparators);
			arenaPos.pos = new Vector3 (float.Parse (splits [0].Trim ()), float.Parse (splits [1].Trim ()), float.Parse (splits [2].Trim ()));
			break;
		case "rotate":
			splits = value.Split (charSeparators);
			arenaPos.rotate = new Vector3 (float.Parse (splits [0].Trim ()), float.Parse (splits [1].Trim ()), float.Parse (splits [2].Trim ()));
			break;
		case "scale":
			splits = value.Split (charSeparators);
			arenaPos.scale = new Vector3 (float.Parse (splits [0].Trim ()), float.Parse (splits [1].Trim ()), float.Parse (splits [2].Trim ()));
			break;
		case "view":
			arenaPos.view = float.Parse (value);
			break;
		default:
			break;
		}
		data [key] = arenaPos;
	}
    
	public ArenaPos getArenaInfo (int key)
	{
		if (!data.ContainsKey (key)) {
			ArenaPos di = new ArenaPos ();
			return di;
		}
		return (ArenaPos)data [key];
	}
}