using UnityEngine;
using System.Collections;
using NetGame;
using System.Collections.Generic;

public class UiPalyerChange : MonoBehaviour {

    private GameObject lblNum;
    private GameObject packNotify;
    const int MAX_ITEMS = 40;
    private bool started = false;
    //void Awake()
    //{
        
    //}
	// Use this for initialization
	void Start () {
        SetRoleName();
	}
	
	// Update is called once per frame
	void Update () {
        if (!started)
        {
            ChangeAsset();
            ChangeFightProperty();
            ChangeLevel();
            ChangeProperty();
            started = true;
        }
	}

    void SetRoleName()
    {
        Transform roleName = transform.FindChild("lbl_role_name");
        if (roleName != null)
        {
            roleName.GetComponent<UILabel>().text = "[ffffff]" + CharacterPlayer.character_property.getNickName() + "[-]";
        }
    }

    /// <summary>
    /// 等级变化
    /// </summary>
    void ChangeLevel()
    {
        Transform level = transform.FindChild("lbl_role_level");
        if (level != null)
        {
            level.GetComponent<UILabel>().text = Global.FormatStrimg(LanguageManager.GetText("lbl_title_level"), CharacterPlayer.character_property.level.ToString());
        }
    }
    /// <summary>
    /// 资产变化
    /// </summary>
    void ChangeAsset()
    {
        Transform gole = transform.FindChild("man_title/gold/lbl_gold");
        Transform diamond = transform.FindChild("man_title/diamond/lbl_diamond");;

        if (gole != null)
        {
            string newStr = "[d9c8a8]" + CharacterPlayer.character_asset.gold + "[-]";
            if (gole.GetComponent<UILabel>().text != newStr)
            {
                gole.GetComponent<UILabel>().text = newStr;
                PlayAnimation(transform.FindChild("man_title/gold"));
            }            
        }
        if (diamond != null)
        {
            string newStr = "[d9c8a8]" + CharacterPlayer.character_asset.diamond + "[-]";
            if (diamond.GetComponent<UILabel>().text != newStr)
            {
                diamond.GetComponent<UILabel>().text = newStr;
                PlayAnimation(transform.FindChild("man_title/diamond"));
            }            
        }
    }
    /// <summary>
    /// 攻击数据
    /// </summary>
    /// <param name="player"></param>
    void ChangeFightProperty()
    {
        CharacterProperty player = CharacterPlayer.character_property;
        Transform life = transform.FindChild("lbl_life_base");
        Transform attk = transform.FindChild("lbl_attk_base");
        Transform def = transform.FindChild("lbl_def_base");
        if (life != null)
        {
            life.GetComponent<UILabel>().text = player.getHP().ToString();
        }
        if (attk != null)
        {
            attk.GetComponent<UILabel>().text = player.attack_power.ToString();
        }
        if (def != null)
        {
            def.GetComponent<UILabel>().text = player.defence.ToString();
        }
    }

    void OnEnable()
    {
        EventDispatcher.GetInstance().PlayerProperty += ChangeFightProperty;
        EventDispatcher.GetInstance().PlayerLevel += ChangeLevel;
        EventDispatcher.GetInstance().PlayerAsset += ChangeAsset;
        EventDispatcher.GetInstance().PlayerProperty += ChangeProperty;
    }

    void OnDisable()
    {
        EventDispatcher.GetInstance().PlayerProperty -= ChangeFightProperty;
        EventDispatcher.GetInstance().PlayerLevel -= ChangeLevel;
        EventDispatcher.GetInstance().PlayerAsset -= ChangeAsset;
        EventDispatcher.GetInstance().PlayerProperty -= ChangeProperty;
        started = false;
    }
    /// <summary>
    /// 播放变更数据动画
    /// </summary>
    /// <param name="obj"></param>
    void PlayAnimation(Transform obj)
    {
        if (started)
        {
            Animation target = null;
            if (target == null) target = obj.GetComponentInChildren<Animation>();

            if (target != null)
            {
                ActiveAnimation anim = ActiveAnimation.Play(target, AnimationOrTween.Direction.Reverse);
                if (anim == null) return;
                anim.Reset();
            }
        }
    }

    void ChangeProperty()
    {
        Transform expObj = transform.FindChild("man_title/exp");
        if (expObj != null)
        {
            int nextTempId = (int)CharacterPlayer.character_property.career * 10000 + CharacterPlayer.character_property.level;
            int upgradeExp = ConfigDataManager.GetInstance().getRoleConfig().getRoleData(nextTempId).upgrade_exp;
            expObj.FindChild("lbl_exp").GetComponent<UILabel>().text =
                Global.FormatStrimg(LanguageManager.GetText("lbl_title_exp"), CharacterPlayer.character_property.getExperience().ToString(), upgradeExp.ToString());
            expObj.FindChild("Slider").GetComponent<UISlider>().sliderValue = CharacterPlayer.character_property.getExperience() / (float)upgradeExp;
        }
        CharacterPlayer.character_property.getExperience();
    }
}
