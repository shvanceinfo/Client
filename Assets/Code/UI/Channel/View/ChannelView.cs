using UnityEngine;
using System.Collections;
using mediator;
using helper;
using System.Collections.Generic;
using model;
using manager;

public class ChannelView : HelperMono
{

    #region Register
    protected override MVC.entrance.gate.ViewMediator Register()
    {
        return new ChannelMediator(this);
    }
    protected override uint RemoveMediator()
    {
        return MediatorName.CHANNEL_MEDIATOR;
    }
    #endregion


    private GameObject _pre;
    private UIGrid _grid;
    private void Awake()
    {
        _pre = F("panel/item");
        _grid = F<UIGrid>("panel/grid");
    }
    private void Start()
    {
        Display();
    }

    private void Display()
    {
        ViewHelper.FormatTemplate<List<ChannelLineVo>, ChannelLineVo>(_pre, _grid.transform, ChannelManager.Instance.Lines,
            (ChannelLineVo v1, ChannelLineVo v2, Transform t) =>
            {
                ChannelLineVo cur = ChannelManager.Instance.CurLine;
                UIToggle left = t.F<UIToggle>("left");
                UIToggle right = t.F<UIToggle>("right");
                left.value = right.value = false;
                left.gameObject.SetActive(true);
                right.gameObject.SetActive(true);
                if (v1 != null)
                {
                    if (v1.Id == cur.Id) left.value = true;
                    t.F<UILabel>("left/lbl1").text = ViewHelper.FormatLanguage("channel_line", v1.Id);
                    t.F<UILabel>("left/lbl2").text = FormatStatus(v1.Type);
                }
                else {
                    left.gameObject.SetActive(false);
                }

                if (v2 != null)
                {
                    if (v2.Id == cur.Id) right.value = true;
                    t.F<UILabel>("right/lbl1").text = ViewHelper.FormatLanguage("channel_line", v2.Id);
                    t.F<UILabel>("right/lbl2").text = FormatStatus(v2.Type);
                }
                else {
                    right.gameObject.SetActive(false);
                }
            });
    }

    private string FormatStatus(ChannelType type)
    {
        switch (type)
        {
            case ChannelType.Max:
                return ViewHelper.FormatLanguage("channel_max");
            case ChannelType.Normal:
                return ViewHelper.FormatLanguage("channel_normal");
            case ChannelType.Free:
                return ViewHelper.FormatLanguage("channel_free");
            default:
                return "";
        }
    }
}
