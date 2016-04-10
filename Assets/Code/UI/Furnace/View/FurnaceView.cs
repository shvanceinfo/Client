using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using helper;

/// <summary>
/// 熔炉功能，负责管理子集功能，方便以后分离功能
/// </summary>
public class FurnaceView : MonoBehaviour
{



    //private GameObject obj1;
    //private GameObject obj2;

    private GameObject _merge_Gem;
    private GameObject _merge_Formula;
    private Table _level1Table; //一级标签
    private Table _mergeTable;  //合成分页标签
    //private UICheckBoxColor _cbk1;
    private void Awake()
    {
        //_cbk1 = transform.FindChild("Table/Table1").GetComponent<UICheckBoxColor>();

        //obj1 = transform.FindChild("Merge").gameObject;
        //obj2 = transform.FindChild("Badge").gameObject;

        _merge_Gem = transform.FindChild("Merge/Gem").gameObject;
        _merge_Formula = transform.FindChild("Merge/Formula").gameObject;
    }
    private void Start()
    {
        _level1Table = Table.None;
        _mergeTable = Table.None;
//        Gate.instance.sendNotification(MsgConstant.MSG_FURNACE_SWING_TABLE, Table.Table1);
    }
    private void OnEnable()
    {
        Gate.instance.registerMediator(new FurnaceMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.FURNACE_MEDIATOR);
    }


    public void SwitchingLevel1Table(Table table)
    {
        if (_level1Table==table)
        {
            return;
        }
        _level1Table = table;
        //obj1.SetActive(false);
        //obj2.SetActive(false);

        switch (table)
        {
            case Table.None:
                break;
            case Table.Table1:
                //obj1.SetActive(true);
                SwitchingMergeTable(Table.Table1);
                break;
            case Table.Table3:
                //obj2.SetActive(true);
                MedalManager.Instance.OnTableClick();
                break;
            default:
                break;
        }
    }
    public void SwitchingMergeTable(Table table)
    {
        if (table==_mergeTable)
        {
            return;
        }
        _mergeTable = table;
        _merge_Gem.SetActive(false);
        _merge_Formula.SetActive(false);
        switch (table)
        {
            case Table.None:
                break;
            case Table.Table1:
                _merge_Gem.SetActive(true);
                MergeManager.Instance.Initial();
                break;
            case Table.Table2:
                _merge_Formula.SetActive(true);
                FormulaManager.Instance.Initial();
                break;
            default:
                break;
        }
    }

    public void DisplayGem()
    {
        //_cbk1.isChecked = true;
        //_cbk2.isChecked = false;
        _level1Table = Table.Table1;
        //obj1.SetActive(true);
        //obj2.SetActive(false);
        _mergeTable = Table.Table1;
        _merge_Formula.SetActive(false);
        _merge_Gem.SetActive(true);
        MergeManager.Instance.Initial();
    }

    public void DisplayFormula()
    {
        //_cbk1.isChecked = true;
        //_cbk2.isChecked = false;
        _level1Table = Table.Table2;
        //obj1.SetActive(true);
        //obj2.SetActive(false);
        _mergeTable = Table.Table2;
        _merge_Formula.SetActive(true);
        _merge_Gem.SetActive(false);
        FormulaManager.Instance.Initial();
    }
}
