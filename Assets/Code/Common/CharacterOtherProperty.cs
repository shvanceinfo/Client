using UnityEngine;
using System.Collections;

public class CharacterOtherProperty : CharacterBaseProperty
{

	public int server_id;
	public string nick_name;
	public CHARACTER_CAREER career;
	public int sex;
	public int weapon;
	public int armor;
	//public int level;
	//public float move_speed;
	public int title_id;
	public uint wingID;
	public uint medalID;
	public int mp;
    public uint petID;
	public void setSeverID (int s)
	{
		server_id = s;
	}
	
	public int getSeverID ()
	{
		return server_id;
	}
	
	public void setNickName (string nn)
	{
		nick_name = nn;
	}

	public void setTitleID (int id)
	{
		title_id = id;
	}
	
	public string getNickName ()
	{
		return nick_name;
	}

	public void setCareer (CHARACTER_CAREER c)
	{
		career = c;
	}

	public override CHARACTER_CAREER getCareer ()
	{
		return career;
	}
	
	public void setSex (int s)
	{
		sex = s;
	}
	
	public int getSex ()
	{
		return sex;
	}
	
	public void setWeapon (int w)
	{
		weapon = w;
	}
	
	public override int getWeapon ()
	{
		return weapon;
	}
	
	public void setArmor (int a)
	{
		armor = a;
	}
	
	public int getArmor ()
	{
		return armor;
	}
	
	//public void setLevel(int l) {
	//    level = l;
	//}
	
	//public int getLevel() {
	//    return level;
	//}
	
	//public void setMoveSpeed(float ms) {
	//    move_speed = ms;
	//}
	
	//public float getMoveSpeed() {
	//    return move_speed;
	//}

	public override string GetName ()
	{
		return getNickName ();
	}

	public override int GetInstanceID ()
	{
		return getSeverID ();
	}

	public override float GetMpRatio ()
	{
		if (mp == fightProperty.GetBaseValue (eFighintPropertyCate.eFPC_MaxMP)) {
			return 0.9999f;
		} else
			return (float)mp / (float)fightProperty.GetBaseValue (eFighintPropertyCate.eFPC_MaxMP);
	}

	public override void SetMP (int m)
	{
		mp = m;
		if (mp < 0)
			mp = 0;
	}

	public override int GetMP ()
	{
		return mp;
	}
	
	public override int getHPMax ()
	{
		if (fightProperty!=null && fightProperty.fightData.ContainsKey (eFighintPropertyCate.eFPC_MaxHP)) {
			if (fightProperty.fightData [eFighintPropertyCate.eFPC_MaxHP] > 0) {
				return fightProperty.fightData [eFighintPropertyCate.eFPC_MaxHP];
			}
		}
        

		return 0;
	}


    public override int getDefence()
    {
        if (fightProperty != null && fightProperty.fightData.ContainsKey(eFighintPropertyCate.eFPC_Defense))
        {
            if (fightProperty.fightData[eFighintPropertyCate.eFPC_Defense] > 0)
            {
                return fightProperty.fightData[eFighintPropertyCate.eFPC_Defense];
            }
        }


        return 0;
    }
}
