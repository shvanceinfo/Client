using UnityEngine;
using System.Collections;

public class SkillWhirlWind : SkillAppear
{
    // 用来控制给服务器发其他人技能的同步消息间隔
    float move_msg_interval = -1;

    float m_fWindTime = 0.0f;
	
	AudioClip clip;

    public SkillWhirlWind(int nSkillId) : base(nSkillId)
    {

    }

    public override void init()
    {
        loadConfig();
        if (BundleMemManager.debugVersion)
        {
            clip = BundleMemManager.Instance.loadResource(PathConst.AUDIO_XUANFENGZHAN) as AudioClip;
            if (owner.audio)
                owner.audio.clip = clip;
        }
        else
        {
            BundleMemManager.Instance.loadPrefabViaWWW<AudioClip>(EBundleType.eBundleMusic, PathConst.AUDIO_XUANFENGZHAN,
                (obj) =>
                {
                    clip = obj as AudioClip;
                    if (owner.audio)
                        owner.audio.clip = clip;
                });
        }
    }

    public override void deActive()
    {
        base.deActive();

        
		owner.skill.setCurrentSkill(null);
		
		if (owner.audio) 
        {
            owner.audio.Stop();
		}
    }

    public override void update(float delta)
    {
        if (m_fWindTime > time_length)
        {
            owner.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
        }


        m_fWindTime += delta;

        float t = updateTime(delta);
		
		if (move_msg_interval > 0) 
        {
			move_msg_interval -= Time.deltaTime;
		}
        
		bool manualMove = false;
		
		if (Input.GetButton("Fire1"))
		{
            if (Global.pressOnUI() || Global.InArena())
                return;
			
			manualMove = true;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        RaycastHit hit;
			LayerMask mask = 1<<LayerMask.NameToLayer("Floor");
	        if (Physics.Raycast(ray, out hit, 1000, mask)) 
            {
				
				Vector3 moveToPos = hit.point;
				moveToPos.y = 0;
                Character monster = MonsterManager.Instance.GetPointNearestMonster(moveToPos, 1.4f);
                if (monster) 
                {
					CharacterPlayer.sPlayerMe.skill.setSkillTarget(monster);
                    //BattleManager.Instance.hideMoveTarget();
				}
				else
				{
                    Vector3 moveDir = moveToPos - owner.transform.position;
                    moveDir.Normalize();
                    if (moveDir.magnitude > 0.1f)
                    {
                        owner.movePosition(moveDir * t * m_kSkillInfo.moveSpeed);
                        owner.setFaceDir(moveDir);
                        if (move_msg_interval < 0)
                        {
                            if (owner.getType() == CharacterType.CT_PLAYER)
                            {
                                MessageManager.Instance.sendMessageAskMove(
                                    CharacterPlayer.character_property.getSeverID(), owner.getFaceDir(), owner.getPosition());
                            }
                            move_msg_interval = 0.2f;
                        }
                    }
                }
			}	
		} 
		else 
        {
			move_msg_interval = -1;
		}
		
		if (!manualMove) 
        {
			if (owner.skill.getSkillTarget()) 
            {
				Vector3 dir = owner.skill.getSkillTarget().getPosition() - owner.getPosition();
				dir.Normalize();
				if (dir.magnitude > 0.1f) 
                {
                    owner.movePosition(dir * t * m_kSkillInfo.moveSpeed);
					owner.setFaceDir(dir);
					if (move_msg_interval < 0) 
                    {
						if (owner.getType() == CharacterType.CT_PLAYER) 
                        {
							MessageManager.Instance.sendMessageAskMove(
								CharacterPlayer.character_property.getSeverID(), owner.getFaceDir(), owner.getPosition());
						}
						move_msg_interval = 0.2f;
					}
				}
			}
		}
		
		if (owner.audio) 
        {
			if (!owner.audio.isPlaying) 
            {
				owner.audio.Play();
			}
		}


        
    }
}
