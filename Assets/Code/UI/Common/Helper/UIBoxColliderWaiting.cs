using UnityEngine;
using System.Collections;

/// <summary>
/// 延时按钮释放
/// </summary>
public class UIBoxColliderWaiting : MonoBehaviour {

    public float waittime = 0.2f;
    private BoxCollider _box;
    private bool _isStart = false;
    private float _curTime=0;
    private void Awake()
    {
        _box = GetComponent<BoxCollider>();
    }
    void OnClick()
    {
        if (_box!=null)
        {
            _box.enabled = false;
            _isStart = true;
        }  
    }

    private void Update()
    {
        if (_isStart)
        {
            if (_curTime>waittime)
            {
                _curTime = 0;
                _box.enabled = true;
                _isStart = false;
            }
            _curTime += Time.deltaTime;
        }
    }
}
