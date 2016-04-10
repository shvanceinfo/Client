
using UnityEngine;
using AnimationOrTween;

/// <summary>
/// 支持颜色变换的CheckBox
/// </summary>
[AddComponentMenu("NGUI/Interaction/CheckboxObject")]
public class UICheckBoxObject : MonoBehaviour
{


    public GameObject normalObj;
    public GameObject selectObj;

    static public UICheckBoxObject current;

    public delegate void OnStateChange(bool state);

    /// <summary>
    /// Sprite that's visible when the 'isChecked' status is 'true'.
    /// </summary>

    public UISprite checkSprite;

    /// <summary>
    /// Animation to play on the checkmark sprite, if any.
    /// </summary>

    public Animation checkAnimation;

    /// <summary>
    /// If checked, tween-based transition will be instant instead.
    /// </summary>

    public bool instantTween = false;

    /// <summary>
    /// Whether the checkbox starts checked.
    /// </summary>

    public bool startsChecked = true;

    /// <summary>
    /// If the checkbox is part of a radio button group, specify the root object to use that all checkboxes are parented to.
    /// </summary>

    public Transform radioButtonRoot;

    /// <summary>
    /// Can the radio button option be 'none'?
    /// </summary>

    public bool optionCanBeNone = false;

    /// <summary>
    /// Generic event receiver that will be notified when the state changes.
    /// </summary>

    public GameObject eventReceiver;

    /// <summary>
    /// Function that will be called on the event receiver when the state changes.
    /// </summary>

    public string functionName = "OnActivate";

    /// <summary>
    /// Delegate that will be called when the checkbox's state changes. Faster than using 'eventReceiver'.
    /// </summary>

    public OnStateChange onStateChange;

    // Prior to 1.90 'option' was used to toggle the radio button group functionality
    [HideInInspector]
    [SerializeField]
    bool option = false;

    bool mChecked = true;
    bool mStarted = false;
    Transform mTrans;

    /// <summary>
    /// Whether the checkbox is checked.
    /// </summary>

    public bool isChecked
    {
        get { return mChecked; }
        set
        {
            if (radioButtonRoot == null || value || optionCanBeNone || !mStarted)
            {
                Set(value);
            }
        }
    }

    /// <summary>
    /// Legacy functionality support -- set the radio button root if the 'option' value was 'true'.
    /// </summary>

    void Awake()
    {
        mTrans = transform;

        if (checkSprite != null) checkSprite.alpha = startsChecked ? 1f : 0f;

        if (option)
        {
            option = false;
            if (radioButtonRoot == null) radioButtonRoot = mTrans.parent;
        }
    }

    /// <summary>
    /// Activate the initial state.
    /// </summary>

    void Start()
    {
        if (eventReceiver == null) eventReceiver = gameObject;
        mChecked = !startsChecked;
        mStarted = true;
        Set(startsChecked);
    }

    /// <summary>
    /// Check or uncheck on click.
    /// </summary>

    void OnClick() { if (enabled) isChecked = !isChecked; }

    /// <summary>
    /// Fade out or fade in the checkmark and notify the target of OnChecked event.
    /// </summary>

    void Set(bool state)
    {
        if (!mStarted)
        {
            mChecked = state;
            startsChecked = state;
            if (checkSprite != null) checkSprite.alpha = state ? 1f : 0f;
        }
        else if (mChecked != state)
        {
            // Uncheck all other checkboxes
            if (radioButtonRoot != null && state)
            {
                UICheckBoxObject[] cbs = radioButtonRoot.GetComponentsInChildren<UICheckBoxObject>(true);

                for (int i = 0, imax = cbs.Length; i < imax; ++i)
                {
                    UICheckBoxObject cb = cbs[i];
                    if (cb != this && cb.radioButtonRoot == radioButtonRoot)
                    {
                        cb.Set(false);
                    }
                }
            }

            // Remember the state
            mChecked = state;

            if (normalObj!=null)
            {
                
                normalObj.SetActive(!mChecked);
            }
            if (selectObj!=null)
            {
                selectObj.SetActive(mChecked);   
            }
            // Tween the color of the checkmark
            if (checkSprite != null)
            {
                if (instantTween)
                {
                    checkSprite.alpha = mChecked ? 1f : 0f;
                }
                else
                {
                    TweenAlpha.Begin(checkSprite.gameObject, 0.15f, mChecked ? 1f : 0f);
                }
            }

            current = this;

            // Notify the delegate
            if (onStateChange != null) onStateChange(mChecked);

            // Send out the event notification
            if (eventReceiver != null && !string.IsNullOrEmpty(functionName))
            {
                eventReceiver.SendMessage(functionName, mChecked, SendMessageOptions.DontRequireReceiver);
            }
            current = null;

            // Play the checkmark animation
            if (checkAnimation != null)
            {
                ActiveAnimation.Play(checkAnimation, state ? Direction.Forward : Direction.Reverse);
            }
        }
    }



}
