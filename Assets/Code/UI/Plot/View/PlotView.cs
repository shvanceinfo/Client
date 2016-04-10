using UnityEngine;
using System.Collections;
using mediator;
using model;
using manager;
using helper;

public class PlotView : HelperMono {

    private UIGrid _gridPlot;
    private GameObject _prePlot;

    #region Register
    protected override MVC.entrance.gate.ViewMediator Register()
    {
        return new PlotMediator(this);
    }
    protected override uint RemoveMediator()
    {
        return MediatorName.PLOT_MEDIATOR;
    }
    #endregion

    private void Awake()
    {
        _gridPlot = F<UIGrid>("panel/grid");
        _prePlot = F("panel/item");
        _prePlot.SetActive(false);
    }

    public void DisplayPlotList()
    {
        BetterList<PlotVo> list = PlotManager.Instance.Maps;

        ViewHelper.FormatTemplate<BetterList<PlotVo>, PlotVo>(_prePlot, _gridPlot.transform,list,
            (PlotVo vo, Transform t) =>
            {
                UILabel title = t.F<UILabel>("title/Label");
                UITexture normal = t.F<UITexture>("normalicon");
                //首先判断是否解锁了
                if (vo.IsUnLock)
                {
                    //判断是否有奖励需要领取
                    if (vo.IsReceive)
                    {
                        Swting(3, t);
                    }
                    else {
                        if (PlotManager.Instance.IsRaid)
                        {

                        }
                        else { 
                            
                        }
                    }
                }
                else {
                    //关闭cbk脚本
                    UICheckBoxObject cbk=t.GetComponent<UICheckBoxObject>();
                    cbk.isChecked = false;
                    cbk.enabled = false;

                    //设置灰色颜色
                    title.text = ViewHelper.FormatLanguage("plot_locktitle", vo.Name);
                    normal.color = new Color(0.004f, 0, 0, 1);

                    //设置解锁提示
                    Swting(0,t);
                    t.F<UILabel>("state0/Label").text = ViewHelper.FormatLanguage("plot_unlockmsg",vo.UnLockLevel);
                }
            });
    }
    private void Swting(int index,Transform t)
    {
        for (int i = 0; i < 5; i++)
        {
            t.F("state" + i).SetActive(false);
        }
        t.F("state" + index).SetActive(true);
    }
}
