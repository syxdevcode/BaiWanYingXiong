using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiWanYingXiong
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        #region 定义程序变量

        // 用来表示是否截图完成
        private bool _catchFinished = false;

        // 用来表示截图开始
        private bool _catchStart = false;

        // 用来保存原始图像
        private Bitmap originBmp;

        // 用来保存截图的矩形
        private Rectangle _catchRectangle;

        #endregion

        //点击鼠标右键时，取消设置
        private void frmCutter_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        //点击鼠标左键时，开始画区域图
        private void frmCutter_MouseDown(object sender, MouseEventArgs e)
        {
            // 鼠标左键按下是开始画图，也就是截图
            if (e.Button == MouseButtons.Left)
            {
                // 如果捕捉没有开始
                if (!_catchStart && !_catchFinished)
                {
                    _catchStart = true;

                    // 保存此时鼠标按下坐标
                    Point newPoint = new Point(e.X, e.Y);

                    Tools.StartPoint = newPoint;
                }
            }
        }

        //鼠标移动时，根据移动的鼠标和点击时的第一个点，绘制矩形
        private void frmCutter_MouseMove(object sender, MouseEventArgs e)
        {
            #region 确保截图开始
            if (_catchStart && !_catchFinished)
            {
                // 新建一个图片对象，让它与屏幕图片相同
                originBmp = (Bitmap)Tools.ScreenShots.Clone();

                // 获取鼠标按下的坐标
                Point newPoint = new Point(Tools.StartPoint.X, Tools.StartPoint.Y);

                // 新建画板和画笔
                Graphics g = Graphics.FromImage(originBmp);
                Pen p = new Pen(Color.Red, 1);

                // 获取矩形的长宽
                int width = Math.Abs(e.X - Tools.StartPoint.X);
                int height = Math.Abs(e.Y - Tools.StartPoint.Y);
                if (e.X < Tools.StartPoint.X)
                {
                    newPoint.X = e.X;
                }
                if (e.Y < Tools.StartPoint.Y)
                {
                    newPoint.Y = e.Y;
                }

                _catchRectangle = new Rectangle(newPoint, new Size(width, height));

                Tools.CatchRectangle = new Rectangle(newPoint, new Size(width, height));
                Tools.CatchRectangleSize = new Size(width, height);


                // 将矩形画在画板上
                g.DrawRectangle(p, _catchRectangle);

                // 释放目前的画板
                g.Dispose();
                p.Dispose();
                // 从当前窗体创建新的画板
                Graphics g1 = this.CreateGraphics();

                // 将刚才所画的图片画到截图窗体上
                // 为什么不直接在当前窗体画图呢？
                // 如果自己解决将矩形画在窗体上，会造成图片抖动并且有无数个矩形
                // 这样实现也属于二次缓冲技术
                g1.DrawImage(originBmp, new Point(0, 0));
                g1.Dispose();
                // 释放拷贝图片，防止内存被大量消耗
                originBmp.Dispose();
            }
            #endregion
        }


        //鼠标点击后，弹起来时，完成矩形的绘制
        private void frmCutter_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // 如果截图已经开始，鼠标左键弹起设置截图完成
                if (_catchStart)
                {
                    Tools.EndPoint = new Point(e.X, e.Y);

                    _catchStart = false;
                    _catchFinished = true;
                }
            }
        }

        //双击，确定当前选择的设置
        private void frmCutter_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _catchFinished)
            {
                if (this.Tag != null)
                {
                    Form1 _frmMain = (Form1)this.Tag;
                    if (_frmMain != null)
                    {
                        //_frmMain.btnRead.Focus();
                        _frmMain.ReadImageResult();
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }
    }
}
