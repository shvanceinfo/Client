using System;

namespace NetGame
{
    class NetBuff
    {
        public const int DEFAULT_BUFF_LENGTH = 1024;
        private byte[] _buff = null;
        private int _length = 0;
        public NetBuff()
        {
            _buff = new byte[DEFAULT_BUFF_LENGTH];
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public void AddBuff(byte[] data, int size)
        {
            int newLength = _length + size;
            if (newLength > _buff.Length)
            {
                Array.Resize<byte>(ref _buff, newLength);
            }
            Array.Copy(data, 0, _buff, _length, size);
            _length = newLength;
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        public void ClearBuff()
        {            
            _length = 0;
        }

        public void ReplaceBuff(byte[] data, int size)
        {
            if (_length < size)
            {
                Console.WriteLine("ReplaceBuff length, want " + size + " real " + _length);
                return;
            }
            Array.Copy(data, _buff, size);
            Array.Copy(_buff, size, _buff, 0, _length - size);
        }

        public void DrainBuff(int size)
        {
            if (_length < size)
            {
                Console.WriteLine("DrainBuff length error: want {0} real {1}", size, _length);
                _length = size;
            }
            Array.Copy(_buff, size, _buff, 0, _length - size);
            _length -= size;
        }

        public byte[] GetBuff()
        {
            return _buff;
        }

        public int GetLength()
        {
            return _length;
        }
    }
}
