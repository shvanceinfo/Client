using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using helper;
using System.Collections.Generic;

public class GuildFlagView : MonoBehaviour
{
    UILabel label;

    UITexture _iconimgflag;
    UILabel _iconmlblflag;

    UILabel _costflag;
    UILabel _richflag;

    private GameObject flagObj;
    private Transform leftGrid, rightGrid;

    public void Awake()
    {

        label = F<UILabel>("flagdetial/info/Label");
        flagObj = transform.FindChild("flagdetial/info/Label").gameObject;
        leftGrid = transform.FindChild("flagdetial/info/current/Left/LeftGrid");
        rightGrid = transform.FindChild("flagdetial/info/next/Right/RightGrid");

        //旗帜(图标，名字)
        _iconimgflag = transform.Find("flagdetial/info/icon/img").GetComponent<UITexture>();
        _iconmlblflag = F<UILabel>("flagdetial/info/icon/lbl");

        //财产 (公会财产，升级需求)
        _costflag = F<UILabel>("flagdetial/info/cost/content");
        _richflag = F<UILabel>("flagdetial/info/rich/content");
        
    }

    void OnEnable()
    {
        Gate.instance.registerMediator(new GuildFlagMediator(this));
    }
    void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.GUILD_MEDIATOR);
    }

    public void DisplayUI()
    {
        DisplayLeftView();
        DisplayRightView();
    }


    private void DisplayLeftView()
    {
        GuildFlagVo vo = new GuildFlagVo();
        vo = GuildManager.Instance.FindFlagVoByLevel(GuildManager.Instance.FlagGuildLevel);

        DisplayCurrentInfo(vo,leftGrid.transform);
        DisplayLeftView(vo);
        DisplayTexture();

        leftGrid.GetComponent<UIGrid>().Reposition();
    }

    private void DisplayRightView()
    {
        GuildFlagVo vo = new GuildFlagVo();
        vo = GuildManager.Instance.FindFlagVoByLevel(GuildManager.Instance.NextFlagGuildLevel);

        DisplayCurrentInfo(vo,rightGrid.transform);
        DisplayRightView(vo);

        rightGrid.GetComponent<UIGrid>().Reposition();
    }

    private void DisplayCurrentInfo(GuildFlagVo vo,Transform Grid)
    {
        BetterList<AttributeValue> list = GuildManager.Instance.FlagCurFlagVo.Attrubutes;
        BetterList<string> slist = new BetterList<string>();

        for (int i = 0; i < list.size; i++)
        {
            string str = EquipmentManager.GetEquipAttributeName(list[i].Type);
            string str1 = string.Format("[{0}]{1}:[-]{2}", ColorConst.Color_HeSe, str + "加成", list[i].Value);
            slist.Add(str1);
            flagObj.SetActive(true);
            flagObj.GetComponent<UILabel>().text = slist[i];
            ViewHelper.AddItemTemplatePrefab(flagObj.gameObject, Grid.transform, i);
        }
    }

    private void DisplayTexture()
    {
        _iconimgflag.mainTexture = SourceManager.Instance.getTextByIconName(GuildManager.Instance.FlagCurFlagVo.Icon, PathConst.ICON_PATH);
        _iconmlblflag.text = "升级旗帜  " + GuildManager.Instance.FlagGuildLevel.ToString() + "阶";
    }

    private void DisplayLeftView(GuildFlagVo vo)
    {
        Dictionary<GuildOffice, BetterList<AttributeValue>> OfficeAttrs = new Dictionary<GuildOffice, BetterList<AttributeValue>>();
        BetterList<string> slists = new BetterList<string>();
        
        DisplayOfficerInfo(GuildOffice.Leader, vo, 0,leftGrid.transform);
        DisplayOfficerInfo(GuildOffice.DeputyLeader, vo, 1, leftGrid.transform);
        DisplayOfficerInfo(GuildOffice.Statesman, vo, 2, leftGrid.transform);
        DisplayOfficerInfo(GuildOffice.Elite, vo, 3, leftGrid.transform);
    }

    private void DisplayRightView(GuildFlagVo vo)
    {
        Dictionary<GuildOffice, BetterList<AttributeValue>> OfficeAttrs = new Dictionary<GuildOffice, BetterList<AttributeValue>>();
        BetterList<string> slists = new BetterList<string>();

        DisplayOfficerInfo(GuildOffice.Leader, vo, 0, rightGrid.transform);
        DisplayOfficerInfo(GuildOffice.DeputyLeader, vo, 1, rightGrid.transform);
        DisplayOfficerInfo(GuildOffice.Statesman, vo, 2, rightGrid.transform);
        DisplayOfficerInfo(GuildOffice.Elite, vo, 3, rightGrid.transform);
    }

    private void DisplayOfficerInfo(GuildOffice office,GuildFlagVo vo,int i,Transform Grid)
    {
        string s = "";
        switch (office)
        {
            case GuildOffice.Leader:
                s = "会长额外";
            break;
            case GuildOffice.DeputyLeader:
                s = "副会长额外";
            break;
            case GuildOffice.Statesman:
                s = "元老额外";
            break;
            case GuildOffice.Elite:
                s = "精英额外";
            break;
            default:
            break;
        }
        if (vo.OfficeAttrs[office][i].Type != eFighintPropertyCate.eFPC_None)
        {
            string str = EquipmentManager.GetEquipAttributeName(vo.OfficeAttrs[office][i].Type);
            string str1 = string.Format("[{0}{1}]:[-]{2}", ColorConst.Color_HeSe, s + str + "加成", vo.OfficeAttrs[office][i].Value);
            flagObj.SetActive(true);
            flagObj.GetComponent<UILabel>().text = str1;
            ViewHelper.AddItemTemplatePrefab(flagObj.gameObject, Grid.transform, i);
        }
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