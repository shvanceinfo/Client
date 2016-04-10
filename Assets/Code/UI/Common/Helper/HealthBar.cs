/// <summary>
/// @Anchor yaogang
/// @time 2014.4.1
/// </summary>
using UnityEngine;
using System.Collections;

public enum BarType
{ 
    Sliced,
    Filled
}

/// <summary>
/// 血条共用组件
/// </summary>
public class HealthBar : MonoBehaviour
{
    public UISprite Background;
    public UILabel Label;

    public BarType Type;
    /// <summary>
    /// 最大值
    /// </summary>
    public int MaxValue
    {
        get { return _maxValue; }
        set
        {
            _maxValue = value;
            this.UpdateDisplaySprite();
        }
    }
    /// <summary>
    /// 当前值
    /// </summary>
    public int Value
    {
        get { return _value; }
        set
        {
            _value = value;
            this.UpdateDisplaySprite();
        }
    }

    public float MaxWidth
    {
        get { return _maxWidth; }
    }

    public float Fill
    {
        get { return (float)Value/MaxValue; }
    }

    private int _value;

    private int _maxValue;

    private float _maxWidth;

    
    private Vector3 _scale;
    private void Awake()
    {
		if(_value==0)_value=1;
		if(_maxValue==0)_maxValue=1;
        if (Background == null)
        {
            Transform t = transform.FindChild("loadingBg");
            if (t != null)
            {
                UISprite back = t.GetComponent<UISprite>();
                if (back != null) Background = back;
                
            }
        }

        if (Label == null)
        {
            Transform t = transform.FindChild("Label");
            if (t != null)
            {
                UILabel lbl = t.GetComponent<UILabel>();
                if (lbl != null) Label = lbl;
            }
        }
        if (Background != null)
        {
            if (Type == BarType.Filled)
            {
                Background.type = UISprite.Type.Filled;
                Background.invert = false;
                Background.fillAmount = 0;
                Background.fillDirection = UISprite.FillDirection.Horizontal;
                
            }
            else {
                _maxWidth = Background.width;
            }
        }

    }

    private void Start()
    {
        _scale = new Vector3(1, 1, 1);
        UpdateDisplaySprite();
    }

    
    //刷新显示UI
    public void UpdateDisplaySprite()
    {
        if (Background != null)
        {

            if (Type == BarType.Filled)
            {
                if (_value <= 0)
                {
                    Background.fillAmount = 0;
                }
                else
                {
                    Background.fillAmount = (float)_value / (float)_maxValue;
                }
            }
            else {
                float fill = 0;
                if (_value != 0 && _maxValue != 0)
                {
                    fill = (float)_value / (float)_maxValue;
					if (_value==_maxValue) {
					fill=1;
					}
                }
				
                if (fill == 0)
                {
                    Background.alpha = 0;
                }
                else if(fill <=1)
                {
                    Background.alpha = 1;
                    Background.width = (int)(_maxWidth * fill);
                    if (Background.width <= Background.minWidth)
                    {
                        float lesW = Background.minWidth / _maxWidth;   //剩余比例值
                        float scaleX = fill / lesW;                     //需要缩放的值
                        if (scaleX <= 0) scaleX = 0;
                        _scale = Background.transform.localScale;
                        _scale.x = scaleX;
                    }
                    else {
                        _scale.x = 1;       //复原缩小值
                    }
                    Background.transform.localScale = _scale;
                }
            }
        }
        if (Label != null)
        {
            Label.text = string.Format("{0}/{1}", _value, _maxValue);
        }
    }
}
