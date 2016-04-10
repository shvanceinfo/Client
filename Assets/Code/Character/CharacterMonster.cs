using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CharacterMonster : Character {
	
	public MonsterProperty monster_property;
    public MonsterAI monster_ai;
	public MonsterArea owner_area;

    // 多人副本怪Transform同步信息
    public List<Vector3> m_kMultiMonsterSyncDir = new List<Vector3>();
    public List<Vector3> m_kMultiMonsterSyncPos = new List<Vector3>();

    public float m_fSyncFrequency = 0.0f;
	
	void Awake() {
		init();
	}
	
	// Use this for initialization
	protected void Start () {
		
		monster_ai.init(this);
		
		//setSpeed(monster_property.getMoveSpeed());
		skill.setHurtProtecting(true);

        Renderer[] allRenderer = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in allRenderer)
        {
            //if (!renderer.gameObject.GetComponent("WillRenderTransparent"))
            //{
            //    renderer.gameObject.AddComponent("WillRenderTransparent");
            //}

            // boss 和 精英怪 需要上色
            if (monster_property.GetSurfaceType() == MonsterProperty.MONSTER_SURFACE_LIGHT.MSL_BOSS)
            {
                renderer.sharedMaterial.SetFloat("_RimPower", (0.5f + 8.0f) * 0.7f);
                //renderer.material.SetFloat("_RimPower", 1.0f);
                renderer.sharedMaterial.SetFloat("_UseRim", 1.0f);
                renderer.sharedMaterial.SetColor("_RimColor", new Color(255 / 255.0f, 0, 227 / 255.0f, 0));

                EffectManager.Instance.CreateFX("Effect/Effect_Prefab/Monster/guangguan_BOSS", getTagPoint("shadow"));
            }
            else if (monster_property.GetSurfaceType() == MonsterProperty.MONSTER_SURFACE_LIGHT.MSL_ELITE)
            {
                renderer.sharedMaterial.SetFloat("_RimPower", (0.5f + 8.0f) * 0.7f);
                //renderer.material.SetFloat("_RimPower", 1.0f);
                renderer.sharedMaterial.SetFloat("_UseRim", 1.0f);
                renderer.sharedMaterial.SetColor("_RimColor", new Color(255 / 255.0f, 227 / 255.0f, 0, 0));

                EffectManager.Instance.CreateFX("Effect/Effect_Prefab/Monster/guangguan_jingying", getTagPoint("shadow"));
            }
        }
	}
	
	// Update is called once per frame
	protected void Update () 
    {
        if (m_kMultiMonsterSyncDir.Count != 0 && m_kMultiMonsterSyncPos.Count != 0)
        {
            if (m_fSyncFrequency > 0.1f)
            {
                MessageManager.Instance.sendMessageAskMove(GetProperty().GetInstanceID(),
                    m_kMultiMonsterSyncDir[m_kMultiMonsterSyncDir.Count - 1],
                    m_kMultiMonsterSyncPos[m_kMultiMonsterSyncPos.Count - 1]
                );

                m_kMultiMonsterSyncDir.Clear();
                m_kMultiMonsterSyncPos.Clear();

                m_fSyncFrequency = 0.0f;
            }

            m_fSyncFrequency += Time.deltaTime;
        }

        if (Global.m_bCameraCruise && !CharacterAI.IsInState(this, CharacterAI.CHARACTER_STATE.CS_BORN))
        {
            if (!CharacterAI.IsInState(this, CharacterAI.CHARACTER_STATE.CS_IDLE))
            {
                GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
            }

            return;
        }
        
		if (MainLogic.sMainLogic.isGameSuspended()) 
        {
			//animation.Stop();
			return;
		}

        if (monster_ai != null)
        {
            monster_ai.Update();
        }
        

        UpdateBuff();


        if ((Global.inMultiFightMap() && !CharacterPlayer.character_property.getHostComputer()) ||
            (Global.InWorldBossMap()))
        {
            UpdateAnimationNet();
        }
        else
		    UpdateAnimation();

        UpdateInput();
		
		transform.LookAt(transform.position + face_dir);
	}
	
	
	public void init() 
    {
		
		//DontDestroyOnLoad (transform.gameObject); //marked by ljx, monster don't destroy??
		character_type = CharacterType.CT_MONSTER;
		
		base.init();
		
		monster_property = new MonsterProperty();
        monster_property.SetPropertyOwner(this);

        if (!Global.inMultiFightMap())
        {
            monster_ai = new MonsterAI();
        }
        else
        {
            if (CharacterPlayer.character_property.getHostComputer())
            {
                monster_ai = new MonsterAI();
            }
        }
	}

    public override void InitAppears()
    {
        base.InitAppears();

        m_kGoblinRunAppear = new GoblinRunAppear();
        m_kGoblinRunAppear.setOwner(this);
        m_kGoblinRunAppear.init();

        m_kDieAppear = new MonsterDieAppear();
        m_kDieAppear.setOwner(this);
        m_kDieAppear.init();
    }

	
	void OnDestroy() 
    {
		owner_area.removeMonsterID(monster_property.getInstanceId());

        if (!MonsterManager.Instance.IsThereMonster())
        {
            EffectManager.Instance.EndCloseUpEffect();
        }
	}

    public override void HurtDamager(SkillAppear skill)
    {
        base.HurtDamager(skill);


    }

    public override CharacterBaseProperty GetProperty()
    {
        return monster_property;
    }

    public override CharacterAI GetAI()
    {
        return monster_ai;
    }

    public override void BeHitBackHitFlyHitBroken(SkillAppear skill)
    {
        base.BeHitBackHitFlyHitBroken(skill);

        // 多人副本流程 start
        if (Global.inMultiFightMap())
        {
            // 多人副本没有击退
            if (CharacterPlayer.character_property.getHostComputer())
            {
                // 主机上的怪有AI 
                if (GetProperty().getHP() > 0 )
                {
                    if (ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(monster_property.template_id).isShouJi)
                    {
                        ArrayList param = new ArrayList();
                        param.Add(0.0f);
                        param.Add(Vector3.zero);
                        monster_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
                    }
                }
                else
                {
                    monster_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_DIE);
                }
            }
            else
            {
                // 非主机上的怪没有AI 
                if (GetProperty().getHP() > 0)
                {
                    ChangeAppear(m_kHitAppear);        
                }
                else
                {
                    ChangeAppear(m_kDieAppear);        
                }
            }

            return;
        }
        // 多人副本流程 end

        // 击退击飞击破处理
        Vector3 hitBackDir = transform.position - skill.getOwner().transform.position;
        hitBackDir.Normalize();

        float fKnockBack = skill.getAttackRepel() * monster_property.getRepelSpeed();

        if (GetProperty().getHP() > 0)
        {
            if (ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(monster_property.template_id).isShouJi)
            {
                ArrayList param = new ArrayList();
                param.Add(fKnockBack);
                param.Add(hitBackDir);
                monster_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
            }
        }
        else
        {
            
            monster_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_DIE);
        }
    }
}
