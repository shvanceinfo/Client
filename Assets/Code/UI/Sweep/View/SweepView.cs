/**该文件实现的基本功能等
function: 实现扫荡的界面表现
author:ljx
date:2014-03-11
**/
using UnityEngine;
using System.Collections;
using manager;
using model;
using System;

public class SweepView : MonoBehaviour
{
	private UILabel _leftTitle; //扫荡左边标题
	private UILabel _rightTitle; //扫荡右边标题
	private GameObject _startLeftSweep;		//开始扫荡
	private GameObject _processLeftSweep;	//正在扫荡中
	private GameObject _startRightSweep;		//开始扫荡
	private GameObject _processRightSweep;	//正在扫荡中
	private UILabel _sweepNumLbl; 	//选择扫荡次数
	private UILabel _sweepNeedTime; 	//扫荡需要时间
	private UILabel _sweepNeedEnergy; 	//扫荡需要体力
	private UILabel _countDownTime; 	//扫荡中的倒计时时间
	private UILabel _accelerateDiamond;	//加速扫荡需要金币
	private UILabel _resultInfo;		//扫荡得到的结果
	private GameObject _rowTemp; 	//扫荡结果的模板
	private GameObject _itemTemp;	//物品的模板
	private GameObject _bagFull; 	//背包已满
	private GameObject _sweepOver; 	//扫荡结束
 
	UILabel _sweepingLbl;			//显示正在扫荡中的标签
	UILabel _counterLbl;			//显示第几次扫荡的标签

	Transform trans;				//模板transform
	Transform _dragList;		//拖拽队列
	GameObject _shuoming;		//说明是否显示
	UIScrollView _sv;			//scrollview队列
	float nextPadding = 0f;     //各行的高度
	public float _stepTime = 1f;	//每次扫荡结果显示的间隔
	

	void Awake ()
	{
		_leftTitle = transform.Find ("left/title").GetComponent<UILabel> ();
		_rightTitle = transform.Find ("right/sweep_title").GetComponent<UILabel> ();
		_startLeftSweep = transform.Find ("left/shuoming").gameObject;
		_processLeftSweep = transform.Find ("left/result").gameObject;
		_startRightSweep = transform.Find ("right/sweep_start").gameObject;
		_processRightSweep = transform.Find ("right/sweeping").gameObject;
		_rowTemp = _processLeftSweep.transform.Find ("dragList/resultTemplate").gameObject;
		_itemTemp = _rowTemp.transform.Find ("item").gameObject;
		_bagFull = transform.Find ("left/overResult/notEnoughBag").gameObject;
		_sweepOver = transform.Find ("left/overResult/sweepOver").gameObject;
		_bagFull.SetActive (false); //默认不显示
		_sweepOver.SetActive (false);
		this.trans = _rowTemp.transform;  //模板trans
		this._dragList = _processLeftSweep.transform.Find ("dragList");
		this._sv = _processLeftSweep.GetComponent<UIScrollView> ();
		_shuoming = transform.Find ("right/sweeping/price_shuoming").gameObject;

	}
	
	void onDisable ()
	{
		if (SweepManager.Instance.IsSweeping)
			SweepManager.Instance.LastCloseTime = Time.realtimeSinceStartup;
		else
			SweepManager.Instance.ResultList.Clear (); //不是在扫荡就清除背包缓存
	}
	
	//初始化扫荡视图
	public void initView ()
	{
		if (SweepManager.Instance.IsSweeping) { //正在扫荡中
			initSweepProcess ();
			showProcessInfo ();
		} else {
			initSweepPrepare ();
			showStartInfo (1); //默认扫荡一次
		}
	}
	
	//更新扫荡的相关信息
	public void showStartInfo (int sweepTime)
	{
		if (SweepManager.Instance.IsSweeping) //扫荡中初始化扫荡中界面
			initSweepProcess ();
		SweepManager.Instance.sweepTotalNum = sweepTime;
		_sweepNumLbl.text = sweepTime.ToString ();
		int needTime = VipManager.Instance.SweepJiaSu * sweepTime;
		int needEnergy = SweepManager.Instance.CurrentMap.engeryConsume * sweepTime;
		_sweepNeedEnergy.text = needEnergy.ToString ();
		DateTime nextTime = DateTime.Now.AddSeconds (needTime);
		TimeSpan span = nextTime.Subtract (DateTime.Now);
		_sweepNeedTime.text = string.Format ("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds); 
	}
	
	//显示扫荡中的信息
	public void showProcessInfo ()
	{
		initSweepProcess ();
		FirstSweep ();
		showCountDownInfo ();
		InvokeRepeating ("onTimer", 1f, 1f);
	}

	public void FirstSweep ()
	{
		this.nextPadding = 0;
		this._processLeftSweep.GetComponent<UIScrollView> ().ResetPosition (); //置位scrollview
		this.ClearAllSweepInfo (); //清空扫荡
		this.AskSweep (SweepManager.Instance.CurrentSweepNum);
	}

	/// <summary>
	/// 请求扫荡
	/// </summary>
	public void AskSweep (int currentSweepNum)
	{
		_rowTemp.SetActive (true);
		GameObject go = NGUITools.AddChild (trans.parent.gameObject, _rowTemp);
		go.name = currentSweepNum.ToString ();
		Transform goTrans = go.transform;
		goTrans.localPosition = new Vector3 (trans.localPosition.x, trans.localPosition.y - nextPadding, trans.localPosition.z);
		goTrans.localScale = new Vector3 (trans.localScale.x, trans.localScale.y, trans.localScale.z);
		goTrans.Find ("exp").gameObject.SetActive (false);
		goTrans.Find ("gold").gameObject.SetActive (false);
		goTrans.Find ("item").gameObject.SetActive (false);
		_sweepingLbl = goTrans.Find ("processing").GetComponent<UILabel> ();
		_sweepingLbl.gameObject.SetActive (true);
		_sweepingLbl.text = LanguageManager.GetText ("sweep_processing_txt");
		_counterLbl = goTrans.Find ("sweepCounter").GetComponent<UILabel> ();
		string origLbl = _counterLbl.text;
		_counterLbl.text = origLbl.Replace (Constant.REPLACE_PARAMETER_1, currentSweepNum.ToString ());
		_rowTemp.SetActive (false);
	}

	/// <summary>
	/// 更新当前的扫荡信息
	/// </summary>
	/// <param name="res">Res.</param>
	void UpdateSweep (SweepResultVO res, int currentSweepNum)
	{
		Transform currentSweepTrans = this._dragList.FindChild (currentSweepNum.ToString ());
		currentSweepTrans.Find ("exp").gameObject.SetActive (true);
		currentSweepTrans.Find ("exp/EXP_num").GetComponent<UILabel> ().text = res.expNum.ToString ();
		currentSweepTrans.Find ("gold").gameObject.SetActive (true);
		currentSweepTrans.Find ("gold/gold_num").GetComponent<UILabel> ().text = res.goldNum.ToString ();
		int itemLen = res.itemHash.Count;
		if (itemLen > 0) {
			//设置物品
			GameObject itemObj = currentSweepTrans.Find ("item").gameObject;
			Transform childTrans = itemObj.transform;
			itemObj.SetActive (true);
			GameObject newItem = null;
			Transform newItemTrans = null;
			int index = 0;
			foreach (DictionaryEntry de in res.itemHash) {
				newItem = NGUITools.AddChild (currentSweepTrans.gameObject, itemObj);
				newItemTrans = newItem.transform;
				newItemTrans.localPosition = new Vector3 (childTrans.localPosition.x, childTrans.localPosition.y - index * SweepResultVO.SINGLE_ROW_HEIGHT, childTrans.localPosition.z);
				newItemTrans.localScale = new Vector3 (childTrans.localScale.x, childTrans.localScale.y, childTrans.localScale.z);
				newItemTrans.Find ("item0").GetComponent<UILabel> ().text = de.Key.ToString ();
				newItemTrans.Find ("num0").GetComponent<UILabel> ().text = " X" + de.Value.ToString ();
				if (index > 0)
					newItem.transform.Find ("wupin").gameObject.SetActive (false);
				//隐藏物品
				index++;
			}
			itemObj.SetActive (false);
		} else
			currentSweepTrans.Find ("item").gameObject.SetActive (false);
		//隐藏物品
		currentSweepTrans.Find ("processing").gameObject.SetActive (false);
		BoxCollider b = currentSweepTrans.GetComponent<BoxCollider> ();
		if (itemLen == 0) {
			b.center = new Vector3 (0, 70, 0);
		} else if (itemLen == 1) {
			b.center = new Vector3 (0, 55, 0);
		} else {
			b.center = new Vector3 (0, 40, 0);
		}
		currentSweepTrans.GetComponent<BoxCollider> ().size = new Vector3 (300, res.SelfHeight, 1);
		nextPadding += res.SelfHeight;
	}

	void MoveSweepPositoin (SweepResultVO res, int currentSweepNum)
	{
		if (currentSweepNum > 3) {
			Transform prevSweepTrans = this._dragList.FindChild ((currentSweepNum - 1).ToString ());
			Transform currentSweepTrans = this._dragList.FindChild (currentSweepNum.ToString ());
			if (prevSweepTrans != null) {
				Vector3 offset = prevSweepTrans.position - currentSweepTrans.position;
//				if (res.itemHash.Count == 1) {
//					offset.y -= 0.07f;
//				} else if (res.itemHash.Count == 2) {
//					offset.y -= 0.14f;
//				}
				 
				this._sv.MoveAbsolute (offset);

			}
		}
	}
 

	//显示扫荡的结果
	public void SweepResult (int currentSweepNum)
	{	
		#region 这是异常处理,因为有可能有最后一次的数据出现。
		if (currentSweepNum == 0) {
			return;
		}
		#endregion

		SweepResultVO resultVO = SweepManager.Instance.ResultList [currentSweepNum - 1]; //当前是第几次扫荡
		StartCoroutine (this.showSweepResult (resultVO, currentSweepNum));
	}
	/// 显示最终结果的扫荡
	public void SweepFinalResult (int currentSweepNum)
	{
		SweepResultVO resultVO = SweepManager.Instance.ResultList [currentSweepNum - 1]; //当前是第几次扫荡
		StartCoroutine (this.ShowSweepFinalResult (resultVO, currentSweepNum));
	}


	//显示扫荡的结果
	IEnumerator showSweepResult (SweepResultVO res, int currentSweepNum)
	{
		if (SweepManager.Instance.IsAccelerate) {
			yield return new WaitForSeconds (_stepTime * currentSweepNum);
		} else {
			yield return 1;
		}


		#region 更新当前次扫荡的数据
		UpdateSweep (res, currentSweepNum);
		#endregion



		#region 显示下次扫荡的数据
		AskSweep (currentSweepNum + 1);
		#endregion

		#region 移动位置
		MoveSweepPositoin (res, currentSweepNum);
		#endregion

	}

	/// <summary>
	/// 显示最终结果的扫荡
	/// </summary>
	/// <returns>The sweep final result.</returns>
	/// <param name="res">Res.</param>
	IEnumerator ShowSweepFinalResult (SweepResultVO res, int currentSweepNum)
	{
		this.FinalStopSweep ();
		if (SweepManager.Instance.IsAccelerate) {
			yield return new WaitForSeconds (_stepTime * currentSweepNum);
		} else {
			yield return 1;
		}

		this.UpdateSweep (res, currentSweepNum);
		#region 移动位置
		MoveSweepPositoin (res, currentSweepNum);
		#endregion
		_sweepOver.SetActive (true);
		SweepManager.Instance.IsAccelerate = false;
		SweepManager.Instance.IsShowResult = false;
 
	}

	//清除所有的扫荡信息
	public void ClearAllSweepInfo ()
	{
		foreach (Transform child in trans.parent) { //必须清除原来得
			if (child != trans)
				Destroy (child.gameObject);
		}
	}
 	
	public void FinalStopSweep ()
	{
		SweepManager.Instance.ClearSweepInfoNoException ();
		_bagFull.SetActive (false);
		_sweepOver.SetActive (false);
		initSweepPrepare ();
		_startLeftSweep.SetActive (false);
		_processLeftSweep.SetActive (true);
		showStartInfo (1); 
		_rightTitle.text = LanguageManager.GetText ("sweep_begin_title");
		CancelInvoke ("onTimer"); //停止计时器
	}

	//停止扫荡
	public void stopSweep (eStopSweep stopType)
	{
		SweepManager.Instance.clearSweepInfo ();
		_bagFull.SetActive (false);
		_sweepOver.SetActive (false);
		switch (stopType) {
		case eStopSweep.ePlayerStop:
			if (_sweepingLbl != null)
				_sweepingLbl.text = LanguageManager.GetText ("sweep_over_txt");
			break;
		case eStopSweep.eSweepOver:
			_sweepOver.SetActive (true);
			if (_sweepingLbl != null)
				_sweepingLbl.gameObject.SetActive (false);
//			if (_counterLbl != null)
//				_counterLbl.gameObject.SetActive (false);
			break;
		case eStopSweep.eLackEngery:
				
			break;
		case eStopSweep.eBagFull:
			_bagFull.SetActive (true);
			if (_sweepingLbl != null)
				_sweepingLbl.text = LanguageManager.GetText ("sweep_over_txt");
			break;
		default:
			break;
		}
		initSweepPrepare ();
		_startLeftSweep.SetActive (false);
		_processLeftSweep.SetActive (true);
		showStartInfo (1); 
		_rightTitle.text = LanguageManager.GetText ("sweep_begin_title");
		CancelInvoke ("onTimer"); //停止计时器
	}
	
	//加速扫荡 
	public void accelerateSweep ()
	{
		
	}
	
	//初始化扫荡中
	private void initSweepProcess ()
	{
		_startLeftSweep.SetActive (false);
		_startRightSweep.SetActive (false);
		_processLeftSweep.SetActive (true);
		_processRightSweep.SetActive (true);
		_bagFull.SetActive (false); 		//隐藏背包
		_sweepOver.SetActive (false);	//隐藏结束
		_leftTitle.text = LanguageManager.GetText ("sweep_processing_title");
		_rightTitle.text = LanguageManager.GetText ("sweep_process_title");
		_countDownTime = _processRightSweep.transform.Find ("time_bg/Label").GetComponent<UILabel> ();
		_accelerateDiamond = _processRightSweep.transform.Find ("price_shuoming/num").GetComponent<UILabel> ();


		if (VipManager.Instance.VipLevel > 0) {
			_processRightSweep.transform.Find ("accelerateBtn").GetComponent<BoxCollider>().enabled = false;
			_processRightSweep.transform.Find ("accelerateBtn/jiasuBtn").GetComponent<UISprite>().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(false);
			this._shuoming.SetActive(false);
		}
	 

	}

 

	//初始化扫荡初始界面
	private void initSweepPrepare ()
	{
		_startLeftSweep.SetActive (true);

        _startLeftSweep.transform.FindChild("sweep_shuoming/shuomin1").GetComponent<UILabel>().text = LanguageManager.GetText("sweep_shuoming_one");
        _startLeftSweep.transform.FindChild("sweep_shuoming/shuomin2").GetComponent<UILabel>().text = LanguageManager.GetText("sweep_shuoming_two");
        _startLeftSweep.transform.FindChild("sweep_shuoming/shuomin3").GetComponent<UILabel>().text = LanguageManager.GetText("sweep_shuoming_three");
        _startLeftSweep.transform.FindChild("sweep_shuoming/shuomin4").GetComponent<UILabel>().text = LanguageManager.GetText("sweep_shuoming_four");
        _startLeftSweep.transform.FindChild("VIP_shuoming/Label").GetComponent<UILabel>().text = LanguageManager.GetText("sweep_shuoming_vip");

		_startRightSweep.SetActive (true);
		_processLeftSweep.SetActive (false);
		_processRightSweep.SetActive (false);
		_leftTitle.text = LanguageManager.GetText ("sweep_description_title");
		_rightTitle.text = LanguageManager.GetText ("sweep_begin_title");
		_sweepNumLbl = _startRightSweep.transform.Find ("sweepNum/Label").GetComponent<UILabel> ();
		_sweepNeedTime = _startRightSweep.transform.Find ("sweep_time/time").GetComponent<UILabel> ();
		_sweepNeedEnergy = _startRightSweep.transform.Find ("sweep_energy/tili").GetComponent<UILabel> ();
	}
	
	//倒计时
	private void onTimer ()
	{
		TimeSpan countDownSpan = SweepManager.Instance.CountDownSpan;
		TimeSpan unitSpan = new TimeSpan (0, 0, 1);
		if (countDownSpan.TotalSeconds > 0) //没有倒计时到0
			SweepManager.Instance.CountDownSpan = countDownSpan.Subtract (unitSpan); //倒计时减去1s
		showCountDownInfo ();
	}
	
	//显示相关倒计时信息
	private void showCountDownInfo ()
	{
		TimeSpan countDownSpan = SweepManager.Instance.CountDownSpan;
		_countDownTime.text = string.Format ("{0:00}:{1:00}:{2:00}", countDownSpan.Hours, countDownSpan.Minutes, countDownSpan.Seconds); //具体时间格式化
		int addMinute = 1;
		if ((int)countDownSpan.TotalSeconds == 0)
			addMinute = 0;
		_accelerateDiamond.text = ((int)countDownSpan.TotalMinutes + addMinute).ToString (); //当前分钟速
	}
}
