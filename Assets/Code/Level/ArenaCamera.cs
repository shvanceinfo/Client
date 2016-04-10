using UnityEngine;
using System.Collections;

public class ArenaCamera : MonoBehaviour {


    private int m_nScreenTexIndex = 0;

    private float m_fUpdateScreenEffectTime = 0.0f;

	// Use this for initialization
	void Start () 
	{
        CameraFollow.sCameraFollow.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
        UpdateScreenEffect();
	}
	
	void Awake() 
	{

        
    }

    void OnDestroy()
    {
		if(CameraFollow.sCameraFollow != null)
        	CameraFollow.sCameraFollow.gameObject.SetActive(true);
    }

    public void UpdateScreenEffect()
    {
        if (m_nScreenTexIndex > 0)
        {
            Debug.Log("竞技场 更新");
            if (Camera.main)
            {
                ScreenOverlayTex bloodOverlay = Camera.main.GetComponent<ScreenOverlayTex>();

                if (bloodOverlay)
                {
                    if (m_fUpdateScreenEffectTime > 0.02f)
                    {
                        if (m_nScreenTexIndex % 3 == 0)
                        {
                            bloodOverlay.index = 1;
                        }
                        else if (m_nScreenTexIndex % 3 == 1)
                        {
                            bloodOverlay.index = 2;
                        }
                        else if (m_nScreenTexIndex % 3 == 2)
                        {
                            bloodOverlay.index = 3;
                        }

                        bloodOverlay.intensity = 1.0f;

                        m_nScreenTexIndex++;

                        m_fUpdateScreenEffectTime = 0.0f;
                    }

                    m_fUpdateScreenEffectTime += Time.deltaTime;
                }
            }
        }
    }

    public void ScreenEffect()
    {
        Time.timeScale = 0.2f;

        m_nScreenTexIndex = 1;

        m_fUpdateScreenEffectTime = 0.0f;

        if (Camera.main)
        {
            ScreenOverlayTex bloodOverlay = Camera.main.GetComponent<ScreenOverlayTex>();

            if (bloodOverlay)
            {
                bloodOverlay.index = 1;
            }
        }
    }

    public void StopEffect()
    {
        m_fUpdateScreenEffectTime = 0.0f;

        m_nScreenTexIndex = 0;

        Time.timeScale = 1.0f;

        if (Camera.main)
        {
            ScreenOverlayTex bloodOverlay = Camera.main.GetComponent<ScreenOverlayTex>();

            if (bloodOverlay)
            {
                bloodOverlay.index = 0;
            }
        }
    }
}
