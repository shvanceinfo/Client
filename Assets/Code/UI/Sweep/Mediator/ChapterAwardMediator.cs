/**该文件实现的基本功能等
function: 实现章节奖励的View控制
author:zyl
date:2014-4-30
**/
using UnityEngine;
using System.Collections.Generic;
using MVC.entrance.gate;
using MVC.interfaces;
using manager;
using model;
using System;
using System.Collections;

namespace mediator
{
	public class ChapterAwardMediator: ViewMediator
	{
		public ChapterAwardMediator (ChapterAwardView view, uint id = MediatorName.CHAPTER_AWARD_MEDIATOR) : base(id, view)
		{
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				 MsgConstant.MSG_CHAPTER_AWARD_SHOWINFO,
				 MsgConstant.MSG_CHAPTER_AWARD_ASK_AWARD
			};
		}
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			if (View != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CHAPTER_AWARD_SHOWINFO:
					ArrayList al = notification.body as ArrayList;
					this.View.ShowInfo((BetterList<ItemInfo>)al[0],(bool)al[1]);
					break;
				case MsgConstant.MSG_CHAPTER_AWARD_ASK_AWARD:
					ChapterAwardManager.Instance.AskChapterAward();
					break;	
				default:
					break;
				}
			}
		}
		
		//getter and setter
		public ChapterAwardView View {
			get {
				if (_viewComponent != null && _viewComponent is ChapterAwardView)
					return _viewComponent as ChapterAwardView;
				return  null;					
			}
		}
	 
	}
}
