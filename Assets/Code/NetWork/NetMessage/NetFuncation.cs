using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using manager;
using model;
using UnityEngine;



//通知客户端功能开启参数信息
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyFuncOpenParam:NetHead
{
	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		
		int count = (int)memRead.ReadUInt32 (); //功能开启条件组数量

		for (int i = 0,max = count; i < max; i++) {
			OpenType type = (OpenType)memRead.ReadInt32();
			int param = memRead.ReadInt32();
			if (FastOpenManager.Instance.DirOpenTypeValue.ContainsKey(type)) {
				FastOpenManager.Instance.DirOpenTypeValue[type] = param;
			}else{
				FastOpenManager.Instance.DirOpenTypeValue.Add(type,param);
			}
		}
		FastOpenManager.Instance.UpdateFunction ();
		Debug.Log (" function " + count);
		memRead.Close ();
		memStream.Close ();
	}
}

