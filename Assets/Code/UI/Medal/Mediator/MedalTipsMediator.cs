using UnityEngine;
using System.Collections;
using mediator;
using model;
using MVC.entrance.gate;
using MVC.interfaces;
using System.Collections.Generic;
using manager;
namespace mediator{
	public class MedalTipsMediator:ViewMediator{

        private MedalTipsView _tipsView;
        public MedalTipsMediator(MedalTipsView view, uint id = MediatorName.MEDAL_TIP_MEDIATOR)
            : base(id, view)
        {
            _tipsView = view;
        }



        public override IList<uint> listReferNotification()
        {
            return new List<uint>() {
                MsgConstant.MSG_MEDAL_BAG_MEDAL_SHOW
            };
        }

        public override void handleNotification(INotification notification)
        {
            if (_tipsView != null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_MEDAL_BAG_MEDAL_SHOW:
                        _tipsView.DisplayMedalTips();
                        break;
                    default:
                        break;
                }
            }
        }
	}
}

