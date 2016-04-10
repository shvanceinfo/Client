using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using helper;
using System.Collections.Generic;

public class GuildCenterView : MonoBehaviour {
    UILabel label;
    private GameObject guildCenterObj; 
    
    UITexture _iconimgcenter;
    UILabel _iconmlblcenter;

    UILabel _costcenter;
    UILabel _richcenter; 
    
    private Transform leftGrid, rightGrid;

    private void Awake()
    {
        label = F<UILabel>("halldetial/info/Label");
        guildCenterObj = transform.FindChild("halldetial/info/Label").gameObject;

        leftGrid = transform.FindChild("halldetial/info/current/Left/LeftGrid");
        rightGrid = transform.FindChild("halldetial/info/next/Right/RightGrid");

        //大厅(图标，名字)
        _iconimgcenter = transform.Find("halldetial/info/icon/img").GetComponent<UITexture>();
        _iconmlblcenter = F<UILabel>("halldetial/info/icon/lbl");

        //财产 (公会财产，升级需求)
        _costcenter = F<UILabel>("halldetial/info/cost/content");
        _richcenter = F<UILabel>("halldetial/info/rich/content");
    }

    void OnEnable()
    {
        Gate.instance.registerMediator(new GuildCenterMediator(this));
    }
    void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.GUILDCENTER_MEDIATOR);
    }

    public void DisplayView()
    {
        DisplayLeftInfo(leftGrid);
        DisplayRightInfo(rightGrid);
        DisplayTexture();
    }

    private void DisplayLeftInfo(Transform Grid)
    {
        BetterList<int> list = new BetterList<int>();
        BetterList<string> slist = new BetterList<string>();

        GuildBaseVo vo = new GuildBaseVo();
        vo = GuildManager.Instance.FindBaseVoByLevel(GuildManager.Instance.InfoGuildLevel);
        list.Add((int) GuildManager.Instance.InfoGuildLevel);
        list.Add( vo.MaxMember );
        list.Add(vo.MaxFlagLevel);
        list.Add(vo.MaxShopLevel);
        list.Add(vo.MaxSkillLevel);

        slist.Add("公会等级");
        slist.Add("公会成员上限");
        slist.Add("公会旗帜上限");
        slist.Add("公会商城上限");
        slist.Add("公会技能等级研究上限");

        for (int i = 0; i < list.size; i++)
        {
            string str = string.Format("[{0}]{1}:[-]{2}",ColorConst.Color_DanHuang,slist[i],list[i].ToString());
            guildCenterObj.SetActive(true);
            guildCenterObj.GetComponent<UILabel>().text = str;
            ViewHelper.AddItemTemplatePrefab(guildCenterObj.gameObject, Grid.transform, i);
        }

        leftGrid.GetComponent<UIGrid>().Reposition();
    }

    private void DisplayRightInfo(Transform Grid)
    {
        BetterList<int> list = new BetterList<int>();
        BetterList<string> slist = new BetterList<string>();

        GuildBaseVo vo = new GuildBaseVo();
        vo = GuildManager.Instance.FindBaseVoByLevel((uint)GuildManager.Instance.CenterNextLvl);
        list.Add((int)GuildManager.Instance.CenterNextLvl);
        list.Add(vo.MaxMember);
        list.Add(vo.MaxFlagLevel);
        list.Add(vo.MaxShopLevel);
        list.Add(vo.MaxSkillLevel);

        slist.Add("公会等级");
        slist.Add("公会成员上限");
        slist.Add("公会旗帜上限");
        slist.Add("公会商城上限");
        slist.Add("公会技能等级研究上限");

        for (int i = 0; i < list.size; i++)
        {
            string str = string.Format("[{0}]{1}:[-]{2}", ColorConst.Color_DanHuang, slist[i], list[i].ToString());
            guildCenterObj.SetActive(true);
            guildCenterObj.GetComponent<UILabel>().text = str;
            ViewHelper.AddItemTemplatePrefab(guildCenterObj.gameObject, Grid.transform, i);
        }

        rightGrid.GetComponent<UIGrid>().Reposition();
    }


    private void DisplayTexture()
    {
        _iconimgcenter.mainTexture = SourceManager.Instance.getTextByIconName(GuildManager.Instance.CtCurCenterVo.Icon, PathConst.ICON_PATH);
        _iconmlblcenter.text = "升级大厅  " + GuildManager.Instance.InfoGuildLevel.ToString() + "级";
    }


    #region 读取组件位置方法

    protected T F<T>(string path) where T : Component
    {
        return this.transform.FindChild(path).GetComponent<T>();
    }

    protected GameObject F(string path)
    {
        return this.transform.FindChild(path).gameObject;
    }
    #endregion
}
