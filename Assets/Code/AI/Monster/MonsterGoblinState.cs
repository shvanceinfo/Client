using UnityEngine;
using System.Collections;

public class MonsterGoblinState : State
{
    public override void Enter(CharacterAI ai)
    {
        ai.SetCharacterState(CharacterAI.CHARACTER_STATE.CS_MOVE);
        ai.getOwner().ChangeAppear(ai.getOwner().m_kGoblinRunAppear);
    }

    public override void Execute(CharacterAI ai)
    {
        if (CharacterPlayer.sPlayerMe.HasBuff(BUFF_TYPE.BT_MEAN))
        {
            ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, CharacterPlayer.sPlayerMe);
        }
    }

    public override void Exit(CharacterAI ai)
    {

    }
}
