using UnityEngine;
using System.Collections;

public class NPCManager 
{
	public const string SIMBOL_ACCENT_FAIL = "TanHao_hui";   //主线任务等级不满足
	public const string SIMBOL_ACCENT = "TanHao_cheng";   //可领取阶段
	public const string SIMBOL_PROCESSING = "WenHao_hui";   //进行中
	public const string SIMBOL_COMPLETE = "WenHao_cheng";   //已完成
	
	public NPC openNPC; //当前打开的NPC
	public NPC mainTaskNPC; //主任务挂载的NPC
    public NPC pathFollowNPC; //任务追踪的NPC
	public bool hasInstantiate;  //是否已经实例化过了
	
	private static NPCManager _instance = null;
	
	private Hashtable _npcHash; //以NPC ID作为索引
	private BetterList<GameObject> _instanceNPCs; //NPC的实例化
	private Camera _modelCamera;
    private int _initCount;
	
	public static NPCManager Instance
	{
		get 
		{ 
			if(_instance == null)
				_instance = new NPCManager();
			return _instance;
		}
	}
	
	private NPCManager()
	{
		_npcHash = new Hashtable();
		_instanceNPCs = new BetterList<GameObject>();
		hasInstantiate = false;
		openNPC = null;
		mainTaskNPC = null;
		_modelCamera = null;
	}
	
	//initiate the model carema
	public void createCamera(bool create=true)
	{
		if(create)
		{
			if(_modelCamera == null)
			{
			    GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.MODEL_CAMERA, EBundleType.eBundleCommon);
                GameObject obj = BundleMemManager.Instance.instantiateObj(asset, new Vector3(0,0,-10), Quaternion.identity);
				_modelCamera = obj.GetComponent<Camera>();
			}
		}
		else if(_modelCamera != null)
		{
			Object.Destroy(_modelCamera.gameObject);
			_modelCamera = null;
		}
	}
	
	//初始化各个NPC
	public void initNPC(int mapID)
	{
		BetterList<NPC> npcs = getNPCs((uint)mapID);
		if(_instanceNPCs.size > 0)
		{
			for(int i=0; i<_instanceNPCs.size; i++) 
			{
				GameObject obj = _instanceNPCs[i];
				GameObject.Destroy(obj);
			}
		}
		_instanceNPCs.Clear();
	    _initCount = 0;
		if(npcs.size > 0)
		{
            initSingleNPC(npcs);
		}
		hasInstantiate = true;
//		TaskManager.Instance.markNPCTag();
	}
	
	//根据NPC的ID获取相应的NPC
	public NPC getNPCByID(uint npcID)
	{
		if(_npcHash.Contains(npcID))
			return _npcHash[npcID] as NPC;
		return null;
	}
	
	//使能NPC的点击
	public void enableCollider(bool enable)
	{
		foreach (GameObject npc in _instanceNPCs) 
		{
			if(npc != null)
			{
				CapsuleCollider collider = npc.GetComponent<CapsuleCollider>();
				if(collider != null)
					collider.enabled = enable;
			}
		}
	}
	
	//对话点击完毕设置各个NPC的状态
	public void markNPCTag(NPC npc)
	{
		if(npc == null || npc.mapID != CharacterPlayer.character_property.getServerMapID()) //不在一个主城不需要打Tag
			return;
		if(SceneManager.Instance.currentScene == SCENE_POS.IN_CITY) //只有主城的NPC才打tag
		{
			BetterList<Task> tasks = TaskManager.Instance.getTaskList(npc.npcID);
			if(tasks.size == 0)
				npc.tagTaskState(null); //无任务清空标记
			else
			{
				Task task = tasks[0];
				if(task.taskLine == TaskManager.MAIN_TASK_LINE)
					mainTaskNPC = npc;
				if(tasks.size > 1) //多余一条任务才需要判断
				{
					Task[] compareTasks = new Task[4];
					for(int i=0; i<4; i++)
						compareTasks[i] = null;
					foreach(Task eachTask in tasks) //遍历打标记的任务
					{
						if(eachTask.taskState == eTaskState.eFinish)
						{
							if(compareTasks[0] == null)
								compareTasks[0] = eachTask;
						}
						else if(eachTask.taskState == eTaskState.eCanAccept)
						{
							if(compareTasks[1] == null)
								compareTasks[1] = eachTask;
						}
						else if(eachTask.taskState == eTaskState.eInProgress)
						{
							if(compareTasks[2] == null)
								compareTasks[2] = eachTask;
						}
						else 
						{
							if(compareTasks[3] == null)
								compareTasks[3] = eachTask;
						}						
					}
					for(int i=0; i<4; i++)
					{
						if(compareTasks[i] != null)
						{
							task = compareTasks[i];
							break;
						}
					}
				}
				npc.tagTaskState(TaskManager.Instance.getTagMark(task)); 				
			}
		}
	}

    //实例化ＮＰＣ
    private void initSingleNPC(BetterList<NPC> npcs)
    {
        if (_initCount >= npcs.size)
	        return;
        NPC npc = npcs[_initCount];
        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleNPC))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleNPC, npc.npcModel,
                (asset) =>
                {
                    GameObject newNPC = npc.instanciateSelf(asset);
                    if (newNPC != null)
                    {
                        _instanceNPCs.Add(newNPC);
                        markNPCTag(npc);
                    }
                    _initCount++;
                    initSingleNPC(npcs);
                });
        }
        else
        {
            GameObject asset = BundleMemManager.Instance.getPrefabByName(npc.npcModel, EBundleType.eBundleNPC);
            GameObject newNPC = npc.instanciateSelf(asset);
            if (newNPC != null)
            {
                _instanceNPCs.Add(newNPC);
                markNPCTag(npc);
            }
            _initCount++;
            initSingleNPC(npcs);
        }                
    }
	
	//获取在一个地图中的所有NPC
	private BetterList<NPC> getNPCs(uint mapID)
	{
		BetterList<NPC> npcs = new BetterList<NPC>();
		foreach (NPC npc in _npcHash.Values)
		{
			if(npc.mapID == mapID)
				npcs.Add(npc);
		}
		return npcs;
	}
	
    //getter and setter
	public Hashtable NpcHash
	{
		get { return _npcHash; }
	}
	
	public Camera ModelCamera
	{
		get 
		{ 
			if(_modelCamera == null)
				createCamera();
			return _modelCamera; 
		}
	}
}
