/**该文件实现的基本功能等
function: 实现角色界面
author:zyl
date:2014-4-12
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using System.Collections.Generic;

public class RoleView : ModelView
{

	Transform _trans; 	//当前对象的transform组件信息

	private UILabel VipLevel;//vip等级显示
	private UILabel FightTxt;//战斗力数值
    private Vector3 _modelRootPos;  //模型坐标
	#region 左边
	UISprite _weaponBg;
	UISprite _weaponBg1;
	UITexture _weaponItem;
	UILabel _weaponPlus;
	UISprite _clothBg;
	UISprite _clothBg1;
	UITexture _clothItem;
	UILabel _clothPlus;
	UISprite _pantsBg;
	UISprite _pantsBg1;
	UITexture _pantsItem;
	UILabel _pantsPlus;
	UISprite _shoeBg;
	UISprite _shoeBg1;
	UITexture _shoeItem;
	UILabel _shoePlus;
	UISprite _necklaceBg;
	UISprite _necklaceBg1;
	UITexture _necklaceItem;
	UILabel _necklacePlus;
	UISprite _ringBg;
	UISprite _ringBg1;
	UITexture _ringItem;
	UILabel _ringPlus;
	UILabel _name;
	#endregion
	
 
	#region 右边
	UILabel _attack;
	UILabel _defense;
	UILabel _hitrate;
	UILabel _miss;
	UILabel _strike;
	UILabel _steel;
	UILabel _break;
	UILabel _wardoff;
	UILabel _zxc;
	UILabel _guard;
	UILabel _career;
	UILabel _level;
	UISlider _exp;
	UILabel _expValue;
	UISlider _life;
	UILabel _lifeValue;
	UISlider _magic;
	UILabel _magicValue;
	#endregion
  
	
	
	void Awake ()
	{
        this._trans = this.transform.FindChild("top_ui");
        VipLevel = _trans.FindChild("roleVip/VipLabel").GetComponent<UILabel>();
        FightTxt = _trans.FindChild("roleVip/FightLabel").GetComponent<UILabel>();
        _modelRootPos = transform.FindChild("bg/fazhen").position;
		#region 左边信息
		this._weaponBg = this._trans.Find ("userinfo/weapon/bg").GetComponent<UISprite> ();
		this._weaponBg1 = this._trans.Find ("userinfo/weapon/bg1").GetComponent<UISprite> ();
		this._weaponItem = this._trans.Find ("userinfo/weapon/item").GetComponent<UITexture> ();
		this._weaponPlus = this._trans.Find ("userinfo/weapon/plus").GetComponent<UILabel> ();
		this._clothBg = this._trans.Find ("userinfo/cloth/bg").GetComponent<UISprite> ();
		this._clothBg1 = this._trans.Find ("userinfo/cloth/bg1").GetComponent<UISprite> ();
		this._clothItem = this._trans.Find ("userinfo/cloth/item").GetComponent<UITexture> ();
		this._clothPlus = this._trans.Find ("userinfo/cloth/plus").GetComponent<UILabel> ();
		this._pantsBg = this._trans.Find ("userinfo/pants/bg").GetComponent<UISprite> ();
		this._pantsBg1 = this._trans.Find ("userinfo/pants/bg1").GetComponent<UISprite> ();
		this._pantsItem = this._trans.Find ("userinfo/pants/item").GetComponent<UITexture> ();
		this._pantsPlus = this._trans.Find ("userinfo/pants/plus").GetComponent<UILabel> ();
		this._shoeBg = this._trans.Find ("userinfo/shoe/bg").GetComponent<UISprite> ();
		this._shoeBg1 = this._trans.Find ("userinfo/shoe/bg1").GetComponent<UISprite> ();
		this._shoeItem = this._trans.Find ("userinfo/shoe/item").GetComponent<UITexture> ();
		this._shoePlus = this._trans.Find ("userinfo/shoe/plus").GetComponent<UILabel> ();
		this._necklaceBg = this._trans.Find ("userinfo/necklace/bg").GetComponent<UISprite> ();
		this._necklaceBg1 = this._trans.Find ("userinfo/necklace/bg1").GetComponent<UISprite> ();
		this._necklaceItem = this._trans.Find ("userinfo/necklace/item").GetComponent<UITexture> ();
		this._necklacePlus = this._trans.Find ("userinfo/necklace/plus").GetComponent<UILabel> ();
		this._ringBg = this._trans.Find ("userinfo/ring/bg").GetComponent<UISprite> ();
		this._ringBg1 = this._trans.Find ("userinfo/ring/bg1").GetComponent<UISprite> ();
		this._ringItem = this._trans.Find ("userinfo/ring/item").GetComponent<UITexture> ();
		this._ringPlus = this._trans.Find ("userinfo/ring/plus").GetComponent<UILabel> ();
		this._name = this._trans.Find ("userinfo/name").GetComponent<UILabel> ();
		#endregion
		
		#region 右边信息
		this._attack = this._trans.Find ("rightinfo/attack/titlename").GetComponent<UILabel> ();
		this._defense = this._trans.Find ("rightinfo/defense/titlename").GetComponent<UILabel> ();
		this._hitrate = this._trans.Find ("rightinfo/hitrate/titlename").GetComponent<UILabel> ();
		this._miss = this._trans.Find ("rightinfo/miss/titlename").GetComponent<UILabel> ();
		this._strike = this._trans.Find ("rightinfo/strike/titlename").GetComponent<UILabel> ();
		this._steel = this._trans.Find ("rightinfo/steel/titlename").GetComponent<UILabel> ();
		this._break = this._trans.Find ("rightinfo/break/titlename").GetComponent<UILabel> ();
		this._wardoff = this._trans.Find ("rightinfo/wardoff/titlename").GetComponent<UILabel> ();
		this._zxc = this._trans.Find ("rightinfo/zxc/titlename").GetComponent<UILabel> ();
		this._guard = this._trans.Find ("rightinfo/guard/titlename").GetComponent<UILabel> ();
		this._career = this._trans.Find ("rightinfo/career/titlename").GetComponent<UILabel> ();
		this._level = this._trans.Find ("rightinfo/level/titlename").GetComponent<UILabel> ();
		this._exp = this._trans.Find ("rightinfo/exp/Slider").GetComponent<UISlider> ();
		this._expValue = this._trans.Find ("rightinfo/exp/value").GetComponent<UILabel> ();
		this._life = this._trans.Find ("rightinfo/life/Slider").GetComponent<UISlider> ();
		this._lifeValue = this._trans.Find ("rightinfo/life/value").GetComponent<UILabel> ();
		this._magic = this._trans.Find ("rightinfo/magic/Slider").GetComponent<UISlider> ();
		this._magicValue = this._trans.Find ("rightinfo/magic/value").GetComponent<UILabel> ();
		#endregion
		
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new  RoleMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.ROLE_MEDIATOR);
	}
	
	
	 
	
	/// <summary>
	/// Shows the equip.
	/// </summary>
	/// <param name='equipData'>
	/// Equip data.
	/// </param>
	public void ShowEquip (Dictionary<eEquipPart, EquipmentStruct> equipData)
	{
		#region 为装备图赋值
		BagView.SetEquipIconAndBg (equipData, eEquipPart.eArcher, this._weaponItem, this._weaponBg,this._weaponBg1,this._weaponPlus);
		BagView.SetEquipIconAndBg (equipData, eEquipPart.eDoublePole, this._weaponItem, this._weaponBg, this._weaponBg1,this._weaponPlus);
		BagView.SetEquipIconAndBg (equipData, eEquipPart.eGreatSword, this._weaponItem, this._weaponBg,this._weaponBg1,this._weaponPlus); 
		BagView.SetEquipIconAndBg (equipData, eEquipPart.eLeggings, this._pantsItem, this._pantsBg,this._pantsBg1,this._pantsPlus);
		BagView.SetEquipIconAndBg (equipData, eEquipPart.eNecklace, this._necklaceItem, this._necklaceBg,this._necklaceBg1,this._necklacePlus);
		BagView.SetEquipIconAndBg (equipData, eEquipPart.eRing, this._ringItem, this._ringBg,this._ringBg1,this._ringPlus);
		BagView.SetEquipIconAndBg (equipData, eEquipPart.eShoes, this._shoeItem, this._shoeBg,this._shoeBg1,this._shoePlus);
		BagView.SetEquipIconAndBg (equipData, eEquipPart.eSuit, this._clothItem, this._clothBg, this._clothBg1,this._clothPlus);
  
		#endregion
		
		this._name.text = CharacterPlayer.character_property.getNickName ();
	}
	
	public void ShowPeopleInfo ()
	{

        float data = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001010).type7Data * 100;

		var userInfo = CharacterPlayer.character_property;
		this._attack.text = userInfo.attack_power.ToString ();
		this._defense.text = userInfo.defence.ToString ();
        this._hitrate.text = string.Format("{0:F2}", (userInfo.fightProperty.GetValue(eFighintPropertyCate.eFPC_Precise) * data)) + "%";		    //命中
        this._miss.text = string.Format("{0:F2}", (userInfo.fightProperty.GetValue(eFighintPropertyCate.eFPC_Dodge) * data) ) + "%"; 			    //闪避
        this._strike.text = string.Format("{0:F2}", (userInfo.fightProperty.GetValue(eFighintPropertyCate.eFPC_BlastAttack) * data) ) + "%";	    //暴击
        this._steel.text = string.Format("{0:F2}", (userInfo.fightProperty.GetValue(eFighintPropertyCate.eFPC_Tenacity) * data) ) + "%";		    //韧性
        this._break.text = string.Format("{0:F2}", (userInfo.fightProperty.GetValue(eFighintPropertyCate.eFPC_AntiFightBreak) * data) ) + "%";	    //破招
        this._wardoff.text = string.Format("{0:F2}", (userInfo.fightProperty.GetValue(eFighintPropertyCate.eFPC_FightBreak) * data) ) + "%";	    //招架
        this._zxc.text = string.Format("{0:F2}", (userInfo.fightProperty.GetValue(eFighintPropertyCate.eFPC_BlastAttackAdd) * data) ) + "%";	    //必杀
        this._guard.text = string.Format("{0:F2}", (userInfo.fightProperty.GetValue(eFighintPropertyCate.eFPC_BlastAttackReduce) * data) ) + "%";   //守护
		
		this._level.text = userInfo.level.ToString ();
		this._career.text = BagManager.Instance.GetItemCareerString (userInfo.career);
		
		//this._exp.value =userInfo.ex
		
		int curLevel = CharacterPlayer.character_property.getLevel ();
		RoleDataItem rdi = ConfigDataManager.GetInstance ()
							.getRoleConfig ().getRoleData ((int)CharacterPlayer.character_property.career * Constant.LEVEL_RATIO + curLevel);//读角色配置表
		this._exp.value = (float)userInfo.getExperience () / rdi.upgrade_exp;
		this._expValue.text = userInfo.getExperience () + "/" + rdi.upgrade_exp;
		 
		
		this._life.value = (float)userInfo.getHP () / userInfo.getHPMax ();
		this._lifeValue.text = userInfo.getHP () + "/" + userInfo.getHPMax ();
		
		this._magic.value = (float)userInfo.GetMP () / userInfo.getMPMax ();
		this._magicValue.text = userInfo.GetMP () + "/" + userInfo.getMPMax ();
		
		
	}

	public void ChangeVipLevel ()
	{
		VipLevel.text = VipManager.Instance.VipLevel.ToString ();
		FightTxt.text = CharacterPlayer.character_property.getFightPower ().ToString ();
	}



}
