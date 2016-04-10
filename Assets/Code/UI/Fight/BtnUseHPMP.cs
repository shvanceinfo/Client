using UnityEngine;
using System.Collections;
using System;
using NetGame;

public class BtnUseHPMP : MonoBehaviour 
{
    private UISprite m_kCoolDown;
    private UISprite skillIcon;

    public UILabel m_kCurNum;
    public UILabel m_kDiamondNum;

    public UISprite diamond_icon;

    public int m_nUsedHPMP;

    public int m_nUsedHPMPConfigID;

    void Awake()
    {
        m_kCoolDown = transform.FindChild("cooldown").GetComponent<UISprite>();
        diamond_icon = transform.FindChild("diamond_icon").GetComponent<UISprite>();

        m_kCurNum = transform.FindChild("num").GetComponent<UILabel>();

        m_kDiamondNum = transform.FindChild("diamond_num").GetComponent<UILabel>();


        m_nUsedHPMP = ConfigDataManager.GetInstance().getMapConfig().getMapData(CharacterPlayer.character_property.getServerMapID()).nXuePing;

        m_nUsedHPMPConfigID = 1;
    }

    void showHpBuy()
    {
        diamond_icon.enabled = true;
        m_kDiamondNum.enabled = true;
        m_kCurNum.enabled = false;
    }

    void showHpCount()
    {
        m_kCurNum.enabled = true;
        diamond_icon.enabled = false;
        m_kDiamondNum.enabled = false;
    }
	// Use this for initialization
	void Start () 
    {
        if (m_nUsedHPMP <= 0)
        {
            showHpBuy();    //显示购买钻石价格
        }
        else { showHpCount(); }
        m_kCurNum.text = m_nUsedHPMP.ToString();
        m_kDiamondNum.text = ConfigDataManager.GetInstance().GetHPMPConfig().getHPMPData(m_nUsedHPMPConfigID).dia_price.ToString();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!MainLogic.sMainLogic.isGameSuspended()) //只有在游戏进行才计算
        {
            float fCurTime = CharacterPlayer.sPlayerMe.GetComponent<CoolDownProperty>().GetCD(-1);
            //float fLeftTime = Global.drupCoolDownTime - fCurTime;
            float r=1-(fCurTime / Global.drupCoolDownTime);

            m_kCoolDown.fillAmount = r == 1 ? 0 : r; 
        }
	}

    private void OnClick()
    {
        

        if (CharacterPlayer.sPlayerMe.GetComponent<CoolDownProperty>().GetCD(-1) > 0.0f)
        {
            return;
        }

        if (CharacterPlayer.sPlayerMe.GetProperty().HPMPIsEnough())
        {
            FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("cur_hp_mp_enough"), true, UIManager.Instance.getRootTrans());
        }
        else
        {
            if (m_nUsedHPMP > 0)
            {

                SendUseHPMPMsg();
                //CharacterPlayer.sPlayerMe.GetProperty().FillHPMPFull();
                CharacterPlayer.sPlayerMe.GetComponent<CoolDownProperty>().AddUseHPMPObj();
                m_nUsedHPMP--;
                m_kCurNum.text = m_nUsedHPMP.ToString();
            }
            else
            {
                int nDiamondPrice = ConfigDataManager.GetInstance().GetHPMPConfig().getHPMPData(m_nUsedHPMPConfigID).dia_price;

                if (CharacterPlayer.character_asset.diamond >= nDiamondPrice)
                {
                    //CharacterPlayer.sPlayerMe.GetProperty().FillHPMPFull();
                    CharacterPlayer.sPlayerMe.GetComponent<CoolDownProperty>().AddUseHPMPObj();
                    SendUseHPMPMsg();

                    m_nUsedHPMPConfigID++;

                    m_kDiamondNum.text = ConfigDataManager.GetInstance().GetHPMPConfig().getHPMPData(m_nUsedHPMPConfigID).dia_price.ToString();
                }
                else
                {
                    FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("use_hp_mp_diamond_not_enough"), true, UIManager.Instance.getRootTrans());
                }
            }
            if (m_nUsedHPMP <= 0)
            {
                showHpBuy();    //显示购买钻石价格
            }
            else { showHpCount(); }
        }
        
    }

    public void SendUseHPMPMsg()
    {
        // 发送使用药品消息
        NetBase net = NetBase.GetInstance();
        GCAskUseItem msg = new GCAskUseItem(1, 0);
        net.Send(msg.ToBytes());
    }
}
