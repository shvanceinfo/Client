/**该文件实现的基本功能等
function: 关卡章节视图表现
author:ljx
date:2014-04-03
**/
using UnityEngine;
using model;
using manager;
using mediator;
using MVC.entrance.gate;

public class ChapterView : MonoBehaviour
{
	private const int FIX_WIDTH = 1136;
	private const float INACTIVE_RATE = 0.8f;
	Transform _trans;
	private GameObject _moveLayer; //移动的层次
	private GameObject _chapter1;  //章节的预支件1
	private GameObject _chapter2;  //章节的预支件2
	private UITexture _bgTexture1; //背景1
	private UITexture _bgTexture2; //背景2
	private UILabel _title1;  //标题1
	private UILabel _title2;  //标题2
	private Transform _itemParent1; //第一个放置章节的位置
	private Transform _itemParent2; //第二个放置章节的位置
	private BetterList<RaidView> _raid1Views; //当前章节的关卡列表
	private BetterList<RaidView> _raid2Views;
	private UISprite _normalBtn;		//普通关卡按钮
	private UISprite _hardBtn;			//精英关卡按钮
	private GameObject _hardRaid;       //精英关卡控件
	private UILabel _energyLabel;		//体力的标签
	private GameObject _nextBtn;       //预览下一章节按钮  
	private GameObject _prevBtn;       //预览上一章节按钮
	
	GameObject _chapterAward;		//章节奖励的父类对象
	UILabel _awardInfo;				//章节星星信息
	GameObject _awardBtn;			//章节奖励按钮
	GameObject _awardLbl;			//是否已经拿过奖励的信息
	UISprite _awardBg;				//章节奖励的按钮图片 
	
	void Awake ()
	{
		this._trans = this.transform;
		_raid1Views = new BetterList<RaidView> ();
		_raid2Views = new BetterList<RaidView> ();
		_moveLayer = transform.Find ("moveLayer").gameObject;
		_chapter1 = transform.Find ("moveLayer/chapter1").gameObject;
		_chapter2 = transform.Find ("moveLayer/chapter2").gameObject;
		_bgTexture1 = _chapter1.transform.Find ("bg/gateBg").GetComponent<UITexture> ();  //背景
		_bgTexture2 = _chapter2.transform.Find ("bg/gateBg").GetComponent<UITexture> ();
		_title1 = _chapter1.transform.Find ("title/title").GetComponent<UILabel> ();  //标题
		_title2 = _chapter2.transform.Find ("title/title").GetComponent<UILabel> ();  //标题
		_itemParent1 = _chapter1.transform.Find ("itemList");
		_itemParent2 = _chapter2.transform.Find ("itemList");

		_normalBtn = transform.Find ("fixLayer/bottom/normalBtn/btnBg").GetComponent<UISprite> ();
		_hardBtn = transform.Find ("fixLayer/bottom/hardBtn/btnBg").GetComponent<UISprite> ();
		_hardRaid = transform.Find ("fixLayer/bottom/hardBtn").gameObject;
		_energyLabel = transform.Find ("fixLayer/bottom/engeryInfo/infoLbl").GetComponent<UILabel> ();
		_nextBtn = transform.Find ("fixLayer/right").gameObject;
		_prevBtn = transform.Find ("fixLayer/left").gameObject;
		this._chapterAward = this._trans.Find ("fixLayer/bottom/chapterAward").gameObject;
		this._awardInfo = this._trans.Find ("fixLayer/bottom/chapterAward/info").GetComponent<UILabel> ();
		this._awardBtn = this._trans.Find ("fixLayer/bottom/chapterAward/award").gameObject;
		this._awardLbl = this._trans.Find ("fixLayer/bottom/chapterAward/award/Label").gameObject;
		this._awardBg = this._trans.Find ("fixLayer/bottom/chapterAward/award/Background").GetComponent<UISprite> ();
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new RaidMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.RAID_MEDIATOR);
		CommonMediator mediator;
		if (Gate.instance.hasMediator (MediatorName.COMMON_MEDIATOR))
			mediator = (CommonMediator)Gate.instance.retrieveMediator (MediatorName.COMMON_MEDIATOR);
		else {
			mediator = new CommonMediator (MediatorName.COMMON_MEDIATOR);
			Gate.instance.registerMediator (mediator);
		}
		mediator.ChapterView = this;
	}
	
	//初始化关卡
	public void initView ()
	{
		clickBtn (RaidManager.Instance.showHard);
		_chapter2.transform.localPosition = new Vector3 (_chapter1.transform.localPosition.x + FIX_WIDTH, _chapter1.transform.localPosition.y, _chapter1.transform.localPosition.x);
		updateEnergy (CharacterPlayer.character_property.currentEngery);
	}
	
	//切换精英跟普通按钮
	public void clickBtn (bool isHard)
	{
		if (RaidManager.Instance.ShowChapter1) {
			_title1.text = RaidManager.Instance.CurrentChapter.chapterName;
			_bgTexture1.mainTexture = SourceManager.Instance.getTextByIconName (RaidManager.Instance.CurrentChapter.chapterIcon, PathConst.RAID_CHAPTER_PATH);
		} else {
			_title2.text = RaidManager.Instance.CurrentChapter.chapterName;
			_bgTexture2.mainTexture = SourceManager.Instance.getTextByIconName (RaidManager.Instance.CurrentChapter.chapterIcon, PathConst.RAID_CHAPTER_PATH);
		}
		setActiveBtn (!isHard); //根据当前关节以及精英度设置按钮
		
 
		
		if (RaidManager.Instance.CurrentChapter.chapterSequence <= RaidManager.Instance.MinChapterSequence) {
			_prevBtn.SetActive (false);
		} else {
			_prevBtn.SetActive (true);
		}
		if (RaidManager.Instance.CurrentChapter.chapterSequence >= RaidManager.Instance.MaxChapterSequence) {
			_nextBtn.SetActive (false);
		} else {
			_nextBtn.SetActive (true);
		}
		
		
		
		initRaidItem (isHard);
		InitChapterAward (isHard);
	}
	
	//更新体力
	public void updateEnergy (int currentEnergy)
	{
		_energyLabel.text = currentEnergy + "/" + 200;
	}

	//切换地图状态
	public void changeRaid (MapVo vo)
	{
		BetterList<RaidView> views;
		if (RaidManager.Instance.ShowChapter1)
			views = _raid1Views;
		else
			views = _raid2Views;
		foreach (RaidView view in views) {
			view.transferView (vo);
		}
	}

	//缓动显示关卡
	public void tweenView (bool showNext)
	{
		clickBtn (RaidManager.Instance.showHard);
		
		TweenPosition tweenPos = UITweener.Begin<TweenPosition> (_moveLayer, 0f);
//		if (tweenPos.tweenFactor!=0) { //只有进度为0才开始执行tween动画
//			return ;
//		}
		
		float currenX = _moveLayer.transform.localPosition.x;
		Vector3 nextPos = Vector3.zero;
		if (showNext) {
			if (RaidManager.Instance.ShowChapter1) { //视野中显示chapter1
				_chapter1.transform.localPosition = new Vector3 (-currenX + FIX_WIDTH, 0f, 0f);
				//_chapter1.transform.localPosition = new Vector3(-currenX, 0f, 0f);
                
			} else { //视野中显示chapter2
				_chapter2.transform.localPosition = new Vector3 (-currenX + FIX_WIDTH, 0f, 0f);
			}
			nextPos = new Vector3 (currenX - FIX_WIDTH, 0f, 0f);
		} else {
			if (RaidManager.Instance.ShowChapter1) {
				_chapter1.transform.localPosition = new Vector3 (-currenX - FIX_WIDTH, 0f, 0f);
				//_chapter1.transform.localPosition = new Vector3(-currenX, 0f, 0f);
			} else {
				// _chapter2.transform.localPosition = new Vector3(-currenX, 0f, 0f);
				_chapter2.transform.localPosition = new Vector3 (-currenX - FIX_WIDTH, 0f, 0f);
			}
			nextPos = new Vector3 (currenX + FIX_WIDTH, 0f, 0f);
		}
		
//		print(tweenPos.tweenFactor);
		tweenPos.duration = 0.5f;
		tweenPos.from = _moveLayer.transform.localPosition;
		tweenPos.to = nextPos;
		tweenPos.enabled = true;
		tweenPos.callWhenFinished = "enableBtn";
		tweenPos.eventReceiver = gameObject;
		enableBtn (); //使得预览按钮不可点击
	}
	//public void tweenView(bool showNext)
	//{
	//    clickBtn(RaidManager.Instance.showHard);
	//    _moveLayer.transform.localPosition = Vector3.zero;
	//    Vector3 nextPos = Vector3.zero;
	//    if (showNext)
	//    {
	//        if (RaidManager.Instance.ShowChapter1) //视野中显示chapter1
	//        {
	//            _chapter1.transform.localPosition = new Vector3(FIX_WIDTH, 0f, 0f);
	//            _chapter2.transform.localPosition = Vector3.zero;
	//        }
	//        else //视野中显示chapter2
	//        {
	//            _chapter2.transform.localPosition = new Vector3(FIX_WIDTH, 0f, 0f);
	//            _chapter1.transform.localPosition = Vector3.zero;
	//        }
	//        nextPos = new Vector3(-FIX_WIDTH, 0f, 0f);
	//    }
	//    else
	//    {
	//        if (RaidManager.Instance.ShowChapter1)
	//        {
	//            _chapter1.transform.localPosition = new Vector3(-FIX_WIDTH, 0f, 0f);
	//            _chapter2.transform.localPosition = Vector3.zero;
	//        }
	//        else
	//        {
	//            _chapter2.transform.localPosition = new Vector3(-FIX_WIDTH, 0f, 0f);
	//            _chapter1.transform.localPosition = Vector3.zero;
	//        }
	//        nextPos = new Vector3(FIX_WIDTH, 0f, 0f);
	//    }
	//    TweenPosition tweenPos = UITweener.Begin<TweenPosition>(_moveLayer, 0f);
	//    tweenPos.duration = 0.5f;
	//    tweenPos.from = Vector3.zero;
	//    tweenPos.to = nextPos;
	//    tweenPos.enabled = true;
	//    tweenPos.callWhenFinished = "enableBtn";
	//    tweenPos.eventReceiver = gameObject;
	//    enableBtn(); //使得预览按钮不可点击
	//}
	
	//显示关卡列表
	private void initRaidItem (bool isHard)
	{
		ChapterVo chapterVo = RaidManager.Instance.CurrentChapter;
		if (chapterVo != null) {
			Transform trans;
			if (RaidManager.Instance.ShowChapter1)
				trans = _itemParent1;
			else
				trans = _itemParent2;
			if (trans != null) {
				foreach (Transform childTrans in trans) {
					Object.Destroy (childTrans.gameObject); //先清空原来的子对象
				}
			}
			BetterList<RaidView> views;
			if (RaidManager.Instance.ShowChapter1)
				views = _raid1Views;
			else
				views = _raid2Views;
			views.Clear (); //先清除原来数据
			foreach (MapVo vo in chapterVo.mapVos) {
				if (vo.isHard == isHard) {
                    GameObject obj = BundleMemManager.Instance.instantiateObj(RaidManager.Instance.RaidPrefab);	 	       
					obj.transform.parent = trans;
					obj.transform.localPosition = vo.gatePos;
					obj.transform.localScale = Vector3.one;
					obj.transform.eulerAngles = Vector3.zero;
					obj.name = vo.mapID.ToString ();
					RaidView raidView = obj.GetComponent<RaidView> ();
					views.Add (raidView);
					raidView.MyMapVo = vo;
					raidView.initView ();
				}		 		
			}
		}
	}

	//精英跟普通按钮的切换
	private void setActiveBtn (bool normalActive)
	{
		string normalSpName = SpriteNameConst.NORMAL_RAID_INACTIVE_SP;
		string hardSpName = SpriteNameConst.HARD_RAID_ACTIVE_SP;
		Vector3 normalScale = new Vector3 (INACTIVE_RATE, INACTIVE_RATE, 0f);
		Vector3 hardScale = Vector3.one;
		_hardBtn.spriteName = SpriteNameConst.HARD_RAID_ACTIVE_SP;
		_normalBtn.spriteName = SpriteNameConst.NORMAL_RAID_INACTIVE_SP;
		if (normalActive) {
			normalSpName = SpriteNameConst.NORMAL_RAID_ACTIVE_SP;
			hardSpName = SpriteNameConst.HARD_RAID_INACTIVE_SP;
			normalScale = Vector3.one;
			hardScale = new Vector3 (INACTIVE_RATE, INACTIVE_RATE, 0f);
		}
		_normalBtn.spriteName = normalSpName;
		_normalBtn.MakePixelPerfect ();
		_normalBtn.transform.localScale = normalScale;      
		if (RaidManager.Instance.IsHardOpen) {  //精英关卡可有打开才显示
			_hardRaid.SetActive (true);
			_hardRaid.GetComponent<BoxCollider> ().enabled = true;
			_hardBtn.spriteName = hardSpName;
			_hardBtn.MakePixelPerfect ();
			_hardBtn.transform.localScale = hardScale;
		} else {
			_hardRaid.SetActive (false);
			_hardRaid.GetComponent<BoxCollider> ().enabled = false;
		}
	}

	//使能按钮，在滑动过程使得精英普通按钮不可点击
	private void enableBtn ()
	{
		_prevBtn.GetComponent<BoxCollider> ().enabled = !_prevBtn.GetComponent<BoxCollider> ().enabled;
		_nextBtn.GetComponent<BoxCollider> ().enabled = !_nextBtn.GetComponent<BoxCollider> ().enabled;
		_hardRaid.GetComponent<BoxCollider> ().enabled = !_hardRaid.GetComponent<BoxCollider> ().enabled;
		_normalBtn.transform.parent.GetComponent<BoxCollider> ().enabled =
            !_normalBtn.transform.parent.GetComponent<BoxCollider> ().enabled;
		_awardBtn.GetComponent<BoxCollider> ().enabled = !_awardBtn.GetComponent<BoxCollider> ().enabled;
	}
	
	/// <summary>
	/// 初始化章节奖励
	/// </summary>
	/// <param name='isHard'>
	/// Is hard.
	/// </param>
	public void InitChapterAward (bool isHard)
	{
		ChapterVo chapterVo = RaidManager.Instance.CurrentChapter;
		int starNumCount = 0;
		MapVo varMap = null;
		int hardCount=0;
		foreach (MapVo vo in chapterVo.mapVos) {//统计星星数量,普通关卡统计普通星星，精英统计精英星星
			if (vo.isHard == isHard) {
				starNumCount += vo.FormatStarNum;
				varMap = vo;
				hardCount++;
			}		 		
		}
		this._awardInfo.text = starNumCount + "/" + hardCount * 3; //星星数量显示
			
		uint mapId = varMap.mapID / 10;
		if (RaidManager.Instance.PassMapHash.ContainsKey (mapId)) {
			sPassMap passmap = (sPassMap)RaidManager.Instance.PassMapHash [mapId]; //得到随机通关地图的信息
			if (passmap.easy == 4) {//是否已经拿过奖励
				this._awardLbl.SetActive (true);
				this._awardBg.color = new Color ((float)1 / 255, 0, 0);
			} else {
				this._awardLbl.SetActive (false);
				this._awardBg.color = new Color (255, 255, 255);
			}
		} else {//如果这关没通过，则必然是可以查看奖励
			this._awardLbl.SetActive (false);
			this._awardBg.color = new Color (255, 255, 255);
		}
	}

	public void OnDestroy ()
	{
		// 界面关闭
		EasyTouchJoyStickProperty.SetJoyStickEnable (true);
	}
}
 