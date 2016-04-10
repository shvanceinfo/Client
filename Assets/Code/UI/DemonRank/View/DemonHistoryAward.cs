using UnityEngine;
using System.Collections;
using model;
using helper;

/// <summary>
/// 显示昨日奖励
/// </summary>
public class DemonHistoryAward : MonoBehaviour {

    const string GATE = "昨日挑战:";
    const string RANK = "排名:";
    //lvl1
    private UILabel _error1;
    private UILabel _gate1;
    private UILabel _rank1;
    private UITexture _icon1;
    private Transform _btn1;        //领取
    private Transform _btnGate1;        //以领取
    //lvl2
    private UILabel _error2;
    private UILabel _gate2;
    private UILabel _rank2;
    private UITexture _icon2;
    private Transform _btn2;
    private Transform _btnGate2;        //以领取
    //lvl3
    private UILabel _error3;
    private UILabel _gate3;
    private UILabel _rank3;
    private UITexture _icon3;
    private Transform _btn3;
    private Transform _btnGate3;        //以领取
    private void Awake()
    {
        _error1 = transform.FindChild("Level1/error").GetComponent<UILabel>();
        _gate1 = transform.FindChild("Level1/reLevel").GetComponent<UILabel>();
        _rank1 = transform.FindChild("Level1/rank").GetComponent<UILabel>();
        _icon1 = transform.FindChild("Level1/icon").GetComponent<UITexture>();
        _btn1 = transform.FindChild("Level1/Button_Reward");
        _btnGate1 = transform.FindChild("Level1/Button_Gate");

        _error2 = transform.FindChild("Level2/error").GetComponent<UILabel>();
        _gate2 = transform.FindChild("Level2/reLevel").GetComponent<UILabel>();
        _rank2 = transform.FindChild("Level2/rank").GetComponent<UILabel>();
        _icon2 = transform.FindChild("Level2/icon").GetComponent<UITexture>();
        _btn2 = transform.FindChild("Level2/Button_Reward");
        _btnGate2 = transform.FindChild("Level1/Button_Gate");

        _error3 = transform.FindChild("Level3/error").GetComponent<UILabel>();
        _gate3 = transform.FindChild("Level3/reLevel").GetComponent<UILabel>();
        _rank3 = transform.FindChild("Level3/rank").GetComponent<UILabel>();
        _icon3 = transform.FindChild("Level3/icon").GetComponent<UITexture>();
        _btn3 = transform.FindChild("Level3/Button_Reward");
        _btnGate3 = transform.FindChild("Level1/Button_Gate");
    }

    public void DisPlayLvl(DemonDiffEnum lvl,HistoryRankVo vo)
    {
        switch (lvl)
        {
            case DemonDiffEnum.Level1:
                _error1.alpha = 0;
                _gate1.text = ColorConst.Format(ColorConst.Color_Juhuang, GATE + vo.Gate);
                _rank1.text = ColorConst.Format(ColorConst.Color_Juhuang, RANK + vo.Id);
                _icon1.mainTexture = SourceManager.Instance.getTextByIconName(vo.AwardIcon);
                _btn1.gameObject.SetActive(!vo.IsReceive);
                _btnGate1.gameObject.SetActive(vo.IsReceive);

                _gate1.alpha = 1;
                _rank1.alpha = 1;
                _icon1.alpha = 1;
                break;
            case DemonDiffEnum.Level2:
                _error2.alpha = 0;
                _gate2.text = ColorConst.Format(ColorConst.Color_Juhuang, GATE + vo.Gate);
                _rank2.text = ColorConst.Format(ColorConst.Color_Juhuang, RANK + vo.Id);
                _icon2.mainTexture = SourceManager.Instance.getTextByIconName(vo.AwardIcon);
                _btn2.gameObject.SetActive(!vo.IsReceive);
                _btnGate2.gameObject.SetActive(vo.IsReceive);

                _gate2.alpha = 1;
                _rank2.alpha = 1;
                _icon2.alpha = 1;
                break;
            case DemonDiffEnum.Level3:
                _error3.alpha = 0;
                _gate3.text = ColorConst.Format(ColorConst.Color_Juhuang, GATE + vo.Gate);
                _rank3.text = ColorConst.Format(ColorConst.Color_Juhuang, RANK + vo.Id);
                _icon3.mainTexture = SourceManager.Instance.getTextByIconName(vo.AwardIcon);
                _btn3.gameObject.SetActive(!vo.IsReceive);
                _btnGate3.gameObject.SetActive(vo.IsReceive);

                _gate3.alpha = 1;
                _rank3.alpha = 1;
                _icon3.alpha = 1;
                break;
        }
    }


    public void DisPlayNone(DemonDiffEnum lvl)
    {
        switch (lvl)
        {
            case DemonDiffEnum.Level1:
                _error1.alpha = 1;
                _btn1.gameObject.SetActive(false);
                _btnGate1.gameObject.SetActive(false);
                _gate1.alpha = 0;
                _rank1.alpha = 0;
                _icon1.alpha = 0;
                break;
            case DemonDiffEnum.Level2:
               _error2.alpha = 1;
                _btn2.gameObject.SetActive(false);
                _btnGate2.gameObject.SetActive(false);
                _gate2.alpha = 0;
                _rank2.alpha = 0;
                _icon2.alpha = 0;
                break;
            case DemonDiffEnum.Level3:
               _error3.alpha = 1;
                _btn3.gameObject.SetActive(false);
                _btnGate3.gameObject.SetActive(false);
                _gate3.alpha = 0;
                _rank3.alpha = 0;
                _icon3.alpha = 0;
                break;
        }
    }
}
