using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "D:\\VSworkspace\\ClassifyImage/test.jpg";
            Bitmap image = new Bitmap(filePath);
            foreach (PropertyItem pi in image.PropertyItems)
            {
                switch (pi.Type)
                {
                    case 2:
                        Console.WriteLine(pi.Id + ":" + Encoding.Default.GetString(pi.Value)); ;
                        break;
                    case 3:
                        Console.WriteLine(pi.Id + ":" + BitConverter.ToUInt16(pi.Value, 0));
                        break;
                    case 4:
                        Console.WriteLine(pi.Id + ":" + BitConverter.ToUInt32(pi.Value, 0));
                        break;
                }

            }
            
        }
    }
}
