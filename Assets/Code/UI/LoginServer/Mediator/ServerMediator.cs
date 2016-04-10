using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using model;
using manager;

namespace mediator
{
    public class ServerMediator : ViewMediator
    {
        private ServerView _view;
        public ServerMediator(ServerView view, uint id = MediatorName.SERVER_CHOOSE)
            : base(id, view)
        {
            _view = view;
        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_SELECT_SERVER_SHOW,
                MsgConstant.MSG_CHOOSE_SERVER_OPTION,
                MsgConstant.MSG_CHOOSE_SERVER_OPTION_UNIQUE,
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (_view != null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_SELECT_SERVER_SHOW:
                        _view.ArrowShow();
                        _view.DisplayView();
                        break;
                    case MsgConstant.MSG_CHOOSE_SERVER_OPTION:
                        _view.GetServerIp((int)notification.body);
                        break;
                    case MsgConstant.MSG_CHOOSE_SERVER_OPTION_UNIQUE:
                        _view.chooseIpAndPortNew((int)notification.body);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
