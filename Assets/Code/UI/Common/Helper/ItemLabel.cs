using UnityEngine;
using System.Collections;

/// <summary>
/// 通用物品图标
/// Id,Quality,Icon,Label
/// </summary>
public class ItemLabel : ItemHelper
{
    public string Lable {
        set {
            if (_UILabel!=null)
            {
                if (value == null)
                {
                    _UILabel.alpha = 0;
                }
                else {
                    _UILabel.alpha = 1;
                    _UILabel.text = value;
                }
            }
        }
    }
    private UILabel _UILabel;
    public override void Init()
    {
        base.Init();
        this._UILabel = transform.FindChild("Label").GetComponent<UILabel>();
    }

}