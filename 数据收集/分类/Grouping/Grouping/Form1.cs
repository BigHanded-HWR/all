﻿using System;
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
        private int mode;
        private byte[] byData;
        private char[] charData = new char[1000];
        private void Form1_Load(object sender, EventArgs e)
        {
            mode = -1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //获取下一张图片,如果没有下一张则抛出Excepthion
        
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
                mode = -1;
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
                mode = 0;

            }catch(ArgumentOutOfRangeException ex)
            {
                DialogResult dr;
                dr = MessageBox.Show("该目录下没有符合要求的图片,点击重试重新选择", "没有找到图片", MessageBoxButtons.RetryCancel,
                         MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Retry)
                {
                    mode = -1;
                    selectPath();
                    goto flag;

                }
                else  if (dr == DialogResult.Cancel)
                {
                    //if(!isChoosing)
                    //Environment.Exit(0);
                    mode = -1;
                    goto flag1;
                }


            }
            flag1:;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (mode == 0)//标注模式
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
                        File.Move(imgFile[n], fp + (int.Parse(textBox1.Text)+n).ToString() + ".png");
                        string newTxtPath = fp + "\\label.txt";
                        Bitmap mp = ReadImageFile(imgFile[n + 1]);
                        StreamWriter sw = new StreamWriter(newTxtPath, true, Encoding.Default);
                        sw.Write(e.KeyChar.ToString());
                        sw.Write(",");
                        sw.Flush();
                        sw.Close();

                        pictureBox1.Image = mp;
                        n++;

                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        string fp = imgPath + "\\export\\";
                        string newTxtPath = fp + "\\label.txt";
                        StreamWriter sw = new StreamWriter(newTxtPath, true, Encoding.Default);
                        sw.Write(e.KeyChar.ToString());
                        sw.Flush();
                        sw.Close();

                        string a = "标注完成！总共标注了" + (n + 1).ToString() + "张图片";
                        MessageBox.Show(a, "完成", MessageBoxButtons.OK,
                              MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    }
                }

            }
            /*else if (mode == 1)//检查模式
            {
                int a = 2 * imgFile.Count();
                
                
                Console.WriteLine(a.ToString()," EE");
                try
                {
                   
                    if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 'x' ||
                    e.KeyChar == '*' || e.KeyChar == '+' || e.KeyChar == '-' || e.KeyChar == '/')
                    //是0到9的数字或者是运算符号
                    {
                        Console.WriteLine(e.KeyChar);
                       
                        label2.Text = charData[n].ToString();
                        if (charData[n] != e.KeyChar)//验证不符合
                        {
                            string i = "这张图是" + charData[n].ToString() + "吗？";
                            DialogResult dr;
                            dr = MessageBox.Show(i, "矛盾", MessageBoxButtons.YesNo,
                                  MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (dr == DialogResult.Yes)//是原来的数值，修改
                            {
                                charData[n] = e.KeyChar;
                            }else if(dr == DialogResult.No)//不是，重新输入
                            {

                            }
                        }
                        Console.WriteLine(charData[n]);
                        file.Close();
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(e.ToString());
                }
            
            }
            */
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            string newTxtPath = "F:\\mnist\\data\\image\\test\\label.txt";
            StreamWriter sw = new StreamWriter(newTxtPath, true, Encoding.Default);
            for (int i = 1; i <= 100; i++)
            {
                sw.Write("+,");
            }
            for (int i = 1; i <= 100; i++)
            {
                sw.Write("-,");
            }
            for (int i = 1; i <= 100; i++)
            {
                sw.Write("*,");
            }

            for (int i = 1; i <= 100; i++)
            {
                sw.Write("/,");
            }
            sw.Flush();
            sw.Close();
            /*
            //弹出一个窗口让用户选图片的路径
            imgPath = selectPath();
            label1.Text = imgPath;
            flag: imgFile = new List<string>();
            string[] strs;
            try
            {
                strs = Directory.GetFiles(imgPath);
            }
            catch (ArgumentException ex)
            {
                mode = -1;
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
            try
            {
                Bitmap mp = ReadImageFile(imgFile[0]);
                pictureBox1.Image = mp;
                try
                {
                    int a = 2 * imgFile.Count();
                    byData = new byte[a];
                    FileStream file = new FileStream(imgPath + "\\label.txt", FileMode.Open);
                    file.Seek(0, SeekOrigin.Begin);
                    file.Read(byData, 0, a);


                }
                mode = 1;

            }
            catch (ArgumentOutOfRangeException ex)
            {
                DialogResult dr;
                dr = MessageBox.Show("该目录下没有符合要求的图片,点击重试重新选择", "没有找到图片", MessageBoxButtons.RetryCancel,
                         MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Retry)
                {
                    mode = -1;
                    selectPath();
                    
                    goto flag;

                }
                else if (dr == DialogResult.Cancel)
                {
                    //if(!isChoosing)
                    //Environment.Exit(0);
                    goto flag1;
                }

     }*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            n = 0;
            List<string> outText = new List<string>();
            List<int> point = new List<int>();
            //List<int> bb = new List<int>() { 1,2,3,4,5 };

            //首先读取现有的文件
            string fp = "F:\\mnist\\data\\image\\test\\";
            //string fp = "C:\\Users\\29951\\Desktop\\mnist\\train-images\\1\\";
            //Directory.CreateDirectory(fp);
            //File.Move(imgFile[n], fp + (int.Parse(textBox1.Text) + n).ToString() + ".png");
            //清除文件缓存
            imgFile = new List<string>() { };
            string[] strs;
            strs = Directory.GetFiles(fp);
            //将图片保存到数组
            for(var i =0;i< strs.Length;i++)
            {
                
                FileInfo fi = new FileInfo(strs[i]);
                if (fi.Extension == ".png")
                {

                    imgFile.Add(i.ToString()+".png");
                    Console.WriteLine(strs[i]);
                    point.Add(i);
                    
                    //
                }
                /*
                if (fi.Extension == ".PNG")
                {
                    Console.WriteLine(fi.Name);
                    Directory.CreateDirectory(fp + "exports\\");
                    File.Copy(strs[i],fp+i.ToString()+".png", true);
                    imgFile.Add(i.ToString() + ".png");
                    point.Add(i);
                    //Console.WriteLine(i.ToString() + ".png");
                }
                */
            }
            //strs = null;
            //读取label
            //string s = File.ReadAllText(fp + "code_train_text.txt");
            //string s = File.ReadAllText("F:\\mnist\\data\\code_train_text.txt");
            string s = File.ReadAllText("F:\\mnist\\data\\image\\test\\label.txt");
            string[] ss=s.Split(new Char[] { ',' });
           
            s = null;
            foreach(string i in ss)
            {
                outText.Add(i);
            }
            ss = null;

            //随机
            point = GetRandom(point);
            n = 0;
            //n被随机到了point[n]的位置
            List<string> kk = new List<string>();
            //用kk保存变换后的文件列表
            foreach (string t in outText)//将文本数组乱序
            {
                kk.Add(outText[point[n]]);
                Console.Write(" " + outText[point[n]]);

                n++;
            }
            n = 0;
            Console.Write("\n");
            string op = "";
            foreach (string t in kk)//将文本数组输出成字符串
            {
                op = op.Insert(op.Length, ",");
                op = op.Insert(op.Length, t);
            }
            Directory.CreateDirectory(fp + "exports\\");
            File.AppendAllText(fp + "exports\\lab.txt", op);
            //处理文件
            kk = new List<string>();
            foreach (string t in imgFile)
            {
                kk.Add(imgFile[point[n]]);
                n++;
            }

            n = 0;
            foreach (string tt in imgFile)
            {
                Console.Write(kk[n].ToString()+" to "+ imgFile[n].ToString());
                //将源文件拷贝到新文件位置
                File.Copy(fp + kk[n], fp + "exports\\" + imgFile[n], true);
                //File.Move(imgFile[n], fp + aa[n]+ ".png");
                n++;
            }
            Console.Write("\n");
            n = 0;
            
            
        }

        private List<int> GetRandom(List<int> myList)
        {

            Random ran = new Random();
            List<int> newList = new List<int>();
            int index = 0;
            int temp = 0;
            for (int i = 0; i < myList.Count; i++)
            {

                index = ran.Next(0, myList.Count - 1);
                if (index != i)
                {
                    temp = myList[i];
                    myList[i] = myList[index];
                    myList[index] = temp;
                }
                Console.WriteLine(i.ToString());
            }

            return myList;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string a = "F:\\mnist\\data\\image\\1\\";

        }
    }

}
