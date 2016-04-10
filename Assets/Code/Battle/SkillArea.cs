using UnityEngine;
using System.Collections;

public class SkillArea : MonoBehaviour {
	
	SkillAppear skill;
	bool hurting = true;
	public Vector3 fly_dir = Vector3.forward;
	public float fly_speed = 0;

    public DAMAGE_TIMES m_eHurtType;

	public float cd_max = 1.0f;
	public int hurt_max = 0;
	float cd = 1000000;
	int hurt = 0;

    float m_fLastTimeClearDirtyState;
	// Use this for initialization
	void Start () 
    {
        m_fLastTimeClearDirtyState = 0.0f;

        // 目前根据 碰撞盒所属技能是否多次伤害来判断，将来 单 多次应该跟盒子绑定
        if (skill.m_kSkillInfo.skillDamageInterval > 0.0f)
        {
            m_eHurtType = DAMAGE_TIMES.DT_MULTI;
        }
        else 
        {
            m_eHurtType = DAMAGE_TIMES.DT_ONCE;
        }

        CharacterAnimCallback.ClearFightDirty(skill.getOwner());
	}
	
	// Update is called once per frame
	void Update  () {
		
		if (MainLogic.sMainLogic.isGameSuspended()) {
			return;
		}
	
		if (fly_speed > 0.001f) {
			transform.position += fly_dir * fly_speed * Time.deltaTime;
		}
		
        //if (hurt_type == 1) {
		
        //    if (hurt >= hurt_max) {
        //        hurting = false;
        //        return;
        //    }
			
        //    if (cd < cd_max) {
        //        hurting = false;
        //        cd += Time.deltaTime;
        //    }
        //    else {
        //        hurting = true;
        //        hurt++;
        //        cd = 0;
        //    }
        //}


        // 持续伤害
        if (skill.m_kSkillInfo.skillDamageInterval > 0.0f)
        {
            if (m_fLastTimeClearDirtyState > skill.m_kSkillInfo.skillDamageInterval)
            {
                MonsterManager.Instance.ClearHasBeenDamagedFlag();
                PlayerManager.Instance.ClearHasBeenDamagedFlag();
                m_fLastTimeClearDirtyState = 0.0f;
            }

            m_fLastTimeClearDirtyState += Time.deltaTime;
        }
	}
	
    //public void setType(int t) {
    //    hurt_type = t;
    //}
	
	public void setSkill(SkillAppear s) {
		skill = s;
	}
	
	public SkillAppear getSkill() {
		return skill;
	}
	
	void OnTriggerEnter(Collider other) 
    {
        Character target = other.gameObject.GetComponent<Character>();

        if (!target || target == skill.getOwner() || null == skill.getOwner())
        {
            return;
        }


        if (CharacterAI.IsInState(target, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            // 死了不能鞭尸
            return;
        }
        


        // 同类不能相互伤害
        if (skill.getOwner().getType() == target.getType())
        {
            return;
        }


        if (!BattleMultiPlay.GetInstance().CollisionJudegeValid(skill.getOwner(), target))
        {
            return;
        }


        // 对象已死
        if (CharacterAI.IsInState(target, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            return;
        }

		if (skill.getOwner().getType() == CharacterType.CT_PLAYER)
			CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_FIGHT_SHAKE, 0.025f);

        BattleManager.Instance.SkillCastProcess(skill, target, CollisionUtil.CalculateJiGuanHitPoint(target, gameObject));


        if (m_eHurtType == DAMAGE_TIMES.DT_ONCE)
        {
            GameObject.Destroy(gameObject);
        }
	}
	
	void OnTriggerStay(Collider other) 
    {
		if (m_eHurtType != DAMAGE_TIMES.DT_MULTI) 
            return;


        Character target = other.gameObject.GetComponent<Character>();

        if (!target || target == skill.getOwner() || null == skill.getOwner())
        {
            return;
        }

        if (CharacterAI.IsInState(target, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            // 死了不能鞭尸
            return;
        }


        // 同类不能相互伤害
        if (skill.getOwner().getType() == target.getType())
        {
            return;
        }

        if (!BattleMultiPlay.GetInstance().CollisionJudegeValid(skill.getOwner(), target))
        {
            return;
        }

        // 对象已死
        if (CharacterAI.IsInState(target, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            return;
        }

		if (skill.getOwner().getType() == CharacterType.CT_PLAYER)
			CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_FIGHT_SHAKE, 0.025f);

        BattleManager.Instance.SkillCastProcess(skill, target, CollisionUtil.CalculateJiGuanHitPoint(target, gameObject));
    }
}
