using UnityEngine;
using System.Collections;

public class VipViewDisplayImg : MonoBehaviour {

    private UILabel _power;
    private UITexture _icon;
    private UISprite _sp;
    private GameObject _tipObj;
    private UILabel _tip;

    private void Awake()
    {
        _power = transform.FindChild("Label").GetComponent<UILabel>();
        _icon = transform.FindChild("Icon").GetComponent<UITexture>();
        _sp = transform.FindChild("Sp").GetComponent<UISprite>();
        _tipObj = transform.FindChild("Tip").gameObject;
        _tip = transform.FindChild("Tip/Label").GetComponent<UILabel>();
    }


    public void DisplayIcon(string icon)
    {
        _icon.mainTexture = SourceManager.Instance.getTextByIconName(icon, PathConst.VIP_PIC_PATH);
    }

    public void DisplayPower(bool isactive=false)
    {
        _sp.alpha = 0;
        _power.alpha = 0;
    }
    public void DisplayPower(int power)
    {
        _sp.alpha = 1;
        _power.alpha = 1;
        _power.text = power.ToString();
    }

    public void DisplayTip(bool isactive = false)
    {
        _tipObj.SetActive(isactive);
    }
    public void DisplayTip(string text)
    {
        _tipObj.SetActive(true);
        _tip.text = text;
    }
}
