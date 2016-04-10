/**该文件实现的基本功能等
function: 竞技场英雄榜的视图控制
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections;
using manager;
using model;

public class HeroBoardView : MonoBehaviour
{
	const int PER_ROW_HEIGHT = 60;
	const string UP_TREND = "arena_jiantou1";
	const string DOWN_TREND = "arena_jiantou2";
	const string SWORD_SP = "arena_zhanshi";
	const string ARCHER_SP = "arena_gongjianshou";
	const string MAGICIAN_SP = "arena_mofashi";
    const string LEVEL_1_COLOR = "fffc00";
    const string LEVEL_2_COLOR = "ff9100";
    const string LEVEL_3_COLOR = "f05501";
	private GameObject _heroRowTemplate; //英雄信息的单个模板
	private BoxCollider _boxCollider; //按钮的碰撞
	void Awake () 
	{
		_heroRowTemplate = transform.Find("heroRank/dragObj/rowTemplate").gameObject;
		_boxCollider = transform.Find("heroRank/dragObj").GetComponent<BoxCollider>();
		initHeroList();
	}
	
	//设置英雄榜信息
	void initHeroList()
	{
		BetterList<HeroListInfo> lists = ArenaManager.Instance.ArenaVo.HeroList;
		_heroRowTemplate.SetActive(true);
		if(lists.size > 0)
		{
			Transform trans = _heroRowTemplate.transform;
			for(int i=0; i<lists.size; i++)
			{
                string color="ffffff";
                if (i == 0)
                    color = LEVEL_1_COLOR;
                else if (i == 1)
                    color = LEVEL_2_COLOR;
                else if (i == 2)
                    color = LEVEL_3_COLOR;

				HeroListInfo info = lists[i];
				GameObject go = NGUITools.AddChild(trans.parent.gameObject, _heroRowTemplate);
				go.transform.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y - i*PER_ROW_HEIGHT, trans.localPosition.z);
				go.transform.localScale = new Vector3(trans.localScale.x, trans.localScale.y, trans.localScale.z);
				Transform tt = go.transform;
				tt.Find("battlePower").GetComponent<UILabel>().text = AddColor(color, info.powerStrength.ToString());
				tt.Find("level").GetComponent<UILabel>().text = AddColor(color,info.level.ToString());
				tt.Find("name").GetComponent<UILabel>().text = AddColor(color,info.challengerName.ToString());
				tt.Find("order").GetComponent<UILabel>().text = AddColor(color,(i+1).ToString());
				Transform noTrendTrans = tt.Find("noTrend");
				Transform trendTrans = tt.Find("trend");
				if(info.trend == 0)
				{
					noTrendTrans.gameObject.SetActive(true);
					trendTrans.gameObject.SetActive(false);
				}
				else
				{
					noTrendTrans.gameObject.SetActive(false);
					trendTrans.gameObject.SetActive(true);
					if(info.trend > 0)
						trendTrans.GetComponent<UISprite>().spriteName = UP_TREND;
					else
						trendTrans.GetComponent<UISprite>().spriteName = DOWN_TREND;
				}
				UISprite vocationSp = tt.Find("vocation").GetComponent<UISprite>();
				switch (info.vocation) 
				{
					case CHARACTER_CAREER.CC_SWORD:
						vocationSp.spriteName = SWORD_SP;
						break;
					case CHARACTER_CAREER.CC_ARCHER:
						vocationSp.spriteName = ARCHER_SP;
						break;
					case CHARACTER_CAREER.CC_MAGICIAN:
						vocationSp.spriteName = MAGICIAN_SP;
						break;
					default:
						break;
				}
				go.name = "row" + i;
			}
		}
		if (lists.size<=5) {
			this._boxCollider.enabled = false;
		}else{
			this._boxCollider.enabled =true;
		}
		
		_heroRowTemplate.SetActive(false); //隐藏模板按钮
	}

    string AddColor(string color,string str)
    {
        return string.Format("[{0}]{1}[-]", color, str);
    }
}
