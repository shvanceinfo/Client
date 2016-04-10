using UnityEngine;
using System.Collections;

public class FloatAnimation  {
    public static void Play(GameObject playObj, GameObject sendObj)
    {
        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(playObj, 0f);
        alpha.duration = 1.5f;
        alpha.from = 1f;
        alpha.to = 0f;

        TweenPosition comp = UITweener.Begin<TweenPosition>(playObj, 1.8f);
        comp.from = new Vector3(0f, 0f, 0f);
        comp.to = new Vector3(0f, 460f, 0f);
        comp.callWhenFinished = "OnFinished";
        comp.eventReceiver = sendObj;
    }
}
