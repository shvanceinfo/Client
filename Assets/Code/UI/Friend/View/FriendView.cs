using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using model;
using mediator;
using MVC.entrance.gate;
using helper;
using NetGame;

public class FriendView : MonoBehaviour {

	GameObject _item;                   //好友列表复制体
	UIGrid _gird;                   //好友grid

    private UILabel _lblReceive;
    private UILabel _lblSend;
    private UILabel _lblFriendCount;
    private GameObject _objAllSend;
    private GameObject _objAllReceive;
    private GameObject _objAddFriendPanel;
    private UIInput _inputAddFriend;


    private GameObject _objRecord;
    private GameObject _objTabel_Record;
    private GameObject _objTabe2_Result;

    private GameObject _preRecord;
    private UIGrid _gridRecord;
    private GameObject _objRecordAgree;
    private GameObject _objRecordRefuse;
    private UICheckBoxColor _cbk1;
    private UICheckBoxColor _cbk2;


    private GameObject _objDeletePanel;
    private UILabel _lblDeleteInfo;

    //头像详细信息
    private GameObject _infoPanel;
    private UISprite _spHandIcon;   //头像
    private UILabel _lblVipLevel;   //VIP等级
    private UILabel _lblName;       //名字
    private UILabel _lblLevel;      //人物等级
    private UILabel _lblPower;      //战斗力

    private GameObject _preLog;
    private UIGrid _gridLog;

    private GlinkNotify _notify;
    public void Awake()
    {
        _notify = F <GlinkNotify>("Background/Notify");
        _notify.activeNotify(false);
        _infoPanel = F("Panel_Friend_Message");
        _spHandIcon = F<UISprite>("Panel_Friend_Message/Friend_message/Friend_Head_Record");
        _lblVipLevel = F<UILabel>("Panel_Friend_Message/Friend_message/Vip");
        _lblName = F<UILabel>("Panel_Friend_Message/Friend_message/Friend_Name_Record");
        _lblLevel = F<UILabel>("Panel_Friend_Message/Friend_message/Friend_Level_Record");
        _lblPower = F<UILabel>("Panel_Friend_Message/Friend_message/Friend_Atk_Record");
        _infoPanel.SetActive(false);

        _item = F("Panel_Friend/Item");
        _item.SetActive(false);
        _gird = F<UIGrid>("Panel_Friend/Grid");

        _lblReceive = F<UILabel>("Panel_Friend_Ui/_number/Label_get");
        _lblSend = F<UILabel>("Panel_Friend_Ui/_number/Label_set");
        _lblFriendCount = F<UILabel>("Panel_Friend_Ui/_number/Label_friend_num");

        _objAllSend = F("Button_Set_All");
        _objAllReceive = F("Button_Get_All");

        _objAddFriendPanel = F("Panel_Add_Friend");
        _objAddFriendPanel.SetActive(false);

        _inputAddFriend = F<UIInput>("Panel_Add_Friend/Input");

        _objRecord = F("Panel_Shenqing_ui");
        _objRecord.SetActive(false);
        _objTabel_Record = F("Panel_Shenqing_ui/Tabel_Record");
        _objTabe2_Result = F("Panel_Shenqing_ui/Tabel_Result");
        _preLog = F("Panel_Shenqing_ui/Tabel_Result/Panel_Shenqing_Result/Item");
        _gridLog = F<UIGrid>("Panel_Shenqing_ui/Tabel_Result/Panel_Shenqing_Result/Grid");
        _objTabe2_Result.SetActive(false);
        _preLog.SetActive(false);

        _preRecord = F("Panel_Shenqing_ui/Tabel_Record/Panel_Shenqing_Record/Item");
        _gridRecord = F<UIGrid>("Panel_Shenqing_ui/Tabel_Record/Panel_Shenqing_Record/Grid");
        _objRecordAgree = F("Panel_Shenqing_ui/Tabel_Record/All_Agree");
        _objRecordRefuse = F("Panel_Shenqing_ui/Tabel_Record/All_Refuse");
        _cbk1 = F<UICheckBoxColor>("Panel_Shenqing_ui/Tables/Table1");
        _cbk2 = F<UICheckBoxColor>("Panel_Shenqing_ui/Tables/Table2");

        _objDeletePanel = F("Panel_Delete_Friend");
        _objDeletePanel.SetActive(false);
        _lblDeleteInfo = F<UILabel>("Panel_Delete_Friend/Delete_Label_Message");
    }


    public void UpdateNotfiy(bool isactive)
    {
        _notify.activeNotify(isactive);
    }

    #region 主界面
    /// <summary>
    /// 显示主界面
    /// </summary>
    public void DisplayMain()
    {
        DisplayTitle();//显示标题
        DisplayFriendList();
        DispalyVipFucntion();
    }
    private void DisplayTitle()
    {
        _lblReceive.text = FriendManager.FormatTitle(FriendManager.RECEIVE,
            FriendManager.Instance.MyInfo.CurReceive, FriendManager.Instance.MyInfo.MaxReceive);
        _lblSend.text = FriendManager.FormatTitle(FriendManager.SEND,
            FriendManager.Instance.MyInfo.CurSendAward, FriendManager.Instance.MyInfo.MaxSendAward);
        _lblFriendCount.text = FriendManager.FormatTitle(FriendManager.FRIEND,
            FriendManager.Instance.MyInfo.CurFriendCount, FriendManager.Instance.MyInfo.MaxFriendCount);
    }
    private void DisplayFriendList()
    {

        ViewHelper.FormatTemplate<List<FriendVo>, FriendVo>(_item, _gird.transform, FriendManager.Instance.MyFriendList,
            (FriendVo vo, int index) =>
            {
                GameObject obj = _gird.transform.Find(index.ToString()).gameObject;

                obj.transform.Find("vip").GetComponent<UILabel>().text = vo.VipLevel.ToString();
                if (vo.IsOnline)
                {
                    obj.transform.FindChild("Label_Friend_Name").GetComponent<UILabel>().text = string.Format("[dacda6]{0}", vo.Name);
                    obj.transform.FindChild("Label_Friend_Level").GetComponent<UILabel>().text = string.Format("[dacda6]等级:{0}", vo.Level);
                }
                else
                {
                    obj.transform.FindChild("Label_Friend_Name").GetComponent<UILabel>().text = string.Format("[5c5c5c]{0}", vo.Name);
                    obj.transform.FindChild("Label_Friend_Level").GetComponent<UILabel>().text = string.Format("[5c5c5c]等级:{0}", vo.Level);

                }

                obj.transform.FindChild("Button_Get").gameObject.SetActive(vo.IsCanReceive);
                obj.transform.FindChild("Normal_Receive").gameObject.SetActive(!vo.IsCanReceive);

                obj.transform.FindChild("Button_Set").gameObject.SetActive(!vo.IsSend);
                obj.transform.FindChild("Normal_Send").gameObject.SetActive(vo.IsSend);
               


                switch (vo.Career)    //状态机
                {
                    case CHARACTER_CAREER.CC_SWORD:
                        obj.transform.FindChild("Sprite_Head").GetComponent<UISprite>().spriteName = "common_zhanshi";
                        break;
                    case CHARACTER_CAREER.CC_ARCHER:
                        obj.transform.FindChild("Sprite_Head").GetComponent<UISprite>().spriteName = "common_gongjianshou";
                        break;
                    case CHARACTER_CAREER.CC_MAGICIAN:
                        obj.transform.FindChild("Sprite_Head").GetComponent<UISprite>().spriteName = "common_mofashi";
                        break;
                    default:
                        break;
                }
                if (vo.IsOnline)
                {//new Color(0.004f, 0, 0, 1);
                    obj.transform.FindChild("Sprite_Head").GetComponent<UISprite>().color = new Color(1, 1, 1);
                }
                else {
                    obj.transform.FindChild("Sprite_Head").GetComponent<UISprite>().color = new Color(0.004f, 0, 0, 1);
                }


            });
        _gird.Reposition();
    }
    private void DispalyVipFucntion()
    {
        _objAllSend.SetActive(FriendManager.Instance.MyInfo.AllSended);
        _objAllReceive.SetActive(FriendManager.Instance.MyInfo.AllReceiveed);
    }
    //显示好友界面
    public void DisplayAddFriendPanle(bool isactive)
    {
        _objAddFriendPanel.SetActive(isactive);
        _inputAddFriend.value = "";
    }
    /// <summary>
    /// 获取添加好友的值
    /// </summary>
    /// <returns></returns>
    public string GetAddFriendInputValue()
    {
        return _inputAddFriend.value;
    }

    #endregion


    public void DisplayInfo(bool isactive)
    {
        _infoPanel.SetActive(isactive);
        if (isactive)
        {
            _spHandIcon.spriteName = ViewHelper.GetHandIcon(CharacterPlayer.character_property.career); ;
            _lblVipLevel.text = FriendManager.Instance.WaitforOpt.VipLevel.ToString();
            _lblName.text = ViewHelper.FormatLanguage("friend_info_name", FriendManager.Instance.WaitforOpt.Name);
            _lblLevel.text = ViewHelper.FormatLanguage("friend_info_level", FriendManager.Instance.WaitforOpt.Level);
            _lblPower.text = ViewHelper.FormatLanguage("friend_info_power", FriendManager.Instance.WaitforOpt.Fighter);

            _infoPanel.transform.FindChild("Button_Set_TiLi").gameObject.SetActive(!FriendManager.Instance.WaitforOpt.IsSend);
            _infoPanel.transform.FindChild("Normal_Set_TiLi").gameObject.SetActive(FriendManager.Instance.WaitforOpt.IsSend);
            
            if (FriendManager.Instance.WaitforOpt.IsOnline)
            {
                _spHandIcon.color = new Color(1, 1, 1);
            }
            else {
                _spHandIcon.color = new Color(0.004f, 0, 0, 1);
            }
            
        }
    }

    public void DisplayExinfo()
    {
        if (_infoPanel.activeSelf)
        {
            _spHandIcon.spriteName = ViewHelper.GetHandIcon(CharacterPlayer.character_property.career); ;
            _lblVipLevel.text = FriendManager.Instance.WaitforOpt.VipLevel.ToString();
            _lblName.text = ViewHelper.FormatLanguage("friend_info_name", FriendManager.Instance.WaitforOpt.Name);
            _lblLevel.text = ViewHelper.FormatLanguage("friend_info_level", FriendManager.Instance.WaitforOpt.Level);
            _lblPower.text = ViewHelper.FormatLanguage("friend_info_power", FriendManager.Instance.WaitforOpt.Fighter);

            if (FriendManager.Instance.WaitforOpt.IsSend)
            {
                _infoPanel.transform.FindChild("Button_Set_TiLi/Label").GetComponent<UILabel>().text = "已赠送";
                _infoPanel.transform.FindChild("Button_Set_TiLi/Background").GetComponent<UISprite>().spriteName = "common_button3_hui";
                _infoPanel.transform.FindChild("Button_Set_TiLi").GetComponent<BoxCollider>().enabled = false;
            }
            else {
                _infoPanel.transform.FindChild("Button_Set_TiLi/Label").GetComponent<UILabel>().text = "赠送体力";
                _infoPanel.transform.FindChild("Button_Set_TiLi/Background").GetComponent<UISprite>().spriteName = "common_button3";
                _infoPanel.transform.FindChild("Button_Set_TiLi").GetComponent<BoxCollider>().enabled = true;
            }
        }
    }

    public void DisplayRecord(Table table)
    {
        _objRecord.SetActive(true);
        _objTabel_Record.SetActive(false);
        _objTabe2_Result.SetActive(false);
        switch (table)
        {
            case Table.Table1:
                FriendManager.Instance.RecordUIIsOpen = true;
                _objTabel_Record.SetActive(true);
                DisplayTable1List();
                DisplayTable1Vip();
                break;
            case Table.Table2:
                FriendManager.Instance.RecordUIIsOpen = false;
                _objTabe2_Result.SetActive(true);
                DisplayLogList();
                break;
        }
    }


    public void CloseRecord()
    {
        _objRecord.SetActive(false);
        _cbk1.isChecked = true;
        _cbk2.isChecked = false;
    }


    //显示日志信息
    public void DisplayLogList()
    {
        ViewHelper.FormatTemplate<List<string>, string>(_preLog, _gridLog.transform,
            FriendManager.Instance.LogList,
            (string vo, Transform t) =>
            {
                t.Find("Label").GetComponent<UILabel>().text = vo;
            });
        _gridLog.Reposition();
    }


    private void DisplayTable1List()
    {
        ViewHelper.FormatTemplate<List<FriendVo>, FriendVo>(_preRecord, _gridRecord.transform, FriendManager.Instance.RecordFriendList, (FriendVo cord, int index) =>
        {
            GameObject player = _gridRecord.transform.Find(index.ToString()).gameObject;

            player.transform.FindChild("Label_Friend_Name_Record").GetComponent<UILabel>().text = string.Format("[ddd4b0]{0}", cord.Name);
            player.transform.FindChild("Label_Friend_Level_Record").GetComponent<UILabel>().text = string.Format("[ddd4b0]等级:{0}", cord.Level.ToString());
            player.transform.FindChild("Label_Atk_Record").GetComponent<UILabel>().text = string.Format("战斗力：{0}", cord.Fighter);

            switch (cord.Career)    //状态机
            {
                case CHARACTER_CAREER.CC_SWORD:
                    player.transform.FindChild("Sprite_Head_Record").GetComponent<UISprite>().spriteName = "common_zhanshi";
                    break;
                case CHARACTER_CAREER.CC_ARCHER:
                    player.transform.FindChild("Sprite_Head_Record").GetComponent<UISprite>().spriteName = "common_gongjianshou";
                    break;
                case CHARACTER_CAREER.CC_MAGICIAN:
                    player.transform.FindChild("Sprite_Head_Record").GetComponent<UISprite>().spriteName = "common_mofashi";
                    break;
                default:
                    break;
            }

        });
        _gridRecord.Reposition();
    }
    private void DisplayTable1Vip()     //显示VIP权限
    {
        _objRecordAgree.SetActive(FriendManager.Instance.MyInfo.AllAgree);
         _objRecordRefuse.SetActive(FriendManager.Instance.MyInfo.AllRefuse);
    }

    public void DisplayDeletePanle(bool isactive)
    {
        _objDeletePanel.SetActive(isactive);
        if (isactive)
        {
            _lblDeleteInfo.text = string.Format(LanguageManager.GetText("friend_delete"),
                FriendManager.Instance.WaitforOpt.Name);
        }
        else {
            FriendManager.Instance.RecordUIIsOpen = false;
        }
    }
    

    void OnEnable()
    {
        Gate.instance.registerMediator(new FriendMeditor(this));
    }
    void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.FRIEND_MEDIATOR);
    }



   // public void DisOnFriendList()       //刷新好友列表
   // {
   //     BetterList<FriendVo> list = FriendManager.Instance.Friends;

   //     ViewHelper.FormatTemplate<BetterList<FriendVo>, FriendVo>(_item, _gird.transform, FriendManager.Instance.Friends,
   //         (FriendVo vo, int index) =>
   //         {
   //             GameObject obj = _gird.transform.Find(index.ToString()).gameObject;

   //             if (vo.IsOnline==1)
   //             {
   //                 obj.transform.FindChild("Label_Friend_Name").GetComponent<UILabel>().text = string.Format("[dacda6]{0}", vo.Name);
   //                 obj.transform.FindChild("Label_Friend_Level").GetComponent<UILabel>().text = string.Format("[dacda6]等级:{0}", vo.Level);
   //                 i++;
   //             }
   //             else
   //             {
   //                 obj.transform.FindChild("Label_Friend_Name").GetComponent<UILabel>().text = string.Format("[5c5c5c]{0}", vo.Name);
   //                 obj.transform.FindChild("Label_Friend_Level").GetComponent<UILabel>().text = string.Format("[5c5c5c]等级:{0}", vo.Level);

   //             }
   //             if (vo.IsReceive == 1)
   //             {
   //                 obj.transform.FindChild("Button_Get").FindChild("Background").GetComponent<UISprite>().spriteName = "common_button3";
   //                 obj.transform.FindChild("Button_Get").FindChild("Label").GetComponent<UILabel>().text = "领 取";
   //                 obj.transform.FindChild("Button_Get").GetComponent<BoxCollider>().enabled = true;
   //             }
   //             else
   //             {
   //                 obj.transform.FindChild("Button_Get").FindChild("Background").GetComponent<UISprite>().spriteName = "common_button3_hui";
   //                 obj.transform.FindChild("Button_Get").FindChild("Label").GetComponent<UILabel>().text = "已领取";
   //                 obj.transform.FindChild("Button_Get").GetComponent<BoxCollider>().enabled = false;
   //             }
   //             if (vo.IsSend == 1)
   //             {
   //                 obj.transform.FindChild("Button_Set").FindChild("Background").GetComponent<UISprite>().spriteName = "common_button3";
   //                 obj.transform.FindChild("Button_Set").FindChild("Label").GetComponent<UILabel>().text = "赠 送";
   //                 obj.transform.FindChild("Button_Set").GetComponent<BoxCollider>().enabled = true;
   //             }
   //             else
   //             {
   //                 obj.transform.FindChild("Button_Set").FindChild("Background").GetComponent<UISprite>().spriteName = "common_button3_hui";
   //                 obj.transform.FindChild("Button_Set").FindChild("Label").GetComponent<UILabel>().text = "已赠送";
   //                 obj.transform.FindChild("Button_Set").GetComponent<BoxCollider>().enabled = false;
   //             }
                


   //             switch (vo.Career)    //状态机
   //             {
   //                 case CHARACTER_CAREER.CC_SWORD:
   //                     obj.transform.FindChild("Sprite_Head").GetComponent<UISprite>().spriteName = "common_zhanshi";
   //                     break;
   //                 case CHARACTER_CAREER.CC_ARCHER:
   //                     obj.transform.FindChild("Sprite_Head").GetComponent<UISprite>().spriteName = "common_gongjianshou";
   //                     break;
   //                 case CHARACTER_CAREER.CC_MAGICIAN:
   //                     obj.transform.FindChild("Sprite_Head").GetComponent<UISprite>().spriteName = "common_mofashi";
   //                     break;
   //                 default:
   //                     break;
   //             }


   //         });


   //     Debug.Log(i);
      
   // }
   

   // public void ReceiveList()        //刷新申请列表
   // {
   //     _requestFriendObj.SetActive(true);
   //     BetterList<FriendRecord> list = FriendManager.Instance.Receive;
   //     ViewHelper.FormatTemplate<BetterList<FriendRecord>, FriendRecord>(_itemRecord, _girdRecord.transform, FriendManager.Instance.Receive, (FriendRecord cord, int index) =>
   //     {
   //         GameObject player = _girdRecord.transform.Find(index.ToString()).gameObject;

   //         player.transform.FindChild("Label_Friend_Name_Record").GetComponent<UILabel>().text = string.Format("[ddd4b0]{0}", cord.PlayName);
   //         player.transform.FindChild("Label_Friend_Level_Record").GetComponent<UILabel>().text = string.Format("[ddd4b0]等级:{0}", cord.Level.ToString());
   //         player.transform.FindChild("Label_Atk_Record").GetComponent<UILabel>().text = string.Format("战斗力：{0}", cord.Fighter);
            
   //         switch (cord.Career)    //状态机
   //         {
   //             case CHARACTER_CAREER.CC_SWORD:
   //                 player.transform.FindChild("Sprite_Head_Record").GetComponent<UISprite>().spriteName = "common_zhanshi";
   //                 break;
   //             case CHARACTER_CAREER.CC_ARCHER:
   //                 player.transform.FindChild("Sprite_Head_Record").GetComponent<UISprite>().spriteName = "common_gongjianshou";
   //                 break;
   //             case CHARACTER_CAREER.CC_MAGICIAN:
   //                 player.transform.FindChild("Sprite_Head_Record").GetComponent<UISprite>().spriteName = "common_mofashi";
   //                 break;
   //             default:
   //                 break;
   //         }

   //     });
   //     _girdRecord.transform.parent.GetComponent<UIScrollView>().ResetPosition();
   // }
	
	


   // public void ResultList(string s)        //刷新结果列表
   // {
   //     _itemResult.SetActive(true);

   //     GameObject obj = GameObject.Instantiate(_itemResult) as GameObject;
   //     obj.transform.parent = _girdResult.transform;
   //     obj.transform.localPosition = new Vector3();
   //     obj.transform.localScale = new Vector3(1, 1, 1);
   //     obj.transform.FindChild("Label").GetComponent<UILabel>().text = s;

   //     _girdResult.GetComponent<UIGrid>().Reposition();
   //     _itemResult.SetActive(false);
   // }




   // public void FriendShow(string a, string b, int c, int d)       //好友信息数据
   // {
   //     _friendname.text = string.Format("[ddd4b0]{0}", a);
   //     _friendlevel.text = string.Format("[ddd4b0]等级:{0}", b);
      
        
   //     switch (c)
   //     {
   //         case (int)CHARACTER_CAREER.CC_SWORD:
   //             _friendcareer.spriteName = "common_zhanshi";
   //             break;
   //         case (int)CHARACTER_CAREER.CC_ARCHER:
   //             _friendcareer.spriteName = "common_gongjianshou";
   //             break;
   //         case (int)CHARACTER_CAREER.CC_MAGICIAN:
   //             _friendcareer.spriteName = "common_mofashi";
   //             break;
   //         default:
   //             break;
   //     }
   //     _friendfighter.text = string.Format("战斗力：{0}", d);
   //     transform.Find("Panel_Friend_Message").gameObject.SetActive(true);
   // }
   // public void FriendClose()                                //关闭好友信息窗口
   // {
   //     transform.Find("Panel_Friend_Message").gameObject.SetActive(false);
   // }


	
   // public void DisplayAddPanel()                   //添加好友  界面开关 ，按钮    
   // {
   //      _addFriendObj.SetActive(!_addFriendObj.activeSelf) ;  
		
   // }
   // public void CloseAddFriend()  					//添加好友 ，界面开关， X
   // {
   //     _addFriendObj.SetActive(false);
   // }
   // public void AddFriendTrue()                    //确定添加好友
   // {
   //   string  ss = _input.text;
   //     //Debug.Log(_input.text);
   //     _addFriendObj.SetActive(false);
   // }
   
   // //public void AddFriendReceive()    //请求 添加好友数据
   // //{ 
        
   // //    ViewHelper.DisplayMessage();  //动态飘字
   // //}
   // public void OpenRecord()                       //申请记录  界面开关 按钮       
   // {
   //     _requestFriendObj.SetActive(!_requestFriendObj.activeSelf);
   // //    FriendManager.Instance.Record_Shenqing();

        
		
   // }
   // public void CloseRecord()                       //申请记录  界面开关， X
   // {
   //     _requestFriendObj.SetActive(false);
       
   // }

   // public void DeleteFd()                     	   //删除好友界面， 按钮开启
   // {
        
   //     _deleteFriend.SetActive(true);
       
       
   // }
   // public void DeleteFdClose()                   //删除好友界面，X或取消关闭
   // {

        
        
   //     _deleteFriend.SetActive(false);
        
   // }
   //public void DeleteTrue()                   //确定删除好友
   // {   
   //      _deleteFriend.SetActive(false);

   // }
   
   // public void CloseUi()                 		  //好友界面， X关闭
   // {
   //     UIManager.Instance.closeWindow(UiNameConst.ui_friend);
   // }
   // public void Change_Tabel1()                  //切换至 申请记录 界面
   // {
   //     _recordtabel.SetActive(true);
   //     _resulttabel.SetActive(false);
   //     Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_TABEL_RECORD);
   //     //_nameShenqing.text=FriendManager.Instance.player_name;
   //    // Debug.Log(_nameShenqing.text);

   // }
   // public void Change_Tabel2()                       //切换至 申请结果 界面
   // {
   //     _recordtabel.SetActive(false);
   //     _resulttabel.SetActive(true);
   //     FriendManager.Instance.AddRusult();
   //     Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_TABEL_RESULT);
   // }

    protected T F<T>(string path) where T : Component
    {
        return this.transform.FindChild(path).GetComponent<T>();
    }
    protected T F<T>(string path, Transform t) where T : Component
    {
        return t.FindChild(path).GetComponent<T>();
    }

    protected GameObject F(string path)
    {
        return this.transform.FindChild(path).gameObject;
    }
}
