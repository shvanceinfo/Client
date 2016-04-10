using UnityEngine;
using System.Collections;

public class MonsterAttackState : AttackState 
{
    public Character m_kTarget;

    public override void Enter(CharacterAI ai)
    {
        base.Enter(ai);

        //ai.getOwner().ChangeAppear(ai.getOwner().skill.GetCommonAttack());
	}

    public override void Execute(CharacterAI ai)
    {
        if (!ai.getOwner().skill.GetCommonAttack().isActive())
        {
            ai.getOwner().GetComponent<CoolDownProperty>().AddCDObj(0);
            m_kMachine.ChangeState(ai.m_kIdleState);
        }
	}

   
	
    //public override void handleMessage(MonsterAI ai, Telegram tel){
		
    //    switch (tel.msg) {
    //    case AIMessage.AM_BEATTACK:
    //        TelegramContextBeattack beattackContext = (TelegramContextBeattack)tel.context;
    //        if (beattackContext == null) {
    //            Loger.Error("error 123");
    //        }
			
    //        CharacterMonster theMonster = (CharacterMonster)ai.getOwner();
    //        if (theMonster.monster_property.getHP() > 0) {
    //            EffectManager.Instance.createFX("Effect/Effect_Prefab/Monster/BloodSplat", theMonster.getTagPoint("help_body"));
    //            EffectManager.Instance.createFX("Effect/Effect_Prefab/Monster/BloodSplat1", theMonster.getTagPoint("help_body"));
    //        }
    //        else {
    //            EffectManager.Instance.createFX("Effect/Effect_Prefab/Monster/Blood_di", theMonster.getPosition(), Quaternion.identity);
				
    //            BattleManager.Instance.onPlayerkillMonster(theMonster);
    //        }

    //        //EffectManager.Instance.createCameraShake(0, 0.1f, 0.1f);
			
    //        ai.setPrefType(beattackContext.perf);
    //        ai.setHitBackDir(beattackContext.hitBackDir);
    //        ai.setSpeedRate(beattackContext.speedRate);
    //        if (beattackContext.perf == 0 || beattackContext.perf == 3) {
    //            machine.changeState(ai.ai_beattack_state);
    //        }
    //        else if (beattackContext.perf == 1) {
    //            if (theMonster.monster_property.getHP() > 0) {
    //                machine.changeState(ai.ai_beattack_state);
    //            }
    //            else {
    //                machine.changeState(ai.ai_dying_state);
    //            }
    //        }
    //        else if (beattackContext.perf == 2) {
    //            if (theMonster.monster_property.getHP() <= 0) {
    //                machine.changeState(ai.ai_dying_state);
    //            }
    //        }
    //        //其它的表现类型
    //        else {
    //            return;
    //        }
				
			
    //        break;
    //    }
    //}
}
