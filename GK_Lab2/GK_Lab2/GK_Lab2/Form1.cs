using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK_Lab2.Properties;

namespace GK_Lab2
{
    public partial class Form1 : Form
    {
        private GameCanvas _gameCanvas;

        public Form1()
        {
            InitializeComponent();

            this._gameCanvas = new GameCanvas();
            this.Controls.Add(_gameCanvas);
            
            this.Size = new Size(GameCanvas.Car.Width,GameCanvas.Car.Height);

            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer, true);


            base.KeyUp += this.Form1_KeyUp;
            base.KeyDown += this.Form1_KeyDown;
            this.ResizeEnd += _gameCanvas.CreateBackBuffer;
            this.Load += _gameCanvas.CreateBackBuffer;
            this.Paint += Form1_Paint;
            this._gameCanvas.Paint += gameCanvas_Paint;

            timer1.Start();
        }


        void gameCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(_gameCanvas.Backbuffer, Point.Empty);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                _gameCanvas.RoadState.Acceleration = _gameCanvas.RoadState.LooseDecceleration;
            }
            else if (e.KeyCode == Keys.Down)
            {
                _gameCanvas.RoadState.Acceleration = _gameCanvas.RoadState.LooseDecceleration;
            }   
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
            {
                _gameCanvas.RoadState.LeftWindow.MoveWindow(10);
            }
            else if (e.KeyCode == Keys.W)
            {
                _gameCanvas.RoadState.LeftWindow.MoveWindow(-10);
            }
            if (e.KeyCode == Keys.E)
            {
                _gameCanvas.RoadState.RightWindow.MoveWindow(-10);
            }
            else if (e.KeyCode == Keys.D)
            {
                _gameCanvas.RoadState.RightWindow.MoveWindow(10);
            }                    
            else if (e.KeyCode == Keys.Up)
            {
                _gameCanvas.RoadState.Acceleration = _gameCanvas.RoadState.SpeedUpAcceleration;
            }
            else if (e.KeyCode == Keys.Down)
            {
                _gameCanvas.RoadState.Acceleration = _gameCanvas.RoadState.BrakeDecceleration;
            }   
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _gameCanvas.RoadState.Update();
            _gameCanvas.Draw();
        }
    }
}
