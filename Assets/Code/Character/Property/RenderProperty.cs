using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class RenderProperty : MonoBehaviour
{
    public Character m_kOwner;


    public Color m_kMainColor = Color.black;
    

    public float m_fHightlightTime;

    bool m_bHitEffect;

    Material m_kOriginMaterial;
    float m_fTime;
    SkinnedMeshRenderer m_kRenderer;

    Texture m_kTexture;

     

    void Awake()
    {
        m_kOwner = gameObject.GetComponent<Character>();
    }

    void Start()
    {
        m_bHitEffect = false;
        
        Component[] meshFilters = m_kOwner.GetComponentsInChildren(typeof(SkinnedMeshRenderer));
        foreach (SkinnedMeshRenderer m in meshFilters)
        {
            m_kRenderer = m;
            m_kOriginMaterial = m.material;
            m_kTexture = m.material.mainTexture;
        }
    }


    public void GenerateEffect(float duration, Color color)
    {
		return;
       // m_kRenderer.material = new Material(Shader.Find("BeHitEffect"));
        m_kRenderer.material = new Material(BundleMemManager.Instance.ShaderBundle.Load("BeHitEffect") as Shader);
        m_kRenderer.material.SetTexture("_MainTex", m_kTexture);
        m_kRenderer.material.SetColor("_RimColor", color);
        m_kRenderer.material.SetFloat("_UseRim", 1.0f);
        

        m_bHitEffect = true;
        m_fHightlightTime = duration;
        m_fTime = 0.0f;
    }

    void Update()
    {
        if (m_bHitEffect)
        {
            if (m_fTime > m_fHightlightTime)
            {
                m_kRenderer.material = m_kOriginMaterial;
                m_bHitEffect = false;
                m_fHightlightTime = 0.0f;
            }
            else if (m_fTime > m_fHightlightTime * 0.5f)
            {
                m_kRenderer.material.SetColor("_RimColor", new Color(46, 0, 0, 0));
            }
        }
        

        m_fTime += Time.deltaTime;
    }
}
