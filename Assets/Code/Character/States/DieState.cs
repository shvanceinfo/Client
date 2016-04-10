using UnityEngine;
using System.Collections;

public class DieState : State
{
    public override void Enter(CharacterAI ai)
    {
        ai.getOwner().RemoveAllBuffObject();
        ai.SetCharacterState(CharacterAI.CHARACTER_STATE.CS_DIE);
        ai.getOwner().ChangeAppear(ai.getOwner().m_kDieAppear);

        // remove all effect obj on this unit
        for (int i = 0; i < ai.getOwner().transform.childCount; ++i )
        {
            Transform child = ai.getOwner().transform.GetChild(i);

            if (child && child.name.Equals("effectObj"))
            {
                child.parent = null;
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
