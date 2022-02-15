using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TestProject
{
    /// <summary>
    /// 메인 폼
    /// </summary>
    public partial class MainForm : Form
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 세그먼트 리스트
        /// </summary>
        private List<Segment> segmentList = new List<Segment>();

        /// <summary>
        /// 신규 세그먼트
        /// </summary>
        private Segment newSegment = null;
        private string type;
        private bool recordFlag = false;
        private bool nextFlag = false;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public
        #region 생성자 - MainForm()

        /// <summary>
        /// 생성자
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            this.pictureBox1.Paint     += canvasPictureBox_Paint;
            this.pictureBox1.MouseDown += canvasPictureBox_MouseDown;
            this.pictureBox1.MouseMove += canvasPictureBox_MouseMove;
            this.pictureBox1.MouseUp += canvasPictureBox_MouseUp;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region 캔버스 픽처 박스 페인트시 처리하기 - canvasPictureBox_Paint(sender, e)

        /// <summary>
        /// 캔버스 픽처 박스 페인트시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void canvasPictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.pictureBox1.BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (Segment segment in this.segmentList)
            {
                segment.Draw(e.Graphics);
            }

            if (this.newSegment != null)
            {
                this.newSegment.Draw(e.Graphics);
            }
        }

        #endregion
        #region 캔버스 픽처 박스 마우스 DOWN 처리하기 - canvasPictureBox_MouseDown(sender, e)

        /// <summary>
        /// 캔버스 픽처 박스 마우스 DOWN 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void canvasPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Down");
            if (radioButton1.Checked && recordFlag == false)
            {
                this.newSegment = new Segment(Pens.Blue, e.Location, e.Location);
                this.newSegment.type = "text";
            }
            else if (radioButton2.Checked && recordFlag == false)
            {
                this.newSegment = new Segment(Pens.Blue, e.Location, e.Location, e.Location);
                this.newSegment.type = "agree";
            }


            this.newSegment.Pen = Pens.Black;


            if (radioButton1.Checked)
            {
                if(recordFlag)
                {
                    Console.WriteLine("recordFlag " + recordFlag);
                    recordFlag = false;
                }
                else
                {
                    Console.WriteLine("recordFlag " + recordFlag);
                    this.segmentList.Add(newSegment);
                    recordFlag = true;
                }
            }
            
            if (radioButton2.Checked)
            {
                if (recordFlag)
                {
                    if (nextFlag)
                    {
                        Console.WriteLine("recordFlag : " + recordFlag + " nextFlag " + nextFlag);
                        nextFlag = false;
                        recordFlag = false;
                        this.segmentList.Add(newSegment);
                    }
                    else
                    {
                        Console.WriteLine("recordFlag : " + recordFlag + " nextFlag " + nextFlag);
                        nextFlag = true;
                    }
                }
                else
                {
                    Console.WriteLine("recordFlag : " + recordFlag + " nextFlag  " + nextFlag);
                    recordFlag = true;
                }
            }

            this.pictureBox1.Refresh();
        }

        #endregion
        #region 캔버스 픽처 박스 마우스 이동시 처리하기 - canvasPictureBox_MouseMove(sender, e)

        /// <summary>
        /// 캔버스 픽처 박스 마우스 이동시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void canvasPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(this.newSegment == null)
            {
                return;
            }


            if (radioButton1.Checked)
            {
                if (recordFlag)
                {
                    this.newSegment.Point2 = e.Location;
                }
            }

            if (radioButton2.Checked)
            {
                if (recordFlag && !nextFlag)
                {
                    this.newSegment.Point2 = e.Location;
                }
                else if(recordFlag && nextFlag)
                {
                    this.newSegment.Point3 = e.Location;
                }
            }

            pictureBox1.Invalidate(false); // WM_PAINT 메시지 발생시킴
            this.pictureBox1.Refresh();
        }

        #endregion
        #region 캔버스 픽처 박스 마우스 UP 처리하기 - canvasPictureBox_MouseUp(sender, e)

        /// <summary>
        /// 캔버스 픽처 박스 마우스 UP 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void canvasPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("UP");

            if(this.newSegment == null)
            {
                return;
            }

            this.pictureBox1.Refresh();
        }

        #endregion



        
    }
}