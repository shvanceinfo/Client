using UnityEngine;
using System.Collections;
using MVC.interfaces;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;
using model;

namespace mediator
{
    public class GuildFlagMediator : ViewMediator
    {
        private GuildFlagView _view;
        public GuildFlagMediator(GuildFlagView view, uint id = MediatorName.GUILDFLAG_MEDIATOR)
            : base(id, view)
        {
            _view = view;
        }
        public override IList<uint> listReferNotification()
        {
            return new List<uint>
            {
                MsgConstant.MSG_COMMON_NOTICE_FLAG_SHOW,
                MsgConstant.MSG_COMMON_NOTICE_FLAG_DISPLAYTABLE
            };

        }

        public override void handleNotification(INotification notification)
        {
            if (_view != null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_COMMON_NOTICE_FLAG_SHOW:
                        GuildManager.Instance.FlagTableSwting((GuildFlagType)notification.body);
                        break;
                    case MsgConstant.MSG_COMMON_NOTICE_FLAG_DISPLAYTABLE:
                        _view.DisplayUI();
                        break;
                }
            }
 	
        }



     }
}