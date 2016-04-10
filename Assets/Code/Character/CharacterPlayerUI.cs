using UnityEngine;
using System.Collections;

public class CharacterPlayerUI : Character {
	
	static public CharacterPlayerUI sPlayerMeUI;

	void Awake() {
		sPlayerMeUI = this;
		init();
	}
	
	// Use this for initialization
	protected void Start () {
		
		setFaceDir(new Vector3(0,0,-1));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void init() {
		SceneManager.Instance.setDontDestroyObj(transform.gameObject);
		//DontDestroyOnLoad (transform.gameObject);
		character_type = CharacterType.CT_PLAYERUI;
		character_avatar = new CharacterAvatar();
		character_avatar.init(this);
	}
	
	public void equipItem (int id,bool isUI=false) {
		
		EquipmentTemplate et = EquipmentManager.GetInstance().GetTemplateByTempId((uint)id);
        if (et.id!=0) {
            BodyPartInfo bpi = new BodyPartInfo();
			switch (et.part) {
			case eEquipPart.eSuit:
				bpi.body_part = BodyPart.BP_ARMOR;
				break;
			case eEquipPart.eGreatSword:
				bpi.body_part = BodyPart.BP_WEAPON;
				break;
			}
            switch (CharacterPlayer.character_property.career) 
            {
            	case CHARACTER_CAREER.CC_SWORD:
            		bpi.texture_name = et.swordTexture;
            		break;
            	case CHARACTER_CAREER.CC_ARCHER:
            		bpi.texture_name = et.archerTexture;
            		break;
            	case CHARACTER_CAREER.CC_MAGICIAN:
            		bpi.texture_name = et.magicianTexture;
            		break;
            	default:
            		break;
            }
            bpi.model_name = et.model_name;
            bpi.weapon_trail_texture = et.weapon_trail_texture;
            bpi.effect_name = et.effect_name;
			character_avatar.changeBodyPart(bpi, CHARACTER_CAREER.CC_ARCHER,isUI);
//            character_avatar.installWing();
		}
		
        //Renderer[] allRenderer = GetComponentsInChildren<Renderer>();
        //foreach (Renderer renderer in allRenderer) {
        //    if (!renderer.gameObject.GetComponent("WillRenderTransparent")) {
        //        renderer.gameObject.AddComponent("WillRenderTransparent");
        //    }
        //}
		
		//UI上去掉碰撞体
		Collider[] allCollider = GetComponentsInChildren<Collider>();
        foreach (Collider col in allCollider) {
            col.enabled = false;
        }
	}


    public override CharacterBaseProperty GetProperty()
    {
        return CharacterPlayer.character_property;
    }
}
