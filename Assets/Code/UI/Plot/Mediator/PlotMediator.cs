using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;

namespace mediator
{
    public class PlotMediator:ViewMediator
    {
        private PlotView _view;
        public PlotMediator(PlotView view,uint id=MediatorName.PLOT_MEDIATOR):base(id,view)
        {
            _view = view;
        }
        public override IList<uint> listReferNotification()
        {
            return new List<uint> 
            {
                MsgConstant.MSG_PLOT_DISPLAY_MAIN
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (_view!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_PLOT_DISPLAY_MAIN:
                        _view.DisplayPlotList();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}