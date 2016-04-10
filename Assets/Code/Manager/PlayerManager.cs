using UnityEngine;
using System.Collections.Generic;
using NetGame;
using manager;

public class PlayerManager  
{
	public List<CharacterPlayerOther> player_other_list;

    public List<GSNotifyRoleAppear> m_kListLoginPlayerOther;

	private static PlayerManager _instance = null;

	public int m_nCurrentVisiblePlayerOther = 0;


	public uint m_unAppearOtherID = 0;

	public static PlayerManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = new PlayerManager();
            return _instance;
        }
    }

    private PlayerManager()
    {
        player_other_list = new List<CharacterPlayerOther>();
        m_kListLoginPlayerOther = new List<GSNotifyRoleAppear>();
    }
	
	public void createPlayerMe(CharacterProperty cp) {
		if (CharacterPlayer.sPlayerMe) {
			return;
		}		
		string resName = "";
		switch (cp.career) 
		{
			case CHARACTER_CAREER.CC_SWORD:
				resName = Constant.SWORD_PREFAB;
				break;
			case CHARACTER_CAREER.CC_ARCHER:
				resName = Constant.BOW_PREFAB;
				break;
			case CHARACTER_CAREER.CC_MAGICIAN:
				resName = Constant.RABBI_PREFAB;
				break;
			default:
				break;
		}
        GameObject playerMe = BundleMemManager.Instance.instantiateObj(
            BundleMemManager.Instance.getPrefabByName(resName, EBundleType.eBundleCommon), 
            new Vector3(Constant.PLAYER_INIT_POSITION_X, Constant.PLAYER_INIT_POSITION_Y, Constant.PLAYER_INIT_POSITION_Z), Quaternion.identity);
		if(playerMe == null) {
			Debug.LogError(resName + " is not exist!");
			return;
		}	
		
		Collider collider = playerMe.GetComponent<Collider>();
		collider.isTrigger = true;

        playerMe.GetComponent<CharacterAnimCallback>().SetCharType(CHAR_ANIM_CALLBACK.CAC_PLAYER);


		
		Rigidbody rb =  playerMe.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.isKinematic = true;
		playerMe.AddComponent("CharacterPlayer");
		playerMe.AddComponent("AnimationController");
		playerMe.AddComponent("RenderProperty");
		playerMe.AddComponent("BindProperty");
        playerMe.AddComponent<HUD>();
        playerMe.AddComponent<CoolDownProperty>();
        playerMe.AddComponent<InputProperty>();
        playerMe.AddComponent<FightProperty>();
        playerMe.AddComponent<AISystem.AIPathFinding>();

		CharacterPlayer.character_property = cp;

        if (cp.getCareer() == CHARACTER_CAREER.CC_ARCHER)
        {
            playerMe.AddComponent<ArcherSkill>();
            CharacterPlayer.sPlayerMe.skill = playerMe.GetComponent<ArcherSkill>();
        }
        else if (cp.getCareer() == CHARACTER_CAREER.CC_SWORD)
        {
            playerMe.AddComponent<SwordSkill>();
            CharacterPlayer.sPlayerMe.skill = playerMe.GetComponent<SwordSkill>();
        }
        else
        {
            playerMe.AddComponent<MagicSkill>();
            CharacterPlayer.sPlayerMe.skill = playerMe.GetComponent<MagicSkill>();
        }
        
		CharacterPlayer.sPlayerMe.applyProperty();

        CharacterPlayer.sPlayerMe.skill.SetSkillOwner(CharacterPlayer.sPlayerMe);

        if (PetManager.Instance.CurrentPet != null)
        {
            CharacterPlayer.sPlayerMe.CreatePet(PetManager.Instance.CurrentPet.Id);
        }

	}

    //public void AddNavMeshComponent()
    //{
    //    agent = CharacterPlayer.sPlayerMe.gameObject.AddComponent<NavMeshAgent>();
    //    agent.radius = 0.3f; //网格半径
    //    agent.speed = 4.0f;
    //    agent.acceleration = 200f;
    //    agent.angularSpeed = 150f; //角速度
    //    agent.stoppingDistance = 0f;
    //    agent.autoTraverseOffMeshLink = false;
    //    agent.walkableMask = 1;
    //    agent.avoidancePriority = 90;

    //    pathFind = CharacterPlayer.sPlayerMe.gameObject.AddComponent<PathFinding>();
    //}

    public void CreateOthersByLoginData()
    {
        for (int i = 0; i < m_kListLoginPlayerOther.Count; ++i)
        {
            GSNotifyRoleAppear roleAppear = m_kListLoginPlayerOther[i];

			OnPlayerAppear(roleAppear);
        }

        m_kListLoginPlayerOther.Clear();
    }

	public CharacterPlayerOther CreateOthersImmidiately(GSNotifyRoleAppear roleAppear)
	{
		CharacterOtherProperty cop = new CharacterOtherProperty ();
		cop.setSeverID ((int)roleAppear.m_un32ObjID);
		cop.setNickName (Global.FromNetString (roleAppear.m_n32RoleNickName));
		cop.setCareer ((CHARACTER_CAREER)roleAppear.m_n32CareerID);
		cop.setSex ((int)roleAppear.m_bGender);
		cop.setWeapon ((int)roleAppear.m_un32WeaponTypeID);
		cop.setArmor ((int)roleAppear.m_un32CoatTypeID);
		cop.setTitleID ((int)roleAppear.m_un32CurTitleID);
		cop.setHP ((int)roleAppear.m_un32CurHp);
		cop.setLevel ((int)roleAppear.m_un32Level);
		cop.wingID = ((uint)roleAppear.m_u32WingID);
		cop.medalID = ((uint)roleAppear.m_u32MedalID);
		cop.petID = roleAppear.m_u32PetID;

		CharacterPlayerOther cpo = null;

		if (roleAppear.m_fTarPosX == 0.0f && roleAppear.m_fTarPosY == 0.0f && roleAppear.m_fTarPosZ == 0.0f) 
		{
			cpo = PlayerManager.Instance.createPlayerOther (cop, Vector3.zero, new Vector3 (roleAppear.m_fCurPosX, 0.0f, roleAppear.m_fCurPosZ));
		} 
		else 
		{
			cpo = PlayerManager.Instance.createPlayerOther (cop, Vector3.zero, new Vector3 (roleAppear.m_fTarPosX, 0.0f, roleAppear.m_fTarPosZ));
		}
		
		cop.fightProperty = roleAppear.fightProperty;

		return cpo;
	}
	
	public void createPlayerCamera() 
    {	
		if (CameraFollow.sCameraFollow) 
        {
			return;
		}
        GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.PLAYER_CAMERA, EBundleType.eBundleCommon);
        BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
	}

    public void CreateEasyTouchJoyStick()
    {
        if (EasyTouchJoyStickProperty.sJoystickProperty)
        {
            Object.Destroy(EasyTouchJoyStickProperty.sJoystickProperty.gameObject);
            EasyTouchJoyStickProperty.sJoystickProperty = null;
        }
        GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.EASY_TOUCH_PATH, EBundleType.eBundleCommon);
        GameObject joystick = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
        joystick.name = "EasyTouchJoyStick";
		joystick.SetActive(false);
    }
	
	public void createPlayerMeUI() {
		if(CharacterPlayer.character_property != null)
		{
			string resName = "";
			switch (CharacterPlayer.character_property.career) 
			{
				case CHARACTER_CAREER.CC_SWORD:
					resName = Constant.SWORD_UI;
					break;
				case CHARACTER_CAREER.CC_ARCHER:
					resName = Constant.BOW_UI;
					break;
				case CHARACTER_CAREER.CC_MAGICIAN:
					resName = Constant.RABBI_UI;
					break;
				default:
					break;
			}
            GameObject asset = BundleMemManager.Instance.getPrefabByName(resName, EBundleType.eBundleWeaponEffect);
            GameObject playerMeUI = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);        
			if(playerMeUI == null) {
				Debug.LogError(resName + " is not exist!");
				return;
			}
			
			playerMeUI.AddComponent("CharacterPlayerUI");
	        //CharacterPlayerUI.sPlayerMeUI.gameObject.SetActive(false);
		}
	}
	
	public CharacterPlayerOther createPlayerOther(CharacterOtherProperty op, Vector3 faceDir, Vector3 bornPos) 
    {
		string resName = "";
		switch (op.career) 
		{
			case CHARACTER_CAREER.CC_SWORD:
				resName = Constant.SWORD_PREFAB;
				break;
			case CHARACTER_CAREER.CC_ARCHER:
				resName = Constant.BOW_PREFAB;
				break;
			case CHARACTER_CAREER.CC_MAGICIAN:
				resName = Constant.RABBI_PREFAB;
				break;
			default:
				break;
		}
        GameObject asset = BundleMemManager.Instance.getPrefabByName(resName, EBundleType.eBundleCommon);
        GameObject playerOther = BundleMemManager.Instance.instantiateObj(asset, bornPos, Quaternion.identity);
		if(playerOther == null) {
			Debug.LogError(resName + " is not exist!");
			return null;
		}
	    if (!Global.inCityMap())
	    {
            BundleMemManager.Instance.addRoleEffectByCareer(op.career);
	    }

        

        Collider collider = playerOther.GetComponent<Collider>();
        collider.isTrigger = true;


        //NavMeshAgent agent = null;
        //agent = playerOther.AddComponent<NavMeshAgent>();
        //agent.radius = 0.3f; //网格半径
        //agent.speed = 4.0f;
        //agent.acceleration = 200f;
        //agent.angularSpeed = 150f; //角速度
        //agent.stoppingDistance = 0f;
        //agent.autoTraverseOffMeshLink = false;
        //agent.walkableMask = 1;
        //agent.avoidancePriority = 99;



        playerOther.GetComponent<CharacterAnimCallback>().SetCharType(CHAR_ANIM_CALLBACK.CAC_OTHERS);

        Rigidbody rb = playerOther.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
		
		//playerOther.AddComponent("PlayerOtherAnimCallback");
		playerOther.AddComponent("CharacterPlayerOther");
		
		playerOther.AddComponent("AnimationController");
        playerOther.AddComponent("RenderProperty");
        playerOther.AddComponent<HUD>();
        playerOther.AddComponent<CoolDownProperty>();
        playerOther.AddComponent<AISystem.AIPathFinding>();
		playerOther.AddComponent<FightProperty>();

        //playerOther.AddComponent<PathFindingOther>();

        //playerOther.GetComponent<CharacterPlayerOther>().setFaceDir(faceDir);
        //playerOther.GetComponent<CharacterPlayerOther>().transform.position = bornPos;
		
		playerOther.GetComponent<CharacterPlayerOther>().skill = playerOther.GetComponent<Skill>();
//		playerOther.GetComponent<CharacterPlayerOther>().animation_controller = playerOther.GetComponent<AnimationController>();
		//优化其它玩家的动画计算
//		playerOther.GetComponent<CharacterPlayerOther>().animation_controller.setNeedCullAnimation(true);
		CharacterPlayerOther playerOtherComp = playerOther.GetComponent<CharacterPlayerOther>();
		playerOtherComp.character_other_property = op;

        if (op.getCareer() == CHARACTER_CAREER.CC_ARCHER)
        {
            playerOther.AddComponent<ArcherSkill>();
            playerOtherComp.skill = playerOther.GetComponent<ArcherSkill>();
        }
        else if (op.getCareer() == CHARACTER_CAREER.CC_SWORD)
        {
            playerOther.AddComponent<SwordSkill>();
            playerOtherComp.skill = playerOther.GetComponent<SwordSkill>();
        }
        else
        {
            playerOther.AddComponent<MagicSkill>();
            playerOtherComp.skill = playerOther.GetComponent<MagicSkill>();
        }


		playerOtherComp.applyProperty();
		player_other_list.Add(playerOtherComp);

        // 把技能保存
        Skill charSkill = playerOtherComp.GetComponent<Skill>();
        charSkill.SetSkillOwner(playerOtherComp);

        // 添加宠物
        playerOtherComp.CreatePet(op.petID);

		return playerOtherComp;
	}
	
	public void destroyPlayerOther(int id) {
		
		for (int i = 0; i < player_other_list.Count; i++) {
			CharacterPlayerOther p = player_other_list[i];
			if (p.character_other_property.getSeverID() == id) {
                Object.Destroy(p.gameObject);
				player_other_list.RemoveAt(i);
				return;
			}
		}
	}
	
	public void destroyAllPlayerOther() {
		
		for (int i = 0; i < player_other_list.Count; i++) {
			CharacterPlayerOther p = player_other_list[i];
			if(p != null)
                Object.Destroy(p.gameObject);
		}
		
		player_other_list.Clear();
	}
	
	public CharacterPlayerOther getPlayerOther(int id) {
		
		for (int i = 0; i < player_other_list.Count; i++) {
			CharacterPlayerOther p = player_other_list[i];
			if (p.character_other_property.getSeverID() == id) {
				return p;
			}
		}
		
		return null;
	}

    public int GetPlayerOtherNum()
    {
        return player_other_list.Count;
    }
	
	public Character getNearestPlayer(Vector3 pos, out float distNearest) 
    {
		
		float minDist = Mathf.Infinity;
		float dist = 0;
		Character ret = null;

        if (!CharacterAI.IsInState(CharacterPlayer.sPlayerMe, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            dist = Vector3.Distance(CharacterPlayer.sPlayerMe.getPosition(), pos);
            if (dist < minDist)
            {
                ret = CharacterPlayer.sPlayerMe;
                minDist = dist;
            }
        }
		
		
		for (int i = 0; i < player_other_list.Count; i++) 
        {
			CharacterPlayerOther p = player_other_list[i];

            if (CharacterAI.IsInState(p, CharacterAI.CHARACTER_STATE.CS_DIE))
            {
                continue;
            }

			dist = Vector3.Distance(p.getPosition(), pos);
			if (dist < minDist) 
            {
				ret = p;
				minDist = dist;
			}
		}
		
		distNearest = minDist;
		return ret;
	}

    public void ClearHasBeenDamagedFlag()
    {
        // 这里受击的都是怪 而且是副本中的，需要扩展到 角色
        for (int i = 0; i < player_other_list.Count; i++)
        {
            CharacterPlayerOther others = player_other_list[i];
            others.m_bHasBeenDamaged = false;
        }

        if (CharacterPlayer.sPlayerMe)
        {
            CharacterPlayer.sPlayerMe.m_bHasBeenDamaged = false;
        }
    }

    public CharacterPlayerOther RandomSelectAnOthers()
    {
        if (player_other_list.Count != 0)
        {
            int nRandomIndex = UnityEngine.Random.Range(0, player_other_list.Count);

            return player_other_list[nRandomIndex];
        }

        return null;
    }


	// once receive other appear message, 
	// 1. check if his id is already in id list
	// if not exist add it to the list, else return
	// 2. calculate the distance between he with me, 

	public void OnPlayerAppear(GSNotifyRoleAppear roleAppear)
	{
		CharacterPlayerOther other = CreateOthersImmidiately(roleAppear);

		m_unAppearOtherID = (uint)other.GetProperty().GetInstanceID();
	}

	public void OnPlayerDisAppear(CharacterPlayerOther kCharacter)
	{
		kCharacter.m_bVisible = true;

		m_nCurrentVisiblePlayerOther--;

		if (m_nCurrentVisiblePlayerOther < 0)
			m_nCurrentVisiblePlayerOther = 0;

		Debug.Log("dec " + m_nCurrentVisiblePlayerOther);


		List<int> listIndex = new List<int>();
		List<float> listDist = new List<float>();

		for (int i = 0; i < player_other_list.Count; ++i) 
		{
			if (!player_other_list[i].m_bVisible)
			{
				// check him is inside camera
				if (GraphicsUtil.IsCharacterInsideCamera(player_other_list[i], Camera.main))
				{
					listIndex.Add(i);
					listDist.Add(Vector3.Distance(CharacterPlayer.sPlayerMe.getPosition(),
					                              player_other_list[i].getPosition()));
				}
			}
		}

		// calculate min dist playerother
		float fMinDist = float.MaxValue;
		int nIndex = -1;

		for (int j = 0; j < listDist.Count; ++j)
		{
			if (listDist[j] < fMinDist)
			{
				fMinDist = listDist[j];
				nIndex = j;
			}
		}

		if (nIndex != -1)
		{
			int nPlayerIndex = listIndex[nIndex];
			GraphicsUtil.ShowGameObj(player_other_list[nPlayerIndex]);
			player_other_list[nPlayerIndex].m_bVisible = true;
			m_nCurrentVisiblePlayerOther++;

			Debug.Log("inc " + m_nCurrentVisiblePlayerOther);
		}
	}


	// update check new other
	public void CheckNewOther()
	{
		if (Camera.main == null) return;

		if (Camera.main.transform.position == Vector3.zero) return;

		if (m_unAppearOtherID == 0) return;

		for (int i = 0; i < player_other_list.Count; ++i)
		{
			CharacterPlayerOther other = player_other_list[i];
			
			if (GraphicsUtil.IsCharacterInsideCamera(other, Camera.main))
			{
				if (m_nCurrentVisiblePlayerOther >= SettingManager.Instance.CurDisplayVo.DisplayPeopleCount)
				{
					GraphicsUtil.HideGameObj(other);
					other.m_bVisible = false;
				}
				else
				{
					GraphicsUtil.ShowGameObj(other);
					other.m_bVisible = true;
					m_nCurrentVisiblePlayerOther++;
					
					Debug.Log("inc new" + m_nCurrentVisiblePlayerOther);
				}
				
				other.m_bOutCam = false;
			}
			else
				other.m_bOutCam = true;
		}

		m_unAppearOtherID = 0;
	}

	// update other destory
	public void CheckDestoryOther(CharacterPlayerOther other)
	{
		if (Camera.main == null) return;

		if (Camera.main.transform.position == Vector3.zero) return;

		if (!other.m_bOutCam)
		{
			if (other.m_bVisible)
			{
				Debug.Log("someone run out my view dead");
				PlayerManager.Instance.OnPlayerDisAppear(other);
			}
			
			other.m_bOutCam = true;
		}
	}

	// move detect
	public void CheckOtherAppear()
	{
		if (Camera.main == null) return;

		if (Camera.main.transform.position == Vector3.zero) return;

		for (int i = 0; i < player_other_list.Count; ++i)
		{
			CharacterPlayerOther other = player_other_list[i];
			
			if (other.m_bOutCam)
			{
				if (GraphicsUtil.IsCharacterInsideCamera(other, Camera.main))
				{
					if (m_nCurrentVisiblePlayerOther >= SettingManager.Instance.CurDisplayVo.DisplayPeopleCount)
					{
						GraphicsUtil.HideGameObj(other);
						other.m_bVisible = false;
					}
					else
					{
						GraphicsUtil.ShowGameObj(other);
						other.m_bVisible = true;
						m_nCurrentVisiblePlayerOther++;
						
						Debug.Log("inc move" + m_nCurrentVisiblePlayerOther);
					}
					
					other.m_bOutCam = false;
				}
			}
		}
	}

	public void CheckOtherDisappear()
	{
		if (Camera.main == null) return;

		if (Camera.main.transform.position == Vector3.zero) return;

		for (int i = 0; i < player_other_list.Count; ++i)
		{
			CharacterPlayerOther other = player_other_list[i];

			if (!other.m_bOutCam)
			{
				if (!GraphicsUtil.IsCharacterInsideCamera(other, Camera.main))
				{
					if (other.m_bVisible)
					{
						Debug.Log("someone run out my view CheckOtherDisappear");
						PlayerManager.Instance.OnPlayerDisAppear(other);
					}

					other.m_bOutCam = true;
				}
			}
		}
	}


	public void OnVisiblePlayerNumChanged(int nNum)
	{
		if (nNum == SettingManager.Instance.CurDisplayVo.DisplayPeopleCount)
			return;

		if (nNum > m_nCurrentVisiblePlayerOther)
		{
			// add more visible player
			int nAddedNum = nNum - m_nCurrentVisiblePlayerOther;

			List<int> listIndex = new List<int>();
			List<float> listDist = new List<float>();


			for (int i = 0; i < player_other_list.Count; ++i)
			{
				CharacterPlayerOther other = player_other_list[i];
				
				if (!other.m_bOutCam)
				{
					// inside camera view frustum
					if (!player_other_list[i].m_bVisible)
					{
						listIndex.Add(i);
						listDist.Add(Vector3.Distance(CharacterPlayer.sPlayerMe.getPosition(),
						                              player_other_list[i].getPosition()));
					}
				}
			}

			// find out neasteast list 
			List<int> addListIndex = new List<int>();
			List<float> addListDist = new List<float>();

			for (int n = 0; n < nAddedNum; ++n)
			{
				float fMinDist = float.MaxValue;
				int nIndex = -1;
				
				for (int j = 0; j < listDist.Count; ++j)
				{
					if (listDist[j] < fMinDist)
					{
						fMinDist = listDist[j];
						nIndex = j;
					}
				}

				if (nIndex != -1)
				{
					addListIndex.Add(listIndex[nIndex]);
					listDist[nIndex] = float.MaxValue;
				}
			}

			for (int m = 0; m < addListIndex.Count; ++m)
			{
				int nPlayerIndex = addListIndex[m];
				GraphicsUtil.ShowGameObj(player_other_list[nPlayerIndex]);
				player_other_list[nPlayerIndex].m_bVisible = true;
				m_nCurrentVisiblePlayerOther++;
				
				Debug.Log("inc " + m_nCurrentVisiblePlayerOther);
			}
		}
		else
		{
			// remove more visible player
			int nAddedNum = m_nCurrentVisiblePlayerOther - nNum;
			
			List<int> listIndex = new List<int>();
			List<float> listDist = new List<float>();
			
			
			for (int i = 0; i < player_other_list.Count; ++i)
			{
				CharacterPlayerOther other = player_other_list[i];
				
				if (!other.m_bOutCam)
				{
					// inside camera view frustum
					if (player_other_list[i].m_bVisible)
					{
						listIndex.Add(i);
						listDist.Add(Vector3.Distance(CharacterPlayer.sPlayerMe.getPosition(),
						                              player_other_list[i].getPosition()));
					}
				}
			}
			
			// find out farest list 
			List<int> addListIndex = new List<int>();
			List<float> addListDist = new List<float>();
			
			for (int n = 0; n < nAddedNum; ++n)
			{
				float fMaxDist = -1.0f;
				int nIndex = -1;
				
				for (int j = 0; j < listDist.Count; ++j)
				{
					if (listDist[j] > fMaxDist)
					{
						fMaxDist = listDist[j];
						nIndex = j;
					}
				}
				
				if (nIndex != -1)
				{
					addListIndex.Add(listIndex[nIndex]);
					listDist[nIndex] = -1.0f;
				}
			}
			
			for (int m = 0; m < addListIndex.Count; ++m)
			{
				int nPlayerIndex = addListIndex[m];
				GraphicsUtil.HideGameObj(player_other_list[nPlayerIndex]);
				player_other_list[nPlayerIndex].m_bVisible = false;

				m_nCurrentVisiblePlayerOther--;
				
				if (m_nCurrentVisiblePlayerOther < 0)
					m_nCurrentVisiblePlayerOther = 0;
				
				Debug.Log("dec " + m_nCurrentVisiblePlayerOther);
			}
		}
	}
}
