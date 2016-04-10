using UnityEngine;
using System.Collections;
using manager;
using model;
using System.Collections.Generic;
using mediator;
using MVC.entrance.gate;

public class ServerView : MonoBehaviour {

    private GameObject prefabItem;
    private GameObject leftBtn, rightBtn;
    private List<GameObject> serverPref = new List<GameObject>();
    private void Awake()
    {
        prefabItem = transform.FindChild("ServerList/Row").gameObject;
        leftBtn = transform.FindChild("ServerListControl/Btn_Left").gameObject;
        rightBtn = transform.FindChild("ServerListControl/Btn_Right").gameObject;

    }

    private void OnEnable()
    {
        Gate.instance.registerMediator(new ServerMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.SERVER_CHOOSE);
    }

    public void ArrowShow()
    {
        if (ServerManager.Instance.ServerList.size/6 + 1 == 1)
        {
            leftBtn.SetActive(false);
            rightBtn.SetActive(false);
        }
        else if (ServerManager.Instance.ServerList.size/6 + 1 == 2)
        {
            leftBtn.SetActive(false);            
        }
    }

    public void SortData()
    { 
        
    }

    public void DisplayView()
    {
        for (int i = 0; i < serverPref.Count; i++)
        {
            if (serverPref[i] != null)
                Destroy(serverPref[i]);
        }
        int a = ServerManager.Instance.ServerList.size;
        List<GameObject> list = new List<GameObject>();
        
        for (int i = 0; i < a/6+1; i++)
        {
            prefabItem.SetActive(true);
            //添加父列表
            GameObject obj = Instantiate(prefabItem) as GameObject;
            obj.transform.parent = transform.FindChild("ServerList/Grid/Table");
            obj.transform.localPosition = new Vector3( 630*i,0,0 );
            obj.transform.localScale = Vector3.one;
            obj.transform.name = i.ToString();

            obj.GetComponent<TweenPosition>().from = new Vector3(630 * i, 0, 0);
            obj.GetComponent<TweenPosition>().to = new Vector3(630 * i + -630, 0, 0);
            list.Add(obj);
            serverPref.Add(obj);
            prefabItem.SetActive(false);

            //修改子列表名称,子列表赋值
            for (int k = 0; k < 6; k++)
            {
                list[i].transform.GetChild(k).name = (i*6+k).ToString();

                ServerVo vo = ServerManager.Instance.FindVoByOrderId( i*6+k );
                DisplayInfo( vo,list[i].transform.GetChild(k).gameObject );
                if (vo != null)
                {
                    ServerState(list[i].transform.GetChild(k).gameObject, vo, vo.ServerState);
                    ServerOpenState(list[i].transform.GetChild(k).gameObject,vo);
                }
                if (vo!= null && !vo.ServerOpenState)
                {
                    list[i].transform.GetChild(k).GetComponent<BoxCollider>().enabled = false;
                }

            }

        }
        //清除多余的item
        for (int i = a%6; i < 6;i++ )
        {
            if (i < list[list.Count - 1].transform.childCount)
                Destroy(list[list.Count-1].transform.GetChild(i).gameObject);
        }


    }


    private void ServerOpenState(GameObject obj, ServerVo vo)
    {
        if (!vo.ServerOpenState)
            obj.transform.FindChild("state").GetComponent<UISprite>().enabled = false;
    }

    private void ServerState(GameObject obj, ServerVo vo, ServState state)
    {
        switch (state)
        {
            case ServState.CLOSED:
                obj.GetComponent<UIToggle>().enabled = false;
                obj.transform.FindChild("state").GetComponent<UISprite>().enabled = false;
                obj.transform.FindChild("select").GetComponent<UISprite>().enabled = false;
                break;
            case ServState.NORMAL_SERVER:
                obj.transform.FindChild("state").GetComponent<UISprite>().enabled = false;
                break;
            case ServState.HOT_SERVER:
                obj.transform.FindChild("state").GetComponent<UISprite>().enabled = true;
                break;
            case ServState.RECENT_SERVER:
                transform.FindChild("Soon/serverRecent/Label").GetComponent<UILabel>().text = vo.Name;
                break;
            case ServState.NEW_SERVER:
                obj.transform.FindChild("state").GetComponent<UISprite>().enabled = true;
                obj.transform.FindChild("state").GetComponent<UISprite>().spriteName = "denglu_fwq_xin";
                break;
            case ServState.RECOMMEND_SERVER:
                transform.FindChild("Recommend/serverRecommend/Label").GetComponent<UILabel>().text = vo.Name;
                break;

            default:
                break;
        }
    }

    //赋值Item
    private void DisplayInfo(ServerVo vo,GameObject obj)
    {
        if(obj != null && vo != null)
            obj.transform.FindChild("Label").GetComponent<UILabel>().text = vo.Name;
    }

    public void GetServerIp(int id)
    {
        ServerVo vo = ServerManager.Instance.FindVoByOrderId(id);
        chooseIpAndPort(vo);
    }

    private void chooseIpAndPort(ServerVo vo)
    {
        GameObject logic = GameObject.Find("Logic");
        logic.GetComponent<MainLogic>().localIP = vo.IpAddr;
        logic.GetComponent<MainLogic>().port = vo.Port;
        Debug.Log("ip:--" + vo.IpAddr + "---port:--" + vo.Port);
    }

    public void chooseIpAndPortNew(int id)
    {
        ServerVo vo = ServerManager.Instance.FindVoById(id);
        GameObject logic = GameObject.Find("Logic");
        logic.GetComponent<MainLogic>().localIP = vo.IpAddr;
        logic.GetComponent<MainLogic>().port = vo.Port;
        Debug.Log("ip:--" + vo.IpAddr + "---port:--" + vo.Port);
    }
}
