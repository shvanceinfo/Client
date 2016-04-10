using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffFactory
{
    public static BaseBuff CreateBuff(int nBuffId, Character character, float param = 0.0f)
    {
        BUFF_TYPE bt = ConfigDataManager.GetInstance().getSkillEffectConfig().getSkillEffectData(nBuffId).buffType;

        switch (bt)
        {
            case BUFF_TYPE.BT_POISON_HURT:
                {
                    return new PoisionBuff(nBuffId, character, param);
                }
            default: 
                {
                    return new BaseBuff(nBuffId, character, param);
                }
        }
    }
}