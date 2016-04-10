using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;


namespace mediator
{
    public class FormulaMediator:ViewMediator
    {
        public FormulaMediator(FormulaView view, uint id = MediatorName.FORMULA_MEDIATOR):base(id,view)
        {

        }
        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_FORMULA_DISPLAY_LIST,
                MsgConstant.MSG_FORMULA_DISPLAY_INFO,
                MsgConstant.MSG_FORMULA_SELECT_LIST_ITEM,
                MsgConstant.MSG_FORMULA_SET_MERGE_COUNT,
                MsgConstant.MSG_FORMULA_MERGE_BUTTON,
                MsgConstant.MSG_FORMULA_SELECT_INDEX_ITEM,
            };
        }

        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_FORMULA_DISPLAY_LIST:
                        View.DisplayFormulaList();
                        break;
                    case MsgConstant.MSG_FORMULA_DISPLAY_INFO:
                        View.DisplayFormulaInfo();
                        break;
                    case MsgConstant.MSG_FORMULA_SELECT_LIST_ITEM:
                        FormulaManager.Instance.SetSelectItem((int)notification.body);
                        break;
                    case MsgConstant.MSG_FORMULA_SET_MERGE_COUNT:
                        FormulaManager.Instance.SetMergeNum((int)notification.body);
                        break;
                    case MsgConstant.MSG_FORMULA_MERGE_BUTTON:
                        FormulaManager.Instance.SendMergeInfo();
                        break;
                    case MsgConstant.MSG_FORMULA_SELECT_INDEX_ITEM:
                        View.SelectIndex();
                        break;
                    default:
                        break;
                }
            }
        }

        public FormulaView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is FormulaView)
                {
                    return base._viewComponent as FormulaView;
                }
                return null;
            }
        }
    }
}
