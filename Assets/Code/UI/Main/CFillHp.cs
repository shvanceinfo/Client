using UnityEngine;
using System.Collections;
using NetGame;

public class CFillHp : MonoBehaviour
{
    public HPMPInit m_parent;
    bool m_bPressed;
    uint m_nIncre;
    uint m_nTotalMount;
    public UIhpmp m_hpmpUI;
    public float m_fDuration;
    uint m_nShowHp;
	ParticleSystem m_ReverHPEffect;
	
	public AudioClip m_BloodMusic;
	
	//GameObject jiaXueObj = null;
    void Awake()
    {
        m_parent = transform.parent.GetComponent<HPMPInit>();
        m_hpmpUI = transform.parent.FindChild("drug_stretch").GetComponent<UIhpmp>();

        m_bPressed = false;
        m_nIncre = 0;
        m_nTotalMount = 0;
        m_fDuration = 0.0f;

		m_ReverHPEffect = transform.parent.FindChild("jiaxue_UI").GetComponent<ParticleSystem>();
    }

    // Use this for initialization
    void Start()
    {
        EventDispatcher.GetInstance().PlayerProperty += OnPlayerProperty;
        m_nShowHp = CharacterPlayer.character_property.getCurHPVessel();
    }
	
	void OnEnable()
	{
		EventDispatcher.GetInstance().DialogSure += OnDialogSure;
		EventDispatcher.GetInstance().DialogCancel += OnDialogCancel;
	}
	
	void OnDisable()
	{
		EventDispatcher.GetInstance().DialogSure -= OnDialogSure;
		EventDispatcher.GetInstance().DialogCancel -= OnDialogCancel;
	} 

    // Update is called once per frame
    void Update()
    {
        if (m_bPressed)
        {
            if (m_fDuration > 0.05f)
            {
                if (CharacterPlayer.character_asset.gold <= 0)
                {
                    //没钱了
					m_bPressed = false;
					OnRelease();
					FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_money_not_enough"), true, UIManager.Instance.getRootTrans());
                    return;
                }
				
				if (CharacterPlayer.character_property.getCurHPVessel() >= CharacterPlayer.character_property.getMaxHPVessel())
				{
					//血包已满
					m_bPressed = false;
					OnRelease();
					FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_full_hp"), true, UIManager.Instance.getRootTrans());
					return;
				}

                m_nIncre += 10;
				
				
				Debug.Log("gold: "+CharacterPlayer.character_asset.gold+"; increate: "+(m_nShowHp + m_nIncre));
                if (CharacterPlayer.character_asset.gold <= (int)m_nTotalMount)
                {
                    //扣最后的钱
					int span = (int)CharacterPlayer.character_property.getCurHPVessel()+(int)CharacterPlayer.character_asset.gold-(int)CharacterPlayer.character_property.getMaxHPVessel();
					if ( span >= 0)
					{
						m_nTotalMount = (uint)(CharacterPlayer.character_asset.gold-span);
					}
					else
					{
						m_nTotalMount = (uint)CharacterPlayer.character_asset.gold;
					}
					SetHpUIData(CharacterPlayer.character_property.getCurHPVessel()+m_nTotalMount);
					OnClick();
                    //m_nIncre = (uint)CharacterPlayer.character_asset.gold;
                }
                

                // 开始加红
                if (m_nShowHp < CharacterPlayer.character_property.getMaxHPVessel())
                {
                    if (m_nShowHp + m_nIncre >= CharacterPlayer.character_property.getMaxHPVessel())
                    {
                        //Debug.Log("####################" + m_nTotalMount + " " + m_nShowHp + " " + m_nIncre + " " + CharacterPlayer.character_property.getMaxHPVessel());
                        //uint delta = (CharacterPlayer.character_property.getMaxHPVessel() - m_nTotalMount);
                        m_nTotalMount = CharacterPlayer.character_property.getMaxHPVessel() - CharacterPlayer.character_property.getCurHPVessel();
                        //Debug.Log("%%%%%%%%%%%%%%%%%%%%%%" + m_nTotalMount);
                        SetHpUIData(CharacterPlayer.character_property.getMaxHPVessel());
                    }
                    else
                    {
                        m_nTotalMount += m_nIncre;
                        SetHpUIData(m_nShowHp + m_nIncre);
                    }

                    m_hpmpUI.ResetAutoHide();
                }
                else
                {
                    OnClick();
                }

                m_fDuration = 0.0f;
            }

            m_fDuration += Time.deltaTime;
        }
    }

    void OnPressBtn()
    {
		//Debug.Log("on press!!!!!!!!!!!!!");
        m_bPressed = true;
        m_nShowHp = CharacterPlayer.character_property.getCurHPVessel();
		
		if (m_ReverHPEffect != null)
		{
			m_ReverHPEffect.Play();
		}
		
		if (m_BloodMusic != null)
		{
			audio.clip = m_BloodMusic;
			audio.Play();
		}
    }
	
	void OnRelease()
	{
		//Debug.Log("OnRelease");
		if (m_ReverHPEffect != null)
		{
			m_ReverHPEffect.Stop();;
		}
		
		if (m_BloodMusic != null)
		{
			//audio.clip = m_BloodMusic;
			audio.Stop();
		}
	}

    void OnClick()
    {
		if (m_bPressed == false)
		{
			return;
		}
		OnRelease();
		m_bPressed = false;
		m_nIncre = 0;
		
		if (m_nTotalMount <= 0)
		{
			return;
		}
		
		UIManager.Instance.ShowDialog(eDialogSureType.eRecoverHPMP, LanguageManager.GetText("msg_recover_blood_pack")+" [efd423]"+m_nTotalMount+" "+LanguageManager.GetText("msg_recover_pack_suffix"));  
    }
	
	void OnDialogSure(eDialogSureType type)
	{
		if (type == eDialogSureType.eRecoverHPMP)
		{
			if (m_nTotalMount <= 0)
			{
				return;
			}
			//Debug.Log("确定补血");
			if (m_nTotalMount > 0)
	        {
	            // 发送消耗金币消息
	            NetBase net = NetBase.GetInstance();
	            GCAskFillHP msg = new GCAskFillHP(m_nTotalMount);
	            net.Send(msg.ToBytes());
	
	            m_nTotalMount = 0;
	        }
	
	        if (CharacterPlayer.character_property.getCurHPVessel() == CharacterPlayer.character_property.getMaxHPVessel())
	        {
	            FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_full_hp"), true, UIManager.Instance.getRootTrans());
	        }
	
	        m_hpmpUI.ResetAutoHide();
		}
		//Debug.Log("dialog click, buy shop item id: "+m_BuyItemID);
	}
	
	void OnDialogCancel(eDialogSureType type)
	{
		if (type == eDialogSureType.eRecoverHPMP)
		{
			//Debug.Log("取消补血");
			SetHpUIData(CharacterPlayer.character_property.getCurHPVessel());
			m_nTotalMount = 0;
			m_hpmpUI.ResetAutoHide();
		}
	}

    void OnPlayerProperty()
    {
        m_parent.m_hp.text = CharacterPlayer.character_property.getCurHPVessel().ToString();
        m_parent.m_hpSlider.sliderValue = (float)CharacterPlayer.character_property.getCurHPVessel() / (float)CharacterPlayer.character_property.getMaxHPVessel();
    }

    void SetHpUIData(uint mp)
    {
        m_nShowHp = mp;
        //Debug.Log("PPPPPPPPPPPPPPPPPPPPPP" + m_nShowHp);
        m_parent.m_hp.text = mp.ToString();
        m_parent.m_hpSlider.sliderValue = mp / (float)CharacterPlayer.character_property.getMaxHPVessel();
    }
}
