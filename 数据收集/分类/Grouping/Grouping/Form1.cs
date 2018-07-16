using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grouping
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<string> imgFile;
        private int n;
        private string imgPath;
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //获取下一张图片,如果没有下一张则抛出Excepthion
        private static String getNextPic()
        {
            String filePath = "";
            //imgFile = Directory.GetFiles(@"E:\image");
            return filePath;
        }
        public static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            Image result = Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }

        private static string selectPath()
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            return path.SelectedPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //弹出一个窗口让用户选图片的路径
            imgPath = selectPath();
            label1.Text = imgPath;
            flag: imgFile = new List<string>();
            string[] strs;
            try
            {
                strs = Directory.GetFiles(imgPath);
            }
            catch(ArgumentException ex)
            {
                goto flag1;
            }
            

            foreach (string file in strs)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Extension == ".png")
                {
                    imgFile.Add(file);
                    Console.WriteLine(file);
                }
            }
            try { 
                Bitmap mp = ReadImageFile(imgFile[0]);
                pictureBox1.Image = mp;

            }catch(ArgumentOutOfRangeException ex)
            {
                DialogResult dr;
                dr = MessageBox.Show("该目录下没有符合要求的图片,点击重试重新选择", "没有找到图片", MessageBoxButtons.RetryCancel,
                         MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Retry)
                {
                    selectPath();
                    goto flag;

                }
                else  if (dr == DialogResult.Cancel)
                {
                    //if(!isChoosing)
                    //Environment.Exit(0);
                    goto flag1;
                }


            }
            flag1:;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 'x' ||
                e.KeyChar == '*' || e.KeyChar == '+' || e.KeyChar == '-' || e.KeyChar == '/')
            //是0到9的数字或者是运算符号
            {
                Console.WriteLine(e.KeyChar);

                try
                {
                    string fp = imgPath + "\\export\\";
                    Directory.CreateDirectory(fp);
                    File.Move(imgFile[n], fp+e.KeyChar+".png");
                    Bitmap mp = ReadImageFile(imgFile[n + 1]);
                    pictureBox1.Image = mp;
                    n++;

                }
                catch (ArgumentOutOfRangeException ex)
                {
                    string fp = imgPath + "\\export\\";
                    Directory.CreateDirectory(fp);
                    File.Move(imgFile[n], fp + e.KeyChar + ".png");
                }
            }


        }
    }
}
