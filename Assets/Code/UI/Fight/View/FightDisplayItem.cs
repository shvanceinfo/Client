using UnityEngine;
using System.Collections;

/// <summary>
/// 动态的显示物品
/// </summary>
public class FightDisplayItem : MonoBehaviour {


    private UISprite _back;
    private UISprite _iconBoder;
    private UITexture _icon;
    private UILabel _count;
    private UILabel _name;
    private void Awake()
    {
        _back = transform.FindChild("Background").GetComponent<UISprite>();
        _iconBoder = transform.FindChild("Icon_Boder").GetComponent<UISprite>();
        _icon = transform.FindChild("Icon").GetComponent<UITexture>();
        _count = transform.FindChild("Count").GetComponent<UILabel>();
        _name = transform.FindChild("Name").GetComponent<UILabel>();

    }
    private void Start()
    {
        IsDisplay(false);
    }

    public void Display(string icon, string iconBoder, int count, string name)
    {
        IsDisplay(true);
        _icon.mainTexture = SourceManager.Instance.getTextByIconName(icon);
        _iconBoder.spriteName = iconBoder;
        _count.text = string.Format("x{0}", count);
        _name.text = name;
    }

    public void IsDisplay(bool isShow)
    {
        _back.enabled = isShow;
        _iconBoder.enabled = isShow;
        _icon.enabled = isShow;
        _count.enabled = isShow;
        _name.enabled = isShow;
    }
}
