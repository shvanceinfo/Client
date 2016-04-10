using UnityEngine;
using System.Collections;
using manager;
using MVC.entrance.gate;
using mediator;
using helper;
using model;

public class FormulaView : MonoBehaviour {


    private UIScrollView _panel;
    private UIGrid _grid;
    private GameObject _prefab;
    private GameObject _description;

    //详细信息
    private ItemHelper _item;
    private GameObject _btn_Active; //可用按钮
    private GameObject _btn_Disable;//不可用按钮
    private UILabel _name;          //名字
    private UILabel _sucess;        //成功率
    private ItemLabel[] _consumeItem;
    private UIInput _input;
	private BtnTipsMsg _topIcon;
    private void Awake()
    {
        _panel = transform.FindChild("Formula/ItemPanel").GetComponent<UIScrollView>();
        _grid = transform.FindChild("Formula/ItemPanel/Grid").GetComponent<UIGrid>();
        _prefab = transform.FindChild("Formula/ItemPanel/Item").gameObject;
        _description = transform.FindChild("Formula/Description").gameObject;

        _item = _description.transform.FindChild("Top/Item").GetComponent<ItemHelper>();
		_topIcon = _description.transform.FindChild("Top/Item/Icon").GetComponent<BtnTipsMsg>();
        _btn_Active = _description.transform.FindChild("Top/Button_Select").gameObject;
        _btn_Disable = _description.transform.FindChild("Top/Button_Normal").gameObject;
        _name = _description.transform.FindChild("Top/Name").GetComponent<UILabel>();
        _sucess = _description.transform.FindChild("Top/prenum").GetComponent<UILabel>();

        _consumeItem = new ItemLabel[MergeManager.MAX_CONSUME_ITEM];
        int i = 0;
        _consumeItem[i++] = _description.transform.FindChild("Center/Item1").GetComponent<ItemLabel>();
        _consumeItem[i++] = _description.transform.FindChild("Center/Item2").GetComponent<ItemLabel>();
        _consumeItem[i++] = _description.transform.FindChild("Center/Item3").GetComponent<ItemLabel>();
        _consumeItem[i++] = _description.transform.FindChild("Center/Item4").GetComponent<ItemLabel>();
        _consumeItem[i++] = _description.transform.FindChild("Center/Item5").GetComponent<ItemLabel>();

        _input = _description.transform.FindChild("Buttom/Input").GetComponent<UIInput>();
    }

    private void OnEnable()
    {
        Gate.instance.registerMediator(new FormulaMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.FORMULA_MEDIATOR);
    }

    /// <summary>
    /// 读取道具列表
    /// </summary>
    public void DisplayFormulaList()
    {
        ViewHelper.FormatTemplate<BetterList<FormulaVo>, FormulaVo, MergeDisplayItem>(_prefab,
            _grid.transform, FormulaManager.Instance.SortVo,
            (FormulaVo vo, MergeDisplayItem item) =>
            {
				item.BindingTipsData((uint)vo.MergeNextId);
                item.Display(vo.Id, MergeManager.FindItem(vo.Id).icon,
                    MergeManager.FindItem(vo.Id).name,
                    vo.CanMergeCount,
                MergeManager.FindQualityByGemId(vo.Id));
            });

        if (FormulaManager.Instance.SortVo.size <= 0)
        {
            transform.FindChild("Label").gameObject.SetActive(true);
        }
        else
        {
            transform.FindChild("Label").gameObject.SetActive(false);
        }

        _panel.ResetPosition();
        _grid.Reposition();
    }
    public void SelectIndex()
    {
        Transform t = _grid.transform.FindChild("0");
        if (t != null)
        {
            _grid.transform.FindChild("0").GetComponent<UIToggle>().value = true;
            Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_SELECT_LIST_ITEM, _grid.transform.FindChild("0").GetComponent<MergeDisplayItem>().Id);
        }
        else {
            Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_SELECT_LIST_ITEM, 0);
        }
    }

    public void DisplayFormulaInfo()
    {
        FormulaVo vo = FormulaManager.Instance.SelectVo;
        if (vo == null)
        {
            _description.SetActive(false);      //如果没有选中的物品隐藏UI显示
        }
        else
        {
            _description.SetActive(true);

            _input.value = FormulaManager.Instance.SelectCount.ToString();
            //判断是否可以使用幸运石
            _btn_Active.SetActive(vo.IsUsedLuckStone);
            //_btn_Disable.SetActive(!vo.IsUsedLuckStone);
            _item.Id = vo.MergeNextId;
            _item.Icon = MergeManager.FindItem(vo.MergeNextId).icon;
            _item.Quality = MergeManager.FindItem(vo.MergeNextId).quality;
			_topIcon.Iteminfo = new ItemInfo((uint)vo.MergeNextId,0,0);
			
            _name.text = MergeManager.FindItem(vo.MergeNextId).name;
            _sucess.text = FormulaManager.Instance.GetSucess();

            int num = FormulaManager.Instance.SelectCount;
            int haveCount = MergeManager.Instance.GetNumById(vo.ConsumeItem[0].Id);
            int needCount = vo.ConsumeItem[0].Value * num;
            int needGold = vo.ConsumeGold * num;
            string lbl = "";
            if (haveCount >= needCount )
            {
                lbl = ColorConst.Format(ColorConst.Color_Green, string.Format("{0}/{1}", haveCount, needCount ));
            }
            else
            {
                lbl = ColorConst.Format(ColorConst.Color_Red, string.Format("{0}/{1}", haveCount, needCount));
            }
            _consumeItem[0].Icon = MergeManager.FindItem(vo.ConsumeItem[0].Id).icon;
            _consumeItem[0].Quality = MergeManager.FindItem(vo.ConsumeItem[0].Id).quality;
            _consumeItem[0].Lable = lbl;
			_consumeItem[0].transform.FindChild("Icon").GetComponent<BtnTipsMsg>().Iteminfo = new ItemInfo((uint)vo.ConsumeItem[0].Id,0,0);

            if (CharacterPlayer.character_asset.gold >= needGold)
            {
                lbl = ColorConst.Format(ColorConst.Color_Green, needGold);
            }
            else
            {
                lbl = ColorConst.Format(ColorConst.Color_Red, needGold);
            }
            _consumeItem[1].Icon = "gold";
            _consumeItem[1].Quality = eItemQuality.ePurple;
            _consumeItem[1].Lable = lbl;


            if (LuckStoneManager.Instance.SelectedStone != null)
            {

                if (LuckStoneManager.Instance.IsHaveItem(num))
                {
                    ItemTemplate tt = MergeManager.FindItem(LuckStoneManager.Instance.SelectedStone.ConsumeItem[0].Id);
                    _consumeItem[2].Icon = tt.icon;
                    _consumeItem[2].Quality = tt.quality;
                    _consumeItem[2].Lable = ColorConst.Format(ColorConst.Color_Green, string.Format("{0}/{1}", LuckStoneManager.Instance.GetSelectCount(),
                    LuckStoneManager.Instance.SelectedStone.ConsumeItem[0].Value * num));
					_consumeItem[2].transform.FindChild("Icon").GetComponent<BtnTipsMsg>().Iteminfo = new ItemInfo((uint)tt.id,0,0);
                }
                else
                {
                    if (CharacterPlayer.character_asset.diamond >= LuckStoneManager.Instance.SelectedStone.ConsumeDiamond * num)
                    {
                        lbl = ColorConst.Format(ColorConst.Color_Green, LuckStoneManager.Instance.SelectedStone.ConsumeDiamond * num);
                    }
                    else
                    {
                        lbl = ColorConst.Format(ColorConst.Color_Red, LuckStoneManager.Instance.SelectedStone.ConsumeDiamond * num);
                    }
                    _consumeItem[2].Icon = "diamond";
                    _consumeItem[2].Quality = eItemQuality.eOrange;
                    _consumeItem[2].Lable = lbl;
                }
            }
            else
            {
                _consumeItem[2].Quality = eItemQuality.eGreen;
                _consumeItem[2].Lable = null;
                _consumeItem[2].Icon = null;
            }
            _consumeItem[3].Quality = eItemQuality.eGreen;
            _consumeItem[3].Lable = null;
            _consumeItem[3].Icon = null;

            _consumeItem[4].Quality = eItemQuality.eGreen;
            _consumeItem[4].Lable = null;
            _consumeItem[4].Icon = null;

        }
    }
}
