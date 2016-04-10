using UnityEngine;
using System.Collections;
using manager;
using model;
using helper;

public class GuildSkillFocusDisplayItem : MonoBehaviour
{
    private UITexture texture;
    private UISprite texticon;
    private UILabel txt1, txt2, txt3, txt4;
    private UILabel caifu;

    private void Awake()
    {
        texture = transform.FindChild("Item/Icon").GetComponent<UITexture>();
        txt1 = transform.FindChild("NowPrice/Title").GetComponent<UILabel>();
        txt2 = transform.FindChild("NowPrice/Title2").GetComponent<UILabel>();
        txt3 = transform.FindChild("NowPrice/Title3").GetComponent<UILabel>();
        txt4 = transform.FindChild("NowPrice/Title4").GetComponent<UILabel>();
        texticon = transform.FindChild("NowPrice/Icon").GetComponent<UISprite>();
        caifu = transform.parent.parent.FindChild("Label").GetComponent<UILabel>();
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
        txt4.text = skillLevel.ToString()+"级技能塔可研究";
    }

    public void DisplayCommonItem()
    {
        string goldIcon = SourceManager.Instance.getIconByType(eGoldType.gold);
        texticon.spriteName = goldIcon;
        caifu.text = "100000财富";
    }

    public void DisplayItemTypestruct(TypeStruct typeStr)
    {
        txt3.text = "消耗 " + typeStr.Value.ToString();
    }
}
