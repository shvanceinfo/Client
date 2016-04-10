using UnityEngine;
public class EventDispatcher
{

    
    /// <summary>
    /// 地图选择
    /// </summary>
    /// <param name="mapData"></param>
    public delegate void EventMapSelect(GameObject mapData);

    //从WWW数据获取数据的回调
    public delegate void EventLoadFromWWW(Object obj);

    
    /// <summary>
    /// 碰撞事件
    /// </summary>
    /// <param name="c"></param>
    public delegate void CollisionHandler(Collision c);

    /// <summary>
    /// 副本等级选择
    /// </summary>
    /// <param name="level"></param>
    public delegate void EventSelectFightLevel(Global.eFightLevel level);
    /// <summary>
    /// 角色数据变化
    /// </summary>
    /// <param name="player"></param>
    public delegate void EventPlayerProperty();
    /// <summary>
    /// 角色资产信息
    /// </summary>
    public delegate void EventPlayerAsset();
    /// <summary>
    /// 角色等级
    /// </summary>
    public delegate void EventPlayerLevel();
    /// <summary>
    /// 连接服务器回掉
    /// </summary>
    public delegate void EventConnectedServer();
    /// <summary>
    /// 丢失连接回掉
    /// </summary>
    public delegate void EventLostConnectServer();
   
    /// <summary>
    /// 战斗中进度条变化
    /// </summary>
    /// <param name="value"></param>
    public delegate void HandleFightChangeSlider(int value);

    /// <summary>
    /// 对话框确认操纵
    /// </summary>
    /// <param name="type"></param>
    public delegate void HandleDialogSure(eDialogSureType type);
    /// <summary>
    /// 窗口名称
    /// </summary>
    /// <param name="name"></param>
    public delegate void HandleWindowName(string name);


    // 成就监听
    public delegate void HandleMissionReceiveChange(bool bReceive);

    // buff 回调
    public delegate void HandleBuffDisappear(BUFF_TYPE buffType);

    /// <summary>
    /// 寻路点寻到 回调
    /// </summary>
    public delegate void HandlePathFindingArrived();


    public delegate void HandleAttackActived();

    public delegate void HandleHUDNeedHideShow(bool bVisible);

    static private EventDispatcher instance;
    public enum eEventType
    {
        MOUSE_CLICK
    }

    static public EventDispatcher GetInstance()
    {
        if (instance == null)
        {
            instance = new EventDispatcher();
        }
        return instance;
    }

    public HandleWindowName EventWindowName;
    public void OnSetWindowName(string name)
    {
        if (EventWindowName != null)
        {
            EventWindowName(name);
        }
    }

    public event CollisionHandler CollisionEnter;
    void OnCollisionEnter(Collision c)
    {
        if (CollisionEnter != null)
        {
            CollisionEnter(c);
        }
    }

    public event CollisionHandler CollisionExit;
    void OnCollisionExit(Collision c)
    {
        if (CollisionExit != null)
        {
            CollisionExit(c);
        }
    }

    /// <summary>
    /// 地图选中事件
    /// </summary>
    public event EventMapSelect MapSelect;
    public void OnMapSelect(GameObject mapData)
    {
        if (MapSelect != null)
        {
            MapSelect(mapData);
        }
    }
    /// <summary>
    /// 选中关卡挑战难度
    /// </summary>
    public event EventSelectFightLevel SelectFightLevel;
    public void OnSelectFightLevel(Global.eFightLevel level)
    {
        if (SelectFightLevel != null)
        {
            SelectFightLevel(level);
        }
    }

    /// <summary>
    /// 角色变化通知
    /// </summary>
    public event EventPlayerProperty PlayerProperty;
    public void OnPlayerProperty()
    {
        if (PlayerProperty != null)
        {
            PlayerProperty();
        }
    }
    /// <summary>
    /// 资产变化
    /// </summary>
    public event EventPlayerAsset PlayerAsset;
    public void OnPlayerAsset()
    {
        if (PlayerAsset != null)
        {
            PlayerAsset();
        }
    }
    /// <summary>
    /// 等级变化
    /// </summary>
    public event EventPlayerLevel PlayerLevel;
    public void OnPlayerLevel()
    {
        if (PlayerLevel != null)
        {
            PlayerLevel();
        }
    }
    /// <summary>
    /// 成功连接服务器回掉
    /// </summary>
    public event EventConnectedServer ConnectedServer;
    public void OnConnectedServer()
    {
        if (ConnectedServer != null)
        {
            ConnectedServer();
        }
    }
    /// <summary>
    /// 丢失连接
    /// </summary>
    public event EventLostConnectServer LostConnectServer;
    public void OnLostConnectServer()
    {
        if (LostConnectServer != null)
        {
            LostConnectServer();
        }
    }

    /// <summary>
    /// 战斗中hp变化
    /// </summary>
    public event HandleFightChangeSlider FightChgangeHP;
    public void OnFightChangeHP(int value)
    {
        if (FightChgangeHP != null)
        {
            FightChgangeHP(value);
        }
    }
    /// <summary>
    /// 战斗中mp变化
    /// </summary>
    public event HandleFightChangeSlider FightChangMP;
    public void OnFightChangeMP(int value)
    {
        if (FightChangMP != null)
        {
            FightChangMP(value);
        }
    }
    /// <summary>
    /// 战斗中exp变化
    /// </summary>
    public event HandleFightChangeSlider FightChangeEXP;
    public void OnFightChangeEXP(int value)
    {
        if (FightChangeEXP != null)
        {
            FightChangeEXP(value);
        }
    }
    /// <summary>
    /// 监听对话框确认按钮
    /// </summary>
    public event HandleDialogSure DialogSure;
    public void OnDialogSure(eDialogSureType type)
    {
        if (DialogSure != null)
        {
            DialogSure(type);
        }
    }
	
	public delegate void HandleDialogCancel(eDialogSureType type);
	public event HandleDialogCancel DialogCancel;
    public void OnDialogCancel(eDialogSureType type)
    {
        if (DialogCancel != null)
        {
            DialogCancel(type);
        }
    }

    public delegate void HandleOpenFucn(bool isOpen);
    public HandleOpenFucn EventOpenFunc;
    public void OnOpenFunc(bool isOpen)
    {
        if (EventOpenFunc != null)
        {
            EventOpenFunc(isOpen);
        }
    }


    // 成就相关的监听
    public event HandleMissionReceiveChange MissionReceiveChange;
    public void OnMissionReceiveChange(bool bRecev)
    {
        if (MissionReceiveChange != null)
        {
            MissionReceiveChange(bRecev);
        }
    }
	
	
	public delegate void EventCloseOtherWindow();
	public EventCloseOtherWindow CloseOtherWindow;
	public void OnCloseOtherWindow()
	{
		if (null != CloseOtherWindow)
		{
			CloseOtherWindow();
		}
	}


    public event HandleBuffDisappear BuffDisappear;
    public void OnBuffDisappear(BUFF_TYPE type)
    {
        if (BuffDisappear != null)
        {
            BuffDisappear(type);
        }
    }

    public event HandlePathFindingArrived PathFindingArrived;
    public void OnPathFindingArrived()
    {
        if (PathFindingArrived != null)
        {
            PathFindingArrived();
        }
    }

    public event HandleAttackActived AttackActived;
    public void OnAttackActived()
    {
        if (AttackActived != null)
        {
            AttackActived();
        }
    }

    public event HandleHUDNeedHideShow HUDNeedHideShow;
    public void OnHUDNeedHideShow(bool bVisible)
    {
        if (HUDNeedHideShow != null)
        {
            HUDNeedHideShow(bVisible);
        }
    }
}
