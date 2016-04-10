using UnityEngine;
using System.Collections;

//public class DungeonDataItem
//{
//	public int id; // 序号
//	public string  duo_name; // 副本名称
//	public string duo_pic;//  副本图片
////	public uint duo_level;	//副本进入限制的等级
//	public uint duo_cishu;	//副本可以打的上限次数
////	public BetterList<uint> duo_jiangli; //多人副本奖励的物品
//	public int map_ID;		//地图ID
////	public int duo_dropOut;
//	public string duo_ZDXX; //世界消息
//	
//	
//	public DungeonDataItem ()
//	{
////		duo_jiangli = new BetterList<uint> ();
//	}
//	
//}
//  
//public class DataReadDungeon : DataReadBase
//{
//	
//	public override string getRootNodeName ()
//	{
//		return "RECORDS";
//	}
//	
//	public override void appendAttribute (int key, string name, string value)
//	{
//		
//		DungeonDataItem item;
//		
//		if (!data.ContainsKey (key)) {
//			item = new DungeonDataItem ();
//			data.Add (key, item);
//		}
//
//		item = (DungeonDataItem)data [key];
//		
//		string[] splits = null;
//		char[] charSeparators = new char[] {','};
//		switch (name) {
//		case "ID":
//			item.id = int.Parse (value);
//			break;
//		case "duo_name":
//			item.duo_name = value;
//			break;
//		case "duo_pic":
//			item.duo_pic = value;
//			break;
////		case "duo_level":
////			item.duo_level = uint.Parse (value);
////			break;
//		case "duo_cishu":
//			item.duo_cishu = uint.Parse (value);
//			break;	
////		case "duo_jiangli":
////			splits = value.Split (charSeparators); //分割的字符串
////			foreach (string c in splits) {
////				item.duo_jiangli.Add (uint.Parse (c.Trim ()));
////			}
////			break;
//		case "map_ID":
//			item.map_ID = int.Parse (value);
//			break;
////		case "duo_dropOut":
////			item.duo_dropOut = int.Parse (value);
////			break;
//		case "duo_ZDXX":
//			item.duo_ZDXX = value;
//			break;
//			
//		default:
//			break;
// 
//		}
//	}
//	
//	public ICollection getKeys(){
//  
//		return data.Keys;
//	}
//	
//	public DungeonDataItem getDungeonData (int key)
//	{
//		
//		if (!data.ContainsKey (key)) {  
//			DungeonDataItem item = new DungeonDataItem ();
//			return item;
//		}
//		
//		return (DungeonDataItem)data [key];
//	}
//	
//	//得到第一条记录
//	public DungeonDataItem getFirst(){
//		if (data.Count>0) {
//			var list = data.Keys.GetEnumerator();
//			list.MoveNext();
//			var key = list.Current;//得到第一个key
//			return data[key] as DungeonDataItem;
//			//return 
//		}else 
//			return null;
//	}
//	
// 	//得到数量
//	public int getCount(){
//		return this.data.Count;
//	}
//	
//	
//	 
//}
