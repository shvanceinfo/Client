using System;
using System.Collections;
using System.Runtime.InteropServices;


namespace model
{
    public class SkillVo
    {
        /// <summary>
        /// xmlID
        /// </summary>
        public int XmlID { get; set; }
        /// <summary>
        /// 短ID（去掉level）
        /// </summary>
        public int SID { get; set; }
        public int Level {
            get { return XmlID % 100 == 0 ? 100 : XmlID % 100; }
        }
        public string Name { get; set; }
        public string Icon { get; set; }
        public float Attack_Range { get; set; }
        public bool Qishou { get; set; }
        public int Sou_Target { get; set; }
        public int No_Target { get; set; }
        public int Cool_Down { get; set; }

        public int Mp_Cost { get; set; }

        public float Attack_Coefficient { get; set; }

        public float Damage_Plus { get; set; }

        public string Max_Active { get; set; }

        public eSkillType Type { get; set; }

        public int Active_Level { get; set; }

        public eFighintPropertyCate AddStateType { get; set; }

        public float LastTick { get; set; }

        public int NextSkillID { get; set; }

        public int WeaponType { get; set; }

        public string SzDesc { get; set; }

        public eGoldType UnLockType { get; set; }
        public int UnLockValue { get; set; }

        public string SkillLevelDescription { get; set; }

        public BetterList<TypeStruct> Consume { get; set; }

        public string SkillDescription { get; set; }

        public bool IsShow { get; set; }
        public SkillVo()
        {
            Consume = new BetterList<TypeStruct>();
        }
    }
   
}

