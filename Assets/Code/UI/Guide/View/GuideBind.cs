using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

/// <summary>
/// 新手引导按钮触发器
/// </summary>
public class GuideBind : MonoBehaviour {

    /// <summary>
    /// 触发器ID
    /// </summary>
    public int Id;
    
    private void OnClick()
    { 
        Gate.instance.sendNotification(MsgConstant.MSG_GUIDE_SEND_TRIGGER,
            new Trigger(model.TriggerType.ButtonClick,Id));
    }
}
