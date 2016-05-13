using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using UnityEngine;
using MVC.entrance.gate;
using manager;
using model;

namespace NetGame
{
	public delegate void processCreateRole (int result); //处理创角的回调
	
	public enum _eMessageType
	{
		C2G_ACCOUNT_LOGIN = 100
	};

	public interface INetObject
	{
		byte[] ToBytes ();

		void ToObject (byte[] bytesData);
	}

	/// <summary>
	/// 允许序列化，设置字节对齐
	/// 包头定义
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class NetHead : INetObject
	{
		public UInt16 _length;
		public UInt16 _assistantCmd;

		public NetHead ()
		{
			this._length = 0;
			this._assistantCmd = 100;
		}

		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
			memWrite.Write (this._length);
			memWrite.Write (this._assistantCmd);            
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			memStream.Close ();
			return bytesData;
		}

		public void ToBytes (ref BinaryWriter memWrite)
		{
			memWrite.Write (this._length);
			memWrite.Write (this._assistantCmd);            
		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			this._length = memRead.ReadUInt16 ();
			this._assistantCmd = memRead.ReadUInt16 ();
			memRead.Close ();
			memStream.Close ();
		}

		public void ToObject (ref BinaryReader memRead)
		{
			this._length = memRead.ReadUInt16 ();
			this._assistantCmd = memRead.ReadUInt16 ();
		}

		public char[] StringToChar (string value, int length)
		{
			if (value == null) {
				value = "";
			}
			value = Global.UnicodeToUtf8 (value);
			return value.PadRight (length, '\0').ToCharArray ();
		}

		public byte[] StringToByte (string value, int length)
		{
			byte[] data = new byte[length];
			byte[] tmpData = System.Text.Encoding.UTF8.GetBytes (Global.UnicodeToUtf8 (value));
			if (tmpData.Length < length) {
				Array.Copy (tmpData, 0, data, 0, tmpData.Length);
			}

			return data;
		}

		public byte[] StringToByte (string value)
		{
			return System.Text.Encoding.UTF8.GetBytes (Global.UnicodeToUtf8 (value)); 
		}
	}

	/// <summary>
	/// 登入请求
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RequestLogin : NetHead
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		//申请空间
        public byte[] _userName;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
		public byte[] _userPassword; 
		//GUID
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
		public byte[] _userGUID;
		//设备号
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 108)]
		public byte[] _deviceToken;
		public byte _reConnect; //是否请求重新登录，0代表第一次登录，1代表重新登录

		public RequestLogin () : base()
		{  
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_Login;
		}

		public RequestLogin (string name, string password, string guid, bool reConnect)
            : base()
		{
			this._assistantCmd = (UInt16)eC2GType.C2G_Login;
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._userName = StringToByte (name, 20);
			this._userPassword = StringToByte (password, 36);
			this._userGUID = StringToByte (guid, 36);
			this._deviceToken = StringToByte (Global.GetDeviceIdentifier (), 108);  
			if (reConnect)
				this._reConnect = 1;
			else
				this._reConnect = 0;          
		}

		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToBytes (ref memWrite);
			memWrite.Write (this._userName);
			memWrite.Write (this._userPassword);
			memWrite.Write (this._userGUID);
			memWrite.Write (this._deviceToken);
			memWrite.Write (this._reConnect);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}

	/// <summary>
	/// 登入回应
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RequestLoginRet : NetHead
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		//申请空间
        public char[] _userName;
		//GUID
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
		public char[] _userGUID;
		//登入标识
		public Int32 _flag;

		public RequestLoginRet ()
            : base()
		{
			this._length = (UInt16)Marshal.SizeOf (this);
		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this._userName = Encoding.UTF8.GetChars (memRead.ReadBytes (20));
			this._userGUID = Encoding.UTF8.GetChars (memRead.ReadBytes (36));
			this._flag = memRead.ReadInt32 ();
			memRead.Close ();
			memStream.Close ();
		}
	}
	
	/// <summary>
	/// ping消息
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RequestPing : NetHead
	{
		public RequestPing ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_Ping;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToBytes (ref memWrite);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	/// <summary>
	/// 角色列表
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyRoleBaseInfo : NetHead
	{
		public UInt32 m_un32ObjID;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public char[] m_n32RoleNickName;
		public Int32 m_n32CareerID;
		public byte m_bGender;
		public UInt32 m_un32CoatTypeID;
		public uint m_un32LegGuardId;
		public uint m_un32ShoeId;
		public uint m_un32NecklaceId;
		public uint m_un32RingId;
		public uint m_un32WeaponId;
		public UInt32 m_un32Level;
		public UInt32 m_un32Exp;
		public UInt32 m_un32CurHP;
		public UInt32 m_un32CurMP;
		public UInt32 m_un32CurHPVessel;
		public UInt32 m_un32CurMPVessel;
		public UInt32 m_un32MaxHP;
		public UInt32 m_un32MaxMP;
		public UInt32 m_un32MaxHPVessel;
		public UInt32 m_un32MaxMPVessel;
		public CharacterFightProperty m_fightProperty = new CharacterFightProperty ();
		public uint m_un32Max_packages;
		public uint wingID;
        
		public GSNotifyRoleBaseInfo ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
		}
		
		public void ToObject (byte[] bytesData)
		{
			Loger.Notice ("length:{0},netLength:{1}", Marshal.SizeOf (this), bytesData.Length);
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.m_un32ObjID = memRead.ReadUInt32 ();
			this.m_n32RoleNickName = Encoding.UTF8.GetChars (memRead.ReadBytes (20));
			this.m_n32CareerID = memRead.ReadInt32 ();
			this.m_bGender = memRead.ReadByte ();
			this.m_un32CoatTypeID = memRead.ReadUInt32 ();

			///add begin
			m_un32LegGuardId = memRead.ReadUInt32 ();
			m_un32ShoeId = memRead.ReadUInt32 ();
			m_un32NecklaceId = memRead.ReadUInt32 ();
			m_un32RingId = memRead.ReadUInt32 ();
			m_un32WeaponId = memRead.ReadUInt32 ();
			///add end


			this.m_un32Level = memRead.ReadUInt32 ();
			this.m_un32Exp = memRead.ReadUInt32 ();
			this.m_un32CurHP = memRead.ReadUInt32 ();
			this.m_un32CurMP = memRead.ReadUInt32 ();
			this.m_un32CurHPVessel = memRead.ReadUInt32 ();
			this.m_un32CurMPVessel = memRead.ReadUInt32 ();
			//this.m_un32MaxHP = memRead.ReadUInt32();
			//this.m_un32MaxMP = memRead.ReadUInt32();
			this.m_un32MaxHPVessel = memRead.ReadUInt32 ();
			this.m_un32MaxMPVessel = memRead.ReadUInt32 ();
            
			///add
			int length = (int)eFighintPropertyCate.eFpc_End;
			for (int i = 1; i < length; i++) {
				m_fightProperty.fightData.Add ((eFighintPropertyCate)i, memRead.ReadInt32 ());
			}
			this.m_un32Max_packages = memRead.ReadUInt32 ();
			this.wingID = memRead.ReadUInt32 ();
			memRead.Close ();
			memStream.Close ();
			this.m_un32MaxHP = (uint)m_fightProperty.GetValue (eFighintPropertyCate.eFPC_MaxHP);
			this.m_un32MaxMP = (uint)m_fightProperty.GetValue (eFighintPropertyCate.eFPC_MaxMP);            
		}
	}
	
	/// <summary>
	/// 选择角色
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskSelectRole : NetHead
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public byte[] _roleName;
		
		public GCAskSelectRole ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_SelectRole;
		}
		
		public void SetRoleName (String roleName)
		{
			_roleName = StringToByte (roleName, 20);
			
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this._roleName);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	/// <summary>
	/// 错误消息提示
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class NetErrorMessage : NetHead
	{
		public Int32 protocolId;
		public UInt32 strLength;
		public int errorCode;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 108)]
		public char[] errorMessage;

		public NetErrorMessage ()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.UTF8);
			base.ToObject (ref memRead);
			this.protocolId = memRead.ReadInt32 ();
			this.errorCode = memRead.ReadInt32 ();
			this.strLength = memRead.ReadUInt32 ();
			byte[] byteStr = memRead.ReadBytes ((int)strLength);
			errorMessage = Encoding.UTF8.GetChars (byteStr);
		}
	}

	/// <summary>
	/// 改变场景
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyChangeScene : NetHead
	{
		public UInt32 m_un32MapID;
		public UInt32 m_un32SceneID;
		public UInt32 m_un32ClientNO;
		public float m_fPosX;
		public float m_fPosY;
		public float m_fPosZ;
		
		public GSNotifyChangeScene ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eG2CType.G2C_RoleChangeScene;
		}
		
		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.m_un32MapID = memRead.ReadUInt32 ();
			this.m_un32SceneID = memRead.ReadUInt32 ();
			this.m_un32ClientNO = memRead.ReadUInt32 ();
			this.m_fPosX = memRead.ReadSingle ();
			this.m_fPosY = memRead.ReadSingle ();
			this.m_fPosZ = memRead.ReadSingle ();
			memRead.Close ();
			memStream.Close ();
		}
	}
	
	/// <summary>
	/// 角色进入视野
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyRoleAppear : NetHead
	{
		public UInt32 m_un32ObjID;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public char[] m_n32RoleNickName;
		public Int32 m_n32CareerID;
		public byte m_bGender;
		public UInt32 m_un32WeaponTypeID;
		public UInt32 m_un32CoatTypeID;
		public UInt32 m_un32Level;
		public float m_fCurPosX;
		public float m_fCurPosY;
		public float m_fCurPosZ;
		public float m_fTarPosX;
		public float m_fTarPosY;
		public float m_fTarPosZ;
		public float m_fMoveSpeed;
		public UInt32 m_un32CurTitleID;
		public UInt32 m_un32CurHp;
		public CharacterFightProperty fightProperty = new CharacterFightProperty ();
		public UInt32 m_u32WingID; //翅膀ID
		public UInt32 m_u32MedalID;
        public UInt32 m_u32PetID;

		public GSNotifyRoleAppear ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eG2CType.G2C_RoleAppear;
		}
		
		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.m_un32ObjID = memRead.ReadUInt32 ();
			this.m_n32RoleNickName = Encoding.UTF8.GetChars (memRead.ReadBytes (20));
			this.m_n32CareerID = memRead.ReadInt32 ();
			this.m_bGender = memRead.ReadByte ();
			this.m_un32WeaponTypeID = memRead.ReadUInt32 ();
			this.m_un32CoatTypeID = memRead.ReadUInt32 ();
			this.m_un32Level = memRead.ReadUInt32 ();
			this.m_fCurPosX = memRead.ReadSingle ();
			this.m_fCurPosY = memRead.ReadSingle ();
			this.m_fCurPosZ = memRead.ReadSingle ();
			this.m_fTarPosX = memRead.ReadSingle ();
			this.m_fTarPosY = memRead.ReadSingle ();
			this.m_fTarPosZ = memRead.ReadSingle ();
			this.m_fMoveSpeed = memRead.ReadSingle ();
			this.m_un32CurTitleID = memRead.ReadUInt32 ();
			this.m_un32CurHp = memRead.ReadUInt32 ();
			int length = (int)eFighintPropertyCate.eFpc_End;
			for (int i = 1; i < length; i++) {
				fightProperty.fightData.Add ((eFighintPropertyCate)i, memRead.ReadInt32 ());
			}
			this.m_u32WingID = memRead.ReadUInt32 ();
			this.m_u32MedalID = memRead.ReadUInt32 ();
			this.m_u32PetID = memRead.ReadUInt32 ();
			memRead.Close ();
			memStream.Close ();
		}
	}
	
	/// <summary>
	/// 角色离开视野
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSObjectDisappear : NetHead
	{
		public UInt32 ObjNum;
		public UInt32[] m_un32ObjID;
		
		public GSObjectDisappear ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eG2CType.G2C_ObjectDisappear;
		}
		
		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.ObjNum = memRead.ReadUInt32 ();
			if (this.ObjNum > 0) {
				this.m_un32ObjID = new UInt32[this.ObjNum];
				for (int i = 0; i < this.ObjNum; i++) {
					this.m_un32ObjID [i] = memRead.ReadUInt32 ();
				}
			}
			
			memRead.Close ();
			memStream.Close ();
		}
	}

	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class GCAskLogout : NetHead
	{
		public GCAskLogout () : base()
		{
			this._assistantCmd = (UInt16)eC2GType.C2G_Logout;
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
		}

		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToBytes (ref memWrite);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	/// <summary>
	/// 其它玩家移动
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSObjectMove : NetHead
	{
		public UInt32 m_un32ObjID;
		public float m_fCurPosX;
		public float m_fCurPosY;
		public float m_fCurPosZ;
		public float m_fTarPosX;
		public float m_fTarPosY;
		public float m_fTarPosZ;
		public float m_fMoveSpeed;
		
		public GSObjectMove ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eG2CType.G2C_ObjectMove;
		}
		
		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.m_un32ObjID = memRead.ReadUInt32 ();
			this.m_fCurPosX = memRead.ReadSingle ();
			this.m_fCurPosY = memRead.ReadSingle ();
			this.m_fCurPosZ = memRead.ReadSingle ();
			this.m_fTarPosX = memRead.ReadSingle ();
			this.m_fTarPosY = memRead.ReadSingle ();
			this.m_fTarPosZ = memRead.ReadSingle ();
			this.m_fMoveSpeed = memRead.ReadSingle ();
			memRead.Close ();
			memStream.Close ();
		}
	}
	
	/// <summary>
	/// 请求移动
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskMove : NetHead
	{
		public UInt32 m_un32ObjID;
		public float m_fCurPosX;
		public float m_fCurPosY;
		public float m_fCurPosZ;
		public UInt32 node_num;
		public float m_fTarPosX;
		public float m_fTarPosY;
		public float m_fTarPosZ;
		
		public GCAskMove ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_AskMove;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
			
			if (this.node_num == 0) {
				this._length -= 12;
			}
			
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32ObjID);
			memWrite.Write (this.m_fCurPosX);
			memWrite.Write (this.m_fCurPosY);
			memWrite.Write (this.m_fCurPosZ);
			memWrite.Write (this.node_num);
			
			
			if (this.node_num == 1) {
				memWrite.Write (this.m_fTarPosX);
				memWrite.Write (this.m_fTarPosY);
				memWrite.Write (this.m_fTarPosZ);
			}
            
			
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	/// <summary>
	/// 其它玩家外观，等级变化
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyRoleProfileChange : NetHead
	{
		public UInt32 m_un32ObjID;
		public UInt32 m_un32WeaponTypeID;
		public UInt32 m_un32CoatTypeID;
		public UInt32 m_un32Level;
		
		public GSNotifyRoleProfileChange ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eG2CType.G2C_RoleInfoChange;
		}
		
		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.m_un32ObjID = memRead.ReadUInt32 ();
			this.m_un32WeaponTypeID = memRead.ReadUInt32 ();
			this.m_un32CoatTypeID = memRead.ReadUInt32 ();
			this.m_un32Level = memRead.ReadUInt32 ();
			memRead.Close ();
			memStream.Close ();
		}
	}
	
	/// <summary>
	/// 请求过关
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCReportPassGate : NetHead
	{
		public UInt32 m_un32MapID;
		public UInt32 m_un32SceneID;
		public UInt32 m_un32HPVessel;
		public UInt32 m_un32MPVessel;
		public UInt32 m_un32GotExpNum;
		public UInt32 m_un32GotSilverNum;
		public UInt32 m_un32UseSecond;
		public byte m_bPickAllTempGoods;

		public GCReportPassGate ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_PassGate;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32MapID);
			memWrite.Write (this.m_un32SceneID);
			memWrite.Write (this.m_un32HPVessel);
			memWrite.Write (this.m_un32MPVessel);
			memWrite.Write (this.m_un32GotExpNum);
			memWrite.Write (this.m_un32GotSilverNum);
			memWrite.Write (this.m_un32UseSecond);
			memWrite.Write (this.m_bPickAllTempGoods);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	/// <summary>
	/// 请求开宝箱
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskTreasureBoxDropGoods : NetHead
	{
		public UInt32 m_un32BoxID;
		public byte m_isShowAward;

		public GCAskTreasureBoxDropGoods ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_BoxDropGoods;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32BoxID);
			memWrite.Write (this.m_isShowAward);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	/// <summary>
	/// 请求进入场景
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskEnterScene : NetHead
	{
		public Int32 m_n32MapID;
		public Int32 m_n32SceneID;
		public byte m_bEnterByUI;
		public UInt32 m_un32HPVessel;
		public UInt32 m_un32MPVessel;
		public UInt32 m_un32GotExp;
		public UInt32 m_un32GotSilver;

		public GCAskEnterScene ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_EnterScene;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_n32MapID);
			memWrite.Write (this.m_n32SceneID);
			memWrite.Write (this.m_bEnterByUI);
			memWrite.Write (this.m_un32HPVessel);
			memWrite.Write (this.m_un32MPVessel);
			memWrite.Write (this.m_un32GotExp);
			memWrite.Write (this.m_un32GotSilver);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}

	/// <summary>
	/// 解析已经通关的地图列表
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyRoleMapScheduleList : NetHead
	{
		public UInt32 lineNum;
		public UInt32 mapID;

		public GSNotifyRoleMapScheduleList ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.lineNum = memRead.ReadUInt32 ();
			if (this.lineNum > 0) {
				byte easy;
				byte normal;
				byte hard;


				for (int i = 0; i < lineNum; ++i) {
					this.mapID = memRead.ReadUInt32 ();
					easy = memRead.ReadByte ();
					normal = memRead.ReadByte ();
					hard = memRead.ReadByte ();
					sPassMap passMap = new sPassMap ();
					passMap.mapID = this.mapID;
					passMap.easy = easy;
					passMap.normal = normal;
					passMap.hard = hard;
					passMap.easyUseSecond = memRead.ReadUInt32 ();
					passMap.normalUseSecond = memRead.ReadUInt32 ();
					passMap.hardUseSecond = memRead.ReadUInt32 ();
					Change (passMap);
				}
			}

			memRead.Close ();
			memStream.Close ();
		}

		void Change (sPassMap passMap)
		{
			if (RaidManager.Instance.PassMapHash.ContainsKey (passMap.mapID)) {
				RaidManager.Instance.PassMapHash [passMap.mapID] = passMap;
			} else {
				RaidManager.Instance.PassMapHash.Add (passMap.mapID, passMap);
			}
		}
	}
	
	/// <summary>
	/// 其它玩家外观，等级变化
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyRoleFightProperty : NetHead
	{
		public GSNotifyRoleFightProperty ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			int length = (int)eFighintPropertyCate.eFpc_End;
			for (int i = 1; i < length; i++) {
				eFighintPropertyCate type = (eFighintPropertyCate)i;
				CharacterPlayer.character_property.fightProperty.SetBaseValue (type, memRead.ReadInt32 ());
			}

            CharacterPlayer.character_property.m_nFightPower = (int)(memRead.ReadUInt32());

            PowerManager.Instance.IsFristGet = true;
			memRead.Close ();
			memStream.Close ();
			CharacterPlayer.character_property.setHPMax (CharacterPlayer.character_property.fightProperty.GetValue (eFighintPropertyCate.eFPC_MaxHP));
			CharacterPlayer.character_property.setMPMax (CharacterPlayer.character_property.fightProperty.GetValue (eFighintPropertyCate.eFPC_MaxMP));


			CharacterPlayer.character_property.setAttackPower (CharacterPlayer.character_property.fightProperty.GetValue (eFighintPropertyCate.eFPC_Attack));
			CharacterPlayer.character_property.setDefence (CharacterPlayer.character_property.fightProperty.GetValue (eFighintPropertyCate.eFPC_Defense));

            PowerManager.Instance.OpenWindow();
			EventDispatcher.GetInstance ().OnPlayerProperty ();
			EventDispatcher.GetInstance ().OnPlayerProperty ();
		}
	}
	/// <summary>
	/// 资产通知
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyAssetChange : NetHead
	{
		public GSNotifyAssetChange ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			int gold = memRead.ReadInt32 ();
			int diamond = memRead.ReadInt32 ();
			int crystal = memRead.ReadInt32 ();
			int honor = memRead.ReadInt32();
          
			memRead.Close ();
			memStream.Close ();

            //第一次登陆不添加漂字数据
            if (!PowerManager.Instance.IsFristGet)
            {
                PowerManager.Instance.PushResources(eGoldType.gold,
               gold - CharacterPlayer.character_asset.gold);
                PowerManager.Instance.PushResources(eGoldType.zuanshi,
                    diamond - CharacterPlayer.character_asset.diamond);
                PowerManager.Instance.PushResources(eGoldType.shuijing,
                    crystal - CharacterPlayer.character_asset.Crystal);
                PowerManager.Instance.PushResources(eGoldType.rongyu,
                    honor - CharacterPlayer.character_asset.Honor);
            }
            PowerManager.Instance.IsFristGet = true;

            Gate.instance.sendNotification(MsgConstant.MSG_ARENA_AWARD_REFRESH, honor.ToString());
			ArenaInfo info = ArenaManager.Instance.ArenaVo.ArenaInfo;
			info.currentHonor = (uint)honor;
			int spendMoney = gold - CharacterPlayer.character_asset.gold;
			int spendDiamond = diamond - CharacterPlayer.character_asset.diamond;
			CharacterPlayer.character_asset.gold = gold;
			CharacterPlayer.character_asset.diamond = diamond;
			CharacterPlayer.character_asset.Crystal = crystal;
			CharacterPlayer.character_asset.Honor = honor;
			EventDispatcher.GetInstance ().OnPlayerAsset ();
			Gate.instance.sendNotification (MsgConstant.MSG_WING_UPDATE_MONEY);
			if (spendMoney != 0)
				DataChangeNotifyer.GetInstance ().OnChangeCurrency (spendMoney); //消耗游戏币的表现
			if (spendDiamond != 0)
				DataChangeNotifyer.GetInstance ().OnChangeMoney (spendDiamond); //消耗RMB的表现    
			if (SweepManager.Instance.IsSweeping)
				SweepManager.Instance.setSweepResult (false, spendMoney);
			Gate.instance.sendNotification (MsgConstant.MSG_SHOP_DISPLAY_MONEY);
            PowerManager.Instance.OpenResourceWindow();
		}
	}

	/// <summary>
	/// 资产等级
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyLevelChange : NetHead
	{
		public GSNotifyLevelChange ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
            int level = memRead.ReadInt32();
            int type = memRead.ReadByte();
            switch (type)
            {
                case 0: //正常等级数据
                    if (CharacterPlayer.sPlayerMe)
                    {
                        //int preLevel = CharacterPlayer.character_property.getLevel();       //当前等级
                        CharacterPlayer.character_property.setLevel(level);

                        MessageManager.Instance.send_exp_dirty = true;

                        CharacterPlayer.character_property.setExperience(0);

                        MessageManager.Instance.sendExpChange();
                        Gate.instance.sendNotification(MsgConstant.MSG_GUIDE_SEND_TRIGGER,
                                           new Trigger(TriggerType.LevelTo, level));
                        GuideInfoManager.Instance.AddGuideTrigger(level);
                    }
                    GuideInfoManager.Instance.CheckItemTrigger();
                    break;
                case 1:
                    VipManager.Instance.UpdateVipLevel(level);
                    break;
                case 2:

                    uint index = 0x01;
                    for (int i = 0; i < VipManager.MAX_VIP_LEVEL; i++)
                    {
                        VipManager.Instance.UpdateVipInfo(i+1,VipManager.Bit(level, i));

                    }
                    VipManager.Instance.DisplayCurTable();
                    break;
                case 3:
                    VipManager.Instance.UpdateSumDiamond(level);
                    break;
                default:
                    break;
            }
			
			memRead.Close ();
			memStream.Close ();
			EventDispatcher.GetInstance ().OnPlayerLevel ();
			//通知等级变化
			Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_REFRESH_HEALT_MAGIC);
			Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_REFRESH_LEVEL);
			Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_EXP_CHANGE);
		}
	}

	/// <summary>
	/// 经验
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyExpChange : NetHead
	{
		public GSNotifyExpChange ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			int exp = memRead.ReadInt32 ();
            int addExp = exp - CharacterPlayer.character_property.getExperience();
			if (SweepManager.Instance.IsSweeping) {			
				SweepManager.Instance.setSweepResult (true, SweepManager.Instance.CurrentMap.sweepExp);
			}
			memRead.Close ();
			memStream.Close ();

            PowerManager.Instance.PushResources(eGoldType.exp, addExp);
            PowerManager.Instance.OpenResourceWindow();

			CharacterPlayer.character_property.setExperience (exp);
			MessageManager.Instance.sendExpChange ();
			EventDispatcher.GetInstance ().OnPlayerProperty ();
			Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_EXP_CHANGE);
		}
	}

	/// <summary>
	/// 血量/经验
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyCurProperty : NetHead
	{
		public GSNotifyCurProperty ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			if (CharacterPlayer.sPlayerMe) {
				CharacterPlayer.character_property.setHP (memRead.ReadInt32 ());
				CharacterPlayer.character_property.SetMP (memRead.ReadInt32 ());
	
				CharacterPlayer.character_property.setCurHPVessel (memRead.ReadUInt32 ());
				CharacterPlayer.character_property.setCurMPVessel (memRead.ReadUInt32 ());
			}

			memRead.Close ();
			memStream.Close ();
			EventDispatcher.GetInstance ().OnPlayerAsset ();
           
		}
	}

	/// <summary>
	/// 经验
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyDisplayGateIncome : NetHead
	{
		public GSNotifyDisplayGateIncome ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			memRead.Close ();
			memStream.Close ();
		}
	}

	/// <summary>
	/// 请求复活
	/// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskRelive : NetHead
	{
		byte _reliveType;
		byte _assetType;
		UInt32 m_un32AssetNum;

		public GCAskRelive (ReliveType reliveType, eGoldType  assetType, UInt32 num)
            : base()
		{
			this._reliveType = (byte)reliveType;
			this._assetType = (byte)assetType;
			this.m_un32AssetNum = num;

			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_AskRelive;
		}

		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToBytes (ref memWrite);
			memWrite.Write (this._reliveType);
			memWrite.Write (this._assetType);
			memWrite.Write (this.m_un32AssetNum);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCReportExp : NetHead
	{
		public UInt32 m_un32GotExp;
		
		public GCReportExp ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_ReportExp;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32GotExp);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//使用技能通知
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCReportUseSkill : NetHead
	{
		public UInt32 m_un32ObjID;
		public UInt32 m_un32SkillID;
		public float m_fPosX;
		public float m_fPosY;
		public float m_fPosZ;
		public float m_fDirX;
		public float m_fDirY;
		public float m_fDirZ;
		
		public GCReportUseSkill ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_ReportUseSkill;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32ObjID);
			memWrite.Write (this.m_un32SkillID);
			memWrite.Write (this.m_fPosX);
			memWrite.Write (this.m_fPosY);
			memWrite.Write (this.m_fPosZ);
			memWrite.Write (this.m_fDirX);
			memWrite.Write (this.m_fDirY);
			memWrite.Write (this.m_fDirZ);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//其它玩家使用技能
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifySkillReleased : NetHead
	{
		public UInt32 m_un32ObjectID;
		public UInt32 m_un32SkillID;
		public float m_fPosX;
		public float m_fPosY;
		public float m_fPosZ;
		public float m_fDirX;
		public float m_fDirY;
		public float m_fDirZ;
		
		public GSNotifySkillReleased ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.m_un32ObjectID = memRead.ReadUInt32 ();
			this.m_un32SkillID = memRead.ReadUInt32 ();
			this.m_fPosX = memRead.ReadSingle ();
			this.m_fPosY = memRead.ReadSingle ();
			this.m_fPosZ = memRead.ReadSingle ();
			this.m_fDirX = memRead.ReadSingle ();
			this.m_fDirY = memRead.ReadSingle ();
			this.m_fDirZ = memRead.ReadSingle ();
			memRead.Close ();
			memStream.Close ();
		}
	}
	
	//对象出现
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCReportOBjectAppear : NetHead
	{
		public UInt32 m_un32ObjID;
		public UInt32 m_un32TempID;
		public float m_fPosX;
		public float m_fPosY;
		public float m_fPosZ;
		public float m_fDirX;
		public float m_fDirY;
		public float m_fDirZ;
		
		public GCReportOBjectAppear ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_ReportOBjectAppear;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32ObjID);
			memWrite.Write (this.m_un32TempID);
			memWrite.Write (this.m_fPosX);
			memWrite.Write (this.m_fPosY);
			memWrite.Write (this.m_fPosZ);
			memWrite.Write (this.m_fDirX);
			memWrite.Write (this.m_fDirY);
			memWrite.Write (this.m_fDirZ);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//收到对象出现
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyObjectAppear : NetHead
	{
		public UInt32 m_un32ObjID;
		public UInt32 m_un32TempID;
		public float m_fPosX;
		public float m_fPosY;
		public float m_fPosZ;
		public float m_fDirX;
		public float m_fDirY;
		public float m_fDirZ;
		
		public GSNotifyObjectAppear ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.m_un32ObjID = memRead.ReadUInt32 ();
			this.m_un32TempID = memRead.ReadUInt32 ();
			this.m_fPosX = memRead.ReadSingle ();
			this.m_fPosY = memRead.ReadSingle ();
			this.m_fPosZ = memRead.ReadSingle ();
			this.m_fDirX = memRead.ReadSingle ();
			this.m_fDirY = memRead.ReadSingle ();
			this.m_fDirZ = memRead.ReadSingle ();
			memRead.Close ();
			memStream.Close ();
		}
	}
	
	//对象消亡
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCReportOBjectDisappear : NetHead
	{
		public UInt32 m_un32ObjID;
		public Int32 m_n32Reason;
		
		public GCReportOBjectDisappear ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_ReportOBjectDisappear;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32ObjID);
			memWrite.Write (this.m_n32Reason);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//收到对象
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyObjectDisappear : NetHead
	{
		public UInt32 m_un32ObjID;
		public Int32 m_n32Reason;
		
		public GSNotifyObjectDisappear ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.m_un32ObjID = memRead.ReadUInt32 ();
			this.m_n32Reason = memRead.ReadInt32 ();
			memRead.Close ();
			memStream.Close ();
		}
	}
	
	//对象行为
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCReportObjectAction : NetHead
	{
		public UInt32 m_un32ObjID;
		public Int32 m_n32ActionCate;
		public float m_fDirX;
		public float m_fDirY;
		public float m_fDirZ;
		public float m_fPosX;
		public float m_fPosY;
		public float m_fPosZ;
		
		public GCReportObjectAction ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_ReportObjectAction;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32ObjID);
			memWrite.Write (this.m_n32ActionCate);
			memWrite.Write (this.m_fDirX);
			memWrite.Write (this.m_fDirY);
			memWrite.Write (this.m_fDirZ);
			memWrite.Write (this.m_fPosX);
			memWrite.Write (this.m_fPosY);
			memWrite.Write (this.m_fPosZ);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//广播对象行为
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyObjectAction : NetHead
	{
		public UInt32 m_un32ObjID;
		public Int32 m_n32ActionCate;
		public float m_fDirX;
		public float m_fDirY;
		public float m_fDirZ;
		public float m_fPosX;
		public float m_fPosY;
		public float m_fPosZ;
		
		public GSNotifyObjectAction ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.m_un32ObjID = memRead.ReadUInt32 ();
			this.m_n32ActionCate = memRead.ReadInt32 ();
			this.m_fDirX = memRead.ReadSingle ();
			this.m_fDirY = memRead.ReadSingle ();
			this.m_fDirZ = memRead.ReadSingle ();
			this.m_fPosX = memRead.ReadSingle ();
			this.m_fPosY = memRead.ReadSingle ();
			this.m_fPosZ = memRead.ReadSingle ();
			memRead.Close ();
			memStream.Close ();
		}
	}
	
	//对怪物伤害
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCReportObjectHurm : NetHead
	{
		public byte m_bHost;
		public UInt32 m_un32ObjID;
		public Int32 m_n32HPValue;
		public Int32 m_n32CurHP;
		public Int32 m_n32Effect;
		
		public GCReportObjectHurm ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_ReportObjectHurm;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_bHost);
			memWrite.Write (this.m_un32ObjID);
			memWrite.Write (this.m_n32HPValue);
			memWrite.Write (this.m_n32CurHP);
			memWrite.Write (this.m_n32Effect);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//广播对象怪物伤害 但是只有主机需要处理
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GSNotifyObjectHurm : NetHead
	{
		public byte m_bHost;
		public UInt32 m_un32ObjID;
		public Int32 m_n32HPValue;
		public Int32 m_n32HP;
		public Int32 m_n32Effect;
		
		public GSNotifyObjectHurm ()
            : base()
		{

		}

		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.m_bHost = memRead.ReadByte ();
			this.m_un32ObjID = memRead.ReadUInt32 ();
			this.m_n32HPValue = memRead.ReadInt32 ();
			this.m_n32HP = memRead.ReadInt32 ();
			this.m_n32Effect = memRead.ReadInt32 ();
			memRead.Close ();
			memStream.Close ();
		}
	}

	//请求进入塔 45
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskEnterTowerInstance : NetHead
	{
		/// <summary>
		/// 请求进入爬塔副本的ID
		/// </summary>
		public UInt32 m_un32TowerId;
		/// <summary>
		/// 是否从当前波数进入,1是0否
		/// </summary>
		public byte m_bCurWave;

		public GCAskEnterTowerInstance ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_AskEnterTowerInstance;
		}

		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32TowerId);
			memWrite.Write (this.m_bCurWave);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}

	//汇报成绩 46
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCReportTowerInstanceScore : NetHead
	{
		/// <summary>
		/// 爬塔的波数ID
		/// </summary>
		public UInt32 m_un32TowerId;

		public GCReportTowerInstanceScore ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_ReportTowerInstanceScore;
		}

		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32TowerId);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}

	//排名 47
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskTowerInstanceRank : NetHead
	{
		/// <summary>
		/// 1请求爬塔基本信息,2请求自己昨日排名,3请求今日爬塔排名
		/// </summary>
		public UInt16 m_un16Type;

		public GCAskTowerInstanceRank ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_AskTowerInstanceRank;
		}

		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un16Type);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}

	//奖品
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskTowerInstanceAward : NetHead
	{
		/// <summary>
		/// 请求的奖励关卡ID
		/// </summary>
		public UInt32 m_u32TowerID;
		/// <summary>
		/// 请求的奖励类型,0代表领取目标奖励,1代表昨日恶魔排名,2代表地狱排名,3代表炼狱排名
		/// </summary>
		public UInt16 m_u16TowerType;

		public GCAskTowerInstanceAward ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_AskTowerInstanceAward;
		}

		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (m_u32TowerID);
			memWrite.Write (m_u16TowerType);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	
	//请求邮件列表
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskEMailList : NetHead
	{		
		public GCAskEMailList () : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_AskEMailList;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//请求阅读邮件
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskReadEMail : NetHead
	{		
		public UInt32 m_un32EMailID = 0;
		
		public GCAskReadEMail () : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_AskReadEMail;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32EMailID);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//请求邮件奖品
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskGetEMailPrize : NetHead
	{		
		public UInt32 m_un32EMailID = 0;
		
		public GCAskGetEMailPrize () : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_AskGetEMailPrize;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32EMailID);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//请求删除邮件
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskRemoveEMail : NetHead
	{		
		public UInt32 m_un32EMailID = 0;
		
		public GCAskRemoveEMail () : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_AskRemoveEMail;
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_un32EMailID);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//通知排行列表
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class GSNotifyRankList : NetHead
	{
		public Byte m_n8RankCate;
		public Int32 m_n32SelfRank;
		public Int32 m_n32SelfRankValue;
		public Int32 m_n32RankNum;
		
		public GSNotifyRankList () : base()
		{
			
		}
		
		public void ToObject (byte[] bytesData)
		{
		}
	}



	//通知邮件列表
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class GSNotifyEMailList : NetHead
	{
		public UInt32	m_un32EMailNum;
		
		public GSNotifyEMailList () : base()
		{
		}
		
		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			this.m_un32EMailNum = memRead.ReadUInt32 ();

            //EmailManager.Instance.ClearEmail();
			for (Int32 i = 0; i < this.m_un32EMailNum; i++) {
                
				UInt32 un32EMailID = memRead.ReadUInt32 ();
				

				UInt16 un16TitleLen = memRead.ReadUInt16 ();
				byte[] titleByteStr = memRead.ReadBytes ((int)un16TitleLen);
				string title = new string (Encoding.UTF8.GetChars (titleByteStr));
				
				UInt16 un16ContentLen = memRead.ReadUInt16 ();
				byte[] contentByteStr = memRead.ReadBytes ((int)un16ContentLen);
				string content = new string (Encoding.UTF8.GetChars (contentByteStr));
				
				uint exp = memRead.ReadUInt32 ();
				uint yibi = memRead.ReadUInt32 ();
				uint diamon = memRead.ReadUInt32 ();
				uint itemid = memRead.ReadUInt32 ();
				uint itemnum = memRead.ReadUInt32 ();
				bool isreceive = memRead.ReadBoolean ();
				bool isread = memRead.ReadBoolean ();

				EmailManager.Instance.AddEmailInfo ((int)un32EMailID, title, content, (int)itemid, (int)itemnum, isreceive, isread);
			}
            //if (UIManager.Instance.isWindowOpen(UiNameConst.ui_email))
            //{
            //    EmailManager.Instance.RefreshWindow();
            //}
			memRead.Close ();
			memStream.Close ();
		}
	}
	
	//发送创建角色消息
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCSendCreateRole : NetHead
	{
		private UInt32 _u32VocationID;  //职业ID
		private byte _byteGender;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]       
		private byte[] _nickName;		//用户昵称
		private byte _byteVerify;
		public static processCreateRole createRole;

		public GCSendCreateRole (int vocationID, bool isMale, string nickName, bool isVerify, processCreateRole callBack)
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_CreateRole;
			this._u32VocationID = (UInt32)vocationID;
			GCSendCreateRole.createRole = callBack;
			if (isMale)
				_byteGender = 0x01;
			else
				_byteGender = 0x00;	
			if (isVerify)
				_byteVerify = 0x01;
			else
				_byteVerify = 0x00;
			this._nickName = StringToByte (nickName, 20);      
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this._u32VocationID);
			memWrite.Write (this._byteGender);
			memWrite.Write (this._nickName);
			memWrite.Write (this._byteVerify);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//发送创建角色消息
	//ToGSFromGC_GCAskRankList
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskRankList : NetHead
	{
		private Byte m_n8RankCate;
		
		public GCAskRankList () : base()
		{
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_n8RankCate);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
    
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAsChargeIOS : NetHead
	{
		private byte m_bIsDebug;
		private UInt32 m_strLen;
		private byte[] m_szAppleReceipt;
		
		public GCAsChargeIOS (bool bIsDebug, string szAppleReceipt) : base()
		{
			this._assistantCmd = (UInt16)eC2GType.C2G_AskChargeIOS;
			if (bIsDebug) {
				this.m_bIsDebug = 1;
			} else {
				this.m_bIsDebug = 0;
			}
			this.m_strLen = (UInt32)szAppleReceipt.Length;
			this.m_szAppleReceipt = StringToByte (szAppleReceipt);

			NetHead head = new NetHead ();
			int headLength = Marshal.SizeOf (head) - 2;
			
			this._length = (UInt16)(headLength + 1 + 4 + this.m_strLen);
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this.m_bIsDebug);
			memWrite.Write (this.m_strLen);
			memWrite.Write (this.m_szAppleReceipt);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCNotifyChargeIOS : NetHead
	{
		public Int32 m_nResult;
		public UInt32 m_strLen;
		public string m_szAppleReceipt;
		
		public GCNotifyChargeIOS ()
            : base()
		{
			//this._length = (UInt16)(Marshal.SizeOf(this) - 2);
			//this._assistantCmd = (UInt16)eC2GType.G2C_NotifyChargeIOS;
		}
		
		public void ToObject (byte[] bytesData)
		{
			MemoryStream memStream = new MemoryStream (bytesData);
			BinaryReader memRead = new BinaryReader (memStream, Encoding.GetEncoding ("utf-8"));
			base.ToObject (ref memRead);
			
			this.m_nResult = memRead.ReadInt32 ();
			this.m_strLen = memRead.ReadUInt32 ();
			Debug.Log ("result: " + m_nResult + " strlen: " + m_strLen);
			this.m_szAppleReceipt = new string (Encoding.UTF8.GetChars (memRead.ReadBytes ((int)m_strLen)));
			if (m_nResult == 0) {
				FloatMessage.GetInstance ().PlayNewFloatMessage ("恭喜,充值成功! ", false, UIManager.Instance.getRootTrans ());
			} else {
				FloatMessage.GetInstance ().PlayNewFloatMessage ("泥马,充值失败! ", false, UIManager.Instance.getRootTrans ());
			}
#if UNITY_IPHONE
//			UnityToXcodeMsgInterface.ReceivePaymentResult(this.m_nResult, this.m_szAppleReceipt, (int)m_strLen);
#endif	
			memRead.Close ();
			memStream.Close ();
		}
	}
    
	//发送制作消息
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCReportProcuce : NetHead
	{
		private UInt32 _bookID;  //制作书ID
		private byte _bUseDiamond; //是否使用钻石

		public GCReportProcuce (UInt32 bookID, bool useDiamond)
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_ReportProduce;
			this._bookID = bookID;
			if (useDiamond)
				_bUseDiamond = 0x01;
			else
				_bUseDiamond = 0x00;				
		}
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this._bookID);
			memWrite.Write (this._bUseDiamond);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}
	
	//请求章节奖励
	[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class GCAskChapterAward: NetHead
	{
		private UInt16 _chapterNum;
		private byte _hardChapter;
		
		public GCAskChapterAward ()
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
			this._assistantCmd = (UInt16)eC2GType.C2G_AskChapterAward;
		}
		
		public byte[] ToBytes (UInt16 chapterNum, byte hardChapter)
		{
			this._chapterNum = chapterNum;
			this._hardChapter = hardChapter;
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);
			memWrite.Write (this._chapterNum);
			memWrite.Write (this._hardChapter);
			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
	}



    //请求勋章信息
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskMedalInfo : NetHead
    {
        public GCAskMedalInfo()
            : base()
        {
            this._length = (UInt16)(Marshal.SizeOf(this) - 2);
            this._assistantCmd = (UInt16)eC2GType.C2G_AskMedalLevel;
        }

        public byte[] ToBytes()
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
            base.ToBytes(ref memWrite);
            byte[] bytesData = memStream.ToArray();
            memWrite.Close();
            return bytesData;
        }
    }
    //请求勋章升级
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskMedalLevelUp : NetHead
    {
        public GCAskMedalLevelUp()
            : base()
        {
            this._length = (UInt16)(Marshal.SizeOf(this) - 2);
            this._assistantCmd = (UInt16)eC2GType.C2G_AskMedalLevelUp;
        }

        public byte[] ToBytes()
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
            base.ToBytes(ref memWrite);
            byte[] bytesData = memStream.ToArray();
            memWrite.Close();
            return bytesData;
        }
    }
//基本好友功能,108
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskAddFriend : NetHead
    {
        //  1:添加好友.2:拒绝好友.3:同意好友申请.4:删除好友.5:赠送好友体力.6:接收好友体力
        public enum FriendSendType
        { 
            /// <summary>
            /// 1:添加好友
            /// </summary>
            AddFriend=1,
            /// <summary>
            /// 2:拒绝好友
            /// </summary>
            Not,
            /// <summary>
            /// 3:同意好友申请
            /// </summary>
            Ok,
            /// <summary>
            /// 4:删除好友
            /// </summary>
            DeleteFriend,
            /// <summary>
            /// 5:赠送好友体力.
            /// </summary>
            SendAward,
            /// <summary>
            /// .6:接收好友体力
            /// </summary>
            ReceiveAward,
        }
        Int32 _FriendOpt;  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] _roleName;

        public GCAskAddFriend(Int32 nFriendOpt, String roleName)
            : base()
		{
			this._length = (UInt16)(Marshal.SizeOf (this) - 2);
            this._assistantCmd = (UInt16)eC2GType.C2G_AskAddFriend;

            _FriendOpt = nFriendOpt;
            _roleName = StringToByte(roleName, 20);
		}
        public GCAskAddFriend(FriendSendType nFriendOpt, String roleName)
            : base()
        {
            this._length = (UInt16)(Marshal.SizeOf(this) - 2);
            this._assistantCmd = (UInt16)eC2GType.C2G_AskAddFriend;

            _FriendOpt = (int)nFriendOpt;
            _roleName = StringToByte(roleName, 20);
        }
		
		public byte[] ToBytes ()
		{
			MemoryStream memStream = new MemoryStream ();
			BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.UTF8);
			base.ToBytes (ref memWrite);

            memWrite.Write(this._FriendOpt);
			memWrite.Write (this._roleName);

			byte[] bytesData = memStream.ToArray ();
			memWrite.Close ();
			return bytesData;
		}
    }
    




   //好友VIP功能,109
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCVipFriend : NetHead
    {
       
        Int32 _FriendOpt;
        public GCVipFriend(Int32 nFriendOpt)
            : base()
        {
            this._length = (UInt16)(Marshal.SizeOf(this) - 2);
            this._assistantCmd = (UInt16)eC2GType.C2G_VipFriend;
            _FriendOpt = nFriendOpt;
        }

        public byte[] ToBytes()
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
            base.ToBytes(ref memWrite);
            memWrite.Write(this._FriendOpt);
            
            byte[] bytesData = memStream.ToArray();
            memWrite.Close();
            return bytesData;
        }
    }

    //请求好友列表,110
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskFriendRecord : NetHead
    {
        public GCAskFriendRecord()
            : base()
        {
            this._length = (UInt16)(Marshal.SizeOf(this) - 2);
            this._assistantCmd = (UInt16)eC2GType.C2G_AskFriendRecord;
        }

        public byte[] ToBytes()
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
            base.ToBytes(ref memWrite);
            byte[] bytesData = memStream.ToArray();
            memWrite.Close();
            return bytesData;
        }
    }
    //
    //请求查询玩家
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskSelectPlayer : NetHead
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        private byte[] _byte;
        public GCAskSelectPlayer()
            : base()
        {
            this._length = (UInt16)(Marshal.SizeOf(this) - 2);
            this._assistantCmd = (UInt16)eC2GType.C2G_AskSelectPlayer;
        }

        public void SetName(string name)
        {
            _byte = StringToByte(name, 20);
        }

        public byte[] ToBytes()
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
            base.ToBytes(ref memWrite);
            memWrite.Write(_byte);
            byte[] bytesData = memStream.ToArray();
            memWrite.Close();
            return bytesData;
        }
    }
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskReceiveArenaAward : NetHead
    {
        public GCAskReceiveArenaAward()
            : base()
        {
            this._length = (UInt16)(Marshal.SizeOf(this) - 2);
            this._assistantCmd = (UInt16)eC2GType.C2G_AskReceiveArenaAward;
        }
        public byte[] ToBytes()
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
            base.ToBytes(ref memWrite);
            byte[] bytesData = memStream.ToArray();
            memWrite.Close();
            return bytesData;
        }
    }

    /// <summary>
    /// 请求新手引导完成保存
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GCAskGuideComplate : NetHead
    {
        public UInt16 GroupId;
        public byte u8Finish;

        public GCAskGuideComplate()
            : base()
        {
            this._length = (UInt16)(Marshal.SizeOf(this) - 2);
            this._assistantCmd = (UInt16)eC2GType.C2G_AskGuideComplate;
        }
        public byte[] ToBytes()
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
            base.ToBytes(ref memWrite);
            memWrite.Write(GroupId);
            memWrite.Write(u8Finish);
            byte[] bytesData = memStream.ToArray();
            memWrite.Close();
            return bytesData;
        }
    }
}

    