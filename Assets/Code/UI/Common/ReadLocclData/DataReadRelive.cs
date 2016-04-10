using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReliveItem
{
    public int id;
    public int VIP;
    public int dia_price;
}



public class DataReadRelive : DataReadBase 
{
	
	public override string getRootNodeName() 
    {
        return "RECORDS";
	}
	
	public override void appendAttribute(int key, string name, string value) 
    {

        ReliveItem mdi;

        if (!data.ContainsKey(key))  
        {
            mdi = new ReliveItem();
			data.Add(key, mdi);
        }

        mdi = (ReliveItem)data[key];
		
		switch (name) 
        {
        case "ID":
			mdi.id = int.Parse(value);
			break;
        case "VIP":
            mdi.VIP = int.Parse(value);
			break;
        case "dia_price":
            mdi.dia_price = int.Parse(value);
            break;
		}
	}

    public ReliveItem GetReliveData(int key)
    {
		
		if (!data.ContainsKey(key))  
        {
            ReliveItem mdi = new ReliveItem();
			return mdi;
        }

        return (ReliveItem)data[key];
	}

}