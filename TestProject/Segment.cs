using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace TestProject
{
    /// <summary>
    /// 세그먼트
    /// </summary>
    public class Segment
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region Field

        /// <summary>
        /// 펜
        /// </summary>
        public Pen Pen;
        public Font Font = new Font("Fixsys", 10, FontStyle.Bold);
        public Brush Brush = new SolidBrush(Color.FromArgb(0,0,0));
        public RectangleF rect;
        /// <summary>
        /// 시작점
        /// </summary>
        public Point Point1;
        
        /// <summary>
        /// 종료점
        /// </summary>
        public Point Point2;
        public Point Point3;

        /// <summary>
        /// point3 : point3 확인 후 있다면 추가
        /// type : draw 시 타입 확인 후 추가
        /// </summary>
        bool point3Flag;
        public string type;
        #endregion


        #region 생성자 - Segment(pen, point1, point2)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="pen">펜</param>
        /// <param name="point1">시작점</param>
        /// <param name="point2">종료점</param>
        public Segment(Pen pen, Point point1, Point point2, Point? point3 = null)
        {
            Pen = pen;
            Point1 = point1;
            Point2 = point2;
            point3Flag = false;

            if (point3.HasValue)
            {
                Point3 = (Point)point3;
                point3Flag = true;
            }
        }

        #endregion

        #region 그리기 - Draw(graphics)

        /// <summary>
        /// 그리기
        /// </summary>
        /// <param name="graphics">그래픽스</param>
        public void Draw(Graphics graphics)
        {
            switch (type)
            {
                case "line":
                    graphics.DrawLine(Pen, Point1, Point2);
                    graphics.DrawLine(Pen, Point2, Point3);
                    break;
                case "text":
                    graphics.DrawLine(Pen, Point1, Point2);
                    DrawLineText(graphics); // --2
                    break;
                case "agree":
                    graphics.DrawLine(Pen, Point1, Point2);
                    graphics.DrawLine(Pen, Point2, Point3);
                    DrawAngle(graphics);
                    break;
                default:
                    graphics.DrawLine(Pen, Point1, Point2);
                    break;
            }
        }
        public void Draw1(Graphics graphics)
        {
            if (point3Flag)
            {
                graphics.DrawLine(Pen, Point2, Point3);
            }
            //DrawLineText(graphics); // --2
        }


        /// <summary>
        /// 길이 텍스트 drawLine
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pixelPitch">(모니터인치 * 25.4)/(가로해상도 ^2 + 세로해상도^2)^0.5 = 픽셀피치</param>
        public void DrawLineText(Graphics graphics)
        {
            float start_X = Point1.X;
            float start_Y = Point1.Y;
            float end_X = Point2.X;
            float end_Y = Point2.Y;

            float pixelPitch = (float)((23.5 * 25.4) / Math.Pow(Math.Pow(Screen.PrimaryScreen.Bounds.Width, 2) + Math.Pow(Screen.PrimaryScreen.Bounds.Height, 2), 0.5));
            float dist = (float)(Math.Sqrt(Math.Pow(start_X - end_X, 2) + Math.Pow(start_Y - end_Y, 2)) * pixelPitch);
            string str_dist = " _ _ " + dist.ToString("N2") + "mm";

            //graphics.DrawLine(Pen, Point1, Point2);

            if (start_X > end_X)
            {
                DrawText(str_dist, graphics, start_X, start_Y);
            }
            else
            {
                DrawText(str_dist, graphics, end_X, end_Y);
            }
        }

        /// <summary>
        /// 각도텍스트 drawLine
        /// </summary>
        public void DrawAngle(Graphics graphics)
        {
            float pixelPitch = (float)((23.5 * 25.4) / Math.Pow(Math.Pow(Screen.PrimaryScreen.Bounds.Width, 2) + Math.Pow(Screen.PrimaryScreen.Bounds.Height, 2), 0.5));
            float Firstdist = (float)(Math.Sqrt(Math.Pow(Point1.X - Point2.X, 2) + Math.Pow(Point1.Y - Point2.Y, 2)) * pixelPitch);
            float Twodist = (float)(Math.Sqrt(Math.Pow(Point2.X - Point3.X, 2) + Math.Pow(Point2.Y - Point3.Y, 2)) * pixelPitch);
            float Threedist = (float)(Math.Sqrt(Math.Pow(Point1.X - Point3.X, 2) + Math.Pow(Point1.Y - Point3.Y, 2)) * pixelPitch);

            float agree = (float)(Math.Pow(Firstdist, 2) + Math.Pow(Twodist, 2) - Math.Pow(Threedist, 2)) / (2 * Firstdist * Twodist);
            float angle = (float)(180 * Math.Acos(agree) / Math.PI);
            if (float.IsNaN(angle))
            {
                angle = 0;
            }
            string ang_str = " _ _ " + angle.ToString("N2") + "°C";
            DrawText(ang_str, graphics, Point2.X, Point2.Y);

        }

        public void DrawText(string text, Graphics graphics, float Point_X, float Point_Y)
        {
            // Get the string's dimensions.
            SizeF size = graphics.MeasureString(text, Font);

            // Make a rectangle to contain the text.
            float y = 0;
            y = y - size.Height;

            //text Rectangle create
            rect = new RectangleF((float)(-size.Width * 0.01), (float)(y * 0.85), size.Width, size.Height);

            graphics.TranslateTransform(Point_X, Point_Y, MatrixOrder.Append);

            //Find Rectangle Center
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                graphics.DrawString(text, Font, Brush, rect, sf);
            }

            graphics.TranslateTransform(-Point_X, -Point_Y, MatrixOrder.Append);
        }
        #endregion
    }
}