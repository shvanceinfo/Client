using UnityEngine;
using System.Collections;

public class MergeDisplayItem : MonoBehaviour {

    public int Id { get; set; }
    private UISprite _boder;
    private UILabel _count;
    private UILabel _name;
    private UITexture _icon;
	BtnTipsMsg _tips;
    void Awake()
    {
        _boder = transform.FindChild("boder").GetComponent<UISprite>();
        _count = transform.FindChild("count").GetComponent<UILabel>();
        _name = transform.FindChild("name").GetComponent<UILabel>();
        _icon = transform.FindChild("icon").GetComponent<UITexture>();
		_tips = transform.FindChild("icon").GetComponent<BtnTipsMsg>();
    }

    public void Display(int id, string icon, string name, int count, string boder)
    {
        Id = id;
        _icon.mainTexture = SourceManager.Instance.getTextByIconName(icon);
        
        _boder.spriteName = boder;
        _count.text = "";
        if (count == 0)
        {
            _name.text = name;
        }
        else {
            _name.text = name+string.Format("  ({0})",count); 
        }
        
    }
	
	public void BindingTipsData(uint itemId){
		this._tips.Iteminfo = new ItemInfo(itemId,0,0);
	}
	
}
