using UnityEngine;
using System.Collections;

public class UiRollCDSlider : MonoBehaviour 
{	
//	GameObject m_pcBackground;
//	GameObject m_pcForeground;
//	GameObject m_pcSprite;
//	GameObject m_pcLabel;
//	static Vector3 s_TempVec;
	static float s_TotalLifeTime = 5.0f;
	static UiRollCDSlider s_pcInstance;
	float m_fLifeTime;
	
	static UiRollCDSlider Instance
	{
		get{return s_pcInstance;}
	}
	
	void Awake()
	{
		m_fLifeTime = 0;
		s_pcInstance = this;
//		m_pcBackground = transform.FindChild("Background").gameObject;
//		m_pcForeground = transform.FindChild("Foreground").gameObject;
//		m_pcSprite = transform.FindChild("Sprite").gameObject;
//		m_pcLabel = transform.FindChild("Label").gameObject;
	}
	
	void Start ()
	{
		//SetBossInfo("role", "MeiDuSha");
		//SetBossCurHP(600, 900);
	}
	
	void Update () 
	{
		m_fLifeTime -= Time.deltaTime;
		float fRate = m_fLifeTime / s_TotalLifeTime;
		gameObject.GetComponent<UISlider>().sliderValue = fRate;
		if (m_fLifeTime <= 0)
		{
			GameObject.Destroy(gameObject);
		}
	}
	
	void OnEnable()
	{
        SkillDataItem pcSkillData = null;

        switch (CharacterPlayer.character_property.getCareer())
        {
        case CHARACTER_CAREER.CC_ARCHER:
        case CHARACTER_CAREER.CC_SWORD:
            pcSkillData = ConfigDataManager.GetInstance().getSkillConfig().getSkillData(400001);
        	break;
        case CHARACTER_CAREER.CC_MAGICIAN:
            pcSkillData = ConfigDataManager.GetInstance().getSkillConfig().getSkillData(400003);
            break;
        }

		if (null != pcSkillData)
		{
			float fSkillCDTime = (float)pcSkillData.cool_down / (float)1000;
			m_fLifeTime = fSkillCDTime;
			s_TotalLifeTime = fSkillCDTime;
		}
	}
	
	void OnDisable()
	{
		
	}
}
