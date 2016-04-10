using UnityEngine;
using System.Collections;

public class SkillMonsterCommonCmd : SkillAppear {

    public SkillMonsterCommonCmd(int skillid)
        : base(skillid)
    {
        battle_state = Appear.BATTLE_STATE.BS_PING_KAN;

        init();
    }
   

    public override void active()
    {
        owner.m_eAppearState = battle_state;

        base.active();

        if ((!CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap()) ||
        Global.InWorldBossMap())
        {
            // 多人副本中的怪只播动画
            return;
        }

        // 设置朝向
        if (owner.GetAI().m_kAttackState.m_kTarget)
        {
            Vector3 tmp = owner.GetAI().m_kAttackState.m_kTarget.transform.position - owner.transform.position;
            tmp.Normalize();
            owner.setFaceDir(tmp);
        }

        if (CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap())
        {
            MessageManager.Instance.sendObjectBehavior((uint)owner.GetProperty().GetInstanceID(),
                (int)battle_state, owner.getFaceDir(), owner.getPosition());
        }
    }
}
