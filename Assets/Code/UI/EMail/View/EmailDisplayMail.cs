using UnityEngine;
using System.Collections;
using model;
using helper;
using MVC.entrance.gate;


/// <summary>
/// 更改邮件状态
/// </summary>
public class EmailDisplayMail : MonoBehaviour {

    public int  Id { get; set; }



    const string normalColor = "cda524";          //没读过的颜色
    const string readColor = "b0b0b1";            //读过的颜色

    UILabel _name;
    UISprite _state;
    private void Awake()
    {
        _name = transform.FindChild("name").GetComponent<UILabel>();
        _state = transform.FindChild("state").GetComponent<UISprite>();
    }


    public void Display(int id, string title, EmailState state,bool isHaveItem)
    {
        Id = id;
        switch (state)
        {
            case EmailState.NotRead:
                _name.text = ColorConst.Format(normalColor, title);
                break;
            default:
                _name.text = ColorConst.Format(readColor, title);
                break;
        }
        if (isHaveItem)
        {
            _state.alpha = 1;
        }
        else {
            _state.alpha = 0;
        }
    }

    public void OnClick()
    {
        Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_READ_EMAIL, Id);
    }
}
