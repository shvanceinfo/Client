using UnityEngine;
using System.Collections;

public class UIhpmp : MonoBehaviour {

    public bool mbOpen = false;

    public Vector3 mVecInitPos;
    public Vector3 mVecDestPos;

    public HPMPInit m_ParentRoot;

    public UISprite m_getLarge;

    public bool m_bHasMouseAction;

    float m_bDuration = 0.0f;

    //Transform mFillHintEffect;

    ParticleSystem mFillHintEffect;

    void Awake()
    {
        m_ParentRoot = transform.parent.GetComponent<HPMPInit>();

        m_getLarge = transform.FindChild("getlarge").GetComponent<UISprite>();

        mFillHintEffect = transform.parent.FindChild("jiaxue").GetComponent<ParticleSystem>();
    }

	// Use this for initialization
	void Start () {

        
        mVecInitPos = transform.parent.localPosition;
        mVecDestPos = new Vector3(65.0f, -99.0f, 0.0f);
	
	}
	
	// Update is called once per frame
	void Update () {
//        if (m_bHasMouseAction)
//        {
//            m_bDuration += Time.deltaTime;
//            
//            if (m_bDuration > 3.0f)
//            {
//                AutoHide();
//                m_bDuration = 0.0f;
//                m_bHasMouseAction = false;
//            }
//        }
		if(mbOpen && Input.GetButtonDown("Fire1"))
		{
			if (UICamera.hoveredObject != null)
		 	{
		 		if(!checkClickHpMp(UICamera.hoveredObject.name))
		 			AutoHide();
		 	}
		 	else if(UICamera.selectedObject != null)
		 	{
		 		if(checkClickHpMp(UICamera.selectedObject.name))
		 			AutoHide();
		 	}
		 	else
		 		AutoHide();
		}

       
        // 计算血量比
        float ratioHp = CharacterPlayer.character_property.getCurHPVessel() / (float)CharacterPlayer.character_property.getMaxHPVessel();
        float ratioMp = CharacterPlayer.character_property.getCurMPVessel() / (float)CharacterPlayer.character_property.getMaxMPVessel();
        if (ratioHp < 0.2f || ratioMp < 0.2f)
        {
            mFillHintEffect.Play(true);
        }
        else
        {
            mFillHintEffect.Stop();
        }
        
	}

    void AutoHide()
    {
        MouseAction(true);
    }


    void MouseAction(bool bOpen)
    {
        if (!bOpen)
        {
            TweenPosition.Begin(transform.parent.gameObject, 0.3f, mVecDestPos);
            mbOpen = true;
            m_getLarge.gameObject.SetActive(false);
        }
        else
        {
            TweenPosition.Begin(transform.parent.gameObject, 0.3f, mVecInitPos);
            mbOpen = false;
            m_getLarge.gameObject.SetActive(true);
        }

        m_ParentRoot.SetConnectVisible(!bOpen);
    }

    public void ResetAutoHide()
    {
        m_bHasMouseAction = true;
        m_bDuration = 0.0f;
    }

    void OnClick()
    {
        ResetAutoHide();
        
        MouseAction(mbOpen);
    }
    
    //判断是否点击到血值
    bool checkClickHpMp(string checkName)
    {
    	if(checkName.Equals("drug_stretch") || checkName.Equals("get_hp") || checkName.Equals("get_mp"))
    		return true;
    	return false;
    }
}
