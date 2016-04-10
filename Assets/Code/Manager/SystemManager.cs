using UnityEngine;

public class SystemManager : MonoBehaviour 
{
	
	bool attack_down = false;
	bool skill_down = false;
	
	int skill_chongci = 0;
	int skill_xuanfengzhan = 0;
	int skill_xiguaijiafang = 0;
	int skill_tiaozhan = 0;
	
	private static SystemManager _instance = null;

	public static SystemManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = new SystemManager();
            return _instance;
        }
    }

    private SystemManager()
    {
    }
	
	
	public void setAttackDown() {
		attack_down = true;
	}
	
	public void setAttackUp() {
		attack_down = false;
	}
	
	public void setSkillDown() {
		skill_down = true;
	}
	
	public void setSkillUp() {
		skill_down = false;
	}
	
	public void useSkillChongCi() {
		skill_chongci = 2;
	}
	
	public void useSkillXuanFengZhan() {
		skill_xuanfengzhan = 2;
	}
	
	public void useSkillXiGuaiJiaFang() {
		skill_xiguaijiafang = 2;
	}
	
	public void useSkillTiaoZhan() {
		skill_tiaozhan = 2;
	}
	
	public bool getSkillChongCi() {
		return skill_chongci > 0;
	}
	
	public bool getSkillXuanFengZhan() {
		return skill_xuanfengzhan > 0;
	}
	
	public bool getSkillXiGuaiJiaFang() {
		return skill_xiguaijiafang > 0;
	}
	
	public bool getSkillTiaoZhan() {
		return skill_tiaozhan > 0;
	}
	
	public bool getAttackDown() {
		return attack_down;
	}
	
	public bool getSkillDown() {
		return skill_down;
	}

    public void setMusicStatus(bool value)
    {
        
    }

    public void setMusicEffectStatus(bool value)
    {

    }
	
	void LateUpdate() {
		
		if (skill_chongci >= 0)
			skill_chongci--;
		
		if (skill_xuanfengzhan >= 0)
			skill_xuanfengzhan--;
		
		if (skill_xiguaijiafang >= 0)
			skill_xiguaijiafang--;
		
		if (skill_tiaozhan >= 0)
			skill_tiaozhan--;
	}
}
