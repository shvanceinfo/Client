using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CharacterMonsterNet : CharacterMonster 
{
    public Vector3 m_kMovePath = Vector3.zero;

    public List<PlayerOtherSkill> skill_caches = null;

	// Use this for initialization
	protected void Start () 
    {
		//setSpeed(monster_property.getMoveSpeed());
		skill.setHurtProtecting(false);

        Renderer[] allRenderer = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in allRenderer)
        {
            //if (!renderer.gameObject.GetComponent("WillRenderTransparent"))
            //{
            //    renderer.gameObject.AddComponent("WillRenderTransparent");
            //}

            // boss 和 精英怪 需要上色
            if (monster_property.GetSurfaceType() == MonsterProperty.MONSTER_SURFACE_LIGHT.MSL_BOSS)
            {
                renderer.sharedMaterial.SetFloat("_RimPower", (0.5f + 8.0f) * 0.7f);
                renderer.sharedMaterial.SetFloat("_UseRim", 1.0f);
                renderer.sharedMaterial.SetColor("_RimColor", new Color(255 / 255.0f, 0, 227 / 255.0f, 0));
            }
            else if (monster_property.GetSurfaceType() == MonsterProperty.MONSTER_SURFACE_LIGHT.MSL_ELITE)
            {
                renderer.sharedMaterial.SetFloat("_RimPower", (0.5f + 8.0f) * 0.7f);
                renderer.sharedMaterial.SetFloat("_RimPower", 1.0f);
                renderer.sharedMaterial.SetFloat("_UseRim", 1.0f);
                renderer.sharedMaterial.SetColor("_RimColor", new Color(255 / 255.0f, 227 / 255.0f, 0, 0));
            }
        }

        skill_caches = new List<PlayerOtherSkill>();
	}

    public override void UpdateInput()
    {
        if (skill_caches.Count > 0)
        {
            PlayerOtherSkill otherSkill = skill_caches[0];
            skill_caches.RemoveAt(0);

            switch (otherSkill.skill_id)
            {
                case 400002001:
                    ChangeAppear(skill.InitSkill(new SkillFlash(400002001)));
                    break;
                case 400003001:
                    ChangeAppear(skill.InitSkill(new SkillMagFlash(400003001)));
                    break;
                default:
                    ChangeAppear(skill.CreateSkill(otherSkill.skill_id));
                    break;
            }
            return;
        }

        if (m_kMovePath != Vector3.zero)
        {
            moveTo(m_kMovePath);
            m_kMovePath = Vector3.zero;
        }
    }

    void OnDestroy()
    {

        if (!MonsterManager.Instance.IsThereMonster())
        {
            EffectManager.Instance.EndCloseUpEffect();
        }
    }


    public void SetMovePath(Vector3 pos)
    {
        m_kMovePath = pos;
    }

    public override void BeHitBackHitFlyHitBroken(SkillAppear skill)
    {
        base.BeHitBackHitFlyHitBroken(skill);

        // 击退击飞击破处理
        Vector3 hitBackDir = transform.position - skill.getOwner().transform.position;

        float fKnockBack = skill.getAttackRepel() * monster_property.getRepelSpeed();

        if (GetProperty().getHP() > 0)
        {
            ArrayList param = new ArrayList();
            param.Add(fKnockBack);
            param.Add(hitBackDir);

        }
        else
        {
            
        }
    }

    public void pushSkill(PlayerOtherSkill otherSkill)
    {
        skill_caches.Add(otherSkill);
    }
}
