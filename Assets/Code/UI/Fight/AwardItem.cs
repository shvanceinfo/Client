using UnityEngine;
using System.Collections;
using System;

public class AwardItem : MonoBehaviour {
    private ItemStruct itemData;
    private ItemTemplate itemTemp;
    private bool isPass;
    public void SetData(uint itemId, bool pass)
    {
        isPass = pass;
        itemData = ItemManager.GetInstance().GetAwardItemById((int)itemId); //得到奖励物品数据
        if (itemData != null)
        {
            itemTemp = ItemManager.GetInstance().GetTemplateByTempId(itemData.tempId); //得到奖励物品模板数据
            UITexture texture = transform.FindChild("item_icon").GetComponent<UITexture>();
            DealTexture.Instance.setTextureToIcon(texture, itemTemp, false);
            transform.FindChild("level").GetComponent<UILabel>().text = Global.FormatStrimg(LanguageManager.GetText("lbl_item_level"), itemTemp.usedLevel);
            transform.FindChild("lbl_item_name").GetComponent<UILabel>().text = Global.FormatStrimg(LanguageManager.GetText("lbl_item_func_name"), itemTemp.name);
            Transform cmp = transform.FindChild("cmp");
            ///装备评分对比
			if (itemTemp.packType == ePackageNavType.Weapon||itemTemp.packType == ePackageNavType.Equip)//如果是装备则比较
            {                
                EquipmentTemplate equipTemp = EquipmentManager.GetInstance().GetTemplateByTempId(itemTemp.id);		//得到装备模板数据
                EquipmentStruct equipData = EquipmentManager.GetInstance().GetDataByItemId(itemData.instanceId);	//得到装备数据

                ItemStruct equipedData = ItemManager.GetInstance().FindEquipedItemByPart(equipTemp.part);			//得到已经装备的装备数据
                int equipSource = 0;
                if (equipedData.instanceId != 0)
                {
                    equipSource = equipedData.equipSource;
                }
                if (equipData.equipPart == eEquipPart.eNone)
                {
                    cmp.gameObject.SetActive(true);
                    int value = equipData.score - equipSource;
                    if (value >= 0)
                    {
                        cmp.FindChild("icon").GetComponent<UISprite>().spriteName = "value_up";
                        cmp.FindChild("value").GetComponent<UILabel>().text = Global.FormatStrimg(LanguageManager.GetText("lbl_item_func_cmp_up"), Math.Abs(value));
                    }
                    else
                    {
                        cmp.FindChild("icon").GetComponent<UISprite>().spriteName = "value_down";
                        cmp.FindChild("value").GetComponent<UILabel>().text = Global.FormatStrimg(LanguageManager.GetText("lbl_item_func_cmp_down"), Math.Abs(value));
                    }
                }
                else
                {
                    cmp.gameObject.SetActive(false);
                }
            }            
            else
            {
                cmp.gameObject.SetActive(false);
            }
        }
    }

    void OnClick()
    {
        if (isPass)
        {
            ItemManager.GetInstance().NetMoveGoods(itemData.instanceId);
        }        
    }
}
