using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PracticeProgram
{
    public class EncriptHelper
    {
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(password)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string password)
        {
            string cl = password;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x2");
            }
            return pwd;
        }
        /// <summary>
        /// 64位
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt64(string password)
        {
            string cl = password;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }


        public static string GetMD5(string myString)
        {
            string cl = myString;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
            //MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            //byte[] targetData = md5.ComputeHash(fromData);
            //string byte2String = null;

            //for (int i = 0; i < targetData.Length; i++)
            //{
            //    byte2String += targetData[i].ToString("x");
            //}

            //return byte2String;
        }

        /*-+-+-+-+-+-+-+-+-+-+-+-+公共方法-+-+-+-+-+-+-+-+-+-+-+-+*/

        /*-+-+-+-+-+-+-+-+-+-+-+-+私有方法-+-+-+-+-+-+-+-+-+-+-+-+*/
        // 返回形式为数字跟字符串
        private static String byteToArrayString(byte bByte)
        {
            int iRet = bByte;
            // System.out.println("iRet="+iRet);
            if (iRet < 0)
            {
                iRet += 256;
            }
            int iD1 = iRet / 16;
            int iD2 = iRet % 16;
            return strDigits[iD1] + strDigits[iD2];
        }

        // 返回形式只为数字
        private static String byteToNum(byte bByte)
        {
            int iRet = bByte;
            if (iRet < 0)
            {
                iRet += 256;
            }
            return iRet + "";
        }

        // 转换字节数组为16进制字串
        private static String byteToString(byte[] bByte)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bByte.Length; i++)
            {
                sb.Append(byteToArrayString(bByte[i]));
            }
            return sb.ToString();
        }
        /***********************私有方法**********************************/

        /***********************字段和属性**********************************/
        // 全局数组
        private readonly static String[] strDigits = { "0", "1", "2", "3", "4", "5",
            "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
        /***********************字段和属性**********************************/
    }
}
