using UnityEngine;
using System.Collections;

public class VipViewDisplayAttribute : MonoBehaviour {

    const string TRUE = "vip_gou";
    const string FALSE = "vip_x";
    const int SIZE = 10;
    private UILabel[] _lbls;
    private UISprite[] _sps;
    private UILabel _title;
    private GameObject _light;
    private void Awake()
    {
        _light = transform.FindChild("Light").gameObject;
        _title = transform.FindChild("Label").GetComponent<UILabel>();
        _lbls = new UILabel[SIZE];
        _sps = new UISprite[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
           _lbls[i]=transform.FindChild(i + "/Label").GetComponent<UILabel>();
           _sps[i] = transform.FindChild(i + "/Sp").GetComponent<UISprite>();
        }
    }

    public void DisplayTitle(string text)
    {
        _title.text = text;
    }

    public void DisplayLabel(int index,string text)
    {
        _lbls[index].alpha = 1;
        _lbls[index].text = text;
        _sps[index].alpha = 0;
    }
    public void DisplaySprite(int index, bool isActive)
    {
        _sps[index].alpha = 1;
        _sps[index].spriteName = isActive?TRUE:FALSE;
        _lbls[index].alpha = 0;
    }

    public void ShowLight(bool isActive)
    {
        _light.SetActive(isActive);
    }
}
