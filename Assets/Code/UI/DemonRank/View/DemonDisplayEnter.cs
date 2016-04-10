using UnityEngine;
using System.Collections;
using helper;

public class DemonDisplayEnter : MonoBehaviour {

    const string X = "x";
    const string color1 = "ff8c1b";
    const string color2 = "ec1d1d";
    const string gonext = "[{0}]继续[-]\n第[{1}]{2}[-]波";
    const string go = "继续";


    private UILabel _count;
    private UILabel _name;
    private UITexture _goldIcon;
    private UISprite _btnBack;  //背景图片
    private BoxCollider _box;
    private void Awake()
    {
        _name = transform.FindChild("Btn/title").GetComponent<UILabel>();
        _count = transform.FindChild("goldnum").GetComponent<UILabel>();
        _goldIcon = transform.FindChild("goldicon").GetComponent<UITexture>();
        _btnBack = transform.FindChild("Btn/back").GetComponent<UISprite>();
        _box = transform.FindChild("Btn").GetComponent<BoxCollider>();
    }


    public void Display(string icon,int gate, int tickCount)
    {
        _goldIcon.mainTexture = SourceManager.Instance.getTextByIconName(icon);
        _name.text = string.Format(gonext, color1, color2, gate);
        _count.text = X + tickCount.ToString();
        _btnBack.color = new Color(255, 255, 255, 255);
        _box.enabled = true;
    }
    public void Display(string icon,int tickCount)
    {
        _goldIcon.mainTexture = SourceManager.Instance.getTextByIconName(icon);
        _count.text = X + tickCount.ToString();
    }
    

    public void Hiddent()
    {

        //Vector4 c1 = _btnBack.color;
        //Vector3 v1 = new Vector3(c1.x, c1.y, c1.z);
        //float grey = Vector3.Dot(v1, new Vector3(0.299f, 0.587f, 0.114f));
        //_btnBack.color = new Color(grey, grey, grey);
        _box.enabled = false;
        _btnBack.color = new Color(0.004f, 0, 0, 1);
        _name.text = ColorConst.Format("888888", go);
    }

}
