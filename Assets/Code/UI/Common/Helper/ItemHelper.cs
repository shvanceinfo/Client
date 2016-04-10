using UnityEngine;
using System.Collections;
using manager;
using helper;

/// <summary>
/// 通用物品图标
/// Id,Quality,Icon
/// </summary>

public class ItemHelper : MonoBehaviour {

    public int Id { get; set; }


    public eItemQuality Quality {
        set {
            if (_UIBoder!=null)
            {
                _UIBoder.spriteName = BagManager.Instance.getItemBgByType(value, true);  
            }
        }
    }
    public eItemQuality QualityNoBoder
    {
        set
        {
            if (_UIBoder != null)
            {
                _UIBoder.spriteName = BagManager.Instance.getItemBgByType(value, false);
            }
        }
    }

    public string Icon
    {
        set
        {
            if (_UIIcon!=null)
            {
                if (value == null)
                {
                    _UIIcon.alpha = 0;
                }
                else
                {
                    _UIIcon.alpha = 1;
                    XmlHelper.CallTry(() =>
                    {
                        _UIIcon.mainTexture = SourceManager.Instance.getTextByIconName(value);
                    });
                } 
            }
        }
    }

    private UISprite _UIBoder;

    private UITexture _UIIcon;

    public virtual void Init()
    {
        _UIBoder = transform.Find("Quality").GetComponent<UISprite>();
        _UIIcon = transform.FindChild("Icon").GetComponent<UITexture>();
    }
    public void Awake()
    {
        Init();
    }    
}





