using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HPMPItem
{
    public int id;
    public int VIP;
    public int dia_price;
}



public class DataReadHPMP : DataReadBase 
{
	
	public override string getRootNodeName() 
    {
        return "RECORDS";
	}
	
	public override void appendAttribute(int key, string name, string value) 
    {

        HPMPItem mdi;

        if (!data.ContainsKey(key))  
        {
            mdi = new HPMPItem();
			data.Add(key, mdi);
        }

        mdi = (HPMPItem)data[key];
		
		switch (name) 
        {
        case "id":
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

    public HPMPItem getHPMPData(int key)
    {
		
		if (!data.ContainsKey(key))  
        {
            HPMPItem mdi = new HPMPItem();
			return mdi;
        }

        return (HPMPItem)data[key];
	}

}