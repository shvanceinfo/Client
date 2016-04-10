using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using helper;
using manager;
using model;
public class StrengThenView : MonoBehaviour
{
    static int MAX_STRENGTHEN = 10;  //最大强化值
    const int MAX_ATTRIBUTE_COUNT = 2;//最大2组属性值
    const int MAX_CONSUME_ITEM = 5;     //最大奖励个数
    const string NameFormat = "{0} +{1}";
    const string NameFormatNone = "{0}";
    const string UP = "↑";
    const string SUCFORMAT = "{0}%";
    const string SUCADDFORMAT = "(+[{0}]{1}[-]%)";
	const float SUCLES=10;
    const string MAX_LEVEL = " 已强化至满级";
    const string EVALUEFORMATE = "([{0}]{1}{2}[-])";
    const string MAX_LBL = "([{0}]↑{1} 满级[-])";

    #region 列表
    private UIScrollView _view;
    private UIGrid _grid;
    private GameObject _prefab;
    private GameObject _descObj;
    private GameObject _button;
    #endregion

    #region 详细信息
    //private UISprite[] starts;      //星星集合
    private UILabel[] _As;          //基础属性，描述
    private UILabel[] _Vs;          //基础属性，值
    private UILabel[] _Es;          //基础属性，对比
    private ItemLabel[] _consumes;  //消耗列表
    private UILabel _suc;           //成功率
    private UILabel _sucAdd;        //增加成功率
    private UILabel _name;          //名称
    private GameObject _luckBtn;    //增加成功率按钮
    private GameObject _maxLbl;     //满级按钮
    private GameObject prefEffect;  //特效prefab
    private GameObject prefEffectCircle;    //特效方圈
    private const string UI_TRAIL = "Effect/Effect_Prefab/UI/UI_Trail";
    #endregion


    private void Awake()
    {
        
        _descObj = transform.FindChild("Description").gameObject;
        _view = transform.FindChild("ItemPanel").GetComponent<UIScrollView>();
        _grid = transform.FindChild("ItemPanel/Grid").GetComponent<UIGrid>();
        _prefab = transform.FindChild("ItemPanel/Item").gameObject;
        _luckBtn = _descObj.transform.FindChild("Buttom/Button_LuckStone").gameObject;
        _button = _descObj.transform.FindChild("Buttom/Button_Ok").gameObject;
        _maxLbl = _descObj.transform.FindChild("Top/maxlbl").gameObject;
        prefEffect = transform.parent.FindChild("UI_Trail").gameObject;
        prefEffectCircle = transform.FindChild("Background/SuccessEffect").gameObject;
        //starts = new UISprite[MAX_STRENGTHEN];
        //for (int i = 0; i < MAX_STRENGTHEN; i++)
        //{
        //    starts[i] = transform.FindChild("Description/Top/Starts/start" + (i + 1)).GetComponent<UISprite>();
        //}

        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, UI_TRAIL,
                (asset) =>
                {
                    SetStart((GameObject)asset);
                });
        }
        else
        {
            GameObject asset = BundleMemManager.Instance.getPrefabByName(UI_TRAIL, EBundleType.eBundleUIEffect);
            SetStart(asset);
        }

        _As = new UILabel[MAX_ATTRIBUTE_COUNT];
        for (int i = 0; i < MAX_ATTRIBUTE_COUNT; i++)
        {
            _As[i] = transform.FindChild("Description/Top/Descs/A" + (i + 1)).GetComponent<UILabel>();
        }
        _Vs = new UILabel[MAX_ATTRIBUTE_COUNT];
        for (int i = 0; i < MAX_ATTRIBUTE_COUNT; i++)
        {
            _Vs[i] = transform.FindChild("Description/Top/Descs/V" + (i + 1)).GetComponent<UILabel>();
        }
        _Es = new UILabel[MAX_ATTRIBUTE_COUNT];
        for (int i = 0; i < MAX_ATTRIBUTE_COUNT; i++)
        {
            _Es[i] = transform.FindChild("Description/Top/Descs/E" + (i + 1)).GetComponent<UILabel>();
        }

        _consumes = new ItemLabel[MAX_CONSUME_ITEM];
        for (int i = 0; i < MAX_CONSUME_ITEM; i++)
        {
            _consumes[i] = transform.FindChild("Description/Center/Item" + (i + 1)).GetComponent<ItemLabel>();
        }

        _suc = transform.FindChild("Description/Buttom/Suc").GetComponent<UILabel>();
        _sucAdd = transform.FindChild("Description/Buttom/SucAdd").GetComponent<UILabel>();
        _name = transform.FindChild("Description/Top/Name").GetComponent<UILabel>();
    }

    #region Same

    private void SetStart(GameObject obj)
    {
        GameObject objNew = BundleMemManager.Instance.instantiateObj(obj);
        objNew.transform.parent = transform.parent.FindChild("UI_Trail");
        objNew.transform.localPosition = Vector3.zero;
        objNew.transform.localScale = Vector3.one;
    }

    private void OnEnable()
    {
        Gate.instance.registerMediator(new StrengThenMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.STRENGTHEN_MEDIATOR);
    }


    /// <summary>
    /// 切换标签
    /// </summary>
    /// <param name="table"></param>
    public void DisplayList(Table table)
    {
        if (table != StrengThenManager.Instance.SelectTable)
        {
            switch (table)
            {
                case Table.Table1:
                    DisplayEquipList();
                    break;
                case Table.Table2:
                    DisplayBagList();
                    break;
            }
            StrengThenManager.Instance.SelectTable = table;
            _view.ResetPosition();
            _grid.Reposition();
            SelectIndexItem();
        }
        
    }

    public void EnforceDisplayList(Table table)
    {
        switch (table)
        {
            case Table.Table1:
                DisplayEquipList();
                break;
            case Table.Table2:
                DisplayBagList();
                break;
        }
        StrengThenManager.Instance.SelectTable = table;
        //_view.ResetPosition();
        //_grid.Reposition();
        DisplayInfo();
        //SelectIndexItem();
    }

    /// <summary>
    /// 显示装备物品
    /// </summary>
    public void DisplayEquipList()
    {
        ViewHelper.FormatTemplate<BetterList<EquipmentVo>,EquipmentVo, StrengThenDisplayItem>
            (_prefab, _grid.transform
            , StrengThenManager.Instance.Equips,
            (EquipmentVo vo, StrengThenDisplayItem d) =>
            {
				d.BindingTipsData((uint)vo.Id,vo.InstanceId);
                d.Display(vo.InstanceId, vo.Item.quality, vo.Item.icon, vo.Item.name,
                    vo.Item.usedLevel.ToString(),
                    StrengThenManager.GetStringByCareer(vo.Item.career),
                    vo.StrengThenLevel,
                    0);
            });
    }
    /// <summary>
    /// 显示背包物品
    /// </summary>
    public void DisplayBagList()
    {
        ViewHelper.FormatTemplate<BetterList<EquipmentVo>, EquipmentVo, StrengThenDisplayItem>
            (_prefab, _grid.transform
            , StrengThenManager.Instance.Bags,
            (EquipmentVo vo, StrengThenDisplayItem d) =>
            {
				d.BindingTipsData((uint)vo.Id,vo.InstanceId);
                d.Display(vo.InstanceId, vo.Item.quality, vo.Item.icon, vo.Item.name,
                    vo.Item.usedLevel.ToString(),
                    StrengThenManager.GetStringByCareer(vo.Item.career),
                    vo.StrengThenLevel,
                    BagManager.Instance.PowerCompare(vo.Info));
                
            });
    }

    public void SelectIndexItem()
    {
        if (_grid.transform.childCount != 0)
        {
            _grid.transform.FindChild("0").GetComponent<UIToggle>().value = true;
            Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_SELECT_ITEM,
                 _grid.transform.FindChild("0").GetComponent<StrengThenDisplayItem>().Id
                );
        }
        else {
            Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_SELECT_ITEM, 0);
        }
    }
    #endregion

    /// <summary>
    /// 显示详细信息
    /// </summary>
    public void DisplayInfo()
    {
        EquipmentVo vo = StrengThenManager.Instance.SelectVo;
        if (vo == null)
        {
            _descObj.SetActive(false);
            return;
        }
        else {
            _descObj.SetActive(true);
        }

        MAX_STRENGTHEN = (int)vo.EquipData.maxForgeLevel;
        //显示名字
        if (vo.StrengThenLevel==0)
            _name.text = string.Format(NameFormatNone, vo.Item.name);
        else
            _name.text = string.Format(NameFormat, vo.Item.name, vo.StrengThenLevel);
        //显示星星
        //DisplayStart(vo.StrengThenLevel);

        //显示基础属性
        DisplayAttribute(vo);

        //显示消耗列表
        DisplayItemList(vo);

        //显示成功概率
        DisplaySuccessrate(vo);
    }

    private void EffectShow(Vector3 pos0, Vector3 pos1, Vector3 pos2)
    {
        prefEffect = transform.parent.FindChild("UI_Trail").gameObject;
        prefEffect.transform.GetChild(0).gameObject.SetActive(true);
        NPCManager.Instance.createCamera(true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(prefEffect.transform.GetChild(0).gameObject) as GameObject;
        obj.layer = LayerMask.NameToLayer("TopUI");
        obj.transform.parent = NPCManager.Instance.ModelCamera.transform;
        obj.transform.position = new Vector3(pos0.x, pos0.y, 10);
        obj.transform.localScale = Vector3.one;

        //obj.AddComponent<TweenPosition>();
        obj.GetComponent<TweenPosition>().from = new Vector3(pos0.x, pos0.y, 10);
        obj.GetComponent<TweenPosition>().to = new Vector3(pos1.x, pos1.y, 10);

        obj.GetComponent<TweenPosition>().ResetToBeginning();
        obj.GetComponent<TweenPosition>().PlayForward();

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("TopUI");
        }
        StartCoroutine(DisplayEffectCircle(pos2));
        SkillTalentManager.Instance.effectObjList.Add(obj);
        prefEffect.transform.GetChild(0).gameObject.SetActive(false);
        Destroy(obj, 1.5f);
    }

    public void DisplayEffect()
    {
        Vector3 pos0 = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                        NPCManager.Instance.ModelCamera, _button.transform.position);
        Vector3 pos1 = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                        NPCManager.Instance.ModelCamera, _grid.transform.GetChild(StrengThenManager.Instance.ChooseItem).FindChild("icon").position);
        EffectShow(pos0, pos1, _grid.transform.GetChild(StrengThenManager.Instance.ChooseItem).FindChild("icon").position);
    }

    IEnumerator DisplayEffectCircle(Vector3 pos)
    {
        yield return new WaitForSeconds(1.2f);
        prefEffectCircle.SetActive(true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(prefEffectCircle) as GameObject;
        obj.transform.parent = transform.FindChild("Background");
        obj.transform.position = pos;
        obj.transform.localScale = Vector3.one;
        prefEffectCircle.SetActive(false);
        Destroy(obj, 1f);
    }

    private void DisplayStart(int level)
    {
        //if (level > MAX_STRENGTHEN) level = MAX_STRENGTHEN;
        //for (int i = 0; i < starts.Length; i++)
        //{
        //    if (i <= level - 1)
        //    {
        //        starts[i].alpha = 1;
        //    }
        //    else
        //    {
        //        starts[i].alpha = 0;
        //    }
        //}
    }

    private void DisplayAttribute(EquipmentVo vo)
    {
        if (vo.EquipData.bStateValue2 == 0)
        {
            _As[0].text = EquipmentManager.GetEquipAttributeName(vo.EquipData.bStateType1);
            _As[1].text = "";
            _Vs[0].text = vo.EquipData.bStateValue1.ToString();
            _Vs[1].text = "";

            if (vo.StrengThenLevel == MAX_STRENGTHEN)
            {
                _Es[0].text = string.Format(MAX_LBL, ColorConst.Color_LiangHuang, vo.CurStrengThen.StrengThenValue[0].Value);
                _Es[1].text = "";
                _maxLbl.SetActive(true);
                _descObj.transform.FindChild("Buttom/lbl").gameObject.SetActive(false);
                _descObj.transform.FindChild("Buttom/Suc").gameObject.SetActive(false);
                _descObj.transform.FindChild("Buttom/SucAdd").gameObject.SetActive(false);
            }
            else
            {
                _maxLbl.SetActive(false);
                _descObj.transform.FindChild("Buttom/lbl").gameObject.SetActive(true);
                _descObj.transform.FindChild("Buttom/Suc").gameObject.SetActive(true);
                _descObj.transform.FindChild("Buttom/SucAdd").gameObject.SetActive(true);
                _Es[0].text = string.Format(EVALUEFORMATE, ColorConst.Color_Green, UP, vo.NextStrengThen.StrengThenValue[0].Value);
                _Es[1].text = "";

            }
        }
        else {
            _As[0].text = EquipmentManager.GetEquipAttributeName(vo.EquipData.bStateType1);
            _As[1].text = EquipmentManager.GetEquipAttributeName(vo.EquipData.bStateType2);
            _Vs[0].text = vo.EquipData.bStateValue1.ToString();
            _Vs[1].text = vo.EquipData.bStateValue2.ToString();

            if (vo.StrengThenLevel == MAX_STRENGTHEN)
            {
                _Es[0].text = string.Format(MAX_LBL, ColorConst.Color_LiangHuang, vo.CurStrengThen.StrengThenValue[0].Value);
                _Es[1].text = string.Format(MAX_LBL, ColorConst.Color_LiangHuang, vo.CurStrengThen.StrengThenValue[1].Value);
                _maxLbl.SetActive(true);
                _descObj.transform.FindChild("Buttom/lbl").gameObject.SetActive(false);
                _descObj.transform.FindChild("Buttom/Suc").gameObject.SetActive(false);
                _descObj.transform.FindChild("Buttom/SucAdd").gameObject.SetActive(false);
            }
            else
            {
                _maxLbl.SetActive(false);
                _descObj.transform.FindChild("Buttom/lbl").gameObject.SetActive(true);
                _descObj.transform.FindChild("Buttom/Suc").gameObject.SetActive(true);
                _descObj.transform.FindChild("Buttom/SucAdd").gameObject.SetActive(true);
                _Es[0].text = string.Format(EVALUEFORMATE, ColorConst.Color_Green, UP, vo.NextStrengThen.StrengThenValue[0].Value);
                _Es[1].text = string.Format(EVALUEFORMATE, ColorConst.Color_Green, UP, vo.NextStrengThen.StrengThenValue[1].Value);

            } 
        }
        
        

    }
    private void DisplayItemList(EquipmentVo vo)
    {

        if (vo.StrengThenLevel == MAX_STRENGTHEN)
        {
            for (int i = 0; i < MAX_CONSUME_ITEM; i++)
            {
                _consumes[i].Icon = null;
                _consumes[i].Lable = null;
                _consumes[i].Quality = eItemQuality.eWhite;
            }
        }
        else {

            BetterList<TypeStruct> item = vo.NextStrengThen.ConsumeItem;
            int index = 0;
            for (int i = 0; i < MAX_CONSUME_ITEM; i++)
            {
                if (i < item.size)
                {
                    if (item[i].Type == ConsumeType.Item)
                    {
                        ItemTemplate tt = StrengThenManager.GetItemById(item[i].Id);
                        _consumes[i].Icon = tt.icon;
                        _consumes[i].Lable = ViewHelper.GetItemHave(item[i].Id, item[i].Value);
                        _consumes[i].Quality = tt.quality;
						_consumes[i].transform.FindChild("Icon").GetComponent<BtnTipsMsg>().Iteminfo = new ItemInfo(tt.id,0,0);
                    }
                    else
                    {
                        _consumes[i].Icon = SourceManager.Instance.getIocnStringByType((eGoldType)item[i].Id);
                        _consumes[i].Lable = ViewHelper.GetGoldHave((eGoldType)item[i].Id, item[i].Value);
                        _consumes[i].Quality = eItemQuality.eOrange;
                    }
                    index = i + 1;
                }
                else
                {
                    _consumes[i].Icon = null;
                    _consumes[i].Lable = null;
                    _consumes[i].Quality = eItemQuality.eGreen;
                }
            }
            //_consumes[index]
            if (vo.NextStrengThen.IsUsedLuckStone)
            {
                LuckStoneVo lv = LuckStoneManager.Instance.SelectedStone;
                if (lv != null)
                {
                    TypeStruct ts = LuckStoneManager.Instance.SelectedStoneConsume;
                    if (ts.Type == ConsumeType.Item)
                    {
                        ItemTemplate tt = StrengThenManager.GetItemById(ts.Id);
                        _consumes[index].Icon = tt.icon;
                        _consumes[index].Lable = ViewHelper.GetItemHave(ts.Id, ts.Value);
                        _consumes[index].Quality = tt.quality;
                    }
                    else
                    {
                        _consumes[index].Icon = SourceManager.Instance.getIocnStringByType((eGoldType)ts.Id);
                        _consumes[index].Lable = ViewHelper.GetGoldHave((eGoldType)ts.Id, ts.Value);
                        _consumes[index].Quality = eItemQuality.eOrange;
                    }
                }
            }
        }

    }

    private void DisplaySuccessrate(EquipmentVo vo)
    {
        if (vo.StrengThenLevel == MAX_STRENGTHEN)
        {
            _luckBtn.SetActive(false);
            _button.SetActive(false);
            _suc.text = string.Format(SUCFORMAT, 100);
            _sucAdd.text = "";
        }
        else {
            _button.SetActive(true);
            _luckBtn.SetActive(vo.NextStrengThen.IsUsedLuckStone);      //是否显示增加成功率按钮

            _suc.text = string.Format(SUCFORMAT, (int)(vo.NextStrengThen.Successrate / SUCLES));
            if (LuckStoneManager.Instance.SelectedStone != null)
            {
                _sucAdd.text = string.Format(SUCADDFORMAT, ColorConst.Color_Green,
                    (int)(LuckStoneManager.Instance.SelectedStone.Successrate / SUCLES));
            }
            else
            {
                _sucAdd.text = string.Format(SUCADDFORMAT, ColorConst.Color_Red, 0);
            }
        }
        
    }
}
