using UnityEngine;
using System.Collections;
using manager;
using helper;

public class StrengThenDisplayItem : MonoBehaviour {

    const string UP = "↑";
    const string DOWN = "↓";


    public int Id { get; set; }
    UISprite _boder;
    UITexture _icon;
    UILabel _name;
    UILabel _level;
    UILabel _career;
    UILabel _strengThenLevel;
    UILabel _attackPower;
	BtnTipsMsg _tips;
    private void Awake()
    {
        _boder = transform.FindChild("boder").GetComponent<UISprite>();
        _icon = transform.FindChild("icon").GetComponent<UITexture>();
        _name = transform.FindChild("name").GetComponent<UILabel>();
        _level = transform.FindChild("level").GetComponent<UILabel>();
        _career = transform.FindChild("career").GetComponent<UILabel>();
        _strengThenLevel = transform.FindChild("strengthen").GetComponent<UILabel>();
        _attackPower = transform.FindChild("atrribute").GetComponent<UILabel>();
		_tips = transform.FindChild("icon").GetComponent<BtnTipsMsg>();
    }


    public void Display(
        int id,eItemQuality boder,string icon,
        string name,string level,string career,
        int strengThenLevel,int attackPower )
    {
        this.Id = id;
        _boder.spriteName = BagManager.Instance.getItemBgByType(boder, false);
        XmlHelper.CallTry(() => { _icon.mainTexture = SourceManager.Instance.getTextByIconName(icon); });
        _name.text = name;
        _level.text = level;
        _career.text = career;
        if (strengThenLevel == 0)
        {
            _strengThenLevel.text = "";
        }
        else
        {
            _strengThenLevel.text = string.Format("+{0}", strengThenLevel);
        }

        string lbl = "";
        if (attackPower > 0)
        {
            lbl = ColorConst.Format(ColorConst.Color_Green, UP, attackPower);
        }
        else if (attackPower < 0)
        {
            lbl = ColorConst.Format(ColorConst.Color_Red, DOWN, attackPower);
        }
        else {
            lbl = ColorConst.Format(ColorConst.Color_Red, "");
        }
        _attackPower.text = lbl;

    }
    public void Display(
        int id, eItemQuality boder, string icon,
        string name, string level, string career,
        int strengThenLevel, string attackPower)
    {
        this.Id = id;
        _boder.spriteName = BagManager.Instance.getItemBgByType(boder, false);
        XmlHelper.CallTry(() => { _icon.mainTexture = SourceManager.Instance.getTextByIconName(icon); });
        _name.text = name;
        _level.text = level;
        _career.text = career;
        if (strengThenLevel == 0)
        {
            _strengThenLevel.text = "";
        }
        else {
            _strengThenLevel.text = string.Format("+{0}", strengThenLevel);
        }
        

        _attackPower.text = attackPower;

    }
	
	public void BindingTipsData(uint itemId,int id){
		this._tips.Iteminfo = new ItemInfo(itemId,id ,0);
	}
	
}
