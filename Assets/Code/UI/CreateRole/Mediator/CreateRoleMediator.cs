using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;



namespace mediator
{
    public class CreateRoleMediator : ViewMediator
    {
        public CreateRoleMediator(CreateRoleView view, uint id = MediatorName.CREATEROLE_MEDIATOR)
            : base(id, view)
        {

        }
        public override System.Collections.Generic.IList<uint> listReferNotification()
        {
            return new System.Collections.Generic.List<uint> 
            { 
                MsgConstant.MSG_CREATEROLE_SETNAME,
                MsgConstant.MSG_CREATEROLE_CREATE,
                MsgConstant.MSG_CREATEROLE_SELECT_CAREER,
                MsgConstant.MSG_CREATEROLE_DISPLAY_NAME,
                MsgConstant.MSG_CREATEROLE_DISPLAY_PLAYER,
				MsgConstant.MSG_CREATEROLE_ROLL_NAME,
                MsgConstant.MSG_CREATEROLE_ERROR,
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View != null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_CREATEROLE_SETNAME:
                        CreateRoleManager.Instance.SetName((string)notification.body); 
                        break;
                    case MsgConstant.MSG_CREATEROLE_CREATE:
                        CreateRoleManager.Instance.CreateRole(View.GetInput());
                        break;
                    case MsgConstant.MSG_CREATEROLE_SELECT_CAREER:
                        CreateRoleManager.Instance.SelectCharaterCareer((CHARACTER_CAREER)notification.body);
                        View.DisplayAnimation((CHARACTER_CAREER)notification.body);
                        break;
                    case MsgConstant.MSG_CREATEROLE_DISPLAY_NAME:
                        View.DisplayName((string)notification.body);
                        break;
                    case MsgConstant.MSG_CREATEROLE_ROLL_NAME:
                        CreateRoleManager.Instance.RandomName();
                        View.RotationRoll();
                        break;
                    case MsgConstant.MSG_CREATEROLE_ERROR:
                        View.DisplayError(notification.body.ToString());
                        break;
                    default:
                        break;
                }
            }
        }



        public CreateRoleView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is CreateRoleView)
                {
                    return base._viewComponent as CreateRoleView;
                }
                return null;
            }
        }
    }
}
