using UnityEngine;
using System.Collections;

public class MedalViewAttribute : MonoBehaviour {

    private UILabel _left;
    private UILabel _right;
    private void Awake()
    {
        _left = transform.FindChild("0").GetComponent<UILabel>();
        _right = transform.FindChild("1").GetComponent<UILabel>();
        _left.alpha = 0;
        _right.alpha = 0;
    }

    public string LeftText
    {
        get {
            return _left.text;
        }
        set {
            _left.alpha = 1;
            _left.text = value;
        }
    }
    public string RightText
    {
        get
        {
            return _right.text;
        }
        set
        {
            _right.alpha = 1;
            _right.text = value;
        }
    }
}
