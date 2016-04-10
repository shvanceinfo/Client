using UnityEngine;
using System.Collections;
using NetGame;

public class CFillMp : MonoBehaviour
{

    public HPMPInit m_parent;

    bool m_bPressed;

    uint m_nIncre;

    uint m_nTotalMount;

    public UIhpmp m_hpmpUI;

    public float m_fDuration;

    uint m_nShowMp;
	ParticleSystem m_ReverHPEffect;
	public AudioClip m_BloodMusic;

    void Awake()
    {
        m_parent = transform.parent.GetComponent<HPMPInit>();
        m_hpmpUI = transform.parent.FindChild("drug_stretch").GetComponent<UIhpmp>();

        m_bPressed = false;
        m_nIncre = 1;



        m_nTotalMount = 0;
        m_fDuration = 0.0f;

		m_ReverHPEffect = transform.parent.FindChild("jialan_UI").GetComponent<ParticleSystem>(); 
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
	
    // Use this for initialization
    void Start()
    {
        EventDispatcher.GetInstance().PlayerProperty += OnPlayerProperty;
        m_nShowMp = CharacterPlayer.character_property.getCurMPVessel();
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
				
				if (CharacterPlayer.character_property.getCurMPVessel() >= CharacterPlayer.character_property.getMaxMPVessel())
				{
					//血包已满
					m_bPressed = false;
					OnRelease();
					FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_full_mp"), true, UIManager.Instance.getRootTrans());
					return;
				}

                m_nIncre += 10;
				
                if (CharacterPlayer.character_asset.gold <= (int)(m_nTotalMount))
                {
                    //扣最后的钱
					int span = (int)CharacterPlayer.character_property.getCurMPVessel()+(int)CharacterPlayer.character_asset.gold-(int)CharacterPlayer.character_property.getMaxMPVessel();
					if ( span >= 0)
					{
						m_nTotalMount = (uint)(CharacterPlayer.character_asset.gold-span);
					}
					else
					{
						m_nTotalMount = (uint)CharacterPlayer.character_asset.gold;
					}
					SetMpUIData(CharacterPlayer.character_property.getCurMPVessel()+m_nTotalMount);
					OnClick();
                    //m_nIncre = (uint)CharacterPlayer.character_asset.gold;
                }

                // 开始加蓝

                if (m_nShowMp < CharacterPlayer.character_property.getMaxMPVessel())
                {
                    if (m_nShowMp + m_nIncre > CharacterPlayer.character_property.getMaxMPVessel())
                    {
                        //m_nTotalMount += (m_nShowMp + m_nIncre - CharacterPlayer.character_property.getMaxMPVessel());
                        m_nTotalMount = CharacterPlayer.character_property.getMaxMPVessel() - CharacterPlayer.character_property.getCurMPVessel();
                        //Debug.Log("%%%%%%%%%%%%%%%%%%%%%%" + m_nTotalMount);
                        SetMpUIData(CharacterPlayer.character_property.getMaxMPVessel());
                    }
                    else
                    {
                        m_nTotalMount += m_nIncre;
                        SetMpUIData(m_nShowMp + m_nIncre);
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
        m_bPressed = true;
        m_nShowMp = CharacterPlayer.character_property.getCurMPVessel();
		
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
		
		UIManager.Instance.ShowDialog(eDialogSureType.eRecoverHPMP, LanguageManager.GetText("msg_recover_magic_pack")+" [efd423]"+m_nTotalMount+" "+LanguageManager.GetText("msg_recover_pack_suffix"));  
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
	            GCAskFillMP msg = new GCAskFillMP(m_nTotalMount);
	            net.Send(msg.ToBytes());
	
	            m_nTotalMount = 0;
	        }
	
	        if (CharacterPlayer.character_property.getCurMPVessel() == CharacterPlayer.character_property.getMaxMPVessel())
	        {
	            FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_full_mp"), true, UIManager.Instance.getRootTrans());
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
			SetMpUIData(CharacterPlayer.character_property.getCurMPVessel());
			m_nTotalMount = 0;
			m_hpmpUI.ResetAutoHide();
		}
	}

    void OnPlayerProperty()
    {
        m_parent.m_mp.text = CharacterPlayer.character_property.getCurMPVessel().ToString();
        m_parent.m_mpSilder.sliderValue = (float)CharacterPlayer.character_property.getCurMPVessel() / (float)CharacterPlayer.character_property.getMaxMPVessel();
    }

    void SetMpUIData(uint mp)
    {
        m_nShowMp = mp;
        //Debug.Log("PPPPPPPPPPPPPPPPPPPPPP" + m_nShowMp);
        m_parent.m_mp.text = mp.ToString();
        m_parent.m_mpSilder.sliderValue = mp / (float)CharacterPlayer.character_property.getMaxMPVessel();
    }
}
