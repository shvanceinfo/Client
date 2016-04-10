using UnityEngine;
using System.Collections;

public class EmailDisplayAward : MonoBehaviour {


    UILabel _count;
    UISprite _boder;
    UITexture _icon;

    private void Awake()
    {
        _count = transform.FindChild("Label").GetComponent<UILabel>();
        _boder = transform.FindChild("Boder").GetComponent<UISprite>();
        _icon = transform.FindChild("Icon").GetComponent<UITexture>();
    }

    public void Display(string icon, string boder, string count)
    {
        _icon.mainTexture = SourceManager.Instance.getTextByIconName(icon);
        _boder.spriteName = boder;
        _count.text = "x" + count;
    }
}
