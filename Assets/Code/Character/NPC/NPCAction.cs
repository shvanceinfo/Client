using UnityEngine;
using System.Collections;
using manager;

public class NPCAction : MonoBehaviour 
{	
	public static bool sClickNPC = false;//csssdsds
	const int TASK_LEN = 3;
	public const int AWARD_LEN = 5;
	
	private NPC _ownerNPC; //属于哪个NPC
	
	private UILabel _npcName;		//NPC名称
	private GameObject _NPCAvasta;  //显示NPC的UI
	private GameObject _awardLbl; //奖励标题
	private GameObject _awardBg;
	private GameObject[] _taskList;  //任务清单
	private GameObject[] _awardList; //奖励清单
	private UILabel _descriptionLbl; //对话框内容
	private GameObject _dialogBtn; //对话点击按钮
	
	void OnMouseDown()
	{
        if (GuideManager.Instance.IsEnforceUI)
        {
            return;
        }

		if(!Global.pressOnUI())
		{
			sClickNPC = true;
			if(checkDistanceTrigger()) //在可触发范围
			{
				Vector3 faceDir = _ownerNPC.npcPos - CharacterPlayer.sPlayerMe.getPosition();
				faceDir.Normalize();
				CharacterPlayer.sPlayerMe.setFaceDir(faceDir);
				showTaskDialog();
			}
			else
			{
				TaskManager.Instance.isTaskFollow = true; //也跟点击任务追踪一样
				Vector3 moveToPos = _ownerNPC.pathLocatePos;
                //PlayerManager.Instance.agent.SetDestination(moveToPos);
                CharacterPlayer.sPlayerMe.GetAI().m_kPursueState.m_bGotoGate = false;
                CharacterPlayer.sPlayerMe.GetAI().m_kPursueState.m_kPursurNPC = _ownerNPC;
                CharacterPlayer.sPlayerMe.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, moveToPos);
                //PlayerManager.Instance.pathFind.beginMove(_ownerNPC);
			}
		}
	}
	
	//走到一定范围才显示task dialog, isClickFollow是否点击自动寻路，点击自动寻路就进入第二个页面
	public void showTaskDialog(bool isClickFollow = false)
	{
		_taskList = new GameObject[TASK_LEN];
		_awardList = new GameObject[AWARD_LEN];
		UIManager.Instance.openWindow(UiNameConst.ui_task_dialog);
		GameObject obj = UIManager.Instance.getUIFromMemory(UiNameConst.ui_task_dialog);
		setTasks(obj);
		if(isClickFollow && TaskManager.Instance.followTask != null&&
		   (_ownerNPC.npcID.Equals(TaskManager.Instance.followTask.submitNPCID)||_ownerNPC.npcID.Equals(TaskManager.Instance.followTask.acceptNPCID))
		   )
		{
			showTask(TaskManager.Instance.followTask);
		}
	}
	
	//设置该NPC身上的任务
	void setTasks(GameObject obj)
	{
		BetterList<Task> tasks = TaskManager.Instance.getTaskList(_ownerNPC.npcID);
		int i = 0;
		string listPrefix = "npc_task";
		if(tasks.size > 0)
		{
			foreach (Task task in tasks) 
			{
				if(i<TASK_LEN)
				{
					if(task != null)
					{
						_taskList[i] = obj.transform.Find("Panel/dialog/taskList/"+listPrefix+(i+1)).gameObject;
						enableCollider(_taskList[i], true);
						BtnClickTask click = _taskList[i].GetComponent<BtnClickTask>();
						UILabel titleLbl = _taskList[i].GetComponentInChildren<UILabel>();
						titleLbl.text = TaskManager.Instance.setTaskTitle(task, false);
						click.RelateTask = task;
						click.NpcAction = this;
						i++;
					}
				}
				else
					break;			
			}
		}
		if(i < TASK_LEN) //不足三条要隐藏
		{
			for(int j=i; j<TASK_LEN; j++)
			{
				_taskList[j] = obj.transform.Find("Panel/dialog/taskList/"+listPrefix+(j+1)).gameObject;
				enableCollider(_taskList[j], false);
			}
		}
		
		_npcName = obj.transform.Find("Panel/title/Label").GetComponent<UILabel>();
		_npcName.text = _ownerNPC.npcName;
		_descriptionLbl = obj.transform.Find("Panel/dialog/content/description").GetComponent<UILabel>();
		_descriptionLbl.text = _ownerNPC.defaultWord;
		_dialogBtn = obj.transform.Find("Panel/dialog/taskList/npcDialog").gameObject;
		enableCollider(_dialogBtn, false);
		
		_awardLbl = obj.transform.Find("Panel/dialog/content/Label").gameObject;
		_awardBg = obj.transform.Find("Panel/dialog/content/awardBg").gameObject;
		_awardLbl.SetActive(false);
		_awardBg.SetActive(false);
		for(i=0; i<AWARD_LEN; i++)
		{
			string awardPrefix = "award";
			_awardList[i] = obj.transform.Find("Panel/dialog/content/"+awardPrefix+(i+1)).gameObject;
			_awardList[i].SetActive(false);
		}
//		_ownerNPC.changeNPCLayer(true, _NPCAvasta.transform);
		_ownerNPC.changeNPCLayer(true);
	}
	
	//显示奖励面板
	public void showTask(Task task)
	{
		int len = task.rewardNum < AWARD_LEN ? task.rewardNum : AWARD_LEN;
		if(len > 0)
		{
			_awardLbl.SetActive(true);
			_awardBg.SetActive(true);
		}
        int index = 0;
        for (int i = 0; i < task.rewardItems.size; i++)
        {
            if (index < len)
            {
                _awardList[index].SetActive(true);
                UITexture texture = _awardList[index].GetComponentInChildren<UITexture>();
                texture.enabled = true;
                UISprite icon = _awardList[index].transform.FindChild("gold").GetComponent<UISprite>();
                UILabel label = _awardList[index].GetComponentInChildren<UILabel>();
                ItemTemplate item = ItemManager.GetInstance().GetTemplateByTempId((uint)task.rewardItems[i].Id);
                _awardList[index].transform.FindChild("icon").GetComponent<BtnTipsMsg>().Iteminfo = new ItemInfo((uint)task.rewardItems[i].Id,0,0);
				DealTexture.Instance.setTextureToIcon(texture, item, false);
                UISprite boder = _awardList[index].transform.FindChild("bg").GetComponent<UISprite>();
                boder.spriteName = BagManager.Instance.getItemBgByType(item.quality, true);
                icon.alpha = 0;
                label.text = Constant.NUM_PREFIX + task.rewardItems[i].Value.ToString();
                index++;
            }
        }
        for (int i = 0; i < task.rewardRes.size; i++)
        {
            if (index < len)
            {
                _awardList[index].SetActive(true);
                UITexture texture = _awardList[index].GetComponentInChildren<UITexture>();
                UISprite icon = _awardList[index].transform.FindChild("gold").GetComponent<UISprite>();
                UILabel label = _awardList[index].GetComponentInChildren<UILabel>();
                texture.mainTexture = null;
                texture.enabled = false;
                UISprite boder = _awardList[index].transform.FindChild("bg").GetComponent<UISprite>();
                boder.spriteName = BagManager.Instance.getItemBgByType(eItemQuality.eOrange, true);
                icon.alpha = 1;
                icon.spriteName = SourceManager.Instance.getIconByType((eGoldType)task.rewardRes[i].Id);
                label.text = Constant.NUM_PREFIX + task.rewardRes[i].Value.ToString();
                index++;
            }
        }
        for (int i = index; i < _awardList.Length; i++)
        {
            _awardList[i].SetActive(false);
        }


		enableCollider(_dialogBtn, true);  //对话按钮监听鼠标		
		for(int i=0; i<TASK_LEN; i++)
		{
			enableCollider(_taskList[i], false); //任务按钮隐藏监听
		}
		if(TaskManager.Instance.isTaskDialogNotOver(task))
		{
			string[] dialogs = TaskManager.Instance.getTaskDialog();
			_descriptionLbl.text = dialogs[0];
			_dialogBtn.GetComponentInChildren<UILabel>().text = dialogs[1];
		}
	}
	
	//使能碰撞
	private void enableCollider(GameObject obj, bool enable)
	{
		BoxCollider boxColli = obj.GetComponent<BoxCollider>();
		boxColli.enabled = enable;
		obj.SetActive(enable);
	}
	
	//计算NPC跟人物的距离是否可触发
	private bool checkDistanceTrigger()
	{
		Vector2 npcPos = new Vector2(_ownerNPC.npcPos.x, _ownerNPC.npcPos.z);
		Vector2 playerPos = new Vector2(CharacterPlayer.sPlayerMe.getPosition().x, CharacterPlayer.sPlayerMe.getPosition().z);
		Vector2 locatPos = new Vector2(_ownerNPC.pathLocatePos.x, _ownerNPC.pathLocatePos.z);
		return Vector2.Distance(npcPos, playerPos) <= Vector2.Distance(npcPos, locatPos)+0.1;
	}
	
	//getter and setter
	public NPC OwnerNPC
	{
		set {_ownerNPC = value;}
	}
}
