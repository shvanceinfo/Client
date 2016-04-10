using UnityEngine;
using System.Collections;
using helper;

public class RefineDisplayAttr : MonoBehaviour {


    private UILabel _attribute1;
    private UILabel _attribute2;
    private UITexture _icon;
    private UILabel _count;
    private GameObject _btn;
    private void Awake()
    {
        _attribute1 = transform.FindChild("A0/Label").GetComponent<UILabel>();
        _attribute2 = transform.FindChild("A1/Label").GetComponent<UILabel>();
        _icon = transform.Find("Icon").GetComponent<UITexture>();
        _count = transform.Find("Label").GetComponent<UILabel>();
        _btn = transform.FindChild("Button_Refine").gameObject;
        
    }

    public void DisplayAttribute1(string at1)
    {
        _attribute1.text = at1;
    }
    public void DisplayAttribute2(string at2)
    {
        _attribute2.text = at2;
    }

    public void DisplayItem(string icon,string count)
    {
        XmlHelper.CallTry(() =>
        {
            _icon.mainTexture = SourceManager.Instance.getTextByIconName(icon);
        });
        _count.text = count;
        IsAcive(true);
        SetRefineButton(true);
    }

    public void SetRefineButton(bool active)
    {
        _btn.SetActive(active);
    }
    public void IsAcive(bool active)
    {
        
        if (active)
        {
            _icon.alpha = 1;
            _count.alpha = 1;
        }else{
            _icon.alpha = 0;
            _count.alpha = 0;
            
        }
    }
}
