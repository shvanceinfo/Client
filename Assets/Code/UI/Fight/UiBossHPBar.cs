using UnityEngine;
using System.Collections;

public class UiBossHPBar : MonoBehaviour 
{	
//	GameObject m_pcBackground;
//	GameObject m_pcForeground;
//	GameObject m_pcSprite;
//	GameObject m_pcLabel;
//	static Vector3 s_TempVec;
	
	void Awake()
	{
//		m_pcBackground = transform.FindChild("Background").gameObject;
//		m_pcForeground = transform.FindChild("Foreground").gameObject;
//		m_pcSprite = transform.FindChild("Sprite").gameObject;
//		m_pcLabel = transform.FindChild("Label").gameObject;
	}
	
	void FixScreen()
	{
//		s_TempVec.x = Screen.width;
//		s_TempVec.y = m_pcBackground.transform.localScale.y;
//		s_TempVec.z = 1;
//		m_pcBackground.transform.localScale = s_TempVec;
//		
//		s_TempVec.x = Screen.width;
//		s_TempVec.y = m_pcForeground.transform.localScale.y;
//		s_TempVec.z = 1;
//		m_pcForeground.transform.localScale = s_TempVec;
//		
//		s_TempVec.x = Screen.width;
//		s_TempVec.y = m_pcSprite.transform.localScale.y;
//		s_TempVec.z = 1;
//		m_pcSprite.transform.localScale = s_TempVec;
	}
	
	void Start ()
	{
		//SetBossInfo("role", "MeiDuSha");
		//SetBossCurHP(600, 900);
	}
	
	void Update () 
	{
		
	}
	
	public void SetActive(bool bActive)
	{
		gameObject.SetActive(bActive);
	}
	
	public void SetBossInfo(string pszIconName, string pszMonsterName)
	{
		//transform.FindChild("Panel").FindChild("BossIcon").gameObject.GetComponent<UISprite>().spriteName = pszIconName;
		//transform.FindChild("Panel").FindChild("BossName").gameObject.GetComponent<UILabel>().text = pszMonsterName;
	}
	
	public void SetBossCurHP(int n32CurHP, int n32MaxHP)
	{
		float fChangeValue = (float)n32CurHP / (float)n32MaxHP;
        transform.FindChild("barFill").gameObject.GetComponent<UISprite>().fillAmount = fChangeValue;
	}
	
}
