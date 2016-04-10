/**该文件实现的基本功能等
function: 扫荡结果的类
author:ljx
date:2014-03-13
**/
using System.Collections;
namespace model
{
	public enum eStopSweep
	{
		ePlayerStop,	//玩家停止扫荡
		eSweepOver,		//扫荡结束自动停止
		eLackEngery,	//缺少体力停止
		eBagFull		//背包已满结束
	}
	
	public class SweepResultVO
	{
		public const int SINGLE_ROW_HEIGHT = 25;	//一行占用25
		const int EVERY_SWEEP_PADDING = 5; //每次扫荡间距
		
		public int sweepCounter;			//第几次扫荡
		public bool isSweepOver; 			//扫荡是否结束
		public int expNum;					//经验数目
		public int goldNum;					//金钱数目
		public Hashtable itemHash; 	//对应扫荡得到的物品名称跟数量
		private int _selfHeight;			//该结果占用多少像素行高

		public SweepResultVO()
		{
			itemHash = new Hashtable();
			isSweepOver = true;
		}
		
		//计算占用多少行高
		public int SelfHeight
		{
			get 
			{ 
				if(isSweepOver)
				{
					_selfHeight = SINGLE_ROW_HEIGHT *3;  //counter，money, exp
					_selfHeight += itemHash.Count *SINGLE_ROW_HEIGHT;
				}
				else
					_selfHeight = SINGLE_ROW_HEIGHT *2; //counter，processing
				_selfHeight += EVERY_SWEEP_PADDING;
				return _selfHeight; 
			}
		}
		
	}
}
