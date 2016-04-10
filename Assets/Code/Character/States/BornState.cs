public class BornState : State 
{
    public override void Enter(CharacterAI ai)
    {
        ai.getOwner().ChangeAppear(ai.getOwner().m_kBornAppear);
        ai.SetCharacterState(CharacterAI.CHARACTER_STATE.CS_BORN);
        ai.getOwner().skill.setHurtProtecting(true);
        ai.getOwner().skill.setHurtHide(true);
    }

    public override void Execute(CharacterAI ai)
    {
        if (!ai.getOwner().m_kBornAppear.isActive())
        {
            if (Global.inGoldenGoblin())
            {
                m_kMachine.ChangeState(ai.m_kGoblinState);
            }
            else
            {
                m_kMachine.ChangeState(ai.m_kIdleState);
            }
        }
    }

    public override void Exit(CharacterAI ai)
    {
        ai.getOwner().skill.setHurtProtecting(false);
        ai.getOwner().skill.setHurtHide(false);

        CharacterMonster monster = ai.getOwner() as CharacterMonster;

        if (monster)
        {
            if (ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(monster.monster_property.template_id).isTeXie)
            {
                if (MonsterManager.Instance.MonsterIDInBossList(monster.GetProperty().GetInstanceID()))
                {
                    CameraFollow.sCameraFollow.BeginCamZoomOut();
                }
            }
        }
    }	
}
