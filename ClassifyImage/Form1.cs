using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ClassifyImage
{
    public partial class Form1 : Form
    {
        private HttpClient _httpClient;
        public Form1()
        {
            InitializeComponent();
            this._httpClient = new HttpClient();
            _httpClient.MaxResponseContentBufferSize = 256000;
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");

        }
      
        public string SendGet(string url, Dictionary<string, string> paramList = null)
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
        public string GetCityInfo(string[] gpsInfo) {
            string gpsw = gpsInfo[0];
            string gpsj = gpsInfo[1];
            //pwUSvedGW717yHq0waX20e3Vl7q5VXKg
            string url = "http://api.map.baidu.com/geocoder/v2/?callback=renderReverse&location={0},{1}&output=json&pois=1&ak=pwUSvedGW717yHq0waX20e3Vl7q5VXKg";
            url = string.Format(url, gpsw, gpsj);
            string rel = this.SendGet(url);
            int start = rel.IndexOf('(');
            rel = rel.Substring(start + 1, rel.Length - 2 - start);
            JObject jo = (JObject)JsonConvert.DeserializeObject(rel);
           
            string province = jo["result"]["addressComponent"]["province"].ToString();
            string city = jo["result"]["addressComponent"]["city"].ToString();
            return province + city;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = "";
            using (OpenFileDialog lvse = new OpenFileDialog()) {
                lvse.Title = "选择图片";
                lvse.InitialDirectory = "";
                lvse.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
                lvse.FilterIndex = 1;
                if (lvse.ShowDialog() == DialogResult.OK)
                {
                    filePath = lvse.FileName;
                    pictureBox1.Load(filePath);
                }
                else return;
            }
            //string filePath = "D:\\VSworkspace\\ClassifyImage/test2.jpg";

            try
            {
                String gps = findGPS(filePath);
                string[] gpsInfo = gps.Split('|');
                textBox1.Text = GetCityInfo(gpsInfo);
                logShow.AppendText("识别正确,图片拍摄地点：" + textBox1.Text + "\r\n");
            }
            catch (IndexOutOfRangeException ex)
            {
                textBox1.Text = "图片暂无位置信息";
                logShow.AppendText("图片无地理位置\r\n");
            }
            catch (NullReferenceException ex) {
                textBox1.Text = "图片暂无位置信息";
                logShow.AppendText("图片无地理位置\r\n");
            }
            catch (Exception ex)
            {
                textBox1.Text = "图片暂无位置信息";
                logShow.AppendText(ex.Message + "\r\n");

            }
           

        }

        /// <summary>
        /// 获取图片中的GPS坐标点
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <returns>返回坐标【纬度+经度】用"+"分割 取数组中第0和1个位置的值</returns>
        public String findGPS(String filePath)
        {
            string[] ret = new string[3];
            String s_GPS坐标 = "";
            //载入图片   
            Image objImage = Image.FromFile(filePath);
            //取得所有的属性(以PropertyId做排序)   
            var propertyItems = objImage.PropertyItems.OrderBy(x => x.Id);
            //暂定纬度N(北纬)   
            char chrGPSLatitudeRef = 'N';
            //暂定经度为E(东经)   
            char chrGPSLongitudeRef = 'E';
            foreach (PropertyItem objItem in propertyItems)
            {
                //只取Id范围为0x0000到0x001e
                if (objItem.Id >= 0x0000 && objItem.Id <= 0x001e)
                {
                    objItem.Id = 0x0002;
                    switch (objItem.Id)
                    {
                        //case 0x0000:
                        //    var query = from tmpb in objItem.Value select tmpb.ToString();
                        //    string sreVersion = string.Join(".", query.ToArray());
                        //    break;
                        //case 0x0001:
                        //    chrGPSLatitudeRef = BitConverter.ToChar(objItem.Value, 0);
                        //    break;
                        case 0x0002:
                            if (objItem.Value.Length == 24)
                            {
                                //degrees(将byte[0]~byte[3]转成uint, 除以byte[4]~byte[7]转成的uint)   
                                double d = BitConverter.ToUInt32(objItem.Value, 0) * 1.0d / BitConverter.ToUInt32(objItem.Value, 4);
                                //minutes(将byte[8]~byte[11]转成uint, 除以byte[12]~byte[15]转成的uint)   
                                double m = BitConverter.ToUInt32(objItem.Value, 8) * 1.0d / BitConverter.ToUInt32(objItem.Value, 12);
                                //seconds(将byte[16]~byte[19]转成uint, 除以byte[20]~byte[23]转成的uint)   
                                double s = BitConverter.ToUInt32(objItem.Value, 16) * 1.0d / BitConverter.ToUInt32(objItem.Value, 20);
                                //计算经纬度数值, 如果是南纬, 要乘上(-1)   
                                double dblGPSLatitude = (((s / 60 + m) / 60) + d) * (chrGPSLatitudeRef.Equals('N') ? 1 : -1);
                                string strLatitude = string.Format("{0:#} deg {1:#}' {2:#.00}\" {3}", d
                                                                    , m, s, chrGPSLatitudeRef);
                                //纬度+经度
                                s_GPS坐标 += dblGPSLatitude + "|";
                            }
                            break;
                        //case 0x0003:
                        //    //透过BitConverter, 将Value转成Char('E' / 'W')   
                        //    //此值在后续的Longitude计算上会用到   
                        //    chrGPSLongitudeRef = BitConverter.ToChar(objItem.Value, 0);
                        //    break;
                        //case 0x0004:
                        //    if (objItem.Value.Length == 24)
                        //    {
                        //        //degrees(将byte[0]~byte[3]转成uint, 除以byte[4]~byte[7]转成的uint)   
                        //        double d = BitConverter.ToUInt32(objItem.Value, 0) * 1.0d / BitConverter.ToUInt32(objItem.Value, 4);
                        //        //minutes(将byte[8]~byte[11]转成uint, 除以byte[12]~byte[15]转成的uint)   
                        //        double m = BitConverter.ToUInt32(objItem.Value, 8) * 1.0d / BitConverter.ToUInt32(objItem.Value, 12);
                        //        //seconds(将byte[16]~byte[19]转成uint, 除以byte[20]~byte[23]转成的uint)   
                        //        double s = BitConverter.ToUInt32(objItem.Value, 16) * 1.0d / BitConverter.ToUInt32(objItem.Value, 20);
                        //        //计算精度的数值, 如果是西经, 要乘上(-1)   
                        //        double dblGPSLongitude = (((s / 60 + m) / 60) + d) * (chrGPSLongitudeRef.Equals('E') ? 1 : -1);
                        //    }
                        //    break;
                        //case 0x0005:
                        //    string strAltitude = BitConverter.ToBoolean(objItem.Value, 0) ? "0" : "1";
                        //    break;
                        //case 0x0006:
                        //    if (objItem.Value.Length == 8)
                        //    {
                        //        //将byte[0]~byte[3]转成uint, 除以byte[4]~byte[7]转成的uint   
                        //        double dblAltitude = BitConverter.ToUInt32(objItem.Value, 0) * 1.0d / BitConverter.ToUInt32(objItem.Value, 4);
                        //    }
                        //    break;
                    }
                }
            }
            return s_GPS坐标;
        }
    }
}
