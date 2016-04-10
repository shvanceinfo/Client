using UnityEngine;
using System.Collections.Generic;

public class CDObject
{
    public CDObject(float cd)
    {
        m_fCDTime = cd;
        m_fCurTime = 0.0f;
    }
    
    public float m_fCDTime;
    public float m_fCurTime;
}

public class CDManager 
{
    public Dictionary<int, CDObject> m_kCDObjContainer;

    List<int> m_kNeedRemove;

    private static CDManager _instance = null;

	public static CDManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = new CDManager();
            return _instance;
        }
    }

    private CDManager()
    {
        m_kNeedRemove = new List<int>();
        m_kCDObjContainer = new Dictionary<int, CDObject>();
    }

    public bool IsInCD(int nTemplateId)
    {
        if (m_kCDObjContainer.ContainsKey(nTemplateId))
        {
            return true;
        }

        return false;
    }

    public bool IsInCD(SKILL_APPEAR skillAppear)
    {
        int nConvertId = 0;

        switch (skillAppear)
        {
            //case SKILL_APPEAR.SA_ROLL:
            //    nConvertId = 400001;
            //break;
            //case SKILL_APPEAR.SA_WHIRL_WIND:
            //    nConvertId = 201003;
            //break;
            //case SKILL_APPEAR.SA_FIRE_RAIN:
            //    nConvertId = 300002;
            //break;
            case SKILL_APPEAR.SA_MAG_FLASH_AWAY:
                nConvertId = 400003001;
            break;
        }

        if (m_kCDObjContainer.ContainsKey(nConvertId))
        {
            return true;
        }

        return false;
    }


    public void AddCDObj(int nTemplateId)
    {
        if (!IsInCD(nTemplateId))
        {
            CDObject cdObj = new CDObject(ConfigDataManager.GetInstance().getSkillConfig().getSkillData(nTemplateId).cool_down * 0.001f);
            m_kCDObjContainer.Add(nTemplateId, cdObj);
        }
    }

	// Use this for initialization
	void Start () 
    {
	
	}

    void Update()
    {
        m_kNeedRemove.Clear();

        foreach (KeyValuePair<int, CDObject> item in m_kCDObjContainer)
        {
            item.Value.m_fCurTime += Time.deltaTime;

            if (item.Value.m_fCurTime > item.Value.m_fCDTime)
            {
                m_kNeedRemove.Add(item.Key);
            }
        }

        for (int i = 0; i < m_kNeedRemove.Count; ++i )
        {
            m_kCDObjContainer.Remove(m_kNeedRemove[i]);
        }
    }
}
