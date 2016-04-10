using UnityEngine;
using System.Collections;
using mediator;
using manager;
using model;

public class GuideView : HelperMono {


    private GameObject _prePoint;

    private void Awake()
    {
        _prePoint = transform.FindChild("Point").gameObject;
        _prePoint.SetActive(false);
    }
    protected override MVC.entrance.gate.ViewMediator Register()
    {
        return new GuideMediator(this);
    }
    protected override uint RemoveMediator()
    {
        return MediatorName.GUIDE_MEDIATOR;
    }




    private void OnGUI()
    {
    	return;
#if UNITY_EDITOR
        GUILayout.Label("当前已激活非强制列表");
        foreach (GuideVo vo in GuideManager.Instance.ActiveNoEnforceList)
        {
            GUILayout.Label("Id:" + vo.Id.ToString());
        }
        GUILayout.Label("当前已激活强制列表");
        foreach (GuideVo vo in GuideManager.Instance.ActiveEnforceList)
        {
            GUILayout.Label("Id:" + vo.Id.ToString());
        }
       

#endif
    }
}
