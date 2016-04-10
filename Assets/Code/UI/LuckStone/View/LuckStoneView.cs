using UnityEngine;
using System.Collections;
using helper;
using manager;
using model;
using System;
using MVC.entrance.gate;
using mediator;
public class LuckStoneView : MonoBehaviour {

    private GameObject _prefab;
    private Transform _grid;

    private UILabel _diamon;
    private UILabel _fushi;
    private void Awake()
    {
        _prefab = transform.FindChild("PanelList/Item").gameObject;
        _grid = transform.FindChild("PanelList/Grid");
        _diamon = transform.FindChild("Diamon/Label").GetComponent<UILabel>();
        _fushi = transform.FindChild("Fushi/Label").GetComponent<UILabel>();
    }
    private void Start()
    {
        DisplayList();
        DisplayMoney();
    }
    private void OnEnable()
    {
        Gate.instance.registerMediator(new LuckStoneMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.LUCKSTONE_MEDIATOR);
    }

    public void DisplayMoney()
    {
        _diamon.text = CharacterPlayer.character_asset.diamond.ToString();
        _fushi.text = LuckStoneManager.Instance.GetConsumeItem().ToString();
    }

    public void DisplayList()
    {
        BetterList<LuckStoneVo> data= LuckStoneManager.Instance.SortLuckStones;
        ViewHelper.FormatTemplate<BetterList<LuckStoneVo>,
            LuckStoneVo,
            LuckStoneDisplayItem>(_prefab, _grid, data,
            (LuckStoneVo vo, LuckStoneDisplayItem dp) =>
            {
                if (LuckStoneManager.Instance.SelectedStone!=null)
                {
                    if (LuckStoneManager.Instance.SelectedStone.Id==vo.Id)
                    {
                        dp.transform.FindChild("CheckBox").GetComponent<UIToggle>().value = true;
                    }
                }
                string item = "";
                if (LuckStoneManager.Instance.GetConsumeItem(vo.ConsumeItem[0].Id) >= vo.ConsumeItem[0].Value)
                {
                    item = ColorConst.Format(ColorConst.Color_Green, vo.ConsumeItem[0].Value);
                }
                else {
                    item = ColorConst.Format(ColorConst.Color_Red, vo.ConsumeItem[0].Value);
                }
                dp.Id = vo.Id;
                dp.Display(vo.Name, vo.ConsumeDiamond.ToString(),item);
            });
    }
}
