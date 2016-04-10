using UnityEngine;
using System.Collections;
using manager;
using model;
using helper;

public class GuildSkillDisplayItem : MonoBehaviour {
    private UITexture texture;
    private UISprite texticon;
    private UISprite texticonbottom;
    private UILabel txt1,txt2,txt3;
    private UILabel gongxiandu;

    private void Awake()
    {
        texture = transform.FindChild("Item/Icon").GetComponent<UITexture>();
        txt1 = transform.FindChild("NowPrice/Title").GetComponent<UILabel>();
        txt2 = transform.FindChild("NowPrice/Title2").GetComponent<UILabel>();
        switch (transform.name)
        {
            case "0":
                texticon = transform.FindChild("NowPrice/Icon").GetComponent<UISprite>();
                texticonbottom = transform.parent.parent.FindChild("Icon").GetComponent<UISprite>();
                txt3 = transform.FindChild("NowPrice/Title3").GetComponent<UILabel>();
                gongxiandu = transform.parent.parent.FindChild("Label").GetComponent<UILabel>();
                break;
            default:
                break;
        }
    }

    public void DisplayItem(string skillName, int skillLevel, AttributeValue attr, string icon)
    {
        txt1.text = string.Format("[{0}{1}]:[-][{2}{3}]:[-]", ColorConst.Color_DanHuang, skillName, ColorConst.Color_Juhuang, skillLevel.ToString());
        txt2.text = "增加" + attr.Type + "点数 " + attr.Value;
        if (icon == null || icon == "")
        {
            return;
        }
        texture.mainTexture = SourceManager.Instance.getTextByIconName(icon, PathConst.SKILL_PATH);
    }

    public void DisplayCommonItem()
    {
        string goldIcon = SourceManager.Instance.getIconByType(eGoldType.gold);
        texticon.spriteName = goldIcon;
        string contributeIcon = SourceManager.Instance.getIconByType(eGoldType.gongxiandu);
        texticonbottom.spriteName = contributeIcon;
        gongxiandu.text = "";
    }

    public void DisplayItemTypestruct(TypeStruct typeStr)
    {
        txt3.text = "消耗 " + typeStr.Value.ToString();
    }
}
