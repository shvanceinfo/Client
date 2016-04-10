using UnityEngine;

public class MissleManager 
{
	private static MissleManager _instance = null;

	public static MissleManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = new MissleManager();
            return _instance;
        }
    }

    private MissleManager()
    {
    }
	
	public void createMissle(GameObject prefab, Vector3 pos, Vector3 dir, float speed, float time, CharacterMonster monster) {

        GameObject missleObject = BundleMemManager.Instance.instantiateObj(prefab, pos, Quaternion.identity);
		Rigidbody rb =  missleObject.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.isKinematic = true;
		missleObject.AddComponent("Missile");
		missleObject.transform.LookAt(missleObject.transform.position + dir);
		Missile missle = missleObject.GetComponent<Missile>();
		dir.Normalize();
		missle.dir = dir;
		missle.speed = speed;
		missle.left_time = time;
		missle.skill = monster.skill.getCurrentSkill();
	}
	
	public void createSkillArea(string res, Vector3 pos, Transform parent, SkillAppear skill, float life, int hurtType, int hurtMax = 0, float cdMax = 1.0f) {
		
		GameObject skillAreaObject = null;
        GameObject asset = BundleMemManager.Instance.getPrefabByName(res, EBundleType.eBundleRoleEffect);
		if (asset != null)
		{
            skillAreaObject = BundleMemManager.Instance.instantiateObj(asset, pos, Quaternion.identity);
            if (parent)
            {
                skillAreaObject.transform.parent = parent;
                skillAreaObject.transform.localPosition = pos;
                skillAreaObject.transform.localRotation = Quaternion.identity;
            }
            Rigidbody rb = skillAreaObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            skillAreaObject.AddComponent("SkillArea");
            //skillAreaObject.transform.LookAt(missleObject.transform.position + dir);
            SkillArea skillArea = skillAreaObject.GetComponent<SkillArea>();
            skillArea.m_eHurtType = skill.m_kSkillInfo.skillDamageInterval > 0.0f ? DAMAGE_TIMES.DT_MULTI : DAMAGE_TIMES.DT_ONCE;
            skillArea.hurt_max = hurtMax;
            skillArea.cd_max = cdMax;
            skillArea.setSkill(skill);
            Object.Destroy(skillAreaObject, life);
		}

		// try to trancate pre string "Model/Prefabe"
		if (asset == null) 
		{
            if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleRoleEffect))
            {
                BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleRoleEffect, res,
                (asset1) =>
                {
                    skillAreaObject = BundleMemManager.Instance.instantiateObj(asset1, pos, Quaternion.identity);
                    if (parent)
                    {
                        skillAreaObject.transform.parent = parent;
                        skillAreaObject.transform.localPosition = pos;
                        skillAreaObject.transform.localRotation = Quaternion.identity;
                    }
                    Rigidbody rb = skillAreaObject.AddComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    skillAreaObject.AddComponent("SkillArea");
                    //skillAreaObject.transform.LookAt(missleObject.transform.position + dir);
                    SkillArea skillArea = skillAreaObject.GetComponent<SkillArea>();
                    skillArea.m_eHurtType = skill.m_kSkillInfo.skillDamageInterval > 0.0f ? DAMAGE_TIMES.DT_MULTI : DAMAGE_TIMES.DT_ONCE;
                    skillArea.hurt_max = hurtMax;
                    skillArea.cd_max = cdMax;
                    skillArea.setSkill(skill);
                    Object.Destroy(skillAreaObject, life);
                }, CharacterPlayer.character_property.career);
            }
            else
            {
                asset = BundleMemManager.Instance.getPrefabByName(res, EBundleType.eBundleRoleEffect);
			    skillAreaObject = BundleMemManager.Instance.instantiateObj(asset, pos, Quaternion.identity);
                if (parent)
                {
                    skillAreaObject.transform.parent = parent;
                    skillAreaObject.transform.localPosition = pos;
                    skillAreaObject.transform.localRotation = Quaternion.identity;
                }

                Rigidbody rb = skillAreaObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
                skillAreaObject.AddComponent("SkillArea");
                //skillAreaObject.transform.LookAt(missleObject.transform.position + dir);
                SkillArea skillArea = skillAreaObject.GetComponent<SkillArea>();
                skillArea.m_eHurtType = skill.m_kSkillInfo.skillDamageInterval > 0.0f ? DAMAGE_TIMES.DT_MULTI : DAMAGE_TIMES.DT_ONCE;
                skillArea.hurt_max = hurtMax;
                skillArea.cd_max = cdMax;
                skillArea.setSkill(skill);
                Object.Destroy(skillAreaObject, life);
            }					
		}
	}
	
	public void createSkillArea(GameObject skillPrefab, Vector3 pos, Transform parent, SkillAppear skill, float life, int hurtType, int hurtMax = 0, float cdMax = 1.0f) {
		
		GameObject skillAreaObject = null;
        GameObject asset = BundleMemManager.Instance.getPrefabByName(skillPrefab.name, EBundleType.eBundleRaid);
		if (asset != null)
		{
            skillAreaObject = BundleMemManager.Instance.instantiateObj(asset, pos, Quaternion.identity);
		}
		else
		{
            skillAreaObject = BundleMemManager.Instance.instantiateObj(skillPrefab, pos, Quaternion.identity);
		}
		
		if (parent) {
			skillAreaObject.transform.parent = parent;
			skillAreaObject.transform.localPosition = pos;
			skillAreaObject.transform.localRotation = Quaternion.identity;
		}
		
		Rigidbody rb =  skillAreaObject.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.isKinematic = true;
		skillAreaObject.AddComponent("SkillArea");
		//skillAreaObject.transform.LookAt(missleObject.transform.position + dir);
		SkillArea skillArea = skillAreaObject.GetComponent<SkillArea>();
        skillArea.m_eHurtType = skill.m_kSkillInfo.skillDamageInterval > 0.0f ? DAMAGE_TIMES.DT_MULTI : DAMAGE_TIMES.DT_ONCE;
		skillArea.hurt_max = hurtMax;
		skillArea.cd_max = cdMax;
		skillArea.setSkill(skill);
        Object.Destroy(skillAreaObject, life);
	}

    public void CreateSkillAreaMissle(Character character, string res, Vector3 pos, Vector3 dir, float speed,
        SkillAppear skill, float life, int hurtType, int hurtMax = 0, float cdMax = 1.0f)
    {
        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleRoleEffect))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleRoleEffect, res,
                (asset) =>
                {
                    GameObject skillAreaObject = BundleMemManager.Instance.instantiateObj(asset, pos, Quaternion.identity);
                    Rigidbody rb = skillAreaObject.AddComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    skillAreaObject.AddComponent("SkillArea");
                    skillAreaObject.transform.LookAt(skillAreaObject.transform.position + dir);
                    SkillArea skillArea = skillAreaObject.GetComponent<SkillArea>();
                    skillArea.fly_dir = dir;
                    skillArea.fly_speed = speed;
                    skillArea.m_eHurtType = skill.m_kSkillInfo.skillDamageInterval > 0.0f ? DAMAGE_TIMES.DT_MULTI : DAMAGE_TIMES.DT_ONCE;
                    skillArea.hurt_max = hurtMax;
                    skillArea.cd_max = cdMax;
                    skillArea.setSkill(skill);
                    Object.Destroy(skillAreaObject, life);
                    if ((CharacterAI.IsInState(character, CharacterAI.CHARACTER_STATE.CS_ATTACK) &&
                       character.getType() == CharacterType.CT_PLAYER) ||
                      (CharacterAI.IsInState(character, CharacterAI.CHARACTER_STATE.CS_SKILL) &&
                       character.getType() == CharacterType.CT_PLAYEROTHER))
                    {
                        // 普通攻击才会出单只剑
                        // 弓箭特效 暂时如此 
                        int nWeaponId = character.GetProperty().getWeapon();
                        string arrow = ConfigDataManager.GetInstance().getEquipmentConfig().getEquipData(nWeaponId).weapon_trail_texture;
                        if (arrow != "")
                        {
                            if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleWeaponEffect))
                            {
                                BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleWeaponEffect,
                                    arrow,
                                    (asset1) =>
                                    {
                                        GameObject gongjian = BundleMemManager.Instance.instantiateObj(asset1, pos,
                                            Quaternion.identity);
                                        gongjian.transform.parent = skillAreaObject.transform;
                                        gongjian.transform.localPosition = Vector3.zero;
                                        gongjian.transform.localRotation = Quaternion.identity;
                                    }, CharacterPlayer.character_property.career);
                            }
                            else
                            {
                                Object obj = BundleMemManager.Instance.getPrefabByName(arrow, EBundleType.eBundleWeaponEffect);
                                if (obj != null)
                                {
                                    GameObject gongjian = BundleMemManager.Instance.instantiateObj(obj, pos, Quaternion.identity);
                                    gongjian.transform.parent = skillAreaObject.transform;
                                    gongjian.transform.localPosition = Vector3.zero;
                                    gongjian.transform.localRotation = Quaternion.identity;
                                }
                            }
                        }
                    }
                }, CharacterPlayer.character_property.career);
        }
        else
        {
            GameObject asset = BundleMemManager.Instance.getPrefabByName(res, EBundleType.eBundleRoleEffect);
            GameObject skillAreaObject = BundleMemManager.Instance.instantiateObj(asset, pos, Quaternion.identity);
            Rigidbody rb = skillAreaObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            skillAreaObject.AddComponent("SkillArea");
            skillAreaObject.transform.LookAt(skillAreaObject.transform.position + dir);
            SkillArea skillArea = skillAreaObject.GetComponent<SkillArea>();
            skillArea.fly_dir = dir;
            skillArea.fly_speed = speed;
            skillArea.m_eHurtType = skill.m_kSkillInfo.skillDamageInterval > 0.0f ? DAMAGE_TIMES.DT_MULTI : DAMAGE_TIMES.DT_ONCE;
            skillArea.hurt_max = hurtMax;
            skillArea.cd_max = cdMax;
            skillArea.setSkill(skill);
			Object.Destroy(skillAreaObject, life);
            if ((CharacterAI.IsInState(character, CharacterAI.CHARACTER_STATE.CS_ATTACK) &&
                character.getType() == CharacterType.CT_PLAYER) ||
               (CharacterAI.IsInState(character, CharacterAI.CHARACTER_STATE.CS_SKILL) &&
                character.getType() == CharacterType.CT_PLAYEROTHER))
            {
                // 普通攻击才会出单只剑
                // 弓箭特效 暂时如此 
                int nWeaponId = character.GetProperty().getWeapon();
                string arrow =
                    ConfigDataManager.GetInstance().getEquipmentConfig().getEquipData(nWeaponId).weapon_trail_texture;
                if (arrow != "")
                {
                    if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleWeaponEffect))
                    {
                        BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleWeaponEffect,
                            arrow,
                            (asset1) =>
                            {
                                if (skillAreaObject == null)
                                    return;
                                GameObject gongjian = BundleMemManager.Instance.instantiateObj(asset1, pos,
                                    Quaternion.identity);
                                gongjian.transform.parent = skillAreaObject.transform;
                                gongjian.transform.localPosition = Vector3.zero;
                                gongjian.transform.localRotation = Quaternion.identity;
                            }, CharacterPlayer.character_property.career);
                    }
                    else
                    {
                        Object obj = BundleMemManager.Instance.getPrefabByName(arrow, EBundleType.eBundleWeaponEffect);
                        if (obj != null)
                        {
                            GameObject gongjian = BundleMemManager.Instance.instantiateObj(obj, pos, Quaternion.identity);
                            gongjian.transform.parent = skillAreaObject.transform;
                            gongjian.transform.localPosition = Vector3.zero;
                            gongjian.transform.localRotation = Quaternion.identity;
                        }
                    }
                }
            }
        }				
    }

    public void createSkillAreaAbsorb(string res, Vector3 pos, Transform parent, SkillAppear skill, float life)
	{
        GameObject asset = BundleMemManager.Instance.getPrefabByName(res, EBundleType.eBundleRaid);
        GameObject skillAreaObject = BundleMemManager.Instance.instantiateObj(asset, pos, Quaternion.identity);
		
		if (parent) {
			skillAreaObject.transform.parent = parent;
			skillAreaObject.transform.localPosition = pos;
		}
		
		Rigidbody rb =  skillAreaObject.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.isKinematic = true;
		skillAreaObject.AddComponent("SkillAreaAbsorb");
		SkillAreaAbsorb skillArea = skillAreaObject.GetComponent<SkillAreaAbsorb>();
		skillArea.setSkill(skill);
        Object.Destroy(skillAreaObject, life);
	}

    public void CreateSkillCollider(string res, Vector3 pos, Character character, SkillAppear skill)
    {
        float speed = Mathf.Abs(skill.m_kSkillInfo.collideSpeed);
        
        
        if (skill.m_kSkillInfo.rangeFollow == 0)
        {
            // 碰撞盒会独立移动

            CreateSkillAreaMissle(
                character,
                res,
                character.getPosition(),
                character.getFaceDir() * (skill.m_kSkillInfo.collideSpeed >= 0 ? 1 : -1 ),
                Mathf.Abs(skill.m_kSkillInfo.collideSpeed),
                skill,
                skill.m_kSkillInfo.collideLife,
                0
                );
        }
        else
        {
            // 碰撞盒跟角色走
            createSkillArea(
                res,
                pos, 
                character.transform,
                skill,
                skill.m_kSkillInfo.collideLife == 0 ? skill.GetLeftTime() : skill.m_kSkillInfo.collideLife,
                0
                );
        }

    }
}
