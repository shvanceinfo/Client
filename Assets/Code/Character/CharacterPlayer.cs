using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterPlayer : Character {
	
	static public CharacterPlayer sPlayerMe = null;
	
	static public CharacterProperty character_property;
    /// <summary>
    /// 资产数据
    /// </summary>
    static public CharacterAsset character_asset;
	public PlayerAI player_ai;
	
	float roll_cd = -1;
	
	float last_click_time = 0;
	float click_interval = 0;
	
	int m_n32EnterSceneTimes = 0;
	

    
    
    bool m_bStartRoll;
    Vector3 m_vecStartPos;
    float m_fLastTime;
    float m_fBeginTime;


    AudioClip m_acLevelUpClip;
    AudioClip m_acBreakClip;

    public Character m_kGMEnemy = null;

    public List<Vector3> m_kSyncDir = new List<Vector3>();
    public List<Vector3> m_kSyncPos = new List<Vector3>();
    public float m_fSyncFrequency = 0.0f;


	public void testChangeAvatar() {
		character_avatar.testChangeAvatar();
	}
	
	public int EnterSceneTimes
	{
		get{return m_n32EnterSceneTimes;}
		set{m_n32EnterSceneTimes = value;}
	}
	
	void Awake() {
		sPlayerMe = this;
		if (character_asset == null) {
        	character_asset = new CharacterAsset();
		}
		init();
	}
	
	// Use this for initialization
	protected void Start () {

        m_fLastTime = 0.0f;
        m_fBeginTime = 0.0f;
	}
		

    //void OnEnable()
    //{
    //    //FingerGestures.OnFingerDragBegin += OnFingerDragBegin;
    //    //FingerGestures.OnFingerDragMove += OnFingerDragMove;
    //}
	
    //void OnDisable()
    //{
    //    //FingerGestures.OnFingerDragBegin -= OnFingerDragBegin;
    //    //FingerGestures.OnFingerDragMove -= OnFingerDragMove;
    //}

    //void OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    //{
    //    Vector3 inputPos = new Vector3(fingerPos.x, fingerPos.y, 0.0f);

    //    m_fLastTime = Time.realtimeSinceStartup;
    //    m_fBeginTime = m_fLastTime;

    //    Ray rayInput = Camera.main.ScreenPointToRay(inputPos);
    //    RaycastHit hitInput;

    //    LayerMask maskFloor = 1 << LayerMask.NameToLayer("Floor");
    //    LayerMask maskDefault = 1 << LayerMask.NameToLayer("Default");
    //    LayerMask maskUI = 1 << LayerMask.NameToLayer("UI");
    //    LayerMask maskWall = 1 << LayerMask.NameToLayer("Wall");
    //    LayerMask maskObstacles = 1 << LayerMask.NameToLayer("obstacles");

    //    if (Physics.Raycast(rayInput, out hitInput, 1000, maskDefault) ||
    //        Physics.Raycast(rayInput, out hitInput, 1000, maskFloor) ||
    //        Physics.Raycast(rayInput, out hitInput, 1000, maskUI) ||
    //        Physics.Raycast(rayInput, out hitInput, 1000, maskWall) ||
    //        Physics.Raycast(rayInput, out hitInput, 1000, maskObstacles))
    //    {
    //        Vector3 rollPos = hitInput.point;
    //        rollPos.y = 0;
    //        m_vecStartPos = rollPos;
    //    }
    //}

    //void OnFingerDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
    //{
    //    if (CanCastSkill(10020) != SkillCanNotCastReason.SCNCR_CAST)
    //    {
    //        return;
    //    }

    //    if (roll_cd >= 0)
    //    {
    //        return;
    //    }
        
    //    // 速度达到了滑动的上限就表示是滑动
    //    float deltaTime = Time.realtimeSinceStartup - m_fLastTime;

    //    float durationTime = Time.realtimeSinceStartup - m_fBeginTime;
        
    //    // 超过1秒算长按 不允许滑动
    //    if (durationTime > 0.5f)
    //    {
    //        m_fLastTime = 0.0f;
    //        m_fBeginTime = 0.0f;
    //        m_vecStartPos = Vector3.zero;
    //        return;
    //    }
        
    //    float fVelocity = delta.magnitude / deltaTime;
    //    if (fVelocity > 2000)
    //    {
    //        Vector3 endPos = new Vector3(fingerPos.x, fingerPos.y, 0.0f);
    //        Ray rayStart = Camera.main.ScreenPointToRay(endPos);

    //        RaycastHit hitStart;
    //        LayerMask maskFloor = 1 << LayerMask.NameToLayer("Floor");
    //        LayerMask maskDefault = 1 << LayerMask.NameToLayer("Default");
    //        LayerMask maskUI = 1 << LayerMask.NameToLayer("UI");
    //        LayerMask maskWall = 1 << LayerMask.NameToLayer("Wall");
    //        LayerMask maskObstacles = 1 << LayerMask.NameToLayer("obstacles");

    //        if (Physics.Raycast(rayStart, out hitStart, 1000, maskDefault) ||
    //            Physics.Raycast(rayStart, out hitStart, 1000, maskFloor) ||
    //            Physics.Raycast(rayStart, out hitStart, 1000, maskUI) ||
    //            Physics.Raycast(rayStart, out hitStart, 1000, maskWall) ||
    //            Physics.Raycast(rayStart, out hitStart, 1000, maskObstacles))
    //        {
    //            if (m_vecStartPos != Vector3.zero)
    //            {
    //                Vector3 rollPos = hitStart.point;
    //                rollPos.y = 0;


    //                Vector3 rollDir = rollPos - m_vecStartPos;
    //                rollDir.Normalize();

                    

                    
    //                // 根据职业调不同的技能
    //                switch (character_property.getCareer())
    //                {
    //                case CHARACTER_CAREER.CC_ARCHER:
    //                case CHARACTER_CAREER.CC_SWORD:
    //                        rollAlong(rollDir);
    //                        roll_cd = ConfigDataManager.GetInstance().getSkillConfig().getSkillData(400001).cool_down / 1000.0f;
    //                    break;
    //                case CHARACTER_CAREER.CC_MAGICIAN:
    //                        magRollAlong(rollDir);
    //                        roll_cd = ConfigDataManager.GetInstance().getSkillConfig().getSkillData(400003).cool_down / 1000.0f;
    //                    break;
    //                }                 
                    
    //                // 防止滚动后还要跑去其他地方
    //                player_ai.m_kMoveState.m_vecMovePos = Vector3.zero;
    //                player_ai.m_kMoveState.m_vecMovePos = Vector3.zero;
    //            }

    //            m_vecStartPos = Vector3.zero;
    //            m_fLastTime = 0.0f;
    //            return;
    //        }
    //    }
        
        
    //    m_fLastTime = Time.realtimeSinceStartup;
    //}

	
	// Update is called once per frame
	protected void Update () 
    {
        if (m_kSyncDir.Count != 0 && m_kSyncPos.Count != 0)
        {
            if (m_fSyncFrequency > 0.1f)
            {
                MessageManager.Instance.sendMessageAskMove(CharacterPlayer.sPlayerMe.GetProperty().GetInstanceID(),
                    m_kSyncDir[m_kSyncDir.Count - 1],
                    m_kSyncPos[m_kSyncPos.Count - 1]
                );

                m_kSyncDir.Clear();
                m_kSyncPos.Clear();

                m_fSyncFrequency = 0.0f;
            }

            m_fSyncFrequency += Time.deltaTime;
        }
        
        if (Global.m_bCameraCruise)
        {
            if (!CharacterAI.IsInState(this, CharacterAI.CHARACTER_STATE.CS_IDLE))
            {
                GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
            }

            return;
        }


		if (MainLogic.sMainLogic.isGameSuspended()) 
        {
            if (!CharacterAI.IsInState(this, CharacterAI.CHARACTER_STATE.CS_IDLE))
            {
                GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
            }

			return;
		}

		if (!Input.GetButton("Fire1")) 
        {
			Global.move_lock = false;
		}

        player_ai.Update();

        UpdateBuff();

		UpdateAnimation();

        UpdateInput();
		
		PlayerManager.Instance.CheckNewOther();
	}
	
	public void init() {
		//SceneManager.Instance.setDontDestroyObj(transform.gameObject);
		//DontDestroyOnLoad (transform.gameObject);  
		character_type = CharacterType.CT_PLAYER;
		
		player_ai = new PlayerAI();
		
		
		character_avatar = new CharacterAvatar();
		character_avatar.init(this);

        base.init();

        player_ai.init(this);

	    if (BundleMemManager.debugVersion)
	    {
	        m_acBreakClip = BundleMemManager.Instance.loadResource(PathConst.AUDIO_BREAK) as AudioClip;
            m_acLevelUpClip = BundleMemManager.Instance.loadResource(PathConst.AUDIO_UP_LEVEL) as AudioClip;
	    }
	    else
	    {
            BundleMemManager.Instance.loadPrefabViaWWW<AudioClip>(EBundleType.eBundleMusic, PathConst.AUDIO_BREAK,
           (obj) => { m_acBreakClip = obj as AudioClip; });
            BundleMemManager.Instance.loadPrefabViaWWW<AudioClip>(EBundleType.eBundleMusic, PathConst.AUDIO_UP_LEVEL,
                (obj) => { m_acLevelUpClip = obj as AudioClip; });
	    }     
	}

    public override void InitAppears()
    {
        base.InitAppears();

        m_kDieAppear = new PlayerDieAppear();
        m_kDieAppear.setOwner(this);
        m_kDieAppear.init();
    }
	
	public override void applyProperty () {
		
        if (character_property.getCareer() == CHARACTER_CAREER.CC_ARCHER )
        {
            character_property.setAttackRange(5.0f);
        }
        else if (character_property.getCareer() == CHARACTER_CAREER.CC_MAGICIAN)
        {
            character_property.setAttackRange(5.0f);
        }
        else
            character_property.setAttackRange(1.5f);


		
		character_property.setMoveSpeed(4.0f);
		character_property.setExtrusionRange(0.4f);
		
		//setSpeed(character_property.getMoveSpeed());
		equipItem(character_property.getWeapon());
		equipItem(character_property.getArmor());
	    character_avatar.installWing();
	}
	
	public void equipItem (int id) {
		EquipmentTemplate et = EquipmentManager.GetInstance().GetTemplateByTempId((uint)id);
        if (et.id!=0) {
            BodyPartInfo bpi = new BodyPartInfo();
			switch (et.part) {
			case eEquipPart.eSuit:
				bpi.body_part = BodyPart.BP_ARMOR;
                character_property.setArmor(id);
				break;
			case eEquipPart.eGreatSword:
				bpi.body_part = BodyPart.BP_WEAPON;
                character_property.setWeapon(id);
				break;
			}
            switch (character_property.career) 
            {
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
//            bpi.texture_name = et.texture_name;
            bpi.model_name = et.model_name;
            bpi.weapon_trail_texture = et.weapon_trail_texture;
            bpi.effect_name = et.effect_name;
            character_avatar.changeBodyPart(bpi);
		}
		
        //Renderer[] allRenderer = GetComponentsInChildren<Renderer>();
        //foreach (Renderer renderer in allRenderer) {
        //    if (!renderer.gameObject.GetComponent("WillRenderTransparent")) {
        //        renderer.gameObject.AddComponent("WillRenderTransparent");
        //    }
        //}
	}

	public override void UpdateInput() 
    {
		
		if (UpdateSkillStandAlone()) 
            return;

        UpdateMouseInput();
		
		if (Input.GetButtonDown("Fire1"))
        {
			if(!NPCAction.sClickNPC)
				TaskManager.Instance.isTaskFollow = false;
			if(Global.pressOnUI() || NPCAction.sClickNPC)
			{
				if (!Global.move_lock) 
	            {
					Global.move_lock = true;
				}
				NPCAction.sClickNPC = false; 
			}
		}

		if (Global.pressOnUI()) return;
		if (Global.move_lock) return;
	}	
	
	void UpdateMouseInput() 
    {
        if (!Input.GetButton("Fire1") || !Camera.main || Global.InArena())
        {
            return;
        } 

        if (GetComponent<InputProperty>().m_bInJoyStickStatus)
        {
            // 摇杆在起作用的时候不需要做 点击怪的处理
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = 1 << LayerMask.NameToLayer("Floor");
        if (!Physics.Raycast(ray, out hit, 1000, mask))
        {
            return;
        }

        Vector3 moveToPos = hit.point;
        moveToPos.y = 0;
        if (Vector3.Distance(moveToPos, transform.position) > 0.1f)
        {
            Character monster = MonsterManager.Instance.GetPointNearestMonster(moveToPos, 1.5f);
            if (monster)
            {
                // 点击怪物
                if (Input.GetButtonDown("Fire1"))
                {
                    GetComponent<FightProperty>().SetLockedEnemy(monster);
                    //skill.setSkillTarget(monster);
                    //BattleManager.Instance.hideMoveTarget();

                    float dist = Vector3.Distance(transform.position, monster.transform.position);

                    if (player_ai.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_ATTACK)
                    {
                        if (dist > GetProperty().getAttackRange())
                        {
                            // 如果大于攻击范围 先保存追击对象 等做完动作再追击
                            player_ai.m_kPursueState.m_kTmpSavedTarget = monster;
                        }
                        else
                        {
                            // 在攻击范围内 直接转向 普通攻击
                            Vector3 faceDir = monster.transform.position - transform.position;
                            faceDir.Normalize();
                            setFaceDir(faceDir);
                            skill.setSkillTarget(monster);
                        }
                    }
                    else
                    {
                        player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, monster);
                    }
                }
            }
            else
            {
                // 没点到怪 点到其他地方 
                //if (player_ai.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_ATTACK)
                //{
                //    // 普通攻击状态下 需要保存当前的路点 等做完动作再强制寻路过去
                //    player_ai.m_kPursueState.m_kPursurPoint = moveToPos;
                //}
                //else
                //{
                //    // 其他状态下 直接强制寻过去
                //    player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, moveToPos);
                //}

                //BattleManager.Instance.showMoveTarget(moveToPos);
            }
        }
	}

    bool UpdateSkillStandAlone()
    {
        if (Input.GetKey("["))
        {
            CameraFollow.sCameraFollow.m_fZoomPinchValue -= 0.02f;
            CameraFollow.sCameraFollow.m_fZoomPinchValue = Mathf.Clamp01(CameraFollow.sCameraFollow.m_fZoomPinchValue);
            return true;
        }

        if (Input.GetKey("]"))
        {
            CameraFollow.sCameraFollow.m_fZoomPinchValue += 0.02f;
            CameraFollow.sCameraFollow.m_fZoomPinchValue = Mathf.Clamp01(CameraFollow.sCameraFollow.m_fZoomPinchValue);
            return true;
        }

        if (Input.GetKeyDown("="))
        {
            GraphicsUtil.m_bShowGraphics = !GraphicsUtil.m_bShowGraphics;
        }

        if (Input.GetKeyDown("0")) 
		{
			GraphicsUtil.m_kScreenEdge.Clear();

			RaycastHit hit1;
			Vector3 bottomleft = Vector3.zero;
			Vector3 bottomright = Vector3.zero;
			Vector3 upright = Vector3.zero;
			Vector3 upleft = Vector3.zero;
			LayerMask mask = 1 << LayerMask.NameToLayer("Floor");

			Ray ray1 = Camera.main.ScreenPointToRay(new Vector3(0, 0, 0));
		
			if (Physics.Raycast(ray1, out hit1, 1000, mask))
			{
				bottomleft = hit1.point;
				bottomleft.y = 0;
			}

			RaycastHit hit2;
			Ray ray2 = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth, 0, 0));
			
			if (Physics.Raycast(ray2, out hit2, 1000, mask))
			{
				bottomright = hit2.point;
				bottomright.y = 0;
			}

			RaycastHit hit3;
			Ray ray3 = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
			
			if (Physics.Raycast(ray3, out hit3, 1000, mask))
			{
				upright = hit3.point;
				upright.y = 0;
			}

			RaycastHit hit4;
			Ray ray4 = Camera.main.ScreenPointToRay(new Vector3(0, Camera.main.pixelHeight, 0));
			
			if (Physics.Raycast(ray4, out hit4, 1000, mask))
			{
				upleft = hit4.point;
				upleft.y = 0;
			}

			GraphicsUtil.m_kScreenEdge.Add(bottomleft);
			GraphicsUtil.m_kScreenEdge.Add(bottomright);
			GraphicsUtil.m_kScreenEdge.Add(upright);
			GraphicsUtil.m_kScreenEdge.Add(upleft);

			Debug.Log("point " + bottomleft.ToString()
			          + " " + bottomright.ToString()
			          + " " + upright.ToString()
			          + " " + upleft.ToString()
			          + " " + Camera.main.pixelHeight
			          + " " + Camera.main.pixelWidth
			          );
		}

        if (Input.GetKeyDown("6"))
		{
			CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_UP_DOWN, 5.0f);
		}

        if (Input.GetKeyDown("7"))
		{
			CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_LEFT_RIGHT, 5.0f);
		}

        if (Input.GetKeyDown("8"))
		{
			CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_FRONT_BACK, 5.0f);
		}

        if (Input.GetKeyDown("9"))
		{
			CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_SPHERE, 5.0f);
		}

        if (Input.GetKeyDown("-"))
        {
            CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_FIGHT_SHAKE, 0.025f);
        }

        if (Input.GetKeyDown((".")))
        {
            EffectManager.Instance.BeginCloseUpEffect(CharacterPlayer.sPlayerMe.transform.position);
        }

        if (Input.GetKeyDown((",")))
        {
            EffectManager.Instance.EndCloseUpEffect();
        }
        

        switch (GetProperty().getCareer())
        {
            case CHARACTER_CAREER.CC_ARCHER:
                {
                    if (Input.GetKeyDown("1"))
                    {
                        UseAction(102001);
                    }

                    if (Input.GetKeyDown("2"))
                    {
                        UseAction(102002);
                    }

                    if (Input.GetKeyDown("3"))
                    {
                        UseAction(102003);
                    }

                    if (Input.GetKeyDown("4"))
                    {
                        UseAction(102004);
                    }

                    if (Input.GetKeyDown("5"))
                    {
                        UseAction(102005);
                    }


                    if (Input.GetKeyDown("q"))
                    {
                        UseAction(202001001);
                    }

                    if (Input.GetKeyDown("w"))
                    {
                        UseAction(202002001);
                    }


                    if (Input.GetKeyDown("e"))
                    {
                        UseAction(202003001);
                    }


                    if (Input.GetKeyDown("r"))
                    {
                        UseAction(202004001);
                    }

                    if (Input.GetKeyDown("a"))
                    {
                        UseAction(202005001);
                    }


                    if (Input.GetKeyDown("s"))
                    {
                        UseAction(400001001);
                    }

                    if (Input.GetKeyDown("z"))
                    {
                        UseAction(400002001);
                    }
                }
                break;
            case CHARACTER_CAREER.CC_SWORD:
                {
                    if (Input.GetKeyDown("1"))
                    {
                        UseAction(101001);
                    }

                    if (Input.GetKeyDown("2"))
                    {
                        UseAction(101002);
                    }

                    if (Input.GetKeyDown("3"))
                    {
                        UseAction(101003);
                    }

                    if (Input.GetKeyDown("4"))
                    {
                        UseAction(101004);
                    }

                    if (Input.GetKeyDown("5"))
                    {
                        UseAction(101005);
                    }

                    if (Input.GetKeyDown("6"))
                    {
                        UseAction(101006);
                    }

                    if (Input.GetKeyDown("7"))
                    {
                        UseAction(101007);
                    }

                    if (Input.GetKeyDown("8"))
                    {
                        UseAction(101008);
                    }

                    if (Input.GetKeyDown("9"))
                    {
                        UseAction(101009);
                    }

                    if (Input.GetKeyDown("0"))
                    {
                        UseAction(101010);
                    }

                    if (Input.GetKeyDown("-"))
                    {
                        UseAction(101011);
                    }

                    if (Input.GetKeyDown("="))
                    {
                        UseAction(101012);
                    }


                    if (Input.GetKeyDown("q"))
                    {
                        UseAction(201001001);
                    }

                    if (Input.GetKeyDown("w"))
                    {
                        UseAction(201002001);
                    }


                    if (Input.GetKeyDown("e"))
                    {
                        UseAction(201003001);
                    }


                    if (Input.GetKeyDown("r"))
                    {
                        UseAction(201004001);
                    }

                    if (Input.GetKeyDown("a"))
                    {
                        UseAction(201005001);
                    }

                    if (Input.GetKeyDown("d"))
                    {
                        UseAction(201006001);
                    }


                    if (Input.GetKeyDown("s"))
                    {
                        UseAction(400001001);
                    }

                    if (Input.GetKeyDown("z"))
                    {
                        UseAction(400002001);
                    }
                }
                break;
            case CHARACTER_CAREER.CC_MAGICIAN:
                {
                    if (Input.GetKeyDown("q"))
                    {
                        UseAction(203001001);
                    }

                    if (Input.GetKeyDown("w"))
                    {
                        UseAction(203002001);
                    }


                    if (Input.GetKeyDown("e"))
                    {
                        UseAction(203003001);
                    }


                    if (Input.GetKeyDown("r"))
                    {
                        UseAction(203004001);
                    }


                    if (Input.GetKeyDown("s"))
                    {
                        UseAction(400001001);
                    }

                    if (Input.GetKeyDown("z"))
                    {
                        UseAction(400003001);
                    }
                }
                break;
        }

		
		if (Input.GetButtonDown("Fire1")) 
        {
			click_interval = Time.realtimeSinceStartup - last_click_time;
			last_click_time = Time.realtimeSinceStartup;
		}


        if (roll_cd >= 0.0f)
        {
			roll_cd -= Time.deltaTime;
		}

		

        ////// 聊天回车功能
        ////if (Input.GetKeyDown(KeyCode.Return))
        ////{
        ////    if (Global.inCityMap())
        ////    {
        ////        ChatManager.GetInstance().ActiveInput();
            
        ////    }
        
        ////}
        
       
		return false;
	}

    public override void HurtDamager(SkillAppear skill)
    {
        base.HurtDamager(skill);
    }

    public override SkillCanNotCastReason CanCastSkill(int nSkillId)
    {
        SkillCanNotCastReason eReason = base.CanCastSkill(nSkillId);

        if (eReason == SkillCanNotCastReason.SCNCR_MP_NOT_ENOUGH)
        {
            FloatMessage.GetInstance().PlayFloatMessage(LanguageManager.GetText("use_skill_mp_error"), UIManager.Instance.getRootTrans(), Vector3.zero, Vector3.zero);
        }

        return eReason;
    }

    public override bool AICastSkill(Character target, int skillId)
    {
        if (Global.InArena())
        {
            return base.AICastSkill(target, skillId);
        }
        else
        {
            return base.AICastSkill(target, skillId);
            //// 模拟点击了 某个技能id
            //Transform btnSkill = UiFightMainMgr.Instance.m_kCurSkillContainer.transform.FindChild(skillId.ToString());

            //BtnUseSkill btn = btnSkill.gameObject.GetComponent<BtnUseSkill>();
            //if (btn)
            //{
            //    btn.OnClick();
            //    return true;
            //}
        }
    }

    public override CharacterBaseProperty GetProperty()
    {
        return character_property;
    }

    public override CharacterAI GetAI()
    {
        return player_ai;
    }


    public override void BeHitBackHitFlyHitBroken(SkillAppear skill)
    {
        base.BeHitBackHitFlyHitBroken(skill);

        
        


        if (GetProperty().getHP() <= 0)
        {
            player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_DIE);
        }
        else
        {
            SkillHurtProcess(skill);
        }

    }

    public void SkillHurtProcess(SkillAppear skill)
    {
        float fKnockBack = skill.getAttackRepel() * GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_KnockBack);

        ArrayList param = new ArrayList();

        if (skill.getOwner().getType() == CharacterType.CT_PLAYEROTHER)
        {
            // 其他人打我
			if (!Global.InArena())
			{
				CharacterPlayerOther other = skill.getOwner() as CharacterPlayerOther;
				
				Vector3 vecOtherSkillDir = transform.position - other.transform.position;
				vecOtherSkillDir.Normalize();
				
				param.Add(fKnockBack);
				param.Add(vecOtherSkillDir);
				player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
			}

			return;
        }

        // 怪打我


        CharacterMonster monster = skill.getOwner() as CharacterMonster;

        Vector3 vecSkillDir = transform.position - monster.transform.position;
        vecSkillDir.Normalize();

        // 特效
        EffectManager.Instance.createFX("Effect/Effect_Prefab/Role/Other/shouji", getTagPoint("help_body"));


        param.Add(fKnockBack);
        param.Add(vecSkillDir);

        switch (monster.monster_property.getType())
        {
            case MonsterProperty.MONSTER_LEVEL_TYPE.MLT_BOSS:
                {
                    player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
                }
                break;
            case MonsterProperty.MONSTER_LEVEL_TYPE.MLT_ELITE:
                {
                    // 怪的状态目前跟人物不通用 暂时这样吧
                    if (monster.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_SKILL)
                    {
                        player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
                    }
                    else
                    {
                        // 有一定几率进入受击
                        if (Random.Range(0.0f, 1.0f) < 0.2f)
                        {
                            player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
                        }
                        else
                        {
                            if (player_ai.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_IDLE)
                            {
                                player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
                            }
                        }
                    }
                }
                break;
            case MonsterProperty.MONSTER_LEVEL_TYPE.MLT_COMMON:
                {
                    if (monster.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_SKILL)
                    {
                        player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
                    }
                    else
                    {
                        if (player_ai.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_IDLE)
                        {
                            player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
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


    public override void OnRevive()
    {
        base.OnRevive();

        if (CharacterPlayer.sPlayerMe.audio)
        {
            CharacterPlayer.sPlayerMe.audio.PlayOneShot(m_acLevelUpClip);
        }
    }


    public void OnUpgrade()
    {
        EffectManager.Instance.createFX("Effect/Effect_Prefab/Role/Other/shengji", getTagPoint("root"));

        if (CharacterPlayer.sPlayerMe.audio)
        {
            CharacterPlayer.sPlayerMe.audio.PlayOneShot(m_acLevelUpClip);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            //m_bCollider = true;
            //if (other.transform.parent)
            //{
            //    if (other.transform.parent.name.Contains("door") || other.transform.parent.name.Contains("box"))
            //    {
            //        PlayerIdleState idlestate = GetAI().m_kIdleState as PlayerIdleState;

            //        if (idlestate != null)
            //        {
            //            //PathFinding path = GetComponent<PathFinding>();
            //            //idlestate.m_bShouldStay = true;

            //            //player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
            //            //path.StopMove();

            //            //InvokeRepeating("AutoFightStayTimer", 1.5f, .0f);
            //        }

            //        return;
            //    }
            //}


        }  

        OnTriggerUpdate(other);
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerUpdate(other);
    }


    void OnTriggerUpdate(Collider other)
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
        if (CharacterAI.IsInState(this, CharacterAI.CHARACTER_STATE.CS_SKILL))
        {
            // 如果是冲撞 碰到人需要停下
            if (skill.getCurrentSkill() != null)
            {
                int preSkillID = skill.getCurrentSkill().GetSkillId() / 1000;
                if (preSkillID == 201001)
                {
                    //skill.getCurrentSkill().m_bSkillIntrupted = true;
                }
            }
            
        }
    }

    void AutoFightStayTimer()
    {
        PlayerIdleState state = GetAI().m_kIdleState as PlayerIdleState;

        if (state != null)
        {
            state.m_bShouldStay = false;
            state.m_bArrived = false;
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

    public override float GetFightDist(Character kEnemy)
    {
        // 判断敌人和自己的距离
        if (!Global.InWorldBossMap())
        {
            return Vector3.Distance(transform.position, kEnemy.transform.position);
        }
        else
        {
            return BattleWorldBoss.CalculateDistFromeBoss(this, kEnemy);
        }
    }
}
