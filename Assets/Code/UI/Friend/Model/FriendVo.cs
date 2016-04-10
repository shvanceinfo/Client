using UnityEngine;
using System.Collections;
using manager;

namespace model
{


	public class FriendVo
	{
		public string Name    //姓名
		{
			get;
			set;
		}
		public int Level      //等级
		{
			get;
			set;
		}
		public int VipLevel    //VIP等级
		{
			get;
			set;
		}
		public bool IsOnline     //是否在线
		{
			get;
			set;
		}
		public CHARACTER_CAREER Career{get;set;}     //职业头像


		public int Fighter {                 //战斗力
			get;
			set;
		}	

        /// <summary>
        /// 如果为真，则可以领取,
        /// 如果为假则以领取
        /// </summary>
		public bool IsCanReceive {             //是否领取
			get;
			set;
		}

        /// <summary>
        /// 如果为真，则为已赠送
        /// 否则为未赠送
        /// </summary>
        public bool IsSend
        {                  //是否赠送
			get;
			set;
		}
	}

    /// <summary>
    /// 好友界面个人信息
    /// </summary>
    public class FriendMyInfoVo
    {
        /// <summary>
        /// 当前已领取次数
        /// </summary>
        public int CurReceive { get; set; }

        /// <summary>
        /// 当前最大能赠送次数
        /// </summary>
        public int MaxReceive
        {
            get
            {
                return VipManager.Instance.FriendMaxTiliSong;
            }
        }

        /// <summary>
        /// 当前发送次数
        /// </summary>
        public int CurSendAward { get; set; }

        /// <summary>
        /// 最大发送次数
        /// </summary>
        public int MaxSendAward {
            get {
                return VipManager.Instance.FriendMaxTiliSong;
            }
        }

        /// <summary>
        /// 当前好友数量
        /// </summary>
        public int CurFriendCount { get; set; }

        /// <summary>
        /// 最大好友数量
        /// </summary>
        public int MaxFriendCount {
            get {
                return VipManager.Instance.FriendMaxNumber;
            }
        }

        /// <summary>
        /// 好友一键接收权限
        /// </summary>
        public bool AllReceiveed {
            get {
                return VipManager.Instance.FriendAllLing;
            }
        }

        /// <summary>
        /// 好友一键领取权限
        /// </summary>
        public bool AllSended {
            get {
                return VipManager.Instance.FriendAllSong;
            }
        }

        /// <summary>
        /// 好友一键拒绝
        /// </summary>
        public bool AllRefuse
        {
            get
            {
                return VipManager.Instance.FriendAllRefuse;
            }
        }

        /// <summary>
        /// 好友一键同意
        /// </summary>
        public bool AllAgree
        {
            get
            {
                return VipManager.Instance.FriendAllAgree;
            }
        }
    }
}