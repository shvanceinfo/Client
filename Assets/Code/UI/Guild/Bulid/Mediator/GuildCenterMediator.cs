using UnityEngine;
using System.Collections;
using MVC.interfaces;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;
using model;

namespace mediator
{
    public class GuildCenterMediator : ViewMediator
    {
        private GuildCenterView _view;
        public GuildCenterMediator(GuildCenterView view, uint id = MediatorName.GUILDCENTER_MEDIATOR)
            : base(id, view)
        {
            _view = view;
        }

        public override IList<uint> listReferNotification()
        { 
            return new List<uint>
            {
                MsgConstant.MSG_COMMON_NOTICE_CENTER_SHOW,
                MsgConstant.MSG_COMMON_NOTICE_CENTER_REFRESH,
            };
        }

        public override void handleNotification(INotification notification)
        {
            if (_view != null)
            {
                switch (notification.notifyId)
                { 
                    case MsgConstant.MSG_COMMON_NOTICE_CENTER_SHOW:
                        GuildManager.Instance.CenterTableSwting(GuildFlagType.GuildFlag);
                        break;
                    case MsgConstant.MSG_COMMON_NOTICE_CENTER_REFRESH:
                        _view.DisplayView();
                        break;  
                    default:
                        break;
                }
            }
        }

    }
}
