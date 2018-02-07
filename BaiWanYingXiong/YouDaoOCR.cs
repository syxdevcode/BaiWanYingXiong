using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BaiWanYingXiong
{
    /// <summary>
    /// 有道OCR 图片识别文字接口帮助类
    /// </summary>
    public class YouDaoOCR
    {
        public static string TranslateTextPlain(string img)
        {

            String url = "http://openapi.youdao.com/ocrapi";
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //string img = ImgToBase64String("d:/1.png");

            string appKey = "1f73fc4e4620bad8";
            string detectType = "10011";
            string langType = "zh-en";
            String imageType = "1";
            string salt = DateTime.Now.Millisecond.ToString();
            string appSecret = "FfnZzIBBFZZ2siKY73DFK50xaDSv1NcY";
            MD5 md5 = new MD5CryptoServiceProvider();
            string md5Str = appKey + img + salt + appSecret;
            byte[] output = md5.ComputeHash(Encoding.UTF8.GetBytes(md5Str));
            string sign = BitConverter.ToString(output).Replace("-", "");
            dic.Add("img", System.Web.HttpUtility.UrlEncode(img));
            dic.Add("appKey", appKey);
            dic.Add("langType", langType);
            dic.Add("detectType", detectType);
            dic.Add("imageType", imageType);
            dic.Add("salt", salt);
            dic.Add("sign", sign);
            string result = Post(url, dic);

            return result;
        }

        protected static string ImgToBase64String(string Imagefilename)
        {
            try
            {
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Imagefilename);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string Post(string url, IDictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            #region 添加Post 参数
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            Console.WriteLine(builder.ToString());
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
