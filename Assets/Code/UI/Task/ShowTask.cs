using UnityEngine;
using System.Collections;

public class ShowTask : MonoBehaviour 
{
	const int LIST_HEIGHT = -30;
	private BetterList<Task> _taskList;  //任务清单
	private GameObject _awardLbl; //奖励标题
	private GameObject[] _awardList; //奖励清单
	private UILabel _stateLbl; 		 //状态标志
	private UILabel _descriptionLbl; //对话框内容
	private GameObject _taskTemplate; //任务列表的模板
	private TaskFollow _follow;
	
	void Awake()
	{
		_taskList = new BetterList<Task>();
		_awardList = new GameObject[NPCAction.AWARD_LEN];
		_taskList= TaskManager.Instance.getTaskList();
		for(int i=1; i<=NPCAction.AWARD_LEN; i++)		
		{
			string awardPrefix = "award";
			_awardList[i-1] = gameObject.transform.Find("Panel/task_detail/taskAward/"+awardPrefix+i).gameObject;
			_awardList[i-1].SetActive(false);
		}
		_awardLbl = gameObject.transform.Find("Panel/task_detail/taskAward/title").gameObject;
		_stateLbl = gameObject.transform.Find("Panel/task_detail/taskState/state").GetComponent<UILabel>();
		_descriptionLbl = gameObject.transform.Find("Panel/task_detail/taskDescription/description").GetComponent<UILabel>();
		_taskTemplate = gameObject.transform.Find("Panel/task_list/list/template").gameObject;
		_awardLbl.SetActive(false);
		_follow = gameObject.transform.Find("Panel/task_detail/followBtn").GetComponent<TaskFollow>();
		_taskTemplate.SetActive(false);
	}
	
	void Start()
	{
		for (int i=0; i<_taskList.size; i++)
		{
			Task task = _taskList[i];
			if(task.taskLine == TaskManager.MAIN_TASK_LINE) //主线任务直接显示
				showDetail(task);
            GameObject listObj = BundleMemManager.Instance.instantiateObj(_taskTemplate);
			listObj.transform.parent = _taskTemplate.transform.parent;
			listObj.transform.localPosition = new Vector3(_taskTemplate.transform.localPosition.x,  _taskTemplate.transform.localPosition.y+i*LIST_HEIGHT, _taskTemplate.transform.localPosition.z);
			listObj.transform.localRotation = _taskTemplate.transform.localRotation;
			listObj.transform.localScale = _taskTemplate.transform.localScale;
			listObj.SetActive(true);
			listObj.name = task.taskID.ToString();
			UILabel titleLbl = listObj.transform.Find("taskTitle").GetComponent<UILabel>();
			titleLbl.text = task.taskName;
			UILabel underLbl = listObj.transform.Find("underline").GetComponent<UILabel>();
			underLbl.text = createUnderLine(titleLbl);
			BtnClickTask click = listObj.GetComponent<BtnClickTask>();
			click.ShowTask = this;
		}
	}
	
	//显示任务详细信息
	public void showDetail(Task task)
	{
		_follow.RelateTask = task;
		_descriptionLbl.text = task.description;
		_stateLbl.text = getTastState(task);
		int len = task.rewardNum < NPCAction.AWARD_LEN ? task.rewardNum : NPCAction.AWARD_LEN;
		if(len > 0)
			_awardLbl.SetActive(true);
        int index = 0;
        for (int i = 0; i < task.rewardItems.size; i++)
        {
            if (index < len)
            {
                _awardList[index].SetActive(true);
                UITexture texture = _awardList[index].GetComponentInChildren<UITexture>();
                UISprite icon = _awardList[index].GetComponentInChildren<UISprite>();
                UILabel label = _awardList[index].GetComponentInChildren<UILabel>();
                ItemTemplate item = ItemManager.GetInstance().GetTemplateByTempId((uint)task.rewardItems[i].Id);
                DealTexture.Instance.setTextureToIcon(texture, item, false);
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
                UISprite icon = _awardList[index].GetComponentInChildren<UISprite>();
                UILabel label = _awardList[index].GetComponentInChildren<UILabel>();
                texture.mainTexture = null;
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
	}
	
	//获取任务状态
	private string getTastState(Task task)
	{
		string state = "", condition = "";
		switch (task.taskState) 
		{
			case eTaskState.eCanAccept:
				if(task.notEnoughLevel)
					state = Constant.COLOR_HUE + LanguageManager.GetText("task_state_cant_accept") + Constant.COLOR_END;
				else
					state = Constant.COLOR_HUE + LanguageManager.GetText("task_state_accept") + Constant.COLOR_END;
				break;
			case eTaskState.eInProgress:
				state = Constant.COLOR_ORANGE + LanguageManager.GetText("task_state_progressing") + Constant.COLOR_END;
				break;
			case eTaskState.eFinish:
				state = Constant.COLOR_GREEN + LanguageManager.GetText("task_state_award") + Constant.COLOR_END;
				break;
			default:
				break;
		}
		if(task.finishType == eTaskFinish.finishGate)
		{
			condition = "  "+Constant.COLOR_RED;
			condition += Constant.LEFT_PARENTHESIS + task.finishGateNum + "/" + task.finishParams[1] + Constant.RIGHT_PARENTHESIS + Constant.COLOR_RED;
		}
		return state + condition;
	}
	
	//生成下划线的文本
	public string createUnderLine(UILabel title)
    {
		int len = (int)(title.relativeSize.x/0.5f);
        string text = "";
        for (int i=0; i<len; i++)
        {
        	text += "_";
        }
        return text;
    }
}
