using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Drawing.Printing;
using System.Diagnostics;

namespace HandWriting_output
{
    public partial class Form1 : Form
    {
        public Form1(string para = "", bool isFile = false)
        {
            InitializeComponent();
            if(para!="")
            {
                if(isFile)
                {
                    textBox1.Text = File.ReadAllText(para);
                }
                else
                {
                    textBox1.Text = para;
                }
            }
        }


        private Image pic;
        private string words = "";
        private List<Image> P= new List<Image>();
        private Image R;
        private string DB = Environment.CurrentDirectory + "\\database\\";

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Left = tabControl1.Left+ 7;
            pictureBox1.Top = tabControl1.TabPages[0].Top+tabControl1.Top + 7;
            
            pic = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            
            comboBox1.SelectedItem = comboBox1.Items[0];
            comboBox2.SelectedItem = comboBox2.Items[0];

            if(!Directory.Exists(Environment.CurrentDirectory+"\\database"))
            {
                MessageBox.Show("首次运行，程序将在字体库解压完后打开...\r\n可能需要20秒的时间\r\n按下确定后开始解压","提示");
                Directory.CreateDirectory(Environment.CurrentDirectory+"\\database");
                CompressHelper.DeCompressMulti(Environment.CurrentDirectory + "\\data.gz", Environment.CurrentDirectory+ "\\database\\");
            }
            button4_Click(null, null);
            if(tabControl1.TabPages.Count==0)
            {
                tabControl1.TabPages.Add("第1页");
            }
            
            text_width = pic_width - Convert.ToInt32(textBox18.Text) - Convert.ToInt32(textBox17.Text);
            text_height = pic_height - Convert.ToInt32(textBox15.Text) - Convert.ToInt32(textBox16.Text);

            textBox1_TextChanged(null, null);
        }



        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Enabled = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            panel3.Enabled = checkBox3.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox11.Enabled = radioButton3.Checked;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.zhihu.com/people/elpwc/activities");
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            panel6.Enabled = !checkBox8.Checked;
        }
        int zonghang = 0;
        int zongzhang = 0;
        Image B;
        private void button4_Click(object sender, EventArgs e)
        {
            
            if (backgroundWorker1.IsBusy)
            {
                MessageBox.Show("请等待转换完成","提醒");
            }
            else
            {
                tabControl1.TabPages.Clear();
                if (dividePages)
                {
                    for (int i = 1; i <= (int)Math.Ceiling(Regex.Split(textBox1.Text, "\r\n").Length / Math.Floor(text_height / (double)Convert.ToInt32(textBox8.Text))); i++)
                    {
                        tabControl1.TabPages.Add("第" + i.ToString() + "页");
                    }
                }
                else
                {
                    tabControl1.TabPages.Add("第1页");
                }
                label18.ForeColor = Color.Red;
                label18.Text = "正在转换..";

                backgroundWorker1.RunWorkerAsync();
            }
            
            
            
        }

        public int weight(int num, int weight, bool AllowNegative = false)
        {
            if (weight < 0)
            {
                return num;
            }
            else
            {
                Random a = new Random();
                int t = a.Next(0, weight);
                int w = a.Next(1, 2);
                if (w == 1)
                {
                    return num + t;
                }
                else
                {
                    if (num - t < 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return num - t;
                    }
                }
                /*
                int t = a.Next(1, 4);
                int w = a.Next(weight - 5, weight + 5);
                if (t == 1 || t == 3)
                {
                    return (int)(num * (1 - w / 100d));
                }
                else if (t == 2 || t == 4)
                {
                    return (int)(num * (1 + w / 100d));
                }
                */
            }
            return 0;
        }

        public int WordExist(string w)
        {
            //for (int i = 0; i < Convert.ToInt32(File.ReadAllText(DB + "_crt")); i++)
            //{
            //    if (getMid(File.ReadAllText(DB + i.ToString()), "<w>", "</w>") == w)
            //    {
            //        return i;
            //    }
            //}
            //return -1;
            int i = File.ReadAllText(DB + "_wlist").IndexOf(w);
            if (i != -1)
            {
                return i;
            }
            else
            {
                return -1;
            }
        }

        public string getMid(string all, string A, string B)
        {
            int IndexofA = all.IndexOf(A);
            int IndexofB = all.Substring(IndexofA + A.Length, all.Length - IndexofA - A.Length).LastIndexOf(B) + IndexofA + A.Length;
            return all.Substring(IndexofA + A.Length, IndexofB - IndexofA - A.Length);
        }

        public Image CombineBitmap(Image A, Image B,/* Color Transparent,*/ int x, int y)
        {
            Image res = (Image)B.Clone();
            using (Graphics g = Graphics.FromImage(res))
            {
                g.DrawImage(A, x, y);
            }
            return res;
        }

        public string[] dividestr(string text)
        {
            List<string> r = new List<string>();
            for (int i = 0; i < text.Length; i++)
            {
                r.Add(text.Substring(i, 1));
            }
            return r.ToArray();
        }

        public int getwidest(string t)
        {
            string[] s = Regex.Split(t, "\r\n");
            string r = "";
            foreach (string p in s)
            {
                if (p.Length >= r.Length)
                {
                    r = p;
                }
            }
            return r.Length;
        }

        public int getheight(string t)
        {
            string[] s = Regex.Split(t, "\r\n");
            return s.Length + 1;
        }

        /// <summary>
        /// 画一个点~
        /// </summary>
        /// <param name="g">要绘制的Graphics</param>
        /// <param name="c">颜色哦~</param>
        /// <param name="site">点的位置~</param>
        /// <param name="width">宽度..</param>
        /// <param name="type">类型：0 圆  1 方</param>
        public void DrawDot(Graphics g, Color c, Point site, int width, int type = 0, bool border = false)
        {
            if (type == 0)
            {
                Rectangle t = new Rectangle(site.X - width / 2, site.Y - width / 2, width, width);
                g.FillEllipse(new SolidBrush(c), t);
                if (border)
                {
                    g.DrawEllipse(new Pen(Color.Black, 1), t);
                }
            }
            else if (type == 1)
            {
                Rectangle t = new Rectangle(site.X - width / 2, site.Y - width / 2, width, width);
                g.FillRectangle(new SolidBrush(c), t);
                if (border)
                {
                    g.DrawRectangle(new Pen(Color.Black, 1), t);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            if (colorDialog1.Color != null)
            {
                button2.BackColor = colorDialog1.Color;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            if (colorDialog1.Color != null)
            {
                button5.BackColor = colorDialog1.Color;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            if (colorDialog1.Color != null)
            {
                button7.BackColor = colorDialog1.Color;
            }
        }

        int pic_width= 2480, pic_height= 3507;
        int text_width = 0, text_height = 0;
        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex==0)
            {

                textBox13.Text = pic_width.ToString();
                textBox14.Text = pic_height.ToString();
            }
            else
            {
                textBox13.Text = (pic_width/11.811).ToString();
                textBox14.Text = (pic_height / 11.811).ToString();
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            pic_width = 2480;
            pic_height = 3507;
            comboBox2_SelectedIndexChanged(null,null);
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            pic_width = 1535;
            pic_height = 2173;
            comboBox2_SelectedIndexChanged(null, null);
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
        }

        bool dividePages = true;
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            dividePages = false;
            panel5.Enabled = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            dividePages = true;
            panel5.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string TEXT = textBox1.Text;
            string res = "";
            string[] hang = Regex.Split(TEXT, "\r\n");
            text_width = pic_width - Convert.ToInt32(textBox18.Text) - Convert.ToInt32(textBox17.Text);
            text_height = pic_height - Convert.ToInt32(textBox15.Text) - Convert.ToInt32(textBox16.Text);
            int wn =text_width/ (10 + Convert.ToInt32(textBox5.Text));
            string T;
            foreach (string H in hang)
            {
                T = H;
                if(H.Length<=wn)
                {
                    res += H+"\r\n";
                }
                else
                {
                    do
                    {
                        res += T.Substring(0, wn) + "\r\n";
                        T = T.Substring(wn, T.Length - wn);
                    } while (T.Length > wn);
                    res += T+"\r\n";
                }
            }
            
            
            textBox1.Text = res;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex!=-1)
            {
                if (dividePages)
                {
                    pictureBox1.BackgroundImage = P[tabControl1.SelectedIndex];
                }
            }

            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string[] tpt = Regex.Split(textBox1.Text, "\r\n");
            if (dividePages)
            {

                label22.Text = tpt.Length.ToString() + "行，" + textBox1.Text.Length.ToString() + "字，" + Math.Ceiling(tpt.Length / Math.Floor(text_height / (double)Convert.ToInt32(textBox8.Text))).ToString() + "页";
            }
            else
            {

                label22.Text = tpt.Length.ToString() + "行，" + textBox1.Text.Length.ToString() + "字，1页";
            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                if(dividePages)
                {
                    P[tabControl1.SelectedIndex].Save(saveFileDialog1.FileName);
                }
                else
                {
                    R.Save(saveFileDialog1.FileName);
                }
                
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            P.Clear();

            words = textBox1.Text;
            int crtx = 0, crty = 0;
            string[] s = Regex.Split(words, "\r\n");
            text_width = pic_width - Convert.ToInt32(textBox18.Text) - Convert.ToInt32(textBox17.Text);
            text_height = pic_height - Convert.ToInt32(textBox15.Text) - Convert.ToInt32(textBox16.Text);
            R = new Bitmap(text_width, text_height);

            if (dividePages)
            {
                B = new Bitmap(pic_width, pic_height);
            }
            else
            {
                B = new Bitmap(getwidest(words) * (10 + Convert.ToInt32(textBox5.Text)), getheight(words) * Convert.ToInt32(textBox8.Text));
            }

            if (!checkBox8.Checked)
            {
                using (Graphics g = Graphics.FromImage(B))
                {
                    g.FillRectangle(new SolidBrush(button7.BackColor), 0, 0, B.Width, B.Height);
                }
            }

            if (dividePages)
            {
                
                zonghang = (int)Math.Floor(text_height / (double)Convert.ToInt32(textBox8.Text));
                zongzhang = (int)Math.Ceiling(s.Length / (double)zonghang);
                for (int jj = 0; jj < zongzhang; jj++)
                {
                    B = new Bitmap(pic_width, pic_height);
                    if (!checkBox8.Checked)
                    {
                        using (Graphics g = Graphics.FromImage(B))
                        {
                            g.FillRectangle(new SolidBrush(button7.BackColor), 0, 0, B.Width, B.Height);
                        }
                    }
                    R = new Bitmap(text_width, text_height);
                    int aton = 0;
                    string t;
                    if (jj == zongzhang - 1)
                    {
                        aton = s.Length % zonghang;
                    }
                    else
                    {
                        aton = zonghang;
                    }
                    crty = 0;
                    for (int kk = 0; kk < aton; kk++)
                    {
                        t = s[jj * zonghang + kk];
                        foreach (string n in dividestr(t))
                        {
                            int ii = WordExist(n);
                            if (ii != -1)
                            {
                                #region s

                                string[] T = Regex.Split(getMid(File.ReadAllText(DB + ii.ToString()), "<d>", "<s>"), "<s>");
                                List<string[]> m = new List<string[]>();
                                foreach (string h in T)
                                {
                                    if (h != "")
                                    {
                                        m.Add(Regex.Split(h, "&"));
                                    }
                                }

                                for (int i = 0; i < m[0].Length; i++)
                                {
                                    int[] P = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                                    int tt = new Random().Next(0, m.Count);
                                    try
                                    {
                                        for (int j = 0; j < m.Count; j++)
                                        {
                                            string[] dott = m[j][i].Split('|');
                                            for (int k = 0; k < 14; k++)
                                            {
                                                if (j == tt)
                                                {
                                                    P[k] += weight(Convert.ToInt32(dott[k]), 5);
                                                }
                                                else
                                                {
                                                    P[k] += Convert.ToInt32(dott[k]);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    List<Point> pp = new List<Point>();
                                    for (int j = 0; j < 13; j += 2)
                                    {
                                        pp.Add(new Point(weight(P[j] / m.Count, Convert.ToInt32(textBox10.Text)) + crtx, weight(P[j + 1] / m.Count, Convert.ToInt32(textBox10.Text)) + crty));
                                    }

                                    using (Graphics g = Graphics.FromImage(R))
                                    {
                                        g.SmoothingMode = SmoothingMode.AntiAlias;
                                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                        g.CompositingQuality = CompositingQuality.HighQuality;
                                        if (ImageType==0)
                                        {
                                            g.DrawCurve(new Pen(button2.BackColor, weight(Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox6.Text))), pp.ToArray());
                                        }
                                        else if (ImageType == 1)
                                        {
                                            g.DrawLines(new Pen(button2.BackColor, weight(Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox6.Text))), pp.ToArray());
                                        }
                                        else if (ImageType == 2)
                                        {
                                            foreach (Point p in pp)
                                            {
                                                DrawDot(g, button2.BackColor, p, weight(Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox6.Text)));
                                            }
                                        }
                                    }
                                }

                                #endregion s
                            }
                            crtx += weight(Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox7.Text));
                            
                        }
                        pictureBox1.BackgroundImage =  R;

                        crty += weight(Convert.ToInt32(textBox8.Text), Convert.ToInt32(textBox9.Text));
                        crtx = 0;
                    }
                    if (checkBox2.Checked)
                    {
                        using (Graphics g = Graphics.FromImage(R))
                        {
                            if (radioButton1.Checked)
                            {
                                for (int h = 0; h <zonghang; h++)
                                {
                                    g.DrawLine(new Pen(new SolidBrush(button5.BackColor), Convert.ToInt32(textBox2.Text)), 0,h*Convert.ToInt32(textBox8.Text) + Convert.ToInt32(textBox3.Text), R.Width, h * Convert.ToInt32(textBox8.Text)+  Convert.ToInt32(textBox3.Text));
                                }
                                
                            }
                            else if (radioButton2.Checked)
                            {
                                for (int h = 0; h <aton; h++)
                                {
                                    g.DrawLine(new Pen(new SolidBrush(button5.BackColor), Convert.ToInt32(textBox2.Text)), 0, h * Convert.ToInt32(textBox8.Text) + Convert.ToInt32(textBox3.Text), R.Width, h * Convert.ToInt32(textBox8.Text) + Convert.ToInt32(textBox3.Text));
                                }

                            }
                            else if (radioButton3.Checked)
                            {
                                for (int h = 0; h <Convert.ToInt32(textBox11.Text); h++)
                                {
                                    g.DrawLine(new Pen(new SolidBrush(button5.BackColor), Convert.ToInt32(textBox2.Text)), 0, h * Convert.ToInt32(textBox8.Text) + Convert.ToInt32(textBox3.Text), R.Width, h * Convert.ToInt32(textBox8.Text) + Convert.ToInt32(textBox3.Text));
                                }

                            }
                        }

                    }

                    B = CombineBitmap(R, B, Convert.ToInt32(textBox18.Text), Convert.ToInt32(textBox15.Text));
                    pictureBox1.BackgroundImage = B;

                    P.Add(B);


                }
                pictureBox1.BackgroundImage = P[0];

            }
            else
            {
                B = new Bitmap(getwidest(words) * (10 + Convert.ToInt32(textBox5.Text)), getheight(words) * Convert.ToInt32(textBox8.Text));
                if (!checkBox8.Checked)
                {
                    using (Graphics g = Graphics.FromImage(B))
                    {
                        g.FillRectangle(new SolidBrush(button7.BackColor), 0, 0, B.Width, B.Height);
                    }
                }
                R = new Bitmap(getwidest(words) * (10 + Convert.ToInt32(textBox5.Text)), getheight(words) * Convert.ToInt32(textBox8.Text));

                foreach (string t in s)
                {
                    foreach (string n in dividestr(t))
                    {
                        int ii = WordExist(n);
                        if (ii != -1)
                        {
                            #region s

                            string[] T = Regex.Split(getMid(File.ReadAllText(DB + ii.ToString()), "<d>", "<s>"), "<s>");
                            List<string[]> m = new List<string[]>();
                            foreach (string h in T)
                            {
                                if (h != "")
                                {
                                    m.Add(Regex.Split(h, "&"));
                                }
                            }

                            for (int i = 0; i < m[0].Length; i++)
                            {
                                int[] P = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                                int tt = new Random().Next(0, m.Count);
                                try
                                {
                                    for (int j = 0; j < m.Count; j++)
                                    {
                                        string[] dott = m[j][i].Split('|');
                                        for (int k = 0; k < 14; k++)
                                        {
                                            if (j == tt)
                                            {
                                                P[k] += weight(Convert.ToInt32(dott[k]), 5);
                                            }
                                            else
                                            {
                                                P[k] += Convert.ToInt32(dott[k]);
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                List<Point> pp = new List<Point>();
                                for (int j = 0; j < 13; j += 2)
                                {
                                    pp.Add(new Point(weight(P[j] / m.Count, Convert.ToInt32(textBox10.Text)) + crtx, weight(P[j + 1] / m.Count, Convert.ToInt32(textBox10.Text)) + crty));
                                }

                                using (Graphics g = Graphics.FromImage(R))
                                {
                                    g.SmoothingMode = SmoothingMode.AntiAlias;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    if (ImageType == 0)
                                    {
                                        g.DrawCurve(new Pen(button2.BackColor, weight(Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox6.Text))), pp.ToArray());
                                    }
                                    else if (ImageType == 1)
                                    {
                                        g.DrawLines(new Pen(button2.BackColor, weight(Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox6.Text))), pp.ToArray());
                                    }
                                    else if (ImageType == 2)
                                    {
                                        foreach (Point p in pp)
                                        {
                                            DrawDot(g, button2.BackColor, p, weight(Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox6.Text)));
                                        }
                                    }
                                }
                            }

                            #endregion s
                        }
                        crtx += weight(Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox7.Text));
                        
                    }
                    pictureBox1.BackgroundImage = B;
                    crty += weight(Convert.ToInt32(textBox8.Text), Convert.ToInt32(textBox9.Text));
                    crtx = 0;
                }
                B = CombineBitmap(R, B, Convert.ToInt32(textBox18.Text), Convert.ToInt32(textBox15.Text));
                pictureBox1.BackgroundImage = B;
            }


        }
        int ImageType=0;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImageType = comboBox1.SelectedIndex;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label18.ForeColor = Color.Black;
            label18.Text = "转换完成";
        }

        private void 关闭程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void panel8_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(panel8);
        }

        private void panel8_MouseMove(object sender, MouseEventArgs e)
        {
            toolTip1.Show("敬请期待啦..", panel8, e.X, e.Y);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            pictureBox1.BackgroundImage = null;
            tabControl1.TabPages.Clear();
            
        }

        private void 初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button10_Click(null,null);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Form2(pictureBox1.BackgroundImage).Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //打印预览
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            PrintDocument pd = new PrintDocument();
            //设置边距
            Margins margin = new Margins(20, 20, 20, 20);
            pd.DefaultPageSettings.Margins = margin;
            ////纸张设置默认
            PaperSize pageSize = new PaperSize("First custom size", pictureBox1.BackgroundImage.Width,pictureBox1.BackgroundImage.Height);
            pd.DefaultPageSettings.PaperSize = pageSize;
            //打印事件设置
            pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
            ppd.Document = pd;
            ppd.ShowDialog();
            try
            {
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pd.PrintController.OnEndPrint(pd, new PrintEventArgs());
            }
        }
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            //读取图片模板
            Image temp = pictureBox1.BackgroundImage;
            
            int x = e.MarginBounds.X;
            int y = e.MarginBounds.Y;
            int width = temp.Width;
            int height = temp.Height;
            Rectangle destRect = new Rectangle(x, y, width, height);
            e.Graphics.DrawImage(temp, destRect, 0, 0, temp.Width, temp.Height, System.Drawing.GraphicsUnit.Pixel);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form3().Show();
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("help.rtf");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                pic_width = Convert.ToInt32(textBox13.Text);
                pic_height = Convert.ToInt32(textBox14.Text);
            }
            else
            {
                pic_width = (int)(Convert.ToDouble(textBox13.Text)*11.811);
                pic_height = (int)(Convert.ToDouble(textBox14.Text)*11.811);
            }
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("单击可看大图",pictureBox1,2500);
        }

        private void panel8_MouseHover(object sender, EventArgs e)
        {
            //toolTip1.Show("敬请期待啦..", panel8, 5000);
        }

        private void panel8_MouseMove_1(object sender, MouseEventArgs e)
        {
            toolTip1.Show("敬请期待啦..", panel8,e.X,e.Y);
        }

        private void panel8_MouseLeave_1(object sender, EventArgs e)
        {
            toolTip1.Hide(panel8);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                if (dividePages)
                {
                    int i=0;
                    foreach (Image T in P)
                    {
                        i++;
                        T.Save(folderBrowserDialog1.SelectedPath + "\\handwriting_output_"+i.ToString()+".png");
                    }
                }
                else
                {
                    R.Save(folderBrowserDialog1.SelectedPath+"\\handwriting_output_1.png");
                }

            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
        }
    }

    static class CompressHelper
    {
        /// <summary>
        /// 单文件压缩（生成的压缩包和第三方的解压软件兼容）
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns></returns>
        public static string CompressSingle(string sourceFilePath)
        {
            string zipFileName = sourceFilePath + ".gz";
            using (FileStream sourceFileStream = new FileInfo(sourceFilePath).OpenRead())
            {
                using (FileStream zipFileStream = File.Create(zipFileName))
                {
                    using (GZipStream zipStream = new GZipStream(zipFileStream, CompressionMode.Compress))
                    {
                        sourceFileStream.CopyTo(zipStream);
                    }
                }
            }
            return zipFileName;
        }
        /// <summary>
        /// 自定义多文件压缩（生成的压缩包和第三方的压缩文件解压不兼容）
        /// </summary>
        /// <param name="sourceFileList">文件列表</param>
        /// <param name="saveFullPath">压缩包全路径</param>
        public static void CompressMulti(string[] sourceFileList, string saveFullPath)
        {
            MemoryStream ms = new MemoryStream();
            foreach (string filePath in sourceFileList)
            {
                Console.WriteLine(filePath);
                if (File.Exists(filePath))
                {
                    string fileName = Path.GetFileName(filePath);
                    byte[] fileNameBytes = System.Text.Encoding.UTF8.GetBytes(fileName);
                    byte[] sizeBytes = BitConverter.GetBytes(fileNameBytes.Length);
                    ms.Write(sizeBytes, 0, sizeBytes.Length);
                    ms.Write(fileNameBytes, 0, fileNameBytes.Length);
                    byte[] fileContentBytes = System.IO.File.ReadAllBytes(filePath);
                    ms.Write(BitConverter.GetBytes(fileContentBytes.Length), 0, 4);
                    ms.Write(fileContentBytes, 0, fileContentBytes.Length);
                }
            }
            ms.Flush();
            ms.Position = 0;
            using (FileStream zipFileStream = File.Create(saveFullPath))
            {
                using (GZipStream zipStream = new GZipStream(zipFileStream, CompressionMode.Compress))
                {
                    ms.Position = 0;
                    ms.CopyTo(zipStream);
                }
            }
            ms.Close();
        }

        /// <summary>
        /// 多文件压缩解压
        /// </summary>
        /// <param name="zipPath">压缩文件路径</param>
        /// <param name="targetPath">解压目录</param>
        public static void DeCompressMulti(string zipPath, string targetPath)
        {
            byte[] fileSize = new byte[4];
            if (File.Exists(zipPath))
            {
                using (FileStream fStream = File.Open(zipPath, FileMode.Open))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (GZipStream zipStream = new GZipStream(fStream, CompressionMode.Decompress))
                        {
                            zipStream.CopyTo(ms);

                        }
                        ms.Position = 0;
                        while (ms.Position != ms.Length)
                        {
                            ms.Read(fileSize, 0, fileSize.Length);
                            int fileNameLength = BitConverter.ToInt32(fileSize, 0);
                            byte[] fileNameBytes = new byte[fileNameLength];
                            ms.Read(fileNameBytes, 0, fileNameBytes.Length);
                            string fileName = System.Text.Encoding.UTF8.GetString(fileNameBytes);
                            string fileFulleName = targetPath + fileName;
                            ms.Read(fileSize, 0, 4);
                            int fileContentLength = BitConverter.ToInt32(fileSize, 0);
                            byte[] fileContentBytes = new byte[fileContentLength];
                            ms.Read(fileContentBytes, 0, fileContentBytes.Length);
                            using (FileStream childFileStream = File.Create(fileFulleName))
                            {
                                childFileStream.Write(fileContentBytes, 0, fileContentBytes.Length);
                            }
                        }
                    }
                }
            }
        }

    }
}
