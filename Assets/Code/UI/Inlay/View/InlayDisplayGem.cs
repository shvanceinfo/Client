using UnityEngine;
using System.Collections;

public class InlayDisplayGem : MonoBehaviour {

    const string CANINLAY = "可镶嵌";
    const string OPEN = "({0}级或VIP{1}开启)";
    const string LOCK = "skill_lock";
    private UITexture _icon;
    private UILabel _name;
    private UILabel _atrName;
    private UILabel _atrValue;
    private GameObject button;
    private void Awake()
    {
        _icon = transform.FindChild("icon").GetComponent<UITexture>();
        _name = transform.FindChild("name").GetComponent<UILabel>();
        _atrName = transform.FindChild("name_atr").GetComponent<UILabel>();
        _atrValue = transform.FindChild("value_atr").GetComponent<UILabel>();
        button = transform.FindChild("Button_ReMove").gameObject;
    }

    public void DisplayGem(string icon, string name, string atrName, string atrValue)
    {
        _icon.alpha = 1;
        _icon.mainTexture = SourceManager.Instance.getTextByIconName(icon);
        _name.text = name;
        _atrName.text = atrName;
        _atrValue.text = atrValue;
        button.SetActive(true);
    }

    public void CanInlay()
    {
        _icon.alpha = 0;
        _name.text = "";
        _atrName.text = CANINLAY;
        _atrValue.text = "";
        button.SetActive(false);
    }
    public void IsLock(string level,string vip)
    {
        _icon.alpha = 1;
        _icon.mainTexture = SourceManager.Instance.getTextByIconName(LOCK,PathConst.SKILL_PATH);
        _name.text = "";
        _atrName.text = "尚未镶嵌";
        _atrValue.text = string.Format(OPEN, level, vip);
        button.SetActive(false);
    }
}
