using UnityEngine;
using System.Collections;
using System;

public class BtnUseHP : MonoBehaviour {
    private UISprite sprColedown;
    private float timeOut = 1f;
    private UILabel lblTime;
    private UILabel lblCurHp;
    private UISlider hpSlider;

    void OnEnable()
    {
        timeOut = 0;
        sprColedown.fillAmount = 0f;
    }

    void Awake()
    {
        EventDispatcher.GetInstance().PlayerProperty += OnPlayerProperty;
        sprColedown = transform.FindChild("hp").FindChild("coledown").GetComponent<UISprite>();
        lblTime = transform.FindChild("hp").FindChild("time").GetComponent<UILabel>();
        lblCurHp = transform.FindChild("hp").FindChild("lbl_num").GetComponent<UILabel>();
        hpSlider = transform.FindChild("hp").GetComponent<UISlider>();

    }

	// Use this for initialization
	void Start () {
        timeOut = 0f;
        lblTime.gameObject.SetActive(false);
        OnPlayerProperty();
	}
	
	// Update is called once per frame
	void Update () {
        if (timeOut > 0)
        {
            timeOut -= Time.deltaTime;
            sprColedown.fillAmount = timeOut / Global.drupCoolDownTime;
            //lblTime.text = Math.Round(timeOut, 0).ToString();
            if (timeOut <= 0)
            {
                sprColedown.fillAmount = 0f;
                PlayAnimation();
                //lblTime.gameObject.SetActive(false);
            }
        }
        else if (lblTime.gameObject.activeSelf)        
        {
            sprColedown.fillAmount = 0;
            //lblTime.gameObject.SetActive(false);
        }
	}

    void PlayAnimation()
    {
        Animation target = null;
       if (target == null) target = GetComponentInChildren<Animation>();

       if (target != null)
       {
           ActiveAnimation anim = ActiveAnimation.Play(target, AnimationOrTween.Direction.Reverse);
           if (anim == null) return;
           anim.Reset();
       }
    }

    void OnClick()
    {
        if (sprColedown.fillAmount == 0)
        {    
            // 死亡
            if (CharacterAI.IsInState(CharacterPlayer.sPlayerMe, CharacterAI.CHARACTER_STATE.CS_DIE))
            {
                return;
            }
            
            if (CharacterPlayer.character_property.UseHP())
            {
                sprColedown.fillAmount = 1f;
                timeOut = Global.drupCoolDownTime;
                //lblTime.text = Global.drupCoolDownTime.ToString();
                //lblTime.gameObject.SetActive(true);
                EventDispatcher.GetInstance().OnPlayerProperty();
            }            
        }
        else if (CharacterPlayer.character_property.cur_hp_vessel > 0)
        {
            FloatMessage.GetInstance().PlayFloatMessage(LanguageManager.GetText("fight_hp_cd"), UIManager.Instance.getRootTrans(), Vector3.zero, Vector3.zero);
        }
    }

    void OnPlayerProperty()
    {
		if (null != CharacterPlayer.character_property)
		{
			lblCurHp.text = CharacterPlayer.character_property.getCurHPVessel().ToString();
            //float total = (float)CharacterPlayer.character_property.fightProperty.GetValue[eFighintPropertyCate.eFPC_MaxHP];
        	hpSlider.sliderValue = (float)CharacterPlayer.character_property.cur_hp_vessel / CharacterPlayer.character_property.getMaxHPVessel();
		}
    }
}
