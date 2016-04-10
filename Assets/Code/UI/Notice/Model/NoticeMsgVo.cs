/**该文件实现的基本功能等
function: 全局公告的vo
author:zyl
date:2014-5-5
**/
using manager;
using System;

public class NoticeMsgVo
{
	private string _from;
	private string _message;
	private byte _order;
	private int _num;

	public string From {
		get {
			return this._from;
		}
		set {
			_from = value;
		}
	}

	public int Num {
		get {
			return this._num;
		}
		set {
			_num = value;
		}
	}
	
	public string Message {
		get {
			return this._message;
		}
		set {
			_message = value;
		}
	}

	public byte Order {
		get {
			return this._order;
		}
		set {
			_order = value;
		}
	}
}
