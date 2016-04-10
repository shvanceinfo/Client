using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;
using model;

public class ServerSelectMsg : MonoBehaviour {
    public bool isClicked;
	// Use this for initialization
	void Start () {
        if (ServerManager.Instance.CurrentVo != null)
        {
            if (transform.name == ServerManager.Instance.CurrentVo.Name)
            {
                transform.GetComponent<UIToggle>().value = true;
            }
        }
        if (transform.GetComponent<UIToggle>().startsActive)
        {
            ServerVo vo = ServerManager.Instance.FindVoByOrderId(int.Parse(transform.name));
            ServerManager.Instance.ChooseVo = vo;
            Gate.instance.sendNotification(MsgConstant.MSG_CHOOSE_SERVER_OPTION, vo.OrderId);
        }

        if (!ServerManager.Instance.isRecommend)
        {
            if (transform.name.Equals("serverRecommend"))
                transform.gameObject.SetActive(false);
        }

        if (!ServerManager.Instance.isRecent)
        {
            if (transform.name.Equals("serverRecent"))
                transform.gameObject.SetActive(false);
        }
	}

    void OnClick()
    {
        isClicked = true;
        switch (transform.name)
        {
            case "serverRecommend":
                ServerVo recVo = ServerManager.Instance.FindVoById(10000);
                ServerManager.Instance.ChooseVo = recVo;
                Gate.instance.sendNotification(MsgConstant.MSG_CHOOSE_SERVER_OPTION_UNIQUE,recVo.Id);
                break;
            case "serverRecent":
                ServerVo curVo = ServerManager.Instance.FindVoById(10003);
                ServerManager.Instance.ChooseVo = curVo;
                Gate.instance.sendNotification(MsgConstant.MSG_CHOOSE_SERVER_OPTION_UNIQUE, curVo.Id);
                break;
            int option;
            default:
            if (int.TryParse(gameObject.name, out option))
            {
                transform.GetComponent<UIToggle>().value = true;
                ServerVo vo = ServerManager.Instance.FindVoByOrderId(int.Parse(transform.name));
                ServerManager.Instance.ChooseVo = vo;
                Gate.instance.sendNotification(MsgConstant.MSG_CHOOSE_SERVER_OPTION, option);
                //ServerState(vo.ServerState);
            }
                break;
        }
    }

    
}
