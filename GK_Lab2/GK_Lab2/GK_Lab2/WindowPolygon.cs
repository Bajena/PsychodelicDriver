using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Lab2
{
    public class WindowPolygon : Polygon
    { 
        public static Polygon LeftWindowBase = new Polygon(new List<Point>()
            {
                new Point(0,11),new Point(0,437),new Point(268,379),new Point(281,267),new Point(235,174),new Point(188,116),new Point(146,77)
            });
        
        public static Polygon RightWindowBase = new Polygon(new List<Point>()
            {
                new Point(1580,29),new Point(1454,84),new Point(1395,125),new Point(1344,199),new Point(1307,271),new Point(1321,384),new Point(1580,441)
            });


        private Rectangle _clipper;
        private Polygon basePolygon;

        static WindowPolygon()
        {
            LeftWindowBase.Scale((float)GameCanvas.Ratio);
            RightWindowBase.Scale((float)GameCanvas.Ratio);
        }

        public WindowPolygon(Point[] vertices) : base(vertices)
        {
            basePolygon = new Polygon(vertices);
            _clipper = new Rectangle(MinX,MinY,MaxX-MinX,MaxY-MinY);
        }

        public WindowPolygon(List<Point> vertices) : base(vertices)
        {
            basePolygon = new Polygon(vertices);
            _clipper = new Rectangle(MinX, MinY, MaxX - MinX, MaxY - MinY);
        }

        public void MoveWindow(int dy)
        {
            if ((dy<0 && _clipper.Y - dy >= basePolygon.MinY ) || (dy>0 && _clipper.Y + dy<=basePolygon.MaxY))
            {
                _clipper = new Rectangle(basePolygon.MinX, _clipper.Y + dy, basePolygon.MaxX - basePolygon.MinX, basePolygon.MaxY - (_clipper.Y + dy));
                this.points = basePolygon.ClipToPolygon(GetClippingRectangle()).Points;
            }

        }

        public Polygon GetClippingRectangle()
        {
            return new Polygon(new List<Point>()
                {
                    new Point(_clipper.Left,_clipper.Top),new Point(_clipper.Left,_clipper.Bottom),
                    new Point(_clipper.Right,_clipper.Bottom),new Point(_clipper.Right,_clipper.Top)
                });
        }

    }
}
