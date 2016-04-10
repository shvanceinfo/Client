using UnityEngine;
using System.Collections;

public class UIEnterGoldenGoblin : MonoBehaviour
{
    UIGoldenGoblin uiGoblin;

    void Awake()
    {
        uiGoblin = transform.parent.parent.GetComponent<UIGoldenGoblin>();
    }

    void OnClick()
    {
		//NPCManager.Instance.ModelCamera.transform.FindChild("huangjingebulin_UI").gameObject.SetActive(true);
		
        if (uiGoblin.m_nTodayBuyNum >= 10)
        {
            FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("golden_goblin_buy_ticket_not_enough"), true, UIManager.Instance.getRootTrans());
        }
        else
        {
            if (uiGoblin.m_nEnterTimes <= 0)
            {
                int nNeedMoney = 10 + uiGoblin.m_nTodayBuyNum * 10;

                if (CharacterPlayer.character_asset.diamond >= nNeedMoney)
                {
                    UIManager.Instance.ShowDialog(eDialogSureType.ePurchaseGoldenGoblinTicket,
                    string.Format(LanguageManager.GetText("msg_buy_golden_goblin_ticket"), nNeedMoney));
					
					// here we should keep golden goblin hide
					NPCManager.Instance.ModelCamera.transform.FindChild("huangjingebulin_UI").gameObject.SetActive(false);
					
                }
                else
                {
                    FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("golden_goblin_double_benefit_diamond_not_enough"), true, UIManager.Instance.getRootTrans());
                }
            }
            else
            {
                MessageManager.Instance.SendAskEnterGoldenGoblin();
            }
        }
    }
}