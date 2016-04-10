using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace manager
{
    /// <summary>
    /// 过滤关键字
    /// </summary>
    public class LeachDirtyManager
    {
        private Hashtable _table;
        private static uint MAX = 3;
        private const char START = '*';
        private LeachDirtyManager()
        {
            _table = new Hashtable();
            ReadText();
        }
        private void ReadText()
        {
            TextAsset txt = null;
            if (BundleMemManager.debugVersion)
            {
                txt = BundleMemManager.Instance.loadResource(PathConst.LEACHDIRTY, typeof(TextAsset)) as TextAsset;
            }
            else
            {
                AssetBundle bundle = BundleMemManager.Instance.ConfigBundle;
                txt = bundle.Load(ToolFunc.TrimPath(PathConst.LEACHDIRTY)) as TextAsset;
            }    
           
            string[] ld = txt.text.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (!BundleMemManager.debugVersion)
            {
                AssetBundle bundle = BundleMemManager.Instance.ConfigBundle;
                bundle.Unload(true);
				BundleMemManager.Instance.ConfigBundle = null;
                BundleItem item = BundleMemManager.Instance.getBundleByType(EBundleType.eBundleConfig);
                BundleMemManager.Instance.AllSceneBundles.Remove(item.bundleName + BundleMemManager.BUNDLE_SUFFIX);
            }               
            uint key = 0;
            for (int i = 0; i < ld.Length; i++)
            {
                if (ld[i].Length > MAX) MAX = (uint)ld[i].Length;
                key = BytesToSum(ld[i]);
                if (_table.ContainsKey(key))
                {
                    (_table[key] as List<string>).Add(ld[i]);
                }
                else
                {
                    _table.Add(key, new List<string> { ld[i] });
                }

            }
        }

        /// <summary>
        /// 写非法关键字到文件
        /// </summary>
        /// <param name="url">物理路径</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public void WriteFilterText(string content)
        {
            FileStream fs = new FileStream(PathConst.LEACHDIRTY, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            sw.Write(content);
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 过滤非法字符,替换为*号
        /// </summary>
        /// <param name="TempStr">需要过滤的字符串</param>
        /// <returns></returns>
        public string FilterInfo(string TempStr)
        {
            return DoSplit(TempStr);
        }

        /// <summary>
        /// 检查名称是否包含非法名称
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool CheckFilter(string content)
        {
            int index=DoSplit(content).IndexOf(START);
            if (index == -1)
            {
                return false;
            }
            else {
                return true;
            }
        }

        /// <summary>
        /// 过滤非法关键字
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string DoSplit(string text)
        {
            string temp = text;
            for (int i = 0; i < temp.Length; i++)
            {
                ReSplit(ref temp, i);
            }
            return temp;
        }
        private void ReSplit(ref string text, int index)
        {
            string temp = "";
            int lastLen = text.Length - index;
            int len = MAX < lastLen ? (int)MAX : lastLen;

            for (int i = 0; i < len; i++)
            {
                temp += text[index + i];
                uint key = BytesToSum(temp);
                if (_table.ContainsKey(key))
                {
                    List<string> sp = _table[key] as List<string>;
                    for (int k = 0; k < sp.Count; k++)
                    {
                        text = text.Replace(sp[k], "".PadLeft(sp[k].Length, START));
                    }
                }
            }
        }

        private uint Sum(byte[] bs)
        {
            uint sum = 0;
            for (int i = 0; i < bs.Length; i++)
            {
                sum += bs[i];
            }
            return sum;
        }

        private uint BytesToSum(string text)
        {
            return Sum(Encoding.UTF8.GetBytes(text));
        }

        #region 单例
        private static LeachDirtyManager _instance;
        public static LeachDirtyManager Instance
        {
            get
            {
                if (_instance == null) 
					_instance = new LeachDirtyManager();
                return _instance;
            }
        }
        #endregion
      
    }
}
