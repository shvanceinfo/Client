using UnityEngine;
using System.Collections;
using model;
using manager;

public class DataReadPet: DataReadBase
{

	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
	
	public override void appendAttribute (int key, string name, string value)
	{	
		PetVo pet;
		uint hashKey = (uint)key;
		if (PetManager.Instance.DictionaryPet.ContainsKey (hashKey))
			pet = PetManager.Instance.DictionaryPet [hashKey];
		else {
			pet = new PetVo ();
			PetManager.Instance.DictionaryPet.Add (hashKey, pet);
		}
		
		string[] splits = null;
		char[] charSeparators = new char[] {','};
		switch (name) {
		case "ID":
			pet.Id = uint.Parse (value);
			break;
		case "stateType":
			splits = value.Split (charSeparators);
			foreach (string every in splits) {
				int addType = int.Parse (every);
				pet.AttrTypes.Add (addType);
			}
			break;
		case "stateValue":
			splits = value.Split (charSeparators);
			foreach (string every in splits) {
				int addValue = int.Parse (every);
				pet.AttrValues.Add (addValue);
			}
			break;
		case "PetName":
			pet.PetName = value;
			break;
		case "PetParam":
			pet.PetParam = uint.Parse(value);
			break;
		case "PetLv":
			pet.PetLv = uint.Parse(value);
			break;
	 
		case "evoCostItem":
			splits = value.Split (charSeparators);
			pet.EvoCostItem = uint.Parse (splits [0]);
			pet.EvoNum = uint.Parse (splits [1]);
			break;
		case "evoCost":
			splits = value.Split (charSeparators);
			eGoldType type = (eGoldType)int.Parse (splits [0]);
			if (type ==  eGoldType.gold)
				pet.CostGold = uint.Parse (splits [1]);
			else if (type == eGoldType.zuanshi)
				pet.CostDiamond = uint.Parse (splits [1]);
			break;
		case "lowLimit":
			pet.LowLimit = uint.Parse (value);
			break;
		case "highLimit":
			pet.HighLimit = uint.Parse (value);
			break;
		case "sucRateAdd":
			pet.SucRateAdd = uint.Parse (value);
			break;
		case "PetModle":
			pet.PetModle = value;
			break;
		case "PetEffect":
			pet.PetEffect = value;
			break;
		case "modelXYZ":
			splits = value.Split (charSeparators);
			pet.ModelPos = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
		case "RotateXYZ":
			splits = value.Split (charSeparators);
			pet.RotateXYZ = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
			break;
        case "modelScale":
            splits = value.Split(charSeparators);
            pet.ModelScale = new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2]));
            break;	
		case "qingling":
			if (value == "0") {
				pet.IsLimit = false;
			} else {
				pet.IsLimit = true;
			}
			break;
		case "PetSkill":
			pet.PetSkill = int.Parse(value);
			break;
		case "UsedDesc":
			pet.UsedDesc = value;
			break;
		case "EffectDesc":
			pet.EffectDesc = value;
			break;

		default:
			break;
		}
	}
}
