using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using hwr;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            /*
            Thread threadRec = new Thread(op);
            threadRec.SetApartmentState(ApartmentState.STA);
            //threadRec.IsBackground = false;
            threadRec.Start();
            */
            
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                //openFileDialog.InitialDirectory = Application.StartupPath;
                InitialDirectory = "C:\\Users\\29951\\Desktop\\mnist\\train-images\\1\\",
                //Console.WriteLine("1111");
                //openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files(*.*)|*>**";
                Filter = "png files (*.png)|*.png;|doc|*.doc|All files(*.*)|*>**;",
                FilterIndex = 1
            };

            Console.WriteLine("1111");
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                
                Bitmap bmpTest = new Bitmap(openFileDialog.FileName);
                var a = new InferImage();
                List<float[]> probabities = a.RecImg(bmpTest);
                label1.Text = probabities[probabities.Count() - 1][0].ToString()+" 概率为"
                    + probabities[probabities.Count() - 1][1].ToString();
                //plan：发现一个原点，把所有和这个点连着的点视为同一个
                /*
                //尝试迭代取图片
                Console.WriteLine(pictureBox1.Height);

                Console.WriteLine(pictureBox1.Height/2);
                Console.WriteLine(pictureBox1.Width/2);
                for (int minboxW = pictureBox1.Height / 4;minboxW<pictureBox1.Width;minboxW+=10) {//最小的框高度
                    for(int minboxH = pictureBox1.Height / 2; minboxH < pictureBox1.Height; minboxH+=10)
                    {
                    Console.WriteLine("11");
                        for (int y = 0; y < pictureBox1.Height - minboxH; y+=10)
                        {
                            for (int x = 0; x < pictureBox1.Width - minboxW; x+=10)
                            {
                                Bitmap bm = new Bitmap(minboxW, minboxH);//创建框
                                Graphics graphic = Graphics.FromImage(bm);//作图区域
                                graphic.DrawImage(pictureBox1.Image, 0, 0,
                                    new Rectangle(x, y, minboxW, minboxH), GraphicsUnit.Pixel);
                                Bitmap saveImage = Image.FromHbitmap(bm.GetHbitmap());
                                List<float[]> prob = a.RecImg(saveImage);
                                float best =0;
                                float bestIdx = 0;
                                for (int i = 0; i < prob.Count(); i++)
                                {
                                    if (prob[i][1] > best)
                                    {
                                        bestIdx = prob[i][0];
                                        best = prob[i][1];
                                        //Console.WriteLine(bestIdx.ToString() + "    ");
                                        Console.Write(bestIdx);
                                    }
                                }
                                saveImage.Dispose();
                                graphic.Dispose();
                                bm.Dispose();
                                Console.Write("\n");
                        }
                        }
                    }

                }
                */

            }

        }

    }
}
