using UnityEngine;
using System.Collections;

public class RefineDisplayResetAttr : MonoBehaviour {

    public bool IsLock { get; set; }
    private UILabel _name;
    private UIToggle _checkBox;
    private UISprite _back;
    private void Awake()
    {
        _name = transform.FindChild("Label").GetComponent<UILabel>();
        _checkBox = transform.FindChild("CheckBox").GetComponent<UIToggle>();
        _back = transform.FindChild("back").GetComponent<UISprite>();
        IsLock = false;
    }

    public string AttributeName {
        set {
            _name.text = value;
        }
    }

    public bool IsCheck
    {
        set {
            _checkBox.value = value;
        }
        get {
            return _checkBox.value;
        }
    }
    public void LockCheckBox()
    {
        IsLock = true;
        _checkBox.gameObject.SetActive(false);
        _back.gameObject.SetActive(false);
    }
    public void UnLockCheckBox()
    {
        IsLock = false;
        _checkBox.gameObject.SetActive(true);
        _back.gameObject.SetActive(true);
    }

}
