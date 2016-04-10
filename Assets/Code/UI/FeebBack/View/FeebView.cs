using UnityEngine;
using System.Collections;
using manager;
using helper;
using mediator;
using MVC.entrance.gate;
using model;

public class FeebView : MonoBehaviour
{

    private GameObject table1;
    private GameObject table2;
    private GameObject doubleTable;

    #region 功能1
    private GameObject _fun1;       //快捷购买

    private UISprite _buyBoder;
    private UITexture _buyIcon;
    private UILabel _buyName;
    private UILabel _buyPrice;

    private UIInput _buyInput;
    private UILabel _buySumPrice;
    #endregion



    #region 功能2
    private GameObject _fun2;       //物品导航
    private UISprite _infoBoder;
    private UILabel _infoName;
    private UITexture _infoIcon;
    private Table _table;
    private GameObject _infoPrefab;
    private UIGrid _infoGrid;
    #endregion
    private void Awake()
    {
        table1 = F("Tables/Table1");
        table2 = F("Tables/Table2");
        doubleTable = F("Tables/Double");

        _fun1 = F("Buy");
        _fun2 = F("FindInfo");
        _buyBoder = F<UISprite>("Buy/Item/Boder");
        _buyIcon = F<UITexture>("Buy/Item/Icon");
        _buyName = F<UILabel>("Buy/Item/Name");
        _buyPrice = F<UILabel>("Buy/Item/price");

        _buyInput = F<UIInput>("Buy/Count/InputCount");
        _buySumPrice = F<UILabel>("Buy/Count/sumPrice");

        _table = Table.Table1;
        _fun1.SetActive(false);
        _fun2.SetActive(false);

        _infoBoder = F<UISprite>("FindInfo/Item/Boder");
        _infoName = F<UILabel>("FindInfo/Item/Name");
        _infoIcon = F<UITexture>("FindInfo/Item/Icon");

        _infoPrefab = F("FindInfo/Panel/Item");
        _infoGrid = F<UIGrid>("FindInfo/Panel/Grid");
    }
    private void Start()
    {
        
        
    }
    private void OnEnable()
    {
        Gate.instance.registerMediator(new FeebMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.FEEB_MEDIATOR);
    }

    public void Display(FeedBack fb)
    {
        _fun1.SetActive(false);
        _fun2.SetActive(false);
        switch (fb)
        {
            case FeedBack.None:
                break;
            case FeedBack.Text:
                break;
            case FeedBack.DoubleDialog:
                DisplayTable(_table);
                break;
            case FeedBack.QuickBuy:
                DisplayQuickBuy();
                break;
            case FeedBack.FindInfo:
                DisplayFindInfo();
                break;
            default:
                break;
        }
    }

    private void DisplayFindInfo()
    {
        doubleTable.SetActive(false);
        table1.SetActive(false);
        table2.SetActive(true);
        FindInfo();
    }

    public void DisplayQuickBuy()       //显示一个便捷购买
    {
        doubleTable.SetActive(false);
        table1.SetActive(true);
        table2.SetActive(false);
        QuickBuy();
    }

    public void DisplayTable(Table table)       //显示2个标签
    {
        doubleTable.SetActive(true);
        table1.SetActive(false);
        table2.SetActive(false);
        _fun1.SetActive(false);
        _fun2.SetActive(false);
        switch (table)
        {
            case Table.None:
                break;
            case Table.Table1:
                QuickBuy();
                break;
            case Table.Table2:
                FindInfo();
                break;
            default:
                break;
        }
        _table = table;
    }

    private void QuickBuy()
    {
        _fun1.SetActive(true);
        _buyBoder.spriteName = ViewHelper.GetBoderById((int)FeebManager.Instance.CheckItem.id);
        _buyIcon.mainTexture = SourceManager.Instance.getTextByIconName(FeebManager.Instance.CheckItem.icon);
        _buyName.text = FeebManager.Instance.CheckItem.name;
        _buyPrice.text = FeebManager.Instance.ItemVo.SellPrice.ToString();

        QuickBuyCount();
    }
    //显示合计钱数
    public void QuickBuyCount()
    {
        _buyInput.value = FeebManager.Instance.BuyCount.ToString();

        int price=FeebManager.Instance.BuyCount * FeebManager.Instance.ItemVo.SellPrice;
        _buySumPrice.text = ViewHelper.GetPriceColor(CharacterPlayer.character_asset.diamond, price);
    }

    private void FindInfo()
    {
        _fun2.SetActive(true);
        _infoBoder.spriteName = ViewHelper.GetBoderById((int)FeebManager.Instance.CheckItem.id);
        _infoIcon.mainTexture = SourceManager.Instance.getTextByIconName(FeebManager.Instance.CheckItem.icon);
        _infoName.text = FeebManager.Instance.CheckItem.name;

        FindInfoList();
    }
    private void FindInfoList()
    {
        ViewHelper.FormatTemplate<BetterList<FastOpenVo>, FastOpenVo>(_infoPrefab, _infoGrid.transform,
            FeebManager.Instance.FastList,
            (FastOpenVo vo, Transform t) =>
            {
                t.FindChild("Label").GetComponent<UILabel>().text = vo.Description;
            });
    }

    private T F<T>(string path) where T:Component
    {
        return transform.FindChild(path).GetComponent<T>();
    }

    private GameObject F(string path)
    {
        return transform.FindChild(path).gameObject;
    }
}
