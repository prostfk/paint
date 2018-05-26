using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Windows.Input;
using prostrmk;
using System.Diagnostics;

namespace MultiplayerPaint
{

    public partial class frmPaint : Form
    {
        PacketManager GP;
        bool update = true;
        bool mouse = true;
        Random RNG;
        Color color = Color.Red;
        int radius = 20;
        public frmPaint()
        {
            InitializeComponent();
            RNG = new Random();
            GP = new PacketManager();
           this.DoubleBuffered = true;
            Thread.Sleep(200);
            timer1.Start();
            update = false;
            
        }

        private void frmPaint_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                Graphics g = this.CreateGraphics();
                for (byte i = 1; i <= GP.PlayerCount; i++)
                {
                    int X = 0, Y = 0;
                    X = GP.PlayerList[i].Position.X;
                    Y = GP.PlayerList[i].Position.Y;
                    g.FillEllipse(new SolidBrush(Color.FromArgb(GP.PlayerList[i].Color)), X - 10, Y - 10, radius, radius);
                    if (X == 0 && Y == 0)
                    {
                        GP.PlayerList[i].Position = new Point(1, 1);
                        GP.Send(TramePreGen.InfoPlayer(GP.PlayerList[i], i, GP.PacketID));
                        this.Refresh();
                    }
                }
                
                if (MouseButtons == MouseButtons.Left)
                {
                    GP.PlayerList[GP.ID].Position = this.PointToClient(Cursor.Position);
                    GP.Send(TramePreGen.InfoPlayer(GP.PlayerList[GP.ID], GP.ID, GP.PacketID));
                }

                if (update)
                {
                    this.Invalidate();
                }
            }));
        }

        private void frmPaint_Paint(object sender, PaintEventArgs e)
        {
            Console.WriteLine("CLICK OPERATION!");
            if (true)
            {
                Invoke(new Action(() =>
                {
                    for (int i = 1; i <= GP.PlayerCount; i++)
                    {
                        int X, Y;
                        X = GP.PlayerList[i].Position.X;
                        Y = GP.PlayerList[i].Position.Y;
                        e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(GP.PlayerList[i].Color)), X - 10, Y - 10, radius, radius);

                        e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(GP.PlayerList[i].Color)), X - 10, Y - 10, radius, radius);
                     
                    }
                    if (mouse)
                    {
                        Point Light = this.PointToClient(Cursor.Position);
                        if (GP.PlayerCount > 0)
                        {
                            GP.PlayerList[GP.ID].Position = Light;
                        }


                    }
                    if (update)
                    {
                        this.Invalidate();
                    }
                }));


            }
        }

        private void frmPaint_Click(object sender, EventArgs e)
        {
            if (update)
            {
                update = false;
                timer1.Start();
            }
            
        }

        private void frmPaint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                byte[] b = new byte[2];
                b[0] = 5;
                b[1] = GP.ID;
                GP.Send(b);
            }
            else
            {
                if (e.KeyData == Keys.Space)
                {
                    using (ColorDialog ColorPicker = new ColorDialog())
                    {
                        if (ColorPicker.ShowDialog() == DialogResult.OK)
                        {
                            GP.PlayerList[GP.ID].Color = ColorPicker.Color.ToArgb();
                            GP.Send(TramePreGen.InfoPlayer(GP.PlayerList[GP.ID], GP.ID, GP.PacketID));
                        }
                    }
                }
                else
                {
                    GP.PlayerList[GP.ID].Position = new Point(0, 0);
                    GP.Send(TramePreGen.InfoPlayer(GP.PlayerList[GP.ID], GP.ID, GP.PacketID));
                }
            }
        }

        private void frmPaint_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        

        

       

        private void открытьНовоеОкноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("MultiplayerPaint.exe");
        }

        private void выбратьЦветToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                using (ColorDialog ColorPicker = new ColorDialog())
                {
                    if (ColorPicker.ShowDialog() == DialogResult.OK)
                    {
                        GP.PlayerList[GP.ID].Color = ColorPicker.Color.ToArgb();
                        GP.Send(TramePreGen.InfoPlayer(GP.PlayerList[GP.ID], GP.ID, GP.PacketID));
                    }
                }
            
        }

        private void выбратьРазмерКистиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SizeChangerForm form = new SizeChangerForm();
            form.ShowDialog();
            radius = form.sizeOfPenAttr;
          
            
        }

        public void SavePicture(string FileName, Graphics panelgr)
        {
            Bitmap memoryImage = new Bitmap((int)panelgr.DpiX, (int)panelgr.DpiY, panelgr);
            memoryImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void отчиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
