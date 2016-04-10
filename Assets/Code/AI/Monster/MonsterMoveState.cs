using UnityEngine;
using System.Collections;

public class MonsterMoveState : MoveState {
	
    //float move_cd = -1.0f;
	
    //public override void enter(MonsterAI ai){
    //    //Loger.Log("move enter");
    //    ai.setMonsterState(MonsterAI.MonsterState.MS_MOVE);
    //}

    //public override void execute(MonsterAI ai){

    //    /*
    //    if (ai.protect_time > 0) {
    //        ai.protect_time -= Time.deltaTime;
    //        if (ai.protect_time <=0) {
    //            ai.getOwner().skill.setHurtProtecting(false);
    //        }
    //    }
    //    */
		
    //    if (move_cd >= 0) {
    //        move_cd -= Time.deltaTime;
    //        //return;
    //    }
		
    //    move_cd = 2.1f;
		
    //    ai.ai_detail.executeMove(machine,ai);
    //}

    //public override void exit(MonsterAI ai){
    //    Loger.Log("move exit");
    //}
	
    //public override void handleMessage(MonsterAI ai, Telegram tel){
    //    switch (tel.msg) {
    //    case AIMessage.AM_BEATTACK:
    //        TelegramContextBeattack beattackContext = (TelegramContextBeattack)tel.context;
    //        if (beattackContext == null) {
    //            Loger.Error("error 123");
    //        }
			
    //        CharacterMonster theMonster = (CharacterMonster)ai.getOwner();
    //        if (theMonster.monster_property.getHP() > 0) {
    //            EffectManager.sEffectManager.createFX("Effect/Effect_Prefab/Monster/BloodSplat", theMonster.getTagPoint("help_body"));
    //            EffectManager.sEffectManager.createFX("Effect/Effect_Prefab/Monster/BloodSplat1", theMonster.getTagPoint("help_body"));
    //        }
    //        else {
    //            EffectManager.sEffectManager.createFX("Effect/Effect_Prefab/Monster/Blood_di", theMonster.getPosition(), Quaternion.identity);
				
    //            BattleManager.Instance.onPlayerkillMonster(theMonster);
    //        }
			
    //        //EffectManager.sEffectManager.createCameraShake(0, 0.1f, 0.1f);
			
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
