using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using helper;
using System.Collections.Generic;

public class GuildDonateView : MonoBehaviour {
    private UILabel myState, myContri, myTotalContri;
    private UISprite myContriTxt, myTotalContriTxt;
    private UILabel guildLevel, guildGold, guildTotalGold;

    private UILabel leftTitle1, leftTitle2, leftTitle3;
    private UILabel middleTitle1, middleTitle2, middleTitle3;
    private UILabel rightTitle1, rightTitle2, rightTitle3;

    void Awake()
    {
        myState = transform.FindChild("guilddetial/info/current/content1").GetComponent<UILabel>();
        myContri = transform.FindChild("guilddetial/info/current/content2").GetComponent<UILabel>();
        myTotalContri = transform.FindChild("guilddetial/info/current/content3").GetComponent<UILabel>();

        myContriTxt = transform.FindChild("guilddetial/info/current/Texture1").GetComponent<UISprite>();
        myTotalContriTxt = transform.FindChild("guilddetial/info/current/Texture2").GetComponent<UISprite>();


        guildLevel = transform.FindChild("guilddetial/info/next/content1").GetComponent<UILabel>();
        guildGold = transform.FindChild("guilddetial/info/next/content2").GetComponent<UILabel>();
        guildTotalGold = transform.FindChild("guilddetial/info/next/content3").GetComponent<UILabel>();

        leftTitle1 = transform.FindChild("guilddetial/info/left/title1").GetComponent<UILabel>();
        leftTitle2 = transform.FindChild("guilddetial/info/left/title2").GetComponent<UILabel>();
        leftTitle3 = transform.FindChild("guilddetial/info/left/title3").GetComponent<UILabel>();
        middleTitle1 = transform.FindChild("guilddetial/info/middle/title1").GetComponent<UILabel>();
        middleTitle2 = transform.FindChild("guilddetial/info/middle/title2").GetComponent<UILabel>();
        middleTitle3 = transform.FindChild("guilddetial/info/middle/title3").GetComponent<UILabel>();
        rightTitle1 = transform.FindChild("guilddetial/info/right/title1").GetComponent<UILabel>();
        rightTitle2 = transform.FindChild("guilddetial/info/right/title2").GetComponent<UILabel>();
        rightTitle3 = transform.FindChild("guilddetial/info/right/title3").GetComponent<UILabel>();
    }

    private void DisplayInfo()
    {
        myState.text = GuildManager.FormatOffcerString(GuildManager.Instance.InfoOwnType);
        myContri.text = GuildManager.Instance.InfoOwnContribution.Value2.ToString();
        myTotalContri.text = GuildManager.Instance.InfoOwnTotalContribution.Value2.ToString();

        myContriTxt.spriteName = SourceManager.Instance.getIconByType(eGoldType.gold);
        myTotalContriTxt.spriteName = SourceManager.Instance.getIconByType(eGoldType.gold);

        guildLevel.text = GuildManager.Instance.InfoGuildLevel.ToString();
        guildGold.text = GuildManager.Instance.InfoGuildGold.ToString();
        guildTotalGold.text = GuildManager.Instance.InfoGuildTotalGold.ToString();
    }

    public void RefreshView()
    {
        DisplayDonate(leftTitle1, leftTitle2, DonateLevel.Low);
        DisplayDonate(middleTitle1, middleTitle2, DonateLevel.Middle);
        DisplayDonate(rightTitle1, rightTitle2, DonateLevel.High);
    }

    private void DisplayDonate(UILabel txt1,UILabel txt2,DonateLevel donateLevel)
    {
        GuildBaseVo vo = GuildManager.Instance.FindBaseVoByLevel(GuildManager.Instance.InfoGuildLevel);
        string s0 = ViewHelper.GetResoucesString((eGoldType)vo.Donates[donateLevel].Id);
        txt1.text = "消耗" + vo.Donates[donateLevel].Value.ToString() + s0;
        for (int i = 0; i < vo.Awards[donateLevel].size; i++)
        {
            string s1 = ViewHelper.GetResoucesString((eGoldType)vo.Awards[DonateLevel.Low][i].Id);
            txt2.text += "增加" + vo.Awards[donateLevel][i].Value.ToString() + s1 + "\n";
        }
    }

}
