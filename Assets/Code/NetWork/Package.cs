using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using NetPackage;
using ProtoBuf;

namespace NetGame
{
    class Package
    {
        /// <summary>
        /// 结构体转换成byte数组
        /// </summary>
        /// <param name="obj">结构体实例</param>
        /// <returns></returns>
        //public static byte[] StructToBytes(object obj)
        //{
        //    int size = Marshal.SizeOf(obj);
        //    byte[] bytes = new byte[size];
        //    IntPtr structPtr = Marshal.AllocCoTaskMem(size);
        //    Marshal.StructureToPtr(obj, structPtr, false);
        //    Marshal.Copy(structPtr, bytes, 0, size);
        //    Marshal.FreeHGlobal(structPtr);
        //    return bytes;
        //}
        /// <summary>
        /// byte数组转成结构体
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //public static object BytesToStruct(byte[] bytes, Type type)
        //{
        //    int size = Marshal.SizeOf(type);
        //    if (size > bytes.Length)
        //    {
        //        return null;
        //    }
        //    IntPtr structPtr = Marshal.AllocHGlobal(size);
        //    Marshal.Copy(bytes, 0, structPtr, size);
        //    object obj = Marshal.PtrToStructure(structPtr, type);
        //    Marshal.FreeHGlobal(structPtr);
        //    return obj;
        //}
        /// <summary>
        /// 封包处理
        /// </summary>
        /// <param name="objStruct">结构体</param>
        /// <returns></returns>
        //public static byte[] Pack(object objStruct)
        //{
        //    //总数据长度
        //    int length= Marshal.SizeOf(objStruct);
            
        //    byte[] bytesData = new byte[length];
        //    Array.Copy(StructToBytes(objStruct), 0, bytesData, 0, length);
        //    return bytesData;
        //}
        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="bytesData"></param>
        /// <returns></returns>
        //public static T UnPack<T>(byte[] bytesData)
        //{
        //    T obj ;
        //    obj = (T)BytesToStruct(bytesData, typeof(T));
        //    return obj;
        //}
        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="mesg"></param>
        public static void Parse(ref NetBuff buff, ref IMessage mesg)
        {
            while (true)
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(buff.GetBuff(), 0, buff.GetLength());
                CNetHead chead = null;
                chead = Serializer.Deserialize<CNetHead>(ms);

                if (chead._length == 0)
                {
                    return;
                }

//                 NetHead head = new NetHead();               
//                 int headLength = Marshal.SizeOf(head);
//                 if (buff.GetLength() < headLength)
//                 {
//                     return;
//                 }
//                 head.ToObject(buff.GetBuff());
//                 int realLength = head._length + 2;
//                 if (buff.GetLength() < realLength)
//                 {
//                     return;
//                 }
                byte[] bytesData = new byte[chead.ToString().Length];
                Buffer.BlockCopy(buff.GetBuff(), 0, bytesData, 0, chead.ToString().Length);
                buff.DrainBuff(chead.ToString().Length);
				MessageManager.Instance.pushMeassage(bytesData);
                ShowLogre(chead);
            }
        }

        static void ShowLogre(NetHead head)
        {
            //if (NetBase.DEBUGER)
            {
                UnityEngine.Debug.Log("receive message type=" + head._assistantCmd + ",headLen=" + head._length);
            }
        }

        static void ShowLogre(CNetHead head)
        {
            {
                UnityEngine.Debug.Log("receive message type=" + head._assistantCmd + ",headLen=" + head._length);
            }
        }
    }
}
