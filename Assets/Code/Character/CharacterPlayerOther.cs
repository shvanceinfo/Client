using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using helper;

public class PlayerOtherSkill
{
	public int skill_id;
	public Vector3 pos;
}

public class CharacterPlayerOther : Character
{
	
	public CharacterOtherProperty character_other_property;
	public Vector3 m_kMovePath = Vector3.zero;
    public List<PlayerOtherSkill> skill_caches = null;
	public PlayerOtherAI playerOther_ai;
	public List<Vector3> m_kXuanFengZhanPos = new List<Vector3> ();
	public List<Vector3> m_kCommonAttackPos = new List<Vector3> ();
	public BattleMultiInfo m_battleMutiInfo;
	
	void Awake ()
	{
		init ();
	}
	
	// Use this for initialization
	protected void Start ()
	{
		Renderer[] allRenderer = GetComponentsInChildren<Renderer> ();
		foreach (Renderer renderer in allRenderer) {
            if (renderer != null)
            {
                if (renderer.sharedMaterial != null)
                {
                    renderer.sharedMaterial.SetFloat("_UseRim", 0.0f);
                }
            }
		}
	}
	
	// Update is called once per frame
	protected void Update ()
	{
		
		/*
		if (MainLogic.sMainLogic.isGameSuspended()) {
			return;
		}
		*/
		playerOther_ai.Update ();

		UpdateBuff ();

		UpdateAnimation ();

		UpdateInput ();
		
		transform.LookAt (transform.position + face_dir);
	}
	
	public void init ()
	{
		//SceneManager.Instance.setDontDestroyObj (transform.gameObject);
		//DontDestroyOnLoad (transform.gameObject);  //marked by ljx, don't destroy other player??
		character_type = CharacterType.CT_PLAYEROTHER;

		playerOther_ai = new PlayerOtherAI ();

		base.init ();
		
        
		//GetProperty().setMoveSpeed(3.5f);

        
		playerOther_ai.init (this);

		character_avatar = new CharacterAvatar ();
		character_avatar.init (this);

        skill_caches = new List<PlayerOtherSkill>();
		
		
		
		
	}

	public override void InitAppears ()
	{
		base.InitAppears ();

		m_kDieAppear = new PlayerOtherDieAppear ();
		m_kDieAppear.setOwner (this);
		m_kDieAppear.init ();
	}

	public override CharacterAI GetAI ()
	{
		return playerOther_ai;
	}
	
	public override void applyProperty ()
	{
		if (character_other_property.getCareer () == CHARACTER_CAREER.CC_ARCHER) {
			character_other_property.setAttackRange (5.0f);
		} else if (character_other_property.getCareer () == CHARACTER_CAREER.CC_MAGICIAN) {
			character_other_property.setAttackRange (5.0f);
		} else
			character_other_property.setAttackRange (1.5f);

		character_other_property.setMoveSpeed (4.0f);
		equipItem (character_other_property.getWeapon ());
		equipItem (character_other_property.getArmor ());

		// 挂上称号
        GetComponent<HUD>().GenerateHeadUI(character_other_property.GetName(), character_other_property.title_id, HUD.HUD_CHARACTER_TYPE.HCT_PLAYER, false, false,
                (int)character_other_property.medalID);
	}

	public void equipTitle ()
	{
        string othername = string.Format("[{0}]{1}:[-]",ColorConst.Color_LiangHuang,character_other_property.nick_name);
        GetComponent<HUD>().SetHeadUIData(character_other_property.nick_name,
                character_other_property.title_id, HUD.HUD_CHARACTER_TYPE.HCT_PLAYER);
	}
	
	public void equipItem (int id, CHARACTER_CAREER inCareer = CHARACTER_CAREER.CC_SWORD)
	{
		
		EquipmentTemplate et = EquipmentManager.GetInstance ().GetTemplateByTempId ((uint)id);
		if (et.id != 0) {
			BodyPartInfo bpi = new BodyPartInfo ();
			switch (et.part) {
			case eEquipPart.eSuit:
				bpi.body_part = BodyPart.BP_ARMOR;
				break;
			case eEquipPart.eGreatSword:
				bpi.body_part = BodyPart.BP_WEAPON;
				break;
			}
			CHARACTER_CAREER career;
			if (character_other_property != null)
				career = character_other_property.career;
			else
				career = inCareer;
			switch (career) {
			case CHARACTER_CAREER.CC_SWORD:
				bpi.texture_name = et.swordTexture;
				break;
			case CHARACTER_CAREER.CC_ARCHER:
				bpi.texture_name = et.archerTexture;
				break;
			case CHARACTER_CAREER.CC_MAGICIAN:
				bpi.texture_name = et.magicianTexture;
				break;
			default:
				break;
			}
			bpi.model_name = et.model_name;
			bpi.weapon_trail_texture = et.weapon_trail_texture;
			bpi.effect_name = et.effect_name;
			character_avatar.changeBodyPart (bpi,career);
			if (character_other_property != null && character_other_property.wingID > 0)
				character_avatar.installWing (character_other_property.wingID);
		}
		
		//Renderer[] allRenderer = GetComponentsInChildren<Renderer>();
		//foreach (Renderer renderer in allRenderer) {
		//    if (!renderer.gameObject.GetComponent("WillRenderTransparent")) {
		//        renderer.gameObject.AddComponent("WillRenderTransparent");
		//    }
		//}
	}
	
	public void SetMovePath (Vector3 pos)
	{
		m_kMovePath = pos;
	}
	
	public void pushSkill (PlayerOtherSkill otherSkill)
	{
		
		skill_caches.Add (otherSkill);
	}
	
	public override void UpdateInput ()
	{
		if (CharacterAI.IsInState(this, CharacterAI.CHARACTER_STATE.CS_DIE))
		{
            return;
		}
		
		if (skill_caches.Count > 0) 
        {
			PlayerOtherSkill otherSkill = skill_caches [0];
			skill_caches.RemoveAt (0);

			switch (otherSkill.skill_id) {
			//case 201003:
			//    ChangeAppear(skill.InitSkill(new SkillWhirlWind(201003)));
			//    break;
			//case 400001:
			//    ChangeAppear(skill.InitSkill(new SkillRollCmd(400001)));
			//    break;
			//case 300002:
			//    ChangeAppear(skill.InitSkill(new FireStorm(300002)));
			//    break;
			case 400002001:
				ChangeAppear (skill.InitSkill (new SkillFlash (400002001)));
				break;
			case 400003001:
				ChangeAppear (skill.InitSkill (new SkillMagFlash (400003001)));
				break;
			//case 203202:
			//    ChangeAppear(skill.InitSkill(new SkillFlashBack(203202)));
			//    break;
			default:
				ChangeAppear (skill.CreateSkill (otherSkill.skill_id));
				break;
			}

            

            
			//if (m_kSkillAnimShowAppear.showSkill(otherSkill.skill_id)) 
			//{
			//    m_kSmoothPosAppear.smoothToPos(otherSkill.pos);
			//    m_kSmoothPosAppear.setNextCmd(m_kSkillAnimShowAppear);
			//    ChangeAppear(m_kSmoothPosAppear);
			//}
			return;
		}

		if (m_kMovePath != Vector3.zero) 
        {
			moveTo (m_kMovePath);
			m_kMovePath = Vector3.zero;
		}
	}

	public override void HurtDamager (SkillAppear skill)
	{
		base.HurtDamager (skill);
	}

	public override CharacterBaseProperty GetProperty ()
	{
		return character_other_property;
	}
    
	public override void BeHitBackHitFlyHitBroken (SkillAppear skill)
	{
		base.BeHitBackHitFlyHitBroken (skill);

		if (GetProperty ().getHP () <= 0) 
        {
			playerOther_ai.SendStateMessage (CharacterAI.CHARACTER_STATE.CS_DIE);
		} 
        else 
        {
			SkillHurtProcess (skill);
		}
	}

	public void SkillHurtProcess (SkillAppear skill)
	{
		float fKnockBack = skill.getAttackRepel () * GetProperty ().fightProperty.GetValue (eFighintPropertyCate.eFPC_KnockBack);

		ArrayList param = new ArrayList ();

		if (skill.getOwner ().getType () == CharacterType.CT_PLAYER) 
		{
			// 主角人打其他人
			if (!Global.InArena())
			{
				CharacterPlayer player = skill.getOwner () as CharacterPlayer;
				
				Vector3 vecOtherSkillDir = transform.position - player.transform.position;
				vecOtherSkillDir.Normalize ();
				
				EffectManager.Instance.createFX("Effect/Effect_Prefab/Role/Other/shouji", getTagPoint("help_body"));
				
				param.Add (fKnockBack);
				param.Add (vecOtherSkillDir);
				
				playerOther_ai.SendStateMessage (CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
			}

			return;
		}



		// 怪打其他人


		CharacterMonster monster = skill.getOwner () as CharacterMonster;

		Vector3 vecSkillDir = transform.position - monster.transform.position;
		vecSkillDir.Normalize ();

		// 特效
        EffectManager.Instance.createFX("Effect/Effect_Prefab/Role/Other/shouji", getTagPoint("help_body"));


		param.Add (fKnockBack);
		param.Add (vecSkillDir);

		switch (monster.monster_property.getType ()) {
		case MonsterProperty.MONSTER_LEVEL_TYPE.MLT_BOSS:
			{
				playerOther_ai.SendStateMessage (CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
			}
			break;
		case MonsterProperty.MONSTER_LEVEL_TYPE.MLT_ELITE:
			{
				// 怪的状态目前跟人物不通用 暂时这样吧
				if (monster.GetAI ().GetCharacterState () == CharacterAI.CHARACTER_STATE.CS_SKILL) {
					playerOther_ai.SendStateMessage (CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
				} else {
					// 有一定几率进入受击
					if (Random.Range (0.0f, 1.0f) < 0.2f) {
						playerOther_ai.SendStateMessage (CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
					} else {
						if (playerOther_ai.GetCharacterState () == CharacterAI.CHARACTER_STATE.CS_IDLE) {
							playerOther_ai.SendStateMessage (CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
						}
					}
				}
			}
			break;
		case MonsterProperty.MONSTER_LEVEL_TYPE.MLT_COMMON:
			{
				if (monster.GetAI ().GetCharacterState () == CharacterAI.CHARACTER_STATE.CS_SKILL) {
					playerOther_ai.SendStateMessage (CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
				} else {
					if (playerOther_ai.GetCharacterState () == CharacterAI.CHARACTER_STATE.CS_IDLE) {
						playerOther_ai.SendStateMessage (CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
					}
				}
			}
			break;
		default:
			{

			}
			break;
		}
	}

	void OnDestroy ()
	{
		PlayerManager.Instance.CheckDestoryOther(this);

		if (Global.InArena ()) 
        {
            EffectManager.Instance.EndCloseUpEffect();
		}

		if (m_kPet != null) 
		{
			GameObject.Destroy(m_kPet.gameObject);
		}
	}

    // 其他人的碰撞 用来做表现同步 比如战士的普通攻击在贴近敌人会停止前移 需要
    void OnTriggerEnter(Collider other)
    {
        OnTriggerUpdate(other);
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerUpdate(other);
    }


    void OnTriggerUpdate(Collider other)
    {
        if (Global.InArena())
        {
            Character target = other.gameObject.GetComponent<Character>();

            // 其他非生物
            if (target == null)
            {
                return;
            }

            // 自己碰自己
            if (target == this)
            {
                return;
            }

            // 碰撞检测 如果是在普通攻击时候不能穿过对象
            if (GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_ATTACK)
            {
                SkillAttackBaseCmd kSkillBase = skill.getCurrentSkill() as SkillAttackBaseCmd;
                if (kSkillBase != null)
                {
                    kSkillBase.m_nColliderStopValue = 0;
                }
            }
            else if (CharacterAI.IsInState(this, CharacterAI.CHARACTER_STATE.CS_SKILL))
            {
                // 如果是冲撞 碰到人需要停下
                if (skill.getCurrentSkill() != null)
                {
                    int preSkillID = skill.getCurrentSkill().GetSkillId() / 1000;
                    if (preSkillID == 201001)
                    {
                        skill.getCurrentSkill().m_bSkillIntrupted = true;
                    }
                }
            }
        }
    }
	
	//返回状态
	public PeopleStatus Status {
		get {
			if (m_battleMutiInfo.Status == PeopleStatus.DROP) {
				return PeopleStatus.DROP;
			}
			if (this.character_other_property.getHP () >= 0) {
				return PeopleStatus.NORMAL;
			} else {
				return PeopleStatus.DEAD;
			}
			
		}
		
	}


    public override void SetWeaponAvailable(bool bAvailable)
    {
        Weapon w = GetComponentInChildren<Weapon>();

        if (w != null)
        {
            w.GetComponent<BoxCollider>().enabled = bAvailable;
            w.m_bStartSkill = bAvailable;
        }
    }
	
}
