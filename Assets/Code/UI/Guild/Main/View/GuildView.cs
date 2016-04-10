using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using mediator;
using manager;
using helper;
using model;


public class GuildView : HelperMono
{
    private void Awake()
    {
        InitialFuncObj();       //初始化功能表
        InitialInfoAwake();     //初始化信息页签
        InitialMemberAwake();
        InitialEventAwake();
        InitialBuildAwake();
        InitialMemberCheckAwake();
        InitialLogAwake();

        InitialMemberInfoAwake();
        InitialOfficeManagerAwake();
    }

    #region Function
    private Dictionary<Table, GameObject> _funcs;
    private Dictionary<Table, UICheckBoxColor> _cbks;
    private void InitialFuncObj()
    {
        _funcs = new Dictionary<Table, GameObject>();
        _funcs[Table.Table1] = F("info");
        _funcs[Table.Table2] = F("member");
        _funcs[Table.Table3] = F("event");
        _funcs[Table.Table4] = F("build");
        _funcs[Table.Table5] = F("checkmember");
        _funcs[Table.Table6] = F("incident");

        _cbks = new Dictionary<Table, UICheckBoxColor>();
        _cbks[Table.Table1] = F<UICheckBoxColor>("tabs/tab1");
        _cbks[Table.Table2] = F<UICheckBoxColor>("tabs/tab2");
        _cbks[Table.Table3] = F<UICheckBoxColor>("tabs/tab3");
        _cbks[Table.Table4] = F<UICheckBoxColor>("tabs/tab4");
        _cbks[Table.Table5] = F<UICheckBoxColor>("tabs/tab5");
        _cbks[Table.Table6] = F<UICheckBoxColor>("tabs/tab6");
    }

    //激活对应的功能Table
    private void ActiveFunctionTable(Table table)
    {
        if (_funcs.ContainsKey(table))
        {
            Dictionary<Table, GameObject>.Enumerator it=_funcs.GetEnumerator();
            while (it.MoveNext())
            {
                it.Current.Value.SetActive(false);
            }
            _funcs[table].SetActive(true);
        }
        if (_cbks.ContainsKey(table))
        {
            Dictionary<Table, UICheckBoxColor>.Enumerator it = _cbks.GetEnumerator();
            while (it.MoveNext())
            {
                it.Current.Value.isChecked = false;
            }
            _cbks[table].isChecked = true;
        }
    }
    #endregion

    #region Register
    protected override MVC.entrance.gate.ViewMediator Register()
    {
        return new GuildMediator(this);
    }
    protected override uint RemoveMediator()
    {
        return MediatorName.GUILD_MEDIATOR;
    }
    #endregion

    #region InfoTable

    //我的信息
    private UILabel _lblInfoOwnOffcer;
    private UILabel _lblInfoOwnContribution;
    private UILabel _lblInfoOwnSunContribution;

    //公会信息
    private UILabel _lblInfoLeaderName;
    private UILabel _lblInfoLevel;
    private UILabel _lblInfoRank;
    private UILabel _lblInfoMember;
    private UILabel _lblInfoGold;
    private UILabel _lblInfoSumGold;
    //公会旗帜
    private UILabel _lblInfoFlagLevel;
    private UITexture _txtInfoFlagIcon;

    //公会升级需求
    private UILabel _lblInfoLevelUpConsume;

    //公告
    private UIInput _inputInfoPost;

    private void InitialInfoAwake()
    {
        _lblInfoOwnOffcer = F<UILabel>("info/myinfo/up/info/pos/info");
        _lblInfoOwnContribution = F<UILabel>("info/myinfo/up/info/contribution/info");
        _lblInfoOwnSunContribution = F<UILabel>("info/myinfo/up/info/hiscontribution/info");

        _lblInfoLeaderName = F<UILabel>("info/guildbuild/info/leader/content");
        _lblInfoLevel = F<UILabel>("info/guildbuild/info/level/content");
        _lblInfoRank = F<UILabel>("info/guildbuild/info/rank/content");
        _lblInfoMember = F<UILabel>("info/guildbuild/info/people/content");
        _lblInfoGold = F<UILabel>("info/guildbuild/info/money/content");
        _lblInfoSumGold = F<UILabel>("info/guildbuild/info/hismoney/content");

        _lblInfoFlagLevel = F<UILabel>("info/guildbuild/info/icon/name");
        _txtInfoFlagIcon = F<UITexture>("info/guildbuild/info/icon/img");

        _lblInfoLevelUpConsume = F<UILabel>("info/guildbuild/levelup/info/content");

        _inputInfoPost = F<UIInput>("info/myinfo/down/Input");
    }

    /// <summary>
    /// 显示信息标签
    /// </summary>
    public void DisplayInfoTable()
    {
        ActiveFunctionTable(Table.Table1);
        GuildManager gm=GuildManager.Instance;
        //我的信息
        _lblInfoOwnOffcer.text = GuildManager.FormatOffcerString(gm.InfoOwnType);
        _lblInfoOwnContribution.text = gm.InfoOwnContribution.Value.ToString();
        _lblInfoOwnSunContribution.text = gm.InfoOwnTotalContribution.Value.ToString();

        //公会信息
        _lblInfoLeaderName.text = gm.InfoLeaderName;
        _lblInfoLevel.text = gm.InfoGuildLevel.ToString();
        _lblInfoRank.text = gm.InfoRankLevel.ToString();
        _lblInfoMember.text = string.Format("{0}/{1}",gm.InfoGuildCurMember,gm.InfoGuildMaxMember);
        _lblInfoGold.text = gm.InfoGuildGold.ToString();
        _lblInfoSumGold.text = gm.InfoGuildTotalGold.ToString();
        //公会旗帜
        _lblInfoFlagLevel.text = gm.FlagCurFlagVo.Name;
        _txtInfoFlagIcon.mainTexture = SourceManager.Instance.getTextByIconName
            (gm.FlagCurFlagVo.Icon);

        //公会升级需求
        _lblInfoLevelUpConsume.text = GuildManager.FormatCenterNeed(gm.CtCurCenterVo.CenterLvlupConsume);

        //公告
        _inputInfoPost.value = gm.InfoGuildPost;
    }
    #endregion

    #region MemberTable
    private UIGrid _gridMb;
    private GameObject _preMb;
    private UILabel _lblMemberCount;
    private void InitialMemberAwake()
    {
        _gridMb = F<UIGrid>("member/panel/membergrid").GetComponent<UIGrid>();
        _preMb = F("member/panel/temp").gameObject;
        _lblMemberCount = F<UILabel>("member/offline/num");
    }
    public void DisplayMemberTable()
    {
        ActiveFunctionTable(Table.Table2);

        _lblMemberCount.text = string.Format("{0}/{1}", GuildManager.Instance.InfoGuildCurMember,
            GuildManager.Instance.InfoGuildMaxMember);

        List<GuildMemberVo> mbs = null;
        if (GuildManager.Instance.MbShowOflineMember)
        {
            mbs = GuildManager.Instance.MbList;
        }
        else {
            mbs = new List<GuildMemberVo>();
            for (int i = 0; i < GuildManager.Instance.MbList.Count; i++)
            {
                if (GuildManager.Instance.MbList[i].IsOnline)
                {
                    mbs.Add(GuildManager.Instance.MbList[i]);
                }
            }
        }

        ViewHelper.FormatTemplate<List<GuildMemberVo>, GuildMemberVo>(_preMb,
           _gridMb.transform, mbs,
           (GuildMemberVo vo, Transform t) =>
           {
               string color;
               if (vo.IsOnline)
               {
                   switch (t.name.toInt32())
                   {
                       case 0:
                           color = ColorConst.ColorRank1;
                           break;
                       case 1: color = ColorConst.ColorRank2;
                           break;
                       case 2: color = ColorConst.ColorRank3;
                           break;
                       default:
                           color = ColorConst.ColorRankOther;
                           break;
                   }
               }
               else {
                   color = ColorConst.ColorRankOfline;
               }
               
               t.F<UILabel>("ladder").text = ColorConst.Format(color, t.name.toInt32()+1);//排名
               t.F<UILabel>("vip/num").text = vo.VipLevel.ToString();           //VIP
               t.F<UILabel>("name").text = ColorConst.Format(color, vo.Name);   //名字
               t.F<UILabel>("level").text = ColorConst.Format(color, vo.Level);//等级
               t.F<UILabel>("career").text = ColorConst.Format(color,           //职业
                   ViewHelper.GetStringByCareer(vo.Career));
               t.F<UILabel>("position").text = ColorConst.Format(color, GuildManager.Instance.FindOffcerVoByType(vo.Office).Name); 
               t.F<UILabel>("contribute").text = ColorConst.Format(color, vo.TotalContribution);//贡献度
           });
        _gridMb.Reposition();


    }
    #endregion

    #region EventTable

    private GameObject _preEventItem;
    private UIGrid _gridEventItem;
    private void InitialEventAwake()
    {
        _preEventItem = F("event/temp");
        _gridEventItem = F<UIGrid>("event/list");
    }

    public void DisplayEventTable()
    {
        ActiveFunctionTable(Table.Table3);

        ViewHelper.FormatTemplate<BetterList<GuildEventVo>, GuildEventVo>(
            _preEventItem, _gridEventItem.transform, GuildManager.Instance.EventShowList,
            (GuildEventVo vo, Transform t) =>
            {
                F<UITexture>("icon/img",t).mainTexture =
                    SourceManager.Instance.getTextByIconName(vo.Icon);
                F<UILabel>("icon/name", t).text = vo.Name;
                F<UILabel>("info/eventtime/content", t).text = vo.EventTime;
                F<UILabel>("info/opentime/content", t).text = vo.EventCustonTime;
                F<UILabel>("info/level/content", t).text = vo.EnableLevel.ToString();
			    F<UILabel>("info/lbl", t).text = vo.EventDesc;
                F<UILabel>("btn/evil/lbl", t).text = vo.EventCustonButtonName1;
                F<UILabel>("btn/join/lbl", t).text = vo.EventCustonButtonName2;
            }
            );
    }
    #endregion

    #region BulidTable

    private GameObject _preBuild;
    private UIGrid _gridBuild;
    private void InitialBuildAwake()
    {
        _preBuild = F("build/temp");
        _gridBuild = F<UIGrid>("build/list");
    }
    public void DisplayBuildTable()
    {
        ActiveFunctionTable(Table.Table4);

        ViewHelper.FormatTemplate<List<GuildBulidVo>, GuildBulidVo>
            (_preBuild, _gridBuild.transform, GuildManager.Instance.BuildDisplayList,
            (GuildBulidVo vo, Transform t) =>
            {
                F<UITexture>("icon/img", t).mainTexture =
                    SourceManager.Instance.getTextByIconName(vo.Icon);
                F<UILabel>("name", t).text = vo.Name;
                F<UILabel>("opentips", t).text = GuildManager.FormatBuildOption(vo);
                F<UILabel>("btn/levelup/info", t).text = vo.ButtonDesc1;
                F<UILabel>("btn/join/info", t).text = vo.ButtonDesc2;
            });
    }
    #endregion

    #region MemberCheckTable
    private UIGrid _gridMbc;
    private GameObject _preMbc;

    private void InitialMemberCheckAwake()
    {
        _gridMbc = F<UIGrid>("checkmember/Panel/memberlist");
        _preMbc = F("checkmember/Panel/temp");
    }

    public void DisplayMemberCheckTable()
    {
        ActiveFunctionTable(Table.Table5);

        ViewHelper.FormatTemplate<List<GuildMemberVo>, GuildMemberVo>(_preMbc,
            _gridMbc.transform, GuildManager.Instance.MbcList,
            (GuildMemberVo vo, Transform t) =>
            {
                string color;
                switch (t.name.toInt32())
                {
                    case 0:
                        color = ColorConst.ColorRank1;
                        break;
                    case 1: color = ColorConst.ColorRank2;
                        break;
                    case 2: color = ColorConst.ColorRank3;
                        break;
                    default:
                        color = ColorConst.ColorRankOther;
                        break;
                }
                t.F<UILabel>("vip/num").text = vo.VipLevel.ToString();
                t.F<UILabel>("name").text = ColorConst.Format(color, vo.Name);
                t.F<UILabel>("level").text = ColorConst.Format(color, vo.Level);
                t.F<UILabel>("battle").text = ColorConst.Format(color, vo.Power);
            });
        _gridMbc.Reposition();
    }
    #endregion

    #region LogTable
    private UIGrid _gridLog;
    private GameObject _preLog;
    private void InitialLogAwake()
    {
        _gridLog = F<UIGrid>("incident/Panel/grid").GetComponent<UIGrid>();
        _preLog = F("incident/Panel/log").gameObject;
    }
    public void DisplayLogTable()
    {
        ActiveFunctionTable(Table.Table6);

        ViewHelper.FormatTemplate<List<GuildLogVo>, GuildLogVo>(_preLog, _gridLog.transform,
            GuildManager.Instance.LogList,
            (GuildLogVo vo, Transform t) =>
            {
                t.F<UILabel>("incident").text = vo.Log;
                t.F<UILabel>("time").text = vo.ToString();
            });
        _gridLog.Reposition();
    }
    #endregion

    #region MemberInfo
    private UISprite _spSfHand;
    private UILabel _lblSfName;
    private UILabel _lblSfVip;
    private UILabel _lblSfPower;
    private UILabel _lblSfOffice;
    private GameObject _objSfPanel;
    private UISwtiching _swSfOffice;
    private UISwtiching _swSfKicking;
    private void InitialMemberInfoAwake()
    {
        _objSfPanel = F("showInfo");
        _spSfHand = F<UISprite>("showInfo/hand/hand");
        _lblSfVip = F<UILabel>("showInfo/vip/num");
        _lblSfName = F<UILabel>("showInfo/name/content");
        _lblSfPower = F<UILabel>("showInfo/power/content");
        _lblSfOffice = F<UILabel>("showInfo/office/content");
        _swSfOffice = F<UISwtiching>("showInfo/btns/btn_office");
        _swSfKicking = F<UISwtiching>("showInfo/btns/btn_getout");
        SetActiveMemberInfo(false);
    }

    public void SetActiveMemberInfo(bool isActive)
    {
        _objSfPanel.SetActive(isActive);
    }

    public void DisplayMemberInfoPanel()
    {
        GuildMemberVo vo=GuildManager.Instance.MbSelectMemeber;
        SetActiveMemberInfo(true);
        _spSfHand.spriteName = ViewHelper.GetHandIcon(vo.Career);
        _lblSfName.text = vo.Name;
        _lblSfVip.text = vo.VipLevel.ToString();
        _lblSfPower.text = vo.Power.ToString();
        GuildOffcerVo of=GuildManager.Instance.FindOffcerVoByType(vo.Office);
        _lblSfOffice.text = of.Name;
        of = GuildManager.Instance.FindOffcerVoByType(GuildManager.Instance.InfoOwnType);
        if (of.ChangOffices.size == 0)
        {
            _swSfOffice.Initial(false);
        }
        else {
            _swSfOffice.Initial(true);
        }
        _swSfKicking.Initial(of.CptKickingGuild);
    }
    #endregion

    #region OfficeManager

    private Dictionary<GuildOffice, UIToggle> _ofToggles;
    private UILabel _ofName;
    private UILabel _ofOffice;
    private GameObject _ofPanel;
    private void InitialOfficeManagerAwake()
    {
        _ofToggles = new Dictionary<GuildOffice, UIToggle>();
        _ofToggles.Add(GuildOffice.Leader, F<UIToggle>("office/checkboxs/0"));
        _ofToggles.Add(GuildOffice.DeputyLeader, F<UIToggle>("office/checkboxs/1"));
        _ofToggles.Add(GuildOffice.Statesman, F<UIToggle>("office/checkboxs/2"));
        _ofToggles.Add(GuildOffice.Elite, F<UIToggle>("office/checkboxs/3"));
        _ofToggles.Add(GuildOffice.Masses, F<UIToggle>("office/checkboxs/4"));
        _ofName = F<UILabel>("office/name/content");
        _ofOffice = F<UILabel>("office/office/content");
        _ofPanel = F("office");
    }

    public void SetActiveOfficeManager(bool isActive)
    {
        _ofPanel.SetActive(isActive);
    }
    public void DisplayOfficeManager()
    {
        SetActiveOfficeManager(true);

        GuildMemberVo vo = GuildManager.Instance.MbSelectMemeber;
        _ofName.text = vo.Name;
        _ofOffice.text = GuildManager.Instance.FindOffcerVoByType(vo.Office).Name;
        GuildOffcerVo myOf = GuildManager.Instance.FindOffcerVoByType(GuildManager.Instance.InfoOwnType);

        foreach (UIToggle toggle in _ofToggles.Values)
        {
            toggle.value = false;
            toggle.GetComponent<BoxCollider>().enabled = false;
            toggle.transform.FindChild("Label").GetComponent<UILabel>().color = Color.gray;
        }

        foreach (GuildOffice office in myOf.ChangOffices)
        {
            _ofToggles[office].GetComponent<BoxCollider>().enabled = true;
            _ofToggles[office].transform.FindChild("Label").GetComponent<UILabel>().color = Color.yellow;
        }
        _ofToggles[vo.Office].value = true;
    }

    #endregion
}
