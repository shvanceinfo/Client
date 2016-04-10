using UnityEngine;
using System.Collections;

public class ShopDisplayItem : MonoBehaviour {

    public int Id { get; set; }

    private UILabel _title;
    private UISprite _hotSellSp;        //热卖标签低
    private UISprite _hotSellLbl;       //热卖文字图片
    private UISprite _boder;
    private UITexture _icon;

    private GameObject _nowPrice;
    private GameObject _doublePrice;


    private void Awake()
    {
        _title = transform.FindChild("Title").GetComponent<UILabel>();
        _hotSellSp = transform.FindChild("State/Background").GetComponent<UISprite>();
        _hotSellLbl = transform.FindChild("State/Title").GetComponent<UISprite>();
        _boder = transform.FindChild("Item/Background").GetComponent<UISprite>();
        _icon = transform.FindChild("Item/Icon").GetComponent<UITexture>();
        _nowPrice = transform.FindChild("NowPrice").gameObject;
        _doublePrice = transform.FindChild("DoublePrice").gameObject;
    }

    //显示商品基本信息
    public void DisplayInfo(string title, string boder, string icon)
    {
        _title.text = title;
        _boder.spriteName = boder;
        if (icon==null||icon=="")
        {
            Debug.LogError(title);
            return;
        }
        Texture2D text = SourceManager.Instance.getTextByIconName(icon);
        if (text != null)
        {
            _icon.mainTexture = text;
        }
    }
    //隐藏商品售卖状态
    public void DisplayStateIsNull()
    {
        _hotSellSp.alpha = 0;
        _hotSellLbl.alpha = 0;
    }
    //显示商品售卖状态
    public void DisplayState(string state,string back)
    {
        _hotSellSp.alpha = 1;
        _hotSellLbl.alpha = 1;
        _hotSellSp.spriteName = back;
        _hotSellLbl.spriteName = state;
    }

    //显示单条金钱
    public void DisplayNowPrice(string price,string goldIcon,bool isHiden=false)
    {
        _doublePrice.SetActive(false);
        _nowPrice.SetActive(true);
        transform.FindChild("NowPrice/Num").GetComponent<UILabel>().text = price;
        UISprite gi=transform.FindChild("NowPrice/Icon").GetComponent<UISprite>();
        if (isHiden)
        {
            gi.alpha = 0;
        }
        else {
            gi.alpha = 1;
            gi.spriteName = goldIcon;  
        }
        
    }
    public void DisplayDoublePrice(string price1, string price2, string goldIocn1, string goldIcon2, bool isHiden = false)
    {
        _doublePrice.SetActive(true);
        _nowPrice.SetActive(false);

       UILabel num1= transform.FindChild("DoublePrice/Num").GetComponent<UILabel>();
       UILabel num2 = transform.FindChild("DoublePrice/Num2").GetComponent<UILabel>();
       UISprite icon1 = transform.FindChild("DoublePrice/Icon").GetComponent<UISprite>();
       UISprite icon2 = transform.FindChild("DoublePrice/Icon2").GetComponent<UISprite>();
       UILabel line = transform.FindChild("DoublePrice/Line").GetComponent<UILabel>();
       UILabel title1 = transform.FindChild("DoublePrice/Title").GetComponent<UILabel>();
       UILabel title2 = transform.FindChild("DoublePrice/Title2").GetComponent<UILabel>();

       num1.text = price1;
       num2.text = price2;
       icon1.spriteName = goldIocn1;
       icon2.spriteName = goldIcon2;
       title1.text = "现价:";
       title2.text = "原价:";
       line.alpha = 1;
       num1.alpha = 1;
       num2.alpha = 1;
       
       title1.alpha = 1;
       title2.alpha = 1;
       if (isHiden)
       {
           icon1.alpha = 0;
           icon2.alpha = 0;
       }
       else {
           icon1.alpha = 1;
           icon2.alpha = 1;
       }
    }
    public void DisplayDoublePrice(string price1, string desc, string goldIcon1,bool isHiden=false)
    {
        _doublePrice.SetActive(true);
        _nowPrice.SetActive(false);
        UILabel num1 = transform.FindChild("DoublePrice/Num").GetComponent<UILabel>();
        UILabel num2 = transform.FindChild("DoublePrice/Num2").GetComponent<UILabel>();
        UISprite icon1 = transform.FindChild("DoublePrice/Icon").GetComponent<UISprite>();
        UISprite icon2 = transform.FindChild("DoublePrice/Icon2").GetComponent<UISprite>();
        UILabel line = transform.FindChild("DoublePrice/Line").GetComponent<UILabel>();
        UILabel title1 = transform.FindChild("DoublePrice/Title").GetComponent<UILabel>();
        UILabel title2 = transform.FindChild("DoublePrice/Title2").GetComponent<UILabel>();

        num1.text = price1;
        num2.text = "";
        icon1.spriteName = goldIcon1;
        icon2.spriteName = goldIcon1;
        title1.text = "现价:";
        title2.text = desc;
        line.alpha = 0;
        icon1.alpha = 1;
        icon2.alpha = 0;
        title1.alpha = 1;
        title2.alpha = 1;

        if (isHiden)
        {
            icon1.alpha = 0;
            icon2.alpha = 0;
        }
        else
        {
            icon1.alpha = 1;
            icon2.alpha = 0;
        }
    }




 
}
