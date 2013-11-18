using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Lab2
{
    //singleton
    public class RoadStateManager
    {
        private double _speed;

        public double Speed
        {
            get { return _speed; }
            set
            {
                if (value >= 0) _speed = value;
                else _speed = 0;
            }
        }

        public WindowPolygon LeftWindow;
        public WindowPolygon RightWindow;

        public int RoadWidth = GameCanvas.Car.Width;//Szerokość jezdni
        public int RoadY = (int)(181 * GameCanvas.Ratio); //Odległość od góry ekranu gdzie zaczyna się droga
        public int RoadLength = (int)(150.0*GameCanvas.Ratio);

        public double MaxSpeed = 20;
        public double SpeedUpAcceleration = 0.8;
        public double BrakeDecceleration = -1;
        public double LooseDecceleration = -0.4;

        public double Acceleration { get; set; }
        public double PlayerX = 0; // -1 do 1

        public List<RoadObject> RoadObjects; 

        public RoadStateManager()
        {
            Speed = 10;
            Acceleration = this.LooseDecceleration;

            RoadObjects = new List<RoadObject>();
            RoadObjects.Add(new RoadStrip(this, -0.33));
            RoadObjects.Add(new RoadStrip(this, -0.33));
            RoadObjects.Last().Z = RoadLength/2;
            RoadObjects.Add(new RoadStrip(this, 0.33));
            RoadObjects.Add(new RoadStrip(this, 0.33));
            RoadObjects.Last().Z = RoadLength/2;

            RoadObjects.Add(new StopSign(this, -1));
            RoadObjects.Add(new StopSign(this, 1));
            RoadObjects.Last().Z = 35;

            LeftWindow = new WindowPolygon(WindowPolygon.LeftWindowBase.Points);
            RightWindow = new WindowPolygon(WindowPolygon.RightWindowBase.Points);
        }

        public void Update()
        {
            if (Speed + Acceleration>MaxSpeed)
                Speed = MaxSpeed;
            else if  (Speed + Acceleration < 0)
                Speed = 0;
            else Speed += Acceleration;

            foreach (var roadObject in RoadObjects)
            {
                roadObject.Update();
            }
        }

        
    }
}
