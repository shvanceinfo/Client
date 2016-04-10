using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnDemonMsg : MonoBehaviour {

    const string btn_close = "Btn_Close";                   //关闭主窗口
    const string level_1_Click = "Btn_Diff_Level1";         //选择难度
    const string level_2_Click = "Btn_Diff_Level2";         //选择难度
    const string level_3_Click = "Btn_Diff_Level3";         //选择难度
    const string btn = "Btn";                               //挑战btns
    const string index1="Index1";                           //继续上一次挑战
    const string index2="Index2";                           //1层
    const string index3="Index3";                           //31层
    const string index4 = "Index4";                         //61层
    const string button_select = "button_select";           //领取挑战奖励
    const string button_rank_close = "Btn_CloseRank";       //关闭今日排名
    const string button_rank_display = "Btn_RankCur";       //显示今日排名
    const string button_rank_all_display = "Btn_Rank";      //显示昨日奖励
    const string button_rank_all_close = "Rank_Close";      //关闭昨日奖励

    const string button_rank1 = "Btn_Rank1";
    const string button_rank2 = "Btn_Rank2";
    const string button_rank3 = "Btn_Rank3";

    const string button_arce = "Button_Arce";               //恶魔洞窟通关界面
    void OnClick()
    {
        switch (gameObject.name)
        {
            case button_rank1:
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_RERUSH_CUR_RANK_UI, DemonDiffEnum.Level1);
                break;
            case button_rank2:
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_RERUSH_CUR_RANK_UI, DemonDiffEnum.Level2);
                break;
            case button_rank3:
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_RERUSH_CUR_RANK_UI, DemonDiffEnum.Level3);
                break;

            case button_rank_all_display:
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_ENTER_HISTORY_AWARD, true);
                break;
            case button_rank_all_close:
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_DISPLAY_RANK, false);
                break;
            case button_rank_display:
                DemonManager.Instance.RequestCurRankData();
                break;
            case button_rank_close:
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_DISPLAY_CUR_RANK, false);
                break;
            case btn_close:
                DemonManager.Instance.CloseWindow();       
                break;
            case level_1_Click:
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_DISPLAY_ITEMLIST,DemonDiffEnum.Level1);
                break;
            case level_2_Click:
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_DISPLAY_ITEMLIST, DemonDiffEnum.Level2);
                break;
            case level_3_Click:
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_DISPLAY_ITEMLIST, DemonDiffEnum.Level3);
                break;
            case btn:
                switch (gameObject.transform.parent.name)
                {
                    case index1:
                        Gate.instance.sendNotification(MsgConstant.MSG_DEMON_ENTER_TOWER,0);
                        break;
                    case index2: Gate.instance.sendNotification(MsgConstant.MSG_DEMON_ENTER_TOWER, 1);
                        break;
                    case index3: Gate.instance.sendNotification(MsgConstant.MSG_DEMON_ENTER_TOWER, 2);
                        break;
                    case index4: Gate.instance.sendNotification(MsgConstant.MSG_DEMON_ENTER_TOWER, 3);
                        break;
                    default:
                        break;
                }
                break;
            case button_select:
                DemonItem di = transform.parent.GetComponent<DemonItem>();
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_RECEIVE_ITEM, di.Id);
                break;
            case button_arce:
                HideEnemyBlood();
                DeathManager.Instance.ToCity();
                break;
            default:
                break;
        }
    }

    private void HideEnemyBlood()
    {
        Transform trans = GameObject.Find("ui_root").transform.FindChild("Camera");
        for (int i = 0; i < trans.childCount; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (trans.GetChild(i).name.Length > 1 && trans.GetChild(i).name.Substring(0, 1).Equals(j.ToString()))
                {
                    //trans.GetChild(i).gameObject.SetActive(false);
                    GameObject.Destroy(trans.GetChild(i).gameObject);
                }
            }
        }
    }
}
