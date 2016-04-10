using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DizzyState : State
{
    public float m_fDuration;

    public override void SetParam(ArrayList param)
    {
        m_fDuration = (float)param[0];
    }

    public override void Enter(CharacterAI ai)
    {
        ai.SetCharacterState(CharacterAI.CHARACTER_STATE.CS_DIZZY);
        ai.getOwner().m_kDizzyAppear.SetDuration(m_fDuration);
        ai.getOwner().ChangeAppear(ai.getOwner().m_kDizzyAppear);

        //if (ai.getOwner().getType() == CharacterType.CT_MONSTER)
        //{
        //    ai.getOwner().GetComponent<PathFindingMonster>().StopMove();
        //}
        //else if (ai.getOwner().getType() == CharacterType.CT_PLAYER)
        //{
        //    ai.getOwner().GetComponent<PathFinding>().StopMove();
        //}
        //else if (ai.getOwner().getType() == CharacterType.CT_PLAYEROTHER)
        //{
        //    ai.getOwner().GetComponent<PathFindingOther>().StopMove();
        //}


        // remove all effect obj on this unit
        for (int i = 0; i < ai.getOwner().transform.childCount; ++i)
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