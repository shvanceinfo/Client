using UnityEngine;
using manager;

public enum CHAR_ANIM_CALLBACK
{
    CAC_PLAYER,
    CAC_OTHERS,
    CAC_MONSTER,
}

public class CharacterAnimCallback : MonoBehaviour {

    Character character;

    Transform bindPoint = null;

    public CHAR_ANIM_CALLBACK m_eCharType;

    void Awake()
    {
        m_eCharType = CHAR_ANIM_CALLBACK.CAC_PLAYER;
    }

	
    
	// Use this for initialization
	void Start () 
    {
        switch (m_eCharType)
        {
            case CHAR_ANIM_CALLBACK.CAC_PLAYER:
                character = GetComponent<CharacterPlayer>();
                break;
            case CHAR_ANIM_CALLBACK.CAC_OTHERS:
                character = GetComponent<CharacterPlayerOther>();
                break;
            case CHAR_ANIM_CALLBACK.CAC_MONSTER:
                character = GetComponent<CharacterMonster>();
                break;
        }

        if (character == null && bindPoint == null)
        {
            bindPoint = transform;
        }
	}

    public void SetCharType(CHAR_ANIM_CALLBACK type)
    {
        m_eCharType = type;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void playWeaponSound()
    {
        if (!character) return;
        Weapon w = character.GetComponentInChildren<Weapon>();
        if (!w) return;
        w.playWeaponSound();
    }

    public void playEffect(GameObject eff)
    {
        if (eff && character)
        {
            EffectManager.Instance.createFX(eff, character.transform, 10.0f);
        }
        else
        {
            EffectManager.Instance.createFX(eff, transform, 10.0f);
        }
    }

    public void playEffectWorldCoord(GameObject eff)
    {
        if (eff && character)
        {
            EffectManager.Instance.createFX(eff, character.transform.position, character.transform.rotation);
        }
    }

    public void showWeaponTrail()
    {
        if (!character) return;
        character.showWeaponTrail();
    }

    public void hideWeaponTrail()
    {
        if (!character) return;
        character.hideWeaponTrail();
    }

    public void ShakeCameraFrontBack(float time)
    {
		CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_FRONT_BACK, time);
    }

    public void shakeCameraUpDown(float time)
    {
		CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_UP_DOWN, time);
    }

    public void shakeCameraLeftRight(float time)
    {
		CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_LEFT_RIGHT, time);
    }

    public void shakeCameraRandomRadius(float time)
    {
		CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_SPHERE, time);
    }



	public void shakeCameraSmallPlayerFront(float time)
	{
		
		EffectManager.Instance.createCameraShake(0, 0.1f, time);
	}
	
	public void shakeCameraBigPlayerFront(float time)
	{
		
		EffectManager.Instance.createCameraShake(0, 0.6f, time);
	}
	
	public void shakeCameraSmallUpDown(float time)
	{
		
		EffectManager.Instance.createCameraShake(1, 0.1f, time);
	}
	
	public void shakeCameraBigUpDown(float time)
	{
		
		EffectManager.Instance.createCameraShake(1, 0.6f, time);
	}
	
	public void shakeCameraSmall(float time)
	{
		
		EffectManager.Instance.createCameraShake(2, 0.1f, time);
	}
	
	public void shakeCameraBig(float time)
	{
		
		EffectManager.Instance.createCameraShake(2, 0.6f, time);
	}
	



    public void createAddAttack()
    {

        if (character == null)
        {
            return;
        }


        //if (character.skill.addAttackEff == null) {
        //    character.skill.addAttackEff = EffectManager.sEffectManager.createFX("Effect/Effect_Prefab/Role/Skill/zhanyishalu", character.transform, 10000);
        //}

        character.AddBuff(1);
    }

    public void createAddDefend()
    {

        if (character == null)
        {
            return;
        }

        //if (character.skill.addDefendEff == null) {
        //    character.skill.addDefendEff = EffectManager.sEffectManager.createFX("Effect/Effect_Prefab/Role/Skill/yongqihudun", character.transform, 10000);
        //}

        character.AddBuff(2);
    }

    public void playAudio(AudioClip clip)
    {
        AudioManager.Instance.PlayAudio(this.gameObject, clip);
    }

    public void clearMonsterFightDirty()
    {
        CharacterAnimCallback.ClearFightDirty(character);
    }

    public static void ClearFightDirty(Character character)
    {
        if (character == null || character.skill == null || character.skill.getCurrentSkill() == null)
        {
            MonsterManager.Instance.ClearHasBeenDamagedFlag();
            PlayerManager.Instance.ClearHasBeenDamagedFlag();
            return;
        }

        if (character.skill.getCurrentSkill().m_kSkillInfo.SpecialEffectLoop == 1)
        {
            if (character.skill.getCurrentSkill().m_bDurationalSkillFirstClearFlag)
            {
                MonsterManager.Instance.ClearHasBeenDamagedFlag();
                PlayerManager.Instance.ClearHasBeenDamagedFlag();
                character.skill.getCurrentSkill().m_bDurationalSkillFirstClearFlag = false;
            }
        }
        else
        {
            MonsterManager.Instance.ClearHasBeenDamagedFlag();
            PlayerManager.Instance.ClearHasBeenDamagedFlag();
        }
    }

    public void GeneradeCollider()
    {
        if (!character)
            return;

        SkillAppear skillCmd = character.skill.getCurrentSkill();
        if (skillCmd != null && skillCmd.m_kSkillInfo.skillRangePre != null)
        {
            if (character.skill.getCurrentSkill().m_kSkillInfo.SpecialEffectLoop == 1)
            {
                if (character.skill.getCurrentSkill().m_bDurationalSkillFirstColliderFlag)
                {
                    MissleManager.Instance.CreateSkillCollider("Model/prefab/" + skillCmd.m_kSkillInfo.skillRangePre, Vector3.zero, character, skillCmd);
                    character.skill.getCurrentSkill().m_bDurationalSkillFirstColliderFlag = false;
                }
            }
            else
                MissleManager.Instance.CreateSkillCollider("Model/prefab/" + skillCmd.m_kSkillInfo.skillRangePre, Vector3.zero, character, skillCmd);
        }
    }

    public void RangeDamageHurt()
    {
        if (!character)
            return;

        SkillAppear kCurSkill = character.skill.getCurrentSkill();
        if (kCurSkill != null)
        {
            // draw lines
            //DrawRangeLines(kCurSkill.m_kSkillInfo.skillAngle, kCurSkill.m_kSkillInfo.skillRadius);
            BattleManager.Instance.RangeSkillDamageCal(kCurSkill);
        }
    }

    void DrawRangeLines(int nDegree, float radius)
    {
        Vector3 vecFaceDir = CharacterPlayer.sPlayerMe.getFaceDir();
        vecFaceDir.Normalize();



        Quaternion rotL = Quaternion.Euler(0, -nDegree * 0.5f, 0);
        GraphicsUtil.m_vecLineStart = CharacterPlayer.sPlayerMe.getPosition() +
            rotL * vecFaceDir * radius;

        GraphicsUtil.m_vecLineStart += new Vector3(0.0f, 0.1f, 0.0f);

        Quaternion rotR = Quaternion.Euler(0, nDegree * 0.5f, 0);
        GraphicsUtil.m_vecLineEnd = CharacterPlayer.sPlayerMe.getPosition() +
            rotR * vecFaceDir * radius;

        GraphicsUtil.m_vecLineEnd += new Vector3(0.0f, 0.1f, 0.0f);

        GraphicsUtil.m_vecOriginal = CharacterPlayer.sPlayerMe.getPosition();

    }
}
