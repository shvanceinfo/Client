/**该文件实现的基本功能等
function: 实现主界面的View显示
author:zyl
date:2014-4-4
**/
using UnityEngine;
using System.Collections;
using mediator;
using MVC.entrance.gate;
using manager;
using model;
using helper;

public class MainView : MonoBehaviour
{
	public Vector3 _bottomBaseLocation = new Vector3(400,1.6f,0);
	public Vector3 _bottomStepValue = new Vector3(-80,0,0);
	public int _bottomCount =0;

	public Vector3 _leftMidBaseLocation = new Vector3(0,0,0);
	public Vector3 _leftMidStepValue = new Vector3(0,-100,0);
	public int _leftMidCount =0;

	public Vector3 _rightTopBaseLocation = new Vector3(-120,0,0);
	public Vector3 _rightTopStepValue = new Vector3(-80,-80,0);
	public int _rightTopCount =0;
	public int _rightTopRowCount = 6;


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
	private UISprite _emailMsg; //邮箱标示
	private UITextList _talkList;
	private GlinkNotify _emailNot;
	private UILabel _lblVip;
	private GlinkNotify _friendNotify;
	private Color _goldColor = Color.white; //货币发生变化
	private Color _diamondcolor = Color.white;	//金钱发生变化
	private float  _currencyShakeTime;	   	//计时器
	private float  _moneyShakeTime;			//金钱效果计时器

    private UILabel _channelLbl;
    private UISprite _hideLbl;
    private OpenRightMenu _openMenu;
    private funcMgr _openFunc;

    //新手引导提示信息
    private GameObject _guideInfo;
    private UITexture _guideIcon;
    private UISprite _guideBoder;
    private UILabel _guideTip;
    private UILabel _guideName;
    private UILabel _guideButtonText;
    private UISprite _guideIconSprite;
	void Awake ()
	{
		this._trans = this.transform;

        _guideInfo = this._trans.F("top_right/tip_fast");
        _guideIcon = _guideInfo.F<UITexture>("Icon");
        _guideBoder = _guideInfo.F<UISprite>("Boder");
        _guideTip = _guideInfo.F<UILabel>("LblFunc");
        _guideName = _guideInfo.F<UILabel>("LblName");
        _guideButtonText = _guideInfo.F<UILabel>("ButtonTrigger/Label");
        _guideIconSprite = _guideInfo.F<UISprite>("IconSprite");
        _guideInfo.SetActive(false);
        _guideIconSprite.gameObject.SetActive(false);

		_lblVip = this._trans.Find ("top_left/vip/lbl_vip").GetComponent<UILabel> ();
		playerbg = this._trans.Find ("top_left/player/playerInfo/player").GetComponent<UISprite> ();
		engeryLbl = this._trans.Find ("top_left/player/data/power/lbl_power").GetComponent<UILabel> ();
		engeryBar = this._trans.Find ("top_left/player/data/power/tili").GetComponent<UISprite> ();
		lblName = this._trans.Find ("top_left/player/data/lbl_name").GetComponent<UILabel> ();
		lblLevel = this._trans.Find ("top_left/player/playerInfo/lbl_level").GetComponent<UILabel> ();
		lblDiamond = this._trans.Find ("top_left/player/data/diamond/lbl_diamond").GetComponent<UILabel> ();
		lblGold = this._trans.Find ("top_left/player/data/gold/lbl_gold").GetComponent<UILabel> ();
		lblShuiJing = this._trans.Find ("top_left/player/data/shuijing/lbl_shuijing").GetComponent<UILabel> ();

		_emailMsg = _trans.FindChild ("top_left/Email/notify").GetComponent<UISprite> ();
		_emailNot = _emailMsg.GetComponent<GlinkNotify> ();
        //_emailMsg.gameObject.SetActive (false);

		_friendNotify = _trans.FindChild ("bottom/func/active/friend/notify").GetComponent<GlinkNotify> ();

		_talkList = _trans.FindChild ("bottom/func/chat/TextListBoard").GetComponent<UITextList> ();

        _channelLbl = _trans.FindChild("top_left/Email/Channel_Function/Label").GetComponent<UILabel>();
        _hideLbl = _trans.FindChild("top_left/Email/Hide_Function/Sprite").GetComponent<UISprite>();
        _openMenu = _trans.FindChild("top_right/showFunc").GetComponent<OpenRightMenu>();
        _openFunc=_trans.F<funcMgr>("bottom/func");
		#region 初始化战斗力精灵
		powerSps = new UISprite[POWER_LEN];
		for (int i=1; i<=POWER_LEN; i++) {
			UISprite sp = this._trans.Find ("top_left/player/playerInfo/fightPower/num" + i).GetComponent<UISprite> ();
			powerSps [i - 1] = sp;
		}
		#endregion
		
	}

	void OnEnable ()
	{
		MainManager.Instance.RegsiterEvent ();//注册绑定的事件
		Gate.instance.registerMediator (new MainMediator (this)); //注册主页面控制器
		#region 注册公共控制器
		CommonMediator mediator;
		if (Gate.instance.hasMediator (MediatorName.COMMON_MEDIATOR))
			mediator = (CommonMediator)Gate.instance.retrieveMediator (MediatorName.COMMON_MEDIATOR);
		else {
			mediator = new CommonMediator (MediatorName.COMMON_MEDIATOR);
			Gate.instance.registerMediator (mediator);
		}
		mediator.Change = this;
		#endregion
		
	}

	void OnDisable ()
	{
		MainManager.Instance.RemoveEvent ();//注销绑定的事件
		Gate.instance.removeMediator (MediatorName.MAIN_MEDIATOR);//注销主页面控制器 
		#region 注销公共控制器
		if (Gate.instance.hasMediator (MediatorName.COMMON_MEDIATOR)) {
			CommonMediator mediator = (CommonMediator)Gate.instance.retrieveMediator (MediatorName.COMMON_MEDIATOR);
			mediator.Change = null;
			Gate.instance.removeMediator (MediatorName.COMMON_MEDIATOR);
		}
		#endregion
		
	}
	
	void Start ()
	{
        GuideInfoManager.Instance.IsFristLogin = false;
        GuideInfoManager.Instance.CheckItemTrigger();
		MainManager.Instance.Init ();//初始化数据
		UpdateVipLevel ();


		UpdateShowOrHideUi ();
        UpdateChannel();
        UpdateHidePeople();
	}

    private void Update()
    {
        UpdateGuideInfo();
    }
    public void UpdateCloseMenu()
    {
        _openMenu.CloseMenu();
        if (_openFunc.IsOpen())
            _openFunc.Close();
    }

    public void UpdateChannel()
    {
        _channelLbl.text = ViewHelper.FormatLanguage("channel_line", ChannelManager.Instance.CurLine.Id);
    }

    public void UpdateHidePeople()
    {
        if (SettingManager.Instance.Hide_Display)
        {
            _hideLbl.spriteName = ViewHelper.FormatLanguage("setting_display");
        }
        else
        {
            _hideLbl.spriteName = ViewHelper.FormatLanguage("setting_hide");
        }
    }

	/// <summary>
	/// 更新功能块的显示或者隐藏
	/// </summary>
	public void UpdateShowOrHideUi ()
	{
		this._bottomCount = 0;
		this._leftMidCount = 0;
		this._rightTopCount = 0;
		FastOpenVo func;
		for (int i = 0, max = FastOpenManager.Instance.FastOpenList.Count; i < max; i++) {
			func = FastOpenManager.Instance.FastOpenList [i];
			if (!string.IsNullOrEmpty (func.UIUrl)) {
				Transform funTrans = this._trans.FindChild (func.UIUrl);
				if (funTrans == null) {
					continue;
				}
				if (!FastOpenManager.Instance.DirOpenTypeValue.ContainsKey(func.Type)) {
					funTrans.gameObject.SetActive (false);
					continue;
				} 
//				print(FastOpenManager.Instance.DirOpenTypeValue [func.Type]);
				if (func.Param > FastOpenManager.Instance.DirOpenTypeValue [func.Type]) {//
					funTrans.gameObject.SetActive (false);
				}
				else {
					switch (func.Location) {
					case LocationType.None:
						funTrans.gameObject.SetActive (false);
						break;
					case LocationType.RightTop:
						funTrans.gameObject.SetActive (true);
						int xVar = this._rightTopCount%this._rightTopRowCount;
						int yVar = this._rightTopCount/this._rightTopRowCount;
						this._trans.FindChild (func.UIUrl).localPosition = new Vector3 (this._rightTopBaseLocation.x+xVar*this._rightTopStepValue.x , this._rightTopBaseLocation.y + yVar*this._rightTopStepValue.y, this._rightTopBaseLocation.z );
						this._rightTopCount++;
						FastOpenManager.Instance.AddFunctionEffect(func);
						break;
					case LocationType.LeftTop:
//						this._trans.FindChild (func.UIUrl).gameObject.SetActive (true);
						break;
					case LocationType.LeftMid:
						funTrans.gameObject.SetActive (true);
						Vector3 leftMidStepVal = this._leftMidCount * this._leftMidStepValue;
						this._trans.FindChild (func.UIUrl).localPosition = new Vector3 (this._leftMidBaseLocation.x + leftMidStepVal.x, this._leftMidBaseLocation.y + leftMidStepVal.y, this._leftMidBaseLocation.z + leftMidStepVal.z);
						this._leftMidCount++;
						break;
					case LocationType.RightMid:
//						this._trans.FindChild (func.UIUrl).gameObject.SetActive (true);
						break;
					case LocationType.Bottom:
						funTrans.gameObject.SetActive (true);
						Vector3 bottomStepVal = this._bottomCount * this._bottomStepValue;
						this._trans.FindChild (func.UIUrl).localPosition = new Vector3 (this._bottomBaseLocation.x + bottomStepVal.x, this._bottomBaseLocation.y + bottomStepVal.y, this._bottomBaseLocation.z + bottomStepVal.z);
						this._bottomCount++;
						FastOpenManager.Instance.AddFunctionEffect(func);
						break;
					default:
						break;
					}
				}
			}else{  //无路径,内部的处理
				if (!FastOpenManager.Instance.DirOpenTypeValue.ContainsKey(func.Type)) {
					continue;
				} 
				if (func.Param <= FastOpenManager.Instance.DirOpenTypeValue [func.Type]) {
					switch (func.Location) {
					case LocationType.RightTop:
						FastOpenManager.Instance.AddFunctionEffect(func);
						break;
					case LocationType.Bottom: 
						FastOpenManager.Instance.AddFunctionEffect(func);
						break;
					default:
						break;
					}
				}
			}
		}
		FastOpenManager.Instance.OpenNewFunctionWindow ();//开始效果
	}

	public void UpdateFriendNotiy (bool isactive)
	{
		_friendNotify.activeNotify (isactive);
	}

	//更新用户信息
	public void UpdatePeopleInfo (string name, string spriteName)
	{
		lblName.text = name;
		playerbg.spriteName = spriteName;
	}
	
	//更新资源
	public 	void UpdatePlayerAsset (int diamond, int gold, int crystal)
	{
		lblDiamond.text = diamond >= 10000 ? diamond / 10000 + "万" : diamond.ToString ();
		lblGold.text = gold >= 10000 ? gold / 10000 + "万" : gold.ToString ();
		lblShuiJing.text = crystal >= 10000 ? crystal / 10000 + "万" : crystal.ToString ();
	}
	
	//更新角色属性
	public void UpdatePlayerProperty (int level, int power)
	{
		lblLevel.text = level.ToString ();
		FloatBloodNum.Instance.setPowerSp (power, powerSps);
	}

	private void AddTalkMsg (TalkVo vo)
	{
		switch (vo.Type) {
		case TalkType.Error:
			_talkList.Add (TalkManager.FormatSystem (vo));
			break;
		case TalkType.World:
			_talkList.Add (TalkManager.FormatWorldText (vo));
			break;
		case TalkType.Guild:
			_talkList.Add (TalkManager.FormatGrildText (vo));
			break;
		case TalkType.Whisper:
			_talkList.Add (TalkManager.FormatWhisper (vo));
			break;
		case TalkType.System:
			_talkList.Add (TalkManager.FormatSystem (vo));
			break;
		case TalkType.Post:
			break;
		case TalkType.SystemAndPost:
			_talkList.Add (TalkManager.FormatSystem (vo));
			break;
		default:
			break;
		}
	}

	public void UpdateTalkMsg ()
	{
		_talkList.Clear ();
		BetterList<TalkVo> talks = TalkManager.Instance.Contents;

		//每次只更新2条
		if (talks.size >= 2) {
			AddTalkMsg (talks [talks.size - 2]);
			AddTalkMsg (talks [talks.size - 1]);
		} else if (talks.size == 1) {
			AddTalkMsg (talks [talks.size - 1]);
		}
        
	}
    
	//更新游戏金钱
	public void UpdatCurrencyChange (int changeNum)
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
    
	//更新钻石
	public void UpdatMoneyChange (int changeNum)
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

	public void UpdateVipLevel ()
	{
		_lblVip.text = VipManager.Instance.CurVip.VipId.ToString ();
	}

	public void UpdateEmailNotify (bool isShow)
	{
		_emailMsg.gameObject.SetActive (isShow);
		_emailNot.activeNotify (isShow);
	}
    
	//动画效果
	IEnumerator shakeLabel (UILabel label, bool isMoney)
	{
		float calTime;
		if (isMoney)
			calTime = _moneyShakeTime;
		else
			calTime = _currencyShakeTime;
		while (calTime < Constant.BLINK_TIME) {
			yield return new WaitForSeconds (Constant.BLINK_INTEVAL);
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


    //更新新手引导信息
    public void UpdateGuideInfo()
    {
        GuideInfoData data = GuideInfoManager.Instance.FindTop();
        if (data != null)
        {
            DisplayGuideInfo(data);
        }
        else {
            HidenGuideInfo();
        }
    }
    private GuideInfoData _guideold;
    private void DisplayGuideInfo(GuideInfoData data)
    {
        if (_guideold==null||!_guideold.Equals(data) )
        {
            _guideInfo.SetActive(true);
            GuideInfoVo vo = GuideInfoManager.Instance.FindVoByType(data.Type);
            _guideIconSprite.gameObject.SetActive(false);
            _guideIcon.gameObject.SetActive(false);
            switch (data.Type)
            {
                case GuideInfoTrigger.Power:
                case GuideInfoTrigger.UseItem:
                    _guideIcon.gameObject.SetActive(true);
                    _guideIcon.mainTexture = SourceManager.Instance.getTextByIconName(data.Icon);
                    break;
                case GuideInfoTrigger.UnLockSkill:
                    _guideIcon.gameObject.SetActive(true);

                    _guideIcon.mainTexture = SourceManager.Instance.getTextByIconName(data.Icon, PathConst.SKILL_PATH);
                    break;
                case GuideInfoTrigger.UnLockTelent:
                    _guideIcon.gameObject.SetActive(true);

                    _guideIcon.mainTexture = SourceManager.Instance.getTextByIconName(data.Icon, PathConst.TALENT_PATH);
                    break;
                case GuideInfoTrigger.Level:
                case GuideInfoTrigger.VipLevel:
                case GuideInfoTrigger.TaskCompalte:
                    switch (data.Vo.IconType)
                    {
                        case GuideInfoIconType.None:
                            break;
                        case GuideInfoIconType.Atlas:
                            _guideIconSprite.gameObject.SetActive(true);
                            _guideIconSprite.atlas = BundleMemManager.Instance.loadResource(data.Vo.IconPath,typeof(UIAtlas)) as UIAtlas;
                            _guideIconSprite.spriteName = data.Vo.IconName;
                            break;
                        case GuideInfoIconType.Icon:
                            _guideIcon.gameObject.SetActive(true);

                            _guideIcon.mainTexture = SourceManager.Instance.getTextByIconName(data.Vo.IconName, data.Vo.IconPath+"/");
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            switch (data.Type)
            {
                case GuideInfoTrigger.Power:
                case GuideInfoTrigger.UseItem:
                case GuideInfoTrigger.UnLockSkill:
                case GuideInfoTrigger.UnLockTelent:
                    _guideBoder.spriteName = BagManager.Instance.getItemBgByType(data.Quality, true);
                    _guideTip.text=vo.TipInfo;
                    _guideName.text=data.Name;
                    break;
                case GuideInfoTrigger.Level:
                case GuideInfoTrigger.VipLevel:
                case GuideInfoTrigger.TaskCompalte:
                    if (string.IsNullOrEmpty(data.Vo.IconBackground))
                    {
                        _guideBoder.spriteName = null;

                    }
                    else {
                        _guideBoder.spriteName = data.Vo.IconBackground;
                    }
                    _guideTip.text=data.Vo.TipInfo;
                    _guideName.text=data.Vo.TipName;
                    break;
                default:
                    break;
            }
            
            _guideButtonText.text=vo.ButtonText;
            _guideold = data;
        }
    }

    private void HidenGuideInfo()
    {
        if (_guideInfo.activeSelf)
        {
            _guideInfo.SetActive(false);
        }
    }
}
