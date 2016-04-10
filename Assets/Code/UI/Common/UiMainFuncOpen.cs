using UnityEngine;
using System.Collections;

public class UiMainFuncOpen : MonoBehaviour {
    public Vector3 position;
    bool isOpend = true;
    public float time = 0.5f;
    TweenPosition tween;
    TweenRotation tweenRotation;
    GameObject tweenObj;
    Transform openIcon;
	// Use this for initialization
	void Start () {
        tweenObj = transform.parent.FindChild("func_grid").gameObject;
        tween = transform.parent.FindChild("func_grid").GetComponent<TweenPosition>();
        
        position = tweenObj.transform.localPosition;
        openIcon = transform.FindChild("Sprite");
        tweenRotation = openIcon.GetComponent<TweenRotation>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClickFinish()
    {
        if (isOpend)
        {            
            CloseFunc();
            ScaleIcon(true);
        }
        else
        {
            OpenFunc();
            ScaleIcon(false);
        }
    }

    void CloseFunc()
    {
        isOpend = false;
        tween.from = tweenObj.transform.localPosition;
        tween.to = new Vector3(50, transform.localPosition.y, transform.localPosition.z);//transform.localPosition;
        tween.method = UITweener.Method.EaseInOut;
        UITweener.Begin<TweenPosition>(tweenObj, time);
    }

    void OpenFunc()
    {
        isOpend = true;
        tween.from = transform.localPosition;
        tween.to = position;
        tween.method = UITweener.Method.EaseInOut;
        UITweener.Begin<TweenPosition>(tweenObj, time);
    }

    void ScaleIcon(bool right)
    {
        if (right)
        {
            tweenRotation.from = new Vector3(0, 0, 60f);
            tweenRotation.to = new Vector3(0, 0, 360f);
        }
        else
        {
            tweenRotation.from = new Vector3(0, 0, 300f);
            tweenRotation.to = new Vector3(0, 0, 0f);
        }
        UITweener.Begin<TweenRotation>(openIcon.gameObject, time);
    }
}
