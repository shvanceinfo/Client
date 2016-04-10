using UnityEngine;
using System.Collections;
using helper;

namespace manager
{
    public class DeathManager
    {
        static DeathManager _instance;

        DataReadRelive _relive;
        public static DeathManager Instance
        {
            get {
                if (_instance==null)
                {
                    _instance = new DeathManager();
                }
                return DeathManager._instance; 
            }
            
        }

        private int price;

        
        private string tips;

		private PublicDataItem _backCityTime;

		private const int backCityTimeKey = 1016001;

		private PublicDataItem BackCityTime{
			get{
				if (this._backCityTime == null) {
					this._backCityTime = ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData (backCityTimeKey);
				}
				return this._backCityTime;
			}
		}

        public int ToCityTime
        {
			get { return this.BackCityTime.type2Data; }
        }
        

        public DeathManager()
        {
            _relive = ConfigDataManager.GetInstance().GetReliveConfig();
        }
        
        public void OpenDeathWindow()
        {
            if (Global.inFightMap()||Global.InAwardMap())
            {
                price = _relive.GetReliveData(Global.requestBornNum).dia_price;
                tips = ConfigDataManager.GetInstance().getLoadingTipsConfig().getTipData(getRandom()).tip;
                UIManager.Instance.openWindow(UiNameConst.ui_born);
            }
			
        }
        public void ToCity()
        {
            MessageManager.Instance.sendMessageReturnCity();
            UIManager.Instance.showWaitting(true); //回主城强制弹出对话框
        }
        public void Revival()
        {
            if (Global.requestBornNum>VipManager.Instance.BornCount) 
            {
                ViewHelper.DisplayMessageLanguage("vip_broncount");
                return;
            }
            if (CharacterPlayer.character_asset.diamond >= price)
            { //如果身上砖石足够则复活
                MessageManager.Instance.sendMessageBorn(ReliveType.Asset, eGoldType.zuanshi, (uint)price);
                Global.requestBornNum += 1;
                AwardManager.Instance.Vo.IsUseRevive = true;
                UIManager.Instance.closeWindow(UiNameConst.ui_born, true);//关闭死亡界面
            }
            else
            {
                ShowError("dungeon_diamond_not_enough");
            }
        }
        public void ShowError(string msg)
        {
            FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText(msg), false, UIManager.Instance.getRootTrans());
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }
        public string Tips
        {
            get { return tips; }
            set { tips = value; }
        }
        private int getRandom()
        {
            return Random.Range(Constant.LOAD_TIP_MIN, Constant.LOAD_TIP_MAX);
        }
    }
}
