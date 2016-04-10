using UnityEngine;
using System.Collections;

namespace model
{
	public enum OpenType
	{
		Level =1,
		Vip =2,
		TaskAccept =3,
		TaskFinish = 4
	}

	public enum LocationType
	{
		None =0,
		LeftTop = 1,
		RightTop =2,
		LeftMid = 3,
		RightMid = 4,
		Bottom = 5
	}

	public class FastOpenVo
	{

		/// <summary>
		/// 功能ID
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 此值用来控制显示的级别，比此值小的ui都能显示
		/// </summary>
		/// <value>The parameter.</value>
		public int Param {
			set;
			get;
		}
		/// <summary>
		/// ui的类型
		/// </summary>
		/// <value>The type.</value>
		public OpenType Type {
			set;
			get;
		}
		/// <summary>
		/// 开打的条件值
		/// </summary>
		/// <value>The value.</value>
		public int Value {
			set;
			get;
		}
		/// <summary>
		/// ui的路径地址
		/// </summary>
		/// <value>The user interface URL.</value>
		public string UIUrl {
			set;
			get;
		}
		/// <summary>
		/// ui显示的顺序
		/// </summary>
		/// <value>The order.</value>
		public int order {
			set;
			get;
		}
		/// <summary>
		/// ui所属的位置
		/// </summary>
		/// <value>The location.</value>
		public LocationType Location {
			set;
			get;
		}
		public string FunctionIcon {
			set;
			get;
		}
		public bool IsNotice {
			set;
			get;
		}
	}
}
