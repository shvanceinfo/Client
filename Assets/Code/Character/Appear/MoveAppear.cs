using UnityEngine;
using System.Collections;

public class MoveAppear : Appear {
	
	protected Vector3 m_kMovePoint;
    public Vector3 m_kMoveDir;
	float m_fMoveTime = 0;

    public MoveAppear()
    {
        battle_state = BATTLE_STATE.BS_MOVE;
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
	}
	
	public override void update(float delta) 
    {
        // 判断是否穿越了
        float run_dist = Vector3.Distance(m_kMovePoint, owner.transform.position);
        
        Vector3 kOwnerToTargetDir = m_kMovePoint - owner.transform.position;

        float fDotValue = Vector3.Dot(kOwnerToTargetDir, owner.getFaceDir());

        if (fDotValue < 0 || run_dist < 0.01)
        {
            if (m_kMoveDir != Vector3.zero)
            {
                m_kMoveDir.Normalize();
                owner.setFaceDir(m_kMoveDir);
            }
            
            is_active = false;
            return;
        }

        // 开始移动
        float t = updateTime(delta);

        Vector3 kNewPt = owner.getPosition() + owner.getVelocity() * t;

        
        owner.setPosition(kNewPt);

        //发送移动消息同步给其他人
        if (owner.getType() == CharacterType.CT_PLAYER)
        {
            // 目前MoveAppear中只有主角用到 怪物是用寻路
            //MessageManager.Instance.sendMessageAskMove(CharacterPlayer.character_property.getSeverID(), owner.getFaceDir(), owner.getPosition());
            SyncManager.GetInstance().SendPlayerTranInfo();

			PlayerManager.Instance.CheckOtherAppear();
			PlayerManager.Instance.CheckOtherDisappear();
        }
        else if (owner.getType() == CharacterType.CT_MONSTER)
        {
            SyncManager.GetInstance().ReportMonsterTranInfo(owner as CharacterMonster);
        }
		else if (owner.getType() == CharacterType.CT_PLAYEROTHER)
		{
			// check is visible
//			if (owner.m_bVisible && !owner.m_bOutCam)
//			{
//				if (!GraphicsUtil.IsCharacterInsideCamera(owner, Camera.main))
//				{
//					Debug.Log("someone run out my view");
//					PlayerManager.Instance.OnPlayerDisAppear(owner as CharacterPlayerOther);
//				}
//			}
			PlayerManager.Instance.CheckOtherAppear();
			PlayerManager.Instance.CheckOtherDisappear();
		}

        if (owner.m_kPet != null)
        {
            owner.m_kPet.m_kMasterPos.Add(owner.transform.position);
        }

        m_fMoveTime -= t;

        if (m_fMoveTime <= 0)
        {
            if (m_kMoveDir != Vector3.zero)
            {
                m_kMoveDir.Normalize();
                owner.setFaceDir(m_kMoveDir);
            }
            
            is_active = false;
        }
	}
	
	public void moveTo(Vector3 pos) 
    {
        m_kMovePoint = pos;

        set_dir(m_kMovePoint - owner.transform.position);
        
        // 位置检测
        float run_dist = Vector3.Distance(m_kMovePoint, owner.transform.position);
		
        //// 判断是否穿越了
        Vector3 kOwnerToTargetDir = m_kMovePoint - owner.transform.position;

        float fDotValue = Vector3.Dot(kOwnerToTargetDir, owner.getFaceDir());

        if (fDotValue < 0 || run_dist < 0.01)
        {
            if (m_kMoveDir != Vector3.zero)
            {
                m_kMoveDir.Normalize();
                owner.setFaceDir(m_kMoveDir);
            }

            is_active = false;
            return;
        }
		

		if (owner.GetProperty().getMoveSpeed() > 0) 
			m_fMoveTime = run_dist / owner.GetProperty().getMoveSpeed();
		
		owner.setVelocity(owner.getFaceDir() * owner.GetProperty().getMoveSpeed());
	}
	
	void set_dir(Vector3 dir) 
    {
		dir.Normalize();
		owner.setFaceDir(dir);
	}
	
	public void moveAlong(Vector3 speed, float time) 
    {
		owner.setVelocity(speed);
		set_dir(speed);
		m_fMoveTime = time;
	}

    public static void InUnwalkableGridProcess(Vector3 kOwnerPos)
    {
        if (!AISystem.AStarAlgorithm.GetInstance().IsPositionWalkable(kOwnerPos))
        {
            // 单位所在点不是可行走区域
            int nIndexX = 0;
            int nIndexZ = 0;

            if (AISystem.AStarAlgorithm.GetInstance().GamePointToIndex(kOwnerPos, out nIndexX, out nIndexZ))
            {
                Vector3 kCenter = AISystem.AStarAlgorithm.GetInstance().GetPositionByIndexCoord(nIndexX, nIndexZ);

                Vector3 kBackDir = kOwnerPos - kCenter;
                kBackDir.Normalize();

                kOwnerPos += kBackDir * 0.01f;


                if (!AISystem.AStarAlgorithm.GetInstance().IsPositionWalkable(kOwnerPos))
                {
                    InUnwalkableGridProcess(kOwnerPos);
                }
            }
        }
    }
}
