using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PracticeProgram
{
    public class HttpHelperP
    {
        HttpClient _httpClient;

        public HttpHelperP()
        {
            this._httpClient = new HttpClient();
            _httpClient.MaxResponseContentBufferSize = 256000;
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public string SendGet(string url, Dictionary<string, string> paramList=null)
        {
            if (paramList != null)
            {
                bool isStart = true;
                foreach (var kp in paramList)
                {
                    url += isStart ? "?" : "&";
                    isStart = false;
                    url += kp.Key;
                    url += "=";
                    url += kp.Value;
                }
            }
            HttpResponseMessage response = _httpClient.GetAsync(new Uri(url)).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public string SendPost(string url, Dictionary<string, string> paramList = null)
        {
            var httpContent = new FormUrlEncodedContent(paramList);
            var token = new CancellationToken(false);
            var resp = _httpClient.PostAsync(new Uri(url), httpContent, token);
            HttpResponseMessage response = resp?.Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }
        /// <summary>
        /// 登录博客园
        /// </summary>
        public static void LoginCnblogs()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            String url = "http://passport.cnblogs.com/login.aspx";
            HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
            String result = response.Content.ReadAsStringAsync().Result;

            String username = "stagebo";
            String password = "0.123456789dyi";

            do
            {
            

                //开始登录
                url = "http://passport.cnblogs.com/login.aspx";
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                paramList.Add(new KeyValuePair<string, string>("__EVENTTARGET", ""));
               
                response = httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(paramList)).Result;
                result = response.Content.ReadAsStringAsync().Result;
               
            } while (result.Contains("验证码错误，麻烦您重新输入"));

            Console.WriteLine("登录成功！");

            //用完要记得释放
            httpClient.Dispose();
        }
         

    }
}
