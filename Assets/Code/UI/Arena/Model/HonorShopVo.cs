/**该文件实现的基本功能等
function: 荣誉商城的VO
author:ljx
date:2013-11-09
**/
using manager;

namespace model
{
	public class HonorShopVO
	{
		public uint id;
		
		public HonorShopVO()
		{
		}
	}
	
	//翅膀配置文件解析
	public class DataReadHonor : DataReadBase
	{		
	    public override string getRootNodeName()
	    {
	        return "RECORDS";
	    }
	
		public override void appendAttribute(int key, string name, string value)
		{	
			HonorShopVO honorShop;
			uint hashKey = (uint)key;
		}
	}
}
