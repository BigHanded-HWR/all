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

        private void op()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            Console.WriteLine("1111");
            //openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files(*.*)|*>**";
            openFileDialog.Filter = "jpg files (*.png)|*.png;|doc|*.doc|All files(*.*)|*>**;";
            openFileDialog.FilterIndex = 1;

            Console.WriteLine("1111");
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var a = new InferImage();
                Bitmap bmpTest = new Bitmap(openFileDialog.FileName);
                label1.Text = a.RecImg(bmpTest);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            /*
            Thread threadRec = new Thread(op);
            threadRec.SetApartmentState(ApartmentState.STA);
            //threadRec.IsBackground = false;
            threadRec.Start();
            */

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            Console.WriteLine("1111");
            //openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files(*.*)|*>**";
            openFileDialog.Filter = "jpg files (*.png)|*.png;|doc|*.doc|All files(*.*)|*>**;";
            openFileDialog.FilterIndex = 1;

            Console.WriteLine("1111");
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var a = new InferImage();
                Bitmap bmpTest = new Bitmap(openFileDialog.FileName);
                label1.Text = a.RecImg(bmpTest);
            }
        }
    }
}
