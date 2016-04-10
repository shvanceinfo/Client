using System.Collections.Generic;
using System;
using manager;
using UnityEngine;

[Obsolete()]
public class EquipmentEvent
{
    private static EquipmentEvent instance;
    private EquipmentEvent() { }
    public static EquipmentEvent GetInstance()
    {
        if (instance == null)
        {
            instance = new EquipmentEvent();
        }
        return instance;
    }

    /// <summary>
    /// 装备变更事件
    /// </summary>
    /// <param name="itemId">物品id</param>
    /// <param name="changeType">变更类型</param>
    public delegate void HandleEquipmentChange(int itemId, ItemEvent.eItemChangeType changeType);
    public HandleEquipmentChange EventEquipmentChange;
    public void OnEquipmentChange(int itemId, ItemEvent.eItemChangeType changeType)
    {
        if (EventEquipmentChange != null)
        {
            EventEquipmentChange(itemId, changeType);
        }
    }
    /// <summary>
    /// 装备洗炼，锁定事件
    /// </summary>
    /// <param name="index">锁定位置</param>
    /// <param name="locked">是否锁定</param>
    public delegate void HandleEquipmentCurtLock(int maxLockNum, int index, bool locked);
    public HandleEquipmentCurtLock EventEquipmentCurtLock;
    public void OnEquipmentCurtLock(int maxLockNum, int index, bool locked)
    {
        if (EventEquipmentCurtLock != null)
        {
            EventEquipmentCurtLock(maxLockNum, index, locked);
        }
    }
    /// <summary>
    /// 角色面板，查看装备信息
    /// </summary>
    /// <param name="equipId"></param>
    public delegate void HandleEquipmentSelect(uint equipId);
    public HandleEquipmentSelect EventEquipmentSelect;
    public void OnEquipmentSelect(uint equipId)
    {
        if (EventEquipmentSelect != null)
        {
            EventEquipmentSelect(equipId);
        }
    }

    public delegate void HandleSelectGemIndex(int index, uint gemTempID);
    public HandleSelectGemIndex EventSelectGemIndex;
    public void OnSelectGemIndex(int index, uint gemTempId)
    {
        if (EventSelectGemIndex != null)
        {
            EventSelectGemIndex(index, gemTempId);
        }
    }
}

class EquipmentManager
{
    public const int MAX_ATTRIBUTE = 4;
    private static EquipmentManager instance;
    public Dictionary<uint, EquipmentStruct> equipments = new Dictionary<uint, EquipmentStruct>();
    

	private EquipmentManager() { }
    /// <summary>
    /// 获取管理实例
    /// </summary>
    /// <returns></returns>
    public static EquipmentManager GetInstance()
    {
        if (instance == null)
        {
            instance = new EquipmentManager();
        }
        return instance;
    }
    
	public void Init(){
		this.equipments = new Dictionary<uint, EquipmentStruct>();
	}

	/// <summary>
    /// 装备变更操作
    /// </summary>
    /// <param name="equipment"></param>
    public void Change(EquipmentStruct equipment)
    {
        if (equipments.ContainsKey(equipment.instanceId))
        {
            Update(equipment);
            BagManager.Instance.UpdateEquipData(equipment.equipPart, equipment);
        }
        else
        {
            Add(equipment);
            BagManager.Instance.InitEquipData(equipment.equipPart, equipment);
        }
    }


    /// <summary>
    /// 添加装备
    /// </summary>
    /// <param name="equipment"></param>
    public void Add(EquipmentStruct equipment)
    {
        if (!equipments.ContainsKey(equipment.instanceId))
        {
            equipments.Add(equipment.instanceId, equipment);
            
        }
    }
    /// <summary>
    /// 更新装备
    /// </summary>
    /// <param name="equipment"></param>
    public void Update(EquipmentStruct equipment)
    {
        if (equipments.ContainsKey(equipment.instanceId))
        {
            equipments[equipment.instanceId] = equipment;
        }
    }
    /// <summary>
    /// 删除装备
    /// </summary>
    /// <param name="instanceId"></param>
    public void Delete(uint instanceId)
    {
        if (equipments.ContainsKey(instanceId))
        {
            equipments.Remove(instanceId);
        }
    }

    public EquipmentStruct GetDataByItemId(uint itemId)
    {
        if (equipments.ContainsKey(itemId))
        {
            return equipments[itemId];
        }
        return null;
    }
	
	
	
	
	
    /// <summary>
    /// 通过模版id提前装备模版数据
    /// </summary>
    /// <param name="tempId"></param>
    /// <returns></returns>
    public EquipmentTemplate GetTemplateByTempId(uint tempId)
    {
        EquipmentTemplate equipTemp = ConfigDataManager.GetInstance().getEquipmentTemplate().getTemplateData((int)tempId);
        return equipTemp;
    }
    /// <summary>
    /// 提取宝石模版
    /// </summary>
    /// <param name="gemTempId"></param>
    /// <returns></returns>
    [Obsolete()]
    public GemTemplate GetGemTemplate(uint gemTempId)
    {
        GemTemplate gemTemp = ConfigDataManager.GetInstance().getGemTemplate().getTemplateData((int)gemTempId);
        return gemTemp;
    }
    
    //获取宝石属性的相关显示
    [Obsolete()]
    public string CreateGemAttribute(uint gemTempId)
    {
    	GemTemplate gemData = GetGemTemplate(gemTempId);
        string attr = GetEquipAttributeName(gemData.stateType1) + " " +
            		Global.FormatStrimg(LanguageManager.GetText("lbl_item_attribute_value"), gemData.stateValue1);
        return attr;
    }
    /// <summary>
    /// 通过物品模版装备位置，提取已经装备的物品实例信息
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public EquipmentStruct FindEquipedItemByPart(eEquipPart part)
    {
        EquipmentStruct findEquip = new EquipmentStruct();
        foreach(KeyValuePair<uint, EquipmentStruct> equip in equipments)
        {
            if (equip.Value.equipPart == part)
            {
                findEquip = equip.Value;
            }
        }
        return findEquip;
    }
    /// <summary>
    /// 提取属性名称
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public static string GetEquipAttributeName(eFighintPropertyCate attribute, bool isAddProp = false)
    {
        string name = "";
        switch (attribute)
        {
            case eFighintPropertyCate.eFPC_MaxHP:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_MaxHP") : LanguageManager.GetText("eFPC_MaxHP");
                break;
            case eFighintPropertyCate.eFPC_HPRecover:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_HPRecover") : LanguageManager.GetText("eFPC_HPRecover");
                break;
            case eFighintPropertyCate.eFPC_MaxMP:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_MaxMP") : LanguageManager.GetText("eFPC_MaxMP");
                break;
            case eFighintPropertyCate.eFPC_MPRecover:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_MPRecover") : LanguageManager.GetText("eFPC_MPRecover");
                break;
            case eFighintPropertyCate.eFPC_Attack:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_Attack") : LanguageManager.GetText("eFPC_Attack");
                break;
            case eFighintPropertyCate.eFPC_Defense:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_Defense") : LanguageManager.GetText("eFPC_Defense");
                break;
            case eFighintPropertyCate.eFPC_Precise:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_Precise") : LanguageManager.GetText("eFPC_Precise");
                break;
            case eFighintPropertyCate.eFPC_Dodge:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_Dodge") : LanguageManager.GetText("eFPC_Dodge");
                break;
            case eFighintPropertyCate.eFPC_BlastAttack:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_BlastAttack") : LanguageManager.GetText("eFPC_BlastAttack");
                break;
            case eFighintPropertyCate.eFPC_BlastAttackAdd:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_BlastAttackAdd") : LanguageManager.GetText("eFPC_BlastAttackAdd");
                break;
            case eFighintPropertyCate.eFPC_BlastAttackReduce:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_BlastAttackReduce") : LanguageManager.GetText("eFPC_BlastAttackReduce");
                break;
            case eFighintPropertyCate.eFPC_Tenacity: //韧性
                name = isAddProp ? LanguageManager.GetText("a_eFPC_Tenacity") : LanguageManager.GetText("eFPC_Tenacity");
                break;
            case eFighintPropertyCate.eFPC_FightBreak: //破击
                name = isAddProp ? LanguageManager.GetText("a_eFPC_FightBrack") : LanguageManager.GetText("eFPC_FightBreak");
                break;
            case eFighintPropertyCate.eFPC_AntiFightBreak://招架
                name = isAddProp ? LanguageManager.GetText("a_eFPC_AntiFightBrack") : LanguageManager.GetText("eFPC_AntiFightBreak");
                break;
            case eFighintPropertyCate.eFPC_KnockDown: //击倒(击飞)
                name = isAddProp ? LanguageManager.GetText("a_eFPC_KnockDown") : LanguageManager.GetText("eFPC_KnockDown");
                break;
            case eFighintPropertyCate.eFPC_KnockBack: //击退
                name = isAddProp ? LanguageManager.GetText("a_eFPC_KnockBack") : LanguageManager.GetText("eFPC_KnockBack");
                break;
            case eFighintPropertyCate.eFPC_IceAttack: //冰伤害
                name = isAddProp ? LanguageManager.GetText("a_eFPC_IceAttack") : LanguageManager.GetText("eFPC_IceAttack");
                break;
            case eFighintPropertyCate.eFPC_AntiIceAttack://冰抗性
                name = isAddProp ? LanguageManager.GetText("a_eFPC_AntiIceAttack") : LanguageManager.GetText("eFPC_AntiIceAttack");
                break;
            case eFighintPropertyCate.eFPC_IceImmunity: // 冰免疫
                name = isAddProp ? LanguageManager.GetText("a_eFPC_IceImmunity") : LanguageManager.GetText("eFPC_IceImmunity");
                break;
            case eFighintPropertyCate.eFPC_FireAttack:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_FireAttack") : LanguageManager.GetText("eFPC_FireAttack");
                break;
            case eFighintPropertyCate.eFPC_AntiFireAttack:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_AntiFireAttack") : LanguageManager.GetText("eFPC_AntiFireAttack");
                break;
            case eFighintPropertyCate.eFPC_FireImmunity:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_FireImmunity") : LanguageManager.GetText("eFPC_FireImmunity");
                break;
            case eFighintPropertyCate.eFPC_PoisonAttack:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_PoisonAttack") : LanguageManager.GetText("eFPC_PoisonAttack");
                break;
            case eFighintPropertyCate.eFPC_AntiPoisonAttack:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_AntiPoisonAttack") : LanguageManager.GetText("eFPC_AntiPoisonAttack");
                break;
            case eFighintPropertyCate.eFPC_PoisonImmunity:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_PoisonImmunity") : LanguageManager.GetText("eFPC_PoisonImmunity");
                break;
            case eFighintPropertyCate.eFPC_ThunderAttack:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_ThunderAttack") : LanguageManager.GetText("eFPC_ThunderAttack");
                break;
            case eFighintPropertyCate.eFPC_AntiThunderAttack:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_AntiThunderAttack") : LanguageManager.GetText("eFPC_AntiThunderAttack");
                break;
            case eFighintPropertyCate.eFPC_ThunderImmunity:
                name = isAddProp ? LanguageManager.GetText("a_eFPC_ThunderImmunity") : LanguageManager.GetText("eFPC_ThunderImmunity");
                break;
        }
        return name;
    }
    
    /// <summary>
    /// 通过物品实例ID，提取强化模版
    /// </summary>
    /// <param name="itemInstanceId"></param>
    /// <returns></returns>
    [Obsolete()]
    public EquipmentForgeTemplate GetEquipmentForgeTemplateByEquipId( ItemStruct itemData, bool nextTmp = true)
    {
        EquipmentForgeTemplate tmp = new EquipmentForgeTemplate();
        string forgeStr = "";
        ItemTemplate itemTemp = ItemManager.GetInstance().GetTemplateByTempId(itemData.tempId);
		EquipmentTemplate equipTemp = ConfigDataManager.GetInstance().getEquipmentTemplate().getTemplateData((int)itemData.tempId);
        if (itemTemp.id != 0)
        {
            uint level = 0;
            if (nextTmp)
            {
                level = GetDataByItemId(itemData.instanceId).intensifyLevel + 1;
            }
            else
            {
                level = GetDataByItemId(itemData.instanceId).intensifyLevel;
            }
            //int type = (int)itemTemp.itemType;
			int type = (int)equipTemp.part;
            forgeStr = type.ToString() + level.ToString().PadLeft(3, '0');
			Debug.Log("forgeStr: "+forgeStr);
            tmp = ConfigDataManager.GetInstance().getEquipmentForgeTemplate().getTemplateData(int.Parse(forgeStr));
        }

        return tmp;
    }
	
	
    /// <summary>
    /// 装备对比
    /// </summary>
    /// <param name="equipedData"></param>
    /// <param name="selectData"></param>
    /// <returns></returns>
    /// 
    /// 
    [Obsolete("已经无法使用")]
	public eFighintPropertyCate[] CompareEquip(EquipmentStruct equipedData, EquipmentStruct selectData)
    {
//        int i = 0;
//        eFighintPropertyCate[] cmpKeys = new eFighintPropertyCate[EquipmentManager.MAX_ATTRIBUTE];
//        foreach (KeyValuePair<eFighintPropertyCate, sEquipProperty> equipProp in equipedData.addProperty)
//        {
//            if (selectData.addProperty.ContainsKey(equipProp.Key))
//            {
//                cmpKeys[i] = equipProp.Key;
//            }
//            i++;
//        }
//        if (equipedData.instanceId != selectData.instanceId)
//        {
//            for (i = 0; i < EquipmentManager.MAX_ATTRIBUTE; i++)
//            {
//                if (cmpKeys[i] == eFighintPropertyCate.eFPC_None)
//                {
//                    bool find = false;
//                    eFighintPropertyCate key = eFighintPropertyCate.eFPC_None;
//                    foreach (KeyValuePair<eFighintPropertyCate, sEquipProperty> equipProp in selectData.addProperty)
//                    {
//                        find = false;
//                        key = equipProp.Key;
//                        for (int j = 0; j < EquipmentManager.MAX_ATTRIBUTE; j++)
//                        {
//                            if (cmpKeys[j] == key)
//                            {
//                                find = true;
//                                break;
//                            }
//                        }
//                        if (!find)
//                        {
//                            cmpKeys[i] = key;
//                        }
//                    }
//                }
//            }
//        }
//        return cmpKeys;    
		throw new Exception("无法使用");
    }


    //////////////////////////////////////////////////网络请求//////////////////////////////////////////////////
    /// <summary>
    /// 强化
    /// </summary>
    /// <param name="itemPos"></param>
    /// <param name="supper"></param>
    [Obsolete()]
    public void NetAskIntensifyEquip(UInt32 itemPos, bool supper)
    {
        //byte b = 0;
        //if (supper)
        //{
        //    b = 1;
        //}
        //GCAskIntensifyEquip net = new GCAskIntensifyEquip(itemPos, b);
        //MainLogic.SendMesg(net.ToBytes());
    }

    /// <summary>
    /// 宝石合成
    /// </summary>
    /// <param name="itemPos"></param>
    /// <param name="supper"></param>
    [Obsolete()]
    public void NetAskCombineGem(UInt32 itemPos, bool supper)
    {
        //byte b = 0;
        //if (supper)
        //{
        //    b = 1;
        //}
        //GCAskCombineGem net = new GCAskCombineGem(itemPos, b);
        //MainLogic.SendMesg(net.ToBytes());
    }

    /// <summary>
    /// 洗炼
    /// </summary>
    /// <param name="itemPos"></param>
    /// <param name="supper"></param>
    [Obsolete()]
    public void NetAskWashEquip(UInt32 itemPos, bool supper, bool lock1, bool lock2, bool lock3, bool lock4)
    {
        //GCAskWashEquip net = new GCAskWashEquip(itemPos, (byte)(supper ? 1 : 0));
        //net.lockFP0 = (byte)(lock1 ? 1 : 0);
        //net.lockFP1 = (byte)(lock2 ? 1: 0);
        //net.lockFP2 = (byte)(lock3 ? 1 : 0);
        //net.lockFP3 = (byte)(lock4 ? 1 : 0);
        //MainLogic.SendMesg(net.ToBytes());
    }

    /// <summary>
    /// 宝石镶嵌
    /// </summary>
    /// <param name="itemPos"></param>
    /// <param name="supper"></param>
    [Obsolete()]
    public void NetAskInlayGem(UInt32 itemPos, UInt32 gemPos, UInt32 index)
    {
        //GCAskInlayGem net = new GCAskInlayGem(itemPos, gemPos, index);
        //MainLogic.SendMesg(net.ToBytes());
    }

    /// <summary>
    /// 宝石移除
    /// </summary>
    /// <param name="itemPos"></param>
    /// <param name="supper"></param>
    [Obsolete()]
    public void NetAskRemoveGem(UInt32 itemPos, UInt32 index)
    {
//        GCAskRemoveGem net = new GCAskRemoveGem(itemPos, index);
//        MainLogic.SendMesg(net.ToBytes());
    }
    /// <summary>
    /// 装备进阶
    /// </summary>
    /// <param name="itemPos"></param>
    [Obsolete()]
    public void NetAskUpgradeEquip(UInt32 itemPos)
    {
        //GCAskUpgradeEquip net = new GCAskUpgradeEquip(itemPos);
        //MainLogic.SendMesg(net.ToBytes());
    }

    /// <summary>
    /// 装备分解
    /// </summary>
    /// <param name="itemPos"></param>
    [Obsolete()]
    public void NetAskDecomposeEquip(UInt32 itemPos)
    {
        //GCAskDecomposeEquip net = new GCAskDecomposeEquip(itemPos);
        //MainLogic.SendMesg(net.ToBytes());
    }
}

