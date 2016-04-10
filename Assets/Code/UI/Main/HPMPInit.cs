using UnityEngine;
using System.Collections;

public class HPMPInit : MonoBehaviour {

    public Transform m_connect;

    public UILabel m_hp;
    public UILabel m_mp;

    public UISlider m_hpSlider;
    public UISlider m_mpSilder;

    void Awake()
    {
        m_connect = transform.FindChild("connect");
        m_connect.gameObject.SetActive(false);

        m_hpSlider = transform.FindChild("drug_stretch/drug/hp_collder/hp").GetComponent<UISlider>();
        m_mpSilder = transform.FindChild("drug_stretch/drug/mp_collder/mp").GetComponent<UISlider>();

        m_hp = transform.FindChild("drug_stretch/drug/hp_collder/hp/lbl_num").GetComponent<UILabel>();
        m_mp = transform.FindChild("drug_stretch/drug/mp_collder/mp/lbl_num").GetComponent<UILabel>();

        m_hp.text = CharacterPlayer.character_property.getCurHPVessel().ToString();
        m_hpSlider.sliderValue = (float)CharacterPlayer.character_property.getCurHPVessel() / (float)CharacterPlayer.character_property.getMaxHPVessel();
        m_mp.text = CharacterPlayer.character_property.getCurMPVessel().ToString();
        m_mpSilder.sliderValue = (float)CharacterPlayer.character_property.getCurMPVessel() / (float)CharacterPlayer.character_property.getMaxMPVessel();
    }

	// Use this for initialization
	void Start () {
	
	}


    public void SetConnectVisible(bool bVisible)
    {
        m_connect.gameObject.SetActive(bVisible);
    }
}
