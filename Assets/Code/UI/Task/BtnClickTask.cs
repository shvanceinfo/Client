using UnityEngine;
using System.Collections;

public class BtnClickTask : MonoBehaviour 
{
	enum eClickBtn
	{
		dialogBtn = 0, //对话按钮
		dialogContent, //对话内容
		btnShowTask,	 //点击底部任务
		clickTaskList //点击左边任务列表
	}
	
	const string CONTENT_FLAG = "npc_task";
	const string DIALOG_FLAG = "npcDialog";
	const string BOTTOM_BTN = "task";
	
	private Task _relateTask;  //关联的任务ID
	private NPCAction _npcAction; //关联的NPC行为
	private UILabel _btnTitle;  //按钮显示的内容
	private UILabel _description; //描述
	private eClickBtn _btnType;
	private ShowTask _showTask;
	
	void Awake () 
	{
		if(this.name.Equals(DIALOG_FLAG)) //点击NPC弹出的任务对话框
		{
			_btnType = eClickBtn.dialogBtn;
			_btnTitle = transform.GetComponentInChildren<UILabel>();
			_description = transform.parent.parent.Find("content/description").GetComponent<UILabel>();
		}
		else if(this.name.IndexOf(CONTENT_FLAG) != -1)
		{
			_btnType = eClickBtn.dialogContent;
			_btnTitle = transform.GetComponentInChildren<UILabel>();
		}
		else if(this.name.Equals(BOTTOM_BTN))
			_btnType = eClickBtn.btnShowTask;
		else
			_btnType = eClickBtn.clickTaskList;
	}
	

	
	void OnClick()
	{	
		switch (_btnType)
		{
			case BtnClickTask.eClickBtn.dialogBtn:
				if(TaskManager.Instance.isTaskDialogNotOver())
				{
					string[] dialogs = TaskManager.Instance.getTaskDialog();
					_description.text = dialogs[0];
					_btnTitle.text = dialogs[1];
				}
				break;
			case BtnClickTask.eClickBtn.dialogContent:
				_npcAction.showTask(_relateTask);
				break;
			case BtnClickTask.eClickBtn.btnShowTask:
				UIManager.Instance.openWindow(UiNameConst.ui_task);
				GameObject obj = UIManager.Instance.getUIFromMemory(UiNameConst.ui_task);
				break;
			case BtnClickTask.eClickBtn.clickTaskList:
				uint key = uint.Parse(this.name);
				Task task = TaskManager.Instance.TaskHash[key] as Task;
				if(task != null)					
					_showTask.showDetail(task);
				break;
			default:
				break;
		}	
	}
	
	//getter and setter
	public Task RelateTask
	{
		set { _relateTask = value; }
	}
	
	public NPCAction NpcAction
	{
		set { _npcAction = value; }
	}
	
	public ShowTask ShowTask
	{
		set { _showTask = value; }
	}
}
