using UnityEngine;
using System.Collections;

public class TaskFollow : MonoBehaviour
{
	private const string FOLLOW_BTN = "followBtn";
	private UITexture _npcTexture; //NPC的头像
	private UILabel _npcName; //右上角NPC的名字
	private Transform _tagMark; //NPC的标记
	private Task _relateTask; //关联的的任务
	private NPC _relateNPC; //关联的NPC
	
	void Awake ()
	{	
		if (!this.name.Equals (FOLLOW_BTN)) { //右上角的图标才需要设置
			_npcTexture = gameObject.transform.Find ("Background").GetComponent<UITexture> ();
			_npcName = gameObject.transform.Find ("npcName").GetComponent<UILabel> ();
			_tagMark = gameObject.transform.Find ("npcTag");		
			if (NPCManager.Instance.mainTaskNPC != null && TaskManager.Instance.MainTask != null) {
				setNPCIcon (TaskManager.Instance.getTagMark (TaskManager.Instance.MainTask));
			}
            if (TaskManager.Instance.MainTask!=null)
            {
                SetTaskName(TaskManager.Instance.MainTask.taskName);
            }
			_relateTask = TaskManager.Instance.MainTask;
			if (_relateTask != null)
				_relateNPC = _relateTask.getRelateNPC ();
		}
	}
	
	void OnClick ()
	{
		TaskManager.Instance.isTaskFollow = true;
		if (!this.name.Equals (FOLLOW_BTN)) {
			_relateTask = TaskManager.Instance.MainTask;
			TaskManager.Instance.followTask = _relateTask;
			_relateNPC = _relateTask.getRelateNPC ();
		}

		if (SceneManager.Instance.currentScene == SCENE_POS.IN_CITY) {
			if (_relateTask.taskState == eTaskState.eInProgress && _relateTask.finishType != eTaskFinish.npcDialog) {

				if (_relateTask.functionId==0&&_relateTask.mapId!=0) {  //这里是任务逻辑
					UIManager.Instance.closeAllUI (); //关闭当前窗口
					GameObject gateTrigger = GameObject.Find ("Trigger1");
					//PlayerManager.Instance.agent.SetDestination(gateTrigger.transform.position);
					CharacterPlayer.sPlayerMe.GetAI ().m_kPursueState.ResetData ();
					CharacterPlayer.sPlayerMe.GetAI ().m_kPursueState.m_bGotoGate = true;
					CharacterPlayer.sPlayerMe.GetAI ().SendStateMessage (CharacterAI.CHARACTER_STATE.CS_PURSUE, gateTrigger.transform.position);
					//PlayerManager.Instance.pathFind.beginMove(null, true);
				}else if(_relateTask.functionId!=0){   //处理弹页面的逻辑
					TaskManager.Instance.OpenWindowByFollowTaskFunId();
				}
 
			} else {
				UIManager.Instance.closeAllUI (); //关闭当前窗口
				//bool isMainTask = false;
				//if (_relateTask.taskType == eTaskType.mainType)
				//	isMainTask = true;
                //PlayerManager.Instance.agent.SetDestination(moveToPos);
				CharacterPlayer.sPlayerMe.GetAI ().m_kPursueState.ResetData ();
				CharacterPlayer.sPlayerMe.GetAI ().m_kPursueState.m_kPursurNPC = _relateNPC;
			    NPCManager.Instance.pathFollowNPC = _relateNPC;
                findNPCPos(_relateNPC);
			    if (TaskManager.Instance.FolllowPath.size > 0)
			    {
			        Vector3 moveToPos = TaskManager.Instance.FolllowPath.Pop();
                    CharacterPlayer.sPlayerMe.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, moveToPos);
			    }
                //PlayerManager.Instance.pathFind.beginMove(_relateNPC);
				CharacterPlayer.sPlayerMe.GetAI ().m_kPursueState.m_bGotoGate = false;
			}    		
		}
	}

    public void SetTaskName(string name)
    {
        _npcName.text = name; //变成任务名称
    }
	//设置右上角NPC的图标跟名称
	public void setNPCIcon (string tagName)
	{
		if (!this.name.Equals (FOLLOW_BTN)) {
			if (TaskManager.Instance.MainTask.taskState == eTaskState.eInProgress && TaskManager.Instance.MainTask.finishType == eTaskFinish.finishGate)
				_npcTexture.mainTexture = SourceManager.Instance.getTextByIconName (SpriteNameConst.GATE_TAG_ICON, PathConst.NPC_ICON_PATH);
			else
				_npcTexture.mainTexture = SourceManager.Instance.getTextByIconName (NPCManager.Instance.mainTaskNPC.npcIcon, PathConst.NPC_ICON_PATH);
			_npcName.text = TaskManager.Instance.MainTask.taskName; //变成任务名称
			if (tagName != null) {
				
				#region 改为图片的效果
				UISprite tagSprite = this._tagMark.GetComponent<UISprite> ();
				switch (tagName) {
				case NPCManager.SIMBOL_ACCENT_FAIL:
					tagSprite.spriteName = Constant.SIMBOL_ACCENT_TANHAO_AN;
					break;
				case NPCManager.SIMBOL_ACCENT:
					tagSprite.spriteName = Constant.SIMBOL_ACCENT_TANHAO_LIANG;
					break;
				case NPCManager.SIMBOL_PROCESSING:
					tagSprite.spriteName = Constant.SIMBOL_PROCESSING_WENHAO_AN;
					break;
				case NPCManager.SIMBOL_COMPLETE:
					tagSprite.spriteName = Constant.SIMBOL_COMPLETE_WENHAO_LIANG;
					break;
				default:
					break;
				}
				#endregion
				
				
 	
 #region 效果改为图片了
//				Transform origTrans = getOrigTrans ();
//				if (tagName != null && origTrans != null) {
//					
//					string resPath = PathConst.NPC_TAG_PATH + tagName;
//					GameObject tag = GameObject.Instantiate (BundleMemManager.Instance.loadResource (resPath, typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
//					foreach (Transform child in tag.transform) {
//						NcBillboard billBoard = child.GetComponent<NcBillboard> ();
//						if (billBoard != null)
//							billBoard.enabled = false;
//						if (child.name.IndexOf ("ring") == -1)
//							child.localEulerAngles = Vector3.zero;
//						else
//							child.localEulerAngles = new Vector3 (90f, 180f, 0f);
//					}
//					tag.name = tagName;
//					tag.transform.parent = _tagMark;
//					tag.transform.localScale = origTrans.localScale;
//					tag.transform.localPosition = origTrans.localPosition;
//					tag.transform.localEulerAngles = origTrans.localEulerAngles;
//					ToolFunc.SetLayerRecursively (tag, LayerMask.NameToLayer ("UI"));
//					foreach (Transform child in _tagMark) {
//						if (child.gameObject != tag)
//							GameObject.Destroy (child.gameObject);//清除原来的标记
//					}
//					//GameObject.Destroy(origTrans.gameObject); 
//				}
 #endregion

			}
		}
	}

    //查找NPC位置
    private void findNPCPos(NPC npc)
    {

        int mapID = CharacterPlayer.character_property.getServerMapID();
        findPath(mapID, npc);
    }

    //查找到达的道路
    private bool findPath(int mapID, NPC npc)
    {
        MapDataItem mapData = ConfigDataManager.GetInstance().getMapConfig().getMapData(mapID); //NPC就在当前地图
        if (npc.mapID == mapID)
        {
            TaskManager.Instance.FolllowPath.Add(npc.pathLocatePos);
            return true;
        }
        else
        {
            for (int i = 0; i < mapData.transferMapIDs.Length; i++)
            {
                if (mapData.transferMapIDs[i] != 0 && mapData.transferPos[i] != Vector3.zero)
                {
                    int nextID = mapData.transferMapIDs[i];
                    if (findPath(nextID, npc))
                    {
                        TaskManager.Instance.FolllowPath.Add(mapData.transferPos[i]);
                        return true; //找到一条直达的路径，不再寻找
                    }
                }
            }
            return false; //遍历完都没有就是没有直达路径
        }
    }

    //获取原来的Transform
    private Transform getOrigTrans ()
	{
		foreach (Transform child in _tagMark) {
			return child;
		}
		return null;
	}
	
	//getter and setter
	public Task RelateTask {
		set { 
			_relateTask = value; 
			_relateNPC = _relateTask.getRelateNPC ();
			TaskManager.Instance.followTask = value;
		}
	}
}
