using UnityEngine;
using System.Collections;

/// <summary>
/// 通用物品图标
/// Id,Quality,Icon,Label,LabelEx
/// </summary>
public class ItemLabelEx : ItemLabel
{
    public string LableEx {
        set {
            if (_UILabelEx!=null)
            {
                if (value == null)
                {
                    _UILabelEx.alpha = 0;
                }
                else {
                    _UILabelEx.alpha = 1;
                    _UILabelEx.text = value;
                } 
            }
        }
    }
    private UILabel _UILabelEx;
    public override void Init()
    {
        base.Init();
        this._UILabelEx = transform.FindChild("LabelEx").GetComponent<UILabel>();
    }
}
