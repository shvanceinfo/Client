using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class HeadBoardManager {

    private static HeadBoardManager instance;

    private UnityEngine.Object prefab;

    public Dictionary<int, GameObject> m_billBoardContainer = new Dictionary<int, GameObject>();

    HeadBoardManager()
    {
        prefab = BundleMemManager.Instance.loadResource("Prefab/UiCommon/head_board");
    }

    static public HeadBoardManager GetInstance()
    {
        if (instance == null)
        {
            instance = new HeadBoardManager();
        }

        return instance;
    }


    public void SetHeadBoard(int playerid, Transform parent, string name, int titleid = 0)
    {
        // 最初始的时候id为0直接pass
        if (playerid == 0)
        {
            return;
        }

        if (!m_billBoardContainer.ContainsKey(playerid))
        {
            m_billBoardContainer.Add(playerid, GenerateBillBoard(parent, name, titleid));
        }
        else
        {
            GameObject.Destroy(m_billBoardContainer[playerid]);
            m_billBoardContainer.Remove(playerid);
            m_billBoardContainer.Add(playerid, GenerateBillBoard(parent, name, titleid));
        }
 

        // 判断是否在主城中， 主城才显示
        m_billBoardContainer[playerid].SetActive(Global.inCityMap());
    }

    //public void DestroyBoard(int playerid)
    //{
    //    if (m_billBoardContainer.ContainsKey(playerid))
    //    {
    //        GameObject.Destroy(m_billBoardContainer[playerid]);
    //    }
    //}

    public GameObject GenerateBillBoard(Transform parent, string name, int titleid = 0)
    {

        GameObject billboard = new GameObject();
        billboard.transform.parent = parent;
        billboard.transform.localPosition = new Vector3(0.0f, 1.2f, 0.0f);
        billboard.transform.localScale = Vector3.one;
        
        billboard.AddComponent("BillBoard");


        GameObject obj = BundleMemManager.Instance.instantiateObj(prefab);
        obj.name = "text";
        obj.transform.parent = billboard.transform;
        obj.transform.localPosition = Vector3.zero;
        billboard.transform.localScale = Vector3.one;
        if (titleid != 0)
        {
            // 加载称号图标
        }
        else
        {
            obj.transform.FindChild("title/icon").GetComponent<UISprite>().gameObject.SetActive(false);
            obj.transform.FindChild("title/icon_bg").GetComponent<UISprite>().gameObject.SetActive(false);
            obj.transform.Find("title_text").GetComponent<UILabel>().gameObject.SetActive(false);
        }
        obj.transform.Find("name").GetComponent<UILabel>().text = name;
        return billboard;
    }
}
