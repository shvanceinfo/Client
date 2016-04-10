using UnityEngine;
using System.Collections;

// The script make UIRoot scale based on manualWidth instead of UIRoot.manualHeight.
// if current aspect ratio > maxAcceptAspectRatio.
[RequireComponent(typeof(UIRoot))]
public class UIRootHelper : MonoBehaviour
{

    public int manualWidth = 960;
    public int manualHeight = 640;

    UIRoot uiRoot_;

    void Awake()
    {
        uiRoot_ = GetComponent<UIRoot>();
    }

    void Update()
    {
        if (!uiRoot_ || manualWidth <= 0 || manualHeight <= 0) { return; }

        int h = manualHeight;
        float r = (float)(Screen.height * manualWidth) / (Screen.width * manualHeight); // (Screen.height / manualHeight) / (Screen.width / manualWidth)
        if (r > 1) { h = (int)(h * r); } // to pretend target height is more high, because screen width is too smaller to show all UI
		
		#region 尝试ngui3.5
		        //if (uiRoot_.automatic) { uiRoot_.automatic = false; }
		#endregion

        if (uiRoot_.manualHeight != h) { uiRoot_.manualHeight = h; }
    }
}