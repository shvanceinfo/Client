using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
namespace manager
{
    /// <summary>
    /// 成员列表
    /// </summary>
    public partial class GuildManager
    {
        private List<GuildMemberVo> _mbList;

        /// <summary>
        /// 成员列表
        /// </summary>
        public List<GuildMemberVo> MbList
        {
            get { return _mbList; }
            set { _mbList = value; }
        }

        private bool _mbShowOflineMember;

        /// <summary>
        /// 显示离线成员
        /// </summary>
        public bool MbShowOflineMember
        {
            get { return _mbShowOflineMember; }
            set { _mbShowOflineMember = value; }
        }

        private GuildMemberVo _mbSelectMemeber;

        /// <summary>
        /// 当前选中的成员，用于显示详细信息，修改职位
        /// </summary>
        public GuildMemberVo MbSelectMemeber
        {
            get { return _mbSelectMemeber; }
            set { _mbSelectMemeber = value; }
        }

        /// <summary>
        /// 每次打开需要重置的数据
        /// </summary>
        private void ResetMember()
        {
            _mbShowOflineMember = true;
        }

        private void InitialMember()
        {
            _mbShowOflineMember = true;
            _mbList = new List<GuildMemberVo>();

            _mbList.Add(new GuildMemberVo() {
             Id=0,
              Office=GuildOffice.Elite,
               Career=CHARACTER_CAREER.CC_SWORD,
                Rank=1, TotalContribution=999,
                 Power=12003,
                  Level=60,
                   Name="金刚狼",
                    IsOnline=true,
                     VipLevel=10
            });
            _mbList.Add(new GuildMemberVo()
            {
                Id = 1,
                Office = GuildOffice.DeputyLeader,
                Career = CHARACTER_CAREER.CC_SWORD,
                Rank = 1,
                TotalContribution = 1000,
                Power = 99999,
                Level = 59,
                Name = "萨达姆",
                IsOnline = false,
                VipLevel = 10
            });
            _mbList.Add(new GuildMemberVo()
            {
                Id = 2,
                Office = GuildOffice.DeputyLeader,
                Career = CHARACTER_CAREER.CC_SWORD,
                Rank = 1,
                TotalContribution = 10000,
                Power = 1000,
                Level = 58,
                Name = "奥巴马",
                IsOnline = true,
                VipLevel = 10
            });

            MbSortList();
        }

        /// <summary>
        /// 排序成员信息
        /// </summary>
        private void MbSortList()
        {
            _mbList.Sort();
        }


        public void SetSelectMember(int index)
        {
            MbSelectMemeber = MbList[index];
        }
    }
}
