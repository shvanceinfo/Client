
namespace model
{
    /// <summary>
    /// 公会建筑功能列表
    /// </summary>
    public class GuildBulidVo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }


        private int _level;
        /// <summary>
        /// 公会建筑-功能等级
        /// </summary>
        public int Level {
            get {
                return _level < DefaultLevel ? DefaultLevel : _level;
            }
            set {
                _level = value;
            }
        }
        

        /// <summary>
        /// 功能开放等级
        /// </summary>
        public int OpenLevel { get; set; }

        /// <summary>
        /// 功能默认等级
        /// </summary>
        public int DefaultLevel { get; set; }

        /// <summary>
        /// 按钮1文字说明
        /// </summary>
        public string ButtonDesc1 { get; set; }

        /// <summary>
        /// 按钮1跳转界面
        /// </summary>
        public int FunctionId1 { get; set; }

        /// <summary>
        /// 按钮2文字说明
        /// </summary>
        public string ButtonDesc2 { get; set; }

        /// <summary>
        /// 按钮2跳转界面
        /// </summary>
        public int FunctionId2 { get; set; }
        public GuildBulidVo()
        {
            Level = 0;
        }
    }
}