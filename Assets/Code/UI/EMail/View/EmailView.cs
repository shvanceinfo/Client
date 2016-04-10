using UnityEngine;
using System.Collections;
using manager;
using model;
using MVC.entrance.gate;
using mediator;

public class EmailView : MonoBehaviour {

    const string Get = "领 取";
    const string Delete = "删 除";

    private UILabel _emailCount;


    private Transform _grid;
    private GameObject _prefab;


    private Transform _awardGrid;
    private GameObject _awardPrefab;       
    private GameObject _awardObj;       //奖励组件,如果没有奖励将隐藏

    private GameObject _descrption;

    private UILabel _buttonLbl;         //按钮文本
    private UILabel _title;             //标题
    private UILabel _content;           //内容

    private void Awake()
    {
        _descrption = transform.FindChild("Descrption").gameObject;
        _emailCount = transform.FindChild("EmailList/Desc/Count").GetComponent<UILabel>();
        _grid = transform.FindChild("EmailList/Panel/Grid");
        _prefab = transform.FindChild("EmailList/Panel/Item").gameObject;

        _awardGrid = transform.FindChild("Descrption/AwardPanel/Panel/Grid");
        _awardPrefab = transform.FindChild("Descrption/AwardPanel/Panel/Item").gameObject;
        _awardObj = transform.FindChild("Descrption/AwardPanel").gameObject;

        _buttonLbl = transform.FindChild("Descrption/Button/Label").GetComponent<UILabel>();
        _title = transform.FindChild("Descrption/HaveItem/Title").GetComponent<UILabel>();
        _content = transform.FindChild("Descrption/HaveItem/Context").GetComponent<UILabel>();

    }

    private void Start()
    {
        SetInfo(false);
        DisplayEmailCount();
        DisplayEmailList();
        Gate.instance.sendNotification(MsgConstant.MSG_SELECT_INDEX_EMAIL);
    }

    private void OnEnable()
    {
        Gate.instance.registerMediator(new EmailMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.EMAIL_MEDIATOR);
    }

    public void SelectIndex()
    {
        if (_grid.childCount != 0)
        {
            _grid.GetChild(0).GetComponent<UIToggle>().value = true;
            Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_READ_EMAIL,
                _grid.GetChild(0).GetComponent<EmailDisplayMail>().Id);
        }
        else {
            SetInfo(false);
        }
    }

    /// <summary>
    /// 显示邮件数量
    /// </summary>
    public void DisplayEmailCount()
    {
        _emailCount.text = string.Format("{0}/{1}", EmailManager.Instance.NotReadCount, EmailManager.Instance.EmailCount);
    }

    /// <summary>
    /// 显示邮件列表
    /// </summary>
    public void DisplayEmailList()
    {
        BetterList<EmailVo> emails = EmailManager.Instance.Emails;
        int childCount = _grid.childCount;
        int size = emails.size;
        if (childCount<size)
        {
            for (int i = childCount; i < emails.size; i++)
            {
                helper.ViewHelper.AddItemTemplatePrefab(_prefab, _grid, i);
            }
        }
        if (childCount>size)
        {
            for (int i = size; i < childCount; i++)
            {
                helper.ViewHelper.DeleteItemTemplate( _grid, i);
            }
        }
        

        for (int i = 0; i < emails.size; i++)
        {
            DisplayEmail(emails[i], i);
        }
        _grid.GetComponent<UIGrid>().Reposition();

    }

    private void DisplayEmail(EmailVo vo,int index)
    {
        Transform t = _grid.FindChild(index.ToString());
        if (t != null)
        {
            EmailDisplayMail dm = t.GetComponent<EmailDisplayMail>();
            dm.Display(vo.Id, vo.Title, vo.State, vo.IsHaveAward);
        }
        else {
            Debug.LogError("not fond transform");
        }
    }



    public void DisplayEmailInfo()
    {
        SetInfo(true);
        EmailVo vo = EmailManager.Instance.SelectMail;

        _title.text = vo.Title;
        _content.text = vo.Content;

        if (vo.IsHaveAward)
        {
            _awardObj.SetActive(true);
            int childCount = _awardGrid.childCount;
            int awardCount = vo.AwardItems.size;
            if (childCount<awardCount)
            {
                for (int i = childCount; i < awardCount; i++)
                {
                    helper.ViewHelper.AddItemTemplatePrefab(_awardPrefab, _awardGrid, i);
                }
            }
            if (childCount>awardCount)
            {
                for (int i = awardCount; i < childCount; i++)
                {
                    helper.ViewHelper.DeleteItemTemplate(_awardGrid, i);
                }
            }

            for (int i = 0; i < awardCount; i++)
            {
                DisplayItem(vo.AwardItems[i],i);
            }

            _buttonLbl.text = Get;
        }
        else {
            _buttonLbl.text = Delete;
            _awardObj.SetActive(false);

        }
    }

    private void DisplayItem(IdStruct vo, int index)
    {
        Transform t = _awardGrid.Find(index.ToString());
        if (t!=null)
        {
            string icon = DemonManager.Instance.FindIcon(vo.Id);
            string boder = DemonManager.Instance.FindBoderById(vo.Id);
            EmailDisplayAward award=t.GetComponent<EmailDisplayAward>();
            award.Display(icon, boder, vo.Value.ToString());
        }
    }

    //显示或隐藏，邮件详细信息
    public void SetInfo(bool isactive)
    {
        _descrption.SetActive(isactive);
    }
}
