using UnityEngine;
using System.Collections;

public class BornAppear : Appear {
	
	float born_time = 4.0f;
	Vector3 born_pos;
	bool first_update = true;
	
	public BornAppear()
    {
        battle_state = BATTLE_STATE.BS_BORN;
    }
	
	public override void active() 
    {
        animation_name = "birth";

		on_active();

        if (CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap())
        {
            if (owner.getType() == CharacterType.CT_MONSTER)
            {
                MonsterProperty property = (owner as CharacterMonster).monster_property;
                MessageManager.Instance.sendObjectBehavior((uint)property.getInstanceId(),
                (int)battle_state, owner.getFaceDir(), owner.getPosition());
                Debug.Log("battle battle state " + owner.getPosition());
            }
        //    else
        //    {
        //        MessageManager.Instance.sendObjectBehavior((uint)CharacterPlayer.character_property.getSeverID(),
        //        (int)battle_state, owner.getFaceDir(), owner.getPosition());
        //    }
        }
        
		
		
		//owner.skill.setHurtHide(true);
		
//		if (owner.getType() == CharacterType.CT_MONSTER) {
//			CharacterMonster m = (CharacterMonster)owner;
//			//boss borned.
//			int n32MonsterType = m.monster_property.getType();
//			if (n32MonsterType == 0)
//			{
//				Loger.Log("Boss " + m.monster_property.name + " borned");
//				UiFightMainMgr.Instance.ShowUiBossHPBar(m.monster_property);
//				UiFightMainMgr.Instance.SetUiBossHPBar(m.monster_property.hp_max, m.monster_property.hp_max);
//			}
//		}
		/*
		Transform shadow = null;
		Transform[] transformChildren = owner.GetComponentsInChildren<Transform>();
		foreach (Transform transformChild in transformChildren) {
			if (transformChild.gameObject.name == "shadow") {
		    	shadow = transformChild;
				break;
			}
		}
		if (shadow) {
			Renderer[] rendererChildren = shadow.GetComponentsInChildren<Renderer>();
			foreach (Renderer rendererChild in rendererChildren) {
				rendererChild.enabled = false;
			}
		}
		*/
	}
	
	public override void deActive() 
    {
        base.deActive();
	
		owner.skill.setHurtProtecting(false);
		owner.skill.setHurtHide(false);
		
		/*
		Transform shadow = null;
		Transform[] transformChildren = owner.GetComponentsInChildren<Transform>();
		foreach (Transform transformChild in transformChildren) {
			if (transformChild.gameObject.name == "shadow") {
		    	shadow = transformChild;
				break;
			}
		}
		if (shadow) {
			Renderer[] rendererChildren = shadow.GetComponentsInChildren<Renderer>();
			foreach (Renderer rendererChild in rendererChildren) {
				rendererChild.enabled = true;
			}
		}
		*/
	}
	
    //public override void update(float delta) {
		
    //    base.update(delta);
		
    //    if (first_update) 
    //    {
    //        //owner.playAnimation("birth");
    //        owner.setPositionLikeGod(born_pos);
    //        first_update = false;
    //    }
    //}
	
	public void setPosition(Vector3 pos) {
		born_pos = pos;
	}
}
