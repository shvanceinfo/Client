using UnityEngine;
using System.Collections;

public class GoblinRunAppear : MoveAppear 
{
    float m_fTime;
    float m_fRunAwaySaveTime;
    float m_fRunAwayTime;

    float m_fRunAwaryMinDuration;
    float m_fRunAwaryMaxDuration;

    Vector3 m_vecRunAwayDir;

    bool m_bNextWay;

    public GoblinRunAppear()
    {
        battle_state = BATTLE_STATE.BS_GOBLIN_RUN;
        m_fRunAwayTime = 0.0f;
        m_fRunAwaySaveTime = 0.0f;
        m_fRunAwaryMinDuration = 0.1f;
        m_fRunAwaryMaxDuration = 1.0f;
        m_fTime = 0.0f;

        m_vecRunAwayDir = Vector3.zero;

        m_bNextWay = true;
    }
	
	public override void update(float delta) 
    {
        owner.playAnimation("run");

        m_fRunAwayTime += delta;
		

        if (m_bNextWay)
        {
            m_fRunAwaySaveTime = Random.Range(m_fRunAwaryMinDuration, m_fRunAwaryMaxDuration);
            m_vecRunAwayDir = new Vector3(Random.Range(-1.0f,1.0f), 0, Random.Range(-1.0f,1.0f));
            m_vecRunAwayDir.Normalize();
                
            m_bNextWay = false;
        }

        owner.setFaceDir(m_vecRunAwayDir);
        owner.movePosition(owner.GetProperty().getMoveSpeed() * delta * m_vecRunAwayDir);

        if (m_fRunAwayTime > m_fRunAwaySaveTime)
        {
            m_bNextWay = true;
            m_fRunAwayTime = 0.0f;
        }
	}
}
