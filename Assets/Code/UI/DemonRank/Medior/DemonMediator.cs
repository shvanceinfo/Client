using System.Collections;
using MVC;
using MVC.entrance.gate;
using MVC.interfaces;
using System.Collections.Generic;
using manager;
namespace mediator
{
    public class DemonMediator : ViewMediator
    {

        public DemonMediator(DemonView view, uint id=MediatorName.DEMON_MEDIATOR)
            : base(id, view)
        { 
            
        }
        public override IList<uint> listReferNotification()
        {
            return new List<uint>{ 
            MsgConstant.MSG_DEMON_DISPLAY_ITEMLIST,
            MsgConstant.MSG_DEMON_RECEIVE_ITEM,
            MsgConstant.MSG_DEMON_DISPLAY_CUR_RANK,
            MsgConstant.MSG_DEMON_DISPLAY_RANK,
            MsgConstant.MSG_DEMON_RERUSH_CUR_RANK_UI,
            MsgConstant.MSG_DEMON_RECEIVE_AWARD,
            MsgConstant.MSG_DEMON_ENTER_TOWER,
            MsgConstant.MSG_DEMON_ENTER_HISTORY_AWARD,
            MsgConstant.MSG_DEMON_DIALOG_SURE,
            MsgConstant.MSG_DEMON_INITIAL_DATA,
            };
        }

        public override void handleNotification(INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_DEMON_RERUSH_CUR_RANK_UI:
                        View.DisplayRankItems((DemonDiffEnum)notification.body);
                        break;
                    case MsgConstant.MSG_DEMON_DISPLAY_ITEMLIST:
                        DemonDiffEnum level = (DemonDiffEnum)notification.body;
                        DemonManager.Instance.SetLevel(level);
                        DemonManager.Instance.TickCount();
                        View.DisplayLevelItems(level);
                        break;

                    case MsgConstant.MSG_DEMON_RECEIVE_ITEM:
                        int id = (int)notification.body;
                        DemonManager.Instance.SendReceiveItem(id);
                        break;

                    case MsgConstant.MSG_DEMON_DISPLAY_CUR_RANK:
                        View.DisplayCurRankView((bool)notification.body);
                        break;
                    case MsgConstant.MSG_DEMON_DISPLAY_RANK:
                        View.DisplayRankView((bool)notification.body);
                        break;
                    case MsgConstant.MSG_DEMON_RECEIVE_AWARD:               
                        DemonManager.Instance.CallBackReceiveItem();
                        break;
                    case MsgConstant.MSG_DEMON_ENTER_TOWER:
                        DemonManager.Instance.RequestEnterTower((int)notification.body);
                        break;
                    case MsgConstant.MSG_DEMON_ENTER_HISTORY_AWARD:
                        DemonManager.Instance.RequestHistoryData();
                        break;
                    case MsgConstant.MSG_DEMON_DIALOG_SURE:
                        DemonManager.Instance.DialogSure();
                        break;
                    case MsgConstant.MSG_DEMON_INITIAL_DATA:
                        DemonManager.Instance.Initial();
                        break;
                    default:
                        break;
                }
            }
        }



        public DemonView View
        {
            get {

                if (base.viewComponent!=null&&base.viewComponent is DemonView)
                {
                    return base._viewComponent as DemonView;
                }
                return null;
            }
        }
    }
}
