using UnityEngine;
using System.Collections;

public class PlayFloatFinish : MonoBehaviour {    
    void OnFinished()
    {
        GameObject.Destroy(gameObject);
    }

    public void PlayMsg()
    {
        StartCoroutine(TweenOut());
    }

    public void PlayAwardItem(float delay)
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.zero;
        gameObject.name = "award_" + delay.ToString();
        TweenScale scale = UITweener.Begin<TweenScale>(gameObject, 0.1f);
        scale.method = UITweener.Method.BounceOut;
        scale.delay = delay;
        scale.from = Vector3.zero;
        scale.to = Vector3.one;
        

        TweenPosition position = UITweener.Begin<TweenPosition>(gameObject, 0.1f);
        position.method = UITweener.Method.EaseOut;
        position.delay = delay;
        position.eventReceiver = gameObject;
        position.callWhenFinished = "PlayAwardFinish";
        position.from = new Vector3(0f, 0f, -250f);
        position.to = new Vector3(0f, 200f, -250f);
        
        //StartCoroutine(Wait(delay));
    }
    void PlayAwardFinish()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {        
        yield return new WaitForSeconds(2f);
        OnFinished();
    }

    IEnumerator TweenOut()
    {
        TweenScale scale = UITweener.Begin<TweenScale>(gameObject, 0.1f);
        scale.from = Vector3.zero;
        scale.to = transform.localScale;
        scale.method = UITweener.Method.BounceOut;

        float y = Random.Range(20, 80);
        float x = Random.Range(-20, 20);
        TweenPosition position = UITweener.Begin<TweenPosition>(gameObject, 0.1f);
        position.from = new Vector3(0f, 0f, -250f);
        position.to = new Vector3(x, y, -250f);
        scale.method = UITweener.Method.EaseOut;

        yield return new WaitForSeconds(0.5f);

        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(gameObject, 0.5f);
        alpha.from = 1f;
        alpha.to = 0f;

        TweenPosition comp = UITweener.Begin<TweenPosition>(gameObject, 0.3f);
        comp.method = UITweener.Method.EaseInOut;
        comp.from = position.to;
        comp.to = new Vector3(0, 350f, -250f);
        comp.callWhenFinished = "OnFinished";
        comp.eventReceiver = gameObject;
    }
	
	public void Disappear()
	{
		Destroy(gameObject);
	}
}
