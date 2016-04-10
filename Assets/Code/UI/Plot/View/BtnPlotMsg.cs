using UnityEngine;
using System.Collections;
using manager;

public class BtnPlotMsg : MonoBehaviour {

    const string CLOSE = "close";
    void OnClick()
    {
        switch (gameObject.name)
        {
            case CLOSE:
                PlotManager.Instance.CloseWindow();
                break;
            default:
                break;
        }
    }
}
