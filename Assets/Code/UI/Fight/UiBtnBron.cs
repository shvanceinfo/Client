using UnityEngine;
using System.Collections;

public class UiBtnBron : MonoBehaviour
{
    void OnClick()
    {
    	if(this.name.Equals("reviveBtn")) //复活
    	{
        	OnClose();
    	}
    	else //退出副本
    	{
    		UIManager.Instance.closeWindow(UiNameConst.ui_born, true); //删除弹出框控件
    		ReturnToCity.Instance.ReturnType = ReturnToCity.RETURN_TYPE.LOSE_BATTLE; //关卡战斗失败
    	}
    }

    void OnClose()
    {
//        if (Global.inFightMap() || Global.inMultiFightMap())
//        {
//
//            int needPrice = ConfigDataManager.GetInstance().GetReliveConfig().GetReliveData(Global.requestBornNum).dia_price;
//
//            int diamondNum = Global.GetBornDiamond();
//
//            if (diamondNum >= needPrice)
//            {
//                MessageManager.Instance.sendMessageBorn(needPrice);
//                Global.requestBornNum += 1;
//            }
//            
//        }        
//        UIManager.Instance.closeWindow(UiNameConst.ui_born, true);
    } 
}
