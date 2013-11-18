using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Lab2
{
    public static class GraphicsExtensions
    {
        public static void FillPolygon(this Graphics g, Polygon polygon, Color color)
        {
            var polygonFillHelper = new PolygonFillHelper(new List<Point>(polygon.Points));
            var segments = polygonFillHelper.FillPolygon(g);

            using (Pen pen = new Pen(color))
            {
                foreach (var segment in segments)
                {
                    g.DrawLine(pen, segment.p1, segment.p2);
                }
            }
        }

        public static void FillPolygon(this Graphics g, Polygon polygon, Bitmap texture, Bitmap destination)
        {
            var polygonFillHelper = new PolygonFillHelper(new List<Point>(polygon.Points));
            var segments = polygonFillHelper.FillPolygon(g);

            for (int y = 0; y < segments.Count; y++)
            {
                for (int x = 0; x < segments[y].p2.X - segments[y].p1.X; x++)
                {
                    Color color = texture.GetPixel(x % texture.Width, y % texture.Height);
                    if (segments[y].p1.X + x < destination.Width && segments[y].p1.X + x > 0 && segments[y].p1.Y > 0 && segments[y].p1.Y < destination.Height)
                        destination.SetPixel(segments[y].p1.X + x, segments[y].p1.Y, color);
                }
            }

        }
    }
}
