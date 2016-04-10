using UnityEngine;
using System.Collections;
using helper;

public class NPC
{
	public uint npcID;
	public string npcName;
	public string npcIcon;
	public string npcModel;
	public uint mapID;
	public Vector3 npcPos;
	public Vector3 rotatePosEuler;
	public string defaultWord;
	public Vector3 uiPos;
	public Vector3 uiRotateEuler;
    public Vector3 uiScale;
	public Vector3 scenarioScale; //剧情缩放
    public Vector3 scenarioPos;     //剧情位置
    public Vector3 scenarioRotate;     //剧情旋转
	public float caremeView;
	public Vector3 pathLocatePos; //自动寻路的位置
	
	private GameObject _sceneNPC;  //在场景中的组件，实例化的时候生成
	private int _origLayer; //原来NPC的相关信息
	private Vector3 _origPos; //在场景的原始位置
	private Quaternion _origRotate;
	private Vector3 _origScale;
	private Transform _origParent;	
	
	private GameObject _markNameRoot; //挂接名称跟任务指引
	private string _taskStateTag; //当前任务的标记
	private GameObject _taskTagObj; //任务标记的对象
	private Vector3 _tagPos; //标记的位置
	private Vector3 _tagScale;
	private float _particleSize; //例子大小
	private Vector3 _faceDir; //人物的朝向
	private NPCAction _action; //NPC的行为

    Transform _modelRoot;
    GameObject obj;
    private void Awake()
    {
        //_modelRoot = transform.FindChild("ui_task_dialog/Background/empty");
        //obj = Instantiate(_modelRoot.gameObject) as GameObject;
        //obj.transform.parent = _modelRoot;
        //obj.transform.localScale = Vector3.one;
        //obj.transform.localPosition = Vector3.zero;
        //obj.transform.name = "pos";
    }



	//实例化自身
	public GameObject instanciateSelf(Object asset)
	{
		if(_sceneNPC == null)  //如果没有初始过NPC
		{		    
		    if (asset == null)
		        return null;
            _sceneNPC = BundleMemManager.Instance.instantiateObj(asset, npcPos, Quaternion.identity);
			_sceneNPC.transform.localEulerAngles = rotatePosEuler;
			NPCAction action = _sceneNPC.AddComponent<NPCAction>();
			action.OwnerNPC = this;
			_action = action;
			_sceneNPC.name = npcName;
			_faceDir = npcPos - pathLocatePos;
			_faceDir.Normalize();
			_markNameRoot = _sceneNPC.transform.Find("name_mark").gameObject;
			if(_markNameRoot != null)
			{
				_markNameRoot.AddComponent<BillBoard>();
				UILabel nameLabel = _markNameRoot.GetComponentInChildren<UILabel>();
				nameLabel.text = npcName;
				#region Ngui3.x使用
				nameLabel.fontSize = 29;
				_markNameRoot.transform.FindChild("Label").localScale = new Vector3(0.006f,0.006f,0.006f);
				nameLabel.overflowMethod = UILabel.Overflow.ResizeFreely;
				#endregion
				_taskStateTag = NPCManager.SIMBOL_ACCENT;
				_taskTagObj = _markNameRoot.transform.Find(_taskStateTag).gameObject;
				_tagPos = _taskTagObj.transform.localPosition;
				_tagScale = _taskTagObj.transform.localScale;
				ParticleSystem particle = _taskTagObj.GetComponentInChildren<ParticleSystem>();
				if(particle != null)
					_particleSize = particle.startSize;
				else
					_particleSize = 2f;		
				tagTaskState(null);
			}
		}
		return _sceneNPC;
	}
	
	//任务从场景到UI还是从UI到场景
	public void changeNPCLayer(bool sceneToUI)
	{
		if(sceneToUI)
		{
			NPCManager.Instance.openNPC = this;
			_origLayer = _sceneNPC.layer;
			_origPos = _sceneNPC.transform.localPosition;
			_origRotate = _sceneNPC.transform.localRotation;
			_origScale = _sceneNPC.transform.localScale;
			_origParent = _sceneNPC.transform.parent;
            ToolFunc.SetLayerRecursively(_sceneNPC, LayerMask.NameToLayer("TopUI"));

			NPCManager.Instance.createCamera(false);
            _sceneNPC.transform.parent = NPCManager.Instance.ModelCamera.transform;
            NPCManager.Instance.ModelCamera.orthographic = false;
            NPCManager.Instance.ModelCamera.fieldOfView = caremeView;

            _modelRoot = UIManager.Instance.getRootTrans().FindChild("Camera/ui_task_dialog/Background/empty");
            _modelRoot.localPosition = _modelRoot.localPosition + uiPos;


            //if (!UIManager.Instance.getRootTrans().FindChild("Camera").gameObject.GetComponent<NPC>())
            //    UIManager.Instance.getRootTrans().FindChild("Camera").gameObject.AddComponent("NPC");


            Vector3 pos = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                NPCManager.Instance.ModelCamera, _modelRoot.position);

            _modelRoot.localPosition = _modelRoot.localPosition - uiPos;

            //_sceneNPC.transform.localPosition = uiPos; 

            _sceneNPC.transform.position = pos;
            _sceneNPC.transform.localEulerAngles = uiRotateEuler;
            _sceneNPC.transform.localScale = uiScale;


			if(_taskStateTag != null)
				_taskTagObj = _markNameRoot.transform.Find(_taskStateTag).gameObject;
			else
				_taskTagObj = null;
			_markNameRoot.SetActive(false); //到UI不显示名字
		}
		else
		{
			ToolFunc.SetLayerRecursively(_sceneNPC, _origLayer);
			_sceneNPC.transform.parent = _origParent;
			_sceneNPC.transform.localPosition = _origPos;
			_sceneNPC.transform.localRotation = _origRotate;
			_sceneNPC.transform.localScale = _origScale;
			_markNameRoot.SetActive(true);
			NPCManager.Instance.createCamera(false); //清除相机
		}
	}
	
	//打上任务标记
	public void tagTaskState(string newTag)
	{
		if(_taskStateTag != newTag)
		{
			if(_taskTagObj != null)
				GameObject.Destroy(_taskTagObj); //清除原有的标记
			_taskStateTag = newTag;
			if(_taskStateTag != null)
			{
                if (_markNameRoot!=null)
                if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
                {
                    BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleTaskEffect, PathConst.NPC_TAG_PATH + _taskStateTag,
                        (asset) =>
                        {                   
                            GameObject tag = BundleMemManager.Instance.instantiateObj(asset, _tagPos, Quaternion.identity);
                            tag.name = _taskStateTag;
                            tag.transform.parent = _markNameRoot.transform;
                            tag.transform.localScale = _tagScale;
                            tag.transform.localPosition = _tagPos;
                        });
                }
                else
                {
					GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.NPC_TAG_PATH + _taskStateTag, EBundleType.eBundleTaskEffect);
                    GameObject tag = BundleMemManager.Instance.instantiateObj(asset, _tagPos, Quaternion.identity);
                    tag.name = _taskStateTag;
                    tag.transform.parent = _markNameRoot.transform;
                    tag.transform.localScale = _tagScale;
                    tag.transform.localPosition = _tagPos;
                }  
			}
		}
	}

    private void Update()
    {
        //if (NPCManager.Instance.ModelCamera.transform.childCount > 0)
        //{

        //    _modelRoot = transform.FindChild("ui_task_dialog/Background/empty");

        //    Vector3 v3 = transform.FindChild("ui_task_dialog/Background/empty/pos").localPosition;
        //    _modelRoot.localPosition = _modelRoot.localPosition + v3;
        //    Vector3 pos = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
        //        NPCManager.Instance.ModelCamera, _modelRoot.position);
        //    _modelRoot.localPosition = _modelRoot.localPosition - v3;
        //    GameObject model = NPCManager.Instance.ModelCamera.transform.GetChild(0).gameObject;
        //    model.transform.position = pos;
        //}
    }

	//getter and setter
	public Vector3 FaceDir
	{
		get { return _faceDir; }
	}

	public NPCAction Action
	{
		get { return _action; }
	}	
}
