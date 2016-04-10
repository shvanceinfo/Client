namespace model
{

    public enum GuideInfoTrigger
    { 
        Power=0,
        UseItem,
        UnLockSkill,
        UnLockTelent,
        Level,
        VipLevel,
        /// <summary>
        /// 任务完成
        /// </summary>
        TaskCompalte
    }
    public enum GuideInfoIconType
    { 
        None=0,
        Atlas,
        Icon
    }

    public class GuideInfoVo
    {
        public int Id { get; set; }

        public GuideInfoTrigger Trigger { get; set; }

        public string Params { get; set; }

        public GuideInfoIconType IconType { get; set; }

        public string IconPath { get; set; }

        public string IconName { get; set; }

        public string IconBackground { get; set; }

        public string TipInfo { get; set; }

        public string TipName { get; set; }

        public string ButtonText { get; set; }

        public int FunctionId { get; set; }

        public int CheckLevelMin { get; set; }

        public int CheckLevelMax { get; set; }

        public GuideInfoVo()
        {
            IconType = GuideInfoIconType.None;
        }
    }

    /// <summary>
    /// 触发器触发的数据
    /// </summary>
    public class GuideInfoData
    {
        public GuideInfoTrigger Type { get; set; }

        /// <summary>
        /// 道具ID 
        /// </summary>
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public eItemQuality Quality { get; set; }

        public GuideInfoVo Vo { get; set; }

    }
    
}
