using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Lab2
{
    public class PolygonFillHelper
    {
        class Edge
        {
            public double x;
            public double dx;
            public int i;

            public Edge(double x, double dx, int i)
            {
                this.x = x;
                this.dx = dx;
                this.i = i;
            }
        }

        public struct Segment
        {
            public Point p1;
            public Point p2;

            public Segment(Point a, Point b)
            {
                p1 = a;
                p2 = b;
            }

            public Segment(int x1, int x2, int y)
            {
                p1 = new Point(x1, y);
                p2 = new Point(x2, y);
            }
        }

        private List<Point> _vertices;
        private List<Edge> _activeEdges;

        public PolygonFillHelper(List<Point> vertices)
        {
            _vertices = new List<Point>(vertices);
            _activeEdges = new List<Edge>();
        }

        public List<Segment> FillPolygon(Graphics g)
        {
            List<Segment> segments = new List<Segment>();


            List<int> ind = new List<int>();
            int k = 0;
            for (; k < _vertices.Count; k++)
                ind.Add(k);
            ind.Sort((a, b) => _vertices[a].Y <= _vertices[b].Y ? -1 : 1);

            int y0 = (int)Math.Ceiling(_vertices[ind[0]].Y - .5);
            int y1 = (int)Math.Floor(_vertices[ind.Last()].Y - .5);

            k = 0;
            for (int y = y0; y <= y1; y++)
            {
                for (; k < _vertices.Count && _vertices[ind[k]].Y <= y + .5; k++)
                {
                    int i = ind[k];
                    int j = i > 0 ? i - 1 : _vertices.Count - 1;

                    if (_vertices[j].Y <= y - .5)
                        DeleteFromAEL(j);
                    else if (_vertices[j].Y > y + .5)
                        InsertToAEL(j, y);
                    j = i < _vertices.Count - 1 ? i + 1 : 0;

                    if (_vertices[j].Y <= y - .5)
                        DeleteFromAEL(i);
                    else if (_vertices[j].Y > y + .5)
                        InsertToAEL(i, y);
                }

                _activeEdges.Sort((a, b) => a.x <= b.x ? -1 : 1);

                for (int j = 0; j < _activeEdges.Count; j += 2)
                {
                    int x1 = (int)Math.Ceiling(_activeEdges[j].x - .5);
                    int x2 = (int)Math.Floor(_activeEdges[j + 1].x - .5);

                    if (x1 <= x2)
                        segments.Add(new Segment(x1, x2, y));

                    _activeEdges[j].x += _activeEdges[j].dx;
                    _activeEdges[j + 1].x += _activeEdges[j + 1].dx;

                }
            }
            return segments;
        }

        private void InsertToAEL(int i, int y)
        {
            int n = _vertices.Count;
            int j = i < n - 1 ? i + 1 : 0;

            Point p, q;
            if (_vertices[i].Y < _vertices[j].Y)
            {
                p = _vertices[i];
                q = _vertices[j];
            }
            else
            {
                p = _vertices[j];
                q = _vertices[i];
            }

            double dx = (double)((q.X - p.X)) / (double)((q.Y - p.Y));
            double x = (dx * (y + .5 - p.Y) + p.X);


            _activeEdges.Add(new Edge(x, dx, i));

        }

        private void DeleteFromAEL(int i)
        {
            int j = 0;
            for (; j < _activeEdges.Count && _activeEdges[j].i != i; j++) ;

            _activeEdges.RemoveAt(j);


        }

    }
}
