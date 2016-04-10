using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using helper;

public class HUD : MonoBehaviour
{
	public enum HUD_CHARACTER_TYPE
	{
		HCT_PLAYER,         // 人物头顶
		HCT_MONSTER,        // 怪物头顶
		HCT_NPC,                // NPC头顶
	}

	public static BetterList<GameObject> nameObjs = new BetterList<GameObject> ();
	public GameObject headPanel;
	public Object m_kPrefab;
	public Character m_kOwner;
	public string m_StrPlayerName;
	public string m_StrMonsterName;
	public int m_nTitleID;
	public bool m_bCreate = true;

    private GameObject resourcesIcon;
    public GameObject insObj;

	void Awake ()
	{
		m_kPrefab = BundleMemManager.Instance.getPrefabByName (PathConst.HEAD_BOARD_PREFAB_PATH, EBundleType.eBundleUI);  
		m_kOwner = GetComponent<Character> ();

	}

	// Use this for initialization
	void Start ()
	{

	}

	void OnEnable ()
	{
		if (m_kOwner.getType () != CharacterType.CT_MONSTER) {
			EventDispatcher.GetInstance ().PlayerProperty += OnPlayerProperty;
		}
	}

	void OnDisable ()
	{
		if (m_kOwner.getType () != CharacterType.CT_MONSTER) {
			EventDispatcher.GetInstance ().PlayerProperty -= OnPlayerProperty;
		}
	}

	void OnPlayerProperty ()
	{
		if (headPanel) {
			HealthBar hb = headPanel.transform.FindChild ("Panel/ui_hp_bar").GetComponent<HealthBar> ();
			hb.MaxValue = m_kOwner.GetProperty ().getHPMax ();
			hb.Value = m_kOwner.GetProperty ().getHP ();
		}
	}

	public void ChangeHPShow ()
	{
		if (headPanel) {
			HealthBar hb = headPanel.transform.FindChild ("Panel/ui_hp_bar").GetComponent<HealthBar> ();
			hb.MaxValue = m_kOwner.GetProperty ().getHPMax ();
			hb.Value = m_kOwner.GetProperty ().getHP ();

			if (hb.Value == 0) {
				RemoveHeadUI ();
			}
            
		}
	}
    
	void LateUpdate ()
	{
		if (Camera.main) {
			if (headPanel != null) {
				Transform mainUI = UIManager.Instance.getRootTrans ().FindChild ("Camera/ui_main");

				if (((mainUI && mainUI.gameObject.activeSelf && Global.inCityMap ()) ||
					Global.inFightMap () ||
					Global.inTowerMap () ||
					Global.inGoldenGoblin () ||
					Global.InArena () ||
					Global.inMultiFightMap () ||
					Global.InWorldBossMap () ||
					Global.InAwardMap ()
                    ) &&
					!CharacterAI.IsInState (m_kOwner, CharacterAI.CHARACTER_STATE.CS_BORN)) {
					headPanel.SetActive (true);


					headPanel.transform.localScale = Vector3.one;

					Vector3 uiPot = Camera.main.WorldToScreenPoint (new Vector3 (m_kOwner.transform.position.x,
                        m_kOwner.transform.position.y + m_kOwner.GetComponent<CapsuleCollider> ().height,
                        m_kOwner.transform.position.z));

					//Debug.Log(" 相机坐标 " + uiPot + "  屏幕 " + Screen.width + " " + Screen.height);
					float fWidhtRatio = uiPot.x / (float)Screen.width;
					float fHeightRatio = uiPot.y / (float)Screen.height;

					float fRatio = (float)(Screen.width) / (Screen.height);

					if (fRatio > 1.0f) {
						float bench = Screen.width / (float)Screen.height * 320.0f;
						float diff = (float)uiPot.x - ((float)Screen.width * 0.5f);
						float ratio = bench / ((float)Screen.width * 0.5f);
						float x = ratio * diff;


						//if (uiPot.y > Screen.height)
						//{
						//    headPanel.SetActive(false);
						//}
						//else
						//{
						//    headPanel.SetActive(true);
						//}


						float diffy = (float)uiPot.y - ((float)Screen.height * 0.5f);
						float ratioy = 320.0f / ((float)Screen.height * 0.5f);
						float y = ratioy * diffy;

						headPanel.transform.localPosition = new Vector3 (x, y, 0.0f);
					} else {
						float benchy = Screen.height / (float)Screen.width * 480.0f;
						float diffy = (float)uiPot.y - ((float)Screen.height * 0.5f);
						float ratioy = benchy / ((float)Screen.height * 0.5f);
						float y = ratioy * diffy;


						//if (uiPot.y > Screen.height)
						//{
						//    headPanel.SetActive(false);
						//}
						//else
						//{
						//    headPanel.SetActive(true);
						//}


						float diffx = (float)uiPot.x - ((float)Screen.width * 0.5f);
						float ratiox = 480.0f / ((float)Screen.width * 0.5f);
						float x = ratiox * diffx;

						headPanel.transform.localPosition = new Vector3 (x, y, 0.0f);
					}
				} else
					headPanel.SetActive (false);


			}
		}
            
	}

	public void RemoveHeadUI ()
	{
		if (UIManager.Instance.getRootTrans () == null) {
			return;
		}

		Transform tran = UIManager.Instance.getRootTrans ().FindChild ("Camera");

		Transform[] headBoards = tran.GetComponentsInChildren<Transform> (true);

		foreach (Transform borad in headBoards) {
			UIHeadBoard hb = borad.GetComponent<UIHeadBoard> ();

			if (hb) {
				int nJudgeId = 0;

				if (m_kOwner.getType () == CharacterType.CT_PLAYER) {
					nJudgeId = -1;
				} else
					nJudgeId = m_kOwner.GetProperty ().GetInstanceID ();

				if (hb.m_nInstanceId == nJudgeId) {
					borad.parent = null;
					nameObjs.Remove (borad.gameObject);
					Destroy (borad.gameObject);
					break;
				}
			}
		}
	}

	public void GenerateHeadUI (string name, int titleid, HUD_CHARACTER_TYPE hctType, bool isfirstLoad = true, bool isSelf = true, int id = 0)
	{
		if (UIManager.Instance.getRootTrans ().FindChild ("Camera")) {
           
			RemoveHeadUI ();



			headPanel = BundleMemManager.Instance.instantiateObj (m_kPrefab);
			headPanel.name = m_kOwner.GetProperty ().GetInstanceID ().ToString ();

			nameObjs.Add (headPanel);
			headPanel.transform.parent = UIManager.Instance.getRootTrans ().FindChild ("Camera");
			headPanel.transform.localPosition = Vector3.zero;
			headPanel.transform.localScale = Vector3.one;

			UIHeadBoard headboardSc = headPanel.GetComponent<UIHeadBoard> ();

			if (m_kOwner.getType () == CharacterType.CT_PLAYEROTHER) {
				if (MedalManager.Instance.MedalHash.ContainsKey (id)) {
					headPanel.transform.FindChild ("title").gameObject.SetActive (true);
				} else {
					headPanel.transform.FindChild ("title").gameObject.SetActive (false);
				}
				if (Global.InArena ()) {
					headPanel.transform.FindChild ("Panel/ui_hp_bar").active = true;
				} else {
					headPanel.transform.FindChild ("Panel/ui_hp_bar").active = false;
				}
			}

			if (m_kOwner.getType () == CharacterType.CT_MONSTER) {
				headPanel.transform.FindChild ("title").gameObject.SetActive (false);
				if (!Global.inGoldenGoblin ()) {
					CharacterMonster monster = m_kOwner as CharacterMonster;

					if (monster && monster.monster_property.getType () != MonsterProperty.MONSTER_LEVEL_TYPE.MLT_BOSS) {
						headPanel.transform.FindChild ("Panel/ui_hp_bar").active = true;
					} else
						headPanel.transform.FindChild ("Panel/ui_hp_bar").active = false;
				} else
					headPanel.transform.FindChild ("Panel/ui_hp_bar").active = false;  
			}

				
			if (m_kOwner.getType () == CharacterType.CT_PLAYER) {
				if (FastOpenManager.Instance [FunctionName.Medal]) {
					headPanel.transform.FindChild ("title").gameObject.SetActive (true);
				}
				headboardSc.m_nInstanceId = -1;

				if (Global.InArena ()) {
					headPanel.transform.FindChild ("Panel/ui_hp_bar").active = true;
				} else {
					headPanel.transform.FindChild ("Panel/ui_hp_bar").active = false;
				}	
			} else
				headboardSc.m_nInstanceId = m_kOwner.GetProperty ().GetInstanceID ();
            
            SetHeadUIData(name, titleid, hctType, isfirstLoad, isSelf, id);


			//headPanel.transform.FindChild("Panel/ui_hp_bar").localPosition = new Vector3(headPanel.transform.FindChild("ui_hp_bar").localPosition.x,
			//    headPanel.transform.FindChild("Panel/ui_hp_bar").localPosition.y,
			//    0.0f);
			switch (m_kOwner.getType ()) {
			case CharacterType.CT_NULL:
				break;
			case CharacterType.CT_PLAYER:
				if (FastOpenManager.Instance [FunctionName.Medal]) {
					if (MedalManager.Instance.CurVo != null) {
						headPanel.transform.FindChild ("title").GetComponent<UITexture> ().mainTexture =
                       SourceManager.Instance.getTextByIconName (MedalManager.Instance.CurVo.ChengHao, PathConst.MEDAL_ICON_PATH);
					}
				}
				break;
			case CharacterType.CT_MONSTER:
				break;
			case CharacterType.CT_PLAYERUI:
				break;
			case CharacterType.CT_PLAYEROTHER:
				if (id != 0) {
					headPanel.transform.FindChild ("title").GetComponent<UITexture> ().mainTexture =
                    SourceManager.Instance.getTextByIconName (MedalManager.Instance.FindVoByID (id).ChengHao, PathConst.MEDAL_ICON_PATH);
				}
                    
				break;
			default:
				break;
			}
			headPanel.SetActive (false);
		}
        
	}

	public void SetHeadUIData (string name, int titleid, HUD_CHARACTER_TYPE hctType, bool isfirstLoad=true, bool isSelf=true, int id=0)
	{
		if (!headPanel) {
			return;
		}
		if (hctType == HUD_CHARACTER_TYPE.HCT_PLAYER) {
			headPanel.transform.FindChild ("iconPanel/title").gameObject.SetActive (true);
			headPanel.transform.FindChild ("player_name").gameObject.SetActive (true);
            if (Global.inGoldenGoblin()) 
            {
                //BundleMemManager.Instance.loadResource(PathConst.GOBULIN_BUFF);
                //UIManager.Instance.openWindow(UiNameConst.ui_head_goblin_buff);
                headPanel.transform.FindChild("player_name").GetComponent<UIWidget>().pivot = UIWidget.Pivot.Left;
                resourcesIcon = BundleMemManager.Instance.loadResource(PathConst.GOBULIN_BUFF) as GameObject;
                //InsGoblinIcon();
            }
            else
            {
                //UIManager.Instance.closeWindow(UiNameConst.ui_head_goblin_buff);
                if (insObj != null)
                {
                    Destroy(insObj);
                }
                headPanel.transform.FindChild("player_name").GetComponent<UIWidget>().pivot = UIWidget.Pivot.Center;
            }
			if (isfirstLoad) {
				UILabel ll = headPanel.transform.FindChild ("player_name").GetComponent<UILabel> ();
				ll.transform.localPosition = new Vector3 (ll.transform.localPosition.x - ll.relativeSize.x * 2, 
            ll.transform.localPosition.y, ll.transform.localPosition.z);
			}
			headPanel.transform.FindChild ("monster_name").gameObject.SetActive (false);

            
            
			if (titleid != 0) {
             
				headPanel.transform.FindChild ("iconPanel/title/icon").GetComponent<UISprite> ().gameObject.SetActive (true);
				headPanel.transform.FindChild ("iconPanel/title/icon_bg").GetComponent<UISprite> ().gameObject.SetActive (true);

				//obj.transform.FindChild("head_board/title/icon_bg").Translate(0.0f, 0.0f, -1.0f, Space.World);
				headPanel.transform.Find ("title_text").GetComponent<UILabel> ().gameObject.SetActive (false);
                
			} else {
				headPanel.transform.FindChild ("iconPanel/title/icon").GetComponent<UISprite> ().gameObject.SetActive (false);
				headPanel.transform.FindChild ("iconPanel/title/icon_bg").GetComponent<UISprite> ().gameObject.SetActive (false);
				headPanel.transform.Find ("title_text").GetComponent<UILabel> ().gameObject.SetActive (false);
			}

            string myname = "";
            if (CharacterPlayer.character_property.getNickName().Equals(name))
            {
                headPanel.transform.Find("player_name").GetComponent<UIWidget>().color = Color.white;
                myname = string.Format("[{0}]{1}[-]", ColorConst.Color_Green, name);
            }
            else
            {
                myname = string.Format("[{0}]{1}[-]", ColorConst.Color_LiangHuang, name);
            }

            headPanel.transform.Find("player_name").GetComponent<UILabel>().text = myname;
		} else {
//			headPanel.transform.FindChild ("title").gameObject.SetActive (false);
			headPanel.transform.FindChild ("iconPanel/title").gameObject.SetActive (false);
			headPanel.transform.FindChild ("player_name").gameObject.SetActive (false);
			headPanel.transform.Find ("title_text").GetComponent<UILabel> ().gameObject.SetActive (false);

			headPanel.transform.FindChild ("monster_name").gameObject.SetActive (true);

			headPanel.transform.Find ("monster_name").GetComponent<UILabel> ().text = name;

            
		}
	}

    public void InsGoblinIcon()
    {
        insObj = BundleMemManager.Instance.instantiateObj(resourcesIcon) as GameObject;
        insObj.transform.parent = UIManager.Instance.getRootTrans().FindChild("Camera");
        insObj.transform.localScale = Vector3.one;
        insObj.transform.position = headPanel.transform.FindChild("player_name").position + new Vector3(-0.05f, 0f, 0);
    }

    public void ShowGoblinIcon(string iconName)
    {
        if (insObj != null)
        {
            insObj.transform.FindChild("title/icon").GetComponent<UISprite>().spriteName = iconName;
            Destroy(insObj.gameObject,15);
        }
    }


	void OnDestroy ()
	{
		if (headPanel != null) {
			headPanel.transform.parent = null;
			nameObjs.Remove (headPanel);
			Destroy (headPanel);
		}
	}

	// hide hud
	public void HideHUD()
	{
		if (UIManager.Instance.getRootTrans () == null) {
			return;
		}
		
		Transform tran = UIManager.Instance.getRootTrans ().FindChild ("Camera");
		
		Transform[] headBoards = tran.GetComponentsInChildren<Transform> (true);
		
		foreach (Transform borad in headBoards) {
			UIHeadBoard hb = borad.GetComponent<UIHeadBoard> ();
			
			if (hb)
			{
				int nJudgeId = 0;
				
				if (m_kOwner.getType () == CharacterType.CT_PLAYER)
				{
					nJudgeId = -1;
				}
				else
					nJudgeId = m_kOwner.GetProperty ().GetInstanceID ();
				
				if (hb.m_nInstanceId == nJudgeId) 
				{
					hb.GetComponent<UIPanel>().enabled = false;
					break;
				}
			}
		}
	}

	public void ShowHUD()
	{
		if (UIManager.Instance.getRootTrans () == null) {
			return;
		}
		
		Transform tran = UIManager.Instance.getRootTrans ().FindChild ("Camera");
		
		Transform[] headBoards = tran.GetComponentsInChildren<Transform> (true);
		
		foreach (Transform borad in headBoards) {
			UIHeadBoard hb = borad.GetComponent<UIHeadBoard> ();
			
			if (hb)
			{
				int nJudgeId = 0;
				
				if (m_kOwner.getType () == CharacterType.CT_PLAYER)
				{
					nJudgeId = -1;
				}
				else
					nJudgeId = m_kOwner.GetProperty ().GetInstanceID ();
				
				if (hb.m_nInstanceId == nJudgeId) 
				{
					hb.GetComponent<UIPanel>().enabled = true;
					break;
				}
			}
		}
	}
}

