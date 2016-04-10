/**该文件实现的基本功能等
function: 荣誉商店管理器
author:ljx
date:2013-11-09
**/
namespace manager
{
	public class HonorShopManager
	{
		private static HonorShopManager _instance;
		
		private HonorShopManager()
		{
			
		}
		
		//getter and setter		
		public static HonorShopManager Instance
		{
			get 
			{ 
				if(_instance == null)
					_instance = new HonorShopManager();
				return _instance; 
			}
		}
	}
}
