using UnityEngine;
using System.Collections;

public class BornPoint : MonoBehaviour {

	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void Awake() 
	{
        //Debug.Log("进入场景的 bornpoint awake");
        //BattleAutomation.GetInstance().Init();
        //BattleAutomation.GetInstance().InitSceneMonsterAreaInfo();

        //if (!Global.inMultiFightMap())
        //{
        //    // 多人副本 坐标由服务器发来
        //    CharacterPlayer.sPlayerMe.transform.position = transform.position;
        //}
        

        //// 先保存当前的自动战斗
        //Global.m_bAutoFightSaved = Global.m_bAutoFight;

        //if (Global.InArena())
        //{
        //    Global.m_bAutoFight = true;

        //    // 默认让对手朝向对手
        //    Quaternion rotR = Quaternion.Euler(0, -90, 0);
        //    CharacterPlayer.sPlayerMe.setFaceDir(rotR * CharacterPlayer.sPlayerMe.transform.forward);
        //}
        //else
        //    Global.m_bAutoFight = false;
        
    }

    void Start()
    {
        //Debug.Log("进入场景的 bornpoint start");

        Global.fightLastMoney = CharacterPlayer.character_asset.gold;

        if (!Global.inMultiFightMap())
        {
            // 多人副本 坐标由服务器发来
            CharacterPlayer.sPlayerMe.transform.position = transform.position;

            if (CharacterPlayer.sPlayerMe.m_kPet != null)
            {
                CharacterPlayer.sPlayerMe.m_kPet.transform.position = transform.position - CharacterPlayer.sPlayerMe.getFaceDir() * CharacterPlayer.sPlayerMe.m_kPet.m_fFollowRadius;
                CharacterPlayer.sPlayerMe.m_kPet.transform.LookAt(transform.position);
            }
        }

        if (Global.InArena())
        {
            Global.m_bAutoFight = true;

            // 默认让对手朝向对手
            Quaternion rotR = Quaternion.Euler(0, -90, 0);
            CharacterPlayer.sPlayerMe.setFaceDir(rotR * CharacterPlayer.sPlayerMe.transform.forward);
        }
        else
            Global.m_bAutoFight = false;
    }
}
