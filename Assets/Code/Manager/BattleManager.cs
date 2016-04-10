using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using NetGame;

public enum DAMAGE_TYPE
{
    DT_COMMON_DAMAGE = 0,  // 一般的伤害
    DT_WITH_STAND,
    DT_BLAST_DAMAGE,        // 暴击带来的伤害
    DT_BUFF_DAMAGE,     // buff 带来的伤害
}

public enum DAMAGE_TIMES
{
    DT_ONCE = 1,
    DT_MULTI,
}

public enum COMBAT_SKILL_RESULT
{
    CSR_HIT = 0,
    CSR_MISS,
    CSR_PARRY,
    CSR_DODGE,
}


public class BattleManager 
{
    private SkillTargetFlag _skill_target_flag;

    public bool m_bTimeOver = false;

	AudioClip clip_broken;

	float m_fMinDamageCoeffic = 0.2f;       // 最低攻击系数
	float m_fBaseDamageLow = 0.9f;          // 伤害浮动区间
	float m_fBaseDamageUp = 1.1f;           // 伤害浮动区间
	float m_fMinDamageNum = 1.0f;           // 攻击最低伤害值

    float m_fMinzhongBase = 0.95f;
    float m_fMinzhongAnti = 0.45f;
    float m_fMinzhongInc = 0.05f;

    float m_fBlastBase = 0.1f;
    float m_fBlastAnti = 0.1f;
    float m_fBlastInc = 0.4f;

    float m_fBlastDamageBase = 2.0f;
    float m_fBlastDamageAnti = 1.0f;
    float m_fBlastDamageInc = 1.0f;

    float m_fZhaojiaBase = 0.0f;
    float m_fZhaojiaAnti = 0.0f;
    float m_fZhaojiaInc = 0.5f;

    float m_fZhaojiaDamageAnti = 0.5f;
    float m_fZhaojiaDamageRef = 0.5f;

	
	private static BattleManager _instance = null;

	public static BattleManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = new BattleManager();
            return _instance;
        }
    }

    private BattleManager()
    {
		m_fMinDamageCoeffic = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData (9001001).type7Data;
		m_fBaseDamageLow = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData (9001002).type8List[0];
		m_fBaseDamageUp = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData (9001002).type8List[1];
		m_fMinDamageNum = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData (9001003).type7Data;

        m_fMinzhongBase = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001004).type8List[0];
        m_fMinzhongAnti = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001004).type8List[1];
        m_fMinzhongInc = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001004).type8List[2];

        m_fBlastBase = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001005).type8List[0];
        m_fBlastAnti = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001005).type8List[1];
        m_fBlastInc = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001005).type8List[2];

        m_fBlastDamageBase = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001006).type8List[0];
        m_fBlastDamageAnti = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001006).type8List[1];
        m_fBlastDamageInc = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001006).type8List[2];

        m_fZhaojiaBase = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001007).type8List[0];
        m_fZhaojiaAnti = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001007).type8List[1];
        m_fZhaojiaInc = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001007).type8List[2];

        m_fZhaojiaDamageAnti = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001008).type7Data;
        m_fZhaojiaDamageAnti = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001009).type7Data;
    }


    public void OnWeaponHitCharacter(Character attacker, Character target, Vector3 hitpoint)
    {
        SkillCastProcess(attacker.skill.getCurrentSkill(), target, hitpoint);
    }
	
	public void addHPBarOnMonster(CharacterMonster monster) 
    {
		
		GameObject phBar;
		
		if (!monster.hp_bar) {
			//显示血条
		    GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.MOSTER_HP_BAR, EBundleType.eBundleUI);
            phBar = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
			phBar.name = "hp_bar";
			phBar.AddComponent("BillBoard");
			phBar.AddComponent("HPBar");
			phBar.GetComponent<HPBar>().tag_point = monster.getTagPoint("help_hp");
			phBar.transform.localPosition = Vector3.zero;
			phBar.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
			monster.hp_bar = phBar;
		}
		else {
			phBar = monster.hp_bar;
		}
		
		//设置血量
		int cur_hp = monster.monster_property.getHP();
		int max_hp = monster.monster_property.getHPMax();
		phBar.GetComponent<UISlider>().sliderValue = (float)cur_hp / (float)max_hp;
		
		Object.Destroy(phBar, 2.0f);
	}
	
	public void setBossMonsterHPBar(CharacterMonster monster) {
		int n32CurHP = monster.monster_property.getHP();
		int n32MaxHP = monster.monster_property.getHPMax();
		//UiFightMainMgr.Instance.SetUiBossHPBar(n32CurHP, n32MaxHP);
        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_BOSS_HEALTBAR);
	}



    public void onSkillAreaAbsorbMonster(SkillAppear skill, CharacterMonster monster, float speed, float t)
    {

        //临时判断用
        if (!CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap()) return;

        if (skill == null) return;
        int cur_hp = monster.monster_property.getHP();
        if (cur_hp <= 0) return;

        if (skill.getOwner().getType() == CharacterType.CT_PLAYER)
        {

            float dist = Vector3.Distance(CharacterPlayer.sPlayerMe.transform.position,
                monster.transform.position);
            if (dist < CharacterPlayer.character_property.getExtrusionRange())
                return;

            dist = dist - CharacterPlayer.character_property.getExtrusionRange();

            float moveDist;
            if (speed * t > dist)
            {
                moveDist = dist;
            }
            else
            {
                moveDist = speed * t;
            }

            Vector3 dir = CharacterPlayer.sPlayerMe.transform.position - monster.transform.position;
            dir.Normalize();

            monster.movePosition(dir * moveDist);
        }
    }
	
	
	public void onPlayerkillMonster(CharacterMonster theMonster) 
    {
        if (Global.inGoldenGoblin())
        {
            int nGainGold = 0;

            if (theMonster.monster_property.getTemplateId() == 400101)
            {
                // little goblin
                // 哥布林死了一个需要补充
                MonsterArea.m_bOneGoblinDie = true;

                nGainGold = BattleGoblin.GetInstance().m_nOneMoney;
                BattleGoblin.GetInstance().m_nKilledNum++;
            }
            else
            {
                MonsterArea.m_bOneGaintGoblinDie = true;

                nGainGold = BattleGoblin.GetInstance().m_nOneMoney * BattleGoblin.GOBLIN_MONEY_MULTIPLE;

                BattleGoblin.GetInstance().m_nKilledGaintNum++;
            }


            if (CharacterPlayer.sPlayerMe.HasBuff(BUFF_TYPE.BT_GREED))
            {
                BattleGoblin.GetInstance().m_nKilledGoblinWithGreed += (nGainGold * 2);
                BattleGoblin.GetInstance().ShowHeadGainGold(theMonster, nGainGold * 2);
            }
            else
            {
                BattleGoblin.GetInstance().m_nKilledGoblin += (nGainGold);
                BattleGoblin.GetInstance().ShowHeadGainGold(theMonster, nGainGold);
            }
        }
        else
        {
            //close boss hp bar.
            if (theMonster.monster_property.getType() == MonsterProperty.MONSTER_LEVEL_TYPE.MLT_BOSS)
            {
                //UiFightMainMgr.Instance.HideUiBossHPBar();
                Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_BOSS_HIDEEN);
            }
            //给人增加钱和经验
            int curExp = CharacterPlayer.character_property.getExperience();
            int curGold = CharacterPlayer.character_asset.gold;
            CharacterPlayer.character_property.setExperience(curExp + theMonster.monster_property.getExp());
            CharacterPlayer.character_asset.SetGold(curGold + theMonster.monster_property.getGold());
            MessageManager.Instance.exp_acc += theMonster.monster_property.getExp();
            MessageManager.Instance.gold_acc += theMonster.monster_property.getGold();
            int curLevel = CharacterPlayer.character_property.getLevel();
			#region 2014-4-12 修改
			RoleDataItem rdi = ConfigDataManager.GetInstance()
								.getRoleConfig().getRoleData((int)CharacterPlayer.character_property.career*Constant.LEVEL_RATIO + curLevel);
			#endregion
            #region 原始写法
//			RoleDataItem rdi = ConfigDataManager.GetInstance()
//								.getRoleConfig().getRoleData(10000 + curLevel);
            #endregion
			//Debug.Log(CharacterPlayer.character_property.getExperience());
			
            if (rdi != null)
            {
                int upGradeExp = rdi.upgrade_exp;
                if (MessageManager.Instance.send_exp_dirty &&
                    CharacterPlayer.character_property.getExperience() > upGradeExp)
                {
                    MessageManager.Instance.sendExpChange();
                    MessageManager.Instance.send_exp_dirty = false;
                }
            }

            if (Global.current_fight_level == Global.eFightLevel.Fight_Level2)
            {
            }
        }
	}


    public void OnPlayerBeHit()
    {
        if (Global.inGoldenGoblin())
        {
            int curGold = CharacterPlayer.character_asset.gold;

            if (CharacterPlayer.sPlayerMe.HasBuff(BUFF_TYPE.BT_MEAN))
            {
                EffectManager.Instance.createFX("Effect/Effect_Prefab/Monster/Act/DZgbl_JinBi", CharacterPlayer.sPlayerMe.getTagPoint("armor"));

                int nLostGold = 0;

                nLostGold = Random.Range(0, 11) + 5;

                BattleGoblin.GetInstance().m_nGoblinHitMeMoney += nLostGold;

                BattleGoblin.GetInstance().ShowHeadLostGold(nLostGold);
            }
        }
    }

    public void OnMonsterBeHit(Character monster)
    {
        if (Global.inGoldenGoblin())
        {
            EffectManager.Instance.createFX("Effect/Effect_Prefab/Monster/Act/DZgbl_JinBi", monster.getTagPoint("body"));

            int nGainGold = 0;

            if (CharacterPlayer.sPlayerMe.HasBuff(BUFF_TYPE.BT_GREED))
            {
                nGainGold = (Random.Range(0, 6) + 5) * 2;
            }
            else
            {
                nGainGold = Random.Range(0, 6) + 5;
            }

            if ((monster as CharacterMonster).monster_property.getTemplateId() != 400101)
            {
                nGainGold *= BattleGoblin.GOBLIN_MONEY_MULTIPLE;
            }

            BattleGoblin.GetInstance().m_nHitGoblinMoney += nGainGold;
            BattleGoblin.GetInstance().ShowHeadGainGold(monster, nGainGold);
        }
        else
        {
            EffectManager.Instance.createFX("Effect/Effect_Prefab/Monster/shouji", monster.getTagPoint("help_body"));
//            EffectManager.sEffectManager.createFX("Effect/Effect_Prefab/Monster/shitibaozha", monster.getTagPoint("help_body"));
        }
    }
	
	
    //public void showMoveTarget(Vector3 pos) {
    //    move_target_flag.position = pos;
    //}
	
    //public void hideMoveTarget() {
    //    move_target_flag.position += new Vector3(0,-1000.0f,0);
    //}

    public void SkillCastProcess(SkillAppear skill, Character target, Vector3 colliderPos, bool isJiguan = false)
    {
        if (!SkillPreProcess(skill, target))
        {
            return;
        }
        
        // 这里表示打中了 需要判断是否触发技能的机关
        if (!isJiguan)
        {
            if (skill.getOwner().getType() == CharacterType.CT_PLAYER)
            {
                BattleJiGuan.Instance.OnJiGuanTrigger(JiGuanItem.EJiGuanType.JGT_SKILL_HIT, skill.m_kSkillInfo.nJiGuanID, colliderPos, skill);
            }
            
        }


        if (Global.InWorldBossMap())
        {
            // 发送打中世界boss消息
			if (skill.getOwner().getType() == CharacterType.CT_PLAYER && target.getType() == CharacterType.CT_MONSTER)
			{
				// 汇报打中世界boss
				ReportHitWorldBoss(skill, target);
				Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_COMBO);
			}
				
				
            return;
        }
        
        // 黄金哥布林玩法 走的流程不一样
        if (Global.inGoldenGoblin())
        {
            GoldenGoblinProcess(skill, target);
            return;
        }

        SkillCastResult(skill, target);
    }


    public void SkillCastResult(SkillAppear skill, Character target)
    {
        if (!IsHit(skill.getOwner(), target))
        {
            MissProcess(skill, target);
        }
        else
        {
            COMBAT_SKILL_RESULT eType = COMBAT_SKILL_RESULT.CSR_HIT;

            // 暴击伤害 start
            float fAttackDamage = 0.0f; // 攻击者受到的伤害 (由招架反射带来)
            float fFinalDamage = 0.0f;  // 受击者最终伤害

            float baseHurt = GetBaseAttack(skill, target);

            if (IsBlastAttack(skill.getOwner(), target))
            {
                eType = COMBAT_SKILL_RESULT.CSR_PARRY;
                fFinalDamage = baseHurt * GetBlastTime(skill.getOwner(), target);
            }
            else
            {
                eType = COMBAT_SKILL_RESULT.CSR_HIT;
                fFinalDamage = baseHurt;
            }

            // 技能不让招架
            if (!CharacterAI.IsInState(skill.getOwner(), CharacterAI.CHARACTER_STATE.CS_SKILL) &&
                IsWithStand(skill.getOwner(), target))
            {
                eType = COMBAT_SKILL_RESULT.CSR_DODGE;
                fAttackDamage = fFinalDamage * m_fZhaojiaDamageRef;
                fFinalDamage *= m_fZhaojiaDamageAnti;
            }


            switch (eType)
            {
                case COMBAT_SKILL_RESULT.CSR_HIT:
                    {
                        // 普通伤害
                        FloatBloodNum.Instance.PlayFloatBood(skill.getOwner().getType() == CharacterType.CT_PLAYER,
                            (int)fFinalDamage,
                            target.getTagPoint("help_hp"));

                        target.GetProperty().m_eDamageReason = DAMAGE_TYPE.DT_COMMON_DAMAGE;

                        HitProcess(skill, target, eType, fFinalDamage);
                    }
                    break;
                case COMBAT_SKILL_RESULT.CSR_PARRY:
                    {
                        // 暴击飘血
                        FloatBloodNum.Instance.PlayFloatBood(skill.getOwner().getType() == CharacterType.CT_PLAYER,
                            (int)fFinalDamage,
                            target.getTagPoint("help_hp"),
                            eHurtType.doubleHurt);

                        target.GetProperty().m_eDamageReason = DAMAGE_TYPE.DT_BLAST_DAMAGE;

                        HitProcess(skill, target, eType, fFinalDamage);
                    }
                    break;
                case COMBAT_SKILL_RESULT.CSR_DODGE:
                    {
                        // 招架伤害
                        FloatBloodNum.Instance.PlayFloatBood(skill.getOwner().getType() == CharacterType.CT_PLAYER,
                            (int)fFinalDamage,
                            target.getTagPoint("help_hp"),
                            eHurtType.withstandHurt);

                        FloatBloodNum.Instance.PlayFloatBood(target.getType() == CharacterType.CT_PLAYER,
                            (int)fFinalDamage,
                            target.getTagPoint("help_hp"),
                            eHurtType.withstandHurt);

                        target.GetProperty().m_eDamageReason = DAMAGE_TYPE.DT_COMMON_DAMAGE;
                        skill.getOwner().GetProperty().m_eDamageReason = DAMAGE_TYPE.DT_WITH_STAND;

                        HitProcess(skill, target, eType, fFinalDamage, fAttackDamage);
                    }
                    break;
            }
        }
    }


    public bool SkillPreProcess(SkillAppear skill, Character target)
    {
        //////////////////////////////////////////////////////////////////////////
        // 翻滚不能受伤害
        if (target.getType() == CharacterType.CT_PLAYER &&
            target.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_SKILL &&
            target.skill.getCurrentSkill() != null)
        {
            if (target.skill.getCurrentSkill().GetSkillId() == 400001 || target.skill.getCurrentSkill().GetSkillId() == 400003)
            {
                return false;
            }
        }

        // 伤害保护
        if (target.skill.getHurtProtecting())
            return false;

        // 目标死亡
        if (CharacterAI.IsInState(target, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            return false;
        }
        

        // 伤害判断 start
        if (target.m_bHasBeenDamaged)
            return false;
        // 这里有点狗，需要将来优化
        //if (target.getType() == CharacterType.CT_MONSTER)
        {
            target.m_bHasBeenDamaged = true;
        }
        // 伤害判断 end
        //////////////////////////////////////////////////////////////////////////


        //////////////////////////////////////////////////////////////////////////
        // target 是否是在施放技能 start
        if (target.m_eAppearState == Appear.BATTLE_STATE.BS_SKILL)
        {
            // 看技能的属性是否可以被打断
            if (target.skill)
            {
                if (target.skill.getCurrentSkill() != null)
                {
                    if (target.skill.getCurrentSkill().m_kSkillInfo.unmatched == 1)
                    {
                        // 目标无敌 但是buff还是要加上
                        BuffProcess(skill, target);
                        return false;
                    }
                }
            }
        }
        // target 是否是在施放技能 end
        //////////////////////////////////////////////////////////////////////////

        return true;
    }

    public void MissProcess(SkillAppear skill, Character target)
    {
//        FloatMessage.GetInstance().PlayFloatMessage3D(
//                Global.FormatStrimg(LanguageManager.GetText("fight_float_miss")),
//                1.0f,
//                target.getTagPoint("help_hp"),
//                new Vector3(0, 0.8f, 0));		

        // target闪避
        if (skill.getOwner().getType() == CharacterType.CT_PLAYER)
        {    
//            FloatMessage.GetInstance().PlayFloatMessage3D(
//                Global.FormatStrimg(LanguageManager.GetText("fight_float_miss_MTP")),
//                1.0f,
//                target.getTagPoint("help_hp"),
//                new Vector3(0, 0.8f, 0));
			FloatBloodNum.Instance.PlayFloatBood(false, 0, target.getTagPoint("help_hp"), eHurtType.escapeHurt);
            // 如果是怪miss了 玩家的攻击 也要击退的
            HitBackFlyBrokenProcess(skill, target);
        }
        else
        {
        	FloatBloodNum.Instance.PlayFloatBood(true, 0, target.getTagPoint("help_hp"), eHurtType.escapeHurt);
        }

        BuffProcess(skill, target);
    }

    // 默认skillDamage为0 表示不是招架
    public void HitProcess(SkillAppear skill, Character target, COMBAT_SKILL_RESULT eDamageType, float targetDamage, float skillDamage = 0.0f)
    {
        // 主机上算出来的伤害直接广播
        if (Global.inMultiFightMap())
        {
            if (CharacterPlayer.character_property.getHostComputer())
            {
                // 这种情况只有 主角打怪 和怪打所有人
                target.GetProperty().setHP(target.GetProperty().getHP() - (int)targetDamage);

                // 发消息告诉其他客户端 被打的单位是否死亡 是否受击  是否被招架
                MessageManager.Instance.sendObjectHurt(true,
                    (uint)target.GetProperty().GetInstanceID(),
                    (int)targetDamage,
                    target.GetProperty().getHP(),
                    (int)target.GetProperty().m_eDamageReason);

                if (eDamageType == COMBAT_SKILL_RESULT.CSR_DODGE)
                {
                    skill.getOwner().GetProperty().setHP(skill.getOwner().GetProperty().getHP() - (int)skillDamage);

                    MessageManager.Instance.sendObjectHurt(true,
                    (uint)skill.getOwner().GetProperty().GetInstanceID(),
                    (int)skillDamage,
                    skill.getOwner().GetProperty().getHP(),
                    (int)DAMAGE_TYPE.DT_WITH_STAND);
                }
            }
            else
            {
                if (skill.getOwner().getType() == CharacterType.CT_PLAYER && target.getType() == CharacterType.CT_MONSTER && target.m_eAppearState != Appear.BATTLE_STATE.BS_DIE)
                {
                    // 非主机 把主角对怪造成的伤害 先发给主机，让主机来决定怪的表现
                    MessageManager.Instance.sendObjectHurt(false,
                        (uint)target.GetProperty().GetInstanceID(),
                        (int)targetDamage,
                        target.GetProperty().getHP(),
                        (int)target.GetProperty().m_eDamageReason);

                    if (eDamageType == COMBAT_SKILL_RESULT.CSR_DODGE)
                    {
                        MessageManager.Instance.sendObjectHurt(false,
                        (uint)skill.getOwner().GetProperty().GetInstanceID(),
                        (int)skillDamage,
                        skill.getOwner().GetProperty().getHP(),
                        (int)DAMAGE_TYPE.DT_WITH_STAND);
                    }

                    return;
                }
            }
        }
        else
        {
            target.GetProperty().setHP(target.GetProperty().getHP() - (int)targetDamage);

            if (eDamageType == COMBAT_SKILL_RESULT.CSR_DODGE)
            {
                skill.getOwner().GetProperty().setHP(skill.getOwner().GetProperty().getHP() - (int)skillDamage);
            }
        }


        if (!Global.InArena() && !Global.inMultiFightMap() && skill.getOwner().getType() == CharacterType.CT_PLAYER)
        {
            GCReportHurt netTarget = new GCReportHurt((uint)skill.getOwner().GetInstanceID(),
            (uint)target.GetProperty().GetInstanceID(),
            (uint)skill.GetSkillId(),
            (int)targetDamage,
            target.GetProperty().getHP());


            GCReportHurt netSkill = new GCReportHurt((uint)target.GetProperty().GetInstanceID(),
            (uint)skill.getOwner().GetProperty().GetInstanceID(),
            (uint)skill.GetSkillId(),
            (int)skillDamage,
            skill.getOwner().GetProperty().getHP());

            MainLogic.SendMesg(netTarget.ToBytes());
            MainLogic.SendMesg(netSkill.ToBytes());
        }
        

        HitBackFlyBrokenProcess(skill, target);

        // 战斗buff 产生的入口
        BuffProcess(skill, target);
    }

    public void HitBackFlyBrokenProcess(SkillAppear skill, Character target)
    {
        target.BeHitBackHitFlyHitBroken(skill);
    }

    public void BuffProcess(SkillAppear skill, Character target)
    {
        // 没死的话 最后走buff流程 
        if (target.GetProperty().getHP() > 0)
        {
            if (skill.m_kSkillInfo.skillEffect != 0)
            {
                // 技能产生的buff
                target.AddBuff(skill.m_kSkillInfo.skillEffect);
            }

            // 判断是否产生冰buff
            if (skill.getOwner().GetProperty().fightProperty.GetBaseValue(eFighintPropertyCate.eFPC_IceAttack) > 0 &&
                Random.Range(0.0f, 1.0f) < FrozenChances(skill.getOwner(), target))
            {
                
                target.AddBuff(5);
            }

            // 判断是否产生火buff
            if (skill.getOwner().GetProperty().fightProperty.GetBaseValue(eFighintPropertyCate.eFPC_FireAttack) > 0 &&
                Random.Range(0.0f, 1.0f) < FireChances(skill.getOwner(), target))
            {
                
                target.AddBuff(10);
            }

            // 判断是否产生毒buff
            if (skill.getOwner().GetProperty().fightProperty.GetBaseValue(eFighintPropertyCate.eFPC_PoisonAttack) > 0 &&
                Random.Range(0.0f, 1.0f) < PoisionChances(skill.getOwner(), target))
            {
                
                target.AddBuff(11, PoisionDamage(skill.getOwner(), target));
            }

            // 判断是否产生雷buff
            if (skill.getOwner().GetProperty().fightProperty.GetBaseValue(eFighintPropertyCate.eFPC_ThunderAttack) > 0 &&
                Random.Range(0.0f, 1.0f) < ThunderChances(skill.getOwner(), target))
            {
                
                target.AddBuff(12);
            }  
        }
    }

    public void GoldenGoblinProcess(SkillAppear skill, Character target)
    {
        target.GetComponent<RenderProperty>().GenerateEffect(0.3f, new Color(43, 0, 0, 0));
        //EffectManager.Instance.BeHitHightlight(target.gameObject);

        if (skill.getOwner().getType() == CharacterType.CT_PLAYER)
        {
            if (CharacterPlayer.sPlayerMe.HasBuff(BUFF_TYPE.BT_RAMPAGE))
            {
                target.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_SKILL, (int)SKILL_APPEAR.SA_FLASH_AWAY);
                return;
            }

            // 带了一击必杀buff就直接弄死哥布林
            if (skill.getOwner().HasBuff(BUFF_TYPE.BT_ONE_KILL))
            {
                target.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_DIE);
                return;
            }
            else
            {
                // 20%概率死亡
                if (Random.Range(0, 100) < 20)
                {
                    target.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_DIE);
                    return;
                }
            }

            // 玩家每砍哥布林 要加钱
            OnMonsterBeHit(target);
        }
        else
        {
            OnPlayerBeHit();
        }
    }



    //是否命中
    bool IsHit(Character attacker, Character target)
    {
        float minzhong = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_Precise) / Constant.DAMAGE_BASE_UNIT;
        float shanbi = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_Dodge) / Constant.DAMAGE_BASE_UNIT;
        

        float hitRate = m_fMinzhongBase + Mathf.Min(Mathf.Max((minzhong - shanbi), -m_fMinzhongAnti), m_fMinzhongInc);
        float runtimeRate = Random.Range(0.0f, 1.0f);
       
        if (runtimeRate < hitRate)
        {
            return true;
        }
        
        return false;
    }


    bool IsWithStand(Character attacker, Character target)
    {
        float poji = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_FightBreak) / Constant.DAMAGE_BASE_UNIT;
        float zhaojia = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiFightBreak) / Constant.DAMAGE_BASE_UNIT;


        float wthStandRate = m_fZhaojiaBase + Mathf.Min(Mathf.Max((zhaojia - poji), -m_fZhaojiaAnti), m_fZhaojiaInc);
        
        float runtimeRate = Random.Range(0.0f, 1.0f);

        if (runtimeRate < wthStandRate)
        {
            return true;
        }

        return false;
    }


    bool IsBlastAttack(Character attacker, Character target)
    {
        float baoji = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_BlastAttack) / Constant.DAMAGE_BASE_UNIT;
        float renxing = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_Tenacity) / Constant.DAMAGE_BASE_UNIT;
        
        
        float blastAttackRate = m_fBlastBase + Mathf.Min(Mathf.Max((baoji - renxing), -m_fBlastAnti), m_fBlastInc);
        float runtimeRate = Random.Range(0.0f, 1.0f);

        if (runtimeRate < blastAttackRate)
        {
            return true;
        }
        
        return false;
    }


    float GetBlastTime(Character attacker, Character target)
    {
        float baojijiacheng = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_BlastAttackAdd) / Constant.DAMAGE_BASE_UNIT;
        float baojijianmian = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_BlastAttackReduce) / Constant.DAMAGE_BASE_UNIT;

        float ret = m_fBlastDamageBase + Mathf.Min(Mathf.Max((baojijiacheng - baojijianmian), -m_fBlastDamageAnti), m_fBlastDamageInc);
        
        return ret;
    }


    float GetBaseAttack(SkillAppear skill, Character target)
    {
     	float gongji = skill.getOwner().GetProperty().getAttackPower()+skill.getOwner().GetProperty().fightProperty.GetBuffValue(eFighintPropertyCate.eFPC_Attack);  
	   	float fangyu = target.GetProperty().getDefence() +  target.GetProperty().fightProperty.GetBuffValue(eFighintPropertyCate.eFPC_Defense) ;
//        int dengji = skill.getOwner().GetProperty().getLevel();
//        float randomAttack = Random.Range(1.0f, 1.0f + (float)dengji / 5.0f);
//
//
//        float baseHurt = gongji * skill.GetAttackCoefficient() - fangyu;
//        baseHurt = Mathf.Max(baseHurt, 0.0f);
//        float ret = Mathf.Max((baseHurt + skill.GetDamagePlus()), randomAttack);


		float fAttackBuffPercent = 0.0f;
		float fDefenceBuffPercent = 0.0f;

		float fAttackBuffNum = 0.0f;
		float fDefenceBuffNum = 0.0f;

		float random = Random.Range (m_fBaseDamageLow, m_fBaseDamageUp);

		float ran1 = Mathf.Max((gongji - fangyu), gongji * m_fMinDamageCoeffic);
		float ran2 = ran1 * skill.GetAttackCoefficient () + skill.GetDamagePlus ();
		float ran3 = ran2 * random;
		float ran4 = ran3 * (1 + fAttackBuffPercent - fDefenceBuffPercent);
		float ran5 = ran4 + (fAttackBuffNum - fDefenceBuffNum);

		float ret = Mathf.Max(ran5, m_fMinDamageNum);
		
        return ret;
    }


    // 属性触发概率计算公式
    // 冰概率
    float FrozenChances(Character attacker, Character target)
    {
        float frozenAttack = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_IceAttack);
        float frozenDefence = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiIceAttack);

        int targetLevel = target.GetProperty().getLevel();

        return Mathf.Min(
            Mathf.Max(0.05f, (frozenAttack - frozenDefence) / (50.0f + targetLevel * 80.0f)),
            0.5f);
    }

    // 火概率
    float FireChances(Character attacker, Character target)
    {
        float fireAttack = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_FireAttack);
        float fireDefence = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiFireAttack);

        int targetLevel = target.GetProperty().getLevel();

        return Mathf.Min(
            Mathf.Max(0.05f, (fireAttack - fireDefence) / (50.0f + targetLevel * 80.0f)),
            0.5f);
    }

    // 毒概率
    float PoisionChances(Character attacker, Character target)
    {
        float posionAttack = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_PoisonAttack);
        float posionDefence = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiPoisonAttack);

        int targetLevel = target.GetProperty().getLevel();

        return Mathf.Min(
            Mathf.Max(0.05f, (posionAttack - posionDefence) / (50.0f + targetLevel * 80.0f)),
            0.5f);
    }

    // 雷概率
    float ThunderChances(Character attacker, Character target)
    {
        float thunderAttack = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_ThunderAttack);
        float thunderDefence = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiThunderAttack);

        int targetLevel = target.GetProperty().getLevel();

        return Mathf.Min(
            Mathf.Max(0.05f, (thunderAttack - thunderDefence) / (50.0f + targetLevel * 80.0f)),
            0.5f);
    }


    // 属性伤害公式
    // 冰伤害
    float FrozenDamage(Character attacker, Character target)
    {
        float frozenAttack = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_IceAttack);
        float frozenDefence = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiIceAttack);

        int targetLevel = target.GetProperty().getLevel();

        return Mathf.Min(
            Mathf.Max(0.1f, (frozenAttack - frozenDefence) / (50.0f + targetLevel * 30.0f)),
            0.6f);
    }

    // 火伤害
    float FireDamage(Character attacker, Character target)
    {
        if (!target.HasBuff(BUFF_TYPE.BT_FIRE_HURT))
        {
            return 0.0f;
        }

        float fireAttack = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_FireAttack);
        float fireDefence = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiFireAttack);

        int targetLevel = target.GetProperty().getLevel();

        return Mathf.Min(
            Mathf.Max(0.0f, (fireAttack - fireDefence) / (200.0f + targetLevel * 50.0f)),
            1.0f);
    }

    // 毒伤害
    float PoisionDamage(Character attacker, Character target)
    {
        float posionAttack = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_PoisonAttack);
        float posionDefence = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiPoisonAttack);

        int targetLevel = target.GetProperty().getLevel();

        int attackerLevel = attacker.GetProperty().getLevel();

        return Mathf.Min(
            Mathf.Max( 30.0f, 
            (posionAttack - posionDefence) / Mathf.Max((targetLevel - attackerLevel), 
            0.5f)),
            500.0f);
    }

    // 雷伤害
    float ThunderDamage(Character attacker, Character target)
    {
        if (!target.HasBuff(BUFF_TYPE.BT_THUNDER_HURT))
        {
            return 0.0f;
        }

        float thunderAttack = attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_ThunderAttack);
        float thunderDefence = target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiThunderAttack);

        int targetLevel = target.GetProperty().getLevel();

        int attackerLevel = attacker.GetProperty().getLevel();

        return Mathf.Min(
            Mathf.Max(50.0f,
            (thunderAttack - thunderDefence) / Mathf.Max((targetLevel - attackerLevel),
            0.5f)),
            1000.0f);
    }


    // 元素伤害 start
    float ElementDamage(Character attacker, Character target)
    {
        float fFireElementDamage = Mathf.Max(attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_FireAttack) -
            target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiFireAttack), 0);

        float fIceElementDamage = Mathf.Max(attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_IceAttack) -
            target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiIceAttack), 0);

        float fThunderElementDamage = Mathf.Max(attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_ThunderAttack) -
            target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiThunderAttack), 0);

        float fPoisionElementDamage = Mathf.Max(attacker.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_PoisonAttack) -
            target.GetProperty().fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiPoisonAttack), 0);

        return fFireElementDamage + fIceElementDamage + fThunderElementDamage + fPoisionElementDamage;
    }
    // 元素伤害 end

    public void PlayAudioOneShot()
    {
        if (CharacterPlayer.sPlayerMe.audio)
        {
            if (clip_broken == null)
            {
                if (BundleMemManager.debugVersion)
                {
                    clip_broken = BundleMemManager.Instance.loadResource(PathConst.AUDIO_BREAK) as AudioClip;
                    CharacterPlayer.sPlayerMe.audio.PlayOneShot(clip_broken);
                }
                else
                {
                    BundleMemManager.Instance.loadPrefabViaWWW<AudioClip>(EBundleType.eBundleMusic, PathConst.AUDIO_BREAK,
                    (obj) =>
                    {
                        clip_broken = obj as AudioClip;
                        if (clip_broken != null)
                            CharacterPlayer.sPlayerMe.audio.PlayOneShot(clip_broken);
                    }); 
                }              
            }
        }
    }

    public void RangeSkillDamageCal(SkillAppear skill)
    {
		CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_FIGHT_SHAKE, 0.025f);

        List<CharacterMonster> hurtChar = SearchEnemy(skill.skill_id);

        if (hurtChar != null)
        {
            for (int i = 0; i < hurtChar.Count; ++i )
            {
                SkillCastProcess(skill, hurtChar[i], CollisionUtil.CalculateJiGuanHitPoint(hurtChar[i], null));
            }
        }
    }
    // 技能影响的对象
    public List<CharacterMonster> SearchEnemy(int nSkillId)
    {
        // 在技能update过程中不断更新list
        int nRangeAngle = ConfigDataManager.GetInstance().getSkillExpressionConfig().getSkillExpressionData(SkillAppear.ConvertLevelIDToSingleID(nSkillId)).skillAngle;
        float fRadius = ConfigDataManager.GetInstance().getSkillExpressionConfig().getSkillExpressionData(SkillAppear.ConvertLevelIDToSingleID(nSkillId)).skillRadius;
        if (nRangeAngle > 0)
        {
            return GetRangeCharacter(nRangeAngle, fRadius);
        }
        else
            return null;
    }


    // 击中了 计算伤害
    //public void ProcessSkillDamage(CombatProperty attacker, CombatProperty target, int nSkill)
    //{

    //}

    // 得到范围内的角色 目前先得怪物，将来得人物
    public List<CharacterMonster> GetRangeCharacter(int nDegree, float radius)
    {
        return MonsterManager.Instance.GetAngleRangleMonster(nDegree, radius);
    }

    // 得到技能范围的敌人
    public Character GetViewRangeEnemy(Character character)
    {
        float fRange = 0.0f;

        switch (character.GetProperty().getCareer())
        {
        case CHARACTER_CAREER.CC_SWORD:
            fRange = Constant.FIGHT_SWORD_VIEW_RANGE;
        	break;
        case CHARACTER_CAREER.CC_ARCHER:
            fRange = Constant.FIGHT_ARCHER_VIEW_RANGE;
            break;
        case CHARACTER_CAREER.CC_MAGICIAN:
            fRange = Constant.FIGHT_MAGIC_VIEW_RANGE;
            break;
        }
        
        if (Global.inFightMap() || Global.inGoldenGoblin() || 
			Global.inTowerMap() || Global.inMultiFightMap() ||
			Global.InWorldBossMap()||Global.InAwardMap())
        {
            return MonsterManager.Instance.GetPointNearestMonster(character.getPosition(), fRange);
        }
        else if (Global.InArena())
        {
            return BattleArena.GetInstance().GetEnemy(character, fRange);
        }

        return null;
    }

    // 锁定目标
    public void ShowCharacterLocked(Character character)
    {
        Skill_Target_Flag.target = character;
        Skill_Target_Flag.playAnimation();
    }

    //getter and setter
    public SkillTargetFlag Skill_Target_Flag
    {
        get
        {
            if (_skill_target_flag == null)
            {
                GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.MOSTER_SELECT_FLAG, EBundleType.eBundleBattleEffect);
                GameObject skillTargetObject = BundleMemManager.Instance.instantiateObj(asset, new Vector3(0, -10000, 0), Quaternion.identity);
                _skill_target_flag = skillTargetObject.GetComponentInChildren<SkillTargetFlag>();
            }
            return _skill_target_flag;
        }
    }

    // 汇报打中世界boss消息
    public void ReportHitWorldBoss(SkillAppear skill, Character target)
    {
        NetBase net = NetBase.GetInstance();
        GCReportAttackedTarget msg = new GCReportAttackedTarget();

        msg.m_un32TargetID = (uint)target.GetProperty().GetInstanceID();
        msg.m_un32SkillID = (uint)skill.GetSkillId();

        net.Send(msg.ToBytes(), false);
    }
}
