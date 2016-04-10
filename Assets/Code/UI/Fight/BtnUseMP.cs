using UnityEngine;
using System.Collections;
using System;

public class BtnUseMP : MonoBehaviour {
    private UISprite sprColedown;
    private float timeOut = 1f;
    private UILabel lblTime;

    private UILabel lblCurMp;
    private UISlider mpSlider;

    void OnEnable()
    {
        timeOut = 0;
        sprColedown.fillAmount = 0f;
    }

    void Awake()
    {
        EventDispatcher.GetInstance().PlayerProperty += OnPlayerProperty;
        sprColedown = transform.FindChild("mp").FindChild("coledown").GetComponent<UISprite>();
        lblTime = transform.FindChild("mp").FindChild("time").GetComponent<UILabel>();
        lblCurMp = transform.FindChild("mp").FindChild("lbl_num").GetComponent<UILabel>();
        mpSlider = transform.FindChild("mp").GetComponent<UISlider>();
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
            if (CharacterPlayer.character_property.UseMP())
            {
                sprColedown.fillAmount = 1f;
                timeOut = Global.drupCoolDownTime;
                //lblTime.text = Global.drupCoolDownTime.ToString();
                //lblTime.gameObject.SetActive(true);
                EventDispatcher.GetInstance().OnPlayerProperty();
            }
        }
        else if (CharacterPlayer.character_property.cur_mp_vessel > 0)
        {
            FloatMessage.GetInstance().PlayFloatMessage(LanguageManager.GetText("fight_mp_cd"), UIManager.Instance.getRootTrans(), Vector3.zero, Vector3.zero);
        }

    }

    void OnPlayerProperty()
    {
        lblCurMp.text = CharacterPlayer.character_property.getCurMPVessel().ToString();
        //mpSlider.sliderValue = (float)CharacterPlayer.character_property.cur_mp_vessel / (float)CharacterPlayer.character_property.fightProperty.GetValue[eFighintPropertyCate.eFPC_MaxMP];
        mpSlider.sliderValue = (float)CharacterPlayer.character_property.cur_mp_vessel / CharacterPlayer.character_property.getMaxMPVessel();
    }
}
