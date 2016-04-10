using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseBuff
{
    public int m_nBuffId;

    public GameObject m_kSpecialEffectPre;
    public GameObject m_kSpecialEffect;


    public Character m_kOwner;

    public SkillEffectItem m_kItem;


    protected float m_fParam;

    protected float m_fTime = 0.0f;

    public bool m_bActive;

    public BaseBuff(int buffid, Character character, float param = 0.0f)
    {
        m_nBuffId = buffid;
        m_kOwner = character;
        m_fParam = param;

        m_bActive = true;

        m_kItem = ConfigDataManager.GetInstance().getSkillEffectConfig().getSkillEffectData(m_nBuffId);

        Init();
    }

    public virtual void Init()
    {
        // 1. 挂载触发特效
        if (m_kItem.triggerEffectPre != null)
        {
            Transform kAddPos = m_kOwner.getTagPoint("help_body");

            GameObject asset = BundleMemManager.Instance.getPrefabByName(m_kItem.triggerEffectPre, EBundleType.eBundleBattleEffect);
            m_kSpecialEffectPre = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
            m_kSpecialEffectPre.transform.parent = kAddPos;
            m_kSpecialEffectPre.transform.localPosition = Vector3.zero;
            m_kSpecialEffectPre.transform.localRotation = Quaternion.identity;
        }


        // 2. 挂载buff特效
        if (m_kItem.effectPre != null)
        {
            Transform kAddPos = null;

            switch (m_kItem.effPos)
            {
                case 0:
                    kAddPos = m_kOwner.getTagPoint("help_hp");
                    break;
                case 1:
                    kAddPos = m_kOwner.getTagPoint("help_body");
                    break;
                case 2:
                    kAddPos = m_kOwner.getTagPoint("shadow");
                    break;
                case 3:
                    kAddPos = m_kOwner.getTagPoint("Footsteps");
                    break;
            }

            GameObject asset = BundleMemManager.Instance.getPrefabByName(m_kItem.effectPre, EBundleType.eBundleBattleEffect);
            m_kSpecialEffect = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
            m_kSpecialEffect.transform.parent = kAddPos;
            m_kSpecialEffect.transform.localPosition = Vector3.zero;
            m_kSpecialEffect.transform.localRotation = Quaternion.identity;
        }
        
        
        // 2. buff动画播放
        switch (m_kItem.buffType)
        {
            case BUFF_TYPE.BT_XUANYUN:
                {
                    if (m_kOwner.GetAI() != null)
                    {
                        m_kOwner.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_DIZZY, m_kItem.lastTime);
                    }
                }
                break;
        }
    }

    public virtual void Update(float delta)
    {
        if (m_bActive)
        {
            if (m_fTime > m_kItem.lastTime)
            {
                m_bActive = false;
                Exit();

                EventDispatcher.GetInstance().OnBuffDisappear(m_kItem.buffType);
                return;
            }

            m_fTime += delta;
        }
    }

    public virtual void Exit()
    {
        ReCalculateBuffProperty();

        DestroySpecialEffect();

        m_kOwner.m_nBuffState &= ~Bit((int)m_kItem.buffType);
    }

    public void DestroySpecialEffect()
    {
        if (m_kSpecialEffect != null)
        {
            GameObject.Destroy(m_kSpecialEffect);
            m_kSpecialEffect = null;
        }

        if (m_kSpecialEffectPre != null)
        {
            GameObject.Destroy(m_kSpecialEffectPre);
            m_kSpecialEffectPre = null;
        }
    }


    public void ReCalculateBuffProperty()
    {
        m_kOwner.ReCalculateBuffProperty();
    }

    public static int Bit(int bit)
    {
        return 1 << bit;
    }
}