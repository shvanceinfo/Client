using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace model
{
	/// <summary>
	/// 公会成员数据
	/// </summary>
    public class GuildMemberVo : IComparable<GuildMemberVo>
	{
        public int Id { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public CHARACTER_CAREER Career { get; set; }

        /// <summary>
        /// 战斗力
        /// </summary>
        public int Power { get; set; }

        /// <summary>
        /// VIP等级
        /// </summary>
        public int VipLevel { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public GuildOffice Office { get; set; }

        /// <summary>
        /// 总贡献度
        /// </summary>
        public int TotalContribution { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get; set; }

        public int CompareTo(GuildMemberVo other)
        {
            int v=this.IsOnline.CompareTo(other.IsOnline);
            if (v==0)
            {
                int tc = this.TotalContribution.CompareTo(other.TotalContribution);
                if (tc==0)
                {
                    int of = this.Office.CompareTo(other.Office);
                    if (of==0)
                    {
                        int lvl = this.Level.CompareTo(other.Level);
                        if (lvl != 0)
                            return -lvl;
                        return 0;
                    }
                    return of;
                }
                return -tc;
            }
            return -v;
        }
    }
}
