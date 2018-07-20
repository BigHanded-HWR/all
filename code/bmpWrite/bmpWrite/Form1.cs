using hwr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bmpWrite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Bitmap digitImage;//用来保存手写数字
        private Bitmap grayTmp;
        private Point startPoint;//用于绘制线段，作为线段的初始端点坐标
        private string t;
        private const int MnistImageSize = 28;//Mnist模型所需的输入图片大小
        private void button1_Click(object sender, EventArgs e)
        {
            //当点击清除时，重新绘制一个白色方框，同时清除label1显示的文本
            digitImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(digitImage);
            g.Clear(Color.Black);
            pictureBox1.Image = digitImage;
            //pictureBox2.Image = grayTmp;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            grayTmp = new Bitmap(28, 28);
            digitImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(digitImage);
            g.Clear(Color.Black);
            pictureBox1.Image = digitImage;
           
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //当鼠标左键被按下时，记录下需要绘制的线段的起始坐标
            startPoint = (e.Button == MouseButtons.Left) ? e.Location : startPoint;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //当鼠标在移动，且当前处于绘制状态时，根据鼠标的实时位置与记录的起始坐标绘制线段，同时更新需要绘制的线段的起始坐标
            if (e.Button == MouseButtons.Left)
            {
                Graphics g = Graphics.FromImage(digitImage);
                Pen myPen = new Pen(Color.White, 40);
                myPen.StartCap = LineCap.Round;
                myPen.EndCap = LineCap.Round;
                g.DrawLine(myPen, startPoint, e.Location);
                pictureBox1.Image = digitImage;
                g.Dispose();
                startPoint = e.Location;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Bitmap digitTmp = (Bitmap)digitImage.Clone();//复制digitImage
                grayTmp = new Bitmap(28, 28);                                            //调整图片大小为Mnist模型可接收的大小：28×28
                using (Graphics g = Graphics.FromImage(digitTmp))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(digitTmp, 0, 0, MnistImageSize, MnistImageSize);
                }
                //将图片转为灰阶图，并将图片的像素信息保存在list中
               // var image = new List<float>(MnistImageSize * MnistImageSize);
                for (var x = 0; x < MnistImageSize; x++)
                {
                    for (var y = 0; y < MnistImageSize; y++)
                    {
                        var color = digitTmp.GetPixel(y, x);
                        var a = (int)((color.R + color.G + color.B) / (3.0));
                        grayTmp.SetPixel(y, x, Color.FromArgb(a, a, a));
                    }
                }
                

            }

        }
        public Bitmap CaptureImage(Bitmap fromImage, int offsetX, int offsetY, int width, int height)
        {
           
            //创建新图位图
            Bitmap bitmap = new Bitmap(width, height);
            //创建作图区域
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            graphic.DrawImage(fromImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
            //从作图区生成新图
            Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
            //保存图片
            return(Bitmap) saveImage;
           // saveImage.Save(toImagePath, ImageFormat.Png);
            //释放资源   
           // saveImage.Dispose();
            //graphic.Dispose();
           // bitmap.Dispose();
        }

        private void Work_Click(object sender, EventArgs e)
        {
            Boolean flag = false,oflag=false;
            int m = 0;
            t = "";
            var bitList = new List<Bitmap>(MnistImageSize * MnistImageSize);

            for (var x = 0; x < MnistImageSize; x++)
            {
                for (var y = 0; y < MnistImageSize; y++)
                {
                    
                    if (grayTmp.GetPixel(x, y) .R== 255 )
                    {
                        flag = true;
                    }
                   // Console.WriteLine(flag);
                }
                if (flag == false&&oflag==true)
                {
                    Bitmap cut = new Bitmap(28, 28);
                    /*for (var i = m; i < x; i++)
                        for (var j = 0; j < MnistImageSize; j++)
                            cut.SetPixel(i,j, grayTmp.GetPixel(i+m,j));*/

                    //pictureBox2.Image = cut;
                    bitList.Add(CaptureImage(grayTmp, m, 0, x - m , MnistImageSize));
                    
                    //Console.WriteLine(x-m+1);
                    m = x;
                    oflag = false;
                    //pictureBox2.Image = bitList[0];
                }
                else if(flag == true && oflag == true)
                {

                    flag = false;
                }
                else if(flag == true && oflag ==false )
                {
                    oflag = true;
                }
            }
            if (bitList.Count() != 0)
            {
                
                for (var i = 0; i < bitList.Count(); i++)
                {
                   
                    Bitmap bmpTest = bitList[i];
                    var a = new InferImage();
                    List<float[]> probabities = a.RecImg(bmpTest);
                    t = t+(probabities[probabities.Count() - 1][0].ToString());
                    label1.Text = t;
                    Console.WriteLine(t);
               

                }
                try
                {
                    StringToMath stm = new StringToMath();
                    label1.Text = t + "=" + stm.STM(t);
                }
                catch (StringIsEmptyException)
                {
                    Console.WriteLine("String Is Empty ");
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Divide By Zero");
                }

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

    }

    public class StringToMath
    {
        public int STM(string t)
        {
//#pragma warning disable IDE0017 // Simplify object initialization
            if (t.Length > 0)
            {
               /* MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControl();
#pragma warning restore IDE0017 // Simplify object initialization
                sc.Language = "JavaScript";*/
                // var x = sc.Eval(t);
                var x = new System.Data.DataTable().Compute(t, "");
                var c = new System.Data.DataTable().Compute("5/0", "");
                //if (x == sc.Eval("5/0"))
                if (x == c)
                {
                    throw (new DivideByZeroException("Divide by zero found"));
                }
                else
                {
                    return (int)x;
                }
            }
            else
            {
                throw (new StringIsEmptyException("Empty input found"));
            }
        }
        /*public int ShowMath(string s)
        {
            if (s.Length > 0)
            {
                int num = 0;
                int oldnum = 0;
                int lastnum = 0;
                int eldnum = 0;
                char math = '+';
                char lmath = '+';
                int i = 0,l=s.Length;

                for (i = 0; i < s.Length; i++)
                {
                    if (s[i] >= 48 && s[i] <= 57)
                    {
                        num = num * 10 + (s[i]-48);
                       // Console.WriteLine(num);
                        if (i == s.Length - 1)
                        {
                            //Console.WriteLine("**********");
                            switch (math)
                            {
                                case '+': oldnum = oldnum + num; break;
                                case '-': oldnum = oldnum - num; break;
                                case '*': oldnum = oldnum * num; break;
                                case '/':try
                                        {
                                           oldnum = oldnum / num;
                                           break;
                                         }
                                         catch (DivideByZeroException)
                                         {
                                          throw (new DivideByZeroException("Divide by zero found"));    
                                         }

                            }

                        }
                    }
                    else
                    {
                        switch (math)
                        {
                            case '+': oldnum = oldnum + num; break;
                            case '-': oldnum = oldnum - num; break;
                            case '*': oldnum = oldnum * num; break;
                            case '/': oldnum = oldnum / num; break;
                        }
                        math = s[i];
                        num = 0;
                    }
                }
                return oldnum;

            }
            else
            {
                throw (new StringIsEmptyException("Empty input found"));
            }
        }*/
    }
    public class StringIsEmptyException : ApplicationException
    {
        public StringIsEmptyException(string message) : base(message)
        {
        }
    }




}
