using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;

public class PlayerChange : MonoBehaviour
{
	 
	const int POWER_LEN = 6;
	private Transform _trans;
	private UILabel lblName;	//人物名字
	private UILabel lblLevel;	//人物等级
	private UILabel lblDiamond;	//钻石数量
	private UILabel lblGold;	//金钱数量
	private UILabel engeryLbl;   //体力文字
	private UISprite engeryBar;	 //体力底板
	private UISprite[] powerSps; //战斗力的UI
	private UISprite playerbg;	//人物头像
	private UILabel lblShuiJing; //水晶数量
 
	private Color _goldColor = Color.white; //货币发生变化
	private Color _diamondcolor = Color.white;	//金钱发生变化
	private float  _currencyShakeTime;	   //计时器
	private float  _moneyShakeTime;

	void Awake ()
	{
		this._trans = this.transform;
		playerbg = this._trans.parent.FindChild ("playerInfo/player").GetComponent<UISprite> ();
		engeryLbl = this._trans.Find ("power/lbl_power").GetComponent<UILabel> ();
		engeryBar = this._trans.Find ("power/tili").GetComponent<UISprite> ();
		lblName = this._trans.FindChild ("lbl_name").GetComponent<UILabel> ();
		lblLevel = this._trans.parent.FindChild ("playerInfo/lbl_level").GetComponent<UILabel> ();
		lblDiamond = this._trans.FindChild ("diamond/lbl_diamond").GetComponent<UILabel> ();
		lblGold = this._trans.FindChild ("gold/lbl_gold").GetComponent<UILabel> ();
		lblShuiJing = this._trans.Find ("shuijing/lbl_shuijing").GetComponent<UILabel> ();
 
		#region 初始化战斗力精灵
		powerSps = new UISprite[POWER_LEN];
		for (int i=1; i<=POWER_LEN; i++) {
			UISprite sp = this._trans.parent.Find ("playerInfo/fightPower/num" + i).GetComponent<UISprite> ();
			powerSps [i - 1] = sp;
		}
		#endregion
		
	}

	void OnEnable ()
	{
 
		EventDispatcher.GetInstance ().PlayerAsset += OnPlayerAsset;
		EventDispatcher.GetInstance ().PlayerProperty += OnPlayerProperty;
		DataChangeNotifyer.GetInstance ().EventChangeCurrency += OnCurrencyChange;
		DataChangeNotifyer.GetInstance ().EventChangeMoney += OnMoneyChange;
		
		CommonMediator mediator;
		if (Gate.instance.hasMediator (MediatorName.COMMON_MEDIATOR))
			mediator = (CommonMediator)Gate.instance.retrieveMediator (MediatorName.COMMON_MEDIATOR);
		else {
			mediator = new CommonMediator (MediatorName.COMMON_MEDIATOR);
			Gate.instance.registerMediator (mediator);
		}
		//mediator.Change = this;
	}

	void OnDisable ()
	{
		EventDispatcher.GetInstance ().PlayerAsset -= OnPlayerAsset;
		EventDispatcher.GetInstance ().PlayerProperty -= OnPlayerProperty;
		DataChangeNotifyer.GetInstance ().EventChangeCurrency -= OnCurrencyChange;
		DataChangeNotifyer.GetInstance ().EventChangeMoney -= OnMoneyChange;
		if (Gate.instance.hasMediator (MediatorName.COMMON_MEDIATOR)) {
			CommonMediator mediator = (CommonMediator)Gate.instance.retrieveMediator (MediatorName.COMMON_MEDIATOR);
			mediator.Change = null;
			Gate.instance.removeMediator (MediatorName.COMMON_MEDIATOR);
		}
	}
	// Use this for initialization
	void Start ()
	{
		
		lblName.text = CharacterPlayer.character_property.nick_name;   //初始化名字
		updateEngery (CharacterPlayer.character_property.currentEngery);//初始化体力
		//初始化人物头像
		switch (CharacterPlayer.character_property.career) {
		case CHARACTER_CAREER.CC_SWORD:
			playerbg.spriteName = Constant.Fight_WarriorHandIcon;
			 
			break;
		case  CHARACTER_CAREER.CC_ARCHER:
			playerbg.spriteName = Constant.Fight_ArcherHandIcon;
			break;
		case CHARACTER_CAREER.CC_MAGICIAN:
			playerbg.spriteName = Constant.Fight_MagicHandIcon;
			break;
		default:
			break;
		}
		OnPlayerAsset ();  //初始化资源数据
		OnPlayerProperty ();//初始化角色属性数据
	}
	 
	void OnPlayerAsset ()
	{
		lblDiamond.text = CharacterPlayer.character_asset.diamond.ToString ();
		lblGold.text = CharacterPlayer.character_asset.gold.ToString ();
		
		
	}

	void OnPlayerProperty ()
	{

		lblLevel.text = CharacterPlayer.character_property.getLevel ().ToString ();
		int fightPower = CharacterPlayer.character_property.getFightPower ();
		FloatBloodNum.Instance.setPowerSp (fightPower, powerSps);
	}
    
	void OnCurrencyChange (int changeNum)
	{
		if (lblGold != null && lblGold.active) {
			_currencyShakeTime = 0f;
			if (changeNum < 0) { //消耗游戏币
				_goldColor = Color.red;
			} else {
				_goldColor = Color.green;
			}
			lblGold.color = _goldColor;
			StartCoroutine (shakeLabel (lblGold, false));
		}
	}
    
	void OnMoneyChange (int changeNum)
	{
		if (lblDiamond != null && lblDiamond.active) {
			_moneyShakeTime = 0f;
			if (changeNum < 0) { //消耗钻石
				_diamondcolor = Color.red;
			} else {
				_diamondcolor = Color.green;
			}
			lblDiamond.color = _diamondcolor;
			StartCoroutine (shakeLabel (lblDiamond, true));
		}
	}
    
	IEnumerator shakeLabel (UILabel label, bool isMoney)
	{
		float calTime;
		if (isMoney)
			calTime = _moneyShakeTime;
		else
			calTime = _currencyShakeTime;
		while (calTime < Constant.BLINK_TIME) {
			yield return new WaitForSeconds(Constant.BLINK_INTEVAL);
			if (isMoney) {
				if (label.color == Color.white) {
					label.color = _diamondcolor;
				} else {
					label.color = Color.white;
				}
			} else {
				if (label.color == Color.white) {
					label.color = _goldColor;
				} else {
					label.color = Color.white;
				}
			}		
			calTime += Constant.BLINK_INTEVAL;
			if (calTime >= Constant.BLINK_TIME) {
				label.color = Color.white; //还原label的颜色
			}
		}    	
	}
    
	//更新体力信息
	public void updateEngery (int curengeEngery)
	{
		engeryLbl.text = curengeEngery.ToString () + "/" + Constant.ENGERY;
		float fillRate = (float)curengeEngery / Constant.ENGERY;
		engeryBar.fillAmount = fillRate;
	}
}
