using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace EZCode.GControls
{
    public interface IGControl
    {
        string Method { get; set; }
        string click { get; set; }
        string mousehover { get; set; }
        string move { get; set; }
        string scale { get; set; }
        string backcolor { get; set; }
        string forecolor { get; set; }
        string font { get; set; }
        string image { get; set; }
        string imagelayout { get; set; }
        string BGImageFile { get; set; }
    }
    public partial class GControl : Control, IGControl
    {
        public string Method { get; set; }
        public string click { get; set; }
        public string mousehover { get; set; }
        public string move { get; set; }
        public string scale { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public string font { get; set; }
        public string image { get; set; }
        public string imagelayout { get; set; }
        public string text { get; set; }
        public string BGImageFile { get; set; }
    }
    public partial class GShape : PictureBox, IGControl
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
        public string Method { get; set; }
        public string click { get; set; }
        public string mousehover { get; set; }
        public string move { get; set; }
        public string scale { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public string font { get; set; }
        public string image { get; set; }
        public string imagelayout { get; set; }
        public string text { get; set; }
        public string BGImageFile { get; set; }
        public Type type;

        public GShape(Type type = Type.Square, int? poly = null, PointF[] points = null)
        {
            //sets all values
            if (poly != null) Poly = (int)poly;
            if (points != null) Points = points;
            this.type = type;

            this.Left = 0;
            this.Top = 0;
            this.Width = 50;
            this.Height = 50;
            this.BackColor = Color.Black;

            Poly = type == Type.Square ? 4 : Poly;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (GraphicsPath obj = new GraphicsPath())
            {
                if (type == Type.Square)
                {
                    Rectangle rectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                    obj.AddRectangle(rectangle);
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
                else if (type == Type.Circle)
                {
                    obj.AddEllipse(0, 0, this.Width - 1, this.Height - 1);
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
                else if (type == Type.Triangle)
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
                else if (type == Type.Polygon && Poly > 2)
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
                else if (type == Type.Custom)
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
    public partial class GButton : Button, IGControl
    {
        public string Method { get; set; }
        public string click { get; set; }
        public int isclick { get; set; }
        public string mousehover { get; set; }
        public string move { get; set; }
        public string scale { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public string font { get; set; }
        public string image { get; set; }
        public string imagelayout { get; set; }
        public string text { get; set; }
        public string BGImageFile { get; set; }
        public GButton()
        {
            this.Left = 0;
            this.Top = 0;
            this.Width = 75;
            this.Height = 25;
            this.BackColor = Color.White;
        }
    }
    public partial class GTextBox : TextBox, IGControl
    {
        public string Method { get; set; }
        public string click { get; set; }
        public string mousehover { get; set; }
        public string move { get; set; }
        public string scale { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public string font { get; set; }
        public string image { get; set; }
        public string imagelayout { get; set; }
        public string text { get; set; }
        public string BGImageFile { get; set; }
        public GTextBox()
        {
            this.Left = 0;
            this.Top = 0;
            this.Width = 75;
            this.Height = 25;
            this.BackColor = Color.White;
        }
    }
    public partial class GLabel : Label, IGControl
    {
        public string Method { get; set; }
        public string click { get; set; }
        public string mousehover { get; set; }
        public string move { get; set; }
        public string scale { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public string font { get; set; }
        public string image { get; set; }
        public string imagelayout { get; set; }
        public string text { get; set; }
        public string BGImageFile { get; set; }
        public GLabel()
        {
            this.Left = 0;
            this.Top = 0;
            this.Width = 75;
            this.Height = 25;
            this.AutoSize = true;
            this.BackColor = Color.White;
        }
    }
}