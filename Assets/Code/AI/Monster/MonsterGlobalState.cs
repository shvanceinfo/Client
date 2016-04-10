using UnityEngine;
using System.Collections;

public class MonsterGlobalState : GlobalState {

	public override void Enter(CharacterAI ai){
		
	}

    public override void Execute(CharacterAI ai)
    {
        if (Global.inGoldenGoblin())
        {
            if (!CharacterPlayer.sPlayerMe.HasBuff(BUFF_TYPE.BT_MEAN))
            {
                if (ai.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_PURSUE ||
                    ai.GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_ATTACK)
                {
                    ai.m_kMachine.ChangeState(ai.m_kGoblinState);
                }
            }  
        }
    }
}
