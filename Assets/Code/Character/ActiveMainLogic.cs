using UnityEngine;
using System.Collections;

public class ActiveMainLogic : MonoBehaviour {
	
	public 	float 				m_fWaitTime = 5;
	public 	GameObject 	m_ActiveObj;
	private 	float 				m_fTimeLapse;
	private  bool				m_bIsActive; 
	// Use this for initialization
	void Start () {
		m_fTimeLapse = 0.0f;
		m_bIsActive = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_bIsActive || m_ActiveObj == null)
		{
			return;
		}
		m_fTimeLapse += Time.deltaTime;
		if (m_fTimeLapse >= m_fWaitTime && m_ActiveObj != null)
		{
			m_ActiveObj.SetActive(true);
			Destroy(this.gameObject);
			m_bIsActive = true;
		}
	}
}
