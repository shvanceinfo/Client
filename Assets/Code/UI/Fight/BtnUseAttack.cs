using UnityEngine;
using System.Collections;
using System;

public class BtnUseAttack : MonoBehaviour 
{
    
    private UISprite skillIcon;

	// Use this for initialization
	void Start () 
    {
        Init();
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}
    
    
    public void SetSkill()
    {
        Init();
        int id = CharacterPlayer.sPlayerMe.skill.GetCommonAttack().skill_id;
        skillIcon.spriteName = ConfigDataManager.GetInstance().getSkillConfig().getSkillData(id).icon;
    }

    private void OnPress(bool press)
    {
        // 攻击键按着不动 进入强制攻击状态
        CharacterPlayer.sPlayerMe.GetComponent<FightProperty>().m_bForceAttack = press;

        if (press)
        {
            if (CharacterPlayer.sPlayerMe.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_ATTACK ||
            CharacterPlayer.sPlayerMe.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_SKILL)
            {
                EventDispatcher.GetInstance().OnAttackActived();
            }
            else
                CharacterPlayer.sPlayerMe.UseAction();
        }
    }


    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        skillIcon = transform.FindChild("icon").GetComponent<UISprite>();
    }
}
