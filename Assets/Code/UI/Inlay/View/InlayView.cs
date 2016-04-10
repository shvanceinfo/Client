using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using helper;
using model;

public class InlayView : MonoBehaviour {

    const string LVL = "LV{0}";
    const int MAXGEMS = 4;
    #region 列表
    private UIScrollView _view;
    private UIGrid _grid;
    private GameObject _prefab;
    private GameObject _descObj;
    #endregion

    InlayDisplayGem[] gems;
    UILabel _equipName;
    UILabel _error;
    private Transform _gemGrid;
    private GameObject _gemPrefab;

    private GameObject prefEffect;
    private GameObject prefEffectCircle;
    private const string UI_TRAIL = "Effect/Effect_Prefab/UI/UI_Trail";

    private void Awake()
    {
        _descObj = transform.FindChild("Description").gameObject;
        _view = transform.FindChild("ItemPanel").GetComponent<UIScrollView>();
        _grid = transform.FindChild("ItemPanel/Grid").GetComponent<UIGrid>();
        _prefab = transform.FindChild("ItemPanel/Item").gameObject;

        gems = new InlayDisplayGem[MAXGEMS];
        for (int i = 0; i < MAXGEMS; i++)
        {
            gems[i] = transform.FindChild("Description/Top/Gems/" + i).GetComponent<InlayDisplayGem>();
        }

        _equipName = transform.FindChild("Description/Top/EquipName").GetComponent<UILabel>();

        _gemGrid = transform.FindChild("Description/Top/Panel/Grid");
        _gemPrefab = transform.FindChild("Description/Top/Panel/Item").gameObject;
        _error = transform.FindChild("Description/Top/Error").GetComponent<UILabel>();

        prefEffect = transform.parent.FindChild("UI_Trail").gameObject;
        prefEffectCircle = transform.FindChild("Background/SuccessEffect").gameObject;

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
        Gate.instance.registerMediator(new InlayMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.INLAY_MEDIATOR);
    }


    /// <summary>
    /// 显示装备物品
    /// </summary>
    public void DisplayEquipList()
    {
        ViewHelper.FormatTemplate<BetterList<EquipmentVo>, EquipmentVo, StrengThenDisplayItem>
            (_prefab, _grid.transform
            , InlayManager.Instance.Equips,
            (EquipmentVo vo, StrengThenDisplayItem d) =>
            {
                d.Display(vo.InstanceId, vo.Item.quality, vo.Item.icon, vo.Item.name,
                    vo.Item.usedLevel.ToString(),
                    StrengThenManager.GetStringByCareer(vo.Item.career),
                    vo.StrengThenLevel,
                    0);
            });
        _view.ResetPosition();
        _grid.Reposition();
        SelectIndexItem();
    }


    public void SelectIndexItem()
    {
        if (_grid.transform.childCount != 0)
        {
            _grid.transform.FindChild("0").GetComponent<UIToggle>().value = true;
            Gate.instance.sendNotification(MsgConstant.MSG_INLAY_SELECT_ITEM,
                 _grid.transform.FindChild("0").GetComponent<StrengThenDisplayItem>().Id
                );
        }
        else
        {
            Gate.instance.sendNotification(MsgConstant.MSG_INLAY_SELECT_ITEM, 0);
        }
    }
    #endregion

    public void DisplayInfo()
    {
        EquipmentVo vo = InlayManager.Instance.SelectVo;
        if (vo == null)
        {
            _descObj.SetActive(false);
            return;
        }
        else {
            _descObj.SetActive(true);
        }
        DsiplayGem(vo);
        DisplayGemList();
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
                        NPCManager.Instance.ModelCamera, _descObj.transform.FindChild("Top/Button_Inlay").position);
        //Debug.LogError("-------------" + AdvancedManager.Instance.SelectVo.SortId);
        Vector3 pos1 = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                        NPCManager.Instance.ModelCamera, _descObj.transform.FindChild("Top/Gems").GetChild(selectId).FindChild("icon").position);
        EffectShow(pos0, pos1, _descObj.transform.FindChild("Top/Gems").GetChild(selectId).FindChild("icon").position);
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


    private void DsiplayGem(EquipmentVo vo)
    {
        BetterList<BoolStruct> inlays= InlayManager.Instance.InlayInfo[vo.EquipType].Gems;
        for (int i = 0; i < inlays.size; i++)
        {
            BoolStruct bs = inlays[i];
            if (bs.Value)   //解锁了
            {
                if (bs.Id == 0)
                    gems[i].CanInlay();
                else
                {
                    ItemTemplate gem = MergeManager.FindItem(bs.Id);//物品模板  
                    GemVo gv = MergeManager.Instance.GetGemVoById(bs.Id);//宝石模板
                    gems[i].DisplayGem(gem.icon,
                        gem.name,
                        EquipmentManager.GetEquipAttributeName( gv.Attribute[0].Type),
                        PowerManager.Instance.ChangeInfoData(gv.Attribute[0].Type, gv.Attribute[0].Value));
                        //gv.Attribute[0].Value.ToString());
                }
            }
            else
            {
                gems[i].IsLock(vo.Part.UnLockLevel[i].ToString(),
                    vo.Part.UnLockVip[i].ToString());
            }
        }
    }


    
    private void DisplayGemList()
    {
        ViewHelper.FormatTemplate<BetterList<InlayVo>, InlayVo, ItemLabelEx>
            (_gemPrefab, _gemGrid, InlayManager.Instance.SelectGems,
            (InlayVo vo,ItemLabelEx ex) =>
            {
                ex.Id = vo.ItemInfo.InstanceId;
                ex.Icon = vo.GemItem.icon;
                ex.Lable = string.Format(LVL, vo.Gem.Level);
                ex.LableEx = ColorConst.Format(ColorConst.Color_Green, vo.ItemInfo.Num);
                ex.QualityNoBoder = vo.GemItem.quality;
            }
            );
        _gemGrid.GetComponent<UIGrid>().Reposition();
        _gemGrid.parent.GetComponent<UIScrollView>().ResetPosition();
        if (InlayManager.Instance.SelectGems.size == 0)
        {
            _error.alpha = 1;
        }
        else {
            _error.alpha = 0;
        }
        SelectIndexGem();
    }
    public void SelectIndexGem()
    {
        if (_gemGrid.transform.childCount != 0)
        {
            
            _gemGrid.transform.FindChild("0").GetComponent<UICheckBoxObject>().isChecked = true;
            Gate.instance.sendNotification(MsgConstant.MSG_INLAY_SELECT_GEM,
                 _gemGrid.transform.FindChild("0").GetComponent<ItemLabelEx>().Id
                );
        }
        else
        {
            
            Gate.instance.sendNotification(MsgConstant.MSG_INLAY_SELECT_GEM, 0);
        }
    }
}
