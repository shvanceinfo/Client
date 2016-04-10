using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAI 
{
    public enum CHARACTER_STATE
    {
        CS_BORN = 0,
        CS_IDLE,
        CS_MOVE,
        CS_PURSUE,
        CS_ATTACK,
        CS_SKILL,
        CS_BE_HIT,
        CS_DIE,
        CS_DIZZY,
        CS_RELIVE //复活
    }

    protected Character m_kOwner;


    public StateMachine m_kMachine;

    public GlobalState m_kGlobalState;

    public BornState m_kBornState;
    public IdleState m_kIdleState;
    public MoveState m_kMoveState;
    public SkillState m_kSkillState;
    public AttackState m_kAttackState;
    public PursueState m_kPursueState;
    public BeHitState m_kBeHitState;
    public DieState m_kDieState;
    public MonsterGoblinState m_kGoblinState;
    public DizzyState m_kDizzyState;


    public CHARACTER_STATE m_ePlayerState = CHARACTER_STATE.CS_IDLE;


    // character AI collection

    public struct AISkillData
    {
        public int nAIID;
        public int nSkillID;
    }

    public List<AISkillData> m_kAISkillBehavious = new List<AISkillData>();


    // 这个变量用来表示第三方的角色是否带AI
    public bool m_bAIAvailable = false;


    public virtual void SetCharacterState(CHARACTER_STATE ms)
    {
        m_ePlayerState = ms;
    }


    public virtual CHARACTER_STATE GetCharacterState()
    {
        return m_ePlayerState;
    }

    public Character getOwner()
    {
        return m_kOwner;
    }

    public virtual void init(Character c)
    {
        m_kOwner = c;
    }


    public virtual void ProcessMessage(StateEvent tel)
    {
        if (m_kMachine != null)
        {
            m_kMachine.ProcessMessage(tel);
        }
    }


    public virtual void Update()
    {
        if (m_kMachine != null)
        {
            m_kMachine.Update();
        }
    }

	//protected float attack_cd = 0;
	
	
	
    //public virtual float getExtrusionRange() {
    //    return extrusion_range;
    //}
	
    //public virtual float getAttackCD() {
    //    return attack_cd;
    //}


    public static bool IsInState(Character character, CHARACTER_STATE eState)
    {
        if (Global.inMultiFightMap() && !CharacterPlayer.character_property.getHostComputer())
        {
            // 多人副本只有表现
            return character.m_eAppearState == GetAppearStateByMachineState(eState);
        }
        else
        {
            if (character.GetAI() != null)
            {
                // 有AI就用状态来判断
                return character.GetAI().GetCharacterState() == eState;
            }
            else
            {
                return character.m_eAppearState == GetAppearStateByMachineState(eState);
            }
        }
    }

    // 状态机的状态转换到表现状态
    public static Appear.BATTLE_STATE GetAppearStateByMachineState(CHARACTER_STATE state)
    {
        switch (state)
        {
        case CHARACTER_STATE.CS_BORN:
                return Appear.BATTLE_STATE.BS_BORN;
        case CHARACTER_STATE.CS_IDLE:
                return Appear.BATTLE_STATE.BS_IDLE;
        case CHARACTER_STATE.CS_PURSUE:
        case CHARACTER_STATE.CS_MOVE:
                return Appear.BATTLE_STATE.BS_MOVE;
        case CHARACTER_STATE.CS_ATTACK:
                return Appear.BATTLE_STATE.BS_PING_KAN;
        case CHARACTER_STATE.CS_SKILL:
                return Appear.BATTLE_STATE.BS_SKILL;
        case CHARACTER_STATE.CS_BE_HIT:
                return Appear.BATTLE_STATE.BS_BE_HIT;
        case CHARACTER_STATE.CS_DIE:
                return Appear.BATTLE_STATE.BS_DIE;
        case CHARACTER_STATE.CS_DIZZY:
                return Appear.BATTLE_STATE.BS_DIZZY;
        }

        return Appear.BATTLE_STATE.BS_NULL;
    }

    public virtual bool FindEnemy()
    {
        FightProperty m_kFightProperty = getOwner().GetComponent<FightProperty>();
		
		if (m_kFightProperty)
		{
			if (m_kFightProperty.m_kLockedEnemy != null)
        {
            Vector3 dir = m_kFightProperty.m_kLockedEnemy.transform.position - m_kOwner.getPosition();
            dir.Normalize();
            m_kOwner.setFaceDir(dir);
            return true;
        }

        //Character enemy = GetNearestEnemy();
        Character enemy = BattleManager.Instance.GetViewRangeEnemy(getOwner());
        if (enemy)
        {
            m_kFightProperty.SetLockedEnemy(enemy);
            return true;
        }

        return false;	
		}
        return false;	
    }



    // 所有的角色都有可能有自己最近的敌人
    public virtual Character GetNearestEnemy()
    {
        if (Global.inFightMap() || Global.inGoldenGoblin() || 
			Global.inTowerMap() || Global.inMultiFightMap() ||
		    Global.InWorldBossMap()||Global.InAwardMap())
        {
			float dist = 0.0f;
            return MonsterManager.Instance.GetNearestMonster(out dist);
        }
        else
        {
            if (Global.InArena())
            {
                if (m_kOwner.getType() == CharacterType.CT_PLAYER)
                {
                    return BattleArena.GetInstance().m_kChallenger;
                }
                else if (m_kOwner.getType() == CharacterType.CT_PLAYEROTHER)
                {
                    return CharacterPlayer.sPlayerMe;
                }
            }
            
            //return CharacterPlayer.sPlayerMe.m_kGMEnemy;
        }

        return null;
    }

    public virtual void SendStateMessage(CHARACTER_STATE state, Vector3 param)
    {
        StateEvent tel = new StateEvent();
        tel.state = state;
        tel.paramList.Add(param);
        ProcessMessage(tel);
    }


    public virtual void SendStateMessage(CHARACTER_STATE state)
    {
        StateEvent tel = new StateEvent();
        tel.state = state;

        ProcessMessage(tel);
    }

    public virtual void SendStateMessage(CHARACTER_STATE state, int param)
    {
        StateEvent tel = new StateEvent();
        tel.state = state;
        tel.paramList.Add(param);
        ProcessMessage(tel);
    }

    public virtual void SendStateMessage(CHARACTER_STATE state, float param)
    {
        StateEvent tel = new StateEvent();
        tel.state = state;
        tel.paramList.Add(param);
        ProcessMessage(tel);
    }

    public virtual void SendStateMessage(CHARACTER_STATE state, Character param)
    {
        StateEvent tel = new StateEvent();
        tel.state = state;
        tel.paramList.Add(param);
        ProcessMessage(tel);
    }

    public virtual void SendStateMessage(CHARACTER_STATE state, ArrayList param)
    {
        StateEvent tel = new StateEvent();
        tel.state = state;
        tel.paramList = param;
        ProcessMessage(tel);
    }
}
