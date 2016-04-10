using UnityEngine;
using System.Collections;
using MVC.entrance.gate;

/// <summary>
/// 声明：只用来显示，不更改数据
/// 用来显示技能点击，计算CD
/// </summary>
public class FightSkillItem : MonoBehaviour
{
    public int SkillId { get; set; }     //技能ID
    public int MaxCdTime { get; set; }   //CD时间
    public bool IsLock { get; set; }     //技能是否被锁定
    private float curCdTime=0;           //用来计算CD时间
    private bool isCdTime;               //是否在CD中
    private UISprite sp_background;
    private UITexture sp_icon;
    private UISprite sp_isLock;
    private UISprite sp_mask;
    private ParticleSystem _system;
    private void Awake()
    {
        MaxCdTime = 0;
        SkillId = 0;
        IsLock = true ;
        sp_background = transform.Find("Background").GetComponent<UISprite>();
        sp_icon = transform.Find("Sprite").GetComponent<UITexture>();
        sp_isLock = transform.Find("IsLock").GetComponent<UISprite>();
        sp_mask = transform.Find("Mask").GetComponent<UISprite>();
        GameObject obj = BundleMemManager.Instance.getPrefabByName(PathConst.SKILL_CD_PARTICLE,
            EBundleType.eBundleBattleEffect);
        GameObject realObj = BundleMemManager.Instance.instantiateObj(obj);
        realObj.transform.parent = transform;
        realObj.transform.localPosition = Vector3.zero;
        realObj.transform.localScale = Vector3.one;
        _system = realObj.GetComponentInChildren<ParticleSystem>();
        _system.transform.localScale = new Vector3(550, 550, 1);
		_system.gameObject.SetActive(false);
        _system.Stop();
    }
    private void Start()
    {
       
    }

    private void OnClick()
    {
        if (!IsLock)        //如果不是锁定
        {
            if (!isCdTime)      //如果不在CD中
            {
                if (CharacterPlayer.sPlayerMe.CanCastSkill(SkillId) == SkillCanNotCastReason.SCNCR_CAST &&
                    !CharacterAI.IsInState(CharacterPlayer.sPlayerMe, CharacterAI.CHARACTER_STATE.CS_DIZZY) &&
                    !Global.m_bCameraCruise)
                {
                    
                    isCdTime = true;
                    Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_SEND_USE_SKILL, SkillId);
                }
            }
        }
        
    }
    private void Update()
    {
        if (!IsLock)
        {
            if (isCdTime)
            {
                if (curCdTime > MaxCdTime)
                {
                    isCdTime = false;
                    curCdTime = 0;
                    sp_mask.fillAmount = 0;
					if (!_system.gameObject.activeSelf) {
						_system.gameObject.SetActive(true);
					}
                    _system.Play();
                    return;
                }
                curCdTime += Time.deltaTime;
                float fix = (MaxCdTime - curCdTime) / MaxCdTime;
                sp_mask.fillAmount = fix;
            } 
        }
    }

    public void DisplaySkill(int id,int cdtime,CHARACTER_CAREER type, string skillIcon)
    {

        SkillId = id;
        MaxCdTime = cdtime/1000;    //使用秒计时
        sp_background.spriteName = Constant.Fight_No_Lock;
        sp_isLock.alpha = 0;
        sp_mask.alpha = 1;
        sp_mask.fillAmount = 0;
        sp_icon.alpha = 1;
        sp_icon.mainTexture = SourceManager.Instance.getTextByIconName(skillIcon,PathConst.SKILL_PATH);
        IsLock = false;
        
    }

    //设置技能为锁上
    public void DisplaySkillIsLock()
    {
        sp_background.spriteName = Constant.Fight_Is_Lock;
        sp_isLock.alpha = 1;
        sp_mask.alpha = 0;
        sp_mask.fillAmount = 0;
        sp_icon.alpha = 0;
        isCdTime = false;
        IsLock = true;
    }




}
