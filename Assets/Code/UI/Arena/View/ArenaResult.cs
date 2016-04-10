/**该文件实现的基本功能等
function: 实现竞技场结束后的奖励面板实现
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections;
using helper;
using manager;
using MVC.entrance.gate;
using System.Collections.Generic;

public class ArenaResult : MonoBehaviour
{
    private UILabel _rankLbl;
    private UISprite _rankIcon;
	private UILabel _awardGold;		//奖励的金币
	private UILabel _awardHonor;	//奖励的荣誉
    private UILabel _awardGoldLbl;  
    private UILabel _awardHonorLbl;
    private GameObject _winPre;     //胜利特效
    private GameObject _losePre;    //失败特效
    private GameObject function;
    private UILabel _time;

	void Awake () 
	{

        function = transform.FindChild("Function").gameObject;
        function.SetActive(true);
        _rankLbl = transform.FindChild("Function/awardInfo/winInfo/newRank").GetComponent<UILabel>();
        _rankIcon = transform.FindChild("Function/awardInfo/winInfo/icon").GetComponent<UISprite>();
        _time = transform.FindChild("Function/awardInfo/WaitTime/time").GetComponent<UILabel>();
        _awardGold = transform.Find("Function/awardInfo/awardGold/goldNum").GetComponent<UILabel>();
        _awardHonor = transform.Find("Function/awardInfo/awardHonor/honorNum").GetComponent<UILabel>();
        _awardGoldLbl = transform.Find("Function/awardInfo/awardGold/goldPrefix").GetComponent<UILabel>();
        _awardHonorLbl = transform.Find("Function/awardInfo/awardHonor/honorPrefix").GetComponent<UILabel>();

        function.SetActive(false);
        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, Constant.Arena_Effect_Win,
                (asset) =>
                {
                    _winPre = (GameObject)asset;
                });
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, Constant.Arena_Effect_Win,
                (asset) =>
                {
                    _losePre = (GameObject)asset;
                });
        }
        else
        {
            _winPre = BundleMemManager.Instance.getPrefabByName(Constant.Arena_Effect_Win, EBundleType.eBundleUIEffect);
            _losePre = BundleMemManager.Instance.getPrefabByName(Constant.Arena_Effect_Lose, EBundleType.eBundleUIEffect);
        }               
	}
	
	//显示胜利面板
    public void showResult(bool success, uint newRank = 0)
    {
        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_STOP_TIME);  
        //显示奖励面板，隐藏计时器
        StartCoroutine(LaterDisplay(success, newRank));     
    }

    private IEnumerator LaterDisplay(bool success, uint newRank )
    {
        yield return new WaitForSeconds(3);
        DisplayEffect(success, newRank);
        yield return new WaitForSeconds(Constant.Arena_Effect_time);
        DisplayUI(success,newRank);
    }

    private void DisplayEffect(bool success, uint newRank )
    {
        if (success)
        {
            CameraManager.Instance.ShowEffect(Constant.Arena_Effect_Win, 13);
        }
        else {
            CameraManager.Instance.ShowEffect(Constant.Arena_Effect_Lose, 13);
        }
    }
    private void DisplayUI(bool success, uint newRank )
    {
        function.SetActive(true);

        if (success) //如果胜利
        {

            _rankIcon.gameObject.SetActive(true);
            _rankLbl.text = ColorConst.Format(ColorConst.Color_Green, newRank.ToString());

            List<int> list = ViewHelper.FindPublicById(1001001).type1List;

            _awardGoldLbl.text = ViewHelper.GetResoucesString((eGoldType)list[0]);
            _awardGold.text = ColorConst.Format(ColorConst.Color_Green, list[1]);

            _awardHonorLbl.text = ViewHelper.GetResoucesString((eGoldType)list[2]);
            _awardHonor.text = ColorConst.Format(ColorConst.Color_Green, list[3]); 
        }
        else
        {
            _rankIcon.gameObject.SetActive(false);
            _rankLbl.text = ColorConst.Format(ColorConst.Color_Red, newRank.ToString());

            List<int> list = ViewHelper.FindPublicById(1001002).type1List;

            _awardGoldLbl.text = ViewHelper.GetResoucesString((eGoldType)list[0]);
            _awardGold.text = ColorConst.Format(ColorConst.Color_Red, list[1]);

            _awardHonorLbl.text = ViewHelper.GetResoucesString((eGoldType)list[2]);
            _awardHonor.text = ColorConst.Format(ColorConst.Color_Red, list[3]); 
        }
        int time=10;
        _time.text = time.ToString();
        StartCoroutine(StartTime(time));
    }
    private IEnumerator StartTime(int time)
    {
        while (true)
        {
            time--;
            if (time<0)
            {
                ArenaManager.Instance.sendArenaResult();
                break;
            }
            yield return new WaitForSeconds(1);
            _time.text = time.ToString();
        }
        

    }

}


