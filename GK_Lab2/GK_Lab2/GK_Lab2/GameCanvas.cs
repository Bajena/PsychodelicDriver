using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK_Lab2.Properties;

namespace GK_Lab2
{
    internal class GameCanvas : PictureBox
    {
        public Bitmap Backbuffer {get; set; }
        public static Image Car = Resources.car_mid;
        public static Image Background = Resources.bg_mid;
        public static Bitmap StopSignPattern = new Bitmap(Resources.red_pattern);

        public RoadStateManager RoadState { get; set; }
        public static Color WindowColor = Color.FromArgb(127, Color.Gray);
        public static Color RoadStripColor = Color.Yellow;
        public static Color StopSignDarkColor = Color.Black;

        public static double Ratio { get { return Car.Height/754.0; } }

        public GameCanvas()
        {
            this.Dock = DockStyle.Fill;
            this.Location = new Point(0, 0);
            this.Size = new Size(GameCanvas.Car.Width, GameCanvas.Car.Height);
            this.TabIndex = 0;
            this.TabStop = false;
            this.RoadState = new RoadStateManager();
        }

        public void CreateBackBuffer(object sender, EventArgs e)
        {
            if (Backbuffer != null)
                Backbuffer.Dispose();

            Backbuffer = new Bitmap(this.Size.Width, this.Size.Height);
        }

        public void Draw()
        {
                using (var g = Graphics.FromImage(Backbuffer))
                {
                    g.DrawImageUnscaled(GameCanvas.Background,0,0);
                    
                    foreach(var roadObject in RoadState.RoadObjects)
                        roadObject.Draw(g,Backbuffer);

                    g.DrawImageUnscaled(GameCanvas.Car,0,0);

                    //szyby
                    g.FillPolygon(RoadState.LeftWindow, WindowColor);
                    g.FillPolygon(RoadState.RightWindow, WindowColor);
                }

                Invalidate();
        }

    }
}
