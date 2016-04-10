using UnityEngine;


public class TransferPointHUD : MonoBehaviour 
{
    public GameObject headPanel;
    public Object m_kPrefab;

    public string headBoardName;
    public string backgroundName;

    public bool m_bVisible = true;

    void Awake()
    {
         m_kPrefab = BundleMemManager.Instance.getPrefabByName(PathConst.TRANSFER_POINT_HEAD_BOARD_PREFAB_PATH, EBundleType.eBundleUI);
        GenerateHeadUI(headBoardName);
    }

	// Use this for initialization
	void Start () 
    {

	}

    void OnEnable()
    {
        EventDispatcher.GetInstance().HUDNeedHideShow += OnHUDNeedHideShow;
    }

    void OnDisable()
    {
        EventDispatcher.GetInstance().HUDNeedHideShow -= OnHUDNeedHideShow;
    }

    void OnHUDNeedHideShow(bool bVisible)
    {
        m_bVisible = bVisible;
    }
    
    void LateUpdate()
    {
        if (Camera.main)
        {
            if (headPanel != null)
            {
            	Transform mainUI = UIManager.Instance.getRootTrans().FindChild("Camera/ui_main");

                bool bJoyStickVisible = EasyTouchJoyStickProperty.sJoystickProperty.gameObject.activeSelf;

                if (mainUI && mainUI.gameObject.activeSelf && Global.inCityMap() && m_bVisible && bJoyStickVisible)
                {
                    headPanel.SetActive(true);
                    headPanel.transform.localScale = Vector3.one;

                    Vector3 uiPot = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x,
                        transform.position.y + gameObject.GetComponent<CapsuleCollider>().height,
                        transform.position.z));

                    float fWidhtRatio = uiPot.x / (float)Screen.width;
                    float fHeightRatio = uiPot.y / (float)Screen.height;

                    float fRatio = (float)(Screen.width) / (Screen.height);

                    if (fRatio > 1.0f)
                    {
                        float bench = Screen.width / (float)Screen.height * 320.0f;
                        float diff = (float)uiPot.x - ((float)Screen.width * 0.5f);
                        float ratio = bench / ((float)Screen.width * 0.5f);
                        float x = ratio * diff;

                        float diffy = (float)uiPot.y - ((float)Screen.height * 0.5f);
                        float ratioy = 320.0f / ((float)Screen.height * 0.5f);
                        float y = ratioy * diffy;

                        headPanel.transform.localPosition = new Vector3(x, y, 0.0f);
                    }
                    else
                    {
                        float benchy = Screen.height / (float)Screen.width * 480.0f;
                        float diffy = (float)uiPot.y - ((float)Screen.height * 0.5f);
                        float ratioy = benchy / ((float)Screen.height * 0.5f);
                        float y = ratioy * diffy;

                        float diffx = (float)uiPot.x - ((float)Screen.width * 0.5f);
                        float ratiox = 480.0f / ((float)Screen.width * 0.5f);
                        float x = ratiox * diffx;

                        headPanel.transform.localPosition = new Vector3(x, y, 0.0f);
                    }
                }
                else
                    headPanel.SetActive(false);
            }
        }      
    }


    public void GenerateHeadUI(string name)
    {
        if (UIManager.Instance.getRootTrans().FindChild("Camera"))
        {
            headPanel = BundleMemManager.Instance.instantiateObj(m_kPrefab);
            headPanel.name = gameObject.name + "_headboard";
            headPanel.transform.parent = UIManager.Instance.getRootTrans().FindChild("Camera");
            headPanel.transform.localPosition = Vector3.zero;
            headPanel.transform.localScale = Vector3.one;

            UIHeadBoard headboardSc = headPanel.GetComponent<UIHeadBoard>();

            headboardSc.m_nInstanceId = -100;

            SetHeadUIData(name);
            headPanel.SetActive(false);
        }
    }
    

    public void SetHeadUIData(string name)
    {
        if (headPanel)
        {
            headPanel.transform.FindChild("name").GetComponent<UILabel>().text = name;
            headPanel.transform.FindChild("background_panel/background").GetComponent<UISprite>().name = backgroundName;
        }
    }


    void OnDestroy()
    {
        GameObject.Destroy(headPanel);
    }
}

