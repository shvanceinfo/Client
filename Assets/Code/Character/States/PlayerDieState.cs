using UnityEngine;
using manager;

public class PlayerDieState : DieState
{
    public override void Enter(CharacterAI ai)
    {
        Debug.Log("主角死亡");
        base.Enter(ai);

        if (Global.InArena())
        {
            BattleArena.GetInstance().PlayerDie();
        }

        if (Global.inMultiFightMap() && CharacterPlayer.character_property.getHostComputer())
        {
            MessageManager.Instance.sendObjectBehavior((uint)ai.getOwner().GetProperty().GetInstanceID(),
                (int)Appear.BATTLE_STATE.BS_DIE, ai.getOwner().getFaceDir(), ai.getOwner().getPosition());
        }

        if (Global.inMultiFightMap() && CharacterPlayer.character_property.getHostComputer())
        {
            BattleMultiPlay.GetInstance().OnPlayerDie(ai.getOwner());
        }
		
		if (Global.InWorldBossMap()) {
			BossManager.Instance.ShowPeopleDeadUI();
		}
		
		ai.getOwner ().GetComponent<FightProperty> ().m_kLockedEnemy = null;
		
    }
}