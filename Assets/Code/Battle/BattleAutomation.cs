using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class BattleAutomation
{
    static private BattleAutomation _instance;


    public Dictionary<int, Vector3> m_kDicAreaNameToPosition = new Dictionary<int, Vector3>();
    public int m_nCurAreaKey = -1;


    static public BattleAutomation GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BattleAutomation();
            return _instance;
        }

        return _instance;
    }


    BattleAutomation()
    {
        
    }

    public void ResetData()
    {
        
    }

    public void Init()
    {
        List<AIDataItem> ailist = ConfigDataManager.GetInstance().getAIConfig().GetAIList(true, CharacterPlayer.character_property.getCareer());

        for (int i = 0; i < ailist.Count; ++i)
        {
            //// 自动战斗初始化 将玩家的技能配置 装进AI.SkillBehavious中
            //Dictionary<uint, sSkillData> skillList = SkillManager.GetInstance().GetSkillList();
            //foreach (KeyValuePair<uint, sSkillData> item in skillList)
            //{
            //    sSkillData skill = SkillManager.GetInstance().GetSkillList()[item.Key];

            //    if (skill.skillId == ailist[i].AIValue)
            //    {
            //        CharacterPlayer.sPlayerMe.GetAI().m_kAISkillBehavious.Add(ailist[i].id);
            //    }
            //}
        }
    }

    // 初始化刷怪点信息
    public void InitSceneMonsterAreaInfo()
    {
        m_kDicAreaNameToPosition.Clear();

        int mapID = CharacterPlayer.character_property.getServerMapID();

        string battleName = ConfigDataManager.GetInstance().getMapConfig().getMapData(mapID).battlePref;

        //battleName = battleName.Replace("Model/prefab/BattlePref/", "");

        GameObject kContainer = GameObject.Find(battleName);

        if (kContainer)
        {
            for (int i = 0; i < kContainer.transform.childCount; ++i)
            {
                if (kContainer.transform.GetChild(i).name.Contains("MonsterArea"))
                {
                    string name = kContainer.transform.GetChild(i).name.Replace("MonsterArea", "");
                    m_kDicAreaNameToPosition.Add(int.Parse(name), kContainer.transform.GetChild(i).position);
                }
            }
        }
    }

    public Vector3 GetNextMonsterArea()
    {
        int min = int.MaxValue;
        foreach (KeyValuePair<int, Vector3> item in m_kDicAreaNameToPosition)
        {
            if (min > item.Key)
            {
                min = item.Key;
            }
        }

        if (min != int.MaxValue)
        {
            m_nCurAreaKey = min;
            return m_kDicAreaNameToPosition[min];
        }

        // 这里就表示没有刷怪点 或者怪都打完了
        m_nCurAreaKey = -1;
        return Vector3.zero;
    }

    public void DeleteMonsterItem(string strAreaName)
    {
        string childName = strAreaName;
        childName = childName.Replace("MonsterArea", ""); 

        if (childName.Length == 1)
        {
            m_kDicAreaNameToPosition.Remove(int.Parse(childName));
        }
    }

    public void EndSceneData()
    {
        m_kDicAreaNameToPosition.Clear();
    }
}