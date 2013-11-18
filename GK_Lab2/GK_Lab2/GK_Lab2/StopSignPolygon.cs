using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Lab2
{
    public class StopSignPolygon : Polygon
    {
        public static Polygon StopSign = new Polygon(new List<Point>()
            {
                 new Point(120,466),new Point(133, 466), new Point(133, 240), new Point(170, 240), new Point(240, 170), new Point(240, 70), new Point(170,0),new Point(70, 0), new Point(0, 70), new Point(0, 170), new Point(70, 240),new Point(108, 240),new Point(108, 466)
            }); 
        
        static StopSignPolygon()
        {
            StopSign.Scale((float)GameCanvas.Ratio);
            StopSign.Scale((float) 0.7);
        }

        public StopSignPolygon(Point[] vertices) : base(vertices)
        {
            
        }

        public StopSignPolygon(List<Point> vertices) : base(vertices)
        {
        }

        //Przesuwa znak, tak że podstawa jest w punkcie x,y
        public void MoveTo(int x,int y)
        {
            int dx = x - Points[0].X;
            int dy = y - Points[0].Y;

            for (int i = 0;i<Points.Length;i++)
            {
                Points[i].X += dx;
                Points[i].Y += dy;
            }
        }

        public void ScaleFromOriginal(double scale)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].X = StopSignPolygon.StopSign.Points[i].X;
                Points[i].Y = StopSignPolygon.StopSign.Points[i].Y;
            }
            Scale((float)scale);
        }
    }
}
