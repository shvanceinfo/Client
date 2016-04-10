using UnityEngine;
using System.Collections;
using model;
using manager;
using helper;

public class TaskView : MonoBehaviour
{

	private TaskFollow _follow;             //跟随
	private BetterList<Task> _taskList;     //任务完成和进行中清单
	private BetterList<Task> _canAcceptTaskList;
	private GameObject[] _awardList;        //奖励


	private GameObject _tabel1Obj;          //table1Obj
	private Transform _table1Grid;          //列表
	private GameObject _taskPrefab;         //预制物体
	private GameObject _error;
	private UICheckBoxColor _cbkTb1;
	private UICheckBoxColor _cbkTb2;
	private UILabel _desc1;
	private UILabel _desc2;
	private UILabel _desc3;
	private int TableIndex;

	private void Awake ()
	{
		TableIndex = 1;
		_taskList = TaskManager.Instance.getTaskCompleteAndPro ();
		_canAcceptTaskList = TaskManager.Instance.getTaslAcceptList ();
		_awardList = new GameObject[NPCAction.AWARD_LEN];
		string award = "award";
		for (int i = 1; i <= NPCAction.AWARD_LEN; i++) {
			_awardList [i - 1] = transform.FindChild ("Table1/Detail/" + award + i).gameObject;
			_awardList [i - 1].SetActive (false);
		}

		_cbkTb1 = transform.FindChild ("Btn_Table1").GetComponent<UICheckBoxColor> ();
		_cbkTb2 = transform.FindChild ("Btn_Table2").GetComponent<UICheckBoxColor> ();
		_tabel1Obj = transform.FindChild ("Table1").gameObject;
		_table1Grid = transform.FindChild ("Table1/Panel/Grid");
		_taskPrefab = transform.FindChild ("Table1/Panel/Item").gameObject;

		_follow = transform.FindChild ("Table1/followBtn").GetComponent<TaskFollow> ();

		_desc1 = transform.FindChild ("Table1/Detail/lbl_Description1").GetComponent<UILabel> ();
		_desc2 = transform.FindChild ("Table1/Detail/lbl_Description2").GetComponent<UILabel> ();
		_desc3 = transform.FindChild ("Table1/Detail/lbl_Description3").GetComponent<UILabel> ();
		_error = transform.FindChild ("Table1/Detail/Error").gameObject;
	}

	private void Start ()
	{
		_error.SetActive (false);
		DisPlayTaskList ();
	}

	private void OnTable1Click ()
	{
		
		if (_cbkTb1.isChecked) {
			TableIndex = 1;
			DisPlayTaskList ();
		}
            
        
	}

	private void OnTable2Click ()
	{
		if (_cbkTb2.isChecked) {
			TableIndex = 2;
			DisPlayTaskGetList ();
		}
	}

	public void OnTaskItemClick (GameObject obj)
	{
		int id = int.Parse (obj.name);
		if (TableIndex == 1) {
			DisplayTaskInfo (_taskList [id], id);
		} else if (TableIndex == 2) {
			DisplayTaskAccaptInfo (_canAcceptTaskList [id], id);
		}
        
	}

	//显示已接任务列表
	private void DisPlayTaskList ()
	{
		int count = _table1Grid.childCount;
        
		if (count < _taskList.size) {
			for (int i = count; i < _taskList.size; i++) {
				AddItemTemplatePrefab (_taskPrefab, _table1Grid, i);
			}
		} else if (count > _taskList.size) {
			for (int i = _taskList.size; i < count; i++) {
				DeleteItemTemplate (_table1Grid, i);
			}
		}
		if (_taskList.size == 0) {
			DisplayTaskInfo (null, -1);
			return;
		}
		for (int i = 0; i < _taskList.size; i++) {
			DisPlayTaskItem (_taskList [i], i);
		}
		if (_taskList.size != 0) {
			DisplayTaskInfo (_taskList [0], 0);
		}
		_table1Grid.GetComponent<UIGrid> ().Reposition ();
		_table1Grid.transform.parent.GetComponent<UIScrollView> ().ResetPosition ();
	}

	private void DisPlayTaskItem (Task task, int index)
	{
		Transform t = _table1Grid.FindChild (index.ToString ());

		if (t != null) {
			t.GetComponentInChildren<UILabel> ().text = string.Format ("[d9cfa7]{0}[-]", task.taskName);
		}
	}

	private void AddItemTemplatePrefab (GameObject prefab, Transform grid, int id)
	{
		prefab.SetActive (true);
		GameObject obj = BundleMemManager.Instance.instantiateObj (prefab);
		obj.transform.parent = grid;
		obj.name = id.ToString ();
		obj.transform.localPosition = new Vector3 (0, 0, 0);
		obj.transform.localScale = new Vector3 (1, 1, 1);
		prefab.SetActive (false);
	}

	private void DeleteItemTemplate (Transform grid, int id)
	{
		Transform t = grid.FindChild (id.ToString ());
		if (t == null) {
			Debug.LogError ("Index error");
		} else {
			Destroy (t.gameObject);
		}
	}



	//显示可接任务
	private void DisPlayTaskGetList ()
	{
		int count = _table1Grid.childCount;
        
		if (count < _canAcceptTaskList.size) {
			for (int i = count; i < _canAcceptTaskList.size; i++) {
				AddItemTemplatePrefab (_taskPrefab, _table1Grid, i);
			}
		} else if (count > _canAcceptTaskList.size) {
			for (int i = _canAcceptTaskList.size; i < count; i++) {
				DeleteItemTemplate (_table1Grid, i);
			}
		}
		if (_canAcceptTaskList.size == 0) {
			DisplayTaskAccaptInfo (null, -1);
			return;
		}
		for (int i = 0; i < _canAcceptTaskList.size; i++) {
			DisPlayTaskItem (_canAcceptTaskList [i], i);
		}
		if (_canAcceptTaskList.size != 0) {
			DisplayTaskAccaptInfo (_canAcceptTaskList [0], 0);
		}
		_table1Grid.GetComponent<UIGrid> ().Reposition ();
		_table1Grid.transform.parent.GetComponent<UIScrollView> ().ResetPosition ();
	}

	private void DisplayTaskAccaptInfo (Task task, int id)
	{
        
            
		if (id >= 0 && task != null) {
			ActiveDesc ();
			_desc1.text = task.description;
			_desc2.text = getTastState (task);
			_desc3.text = "";
			_follow.RelateTask = task;
			int len = task.rewardNum < NPCAction.AWARD_LEN ? task.rewardNum : NPCAction.AWARD_LEN;
			int index = 0;
			for (int i = 0; i < task.rewardItems.size; i++) {
				if (index < len) {
					_awardList [index].SetActive (true);
					UITexture texture = _awardList [index].GetComponentInChildren<UITexture> ();
					texture.enabled = true;
					UISprite icon = _awardList [index].transform.FindChild ("gold").GetComponent<UISprite> ();
					UILabel label = _awardList [index].GetComponentInChildren<UILabel> ();
					ItemTemplate item = ItemManager.GetInstance ().GetTemplateByTempId ((uint)task.rewardItems [i].Id);
					UISprite boder = _awardList [index].transform.FindChild ("bg").GetComponent<UISprite> ();
					boder.spriteName = BagManager.Instance.getItemBgByType (item.quality, true);
					_awardList [index].transform.FindChild ("icon").GetComponent<BtnTipsMsg> ().Iteminfo = new ItemInfo ((uint)task.rewardItems [i].Id, 0, 0);
					DealTexture.Instance.setTextureToIcon (texture, item, false);
					icon.alpha = 0f;
					label.text = Constant.NUM_PREFIX + task.rewardItems [i].Value.ToString ();
					index++;
				}
			}
			for (int i = 0; i < task.rewardRes.size; i++) {
				if (index < len) {
					_awardList [index].SetActive (true);
					UITexture texture = _awardList [index].GetComponentInChildren<UITexture> ();
					UISprite icon = _awardList [index].transform.FindChild ("gold").GetComponent<UISprite> ();
					UILabel label = _awardList [index].GetComponentInChildren<UILabel> ();
					texture.enabled = false;
					UISprite boder = _awardList [index].transform.FindChild ("bg").GetComponent<UISprite> ();
					boder.spriteName = BagManager.Instance.getItemBgByType (eItemQuality.eOrange, true);
					icon.alpha = 1f;
					icon.spriteName = SourceManager.Instance.getIconByType ((eGoldType)task.rewardRes [i].Id);
					label.text = Constant.NUM_PREFIX + task.rewardRes [i].Value.ToString ();
					index++;
				}
			}
			for (int i = index; i < _awardList.Length; i++) {
				_awardList [i].SetActive (false);
			}


		} else {
			HiddenDesc ();

		}
	}

	private void DisplayTaskInfo (Task task, int id)
	{
		if (id >= 0 && task != null) {
			ActiveDesc ();
			_desc1.text = string.Format ("[d9cfa7]{0}[-]", task.description);
			_desc2.text = getTastState (task);
			_desc3.text = "";
			_follow.RelateTask = task;
			int len = task.rewardNum < NPCAction.AWARD_LEN ? task.rewardNum : NPCAction.AWARD_LEN;
			int index = 0;
			for (int i = 0; i < task.rewardItems.size; i++) {
				if (index < len) {
					_awardList [index].SetActive (true);
					UITexture texture = _awardList [index].GetComponentInChildren<UITexture> ();
					texture.enabled = true;
					UISprite icon = _awardList [index].transform.FindChild ("gold").GetComponent<UISprite> ();
					UILabel label = _awardList [index].GetComponentInChildren<UILabel> ();
					ItemTemplate item = ItemManager.GetInstance ().GetTemplateByTempId ((uint)task.rewardItems [i].Id);
					_awardList [index].transform.FindChild ("icon").GetComponent<BtnTipsMsg> ().Iteminfo = new ItemInfo ((uint)task.rewardItems [i].Id, 0, 0);
					DealTexture.Instance.setTextureToIcon (texture, item, false);
					UISprite boder = _awardList [index].transform.FindChild ("bg").GetComponent<UISprite> ();
					boder.spriteName = BagManager.Instance.getItemBgByType (item.quality, true);
					icon.alpha = 0f;
					label.text = Constant.NUM_PREFIX + task.rewardItems [i].Value.ToString ();
					index++;
				}
			}
			for (int i = 0; i < task.rewardRes.size; i++) {
				if (index < len) {
					_awardList [index].SetActive (true);
					UITexture texture = _awardList [index].GetComponentInChildren<UITexture> ();
					UISprite icon = _awardList [index].transform.FindChild ("gold").GetComponent<UISprite> ();
					UILabel label = _awardList [index].GetComponentInChildren<UILabel> ();
					texture.mainTexture = null;
					texture.enabled = false;
					UISprite boder = _awardList [index].transform.FindChild ("bg").GetComponent<UISprite> ();
					boder.spriteName = BagManager.Instance.getItemBgByType (eItemQuality.eOrange, true);
					icon.alpha = 1f;
					icon.spriteName = SourceManager.Instance.getIconByType ((eGoldType)task.rewardRes [i].Id);
					label.text = Constant.NUM_PREFIX + task.rewardRes [i].Value.ToString ();
					index++;
				}
			}
			for (int i = index; i < _awardList.Length; i++) {
				_awardList [i].SetActive (false);
			}


		} else {

			HiddenDesc ();
		}
	}

	private void ActiveDesc ()
	{
		_error.SetActive (false);
		_follow.gameObject.SetActive (true);
	}

	private void HiddenDesc ()
	{
		_error.SetActive (true);
		_follow.gameObject.SetActive (false);
		for (int i = 0; i < _awardList.Length; i++) {
			_awardList [i].SetActive (false);
		}
		_desc1.text = "";
		_desc2.text = "";
		_desc3.text = "";
	}

	private string getTastState (Task task)
	{
        
		string str = "";
		BetterList<string> dess = task.finish_Des;
		BetterList<IdStruct> cs = task.GetTaskCondition ();
		BetterList<IdStruct> ccs = task.curCondition;
		switch (task.taskState) {
                
		case eTaskState.eCanAccept:
			str = "可领取";
			break;
		case eTaskState.eInProgress:
			switch (task.finishType) {
			case eTaskFinish.finishGate:
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
                            ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case eTaskFinish.npcDialog:
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0], task.finishParams [1], task.finishParams [1]);
				break;
			case eTaskFinish.killEnemy:
				for (int i = 0; i < dess.size; i++) {
					str += string.Format ("{0}  进行中({1}/{2})\n", dess [i],
                                ccs.size == 0 ? 0 : ccs [i].Value, cs [i].Value);
				}
				break;
			case eTaskFinish.getBindItem:
				for (int i = 0; i < dess.size; i++) {
					str += string.Format ("{0}  进行中({1}/{2})\n", dess [i],
                                ccs.size == 0 ? 0 : ccs [i].Value, cs [i].Value);
				}
				break;
			case eTaskFinish.getItem:
				for (int i = 0; i < dess.size; i++) {
					str += string.Format ("{0}  进行中({1}/{2})\n", dess [i],
                                ccs.size == 0 ? 0 : ccs [i].Value, cs [i].Value);
				}
				break;
					
			case eTaskFinish.levelUp: //人物等级提升
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0], CharacterPlayer.character_property.level, task.finishParams [0]);
				break;
			case  eTaskFinish.vipUp:  //vip等级提升
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0], VipManager.Instance.VipLevel, task.finishParams [0]);
				break;
			case 	 eTaskFinish.equipPlus:		//强化装备
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case 	 eTaskFinish.equipAdvance:	//进阶装备
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case 	 eTaskFinish.equipXiLian:	//洗练装备
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.wingUp:	//羽翼升级
				switch ((WingUpState)int.Parse (task.finishParams [0])) {
				case WingUpState.LvUpCount:
					str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
					                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
					break;
				case WingUpState.GetNewWing:
					str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
					                     0, 1);
					break;
				default:
					break;
				}

				break;
			case 	 eTaskFinish.petUp:	//宠物升级
				switch ((PetUpState)int.Parse (task.finishParams [0])) {
				case PetUpState.LvUpCount:
					str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
					                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
					break;
				case PetUpState.GetNewPet:
					str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
					                     0, 1);
					break;
				default:
					break;
				}
				break;
			case 	 eTaskFinish.medalUp:	//勋章升级
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     0, 1);
				break;
			case 	 eTaskFinish.addFriend:	//添加好友
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.joinArena: //参加竞技场
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case 	 eTaskFinish.eMoDongKu: //通关恶魔洞窟的层数
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     0, 1);

				break;
			case 	 eTaskFinish.goblin: //哥布林巢穴
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [0]);
				break;
			case	 eTaskFinish.rongLu:    //熔炉
				switch ((RongLuState)int.Parse (task.finishParams [0])) {
				case  RongLuState.SuccessCount:
					str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
					                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
					break;
				case RongLuState.UseCount:
					str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
					                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
					break;
				case RongLuState.Item:
					str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
					                     0, 1);
					break;
				default:
					break;
				}
				break;
			case	 eTaskFinish.chongZhiNum: //充值货币的数量
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.joinGuild:  //加入公会

				break;
			case	 eTaskFinish.jiNengLevel: //技能提升的级别
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.tianFuLevel: //天赋提升的级别
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.worldBoss:  //参与世界boss
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.shop:	//商城购买物品
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;

			default:
				break;
			}
			break;
		case eTaskState.eFinish:
			switch (task.finishType) {
			case eTaskFinish.finishGate:
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
                            ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case eTaskFinish.npcDialog:
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
                            task.finishParams [1], task.finishParams [1]);
				break;
			case eTaskFinish.killEnemy:
				for (int i = 0; i < dess.size; i++) {
					str += string.Format ("{0}  完成({1}/{2})\n", dess [i],
                                ccs.size == 0 ? 0 : ccs [i].Value, cs [i].Value);
				}
				break;
			case eTaskFinish.getBindItem:
				for (int i = 0; i < dess.size; i++) {
					str += string.Format ("{0}  完成({1}/{2})\n", dess [i],
                                ccs.size == 0 ? 0 : ccs [i].Value, cs [i].Value);
				}
				break;
			case eTaskFinish.getItem:
				for (int i = 0; i < dess.size; i++) {
					str += string.Format ("{0}  完成({1}/{2})\n", dess [i],
                                ccs.size == 0 ? 0 : ccs [i].Value, cs [i].Value);
				}
				break; 
			case eTaskFinish.levelUp: //人物等级提升
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     CharacterPlayer.character_property.level, task.finishParams [0]);
				break;
			case  eTaskFinish.vipUp:  //vip等级提升
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     VipManager.Instance.VipLevel, task.finishParams [0]);
				break;
			case 	 eTaskFinish.equipPlus:		//强化装备
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case 	 eTaskFinish.equipAdvance:	//进阶装备
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case 	 eTaskFinish.equipXiLian:	//洗练装备
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.wingUp:	//羽翼升级
				switch ((WingUpState)int.Parse (task.finishParams [0])) {
				case WingUpState.LvUpCount:
					str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
					                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
					break;
				case WingUpState.GetNewWing:
					str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
					                     1, 1);
					break;
				default:
					break;
				}
				break;
			case 	 eTaskFinish.petUp:	//宠物升级
				switch ((PetUpState)int.Parse (task.finishParams [0])) {
				case PetUpState.LvUpCount:
					str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
					                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
					break;
				case PetUpState.GetNewPet:
					str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
					                     1, 1);
					break;
				default:
					break;
				}
				break;
			case 	 eTaskFinish.medalUp:	//勋章升级
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     1, 1);
				break;
			case 	 eTaskFinish.addFriend:	//添加好友
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.joinArena: //参加竞技场
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case 	 eTaskFinish.eMoDongKu: //通关恶魔洞窟的层数
				str = string.Format ("{0}  进行中({1}/{2})\n", task.finish_Des [0],
				                     1, 1);
				break;
			case 	 eTaskFinish.goblin: //哥布林巢穴
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [0]);
				break;
			case	 eTaskFinish.rongLu:    //熔炉
				switch ((RongLuState)int.Parse (task.finishParams [0])) {
				case  RongLuState.SuccessCount:
					str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
					                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
					break;
				case RongLuState.UseCount:
					str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
					                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
					break;
				case RongLuState.Item:
					str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
					                     1, 1);
					break;
				default:
					break;
				}
				break;
			case	 eTaskFinish.chongZhiNum: //充值货币的数量
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.joinGuild:  //加入公会

				break;
			case	 eTaskFinish.jiNengLevel: //技能提升的级别
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.tianFuLevel: //天赋提升的级别
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.worldBoss:  //参与世界boss
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;
			case	 eTaskFinish.shop:	//商城购买物品
				str = string.Format ("{0}  完成({1}/{2})\n", task.finish_Des [0],
				                     ccs.size == 0 ? 0 : ccs [0].Value, task.finishParams [1]);
				break;

			default:
				break;
			}
			break;
		}
		return string.Format ("[d9cfa7]{0}[-]", str); 
	}

	private void OnCloseTaskUI ()
	{
		UIManager.Instance.closeWindow (UiNameConst.ui_task, true);
	}
}
