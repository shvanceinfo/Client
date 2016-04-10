/**该文件实现的基本功能等
function: 实现竞技场的倒计时功能
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections;

public class ArenaCountDown : MonoBehaviour
{
	const int countDownNum = 5; //倒计时5秒
	private GameObject[] _countDownObjs; //倒计时的Sprite
	private int currentCount = 0; //当前的倒计时
	
	void Awake () 
	{
		init();
	}
	
	//开始倒计时
	public void beginCountDown() 
	{
		if(currentCount < countDownNum)
		{
			init();
			activeSp();
			GameObject currentObj = _countDownObjs[currentCount];
			TweenAlpha alpha = UITweener.Begin<TweenAlpha>(currentObj, 0.8f);
	        alpha.duration = 0.8f;
	        alpha.from = 1f;
	        alpha.to = 0f;
			alpha.enabled = true;
	
	        TweenPosition tweenPos = UITweener.Begin<TweenPosition>(currentObj, 0f);
	        tweenPos.duration = 1f;
	        tweenPos.from = new Vector3(0f, 0f, 0f);
	        tweenPos.to = new Vector3(0f, 200f, 0f);
	        tweenPos.callWhenFinished = "countDownNext";
	        tweenPos.eventReceiver = gameObject;
			tweenPos.enabled = true;
	        
	        TweenScale scale1 = UITweener.Begin<TweenScale>(currentObj, 0f);
	        scale1.duration = 0.8f;
	        scale1.from = new Vector3(0.8f, 0.8f, 1f);
	        scale1.to = new Vector3(1f, 1f, 1f);
			scale1.enabled = true;
	        
	        TweenScale scale2 = UITweener.Begin<TweenScale>(currentObj, 0.8f);
	        scale2.duration = 0.8f;
	        scale2.from = new Vector3(1f, 1f, 1f);
	        scale2.to = new Vector3(1.2f, 1.2f, 1f);
			scale2.enabled = true;
			currentCount++;
		}
	}
	
	private void init()
	{
		if(_countDownObjs == null)
		{
			_countDownObjs = new GameObject[countDownNum];
			for(int i=1; i<=countDownNum; i++)
			{
				_countDownObjs[countDownNum-i] = transform.Find("num"+i).gameObject;
				_countDownObjs[countDownNum-i].transform.localScale = Vector3.one;
			}
			currentCount = 0;
		}
	}
	
	//使能相关的控件
	private void activeSp()
	{
		for(int i=0; i<countDownNum; i++)
		{
			if(i == currentCount)
				_countDownObjs[i].SetActive(true);
			else
				_countDownObjs[i].SetActive(false);
		}
	}
	
	//开始战斗
	private void countDownNext()
	{
		if(currentCount < countDownNum)
			beginCountDown();
		else //倒计时结束，开始战斗
		{
			BattleArena.GetInstance().StartBattleArena();
			Destroy(gameObject, 1f);
		}
	}
}
