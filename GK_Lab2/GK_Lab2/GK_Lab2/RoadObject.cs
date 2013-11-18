using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK_Lab2.Properties;

namespace GK_Lab2
{
    public abstract class RoadObject
    {
        protected RoadStateManager _roadStateManager;

        public double Z { get; set; } //Odległość od samochodu
        public abstract double DistFromCar { get; }
        public double X { get; set; } //-1 do 1

        public abstract void Update();
        public abstract void CalculatePositions();
        public abstract void Draw(Graphics graphics,Bitmap backbuffer);
    }

    public class StopSign : RoadObject
    {
        public StopSignPolygon PolygonRepresentation { get; set; }
        
        public StopSign(RoadStateManager roadStateManager)
        {

            this._roadStateManager = roadStateManager;
            this.Z = _roadStateManager.RoadLength;
            this.X = 0;
            this.PolygonRepresentation = new StopSignPolygon(StopSignPolygon.StopSign.Points);
            CalculatePositions();
        }

        public StopSign(RoadStateManager roadStateManager, double x)
        {
            this._roadStateManager = roadStateManager;
            this.Z = _roadStateManager.RoadLength;
            this.X = x;
            this.PolygonRepresentation = new StopSignPolygon(StopSignPolygon.StopSign.Points);
            CalculatePositions();
        }

        public override double DistFromCar
        {
            get { return _roadStateManager.RoadLength - Z; }
        }

        //Przesuwanie obiektu
        public override void Update()
        {
            Z -= _roadStateManager.Speed;
            if (Z <= 0)
                Z = _roadStateManager.RoadLength;
            CalculatePositions();
        }

        //Obliczenie nowych pozycji
        public override void CalculatePositions()
        {
            double scale1 = (DistFromCar) / _roadStateManager.RoadLength;

            double yscreen = _roadStateManager.RoadY + DistFromCar;

            double xscreen = (_roadStateManager.RoadWidth/2)*(1 + X*scale1);

            PolygonRepresentation.ScaleFromOriginal(scale1);
            PolygonRepresentation.MoveTo((int) xscreen,(int) yscreen);
        }

        public override void Draw(Graphics g, Bitmap backbuffer)
        {
            g.FillPolygon(PolygonRepresentation, GameCanvas.StopSignPattern, backbuffer);
            
            using (var brush = new SolidBrush(GameCanvas.StopSignDarkColor))
            {
                //Lewa szyba
                if (X < 0)
                {
                    var windowClippedPolygon = PolygonRepresentation.ClipToPolygon(_roadStateManager.LeftWindow).Points;
                    if (windowClippedPolygon.Length > 0)
                        g.FillPolygon(brush, windowClippedPolygon);
                }
                else //Prawa szyba
                {
                   var windowClippedPolygon = PolygonRepresentation.ClipToPolygon(_roadStateManager.RightWindow).Points;
                    if (windowClippedPolygon.Length > 0)
                        g.FillPolygon(brush, windowClippedPolygon);
                }
            }
        }
    }

    public class RoadStrip : RoadObject
    {
        public Polygon PolygonRepresentation { get; set; }
        public static int StripLength = (int)(50*GameCanvas.Ratio); //Długość w Z-coord
        public static int StripWidth = (int)(20*GameCanvas.Ratio);

        public RoadStrip(RoadStateManager roadStateManager)
        {
            this._roadStateManager = roadStateManager;
            this.Z = _roadStateManager.RoadLength;
            this.X = 0;
            CalculatePositions();
        }

        public RoadStrip(RoadStateManager roadStateManager, double x)
        {
            this._roadStateManager = roadStateManager;
            this.Z = _roadStateManager.RoadLength;
            this.X = x;
            CalculatePositions();
        }

        public override double DistFromCar { get { return _roadStateManager.RoadLength - Z; } }

        public override void Update()
        {
            Z -= _roadStateManager.Speed;
            if (Z <= 0)
                Z = _roadStateManager.RoadLength;
            CalculatePositions();
        }

        public override void CalculatePositions()
        {
            
            //double scaledStripLength = StripLength
            double scale1 = DistFromCar / _roadStateManager.RoadLength;

            double scale2 = (DistFromCar+StripLength*(DistFromCar/_roadStateManager.RoadLength)) / _roadStateManager.RoadLength;

            double rwdiv2 = (_roadStateManager.RoadWidth/2);

            double y1screen = _roadStateManager.RoadY + DistFromCar;
            double y2screen = _roadStateManager.RoadY + _roadStateManager.RoadLength*scale2;
            
            double x1screen = rwdiv2*(1 + X*scale1) - StripWidth * scale1;
            double x2screen = rwdiv2 * (1 + X * scale1) + StripWidth * scale1;

            double x3screen = rwdiv2 * (1 + X * scale2) - StripWidth * scale2;
            double x4screen = rwdiv2 * (1 + X * scale2) + StripWidth * scale2;

            List<Point> vertices = new List<Point>();
            //dodaje przeciwnie do wsk zegara
            vertices.Add(new Point((int)x2screen, (int)y1screen));
            vertices.Add(new Point((int)x1screen, (int)y1screen));
            vertices.Add(new Point((int)x3screen, (int)y2screen));
            vertices.Add(new Point((int)x4screen, (int)y2screen));

            PolygonRepresentation = new Polygon(vertices);
        }

        public override void Draw(Graphics g, Bitmap backbuffer)
        {
            g.FillPolygon(PolygonRepresentation,GameCanvas.RoadStripColor);
        }
    }
}
