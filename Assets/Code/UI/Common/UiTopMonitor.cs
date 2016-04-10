using UnityEngine;
using System.Collections;

public class UiTopMonitor : MonoBehaviour
{
    bool started = false;
    private Color _goldColor = Color.white; //货币发生变化
    private Color _diamondcolor = Color.white;	//金钱发生变化
    private float  _currencyShakeTime;	   //计时器
    private float  _moneyShakeTime;
    
    void OnEnable()
    {
        EventDispatcher.GetInstance().PlayerLevel += ChangeLevel;
        EventDispatcher.GetInstance().PlayerAsset += ChangeAsset;
        EventDispatcher.GetInstance().PlayerProperty += ChangeProperty;
        EventDispatcher.GetInstance().EventWindowName += OnSetWindowName;
        DataChangeNotifyer.GetInstance().EventChangeCurrency += OnCurrencyChange;
        DataChangeNotifyer.GetInstance().EventChangeMoney += OnMoneyChange;
    }

    void OnDisable()
    {
        EventDispatcher.GetInstance().PlayerLevel -= ChangeLevel;
        EventDispatcher.GetInstance().PlayerAsset -= ChangeAsset;
        EventDispatcher.GetInstance().PlayerProperty -= ChangeProperty;
        EventDispatcher.GetInstance().EventWindowName -= OnSetWindowName;
        DataChangeNotifyer.GetInstance().EventChangeCurrency -= OnCurrencyChange;
        DataChangeNotifyer.GetInstance().EventChangeMoney -= OnMoneyChange;
        started = false;
    }

    void Update()
    {
        if (!started)
        {
            started = true;
            ChangeAsset();
            ChangeLevel();
            ChangeProperty();            
        }
    }

    void OnSetWindowName(string name)
    {
        //transform.FindChild("lbl_window_name").GetComponent<UILabel>().text = name;
		if (name == LanguageManager.GetText("lbl_pack_window_name"))
		{
			transform.FindChild("window_title/Sprite").GetComponent<UISprite>().spriteName = "beibao";
		}
		else if (name == LanguageManager.GetText("lbl_window_name_mission"))
		{
			transform.FindChild("window_title/Sprite").GetComponent<UISprite>().spriteName = "chengjiu";	
		}
		else if (name == LanguageManager.GetText("lbl_rol_window_name"))
		{
			transform.FindChild("window_title/Sprite").GetComponent<UISprite>().spriteName = "juese";	
		}
    }

    /// <summary>
    /// 经验变更
    /// </summary>
    void ChangeProperty()
    {
		return;
		
        Transform expObj = transform.FindChild("exp");
        if (expObj != null)
        {
            int nextTempId = (int)CharacterPlayer.character_property.career * 10000 + CharacterPlayer.character_property.level;
            int upgradeExp = ConfigDataManager.GetInstance().getRoleConfig().getRoleData(nextTempId).upgrade_exp;
            expObj.FindChild("lbl_exp").GetComponent<UILabel>().text =
                Global.FormatStrimg(LanguageManager.GetText("lbl_title_exp"), CharacterPlayer.character_property.getExperience().ToString(), upgradeExp.ToString());
            expObj.FindChild("Slider").GetComponent<UISlider>().sliderValue = CharacterPlayer.character_property.getExperience() / (float)upgradeExp;
        }
        CharacterPlayer.character_property.getExperience();
    }
    /// <summary>
    /// 等级变化
    /// </summary>
    void ChangeLevel()
    {
        Transform level = transform.FindChild("exp/lbl_level");
        if (level != null)
        {
            level.GetComponent<UILabel>().text = Global.FormatStrimg(LanguageManager.GetText("lbl_title_level"), CharacterPlayer.character_property.level.ToString());
        }
    }
    /// <summary>
    /// 资产变化
    /// </summary>
    void ChangeAsset()
    {
        Transform gold = transform.FindChild("gold/lbl_gold");
        Transform diamond = transform.FindChild("diamond/lbl_diamond");
        gold.GetComponent<UILabel>().text = CharacterPlayer.character_asset.gold.ToString();
        diamond.GetComponent<UILabel>().text = CharacterPlayer.character_asset.diamond.ToString();       
//
//        if (gole != null)
//        {
//            string newStr = "[d9c8a8]" + CharacterPlayer.character_asset.gold + "[-]";
//            if (gole.GetComponent<UILabel>().text != newStr)
//            {
//                gole.GetComponent<UILabel>().text = newStr;
////                PlayAnimation(transform.FindChild("man_title/gold"));
//            }
//        }
//        if (diamond != null)
//        {
//            string newStr = "[d9c8a8]" + CharacterPlayer.character_asset.diamond + "[-]";
//            if (diamond.GetComponent<UILabel>().text != newStr)
//            {
//                diamond.GetComponent<UILabel>().text = newStr;
////                PlayAnimation(transform.FindChild("man_title/diamond"));
//            }
//        }
    }

    void OnCurrencyChange(int changeNum)
    {
    	UILabel lblGold = transform.FindChild("gold/lbl_gold").GetComponent<UILabel>();
    	if(lblGold != null && lblGold.active)
    	{
			_currencyShakeTime = 0f;
	    	if(changeNum < 0) //消耗游戏币
	    	{
	    		_goldColor = Color.red;
	    	}
	    	else //得到游戏币
	    	{
	    		_goldColor = Color.green;
	    	}
	    	lblGold.color = _goldColor;
	    	int playNum = -changeNum;
	    	string prefix = LanguageManager.GetText("consume_prefix");
	    	string color = LanguageManager.GetText("consume_color");
//	    	if(changeNum >= 0)
//	    	{
//	    		playNum = changeNum;
//	    		prefix = LanguageManager.GetText("attain_prefix");
//	    		color = LanguageManager.GetText("attain_color");
//	    		BtnSellItemSure.sSellItem = false;
//	    	}
	    	FloatMessage.GetInstance().PlayFloatMessage(
	    		prefix + color + playNum + Constant.COLOR_END + LanguageManager.GetText("consume_money"),
	    		UIManager.Instance.getRootTrans(), Vector3.zero, Vector3.zero);
	    	StartCoroutine(shakeLabel(lblGold, false));
    	}
    }
    
    void OnMoneyChange(int changeNum)
    {
    	UILabel lblDiamond = transform.FindChild("diamond/lbl_diamond").GetComponent<UILabel>();
    	if(lblDiamond != null && lblDiamond.active)
    	{
			_moneyShakeTime = 0f;
	    	if(changeNum < 0) //消耗钻石
	    	{
	    		_diamondcolor = Color.red;
	    	}
	    	else
	    	{
	    		_diamondcolor = Color.green;
	    	}
	    	lblDiamond.color = _diamondcolor;
	    	int playNum = -changeNum;
	    	string prefix = LanguageManager.GetText("consume_prefix");
	    	string color = LanguageManager.GetText("consume_color");
//	    	if(BtnSellItemSure.sSellItem || changeNum>=0)
//	    	{
//	    		playNum = changeNum;
//	    		prefix = LanguageManager.GetText("attain_prefix");
//	    		color = LanguageManager.GetText("attain_color");
//	    		BtnSellItemSure.sSellItem = false;
//	    	}
	    	FloatMessage.GetInstance().PlayFloatMessage(
	    		prefix + color + playNum + Constant.COLOR_END + LanguageManager.GetText("consume_diamond"), 
	    		UIManager.Instance.getRootTrans(), Vector3.zero, Vector3.zero);
	    	StartCoroutine(shakeLabel(lblDiamond, true));
    	}
    }
    
    IEnumerator shakeLabel(UILabel label, bool isMoney)
    {
    	float calTime;
    	if(isMoney)
    		calTime = _moneyShakeTime;
    	else
    		calTime = _currencyShakeTime;
    	while (calTime < Constant.BLINK_TIME) 
    	{
    		yield return new WaitForSeconds(Constant.BLINK_INTEVAL);
    		if(isMoney)
			{
    			if(label.color == Color.white)
		    	{
		    		label.color = _diamondcolor;
		    	}
		    	else
		    	{
		    		label.color = Color.white;
		    	}
    		}
			else
    		{
		    	if(label.color == Color.white)
		    	{
		    		label.color = _goldColor;
		    	}
		    	else
		    	{
		    		label.color = Color.white;
		    	}
    		}		
	    	calTime += Constant.BLINK_INTEVAL;
	    	if(calTime >= Constant.BLINK_TIME)
	    	{
	    		label.color = Color.white; //还原label的颜色
	    	} 	
    	}    	
    }
}
