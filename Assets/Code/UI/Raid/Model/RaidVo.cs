/**该文件实现的基本功能等
function: 实现关卡的数据VO
author:ljx
date:date:2014-04-03
**/
using UnityEngine;

namespace model
{
	//单个关卡VO
	public class MapVo 
	{	
		public uint mapID;		//关卡ID
		public string gateName;		//关卡名称
		public string gateIcon;		//关卡图标
		public int whichChapter;	//所属章节
		public bool isTaskGate = false;		//是否是任务关卡
		public bool isPass = false;			//是否通关
	    public bool canEnter = false;         //该关卡是否可以进入
		public Vector3 gatePos;  //关卡位置
		public int starNum;		//有几个星星
		public bool isHard;		//是否精英关卡
		
		public int FormatStarNum{
			get{
				return Mathf.Min(this.starNum,3);
			}
		}
	}
	
	//单个章节Vo
	public class ChapterVo
	{
		public int chanpterID;			//章节的ID
		public int chapterSequence;		//章节的顺序
		public string chapterName;		//章节名称
		public string chapterIcon;		//章节icon
		public BetterList<MapVo> mapVos; //关卡包含的VO
		public BetterList<ItemInfo>  itemInfoList;//章节包含的奖励物品数量
		
		
		public ChapterVo()
		{
			mapVos = new BetterList<MapVo>();
			this.itemInfoList = new BetterList<ItemInfo>();
		}

	    public MapVo getMaxPassVo(bool isHard) //获取当前能够打开的关卡
	    {
	        MapVo maxVo = null;
            foreach (MapVo mapVo in mapVos)
            {
                if(isHard != mapVo.isHard) //精英的就比较精英,普通只比较普通
                    continue;
                if (mapVo.canEnter)
                {
                    if(maxVo == null || maxVo.mapID < mapVo.mapID)
                        maxVo = mapVo;
                }
                
            }
	        return maxVo;
	    }
	}
	
	//通关数据
	public struct sPassMap
    {
        public uint mapID;
        public byte easy;
        public byte normal;
        public byte hard;
        public uint easyUseSecond;
        public uint normalUseSecond;
        public uint hardUseSecond;
    }
}
