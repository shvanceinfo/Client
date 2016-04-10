/**该文件实现的基本功能等
function: 竞技场战斗日志的视图控制
author:zyl
date:2014-3-15
**/
using UnityEngine;
using System;
using System.Collections;
using manager;
using model;
using System.Text;
using helper;

public class BattleLogView : MonoBehaviour
{

    private UIGrid _grid;
    private GameObject _preObj;
	void Awake ()
	{
        _grid = transform.Find("arenaMsg/grid").GetComponent<UIGrid>();
        _preObj = transform.Find("arenaMsg/item").gameObject;
        _preObj.SetActive(false);
	}
	
	void Start ()
	{
		this.InitView ();
	}
	
	void InitView ()
	{
		BetterList<ResultInfo> results = ArenaManager.Instance.ArenaVo.ResultList;

        ViewHelper.FormatTemplate<BetterList<ResultInfo>, ResultInfo>(_preObj,
            _grid.transform, results,
            (ResultInfo vo, Transform t) =>
            {
                UILabel lbl=t.FindChild("Label").GetComponent<UILabel>();
                if (!vo.beFight)    //挑战
                {
                    if (vo.fightResult)
                    {
                        lbl.text = ViewHelper.FormatLanguage("battle_log_we_win",
                            vo.ToString(),
                            vo.roleName,
                            vo.newRank
                            );
                    }
                    else {
                        lbl.text = ViewHelper.FormatLanguage("battle_log_we_lose",
                            vo.ToString(),
                            vo.roleName
                            );
                    }
                }
                else { //被挑战
                    if (vo.fightResult)
                    {
                        lbl.text = ViewHelper.FormatLanguage("battle_log_are_win",
                            vo.ToString(),
                            vo.roleName,
                            vo.newRank
                            );
                    }
                    else
                    {
                        lbl.text = ViewHelper.FormatLanguage("battle_log_are_lose",
                            vo.ToString(),
                            vo.roleName,
                            vo.newRank
                            );
                    }
                }
            });
	}
}
