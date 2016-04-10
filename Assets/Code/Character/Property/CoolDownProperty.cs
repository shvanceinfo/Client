using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoolDownObject
{
    public CoolDownObject(float cd)
    {
        m_fCDTime = cd;
        m_fCurTime = 0.0f;
    }

    public float m_fCDTime;
    public float m_fCurTime;
}

public class CoolDownProperty : MonoBehaviour
{
    public Character m_kOwner = null;

    public Dictionary<int, CoolDownObject> m_kCDObjContainer = new Dictionary<int, CoolDownObject>();

    List<int> m_kNeedRemove = new List<int>();

    void Awake()
    {
        m_kOwner = gameObject.GetComponent<Character>();
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
            //    break;
            //case SKILL_APPEAR.SA_WHIRL_WIND:
            //    nConvertId = 201003;
            //    break;
            //case SKILL_APPEAR.SA_FIRE_RAIN:
            //    nConvertId = 300002;
            //    break;
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

    public float GetCD(int nSkillId)
    {
        if (m_kCDObjContainer.ContainsKey(nSkillId))
        {
            return m_kCDObjContainer[nSkillId].m_fCurTime;
        }

        return 0.0f;
    }


    public void AddCDObj(int nTemplateId)
    {
        if (!IsInCD(nTemplateId))
        {
            CoolDownObject cdObj = null;

            if (nTemplateId == 0)
            {
                CharacterMonster owner = m_kOwner as CharacterMonster;
                float nInterval = Random.Range(owner.monster_property.attack_interval_low, owner.monster_property.attack_interval_upper);
                cdObj = new CoolDownObject(nInterval);
            }
            else
            {
                cdObj = new CoolDownObject(ConfigDataManager.GetInstance().getSkillConfig().getSkillData(nTemplateId).cool_down * 0.001f);
            }

            m_kCDObjContainer.Add(nTemplateId, cdObj);
        }
    }

    // 添加使用红蓝的CD
    public void AddUseHPMPObj()
    {
        // 这里先用-1表示使用红蓝
        if (!IsInCD(-1))
        {
            CoolDownObject cdObj = null;

            cdObj = new CoolDownObject(Global.drupCoolDownTime);

            m_kCDObjContainer.Add(-1, cdObj);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        m_kNeedRemove.Clear();

        foreach (KeyValuePair<int, CoolDownObject> item in m_kCDObjContainer)
        {
            item.Value.m_fCurTime += Time.deltaTime;

            if (item.Value.m_fCurTime > item.Value.m_fCDTime)
            {
                m_kNeedRemove.Add(item.Key);
            }
        }

        for (int i = 0; i < m_kNeedRemove.Count; ++i)
        {
            m_kCDObjContainer.Remove(m_kNeedRemove[i]);
        }
    }
}
