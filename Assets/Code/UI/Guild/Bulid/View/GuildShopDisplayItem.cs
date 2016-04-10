using UnityEngine;
using System.Collections;
using helper;

public class GuildShopDisplayItem : MonoBehaviour {

    public int Id { get; set; }

    private UILabel _title;
    private UISprite _boder;
    private UITexture _icon;

    private void Awake()
    {
        _title = transform.FindChild("Title").GetComponent<UILabel>();
        _boder = transform.FindChild("Item/Background").GetComponent<UISprite>();
        _icon = transform.FindChild("Item/Icon").GetComponent<UITexture>();
    }

    //显示商品基本信息
    public void ShopDisplayInfo(string title, string boder, string icon)
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

    
    //显示金钱
    public void ShopDisplayNowPrice(string price,string goldIcon,string a,string b,bool isHiden=false)
    {
        transform.FindChild("NowPrice/Num").GetComponent<UILabel>().text = price;       // 10000
        if (int.Parse(a) < int.Parse(b))
        {
            transform.FindChild("NowPrice/Num2").GetComponent<UILabel>().text = a + "/" + b;    //5/5
        }
        else
        {
            ViewHelper.DisplayMessage("兑换次数已达上限");
            transform.FindChild("Button_Buy").GetComponent<UISprite>().spriteName = "common_button1_hui";
            Destroy(transform.FindChild("Button_Buy").GetComponent<BoxCollider>());
        }
        UISprite gi=transform.FindChild("NowPrice/Icon").GetComponent<UISprite>();      
        gi.alpha = 1;
        gi.spriteName = goldIcon;
    }

}
