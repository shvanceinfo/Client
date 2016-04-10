using UnityEngine;
using System.Collections;



public class UIGoldenGoblin : MonoBehaviour {

    public static UIGoldenGoblin sUIGoldenGoblin;

    public int m_nEnterTimes;

    UILabel m_kLabel;

    UILabel m_kEnterLabel;

    public int m_nTodayBuyNum;
    

    void Awake()
    {
        sUIGoldenGoblin = this;
        //m_kLabel = transform.Find("Content/enter_times").GetComponent<UILabel>();
        m_kLabel = transform.Find("middle/right/btn/enter/times").GetComponent<UILabel>();
//		m_kEnterLabel = transform.Find("Content/enter_game/Label").GetComponent<UILabel>();
        m_nEnterTimes = 0;
    }

    void Start()
	{
		
    }

    void OnEnable()
    {
        EventDispatcher.GetInstance().DialogSure += OnDialogSure;
		EventDispatcher.GetInstance().DialogCancel += OnDialogCancel;
        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, PathConst.GOBULIN_UI,
                (asset) =>
                {                    
                    GameObject model = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
                    model.name = PathConst.GOBULIN_UI;
                    ModelPos modelPos = ConfigDataManager.GetInstance().getModelPos().getModelInfo(1001);
                    NPCManager.Instance.ModelCamera.fieldOfView = modelPos.cameraView;
                    model.transform.parent = NPCManager.Instance.ModelCamera.transform;
                    model.transform.localPosition = modelPos.modelPos;
                    model.transform.localScale = Vector3.one;
                    model.transform.localRotation = Quaternion.Euler(modelPos.modelRolate);
                    ToolFunc.SetLayerRecursively(model, LayerMask.NameToLayer("TopUI")); 
                });
        }
        else
        {
            GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.GOBULIN_UI, EBundleType.eBundleUIEffect);
            GameObject model = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
            model.name = PathConst.GOBULIN_UI;
            ModelPos modelPos = ConfigDataManager.GetInstance().getModelPos().getModelInfo(1001);
            NPCManager.Instance.ModelCamera.fieldOfView = modelPos.cameraView;
            model.transform.parent = NPCManager.Instance.ModelCamera.transform;
            model.transform.localPosition = modelPos.modelPos;
            model.transform.localScale = Vector3.one;
            model.transform.localRotation = Quaternion.Euler(modelPos.modelRolate);
            ToolFunc.SetLayerRecursively(model, LayerMask.NameToLayer("TopUI")); 
        }
    }

    void OnDisable()
    {
        EventDispatcher.GetInstance().DialogSure -= OnDialogSure;
		EventDispatcher.GetInstance().DialogCancel -= OnDialogCancel;
        NPCManager.Instance.createCamera(false);
    }

	// Update is called once per frame
	void Update () 
	{
	    
	}


    public void SetInitGoblinData(int nRemainTimes, int nTodayBuy)
    {
        m_nEnterTimes = nRemainTimes;
		
//		if (m_nEnterTimes <= 0)
//        {
//            // 票不够
//            m_kEnterLabel.text = "购买票券";
//        }
//		else
//		{
//			m_kEnterLabel.text = "进入巢穴";
//		}
		
        m_kLabel.text = "可使用";
        m_kLabel.text += nRemainTimes.ToString();
        m_kLabel.text += "次";

        m_nTodayBuyNum = nTodayBuy;
    }


    void OnDialogSure(eDialogSureType type)
    { 
        if (type == eDialogSureType.ePurchaseGoldenGoblinTicket)
        { 
            MessageManager.Instance.SendAskBuyGoldenGoblinTimes();
        }
    }
	
	void OnDialogCancel(eDialogSureType type)
	{ 
		NPCManager.Instance.ModelCamera.transform.FindChild("huangjingebulin_UI").gameObject.SetActive(true);
	}
}
