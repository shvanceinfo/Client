using UnityEngine;
using System.Collections;

public class UIBaoji : MonoBehaviour
{



    private GameObject preIn;
    private GameObject preOut;

    private GameObject waitForDelete;

    private GameObject _backcombo;
    private TweenScale _backScale;
    private TweenPosition _backPos;

    private TweenScale _backgroundScale;
    private TweenAlpha _backgroundAlpah;
    private TweenPosition _backgroundPosition;
    private TweenColor _backgroundColor;

    private TweenScale _whiteLightScale;

    private TweenPosition _comboPos;
    private float _waitTime = 2.0f;
    private float _curTime = 0;
    private bool _isShow = false;
    private int _comboNum = 0;
    private void Awake()
    {
        //_waitTime = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(1015001)
        //    .type7Data;
        _comboPos = GetComponent<TweenPosition>();

        preIn = transform.FindChild("Label/In").gameObject;
        preIn.SetActive(false);
        _backcombo = transform.FindChild("combo").gameObject;
        _backScale = _backcombo.GetComponent<TweenScale>();
        _backPos = _backcombo.GetComponent<TweenPosition>();
        preOut = transform.FindChild("Label/Out").gameObject;
        preOut.SetActive(false);
        _backgroundScale = transform.FindChild("background").GetComponent<TweenScale>();
        _backgroundAlpah = transform.FindChild("background").GetComponent<TweenAlpha>();
        _backgroundPosition = transform.FindChild("background").GetComponent<TweenPosition>();
        _backgroundColor = transform.FindChild("background").GetComponent<TweenColor>();
        _backgroundScale.enabled = false;
        _whiteLightScale = transform.FindChild("whitelight").GetComponent<TweenScale>();

    }

    private void Start()
    {
        _isShow = false;
    }

    public void Play()
    {
        _comboNum++;
        InLabel(_comboNum);
        OutLabel(_comboNum);

        _backgroundScale.transform.localScale = new Vector3(0, 0, 0);
        _backgroundScale.ResetToBeginning();
        _backgroundScale.PlayForward();
        _backgroundAlpah.ResetToBeginning();
        _backgroundAlpah.PlayForward();
        _backgroundPosition.ResetToBeginning();
        _backgroundPosition.PlayForward();
        _whiteLightScale.ResetToBeginning();
        _whiteLightScale.PlayForward();
        _backgroundColor.ResetToBeginning();
        _backgroundColor.PlayForward();

        //iTween.ShakePosition(_backcombo, iTween.Hash(
        //        "x", 0.05f,
        //        "y", 0.05f,
        //        "z", 0,
        //        "time", 0.1f,
        //        "delay", 0.35f
        //        ));
        _backScale.ResetToBeginning();
        _backScale.PlayForward();
        _backPos.ResetToBeginning();
        _backPos.PlayForward();

        _comboPos.ResetToBeginning();
        _comboPos.PlayForward();

        if (!_isShow)
        {
            //Show(true);
        }
        _isShow = true;
        _curTime = 0;
    }

    public void OnComplate()
    {
        if (waitForDelete != null)
        {
            waitForDelete.SetActive(true);
        }
    }

    public void InLabel(int num)
    {
        preIn.SetActive(true);
        GameObject obj = Instantiate(preIn) as GameObject;
        obj.transform.parent = transform.FindChild("Label").transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<UILabel>().text = num.ToString();
        TweenAlpha a = obj.GetComponent<TweenAlpha>();
        a.ResetToBeginning();
        a.PlayForward();
        TweenPosition pos = obj.GetComponent<TweenPosition>();
        pos.ResetToBeginning();
        pos.PlayForward();
        preIn.SetActive(false);

        if (waitForDelete == null)
        {
            waitForDelete = obj;
        }
        else
        {
            waitForDelete.GetComponent<UILabel>().text = num.ToString();
            waitForDelete.SetActive(false);
            obj.SetActive(false);
            obj.AddComponent<WaitDeal>().dealtime = 0.3f;
            obj.SetActive(true);
        }
    }

    private void OutLabel(int num)
    {
        preOut.SetActive(true);
        GameObject obj = Instantiate(preOut) as GameObject;
        obj.transform.parent = transform.FindChild("Label").transform;
        obj.transform.localPosition = new Vector3(0,0,0);
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<UILabel>().text = (num -= 1).ToString();
        TweenAlpha a = obj.GetComponent<TweenAlpha>();
        a.ResetToBeginning();
        a.PlayForward();
        TweenPosition pos = obj.GetComponent<TweenPosition>();
        pos.ResetToBeginning();
        if (Random.Range(0, 2) == 1)
        {
            pos.to = new Vector3(Random.Range(-100, -30), 100, 0);
        }
        else
        {
            pos.to = new Vector3(Random.Range(30, 100), 100, 0);
        }
        pos.PlayForward();
        DestroyObject(obj, 0.3f);
        preOut.SetActive(false);

        TweenRotation rot = obj.GetComponent<TweenRotation>();
        rot.ResetToBeginning();
        rot.PlayForward();
        TweenScale scale = obj.GetComponent<TweenScale>();
        scale.ResetToBeginning();
        scale.PlayForward();
    }

    public void Show(bool isforward)
    {
        if (!isforward)
        {
            iTween.MoveTo(gameObject, iTween.Hash(
                "easetype", iTween.EaseType.linear,
             "x", 355,
             "time", 0.2f,
             "islocal", true,
             "onstart", "ShowOnStart"
             ));
        }
        else
        {
            iTween.MoveTo(gameObject, iTween.Hash(
                "easetype", iTween.EaseType.linear,
             "x", 0,
             "time", 0.2f,
             "islocal", true
             ));
        }

    }

    private void Update()
    {
        if (_isShow)
        {
            if (_curTime >= _waitTime)
            {
                _curTime = 0;
                _comboNum = 0;
                _isShow = false;
                //Show(false);
            }
            _curTime += Time.deltaTime;
        }

        //if (Input.GetMouseButtonDown(0))
        //    Play();
    }
}
