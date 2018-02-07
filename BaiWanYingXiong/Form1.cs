using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolGood.Words;

namespace BaiWanYingXiong
{
    public partial class Form1 : Form
    {
        public ChromiumWebBrowser chromeBrowser;

        public Form1()
        {
            InitializeComponent();

            InitializeChromium();
        }

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            settings.BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe";
            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("http://www.baidu.com");
            // Add it to the form and fill it to the form window.
            //this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            this.splitContainer1.Panel2.Controls.Add(chromeBrowser);
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.IsSplitterFixed = true;// 不移动SplitterFixed
            chromeBrowser.FrameLoadEnd += browser_FrameLoadEnd;

        }

        async void browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var result = await chromeBrowser.GetSourceAsync();

            if (!string.IsNullOrEmpty(label3.Text))
            {
                label6.Text = "次数:" + SearchStr(result, label3.Text).ToString();
            }
            if (!string.IsNullOrEmpty(label4.Text))
            {
                label7.Text = "次数:" + SearchStr(result, label4.Text).ToString();
            }
            if (!string.IsNullOrEmpty(label5.Text))
            {
                label8.Text = "次数:" + SearchStr(result, label5.Text).ToString();
            }
        }

        #region 
        /// <summary>
        /// 查找字符串数量（正则表达式）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public int CharNum(string str, string search)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(str) || !string.IsNullOrEmpty(search))
            {
                string[] resultString = Regex.Split(str, search, RegexOptions.IgnoreCase);

                count = resultString.Length;
            }
            return count;
        }
        #endregion

        /// <summary>
        /// 查找字符串数量
        /// </summary>
        /// <param name="str"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public int SearchStr(string str, string search)
        {
            StringSearch iwords = new StringSearch();
            iwords.SetKeywords(new string[1] { search });
            var all = iwords.FindAll(str);
            return all.Count;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Form2 form2 = new Form2();
            //form2.StartPosition = FormStartPosition.CenterScreen;
            //form2.Show();

            // 新建一个和屏幕大小相同的图片
            Bitmap catchBmp = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);

            // 创建一个画板，让我们可以在画板上画图
            // 这个画板也就是和屏幕大小一样大的图片
            // 我们可以通过Graphics这个类在这个空白图片上画图
            Graphics g = Graphics.FromImage(catchBmp);

            // 把屏幕图片拷贝到我们创建的空白图片 catchBmp中
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));

            // 创建截图窗体
            Form2 _frmCutter = new Form2();
            _frmCutter.Tag = this;

            // 指示窗体的背景图片为屏幕图片
            _frmCutter.BackgroundImage = catchBmp;
            Tools.ScreenShots = catchBmp;

            _frmCutter.Width = Screen.AllScreens[0].Bounds.Width;
            _frmCutter.Height = Screen.AllScreens[0].Bounds.Height;
            DialogResult dr = _frmCutter.ShowDialog();

            //设置识别图片按钮可用
            button2.Enabled = true;
            button3.Enabled = true;
        }

        internal void ReadImageResult()
        {
            //截取设置的区域屏幕图片
            Bitmap _screenShots = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            // 创建一个画板，让我们可以在画板上画图
            // 这个画板也就是和屏幕大小一样大的图片
            // 我们可以通过Graphics这个类在这个空白图片上画图
            Graphics g_screenShots = Graphics.FromImage(_screenShots);
            // 把屏幕图片拷贝到我们创建的空白图片 CatchBmp中
            g_screenShots.CopyFromScreen(new Point(1, 1), new Point(0, 0), new Size(Screen.AllScreens[0].Bounds.Width,
         Screen.AllScreens[0].Bounds.Height));

            //剪切的图片
            var _catchBmp = new Bitmap(Tools.CatchRectangleSize.Width, Tools.CatchRectangleSize.Height);
            Graphics g = Graphics.FromImage(_catchBmp);
            g.DrawImage(_screenShots, new Rectangle(1, 1, Tools.CatchRectangleSize.Width, Tools.CatchRectangleSize.Height),
           Tools.CatchRectangle, GraphicsUnit.Pixel);
            g.Dispose();
            g_screenShots.Dispose();

            pictureBox1.Width = Tools.CatchRectangleSize.Width;

            pictureBox1.Height = Tools.CatchRectangleSize.Height;

            Image displayImage = (Image)_catchBmp;

            //显示图像
            this.pictureBox1.BackgroundImage = displayImage;

            var result = BaiDuOCR.GeneralBasicDemo(_catchBmp);
            BindData(result);
            //BaiDuOCR.AccurateBasicDemo(_catchBmp);
        }

        private Bitmap _catchBmp_btn2;

        private void button2_Click(object sender, EventArgs e)
        {
            //截取设置的区域屏幕图片
            Bitmap _screenShots = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            // 创建一个画板，让我们可以在画板上画图
            // 这个画板也就是和屏幕大小一样大的图片
            // 我们可以通过Graphics这个类在这个空白图片上画图
            Graphics g_screenShots = Graphics.FromImage(_screenShots);
            // 把屏幕图片拷贝到我们创建的空白图片 CatchBmp中
            g_screenShots.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.AllScreens[0].Bounds.Width,
         Screen.AllScreens[0].Bounds.Height));

            //剪切的图片
            _catchBmp_btn2 = new Bitmap(Tools.CatchRectangleSize.Width, Tools.CatchRectangleSize.Height);
            Graphics g = Graphics.FromImage(_catchBmp_btn2);
            g.DrawImage(_screenShots, new Rectangle(0, 0, Tools.CatchRectangleSize.Width, Tools.CatchRectangleSize.Height),
           Tools.CatchRectangle, GraphicsUnit.Pixel);
            g.Dispose();
            g_screenShots.Dispose();

            pictureBox1.Width = Tools.CatchRectangleSize.Width;

            pictureBox1.Height = Tools.CatchRectangleSize.Height;

            //显示图像
            this.pictureBox1.BackgroundImage = (Image)_catchBmp_btn2;
            var result = BaiDuOCR.GeneralBasicDemo(_catchBmp_btn2);
            BindData(result);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Enabled == true)
            {
                MemoryStream ms = new MemoryStream();
                _catchBmp_btn2.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);

                var result = YouDaoOCR.TranslateTextPlain(strbaser64);
            }
        }

        private void BindData(JObject obj)
        {
            var words = obj?.SelectToken("words_result").ToString();
            dynamic json = JToken.Parse(words) as dynamic;

            string text = string.Empty;

            int imageRowCount = json.Count;

            if (imageRowCount == 0) return;

            if (imageRowCount >= 5)
                text = json[0].words + json[1].words;

            label1.Text = json.Count >= 1 ? json[0].words : "";

            label2.Text = json.Count >= 2 ? json[1].words : "";

            label3.Text = json.Count >= 3 ? json[2].words : "";

            label4.Text = json.Count >= 4 ? json[3].words : "";

            label5.Text = json.Count >= 5 ? json[4].words : "";

            chromeBrowser.Load(string.Format("http://www.baidu.com/s?wd={0}", text));
        }

    }
}
