using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using MVC.interfaces;
using helper;
using NetGame;

namespace manager {
    public class MedalTipsManager
    {
        private static MedalTipsManager _instance;
        public static MedalTipsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MedalTipsManager();
                }
                return _instance;
            }
        }

        public void ShowMedalUI() 
        {
            UIManager.Instance.openWindow(UiNameConst.ui_medal);
            if (!MedalManager.Instance.IsRequestData)
            {
                GCAskMedalInfo ask = new GCAskMedalInfo();
                NetBase.GetInstance().Send(ask.ToBytes());
            }
            else {
                UpdateInfo();
            }
            

        }


        public void UpdateInfo() {
            Gate.instance.sendNotification(MsgConstant.MSG_MEDAL_BAG_MEDAL_SHOW);
        }
    }
}
