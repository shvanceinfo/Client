using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillAnimationShowAppear : Appear
{
	int skill_id = -1;
    float show_time;
	string show_name;
	float show_speed;
	string effect_name;
	
	//位移技能用
	List<Vector3> move_to = null;

    public override void init()
    {
        move_to = new List<Vector3>();
    }

    public override void active()
    {
        //owner.crossFadeAnimation(show_name, 0.1f);
        on_active(show_time);
		
        // 旋风咱10010
		if (skill_id == 10010) {
            EffectManager.Instance.createFX(effect_name, owner.transform, show_time);
		}
    }

    public override void deActive()
    {
        base.deActive();
		skill_id = -1;
    }

    public override void update(float delta)
    {
        float t = updateTime(delta);
		
		if (skill_id == -1) return;
		
		if (skill_id == (int)10021) {
			
			if (move_to.Count > 0) {
				Vector3 movePos = move_to[0];
				
				float dist = Vector3.Distance(movePos, owner.transform.position);
				if (dist <= show_speed * t) {
					owner.setPosition(movePos);
					move_to.Clear();
				}
				else {
					Vector3 moveDir = movePos - owner.transform.position;
					moveDir.Normalize();
					owner.setFaceDir(moveDir);
					owner.movePosition(owner.getFaceDir() * t * show_speed);
				}
			}
		}
		else {
			owner.movePosition(owner.getFaceDir() * t * show_speed);
		}
    }
	
	public int getCurrentSkillId() {
		return skill_id;
	}
	
	public bool showSkill(int id) {
		
		skill_id = (int)id;

        SkillExpressionDataItem sdi = ConfigDataManager.GetInstance().getSkillExpressionConfig().getSkillExpressionData(SkillAppear.ConvertLevelIDToSingleID(id));
		show_speed = sdi.moveSpeed;

        show_name = sdi.skillAction;

        if (show_name != null)
        {
            show_time = owner.animation[show_name].length;
        }
        
        
		
        //switch (id) {
        //case SkillAppear.SkillType.ST_PLAYER_COMMON_1:
        //    {
        //        show_name = "attack1";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_PLAYER_COMMON_2:
        //    {
        //        show_name = "attack2";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_PLAYER_COMMON_3:
        //    {
        //        show_name = "attack3";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_PLAYER_COMMON_4:
        //    {
        //        show_name = "attack4";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_PLAYER_COMMON_5:
        //    {
        //        show_name = "attack5";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_PLAYER_COMMON_6:
        //    {
        //        show_name = "attack6";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_PLAYER_COMMON_7:
        //    {
        //        show_name = "attack7";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_PLAYER_COMMON_8:
        //    {
        //        show_name = "attack8";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_PLAYER_COMMON_9:
        //    {
        //        show_name = "attack9";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_PLAYER_COMMON_10:
        //    {
        //        show_name = "attack10";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_PLAYER_COMMON_11:
        //    {
        //        show_name = "attack11";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
			
        //case SkillAppear.SkillType.ST_ROLL:
        //    {
        //        show_name = "tumble";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_BIG_SWORD_DASH_1:
        //    {
        //        show_name = "chongzhuang1";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_BIG_SWORD_DASH_2:
        //    {
        //        show_name = "chongzhuang2";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_WHIRL_WIND:
        //    {
        //        show_name = "xuanfengzhan";
        //        show_time = 5.0f;
        //        effect_name = "Effect/Effect_Prefab/Role/Skill/xuanfengzhan";
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_ABSORB:
        //    {
        //        show_name = "xiguaijiafang";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //case SkillAppear.SkillType.ST_SMASH:
        //    {
        //        show_name = "tiaozhan";
        //        show_time = owner.animation[show_name].length;
        //    }
        //    break;
        //default:
        //    return false;
        //}
		
		return true;
	}
	
	public void skillMoveTo(Vector3 pos) {
		
		move_to.Clear();
		move_to.Add(pos);
	}
}