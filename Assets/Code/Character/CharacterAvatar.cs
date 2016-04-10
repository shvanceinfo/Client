using UnityEngine;
using System.Collections.Generic;
using manager;
using model;

public enum BodyPart {
	
	BP_NULL,
	BP_ARMOR,
	BP_WEAPON,
	BP_WING
    //BP_HAIR,
    //BP_BODY,
}

public class BodyPartInfo {
	
	public BodyPart body_part;
	public string texture_name;
	public string model_name;
	public string effect_name;
	public string weapon_trail_texture;
}

public class CharacterAvatar {
	
	Character owner;
	BodyPartInfo hair;
	BodyPartInfo body;
	BodyPartInfo armor;
	BodyPartInfo weapon;
	
	public void init(Character c) {
		
		owner = c;
	}
	
	public void installWing(uint wingID = 0, bool showInUI = false)
	{
		string model = null;
		if(wingID == 0 && WingManager.Instance.CurrentWing != null)
			model = WingManager.Instance.CurrentWing.wingModle;
		else if(wingID > 0)
		{
			WingVO wingVO = WingManager.Instance.WingHash[wingID] as WingVO;
			if(wingVO != null)
				model = wingVO.wingModle;
		}
		if(model != null)
		{
            if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleWing))
            {
                BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleWing, model,
                    (asset) =>
                    {
                        addWingToOwner(asset, showInUI);
                    });
            }
            else
            {
                GameObject asset = BundleMemManager.Instance.getPrefabByName(model, EBundleType.eBundleWing);
                addWingToOwner(asset, showInUI);
            }                    
		}
	}

    private void addWingToOwner(Object obj, bool showInUI)
    {
        if (owner == null)
            return;
        Transform[] allChildren = owner.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name == "effect_spine")
            {
                foreach (Transform subChild in child) //清除原来的翅膀
                {
                    Object.Destroy(subChild.gameObject);
                }
                GameObject wing = BundleMemManager.Instance.instantiateObj(obj, Vector3.zero, Quaternion.identity);
                wing.transform.parent = child.gameObject.transform;
                wing.transform.localPosition = Vector3.zero;
                wing.transform.localRotation = Quaternion.identity;
                wing.transform.localScale = Vector3.one;
                if(showInUI)
                    ToolFunc.SetLayerRecursively(wing, LayerMask.NameToLayer("TopUI"));
                break;
            }
        }
    }
	
	public void changeBodyPart(BodyPartInfo bpi, CHARACTER_CAREER inCareer = CHARACTER_CAREER.CC_ARCHER, bool isUI = false) 
    {
		
		switch (bpi.body_part) 
		{
			case BodyPart.BP_ARMOR:
				changeTexture("armor", bpi.texture_name);
				armor = bpi;
				break;
			case BodyPart.BP_WEAPON:
	        {
	            string strWeaponHang = "right_hand";
				CHARACTER_CAREER career;
				if(owner.GetProperty() != null)
					career = owner.GetProperty().getCareer();
				else
					career = inCareer;
	            switch (career) 
	            {
	            	case CHARACTER_CAREER.CC_SWORD:
	            		strWeaponHang = "right_hand";
	            		break;
	            	case CHARACTER_CAREER.CC_ARCHER:
	            		strWeaponHang = "left_hand";
	            		break;
	            	case CHARACTER_CAREER.CC_MAGICIAN:
	            		strWeaponHang = "right_hand";
	            		break;
	            	default:
	            		break;
	            }

                if (!isUI)
                {
                    if (Global.inFightMap() || Global.inTowerMap() ||
                        Global.inGoldenGoblin() || Global.InArena() ||
                        Global.inMultiFightMap() || Global.InWorldBossMap() || Global.InAwardMap())
                    {
                        uninstallWeapon(strWeaponHang);
                        installWeapon(strWeaponHang, bpi.model_name, bpi.weapon_trail_texture, bpi.effect_name);
                    }
                    else
                    {
                        uninstallWeapon("weapon_spine");
                        installWeapon("weapon_spine", bpi.model_name, bpi.weapon_trail_texture, bpi.effect_name);
                    }
                }
                else
                {
                    uninstallWeapon("weapon_spine");
                    installWeapon("weapon_spine", bpi.model_name, bpi.weapon_trail_texture, bpi.effect_name, isUI);
                }
	            
	        }		
			weapon = bpi;
			break;
		}
	}
		
	public void packUpWeapon() {
		
		if (weapon == null) return;
		
		BodyPartInfo bpi = weapon;

        string strWeaponHang = "right_hand";

        if (owner.GetProperty().getCareer() == CHARACTER_CAREER.CC_ARCHER)
        {
            strWeaponHang = "left_hand";
        }
        else if (owner.GetProperty().getCareer() == CHARACTER_CAREER.CC_SWORD)
        {
            strWeaponHang = "right_hand";
        }
        else
        {
            strWeaponHang = "right_hand";
        }

        uninstallWeapon(strWeaponHang);
		uninstallWeapon("weapon_spine");
		installWeapon("weapon_spine", bpi.model_name, bpi.weapon_trail_texture, bpi.effect_name);
	}
	
	public void catchWeapon() {
		if (weapon == null) return;
		
		BodyPartInfo bpi = weapon;


        string strWeaponHang = "right_hand";

        if (owner.GetProperty().getCareer() == CHARACTER_CAREER.CC_ARCHER)
        {
            strWeaponHang = "left_hand";
        }
        else if (owner.GetProperty().getCareer() == CHARACTER_CAREER.CC_SWORD)
        {
            strWeaponHang = "right_hand";
        }
        else
        {
            strWeaponHang = "right_hand";
        }


        uninstallWeapon(strWeaponHang);
		uninstallWeapon("weapon_spine");
        installWeapon(strWeaponHang, bpi.model_name, bpi.weapon_trail_texture, bpi.effect_name);
	}
	
	void changeTexture(string part, string texture) 
    {		
		Transform[] allChildren = owner.GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) 
        {
		    if (child.gameObject.name == part)
		    {
                child.renderer.sharedMaterial.mainTexture = BundleMemManager.Instance.loadResource(texture, typeof(Texture2D)) as Texture;
                    //BundleMemManager.Instance.getPrefabFromCache<Texture2D>(EBundleType.eBundlePicture, ToolFunc.TrimPath(texture));
				break;
			}
		}
	}
	
	void installWeapon(string part, string model, string trail_name, string eff_name, bool showInUI = false) {
		Transform[] allChildren = owner.GetComponentsInChildren<Transform>();
	    Transform weaponTrans = null;
	    foreach (Transform child in allChildren)
	    {
	        if (child.gameObject.name == part)
	        {
	            weaponTrans = child;
	            break;
	        }
	    }
        if(weaponTrans != null)
        {
	        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleWeapon))
            {
                BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleWeapon, model,
                    (asset) =>
                    {
                        setWeaponEff((GameObject)asset, eff_name, weaponTrans, owner.GetProperty().getCareer(), showInUI);
                    }, owner.GetProperty().getCareer());
            }
            else
            {
                GameObject asset = BundleMemManager.Instance.getPrefabByName(model, EBundleType.eBundleWeapon);
                setWeaponEff(asset, eff_name, weaponTrans, owner.GetProperty().getCareer(), showInUI);
            }				
		}
	}

    void setWeaponEff(GameObject asset, string eff_name, Transform parentTrans, CHARACTER_CAREER career, bool showInUI)
    {
        GameObject weapon = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
        weapon.transform.parent = parentTrans;
		weapon.transform.localPosition = Vector3.zero;
		weapon.transform.localRotation = Quaternion.identity;
		weapon.transform.localScale = Vector3.one;				
		if(eff_name != "") {
			Transform[] weaponChildren = weapon.GetComponentsInChildren<Transform>();
		    Transform childTrans = null;
		    foreach (Transform weaponChild in weaponChildren)
		    {
		        if (weaponChild.gameObject.name == "Effect")
		        {
                    childTrans = weaponChild;
		            break;
		        }
		    }
            if (childTrans != null)
            {
			    if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleWeaponEffect))
                {
                    BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleWeaponEffect, eff_name,
                        (asset_eff) =>
                        {                                        
                            if (!asset_eff)
                            {
                                MonoBehaviour.print("prefab error : " + eff_name);
                                return;
                            }
                            GameObject weapon_eff = BundleMemManager.Instance.instantiateObj(asset_eff, Vector3.zero, Quaternion.identity);
                            weapon_eff.transform.parent = childTrans;
                            weapon_eff.transform.localPosition = Vector3.zero;
                            weapon_eff.transform.localRotation = Quaternion.identity;
                            weapon_eff.transform.localScale = Vector3.one;
                            if(showInUI)
                                ToolFunc.SetLayerRecursively(parentTrans.gameObject, LayerMask.NameToLayer("TopUI"));
                        }, career);
                }
                else
                {
                    GameObject asset_eff = BundleMemManager.Instance.getPrefabByName(eff_name, EBundleType.eBundleWeaponEffect);
                    if (!asset_eff)
                    {
                        MonoBehaviour.print("prefab error : " + eff_name);
                        return;
                    }
                    GameObject weapon_eff = BundleMemManager.Instance.instantiateObj(asset_eff, Vector3.zero, Quaternion.identity);
                    weapon_eff.transform.parent = childTrans;
                    weapon_eff.transform.localPosition = Vector3.zero;
                    weapon_eff.transform.localRotation = Quaternion.identity;
                    weapon_eff.transform.localScale = Vector3.one;
                }  
			}
		}
        // 先把刀光注释掉
        //WeaponTrail trail = weapon.GetComponentInChildren<WeaponTrail>();
        //if(trail) {
        //    //owner.animation_controller.AddTrail(trail);
        //    trail.StartTrail(0.5f, 0.4f);
        //    trail.SetTime (0.5f, 0.2f, 0.2f);	
        //    if (trail_name != null)  {
        //          trail.renderer.material.mainTexture = BundleMemManager.Instance.getTextureByName(trail_name);
        //    }
        //    owner.trail = trail;
        //}
        Weapon weapon_script = weapon.GetComponentInChildren<Weapon>();
        if (weapon_script)
        {
            weapon_script.setHolder(owner);
        }
	}
	
	void uninstallWeapon(string part) {
		if (weapon != null) {
			
			Transform[] allChildren = owner.GetComponentsInChildren<Transform>();
			foreach (Transform child in allChildren) {
			    if (child.gameObject.name == part) {
					foreach(Transform child_weapon in child.gameObject.transform){
					    GameObject.Destroy(child_weapon.gameObject);
					}
					break;
				}
			}
			
			/*
            if (owner.animation_controller) {
                owner.animation_controller.RemoveAllTrail();
            }
            */
           
			weapon = null;
		}
	}
	
	int testNum = 0;
    List<BodyPartInfo> testInfoArray = null;
	public void initTestAvatar() {
		testInfoArray = new List<BodyPartInfo>();
		BodyPartInfo bpi1 = new BodyPartInfo();
		BodyPartInfo bpi2 = new BodyPartInfo();
		BodyPartInfo bpi3 = new BodyPartInfo();
		BodyPartInfo bpi4 = new BodyPartInfo();
		BodyPartInfo bpi5 = new BodyPartInfo();
		BodyPartInfo bpi6 = new BodyPartInfo();
		bpi1.body_part = BodyPart.BP_ARMOR;
		bpi1.texture_name = "Texture/armor_blue";
		//bpi2.body_part = BodyPart.BP_HAIR;
		//bpi2.texture_name = "Texture/hair_blue";
		bpi3.body_part = BodyPart.BP_ARMOR;
		bpi3.texture_name = "Texture/armor_red";
		//bpi4.body_part = BodyPart.BP_HAIR;
		//bpi4.texture_name = "Texture/hair_red";
		bpi5.body_part = BodyPart.BP_WEAPON;
		bpi5.model_name = "Model/prefab/weapon1";
		bpi6.body_part = BodyPart.BP_WEAPON;
		bpi6.model_name = "Model/prefab/weapon2";
		bpi6.weapon_trail_texture = "Texture/SwordTrailRed";
		bpi6.effect_name = "Model/prefab/eff_electricityball";
		testInfoArray.Add(bpi1);
		testInfoArray.Add(bpi2);
		testInfoArray.Add(bpi3);
		testInfoArray.Add(bpi4);
		testInfoArray.Add(bpi5);
		testInfoArray.Add(bpi6);
	}
	public void testChangeAvatar() {
		changeBodyPart((BodyPartInfo)testInfoArray[testNum++]);
		if (testNum >= testInfoArray.Count) {
			testNum = 0;
		}
	}
}
