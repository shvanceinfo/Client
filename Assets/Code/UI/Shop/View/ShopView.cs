using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using mediator;
using model;
using manager;
using helper;
public class ShopView : MonoBehaviour {

    const string hotSellBg = "shop_bq_remai";               //商品状态图标
    const string hotSellLbl = "shop_zi_remai";
    const string disCountBg = "shop_bq_zhekou";
    const string disCountLbl = "shop_zi_zhekou";
    const string preferBg = "shop_bq_youhui";
    const string preferLbl = "shop_zi_youhui";


    private UILabel _diamondLbl;
    private UILabel _goldLbl;

    private ShopDisplayBuy _buyInfo;            //购买面板

    private Transform _grid;
    private UIGrid _uiGrid;
    private GameObject _prefab;
    private UIScrollView _scrollView;
    DataReadItem dr;
    private Dictionary<SellShopType, UICheckBoxColor> _cbs;
    private void Awake()
    {
        dr = ConfigDataManager.GetInstance().getItemTemplate();
        _diamondLbl = transform.FindChild("Buttom/Diamond/Label").GetComponent<UILabel>();
        _goldLbl = transform.FindChild("Buttom/Gold/Label").GetComponent<UILabel>();
        _grid = transform.FindChild("ItemPanel/Grid");
        _prefab = transform.FindChild("ItemPanel/DoubleItem").gameObject;
        _buyInfo = GetComponent<ShopDisplayBuy>();
        _uiGrid = _grid.GetComponent<UIGrid>();
        _scrollView = transform.FindChild("ItemPanel").GetComponent<UIScrollView>();

        _cbs = new Dictionary<SellShopType, UICheckBoxColor>();
        _cbs.Add(SellShopType.HotSell, transform.FindChild("Table/Table1").GetComponent<UICheckBoxColor>());
        _cbs.Add(SellShopType.Equip, transform.FindChild("Table/Table2").GetComponent<UICheckBoxColor>());
        _cbs.Add(SellShopType.Item, transform.FindChild("Table/Table3").GetComponent<UICheckBoxColor>());
        _cbs.Add(SellShopType.Shuijing, transform.FindChild("Table/Table4").GetComponent<UICheckBoxColor>());
        _cbs.Add(SellShopType.Diamon, transform.FindChild("Table/Table5").GetComponent<UICheckBoxColor>());
    }

    private void Start()
    {
        DisplayMoney();
    }
    private void OnEnable()
    {
        Gate.instance.registerMediator(new ShopMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.SHOP_MEDIATOR);
    }

    //显示当前金钱数
    public void DisplayMoney()
    {
        _diamondLbl.text = CharacterPlayer.character_asset.diamond.ToString();
        _goldLbl.text = CharacterPlayer.character_asset.Crystal.ToString();
    }

    //显示购买详细信息
    public void DisplayBuyPanel()
    {
        ShopVo vo=ShopManager.Instance.SelectItem;
        _buyInfo.DisplayPanel(true);
        string boder=BagManager.Instance.getItemBgByType(dr.getTemplateData(vo.ItemId).quality,true);
        string goldIcon=SourceManager.Instance.getIconByType(vo.SellMoneyType);
        string icon=dr.getTemplateData(vo.ItemId).icon;

        if (vo.SellMoneyType==eGoldType.none)
        {
            _buyInfo.DisplayInfo(vo.Name, boder, icon, goldIcon, vo.RmbPrice,true);
            _buyInfo.DisplayBuyCount(ShopManager.Instance.SelectItemBuyCount,true);
        }else{
            _buyInfo.DisplayInfo(vo.Name, boder, icon, goldIcon, vo.SellPrice);
            _buyInfo.DisplayBuyCount(ShopManager.Instance.SelectItemBuyCount);
        }
        
    }
    //显示购买的数量
    public void DisplayBuyPanelCount()
    {
        if (ShopManager.Instance.SelectItem.SellMoneyType == eGoldType.none)
        {
            _buyInfo.DisplayBuyCount(ShopManager.Instance.SelectItemBuyCount,true);
        }
        else {
            _buyInfo.DisplayBuyCount(ShopManager.Instance.SelectItemBuyCount);
        }
        
    }
    public void HideBuyPanel()
    {
        _buyInfo.DisplayPanel(false);
    }

    private void ActiveTable(SellShopType table)
    {
        foreach (UICheckBoxColor cbk in _cbs.Values)
        {
            cbk.isChecked = false;
        }
        _cbs[table].isChecked = true;
    }
    //显示当前页签商品列表
    public void DisplayTable(SellShopType table)
    {
        ActiveTable(table);
        BetterList<ShopVo> items = ShopManager.Instance.GetTableLvl(table);
        
        int size = items.size % 2 == 0 ? items.size : items.size + 1;
        int tsSize = _grid.childCount;
        if (size<tsSize*2)
        {
            for (int i = size; i < tsSize*2; i+=2)
            {
                ViewHelper.DeleteItemTemplate(_grid, i);
            }
        }
        if (size>tsSize*2)
        {
            for (int i = tsSize*2; i < size; i+=2)
            {
                ViewHelper.AddItemTemplatePrefab(_prefab, _grid, i);
            }
        }

        for (int i = 0; i < items.size; i +=2)
        {
            if (i + 1 >= items.size)
            {
                DisplayItem(i, items[i], null);
            }
            else {
                DisplayItem(i, items[i], items[i + 1]); 
            }
            
        }

        _scrollView.ResetPosition();
        _uiGrid.Reposition();

    }


    private void DisplayItem(int douleIndex, ShopVo item1, ShopVo item2)
    {
        Transform _t = _grid.FindChild(douleIndex.ToString());
        _t.FindChild("1").gameObject.SetActive(true);
        ShopDisplayItem sd1 = _t.FindChild("0").GetComponent<ShopDisplayItem>();
        ShopDisplayItem sd2 = _t.FindChild("1").GetComponent<ShopDisplayItem>();
        
        
        ItemTemplate it;
        if (item1 != null)
        {
             it= dr.getTemplateData(item1.ItemId);

            //显示基本信息
             sd1.DisplayInfo(it.name,
            BagManager.Instance.getItemBgByType(it.quality, true),
            it.icon);
            //是否显示活动图标
            SetShopState(sd1, item1);
            sd1.Id = item1.Id;
			_t.FindChild("0/Item").GetComponent<BtnTipsMsg>().Iteminfo = new ItemInfo((uint)item1.ItemId,0,0);
        }
        if (item2 != null)
        {
            it = dr.getTemplateData(item2.ItemId);

            //显示基本信息
            sd2.DisplayInfo(it.name,
            BagManager.Instance.getItemBgByType(it.quality, true),
            it.icon);
            //是否显示活动图标
            SetShopState(sd2, item2);
            sd2.Id = item2.Id;
			_t.FindChild("1/Item").GetComponent<BtnTipsMsg>().Iteminfo = new ItemInfo((uint)item2.ItemId,0,0);
        }
        else {
            sd2.gameObject.SetActive(false);
        }
    }
   
    private void SetShopState(ShopDisplayItem sd, ShopVo vo)
    {
        string goldIcon=SourceManager.Instance.getIconByType(vo.SellMoneyType);
        if (vo.SellMoneyType ==eGoldType.none)
        {
            switch (vo.SellState)
            {
                case ShopStateType.None:
                    sd.DisplayStateIsNull();
                    sd.DisplayNowPrice(vo.RmbPrice.ToString() + Constant.RMB, goldIcon, true);
                    break;
                case ShopStateType.HotSell:
                    sd.DisplayState(hotSellLbl, hotSellBg);
                    sd.DisplayNowPrice(vo.RmbPrice.ToString() + Constant.RMB, goldIcon, true);
                    break;
                case ShopStateType.Prefer:
                    sd.DisplayState(preferLbl, preferBg);
                    sd.DisplayDoublePrice(vo.RmbPrice.ToString() + Constant.RMB, vo.StateDescription, goldIcon, goldIcon, true);
                    break;
                case ShopStateType.DisCount:
                    sd.DisplayState(disCountLbl, disCountBg);
                    sd.DisplayDoublePrice(vo.RmbPrice.ToString() + Constant.RMB, vo.StateDescription, goldIcon, true);
                    break;
                default:
                    break;
            }
        }
        else {
            switch (vo.SellState)
            {
                case ShopStateType.None:
                    sd.DisplayStateIsNull();
                    sd.DisplayNowPrice(vo.SellPrice.ToString(), goldIcon);
                    break;
                case ShopStateType.HotSell:
                    sd.DisplayState(hotSellLbl, hotSellBg);
                    sd.DisplayNowPrice(vo.SellPrice.ToString(), goldIcon);
                    break;
                case ShopStateType.Prefer:
                    sd.DisplayState(preferLbl, preferBg);
                    sd.DisplayDoublePrice(vo.SellPrice.ToString(), vo.StateDescription, goldIcon, goldIcon);
                    break;
                case ShopStateType.DisCount:
                    sd.DisplayState(disCountLbl, disCountBg);
                    sd.DisplayDoublePrice(vo.SellPrice.ToString(), vo.StateDescription, goldIcon);
                    break;
                default:
                    break;
            }
        }

        
    }
    
}
