using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;
using helper;
namespace mediator
{
    public class FurnaceMediator:ViewMediator
    {
        public FurnaceMediator(FurnaceView view,uint id=MediatorName.FURNACE_MEDIATOR)
            : base(id,view)
        { 
            
        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_FURNACE_SWING_TABLE,
                MsgConstant.MSG_FURNACE_SWING_MERGE_TABLE,
                MsgConstant.MSG_FURNACE_DISPLAY_GEM,
                MsgConstant.MSG_FURNACE_DISPLAY_FORMULA,
            };
        }

        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_FURNACE_SWING_TABLE:
                        Table table = (Table)notification.body;
                        View.SwitchingLevel1Table(table);
                        break;
                    case MsgConstant.MSG_FURNACE_SWING_MERGE_TABLE:     //切换二级标签
                        View.SwitchingMergeTable((Table)notification.body);
                        break;
                    case MsgConstant.MSG_FURNACE_DISPLAY_GEM:
                        View.DisplayGem();
                        break;
                    case MsgConstant.MSG_FURNACE_DISPLAY_FORMULA:
                        View.DisplayFormula();
                        break;
                    default:
                        break;
                }
            }  
        }


        public  FurnaceView View
        {

            get
            {
                if (base._viewComponent != null && base._viewComponent is FurnaceView)
                {
                    return base._viewComponent as FurnaceView;
                }
                return null;
            }
        }
    }
}
