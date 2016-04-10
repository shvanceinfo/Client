using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using MVC.interfaces;
using manager;

namespace mediator
{
    public class MergeMediator:ViewMediator
    {
        public MergeMediator(MergeView view, uint id = MediatorName.MERGE_MEDIATOR)
            : base(id, view)
        { 
            
        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint>
            {
                MsgConstant.MSG_MERGE_DISPLAY_LIST,
                MsgConstant.MSG_MERGE_SELECT_ITEM,
                MsgConstant.MSG_MERGE_DISPLAY_GEM_INFO,
                MsgConstant.MSG_MERGE_SET_MERGE_COUNT,
                MsgConstant.MSG_MERGE_BUTTON_MERGE,
                MsgConstant.MSG_MERGE_SELECT_INDEX_ITEM,
            };
        }

        public override void handleNotification(INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_MERGE_DISPLAY_LIST:
                        View.DisplayGemList();
	                   break;
                    case MsgConstant.MSG_MERGE_DISPLAY_GEM_INFO:
                       View.DisplayGemInfo();
                       break;
                    case MsgConstant.MSG_MERGE_SELECT_ITEM:         //选中物体
                       MergeManager.Instance.SelectItem((int)notification.body);
                       break;
                    case MsgConstant.MSG_MERGE_SET_MERGE_COUNT:
                       MergeManager.Instance.SetMergeNum((int)notification.body);
                       break;
                    case MsgConstant.MSG_MERGE_BUTTON_MERGE:
                       MergeManager.Instance.SendMergeInfo();
                       break;
                    case MsgConstant.MSG_MERGE_SELECT_INDEX_ITEM:
                       View.SelectIndex();
                       break;
                }
            }
        }

        public MergeView View
        {

            get
            {
                if (base._viewComponent != null && base._viewComponent is MergeView)
                {
                    return base._viewComponent as MergeView;
                }
                return null;
            }
        }
    }
}