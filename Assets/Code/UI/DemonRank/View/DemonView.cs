using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using helper;
public class DemonView : MonoBehaviour {



    private GameObject itemPrefab;
    private Transform _itemGrid;
    private GameObject _curRankViewObj; //今日排名obj
    private GameObject _curRankPanelObj;//
    private GameObject _curRankPrefab;
    private Transform _curRankGrid;
    private GameObject _curError;       //如果没有排名数据，显示暂无排名

    private GameObject _oldRankViewObj; //昨日排名obj
    private DemonHistoryAward _rankAward;   //昨日排名组件

    //自己的排名组件
    private UILabel _meRank;
    private UILabel _meGate;
    private UILabel _meAwardCount;
    private UITexture _meAwardIcon;
    private UISprite _meCareer;

    private UILabel _tick1Count;    //门票数量标签
    private UILabel _tick2Count;
    private UILabel _tick3Count;

    private DemonDisplayEnter _enter1;
    private DemonDisplayEnter _enter2;
    private DemonDisplayEnter _enter3;
    private DemonDisplayEnter _enter4;
    private void Awake()
    {
        itemPrefab = transform.FindChild("RankReward/Panel/Item").gameObject;
        _itemGrid = transform.FindChild("RankReward/Panel/Grid");
        _curRankViewObj = transform.FindChild("CurRank").gameObject;
        _curRankPanelObj = transform.FindChild("RankPanel").gameObject;
        _curError = transform.FindChild("CurRank/Error").gameObject;
        _oldRankViewObj = transform.FindChild("Rank").gameObject;
        _oldRankViewObj.SetActive(true);
        _rankAward = _oldRankViewObj.GetComponent<DemonHistoryAward>();
   
        _curRankPrefab = transform.FindChild("RankPanel/Item").gameObject;
        _curRankGrid = transform.FindChild("RankPanel/Grid");

        _curRankViewObj.SetActive(true);
        _meRank = transform.FindChild("CurRank/Rank_Me/level").GetComponent<UILabel>();
        _meGate = transform.FindChild("CurRank/Rank_Me/rank").GetComponent<UILabel>();
        _meAwardCount = transform.FindChild("CurRank/Rank_Me/num").GetComponent<UILabel>();
        _meAwardIcon = transform.FindChild("CurRank/Rank_Me/item").GetComponent<UITexture>();
        _meCareer = transform.FindChild("CurRank/Rank_Me/career").GetComponent<UISprite>();
        _curRankViewObj.SetActive(false);


        _tick1Count = transform.FindChild("Panel/Func/lbls/lbl1").GetComponent<UILabel>();
        _tick2Count = transform.FindChild("Panel/Func/lbls/lbl2").GetComponent<UILabel>();
        _tick3Count = transform.FindChild("Panel/Func/lbls/lbl3").GetComponent<UILabel>();

        transform.FindChild("Panel/Background/LabelText").GetComponent<UILabel>().text = LanguageManager.GetText("demon_background_info");

        _enter1 = transform.FindChild("GoList/Panel/Grild/Index1").GetComponent<DemonDisplayEnter>();
        _enter2 = transform.FindChild("GoList/Panel/Grild/Index2").GetComponent<DemonDisplayEnter>();
        _enter3 = transform.FindChild("GoList/Panel/Grild/Index3").GetComponent<DemonDisplayEnter>();
        _enter4 = transform.FindChild("GoList/Panel/Grild/Index4").GetComponent<DemonDisplayEnter>();
    }

    private void Start()
    {
        Gate.instance.sendNotification(MsgConstant.MSG_DEMON_INITIAL_DATA);//初始化数据
        DisplayLevelItems(DemonDiffEnum.Level1);    //初始化显示噩梦难度奖励
        DisplayTickCount();                         //显示门票
        _curRankViewObj.SetActive(false);           //初始化隐藏二级窗口
        _oldRankViewObj.SetActive(false);
        _curRankPanelObj.SetActive(false);
    }
    private void OnEnable()
    {
        Gate.instance.registerMediator(new DemonMediator(this));
    }

    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.DEMON_MEDIATOR);
    }

    //显示今日排名
    public void DisplayCurRankView(bool isShow)
    {
        _curRankViewObj.SetActive(isShow);
        _curRankPanelObj.SetActive(isShow);
        if (isShow)
        {
            DisplayRankItems(DemonDiffEnum.Level1);
        }
    }

    //显示昨日排名
    public void DisplayRankView(bool isShow)
    {
        _oldRankViewObj.SetActive(isShow);

        if (isShow)
        {
            Hashtable hash = DemonManager.Instance.InfoVo.HistoryRank;
            HistoryRankVo vo1= hash[DemonDiffEnum.Level1] as HistoryRankVo;
            HistoryRankVo vo2 = hash[DemonDiffEnum.Level1] as HistoryRankVo;
            HistoryRankVo vo3 = hash[DemonDiffEnum.Level1] as HistoryRankVo;

            DisplayHistoryAward(DemonDiffEnum.Level1, vo1);
            DisplayHistoryAward(DemonDiffEnum.Level2, vo2);
            DisplayHistoryAward(DemonDiffEnum.Level3, vo3);
        }
    }
    private void DisplayHistoryAward(DemonDiffEnum lvl,HistoryRankVo vo)
    {
        if (vo.IsAward)
        {
            _rankAward.DisPlayLvl(lvl, vo);
        }
        else {
            _rankAward.DisPlayNone(lvl);
        }
    }


    /// <summary>
    /// 显示门票数量
    /// </summary>
    private void DisplayTickCount()
    {
        _tick1Count.text = string.Format("x{0}", (int)DemonManager.Instance.InfoVo.TickCount[DemonDiffEnum.Level1]);
        _tick2Count.text = string.Format("x{0}", (int)DemonManager.Instance.InfoVo.TickCount[DemonDiffEnum.Level2]);
        _tick3Count.text = string.Format("x{0}", (int)DemonManager.Instance.InfoVo.TickCount[DemonDiffEnum.Level3]);
    }

    /// <summary>
    /// 显示当前每个门票的数量
    /// </summary>
    /// <param name="level"></param>
    private void DisplayTickConsumeCount(DemonDiffEnum level)
    {
        BetterList<DemonVoItem> cts = DemonManager.Instance.InfoVo.ConsumeTickCount[level] as BetterList<DemonVoItem>;
        if (cts[0].Nums == 0)
        {
            _enter1.Display(StrengThenManager.GetItemById(cts[0].Id).icon, 1, cts[0].Nums);
            _enter1.Hiddent();        }
        else
        {
			int lvl=(int)DemonManager.Instance.InfoVo.CurGate[level] + 1;
			if(lvl>100)
				lvl=1;
            _enter1.Display(StrengThenManager.GetItemById(cts[0].Id).icon, lvl, cts[0].Nums);
        }
        _enter2.Display(StrengThenManager.GetItemById(cts[1].Id).icon, cts[1].Nums);
        _enter3.Display(StrengThenManager.GetItemById(cts[2].Id).icon, cts[2].Nums);
        _enter4.Display(StrengThenManager.GetItemById(cts[3].Id).icon, cts[3].Nums);
    }


    //显示领取列表
    public void DisplayLevelItems(DemonDiffEnum level)
    {
        int count= _itemGrid.childCount;

        Hashtable hash= DemonManager.Instance.GetLevelHash(level);

        if (count>hash.Count)
        {
            for (int i = hash.Count; i < count; i++)
            {
                DeletePrefab(i);
            }
        }
        else if (count < hash.Count)
        {
            for (int i = count; i < hash.Count; i++)
            {
                AddItemPrefab(i);
            }
        }

        //排序ID
        List<int> idSort = new List<int>();
        foreach (int key in hash.Keys)
        {
            idSort.Add(key);
        }
        idSort.Sort();

        for (int i = 0; i < idSort.Count; i++)
        {
            DemonVo dv = hash[idSort[i]] as DemonVo;
            DisplayItem(dv, i);
        }
        DisplayTickConsumeCount(level);
    }

    private void DisplayItem(DemonVo dv,int index)
    {
        Transform t = _itemGrid.FindChild(index.ToString());
        if (t != null)
        {
            DemonItem di=t.GetComponent<DemonItem>();
            string result="";
            for (int i = 0; i < dv.RankRewards.size; i++)
			{
                ItemTemplate gt=ConfigDataManager.GetInstance().getItemTemplate().getTemplateData(dv.RankRewards[0].Id);
			    result+=string.Format("{0}x{1}",gt.name,dv.RankRewards[i].Nums);
			}
            di.Id = dv.Id;
            di.Display(DemonManager.GetLevelString((DemonDiffEnum)dv.Diff),
                string.Format("第{0}层", dv.Level), result, dv.IsReceive,dv.IsComplate);
        }
        else {
            Debug.LogError("t is null !");
        }
    }

    private void AddItemPrefab(int id)
    {
        itemPrefab.SetActive(true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(itemPrefab);
        obj.transform.parent = _itemGrid;
        obj.name = id.ToString();
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);
        itemPrefab.SetActive(false);
    }

    private void DeletePrefab(int id)
    {
        Transform t= _itemGrid.FindChild(id.ToString());
        if (t==null)
        {
            Debug.LogError("Index error");
        }else{
            Destroy(t.gameObject);
        }
    }


    //显示对应的数据列表
    public void DisplayRankItems(DemonDiffEnum level)
    {
        int count = _curRankGrid.childCount;
        BetterList<RankVo> rvs = DemonManager.Instance.GetRankLevel(level);

        
        if (count > rvs.size)
        {
            for (int i = rvs.size; i < count; i++)
            {
                DeleteItemTemplate(_curRankGrid, i);
            }
        }
        else if (count < rvs.size)
        {
            for (int i = count; i < rvs.size; i++)
            {
                AddItemTemplatePrefab(_curRankPrefab, _curRankGrid, i);
            }
        }

        for (int i = 0; i < rvs.size; i++)
        {
            DisplayRank(rvs[i], i);
        }
        if (rvs.size == 0)    //如果没有排名
        {
            _curError.SetActive(true);  //显示暂无排名
        }
        else {
            _curError.SetActive(false); 
        }
        DisplayMyRank(level);

        _curRankPanelObj.GetComponent<UIScrollView>().ResetPosition();
        _curRankGrid.GetComponent<UIGrid>().Reposition();
    }

    //显示自己的排名
    private void DisplayMyRank(DemonDiffEnum level)
    {
        int rank = (int)DemonManager.Instance.InfoVo.CurRank[level];

        if (rank == 0)
        {
            _meRank.text = ColorConst.Format(ColorConst.Color_HeSe,"暂无排名");
            _meGate.text = ColorConst.Format(ColorConst.Color_HeSe,"1层");
            _meAwardCount.text = ColorConst.Format(ColorConst.Color_HeSe,"x1");
            string icon=DemonManager.Instance.FindIcon(DemonManager.Instance.FindAward(10000).Id);
            _meAwardIcon.mainTexture = SourceManager.Instance.getTextByIconName(icon);
        }
        else {
            
            int gate = (int)DemonManager.Instance.InfoVo.CurGate[level];
            DemonVoItem dv = DemonManager.Instance.FindAward(rank);
            string icon = DemonManager.Instance.FindIcon(dv.Id);

            _meRank.text = ColorConst.Format(ColorConst.Color_HeSe, rank);
            _meGate.text = ColorConst.Format(ColorConst.Color_HeSe, gate+"层");
            _meAwardCount.text = ColorConst.Format(ColorConst.Color_HeSe, "x"+dv.Nums) ;
            _meAwardIcon.mainTexture = SourceManager.Instance.getTextByIconName(icon);
        }
        _meCareer.spriteName = ViewHelper.GetCarrerSpriteByEnum(CharacterPlayer.character_property.getCareer());
    }

    private void DisplayRank(RankVo rv, int index)
    {
        Transform t = _curRankGrid.FindChild(index.ToString());
        if (t != null)
        {
            DemonRankItem dr = t.GetComponent<DemonRankItem>();

            dr.Display(rv.Id, rv.Id,
                ViewHelper.GetCarrerSpriteByEnum(rv.Career),
                rv.Name, rv.RankTower.ToString(), rv.ItemIcon, rv.ItemCount);
            
        }
        else
        {
            Debug.LogError("t is null !");
        }
    }

    private void AddItemTemplatePrefab(GameObject prefab,Transform grid,int id)
    {
        prefab.SetActive(true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(prefab);
        obj.transform.parent = grid;
        obj.name = id.ToString();
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);
        prefab.SetActive(false);
    }

    private void DeleteItemTemplate(Transform grid, int id)
    {
        Transform t = grid.FindChild(id.ToString());
        if (t == null)
        {
            Debug.LogError("Index error");
        }
        else
        {
            Destroy(t.gameObject);
        }
    }
}
