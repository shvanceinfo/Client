using UnityEngine;
using System.Collections;
using manager;
using model;
using mediator;
using MVC.entrance.gate;
using MVC.interfaces;

public class GuildShopOpenwindow : MonoBehaviour
{
    private DataReadGuildShop readGuildShop;
    void OnClick()
    {
        UIManager.Instance.openWindow(UiNameConst.ui_guildshop);

        Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_SHOP, SellShopType.HotSell);

    }
}
