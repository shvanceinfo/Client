using UnityEngine;
using System.Collections;

public class EnterEModongku : MonoBehaviour {

    void OnClick ()
    {
        UIManager.Instance.openWindow(UiNameConst.ui_demon);
    }
}
