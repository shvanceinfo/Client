using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using helper;
using model;
using helper;
public class RefineView : MonoBehaviour
{

    const string NameFormat = "{0} +{1}";
    const string NameFormatNone = "{0}";
    const int MAX_ATTR = RefineManager.MAX_REFINE_SIZE / 2;

    const string AT = "[{0}]{1}[-] [{2}]{3}[-]({4}-{5})";//颜色1,属性名,颜色2,基础值,最大值
    const string AM = "[{0}]{1}[-] [{2}]{3}[-]([{4}]满值[-])";//颜色1,属性名,颜色2,基础值,颜色3
    const string AN = "[{0}]可洗练[-]";
    const string AL = "[{0}]未洗练[-] ({1}级或VIP{2}开启)";


    private UIScrollView _view;
    private UIGrid _grid;
    private GameObject _prefab;
    private GameObject _descObj;

    private UILabel _name;
    private RefineDisplayAttr[] _attrs;

    private GameObject _resetObj;

    private GameObject _resetGrid;
    private GameObject _resetPrefab;
    private UITexture _goldIcon;
    private UILabel _count;

    private GameObject prefEffect;
    private GameObject prefEffectCircle;
    private const string UI_TRAIL = "Effect/Effect_Prefab/UI/UI_Trail";

    private void Awake()
    {
        _descObj = transform.FindChild("Description").gameObject;
        _view = transform.FindChild("ItemPanel").GetComponent<UIScrollView>();
        _grid = transform.FindChild("ItemPanel/Grid").GetComponent<UIGrid>();
        _prefab = transform.FindChild("ItemPanel/Item").gameObject;

        prefEffect = transform.parent.FindChild("UI_Trail").gameObject;
        prefEffectCircle = transform.FindChild("Background/SuccessEffect").gameObject;

        _name = _descObj.transform.FindChild("Top/Name").GetComponent<UILabel>();

        _attrs = new RefineDisplayAttr[MAX_ATTR];
        for (int i = 0; i < MAX_ATTR; i++)
        {
            _attrs[i] = _descObj.transform.FindChild("Center/" + i).GetComponent<RefineDisplayAttr>();
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

        _resetObj = transform.FindChild("Reset").gameObject;
        _resetObj.SetActive(false);
        _resetGrid = transform.FindChild("Reset/PanelList/Grid").gameObject;
        _resetPrefab = transform.FindChild("Reset/PanelList/Item").gameObject;
        _goldIcon = transform.FindChild("Reset/Consume/Icon").GetComponent<UITexture>();
        _count = transform.FindChild("Reset/Consume/Label").GetComponent<UILabel>();
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
        Gate.instance.registerMediator(new RefineMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.REFINE_MEDIATOR);
    }


    /// <summary>
    /// 切换标签
    /// </summary>
    /// <param name="table"></param>
    public void DisplayList(Table table)
    {
        if (table != RefineManager.Instance.SelectTable)
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
            RefineManager.Instance.SelectTable = table;
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
        RefineManager.Instance.SelectTable = table;
        _view.ResetPosition();
        _grid.Reposition();
        //SelectIndexItem();
        DisplayInfo();
    }

    /// <summary>
    /// 显示装备物品
    /// </summary>
    public void DisplayEquipList()
    {
        ViewHelper.FormatTemplate<BetterList<EquipmentVo>, EquipmentVo, StrengThenDisplayItem>
            (_prefab, _grid.transform
            , RefineManager.Instance.Equips,
            (EquipmentVo vo, StrengThenDisplayItem d) =>
            {
                d.BindingTipsData((uint)vo.Id, vo.InstanceId);
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
            , RefineManager.Instance.Bags,
            (EquipmentVo vo, StrengThenDisplayItem d) =>
            {
                d.BindingTipsData((uint)vo.Id, vo.InstanceId);
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
            Gate.instance.sendNotification(MsgConstant.MSG_REFINE_SELECT_ITEM,
                 _grid.transform.FindChild("0").GetComponent<StrengThenDisplayItem>().Id
                );
        }
        else
        {
            Gate.instance.sendNotification(MsgConstant.MSG_REFINE_SELECT_ITEM, 0);
        }
    }
    #endregion
    
    public void DisplayInfo()
    {
        EquipmentVo vo = RefineManager.Instance.SelectVo;
        if (vo == null)
        {
            _descObj.SetActive(false);
            return;
        }
        else
        {
            _descObj.SetActive(true);
        }
        
        if (vo.StrengThenLevel == 0)
            _name.text = string.Format(NameFormatNone, vo.Item.name);
        else
            _name.text = string.Format(NameFormat, vo.Item.name, vo.StrengThenLevel);

        DisplayAttribute(vo);
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

    public void DisplayEffect(int selectId)
    {
        Vector3 pos0 = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                        NPCManager.Instance.ModelCamera, _descObj.transform.FindChild("Center").GetChild(selectId).FindChild("Button_Refine").position);
        Vector3 pos1 = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                        NPCManager.Instance.ModelCamera, _grid.transform.GetChild(RefineManager.Instance.SelectRefineItem).FindChild("icon").position);
        EffectShow(pos0, pos1, _grid.transform.GetChild(RefineManager.Instance.SelectRefineItem).FindChild("icon").position);
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


    public void DisplayAttribute(EquipmentVo vo)
    {
        BetterList<RefineInfoVo> infos = RefineManager.Instance.EquipInfo;
        for (int i = 0; i < MAX_ATTR; i++)
        {
            int index = i * 2;
            if (infos[index].Status == RefineStatus.Lock && infos[index + 1].Status == RefineStatus.Lock)
            {
                _attrs[i].SetRefineButton(false);
                _attrs[i].IsAcive(false);
            }
            else
            {
                if (infos[index].Status!=RefineStatus.Lock)
                {
                    string icon = StrengThenManager.GetItemById(infos[index].Vo.ConsumeItems[0].Id).icon;

                    int needCount = infos[index].Vo.ConsumeItems[0].Value;
                    int haveCount = LuckStoneManager.Instance.GetConsumeItem(infos[index].Vo.ConsumeItems[0].Id);
                    string color = haveCount >= needCount ? ColorConst.Color_Green : ColorConst.Color_Red;
                    string count = string.Format("[{0}]{1}/{2}[-]", color, haveCount, needCount);

                    _attrs[i].DisplayItem(icon, count);
                }
                if (infos[index+1].Status!=RefineStatus.Lock)
                {
                    string icon = StrengThenManager.GetItemById(infos[index + 1].Vo.ConsumeItems[0].Id).icon;

                    int needCount = infos[index + 1].Vo.ConsumeItems[0].Value;
                    int haveCount = LuckStoneManager.Instance.GetConsumeItem(infos[index + 1].Vo.ConsumeItems[0].Id);
                    string color = haveCount >= needCount ? ColorConst.Color_Green : ColorConst.Color_Red;
                    string count = string.Format("[{0}]{1}/{2}[-]", color, haveCount, needCount);

                    _attrs[i].DisplayItem(icon, count);
                }


            }
            _attrs[i].DisplayAttribute1(Format(infos[index]));
            _attrs[i].DisplayAttribute2(Format(infos[index + 1]));

        }
    }

    //改变属性显示方式
    private string ChangeInfoShow(RefineInfoVo vo, string data)
    {
        float dataInfo = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001010).type7Data * 100;
        switch (vo.Type)
        {
            case eFighintPropertyCate.eFPC_Precise:
            case eFighintPropertyCate.eFPC_Dodge:
            case eFighintPropertyCate.eFPC_BlastAttack:
            case eFighintPropertyCate.eFPC_Tenacity:
            case eFighintPropertyCate.eFPC_AntiFightBreak:
            case eFighintPropertyCate.eFPC_FightBreak:
            case eFighintPropertyCate.eFPC_BlastAttackAdd:
            case eFighintPropertyCate.eFPC_BlastAttackReduce:
                string info = (vo.BaseValue * dataInfo) + "%";
                return info;
                break;
            default:
                break;
        }
        return "";
    }


    public string Format(RefineInfoVo info)
    {

        float dataInfo = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001010).type7Data * 100;
        

        string str = "";

        switch (info.Status)
        {
            case RefineStatus.Lock:
                str = string.Format(AL, ColorConst.Color_HeSe, info.Vo.UnLockLevel, info.Vo.UnLockVip);
                break;
            case RefineStatus.Standby:
                str = string.Format(AN, ColorConst.Color_LiangHuang);
                break;
            case RefineStatus.UnLock:


                int maxValue = info.Vo[info.Type];

                string s0 = PowerManager.Instance.ChangeInfoData(info.Type, info.BaseValue);
                string s1 = PowerManager.Instance.ChangeInfoData(info.Type, 1);
                string s2 = PowerManager.Instance.ChangeInfoData(info.Type, maxValue);

                if (info.BaseValue == maxValue) //满值
                {
                    str = string.Format(AM, ColorConst.Color_HeSe,
                        EquipmentManager.GetEquipAttributeName(info.Type),
                        ColorConst.Color_LiangHuang,
                        s0,
                        ColorConst.Color_LiangHuang);
                }
                else //未满值
                {
                    str = string.Format(AT, ColorConst.Color_HeSe,
                        EquipmentManager.GetEquipAttributeName(info.Type),
                        ColorConst.Color_Green,
                        s0,
                        s1,
                        s2);
                }
                break;
            default:
                break;
        }

        return str;
    }

    //显示重置属性列表
    public void DisplayReset(bool isActive)
    {
        _resetObj.SetActive(isActive);
        if (!isActive)
        {
            return;
        }
        ViewHelper.FormatTemplate<BetterList<RefineInfoVo>, RefineInfoVo, RefineDisplayResetAttr>
            (_resetPrefab, _resetGrid.transform
            , RefineManager.Instance.EquipInfo,
            (RefineInfoVo vo, RefineDisplayResetAttr d) =>
            {
                d.AttributeName = Format(vo);
                d.IsCheck = false;
                d.IsLock = false;
                if (vo.Status != RefineStatus.UnLock)
                {
                    d.LockCheckBox();
                }
                else {
                    d.UnLockCheckBox();   
                }
            });
        DisplayResetConsume();
    }

    public void DisplayResetConsume()
    {
        int need = 0;
        int have = LuckStoneManager.Instance.GetConsumeItem(RefineManager.Instance.ResetItemID);
        BetterList<int> bs = RefineManager.Instance.ResetList;
        for (int i = 0; i < bs.size; i++)
        {
            need += RefineManager.Instance.EquipInfo[bs[i]].Vo.ResetConsume[0].Value;
        }
        string color = ColorConst.Color_Green;
        if (have < need)
            color = ColorConst.Color_Red;
        _count.text = string.Format("[{0}]{1}/{2}[-]", color, have, need);

        _goldIcon.mainTexture = SourceManager.Instance.getTextByIconName(ItemManager.GetInstance().GetTemplateByTempId((uint)RefineManager.Instance.ResetItemID).icon);
    }
}
