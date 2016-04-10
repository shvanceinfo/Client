using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillMagFlash : SkillAppear
{
    float m_fDuration = .0f;
    float m_fDistant = .0f;

    protected Vector3 m_vecInitFaceDir;

    public SkillMagFlash(int nSkillId)
        : base(nSkillId)
    {

    }

    public override void init()
    {
        loadConfig();
    }

    public override void active()
    {
        PlayerSpecialEffect();
        owner.skill.setCurrentSkill(this);
        is_active = true;


        Component[] meshFilters = owner.gameObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer)); // typeof or MeshFilter
        foreach (SkinnedMeshRenderer m in meshFilters)
        {
            m.enabled = false;
        }

        Component[] mesh = owner.gameObject.GetComponentsInChildren(typeof(MeshRenderer)); // typeof or MeshFilter
        foreach (MeshRenderer m in mesh)
        {
            m.enabled = false;
        }

        Weapon wp = owner.GetComponentInChildren<Weapon>();

        if (wp != null)
        {
            Component[] wpMesh = wp.gameObject.GetComponentsInChildren(typeof(MeshRenderer)); // typeof or MeshFilter
            foreach (MeshRenderer m in wpMesh)
            {
                m.enabled = false;
            }
        }




        m_vecInitFaceDir = owner.getFaceDir();

        time_length = 0.2f;
    }

    public override void deActive()
    {
        base.deActive();

        Component[] meshFilters = owner.gameObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer)); // typeof or MeshFilter
        foreach (SkinnedMeshRenderer m in meshFilters)
        {
            m.enabled = true;
        }

        Component[] mesh = owner.gameObject.GetComponentsInChildren(typeof(MeshRenderer)); // typeof or MeshFilter
        foreach (MeshRenderer m in mesh)
        {
            m.enabled = true;
        }



        Weapon wp = owner.GetComponentInChildren<Weapon>();

        if (wp != null)
        {
            Component[] wpMesh = wp.gameObject.GetComponentsInChildren(typeof(MeshRenderer)); // typeof or MeshFilter
            foreach (MeshRenderer m in wpMesh)
            {
                m.enabled = false;
            }
        }

        List<Transform> allSpe = owner.GetTagPoints("effectObj");

        for (int i = 0; i < allSpe.Count; ++i)
        {
            if (allSpe[i] != null)
            {
                Component[] special = allSpe[i].GetComponentsInChildren(typeof(MeshRenderer)); // typeof or MeshFilter
                foreach (MeshRenderer m in special)
                {
                    m.enabled = false;
                }
            }
        }

        owner.SetPos(m_vecInitFaceDir * 5.0f + owner.getPosition());
        PlayerSpecialEffect();
    }
}
