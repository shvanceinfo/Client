using UnityEngine;
using System.Collections;

public class PathFindingMonster : MonoBehaviour 
{
	private NavMeshAgent m_kAgent;
	private CharacterMonster m_kMonster;
	private bool m_bBeginMove;  //人物开始移动
	private bool m_bArrive; //已经移动过
	
	private float m_fUpdateTime = 0.0f;

    public Character m_kFacedCharacter = null;
	
	void Awake()
	{
        m_kMonster = GetComponent<CharacterMonster>();
        m_kAgent = GetComponent<NavMeshAgent>();
        m_bBeginMove = false;
        m_kAgent.updateRotation = false;
        m_kAgent.updatePosition = false;
        m_bArrive = false;

        m_kFacedCharacter = CharacterPlayer.sPlayerMe;
	}
	
	void Update()
	{
        GraphicsUtil.m_LinePoint.Clear();

        if (m_bBeginMove)
		{
            if (m_kAgent.remainingDistance == 0 && m_bArrive)
            {
                m_bBeginMove = false;
				//MessageManager.Instance.sendMessageAskMove(m_kMonster.GetProperty().GetInstanceID(), m_kMonster.getFaceDir(), m_kMonster.getPosition());
            }
            else
            {
                m_bArrive = true;

                Vector3 dircection = Vector3.zero;

                if (m_kFacedCharacter)
                {
                    dircection = m_kFacedCharacter.transform.position - m_kMonster.getPosition();
                }
                else
                {
                    dircection = m_kAgent.nextPosition - m_kMonster.getPosition();
                }

                dircection.Normalize();

                m_kMonster.setFaceDir(dircection);
                m_kMonster.setPosition(m_kAgent.nextPosition);
                //m_kMonster.transform.position = m_kAgent.nextPosition;

                //GraphicsUtil.m_LinePoint.Add(m_kAgent.nextPosition);
                //GraphicsUtil.m_LinePoint.Add(m_kAgent.nextPosition + dircection);


                // 多人副本 需要将怪物信息同步出去
                if (CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap())
                {
					
					//if (m_fUpdateTime > 0.1f)
					{
						MessageManager.Instance.sendMessageAskMove(m_kMonster.GetProperty().GetInstanceID(), m_kMonster.getFaceDir(), m_kMonster.getPosition());
						m_fUpdateTime = 0.0f;
					}
						
                }
            }
			
			m_fUpdateTime += Time.deltaTime;
        }
    }
	
	public void beginMove(Character target)
	{
        m_bBeginMove = true;
		m_bArrive = false;
        m_kFacedCharacter = target;
	}

    public void StopMove()
    {
        m_bBeginMove = false;
        m_bArrive = true;

        m_kAgent.Stop();
    }
}
