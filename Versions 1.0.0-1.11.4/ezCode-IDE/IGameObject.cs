using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace GControls
{
    internal interface IObject
    {
        PointF[] Points { get; set; }
        int Poly { get; set; }
    }

    public partial class GObject : IObject
    {
        public enum Type
        {
            Square,
            Circle,
            Triangle,
            Polygon,
            Custom,
        }
        public PointF[] Points { get; set; }
        public int Poly { get; set; }
        public Type Square { get; }
        int types;

        public GObject(Type type, int? poly = null, PointF[] points = null)
        {
            //sets all values
            if (poly != null) Poly = (int)poly;
            if (points != null) Points = points;

            switch (type)
            {
                case Type.Square:
                    types = 1;
                    break;
                case Type.Circle:
                    types = 2;
                    break;
                 case Type.Triangle:
                    types = 3;
                    break;
                case Type.Polygon:
                    types = 4;
                    break;
                case Type.Custom:
                    types = 5;
                    break;
            }
        }
    }
    public partial class GObject : PictureBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (GraphicsPath obj = new GraphicsPath())
            {
                if (types == 1)
                {
                    Rectangle rectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                    obj.AddRectangle(rectangle);
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
                else if (types == 2)
                {
                    obj.AddEllipse(0, 0, this.Width - 1, this.Height - 1);
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
                else if (types == 3)
                {
                    obj.AddPolygon(new Point[] {
                        new Point(this.Width / 2, 0),
                        new Point(0, Height),
                        new Point(Width, Height) }
                    );
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
                else if (types == 4 && Poly > 2)
                {
                    PointF center = new PointF(this.Width / 2, this.Height / 2);
                    PointF[] points = new PointF[Poly];
                    for (int i = 0; i < Poly; i++)
                    {
                        float angle = (2 * (float)Math.PI / Poly) * i;
                        float x = center.X + (this.Width / 2) * (float)Math.Cos(angle);
                        float y = center.Y + (this.Height / 2) * (float)Math.Sin(angle);
                        points[i] = new PointF(x, y);
                    }
                    obj.AddPolygon(points);
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
                else if (types == 5)
                {
                    PointF center = new PointF(this.Width / 2, this.Height / 2);
                    obj.AddPolygon(Points);
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
            }
        }
    }
}