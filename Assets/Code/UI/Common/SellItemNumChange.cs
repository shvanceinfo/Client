using UnityEngine;
using System.Collections;
using System;

public class SellItemNumChange : MonoBehaviour {
    UILabel numObj;
    UILabel gooldObj;

    ItemStruct itemData;
    ItemTemplate itemTemp;
    UISlider slider;

    public int saleNum = 0;
    void Awake()
    {
        itemData = ItemManager.GetInstance().GetItemByItemId(ItemManager.GetInstance().lastSelectItem);
        itemTemp = ItemManager.GetInstance().GetTemplateByTempId(itemData.tempId);
        slider = transform.parent.FindChild("Slider").GetComponent<UISlider>();
        numObj = transform.FindChild("num").GetComponent<UILabel>();
        gooldObj = transform.FindChild("coin").GetComponent<UILabel>();
    }
	// Use this for initialization
	void Start () {
        transform.parent.FindChild("Slider").GetComponent<UISlider>().sliderValue = 1 / itemData.num;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnSliderChange(float val)
    {
        int num = (int)Math.Round(val * itemData.num, 0);
        if (num <= 0)
        {
            num = 1;
        }
        numObj.text = Global.FormatStrimg(LanguageManager.GetText("item_sell_num"), num, itemData.num);
        gooldObj.text = Global.FormatStrimg(LanguageManager.GetText("item_sell_goold"), itemTemp.silivePrice * num);

        saleNum = num;
        if (itemData.num == 1)
        {
            slider.enabled = false;
        }
    }
}
