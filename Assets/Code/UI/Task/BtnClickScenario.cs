using UnityEngine;
using System.Collections;

public class BtnClickScenario : MonoBehaviour 
{
	const int PLAYER_POS_Y = -80;
	const int PLAYER_SCALE_X = 640;
	const int PLAYER_SCALE_Y = 500;
	
	private GameObject _blackScreen;
	private GameObject _dialog;
	private Transform _left;	//左边的区域
	private Transform _right;
	private UILabel _npcDialog; //对话的Label
	private float _calTime; //计时使用
	private float _delayTime;
	private Hashtable _modelHash; //模型的集合
	
	private Scenario _scenario;
	
	void Awake()
	{
		_blackScreen = transform.parent.Find("blackScreen").gameObject;
		_dialog = transform.parent.Find("scenario").gameObject;
		_left = _dialog.transform.Find("left");
		_right = _dialog.transform.Find("right");
		_npcDialog = _dialog.transform.Find("dialog").GetComponent<UILabel>();
		_blackScreen.SetActive(false);
		_dialog.SetActive(false);
		_delayTime = 2f;
		_modelHash = new Hashtable();
	}
	
	void OnClick()
	{
		_calTime = 0f;
		if(!_scenario.playScenarioOver())
			setDiaLog(_scenario.getSubScenario());
		else
		{
			showModel(true);
			ScenarioManager.Instance.OnScenarioOver();
			_scenario.submitServer();
			UIManager.Instance.closeAllUI();
//			NPCManager.Instance.openNPC.changeNPCLayer(false);  //重置NPC到场景中
		}
	}
	
	void Update()
	{
		_calTime += Time.deltaTime;
		if(_calTime >= _delayTime)
			OnClick();
	}
	
	//设置对话框内容
	private void setDiaLog(SubScenario subScenario)
	{
		UILabel contentLbl;
		string content = subScenario.content;
		_delayTime = (float)subScenario.delaySecond;
		if(content.IndexOf(Constant.PLAYER_NAME) != -1)
			content = content.Replace(Constant.PLAYER_NAME, CharacterPlayer.character_property.nick_name);
		if(subScenario.display == eDisplayType.blackSceen) //黑屏操作
		{
			_blackScreen.SetActive(true);
			_dialog.SetActive(false);
			contentLbl = _blackScreen.transform.Find("dialog").GetComponent<UILabel>();
		}
		else
		{
			_blackScreen.SetActive(false);
			_dialog.SetActive(true);
			contentLbl = _npcDialog;
			UILabel nameLbl;
			Transform model;
			if(subScenario.iconLeft)
			{
				_left.gameObject.SetActive(true);
				_right.gameObject.SetActive(false);
				model = _left.Find("model");
				nameLbl = _left.Find("nameLbl").GetComponent<UILabel>();
			}
			else
			{
				_left.gameObject.SetActive(false);
				_right.gameObject.SetActive(true);
				model = _right.Find("model");
				nameLbl = _right.Find("nameLbl").GetComponent<UILabel>();
			}
			if(subScenario.iconID > 0)
			{
				NPC npc = NPCManager.Instance.getNPCByID(subScenario.iconID);
				if(npc != null)
				{
					nameLbl.text = npc.npcName;
					showModel(false, npc.npcID, npc.npcModel, model);
				}
			}
			else
			{
				nameLbl.text = CharacterPlayer.character_property.nick_name;
				string resName = "";
				switch (CharacterPlayer.character_property.career) {
					case CHARACTER_CAREER.CC_SWORD:
						resName = Constant.SWORD_UI;
						break;
					case CHARACTER_CAREER.CC_ARCHER:
						resName = Constant.BOW_UI;
						break;
					case CHARACTER_CAREER.CC_MAGICIAN:
						resName = Constant.RABBI_UI;
						break;
					default:
						break;
				}
				showModel(false, 0, resName, model);
			}
		}
		contentLbl.text = content;
	}
	
	//在剧情面板显示模型
	private void showModel(bool isDestroy, uint npcID = 0, string modelName=null, Transform parentTrans=null)
	{
		NPCManager.Instance.createCamera ();

		if(isDestroy) //删除
		{
			if(_modelHash.Count > 0)  //播放完剧情要删除全部模型
			{
				foreach (GameObject obj in _modelHash.Values) 
					Destroy(obj);
				_modelHash.Clear();
			}
		}
		else if(modelName != null) //显示
		{
			if(_modelHash.Count > 0) //hidden all model
			{
				foreach (GameObject obj in _modelHash.Values) 
					obj.SetActive(false);
			}
			if(_modelHash.Contains(modelName))
			{
				GameObject model = _modelHash[modelName] as GameObject;
				model.transform.parent = parentTrans;
				model.SetActive(true);
			}
			else
			{
			    if (npcID == 0) //是人物模型
			    {
                    if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleWeaponEffect))
                    {
                        BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleWeaponEffect, modelName,
                            (asset) =>
                            {
                                createNPCModel(asset, npcID, modelName);
                            }, CharacterPlayer.character_property.career);
                    }
                    else
                    {
                        GameObject asset = BundleMemManager.Instance.getPrefabByName(modelName, EBundleType.eBundleWeaponEffect);
                        createNPCModel(asset, npcID, modelName);
                    }				
			    }
			    else
			    {
					if (Global.inCityMap())
                    {
						if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleNPC))
                        {
                            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleNPC, modelName,
                                (asset) =>
                                {
                                    createNPCModel(asset, npcID, modelName);
                                });
                        }
                        else
                        {
                            GameObject asset = BundleMemManager.Instance.getPrefabByName(modelName, EBundleType.eBundleNPC);
                            createNPCModel(asset, npcID, modelName);
                        }
                    }
                    else
                    {
                        GameObject asset = BundleMemManager.Instance.getPrefabByName(modelName, EBundleType.eBundleMonster);
                        createNPCModel(asset, npcID, modelName);
                    }				
			    }
			}
		}
	}

    //根据Model名称创建NPC model
    private void createNPCModel(Object asset, uint npcID, string modelName)
    {              
        GameObject model = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
        ToolFunc.SetLayerRecursively(model, LayerMask.NameToLayer("TopUI"));
        NPCManager.Instance.ModelCamera.orthographic = true;
        NPCManager.Instance.ModelCamera.nearClipPlane = -3;
        NPCManager.Instance.ModelCamera.orthographicSize = 1;
        model.transform.parent = NPCManager.Instance.ModelCamera.transform;
        //setRender(model);
        if (npcID == 0)
        {
            ModelPos modelPos = ConfigDataManager.GetInstance().getModelPos().getModelInfo((int)CharacterPlayer.character_property.career);
            //model.transform.localPosition = new Vector3(0f, PLAYER_POS_Y, 0f);
            //model.transform.localScale = new Vector3(PLAYER_SCALE_X, PLAYER_SCALE_Y, 10f);
            //model.transform.localEulerAngles = Vector3.zero;
            model.transform.localPosition = modelPos.scenarioPos;
            model.transform.localEulerAngles = modelPos.scenarioRotate;
            model.transform.localScale = modelPos.scenarioScale;
        }
        else
        {
            Transform nameMark = model.transform.Find("name_mark"); //名称标记
            if (nameMark != null)
                Destroy(nameMark.gameObject);
            model.transform.localEulerAngles = NPCManager.Instance.getNPCByID(npcID).scenarioRotate;
            model.transform.localPosition = NPCManager.Instance.getNPCByID(npcID).scenarioPos;
            model.transform.localScale = NPCManager.Instance.getNPCByID(npcID).scenarioScale;
        }
        model.SetActive(true);
        _modelHash.Add(modelName, model);
    }


	//获取skinned 的渲染
	private void setRender(GameObject go)
	{
		foreach (Transform child in go.transform) 
		{
			if(child.GetComponent<SkinnedMeshRenderer>() != null)
			{
				SkinnedMeshRenderer render = child.GetComponent<SkinnedMeshRenderer>();
				render.material.renderQueue = Constant.ABOVE_SHADER_LEVEL; 
				render.material.shader = Shader.Find("Transparent/Cutout/Diffuse");
			}
			else if(child.GetComponent<MeshRenderer>() != null)
			{
				MeshRenderer render = child.GetComponent<MeshRenderer>();
				render.material.renderQueue = Constant.ABOVE_SHADER_LEVEL; 
				render.material.shader = Shader.Find("Transparent/Cutout/Diffuse");
			}
			else if(child.childCount > 0)
				setRender(child.gameObject);
		}
	}
	
	//getter and setter
	public Scenario Scenario
	{
		set 
		{ 
			_scenario = value; 
			OnClick();
		}
	}
}
