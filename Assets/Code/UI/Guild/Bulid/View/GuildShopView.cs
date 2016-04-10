using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using model;
using manager;
using helper;

public class GuildShopView : MonoBehaviour
{

    const string hotSellBg = "shop_bq_remai";               //商品状态图标
    const string hotSellLbl = "shop_zi_remai";
    const string disCountBg = "shop_bq_zhekou";
    const string disCountLbl = "shop_zi_zhekou";
    const string preferBg = "shop_bq_youhui";
    const string preferLbl = "shop_zi_youhui";


    private GuildShopDisplayBuy _buyInfo;            //购买面板

    private UILabel myPriceText;

    private Transform _grid;
    private UIGrid _uiGrid;
    private GameObject _prefab;
    private UIScrollView _scrollView;
    private UILabel _conttibutionlabel;
    private UISprite _bottomIcon;

    DataReadItem dr;
    private void Awake()
    {
        dr = ConfigDataManager.GetInstance().getItemTemplate();
        _grid = transform.FindChild("ItemPanel/Grid");
        _prefab = transform.FindChild("ItemPanel/DoubleItem").gameObject;
        _buyInfo = GetComponent<GuildShopDisplayBuy>();
        _uiGrid = _grid.GetComponent<UIGrid>();
        _scrollView = transform.FindChild("ItemPanel").GetComponent<UIScrollView>();
        myPriceText = transform.FindChild("Bottom/Label").GetComponent<UILabel>();
        _conttibutionlabel = transform.FindChild("Bottom/Label").GetComponent<UILabel>();
        _bottomIcon = transform.FindChild("Bottom/Icon").GetComponent<UISprite>();
    }

    private void Start()
    {
        //DisplayMoney();
    }
    private void OnEnable()
    {
        Gate.instance.registerMediator(new GuildShopMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.GUILD_SHOP_MEDIATOR);
    }

    public void OpenWindow()
    {
        //UIManager.Instance.openWindow(UiNameConst.ui_guildshop);
    }

    //显示当前金钱数（贡献值）
    public void ShopDisplayMoney()
    {
        myPriceText.text = CharacterPlayer.character_asset.Crystal.ToString();
    }


    //显示购买详细信息
    public void ShopDisplayBuyPanel()
    {
        GuildShopVo vo = GuildManager.Instance.ShopSelectItem;
        //判断今日购买次数是否小于上限

        _buyInfo.DisplayPanel(true);

        string boder = BagManager.Instance.getItemBgByType(dr.getTemplateData(vo.ItemId).quality, true);
        string goldIcon = SourceManager.Instance.getIconByType(vo.SellMoneyType);
        string icon = dr.getTemplateData(vo.ItemId).icon;

        if (vo.SellMoneyType == eGoldType.none)
        {
            _buyInfo.DisplayInfo(vo.Name, boder, icon, goldIcon, vo.ResourceNum, true);
            _buyInfo.DisplayBuyCount(GuildManager.Instance.ShopSelectItemBuyCount, true);
        }
        else
        {
            _buyInfo.DisplayInfo(vo.Name, boder, icon, goldIcon, vo.ResourceNum);
            _buyInfo.DisplayBuyCount(GuildManager.Instance.ShopSelectItemBuyCount);
        }

    }
    //显示购买的数量
    public void ShopDisplayBuyPanelCount()
    {
        if (GuildManager.Instance.ShopSelectItem.SellMoneyType == eGoldType.none)
        {
            _buyInfo.DisplayBuyCount(GuildManager.Instance.ShopSelectItemBuyCount, true);
        }
        else
        {
            _buyInfo.DisplayBuyCount(GuildManager.Instance.ShopSelectItemBuyCount);
        }

    }
    public void ShopHideBuyPanel()
    {
        _buyInfo.DisplayPanel(false);
    }


    //显示当前页签商品列表
    public void ShopDisplayTable(SellShopType table)
    {
        BetterList<GuildShopVo> items = GuildManager.Instance.ShopGetTableLvl(table);

        int size = items.size % 2 == 0 ? items.size : items.size + 1;
        int tsSize = _grid.childCount;
        if (size < tsSize * 2)
        {
            for (int i = size; i < tsSize * 2; i += 2)
            {
                ViewHelper.DeleteItemTemplate(_grid, i);
            }
        }
        if (size > tsSize * 2)
        {
            for (int i = tsSize * 2; i < size; i += 2)
            {
                ViewHelper.AddItemTemplatePrefab(_prefab, _grid, i);
            }
        }

        for (int i = 0; i < items.size; i += 2)
        {
            if (i + 1 >= items.size)
            {
                ShopDisplayItem(i, items[i], null);
            }
            else
            {
                ShopDisplayItem(i, items[i], items[i + 1]);
            }

        }

        _scrollView.ResetPosition();
        _uiGrid.Reposition();

    }


    private void ShopDisplayItem(int douleIndex, GuildShopVo item1, GuildShopVo item2)
    {
        Transform _t = _grid.FindChild(douleIndex.ToString());
        _t.FindChild("1").gameObject.SetActive(true);
        GuildShopDisplayItem sd1 = _t.FindChild("0").GetComponent<GuildShopDisplayItem>();
        GuildShopDisplayItem sd2 = _t.FindChild("1").GetComponent<GuildShopDisplayItem>();


        ItemTemplate it = dr.getTemplateData(0);
        if (item1 != null)
        {
            it = dr.getTemplateData(item1.ItemId);

            //显示基本信息
            sd1.ShopDisplayInfo(it.name,
           BagManager.Instance.getItemBgByType(it.quality, true),
           it.icon);

            //是否显示活动图标
            SetShopState(sd1, item1);
            sd1.Id = item1.Id;
            _t.FindChild("0/Item").GetComponent<BtnTipsMsg>().Iteminfo = new ItemInfo((uint)item1.ItemId, 0, 0);
        }
        if (item2 != null)
        {
            it = dr.getTemplateData(item2.ItemId);

            //显示基本信息
            sd2.ShopDisplayInfo(it.name,
            BagManager.Instance.getItemBgByType(it.quality, true),
            it.icon);
            //是否显示活动图标
            SetShopState(sd2, item2);
            sd2.Id = item2.Id;
            _t.FindChild("1/Item").GetComponent<BtnTipsMsg>().Iteminfo = new ItemInfo((uint)item2.ItemId, 0, 0);
        }
        else
        {
            sd2.gameObject.SetActive(false);
        }
        //计算贡献值
        _conttibutionlabel.text = GuildManager.Instance.InfoOwnContribution.Value.ToString();
        //显示下方Icon
        _bottomIcon.spriteName = it.icon;

    }

    private void SetShopState(GuildShopDisplayItem sd, GuildShopVo vo)
    {
        string goldIcon = SourceManager.Instance.getIconByType(vo.SellMoneyType);
        if (vo.SellMoneyType == eGoldType.none || vo.SellMoneyType == eGoldType.rongyu)
        {
            switch (vo.SellState)
            {
                case ShopStateType.None:
                    sd.ShopDisplayNowPrice(vo.ResourceNum.ToString() + Constant.RMB, goldIcon, vo.CurBuyCount.ToString(), vo.SellLimit.ToString(), true);
                    //控制今日兑换数值
                    break;
                case ShopStateType.HotSell:
                    sd.ShopDisplayNowPrice(vo.ResourceNum.ToString() + Constant.RMB, goldIcon, vo.CurBuyCount.ToString(), vo.SellLimit.ToString(), true);
                    break;
                default:
                    break;
            }
        }
    }

}
