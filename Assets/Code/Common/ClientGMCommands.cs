using System;
using System.Collections.Generic;
using UnityEngine;
using NetGame;

public class ClientGMCommands
{
    public static void CreateEnemy(List<int> param)
    {
        CharacterOtherProperty enemyProp = new CharacterOtherProperty();
        enemyProp.server_id = -1;
        enemyProp.nick_name = "GM专用练级怪";
        //enemyProp.career = CHARACTER_CAREER.CC_ARCHER;

        enemyProp.career = CHARACTER_CAREER.CC_SWORD;
        enemyProp.sex = 0;
        enemyProp.weapon = 1010620;
        //enemyProp.armor = 1010120;
        enemyProp.level = 1;
        enemyProp.setHP(118600);
        enemyProp.hp_max = 118600;
        enemyProp.mp = 545;
        //enemyProp.setWeapon(1010710);
        enemyProp.setWeapon(1010620);
        enemyProp.attack_power = 90;
        enemyProp.defence = 25;




        CharacterPlayerOther cpo = PlayerManager.Instance.createPlayerOther(enemyProp, Vector3.one, new Vector3(0.0f, 0.0f, 1.3f));

        enemyProp.fightProperty.ResetProperty();
        enemyProp.fightProperty.SetBaseValue(eFighintPropertyCate.eFPC_MaxMP, 545);

        cpo.gameObject.name = "other";

        cpo.GetComponent<CharacterAnimCallback>().SetCharType(CHAR_ANIM_CALLBACK.CAC_OTHERS);

        List<AIDataItem> ailist = ConfigDataManager.GetInstance().getAIConfig().GetAIList(true, enemyProp.getCareer());

        for (int i = 0; i < ailist.Count; ++i)
        {
            CharacterAI.AISkillData aiData = new CharacterAI.AISkillData();
            aiData.nAIID = ailist[i].id;
            aiData.nSkillID = ConfigDataManager.GetInstance().getAIConfig().getAIData(aiData.nAIID).AIValue;
            cpo.GetAI().m_kAISkillBehavious.Add(aiData);
        }


        cpo.skill.setSkillTarget(CharacterPlayer.sPlayerMe);

        CharacterPlayer.sPlayerMe.m_kGMEnemy = cpo;


        List<AIDataItem> mylist = ConfigDataManager.GetInstance().getAIConfig().GetAIList(true, CharacterPlayer.character_property.getCareer());

        CharacterPlayer.sPlayerMe.GetAI().m_kAISkillBehavious.Clear();
        for (int i = 0; i < mylist.Count; ++i)
        {
            CharacterAI.AISkillData aiData = new CharacterAI.AISkillData();
            aiData.nAIID = mylist[i].id;
            aiData.nSkillID = ConfigDataManager.GetInstance().getAIConfig().getAIData(aiData.nAIID).AIValue;
            CharacterPlayer.sPlayerMe.GetAI().m_kAISkillBehavious.Add(aiData);
        }
    }

    public static void CreateNpc(List<int> param)
    {
        for (int i = 0; i < param[1]; ++i )
        {
            MonsterManager.Instance.spawnMonster(new Vector3(3.6f, 0.0f, 1.36f), Quaternion.identity, param[0]);
        }
        
    }


    public static void GetItem(List<int> param)
    {
        GCAskAddGoods addGoods = new GCAskAddGoods((UInt32)param[0], param[1]);
        NetBase.GetInstance().Send(addGoods.ToBytes());
    }

    public void MoveEnemy()
    {

    }

    public static void SetLevel(List<int> param)
    {
        if (param.Count >= 2)
        {
            NetBase.GetInstance().Send((new GCAskGMChangeLevel((UInt16)param[0], (UInt16)param[1])).ToBytes());
        }       
    }

    public static void GMCommand(string name, List<int> param)
    {
        if (name.ToLower() == "cm")
        {
            CreateEnemy(param);
        }
        else if (name.ToLower() == "cn")
        {
            CreateNpc(param);
        }
        else if (name.ToLower() == "setlevel")
        {
            SetLevel(param);
        }
        else if (name.ToLower() == "getitem")
        {
            GetItem(param);
        }
        else if (name.ToLower() == "allgate")
        {
            ThroughGate(true, param);
        }
        else if (name.ToLower() == "gate")
        {
            ThroughGate(false, param);
        }
    }

    public static void ThroughGate(bool allGate, List<int> param)
    {
        if (allGate)
        {
            GCAskGMPassGate passGateMsg = new GCAskGMPassGate(200100531);
            NetBase.GetInstance().Send(passGateMsg.ToBytes());
        }
        else
        {
            GCAskGMPassGate passGateMsg = new GCAskGMPassGate((UInt32)param[0]);
            NetBase.GetInstance().Send(passGateMsg.ToBytes());
        }
    }
}