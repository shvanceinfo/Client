using UnityEngine;
using System.Collections.Generic;

public class UISuperText : MonoBehaviour {

    public enum TextType
    { 
        Text,   //正常文本
        Link,   //超链接
        Img     //图片
    }
    public UIAtlas spriteAtlas;

    public UILabel prefabLabel;

    //示例
    //正常文本 xxx  [ffffff]xxxx[-] [u]xxxx[/u]
    //角色名Link   [url=0]龙少[/url]
    //装备Link    [url id=fa,=itemid][/url]
    //功能Link    [url=2]打副本[/url]
    //静态表情Link     [img=0]lol[/img]
    //动态表情Link     [img=1]biglol[/img]
    //"[ff0000]Alex[-]Haha[url=type=1,id=10]装备[/url]xxx[img=1]lol[/img]"

    const string URLSF = @"[url=";
    const string URLEF = @"[/url]";
    const string IMGSF = @"[img=";
    const string IMGEF = @"[/img]";


    private List<LineText> _mTexts;

    private UIScrollView _mView;

    private UIPanel _mPanel;

    private int LineWidth
    {
        get { return (int)_mPanel.width; }
    }

    private int LineHeight
    {
        get { return prefabLabel.height; }
    }

    private int SpriteWidth {
        get {
            return prefabLabel.fontSize;
        }
    }

    private void Awake()
    {
        prefabLabel.alpha = 0;
        _mTexts = new List<LineText>();
        _mView = GetComponent<UIScrollView>();
        _mPanel = GetComponent<UIPanel>();
        if (_mView == null) Debug.LogError("Plz Add UIScrollView Component");
    }


    private void GetLine(List<SuperText> Values)
    {
        
       
    }

    #region Public
    public void AppendString(string text)
    {
        List<SuperText> st = Split(text);

        foreach (SuperText sp in st)
        {
            LineText line = null;

            switch (sp.Type)
            {
                case TextType.Text:
                    char[] cs = sp.Text.ToCharArray();
                    for (int i = 0; i < cs.Length; i++)
                    {
                        if (line == null)
                        {
                            line = new LineText(LineWidth, SpriteWidth);
                        }
                        bool isrun=line.AddChat(cs[i]);
                        if (!isrun)
                        {
                            line = new LineText(LineWidth, SpriteWidth);
                            line.AddChat(cs[i]);
                        }
                    }
                    break;
                case TextType.Link:
                    
                    break;
                case TextType.Img:
                    break;
                default:
                    break;
            }
        }
    }

    #endregion

    #region Static

   
    

    //将字符串分割成 text，url，img
    public static List<SuperText> Split(string text)
    {
        List<string> list = URLFormat(text);
        List<SuperText> sList = new List<SuperText>();
        foreach (string str in list)
        {
            sList.Add(new SuperText { Text=str,Type=PraseType(str) });
        }
        return sList;
    }

    //分割字符提取关键字
    public static List<string> URLFormat(string value)
    {
        List<string> texts = new List<string>();

        bool IsRun = true;
        int index = 0;
        while (IsRun)
        {
            int sec = PraseIndex(value, index);
            if (sec == -1)
            {
                texts.Add(value);
                IsRun = false;
            }
            else
            {
                string temp = value.Substring(index, sec);
                texts.Add(temp);
                value = value.Substring(sec);
                if (value.Length == 0) IsRun = false;
            }

        }

        return texts;
    }

    static int PraseIndex(string value, int start)
    {
        int u = value.IndexOf(URLSF, start);
        if (u == 0)
        {
            u = value.IndexOf(URLEF, start) + URLEF.Length;
            return u;
        }
        int i = value.IndexOf(IMGSF, start);
        if (i == 0)
        {
            i = value.IndexOf(IMGEF, start) + IMGEF.Length;
            return i;
        }
        if (u == -1 && i == -1)
        {
            return -1;
        }
        else if (u != -1 && i != -1)
        {
            return u > i ? i : u;
        }
        else
        {
            return u > i ? u : i;
        }
    }

    //查找文本类型
    public static UISuperText.TextType PraseType(string value)
    { 
        int index=value.IndexOf(URLSF);
        if (index != -1)
            return TextType.Link;
        index = value.IndexOf(IMGSF);
        if (index != -1)
            return TextType.Img;

        return TextType.Text;
    }

    public static int PrintSizeX(string value)
    {
        UISuperText.Instance.prefabLabel.text=value;
        int x=UISuperText.Instance.prefabLabel.width;
        UISuperText.Instance.prefabLabel.text = "";
        return x;
    }

    #endregion

    #region Create
    private UILabel CreateNormalLabel(Vector2 pos,int width,string text)
    {
        GameObject obj = BundleMemManager.Instance.instantiateObj(prefabLabel);
        obj.transform.parent = this.transform;
        obj.transform.localPosition = new Vector3();
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.name = "Label";
        UILabel lbl = obj.GetComponent<UILabel>();
        lbl.text = "Default Text";
        return lbl;
    }
    #endregion
    
    #region 单例
    private static UISuperText _instance;
    public static UISuperText Instance
    {
        get
        {
            return _instance;
        }
    }
    void OnEnable()
    {
        _instance = this;
    }
    void OnDisable()
    {
        _instance = null;
    }
    #endregion
      

    public class SuperText
    {
        public UISuperText.TextType Type { get; set; }
        public string Text { get; set; }
        public int Width
        {
            get {
                if (!string.IsNullOrEmpty(Text))
                {
                    switch (Type)
                    {
                        case TextType.Text:
                        case TextType.Link:
                            return (int)UISuperText.PrintSizeX(Text);
                        case TextType.Img:
                            return 0;
                    }
                    return -1;
                }
                else {
                    Debug.LogError("Text is null or empty!");
                    return -1;
                }
                
            }
        }
    }

    public class LineText
    {
        public List<SuperText> Texts { get; set; }

        private int _maxLine;
        private int _curWidth;
        private int _spSize;
        public LineText(int maxWidth,int spriteSize)
        {
            _spSize=spriteSize;
            _maxLine = maxWidth;
            _curWidth = 0;
            Texts = new List<SuperText>();
        }
        public bool AddChat(char text)
        {
            int w=UISuperText.PrintSizeX(text.ToString());
            if (_curWidth + w > _maxLine)
            {
                return false;
            }
            else {
                if (Texts.Count == 0)
                {
                    Texts.Add(new SuperText() { Text = text.ToString(), Type = TextType.Text });
                }
                else {
                    SuperText st = Texts[Texts.Count - 1];
                    if (st.Type == TextType.Text)
                    {
                        st.Text += text.ToString();
                    }
                    else {
                        Texts.Add(new SuperText() { Text = text.ToString(), Type = TextType.Text });
                    }
                }
                _curWidth += w;
            }
            return true;
        }
        public bool AddSprite(SuperText sp)
        {
            if (_curWidth + _spSize > _maxLine)
            {
                return false;
            }

            Texts.Add(sp);
            _curWidth += _spSize;
            return true;
        }
        public bool AddUrl(SuperText url)
        {
            int w = UISuperText.PrintSizeX(url.Text);
            if (_curWidth + w > _maxLine)
            {
                return false;
            }
            Texts.Add(url);
            _curWidth += w;
            return true;
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("Text"))
        {
            AppendString(@"[ff0000]世界[-] [url=type=1,id=10]龙少爷撒啊[/url]:出售武器[url=type=2,id=50002][ff0000]符文之剑[-][/url]，[img=1]哈哈哈[/img]要的MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM");
        }
    }
}



