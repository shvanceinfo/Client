using UnityEngine;
using System.Collections;
using NetGame;

public enum eAddType
{
	eBlood,
	eMagic,
};

public class AddRoleBloodAndMagic : MonoBehaviour {
	
	public HPMPInit m_parent;
	public UIhpmp m_hpmpUI;
	
	public eAddType m_eType;
	// Use this for initialization
	ParticleSystem m_ReverEffect;
	void Start () {
		m_parent = transform.parent.GetComponent<HPMPInit>();
        m_hpmpUI = transform.parent.FindChild("drug_stretch").GetComponent<UIhpmp>();
		
		if (m_eType == eAddType.eBlood)
		{
			m_ReverEffect = transform.parent.FindChild("jiaxue_UI").GetComponent<ParticleSystem>();
		}
		else
		{
			m_ReverEffect = transform.parent.FindChild("jialan_UI").GetComponent<ParticleSystem>(); 
		}
	}
	
	void HPMPChangeEvent()
	{
		SetHpUIData();
		SetMpUIData();
	}
	
	void OnEnable()
	{
		EventDispatcher.GetInstance().PlayerAsset += HPMPChangeEvent;
	}
	
	void OnDisable()
	{
		EventDispatcher.GetInstance().PlayerAsset -= HPMPChangeEvent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnAddEvent()
	{
		//m_ReverEffect.Play();
		StartCoroutine(StopEffect());
		if (m_eType == eAddType.eBlood)
		{
			//一键加血 
			uint nNeedMoney = CharacterPlayer.character_property.getMaxHPVessel() - CharacterPlayer.character_property.getCurHPVessel();
			if (nNeedMoney == 0)
			{
				FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_full_hp"), true, UIManager.Instance.getRootTrans());
				return;
			}
			
			if (CharacterPlayer.character_asset.gold == 0)
			{
				FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_money_not_enough"), true, UIManager.Instance.getRootTrans());
				return;
			}
			
			if (CharacterPlayer.character_asset.gold < nNeedMoney)
			{
				nNeedMoney = (uint)CharacterPlayer.character_asset.gold;
			}
			
            // 发送消耗金币消息
            NetBase net = NetBase.GetInstance();
            GCAskFillHP msg = new GCAskFillHP(nNeedMoney);
            net.Send(msg.ToBytes());
			FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_cost_money_desc")+nNeedMoney.ToString()+LanguageManager.GetText("msg_cost_money"), false, UIManager.Instance.getRootTrans());
		}
		else 
		{
			//一键加蓝 
			uint nNeedMoney = CharacterPlayer.character_property.getMaxMPVessel() - CharacterPlayer.character_property.getCurMPVessel();
			Debug.Log("最大蓝："+CharacterPlayer.character_property.getMaxMPVessel());
			Debug.Log("当前蓝："+CharacterPlayer.character_property.getCurMPVessel());
			if (nNeedMoney == 0)
			{
				FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_full_mp"), true, UIManager.Instance.getRootTrans());
				return;
			}
			Debug.Log("需要金币: "+nNeedMoney);
			if (CharacterPlayer.character_asset.gold == 0)
			{
				FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_money_not_enough"), true, UIManager.Instance.getRootTrans());
				return;
			}
			
			Debug.Log("当前金币: "+CharacterPlayer.character_asset.gold);
			if (CharacterPlayer.character_asset.gold < nNeedMoney)
			{
				nNeedMoney = (uint)CharacterPlayer.character_asset.gold;
			}
			
            // 发送消耗金币消息
            NetBase net = NetBase.GetInstance();
            GCAskFillMP msg = new GCAskFillMP(nNeedMoney);
            net.Send(msg.ToBytes());
			FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("msg_cost_money_desc")+nNeedMoney.ToString()+LanguageManager.GetText("msg_cost_money"), false, UIManager.Instance.getRootTrans());
		}
		
	}
	
	void SetHpUIData()
    {
        m_parent.m_hp.text = CharacterPlayer.character_property.getCurHPVessel().ToString();
        m_parent.m_hpSlider.sliderValue = (float)CharacterPlayer.character_property.getCurHPVessel() / (float)CharacterPlayer.character_property.getMaxHPVessel();
    }
	
	void SetMpUIData()
    {
        m_parent.m_mp.text = CharacterPlayer.character_property.getCurMPVessel().ToString();
        m_parent.m_mpSilder.sliderValue = (float)CharacterPlayer.character_property.getCurMPVessel() / (float)CharacterPlayer.character_property.getMaxMPVessel();
    }
	
	IEnumerator StopEffect()
	{
		m_ReverEffect.Play();
		yield return new WaitForSeconds(0.1f);
		m_ReverEffect.Stop();
	}	
}
