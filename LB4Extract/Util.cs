
using System.Collections.Generic;

namespace LB4Extract
{
    public class Util
    {
        public static uint getLEValue(byte[] val)
        {
            uint weight = 1;
            uint ret = 0;
            for(int i = 0;i < val.Length;i++)
            {
                ret += val[i] * weight;
                weight *= 256;
            }
            return ret;
        }
        /// <summary>
        /// Extract byte array from source array with index and length
        /// </summary>
        /// <param name="source">byte array to be extracted</param>
        /// <param name="firstIndex">first index of array to extract</param>
        /// <param name="count">byte array length</param>
        /// <returns>the byte array</returns>
        public static byte[] SubBytes(byte[] source,uint firstIndex,uint length)
        {
            List<byte> list = new List<byte>();
            for(int i = 0;i < length;i++)
            {
                list.Add(source[firstIndex + i]);
            }
            return list.ToArray();
        }
    }
}
