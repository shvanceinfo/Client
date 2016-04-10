using UnityEngine;
using System.Collections;
using manager;

public class SettingDisplayItem : MonoBehaviour {

    const string normalColor = "493d20";
    const string selectColor = "ce8a02";

    public int Id { get; set; }

    private UILabel _lbl;
    private void Awake()
    {
        _lbl = transform.FindChild("Label").GetComponent<UILabel>();
    }


    public void Display(int id, string lbl)
    {
        this.Id = id;
        this._lbl.text = lbl;
        NormalColor();
    }

    public void NormalColor()
    {
        this._lbl.color = NGUIText.ParseColor(normalColor, 0);
    }

    public void SelectColor()
    {
        this._lbl.color = NGUIText.ParseColor(selectColor, 0);
    }

    public void SelectArrowShow(GameObject buttonUp,GameObject buttonDown)
    {
        if (transform.name == (SettingManager.Instance.Helps.size-1).ToString())
        {
            buttonDown.SetActive(false);
        }
        else
        {
            buttonDown.SetActive(true);
        }

        if (transform.name == "0")
        {
            buttonUp.SetActive(false);
        }
        else
        {
            buttonUp.SetActive(true);
        }
    }
}
