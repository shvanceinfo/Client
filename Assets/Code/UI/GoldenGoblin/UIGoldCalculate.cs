using UnityEngine;
using System.Collections;

public class UIGoldCalculate : MonoBehaviour {

	private UILabel m_kKillNum;
    private UILabel m_kKillGaintNum;

    private UILabel btn1_Gold;
    private UILabel btn2_Gold;
    private UILabel btn3_Gold;

    void Awake()
    {
		m_kKillNum = transform.Find("lbl_kill_num").GetComponent<UILabel>();
        m_kKillGaintNum = transform.Find("lbl_kill_gaint_num").GetComponent<UILabel>();
		btn1_Gold = transform.Find("btn1/count").GetComponent<UILabel>();
        btn2_Gold = transform.Find("btn2/count").GetComponent<UILabel>();
        btn3_Gold = transform.Find("btn3/count").GetComponent<UILabel>();
    }

    public void Start()
    {
        m_kKillNum.text =Global.FormatStrimg("击杀 ", 
            "[ff5a00]", Constant.COLOR_END);

        m_kKillNum.text += Global.FormatStrimg(BattleGoblin.GetInstance().m_nKilledNum.ToString(), 
            "[ffffff]", Constant.COLOR_END);


        m_kKillGaintNum.text =Global.FormatStrimg("击杀 ", 
            "[ff5a00]", Constant.COLOR_END);

        m_kKillGaintNum.text += Global.FormatStrimg(BattleGoblin.GetInstance().m_nKilledGaintNum.ToString(), 
            "[ffffff]", Constant.COLOR_END);

        int money=BattleGoblin.GetInstance().CalGainedMoney();
        btn1_Gold.text = money.ToString();
        btn2_Gold.text = (money*2).ToString();
        btn3_Gold.text = (money*5).ToString();
    }

	// Update is called once per frame
	void Update () 
	{
	
	}

   
}
