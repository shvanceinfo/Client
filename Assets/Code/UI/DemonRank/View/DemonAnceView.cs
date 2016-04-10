using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using helper;
using manager;
using model;

public class DemonAnceView : MonoBehaviour {

    public enum DemonArceType
    { 
        Death,      //死亡
        NextStop,   //等级不足
        Perfect     //通关
    }

    const string WIN_SP = "tongguan_wanmei";
    const string LOSE_SP = "tongguan_zaijiezaili";
    const string GATESTR = "[{0}]{1}[-] [{2}]{3}[-] [{4}]层[-]";     //颜色，diff，颜色2，gate，颜色3
    const string NEXTSTR = "[{0}]需要主角达到[-] [{1}]{2}[-] [{3}]级[-]";
    const string STOPSTR = "[{0}]暂无新挑战[-]";

    private UISprite _title;        //标题
    private UILabel _gate;          //当前层数
    private UILabel _nextLevel;     //需求等级
    private UILabel _time;          //倒计时
    private UILabel _tip;           //tip

    private GameObject grid;
    private GameObject itemPrefab;

    private void Awake()
    {
        _title = transform.FindChild("Title").GetComponent<UISprite>();
        _gate = transform.FindChild("Gate/LabelCt").GetComponent<UILabel>();
        _nextLevel = transform.FindChild("Info/LabelCt").GetComponent<UILabel>();
        _time = transform.FindChild("ReTime/Time").GetComponent<UILabel>();
        _tip = transform.FindChild("Tips/Label").GetComponent<UILabel>();
        grid = transform.FindChild("Tips/panel").gameObject;
        itemPrefab = transform.FindChild("Tips/Item").gameObject;
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        Gate.instance.registerMediator(new DemonAnceMediator(this));
    }

    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.DEMON_ANCE_MEDIATOR);
    }



    /// <summary>
    /// 死亡界面
    /// </summary>
    private void Display_DeadPage()
    {
        DemonVo vo = DemonManager.Instance.GetDemonVoById(Global.cur_TowerId);
        string diff = DemonManager.GetLevelString((DemonDiffEnum)vo.Diff);
        _title.spriteName = LOSE_SP;
        _title.MakePixelPerfect();

        _gate.text = string.Format(GATESTR,
            ColorConst.Color_HeSe,
            diff,
            ColorConst.Color_Green,
            vo.Level.ToString(),
            ColorConst.Color_HeSe);

        _nextLevel.text = string.Format(NEXTSTR, ColorConst.Color_HeSe,
            ColorConst.Color_Green,
            vo.UnLockLevel,
            ColorConst.Color_HeSe);

        _tip.text = ViewHelper.GetTips();

        //设置倒计时
        StartCoroutine(LoseTime(DeathManager.Instance.ToCityTime));
    }
    /// <summary>
    /// 通关
    /// </summary>
    private void Display_WinPage()
    {

        DemonVo vo = DemonManager.Instance.GetDemonVoById(Global.cur_TowerId);
        string diff = DemonManager.GetLevelString((DemonDiffEnum)vo.Diff);
        _title.spriteName = WIN_SP;
        _title.MakePixelPerfect();

        _gate.text = string.Format(GATESTR,
            ColorConst.Color_HeSe,
            diff,
            ColorConst.Color_Green,
            vo.Level.ToString(),
            ColorConst.Color_HeSe);

        _nextLevel.text = string.Format(STOPSTR, ColorConst.Color_HeSe);

        _tip.text = ViewHelper.GetTips();

        //设置倒计时
        StartCoroutine(LoseTime(DeathManager.Instance.ToCityTime));
    }
    /// <summary>
    /// 等级不足
    /// </summary>
    private void Display_LevelNotEnoughPage()
    {
        DemonVo vo=DemonManager.Instance.GetDemonVoById(Global.cur_TowerId);
        string diff= DemonManager.GetLevelString((DemonDiffEnum)vo.Diff);
        _title.spriteName = WIN_SP;
        _title.MakePixelPerfect();

        _gate.text = string.Format(GATESTR,
            ColorConst.Color_HeSe,
            diff,
            ColorConst.Color_Green,
            vo.Level.ToString(),
            ColorConst.Color_HeSe);

        _nextLevel.text = string.Format(NEXTSTR, ColorConst.Color_HeSe,
            ColorConst.Color_Green,
            vo.UnLockLevel,
            ColorConst.Color_HeSe);

        _tip.text = ViewHelper.GetTips();

        //设置倒计时
        StartCoroutine(LoseTime(DeathManager.Instance.ToCityTime));
    }


    private IEnumerator LoseTime(int t)
    {
        _time.text = t.ToString();
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (t < 0)
            {
                DeathManager.Instance.ToCity();
                yield break;
            }
            _time.text = t.ToString();
            t--;
        }

    }

    private void DisplayAwardInfo(ItemStruct item,int index)
    {
        itemPrefab.SetActive(true);
        GameObject obj = Instantiate(itemPrefab) as GameObject;
        obj.transform.name = index.ToString();
        obj.transform.parent = grid.transform;
        obj.transform.localPosition = new Vector3();
        obj.transform.localScale = new Vector3(1,1,1);
        obj.transform.FindChild("border").GetComponent<UISprite>().spriteName = ViewHelper.GetBoderById((int)item.tempId);
        string iconName = AwardManager.Instance.GetTemplateByTempId(item.tempId).icon;
        obj.transform.FindChild("icon").GetComponent<UITexture>().mainTexture = SourceManager.Instance.getTextByIconName(iconName);
        obj.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format("x{0}", item.num);
        itemPrefab.SetActive(false);
        grid.GetComponent<UIGrid>().Reposition();
    }

    private void RefreshAward()
    {
        for (int i = 0; i < AwardManager.Instance.Vo.CurAwardItemList.size; i++)
        {
            DisplayAwardInfo(AwardManager.Instance.Vo.CurAwardItemList[i], i);
        }
        if (AwardManager.Instance.Vo.CurAwardItemList.size == 0)
            transform.FindChild("Tips/Label0").gameObject.SetActive(true);
        else
            transform.FindChild("Tips/Label0").gameObject.SetActive(false);

    }


    public void DisplaySwtingPage(DemonArceType table)
    {
        switch (table)
        {
            case DemonArceType.Death: 
                Display_DeadPage();
                RefreshAward();
                break;
            case DemonArceType.NextStop: 
                Display_LevelNotEnoughPage();
                RefreshAward();
                break;
            case DemonArceType.Perfect: 
                Display_WinPage();
                RefreshAward();
                break;
            default:
                break;
        }
    }
}
