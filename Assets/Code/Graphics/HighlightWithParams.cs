using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class HighlightWithParams : MonoBehaviour 
{

    public float delay;
    public float duration;
    public float interval;
    public bool once;
    public Color color = Color.white;
    

    Character m_kHighLightObj;
    float m_fTime;
    bool m_bHasPlayed;
    bool m_bHasFinished;
    float m_fLastPlayTime;

    bool m_bFirst;

    

    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () 
    {
        m_kHighLightObj = transform.parent.parent.gameObject.GetComponent<Character>();
	    //InvokeRepeating("Timer", 1, 0.1f);

        m_fTime = 0.0f;
        m_bHasPlayed = false;
        m_bHasFinished = false;
        m_fLastPlayTime = 0.0f;
        m_bFirst = true;
	}

    void Update()
    {
        if (m_fTime > delay)
        {
            float timeSincePlay = m_fTime - delay;

            

            // 开始 周期性播放 高光
            if (m_bFirst)
            {
                if (!m_bHasPlayed)
                {
                    if (m_kHighLightObj != null)
                    {
                        Debug.Log("上行dsdddddddd");
                        //EffectManager.Instance.BeHitHightlight(m_kHighLightObj, duration);
                        m_kHighLightObj.GetComponent<RenderProperty>().GenerateEffect(duration, color);
                        //m_kHighLightObj.m_kRenderProp.GenerateEffect(duration);
                        m_bHasPlayed = true;
                        m_bHasFinished = false;
                        m_fLastPlayTime = timeSincePlay;
                    }
                }

                m_bFirst = false;
            }
            else
            {
                if (!once)
                {
                    if (timeSincePlay >= m_fLastPlayTime + duration)
                    {
                        if (!m_bHasFinished)
                        {
                            m_bHasPlayed = false;
                            m_bHasFinished = true;
                        }
                    }


                    if (timeSincePlay >= m_fLastPlayTime + duration + interval)
                    {
                        if (!m_bHasPlayed)
                        {
                            if (m_kHighLightObj != null)
                            {
                                Debug.Log("下行dsdddddddd");
                                //EffectManager.Instance.BeHitHightlight(m_kHighLightObj, duration);
                                //m_kHighLightObj.m_kRenderProp.GenerateEffect(duration);
                                m_kHighLightObj.GetComponent<RenderProperty>().GenerateEffect(duration, color);
                                m_bHasPlayed = true;
                                m_bHasFinished = false;
                                m_fLastPlayTime = timeSincePlay;
                            }
                        }
                    }
                }
            }
        }

        m_fTime += Time.deltaTime;
    }
}
