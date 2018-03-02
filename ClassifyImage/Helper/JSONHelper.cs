//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.Serialization.Json;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Script.Serialization;
//using Newtonsoft.Json;
//namespace PracticeProgram
//{
//   public class JSONHelper
//    {
//        public static Dictionary<string, string> getJson(string jsonString)
//        {
//            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
//            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
//            Dictionary<string, string> jsonObject = (Dictionary<string, string>)ser.ReadObject(ms);
//            return jsonObject;
//        }
//        /// <summary>
//        /// 将json数据反序列化为Dictionary
//        /// </summary>
//        /// <param name="jsonData">json数据</param>
//        /// <returns></returns>
//        public static Dictionary<string, object> JsonToDictionary(string jsonData)
//        {
//            //实例化JavaScriptSerializer类的新实例
//            JavaScriptSerializer jss = new JavaScriptSerializer();
//            try
//            {
//                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
//                return jss.Deserialize<Dictionary<string, object>>(jsonData);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//    }
//}
