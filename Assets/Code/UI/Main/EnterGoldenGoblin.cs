using UnityEngine;
using System.Collections;

public class EnterGoldenGoblin : MonoBehaviour {

    void OnClick ()
    {
       UIManager.Instance.openWindow(UiNameConst.ui_golden_goblin);

        MessageManager.Instance.SendAskGoldenGoblinTimes();
    }
}
