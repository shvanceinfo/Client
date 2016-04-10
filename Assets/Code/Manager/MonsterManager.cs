using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;

public class MonsterManager 
{
	private static  MonsterManager _instance = null;

    List<CharacterMonster> m_kMonsterList = null;
	
	//怪物ID范围 100 - 999
	int current_monster_id = 100;

    //boss instance id list for camera cruise
    public List<int> m_listBossIDInOnePrefab = null;

    public static MonsterManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = new MonsterManager();
            return _instance;
        }
    }

    private MonsterManager()
    {
        m_kMonsterList = new List<CharacterMonster>();
        m_listBossIDInOnePrefab = new List<int>();
    }
	
	public GameObject spawnMonster(Vector3 pos, Quaternion rot, int templateId, int instanceId = 0) 
    {
        MonsterDataItem data_item = ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(templateId);

		if(data_item == null) 
        {
			Loger.Error(templateId.ToString() + " is not exist!");
			return null;
		}
		
		GameObject monster = null;
        GameObject asset = BundleMemManager.Instance.getPrefabByName(data_item.name, EBundleType.eBundleMonster);
		if (asset != null)
		{
            monster = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, rot);
		}
		else
		{
            Debug.LogError("not found in monster manager " + data_item.name);
		}

		if(monster == null) 
        {
            Debug.LogError(data_item.name + " is not exist!");
			return null;
		}

        Collider collider = monster.GetComponent<Collider>();
        collider.isTrigger = true;

        Rigidbody rb = monster.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
		monster.AddComponent("CharacterMonster");
        monster.AddComponent<MonsterSkill>();
        monster.AddComponent("RenderProperty");
        monster.AddComponent<HUD>();
        monster.AddComponent<CoolDownProperty>();
        monster.AddComponent<AISystem.AIPathFinding>();
		
		CharacterMonster cm = monster.GetComponent<CharacterMonster>();
		cm.setFaceDir(rot * Vector3.forward);
		cm.skill = monster.GetComponent<Skill>();
        if (instanceId == 0)
        {
            cm.monster_property.setInstanceId(current_monster_id++);
        }
        else
            cm.monster_property.setInstanceId(instanceId);
        cm.monster_property.setTemplateId(templateId);
		cm.monster_property.setName(data_item.name);
        cm.monster_property.strDesName = data_item.desName;
		cm.monster_property.setLevel(data_item.level);
		cm.monster_property.setType((MonsterProperty.MONSTER_LEVEL_TYPE)data_item.type);
        cm.monster_property.SetSurfaceType((MonsterProperty.MONSTER_SURFACE_LIGHT)data_item.surfaceLight);
		cm.monster_property.setHP(data_item.hp);
		cm.monster_property.setHPMax(data_item.hp);
		cm.monster_property.setAttackPower(data_item.attack_power);
		cm.monster_property.setAttackRange(data_item.attack_range);
		cm.monster_property.setDefence(data_item.defence);
		cm.monster_property.setMoveSpeed(data_item.move_speed);
        cm.monster_property.attack_interval_upper = data_item.attack_interval_upper;
        cm.monster_property.attack_interval_low = data_item.attack_interval_low;
		cm.monster_property.setAttackType(data_item.attack_type);
		cm.monster_property.setRepelSpeed(data_item.repel_speed);
		cm.monster_property.setBroken(data_item.broken);
		cm.monster_property.setBrokenPrefab(data_item.broken_prefab);
		cm.monster_property.setExp(data_item.exp);
		cm.monster_property.setGold(data_item.gold);		
		cm.monster_property.pszDisplayName = data_item.pszDisplayName;
		cm.monster_property.pszDisplayIcon = data_item.pszDisplayIcon;		

		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_Precise, data_item.FPC_Precise);
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_Dodge, data_item.FPC_Dodge);
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_BlastAttack, data_item.FPC_BlastAttack);
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_BlastAttackAdd, data_item.FPC_BlastAttackAdd);
		
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_BlastAttackReduce, data_item.FPC_BlastAttackReduce);
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_Tenacity, data_item.FPC_Tenacity);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_FightBreak, data_item.FPC_FightBreak);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_AntiFightBreak, data_item.FPC_AntiFightBreak);
		
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_IceAttack, data_item.FPC_IceAttack);
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_AntiIceAttack, data_item.FPC_AntiIceAttack);
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_FireAttack, data_item.FPC_FireAttack);
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_AntiFireAttack, data_item.FPC_AntiFireAttack);
		
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_PoisonAttack, data_item.FPC_PoisonAttack);
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_AntiPoisonAttack, data_item.FPC_AntiPoisonAttack);
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_ThunderAttack, data_item.FPC_ThunderAttack);
		cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_AntiThunderAttack, data_item.FPC_AntiThunderAttack);
        cm.transform.position = new Vector3(pos.x, pos.y, pos.z);
		
		m_kMonsterList.Add(cm);
		
        if (CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap()) 
        {
			MessageManager.Instance.sendObjectAppear((uint)cm.monster_property.getInstanceId(),
				(uint)cm.monster_property.getTemplateId(), pos, cm.getFaceDir());
		}
		
		if (cm.monster_property.getType() == MonsterProperty.MONSTER_LEVEL_TYPE.MLT_BOSS)
		{
            Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_BOSS_SHOW,cm.monster_property);
		}

        // 把技能保存
        Skill charSkill = cm.GetComponent<Skill>();
        charSkill.SetSkillOwner(cm);
        
        string monsterName = cm.monster_property.GetName() + " Lv." + cm.monster_property.level;
        monster.GetComponent<HUD>().GenerateHeadUI(monsterName, 0, HUD.HUD_CHARACTER_TYPE.HCT_MONSTER);
        
		return monster;
	}
	
	public GameObject spawnMonsterNet(Vector3 pos, Vector3 dir, int templateId, int instanceId) 
    {	
        MonsterDataItem data_item = ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(templateId);
		if(data_item == null) {
            Debug.LogError(templateId.ToString() + " is not exist!");
			return null;
		}		
		GameObject monster = null;
        GameObject asset = BundleMemManager.Instance.getPrefabByName(data_item.name, EBundleType.eBundleMonster);
		if (asset != null)
		{
            monster = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
		}
		else
		{
			Debug.LogError("not found in monster manager " + data_item.name);
		}
		
		if(monster == null) {
            Debug.LogError(data_item.name + " is not exist!");
			return null;
		}
		
		Collider collider = monster.GetComponent<Collider>();
		collider.isTrigger = true;
		
		Rigidbody rb =  monster.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.isKinematic = true;
        monster.AddComponent<CharacterMonsterNet>();
        monster.AddComponent<MonsterSkill>();
        monster.AddComponent<RenderProperty>();
        monster.AddComponent<HUD>();

		//monster.AddComponent("AnimationController");

        CharacterMonsterNet cm = monster.GetComponent<CharacterMonsterNet>();
		cm.setFaceDir(dir);
		cm.skill = monster.GetComponent<Skill>();
		//cm.animation_controller = monster.GetComponent<AnimationController>();

        cm.monster_property.setInstanceId(instanceId);
		cm.monster_property.setTemplateId(templateId);
		cm.monster_property.setName(data_item.name);
        cm.monster_property.strDesName = data_item.desName;
        cm.monster_property.setLevel(data_item.level);
        cm.monster_property.setType((MonsterProperty.MONSTER_LEVEL_TYPE)data_item.type);
        cm.monster_property.SetSurfaceType((MonsterProperty.MONSTER_SURFACE_LIGHT)data_item.surfaceLight);
        cm.monster_property.setHP(data_item.hp);
        cm.monster_property.setHPMax(data_item.hp);
        cm.monster_property.setAttackPower(data_item.attack_power);
        cm.monster_property.setAttackRange(data_item.attack_range);
        cm.monster_property.setDefence(data_item.defence);
        cm.monster_property.setMoveSpeed(data_item.move_speed);
        cm.monster_property.attack_interval_upper = data_item.attack_interval_upper;
        cm.monster_property.attack_interval_low = data_item.attack_interval_low;
        cm.monster_property.setAttackType(data_item.attack_type);
        cm.monster_property.setRepelSpeed(data_item.repel_speed);
        cm.monster_property.setBroken(data_item.broken);
        cm.monster_property.setBrokenPrefab(data_item.broken_prefab);
        cm.monster_property.setExp(data_item.exp);
        cm.monster_property.setGold(data_item.gold);
        cm.monster_property.pszDisplayName = data_item.pszDisplayName;
        cm.monster_property.pszDisplayIcon = data_item.pszDisplayIcon;

        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_Precise, data_item.FPC_Precise);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_Dodge, data_item.FPC_Dodge);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_BlastAttack, data_item.FPC_BlastAttack);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_BlastAttackAdd, data_item.FPC_BlastAttackAdd);

        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_BlastAttackReduce, data_item.FPC_BlastAttackReduce);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_Tenacity, data_item.FPC_Tenacity);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_FightBreak, data_item.FPC_FightBreak);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_AntiFightBreak, data_item.FPC_AntiFightBreak);

        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_IceAttack, data_item.FPC_IceAttack);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_AntiIceAttack, data_item.FPC_AntiIceAttack);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_FireAttack, data_item.FPC_FireAttack);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_AntiFireAttack, data_item.FPC_AntiFireAttack);

        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_PoisonAttack, data_item.FPC_PoisonAttack);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_AntiPoisonAttack, data_item.FPC_AntiPoisonAttack);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_ThunderAttack, data_item.FPC_ThunderAttack);
        cm.monster_property.fightProperty.fightData.Add(eFighintPropertyCate.eFPC_AntiThunderAttack, data_item.FPC_AntiThunderAttack);


        cm.transform.position = new Vector3(pos.x, pos.y, pos.z);

		m_kMonsterList.Add(cm);

        string monsterName = cm.monster_property.GetName() + " Lv." + cm.monster_property.level;
        monster.GetComponent<HUD>().GenerateHeadUI(monsterName, 0, HUD.HUD_CHARACTER_TYPE.HCT_MONSTER);

        if (cm.monster_property.getType() == MonsterProperty.MONSTER_LEVEL_TYPE.MLT_BOSS)
        {
            Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_BOSS_SHOW, cm.monster_property);
        }

		return monster;
	}
	
	public void destroyMonster(CharacterMonster monster, float deltaTime = 0) 
    {	
		for (int i = 0; i < m_kMonsterList.Count; i++) 
        {
            if (m_kMonsterList[i] == monster) 
            {
                Object.Destroy(m_kMonsterList[i].gameObject, deltaTime);
				m_kMonsterList.RemoveAt(i);
                break;
			}
		}
	}
	
	public void destroyMonster(int instanceID, float deltaTime = 0) 
    {
        for (int i = 0; i < m_kMonsterList.Count; i++) 
        {
            if (m_kMonsterList[i].monster_property.getInstanceId() == instanceID) 
            {
                Object.Destroy(m_kMonsterList[i].gameObject, deltaTime);
				m_kMonsterList.RemoveAt(i);
                break;
			}
		}

        if (!IsThereMonster())
        {
            EffectManager.Instance.EndCloseUpEffect();
        }
	}
	
	public void destroyAllMonster() 
    {
        for (int i = 0; i < m_kMonsterList.Count; i++) 
        {
            if (m_kMonsterList[i] != null)
            {
                Object.Destroy(m_kMonsterList[i].gameObject);
            }
		}

		m_kMonsterList.Clear();

        EffectManager.Instance.EndCloseUpEffect();
	}
	
	public bool isMonsterExist(int instanceID) 
    {

        for (int i = 0; i < m_kMonsterList.Count; i++)
        {
            if (m_kMonsterList[i].monster_property.getInstanceId() == instanceID)
            {
				return true;
			}
		}
		
		return false;
	}
	
	public CharacterMonster GetMonster(int instanceID) 
    {
        for (int i = 0; i < m_kMonsterList.Count; i++) 
        {
            if (m_kMonsterList[i].monster_property.getInstanceId() == instanceID)
            {
                if (!CharacterAI.IsInState(m_kMonsterList[i], CharacterAI.CHARACTER_STATE.CS_DIE))
                {
                    return m_kMonsterList[i];
                }
            }
		}
		
		return null;
	}

	public CharacterMonster getPlayerNearestMonster() 
    {
		CharacterMonster monster = null;
		
		float minDist = CharacterPlayer.character_property.getAttackRange() + 1.5f;
		Vector3 dir = Vector3.zero;
		for (int i = 0; i < m_kMonsterList.Count; i++) {
			
			CharacterMonster lookMonster = (CharacterMonster)m_kMonsterList[i];

            if (CharacterAI.IsInState(lookMonster, CharacterAI.CHARACTER_STATE.CS_DIE))
				continue;
			
			if (lookMonster.skill.getHurtHide())
				continue;
			
			//simply filter backward enemy
			Vector3 posDir = lookMonster.transform.position - CharacterPlayer.sPlayerMe.transform.position;
			float tmp = Vector3.Dot(posDir, CharacterPlayer.sPlayerMe.getFaceDir());
			if (tmp < 0)
				continue;
			
			float dist = Vector3.Distance(CharacterPlayer.sPlayerMe.transform.position, lookMonster.transform.position);
			CapsuleCollider cc = lookMonster.GetComponent<CapsuleCollider>();
			if (cc) {
				minDist += cc.radius * lookMonster.transform.localScale.x;
			}
			if (dist < minDist) 
            {
				minDist = dist;
				monster = lookMonster;
			}
		}

        return monster;
	}

    // 距离最近的怪
    public CharacterMonster GetNearestMonster(out float fDist)
    {
        CharacterMonster monster = null;

        if (m_kMonsterList.Count == 0)
        {
			fDist = 0.0f;
            return null;
        }
        
        float minDist = 99999.0f;
        
        int nMonsterIndex = 0;

        for (int i = 0; i < m_kMonsterList.Count; i++)
        {
            CharacterMonster lookMonster = (CharacterMonster)m_kMonsterList[i];

            if (CharacterAI.IsInState(lookMonster, CharacterAI.CHARACTER_STATE.CS_DIE))
                continue;

            if (lookMonster.skill.getHurtHide())
                continue;

            float dist = Vector3.Distance(CharacterPlayer.sPlayerMe.transform.position, lookMonster.transform.position);
            
            if (dist < minDist)
            {
                minDist = dist;
                nMonsterIndex = i;
            }
        }

        if (minDist != 99999.0f)
        {
            monster = (CharacterMonster)m_kMonsterList[nMonsterIndex];
			fDist = minDist;
            return monster;
        }

		fDist = 0.0f;
        return null;
    }

    // 得到一定角度内的怪
    public List<CharacterMonster> GetAngleRangleMonster(int nDegree, float radius)
    {
        List<CharacterMonster> ret = new List<CharacterMonster>();


        if (m_kMonsterList.Count == 0)
        {
            return null;
        }

        //float minDist = 99999.0f;

        //int nMonsterIndex = 0;

        for (int i = 0; i < m_kMonsterList.Count; i++)
        {
            CharacterMonster lookMonster = (CharacterMonster)m_kMonsterList[i];

            if (CharacterAI.IsInState(lookMonster, CharacterAI.CHARACTER_STATE.CS_DIE))
                continue;

            if (lookMonster.skill.getHurtHide())
                continue;

            float dist = Vector3.Distance(CharacterPlayer.sPlayerMe.transform.position, lookMonster.transform.position);

            if (dist > radius)
            {
                continue;
            }

            

            //simply filter backward enemy
            Vector3 posDir = lookMonster.transform.position - CharacterPlayer.sPlayerMe.transform.position;
            float tmp = Vector3.Dot(posDir, CharacterPlayer.sPlayerMe.getFaceDir());
            if (tmp < Mathf.Cos(nDegree * 0.5f * Mathf.PI / 180.0f))
                continue;


            ret.Add(lookMonster);

        }

        return ret;
    }
	
    // 得到离某点最近的怪
	public Character GetPointNearestMonster(Vector3 pos, float r) 
    {
        float fMinDist = float.MaxValue;
        int index = -1;

		for (int i = 0; i < m_kMonsterList.Count; i++) 
        {
			
			CharacterMonster lookMonster = (CharacterMonster)m_kMonsterList[i];

            if (CharacterAI.IsInState(lookMonster, CharacterAI.CHARACTER_STATE.CS_DIE))
				continue;

            // 怪物如果在出生状态 continue
            //if (lookMonster.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_BORN)
            //{
            //    continue;
            //}
            

            if (lookMonster.m_eAppearState == Appear.BATTLE_STATE.BS_BORN)
            {
                continue;
            }
            
			
			Vector3 posDir = lookMonster.getPosition() - pos;
			
			float dist = posDir.magnitude;

            if (dist < r && dist < fMinDist) 
            {
                fMinDist = dist;
                index = i;
			}
		}

        if (index != -1)
        {
            return (Character)m_kMonsterList[index];
        }
        
		
		return null;
	}
	
	public void getMonsterNeighbors(CharacterMonster m, float r, out ArrayList neighbors) {
		
		ArrayList ns = new ArrayList();
		for (int i = 0; i < m_kMonsterList.Count; i++) {
			CharacterMonster monsterComp = (CharacterMonster)m_kMonsterList[i];
			if (monsterComp == m) {
				continue;
			}
			
			Vector3 toNeighbor = monsterComp.getPosition() - m.getPosition();
			if (toNeighbor.sqrMagnitude < r * r) {
				ns.Add(monsterComp);
			}
		}
		
		neighbors = ns;
	}
	
	public void ClearHasBeenDamagedFlag() 
    {
		for (int i = 0; i < m_kMonsterList.Count; i++) 
        {
			CharacterMonster monsterComp = (CharacterMonster)m_kMonsterList[i];
			monsterComp.m_bHasBeenDamaged = false;
		}
	}
	
	public void onEnterLevel() 
    {		
		destroyAllMonster();
		current_monster_id = 100;

        // 多人副本刷怪在这之后
        if (Global.inMultiFightMap() && !CharacterPlayer.character_property.getHostComputer())
        {
            BattleMultiPlay.GetInstance().SpawnMultiMonsterByInfo();
        }

        if (Global.InWorldBossMap())
        {
            BattleWorldBoss.GetInstance().SpawnWorldBossInfo();
        }
	}

    public bool IsLastMonster(CharacterMonster character)
    {
        if (Global.inTowerMap())
        {
            int nEmptyNum = 0;
            for (int i = 0; i < m_kMonsterList.Count; ++i)
            {
                if (m_kMonsterList[i].gameObject.GetComponent<CharacterMonster>().GetProperty().getHP() <= 0)
                {
                    nEmptyNum++;
                }
            }

            if (nEmptyNum == m_kMonsterList.Count)
            {
                // 全死了
                return true;
            }
        }
        else
        {
            if (character == null)
            {
                return false;
            }

            if (character.owner_area == null)
            {
                return false;
            }

            if (character.owner_area.monster_clear_handler == null)
            {
                return false;
            }

            // 如果是最后一个子场景最后一只怪 需要慢动作播放
            if (character.owner_area.monster_clear_handler.GetType() == typeof(Box))
            {
                Box lastBox = character.owner_area.monster_clear_handler as Box;

                if (lastBox.is_complete)
                {
                    int nEmptyNum = 0;
                    for (int i = 0; i < m_kMonsterList.Count; ++i)
                    {
                        if (m_kMonsterList[i].gameObject.GetComponent<CharacterMonster>().GetProperty().getHP() <= 0)
                        {
                            nEmptyNum++;
                        }
                    }

                    if (nEmptyNum == m_kMonsterList.Count)
                    {
                        // 全死了
                        return true;
                    }
                }

            }
        }
        
        return false;
    }

    public bool IsThereMonster()
    {
        return m_kMonsterList.Count != 0;
    }

    // check monster id if in boss list for camera cruise
    public bool MonsterIDInBossList(int bossId)
    {
        bool bFound = false;

        for (int i = 0; i < m_listBossIDInOnePrefab.Count; ++i )
        {
            if (m_listBossIDInOnePrefab[i] == bossId)
            {
                m_listBossIDInOnePrefab.RemoveAt(i);
                bFound = true;

                break;
            }
        }

        if (bFound && m_listBossIDInOnePrefab.Count == 0)
        {
            return true;
        }

        return false;
    }

    public List<Vector3> GetOtherMonsters(CharacterMonster kMe)
    {
        List<Vector3> kRet = new List<Vector3>();

        for (int i = 0; i < m_kMonsterList.Count; ++i)
        {
            if (m_kMonsterList[i] != kMe)
            {
                kRet.Add(m_kMonsterList[i].getPosition());
            }
        }

        return kRet;
    }
}
