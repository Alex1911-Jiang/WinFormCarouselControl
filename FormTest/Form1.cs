using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FormTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Dictionary<Image, Action> imageAndActions = new Dictionary<Image, Action>();
            imageAndActions.Add(Resources.图片1, () => MessageBox.Show("hit", "你点击了第1张图片"));
            imageAndActions.Add(Resources.图片2, () => Process.Start("https://github.com/Alex1911-Jiang/WinFormCarouselControl"));
            imageAndActions.Add(Resources.图片3, () => Process.Start("https://www.baidu.com/"));
            imageAndActions.Add(Resources.图片4, () => Close());
            carouselPanel1.ImageAndActions = imageAndActions;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (MessageBox.Show("你确定要关闭这个窗口吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
                e.Cancel = true;
            base.OnFormClosing(e);
        }
    }
}
