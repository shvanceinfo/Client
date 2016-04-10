using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using manager;
using model;
using MVC.entrance.gate;
using mediator;
using helper;

public class PowerView : MonoBehaviour {

    private const int MAXSIZE = 6;  //最大显示6位数

    private GameObject _pwoerObj;

    private GameObject _background;

    private UIGrid[] _grids;

    private List<List<int>> _nums = new List<List<int>>();

    private GameObject prefab;

    private GameObject _lblPanel;
    private GameObject _lblPrefab;
    private void Awake()
    {
        _pwoerObj = transform.FindChild("PowerPanel").gameObject;
        _background = transform.FindChild("background").gameObject;
        prefab = transform.FindChild("PowerPanel/Num").gameObject;

        _grids = new UIGrid[MAXSIZE];
        for (int i = 0; i < MAXSIZE; i++)
        {
            _grids[i] = transform.FindChild("PowerPanel/"+i+"/Grid").GetComponent<UIGrid>();
        }

        _lblPanel = transform.FindChild("LabelPanel").gameObject;
        _lblPrefab = transform.FindChild("LabelPanel/Label").gameObject;
        _lblPrefab.SetActive(false);
    }
    private void Start()
    {
        _pwoerObj.SetActive(PowerManager.Instance.IsShowPower);
        _background.SetActive(PowerManager.Instance.IsShowPower);
    }
    private void OnEnable()
    {
        Gate.instance.registerMediator(new PowerMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.POWER_MEDIATOR);
    }


    private void AddPower(int oldPower, int curPower,bool isUp)
    {
        _nums.Clear();
        int size = curPower.ToString().Length;  //位数长度

        string a = oldPower.ToString().PadLeft(size, '0');
        string b = curPower.ToString();

        int[] olds = new int[size];
        int[] curs = new int[size];
        char[] ops = a.ToCharArray();
        char[] cps = b.ToCharArray();

        for (int i = 0; i < size; i++)
        {
            // int tmp = GetNum(ops[i]);
            olds[i] = int.Parse(ops[i].ToString());
            //  tmp=GetNum(cps[i]);
            curs[i] = int.Parse(cps[i].ToString());
        }
        //
        for (int i = 0; i < MAXSIZE; i++)
        {
            //Vector3 pos = _grids[i].transform.localPosition;
            //pos.y = 0;
            //_grids[i].transform.localPosition = pos;
            if (i < size)
            {
                _grids[i].gameObject.SetActive(true);
            }
            else
            {
                _grids[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < size; i++)
        {
            _nums.Add(GetNunList(olds[i], curs[i], isUp));   
        }
        CreateNums(isUp);
    }

    private void CreateNums(bool isUp)
    {
        for (int i = 0; i < _nums.Count; i++)
        {
            Hidden(_grids[i].transform);
            for (int j = 0; j < _nums[i].Count; j++)
            {
                AddNum(_grids[i].transform, _nums[i][j], j);
            }
            _grids[i].repositionNow = true;
            //_grids[i].Reposition();

            int nextPos=(_nums[i].Count-1) * 40;
            TweenPosition tp = _grids[i].transform.parent.GetComponent<TweenPosition>();
            
            tp.ResetToBeginning();
            if (isUp)
            {
                tp.from.y = 0;
                tp.to.y = nextPos;
                tp.PlayForward();
            }
            else {
                tp.from.y = nextPos;
                tp.to.y = 0;
                tp.PlayForward();
            }
            
        }
        //StartCoroutine(LaterStart());
    }
    private IEnumerator LaterStart()
    {
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < _nums.Count; i++)
        {
            TweenPosition tp = _grids[i].transform.parent.GetComponent<TweenPosition>();
            tp.enabled = true;
            tp.PlayForward();
        }
    }

    private void Hidden(Transform grid)
    {
        for (int i = 0; i < grid.childCount; i++)
        {
            grid.GetChild(i).gameObject.SetActive(false);
        }
    }

    private List<int> GetNunList(int old, int next, bool isUp)
    {
        List<int> list = new List<int>();
        if (old==next)
        {
            list.Add(next);
        }else if(old>next)
        {
            for (int i = old; i < 10; i++)
            {
                list.Add(i);
            }
            for (int i = 0; i <= next; i++)
            {
                list.Add(i);
            }
        }
        else if (old < next)
        {
            for (int i = old; i <= next; i++)
            {
                list.Add(i);
            }
        }

        List<int> dlist = new List<int>();
        if (!isUp)
        {
            for (int i = list.Count-1; i >=0; i--)
            {
                dlist.Add(list[i]);
            }
            return dlist;
        }
        return list;
    }

    private void AddNum(Transform grid, int num,int index)
    {
        GameObject obj = null;
        Transform t= grid.FindChild(index.ToString());
        if (t != null)
        {
            obj = t.gameObject;
            obj.SetActive(true);
        }
        else {
            prefab.SetActive(true);
            obj = BundleMemManager.Instance.instantiateObj(prefab);
            prefab.SetActive(false); 
        }
        obj.transform.parent = grid;
        obj.name = index.ToString();
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.transform.localPosition = new Vector3();
        obj.GetComponent<UISprite>().spriteName = "number_gold_" + num;
        
    }

    public void DisplayPower()
    {
        isStart = true;
        killTime = PowerManager.Instance.Vo.PowerList.Count * _powerTime;
        float lblTime = PowerManager.Instance.Vo.AttibuteList.Count * _lblTime;
        killTime = killTime > lblTime ? killTime : lblTime;
        _curPowerTime = _powerTime;
        //_curlblTime = _lblTime;
    }

    void StartShow()
    {
        _pwoerObj.SetActive(PowerManager.Instance.IsShowPower);
        _background.SetActive(PowerManager.Instance.IsShowPower);
        //不显示战斗力
        if (!PowerManager.Instance.IsShowPower)
            return;
        Queue<PowerNum> qes = PowerManager.Instance.Vo.PowerList;
        if (qes.Count == 0)
        {
           
        }
        else
        {
            lock (PowerManager.Instance.Vo.PowerList)
            {
                PowerNum num = qes.Dequeue();
                AddPower(num.OldPower, num.CurPower, num.IsAddition);
            }
        }

    }


    void StartShowLbl()
    {
        Queue<PowerAttribute> ps = PowerManager.Instance.Vo.AttibuteList;
        if (ps.Count == 0)
        {
            //Gate.instance.sendNotification(MsgConstant.MSG_POWER_CLOSE);
        }
        else
        {
            lock (PowerManager.Instance.Vo.AttibuteList)
            {
                PowerAttribute p = ps.Dequeue();
                string text = "";
                if (p.IsAddition)
                {
                    if (p.IsAttribute)
                    {
                        string s = PowerManager.Instance.ChangeInfoData(p.Attribute, p.Num);
                        text = string.Format("[{0}]{1} +{2}[-]", helper.ColorConst.Color_Green,
                        EquipmentManager.GetEquipAttributeName(p.Attribute),
                        s);
                    }
                    else {
                        text = string.Format("[{0}]{1} +{2}[-]", helper.ColorConst.Color_Green,
                       ViewHelper.GetResoucesString(p.Gold),
                       p.Num);
                    }
                    
                }
                else
                {
                    if (p.IsAttribute)
                    {
                        string s = PowerManager.Instance.ChangeInfoData(p.Attribute, p.Num);
                        text = string.Format("[{0}]{1} {2}[-]", helper.ColorConst.Color_Red,
                         EquipmentManager.GetEquipAttributeName(p.Attribute),
                         s);
                    }
                    else {
                        text = string.Format("[{0}]{1} {2}[-]", helper.ColorConst.Color_Red,
                       ViewHelper.GetResoucesString(p.Gold),
                       p.Num);
                    }
                   
                }
                CoypLabel(text);
            }

        }
    }

    private void CoypLabel(string text)
    { 
        _lblPrefab.SetActive(true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(_lblPrefab);
        obj.transform.parent = _lblPanel.transform;
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.transform.localPosition = new Vector3(0,0,0);
        PowerDisplayLabel pd=obj.AddComponent<PowerDisplayLabel>();
        pd.text = text;
        _lblPrefab.SetActive(false);
    }

    private bool isStart = false;
    private float killTime = 0;
    private float _curKillTime = 0;

    private float _powerTime = 1.2f;
    private float _curPowerTime = 0;

    private float _lblTime = 0.5f;
    private float _curlblTime = 0;
    void Update()
    {
        if (isStart)
        {
            if (_curKillTime>killTime)
            {
                Gate.instance.sendNotification(MsgConstant.MSG_POWER_CLOSE);
                return;
            }

            if (_curPowerTime>=_powerTime)
            {
                _curPowerTime = 0;
                StartShow();
            }
            if (_curlblTime>=_lblTime)
            {
                _curlblTime = 0;
                StartShowLbl();
            }

            _curPowerTime += Time.deltaTime;
            _curKillTime += Time.deltaTime;
            _curlblTime += Time.deltaTime;
        }
    }
}
