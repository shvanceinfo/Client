using UnityEngine;
using System.Collections;

// 本类实现机关相关的功能 (技能产生的机关和场景中的固有机关)
public class JiGuan : MonoBehaviour 
{
    public int m_nID = 0;   // 非技能产生的机关可以在预制件中配的ID

    public SkillAppear m_kSkillInfo = null;   // 产生机关的技能信息 如果为空表示不是技能产生的 只用到由技能带来的信息

    public JiGuanItem m_kJiGuanInfo; // 机关信息

    

    private float m_fDelayTimer = 0.0f;



    float m_fLastTimeClearDirtyState = 0.0f;

    private bool m_bCanDamage = false;

	private bool m_bExplosive = false;

	// Use this for initialization
	void Start () 
    {
		m_bExplosive = false;
        MonsterManager.Instance.ClearHasBeenDamagedFlag();
        PlayerManager.Instance.ClearHasBeenDamagedFlag();
	}

    public void SetSkillInfo(SkillAppear skill, int nSkillID)
    {
		skill.m_bSkillTriggedJiGuan = true;

        m_kSkillInfo = new SkillAppear(skill.GetSkillId());
        // 得到信息
        m_kSkillInfo.setOwner(skill.getOwner());
        m_kSkillInfo.init();

        SetSkillInfo(nSkillID);
    }

    public void SetSkillInfo(int nSkillID)
    {
        m_nID = nSkillID;

        if (m_nID != 0)
        {
            m_kJiGuanInfo = ConfigDataManager.GetInstance().GetJiGuanConfig().GetJiGuanData(m_nID);
        }
    }
	
	// Update is called once per frame
	void Update() 
    {
        m_fDelayTimer += Time.deltaTime;

        if (m_fDelayTimer > m_kJiGuanInfo.fLifeTime)
        {
            // 生命周期
			GenerateJiGuanEffect();
            GameObject.Destroy(gameObject);
            return;
        }


        if ( m_fDelayTimer < m_kJiGuanInfo.fDelay)
        {
            // 延时没到
            return;
        }

        m_bCanDamage = true;


        // 持续伤害
        if (m_kJiGuanInfo.eDamageType == DAMAGE_TIMES.DT_MULTI)
        {
            if (m_fLastTimeClearDirtyState > m_kJiGuanInfo.fIntervalTime)
            {
                MonsterManager.Instance.ClearHasBeenDamagedFlag();
                PlayerManager.Instance.ClearHasBeenDamagedFlag();
                m_fLastTimeClearDirtyState = 0.0f;
            }
            else
            {
                m_fLastTimeClearDirtyState += Time.deltaTime;
            }
        }
	}
	
	void OnTriggerEnter(Collider other) 
    {
        //if (m_kJiGuanInfo.eDamageType != DAMAGE_TIMES.DT_ONCE) 
            //return;

        if (!m_bCanDamage)
        {
            return;
        }

        Character target = other.gameObject.GetComponent<Character>();

        if (!target || target == m_kSkillInfo.getOwner() || null == m_kSkillInfo.getOwner())
        {
            return;
        }


        if (CharacterAI.IsInState(target, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            // 死了不能鞭尸
            return;
        }

        // 同类不能相互伤害
        if (m_kSkillInfo.getOwner().getType() == target.getType())
        {
            return;
        }


        if (!BattleMultiPlay.GetInstance().CollisionJudegeValid(m_kSkillInfo.getOwner(), target))
        {
            return;
        }

        if (m_kSkillInfo.getOwner().getType() == CharacterType.CT_PLAYER)
        {
            CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_FIGHT_SHAKE, 0.025f);
        }


        BattleManager.Instance.SkillCastProcess(m_kSkillInfo, target, Vector3.zero, true);

		GenerateJiGuanEffect();
        GameObject.Destroy(gameObject);
	}

    void OnTriggerStay(Collider other)
    {
        //if (m_kJiGuanInfo.eDamageType != DAMAGE_TIMES.DT_MULTI)
            //return;

        if (!m_bCanDamage)
        {
            return;
        }

        Character target = other.gameObject.GetComponent<Character>();

        if (!target || target == m_kSkillInfo.getOwner() || null == m_kSkillInfo.getOwner())
        {
            return;
        }

        if (CharacterAI.IsInState(target, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            // 死了不能鞭尸
            return;
        }

        // 同类不能相互伤害
        if (m_kSkillInfo.getOwner().getType() == target.getType())
        {
            return;
        }

        if (!BattleMultiPlay.GetInstance().CollisionJudegeValid(m_kSkillInfo.getOwner(), target))
        {
            return;
        }

        if (m_kSkillInfo.getOwner().getType() == CharacterType.CT_PLAYER)
        {
            CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_FIGHT_SHAKE, 0.025f);
        }

        BattleManager.Instance.SkillCastProcess(m_kSkillInfo, target, Vector3.zero, true);

        if (m_kJiGuanInfo.eDamageType != DAMAGE_TIMES.DT_ONCE)
        {
			GenerateJiGuanEffect();
            GameObject.Destroy(gameObject);
        }
    }

	void GenerateJiGuanEffect()
	{
		if (m_kJiGuanInfo.strEffect != null && !m_bExplosive)
		{
			Debug.Log("explosive");
			m_bExplosive = true;
			EffectManager.Instance.createFX (m_kJiGuanInfo.strEffect, gameObject.transform.position, Quaternion.identity);
		}
	}
}
