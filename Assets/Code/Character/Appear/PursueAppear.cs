using UnityEngine;
using System.Collections;

public class PursueAppear : Appear
{
    // 寻路目标点
    Vector3 m_kMovePoint = Vector3.zero;

    // 需要移动的时间
    float m_fMoveTime = 0.0f;

    // 开始寻路点
    Vector3 m_kCurPos = Vector3.zero;

    // 更新的时间点
    float m_fUpdateTime = 0.0f;

    public PursueAppear()
    {
        battle_state = BATTLE_STATE.BS_PURSUE;
    }

    public override void active()
    {
        if (Global.inCityMap() &&
            (owner.getType() == CharacterType.CT_PLAYER || owner.getType() == CharacterType.CT_PLAYEROTHER))
        {
            animation_name = "run2";
        }
        else
        {
            animation_name = "run";
        }

        //owner.animation[animation_name].speed = 1.0f;
        owner.animation[animation_name].speed *= owner.GetProperty().fightProperty.m_fFrozenRate;

        on_active(int.MaxValue);

        ResetData();
    }

    public void ResetData()
    {
        m_fUpdateTime = 0.0f;
        m_fMoveTime = 0.0f;
        m_kMovePoint = Vector3.zero;
        m_kCurPos = Vector3.zero;
    }

    public override void update(float delta)
    {
        if (Global.inCityMap() &&
            (owner.getType() == CharacterType.CT_PLAYER || owner.getType() == CharacterType.CT_PLAYEROTHER))
        {
            animation_name = "run2";
        }
        else
        {
            animation_name = "run";
        }

        owner.playAnimation(animation_name);

        if (m_fUpdateTime > (m_fMoveTime - delta*2))
        {
            // 寻路时间到达 并且在目标点的格子内
            owner.GetComponent<AISystem.AIPathFinding>().m_bArrived = true;

            // 如果是主角最好是能判断阻挡 如果是怪可以不考虑
            if (owner.getType() == CharacterType.CT_PLAYER)
            {
                owner.setPosition(m_kMovePoint);
                SyncManager.GetInstance().SendPlayerTranInfo();

				PlayerManager.Instance.CheckOtherAppear();
				PlayerManager.Instance.CheckOtherDisappear();
            }
            else if (owner.getType() == CharacterType.CT_MONSTER)
            {
                owner.transform.position = m_kMovePoint;
                SyncManager.GetInstance().ReportMonsterTranInfo(owner as CharacterMonster);
            }

            if (owner.m_kPet != null)
            {
                owner.m_kPet.m_kMasterPos.Add(owner.transform.position);
            }

            is_active = false;

            ResetData();
            return;
        }

        m_fUpdateTime += delta;

        Vector3 kOldPos = owner.transform.position;

        Vector3 kNewPos = Sample(m_fUpdateTime / m_fMoveTime, m_kMovePoint);

        // 如果是主角最好是能判断阻挡 如果是怪可以不考虑
        if (owner.getType() == CharacterType.CT_PLAYER)
        {
            owner.setPosition(kNewPos);
            SyncManager.GetInstance().SendPlayerTranInfo();
        }
        else if (owner.getType() == CharacterType.CT_MONSTER)
        {
            owner.transform.position = kNewPos;
            SyncManager.GetInstance().ReportMonsterTranInfo(owner as CharacterMonster);
        }

        if (owner.m_kPet != null)
        {
            owner.m_kPet.m_kMasterPos.Add(owner.transform.position);
        }
    }

    public void PursueTo(Vector3 pos)
    {
        m_kMovePoint = pos;
        m_kCurPos = owner.getPosition();

        owner.setFaceDir(m_kMovePoint - owner.transform.position);

        // 位置检测
        float run_dist = Vector3.Distance(m_kMovePoint, owner.transform.position);

        if (owner.GetProperty().getMoveSpeed() > 0)
        {
            // 在m_fMoveTime 内做位置插值
            m_fMoveTime = run_dist / owner.GetProperty().getMoveSpeed();

            owner.setVelocity(owner.getFaceDir() * owner.GetProperty().getMoveSpeed());
        }
    }

   
    public Vector3 Sample(float factor, Vector3 m_kTargetPos)
    {
        float val = Mathf.Clamp01(factor);

        Vector3 kRetPos = new Vector3();

        kRetPos = m_kTargetPos * val + m_kCurPos * (1.0f - val);

        return kRetPos;
    }
}
