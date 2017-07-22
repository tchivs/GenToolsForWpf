﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenEngine
{/// <summary>
 /// 十六进制的静态方法
 /// </summary>
    public static class HexConvert
    {
        /// <summary>
        /// PCI转换到十六进制方法
        /// </summary>
        /// <param name="str">要转换的数字</param>
        /// <returns></returns>
        public static string TenToHex(string str)
        {
            int number = Convert.ToInt32(str);
            string result = Convert.ToString(number, 16).ToUpper();
            if (result.Length == 3) result = result.Substring(1, 2) + "0" + result.Substring(0, 1);//前后互换位
            if (result.Length == 1) result = "0" + result + "00"; //补0
            if (result.Length == 2) result = result + "00"; //补0
            //int number = Convert.ToInt32(str);
            //string result = number.ToString("X2");
            return result;
        }

        /// <summary>
        ///  字节转十六进制
        /// </summary>
        /// <param name="tempByte">要转为十六进制的字节</param>
        /// <returns></returns>
        public static string ByteToHex(byte tempByte)
        {
            string tempStr = tempByte.ToString("X2");
            return tempStr;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  十六进制字符串转字节数组 </summary>
        ///
        /// <remarks>   Topiv, 2017-06-26. </remarks>
        ///
        /// <param name="s">   要转换的字符串 </param>
        ///
        /// <returns>   A byte[]. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

    }
}
