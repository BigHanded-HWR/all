using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using TensorFlow;
using System.Drawing.Drawing2D;

namespace hwr
{
    public class InferImage
    {
        TFGraph graph;
        TFSession session;
        byte[] model;
       
        private void Load()
        {
            graph = new TFGraph();
            model = File.ReadAllBytes("my.pb");
            session = new TFSession(graph);
            graph.Import(model, "");
        }
        public List<float[]> RecImg(Bitmap bmpTest)
        {
            this.Load();
            //Bitmap bmpTest = new Bitmap(imgName);
            //处理接受到的图片
            if (!((bmpTest.Height == 28) && (bmpTest.Width == 28))) bmpTest = GetSmall(bmpTest);
            float[] Value = new float[784];

            float pixelmin = bmpTest.GetPixel(0, 0).R;
            float pixelmax = bmpTest.GetPixel(0, 0).R;
            for (int y = 0; y < bmpTest.Height; y++)
            {
                for (int x = 0; x < bmpTest.Width; x++)
                {
                    if (pixelmin > bmpTest.GetPixel(x, y).R)
                    {
                        pixelmin = bmpTest.GetPixel(x, y).R;
                    }
                    if (pixelmax < bmpTest.GetPixel(x, y).R)
                    {
                        pixelmax = bmpTest.GetPixel(x, y).R;
                    }
                }
            }

            for (int y = 0; y < bmpTest.Height; y++)
            {
                for (int x = 0; x < bmpTest.Width; x++)
                {
                    Value[y * 28 + x] = ((float)bmpTest.GetPixel(x, y).R - pixelmin) / (pixelmax - pixelmin);
                }
            }

            var tensor = new TFTensor(Value);
            var runner = session.GetRunner();
            runner.AddInput(graph["input"][0], tensor).AddInput(graph["keep_prob"][0], 1.0f).Fetch(graph["output"][0]);
            var output = runner.Run();
            
            var result = output[0];
            var bestIdx = 0;

            List <float[]> inf = new List<float[]>();
            float best = 0;
            var probabilities = ((float[][])result.GetValue(jagged: true))[0];
            for (int i = 0; i < probabilities.Length; i++)
            {
                if (probabilities[i] > best)
                {
                    float[] a = new float[2];
                    bestIdx = i;
                    a[0] = i;
                    a[1] = probabilities[i];
                    inf.Add(a);
                    best = probabilities[i];
                    //Console.WriteLine(bestIdx.ToString() + "    ");
                    //Console.WriteLine(probabilities[i]);
                }
            }


            //return bestIdx.ToString();
            //return probabilities;
            return inf;

        }
        private Bitmap GetSmall(Bitmap bm)
        {
            //int nowWidth = (int)(bm.Width / times);
            //int nowHeight = (int)(bm.Height / times);
            Bitmap newbm = new Bitmap(28, 28);//新建一个放大后大小的图片

            //if (times >= 1 && times <= 1.1)
            //{
               // newbm = bm;
            //}
           // else
            //{
                Graphics g = Graphics.FromImage(newbm);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(bm, new Rectangle(0, 0, 28, 28), new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                g.Dispose();
           // }
            return newbm;
        }
    }
    
}
