using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using helper;

public class AdvancedView : MonoBehaviour {

    const int MAX_STRENGTHEN = 10;  //最大强化值
    const int MAX_ATTRIBUTE_COUNT = 2;//最大2组属性值
    const int MAX_CONSUME_ITEM = 5;     //最大奖励个数
    
    const string LEVELFORMAT = "lv{0}";     //装备等级格式
    const string ST_LEVEL_FORMAT = "+{0}";  //强化等级格式

    const string EVALUEFORMATE = "([{0}]{1}{2}[-])";
    const string UP = "↑";
    const string SUCFORMAT = "{0}%";

    const string SUCADDFORMAT = "(+[{0}]{1}[-]%)";
    const float SUCLES = 10;


    #region 列表
    private UIScrollView _view;
    private UIGrid _grid;
    private GameObject _prefab;
    private GameObject _descObj;
    private GameObject _luckBtn;    //增加成功率按钮
    private GameObject _button;
    private GameObject prefEffect;
    private GameObject prefEffectCircle;
    private const string UI_TRAIL = "Effect/Effect_Prefab/UI/UI_Trail";
    #endregion

    #region 详细信息
    private ItemLabelEx _baseItem;  //当前装备
    private ItemLabelEx _newItem;   //进阶后的装备
    private UILabel[] _As;          //基础属性，描述
    private UILabel[] _Vs;          //基础属性，值
    private UILabel[] _Es;          //基础属性，对比
    private UILabel[] _BAs;          //基础属性，描述
    private UILabel[] _BVs;          //基础属性，值
    private ItemLabel[] _consumes;  //消耗列表
    private UILabel _suc;           //成功率
    private UILabel _sucAdd;        //增加成功率
    #endregion
    #region 背景
    private UISprite _sp1;
    private GameObject _newObj;     //进阶属性
    private GameObject _lbl;
    #endregion
    private void Awake()
    {
        _descObj = transform.FindChild("Description").gameObject;
        _view = transform.FindChild("ItemPanel").GetComponent<UIScrollView>();
        _grid = transform.FindChild("ItemPanel/Grid").GetComponent<UIGrid>();
        _prefab = transform.FindChild("ItemPanel/Item").gameObject;
        _luckBtn = _descObj.transform.FindChild("Buttom/Button_LuckStone").gameObject;
        _button = _descObj.transform.FindChild("Buttom/Button_Ok").gameObject;
        prefEffect = transform.parent.FindChild("UI_Trail").gameObject;

        _lbl = transform.FindChild("Description/Top/lbl1").gameObject;
        _newObj = transform.FindChild("Description/Top/Descs").gameObject;
        _sp1 = transform.FindChild("Description/Top/sp").GetComponent<UISprite>();
        _baseItem = transform.FindChild("Description/Top/Item1").GetComponent<ItemLabelEx>();
        _newItem = transform.FindChild("Description/Top/Item2").GetComponent<ItemLabelEx>();
        prefEffectCircle = transform.FindChild("Background/SuccessEffect").gameObject;

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

        //基础属性
        _BAs = new UILabel[MAX_ATTRIBUTE_COUNT];
        for (int i = 0; i < MAX_ATTRIBUTE_COUNT; i++)
        {
            _BAs[i] = transform.FindChild("Description/Top/DescsBase/A" + (i + 1)).GetComponent<UILabel>();
        }
        _BVs = new UILabel[MAX_ATTRIBUTE_COUNT];
        for (int i = 0; i < MAX_ATTRIBUTE_COUNT; i++)
        {
            _BVs[i] = transform.FindChild("Description/Top/DescsBase/V" + (i + 1)).GetComponent<UILabel>();
        }

        _consumes = new ItemLabel[MAX_CONSUME_ITEM];
        for (int i = 0; i < MAX_CONSUME_ITEM; i++)
        {
            _consumes[i] = transform.FindChild("Description/Center/Item" + (i + 1)).GetComponent<ItemLabel>();
        }

        _suc = transform.FindChild("Description/Buttom/Suc").GetComponent<UILabel>();
        _sucAdd = transform.FindChild("Description/Buttom/SucAdd").GetComponent<UILabel>();
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
        Gate.instance.registerMediator(new AdvancedMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.ADVANCED_MEDIATOR);
    }


    /// <summary>
    /// 切换标签
    /// </summary>
    /// <param name="table"></param>
    public void DisplayList(Table table)
    {
        if (table != AdvancedManager.Instance.SelectTable)
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
            AdvancedManager.Instance.SelectTable = table;
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
        AdvancedManager.Instance.SelectTable = table;
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
        ViewHelper.FormatTemplate<BetterList<EquipmentVo>, EquipmentVo, StrengThenDisplayItem>
            (_prefab, _grid.transform
            , AdvancedManager.Instance.Equips,
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
            , AdvancedManager.Instance.Bags,
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
            Gate.instance.sendNotification(MsgConstant.MSG_ADVANCED_SELECT_ITEM,
                 _grid.transform.FindChild("0").GetComponent<StrengThenDisplayItem>().Id
                );
        }
        else
        {
            Gate.instance.sendNotification(MsgConstant.MSG_ADVANCED_SELECT_ITEM, 0);
        }
    }
    #endregion


    public void DisplayInfo()
    {
        EquipmentVo vo = AdvancedManager.Instance.SelectVo;
        if (vo == null)
        {
            _descObj.SetActive(false);
            return;
        }
        else
        {
            _descObj.SetActive(true);
        }

        //显示物品
        DisplayItem(vo);

        //显示基本信息
        DisplayAttribute(vo);

        //显示消耗物品
        DisplayItemList(vo);

        //显示成功率
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
                        NPCManager.Instance.ModelCamera, _grid.transform.GetChild(AdvancedManager.Instance.selectItem).FindChild("icon").position);
        EffectShow(pos0, pos1, _grid.transform.GetChild(AdvancedManager.Instance.selectItem).FindChild("icon").position);
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


    private void DisplayItem(EquipmentVo vo)
    {
        _baseItem.Quality = vo.Item.quality;
        _baseItem.Icon = vo.Item.icon;
        _baseItem.Lable = string.Format(LEVELFORMAT,vo.Item.usedLevel);
        if (vo.StrengThenLevel == 0)
        {
            _baseItem.LableEx = null;
        }
        else {
            _baseItem.LableEx = string.Format(ST_LEVEL_FORMAT, vo.StrengThenLevel);
        }
        
		_baseItem.transform.FindChild("Icon").GetComponent<BtnTipsMsg>().Iteminfo = new ItemInfo((uint)vo.Id,vo.InstanceId,0);

        if (vo.EquipData.IsMaxAdvanced)//达到最大进阶
        {
            _sp1.alpha = 0;
            _newItem.gameObject.SetActive(false);
        }
        else {
            _sp1.alpha = 1;
            _newItem.gameObject.SetActive(true);
            _newItem.Quality = vo.NextAdvancedItem.quality;
            _newItem.Icon = vo.NextAdvancedItem.icon;
            _newItem.Lable = string.Format(LEVELFORMAT, vo.NextAdvancedItem.usedLevel);
            if (vo.StrengThenLevel == 0)
                _newItem.LableEx = null;
            else
                _newItem.LableEx = string.Format(ST_LEVEL_FORMAT, vo.StrengThenLevel);
			_newItem.transform.FindChild("Icon").GetComponent<BtnTipsMsg>().Iteminfo = new ItemInfo((uint)vo.NextAdvancedItem.id,vo.InstanceId,0);
        }
    }
    private void DisplayAttribute(EquipmentVo vo)
    {
        if (vo.EquipData.bStateValue2 == 0)
        {
            _BAs[0].text = EquipmentManager.GetEquipAttributeName(vo.EquipData.bStateType1);
            _BAs[1].text = "";
            _BVs[0].text = vo.EquipData.bStateValue1.ToString();
            _BVs[1].text = "";

            if (vo.EquipData.IsMaxAdvanced)
            {
                _lbl.SetActive(true);
                _newObj.SetActive(false);
                _descObj.transform.FindChild("Buttom/lbl").gameObject.SetActive(false);
                _descObj.transform.FindChild("Buttom/Suc").gameObject.SetActive(false);
                _descObj.transform.FindChild("Buttom/SucAdd").gameObject.SetActive(false);
            }
            else
            {
                _lbl.SetActive(false);
                _newObj.SetActive(true);
                _descObj.transform.FindChild("Buttom/lbl").gameObject.SetActive(true);
                _descObj.transform.FindChild("Buttom/Suc").gameObject.SetActive(true);
                _descObj.transform.FindChild("Buttom/SucAdd").gameObject.SetActive(true);
                _As[0].text = EquipmentManager.GetEquipAttributeName(vo.NextAdvanceEquip.bStateType1);
                _As[1].text = "";
                _Vs[0].text = vo.NextAdvanceEquip.bStateValue1.ToString();
                _Vs[1].text = "";

                _Es[0].text = string.Format(EVALUEFORMATE, ColorConst.Color_Green, UP,
                    vo.NextAdvanceEquip.bStateValue1 - vo.EquipData.bStateValue1);
                _Es[1].text = "";

            }
        }
        else {
            _BAs[0].text = EquipmentManager.GetEquipAttributeName(vo.EquipData.bStateType1);
            _BAs[1].text = EquipmentManager.GetEquipAttributeName(vo.EquipData.bStateType2);
            _BVs[0].text = vo.EquipData.bStateValue1.ToString();
            _BVs[1].text = vo.EquipData.bStateValue2.ToString();

            if (vo.EquipData.IsMaxAdvanced)
            {
                _lbl.SetActive(true);
                _newObj.SetActive(false);
                _descObj.transform.FindChild("Buttom/lbl").gameObject.SetActive(false);
                _descObj.transform.FindChild("Buttom/Suc").gameObject.SetActive(false);
                _descObj.transform.FindChild("Buttom/SucAdd").gameObject.SetActive(false);
            }
            else
            {
                _lbl.SetActive(false);
                _newObj.SetActive(true);
                _descObj.transform.FindChild("Buttom/lbl").gameObject.SetActive(true);
                _descObj.transform.FindChild("Buttom/Suc").gameObject.SetActive(true);
                _descObj.transform.FindChild("Buttom/SucAdd").gameObject.SetActive(true);
                _As[0].text = EquipmentManager.GetEquipAttributeName(vo.NextAdvanceEquip.bStateType1);
                _As[1].text = EquipmentManager.GetEquipAttributeName(vo.NextAdvanceEquip.bStateType2);
                _Vs[0].text = vo.NextAdvanceEquip.bStateValue1.ToString();
                _Vs[1].text = vo.NextAdvanceEquip.bStateValue2.ToString();

                _Es[0].text = string.Format(EVALUEFORMATE, ColorConst.Color_Green, UP,
                    vo.NextAdvanceEquip.bStateValue1 - vo.EquipData.bStateValue1);
                _Es[1].text = string.Format(EVALUEFORMATE, ColorConst.Color_Green, UP,
                   vo.NextAdvanceEquip.bStateValue2 - vo.EquipData.bStateValue2);

            }
        }
        
        
    }


    private void DisplayItemList(EquipmentVo vo)
    {
        if (vo.EquipData.IsMaxAdvanced)
        {
            for (int i = 0; i < MAX_CONSUME_ITEM; i++)
            {
                _consumes[i].Icon = null;
                _consumes[i].Lable = null;
                _consumes[i].Quality = eItemQuality.eWhite;
            }
        }
        else {
            BetterList<TypeStruct> item = vo.EquipData.EquipmentUpItem;
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
            if (vo.NextAdvanceEquip.IsUsedLuckStone)
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
        if (vo.EquipData.IsMaxAdvanced)
        {
            _suc.text = string.Format(SUCFORMAT, 100);
            _sucAdd.text = "";
            _button.SetActive(false);
            _luckBtn.SetActive(false);
        }
        else {
            _button.SetActive(true);
            _luckBtn.SetActive(vo.NextAdvanceEquip.IsUsedLuckStone);      //是否显示增加成功率按钮
            _suc.text = string.Format(SUCFORMAT, (int)(vo.NextAdvanceEquip.EquipmentUpSuccessrate / SUCLES));
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
