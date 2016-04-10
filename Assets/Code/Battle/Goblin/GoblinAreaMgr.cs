using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoblinAreaMgr : MonoBehaviour
{
    public GoblinItemArea m_kArea0;
    public GoblinItemArea m_kArea1;
    public GoblinItemArea m_kArea2;

    public int m_nItemTemplateID0 = 0;
    public int m_nItemTemplateID1 = 0;
    public int m_nItemTemplateID2 = 0;
    public int m_nItemTemplateID3 = 0;
    public int m_nItemTemplateID4 = 0;

    List<int> m_nIDContainer = new List<int>();

	void Awake () 
    {
        m_kArea0.m_nAreaID = 0;
        m_kArea1.m_nAreaID = 1;
        m_kArea2.m_nAreaID = 2;

        if (m_nItemTemplateID0 != 0)
        {
            m_nIDContainer.Add(m_nItemTemplateID0);
        }

        if (m_nItemTemplateID1 != 0)
        {
            m_nIDContainer.Add(m_nItemTemplateID1);
        }

        if (m_nItemTemplateID2 != 0)
        {
            m_nIDContainer.Add(m_nItemTemplateID2);
        }

        if (m_nItemTemplateID3 != 0)
        {
            m_nIDContainer.Add(m_nItemTemplateID3);
        }

        if (m_nItemTemplateID4 != 0)
        {
            m_nIDContainer.Add(m_nItemTemplateID4);
        }
	}

	// Use this for initialization
	void Start () 
    {
        // 数据重置
        BattleGoblin.GetInstance().ResetData();
        MonsterArea ma = GetComponentInChildren<MonsterArea>();
        if (ma.monster1_template_id != 0)
        {
            BattleGoblin.GetInstance().m_nGoblinId = ma.monster1_template_id;
        }

        
        // 刚开始 随机选一个点刷 一个随机道具
        GenerateRandomItem();
	}

    void OnEnable()
    {
        EventDispatcher.GetInstance().BuffDisappear += OnBuffDisappear;
    }

    void OnDisable()
    {
        EventDispatcher.GetInstance().BuffDisappear -= OnBuffDisappear;
    }
	
	// Update is called once per frame
	void Update () 
    {
       
	}

    void OnDestroy()
    {
        BattleGoblin.GetInstance().OnDestroy();
    }

    public void GenerateRandomItem()
    {
        GetRandomArea().GenerateItem(GetRandomTemplateID());
    }

    GoblinItemArea GetRandomArea()
    {
        switch (Random.Range(0, 3))
        {
        case 0:
            return m_kArea0;
        case 1:
            return m_kArea1;
        case 2:
            return m_kArea2;
        default:
            return m_kArea0;
        }
    }

    int GetRandomTemplateID()
    {
        return m_nIDContainer.Count != 0 ? m_nIDContainer[Random.Range(0, m_nIDContainer.Count)] : 0;
    }

    void OnBuffDisappear(BUFF_TYPE type)
    {
        if (type == BUFF_TYPE.BT_ONE_KILL ||
            type == BUFF_TYPE.BT_GREED ||
            type == BUFF_TYPE.BT_MEAN ||
            type == BUFF_TYPE.BT_RAMPAGE)
        {
            BattleGoblin.GetInstance().RemoveHeadBuff();

            GenerateRandomItem();
        }
    }
}
