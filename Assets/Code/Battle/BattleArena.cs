using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NetGame;
using manager;
using MVC.entrance.gate;

public class TimerToJudgeResult : MonoBehaviour
{
    int m_nTickNum = 0;
    const int MaxTime = 60;
    void Awake()
    {
        InvokeRepeating("JudgeFailed", 0.0f, 1.0f);

        FightManager.Instance.ItemData.ReTime = MaxTime;
        FightManager.Instance.ItemData.IsReTime = true;
        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_TIME);
    }

    void JudgeFailed()
    {
        m_nTickNum++;

        if (m_nTickNum == MaxTime)
        {
            BattleArena.GetInstance().PlayerDie();
        }
    }
}

public class TimerToTimeScale : MonoBehaviour
{
	void Awake()
	{
        EffectManager.Instance.BeginCloseUpEffect(Vector3.zero);

		Invoke("TimeScale", 1);
	}

	void TimeScale()
	{
        EffectManager.Instance.EndCloseUpEffect();
	}
}

class BattleArena
{
    public GameObject m_kTimer = null;

    public bool m_bPlayerWin = false;

    public CharacterPlayerOther m_kChallenger = null;

    public GSNotifyChallegerInfo m_kInfo = null;

    static private BattleArena _instance;

    static public BattleArena GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BattleArena();
            return _instance;
        }

        return _instance;
    }

    public bool m_bStartFight = false;

    public bool m_bFirstJudgeWin = true;

    public BattleArena()
    {
        
    }

    public void Init()
    {

    }


    // 准备阶段 创建竞技场信息
    public void PrepareBattleArena()
    {
        if (m_kInfo != null)
        {
            GenerateChallenger(m_kInfo);
        }
    }

    // 开始战斗
    public void StartBattleArena()
    {
        if (m_kChallenger)
        {
            m_bStartFight = true;
            m_bFirstJudgeWin = true;
            Global.m_bAutoFight = true;
            m_bPlayerWin = false;

            if (m_kTimer == null)
            {
                m_kTimer = new GameObject();
                m_kTimer.name = "time_to_judge";
                m_kTimer.AddComponent<TimerToJudgeResult>();
            }
            
        }
    }

    // 竞技场结束后的工作
    public void EndFight()
    {
        m_bStartFight = false;
        if (m_kChallenger)
        {
            if (m_kChallenger.gameObject)
            {
				m_kTimer.AddComponent<TimerToTimeScale>();
            }
        }

        Global.m_bAutoFight = false;
		
        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_STOP_TIME);
		ArenaManager.Instance.showAwardInfo ();

        Global.m_bInGame = false;
    }

    public void PlayerDie()
    {
        if (m_bFirstJudgeWin)
        {
            Debug.Log("玩家死亡");
            m_bPlayerWin = false;

            m_bFirstJudgeWin = false;

            //m_kChallenger.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
            m_kChallenger.GetAI().m_bAIAvailable = false;

            m_kInfo = null;

            EndFight();
        }
    }

    public void ComputerDie()
    {
        if (m_bFirstJudgeWin)
        {
            Debug.Log("电脑死亡");
            m_bPlayerWin = true;

            m_bFirstJudgeWin = false;

            CharacterPlayer.sPlayerMe.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);

            m_kInfo = null;

            EndFight();
        }
    }

    public void GenerateChallenger(GSNotifyChallegerInfo info)
    {
        CharacterOtherProperty enemyProp = new CharacterOtherProperty();
        enemyProp.fightProperty.ResetProperty();
        
        enemyProp.setNickName(info.roleName);
        enemyProp.setCareer((CHARACTER_CAREER)info.vocationID);
        BundleMemManager.Instance.addRoleEffectByCareer(enemyProp.career); //根据职业加载挑战的特效
        enemyProp.setAttackPower((int)info.m_fightProperty.GetValue(eFighintPropertyCate.eFPC_Attack));
        enemyProp.setDefence((int)info.m_fightProperty.GetValue(eFighintPropertyCate.eFPC_Defense));
        enemyProp.setArmor((int)info.suitID);

        int weaponID = (int)info.weaponID;
        enemyProp.setWeapon(weaponID);

        enemyProp.setLevel((int)info.level);
        enemyProp.setHP((int)info.curHp);
        enemyProp.mp = ((int)info.curMp);
        enemyProp.setHPMax((int)info.m_fightProperty.fightData[eFighintPropertyCate.eFPC_MaxHP]);
        //enemyProp.setMPMax((int)info.m_fightProperty.fightData[eFighintPropertyCate.eFPC_MaxMP]);
        enemyProp.fightProperty = info.m_fightProperty;


        CharacterPlayerOther cpo = PlayerManager.Instance.createPlayerOther(enemyProp, Vector3.one, GameObject.Find(Constant.Enemy_Born_Point).transform.position);

        cpo.GetComponent<CharacterAnimCallback>().SetCharType(CHAR_ANIM_CALLBACK.CAC_OTHERS);
        cpo.gameObject.name = "challenger";
        cpo.character_avatar.installWing(info.wingId);

        cpo.applyProperty();

        Collider collider = cpo.GetComponent<Collider>();
        collider.isTrigger = true;


        List<AIDataItem> ailist = ConfigDataManager.GetInstance().getAIConfig().GetAIList(true, enemyProp.getCareer());

        List<int> skillList = new List<int>();
        skillList.Add((int)info.skill1Id);
        skillList.Add((int)info.skill2Id);
        skillList.Add((int)info.skill3Id);
        skillList.Add((int)info.skill4Id);

        for (int i = 0; i < ailist.Count; ++i)
        {
            // 自动战斗初始化 将玩家的技能配置 装进AI.SkillBehavious中
            foreach (int item in skillList)
            {
                int nPreSkillID = item / 1000;
                if (nPreSkillID == ailist[i].AIValue)
                {
                    CharacterAI.AISkillData aiData = new CharacterAI.AISkillData();
                    aiData.nAIID = ailist[i].id;
                    aiData.nSkillID = item;
                    cpo.GetAI().m_kAISkillBehavious.Add(aiData);
                }
            }
        }

        // 默认让对手朝向对手
        Quaternion rotR = Quaternion.Euler(0, 90, 0);
        cpo.setFaceDir(rotR * cpo.transform.forward);

        
        m_kChallenger = cpo;

        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_ARENA_PLAYER);
        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_ARENA_PLAYER_HEALTH);
        // 该角色具有AI功能
        m_kChallenger.GetAI().m_bAIAvailable = true;
    }

    void SendReport()
    {

    }

    public Character GetEnemy(Character character, float radius)
    {
		if (character.getType () == CharacterType.CT_PLAYER) 
		{
			if (m_kChallenger == null)
				return null;

			float dist = Vector3.Distance (character.getPosition (), m_kChallenger.getPosition ());

			if (dist < radius)
			{
				return m_kChallenger;
			}
		} 
		else if (character.getType () == CharacterType.CT_PLAYEROTHER) 
		{
			float dist = Vector3.Distance (character.getPosition (), CharacterPlayer.sPlayerMe.getPosition ());

			if (dist < radius)
			{
				return CharacterPlayer.sPlayerMe;
			}
		}

        return null;
    }
}