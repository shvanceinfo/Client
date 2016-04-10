/**该文件实现的基本功能等
function: 实现游戏币，金币，新邮件，新物品（服务器发送过来的相关数据变动时后通知UI层发生变化）
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections;

public class DataChangeNotifyer 
{	
	//通知金钱发生变化
	public delegate void ChangeMoney(int moneyNum);
	//通知游戏币发生变化
    public delegate void ChangeCurrency(int currencyNum);
    //email的数目或者EMail的变化
    public delegate void ChangeEmail(int unreadNum, int emailNum);
    //物品类型的参数
    public delegate void ChangeItem(eItemType type);
    //加载完成，通知主线程做其它事情
    public delegate void FinishLoading();
    
    public ChangeMoney EventChangeMoney;
    public ChangeCurrency EventChangeCurrency;
    public ChangeEmail EventChangeEmail;
    public ChangeItem EventChangeItem;
    public FinishLoading EventFinishLoading;
    
    private static DataChangeNotifyer _instance; //数据变化通知的单例
    public static DataChangeNotifyer GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DataChangeNotifyer();
        }
        return _instance;
    }
    
    private DataChangeNotifyer()
    {
    	EventChangeMoney = null;
	    EventChangeCurrency = null;
	    EventChangeEmail = null;
	    EventChangeItem = null;
	    EventFinishLoading = null;
    }
    
    public void OnChangeMoney(int moneyNum)
    {
        if (EventChangeMoney != null)
            EventChangeMoney(moneyNum);
    }
    
    public void OnChangeCurrency(int currencyNum)
    {
        if (EventChangeCurrency != null)
            EventChangeCurrency(currencyNum);
    }
    
    public void OnChangeEmail(int unReadNum, int emailNum)
    {
        if (EventChangeEmail != null)
            EventChangeEmail(unReadNum, emailNum);
    }
    
    public void OnChangeItem(eItemType type)
    {
        if (EventChangeItem != null)
            EventChangeItem(type);
    }
    
    public void OnFinishLoading()
    {
    	if(EventFinishLoading != null)
    		EventFinishLoading();
    }
}
