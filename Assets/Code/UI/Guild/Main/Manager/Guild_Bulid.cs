using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
namespace manager
{
    /// <summary>
    /// 公会建设
    /// </summary>
    public partial class GuildManager
    {
        List<GuildBulidVo> _buildDisplayList;

        /// <summary>
        /// 用于显示公会建筑数据
        /// </summary>
        public List<GuildBulidVo> BuildDisplayList
        {
            get {
                if (_buildDisplayList == null)
                {
                    _buildDisplayList = new List<GuildBulidVo>();
                    foreach (GuildBulidVo vo in XmlGuildBuild.Values)
                    {
                        _buildDisplayList.Add(vo);
                    }
                }
                return _buildDisplayList; 
            }
        }

        private void InitialBulid()
        {

        }
    }
}