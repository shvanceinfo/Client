using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using helper;

public class SettingView : MonoBehaviour {

    private GameObject table1;
    private GameObject table2;
    private GameObject table3;
    private GameObject table4;

    //post
    private UILabel _postAuthor;
    private UILabel _postContent;

    private GameObject _leftPrefab;
    private Transform _grid;
    private UICenterOnChild _center;
    private UIScrollView _leftView;

    private GameObject _rightPrefab;
    private UIGrid _rightGrid;
    private UIScrollView _rightView;
    private UILabel _lblTitle;

    private UISwtiching _audio;
    private UISwtiching _music;

    private GameObject buttonUp, buttonDown;

    private GameObject _preOpt;
    private UIGrid _gridOpt;

    private void Awake()
    {
        table1 = transform.FindChild("Post").gameObject;
        table2 = transform.FindChild("Question").gameObject;
        table3 = transform.FindChild("Help").gameObject;
        table4 = transform.FindChild("Setting").gameObject;

        _postAuthor = transform.FindChild("Post/Background/Author").GetComponent<UILabel>();
        _postContent = transform.FindChild("Post/Background/Content").GetComponent<UILabel>();

        //左边列表
        _leftPrefab = transform.FindChild("Help/Panel/Item").gameObject;
        _leftPrefab.SetActive(false);
        _grid = transform.FindChild("Help/Panel/Grid");
        _leftView = transform.FindChild("Help/Panel").GetComponent<UIScrollView>();

        _center = _grid.GetComponent<UICenterOnChild>();
        _center.onFinished = Finished;


        buttonUp = transform.FindChild("Help/Button_Up").gameObject;
        buttonDown = transform.FindChild("Help/Button_Down").gameObject;

        //右边列表
        _lblTitle = transform.FindChild("Help/Title").GetComponent<UILabel>();
        _rightView = transform.FindChild("Help/PanelContent").GetComponent<UIScrollView>();
        _rightGrid = _rightView.transform.FindChild("Grid").GetComponent<UIGrid>();
        _rightPrefab = _rightView.transform.FindChild("Item").gameObject;
        _rightPrefab.SetActive(false);


        _audio = transform.FindChild("Setting/MusicEx/Button_MusicEx").GetComponent<UISwtiching>();
        _music = transform.FindChild("Setting/Music/Button_Music").GetComponent<UISwtiching>();

        _preOpt = transform.FindChild("Setting/PeopleNum/Panle/item").gameObject;
        _gridOpt = transform.FindChild("Setting/PeopleNum/Panle/grid").GetComponent<UIGrid>();
    }
    
    private void OnEnable()
    {
        Gate.instance.registerMediator(new SettingMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.SETTING_MEDIATOR);
    }

    private void Start()
    {
        _audio.Initial(AudioManager.Instance.AudioActive);
        _music.Initial(AudioManager.Instance.MusicActive);
        DisplayOptions();
        Swtiching(Table.Table1);
        DisplayLeftList();
    }

    public void Swtiching(Table table)
    {
        table1.SetActive(false);
        table2.SetActive(false);
        table3.SetActive(false);
        table4.SetActive(false);
        switch (table)
        {
            case Table.None:
                break;
            case Table.Table1: table1.SetActive(true);
                DisplayPost();
                break;
            case Table.Table2: table2.SetActive(true);
                break;
            case Table.Table3: table3.SetActive(true);
                
                break;
            case Table.Table4: table4.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void DisplayOptions()
    {
        SettingVo vo = SettingManager.Instance.CurDisplayVo;
        ViewHelper.FormatTemplate<List<int>, int>(_preOpt, _gridOpt.transform, vo.Options,
            (int peoplecount, Transform t) =>
            {
                UILabel lbl = t.F<UILabel>("Label");
                UIToggle toggle = t.GetComponent<UIToggle>();
                if (int.Parse(t.name) == vo.CurOption)
                {
                    toggle.value = true;
                }
                else {
                    toggle.value = false;
                }
                if (peoplecount == 0)
                {
                    lbl.text = ViewHelper.FormatLanguage("setting_hideall");
                }
                else {
                    lbl.text = ViewHelper.FormatLanguage("setting_people",peoplecount);
                }
            });
    }

    //显示公告
    public void DisplayPost()
    {
        //PostVo vo=SettingManager.Instance.GetSystemPostVo();
       // _postContent.text = vo.Content;
        //_postAuthor.text = vo.Author;
    }

    //显示帮助左边列表
    public void DisplayLeftList()
    {
        ViewHelper.FormatTemplate<BetterList<HelpInfo>, HelpInfo, SettingDisplayItem>(_leftPrefab, _grid, SettingManager.Instance.Helps,
            (HelpInfo vo,SettingDisplayItem d) =>
            {
                d.Display(vo.helpID, vo.helpTitle);
            }
            );
        _grid.GetComponent<UIGrid>().Reposition();
        _grid.parent.gameObject.GetComponent<UIScrollView>().ResetPosition();
        _center.Recenter();
    }

    private void ResetColor()
    {
        for (int i = 0; i < _grid.childCount; i++)
        {
            _grid.GetChild(i).GetComponent<SettingDisplayItem>().NormalColor();
        }
    }
    //坐标列表选择事件完成
    private void Finished()
    {
        ResetColor();

        SettingDisplayItem sd = _center.centeredObject.GetComponent<SettingDisplayItem>();
        HelpInfo vo = SettingManager.Instance.GetHelpVoById(sd.Id);
        sd.SelectColor();
        sd.SelectArrowShow(buttonUp,buttonDown);
        _lblTitle.text = vo.helpTitle;

        int ct=_rightGrid.transform.childCount;
        if (ct<vo.helpContent.size)
        {
            for (int i = ct; i < vo.helpContent.size; i++)
            {
                ViewHelper.AddItemTemplatePrefab(_rightPrefab, _rightGrid.transform, i);

            }
        }

        ct = _rightGrid.transform.childCount;
        for (int i = 0; i < ct; i++)
        {
            if (i < vo.helpContent.size)
            {
                AddContext(vo.helpContent[i], i);
            }
            else {
                _rightGrid.transform.FindChild(i.ToString()).gameObject.SetActive(false);
            }
            
        }
        _rightGrid.Reposition();
        _rightView.ResetPosition();

    }
    private void AddContext(string vo,int index)
    {
        Transform t=_rightGrid.transform.FindChild(index.ToString());
        
        if (t!=null)
        {
            t.gameObject.SetActive(true);
            UILabel lbl = t.FindChild("Label").GetComponent<UILabel>();
            lbl.text = vo;
        }
    }

    public void MoveSelect(bool next)
    {
        if (next) MoveDown();
        else MoveUp();
    }
    private void MoveUp()
    {
        int index = int.Parse(_center.centeredObject.name);
        index = index - 1;
        if (index >= 0 && index < _center.transform.childCount)
        {
            Transform t = _center.transform.GetChild(index);
            if (t != null)
            {
                _center.CenterOn(t);
            }
        }        
    }
    private void MoveDown()
    {
        int index = int.Parse(_center.centeredObject.name);
        index = index + 1;
        if (index >= 0 && index < _center.transform.childCount)
        {
            Transform t = _center.transform.GetChild(index);
            if (t != null)
            {
                _center.CenterOn(t);
            }
        }
        

    }

    
}
