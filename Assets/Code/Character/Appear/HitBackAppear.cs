using UnityEngine;
using System.Collections;

public class HitBackAppear : Appear {

	Vector3 hit_back_dir = Vector3.forward;
	
	float hit_back_speed = 0;
	float speed_rate = 1.0f;
	float hurt_time;

    public HitBackAppear()
    {
        battle_state = BATTLE_STATE.BS_BE_HIT_BACK;
    }
	
	public override void init() 
    {

        
	}
	
	public override void active() 
    {
        hit_back_dir.Normalize();

		hit_back_speed = 0.0f;

		if (owner.getType() == CharacterType.CT_MONSTER) 
        {
			CharacterMonster monster = (CharacterMonster)owner;
			hit_back_speed = monster.monster_property.getRepelSpeed();
		}

        animation_name = "hurt";
		hurt_time = owner.animation["hurt"].length;
		owner.setAnimationTime("hurt",hurt_time);
		on_active(hurt_time);
		
        //if (Global.inMultiFightMap()) 
        //{
        //    if (owner.getType() == CharacterType.CT_PLAYER) 
        //    {
        //        MessageManager.Instance.sendObjectBehavior((uint)CharacterPlayer.character_property.getSeverID(),
        //            (int)battle_state, hit_back_dir, CharacterPlayer.sPlayerMe.getPosition());
        //    }
        //    else if (owner.getType() == CharacterType.CT_MONSTER && CharacterPlayer.character_property.getHostComputer()) 
        //    {
        //        CharacterMonster theMonster = (CharacterMonster)owner;
        //        MessageManager.Instance.sendObjectBehavior((uint)theMonster.monster_property.getInstanceId(),
        //            (int)battle_state, hit_back_dir, theMonster.getPosition());
        //    }
        //    else if (owner.getType() == CharacterType.CT_PLAYEROTHER)
        //    {
        //        CharacterPlayerOther other = (CharacterPlayerOther)owner;
        //        MessageManager.Instance.sendObjectBehavior((uint)other.character_other_property.getSeverID(),
        //            (int)battle_state, hit_back_dir, other.getPosition());
        //    }
        //}
	}
	
	public override void update(float delta) 
    {
		float t = updateTime(delta);
			
		owner.movePosition(hit_back_dir * t * hit_back_speed * speed_rate);
	}
	
	public void setDir(Vector3 dir) 
    {
		
		hit_back_dir = dir;
		hit_back_dir.y = 0;
		owner.setFaceDir(-hit_back_dir);
	}
	
	public void setSpeedRate(float rate) 
    {
		speed_rate = rate;
	}	
	
}
