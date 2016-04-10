using UnityEngine;
using System.Collections;

public class SubPanelPosition : MonoBehaviour
{
    public ScreenDirection screenDirection;
    //horizontal表示水平滑动；vertical表示垂直滑动。
    public enum ScreenDirection
    {
        horizontal,
        vertical
    }

    public Vector2 relativeSize = Vector2.one;
    private Transform parent;
    private Transform child;
    private float ScaleSize;
    private float rateX;
    private float rateY;
    UIPanel PanelScript;
    void Start()
    {
        parent = transform.parent;
        child = transform.GetChild(0);
        PanelScript = transform.GetComponent<UIPanel>();
    }

    void Update()
    {
        if ((Screen.width / relativeSize.x) > (Screen.height / relativeSize.y))
        {
            screenDirection = ScreenDirection.horizontal;
        }
        else
        {
            screenDirection = ScreenDirection.vertical;
        }
        SetPanel();
    }

    void SetPanel()
    {
        transform.parent = null;
        child.parent = null;
        

        if (screenDirection == ScreenDirection.vertical)
        {
            ScaleSize = transform.localScale.y;
            rateX = ScaleSize / transform.localScale.x;
            rateY = 1;
        }
        else if (screenDirection == ScreenDirection.horizontal)
        {
            ScaleSize = transform.localScale.x;
            rateX = 1;
            rateY = ScaleSize / transform.localScale.y;
        }

        transform.localScale = new Vector4(ScaleSize, ScaleSize, ScaleSize, ScaleSize);
        transform.parent = parent;
        child.parent = transform;
        PanelScript.clipRange = new Vector4(PanelScript.clipRange.x, PanelScript.clipRange.y, PanelScript.clipRange.z * rateX, PanelScript.clipRange.w * rateY);
    }

}
