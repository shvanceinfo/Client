using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    /// <summary>
    /// ID,Value 键值对
    /// </summary>
    public class IdStruct
    {
		public int Id { get; set; }
        public int Value { get; set; }
        public int Value2 { get; set; }
        public IdStruct()
        {

        }
        public IdStruct(int id,int value)
        {
            this.Id = id;
            this.Value = value;
        }
        public IdStruct(int id, int value, int value2)
            
        {
            this.Id = id;
            this.Value = value;
            this.Value2 = value2;
        }
    }
    /// <summary>
    /// 消耗类型组合,可合并物品消耗，和货币消耗
    /// </summary>
    public class TypeStruct:IComparable<TypeStruct>
    { 
        /// <summary>
        /// 如果Type=Item,ID就是物品ID
        /// 如果Type=Gold,ID就是货币枚举
        /// </summary>
        public int Id { get; set; }
        public int Value { get; set; }
        public ConsumeType Type { get; set; }
        public TypeStruct()
        {

        }
        public TypeStruct(int id,int value)
        {
            this.Id = id;
            this.Value = value;
        }
        public TypeStruct(int id,ConsumeType tp ,int value)
        {
            this.Id = id;
            this.Value = value;
            this.Type = tp;
        }

        public int CompareTo(TypeStruct other)
        {
            if (other.Type==ConsumeType.Gold&&this.Type==ConsumeType.Item)
            {
                return -1;
            }
            else if (other.Type == ConsumeType.Item && this.Type == ConsumeType.Gold)
            {
                return 1;
            }
            else if (other.Type == ConsumeType.Gold && this.Type == ConsumeType.Gold)
            {
                return 0;
            }
            return 0;
        }
    }
	[Serializable]
    public class BoolStruct
    {
        public int Id { get; set; }
        public bool Value { get; set; }
        public BoolStruct()
        {

        }
        public BoolStruct(int i,bool v)
        {
            Id = i;
            Value = v;
        }
    }
}
