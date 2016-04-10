using UnityEngine;
using System.Collections;
using helper;

/// <summary>
/// 挑战目标奖励item
/// </summary>
public class DemonItem : MonoBehaviour {

    public bool IsLock { get; set; }
    public int Id { get; set; }
    private UILabel lbl_Nadu;
    private UILabel lbl_level;
    private UILabel lbl_result;
    private GameObject select;
    private GameObject none;
    private GameObject noComplate;
    private void Awake()
    {
        lbl_Nadu = transform.FindChild("lbl1").GetComponent<UILabel>();
        lbl_level = transform.FindChild("lbl2").GetComponent<UILabel>();
        lbl_result = transform.FindChild("lbl3").GetComponent<UILabel>();
        select = transform.FindChild("button_select").gameObject;
        none = transform.FindChild("button_none").gameObject;
        noComplate = transform.FindChild("button_nowancheng").gameObject;
    }



    public void Display(string diff, string level, string result, bool isLock,bool isCompalte)
    {
        IsLock = isLock;

        if (isCompalte)
        {
            if (isLock)
            {
                select.SetActive(!isLock);
                none.SetActive(isLock);
            }
            else
            {
                select.SetActive(!isLock);
                none.SetActive(isLock);
            }
            noComplate.SetActive(false);
        }
        else {
            noComplate.SetActive(true);
            select.SetActive(false);
            none.SetActive(false);
        }
        

        lbl_Nadu.text = ColorConst.Format(ColorConst.Color_Blue, diff);
        lbl_level.text = ColorConst.Format(ColorConst.Color_Blue, level);
        lbl_result.text = result; 
    }
}
