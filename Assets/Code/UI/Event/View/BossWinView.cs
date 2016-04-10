using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using mediator;
using MVC.entrance.gate;
using model;
using System.Text;

public class BossWinView : MonoBehaviour
{
	const int CDTIME = 10;
	const int SUBTIME = 1;
	const int HEIGHT = -60;
	Transform _trans;
	Transform _me;
	Transform _last;
	Transform _temp;
	Transform _playerlist;
	void Awake ()
	{
		this._trans = this.transform;
		this._me = this._trans.FindChild ("info/me");
		this._last = this._trans.FindChild ("info/last");
		this._temp = this._trans.FindChild ("info/temp");
		this._playerlist = this._trans.FindChild("info/playerlist");
	}
 
	void OnEnable ()
	{
		Gate.instance.registerMediator (new BossWinMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.BOSS_WIN_MEDIATOR);
	}
	
 	void Start(){
		StartCoroutine(this.ReturnCity());
	}
 
	
	
	public void Show (BossWinVo bossWinVo)
	{
		this._me.FindChild ("info/name").GetComponent<UILabel> ().text = LanguageManager.GetText ("msg_world_boss_me_award_name");
		this._me.FindChild ("info/dps").GetComponent<UILabel> ().text = 
				string.Format (LanguageManager.GetText ("msg_world_boss_me_award"), BossManager.Instance.GetLerpValueString (BossManager.Instance.GetLerpValue (0, BossManager.Instance.WorldBossData.hp
																											, (int)bossWinVo.Me.dps), bossWinVo.Me.dps.ToString ()));
		RepeatResource (this._me, bossWinVo.Me.awardList, "msg_world_boss_me_award");
		
		this._temp.gameObject.SetActive (true);
		GameObject obj = null;
		for (int i = 0; i <  bossWinVo.PlayList.Count; i++) {
			obj = NGUITools.AddChild (this._playerlist.gameObject, this._temp.gameObject);
			int ladderNum = i + 1;
			obj.name = ladderNum.ToString();
			Transform objTrans = obj.transform;
			objTrans.localPosition = new Vector3 (this._temp.localPosition.x, this._temp.localPosition.y + i * HEIGHT, 0);
			
			if (ladderNum <= 3) {
				objTrans.FindChild ("info/name").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_num_award" + ladderNum), bossWinVo.PlayList [i].playerName);
				objTrans.FindChild ("info/dps").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_num_award" + ladderNum), BossManager.Instance.GetLerpValueString (BossManager.Instance.GetLerpValue (0, BossManager.Instance.WorldBossData.hp
																											, (int)bossWinVo.PlayList [i].dps), bossWinVo.PlayList [i].dps.ToString ()));
				
	 			
				objTrans.FindChild ("info/ladder").GetComponent<UILabel> ().text =  LanguageManager.GetText ("msg_world_boss_ladder" + ladderNum);
				UISprite iconUI = objTrans.FindChild ("bg/icon").GetComponent<UISprite> ();
				iconUI.spriteName = SourceManager.Instance.GetLadderIconByNum (i + 1);
				iconUI.MakePixelPerfect ();
				iconUI.gameObject.SetActive (true);
				RepeatResource (objTrans, bossWinVo.PlayList [i].awardList, "msg_world_boss_num_award" + ladderNum);
			} else {
				objTrans.FindChild ("info/name").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_num_award"), bossWinVo.PlayList [i].playerName);
				objTrans.FindChild ("info/dps").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_num_award"), BossManager.Instance.GetLerpValueString (BossManager.Instance.GetLerpValue (0, BossManager.Instance.WorldBossData.hp
																											, (int)bossWinVo.PlayList [i].dps), bossWinVo.PlayList [i].dps.ToString ()));
				
	 
				objTrans.FindChild ("info/ladder").GetComponent<UILabel> ().text = LanguageManager.GetText ("msg_world_boss_ladder" + ladderNum);
				RepeatResource (objTrans, bossWinVo.PlayList [i].awardList, "msg_world_boss_num_award");
			}
 
		}
		this._temp.gameObject.SetActive (false); 
		
	 	
		if (!string.IsNullOrEmpty(bossWinVo.Last.playerName)) {
			this._last.FindChild ("info/name").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_num_award"),bossWinVo.Last.playerName);
			this._last.FindChild ("info/dps").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_num_award"),BossManager.Instance.GetLerpValueString (BossManager.Instance.GetLerpValue (0, BossManager.Instance.WorldBossData.hp
 																															, (int)bossWinVo.Last.dps), bossWinVo.Last.dps.ToString ())); 
		
			
		}else{
			this._last.FindChild ("info/name").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_num_award"),"-");
			this._last.FindChild ("info/dps").GetComponent<UILabel> ().text =  string.Format (LanguageManager.GetText ("msg_world_boss_num_award"),"-");
		}
		RepeatResource (this._last, bossWinVo.Last.awardList, "msg_world_boss_num_award");
		 
	}
	
	public static void RepeatResource (Transform _trans, List<Award> awardList, string zhcnMsg)
	{
		for (int i = 0; i < awardList.Count; i++) {
			
			StringBuilder assetNum = new StringBuilder ();
			assetNum.Append ("info/resource").Append (i + 1);
			_trans.FindChild (assetNum.ToString ()).gameObject.SetActive (true);
			assetNum.Append ("/Label");
			if (awardList [i].num == 0) {
				_trans.FindChild (assetNum.ToString ()).GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_me_award"), "-");
			
			}else{
				_trans.FindChild (assetNum.ToString ()).GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_me_award"), awardList [i].num);
			}
			
			StringBuilder assetSprite = new StringBuilder ();
			assetSprite.Append ("info/resource").Append (i + 1).Append ("/Sprite");
			_trans.FindChild (assetSprite.ToString ()).GetComponent<UISprite> ().spriteName = SourceManager.Instance.getIconByType ((eGoldType)awardList [i].type);
		}
	}
	
	IEnumerator ReturnCity(){
		
		WaitForSeconds wf = new WaitForSeconds(SUBTIME);
		
		for (int i = 0; i < CDTIME; i++) {
			yield return wf;
		}
		
		BossManager.Instance.WinBackCity();
	}
	
}
