using UnityEngine;
using System.Collections;
using LitJson;
using model;
using manager;
using System;

public class AskServerData : MonoBehaviour {
    public string url = "http://192.168.3.169/api/GetServerList.php";

    private int deviceType = 1;
    private int platformId = 50;
    private int version = 6;

    private void Start()
    {
        //StartCoroutine(AskServer());
    }

    private void platformChoose()
    {
        if (Application.platform == RuntimePlatform.Android)
            platformId = 0;
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            platformId = 1;
        else if(Application.platform == RuntimePlatform.WindowsEditor)
            platformId = 2;
    }

    public void AskData()
    {
        StartCoroutine(AskServer());
    }

    IEnumerator AskServer()
    {
		yield return 1;
		/*
        WWWForm form = new WWWForm();
        form.AddField("deviceType", deviceType);
        form.AddField("platformId", platformId);
        form.AddField("version", version);

        WWW download = new WWW(url,form);
        yield return download;
        if (download.error != null)
            Debug.LogError(download.error);

        DataReadServer.Instance._serverList.Clear();

        JsonData jd = JsonMapper.ToObject(download.text);
        for (int i = 0; i < jd.Count; i++)
        {
            ServerVo vo = new ServerVo();
            vo.ServerOpenState = Convert.ToBoolean( int.Parse( jd[i]["server_on_off"].ToString() ) );
            vo.Id = i + 1;
            vo.ServerId = int.Parse(jd[i]["server_id"].ToString());
            vo.Name = jd[i]["server_name"].ToString();
            vo.OrderId = i;
            //vo.IpAddr = jd[i]["server_ip"].ToString();
            vo.IpAddr = "192.168.1.117";
            vo.Port = int.Parse( jd[i]["server_port"].ToString() );
            vo.ServerState = (ServState)(int.Parse( jd[i]["server_state"].ToString() ));
            if (!vo.ServerOpenState)
            {
                vo.ServerOpenState = true;
                vo.ServerState = ServState.NEW_SERVER;
            }

            int a = CacheManager.GetInstance().GetServerId();
            if (CacheManager.GetInstance().GetServerId() == vo.ServerId)
            {
                vo.ServerState = ServState.RECENT_SERVER;
                ServerManager.Instance.CurrentVo = vo;
                ServerManager.Instance.isRecent = true;
            }

            DataReadServer.Instance._serverList.Add(vo);
            Debug.Log(vo.ServerState);

        }
        DataReadServer.Instance.DataInfo();
        */
    }

}
