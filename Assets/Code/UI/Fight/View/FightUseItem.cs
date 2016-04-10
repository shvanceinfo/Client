using UnityEngine;
using System.Collections;
using manager;
using model;
using MVC.entrance.gate;
/// <summary>
/// 显示使用物品
/// </summary>
public class FightUseItem : MonoBehaviour {



    public int ItemId { get; set; }

    private UISprite _goldIcon;     //金币图标
    private UILabel _lblCount;      //数字显示
    private UISprite _mask;         //CD遮罩
    private bool isCdTime;          //CD中
    private float maxCdTime;        //最大CD时间
    private float curCdTime;        //当前CD时间
    private void Awake()
    {
        _goldIcon = transform.FindChild("GoldIcon").GetComponent<UISprite>();
        _lblCount = transform.FindChild("Label").GetComponent<UILabel>();
        _mask = transform.FindChild("Mask").GetComponent<UISprite>();
        maxCdTime=Constant.FIGHT_MAX_HHEALT_MAGIC_ITEM_CD;
        _goldIcon.alpha = 0;
    }

    //使用并显示UI
    public  void DisplayItemChange(int num,bool isShowGoldIcon=false)
    {
        if (isShowGoldIcon)
        {
            _goldIcon.alpha = 1;
        }
        else {
            _goldIcon.alpha = 0;
        }
        _lblCount.text = num.ToString();
        isCdTime = true;
        _mask.fillAmount = 1;
    }
    private void Start()
    {
        isCdTime = false;
        _mask.fillAmount = 0;
        curCdTime = 0;
    }
    public void Initial(int num)
    {
        _lblCount.text = num.ToString();
    }

    private void OnClick()
    {
        if (!isCdTime)
        {
            Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_USE_HHEAL_MAGIC_ITEM);
        }
    }

    private void Update()
    {
        if (isCdTime)
        {
            if (curCdTime>maxCdTime)
            {
                curCdTime = 0;
                isCdTime = false;
                _mask.fillAmount = 0;
                return;
            }
            curCdTime += Time.deltaTime;

            _mask.fillAmount = (maxCdTime - curCdTime) / maxCdTime;
        }
    }

}
