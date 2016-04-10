using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;


public enum CharacterType 
{
	CT_NULL,
	CT_PLAYER,
	CT_MONSTER,
	CT_PLAYERUI,
	CT_PLAYEROTHER,
}

public enum SkillCanNotCastReason
{
    SCNCR_CAST = 0,                     // 可以放
    SCNCR_MP_NOT_ENOUGH,        // 蓝不够
    SCNCR_SKILL_IN_CD,                  // CD中
    SCNCR_IS_DEAD,                      // 死亡
    SCNCR_IN_SKILL_STATUS,          // 技能状态
    SCNCR_IN_CITY,                      // 主城中
    SCNCR_NO_ENEMY                      // 没有敌人 并且技能不能平放
}

public abstract class Character : MonoBehaviour 
{
	
	protected CharacterType character_type = CharacterType.CT_NULL;
    
	protected Vector3 velocity = Vector3.zero;
	protected Vector3 face_dir = Vector3.forward;
	protected Vector3 body_center = Vector3.zero;
	protected Vector3 old_position = Vector3.zero;
	
	public CharacterAvatar character_avatar;
	public WeaponTrail trail;
    public Skill skill;


    
	
	
	public BornAppear m_kBornAppear;
	public IdleAppear m_kIdleAppear;
	public MoveAppear m_kMoveAppear;
	public HitAppear m_kHitAppear;
    public HitBackAppear m_kHitBackAppear;
	public DizzyAppear m_kDizzyAppear;
	public DieAppear m_kDieAppear;
	public SkillAnimationShowAppear m_kSkillAnimShowAppear;
	public SmoothPosAppear m_kSmoothPosAppear;
    public PursueAppear m_kPursueAppear;
    public GoblinRunAppear m_kGoblinRunAppear;
    


	public GameObject hp_bar;

    public Appear.BATTLE_STATE m_eAppearState;
	
	protected Appear m_kAppear;

    // 是否被某个技能伤害过 每次每个技能只能造成一次伤害
    public bool m_bHasBeenDamaged = false;



    public Dictionary<BUFF_TYPE, BaseBuff> m_dirBuff = new Dictionary<BUFF_TYPE, BaseBuff>();
    public int m_nBuffState = 0;

    // 是否已经死亡 在多人副本中由主机来决定
    public bool m_bIsNetDead = false;


    // 所有的character都有可能会带宠物
    public CharacterPet m_kPet = null;

	public bool m_bVisible = true;

	public bool m_bOutCam = false;

    // 属性虚接口
    abstract public CharacterBaseProperty GetProperty();

    public virtual CharacterAI GetAI()
    {
        return null;
    }

    // 角色动画控制器
    public CharacterPlayerAnimatorControl m_kAnimControl;
    

    public virtual void SetState(Appear.BATTLE_STATE battleState)
    {
        m_eAppearState = battleState;
    }

    public virtual SkillCanNotCastReason CanCastSkill(int nSkillId)
    {
        /*if (getType() != CharacterType.CT_MONSTER)
        {
            if (ConfigDataManager.GetInstance().getSkillConfig().getSkillData(nSkillId).mp_cost >
            GetProperty().GetMP())
            {
                return SkillCanNotCastReason.SCNCR_MP_NOT_ENOUGH;
            }
        }*/
        

        if (GetComponent<CoolDownProperty>().IsInCD(nSkillId))
        {
            return SkillCanNotCastReason.SCNCR_SKILL_IN_CD;
        }

        // 通用状态
        if (GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_DIE)
        {
            return SkillCanNotCastReason.SCNCR_IS_DEAD;
        }

        // 技能状态
        if (GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_SKILL)
        {
            return SkillCanNotCastReason.SCNCR_IN_SKILL_STATUS;
        }

        if (Global.inCityMap())
        {
            return SkillCanNotCastReason.SCNCR_IN_CITY;
        }
        
        // BUFF 状态

        return SkillCanNotCastReason.SCNCR_CAST;
    }

    // 人物放技能
    public virtual bool AICastSkill(Character target, int skillId)
    {
        if (CanCastSkill(skillId) == SkillCanNotCastReason.SCNCR_CAST)
        {
            CastSkill(target, skillId);
            return true;
        }

        return false;
    }

    // 直接释放
    public virtual void CastSkill(Character target, int skillId)
    {
        // 扣蓝 放技能
        int nMP = GetProperty().GetMP();
        int nMPCost = ConfigDataManager.GetInstance().getSkillConfig().getSkillData(skillId).mp_cost;
        GetProperty().SetMP(nMP - nMPCost);

        skill.setSkillTarget(target);
        GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_SKILL, skillId);
    }


	public CharacterType getType() {
		
		return character_type;
	}
	
	public void ChangeAppear(Appear cmd) 
    {
        if (m_kAppear != null)
        {
            m_kAppear.deActive();
            m_kAppear = null;
        }

        m_kAppear = cmd;
        m_kAppear.active();
	}

    public void ChangeAppear(Appear.BATTLE_STATE battlestate)
    {
        switch (battlestate)
        {
        case Appear.BATTLE_STATE.BS_BORN:
            ChangeAppear(m_kBornAppear);
        	break;
        case Appear.BATTLE_STATE.BS_DIE:
            ChangeAppear(m_kDieAppear);
            break;
        case Appear.BATTLE_STATE.BS_PING_KAN:
            ChangeAppear(skill.GetCommonAttack());
            break;
        case Appear.BATTLE_STATE.BS_MOVE:
            ChangeAppear(m_kMoveAppear);
            break;
        case Appear.BATTLE_STATE.BS_PURSUE:
            ChangeAppear(m_kPursueAppear);
            break;
        case Appear.BATTLE_STATE.BS_SKILL:
            ChangeAppear(skill.getCurrentSkill());
            break;
        case Appear.BATTLE_STATE.BS_IDLE:
            ChangeAppear(m_kIdleAppear);
            break;

        }
    }
	
	public void setFaceDir(Vector3 dir) 
    {
        if (dir == Vector3.zero)
        {
            return;
        }
		
		face_dir = dir;
        face_dir.y = 0.0f;
        face_dir.Normalize();
		transform.LookAt(transform.position + face_dir);
	}
	
	public Vector3 getFaceDir() {
	    	
		return face_dir;
	}
	
	public void setVelocity(Vector3 v) {
		
		velocity = v;
	}
	
	public Vector3 getVelocity() {
		
		return velocity;
	}
	
	public Vector3 getBodyCenter() {
		
		return body_center;
	}
	
	public virtual void InitAppears() {
	
		m_kAppear = new Appear();
		
		m_kBornAppear = new BornAppear();
		m_kBornAppear.setOwner(this);
		m_kBornAppear.init();
		
		m_kIdleAppear = new IdleAppear();
		m_kIdleAppear.setOwner(this);
		m_kIdleAppear.init();
		
		m_kMoveAppear = new MoveAppear();
		m_kMoveAppear.setOwner(this);
		m_kMoveAppear.init();
		
		m_kHitAppear = new HitAppear();
		m_kHitAppear.setOwner(this);
		m_kHitAppear.init();

        m_kHitBackAppear = new HitBackAppear();
        m_kHitBackAppear.setOwner(this);
        m_kHitBackAppear.init();
		
		m_kDizzyAppear = new DizzyAppear();
		m_kDizzyAppear.setOwner(this);
		m_kDizzyAppear.init();
	
		
        //m_kDieAppear = new DieAppear();
        //m_kDieAppear.setOwner(this);
        //m_kDieAppear.init();
		
		m_kSkillAnimShowAppear = new SkillAnimationShowAppear();
		m_kSkillAnimShowAppear.setOwner(this);
		m_kSkillAnimShowAppear.init();
		
		m_kSmoothPosAppear = new SmoothPosAppear();
		m_kSmoothPosAppear.setOwner(this);
		m_kSmoothPosAppear.init();
		
        m_kPursueAppear = new PursueAppear();
        m_kPursueAppear.setOwner(this);
        m_kPursueAppear.init();
	}

	public bool playAnimation(string name) {

		AnimationState state = animation[name];
		if (state) {
			//animation_controller.PlayAnimation(state);
			animation.Play(name);
			return true;
		}
		return false;
	}
	
	public bool crossFadeAnimation(string name, float time) {
	
		AnimationState state = animation[name];
		if (state) {
			//animation_controller.CrossfadeAnimation(state, time);
			animation.CrossFade(name, time);
			return true;
		}
		return false;
	}
	
	//change animation play time by resize it's speed
	public void setAnimationTime(string name, float time) {
		AnimationState state = animation[name];
		state.speed = state.length / time ;
	}
	
	public void setAnimationSpeed(string name, float speed) {
		AnimationState state = animation[name];
		state.speed = speed ;
	}
	
	public void showWeaponTrail() {
		/*
		if (animation_controller) {
			animation_controller.stepIncAnimationTime = true;
		}
		if (trail) {
			trail.renderer.enabled = true;
		}
		*/
	}
	
	public void hideWeaponTrail() {
		/*
		if (animation_controller) {
			animation_controller.stepIncAnimationTime = false;
		}
		if (trail) {
			trail.renderer.enabled = false;
		}
		*/
	}
	
	public void rollAlong(Vector3 dir) 
    {
        setFaceDir(dir);
        CharacterPlayer.sPlayerMe.player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_SKILL, (int)SKILL_APPEAR.SA_ROLL);
	}

    public void magRollAlong(Vector3 dir)
    {
        setFaceDir(dir);
        CharacterPlayer.sPlayerMe.player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_SKILL, (int)SKILL_APPEAR.SA_MAG_FLASH_AWAY);
    }
	
	public void moveTo(Vector3 pos) 
    {
		ChangeAppear(m_kMoveAppear);
        m_kMoveAppear.moveTo(pos);
	}

    public void PursueTo(Vector3 pos)
    {
        ChangeAppear(m_kPursueAppear);
        m_kPursueAppear.PursueTo(pos);
    }

	
	public void moveAlong(Vector3 speed, float time) {
		m_kMoveAppear.moveAlong(speed, time);
		//ChangeAppear(m_kMoveAppear);
	}
	
    //public void changeMoveSpeed(float s) {
    //    setSpeed(s);
    //}
	
	public void init() {

		InitAppears();

		setVelocity(transform.forward);
		//transform.position = new Vector3(0f, 20f, 0f);
		body_center = transform.position + new Vector3(0,0.5f,0);
		old_position = transform.position;
	}
	
	public virtual void applyProperty () {}
	
	public void setPositionLikeGod(Vector3 pos) 
    {
		transform.position = pos;

        // set pet pos
        if (m_kPet != null)
        {
            m_kPet.transform.position = transform.position - getFaceDir() * m_kPet.m_fFollowRadius;
            m_kPet.transform.LookAt(transform.position);
        }
	}
	
	public void setPosition(Vector3 pos) 
    {

        bool bMoveRet = tryMoveTo(pos);

        if (!bMoveRet) 
        {
            tryExtrudingTo(pos);
		}

        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
	}

    public void SetPos(Vector3 pos)
    {
        Vector3 vecFlashDir = pos - transform.position;
        vecFlashDir.Normalize();

        RaycastHit hitInfo;
        LayerMask mask = 1 << LayerMask.NameToLayer("Wall");
        if (Physics.Linecast(transform.position, pos, out hitInfo, mask))
        {
            transform.position = hitInfo.point - vecFlashDir * 0.5f;
            return;
        }
        else
        {
            LayerMask maskObs = 1 << LayerMask.NameToLayer("obstacles");
            if (Physics.Linecast(transform.position, pos, out hitInfo, maskObs))
            {
                // see if inside the collider
                CapsuleCollider collider = hitInfo.collider as CapsuleCollider;
                Vector3 colliderCenter = new Vector3(collider.transform.position.x, 0, collider.transform.position.z);

                float dist = Vector3.Distance(colliderCenter, pos);
                float scaleMax = Mathf.Max(collider.transform.localScale.x, collider.transform.localScale.y, collider.transform.localScale.z);

                if (dist < collider.radius * scaleMax)
                {
                    // inside
                    Vector3 backOutDir = pos - colliderCenter;
                    backOutDir.Normalize();

                    transform.position = colliderCenter + backOutDir * collider.radius * scaleMax;
                }
                else
                {
                    transform.position = pos;
                }
            }
            else
                transform.position = pos;
        }        
    }
	
	public void movePosition(Vector3 deltaPos) 
    {	
		Vector3 curPos = getPosition();
		setPosition(curPos + deltaPos);

        if (m_kPet != null && deltaPos != Vector3.zero)
        {
            m_kPet.m_kMasterPos.Add(transform.position);
        }
	}	
	
	public Vector3 getPosition() {
		
		return transform.position;
	}
	
	public bool canMoveTo(Vector3 pos) {
		
		RaycastHit hitInfo;
		LayerMask mask = 1<<LayerMask.NameToLayer("Wall");
        mask |= 1 << LayerMask.NameToLayer("obstacles");
		
		Vector3 dir = pos - transform.position;
		
		dir.Normalize();
		
		if (Physics.Linecast(transform.position, pos + dir * 0.1f, out hitInfo, mask))
		{
			return false;
		}

        return true;
	}
	
	public void setVisible(bool b) {
		Renderer[] renderChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderChild in renderChildren) {
		    renderChild.enabled = b;
		}
	}
	
	public Transform getTagPoint(string name) {
		
		Transform[] allChildren = GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
		    if (child.gameObject.name == name) {
				return child;
			}
		}
		
		return null;
	}

    public List<Transform> GetTagPoints(string name)
    {
        List<Transform> retTags = new List<Transform>();

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name == name)
            {
                retTags.Add(child);
            }
        }

        return retTags;
    }

	public void packUpWeapon() {
		character_avatar.packUpWeapon();
	}
	
	public void catchWeapon() {
		character_avatar.catchWeapon();
	}
	
	bool tryMoveTo(Vector3 pos) 
    {
        bool bRet = canMoveTo(pos);

        if (bRet) 
        {
			transform.position = pos;
			//body_center = transform.position + new Vector3(0,0.01f,0);
		}

        return bRet;
	}
	
	void tryExtrudingTo(Vector3 pos) 
    {	
		RaycastHit hitInfo;
		LayerMask mask = 1<<LayerMask.NameToLayer("Wall");
        mask |= 1 << LayerMask.NameToLayer("obstacles");
		Vector3 dir = pos - transform.position;
		dir.Normalize();
		if (Physics.Linecast(transform.position, pos + dir * 0.1f, out hitInfo, mask))
		{
			Vector3 normal = -hitInfo.normal;
			Vector3 moveDir = pos - transform.position;
			normal.Normalize();
			moveDir.Normalize();

			float cos_theta = Vector3.Dot(normal, moveDir);
			Vector3 extrusionDir = moveDir - normal * cos_theta;

			bool bCanMove = tryMoveTo(transform.position + extrusionDir * Vector3.Distance(pos, transform.position));

            if (!bCanMove) 
            {
				extrusionDir += hitInfo.normal * 0.1f;
				tryMoveTo(transform.position + extrusionDir * Vector3.Distance(pos, transform.position));
			}
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () 
    {
	}

	void LateUpdate() {
		
    }

    protected void UpdateBuff()
    {
        // buff更新
        foreach (KeyValuePair<BUFF_TYPE, BaseBuff> item in m_dirBuff)
        {
            item.Value.Update(Time.deltaTime);
        }
    }

	protected void UpdateAnimation() 
    {
		float t = Time.deltaTime;

        if (m_kAppear == null)
        {
            if (GetAI().GetCharacterState() != CharacterAI.CHARACTER_STATE.CS_DIE)
            {
                GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
            }
            return;
        }

        if (m_kAppear.isActive())
        {
            m_kAppear.update(t);
        }
        else
        {
            m_kAppear.deActive();
            Appear nextCmd = m_kAppear.getNextCmd();
            m_kAppear = null;

            if (nextCmd != null)
            {
                ChangeAppear(nextCmd);
            }
        }
	}

    protected void UpdateAnimationNet()
    {
        float t = Time.deltaTime;

        if (m_kAppear == null)
        {
            if (!CharacterAI.IsInState(this, CharacterAI.CHARACTER_STATE.CS_DIE))
            {
                ChangeAppear(m_kIdleAppear);
            }
            return;
        }

        if (m_kAppear.GetBattleState() != Appear.BATTLE_STATE.BS_DIE)
        {
            if (m_bIsNetDead)
            {
                MonsterManager.Instance.destroyMonster(this as CharacterMonster);
                return;
            }
        }
        

        if (m_kAppear.isActive())
        {
            m_kAppear.update(t);
        }
        else
        {
            m_kAppear.deActive();
            Appear nextCmd = m_kAppear.getNextCmd();
            m_kAppear = null;

            if (nextCmd != null)
            {
                ChangeAppear(nextCmd);
            }
        }
    }
	
	public virtual void UpdateInput() {
		
	}
	
    //public bool IsDie() 
    //{
    //    if (CharacterPlayer.character_property.getHostComputer())
    //    {
    //        return GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_DIE;
    //    }
    //    else
    //    {
    //        // 由主机的发来的数据决定
    //        return false;
    //    }
        
    //}

    //public virtual void EnterFight()
    //{
    //    Character enemy = BattleManager.Instance.GetViewRangeEnemy(this, nSkillId);
    //    if (enemy)
    //    {

    //    }
    //}

    public virtual SkillCanNotCastReason UseAction(int id = 0) 
    {
        int nSkillId = 0;

        if (id == 0)
        {
            nSkillId = skill.GetCommonAttack().skill_id;
        }
        else
            nSkillId = id;


        // 检测是否可以释放
        SkillCanNotCastReason eCanCast = CanCastSkill(nSkillId);

        if (CanCastSkill(nSkillId) != SkillCanNotCastReason.SCNCR_CAST)
        {
            return eCanCast;
        }

        
        if (ConfigDataManager.GetInstance().getSkillConfig().getSkillData(nSkillId).no_target == 1)
        {
            if (ConfigDataManager.GetInstance().getSkillConfig().getSkillData(nSkillId).suo_target == 0)
            {
                // 可以平放
                if (id == 0)
                {
                    GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_ATTACK);  
                }
                else
                {
                    CastSkill(null, nSkillId);
                }
            }
            else
            {
                Character enemy = BattleManager.Instance.GetViewRangeEnemy(this);
                if (enemy)
                {
                    GetComponent<FightProperty>().m_bEnterFight = true;

                    GetComponent<FightProperty>().SetLockedEnemy(enemy);
                    ArrayList param = new ArrayList();
                    param.Add(enemy);
                    param.Add(id);
                    CharacterPlayer.sPlayerMe.player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, param);
                }
                else
                {
                    // 可以平放
                    if (id == 0)
                    {
                        GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_ATTACK);
                    }
                    else
                    {
                        CastSkill(null, nSkillId);
                    }
                }
            }
        }
        else
        {
            if (ConfigDataManager.GetInstance().getSkillConfig().getSkillData(nSkillId).suo_target == 1)
            {
                Character enemy = BattleManager.Instance.GetViewRangeEnemy(this);
                if (enemy)
                {
                    GetComponent<FightProperty>().m_bEnterFight = true;

                    GetComponent<FightProperty>().SetLockedEnemy(enemy);
                    ArrayList param = new ArrayList();
                    param.Add(enemy);
                    param.Add(id);
                    CharacterPlayer.sPlayerMe.player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, param);
                }
                else
                {

                }
            }
        }


        return SkillCanNotCastReason.SCNCR_CAST;
	}

    // 被技能施放后获得的buff
    public void AddBuff(int buffId, float param = 0.0f)
    {

        BUFF_TYPE type = ConfigDataManager.GetInstance().getSkillEffectConfig().getSkillEffectData(buffId).buffType;

        if (type == BUFF_TYPE.BT_XUANYUN)
        {
            // 如果是怪眩晕的buff需要判断字段
            if (getType() == CharacterType.CT_MONSTER)
            {
                CharacterMonster monster = this as CharacterMonster;

                if (monster != null)
                {
                    if (!ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(monster.monster_property.template_id).swoon)
                    {
                        return;
                    }
                }
            }
            else
            {
                if (Global.inMultiFightMap())
                {
                    // 怪打人眩晕在多人就先这样
                    if (PlayerManager.Instance.player_other_list.Count != 0)
                    {
                        return;
                    }
                }
            }
        }


        if (m_dirBuff.ContainsKey(type))
        {
            m_dirBuff[type].Exit();
            m_dirBuff.Remove(type);
        }

        BaseBuff buff = BuffFactory.CreateBuff(buffId, this, param);
        m_dirBuff.Add(type, buff);
        ReCalculateBuffProperty();

        SetBuffState(type);

        if (Global.inMultiFightMap() && CharacterPlayer.character_property.getHostComputer())
        {
            // 需要将战斗buff同步出去

        }
        
    }

    public virtual void HurtDamager(SkillAppear skill)
    {
        // 计算被击伤害函数

        // 单次伤害
        if (skill.m_kSkillInfo.skillDamageType == SKILL_DAMAGE_TYPE.SDT_ONCE )
        {
            return;
        }

        


    }
    
    // 需要override
    public virtual void BeHitBackHitFlyHitBroken(SkillAppear skill)
    {
        // 受击高亮
        //EffectManager.Instance.BeHitHightlight(gameObject);
        GetComponent<RenderProperty>().GenerateEffect(0.25f, new Color(46, 46, 46, 0));
    }

    public void SetBuffState(BUFF_TYPE state)
    {
        m_nBuffState |= (1 << (int)state);
    }

    public bool HasBuff(BUFF_TYPE state)
    {
        int ret = m_nBuffState;
        bool temp = ((ret & (1 << (int)state)) == 0) ? false : true;
        return temp;
    }

    public bool HasAnyBuff()
    {
        return m_nBuffState != 0;
    }

    public bool HasNotBuff(BUFF_TYPE state)
    {
        return !HasBuff(state);
    }

    public virtual void PlayerBeHitAnimation(float fKnockBack, Vector3 vecSkillDir)
    {
        // 播放受击动作
        if (fKnockBack > 0.0f)
        {
            m_kHitBackAppear.setDir(vecSkillDir);
            ChangeAppear(m_kHitBackAppear);
        }
        else
            ChangeAppear(m_kHitAppear);
    }

    public void ReCalculateBuffProperty()
    {
        // 重新计算由buff带来的属性变化
        GetProperty().fightProperty.ResetBuffData();

        foreach (KeyValuePair<BUFF_TYPE, BaseBuff> item in m_dirBuff)
        {
            if (item.Value.m_bActive)
            {
                if (item.Value.m_kItem.speedRate != 1.0f)
                {
                    GetProperty().fightProperty.SetBuffSpeed(item.Value.m_kItem.speedRate);
                }

                GetProperty().fightProperty.m_fFrozenRate = GetProperty().fightProperty.GetBuffSpeed();
                
                if (item.Value.m_kItem.eType != 0)
                {
                    GetProperty().fightProperty.SetBuffValue(item.Value.m_kItem.eType, item.Value.m_kItem.baseData);
                }
            }
            
        }
    }

    public void RemoveAllBuffObject()
    {
        foreach (KeyValuePair<BUFF_TYPE, BaseBuff> item in m_dirBuff)
        {
            item.Value.DestroySpecialEffect();
        }
    }

    public virtual void OnRevive()
    {
        GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_RELIVE);
        EffectManager.Instance.createFX("Effect/Effect_Prefab/Role/Other/fuhuo", getTagPoint("root"));
    }

    public virtual void SetWeaponAvailable(bool bAvailable)
    {
        
    }

    public virtual void CreatePet(uint nTemplateID)
    {
		if (nTemplateID == 0)
        {
            return;
        }
		 
		if (m_kPet != null) {
			DestroyPet ();
		} 

        string modelName = PetManager.Instance.DictionaryPet[nTemplateID].PetModle;
        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundlePet))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundlePet, modelName,
                (asset) =>
                {
					if(CharacterPlayer.sPlayerMe == null || gameObject == null)
						return;
                    GameObject obj = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
                    m_kPet = obj.transform.GetComponent<CharacterPet>();
                    m_kPet.SetMaster(this);
                    m_kPet.transform.position = transform.position - getFaceDir() * m_kPet.m_fFollowRadius;
                    m_kPet.transform.LookAt(transform.position);
                });
        }
        else
        {
            GameObject asset = BundleMemManager.Instance.getPrefabByName(modelName, EBundleType.eBundlePet);
            GameObject obj = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
            m_kPet = obj.transform.GetComponent<CharacterPet>();
            m_kPet.SetMaster(this);
            m_kPet.transform.position = transform.position - getFaceDir() * m_kPet.m_fFollowRadius;
            m_kPet.transform.LookAt(transform.position);
        }				
    }

    public void DestroyPet()
    {
        if (m_kPet == null)
        {
            return;
        }

        GameObject.Destroy(m_kPet.gameObject);
        m_kPet = null;
    }

    public virtual float GetFightDist(Character kEnemy)
    {
        return 0.0f;
    }


    
}
