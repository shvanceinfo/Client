using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;

namespace mediator
{
    public class SettingMediator:ViewMediator
    {
        public SettingMediator(SettingView view, uint id = MediatorName.SETTING_MEDIATOR):base(id,view)
        {

        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_SETTING_SWTICHING_TABLE,
                MsgConstant.MSG_SETTING_SUBMIT,
                MsgConstant.MSG_SETTING_RELOGIN,
                MsgConstant.MSG_SETTING_SETAUDIO,
                MsgConstant.MSG_SETTING_SETMUSIC,
                MsgConstant.MSG_SETTING_MOVEHELP,
                MsgConstant.MSG_SETTING_HIDE,
                MsgConstant.MSG_SETTING_PEOPLE_OPTION
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View != null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_SETTING_SWTICHING_TABLE:
                        View.Swtiching((Table)notification.body);
                        break;
                    case MsgConstant.MSG_SETTING_CHECK_BOX:
                        SettingManager.Instance.SetCheckBox((uint)notification.body);
                        break;
                    case MsgConstant.MSG_SETTING_SUBMIT:
                        SettingManager.Instance.SendQustion();
                        break;
                    case MsgConstant.MSG_SETTING_RELOGIN:
                        UIManager.Instance.ShowDialog(eDialogSureType.eReturnLogin, "确定重新登录？");
                        break;
                    case MsgConstant.MSG_SETTING_SETAUDIO:
                        SettingManager.Instance.SetAudio();
                        break;
                    case MsgConstant.MSG_SETTING_SETMUSIC:
                        SettingManager.Instance.SetMusic();
                        break;
                    case MsgConstant.MSG_SETTING_MOVEHELP:
                        View.MoveSelect((bool)notification.body);
                        break;

                    case MsgConstant.MSG_SETTING_PEOPLE_OPTION:
                        SettingManager.Instance.OnChangePeopleOption((int)notification.body);
                        break;

                    default:
                        break;
                }
            }
        }

        public SettingView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is SettingView)
                {
                    return base._viewComponent as SettingView;
                }
                return null;
            }
        }
    }
}