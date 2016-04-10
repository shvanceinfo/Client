using UnityEngine;
using System.Collections;

//////////////////////////////////////////////////////////////////////////
// AI 静态函数
//////////////////////////////////////////////////////////////////////////

public class AIUtil 
{
    // 静态函数 通用的AI行为 由AI.xml驱动
    public static bool CanBehaviousBeExecute(CharacterAI ai, Character target, CharacterAI.AISkillData aiData)
    {
        float dist = Vector3.Distance(ai.getOwner().transform.position, target.transform.position);

        AIDataItem aiItem = ConfigDataManager.GetInstance().getAIConfig().getAIData(aiData.nAIID);

        // 判断是否在技能范围
        if (dist < aiItem.distanceConstraint)
        {
            float hpRatio = ai.getOwner().GetProperty().GetHpRatio();
            float mpRatio = ai.getOwner().GetProperty().GetMpRatio();

            if (hpRatio < (float)aiItem.HPConstraintDown)
            {
                if (hpRatio > (float)aiItem.HPConstraintUp)
                {
                    if (ai.getOwner().getType() == CharacterType.CT_MONSTER)
                    {
                        return ai.getOwner().AICastSkill(target, aiData.nSkillID);
                    }
                    else
                    {
                        if (mpRatio < (float)aiItem.MPConstraintDown)
                        {
                            if (mpRatio > (float)aiItem.MPConstraintUp)
                            {
                                return ai.getOwner().AICastSkill(target, aiData.nSkillID);
                            }
                        }
                    }
                }
            }
        }

        return false;
    }


    public static bool TryExecuteAI(CharacterAI ai, Character target)
    {
        // 随机放一个技能
        if (ai.m_kAISkillBehavious.Count != 0)
        {
            CharacterAI.AISkillData nAISkillId = ai.m_kAISkillBehavious[Random.Range(0, ai.m_kAISkillBehavious.Count)];

            return CanBehaviousBeExecute(ai, target, nAISkillId);
        }

        return false;
    }


    public static void AIBehaviour(CharacterAI ai, Character target)
    {
        if (!TryExecuteAI(ai, target))
        {
            CommonBehaviour(ai, target);
        }
    }

    public static void CommonBehaviour(CharacterAI ai, Character target)
    {
        float dist = Vector3.Distance(ai.getOwner().transform.position, target.transform.position);

        if (dist > ai.getOwner().GetProperty().getAttackRange())
        {
            //Debug.Log("进入追击");
			ArrayList param = new ArrayList();
			param.Add(target);
			param.Add(0);
			ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_PURSUE, param);
        }
        else
        {
            // 如果眩晕 不能攻击
            if (ai.getOwner().HasNotBuff(BUFF_TYPE.BT_XUANYUN))
            {
                // 攻击间隔
                if (ai.getOwner().getType() == CharacterType.CT_MONSTER)
                {
                    if (!ai.getOwner().GetComponent<CoolDownProperty>().IsInCD(0))
                    {
                        ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_ATTACK, target);
                    }
                }
                else
                {
                    Vector3 faceToMonster = target.transform.position - ai.getOwner().transform.position;
                    faceToMonster.Normalize();
                    //ai.getOwner().setFaceDir(faceToMonster);
                    ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_ATTACK, target);
                }
            }
        }
    }
}