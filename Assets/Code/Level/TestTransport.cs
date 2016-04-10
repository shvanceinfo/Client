using UnityEngine;
using System.Collections;
using manager;
using MVC.entrance.gate;

public class TestTransport : LevelEventHandler {
	
	public int id = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void onTrigger() 
    {
        Global.transportId = id;

        if (id == 1)
        {
            if (SweepManager.Instance.IsSweeping) //正在扫荡中不打开关卡界面
            {
                Gate.instance.sendNotification(MsgConstant.MSG_OPEN_SWEEP);
            }
            else
            {
                int mapId = MessageManager.Instance.my_property.getServerMapID();
                MapDataItem mapItem = ConfigDataManager.GetInstance().getMapConfig().getMapData(mapId);

                CharacterPlayer.sPlayerMe.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_MOVE, ParseBackPosition(mapItem.transferBack1));
                EasyTouchJoyStickProperty.SetJoyStickEnable(false);
                RaidManager.Instance.initRaid();
            }
        }
        else
        {
            if (id == 2 || id == 3)
            {
                int mapId = MessageManager.Instance.my_property.getServerMapID();
                if(ConfigDataManager.GetInstance().getMapConfig().getMapData(mapId).transferMapIDs[id-2] > 0)
                    MessageManager.Instance.sendMessageChangeScene(
                        ConfigDataManager.GetInstance().getMapConfig().getMapData(mapId).transferMapIDs[id - 2], false);
            }
            EventDispatcher.GetInstance().OnHUDNeedHideShow(false);
        }
	}

    Vector3 ParseBackPosition(string value)
    {
        string[] postion = value.Split(',');
        return new Vector3(float.Parse(postion[0]), float.Parse(postion[1]), float.Parse(postion[2]));
    }
}
