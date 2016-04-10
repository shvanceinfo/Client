using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SyncManager 
{
    static private SyncManager _instance;

    static public SyncManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new SyncManager();
            return _instance;
        }

        return _instance;
    }

    public void SendPlayerTranInfo()
    {
        // 只有在这些情况下才发送主角同步信息
        if (Global.inCityMap() || Global.inMultiFightMap() || Global.InWorldBossMap())
        {
            // 位置和朝向信息存到列表中 根据发送频率来发送消息
            CharacterPlayer.sPlayerMe.m_kSyncDir.Add(CharacterPlayer.sPlayerMe.getFaceDir());
            CharacterPlayer.sPlayerMe.m_kSyncPos.Add(CharacterPlayer.sPlayerMe.getPosition());
        }
    }

    public void ReportMonsterTranInfo(CharacterMonster kMonster)
    {
        // 只有在这些情况下才发送怪的同步信息
        if (Global.inMultiFightMap() && CharacterPlayer.character_property.getHostComputer())
        {
            kMonster.m_kMultiMonsterSyncDir.Add(kMonster.getFaceDir());
            kMonster.m_kMultiMonsterSyncPos.Add(kMonster.getPosition());
        }
    }
}
