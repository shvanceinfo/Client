using UnityEngine;
using System.Collections;
using manager;
using helper;
using model;
using MVC.entrance.gate;
using mediator;

public class VipView : HelperMono {

    private GameObject _titleObj;
    private UILabel _title;
    private UILabel _nextVip;
    private UILabel _curVip;
    private HealthBar _expBar;
    private UISprite _light;

    private GameObject _fun1;   //功能1
    private GameObject _fun2;   //功能2

    private UILabel _itemsLabel;    //礼包说明
    private UIGrid _itemGrid;
    private GameObject _itemPrefab;

    private VipViewDisplayImg[] _imgs;

    private UILabel _selectVip;         //下层展示VIP等级

    private GameObject _tipObj;         //礼包状态
    private GameObject _btnReceive;     //领取礼包按钮
    private UILabel _tipLabel;          //礼包文字
    private UILabel _tipLabelLevel;     //礼包VIP等级

    //-----------------------------------【Table2】-------------
    private GameObject _vipTitlePrefab,_vipPrefab;
    private UIGrid _vipTitleGrid,_vipGrid;
    private UIScrollView _vipView;

    private Table _oldTable;
    private void Awake()
    {
        _fun1 = F("Show");
        _fun2 = F("List");

        _titleObj = F("Top/Title");
        _title = F<UILabel>("Top/Title/Label");
        _nextVip = F<UILabel>("Top/Title/VipLevel");
        _curVip = F<UILabel>("Top/VipExp/VipLevel");
        _expBar = F<HealthBar>("Top/VipExp/ExpBar");
        _light = F<UISprite>("Top/VipExp/ExpBar/Back/Mask/guang");

        _itemsLabel = F<UILabel>("Show/Top/Level");
        _itemGrid = F<UIGrid>("Show/Top/Panel/Grid");
        _itemPrefab = F("Show/Top/Panel/Item");

        _imgs = new VipViewDisplayImg[VipManager.MAX_VIP_IMG_LEN];
        for (int i = 0; i < VipManager.MAX_VIP_IMG_LEN; i++)
        {
            _imgs[i] = F<VipViewDisplayImg>("Show/Center/" + i);
        }
        _selectVip = F<UILabel>("Show/Bottom/Level");

        _tipObj = F("Show/Top/Vip_Tip");
        _btnReceive = F("Show/Top/Button_Receive");
        _tipLabel = F<UILabel>("Show/Top/Vip_Tip/title");
        _tipLabelLevel = F<UILabel>("Show/Top/Vip_Tip/level");

        //-------------------------
        _vipTitlePrefab = F("List/TopPanel/Item");
        _vipTitleGrid = F<UIGrid>("List/TopPanel/Grid");
        _vipPrefab = F("List/CenterPanel/Item");
        _vipGrid = F<UIGrid>("List/CenterPanel/Grid");
        _vipView = F<UIScrollView>("List/CenterPanel");
        _oldTable = Table.None;
    }
    protected override ViewMediator Register()
    {
        return new VipMediator(this) ;
    }
    protected override uint RemoveMediator()
    {
        return MediatorName.VIP_MEDIATOR;
    }


    public void HiddenAllTip()
    {
        for (int i = 0; i < _imgs.Length; i++)
        {
            _imgs[i].DisplayTip();
        }
    }
    public void DisplayTip(int id)
    {
        _imgs[id].DisplayTip(VipManager.Instance.ShowVip.VipPictures[id].TipValue);
    }
    
    //显示开头
    private void DisplayTitle()
    {
        int value=1, maxvalue=1;
        if (VipManager.Instance.CurVip.VipId == VipManager.MAX_VIP_LEVEL)
        {
            _titleObj.SetActive(false);
        }
        else {
            //赋值标题
            _title.text = VipManager.FromatNextMoney();
            _nextVip.text = (VipManager.Instance.VipLevel + 1).ToString();
            value = VipManager.Instance.HaveMoney;
            maxvalue = VipManager.Instance.MaxMoney;
        }
        _curVip.text = string.Format("V{0}", VipManager.Instance.VipLevel);
        

        //赋值经验条
        _expBar.MaxValue = maxvalue;
        _expBar.Value = value;
        Vector3 v3= _light.transform.localPosition;
        v3.x = _expBar.MaxWidth * _expBar.Fill;
        _light.transform.localPosition = v3;
    }

    public void DisplayTable(Table table)
    {
        
        _fun1.SetActive(false);
        _fun2.SetActive(false);
        
        switch (table)
        {
            case Table.None:
                break;
            case Table.Table1:
                _fun1.SetActive(true);
                if (_oldTable == Table.Table1)
                    DisplayInfo(false);
                else DisplayInfo(true);
                break;
            case Table.Table2:
                FastOpenManager.Instance.CleraModelCamera();
                _fun2.SetActive(true);
                DisplayTequan();
                break;
            case Table.Table3:
                FastOpenManager.Instance.CleraModelCamera();
                _fun2.SetActive(true);
                DisplayAttribute();
                break;
            default:
                break;
        }
        DisplayTitle();
        _oldTable = table;
    }

    //显示Table1福利
    public void DisplayInfo(bool displayMode)
    {
        DisplayModel(displayMode);
        DisplayItems();
        DisplayTips();
    }
    //显示Left
    private void DisplayModel(bool displayMode)
    {
        if (displayMode)
        {
            NPCManager.Instance.createCamera(false);
            if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
            {
                BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, VipManager.Instance.ShowVip.ModelPath,
                    (asset) =>
                    {                  
                        GameObject model = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
                        model.name = "model";
                        model.transform.parent = NPCManager.Instance.ModelCamera.transform;
                        model.transform.localPosition = VipManager.Instance.ShowVip.ModelPosition;
                        model.transform.localScale = Vector3.one;
                        model.transform.rotation = Quaternion.Euler(VipManager.Instance.ShowVip.ModelRotation);
                        ToolFunc.SetLayerRecursively(model, LayerMask.NameToLayer("TopUI"));	
                    });
            }
            else
            {
                GameObject asset = BundleMemManager.Instance.getPrefabByName(VipManager.Instance.ShowVip.ModelPath, EBundleType.eBundleUIEffect);
                GameObject model = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
                model.name = "model";
                model.transform.parent = NPCManager.Instance.ModelCamera.transform;
                model.transform.localPosition = VipManager.Instance.ShowVip.ModelPosition;
                model.transform.localScale = Vector3.one;
                model.transform.rotation = Quaternion.Euler(VipManager.Instance.ShowVip.ModelRotation);
                ToolFunc.SetLayerRecursively(model, LayerMask.NameToLayer("TopUI"));	
            }
        }
        //显示模型
        
    }

    //显示Top
    private void DisplayItems()
    {
        _itemsLabel.text = VipManager.FormatItemLabel();

        ViewHelper.FormatTemplate<BetterList<ItemCount>, ItemCount, ItemLabel>
            (_itemPrefab, _itemGrid.transform, VipManager.Instance.ShowVip.LootItem.AwardItems,
            (ItemCount vo, ItemLabel d) =>
            {
                d.QualityNoBoder = vo.Item.quality;
                d.Icon = vo.Item.icon;
                d.Lable = vo.Count.ToString();
            }
            );
        _itemGrid.Reposition();

        //显示领取状态
        _btnReceive.SetActive(false);
        _tipObj.SetActive(false);
        switch (VipManager.Instance.ShowVip.IsReceive)
        {
            case VipState.None:
                _tipObj.SetActive(true);
                _tipLabel.text = LanguageManager.GetText("vip_tip1");
                _tipLabelLevel.text = VipManager.Instance.ShowVip.VipId.ToString();
                break;
            case VipState.Receiveed:
                _tipObj.SetActive(true);
                _tipLabel.text = LanguageManager.GetText("vip_tip2");
                _tipLabelLevel.text = VipManager.Instance.ShowVip.VipId.ToString();
                break;
            case VipState.CanReveive:
                _btnReceive.SetActive(true);
                break;
            default:
                break;
        }

    }

    //显示Center
    private void DisplayTips()
    {
        VipVo vo= VipManager.Instance.ShowVip;
        for (int i = 0; i < _imgs.Length; i++)
        {
            VipViewDisplayImg d=_imgs[i];
            VipValue v = vo.VipPictures[i];
            switch (v.TipType)
            {
                case VipPicType.None:
                    break;
                case VipPicType.Pic:
                    d.DisplayIcon(v.TextureName);
                    d.DisplayPower();
                    break;
                case VipPicType.Number:
                    d.DisplayIcon(v.TextureName);
                    d.DisplayPower(vo.VipPower);
                    break;
                default:
                    break;
            }
            d.DisplayTip();
        }
        _selectVip.text = VipManager.Instance.ShowVip.VipId.ToString();
    }


    //显示顶部VIP列表
    private void DisplayTitleList()
    {
        ViewHelper.FormatTemplate<BetterList<int>, int>(_vipTitlePrefab, _vipTitleGrid.transform,
            VipManager.Instance.TitleVips,
            (int vo, Transform t) => 
            {
                t.Find("Label").GetComponent<UILabel>().text = "VIP" + vo;
            });
        _vipTitleGrid.Reposition();
    }



    //显示特权对比
    public void DisplayAttribute()
    {
        DisplayTitleList();
        bool isactive = false;
        ViewHelper.FormatTemplate<BetterList<AttributeKeyValue>, AttributeKeyValue, VipViewDisplayAttribute>
            (_vipPrefab, _vipGrid.transform, VipManager.Instance.Attributes,
            (AttributeKeyValue vo, VipViewDisplayAttribute d) =>
            {
                //显示左边名称
                d.DisplayTitle(ColorConst.Format(ColorConst.Color_HeSe, EquipmentManager.GetEquipAttributeName(vo.Type)));
                for (int i = 1; i < vo.Values.size; i++)
                {
                    string s = PowerManager.Instance.ChangeInfoData(vo.Type, vo.Values[i]);
                    d.DisplayLabel(i-1, VipManager.FormatAttribute(s));
                }
                d.ShowLight(isactive);
                isactive = !isactive;
            });
        _vipGrid.Reposition();
        _vipView.ResetPosition();
    }

    public void DisplayTequan()
    {
        bool isactive = false;
        DisplayTitleList();
        ViewHelper.FormatTemplate<BetterList<PrivilegeVo>, PrivilegeVo, VipViewDisplayAttribute>
            (_vipPrefab, _vipGrid.transform, VipManager.Instance.DisplayList,
            (PrivilegeVo vo, VipViewDisplayAttribute d) =>
            {
                d.DisplayTitle(ColorConst.Format(ColorConst.Color_HeSe,vo.Disction));

                for (int i = 1; i < VipManager.MAX_VIP_LEVEL+1; i++)
                {
                    Privilege vv = VipManager.Instance.FindVoById(i).Privileges[vo.Key];
                    switch (vv.Type)
                    {

                        case SuffixType.Percent:
                            d.DisplayLabel(i-1,ColorConst.Format(ColorConst.Color_HeSe,vv.GetInt(),"%"));
                            break;
                        case SuffixType.Boolean:
                            d.DisplaySprite(i-1,vv.GetBoolean());
                            break;
                        case SuffixType.Add:
                            d.DisplayLabel(i - 1, ColorConst.Format(ColorConst.Color_HeSe, "+", vv.GetInt()));
                            break;
                        case SuffixType.Day:
                            d.DisplayLabel(i - 1, ColorConst.Format(ColorConst.Color_HeSe, vv.GetInt(), LanguageManager.GetText("vip_day")));
                            break;
                        case SuffixType.Count:
                            d.DisplayLabel(i - 1, ColorConst.Format(ColorConst.Color_HeSe, vv.GetInt(), LanguageManager.GetText("vip_count")));
                            break;
                        case SuffixType.Second:
                            d.DisplayLabel(i - 1, ColorConst.Format(ColorConst.Color_HeSe, vv.GetInt(), LanguageManager.GetText("vip_sceond")));
                            break;
                        case SuffixType.Number:
                            d.DisplayLabel(i - 1, ColorConst.Format(ColorConst.Color_HeSe, vv.GetInt()));
                            break;
                        default:
                            break;
                    }            
                }
                d.ShowLight(isactive);
                isactive = !isactive;
            });
        _vipGrid.Reposition();
        _vipView.ResetPosition();
    }
}
