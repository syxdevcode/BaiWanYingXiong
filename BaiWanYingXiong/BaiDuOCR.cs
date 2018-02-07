using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiWanYingXiong
{
    public class BaiDuOCR
    {
        // 设置APPID/AK/SK
        public static string APP_ID = "10796622";
        public static string API_KEY = "4e6u4E3ApI5NqEWyDCDZT0Gx";
        public static string SECRET_KEY = "C2qQyvdx2PhMb8kiD1rVPcADXhOsQ3Tq";

        public static Baidu.Aip.Ocr.Ocr client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);


        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }

        /// <summary>
        /// 通用文字识别
        /// </summary>
        /// <param name="bitmap"></param>
        public static JObject GeneralBasicDemo(Bitmap bitmap)
        {
            var image = Bitmap2Byte(bitmap);//File.ReadAllBytes(imgFile);

            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.GeneralBasic(Bitmap2Byte(bitmap));
            return result;
        }


        /// <summary>
        /// 高精度版本
        /// </summary>
        /// <param name="bitmap"></param>
        public static void AccurateBasicDemo(Bitmap bitmap)
        {
            var image = Bitmap2Byte(bitmap);// File.ReadAllBytes(imgFile);
            // 调用通用文字识别（高精度版），可能会抛出网络等异常，请使用try/catch捕获
            var result = client.AccurateBasic(image);
        }

        /// <summary>
        /// 含有生僻字
        /// </summary>
        /// <param name="bitmap"></param>
        public void GeneralEnhancedDemo(Bitmap bitmap)
        {
            var image = Bitmap2Byte(bitmap);// File.ReadAllBytes(imgFile);
            // 调用通用文字识别（含生僻字版）, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.GeneralEnhanced(image);

            // 如果有可选参数
            var options = new Dictionary<string, object>{
                {"language_type", "CHN_ENG"},
                {"detect_direction", "true"},
                {"detect_language", "true"},
                {"probability", "true"}
            };
            // 带参数调用通用文字识别（含生僻字版）, 图片参数为本地图片
            result = client.GeneralEnhanced(image, options);
        }
    }
}
