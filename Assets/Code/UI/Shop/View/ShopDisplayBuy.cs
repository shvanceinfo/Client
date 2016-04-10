using UnityEngine;
using System.Collections;

public class ShopDisplayBuy : MonoBehaviour {

    public int Id { get; set; }
    public int Price { get; set; }
    public int BuyCount { get; set; }          //购买数量
    private UISprite _boder;                //物品边框
    private UITexture _icon;                //物品图标
    private UILabel _name;                  //物品名称
    private UISprite _goldIocn;             //单价货币图标
    private UILabel _price;                 //单价价格标签
    private UIInput _count;                 //输入数量
    private UILabel _sumPrice;              //总价标签
    private UISprite _sumIcon;              //总价货币图标
    private Transform _buyPanel;            //功能组件


    private void Awake()
    {
        _buyPanel = transform.FindChild("BuyPanel");

        _boder = _buyPanel.FindChild("Item/Boder").GetComponent<UISprite>();
        _icon = _buyPanel.FindChild("Item/Icon").GetComponent<UITexture>();
        _name = _buyPanel.FindChild("Item/Name").GetComponent<UILabel>();
        _goldIocn = _buyPanel.FindChild("Item/GoldIcon").GetComponent<UISprite>();
        _price = _buyPanel.FindChild("Item/price").GetComponent<UILabel>();

        _count = _buyPanel.FindChild("Count/InputCount").GetComponent<UIInput>();
        _sumPrice = _buyPanel.FindChild("Count/sumPrice").GetComponent<UILabel>();
        _sumIcon = _buyPanel.FindChild("Count/GoldIcon").GetComponent<UISprite>();
    }



    public void DisplayInfo(string name, string boder, string icon, string goldIcon, int price,bool isShowRMB=false)
    {
        _name.text = name;
        _boder.spriteName = boder;
        _icon.mainTexture = SourceManager.Instance.getTextByIconName(icon);
        _goldIocn.spriteName = goldIcon;
        this.Price = price;
        
        _sumIcon.spriteName = goldIcon;
        if (isShowRMB)
        {
            _price.text = this.Price.ToString() + Constant.RMB;
        }
        else {
            _price.text = this.Price.ToString();
        }
    }

    public void DisplayBuyCount(int count, bool isShowRMB = false)
    {
        BuyCount=count;
        _count.value = count.ToString();

        if (isShowRMB)
        {
            _sumPrice.text = (Price * BuyCount).ToString() + Constant.RMB;
        }
        else {
            _sumPrice.text = (Price * BuyCount).ToString();
        }
        
    }

    public void DisplayPanel(bool isShow)
    {
        _buyPanel.gameObject.SetActive(isShow);
    }
}
