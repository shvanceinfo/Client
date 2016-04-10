using UnityEngine;
using System.Collections;
using MVC.entrance.gate;

public class BtnDeathMsg : MonoBehaviour {


    private const string toCity = "Btn_GoCity";
    private const string revive = "Btn_Revive";

    void OnClick()
    {
        switch (gameObject.name)
        {
            case toCity: Gate.instance.sendNotification(MsgConstant.MSG_DEATH_TO_CITY);
                break;
            case revive: Gate.instance.sendNotification(MsgConstant.MSG_DEATH_REVIVE);
                break;
            default:
                break;
        }
    }
}
