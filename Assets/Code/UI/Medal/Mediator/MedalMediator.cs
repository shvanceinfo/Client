using UnityEngine;
using System.Collections;
using mediator;
using model;
using MVC.entrance.gate;
using MVC.interfaces;
using System.Collections.Generic;
using manager;
namespace mediator{
	public class MedalMediator:ViewMediator{

        private MedalView _view;
        private MedalTipsView _tipsView;
        public MedalMediator(MedalView view,uint id=MediatorName.MEDAL_MEDIATOR):base(id,view)
        {
            _view = view;
        }



        public override IList<uint> listReferNotification()
        {
            return new List<uint>() { 
                MsgConstant.MSG_MEDAL_DISPLAY_VIEW,
                MsgConstant.MSG_MEDAL_SWING_MEDAL_LEVELUP,
                MsgConstant.MSG_MEDAL_BAG_MEDAL_SHOW
            };
        }

        public override void handleNotification(INotification notification)
        {
            if (_view!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_MEDAL_DISPLAY_VIEW:
                        _view.DisplayMedal();
                        break;
                    case MsgConstant.MSG_MEDAL_SWING_MEDAL_LEVELUP:
                        MedalManager.Instance.MedalLevelUp();
                        _view.DisplayMedalLevelup();
                        break;
                    case MsgConstant.MSG_MEDAL_BAG_MEDAL_SHOW:
                        _view.DisplayMedalTips();
                        break;
                    default:
                        break;
                }
            }
        }
	}
}

