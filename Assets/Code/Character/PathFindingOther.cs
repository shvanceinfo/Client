using UnityEngine;
using System.Collections;

// 其他人的寻路 一般只在 主角打数据的情况出现
public class PathFindingOther : MonoBehaviour 
{
    private NavMeshAgent m_kAgent;
    private CharacterPlayerOther m_kOther;
    private bool m_bBeginMove;
    private bool m_bArrive;

    void Awake()
    {
        m_kOther = GetComponent<CharacterPlayerOther>();
        m_kAgent = GetComponent<NavMeshAgent>();
        m_bBeginMove = false;
        m_kAgent.updateRotation = false;
        m_kAgent.updatePosition = false;
        m_bArrive = false;
    }

    void Update()
    {
        if (m_bBeginMove)
        {
            if (m_kAgent.remainingDistance == 0 && m_bArrive)
            {
                m_bBeginMove = false;
                m_kOther.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
            }
            else
            {
                m_bArrive = true;

                Vector3 dircection = m_kAgent.destination - m_kOther.getPosition();
                dircection.Normalize();

                m_kOther.setFaceDir(dircection);
                m_kOther.SetPos(m_kAgent.nextPosition);
            }
        }
    }

    public void beginMove()
    {
        m_bBeginMove = true;
        m_bArrive = false;
    }

    public void StopMove()
    {
        m_bBeginMove = false;
        m_bArrive = true;

        m_kAgent.Stop();
    }
}
