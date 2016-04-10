using UnityEngine;
using System.Collections;
using manager;
using MVC.entrance.gate;
using model;

/// <summary>
/// 提示位置绑定
/// </summary>
public class TipBind : MonoBehaviour
{

    public int Id = 0;

    public bool IsTrigger = true;       
        
    public bool AutoBind = false;       //是否自动绑定ID

    public bool AutoBagId = false;      //自动绑定背包ID

    public bool AutoSweep = false;      //自动绑定打开关卡按钮

    public Vector2 Size = new Vector2(2, 2);

    public GuideVo Bind { get; set; }     //绑定信息

    private GuideVo vo;
    
    private GameObject _pre;
    private GameObject _tip;        //显示的TIP

    private bool isStart;
    private void Awake()
    {
        isStart = false;
    }
    private void Start()
    {
        if (AutoSweep)
        {
            Id = SweepManager.Instance.CurrentMap.id*10;
        }

        if (AutoBagId)
        {
            //外部程序实现自动绑定
            return;
        }
        if (AutoBind)
        {
            if (int.TryParse(gameObject.name, out Id))
            {

            }
            else {
                Debug.LogError("AutoBind Faild");
            }
        }
        
            
    }

    


    private void Update()
    {
        //当前tip激活，查询是否有符合条件的数据
        vo = GuideManager.Instance.FindDisplayVo(Id);

        //数据不为空，则显示
        if (vo != null)
        {
            //判断当前是否需要关闭UI才能触发
            if (vo.CloseUI)
            {
                //UI都关闭的情况下，开始激活
                if (UIManager.Instance.IsHaveWindow)
                {
                    HideTip();
                    return;
                }
            }
            //标志位，如果为真，则显示过了
            if (IsStart)
            {

            }
            else {
                //获取到锁
                if (GuideManager.Instance.TriggerLock(this))
                {
                    ShowTip();
                }
            }
        }
        else {
            if (IsStart)
            {
                HideTip();
            }
            
        }
    }

    //被顶掉
    public void OnHook()
    {
        HideTip();
    }

    public bool IsStart
    {
        get { return isStart; }
    }
    private void ShowTip()
    {

        isStart = true;
        //全模锁UI事件
        if (vo==null)
        {
            Debug.Log("");
        }
        if (vo.Enforce)
        {
            GuideManager.Instance.EnforceTagId = gameObject.GetInstanceID();
        }
        else
        {
            GuideManager.Instance.EnforceTagId = -1;
        }
        GuideManager.Instance.IsEnforceUI = vo.Enforce;

        //锁背包滑动
        if (AutoBagId)
        {
            GetComponent<UIDragScrollView>().enabled = false;
        }

        //设置当前绑定信息
        Bind = vo;

        _pre = UIManager.Instance.getRootTrans().Find("TopCamera/ui_guide/Point").gameObject;

        _pre.SetActive(true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(_pre);
        obj.transform.parent = transform.parent;
        obj.transform.name = "Tip_" + Id.ToString();
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        ToolFunc.SetLayerRecursively(obj, gameObject.layer);
        TipAnchor anchor = obj.transform.GetComponent<TipAnchor>();
        anchor.SetBindData(this, vo, vo.Enforce);

        _pre.SetActive(false);
    }

    private void HideTip()
    {
        isStart = false;
        
        Transform t = transform.parent.Find("Tip_" + Id.ToString());
        if (t != null)
        {
            //尝试解锁
            GuideManager.Instance.TriggerUnLock(this);

            Bind = null;
            vo = null;
            if (AutoBagId)
            {
                GetComponent<UIDragScrollView>().enabled = true;
            }

            //如果当前为全模，则解锁
            if (GuideManager.Instance.IsEnforceUI)
            {
                GuideManager.Instance.IsEnforceUI = false;
                GuideManager.Instance.EnforceTagId = -1;
            }
            DestroyObject(t.gameObject);
        }
    }

    private void OnClick()
    {
        //当当前激活，才发消息
        if (IsTrigger&&IsStart)
        {
            Gate.instance.sendNotification(MsgConstant.MSG_GUIDE_SEND_TRIGGER,
         new Trigger(model.TriggerType.ButtonClick, Id.ToString()));

            //if (vo != null && vo.CloseUI)
            //{
            //    Gate.instance.sendNotification(MsgConstant.MSG_MAIN_CLOSE_MENU);
            //    NPCManager.Instance.createCamera(false);
            //    UIManager.Instance.closeAllUI();
            //}   
        }
    }

    private void OnEnable()
    {
        GuideManager.Instance.AddTipBind(this);
    }

    private void OnDisable()
    {
        GuideManager.Instance.RemoveTipBind(this);
        HideTip();
    }
    private void OnDestroy()
    {
        HideTip();
    }

    #region Show
    
   
#if UNITY_EDITOR
    public Vector3 TextPos
    {
        get
        {
            Vector3 old = transform.position;
            transform.localPosition = transform.localPosition + new Vector3(Size.x, Size.y, 0);
            Vector3 tempSize = transform.position - old;
            transform.localPosition = transform.localPosition - new Vector3(Size.x, Size.y, 0);
            return transform.position + new Vector3(-tempSize.x / 2, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 old = transform.position;
        transform.localPosition = transform.localPosition + new Vector3(Size.x, Size.y, 0);
        Vector3 tempSize = transform.position - old;
        transform.localPosition = transform.localPosition - new Vector3(Size.x, Size.y, 0);
        DrawRect(transform.position, new Vector2(tempSize.x, tempSize.y));

    }

    private void DrawRect(Vector3 pos, Vector2 size)
    {
        size /= 2;
        Vector3[] vs = new Vector3[5];
        vs[0] = new Vector3(-size.x, size.y);
        vs[1] = new Vector3(size.x, size.y);
        vs[2] = new Vector3(size.x, -size.y);
        vs[3] = new Vector3(-size.x, -size.y);
        vs[4] = new Vector3(-size.x, size.y);
        Gizmos.color = Color.yellow;
        for (int i = 0; i < vs.Length - 1; i++)
        {
            Gizmos.DrawLine(pos + vs[i], pos + vs[i + 1]);
        }
    }
    #endif
    #endregion
}
