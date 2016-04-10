using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using MVC.interfaces;
using System.Collections.Generic;
using manager;
using model;

namespace mediator
{
    public class GuildSkillMediator : ViewMediator
    {
        private GuildSkillView _view;
        public GuildSkillMediator(GuildSkillView view, uint id = MediatorName.GUILDSKILL_MEDIATOR)
            : base(id, view)
        {
            _view = view;
        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint>
            {
                MsgConstant.MSG_COMMON_NOTICE_SKILL_REFRESH,
                MsgConstant.MSG_COMMON_NOTICE_SKILL_DISPLAYTABLE,
            };
        }

        public override void handleNotification(INotification notification)
        {
            if (_view != null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_COMMON_NOTICE_SKILL_REFRESH:
                        _view.SwitchSkillTable((GuildSkillType)notification.body);
                        //GuildManager.Instance.SkillTableSwting((GuildSkillType)notification.body);
                        break;
                    case MsgConstant.MSG_COMMON_NOTICE_SKILL_DISPLAYTABLE:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
