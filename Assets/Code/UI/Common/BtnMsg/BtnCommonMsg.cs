/**该文件实现的基本功能等
function: 实现一些通用的按钮消息发送
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;
using helper;

public class BtnCommonMsg : MonoBehaviour
{
	const string BUY_ENGERY = "add_power";
	
	void OnClick()
	{
		bool clickChallenge = false;
		switch (gameObject.name) 
		{
			case BUY_ENGERY:
			{
				bool showError = false;
				string showMsg = "";
				EnergyData data = SweepManager.Instance.EnergyHash[CharacterPlayer.character_property.buyEngeryNum+1] as EnergyData;
				int needDiamond = data.needDiamond;
                if (CharacterPlayer.character_property.buyEngeryNum>=VipManager.Instance.TiliCount)
                {
                    ViewHelper.DisplayMessageLanguage("tili_buycount");
                    return;
                }
				if(needDiamond > CharacterPlayer.character_asset.diamond)
				{
					showError = true;
					showMsg = LanguageManager.GetText("buy_energy_not_diamond");
				}
				else if(CharacterPlayer.character_property.currentEngery + data.addEngeryNum > 200)
				{
					showError = true;
					showMsg = LanguageManager.GetText("buy_energy_enough_energy");
				}
				else
				{
					showError = false;
					showMsg = LanguageManager.GetText("buy_energy_sure");
					showMsg = showMsg.Replace(Constant.REPLACE_PARAMETER_1, needDiamond.ToString());
                    showMsg = showMsg.Replace(Constant.REPLACE_PARAMETER_2, data.addEngeryNum.ToString());
				}
				if(showError)
					FloatMessage.GetInstance().PlayFloatMessage(showMsg, UIManager.Instance.getRootTrans(), 
									new Vector3(0f, 100f, -50f), new Vector3(0f, 200f, -50f));
				else
					UIManager.Instance.ShowDialog(eDialogSureType.eSureBuyEngery, showMsg);
			}
			break;
			default:
			break;
		}
	}
}
