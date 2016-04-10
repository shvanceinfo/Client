using UnityEngine;
using System.Collections;


public class GBLDataItem{
	public int id; // 哥布林购买的次数
    public int vip; // 购买的VIP等级限制
 	public uint dia_price;// 购买的钻石价格
}




public class DataReadGBL : DataReadBase {
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
	
	public override void appendAttribute (int key, string name, string value)
	{
		
		GBLDataItem item;
		
		if (!data.ContainsKey (key)) {
			item = new GBLDataItem ();
			data.Add (key, item);
		}

		item = (GBLDataItem)data [key];
		
		switch (name) {
		case "ID":
			item.id = int.Parse (value);
			break;
		case "VIP":
			item.vip = int.Parse (value);
			break;
		case "dia_price":
			item.dia_price = uint.Parse (value);
			break;
 
		}
	}
	
	public GBLDataItem getGBLData (int key)
	{
		
		if (!data.ContainsKey (key)) {  
			GBLDataItem item = new GBLDataItem ();
			return item;
		}
		
		return (GBLDataItem)data [key];
	}
	
	
	 
}
