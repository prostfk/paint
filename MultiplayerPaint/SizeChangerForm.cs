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

namespace MultiplayerPaint
{
    public partial class SizeChangerForm : Form
    {
        public SizeChangerForm()
        {
            InitializeComponent();
            TopMost = true;
        }
        private int sizeOfPen;
        public int sizeOfPenAttr
        {
            get { return sizeOfPen; }
            set { sizeOfPen = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //sizeOfPen = int.Parse(textBox1.Text);
                sizeOfPenAttr = int.Parse(textBox1.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Значение от 1 до 100");
            }
            finally
            {
                this.Hide();
            }
        }

        

        

    }
}
