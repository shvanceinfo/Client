using UnityEngine;
using System.Collections;

public class GoblinDieAppear : DieAppear 
{
    float m_fDieTime;

	public override void active() 
    {
        animation_name = "die2a";

        on_active();
	}

   
    public override void update(float delta)
    {
        base.update(delta);

        if (time_since_begin > m_fDieTime)
        {
            MonsterManager.Instance.destroyMonster(owner as CharacterMonster);
        }
    }
}
