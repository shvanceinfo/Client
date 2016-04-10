using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;

namespace mediator
{
    public class GuildMediator:ViewMediator
    {
        public GuildView View { get; set; }
        public GuildMediator(GuildView view, uint id = MediatorName.GUILD_MEDIATOR):base(id,view)
        {
            this.View = view;
        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_GUILD_DISPLAY_TABLE,
                MsgConstant.MSG_GUILD_DISPLAY_MEMBER_TOGGLE,
                MsgConstant.MSG_GUILD_DISPLAY_MEMBER_INFO,
                MsgConstant.MSG_GUILD_DISPLAY_MEMBER_OFFICE
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_GUILD_DISPLAY_TABLE:
                        switch ((Table)notification.body)
                        {
                            case Table.Table1: View.DisplayInfoTable();
                                break;
                            case Table.Table2: View.DisplayMemberTable();
                                break;
                            case Table.Table3: View.DisplayEventTable();
                                break;
                            case Table.Table4: View.DisplayBuildTable();
                                break;
                            case Table.Table5: View.DisplayMemberCheckTable();
                                break;
                            case Table.Table6: View.DisplayLogTable();
                                break;
                            default:
                                break;
                        }
                        break;
                    case MsgConstant.MSG_GUILD_DISPLAY_MEMBER_TOGGLE:
                        GuildManager.Instance.MbShowOflineMember = (bool)notification.body;
                        View.DisplayMemberTable();
                        break;
                    case MsgConstant.MSG_GUILD_DISPLAY_MEMBER_INFO:
                        int index = (int)notification.body;
                        if (index < 0)
                        {
                            View.SetActiveMemberInfo(false);
                        }
                        else {
                            GuildManager.Instance.SetSelectMember(index);
                            View.DisplayMemberInfoPanel();
                        }
                        break;
                    case MsgConstant.MSG_GUILD_DISPLAY_MEMBER_OFFICE:
                        if ((bool)notification.body)
                        {
                            View.SetActiveMemberInfo(false);
                            View.DisplayOfficeManager();
                        }
                        else {
                            View.SetActiveOfficeManager(false);
                        }
                        
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
