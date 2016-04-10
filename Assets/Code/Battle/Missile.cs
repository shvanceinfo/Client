using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
	
	public float left_time = 10;
	public float speed = 0;
	public Vector3 dir = Vector3.zero;
	public SkillAppear skill;

	// Use this for initialization
	void Start () 
    {
        CharacterAnimCallback.ClearFightDirty(skill == null ? null : skill.getOwner());
	}
	
	// Update is called once per frame
	void Update () {
		
		if (MainLogic.sMainLogic.isGameSuspended()) 
        {
			return;
		}
	
		if (left_time > 0) 
        {
			left_time -= Time.deltaTime;

            if (!Global.inMultiFightMap())
            {
                transform.position += dir * speed * Time.deltaTime;
            }
            else
            {
                if (CharacterPlayer.character_property.getHostComputer())
                {
                    // 多人副本也需要把 发射的物件位置同步出去
                    transform.position += dir * speed * Time.deltaTime;
                    MessageManager.Instance.sendMessageAskMove((int)NetSyncObj.NSO_MEIDUSHA_OBJ, dir, transform.position);
                }
                else
                {
                    Vector3 synPt = BattleMultiPlay.GetInstance().UseNetSyncPos(NetSyncObj.NSO_MEIDUSHA_OBJ);

                    if (synPt != Vector3.zero)
                    {
                        transform.position = synPt;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
		}
		else 
        {
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other) 
    {
        Character target = other.gameObject.GetComponent<Character>();

        if (target == null || skill == null || !skill.getOwner())
        {
            return;
        }

        if (CharacterAI.IsInState(target, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            // 死了不能鞭尸
            return;
        }

        // 自己的飞翔类物体不能伤自己
        if (target == skill.getOwner())
        {
            return;
        }
        
        // 同类目前不能伤同类
        if (skill.getOwner().getType() == target.getType())
        {
            return;
        }


        // 对象已死
        if (CharacterAI.IsInState(target, CharacterAI.CHARACTER_STATE.CS_DIE))
        {
            return;
        }

        if (!BattleMultiPlay.GetInstance().CollisionJudegeValid(skill.getOwner(), target))
        {
            return;
        }

		// add camera shake
		if (skill.getOwner().getType() == CharacterType.CT_PLAYER)
			CameraFollow.sCameraFollow.ShakeBegin(ECameraShakeType.CST_FIGHT_SHAKE, 0.025f);


        BattleManager.Instance.SkillCastProcess(skill, target, CollisionUtil.CalculateJiGuanHitPoint(target, gameObject));

        Destroy(gameObject);
    }
}
