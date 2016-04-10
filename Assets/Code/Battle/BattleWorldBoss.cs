using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NetGame;
using manager;
using System;

// 世界boss
class BattleWorldBoss
{
    static private BattleWorldBoss _instance;

    //	private PublicDataItem proPertydata;    //鼓舞属性
    //	private PublicDataItem buffPerValData;  //鼓舞每次提升的千分比
    //	private PublicDataItem topLimitData;	//鼓舞每次千分比上限


    private BattleWorldBoss()
    {

        //		proPertydata = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData ((int)EBuffData.Buff_Type); //得到鼓舞属性类型的信息
        //		buffPerValData = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData ((int)EBuffData.Buff_Per_Value); //得到每次鼓舞提升属性的千分比
        //		topLimitData = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData ((int)EBuffData.Buff_Top_Limit);	//得到鼓舞千分比上限
    }



    static public BattleWorldBoss GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BattleWorldBoss();
            return _instance;
        }

        return _instance;
    }

    enum EBuffData
    {
        Buff_Type = 1003003,				//鼓舞属性类型-（属性枚举，属性枚举）
        Buff_Per_Value = 1003004,		//鼓舞每次提升属性的千分比-（千分比）
        Buff_Top_Limit = 1003005,		//鼓舞千分比上限-（千分比）
    }

    // 还是为了解决时序问题
    public GSNotifyObjectAppear m_kMonsterInfo;
    public bool m_bEnterLevel = false;
    public bool m_bSpawed = false;

    public void ResetData()
    {
        m_bSpawed = false;
        m_bEnterLevel = false;
        m_kMonsterInfo = null;
    }

    public void SpawnWorldBossInfo()
    {
        if (m_kMonsterInfo != null)
        {
            GameObject obj = MonsterManager.Instance.spawnMonsterNet(
            new Vector3(m_kMonsterInfo.m_fPosX, m_kMonsterInfo.m_fPosY, m_kMonsterInfo.m_fPosZ),
            new Vector3(0.0f, 0.0f, 0.0f),
            (int)m_kMonsterInfo.m_un32TempID,
            (int)m_kMonsterInfo.m_un32ObjID
            );

            Vector3 oldFace = obj.GetComponent<CharacterMonsterNet>().getFaceDir();

            Quaternion rot = Quaternion.Euler(m_kMonsterInfo.m_fDirX, m_kMonsterInfo.m_fDirY, m_kMonsterInfo.m_fDirZ);

            oldFace = rot * oldFace;

            obj.GetComponent<CharacterMonsterNet>().setFaceDir(oldFace);

            m_bEnterLevel = true;
            m_bSpawed = true;
            m_kMonsterInfo = null;

            return;
        }

        m_bEnterLevel = true;
        m_bSpawed = false;
    }

    public void RealDamageProcess(GSNotifyObjectHurm kHurtData)
    {
        // 判断是否是自己
        if (CharacterPlayer.character_property.GetInstanceID() == kHurtData.m_un32ObjID)
        {
            // 自己受到怪的攻击
            DamageProcessByType(CharacterPlayer.sPlayerMe, (DAMAGE_TYPE)kHurtData.m_n32Effect, kHurtData.m_n32HPValue, kHurtData.m_n32HP);
            return;
        }

        // 再判断是其他人还是怪
        CharacterPlayerOther cpo = PlayerManager.Instance.getPlayerOther((int)kHurtData.m_un32ObjID);
        if (cpo)
        {
            // 其他人被怪打 副本中不能人打人 走被怪打流程
            DamageProcessByType(cpo, (DAMAGE_TYPE)kHurtData.m_n32Effect, kHurtData.m_n32HPValue, kHurtData.m_n32HP);
            return;
        }

        CharacterMonster cm = MonsterManager.Instance.GetMonster((int)kHurtData.m_un32ObjID);
        if (cm)
        {
            // 怪被打了
            DamageProcessByType(cm, (DAMAGE_TYPE)kHurtData.m_n32Effect, kHurtData.m_n32HPValue, kHurtData.m_n32HP);
            return;
        }
    }

    public void DamageProcessByType(Character target, DAMAGE_TYPE type, int damageValue, int hp)
    {
        if (target.getType() == CharacterType.CT_MONSTER)
        {
            if (target.m_bIsNetDead)
            {
                return;
            }

            target.GetProperty().setHP(hp);

            if (hp == 0)
            {
                target.m_bIsNetDead = true;
                return;
            }
        }
        else
        {
            target.GetProperty().setHP(hp);

            if (hp == 0)
            {
                target.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_DIE);

                return;
            }
        }



        // 根据伤害类型来走不同的表现
        switch ((DAMAGE_TYPE)type)
        {
            case DAMAGE_TYPE.DT_COMMON_DAMAGE:
            case DAMAGE_TYPE.DT_BLAST_DAMAGE:
                CommonBlastDamageAppear(target, damageValue, type == DAMAGE_TYPE.DT_COMMON_DAMAGE ? false : true);
                break;
            case DAMAGE_TYPE.DT_WITH_STAND:
                DodgeDamageAppear(target, damageValue);
                break;
            case DAMAGE_TYPE.DT_BUFF_DAMAGE:
                BuffDamageAppear(target, damageValue);
                break;
        }
    }

    // 普通伤害表现
    public void CommonBlastDamageAppear(Character target, int hurtNum, bool bBlast)
    {
        EffectManager.Instance.BeHitHightlight(target.gameObject);

        if (target.getType() == CharacterType.CT_MONSTER)
        {
            CharacterMonster monster = target as CharacterMonster;

            if (ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(monster.monster_property.template_id).isShouJi)
            {
                EffectManager.Instance.createFX("Effect/Effect_Prefab/Role/Other/shouji", target.getTagPoint("help_body"));
            }
        }
        else
        {
            ArrayList param = new ArrayList();
            param.Add(0.0f);
            param.Add(Vector3.zero);
            target.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
        }


        FloatBloodNum.Instance.PlayFloatBood(target.getType() == CharacterType.CT_MONSTER,
            hurtNum, target.getTagPoint("help_hp"), bBlast ? eHurtType.doubleHurt : eHurtType.normalHurt);
    }

    // 招架伤害表现
    public void DodgeDamageAppear(Character target, int hurtNum)
    {
        EffectManager.Instance.createFX("Effect/Effect_Prefab/Role/Other/zhaojia", target.transform, 10.0f);

        ArrayList param = new ArrayList();
        param.Add(0.0f);
        param.Add(Vector3.zero);
        target.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);

        FloatBloodNum.Instance.PlayFloatBood(false,
            hurtNum, target.getTagPoint("help_hp"), eHurtType.withstandHurt);
    }

    // buff伤害表现
    public void BuffDamageAppear(Character target, int hurtNum)
    {
        // buff 特效表现 需要再走另外的消息流程 这里只走伤害
    }


    // 判断在世界Boss中 碰撞是否有效
    public bool CollisionJudegeValid(Character charSrc, Character charTarget)
    {
        if (Global.InWorldBossMap())
        {
            // 人对人没有伤害
            if (charSrc.getType() != CharacterType.CT_MONSTER && charTarget.getType() != CharacterType.CT_MONSTER)
            {
                return false;
            }

            // 只管主角对怪 的碰撞
            // 其他人对怪 怪对其他人 怪对主角 不管
            if (charSrc.getType() == CharacterType.CT_PLAYEROTHER && charTarget.getType() == CharacterType.CT_MONSTER)
            {
                return false;
            }

            if (charSrc.getType() == CharacterType.CT_MONSTER && charTarget.getType() == CharacterType.CT_PLAYEROTHER)
            {
                return false;
            }

            if (charSrc.getType() == CharacterType.CT_MONSTER && charTarget.getType() == CharacterType.CT_PLAYER)
            {
                return false;
            }
        }

        return true;
    }

    public void OnRecvPropertyChange()
    {

        //		double buffVal = (double)buffPerValData.type2Data / 1000; //得到提升的千分比值
        //		double topVal = (double)topLimitData.type2Data / 1000;	//得到千分比上限
        //		
        //		
        //		CharacterFightProperty fightPro= CharacterPlayer.character_property.fightProperty;
        //		for (int i = 0,max = proPertydata.type1List.Count; i < max; i++) {
        //			eFighintPropertyCate propertyCate = (eFighintPropertyCate)proPertydata.type1List[i];
        //
        //			double newVal =  fightPro.GetBuffValue(propertyCate) + fightPro.GetBaseValue(propertyCate)*buffVal; //得到增加的属性值
        //			newVal = Math.Min(newVal,fightPro.GetBaseValue(propertyCate)*topVal);  			//得到最小值
        //			
        //			fightPro.SetBuffValue(propertyCate,(int)newVal);  //设置buff值
        //		}
    }

    // 收到服务器发来的有人复活了
    public void OnPlayerRelive(GCNotifyPlayerRelive info)
    {
        if (!Global.InWorldBossMap())
        {
            return;
        }

        if (info.m_un32PlayerID == CharacterPlayer.character_property.GetInstanceID())
        {
            // 自己复活了
            CharacterPlayer.sPlayerMe.transform.position = new Vector3(info.m_fPosX, info.m_fPosY, info.m_fPosZ);
            CharacterPlayer.sPlayerMe.setFaceDir(new Vector3(info.m_fDirX, info.m_fDirY, info.m_fDirZ));
            CharacterPlayer.sPlayerMe.OnRevive();
        }
        else
        {
            //看是否是有其他人复活了
            CharacterPlayerOther other = PlayerManager.Instance.getPlayerOther((int)info.m_un32PlayerID);
            if (other)
            {
                other.GetProperty().setHP(other.GetProperty().getHPMax());//重置HP
                other.transform.position = new Vector3(info.m_fPosX, info.m_fPosY, info.m_fPosZ);
                other.setFaceDir(new Vector3(info.m_fDirX, info.m_fDirY, info.m_fDirZ));
                other.OnRevive();
            }
        }
    }

    public static float CalculateDistFromeBoss(Character player, Character boss)
    {
        if (boss == null || player == null)
        {
            return 0.0f;
        }

        // 世界boss的追击 根据Boss包围盒的半径与角色的方向来确定
        if (Global.InWorldBossMap())
        {
            // 这里默认缩放是normal scale
            float scale = Mathf.Max(boss.transform.localScale.x, boss.transform.localScale.y, boss.transform.localScale.z); ;
            float fRealRadius = boss.GetComponent<CapsuleCollider>().radius * scale;
            fRealRadius += 1.0f;

            Vector3 pathDir = player.getPosition() - boss.getPosition();
            pathDir.Normalize();

            Vector3 kSearchPt = boss.getPosition() + pathDir * fRealRadius;

            return Vector3.Distance(player.transform.position, kSearchPt);
        }
        else
            return Vector3.Distance(player.transform.position, boss.transform.position);
    }
}