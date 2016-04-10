using UnityEngine;
using System.Collections;
using NetGame;

public class BronCountdown : MonoBehaviour
{
    public float timeOut;
    private float curTimeSec = 0;
    private UISlider fgSlider;

    void Start()
    {
        fgSlider = GetComponent<UISlider>();
    }
    void OnEnable()
    {
        curTimeSec = 0;
    }

	// Update is called once per frame
	void Update () {
        if (curTimeSec < timeOut)
        {
            curTimeSec += Time.deltaTime;
            fgSlider.sliderValue = 1 - curTimeSec / timeOut;
        }
        else
        {
            OnTimeOver();
        }
	}

    public void OnTimeOver()
    {
        if (!Global.inMultiFightMap())
        {
            MessageManager.Instance.sendMessageReturnCity();
            Global.ResetBornData();
            ItemManager.GetInstance().awardItems.Clear();
            //
            UIManager.Instance.closeWindow(UiNameConst.ui_born, true);
        }
    }
}
