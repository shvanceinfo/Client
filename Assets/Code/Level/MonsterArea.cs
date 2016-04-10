using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using manager;
using model;

public class PendingSpawnMonster
{

    public Vector3 pos;
    public Quaternion rot;
    public float time_to_spawn;
    public int template_id;
}

public class MonsterArea : LevelEventHandler
{

    bool triggered = false;
    bool bSuspendForBoss = false;
    bool spawn_over = false;
    public int area_id;
    public float spawn_delta = 0.5f;
    public float trigger_delta = 0;
    public bool auto_spawn = false;
    public float spawn_delay = 2.0f;
    public int monster1_template_id = -1;
    public int monster2_template_id = -1;
    public int monster3_template_id = -1;
    public int monster4_template_id = -1;
    public LevelEventHandler monster_clear_handler;
    List<Vector3> spawn1_points = null;
    List<Vector3> spawn2_points = null;
    List<Vector3> spawn3_points = null;
    List<Vector3> spawn4_points = null;
    List<Quaternion> spawn1_rots = null;
    List<Quaternion> spawn2_rots = null;
    List<Quaternion> spawn3_rots = null;
    List<Quaternion> spawn4_rots = null;
    List<int> monster_ids = null;
    List<PendingSpawnMonster> pending_spawn_monsters = null;

    public Dictionary<int, List<int>> m_kTempleID2InstanceID;

    public static bool m_bOneGoblinDie = false;
    // whether the gaint goblin die
    public static bool m_bOneGaintGoblinDie = false;


    void Awake()
    {
        spawn1_points = new List<Vector3>();
        spawn2_points = new List<Vector3>();
        spawn3_points = new List<Vector3>();
        spawn4_points = new List<Vector3>();
        spawn1_rots = new List<Quaternion>();
        spawn2_rots = new List<Quaternion>();
        spawn3_rots = new List<Quaternion>();
        spawn4_rots = new List<Quaternion>();
        monster_ids = new List<int>();
        pending_spawn_monsters = new List<PendingSpawnMonster>();

        m_kTempleID2InstanceID = new Dictionary<int, List<int>>();

    }

    // Use this for initialization
    void Start() 
    {

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child == transform)
                continue;
            if (child.gameObject.name == "m1")
            {
                spawn1_points.Add(child.position);
                spawn1_rots.Add(child.rotation);
            }
            else if (child.gameObject.name == "m2")
            {
                spawn2_points.Add(child.position);
                spawn2_rots.Add(child.rotation);
            }
            else if (child.gameObject.name == "m3")
            {
                spawn3_points.Add(child.position);
                spawn3_rots.Add(child.rotation);
            }
            else if (child.gameObject.name == "m4")
            {
                spawn4_points.Add(child.position);
                spawn4_rots.Add(child.rotation);
            }
        }
		
		if (Global.inFightMap())
		{
			if (RaidManager.Instance.CurrentRaid.isHard) //如果当前关卡是精英关卡,那么刷怪方式不一样
            {
                if (monster1_template_id != -1)
                {
                    monster1_template_id += 100000;
                }

                if (monster2_template_id != -1)
                {
                    monster2_template_id += 100000;
                }

                if (monster3_template_id != -1)
                {
                    monster3_template_id += 100000;
                }

                if (monster4_template_id != -1)
                {
                    monster4_template_id += 100000;
                }
            }	
		}
        

        if (auto_spawn)
        {
            onTrigger();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!triggered)
        {
            return;
        }

        if (MainLogic.sMainLogic.isGameSuspended())
        {
            return;
        }

        if (!spawn_over)
        {
            update_spawn(Time.deltaTime);
            return;
        }

        if (monster_ids.Count == 0)
        {
            if (trigger_delta > 0)
            {
                trigger_delta -= Time.deltaTime;
                return;
            }

            if (monster_clear_handler)
                monster_clear_handler.onTrigger();

            if (Global.inTowerMap())
            {
                MessageManager.Instance.sendReportTowerScore((uint)Global.cur_TowerId); //所有怪物都死亡后才汇报

                uint nNextTowerId = Global.cur_TowerId + 1;
                TowerDataItem di = ConfigDataManager.GetInstance().getTowerConfig().getTowerData((int)nNextTowerId);

                if (di.id == 0)
                {
                    //ReturnToCity.Instance.ReturnType = ReturnToCity.RETURN_TYPE.PASS_DEVIL; //恶魔洞窟全部挑战完毕
                    DemonManager.Instance.OpenDemonAnceWindow(DemonAnceView.DemonArceType.Perfect);
                }
                else
                {
                    uint boxid = (uint)DemonManager.Instance.GetDemonVoById(Global.cur_TowerId).DropOutBoxId;

                    if (boxid != 0)
                    {
                        string boxPrefab = DemonManager.Instance.GetDemonVoById(Global.cur_TowerId).BoxType;

                        BattleEmodongku.GetInstance().ShowTowerAward(boxid, boxPrefab);
                    }
                    


                    Global.cur_TowerId++;

                    if (DemonManager.Instance.CheckCanbeGoing((int)Global.cur_TowerId))
                    {
                        string langStr = LanguageManager.GetText("devil_current_wave")
            + LanguageManager.GetText("devil_wave_prefix") + LanguageManager.GetText("devil_wave_color") +
            DemonManager.Instance.GetDemonVoById(Global.cur_TowerId).Level.ToString()
            + Constant.COLOR_END + LanguageManager.GetText("devil_wave_suffix");
                        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_MAP_NAME_MSG, langStr);
                        GameObject asset = BundleMemManager.Instance.getPrefabByName(di.battlePref, EBundleType.eBundleRaid);
                        BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
                    }
                    else 
                    {
                        DemonManager.Instance.OpenDemonAnceWindow(DemonAnceView.DemonArceType.NextStop);
                    }
                }
            }

            Destroy(gameObject);
        }

        // 刷小哥布林
        if (m_bOneGoblinDie)
        {
            // 哥布林玩法有一个死了 随机选个复活点刷新一个
            int nSpawnIndex = Random.Range(0, spawn1_points.Count);

            GameObject monster = MonsterManager.Instance.spawnMonster((Vector3)spawn1_points[nSpawnIndex],
            (Quaternion)spawn1_rots[nSpawnIndex],
            monster1_template_id);

            if (monster)
            {
                CharacterMonster cm = (CharacterMonster)monster.GetComponent("CharacterMonster");
                cm.owner_area = this;
                monster_ids.Add(cm.monster_property.getInstanceId());
            }

            m_bOneGoblinDie = false;
        }


        // 刷大哥布林
        if (m_bOneGaintGoblinDie)
        {
            // 哥布林玩法有一个死了 随机选个复活点刷新一个
            int nSpawnIndex = Random.Range(0, spawn2_points.Count);

            GameObject monster = MonsterManager.Instance.spawnMonster((Vector3)spawn2_points[nSpawnIndex],
                (Quaternion)spawn2_rots[nSpawnIndex],
                monster2_template_id);

            if (monster)
            {
                CharacterMonster cm = (CharacterMonster)monster.GetComponent("CharacterMonster");
                cm.owner_area = this;
                monster_ids.Add(cm.monster_property.getInstanceId());
            }

            m_bOneGaintGoblinDie = false;
        }
    }

    void spawn() 
    {
		
		float timeToSpawn = spawn_delay;
		float spawn_delta = 1.0f;
		for (int i = 0; i < spawn1_points.Count; i++) {
			Vector3 pos = (Vector3)spawn1_points[i];
			pos.y = 0;
			Quaternion rot = (Quaternion)spawn1_rots[i];
			PendingSpawnMonster psm = new PendingSpawnMonster();
			psm.pos = pos;
			psm.rot = rot;
			psm.template_id = monster1_template_id;

            if (Global.inTowerMap())
            {
                // 根据恶魔洞窟的类型来找对应难度的怪的模板ID
                //TowerDataItem kTowerData = ConfigDataManager.GetInstance().getTowerConfig().getTowerData((int)Global.cur_TowerId);
                //TowerDataItem.ETowerType eType = DemonManager.Instance.GetDemonVoById(Global.cur_TowerId).eTowerType;
                //if (eType != null)
                //{
                //    switch (eType)
                //    {
                //        case TowerDataItem.ETowerType.TT_E_MONG:
                //            psm.template_id += 0;
                //            break;
                //        case TowerDataItem.ETowerType.TT_E_MO:
                //            psm.template_id += 10000;
                //            break;
                //        case TowerDataItem.ETowerType.TT_LIAN_YU:
                //            psm.template_id += 20000;
                //            break;
                //    }
                //}
                setTemplateID(psm);
            }
            
            
			psm.time_to_spawn = timeToSpawn;
			pending_spawn_monsters.Add(psm);
			timeToSpawn += spawn_delta;

            if (!m_kTempleID2InstanceID.ContainsKey(psm.template_id))
            {
                if (Global.inTowerMap())
                {
                    List<int> instan = ConfigDataManager.GetInstance().getMonsterInstanceConfig().GetMonsterInstanceIDInTower(psm.template_id,
                    CharacterPlayer.character_property.getServerMapID());

                    m_kTempleID2InstanceID.Add(psm.template_id, instan);
                }
                else
                {
                    List<int> instan = ConfigDataManager.GetInstance().getMonsterInstanceConfig().GetMonsterInstanceID(psm.template_id,
                    CharacterPlayer.character_property.getServerMapID(),
                    gameObject.name);

                    m_kTempleID2InstanceID.Add(psm.template_id, instan);
                }
            }

            /*
            GameObject monster = MonsterManager.Instance.spawnMonster(pos, rot, monster1_template_id);
            if (monster) {
                CharacterMonster cm = (CharacterMonster)monster.GetComponent("CharacterMonster");
                monster_ids.Add(cm.monster_property.getInstanceId());
            }
            */
        }
		
		for (int i = 0; i < spawn2_points.Count; i++) {
			Vector3 pos = (Vector3)spawn2_points[i];
			pos.y = 0;
			Quaternion rot = (Quaternion)spawn2_rots[i];
			PendingSpawnMonster psm = new PendingSpawnMonster();
			psm.pos = pos;
			psm.rot = rot;
			psm.template_id = monster2_template_id;
			psm.time_to_spawn = timeToSpawn;
			pending_spawn_monsters.Add(psm);
			timeToSpawn += spawn_delta;
		    if (Global.inTowerMap())
		        setTemplateID(psm);

            if (!m_kTempleID2InstanceID.ContainsKey(psm.template_id))
            {
                if (Global.inTowerMap())
                {
                    List<int> instan = ConfigDataManager.GetInstance().getMonsterInstanceConfig().GetMonsterInstanceIDInTower(psm.template_id,
                    CharacterPlayer.character_property.getServerMapID());

                    m_kTempleID2InstanceID.Add(psm.template_id, instan);
                }
                else
                {
                    List<int> instan = ConfigDataManager.GetInstance().getMonsterInstanceConfig().GetMonsterInstanceID(psm.template_id,
                    CharacterPlayer.character_property.getServerMapID(),
                    gameObject.name);

                    m_kTempleID2InstanceID.Add(psm.template_id, instan);
                }
            }
		}
		
		for (int i = 0; i < spawn3_points.Count; i++) {
			Vector3 pos = (Vector3)spawn3_points[i];
			pos.y = 0;
			Quaternion rot = (Quaternion)spawn3_rots[i];
			PendingSpawnMonster psm = new PendingSpawnMonster();
			psm.pos = pos;
			psm.rot = rot;
			psm.template_id = monster3_template_id;
			psm.time_to_spawn = timeToSpawn;
			pending_spawn_monsters.Add(psm);
			timeToSpawn += spawn_delta;
            if (Global.inTowerMap())
                setTemplateID(psm);

            if (!m_kTempleID2InstanceID.ContainsKey(monster3_template_id))
            {
                if (Global.inTowerMap())
                {
                    List<int> instan = ConfigDataManager.GetInstance().getMonsterInstanceConfig().GetMonsterInstanceIDInTower(psm.template_id,
                    CharacterPlayer.character_property.getServerMapID());

                    m_kTempleID2InstanceID.Add(psm.template_id, instan);
                }
                else
                {
                    List<int> instan = ConfigDataManager.GetInstance().getMonsterInstanceConfig().GetMonsterInstanceID(psm.template_id,
                    CharacterPlayer.character_property.getServerMapID(),
                    gameObject.name);

                    m_kTempleID2InstanceID.Add(psm.template_id, instan);
                }
            }
		}
		
		for (int i = 0; i < spawn4_points.Count; i++) {
			Vector3 pos = (Vector3)spawn4_points[i];
			pos.y = 0;
			Quaternion rot = (Quaternion)spawn4_rots[i];
			PendingSpawnMonster psm = new PendingSpawnMonster();
			psm.pos = pos;
			psm.rot = rot;
			psm.template_id = monster4_template_id;
			psm.time_to_spawn = timeToSpawn;
			pending_spawn_monsters.Add(psm);
			timeToSpawn += spawn_delta;
            if (Global.inTowerMap())
                setTemplateID(psm);

            if (!m_kTempleID2InstanceID.ContainsKey(psm.template_id))
            {
                if (Global.inTowerMap())
                {
                    List<int> instan = ConfigDataManager.GetInstance().getMonsterInstanceConfig().GetMonsterInstanceIDInTower(psm.template_id,
                    CharacterPlayer.character_property.getServerMapID());

                    m_kTempleID2InstanceID.Add(psm.template_id, instan);
                }
                else
                {
                    List<int> instan = ConfigDataManager.GetInstance().getMonsterInstanceConfig().GetMonsterInstanceID(monster4_template_id,
                    CharacterPlayer.character_property.getServerMapID(),
                    gameObject.name);

                    m_kTempleID2InstanceID.Add(psm.template_id, instan);
                }
            }
		}


        // 刷怪后的UI提示

        
        if (ConfigDataManager.GetInstance().getMapTipsConfig().HasRecord(CharacterPlayer.character_property.getServerMapID()))
        {
            MapTipsItem item = ConfigDataManager.GetInstance().getMapTipsConfig().getMapTipsData(CharacterPlayer.character_property.getServerMapID());
               
            string tag = item.strAreaName;
            string audio = item.strSoundRes;

            List<int> listIndex = MonsterInThisArea(ConfigDataManager.GetInstance().getMapTipsConfig().getMapTipsData(CharacterPlayer.character_property.getServerMapID()).listMonsterID);
            if (listIndex.Count != 0)
            {
                List<string> tips = ConfigDataManager.GetInstance().getMapTipsConfig().RealShowData(CharacterPlayer.character_property.getServerMapID(), listIndex);
                if (tips.Count > 0)
                {
                    if (gameObject.name == tag)
                    {
                        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_BOSS_TIPS, tips[0]);
                    }
                    else
                    {
                        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_BOSS_TIPS, tips[0]);
                    }
                    if (BundleMemManager.debugVersion)
                    {
                        AudioClip clip = BundleMemManager.Instance.loadResource(audio) as AudioClip;
                        CharacterPlayer.sPlayerMe.audio.PlayOneShot(clip);
                    }
                    else
                    {
                        BundleMemManager.Instance.loadPrefabViaWWW<AudioClip>(EBundleType.eBundleMusic, audio,
                        (obj) =>
                        {
                            AudioClip clip = obj as AudioClip;
                            CharacterPlayer.sPlayerMe.audio.PlayOneShot(clip);
                        });  
                    }                                     
                }

            }
        }
	}

    void update_spawn(float delta)
    {
        // camera curise when boss appear
        // check whether the monster area trigger contain boss
        if (!bSuspendForBoss)
        {
            for (int i = 0; i < pending_spawn_monsters.Count; i++)
            {
                PendingSpawnMonster psm = (PendingSpawnMonster)pending_spawn_monsters[i];

                if (ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(psm.template_id).isTeXie)
                {
                    // here we should count the boss 
                    // because camere cruise till all boss born animation play over.
                    CameraFollow.sCameraFollow.BossBorn(transform.position);
                    bSuspendForBoss = true;
                    triggered = true;
                    return;
                }
            }
        }        
        

        for (int i = 0; i < pending_spawn_monsters.Count; i++)
        {
            PendingSpawnMonster psm = (PendingSpawnMonster)pending_spawn_monsters[i];
            psm.time_to_spawn -= delta;
            if (psm.time_to_spawn < 0)
            {
                //发送刷怪触发器
                if(i==0)
                {
                    Gate.instance.sendNotification(MsgConstant.MSG_GUIDE_SEND_TRIGGER,
        new Trigger(TriggerType.MonsterArea, gameObject.name));
                }

                if (Global.inGoldenGoblin())
                {
                    GameObject monster = MonsterManager.Instance.spawnMonster(psm.pos, psm.rot, psm.template_id);
                    if (monster)
                    {
                        CharacterMonster cm = (CharacterMonster)monster.GetComponent("CharacterMonster");
                        cm.owner_area = this;
                        monster_ids.Add(cm.monster_property.getInstanceId());
                    }
                }
                else
                {
                    if (m_kTempleID2InstanceID.ContainsKey(psm.template_id))
                    {
                        if (m_kTempleID2InstanceID[psm.template_id].Count != 0)
                        {
                            GameObject monster = MonsterManager.Instance.spawnMonster(psm.pos, psm.rot, psm.template_id,
                                m_kTempleID2InstanceID[psm.template_id][0]);


                            if (monster)
                            {
                                CharacterMonster cm = (CharacterMonster)monster.GetComponent("CharacterMonster");
                                cm.owner_area = this;
                                monster_ids.Add(cm.monster_property.getInstanceId());

                                if (ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(psm.template_id).isTeXie)
                                {
                                    // here we should count the boss 
                                    // because camere cruise till all boss born animation play over.
                                    MonsterManager.Instance.m_listBossIDInOnePrefab.Add(cm.monster_property.getInstanceId());
                                }
                            }

                            m_kTempleID2InstanceID[psm.template_id].RemoveAt(0);
                        }
                    }
                }

                pending_spawn_monsters.RemoveAt(i);
                i--;
            }
        }

        if (pending_spawn_monsters.Count == 0)
        {
            spawn_over = true;
        }
    }

    public override void onTrigger()
    {

        if (triggered)
            return;
        spawn();
        triggered = true;

        // 自动打怪 用来删除之前走过的区域信息
        BattleAutomation.GetInstance().DeleteMonsterItem(gameObject.name);
    }

    public void removeMonsterID(int monsterID)
    {

        for (int i = 0; i < monster_ids.Count; i++)
        {
            int id = (int)monster_ids[i];
            if (monsterID == id)
            {
                monster_ids.RemoveAt(i);
                break;
            }
        }
    }

    public List<int> MonsterInThisArea(List<int> ListMonsterID)
    {
        List<int> retValue = new List<int>();

        for (int i = 0; i < ListMonsterID.Count; ++i)
        {
            if ((monster1_template_id == ListMonsterID[i] ||
            monster2_template_id == ListMonsterID[i] ||
            monster3_template_id == ListMonsterID[i] ||
            monster4_template_id == ListMonsterID[i]))
            {
                retValue.Add(i);
            }
        }

        return retValue;
    }

    private void setTemplateID(PendingSpawnMonster psm)
    {
        TowerDataItem.ETowerType eType = DemonManager.Instance.GetDemonVoById(Global.cur_TowerId).eTowerType;
        if (eType != null)
        {
            switch (eType)
            {
                case TowerDataItem.ETowerType.TT_E_MONG:
                    psm.template_id += 0;
                    break;
                case TowerDataItem.ETowerType.TT_E_MO:
                    psm.template_id += 10000;
                    break;
                case TowerDataItem.ETowerType.TT_LIAN_YU:
                    psm.template_id += 20000;
                    break;
            }
        }
    }
}
