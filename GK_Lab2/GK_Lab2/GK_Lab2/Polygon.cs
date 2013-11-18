using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GK_Lab2
{
    public class Polygon
    {
       protected Point[] points;

        public Point[] Points
        {
            get
            {
                return this.points;
            }
        }

        public int MaxX
        {
            get { return points.Max(p => p.X); }
        }
        public int MinX
        {
            get { return points.Min(p => p.X); }
        }

        public int MaxY
        {
            get { return points.Max(p => p.Y); }
        }
        public int MinY
        {
            get { return points.Min(p => p.Y); }
        }

        public Polygon(Point[] vertices)
        {
            this.points = new Point[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                this.points[i] = new Point(vertices[i].X, vertices[i].Y);
            }
        }

        public Polygon(List<Point> vertices)
        {
            this.points = new Point[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                this.points[i] = new Point(vertices[i].X, vertices[i].Y);
            }
        }

        public void Scale(float scale)
        {
            Matrix m = new Matrix();
            m.Scale(scale, scale);
            m.TransformPoints(Points);
            m.Dispose();
        }

        public void Move(int dx, int dy)
        {
            for (int i = 0; i < this.points.Length; i++)
            {
                this.points[i].X = this.points[i].X + dx;
                this.points[i].Y = this.points[i].Y + dy;
            }
        }

        public Polygon ClipToPolygon(Polygon clipper)
        {
            Point[] inPolygonVertices = new Point[this.Points.Length + 2 * clipper.Points.Length];
            for (int i = 0; i < this.Points.Length; i++)
            {
                inPolygonVertices[i] = this.Points[i];
            }
            Point[] outPolygonVertices = new Point[this.Points.Length + 2 * clipper.Points.Length];

            Polygon.ClipEdge clipEdge = new Polygon.ClipEdge(clipper.Points.Last(), clipper.Points.First());
            int num = this.SutherlandHodgmanPolygonClip(inPolygonVertices, this.Points.Length, outPolygonVertices, clipEdge);

            for (int j = 0; j < clipper.points.Length - 1; j++)
            {
                Point[] tmpVertices = inPolygonVertices;
                inPolygonVertices = outPolygonVertices;
                outPolygonVertices = tmpVertices;
                clipEdge = new Polygon.ClipEdge(clipper.Points[j], clipper.Points[j + 1]);
                num = this.SutherlandHodgmanPolygonClip(inPolygonVertices, num, outPolygonVertices, clipEdge);
            }

            Point[] clippedPolygonVertices = new Point[num];
            for (int k = 0; k < num; k++)
            {
                clippedPolygonVertices[k] = outPolygonVertices[k];
            }
            return new Polygon(clippedPolygonVertices);
        }

        private bool Inside(Point p, Polygon.ClipEdge ClipBoundary)
        {
            return ClipBoundary.IsPointInside(p);
        }

        private Point Intersect(Point s, Point p, Polygon.ClipEdge ClipBoundary)
        {
            return ClipBoundary.IntersectWith(s, p);
        }

        private int SutherlandHodgmanPolygonClip(Point[] inArray, int inLength, Point[] outArray, Polygon.ClipEdge clipBoundary)
        {
            if (inLength < 3)
            {
                return 0;
            }
            int num = 0;

            Point point = inArray[inLength - 1];
            for (int i = 0; i < inLength; i++)
            {
                Point point1 = inArray[i];
                if (this.Inside(point1, clipBoundary))
                {
                    if (!this.Inside(point, clipBoundary))
                    {
                        int num1 = num;
                        num = num1 + 1;
                        outArray[num1] = this.Intersect(point, point1, clipBoundary);
                        int num2 = num;
                        num = num2 + 1;
                        outArray[num2] = point1;
                    }
                    else
                    {
                        int num3 = num;
                        num = num3 + 1;
                        outArray[num3] = point1;
                    }
                }
                else if (this.Inside(point, clipBoundary))
                {
                    int num4 = num;
                    num = num4 + 1;
                    outArray[num4] = this.Intersect(point, point1, clipBoundary);
                }
                point = point1;
            }
            return num;
        }

        private class ClipEdge
        {
            private Point p1;

            private Point p2;

            public ClipEdge(Point p1, Point p2)
            {
                this.p1 = p1;
                this.p2 = p2;
            }

            public Point IntersectWith(Point start, Point end)
            {
                double a;
                double a1;
                int x;
                int y;
                int dx = end.X - start.X;
                int dy = end.Y - start.Y;

                int dx1 = this.p2.X - this.p1.X;
                int dy1 = this.p2.Y - this.p1.Y;

                if (dx==0)
                {
                    a1 = (double)(dy1) / (dx1);
                    x = start.X;
                    y = (int)(a1 * (x - this.p1.X) + this.p1.Y);
                    return new Point(x, y);
                }
                if (this.p1.X == this.p2.X)
                {
                    a = (double)(dy) / (dx);
                    x = this.p1.X;
                    y = (int)(a * (x - start.X) + start.Y);
                    return new Point(x, y);
                }

                a = (double)(dy) / (dx);
                a1 = (double)(dy1) / (dx1);
                x = (int)((a * start.X - start.Y - a1 * this.p1.X + this.p1.Y) / (a - a1) + 0.5);
                y = (int)(a * (x - start.X) + start.Y + 0.5);
                return new Point(x, y);
            }

            public bool IsPointInside(Point p)
            {
                int dx = this.p2.X - this.p1.X;
                int dy = this.p2.Y - this.p1.Y;
                int dx1 = p.X - this.p1.X;
                int dy1 = p.Y - this.p1.Y;
                return dx1 * dy - dy1 * dx >= 0;
            }
        }
    }
}