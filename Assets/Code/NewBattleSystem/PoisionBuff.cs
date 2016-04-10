using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoisionBuff : BaseBuff
{
    float m_poisionTime;
    public PoisionBuff(int buffid, Character character, float param = 0.0f)
        : base(buffid, character, param)
    {
        m_poisionTime = 0.0f;
    }

    public override void Update(float delta)
    {
        base.Update(delta);

        if (m_bActive)
        {
            if (m_poisionTime > 1.0f)
            {
                PoisionHurt();
                m_poisionTime = 0.0f;
            }

            m_poisionTime += delta;
        }
    }

    void PoisionHurt()
    {
        // 如果在PVP需要发送伤害消息
        if (Global.inMultiFightMap())
        {
            if (CharacterPlayer.character_property.getHostComputer())
            {
                // 这种情况只有 主角打怪 和怪打所有人
                m_kOwner.GetProperty().setHP(m_kOwner.GetProperty().getHP() - (int)m_fParam);
                // 发消息告诉其他客户端 被打的单位是否死亡 是否受击  是否被招架
                MessageManager.Instance.sendObjectHurt(true,
                    (uint)m_kOwner.GetProperty().GetInstanceID(),
                    (int)m_fParam,
                    m_kOwner.GetProperty().getHP(),
                    (int)DAMAGE_TYPE.DT_BUFF_DAMAGE);
            }
            else
            {
                if (m_kOwner.getType() == CharacterType.CT_MONSTER && m_kOwner.m_eAppearState != Appear.BATTLE_STATE.BS_DIE)
                {
                    // 非主机 把主角对怪造成的伤害 先发给主机，让主机来决定怪的表现
                    MessageManager.Instance.sendObjectHurt(false,
                        (uint)m_kOwner.GetProperty().GetInstanceID(),
                        (int)m_fParam,
                        m_kOwner.GetProperty().getHP(),
                        (int)DAMAGE_TYPE.DT_BUFF_DAMAGE);

                    return;
                }
            }
        }
        else
            m_kOwner.GetProperty().setHP(m_kOwner.GetProperty().getHP() - (int)m_fParam);


        if (!Global.InArena() && !Global.inMultiFightMap() && m_kOwner.getType() == CharacterType.CT_MONSTER)
        {
	        GCReportHurt net = new GCReportHurt((uint)0,
	            (uint)m_kOwner.GetProperty().GetInstanceID(),
	            0,
	            (int)m_fParam,
	            m_kOwner.GetProperty().getHP());
	
	         MainLogic.SendMesg(net.ToBytes());

             //Debug.Log("汇报伤害buff " + m_fParam);
        }

        // 中毒飘血
//        FloatMessage.GetInstance().PlayFloatMessage3D(
//        Global.FormatStrimg(LanguageManager.GetText("fight_float_poision_buff"), m_fParam.ToString()),
//        1.0f,
//        m_kOwner.getTagPoint("help_hp"),
//        new Vector3(0, 0.8f, 0));
		FloatBloodNum.Instance.PlayFloatBood(false, (int)m_fParam, m_kOwner.getTagPoint("help_hp"), eHurtType.golbinPoison);

        if (m_kOwner.GetProperty().getHP() <= 0)
        {
            m_kOwner.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_DIE);
        }
    }
}