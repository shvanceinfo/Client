using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	
	public Character holder;
	
	public AudioSource weapon_sound;

    // 某个武器特有的技能
    public WeaponSkill m_kSkill;

    public bool m_bStartSkill = false;

	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setHolder(Character c) {
		holder = c;
	}
	
	public void playWeaponSound() {
		weapon_sound.Play();
	}

    public void OnTriggerEnter(Collider other)
    {
        m_bStartSkill = true;
        OnTriggerUpdate(other);
    }

    public void OnTriggerStay(Collider other)
    {
        if (m_bStartSkill)
        {
            OnTriggerUpdate(other);
        }
    }

    public void OnTriggerUpdate(Collider other)
    {
        if (holder == null)
        {
            return;
        }

        if (holder.GetAI() == null)
        {
            return;
        }


        // 不是普攻 刀就没用
        if (holder.GetAI().GetCharacterState() != CharacterAI.CHARACTER_STATE.CS_ATTACK)
        {
            return;
        }

        Character character = other.gameObject.GetComponent<Character>();

        // 刀碰到自己
        if (character == holder)
        {
            return;
        }


        // 碰到非生物
        if (character == null)
        {
            return;
        }

        if (character.m_bHasBeenDamaged)
        {
            return;
        }


        if (CharacterAI.IsInState(character, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            // 死了不能鞭尸
            return;
        }


        if (!BattleMultiPlay.GetInstance().CollisionJudegeValid(holder, character))
        {
            return;
        }

        if (!BattleWorldBoss.GetInstance().CollisionJudegeValid(holder, character))
        {
            return;
        }

        // 平砍的几段动作不会造成伤害
        SkillAttackBaseCmd kPingkanSkill = holder.skill.getCurrentSkill() as SkillAttackBaseCmd;

        if (kPingkanSkill == null)
        {
            return;
        }

        // 对象已死
        if (CharacterAI.IsInState(character, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            return;
        }

        if (kPingkanSkill.m_ePingKanState == PING_KAN.PK_2 ||
            kPingkanSkill.m_ePingKanState == PING_KAN.PK_5 ||
            kPingkanSkill.m_ePingKanState == PING_KAN.PK_8 ||
            kPingkanSkill.m_ePingKanState == PING_KAN.PK_11)
        {
            // 碰到其他人或者怪物

            if (m_bStartSkill)
            {
                CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_FIGHT_SHAKE, 0.025f);


                BattleManager.Instance.OnWeaponHitCharacter(holder, character, CollisionUtil.CalculateJiGuanHitPoint(character, gameObject));
                m_bStartSkill = false;
            }
        }
    }
}
