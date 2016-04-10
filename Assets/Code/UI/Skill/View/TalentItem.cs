using UnityEngine;
using System.Collections;
using MVC.entrance.gate;

public class TalentItem : MonoBehaviour {

    public int ID { get; set; }
    private UITexture spIcon;
    private UILabel lblName;
    private UILabel lblLevel;
    private UILabel lblMoney;
    private UILabel lblEffectValue;
    private UILabel lblDescription;
    private UILabel lblEffect;
    private UISprite spGoldicon;
    private void Awake()
    {
        spIcon = transform.FindChild("icon").GetComponent<UITexture>();
        lblName = transform.FindChild("name").GetComponent<UILabel>();
        lblLevel = transform.FindChild("lbl_level").GetComponent<UILabel>();
        lblMoney = transform.FindChild("lbl_money").GetComponent<UILabel>();
        lblEffectValue = transform.FindChild("lbl_hp").GetComponent<UILabel>();
        lblDescription = transform.FindChild("lbl_desc").GetComponent<UILabel>();
        lblEffect = transform.FindChild("lbl2").GetComponent<UILabel>();
        spGoldicon = transform.FindChild("goldicon").GetComponent<UISprite>();
    }

    /// <summary>
    /// 修改item显示内容
    /// </summary>
    /// <param name="icon">图标</param>
    /// <param name="name">名字</param>
    /// <param name="level">等级 (100/100)</param>
    /// <param name="money">消耗金钱</param>
    /// <param name="effect">效果属性</param>
    /// <param name="effectValue">效果属性值</param>
    /// <param name="desc">描述</param>
    public void Display(string icon,string name,
        string level, string money, string effect, string effectValue, string desc, string goldiocn)
    {
        spIcon.mainTexture = SourceManager.Instance.getTextByIconName(icon, PathConst.TALENT_PATH); 
        lblName.text = name;
        lblLevel.text = level;
        lblMoney.text = money;
        lblEffect.text = effect;
        lblEffectValue.text = effectValue;
        lblDescription.text = desc;
        spGoldicon.spriteName = goldiocn;
    }

    public void Display(int id,string icon, string name,
        string level, string money, string effect, string effectValue, string desc,string goldiocn)
    {
        this.ID = id;
        spIcon.mainTexture = SourceManager.Instance.getTextByIconName(icon, PathConst.TALENT_PATH); 
        lblName.text = name;
        lblLevel.text = level;
        lblMoney.text = money;
        lblEffect.text = effect;
        lblEffectValue.text = effectValue;
        lblDescription.text = desc;
        spGoldicon.spriteName = goldiocn;
    }
}
