/**该文件实现的基本功能等
function: 实现章节奖励的管理
author:zyl
date:2014-4-30
**/
using UnityEngine;
using System.Collections;
using NetGame;
using System;
using model;
using MVC.entrance.gate;
using mediator;


namespace manager
{	
	public class ChapterAwardManager
	{
		private static ChapterAwardManager _instance;
		
		private GCAskChapterAward _askChapterAward;
		
 
		private ChapterAwardManager(){
			_askChapterAward = new GCAskChapterAward();
		}
	 	
		
		public void ShowChapterAwardUI(){
			UIManager.Instance.openWindow(UiNameConst.ui_chapter_award);
			this.ChapterAwardShowInfo();
		}
		
		public void CloseChapterAwardUI(){
			UIManager.Instance.closeWindow(UiNameConst.ui_chapter_award);
		}
		
		
		
		
		public void ChapterAwardShowInfo(){
			var chapterVo = RaidManager.Instance.CurrentChapter;
			int starNumCount = 0;
			bool isHard = RaidManager.Instance.showHard;  			 
			CountStarNum (chapterVo, ref starNumCount, isHard);
			bool isCanTakeAward = CheckIsCanTakeAward (chapterVo, starNumCount,isHard); //判断是否可以拿奖励,只有满星星才能拿
			ArrayList al = new ArrayList();
			al.Add(RaidManager.Instance.CurrentChapter.itemInfoList);
			al.Add(isCanTakeAward);
			Gate.instance.sendNotification (MsgConstant.MSG_CHAPTER_AWARD_SHOWINFO,al);
		}
		
		/// <summary>
		/// 计算星星总数
		/// </summary>
		/// <param name='chapterVo'>
		/// Chapter vo.
		/// </param>
		/// <param name='starNumCount'>
		/// Star number count.
		/// </param>
		/// <param name='isHard'>
		/// Is hard.
		/// </param>
		public static void CountStarNum (ChapterVo chapterVo, ref int starNumCount, bool isHard)
		{
			foreach (MapVo vo in chapterVo.mapVos) {//统计星星数量
				if (vo.isHard == isHard) {
					starNumCount += vo.FormatStarNum;
				}		 		
			}
		}
		
		/// <summary>
		/// 是否能拿奖励
		/// </summary>
		/// <returns>
		/// The is can take award.
		/// </returns>
		/// <param name='chapterVo'>
		/// If set to <c>true</c> chapter vo.
		/// </param>
		/// <param name='starNumCount'>
		/// If set to <c>true</c> star number count.
		/// </param>
		public static bool CheckIsCanTakeAward (ChapterVo chapterVo, int starNumCount,bool isHard)
		{
			int count=0;
			for (int i = 0,max =chapterVo.mapVos.size; i < max; i++) {
				 if (chapterVo.mapVos[i].isHard == isHard) {
					count++;
				 }
			}
			return starNumCount == count*3;
		}
		
		#region 网络通信
		
		public void AskChapterAward(){
			NetBase.GetInstance ().Send (this._askChapterAward.ToBytes((ushort)RaidManager.Instance.CurrentChapter.chanpterID,0), true);
		}
		
		#endregion
		
		
		#region 事件注册
		public void RegisterEvent ()
		{
			ItemEvent.GetInstance ().EventItemChangeReason += HandleEventItemChangeReason;			 
			 
		}

		public void CancelEvent ()
		{
 
			ItemEvent.GetInstance ().EventItemChangeReason -= HandleEventItemChangeReason;						 
			 
		}
		#endregion
		
		
		void HandleEventItemChangeReason (int itemId, ItemChangeReason changeReason)
		{
			if (changeReason == ItemChangeReason.TakeChapterAward) { //如果是拿奖励触发事件
				this.CloseChapterAwardUI();
				RaidManager.Instance.UpdateChapterAward(false); //更新界面
			}
		}
		
		
		
		
		public static ChapterAwardManager Instance{
			get{
				if (_instance == null) {
					_instance = new ChapterAwardManager();
				}
				return _instance;
			}
		}
		
	}
}