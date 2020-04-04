using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace WpfApplication3
{
    /// <summary>
    /// Window3.xaml 的交互逻辑
    /// </summary>
    
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
        }

        #region     输入

        #region     清空
        /// <summary>
        /// 清空当前显示部分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void qingkong(object sender, RoutedEventArgs e)
        {
            bilichi = 1;
            wanjuline.Clear();
            order = 0;
            kaishiBt.Background = Brushes.AliceBlue;
            tbr = 1;
            xishu = 1;
            yuebiao = 1;
            tension = 0;
            wanju1 = 0;
            prac = 0;
            prac_Elli1List.Clear();
            prac1List.Clear();
            prac3List.Clear();
            lian1ListList.Clear();
            lian3ListList.Clear();
            quxianList.Clear();
            quxian3List.Clear();
            qu_Num = 0;
            rigid = true;

            #region     列表清空

            #region     Rectangle列表
            rec1List.Clear();
            rec2List.Clear();
            rec3List.Clear();
            rec = 0;
            #endregion
            
            //杆件列表清空
            m_LineModelList.Clear();
            //铰节点列表清空
            m_HingeJointList.Clear();
            //刚节点列表清空
            m_RigidJointList.Clear();
            //支座列表清空
            m_ZhizuoList.Clear();
            //未知量列表清空
            x_dList.Clear();
            //载荷列表清空
            m_LoadModelList.Clear();
            //关键点列表清空
            m_KeyPonitList.Clear();
            //点列表清空
            pointList.Clear();
            // 画板清空
            can.Children.Clear();
            //支座反力清空
            dxList.Clear();
            //弯矩数据清空
            n_LoadModelList.Clear();
            n_LineLoad_Num = 0;
            n_LineLoadList.Clear();
            shuListList.Clear();
            mLine1List.Clear();
            mLine2List.Clear();
            choose1 = 0;
            choList.Clear();
            huifu1.Clear();
            #endregion

            #region     初始值清空
            ruler = 0;
            n_Load_Num = 0;
            m_Line_Num = 0;
            yueshu = 0;
            m_Line_Num = 0;
            m_Zhizuo_Num = 0;
            m_Load_Num = 0;
            x_Num = 0;
            m_Hinge_Num = 0;
            m_Rigid_Num = 0;
            m_Num = 0;
            f_Num = 0;
            q_Num = 0;
            det = 0;
            #endregion

            tran = new Point();
            startPoint = new Point();
            m_LineNow = new Line();
            m_LineModel = new LineModelClass();
            pracTemp = new PracticeClass();
            wanjuF = new double();
            wanjuLength = new double();
            ruler = new double();
            zz = 0;
            ganjian = true;
            tuodong = false;
            rePlacePt = new Point();
            rigid = true;
            kelin = new int();
            nweishu = new int();
            det = new double();
            this.menuTabctrl.SelectedItem = this.menuTabctrl.Items[0];
        }
        #endregion
        
        #region 变量

        //支座铰节点的标签序号
        private int yuebiao = 1;
        double bilichi=1;
        #region     直线部分
        //直线起点终点转换点//
        private Point tran;

        //网格背景 brush//
        private DrawingBrush _gridBrush;

        /// <summary>
        /// 当前 当前绘制的Line模型的 开始点
        /// </summary>
        private Point startPoint;

        //当前划线信息
        private Line m_LineNow = new Line();

        /// <summary>
        /// model 当前绘制的Line模型
        /// </summary>
        private LineModelClass m_LineModel = new LineModelClass();

        /// <summary>
        /// 当前绘制的所有线段集合
        /// </summary>
        private List<LineModelClass> m_LineModelList = new List<LineModelClass>();

        /// <summary>
        /// 鼠标划线汇集的点的集合
        /// </summary>
        private List<Point> pointList = new List<Point>();


        /// <summary>
        /// 当前线段数 从0开始
        /// </summary>
        private int m_Line_Num = 0;
        private int m_Zhizuo_Num = 0;
        #endregion

        #region     练习部分--变量声明
        private DoubleCollection xuxian = new DoubleCollection { 5 };
        private Rectangle recta = new Rectangle();
        private PracticeClass pracTemp = new PracticeClass();
        private PracListClass lian1List = new PracListClass();
        private PracListClass lian2List = new PracListClass();
        private PracListClass lian3List = new PracListClass();
        private List<PracListClass> lian1ListList = new List<PracListClass>();
        private List<PracListClass> lian2ListList = new List<PracListClass>();
        private List<PracListClass> lian3ListList = new List<PracListClass>();
        private double wanju1;
        private List<Line> mLine1List = new List<Line>();
        private List<Line> mLine2List = new List<Line>();
        #endregion

        #region     弯矩变量
        //转换变量
        private double wanjuF, wanjuLength;
        //比例尺
        private double ruler;
        #endregion

        #region     均布载荷部分的竖线
        private ShuXianClass shu = new ShuXianClass();
        private ShuXianListClass shuList = new ShuXianListClass();
        private List<ShuXianListClass> shuListList=new List<ShuXianListClass>();
        #endregion

        #endregion

        #endregion

        #region 界面触发事件

        #region 界面初始化部分
        
        /// <summary>
        /// 初始化 网格背景 网格间隔20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (_gridBrush == null)
            {
                _gridBrush = new DrawingBrush(new GeometryDrawing(
                    new SolidColorBrush(Colors.White),
                         new Pen(new SolidColorBrush(Colors.LightGray), 1.0),
                             new RectangleGeometry(new Rect(0, 0, 20, 20))));
                _gridBrush.Stretch = Stretch.None;
                _gridBrush.TileMode = TileMode.Tile;
                _gridBrush.Viewport = new Rect(0.0, 0.0, 20, 20);
                _gridBrush.ViewportUnits = BrushMappingMode.Absolute;
                can.Background = _gridBrush;
            }
        }

        /// <summary>
        /// /移动鼠标，显示坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(can);
        }

        #endregion
       
        #region 画布点击事件

        #region  鼠标按下事件

        #region     变量声明
        /// <summary>
        /// 鼠标左键点击 canvas函数 获取鼠标绘制线段的起点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool paint;
        private List<Ellipse> prac_Elli1List = new List<Ellipse>();
        private int prac;
        private PracticeClass prac1 = new PracticeClass();
        private List<PracticeClass> prac1List = new List<PracticeClass>();
        private PracticeClass prac3 = new PracticeClass();
        private List<PracticeClass> prac3List = new List<PracticeClass>();
        /// <summary>
        /// 区分各个圆点是否重合
        /// </summary>
        private bool chonghe;
        /// <summary>
        /// 替代点
        /// </summary>
        private Point ptTemp;
        /// <summary>
        /// 状态区分：0代表选取杆件，1表示某横杆上选取关键点、2某竖杆上选取关键点
        /// 3代表均布载荷虚线上的曲线绘制
        /// </summary>
        private int tension;
        private int choose1;
        private List<Line> choList=new List<Line>();
        private List<Line> huifu1 = new List<Line>();
        private int ganhao;
        /// <summary>
        /// 图一弯矩图上的代表点的图标
        /// </summary>
        private Ellipse elli1;
        /// <summary>
        /// 图一弯矩图上的点
        /// </summary>
        private Point pt_lian1;
        private List<Point> pt_lian1List = new List<Point>();
        private Ellipse redEllipse=new Ellipse ();
        #endregion

        #region 绘图函数

        private void get_startPt(object sender, MouseButtonEventArgs e)
        {
            #region     原图部分
            //初始化 Line 

            m_LineNow = new Line();

            //获取开始点
            startPoint = e.GetPosition(can);
            if (paint == true)
            {
                if ((startPoint.X <x_area/2) && (startPoint.Y > y_area/2) && (startPoint.Y < y_area))
                {
                    #region      原图
                    startPoint.X = (Math.Round(startPoint.X / 20) * 20);
                    startPoint.Y = (Math.Round(startPoint.Y / 20) * 20);

                    //设置开始点坐标

                    m_LineNow.X1 = (Math.Round(startPoint.X / 20)) * 20;
                    m_LineNow.Y1 = (Math.Round(startPoint.Y / 20)) * 20;

                    m_LineNow.X2 = (Math.Round(startPoint.X / 20)) * 20;
                    m_LineNow.Y2 = (Math.Round(startPoint.Y / 20)) * 20;
                    m_LineNow.Name = "Line_" + m_Line_Num;
                    m_LineModel.Line_BeginPoint = startPoint;

                    //圆点加粗
                    Ellipse beginEllipse = new Ellipse();
                    beginEllipse.Height = 4;
                    beginEllipse.Width = 4;
                    beginEllipse.Fill = Brushes.Black;
                    beginEllipse.SetValue(Canvas.LeftProperty, startPoint.X - 2);
                    beginEllipse.SetValue(Canvas.TopProperty, startPoint.Y - 2);
                    can.Children.Add(beginEllipse);
                    #endregion

                    can.Children.Add(m_LineNow);
                }
            }
            #endregion

            #region     练习图
            if((startPoint.X>x_area/2)&&(startPoint.Y>y_area/2)&&(startPoint.Y<y_area))
            {
                try
                {
                    ptTemp = startPoint;

                    #region     选取杆件
                    if (tension == 0)
                    {
                        for (int i = 0; i < m_Line_Num; i++)
                        {
                            #region     横杆
                            if (((ptTemp.Y - mLine1List[i].Y1) <= 2) && ((ptTemp.Y - mLine1List[i].Y1) >= (-2))
                                && ((ptTemp.X - mLine1List[i].X1) * (ptTemp.X - mLine1List[i].X2) <= 0))
                            {
                                
                                tension = 1;
                                #region     所选取杆件变色

                                #region     杆件擦除与新增
                                can.Children.Remove(mLine1List[i]);

                                m_LineNow = new Line();
                                m_LineNow.X1 = mLine1List[i].X1;
                                m_LineNow.Y1 = mLine1List[i].Y1;
                                m_LineNow.X2 = mLine1List[i].X2;
                                m_LineNow.Y2 = mLine1List[i].Y2;
                                m_LineNow.Stroke = Brushes.Red;
                                m_LineNow.StrokeThickness = 1;
                                can.Children.Add(m_LineNow);

                                choList.Add(m_LineNow);

                                m_LineNow = new Line();
                                m_LineNow.X1 = mLine1List[i].X1;
                                m_LineNow.Y1 = mLine1List[i].Y1;
                                m_LineNow.X2 = mLine1List[i].X2;
                                m_LineNow.Y2 = mLine1List[i].Y2;
                                m_LineNow.Stroke = Brushes.Black;
                                m_LineNow.StrokeThickness = 1;
                                huifu1.Add(m_LineNow);
                                #endregion
                                if (choose1 != 0)
                                {
                                    can.Children.Remove(choList[choose1 - 1]);
                                    can.Children.Add(huifu1[choose1 - 1]);
                                }
                                choose1++;
                                #endregion
                                ganhao = i;
                            }
                            #endregion

                            #region     竖杆
                            if (((ptTemp.X - mLine1List[i].X1) <= 2) && ((ptTemp.X - mLine1List[i].X1) >= (-2))
                                && ((ptTemp.Y - mLine1List[i].Y1) * (ptTemp.Y - mLine1List[i].Y2) <= 0))
                            {
                                tension = 1;
                                #region     所选取杆件变色
                                #region     杆件擦除与新增
                                can.Children.Remove(mLine1List[i]);

                                m_LineNow = new Line();
                                m_LineNow.X1 = mLine1List[i].X1;
                                m_LineNow.Y1 = mLine1List[i].Y1;
                                m_LineNow.X2 = mLine1List[i].X2;
                                m_LineNow.Y2 = mLine1List[i].Y2;
                                m_LineNow.Stroke = Brushes.Red;
                                m_LineNow.StrokeThickness = 1;
                                can.Children.Add(m_LineNow);

                                choList.Add(m_LineNow);

                                m_LineNow = new Line();
                                m_LineNow.X1 = mLine1List[i].X1;
                                m_LineNow.Y1 = mLine1List[i].Y1;
                                m_LineNow.X2 = mLine1List[i].X2;
                                m_LineNow.Y2 = mLine1List[i].Y2;
                                m_LineNow.Stroke = Brushes.Black;
                                m_LineNow.StrokeThickness = 1;
                                huifu1.Add(m_LineNow);
                                #endregion
                                if (choose1 != 0)
                                {
                                    can.Children.Remove(choList[choose1 - 1]);
                                    can.Children.Add(huifu1[choose1 - 1]);
                                }
                                choose1++;
                                #endregion
                                ganhao = i;
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region    选取点
                    if (tension == 1)
                    {
                        for (int i = 0; i < lian1ListList[ganhao].pra_Num; i++)
                        {
                            //如果ptTemp在lian1ListList[ganhao].lian1List[i]的圆点范围内
                            if (((ptTemp.X - lian1ListList[ganhao].praList[i].prac_Ep.X) <= 1)
                                && ((ptTemp.X - lian1ListList[ganhao].praList[i].prac_Ep.X) >= (-1))
                                && ((ptTemp.Y - lian1ListList[ganhao].praList[i].prac_Ep.Y) <= 1)
                                && ((ptTemp.Y - lian1ListList[ganhao].praList[i].prac_Ep.Y) >= (-1)))
                            {
                                BendWindow bendwin = new BendWindow();
                                bendwin.ShowDialog();
                                wanju1 = Convert.ToDouble(bendwin.wanju.Text);
                                wanju1 = wanju1 * 400*bilichi*bilichi;
                                pt_lian1 = lian1ListList[ganhao].praList[i].prac_bend;
                                lian1ListList[ganhao].praList[i].wanju = wanju1;

                                double wan1 = wanju1 * 60 / ruler;
                                if (m_LineModelList[ganhao].Line_Style == 0)
                                {
                                    pt_lian1.Y += wan1;
                                }

                                if (m_LineModelList[ganhao].Line_Style == 1)
                                {
                                    pt_lian1.X += wan1;
                                }

                                elli1 = new Ellipse();
                                elli1.Height = 4;
                                elli1.Width = 4;
                                elli1.Fill = Brushes.Red;
                                elli1.SetValue(Canvas.LeftProperty, pt_lian1.X - 2);
                                elli1.SetValue(Canvas.TopProperty, pt_lian1.Y - 2);
                                can.Children.Add(elli1);

                                lian1ListList[ganhao].praList[i].prac_bend = pt_lian1;

                                if (wanju1 > 0)
                                {
                                    TextBlock f_TB = new TextBlock();
                                    var f_TBX = pt_lian1.X - 10;
                                    var f_TBY = pt_lian1.Y + 20;

                                    ScaleTransform f_TBscale = new ScaleTransform();
                                    f_TBscale.ScaleY = -1;
                                    f_TB.RenderTransform = f_TBscale;
                                    f_TB.Text = "" + Math.Round(wanju1 / 400 / bilichi / bilichi, 2);
                                    f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                    can.Children.Add(f_TB);
                                }
                                if (wanju1 == 0)
                                {
                                    TextBlock f_TB = new TextBlock();
                                    var f_TBX = pt_lian1.X - 10;
                                    var f_TBY = pt_lian1.Y - 5;

                                    ScaleTransform f_TBscale = new ScaleTransform();
                                    f_TBscale.ScaleY = -1;
                                    f_TB.RenderTransform = f_TBscale;
                                    double wanju111 = -wanju1;
                                    f_TB.Text = "" + Math.Round(wanju111 /400 / bilichi / bilichi, 2);
                                    f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                    can.Children.Add(f_TB);
                                }
                                if (wanju1 < 0)
                                {
                                    TextBlock f_TB = new TextBlock();
                                    var f_TBX = pt_lian1.X - 10;
                                    var f_TBY = pt_lian1.Y - 5;

                                    ScaleTransform f_TBscale = new ScaleTransform();
                                    f_TBscale.ScaleY = -1;
                                    f_TB.RenderTransform = f_TBscale;
                                    double wanju111 = -wanju1;
                                    f_TB.Text = "" + Math.Round(wanju111 / 400 / bilichi / bilichi, 2);
                                    f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                    can.Children.Add(f_TB);
                                }
                            }
                        }
                    }
                    #endregion

                    #region     二次绘制曲线
                    if (tension == 3)
                    {
                        for (int i = 0; i < qu_Num; i++)
                        {
                            if (((ptTemp.X - quxianList[i].bend_pt.X) <= 2)
                                && ((ptTemp.X - quxianList[i].bend_pt.X) >= (-2))
                                && ((ptTemp.Y - quxianList[i].bend_pt.Y) <= 2)
                                && ((ptTemp.Y - quxianList[i].bend_pt.Y) >= (-2)))
                            {
                                BendWindow bendwin = new BendWindow();
                                bendwin.ShowDialog();
                                wanju1 = Convert.ToDouble(bendwin.wanju.Text);
                                wanju1 = wanju1 * 400*bilichi*bilichi;
                                double wan1 = wanju1 * 60 / ruler;
                                quxianList[i].qu_M = wanju1;
                                quxianList[i].qu_wan = wan1;

                                m_LineNow = new Line();
                                m_LineNow.StrokeThickness = 1;
                                m_LineNow.Stroke = Brushes.Green;
                                m_LineNow.X1 = quxianList[i].bend_pt.X;
                                m_LineNow.Y1 = quxianList[i].bend_pt.Y;
                                //横杆
                                if (quxianList[i].qu_Style == 1)
                                {
                                    quxianList[i].bend_pt.Y += wan1;
                                    m_LineNow.Y1 += wan1 / 2;

                                }
                                //竖杆
                                if (quxianList[i].qu_Style == 2)
                                {
                                    quxianList[i].bend_pt.X += wan1;
                                    m_LineNow.X1 += wan1 / 2;
                                }

                                elli1 = new Ellipse();
                                elli1.Height = 4;
                                elli1.Width = 4;
                                elli1.Fill = Brushes.Red;
                                elli1.SetValue(Canvas.LeftProperty, quxianList[i].bend_pt.X - 2);
                                elli1.SetValue(Canvas.TopProperty, quxianList[i].bend_pt.Y - 2);
                                can.Children.Add(elli1);


                                m_LineNow.X2 = m_LineNow.X1 + 40;
                                m_LineNow.Y2 = m_LineNow.Y1 + 40;
                                can.Children.Add(m_LineNow);

                                if (wanju1 > 0)
                                {
                                    TextBlock f_TB = new TextBlock();
                                    var f_TBX = quxianList[i].bend_pt.X - 10 + 40;
                                    var f_TBY = quxianList[i].bend_pt.Y + 20 + 40;

                                    ScaleTransform f_TBscale = new ScaleTransform();
                                    f_TBscale.ScaleY = -1;
                                    f_TB.RenderTransform = f_TBscale;
                                    f_TB.Text = "" + Math.Round(wanju1 / 400 / bilichi / bilichi, 2);
                                    f_TB.Margin = new Thickness(f_TBX, f_TBY - 20, 0, 0);
                                    can.Children.Add(f_TB);
                                }
                                if (wanju1 < 0)
                                {
                                    TextBlock f_TB = new TextBlock();
                                    var f_TBX = quxianList[i].bend_pt.X - 10 + 40;
                                    var f_TBY = quxianList[i].bend_pt.Y + 20 + 40;

                                    ScaleTransform f_TBscale = new ScaleTransform();
                                    f_TBscale.ScaleY = -1;
                                    f_TB.RenderTransform = f_TBscale;
                                    double wanju1Temp = -wanju1;
                                    f_TB.Text = "" + Math.Round(wanju1Temp /400 / bilichi / bilichi, 2);
                                    f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                    can.Children.Add(f_TB);
                                }
                            }
                        }
                    }
                    #endregion
                }
                catch
                {
                    MessageBox.Show("练习图绘制错误");
                }

            }
            #endregion
            
        }

        #endregion

        #endregion

        #region     鼠标移动事件

        private void move(object sender, MouseEventArgs e)
        {
            if (paint == true)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point point = e.GetPosition(can);
                    if ((point.X<x_area /2)&&(point.Y>y_area /2)&&(point.Y<y_area ))
                    {
                        if (pointList.Count == 0)
                        {
                            pointList.Add(new Point(this.startPoint.X, this.startPoint.Y));
                        }
                        else
                        {
                            pointList.Add(point);
                        }
                        var disList = pointList.Distinct().ToList();
                        var count = disList.Count();

                        if (point != this.startPoint && this.startPoint != null)
                        {
                            m_LineNow.Stroke = Brushes.Black;
                            m_LineNow.StrokeThickness = 1;

                            if (count < 2)
                                return;

                            //直线画在网格上、只能画竖向和横向

                            if (Math.Abs(point.Y - startPoint.Y) > Math.Abs(point.X - startPoint.X))//竖线//
                            {
                                m_LineNow.X2 = m_LineNow.X1;
                                m_LineNow.Y2 = (Math.Round(point.Y / 20)) * 20;
                            }
                            else //横线//
                            {
                                m_LineNow.X2 = (Math.Round(point.X / 20)) * 20;
                                m_LineNow.Y2 = m_LineNow.Y1;
                            }
                        }
                    }
                }
            }

        }

        #endregion

        #region     鼠标抬起事件
        /// <summary>
        /// 鼠标左键抬起函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void can_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //获取线段终点
            Point endPoint = e.GetPosition(can);
            if ((endPoint.X < x_area /2) && (endPoint.Y > y_area/2))
            {
                endPoint.X = m_LineNow.X2;
                endPoint.Y = m_LineNow.Y2;
                m_LineModel.Line_EndPoint = endPoint;

                //圆点加粗
                Ellipse endEllipse = new Ellipse();
                endEllipse.Height = 4;
                endEllipse.Width = 4;
                endEllipse.Fill = Brushes.Black;
                endEllipse.SetValue(Canvas.LeftProperty, endPoint.X - 2);
                endEllipse.SetValue(Canvas.TopProperty, endPoint.Y - 2);
                can.Children.Add(endEllipse);


                //判断直线类型
                if (endPoint.X == startPoint.X)
                {
                    m_LineModel.Line_Style = 1;
                }
                else
                    m_LineModel.Line_Style = 0;

                //计算长度
                var length = Math.Sqrt(Math.Pow(m_LineNow.X1 - m_LineNow.X2, 2) + Math.Pow(m_LineNow.Y1 - m_LineNow.Y2, 2));
                m_LineModel.LineLength = length; 

                //  建立对应Line模型
                m_LineModel.LineInfo = m_LineNow;

                //命名
                m_LineModel.LineName = m_LineNow.Name;
                //线段数量加一


                //将当前线段保存到List
                if (m_LineModel.LineLength >= 20)
                {
                    m_Line_Num++;

                    #region   直线格式：横线左端为起点；纵线上端为起点
                    //横线
                    if (m_LineModel.Line_Style == 0)
                    {
                        if (m_LineModel.Line_BeginPoint.X > m_LineModel.Line_EndPoint.X)
                        {
                            tran = m_LineModel.Line_BeginPoint;
                            m_LineModel.Line_BeginPoint = m_LineModel.Line_EndPoint;
                            m_LineModel.Line_EndPoint = tran;
                        }
                    }
                    //纵线
                    if (m_LineModel.Line_Style == 1)
                    {
                        if (m_LineModel.Line_BeginPoint.Y < m_LineModel.Line_EndPoint.Y)
                        {
                            tran = m_LineModel.Line_BeginPoint;
                            m_LineModel.Line_BeginPoint = m_LineModel.Line_EndPoint;
                            m_LineModel.Line_EndPoint = tran;
                        }
                    }
                    #endregion

                    m_LineModelList.Add(m_LineModel);
                    #region     加标签
                    //横杆
                    if (m_LineModel.Line_Style==0)
                    {
                        TextBlock f_TBline = new TextBlock();
                        var f_TBX = (m_LineModel.Line_BeginPoint.X + m_LineModel.Line_EndPoint.X) / 2-10;
                        var f_TBY = m_LineModel.Line_BeginPoint.Y+20;
                        ScaleTransform f_TBscale = new ScaleTransform();
                        f_TBscale.ScaleY = -1;
                        f_TBline.RenderTransform = f_TBscale;
                        f_TBline.Foreground = Brushes.Blue;
                        f_TBline.Text = "(" + m_Line_Num+")";
                        f_TBline.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                        can.Children.Add(f_TBline);
                    }
                    //竖杆
                    if(m_LineModel.Line_Style==1)
                    {
                        TextBlock f_TBline = new TextBlock();
                        var f_TBX = m_LineModel.Line_BeginPoint.X + 5;
                        var f_TBY = (m_LineModel.Line_BeginPoint.Y +m_LineModel.Line_EndPoint.Y)/2+5;
                        ScaleTransform f_TBscale = new ScaleTransform();
                        f_TBscale.ScaleY = -1;
                        f_TBline.RenderTransform = f_TBscale;
                        f_TBline.Foreground = Brushes.Blue;
                        f_TBline.Text = "(" + m_Line_Num+")";
                        f_TBline.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                        can.Children.Add(f_TBline);
                    }
                    #endregion
                    m_LineModel = new LineModelClass();
                }
                return;
            }
        }
    
        //调用save 保存所有画好的线
        private void savedraw_click(object sender, RoutedEventArgs e)
        {
            if (order == 1)
            {
                paint = false;
                order = 2;

                this.menuTabctrl.SelectedItem = this.menuTabctrl.Items[1];
                TextBlock TB_ruler = new TextBlock();
                var f_TBrulerX = x_area / 2 - 90;
                var f_TBrulerY = y_area / 2 + 15;
                ScaleTransform f_TBrulerscale = new ScaleTransform();
                f_TBrulerscale.ScaleY = -1;
                TB_ruler.RenderTransform = f_TBrulerscale;
                TB_ruler.Foreground = Brushes.Green;
                TB_ruler.Background = Brushes.White;
                TB_ruler.FontSize = 20;
                TB_ruler.Text = "当前比例尺：" + blc.Text;
                TB_ruler.Margin = new Thickness(f_TBrulerX, f_TBrulerY, 0, 0);
                can.Children.Add(TB_ruler);
                string[] blcs = blc.Text.Split(':');
                bilichi = Convert.ToDouble(blcs[0]) / Convert.ToDouble(blcs[1]);
                MessageBox.Show("请添加支座！");
            }
        }
        #endregion
        #endregion

        #region     加支座、载荷（1--7支座，8--19载荷）

        #region     变量声明

        private int zz = 0;
        private int q_Num = 1, f_Num = 1, m_Num = 1;
        private int yueshu = 0;
        private bool ganjian = true;
        private bool tuodong = false;

        #region  创建支座、载荷、铰节点的list
        //已经添加的支座列表
        private ZhizuoModelClass m_Zhizuo = new ZhizuoModelClass();
        private List<ZhizuoModelClass> m_ZhizuoList = new List<ZhizuoModelClass>();

        /// <summary>
        /// model 当前添加的Load模型
        /// </summary>
        private LoadModel m_LoadModel = new LoadModel();

        /// <summary>
        /// 当前添加的所有载荷集合
        /// </summary>
        private List<LoadModel> m_LoadModelList = new List<LoadModel>();

        /// <summary>
        /// 当前载荷数 从0开始
        /// </summary>
        private int m_Load_Num = 0;

        /// <summary>
        /// Model 当前绘制的铰连接
        /// </summary>
        private JointClass m_HingeJoint = new JointClass();

        /// <summary>
        /// 当前添加的所有铰连接集合
        /// </summary>
        private List<JointClass> m_HingeJointList = new List<JointClass>();

        ///<summary>
        ///当前铰连接从0开始
        ///</summary>
        private int m_Hinge_Num = 0;
        #endregion

        #endregion

        #region 鼠标移过位置函数

        private void Rect_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle m_Rectangle = (Rectangle)sender;
            var m_Tag = m_Rectangle.Tag;
            zz = Convert.ToInt32(m_Tag);
            text_zz.Text = zz.ToString();

            #region  根据种类初始化drap函数
            switch (zz)
            {
                case 1:
                    {
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.gudzz_Rect);
                        DragDrop.DoDragDrop(this.gudzz_Rect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "固定铰支座";
                        break;
                    }
                case 2:
                    {
                        DataObject huodzz_data = new DataObject(typeof(Rectangle), this.huodzz_Rect);
                        DragDrop.DoDragDrop(this.huodzz_Rect, huodzz_data, DragDropEffects.Copy);
                        text_zz.Text = "活动纵铰支座";
                        break;
                    }
                case 3:
                    {
                        DataObject zuogdd_data = new DataObject(typeof(Rectangle), this.zuogdd_Rect);
                        DragDrop.DoDragDrop(this.zuogdd_Rect, zuogdd_data, DragDropEffects.Copy);
                        text_zz.Text = "左固定端";
                        break;
                    }

                case 4:
                    {
                        DataObject zuogdd_data = new DataObject(typeof(Rectangle), this.yougdd_Rect);
                        DragDrop.DoDragDrop(this.yougdd_Rect, zuogdd_data, DragDropEffects.Copy);
                        text_zz.Text = "右固定端";
                        break;
                    }
                case 5:
                    {
                        DataObject huadzzX_data = new DataObject(typeof(Rectangle), this.huadzzX_Rect);
                        DragDrop.DoDragDrop(this.huadzzX_Rect, huadzzX_data, DragDropEffects.Copy);
                        text_zz.Text = "滑动支座X";
                        break;
                    }
                case 6:
                    {
                        DataObject huadzzY_data = new DataObject(typeof(Rectangle), this.huadzzY_Rect);
                        DragDrop.DoDragDrop(this.huadzzY_Rect, huadzzY_data, DragDropEffects.Copy);
                        text_zz.Text = "滑动支座Y";
                        break;
                    }
                case 7:
                    {
                        DataObject jiaolian_data = new DataObject(typeof(Rectangle), this.jiaolian_Rect);
                        DragDrop.DoDragDrop(this.jiaolian_Rect, jiaolian_data, DragDropEffects.Copy);
                        text_zz.Text = "铰连接";
                        break;
                    }
                case 8:
                    {
                        DataObject huodongx_data = new DataObject(typeof(Rectangle), this.huodongx_Rect);
                        DragDrop.DoDragDrop(this.huodongx_Rect, huodongx_data, DragDropEffects.Copy);
                        text_zz.Text = "活动横铰支座";
                        break;
                    }
                case 9:
                    {
                        DataObject shangjzl_data = new DataObject(typeof(Rectangle), this.shangjzl_Rect);
                        DragDrop.DoDragDrop(this.shangjzl_Rect, shangjzl_data, DragDropEffects.Copy);
                        text_zz.Text = "向上集中力";
                        break;
                    }
                case 10:
                    {
                        DataObject xiajzl_data = new DataObject(typeof(Rectangle), this.xiajzl_Rect);
                        DragDrop.DoDragDrop(this.xiajzl_Rect, xiajzl_data, DragDropEffects.Copy);
                        text_zz.Text = "向下集中力";
                        break;
                    }
                case 11:
                    {
                        DataObject youjzl_data = new DataObject(typeof(Rectangle), this.youjzl_Rect);
                        DragDrop.DoDragDrop(this.youjzl_Rect, youjzl_data, DragDropEffects.Copy);
                        text_zz.Text = "向右集中力";
                        break;
                    }
                case 12:
                    {
                        DataObject zuojzl_data = new DataObject(typeof(Rectangle), this.zuojzl_Rect);
                        DragDrop.DoDragDrop(this.zuojzl_Rect, zuojzl_data, DragDropEffects.Copy);
                        text_zz.Text = "向左集中力";
                        break;
                    }

                case 13:
                    {
                        DataObject youjbzh_data = new DataObject(typeof(Rectangle), this.youjbzh_Rect);
                        DragDrop.DoDragDrop(this.youjbzh_Rect, youjbzh_data, DragDropEffects.Copy);
                        text_zz.Text = "向右均布载荷";
                        break;
                    }
                case 14:
                    {
                        DataObject zuojbzh_data = new DataObject(typeof(Rectangle), this.zuojbzh_Rect);
                        DragDrop.DoDragDrop(this.zuojbzh_Rect, zuojbzh_data, DragDropEffects.Copy);
                        text_zz.Text = "向左均布载荷";
                        break;
                    }
                case 15:
                    {
                        DataObject xiajbzh_data = new DataObject(typeof(Rectangle), this.xiajbzh_Rect);
                        DragDrop.DoDragDrop(this.xiajbzh_Rect, xiajbzh_data, DragDropEffects.Copy);
                        text_zz.Text = "向下均布载荷";
                        break;
                    }
                case 16:
                    {
                        DataObject shangjbzh_data = new DataObject(typeof(Rectangle), this.shangjbzh_Rect);
                        DragDrop.DoDragDrop(this.shangjbzh_Rect, shangjbzh_data, DragDropEffects.Copy);
                        text_zz.Text = "向上均布载荷";
                        break;
                    }
                case 17:
                    {
                        DataObject shunljX_data = new DataObject(typeof(Rectangle), this.shunljX_Rect);
                        DragDrop.DoDragDrop(this.shunljX_Rect, shunljX_data, DragDropEffects.Copy);
                        text_zz.Text = "横轴顺时针力矩";
                        break;
                    }
                case 18:
                    {
                        DataObject niljX_data = new DataObject(typeof(Rectangle), this.niljX_Rect);
                        DragDrop.DoDragDrop(this.niljX_Rect, niljX_data, DragDropEffects.Copy);
                        text_zz.Text = "横轴逆时针力矩";
                        break;
                    }
                case 19:
                    {
                        DataObject shunljY_data = new DataObject(typeof(Rectangle), this.shunljY_Rect);
                        DragDrop.DoDragDrop(this.shunljY_Rect, shunljY_data, DragDropEffects.Copy);
                        text_zz.Text = "纵轴顺时针力矩";
                        break;
                    }
                case 20:
                    {
                        DataObject niljY_data = new DataObject(typeof(Rectangle), this.niljY_Rect);
                        DragDrop.DoDragDrop(this.niljY_Rect, niljY_data, DragDropEffects.Copy);
                        text_zz.Text = "纵轴逆时针力矩";
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
#endregion

        }

        #endregion

        #region     拖拽事件
        private void can_DragOver(object sender, DragEventArgs e)
        {
            //固定支座
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {

                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //活动支座
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //左固定端
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }

            //右固定端
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //滑动支座X
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //滑动支座Y
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //铰连接
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //活动横支座
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }

            //上集中力
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //下集中力
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //右集中力
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //左集中力
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //右均布载荷
            if (!e.Data.GetDataPresent(typeof(Button)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //左均布载荷
            if (!e.Data.GetDataPresent(typeof(Button)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //下均布载荷
            if (!e.Data.GetDataPresent(typeof(Button)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //上均布载荷
            if (!e.Data.GetDataPresent(typeof(Button)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //顺时针横轴力矩
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //逆时针横轴力矩
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //顺时针纵轴力矩
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //逆时针纵轴力矩
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }


        }
        #endregion

        #region     图片信息整合
        private List<Rectangle> rec1List=new List<Rectangle>();
        private List<Rectangle> rec2List = new List<Rectangle>();
        private List<Rectangle> rec3List = new List<Rectangle>();
        private int rec;
        #endregion

        #region     重点信息 
        //drop固定支座

        Point rePlacePt = new Point();
        private void can_Drop(object sender, DragEventArgs e)
        {
            switch (zz)
            {
                #region 支座信息

                #region     固定支座
                case 1:
                    {
                        if (order == 2)
                        {
                            #region     图片信息
                            Rectangle gudzz_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_gudzz = new Rectangle();
                            #region     附图
                            Rectangle new_gudzz1 = new Rectangle();
                            Rectangle new_gudzz2 = new Rectangle();
                            Rectangle new_gudzz3 = new Rectangle();
                            #endregion
                            new_gudzz.Height = gudzz_dataobj.RenderSize.Height;
                            new_gudzz.Width = gudzz_dataobj.RenderSize.Width;
                            new_gudzz.Fill = gudzz_dataobj.Fill;
                            new_gudzz.Stroke = gudzz_dataobj.Stroke;
                            new_gudzz.StrokeThickness = gudzz_dataobj.StrokeThickness;
                            #region     附图
                            new_gudzz1.Height = gudzz_dataobj.RenderSize.Height;
                            new_gudzz1.Width = gudzz_dataobj.RenderSize.Width;
                            new_gudzz1.Fill = gudzz_dataobj.Fill;
                            new_gudzz1.Stroke = gudzz_dataobj.Stroke;
                            new_gudzz1.StrokeThickness = gudzz_dataobj.StrokeThickness;

                            new_gudzz2.Height = gudzz_dataobj.RenderSize.Height;
                            new_gudzz2.Width = gudzz_dataobj.RenderSize.Width;
                            new_gudzz2.Fill = gudzz_dataobj.Fill;
                            new_gudzz2.Stroke = gudzz_dataobj.Stroke;
                            new_gudzz2.StrokeThickness = gudzz_dataobj.StrokeThickness;

                            new_gudzz3.Height = gudzz_dataobj.RenderSize.Height;
                            new_gudzz3.Width = gudzz_dataobj.RenderSize.Width;
                            new_gudzz3.Fill = gudzz_dataobj.Fill;
                            new_gudzz3.Stroke = gudzz_dataobj.Stroke;
                            new_gudzz3.StrokeThickness = gudzz_dataobj.StrokeThickness;
                            #endregion
                            new_gudzz.Tag = gudzz_dataobj.Tag;
                            #endregion

                            #region     信息存储
                            m_Zhizuo.Zhizuo_pt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                            m_Zhizuo.Zhizuo_pt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                            m_Zhizuo.ganjian = 1;
                            
                            #region     记录所在杆件号

                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                    && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[i].Line_EndPoint.X)
                                    && (m_LineModelList[i].Line_Style == 0)) 
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_Zhizuo.Zhizuo_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion

                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                    && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_Zhizuo.Zhizuo_pt.Y >= m_LineModelList[i].Line_EndPoint.Y)
                                    && (m_LineModelList[i].Line_Style == 1)) 
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[j].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                            && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[j].Line_BeginPoint.X)
                                            && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[j].Line_EndPoint.X))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_LineModelList[i].Line_BeginPoint.Y - m_Zhizuo.Zhizuo_pt.Y;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion
                            }
                            #endregion

                            #endregion

                            #region     完成
                            if (tuodong == true)
                            {
                                new_gudzz.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 20);
                                new_gudzz.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y);
                                #region     附图
                                new_gudzz1.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 2 - 20);
                                new_gudzz1.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y);

                                new_gudzz2.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 20);
                                new_gudzz2.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area / 2);

                                new_gudzz3.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 4 - 20);
                                new_gudzz3.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area / 2);
                                #endregion
                                m_Zhizuo.Zhizuo_Style = 1;

                                #region     后续(竖向颠倒）
                                ScaleTransform gudzz_scale = new ScaleTransform();
                                gudzz_scale.ScaleY = -1;
                                new_gudzz.RenderTransform = gudzz_scale;
                                #region     附图
                                new_gudzz1.RenderTransform = gudzz_scale;
                                new_gudzz2.RenderTransform = gudzz_scale;
                                new_gudzz3.RenderTransform = gudzz_scale;
                                #endregion
                                #endregion

                                can.Children.Add(new_gudzz);
                                #region     附图
                                rec++;
                                rec1List.Add(new_gudzz1);
                                rec2List.Add(new_gudzz2);
                                rec3List.Add(new_gudzz3);
                                #endregion

                                m_ZhizuoList.Add(m_Zhizuo);
                                #region     标签
                                m_Zhizuo_Num++;
                                TextBlock f_TB = new TextBlock();
                                var f_TBX = m_Zhizuo.Zhizuo_pt.X;
                                var f_TBY = m_Zhizuo.Zhizuo_pt.Y - 34;

                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Foreground = Brushes.Red;
                                f_TB.Text = "" + yuebiao;

                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                can.Children.Add(f_TB);
                                #endregion
                                m_Zhizuo = new ZhizuoModelClass();
                                zz = 0;
                                //自由度计算约束叠加数 
                                yueshu = yueshu + 2;
                                yuebiao++;
                            }
                            #endregion
                        }
                        if (order != 2)
                        {
                            MessageBox.Show("请先完成杆件绘制，并点击“杆件”菜单页上的“完成绘制”按钮！");
                        }
                        tuodong = false;
                        break;
                    }
                #endregion

                #region         活动纵支座
                case 2:
                    {
                        if (order == 2)
                        {
                            #region     图片信息
                            Rectangle huodzz_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_huodzz = new Rectangle();
                            new_huodzz.Height = huodzz_dataobj.RenderSize.Height;
                            new_huodzz.Width = huodzz_dataobj.RenderSize.Width;
                            new_huodzz.Fill = huodzz_dataobj.Fill;
                            new_huodzz.Stroke = huodzz_dataobj.Stroke;
                            new_huodzz.StrokeThickness = huodzz_dataobj.StrokeThickness;
                            #region     附图
                            Rectangle new_huodzz1 = new Rectangle();
                            new_huodzz1.Height = huodzz_dataobj.RenderSize.Height;
                            new_huodzz1.Width = huodzz_dataobj.RenderSize.Width;
                            new_huodzz1.Fill = huodzz_dataobj.Fill;
                            new_huodzz1.Stroke = huodzz_dataobj.Stroke;
                            new_huodzz1.StrokeThickness = huodzz_dataobj.StrokeThickness;

                            Rectangle new_huodzz2 = new Rectangle();
                            new_huodzz2.Height = huodzz_dataobj.RenderSize.Height;
                            new_huodzz2.Width = huodzz_dataobj.RenderSize.Width;
                            new_huodzz2.Fill = huodzz_dataobj.Fill;
                            new_huodzz2.Stroke = huodzz_dataobj.Stroke;
                            new_huodzz2.StrokeThickness = huodzz_dataobj.StrokeThickness;

                            Rectangle new_huodzz3 = new Rectangle();
                            new_huodzz3.Height = huodzz_dataobj.RenderSize.Height;
                            new_huodzz3.Width = huodzz_dataobj.RenderSize.Width;
                            new_huodzz3.Fill = huodzz_dataobj.Fill;
                            new_huodzz3.Stroke = huodzz_dataobj.Stroke;
                            new_huodzz3.StrokeThickness = huodzz_dataobj.StrokeThickness;
                            #endregion

                            #endregion

                            #region     信息存储
                            m_Zhizuo.ganjian = 1;
                            m_Zhizuo.Zhizuo_pt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                            m_Zhizuo.Zhizuo_pt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                            new_huodzz.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 20);
                            new_huodzz.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y);

                            #region     附图
                            new_huodzz1.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 2 - 20);
                            new_huodzz1.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y);

                            new_huodzz2.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 20);
                            new_huodzz2.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area / 2);

                            new_huodzz3.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 4 - 20);
                            new_huodzz3.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area / 2);
                            #endregion

                            #region     记录所在杆件号

                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                    && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[i].Line_EndPoint.X)
                                    && (m_LineModelList[i].Line_Style == 0))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_Zhizuo.Zhizuo_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion

                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                    && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_Zhizuo.Zhizuo_pt.Y >= m_LineModelList[i].Line_EndPoint.Y)
                                    && (m_LineModelList[i].Line_Style == 1))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[j].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                            && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[j].Line_BeginPoint.X)
                                            && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[j].Line_EndPoint.X))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_LineModelList[i].Line_BeginPoint.Y - m_Zhizuo.Zhizuo_pt.Y;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion
                            }
                            #endregion

                            ScaleTransform huodzz_scale = new ScaleTransform();
                            huodzz_scale.ScaleY = -1;
                            new_huodzz.RenderTransform = huodzz_scale;
                            #region     附图
                            new_huodzz1.RenderTransform = huodzz_scale;
                            new_huodzz2.RenderTransform = huodzz_scale;
                            new_huodzz3.RenderTransform = huodzz_scale;
                            #endregion
                            m_Zhizuo.Zhizuo_Style = 2;
                            #endregion

                            #region     完成
                            if (tuodong == true)
                            {
                                can.Children.Add(new_huodzz);
                                #region     附图
                                rec++;
                                rec1List.Add(new_huodzz1);
                                rec2List.Add(new_huodzz2);
                                rec3List.Add(new_huodzz3);
                                #endregion
                                m_ZhizuoList.Add(m_Zhizuo);

                                #region     标签
                                m_Zhizuo_Num++;
                                TextBlock f_TB = new TextBlock();
                                var f_TBX = m_Zhizuo.Zhizuo_pt.X;
                                var f_TBY = m_Zhizuo.Zhizuo_pt.Y - 40 + 5;

                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Foreground = Brushes.Red;
                                f_TB.Text = "" + yuebiao;
                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                can.Children.Add(f_TB);
                                #endregion
                                m_Zhizuo = new ZhizuoModelClass();
                                zz = 0;

                                //自由度计算约束叠加数 
                                yueshu = yueshu + 1;
                                yuebiao++;
                            }
                            #endregion
                        }
                        if (order != 2)
                        {
                            MessageBox.Show("请先完成杆件绘制，并点击“杆件”菜单页上的“完成绘制”按钮！");
                        }
                        tuodong = false;
                        break;
                    }
                #endregion

                #region     活动横支座
                case 8:
                    {
                        if (order == 2)
                        {
                            #region     图片信息
                            Rectangle huodongx_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_huodongx = new Rectangle();
                            new_huodongx.Height = huodongx_dataobj.RenderSize.Height;
                            new_huodongx.Width = huodongx_dataobj.RenderSize.Width;
                            new_huodongx.Fill = huodongx_dataobj.Fill;
                            new_huodongx.Stroke = huodongx_dataobj.Stroke;
                            new_huodongx.StrokeThickness = huodongx_dataobj.StrokeThickness;
                            #region     附图
                            Rectangle new_huodongx1 = new Rectangle();
                            new_huodongx1.Height = huodongx_dataobj.RenderSize.Height;
                            new_huodongx1.Width = huodongx_dataobj.RenderSize.Width;
                            new_huodongx1.Fill = huodongx_dataobj.Fill;
                            new_huodongx1.Stroke = huodongx_dataobj.Stroke;
                            new_huodongx1.StrokeThickness = huodongx_dataobj.StrokeThickness;

                            Rectangle new_huodongx2 = new Rectangle();
                            new_huodongx2.Height = huodongx_dataobj.RenderSize.Height;
                            new_huodongx2.Width = huodongx_dataobj.RenderSize.Width;
                            new_huodongx2.Fill = huodongx_dataobj.Fill;
                            new_huodongx2.Stroke = huodongx_dataobj.Stroke;
                            new_huodongx2.StrokeThickness = huodongx_dataobj.StrokeThickness;

                            Rectangle new_huodongx3 = new Rectangle();
                            new_huodongx3.Height = huodongx_dataobj.RenderSize.Height;
                            new_huodongx3.Width = huodongx_dataobj.RenderSize.Width;
                            new_huodongx3.Fill = huodongx_dataobj.Fill;
                            new_huodongx3.Stroke = huodongx_dataobj.Stroke;
                            new_huodongx3.StrokeThickness = huodongx_dataobj.StrokeThickness;
                            #endregion
                            #endregion

                            #region     信息存储
                            m_Zhizuo.ganjian = 1;
                            m_Zhizuo.Zhizuo_pt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                            m_Zhizuo.Zhizuo_pt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                            new_huodongx.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 40);
                            new_huodongx.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20);
                            #region     附图
                            new_huodongx1.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 2 - 40);
                            new_huodongx1.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20);

                            new_huodongx2.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 40);
                            new_huodongx2.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area / 2 + 20);

                            new_huodongx3.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 4 - 40);
                            new_huodongx3.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area / 2 + 20);
                            #endregion

                            #region     记录所在杆件号

                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                    && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[i].Line_EndPoint.X)
                                    && (m_LineModelList[i].Line_Style == 0))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_Zhizuo.Zhizuo_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion

                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                    && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_Zhizuo.Zhizuo_pt.Y >= m_LineModelList[i].Line_EndPoint.Y)
                                    && (m_LineModelList[i].Line_Style == 1))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[j].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                            && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[j].Line_BeginPoint.X)
                                            && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[j].Line_EndPoint.X))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_LineModelList[i].Line_BeginPoint.Y - m_Zhizuo.Zhizuo_pt.Y;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion
                            }
                            #endregion

                            ScaleTransform huodongx_scale = new ScaleTransform();
                            huodongx_scale.ScaleY = -1;
                            new_huodongx.RenderTransform = huodongx_scale;
                            #region     附图
                            new_huodongx1.RenderTransform = huodongx_scale;
                            new_huodongx2.RenderTransform = huodongx_scale;
                            new_huodongx3.RenderTransform = huodongx_scale;
                            #endregion
                            m_Zhizuo.Zhizuo_Style = 3;
                            #endregion

                            #region     完成
                            if (tuodong == true)
                            {
                                can.Children.Add(new_huodongx);
                                #region     附图
                                rec++;
                                rec1List.Add(new_huodongx1);
                                rec2List.Add(new_huodongx2);
                                rec3List.Add(new_huodongx3);
                                #endregion
                                m_ZhizuoList.Add(m_Zhizuo);
                                #region     标签
                                TextBlock f_TB = new TextBlock();
                                var f_TBX = m_Zhizuo.Zhizuo_pt.X - 10;
                                var f_TBY = m_Zhizuo.Zhizuo_pt.Y;

                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.Foreground = Brushes.Red;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Text = "" + yuebiao;
                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                can.Children.Add(f_TB);
                                #endregion
                                m_Zhizuo = new ZhizuoModelClass();
                                zz = 0;
                                m_Zhizuo_Num++;
                                yueshu = yueshu + 1;
                                yuebiao++;
                            }
                            #endregion
                        }
                        if (order != 2)
                        {
                            MessageBox.Show("请先完成杆件绘制，并点击“杆件”菜单页上的“完成绘制”按钮!");
                        }
                        tuodong = false;
                        //自由度计算约束叠加数 
                        break;
                    }

                #endregion

                #region     固定端

                #region 左
                case 3:
                    {
                        if (order == 2)
                        {
                            #region     图片信息
                            Rectangle zuogdd_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_zuogdd = new Rectangle();
                            new_zuogdd.Height = zuogdd_dataobj.RenderSize.Height;
                            new_zuogdd.Width = zuogdd_dataobj.RenderSize.Width;
                            new_zuogdd.Fill = zuogdd_dataobj.Fill;
                            new_zuogdd.Stroke = zuogdd_dataobj.Stroke;
                            new_zuogdd.StrokeThickness = zuogdd_dataobj.StrokeThickness;

                            #region     附图
                            Rectangle new_zuogdd1 = new Rectangle();
                            new_zuogdd1.Height = zuogdd_dataobj.RenderSize.Height;
                            new_zuogdd1.Width = zuogdd_dataobj.RenderSize.Width;
                            new_zuogdd1.Fill = zuogdd_dataobj.Fill;
                            new_zuogdd1.Stroke = zuogdd_dataobj.Stroke;
                            new_zuogdd1.StrokeThickness = zuogdd_dataobj.StrokeThickness;

                            Rectangle new_zuogdd2 = new Rectangle();
                            new_zuogdd2.Height = zuogdd_dataobj.RenderSize.Height;
                            new_zuogdd2.Width = zuogdd_dataobj.RenderSize.Width;
                            new_zuogdd2.Fill = zuogdd_dataobj.Fill;
                            new_zuogdd2.Stroke = zuogdd_dataobj.Stroke;
                            new_zuogdd2.StrokeThickness = zuogdd_dataobj.StrokeThickness;

                            Rectangle new_zuogdd3 = new Rectangle();
                            new_zuogdd3.Height = zuogdd_dataobj.RenderSize.Height;
                            new_zuogdd3.Width = zuogdd_dataobj.RenderSize.Width;
                            new_zuogdd3.Fill = zuogdd_dataobj.Fill;
                            new_zuogdd3.Stroke = zuogdd_dataobj.Stroke;
                            new_zuogdd3.StrokeThickness = zuogdd_dataobj.StrokeThickness;
                            #endregion
                            #endregion

                            #region     信息存储
                            m_Zhizuo.ganjian = 1;
                            m_Zhizuo.Zhizuo_pt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                            m_Zhizuo.Zhizuo_pt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                            new_zuogdd.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 40);
                            new_zuogdd.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20);
                            #region     附图
                            new_zuogdd1.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 2 - 40);
                            new_zuogdd1.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20);

                            new_zuogdd2.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 40);
                            new_zuogdd2.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area / 2 + 20);

                            new_zuogdd3.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 4 - 40);
                            new_zuogdd3.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area / 2 + 20);
                            #endregion
                            m_Zhizuo.Zhizuo_Style = 4;

                            #region     记录所在杆件号

                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                    && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[i].Line_EndPoint.X)
                                    && (m_LineModelList[i].Line_Style == 0))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_Zhizuo.Zhizuo_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion

                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                    && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_Zhizuo.Zhizuo_pt.Y >= m_LineModelList[i].Line_EndPoint.Y)
                                    && (m_LineModelList[i].Line_Style == 1))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[j].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                            && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[j].Line_BeginPoint.X)
                                            && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[j].Line_EndPoint.X))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_LineModelList[i].Line_BeginPoint.Y - m_Zhizuo.Zhizuo_pt.Y;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion
                            }
                            #endregion

                            ScaleTransform zuogdd_scale = new ScaleTransform();
                            zuogdd_scale.ScaleY = -1;
                            new_zuogdd.RenderTransform = zuogdd_scale;
                            #region     附图
                            new_zuogdd1.RenderTransform = zuogdd_scale;
                            new_zuogdd2.RenderTransform = zuogdd_scale;
                            new_zuogdd3.RenderTransform = zuogdd_scale;
                            #endregion
                            #endregion

                            #region     完成
                            if (tuodong == true)
                            {
                                can.Children.Add(new_zuogdd);
                                #region     附图
                                rec++;
                                rec1List.Add(new_zuogdd1);
                                rec2List.Add(new_zuogdd2);
                                rec3List.Add(new_zuogdd3);
                                #endregion
                                m_ZhizuoList.Add(m_Zhizuo);
                                #region     标签
                                TextBlock f_TB = new TextBlock();
                                var f_TBX = m_Zhizuo.Zhizuo_pt.X - 12;
                                var f_TBY = m_Zhizuo.Zhizuo_pt.Y + 10;

                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Foreground = Brushes.Red;
                                f_TB.Text = "" + yuebiao;
                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                can.Children.Add(f_TB);
                                #endregion
                                m_Zhizuo = new ZhizuoModelClass();
                                zz = 0;
                                yueshu = yueshu + 3;
                                yuebiao++;
                                m_Zhizuo_Num++;
                            }
                            #endregion
                        }
                        if (order != 2)
                        {
                            MessageBox.Show("请先完成杆件绘制，并点击“杆件”菜单页上的“完成绘制”按钮！");
                        }
                        tuodong = false;
                        //自由度计算约束叠加数 

                        break;
                    }
                #endregion

                #region     右
                case 4:
                    {
                        if (order == 2)
                        {
                            #region     图片信息
                            Rectangle yougdd_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_yougdd = new Rectangle();
                            new_yougdd.Height = yougdd_dataobj.RenderSize.Height;
                            new_yougdd.Width = yougdd_dataobj.RenderSize.Width;
                            new_yougdd.Fill = yougdd_dataobj.Fill;
                            new_yougdd.Stroke = yougdd_dataobj.Stroke;
                            new_yougdd.StrokeThickness = yougdd_dataobj.StrokeThickness;

                            #region     附图
                            Rectangle new_yougdd1 = new Rectangle();
                            new_yougdd1.Height = yougdd_dataobj.RenderSize.Height;
                            new_yougdd1.Width = yougdd_dataobj.RenderSize.Width;
                            new_yougdd1.Fill = yougdd_dataobj.Fill;
                            new_yougdd1.Stroke = yougdd_dataobj.Stroke;
                            new_yougdd1.StrokeThickness = yougdd_dataobj.StrokeThickness;

                            Rectangle new_yougdd2 = new Rectangle();
                            new_yougdd2.Height = yougdd_dataobj.RenderSize.Height;
                            new_yougdd2.Width = yougdd_dataobj.RenderSize.Width;
                            new_yougdd2.Fill = yougdd_dataobj.Fill;
                            new_yougdd2.Stroke = yougdd_dataobj.Stroke;
                            new_yougdd2.StrokeThickness = yougdd_dataobj.StrokeThickness;

                            Rectangle new_yougdd3 = new Rectangle();
                            new_yougdd3.Height = yougdd_dataobj.RenderSize.Height;
                            new_yougdd3.Width = yougdd_dataobj.RenderSize.Width;
                            new_yougdd3.Fill = yougdd_dataobj.Fill;
                            new_yougdd3.Stroke = yougdd_dataobj.Stroke;
                            new_yougdd3.StrokeThickness = yougdd_dataobj.StrokeThickness;
                            #endregion
                            #endregion

                            #region 信息存储
                            m_Zhizuo.ganjian = 1;
                            m_Zhizuo.Zhizuo_pt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                            m_Zhizuo.Zhizuo_pt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                            new_yougdd.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X);
                            new_yougdd.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20);
                            #region     附图
                            new_yougdd1.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 2);
                            new_yougdd1.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20);

                            new_yougdd2.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X);
                            new_yougdd2.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20 - y_area / 2);

                            new_yougdd3.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 4);
                            new_yougdd3.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20 - y_area / 2);
                            #endregion

                            m_Zhizuo.Zhizuo_Style = 4;

                            #region     记录所在杆件号

                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                    && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[i].Line_EndPoint.X)
                                    && (m_LineModelList[i].Line_Style == 0))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_Zhizuo.Zhizuo_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion

                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                    && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_Zhizuo.Zhizuo_pt.Y >= m_LineModelList[i].Line_EndPoint.Y)
                                    && (m_LineModelList[i].Line_Style == 1))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[j].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                            && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[j].Line_BeginPoint.X)
                                            && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[j].Line_EndPoint.X))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_LineModelList[i].Line_BeginPoint.Y - m_Zhizuo.Zhizuo_pt.Y;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion
                            }
                            #endregion

                            if (tuodong == true)
                            {
                                ScaleTransform yougdd_scale = new ScaleTransform();
                                yougdd_scale.ScaleY = -1;
                                new_yougdd.RenderTransform = yougdd_scale;
                                #region     附图
                                new_yougdd1.RenderTransform = yougdd_scale;
                                new_yougdd2.RenderTransform = yougdd_scale;
                                new_yougdd3.RenderTransform = yougdd_scale;
                                #endregion

                                #region     完成
                                can.Children.Add(new_yougdd);
                                #region     附图
                                rec++;
                                rec1List.Add(new_yougdd1);
                                rec2List.Add(new_yougdd2);
                                rec3List.Add(new_yougdd3);
                                #endregion
                                m_ZhizuoList.Add(m_Zhizuo);
                                #region     标签
                                TextBlock f_TB = new TextBlock();
                                var f_TBX = m_Zhizuo.Zhizuo_pt.X - 14 + 20;
                                var f_TBY = m_Zhizuo.Zhizuo_pt.Y + 10;

                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Foreground = Brushes.Red;
                                f_TB.Text = "" + yuebiao;
                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                can.Children.Add(f_TB);
                                #endregion
                                m_Zhizuo = new ZhizuoModelClass();
                                zz = 0;


                                //自由度计算约束叠加数 
                                yueshu = yueshu + 3;
                                m_Zhizuo_Num++;

                                #endregion
                                yuebiao++;
                            }
                            #endregion
                        }
                        tuodong = false;
                        if (order != 2)
                        {
                            MessageBox.Show("请先完成杆件绘制，并点击“杆件”菜单页上的“完成绘制”按钮！");
                        }
                        break;
                    }
                #endregion

                #endregion

                #region     滑动横支座
                case 5:
                    {
                        if (order == 2)
                        {
                            #region     图片信息
                            Rectangle huadzzX_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_huadzzX = new Rectangle();
                            new_huadzzX.Height = huadzzX_dataobj.RenderSize.Height;
                            new_huadzzX.Width = huadzzX_dataobj.RenderSize.Width;
                            new_huadzzX.Fill = huadzzX_dataobj.Fill;
                            new_huadzzX.Stroke = huadzzX_dataobj.Stroke;
                            new_huadzzX.StrokeThickness = huadzzX_dataobj.StrokeThickness;
                            #region     附图
                            Rectangle new_huadzzX1 = new Rectangle();
                            new_huadzzX1.Height = huadzzX_dataobj.RenderSize.Height;
                            new_huadzzX1.Width = huadzzX_dataobj.RenderSize.Width;
                            new_huadzzX1.Fill = huadzzX_dataobj.Fill;
                            new_huadzzX1.Stroke = huadzzX_dataobj.Stroke;
                            new_huadzzX1.StrokeThickness = huadzzX_dataobj.StrokeThickness;

                            Rectangle new_huadzzX2 = new Rectangle();
                            new_huadzzX2.Height = huadzzX_dataobj.RenderSize.Height;
                            new_huadzzX2.Width = huadzzX_dataobj.RenderSize.Width;
                            new_huadzzX2.Fill = huadzzX_dataobj.Fill;
                            new_huadzzX2.Stroke = huadzzX_dataobj.Stroke;
                            new_huadzzX2.StrokeThickness = huadzzX_dataobj.StrokeThickness;

                            Rectangle new_huadzzX3 = new Rectangle();
                            new_huadzzX3.Height = huadzzX_dataobj.RenderSize.Height;
                            new_huadzzX3.Width = huadzzX_dataobj.RenderSize.Width;
                            new_huadzzX3.Fill = huadzzX_dataobj.Fill;
                            new_huadzzX3.Stroke = huadzzX_dataobj.Stroke;
                            new_huadzzX3.StrokeThickness = huadzzX_dataobj.StrokeThickness;
                            #endregion
                            #endregion

                            #region     信息存储
                            m_Zhizuo.ganjian = 1;
                            m_Zhizuo.Zhizuo_pt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                            m_Zhizuo.Zhizuo_pt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                            new_huadzzX.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 40);
                            new_huadzzX.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20);
                            #region     附图
                            new_huadzzX1.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 2 - 40);
                            new_huadzzX1.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20);

                            new_huadzzX2.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 40);
                            new_huadzzX2.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area / 2 + 20);

                            new_huadzzX3.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 4 - 40);
                            new_huadzzX3.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20 - y_area / 2);
                            #endregion
                            m_Zhizuo.Zhizuo_Style = 5;

                            #region     记录所在杆件号

                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                    && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[i].Line_EndPoint.X)
                                    && (m_LineModelList[i].Line_Style == 0))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_Zhizuo.Zhizuo_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion

                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                    && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_Zhizuo.Zhizuo_pt.Y >= m_LineModelList[i].Line_EndPoint.Y)
                                    && (m_LineModelList[i].Line_Style == 1))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[j].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                            && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[j].Line_BeginPoint.X)
                                            && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[j].Line_EndPoint.X))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_LineModelList[i].Line_BeginPoint.Y - m_Zhizuo.Zhizuo_pt.Y;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion
                            }
                            #endregion

                            ScaleTransform huadzzX_scale = new ScaleTransform();
                            huadzzX_scale.ScaleY = -1;
                            new_huadzzX.RenderTransform = huadzzX_scale;
                            #region     附图
                            new_huadzzX1.RenderTransform = huadzzX_scale;
                            new_huadzzX2.RenderTransform = huadzzX_scale;
                            new_huadzzX3.RenderTransform = huadzzX_scale;
                            #endregion
                            #endregion

                            if (tuodong == true)
                            {
                                #region     完成
                                if (tuodong == true)
                                {
                                    can.Children.Add(new_huadzzX);
                                    #region 附图
                                    rec++;
                                    rec1List.Add(new_huadzzX1);
                                    rec2List.Add(new_huadzzX2);
                                    rec3List.Add(new_huadzzX3);
                                    #endregion
                                    m_ZhizuoList.Add(m_Zhizuo);
                                    #region     标签
                                    TextBlock f_TB = new TextBlock();
                                    var f_TBX = m_Zhizuo.Zhizuo_pt.X - 18;
                                    var f_TBY = m_Zhizuo.Zhizuo_pt.Y + 8;

                                    ScaleTransform f_TBscale = new ScaleTransform();
                                    f_TBscale.ScaleY = -1;
                                    f_TB.RenderTransform = f_TBscale;
                                    f_TB.Foreground = Brushes.Red;
                                    f_TB.Text = "" + yuebiao;
                                    f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                    can.Children.Add(f_TB);
                                    #endregion

                                    m_Zhizuo = new ZhizuoModelClass();
                                    zz = 0;
                                    m_Zhizuo_Num++;
                                    //约束叠加数
                                    yueshu = yueshu + 2;
                                }
                                #endregion
                                yuebiao++;
                                tuodong = false;
                            }
                        }
                        if (order != 2)
                        {
                            MessageBox.Show("请先完成杆件绘制，并点击“杆件”菜单页上的“完成绘制”按钮！");
                        }
                        break;
                    }
                #endregion

                #region     滑动纵支座
                case 6:
                    {
                        if (order == 2)
                        {
                            #region     图片信息
                            //滑动支座Y
                            Rectangle huadzzY_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_huadzzY = new Rectangle();
                            new_huadzzY.Height = huadzzY_dataobj.RenderSize.Height;
                            new_huadzzY.Width = huadzzY_dataobj.RenderSize.Width;
                            new_huadzzY.Fill = huadzzY_dataobj.Fill;
                            new_huadzzY.Stroke = huadzzY_dataobj.Stroke;
                            new_huadzzY.StrokeThickness = huadzzY_dataobj.StrokeThickness;

                            #region     附图
                            Rectangle new_huadzzY1 = new Rectangle();
                            new_huadzzY1.Height = huadzzY_dataobj.RenderSize.Height;
                            new_huadzzY1.Width = huadzzY_dataobj.RenderSize.Width;
                            new_huadzzY1.Fill = huadzzY_dataobj.Fill;
                            new_huadzzY1.Stroke = huadzzY_dataobj.Stroke;
                            new_huadzzY1.StrokeThickness = huadzzY_dataobj.StrokeThickness;

                            Rectangle new_huadzzY2 = new Rectangle();
                            new_huadzzY2.Height = huadzzY_dataobj.RenderSize.Height;
                            new_huadzzY2.Width = huadzzY_dataobj.RenderSize.Width;
                            new_huadzzY2.Fill = huadzzY_dataobj.Fill;
                            new_huadzzY2.Stroke = huadzzY_dataobj.Stroke;
                            new_huadzzY2.StrokeThickness = huadzzY_dataobj.StrokeThickness;

                            Rectangle new_huadzzY3 = new Rectangle();
                            new_huadzzY3.Height = huadzzY_dataobj.RenderSize.Height;
                            new_huadzzY3.Width = huadzzY_dataobj.RenderSize.Width;
                            new_huadzzY3.Fill = huadzzY_dataobj.Fill;
                            new_huadzzY3.Stroke = huadzzY_dataobj.Stroke;
                            new_huadzzY3.StrokeThickness = huadzzY_dataobj.StrokeThickness;
                            #endregion
                            #endregion

                            #region     信息存储
                            m_Zhizuo.ganjian = 1;
                            m_Zhizuo.Zhizuo_pt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                            m_Zhizuo.Zhizuo_pt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                            new_huadzzY.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 20);
                            new_huadzzY.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y);
                            #region     附图
                            new_huadzzY1.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 2 - 20);
                            new_huadzzY1.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y);

                            new_huadzzY2.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 20);
                            new_huadzzY2.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area / 2);

                            new_huadzzY3.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 4 - 20);
                            new_huadzzY3.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area);
                            #endregion
                            m_Zhizuo.Zhizuo_Style = 6;

                            #region     记录所在杆件号

                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                    && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[i].Line_EndPoint.X)
                                    && (m_LineModelList[i].Line_Style == 0))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_Zhizuo.Zhizuo_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion

                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                    && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_Zhizuo.Zhizuo_pt.Y >= m_LineModelList[i].Line_EndPoint.Y)
                                    && (m_LineModelList[i].Line_Style == 1))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[j].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                            && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[j].Line_BeginPoint.X)
                                            && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[j].Line_EndPoint.X))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_LineModelList[i].Line_BeginPoint.Y - m_Zhizuo.Zhizuo_pt.Y;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion
                            }
                            #endregion

                            ScaleTransform huadzzY_scale = new ScaleTransform();
                            huadzzY_scale.ScaleY = -1;
                            new_huadzzY.RenderTransform = huadzzY_scale;
                            #region     附图
                            new_huadzzY1.RenderTransform = huadzzY_scale;
                            new_huadzzY2.RenderTransform = huadzzY_scale;
                            new_huadzzY3.RenderTransform = huadzzY_scale;
                            #endregion
                            #endregion

                            if (tuodong == true)
                            {

                                #region     完成
                                can.Children.Add(new_huadzzY);
                                #region     附图
                                rec++;
                                rec1List.Add(new_huadzzY1);
                                rec2List.Add(new_huadzzY2);
                                rec3List.Add(new_huadzzY3);
                                #endregion
                                m_ZhizuoList.Add(m_Zhizuo);
                                #region     标签
                                TextBlock f_TB = new TextBlock();
                                var f_TBX = m_Zhizuo.Zhizuo_pt.X - 3;
                                var f_TBY = m_Zhizuo.Zhizuo_pt.Y - 4;

                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Foreground = Brushes.Red;
                                f_TB.Text = "" + yuebiao;
                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                can.Children.Add(f_TB);
                                #endregion
                                m_Zhizuo = new ZhizuoModelClass();
                                zz = 0;


                                // 约束叠加数
                                yueshu = yueshu + 2;


                                m_Zhizuo_Num++;
                                #endregion
                                tuodong = false;
                                yuebiao++;
                            }
                        }
                        if (order != 2)
                        {
                            MessageBox.Show("请先完成杆件绘制，并点击“杆件”菜单页上的“完成绘制”按钮！");
                        }
                        break;
                    }
                #endregion

                #endregion

                #region   铰节点
                case 7:
                    {
                        if (order == 2)
                        {
                            #region     图片信息
                            Rectangle jiaolian_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_jiaolian = new Rectangle();

                            #region     附图
                            Rectangle new_jiaolian1 = new Rectangle();
                            Rectangle new_jiaolian2 = new Rectangle();
                            Rectangle new_jiaolian3 = new Rectangle();
                            #endregion

                            new_jiaolian.Height = jiaolian_dataobj.RenderSize.Height;
                            new_jiaolian.Width = jiaolian_dataobj.RenderSize.Width;
                            new_jiaolian.Fill = jiaolian_dataobj.Fill;
                            new_jiaolian.Stroke = jiaolian_dataobj.Stroke;
                            new_jiaolian.StrokeThickness = jiaolian_dataobj.StrokeThickness;

                            #region     附图
                            new_jiaolian1.Height = jiaolian_dataobj.RenderSize.Height;
                            new_jiaolian1.Width = jiaolian_dataobj.RenderSize.Width;
                            new_jiaolian1.Fill = jiaolian_dataobj.Fill;
                            new_jiaolian1.Stroke = jiaolian_dataobj.Stroke;
                            new_jiaolian1.StrokeThickness = jiaolian_dataobj.StrokeThickness;

                            new_jiaolian2.Height = jiaolian_dataobj.RenderSize.Height;
                            new_jiaolian2.Width = jiaolian_dataobj.RenderSize.Width;
                            new_jiaolian2.Fill = jiaolian_dataobj.Fill;
                            new_jiaolian2.Stroke = jiaolian_dataobj.Stroke;
                            new_jiaolian2.StrokeThickness = jiaolian_dataobj.StrokeThickness;

                            new_jiaolian3.Height = jiaolian_dataobj.RenderSize.Height;
                            new_jiaolian3.Width = jiaolian_dataobj.RenderSize.Width;
                            new_jiaolian3.Fill = jiaolian_dataobj.Fill;
                            new_jiaolian3.Stroke = jiaolian_dataobj.Stroke;
                            new_jiaolian3.StrokeThickness = jiaolian_dataobj.StrokeThickness;
                            #endregion

                            m_Zhizuo.Zhizuo_pt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                            m_Zhizuo.Zhizuo_pt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                            new_jiaolian.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 20);
                            new_jiaolian.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20);

                            #region     附图
                            new_jiaolian1.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 2 - 20);
                            new_jiaolian1.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y + 20);

                            new_jiaolian2.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X - 20);
                            new_jiaolian2.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area / 2 + 20);

                            new_jiaolian3.SetValue(Canvas.LeftProperty, m_Zhizuo.Zhizuo_pt.X + x_area / 4 - 20);
                            new_jiaolian3.SetValue(Canvas.TopProperty, m_Zhizuo.Zhizuo_pt.Y - y_area + 20);
                            #endregion
                            ScaleTransform jiaolian_scale = new ScaleTransform();
                            jiaolian_scale.ScaleY = -1;
                            new_jiaolian.RenderTransform = jiaolian_scale;
                            #region     附图
                            new_jiaolian1.RenderTransform = jiaolian_scale;
                            new_jiaolian2.RenderTransform = jiaolian_scale;
                            new_jiaolian3.RenderTransform = jiaolian_scale;
                            #endregion
                            #endregion

                            #region     信息存储
                            m_HingeJoint.J_pt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                            m_HingeJoint.J_pt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                            m_HingeJoint.J_Layer = -1;

                            #region  复铰层数
                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                if (m_HingeJoint.J_pt == m_LineModelList[i].Line_BeginPoint)
                                {
                                    m_HingeJoint.J_Layer = m_HingeJoint.J_Layer + 1;
                                }
                                if (m_HingeJoint.J_pt == m_LineModelList[i].Line_EndPoint)
                                {
                                    m_HingeJoint.J_Layer = m_HingeJoint.J_Layer + 1;
                                }
                            }
                            #endregion

                            #endregion

                            #region     记录所在杆件号

                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                    && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[i].Line_EndPoint.X)
                                    && (m_LineModelList[i].Line_Style == 0))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_Zhizuo.Zhizuo_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion

                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_Zhizuo.Zhizuo_pt.X)
                                    && (m_Zhizuo.Zhizuo_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_Zhizuo.Zhizuo_pt.Y >= m_LineModelList[i].Line_EndPoint.Y)
                                    && (m_LineModelList[i].Line_Style == 1))
                                {
                                    tuodong = true;

                                    #region     判断是否在节点上
                                    for (int j = i + 1; j < m_Line_Num; j++)
                                    {
                                        if ((m_LineModelList[j].Line_BeginPoint.Y == m_Zhizuo.Zhizuo_pt.Y)
                                            && (m_Zhizuo.Zhizuo_pt.X >= m_LineModelList[j].Line_BeginPoint.X)
                                            && (m_Zhizuo.Zhizuo_pt.X <= m_LineModelList[j].Line_EndPoint.X))
                                        {
                                            m_Zhizuo.ganjian = 2;
                                        }
                                    }
                                    #endregion

                                    if (m_Zhizuo.ganjian == 1)
                                    {
                                        m_Zhizuo.Z_Length = m_LineModelList[i].Line_BeginPoint.Y - m_Zhizuo.Zhizuo_pt.Y;
                                        m_Zhizuo.Z_Line_Num = i;
                                    }

                                }
                                #endregion
                            }
                            #endregion

                            #region     完成
                            //自由度计算约束叠加数 
                            if (tuodong == true)
                            {
                                yueshu = yueshu + 2 * m_HingeJoint.J_Layer;

                                m_HingeJointList.Add(m_HingeJoint);

                                #region     标签
                                TextBlock f_TB = new TextBlock();
                                var f_TBX = m_HingeJoint.J_pt.X - 3;
                                var f_TBY = m_HingeJoint.J_pt.Y + 20;

                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Foreground = Brushes.Red;
                                f_TB.Text = "" + yuebiao;
                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                                can.Children.Add(f_TB);

                                m_HingeJoint = new JointClass();

                                can.Children.Add(new_jiaolian);
                                #region     附图
                                rec++;
                                rec1List.Add(new_jiaolian1);
                                rec2List.Add(new_jiaolian2);
                                rec3List.Add(new_jiaolian3);
                                #endregion

                                m_Hinge_Num = m_Hinge_Num + 1;

                                #endregion
                                zz = 0;
                                yuebiao++;
                            }
                            #endregion
                        }
                        tuodong = false;
                        break;
                    }
                #endregion

                #region 载荷详细信息

                #region     集中力

                #region     上集中力
                case 9:
                    {
                        if (order == 3)
                        {
                            #region     获取图片信息
                            Rectangle shangjzl_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_shangjzl = new Rectangle();
                            new_shangjzl.Height = shangjzl_dataobj.RenderSize.Height;
                            new_shangjzl.Width = shangjzl_dataobj.RenderSize.Width;
                            new_shangjzl.Fill = shangjzl_dataobj.Fill;
                            new_shangjzl.Stroke = shangjzl_dataobj.Stroke;
                            new_shangjzl.StrokeThickness = shangjzl_dataobj.StrokeThickness;
                            new_shangjzl.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20 - 5);
                            new_shangjzl.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20);

                            ScaleTransform shangjzl_scale = new ScaleTransform();
                            shangjzl_scale.ScaleY = -1;
                            new_shangjzl.RenderTransform = shangjzl_scale;
                            #endregion

                            #region     载荷信息存储
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;


                            #region     载荷所在杆件、节点信息
                            for (int h = 0; h < m_Hinge_Num; h++)
                            {
                                if (m_HingeJointList[h].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 1;
                                    m_LoadModel.Load_Joint_Num = h;
                                }
                            }
                            for (int r = 0; r < m_Rigid_Num; r++)
                            {
                                if (m_RigidJointList[r].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 2;
                                    m_LoadModel.Load_Joint_Num = r;
                                }
                            }
                            if (ganjian == true)
                            {
                                for (int i = 0; i < m_Line_Num; i++)
                                {

                                    if (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_BeginPoint.Y)
                                    {
                                        if ((m_LoadModel.Load_pt.X > m_LineModelList[i].Line_BeginPoint.X)
                                            && (m_LoadModel.Load_pt.X < m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LoadModel.Load_pt.X
                                                - m_LineModelList[i].Line_BeginPoint.X;
                                        }
                                        if (m_LoadModel.Load_pt.X == m_LineModelList[i].Line_BeginPoint.X)
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LoadModel.Load_pt.X
                                                - m_LineModelList[i].Line_BeginPoint.X;
                                        }
                                        if (m_LoadModel.Load_pt.X == m_LineModelList[i].Line_EndPoint.X)
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LoadModel.Load_pt.X
                                                - m_LineModelList[i].Line_BeginPoint.X;
                                        }
                                    }
                                }
                            }
                            #endregion

                            m_LoadModel.Load_F = 1.0 * double_F;
                            m_LoadModel.Load_l = 0;
                            m_LoadModel.Load_q = 0;
                            m_LoadModel.Load_M = 0;
                            m_LoadModel.Load_Style = 1;
                            tuodong = false;
                            #region     锁定到杆件上
                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_LoadModel.Load_pt.Y)
                                    && (m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                {
                                    tuodong = true;
                                }
                                #endregion
                            }
                            #endregion
                            if (tuodong == true)
                            {
                                m_LoadModelList.Add(m_LoadModel);
                                TextBlock f_TB = new TextBlock();
                                var f_TBX = m_LoadModel.Load_pt.X;
                                var f_TBY = m_LoadModel.Load_pt.Y - 50;
                                m_LoadModel = new LoadModel();
                                ganjian = true;


                                m_Load_Num = m_Load_Num + 1;

                                #region 集中力标签、图片添加

                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Text = double_F/20/bilichi + "";
                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);

                                can.Children.Add(new_shangjzl);
                                can.Children.Add(f_TB);
                                f_Num = f_Num + 1;
                                zz = 0;

                                #endregion
                            }
                            #endregion
                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }

                #endregion

                #region     下集中力
                case 10:
                    {
                        if (order == 3)
                        {
                            #region     获取图片信息
                            Rectangle xiajzl_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_xiajzl = new Rectangle();
                            new_xiajzl.Height = xiajzl_dataobj.RenderSize.Height;
                            new_xiajzl.Width = xiajzl_dataobj.RenderSize.Width;
                            new_xiajzl.Fill = xiajzl_dataobj.Fill;
                            new_xiajzl.Stroke = xiajzl_dataobj.Stroke;
                            new_xiajzl.StrokeThickness = xiajzl_dataobj.StrokeThickness;
                            new_xiajzl.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20 - 5);
                            new_xiajzl.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20 + 50);

                            ScaleTransform xiajzl_scale = new ScaleTransform();
                            xiajzl_scale.ScaleY = -1;
                            new_xiajzl.RenderTransform = xiajzl_scale;
                            #endregion

                            #region     载荷信息存储
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;

                            #region     载荷所在杆件、节点信息
                            for (int h = 0; h < m_Hinge_Num; h++)
                            {
                                if (m_HingeJointList[h].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 1;
                                    m_LoadModel.Load_Joint_Num = h;
                                }
                            }
                            for (int r = 0; r < m_Rigid_Num; r++)
                            {
                                if (m_RigidJointList[r].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 2;
                                    m_LoadModel.Load_Joint_Num = r;
                                }
                            }
                            if (ganjian == true)
                            {
                                for (int i = 0; i < m_Line_Num; i++)
                                {

                                    if (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_BeginPoint.Y)
                                    {
                                        if ((m_LoadModel.Load_pt.X > m_LineModelList[i].Line_BeginPoint.X)
                                            && (m_LoadModel.Load_pt.X < m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LoadModel.Load_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        }
                                        if (m_LoadModel.Load_pt.X == m_LineModelList[i].Line_BeginPoint.X)
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LoadModel.Load_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        }
                                        if (m_LoadModel.Load_pt.X == m_LineModelList[i].Line_EndPoint.X)
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LoadModel.Load_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        }
                                    }
                                }
                            }
                            #endregion
                            tuodong = false;

                            #region     锁定到杆件上
                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_LoadModel.Load_pt.Y)
                                    && (m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                {
                                    tuodong = true;
                                }
                                #endregion
                            }
                            #endregion
                            if (tuodong == true)
                            {
                                m_LoadModel.Load_F = -1.0 * double_F;
                                m_LoadModel.Load_l = 0;
                                m_LoadModel.Load_q = 0;
                                m_LoadModel.Load_M = 0;
                                m_LoadModel.Load_Style = 1;

                                m_LoadModelList.Add(m_LoadModel);


                                m_Load_Num = m_Load_Num + 1;

                                #region 集中力标签、图片添加
                                TextBlock f_TB = new TextBlock();
                                var f_TBX = m_LoadModel.Load_pt.X;
                                var f_TBY = m_LoadModel.Load_pt.Y + 70;

                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Text = double_F/20/bilichi + "";
                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);

                                can.Children.Add(new_xiajzl);
                                can.Children.Add(f_TB);
                                f_Num = f_Num + 1;
                                zz = 0;

                                #endregion
                            }

                            m_LoadModel = new LoadModel();
                            ganjian = true;
                            #endregion
                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }
                #endregion

                #region     右集中力
                case 11:
                    {
                        if (order == 3)
                        {
                            #region     图片信息获取
                            Rectangle youjzl_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_youjzl = new Rectangle();
                            new_youjzl.Height = youjzl_dataobj.RenderSize.Height;
                            new_youjzl.Width = youjzl_dataobj.RenderSize.Width;
                            new_youjzl.Fill = youjzl_dataobj.Fill;
                            new_youjzl.Stroke = youjzl_dataobj.Stroke;
                            new_youjzl.StrokeThickness = youjzl_dataobj.StrokeThickness;
                            new_youjzl.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20 - 50);
                            new_youjzl.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20 + 5);

                            ScaleTransform youjzl_scale = new ScaleTransform();
                            youjzl_scale.ScaleY = -1;
                            new_youjzl.RenderTransform = youjzl_scale;
                            #endregion

                            #region 载荷信息存储
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;

                            #region     载荷位置信息
                            for (int h = 0; h < m_Hinge_Num; h++)
                            {
                                if (m_HingeJointList[h].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 1;
                                    m_LoadModel.Load_Joint_Num = h;
                                }
                            }
                            for (int r = 0; r < m_Rigid_Num; r++)
                            {
                                if (m_RigidJointList[r].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 2;
                                    m_LoadModel.Load_Joint_Num = r;
                                }
                            }
                            if (ganjian == true)
                            {
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    if (m_LoadModel.Load_pt.X == m_LineModelList[i].Line_BeginPoint.X)
                                    {
                                        if ((m_LoadModel.Load_pt.Y < m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_LoadModel.Load_pt.Y > m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y - m_LoadModel.Load_pt.Y;
                                        }
                                        if (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_BeginPoint.Y)
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y - m_LoadModel.Load_pt.Y;
                                        }
                                        if (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_EndPoint.Y)
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y - m_LoadModel.Load_pt.Y;
                                        }
                                    }
                                }
                            }
                            #endregion

                            tuodong = false;

                            #region     锁定到杆件上
                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_LoadModel.Load_pt.X)
                                    && (m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                {
                                    tuodong = true;
                                }
                                #endregion
                            }
                            #endregion

                            if (tuodong == true)
                            {
                                m_LoadModel.Load_F = double_F;
                                m_LoadModel.Load_l = 0;
                                m_LoadModel.Load_q = 0;
                                m_LoadModel.Load_M = 0;
                                m_LoadModel.Load_Style = 3;
                                m_LoadModelList.Add(m_LoadModel);
                                m_Load_Num = m_Load_Num + 1;

                                #region     集中力标签、图片添加
                                TextBlock f_TB = new TextBlock();
                                var f_TBX = m_LoadModel.Load_pt.X - 10;
                                var f_TBY = m_LoadModel.Load_pt.Y + 20;

                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Text = double_F/20/bilichi + "";
                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);

                                can.Children.Add(new_youjzl);
                                can.Children.Add(f_TB);
                                f_Num = f_Num + 1;
                                zz = 0;
                                #endregion
                            }
                            m_LoadModel = new LoadModel();
                            ganjian = true;
                            #endregion
                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }
                #endregion

                #region     左集中力
                case 12:
                    {
                        if (order == 3)
                        {
                            #region     获取图片信息
                            Rectangle zuojzl_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_zuojzl = new Rectangle();
                            new_zuojzl.Height = zuojzl_dataobj.RenderSize.Height;
                            new_zuojzl.Width = zuojzl_dataobj.RenderSize.Width;
                            new_zuojzl.Fill = zuojzl_dataobj.Fill;
                            new_zuojzl.Stroke = zuojzl_dataobj.Stroke;
                            new_zuojzl.StrokeThickness = zuojzl_dataobj.StrokeThickness;
                            new_zuojzl.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20);
                            new_zuojzl.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20 + 5);

                            ScaleTransform zuojzl_scale = new ScaleTransform();
                            zuojzl_scale.ScaleY = -1;
                            new_zuojzl.RenderTransform = zuojzl_scale;
                            #endregion

                            #region     载荷信息
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;

                            #region     载荷位置信息
                            for (int h = 0; h < m_Hinge_Num; h++)
                            {
                                if (m_HingeJointList[h].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 1;
                                    m_LoadModel.Load_Joint_Num = h;
                                }
                            }
                            for (int r = 0; r < m_Rigid_Num; r++)
                            {
                                if (m_RigidJointList[r].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 2;
                                    m_LoadModel.Load_Joint_Num = r;
                                }
                            }
                            if (ganjian == true)
                            {
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    if (m_LoadModel.Load_pt.X == m_LineModelList[i].Line_BeginPoint.X)
                                    {
                                        if ((m_LoadModel.Load_pt.Y < m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_LoadModel.Load_pt.Y > m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y - m_LoadModel.Load_pt.Y;
                                        }
                                        if (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_BeginPoint.Y)
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y - m_LoadModel.Load_pt.Y;
                                        }
                                        if (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_EndPoint.Y)
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y - m_LoadModel.Load_pt.Y;
                                        }
                                    }
                                }
                            }
                            #endregion

                            tuodong = false;

                            #region     锁定到杆件上
                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_LoadModel.Load_pt.X)
                                    && (m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                {
                                    tuodong = true;
                                }
                                #endregion
                            }
                            #endregion

                            m_LoadModel.Load_F = -1.0 * double_F;
                            m_LoadModel.Load_l = 0;
                            m_LoadModel.Load_q = 0;
                            m_LoadModel.Load_M = 0;
                            m_LoadModel.Load_Style = 3;

                            if (tuodong == true)
                            {
                                m_LoadModelList.Add(m_LoadModel);

                                m_Load_Num = m_Load_Num + 1;

                                #region 集中力标签、图片添加
                                TextBlock f_TB = new TextBlock();
                                var f_TBX = m_LoadModel.Load_pt.X + 10;
                                var f_TBY = m_LoadModel.Load_pt.Y + 20;

                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Text = double_F/20/bilichi + "";
                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);

                                can.Children.Add(new_zuojzl);
                                can.Children.Add(f_TB);
                                f_Num = f_Num + 1;

                                #endregion
                            }

                            m_LoadModel = new LoadModel();
                            ganjian = true;
                            #endregion
                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }
                #endregion

                #endregion

                #region     均布载荷

                #region     右均布载荷
                case 13:
                    {
                        if (order == 3)
                        {
                            #region     图片信息
                            Rectangle youjbzh_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_youjbzh = new Rectangle();

                            new_youjbzh.Height = double_l;

                            new_youjbzh.Width = youjbzh_dataobj.RenderSize.Width;
                            new_youjbzh.Fill = youjbzh_dataobj.Fill;
                            new_youjbzh.Stroke = youjbzh_dataobj.Stroke;
                            new_youjbzh.StrokeThickness = youjbzh_dataobj.StrokeThickness;
                            new_youjbzh.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20 - 20);
                            new_youjbzh.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20);

                            ScaleTransform youjbzh_scale = new ScaleTransform();
                            youjbzh_scale.ScaleY = -1;
                            new_youjbzh.RenderTransform = youjbzh_scale;
                            #endregion
                            
                            #region 载荷信息存储
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;
                            m_LoadModel.Load_F = 0;
                            m_LoadModel.Load_l = double_l;
                            m_LoadModel.Load_q = double_q;
                            m_LoadModel.Load_M = 0;
                            m_LoadModel.Load_Style = 5;

                            #region     载荷位置信息
                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                if ((m_LoadModel.Load_pt.X == m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_LoadModel.Load_pt.X == m_LineModelList[i].Line_EndPoint.X))
                                {
                                    if ((m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                        && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                    {
                                        m_LoadModel.Load_Location = 3;
                                        m_LoadModel.Load_Line_Num = i;
                                        m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y
                                            - m_LoadModel.Load_pt.Y + m_LoadModel.Load_l / 2;
                                    }
                                }
                            }
                            #endregion

                            m_LoadModel.Load_F = 0;
                            m_LoadModel.Load_l = double_l;
                            m_LoadModel.Load_q = double_q;
                            m_LoadModel.Load_M = 0;
                            m_LoadModel.Load_Style = 5;
                            m_LoadModel.Load_Location = 3;
                            tuodong = false;

                            #region     锁定到杆件上
                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_LoadModel.Load_pt.X)
                                    && (m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                {
                                    tuodong = true;
                                }
                                #endregion
                            }
                            #endregion
                            if (tuodong == true)
                            {
                                m_LoadModelList.Add(m_LoadModel);
                                m_Load_Num = m_Load_Num + 1;

                                #region 均布载荷标签、图片添加
                                TextBlock q_TB = new TextBlock();
                                var q_TBX = e.GetPosition(can).X - 10;
                                var q_TBY = e.GetPosition(can).Y + 20;

                                ScaleTransform q_TBscale = new ScaleTransform();
                                q_TBscale.ScaleY = -1;
                                q_TB.RenderTransform = q_TBscale;
                                q_TB.Text = double_q + " ";
                                q_TB.Margin = new Thickness(q_TBX, q_TBY, 0, 0);

                                can.Children.Add(new_youjbzh);
                                can.Children.Add(q_TB);
                                q_Num = q_Num + 1;
                                #endregion
                            }
                            m_LoadModel = new LoadModel();
                            ganjian = true;
                            #endregion
                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }
                #endregion

                #region     左均布载荷
                case 14:
                    {
                        if (order == 3)
                        {
                            #region 获取图片信息
                            Rectangle zuojbzh_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_zuojbzh = new Rectangle();
                            new_zuojbzh.Height = double_l;
                            new_zuojbzh.Width = zuojbzh_dataobj.RenderSize.Width;
                            new_zuojbzh.Fill = zuojbzh_dataobj.Fill;
                            new_zuojbzh.Stroke = zuojbzh_dataobj.Stroke;
                            new_zuojbzh.StrokeThickness = zuojbzh_dataobj.StrokeThickness;
                            new_zuojbzh.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20);
                            new_zuojbzh.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20);

                            ScaleTransform zuojbzh_scale = new ScaleTransform();
                            zuojbzh_scale.ScaleY = -1;
                            new_zuojbzh.RenderTransform = zuojbzh_scale;
                            #endregion

                            #region 载荷信息存储
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;
                            m_LoadModel.Load_F = 0;
                            m_LoadModel.Load_l = double_l;
                            m_LoadModel.Load_q = -1.0 * double_q;
                            m_LoadModel.Load_M = 0;
                            m_LoadModel.Load_Style = 5;

                            double BeginY = m_LoadModel.Load_pt.Y;
                            double sumlength = double_l;
                            double EndY = m_LoadModel.Load_pt.Y - m_LoadModel.Load_l;

                            bool kuayue = false;        //判断均布载荷是否跨越了铰结点 

                            for (int j = 0; j < m_Hinge_Num; j++)
                            {
                                if (m_HingeJointList[j].J_pt.X == m_LoadModel.Load_pt.X
                                    && m_HingeJointList[j].J_pt.Y < m_LoadModel.Load_pt.Y
                                    && m_HingeJointList[j].J_pt.Y > (m_LoadModel.Load_pt.Y - m_LoadModel.Load_l))
                                {
                                    kuayue = true;

                                    #region     载荷位置信息

                                    m_LoadModel.Load_l = m_LoadModel.Load_pt.Y - m_HingeJointList[j].J_pt.Y;

                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        if ((m_LoadModel.Load_pt.X == m_LineModelList[i].Line_BeginPoint.X) && (m_LoadModel.Load_pt.X == m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            if ((m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y) && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                            {
                                                m_LoadModel.Load_Line_Num = i;
                                                m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y - m_LoadModel.Load_pt.Y + m_LoadModel.Load_l / 2;
                                            }
                                        }
                                    }

                                    m_LoadModel.Load_Location = 3;
                                    tuodong = false;

                                    #endregion

                                    #region     锁定到杆件上
                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        #region     竖杆
                                        if ((m_LineModelList[i].Line_BeginPoint.X == m_LoadModel.Load_pt.X)
                                            && (m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            tuodong = true;
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region 均布载荷加入链表
                                    if (tuodong == true)
                                    {
                                        m_LoadModelList.Add(m_LoadModel);
                                        m_Load_Num = m_Load_Num + 1;
                                        q_Num = q_Num + 1;
                                    }

                                    #endregion

                                    m_LoadModel = new LoadModel();
                                    m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                                    m_LoadModel.Load_F = 0;
                                    m_LoadModel.Load_q = -1.0 * double_q;
                                    m_LoadModel.Load_M = 0;
                                    m_LoadModel.Load_Style = 5;
                                    m_LoadModel.Load_pt.Y = m_HingeJointList[j].J_pt.Y;
                                    m_LoadModel.Load_l = m_LoadModel.Load_pt.Y - EndY;

                                    #region     载荷位置信息
                                    
                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        if ((m_LoadModel.Load_pt.X == m_LineModelList[i].Line_BeginPoint.X) && (m_LoadModel.Load_pt.X == m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            if ((m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y) && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                            {
                                                m_LoadModel.Load_Line_Num = i;
                                                m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y - m_LoadModel.Load_pt.Y + m_LoadModel.Load_l / 2;
                                            }
                                        }
                                    }

                                    m_LoadModel.Load_Location = 3;
                                    tuodong = false;

                                    #endregion

                                    #region     锁定到杆件上
                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        #region     竖杆
                                        if ((m_LineModelList[i].Line_BeginPoint.X == m_LoadModel.Load_pt.X)
                                            && (m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            tuodong = true;
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region 均布载荷图片添加

                                    if (tuodong == true)
                                    {
                                        m_LoadModelList.Add(m_LoadModel);
                                        m_Load_Num = m_Load_Num + 1;

                                        TextBlock q_TB = new TextBlock();
                                        var q_TBX = m_LoadModel.Load_pt.X + 15;
                                        var q_TBY = BeginY;

                                        ScaleTransform q_TBscale = new ScaleTransform();
                                        q_TBscale.ScaleY = -1;
                                        q_TB.RenderTransform = q_TBscale;
                                        q_TB.Text = double_q + " ";
                                        q_TB.Margin = new Thickness(q_TBX, q_TBY, 0, 0);

                                        can.Children.Add(new_zuojbzh);
                                        can.Children.Add(q_TB);
                                        q_Num = q_Num + 1;

                                    }

                                    m_LoadModel = new LoadModel();
                                    ganjian = true;

                                    #endregion

                                }
                            }

                            if (kuayue == false)
                            {
                                #region     载荷位置信息
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    if ((m_LoadModel.Load_pt.X == m_LineModelList[i].Line_BeginPoint.X) && (m_LoadModel.Load_pt.X == m_LineModelList[i].Line_EndPoint.X))
                                    {
                                        if ((m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y) && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y - m_LoadModel.Load_pt.Y + m_LoadModel.Load_l / 2;
                                        }
                                    }
                                }

                                m_LoadModel.Load_Location = 3;
                                tuodong = false;

                                #endregion

                                #region     锁定到杆件上
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    #region     竖杆
                                    if ((m_LineModelList[i].Line_BeginPoint.X == m_LoadModel.Load_pt.X)
                                        && (m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                        && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                    {
                                        tuodong = true;
                                    }
                                    #endregion
                                }
                                #endregion

                                #region 均布载荷图片添加

                                if (tuodong == true)
                                {
                                    m_LoadModelList.Add(m_LoadModel);
                                    m_Load_Num = m_Load_Num + 1;

                                    TextBlock q_TB = new TextBlock();
                                    var q_TBX = m_LoadModel.Load_pt.X +15;
                                    var q_TBY = m_LoadModel.Load_pt.Y ;

                                    ScaleTransform q_TBscale = new ScaleTransform();
                                    q_TBscale.ScaleY = -1;
                                    q_TB.RenderTransform = q_TBscale;
                                    q_TB.Text = double_q + " ";
                                    q_TB.Margin = new Thickness(q_TBX, q_TBY, 0, 0);

                                    can.Children.Add(new_zuojbzh);
                                    can.Children.Add(q_TB);
                                    q_Num = q_Num + 1;

                                }

                                m_LoadModel = new LoadModel();
                                ganjian = true;

                                #endregion

                            }
                            #endregion
                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }
                #endregion

                #region     下均布载荷
                case 15:
                    {
                        if (order == 3)
                        {
                            #region     获取图片信息
                            Rectangle xiajbzh_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_xiajbzh = new Rectangle();
                            new_xiajbzh.Height = xiajbzh_dataobj.RenderSize.Height;
                            new_xiajbzh.Width = double_l;
                            new_xiajbzh.Fill = xiajbzh_dataobj.Fill;
                            new_xiajbzh.Stroke = xiajbzh_dataobj.Stroke;
                            new_xiajbzh.StrokeThickness = xiajbzh_dataobj.StrokeThickness;
                            new_xiajbzh.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20);
                            new_xiajbzh.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20 + 20);

                            ScaleTransform xiajbzh_scale = new ScaleTransform();
                            xiajbzh_scale.ScaleY = -1;
                            new_xiajbzh.RenderTransform = xiajbzh_scale;
                            #endregion

                            #region 载荷信息存储
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;
                            m_LoadModel.Load_F = 0;
                            m_LoadModel.Load_l = double_l;
                            m_LoadModel.Load_q = -1.0 * double_q;
                            m_LoadModel.Load_M = 0;
                            m_LoadModel.Load_Style = 7;

                            double BeginX = m_LoadModel.Load_pt.X;
                            double sumlength = double_l;
                            double EndX = m_LoadModel.Load_pt.X + m_LoadModel.Load_l;
                            
                            bool kuayue = false;        //判断均布载荷是否跨越了铰结点 

                            for (int j = 0; j < m_Hinge_Num; j++)
                            {
                                if (m_HingeJointList[j].J_pt.Y == m_LoadModel.Load_pt.Y
                                    && m_HingeJointList[j].J_pt.X > m_LoadModel.Load_pt.X
                                    && m_HingeJointList[j].J_pt.X < (m_LoadModel.Load_pt.X + m_LoadModel.Load_l))
                                {
                                    kuayue = true;

                                    #region     载荷位置信息

                                    m_LoadModel.Load_l = m_HingeJointList[j].J_pt.X - m_LoadModel.Load_pt.X;

                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        if ((m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_BeginPoint.Y) && (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            if ((m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X) && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                            {
                                                m_LoadModel.Load_Line_Num = i;
                                                m_LoadModel.Load_Length = m_LoadModel.Load_pt.X - m_LineModelList[i].Line_BeginPoint.X + m_LoadModel.Load_l / 2;
                                            }
                                        }
                                    }

                                    m_LoadModel.Load_Location = 3;
                                    tuodong = false;

                                    #endregion

                                    #region     锁定到杆件上
                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        #region     横杆
                                        if ((m_LineModelList[i].Line_BeginPoint.Y == m_LoadModel.Load_pt.Y)
                                            && (m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                            && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            tuodong = true;
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region 均布载荷加入链表
                                    if (tuodong == true)
                                    {
                                        m_LoadModelList.Add(m_LoadModel);
                                        m_Load_Num = m_Load_Num + 1;
                                        q_Num = q_Num + 1;
                                    }

                                    #endregion

                                    m_LoadModel = new LoadModel();
                                    m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;
                                    m_LoadModel.Load_F = 0;
                                    m_LoadModel.Load_q = -1.0 * double_q;
                                    m_LoadModel.Load_M = 0;
                                    m_LoadModel.Load_Style = 7;
                                    m_LoadModel.Load_pt.X = m_HingeJointList[j].J_pt.X;
                                    m_LoadModel.Load_l = EndX - m_LoadModel.Load_pt.X;

                                    #region     载荷位置信息

                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        if ((m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_BeginPoint.Y) && (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            if ((m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X) && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                            {
                                                m_LoadModel.Load_Line_Num = i;
                                                m_LoadModel.Load_Length = m_LoadModel.Load_pt.X - m_LineModelList[i].Line_BeginPoint.X + m_LoadModel.Load_l / 2;
                                            }
                                        }
                                    }

                                    m_LoadModel.Load_Location = 3;
                                    tuodong = false;

                                    #endregion

                                    #region     锁定到杆件上
                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        #region     横杆
                                        if ((m_LineModelList[i].Line_BeginPoint.Y == m_LoadModel.Load_pt.Y)
                                            && (m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                            && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            tuodong = true;
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region 均布载荷图片添加

                                    if (tuodong == true)
                                    {
                                        m_LoadModelList.Add(m_LoadModel);
                                        m_Load_Num = m_Load_Num + 1;

                                        TextBlock q_TB = new TextBlock();
                                        var q_TBX = BeginX - 25 + sumlength / 2;
                                        var q_TBY = m_LoadModel.Load_pt.Y + 30;

                                        ScaleTransform q_TBscale = new ScaleTransform();
                                        q_TBscale.ScaleY = -1;
                                        q_TB.RenderTransform = q_TBscale;
                                        q_TB.Text = double_q + " ";
                                        q_TB.Margin = new Thickness(q_TBX, q_TBY, 0, 0);

                                        can.Children.Add(new_xiajbzh);
                                        can.Children.Add(q_TB);
                                        q_Num = q_Num + 1;

                                    }

                                    m_LoadModel = new LoadModel();
                                    ganjian = true;

                                    #endregion

                                }
                            }
                            
                            if (kuayue == false)
                            {
                                #region     载荷位置信息
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    if ((m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_BeginPoint.Y) && (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_EndPoint.Y))
                                    {
                                        if ((m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X) && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LoadModel.Load_pt.X - m_LineModelList[i].Line_BeginPoint.X + m_LoadModel.Load_l / 2;
                                        }
                                    }
                                }
                                
                                m_LoadModel.Load_Location = 3;
                                tuodong = false;

                                #endregion

                                #region     锁定到杆件上
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    #region     横杆
                                    if ((m_LineModelList[i].Line_BeginPoint.Y == m_LoadModel.Load_pt.Y)
                                        && (m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                        && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                    {
                                        tuodong = true;
                                    }
                                    #endregion
                                }
                                #endregion

                                #region 均布载荷图片添加

                                if (tuodong == true)
                                {
                                    m_LoadModelList.Add(m_LoadModel);
                                    m_Load_Num = m_Load_Num + 1;
                                    
                                    TextBlock q_TB = new TextBlock();
                                    var q_TBX = m_LoadModel.Load_pt.X - 25 + double_l / 2;
                                    var q_TBY = m_LoadModel.Load_pt.Y + 30;

                                    ScaleTransform q_TBscale = new ScaleTransform();
                                    q_TBscale.ScaleY = -1;
                                    q_TB.RenderTransform = q_TBscale;
                                    q_TB.Text = double_q + " ";
                                    q_TB.Margin = new Thickness(q_TBX, q_TBY, 0, 0);

                                    can.Children.Add(new_xiajbzh);
                                    can.Children.Add(q_TB);
                                    q_Num = q_Num + 1;
                                    
                                }

                                m_LoadModel = new LoadModel();
                                ganjian = true;

                                #endregion

                            }
                            #endregion
                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }
                #endregion

                #region     上均布载荷
                case 16:
                    {
                        if (order == 3)
                        {
                            #region     获取图片信息
                            Rectangle shangjbzh_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_shangjbzh = new Rectangle();
                            new_shangjbzh.Height = shangjbzh_dataobj.RenderSize.Height;
                            new_shangjbzh.Width = double_l;
                            new_shangjbzh.Fill = shangjbzh_dataobj.Fill;
                            new_shangjbzh.Stroke = shangjbzh_dataobj.Stroke;
                            new_shangjbzh.StrokeThickness = shangjbzh_dataobj.StrokeThickness;
                            new_shangjbzh.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20);
                            new_shangjbzh.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20);

                            ScaleTransform shangjbzh_scale = new ScaleTransform();
                            shangjbzh_scale.ScaleY = -1;
                            new_shangjbzh.RenderTransform = shangjbzh_scale;
                            #endregion

                            #region 载荷信息存储
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;
                            m_LoadModel.Load_F = 0;
                            m_LoadModel.Load_l = double_l;
                            m_LoadModel.Load_q = double_q;
                            m_LoadModel.Load_M = 0;
                            m_LoadModel.Load_Style = 7;

                            double BeginX = m_LoadModel.Load_pt.X;
                            double sumlength = double_l;
                            double EndX = m_LoadModel.Load_pt.X + m_LoadModel.Load_l;

                            bool kuayue = false;        //判断均布载荷是否跨越了铰结点 

                            for (int j = 0; j < m_Hinge_Num; j++)
                            {
                                if (m_HingeJointList[j].J_pt.Y == m_LoadModel.Load_pt.Y
                                    && m_HingeJointList[j].J_pt.X > m_LoadModel.Load_pt.X
                                    && m_HingeJointList[j].J_pt.X < (m_LoadModel.Load_pt.X + m_LoadModel.Load_l))
                                {
                                    kuayue = true;

                                    #region     载荷位置信息

                                    m_LoadModel.Load_l = m_HingeJointList[j].J_pt.X - m_LoadModel.Load_pt.X;

                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        if ((m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_BeginPoint.Y) && (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            if ((m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X) && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                            {
                                                m_LoadModel.Load_Line_Num = i;
                                                m_LoadModel.Load_Length = m_LoadModel.Load_pt.X - m_LineModelList[i].Line_BeginPoint.X + m_LoadModel.Load_l / 2;
                                            }
                                        }
                                    }

                                    m_LoadModel.Load_Location = 3;
                                    tuodong = false;

                                    #endregion

                                    #region     锁定到杆件上
                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        #region     横杆
                                        if ((m_LineModelList[i].Line_BeginPoint.Y == m_LoadModel.Load_pt.Y)
                                            && (m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                            && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            tuodong = true;
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region 均布载荷加入链表
                                    if (tuodong == true)
                                    {
                                        m_LoadModelList.Add(m_LoadModel);
                                        m_Load_Num = m_Load_Num + 1;
                                        q_Num = q_Num + 1;
                                    }

                                    #endregion

                                    m_LoadModel = new LoadModel();
                                    m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;
                                    m_LoadModel.Load_F = 0;
                                    m_LoadModel.Load_q = double_q;
                                    m_LoadModel.Load_M = 0;
                                    m_LoadModel.Load_Style = 7;
                                    m_LoadModel.Load_pt.X = m_HingeJointList[j].J_pt.X;
                                    m_LoadModel.Load_l = EndX - m_LoadModel.Load_pt.X;

                                    #region     载荷位置信息

                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        if ((m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_BeginPoint.Y) && (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            if ((m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X) && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                            {
                                                m_LoadModel.Load_Line_Num = i;
                                                m_LoadModel.Load_Length = m_LoadModel.Load_pt.X - m_LineModelList[i].Line_BeginPoint.X + m_LoadModel.Load_l / 2;
                                            }
                                        }
                                    }

                                    m_LoadModel.Load_Location = 3;
                                    tuodong = false;

                                    #endregion

                                    #region     锁定到杆件上
                                    for (int i = 0; i < m_Line_Num; i++)
                                    {
                                        #region     横杆
                                        if ((m_LineModelList[i].Line_BeginPoint.Y == m_LoadModel.Load_pt.Y)
                                            && (m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                            && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            tuodong = true;
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region 均布载荷图片添加

                                    if (tuodong == true)
                                    {
                                        m_LoadModelList.Add(m_LoadModel);
                                        m_Load_Num = m_Load_Num + 1;

                                        TextBlock q_TB = new TextBlock();
                                        var q_TBX = BeginX - 25 + sumlength / 2;
                                        var q_TBY = m_LoadModel.Load_pt.Y - 20;

                                        ScaleTransform q_TBscale = new ScaleTransform();
                                        q_TBscale.ScaleY = -1;
                                        q_TB.RenderTransform = q_TBscale;
                                        q_TB.Text = double_q + " ";
                                        q_TB.Margin = new Thickness(q_TBX, q_TBY, 0, 0);

                                        can.Children.Add(new_shangjbzh);
                                        can.Children.Add(q_TB);
                                        q_Num = q_Num + 1;

                                    }

                                    m_LoadModel = new LoadModel();
                                    ganjian = true;

                                    #endregion

                                }
                            }

                            if (kuayue == false)
                            {
                                #region     载荷位置信息
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    if ((m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_BeginPoint.Y) && (m_LoadModel.Load_pt.Y == m_LineModelList[i].Line_EndPoint.Y))
                                    {
                                        if ((m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X) && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LoadModel.Load_pt.X - m_LineModelList[i].Line_BeginPoint.X + m_LoadModel.Load_l / 2;
                                        }
                                    }
                                }

                                m_LoadModel.Load_Location = 3;
                                tuodong = false;

                                #endregion

                                #region     锁定到杆件上
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    #region     横杆
                                    if ((m_LineModelList[i].Line_BeginPoint.Y == m_LoadModel.Load_pt.Y)
                                        && (m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                        && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                    {
                                        tuodong = true;
                                    }
                                    #endregion
                                }
                                #endregion

                                #region 均布载荷图片添加

                                if (tuodong == true)
                                {
                                    m_LoadModelList.Add(m_LoadModel);
                                    m_Load_Num = m_Load_Num + 1;

                                    TextBlock q_TB = new TextBlock();
                                    var q_TBX = m_LoadModel.Load_pt.X - 25 + double_l / 2;
                                    var q_TBY = m_LoadModel.Load_pt.Y -20;

                                    ScaleTransform q_TBscale = new ScaleTransform();
                                    q_TBscale.ScaleY = -1;
                                    q_TB.RenderTransform = q_TBscale;
                                    q_TB.Text = double_q + " ";
                                    q_TB.Margin = new Thickness(q_TBX, q_TBY, 0, 0);

                                    can.Children.Add(new_shangjbzh);
                                    can.Children.Add(q_TB);
                                    q_Num = q_Num + 1;

                                }

                                m_LoadModel = new LoadModel();
                                ganjian = true;

                                #endregion

                            }
                            #endregion

                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }
                #endregion

                #endregion

                #region     集中力偶

                #region     顺时针横杆集中力偶
                case 17:
                    {
                        if (order == 3)
                        {
                            #region     获取图片信息
                            Rectangle shunljX_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_shunljX = new Rectangle();
                            new_shunljX.Height = shunljX_dataobj.RenderSize.Height;
                            new_shunljX.Width = shunljX_dataobj.RenderSize.Width;
                            new_shunljX.Fill = shunljX_dataobj.Fill;
                            new_shunljX.Stroke = shunljX_dataobj.Stroke;
                            new_shunljX.StrokeThickness = shunljX_dataobj.StrokeThickness;
                            new_shunljX.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20 - 15);
                            new_shunljX.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20 + 22.5);

                            TextBlock m_TB = new TextBlock();

                            #endregion

                            #region     载荷信息存储
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;

                            double m_TBX = m_LoadModel.Load_pt.X;
                            double m_TBY = m_LoadModel.Load_pt.Y + 35;
                            
                            #region     载荷位置信息
                            m_LoadModel.Load_Location = 3;
                            rePlacePt.X = m_LoadModel.Load_pt.X;
                            rePlacePt.Y = m_LoadModel.Load_pt.Y;

                            for (int h = 0; h < m_Hinge_Num; h++)
                            {
                                if (m_HingeJointList[h].J_pt == m_LoadModel.Load_pt)
                                {
                                    JiZhongLiOuWindow jzlo = new JiZhongLiOuWindow();
                                    jzlo.ShowDialog();
                                    //显示的杆件号从1开始，存储的杆件号从0开始
                                    int gan_Num = Convert.ToInt32(jzlo.ganjian_Num.Text) - 1;
                                    m_LoadModel.Load_Location = 3;
                                    if (gan_Num <= m_Line_Num && gan_Num>=0 )
                                    {
                                        m_LoadModel.Load_Line_Num = gan_Num;
                                    }
                                    else
                                    {
                                        MessageBox.Show("输入杆件号错误");
                                    }

                                    #region     坐标调整
                                    if (m_LineModelList[gan_Num].Line_Style == 0)
                                    {
                                        #region     图标
                                        //横杆左端点，往右边平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_BeginPoint)
                                        {
                                            new_shunljX.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15 + 5);
                                            new_shunljX.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5);

                                            rePlacePt.X = m_LoadModel.Load_pt.X + 5;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y;

                                            m_LoadModel.Load_Length = 0;
                                        }
                                        //横杆右端点，往左平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_EndPoint)
                                        {
                                            new_shunljX.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15 - 5);
                                            new_shunljX.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5);

                                            rePlacePt.X = m_LoadModel.Load_pt.X - 5;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y;

                                            m_LoadModel.Load_Length = m_LineModelList[gan_Num].LineLength;

                                        }
                                        #endregion

                                        #region     标签（有问题）
                                        if (m_LineModelList[gan_Num].Line_BeginPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBX += 20;
                                        }
                                        if (m_LineModelList[gan_Num].Line_EndPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBX -= 20;
                                        }
                                        #endregion
                                    }
                                    if (m_LineModelList[gan_Num].Line_Style == 1)
                                    {
                                        #region     图标
                                        //横杆左端点，往右边平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_BeginPoint)
                                        {
                                            new_shunljX.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15);
                                            new_shunljX.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5 - 5);
                                            m_LoadModel.Load_Length = 0;

                                            rePlacePt.X = m_LoadModel.Load_pt.X;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y - 5;

                                        }
                                        //横杆右端点，往左平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_EndPoint)
                                        {
                                            new_shunljX.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15);
                                            new_shunljX.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5 + 5);
                                            m_LoadModel.Load_Length = m_LineModelList[gan_Num].LineLength;

                                            rePlacePt.X = m_LoadModel.Load_pt.X;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y + 5;

                                        }
                                        #endregion

                                        #region     标签（有问题）
                                        if (m_LineModelList[gan_Num].Line_BeginPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBY -= 20;
                                        }
                                        if (m_LineModelList[gan_Num].Line_EndPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBY += 20;
                                        }
                                        #endregion
                                    }
                                    #endregion

                                }
                            }
                            for (int r = 0; r < m_Rigid_Num; r++)
                            {
                                if (m_RigidJointList[r].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 2;
                                    m_LoadModel.Load_Joint_Num = r;
                                }
                            }
                            if (ganjian == true)
                            {
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    if (rePlacePt.Y == m_LineModelList[i].Line_BeginPoint.Y)
                                    {
                                        if ((rePlacePt.X >= m_LineModelList[i].Line_BeginPoint.X) && (rePlacePt.X <= m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LoadModel.Load_pt.X - m_LineModelList[i].Line_BeginPoint.X;

                                        }
                                    }
                                }
                            }
                            #endregion

                            ScaleTransform shunljX_scale = new ScaleTransform();
                            shunljX_scale.ScaleY = -1;
                            new_shunljX.RenderTransform = shunljX_scale;

                            m_LoadModel.Load_F = 0;
                            m_LoadModel.Load_l = 0;
                            m_LoadModel.Load_q = 0;
                            m_LoadModel.Load_M = -1.0 * double_M;
                            m_LoadModel.Load_Style = 9;
                            tuodong = false;

                            #region     锁定到杆件上
                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_LoadModel.Load_pt.Y)
                                    && (m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                {
                                    tuodong = true;
                                }
                                #endregion
                            }
                            #endregion

                            if (tuodong == true)
                            {
                                m_LoadModelList.Add(m_LoadModel);
                                m_Load_Num = m_Load_Num + 1;

                                #region 力矩标签、图片添加
                                ScaleTransform m_TBscale = new ScaleTransform();
                                m_TBscale.ScaleY = -1;
                                m_TB.RenderTransform = m_TBscale;
                                m_TB.Text = double_M/400 / bilichi / bilichi + "";
                                m_TB.Margin = new Thickness(m_TBX, m_TBY, 0, 0);

                                can.Children.Add(new_shunljX);
                                can.Children.Add(m_TB);
                                m_Num = m_Num + 1;
                                zz = 0;

                                #endregion
                            }

                            m_LoadModel = new LoadModel();
                            ganjian = true;
                            #endregion
                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }
                #endregion

                #region     逆时针横杆集中力偶
                case 18:
                    {
                        if (order == 3)
                        {
                            #region     获取图片信息
                            Rectangle niljX_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_niljX = new Rectangle();
                            new_niljX.Height = niljX_dataobj.RenderSize.Height;
                            new_niljX.Width = niljX_dataobj.RenderSize.Width;
                            new_niljX.Fill = niljX_dataobj.Fill;
                            new_niljX.Stroke = niljX_dataobj.Stroke;
                            new_niljX.StrokeThickness = niljX_dataobj.StrokeThickness;
                            new_niljX.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20 - 15);
                            new_niljX.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20 + 22.5);

                            ScaleTransform niljX_scale = new ScaleTransform();
                            niljX_scale.ScaleY = -1;
                            new_niljX.RenderTransform = niljX_scale;

                            TextBlock m_TB = new TextBlock();
                            var m_TBX = e.GetPosition(can).X - 10;
                            var m_TBY = e.GetPosition(can).Y + 30 + 10;
                            #endregion

                            #region     载荷信息存储
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;


                            #region     载荷位置信息
                            m_LoadModel.Load_Location = 3;
                            rePlacePt.X = m_LoadModel.Load_pt.X;
                            rePlacePt.Y = m_LoadModel.Load_pt.Y;
                            for (int h = 0; h < m_Hinge_Num; h++)
                            {
                                if (m_HingeJointList[h].J_pt == m_LoadModel.Load_pt)
                                {
                                    JiZhongLiOuWindow jzlo = new JiZhongLiOuWindow();
                                    jzlo.ShowDialog();
                                    //显示的杆件号从1开始，存储的杆件号从0开始
                                    int gan_Num = Convert.ToInt32(jzlo.ganjian_Num.Text) - 1;
                                    m_LoadModel.Load_Location = 3;
                                    if (gan_Num <= m_Line_Num && gan_Num >= 0)
                                    {
                                        m_LoadModel.Load_Line_Num = gan_Num;
                                    }
                                    else
                                    {
                                        MessageBox.Show("输入杆件号错误");
                                    }
                                    #region     坐标调整
                                    if (m_LineModelList[gan_Num].Line_Style == 0)
                                    {
                                        #region     图标
                                        //横杆左端点，往右边平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_BeginPoint)
                                        {
                                            new_niljX.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15 + 5);
                                            new_niljX.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5);
                                            m_LoadModel.Load_Length = 0;

                                            rePlacePt.X = m_LoadModel.Load_pt.X + 5;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y;

                                        }
                                        //横杆右端点，往左平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_EndPoint)
                                        {
                                            new_niljX.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15 - 5);
                                            new_niljX.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5);
                                            m_LoadModel.Load_Length = m_LineModelList[gan_Num].LineLength;

                                            rePlacePt.X = m_LoadModel.Load_pt.X - 5;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y;

                                        }
                                        #endregion

                                        #region     标签
                                        if (m_LineModelList[gan_Num].Line_BeginPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBX += 20;
                                        }
                                        if (m_LineModelList[gan_Num].Line_EndPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBX -= 20;
                                        }
                                        #endregion
                                    }
                                    if (m_LineModelList[gan_Num].Line_Style == 1)
                                    {
                                        #region     图标
                                        //横杆左端点，往右边平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_BeginPoint)
                                        {
                                            new_niljX.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15);
                                            new_niljX.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5 - 5);
                                            m_LoadModel.Load_Length = 0;

                                            rePlacePt.X = m_LoadModel.Load_pt.X;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y - 5;

                                        }
                                        //横杆右端点，往左平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_EndPoint)
                                        {
                                            new_niljX.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15);
                                            new_niljX.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5 + 5);
                                            m_LoadModel.Load_Length = m_LineModelList[gan_Num].LineLength;

                                            rePlacePt.X = m_LoadModel.Load_pt.X;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y + 5;

                                        }
                                        #endregion

                                        #region     标签
                                        if (m_LineModelList[gan_Num].Line_BeginPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBY -= 20;
                                        }
                                        if (m_LineModelList[gan_Num].Line_EndPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBY += 20;
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            for (int r = 0; r < m_Rigid_Num; r++)
                            {
                                if (m_RigidJointList[r].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 2;
                                    m_LoadModel.Load_Joint_Num = r;
                                }
                            }
                            if (ganjian == true)
                            {
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    if (rePlacePt.Y == m_LineModelList[i].Line_BeginPoint.Y)
                                    {
                                        if ((rePlacePt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                            && (rePlacePt.X <= m_LineModelList[i].Line_EndPoint.X))
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LoadModel.Load_pt.X - m_LineModelList[i].Line_BeginPoint.X;
                                        }
                                    }
                                }
                            }
                            #endregion

                            m_LoadModel.Load_F = 0;
                            m_LoadModel.Load_l = 0;
                            m_LoadModel.Load_q = 0;
                            m_LoadModel.Load_M = double_M;
                            m_LoadModel.Load_Style = 9;
                            tuodong = false;

                            #region     锁定到杆件上
                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     横杆
                                if ((m_LineModelList[i].Line_BeginPoint.Y == m_LoadModel.Load_pt.Y)
                                    && (m_LoadModel.Load_pt.X >= m_LineModelList[i].Line_BeginPoint.X)
                                    && (m_LoadModel.Load_pt.X <= m_LineModelList[i].Line_EndPoint.X))
                                {
                                    tuodong = true;
                                }
                                #endregion
                            }
                            #endregion

                            if (tuodong == true)
                            {
                                m_LoadModelList.Add(m_LoadModel);
                                m_Load_Num = m_Load_Num + 1;
                                #region 力矩标签、图片添加
                                ScaleTransform m_TBscale = new ScaleTransform();
                                m_TBscale.ScaleY = -1;
                                m_TB.RenderTransform = m_TBscale;
                                m_TB.Text = double_M/400/bilichi/bilichi + "";
                                m_TB.Margin = new Thickness(m_TBX, m_TBY, 0, 0);

                                can.Children.Add(new_niljX);
                                can.Children.Add(m_TB);
                                m_Num = m_Num + 1;

                                #endregion
                            }
                            m_LoadModel = new LoadModel();
                            ganjian = true;
                            #endregion
                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }
                #endregion

                #region     顺时针竖杆集中力偶
                case 19:
                    {
                        if (order == 3)
                        {
                            #region     获取图片信息
                            Rectangle shunljY_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_shunljY = new Rectangle();
                            new_shunljY.Height = shunljY_dataobj.RenderSize.Height;
                            new_shunljY.Width = shunljY_dataobj.RenderSize.Width;
                            new_shunljY.Fill = shunljY_dataobj.Fill;
                            new_shunljY.Stroke = shunljY_dataobj.Stroke;
                            new_shunljY.StrokeThickness = shunljY_dataobj.StrokeThickness;
                            new_shunljY.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20 - 15);
                            new_shunljY.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20 + 15);

                            ScaleTransform shunljY_scale = new ScaleTransform();
                            shunljY_scale.ScaleY = -1;
                            new_shunljY.RenderTransform = shunljY_scale;
                            #endregion

                            #region     载荷信息存储
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;

                            #region     载荷位置信息
                            //标签
                            TextBlock m_TB = new TextBlock();
                            var m_TBX = e.GetPosition(can).X - 30;
                            var m_TBY = e.GetPosition(can).Y + 20;
                            m_LoadModel.Load_Location = 3;
                            rePlacePt.X = m_LoadModel.Load_pt.X;
                            rePlacePt.Y = m_LoadModel.Load_pt.Y;
                            for (int h = 0; h < m_Hinge_Num; h++)
                            {
                                if (m_HingeJointList[h].J_pt == m_LoadModel.Load_pt)
                                {
                                    JiZhongLiOuWindow jzlo = new JiZhongLiOuWindow();
                                    jzlo.ShowDialog();
                                    //显示的杆件号从1开始，存储的杆件号从0开始
                                    int gan_Num = Convert.ToInt32(jzlo.ganjian_Num.Text) - 1;
                                    m_LoadModel.Load_Location = 3;
                                    if (gan_Num <= m_Line_Num && gan_Num >= 0)
                                    {
                                        m_LoadModel.Load_Line_Num = gan_Num;
                                    }
                                    else
                                    {
                                        MessageBox.Show("输入杆件号错误");
                                    }
                                    #region     坐标调整
                                    if (m_LineModelList[gan_Num].Line_Style == 0)
                                    {
                                        #region     图标
                                        //横杆左端点，往右边平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_BeginPoint)
                                        {
                                            new_shunljY.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15 + 5);
                                            new_shunljY.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5);
                                            m_LoadModel.Load_Length = 0;

                                            rePlacePt.X = m_LoadModel.Load_pt.X + 5;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y;

                                        }
                                        //横杆右端点，往左平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_EndPoint)
                                        {
                                            new_shunljY.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15 - 5);
                                            new_shunljY.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5);
                                            m_LoadModel.Load_Length = m_LineModelList[gan_Num].LineLength;

                                            rePlacePt.X = m_LoadModel.Load_pt.X - 5;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y;

                                        }
                                        #endregion

                                        #region     标签（有问题）
                                        if (m_LineModelList[gan_Num].Line_BeginPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBX += 20;
                                        }
                                        if (m_LineModelList[gan_Num].Line_EndPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBX -= 20;
                                        }
                                        #endregion
                                    }
                                    if (m_LineModelList[gan_Num].Line_Style == 1)
                                    {
                                        #region     图标
                                        //横杆左端点，往右边平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_BeginPoint)
                                        {
                                            new_shunljY.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15);
                                            new_shunljY.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5 - 5);
                                            m_LoadModel.Load_Length = 0;

                                            rePlacePt.X = m_LoadModel.Load_pt.X;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y - 5;

                                        }
                                        //横杆右端点，往左平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_EndPoint)
                                        {
                                            new_shunljY.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15);
                                            new_shunljY.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5 + 5);
                                            m_LoadModel.Load_Length = m_LineModelList[gan_Num].LineLength;

                                            rePlacePt.X = m_LoadModel.Load_pt.X;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y + 5;

                                        }
                                        #endregion

                                        #region     标签（有问题）
                                        if (m_LineModelList[gan_Num].Line_BeginPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBY -= 20;
                                        }
                                        if (m_LineModelList[gan_Num].Line_EndPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBY += 20;
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            for (int r = 0; r < m_Rigid_Num; r++)
                            {
                                if (m_RigidJointList[r].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 2;
                                    m_LoadModel.Load_Joint_Num = r;
                                }
                            }
                            if (ganjian == true)
                            {
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    if (rePlacePt.X == m_LineModelList[i].Line_BeginPoint.X)
                                    {
                                        if ((rePlacePt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (rePlacePt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y - m_LoadModel.Load_pt.Y;
                                        }
                                    }
                                }
                            }
                            #endregion

                            m_LoadModel.Load_F = 0;
                            m_LoadModel.Load_l = 0;
                            m_LoadModel.Load_q = 0;
                            m_LoadModel.Load_M = -1.0 * double_M;
                            m_LoadModel.Load_Style = 11;

                            tuodong = false;

                            #region     锁定到杆件上
                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_LoadModel.Load_pt.X)
                                    && (m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                {
                                    tuodong = true;
                                }
                                #endregion
                            }
                            #endregion
                            if (tuodong == true)
                            {
                                m_LoadModelList.Add(m_LoadModel);
                                m_Load_Num = m_Load_Num + 1;

                                #region 力矩标签、图片添加

                                ScaleTransform m_TBscale = new ScaleTransform();
                                m_TBscale.ScaleY = -1;
                                m_TB.RenderTransform = m_TBscale;
                                m_TB.Text = double_M/400/bilichi/bilichi + "";
                                m_TB.Margin = new Thickness(m_TBX, m_TBY, 0, 0);

                                can.Children.Add(new_shunljY);
                                can.Children.Add(m_TB);
                                m_Num = m_Num + 1;

                                #endregion
                            }
                            m_LoadModel = new LoadModel();
                            ganjian = true;
                            #endregion
                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }
                #endregion

                #region     逆时针竖杆集中力偶
                case 20:
                    {
                        if (order == 3)
                        {
                            #region     获取图片信息
                            Rectangle niljY_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                            Rectangle new_niljY = new Rectangle();
                            new_niljY.Height = niljY_dataobj.RenderSize.Height;
                            new_niljY.Width = niljY_dataobj.RenderSize.Width;
                            new_niljY.Fill = niljY_dataobj.Fill;
                            new_niljY.Stroke = niljY_dataobj.Stroke;
                            new_niljY.StrokeThickness = niljY_dataobj.StrokeThickness;
                            new_niljY.SetValue(Canvas.LeftProperty, Math.Round(e.GetPosition(can).X / 20) * 20 - 15);
                            new_niljY.SetValue(Canvas.TopProperty, Math.Round(e.GetPosition(can).Y / 20) * 20 + 15);

                            ScaleTransform niljY_scale = new ScaleTransform();
                            niljY_scale.ScaleY = -1;
                            new_niljY.RenderTransform = niljY_scale;
                            #endregion

                            #region     载荷信息存储
                            m_LoadModel.Load_pt.X = (Math.Round(e.GetPosition(can).X / 20)) * 20;
                            m_LoadModel.Load_pt.Y = (Math.Round(e.GetPosition(can).Y / 20)) * 20;
                            
                            #region     载荷位置信息
                            //标签
                            TextBlock m_TB = new TextBlock();
                            var m_TBX = e.GetPosition(can).X - 30;
                            var m_TBY = e.GetPosition(can).Y + 20;
                            m_LoadModel.Load_Location = 3;
                            rePlacePt.X = m_LoadModel.Load_pt.X;
                            rePlacePt.Y = m_LoadModel.Load_pt.Y;
                            for (int h = 0; h < m_Hinge_Num; h++)
                            {
                                if (m_HingeJointList[h].J_pt == m_LoadModel.Load_pt)
                                {
                                    JiZhongLiOuWindow jzlo = new JiZhongLiOuWindow();
                                    jzlo.ShowDialog();
                                    //显示的杆件号从1开始，存储的杆件号从0开始
                                    int gan_Num = Convert.ToInt32(jzlo.ganjian_Num.Text) - 1;
                                    m_LoadModel.Load_Location = 3;
                                    if (gan_Num <= m_Line_Num && gan_Num >= 0)
                                    {
                                        m_LoadModel.Load_Line_Num = gan_Num;
                                    }
                                    else
                                    {
                                        MessageBox.Show("输入杆件号错误");
                                    }
                                    #region     坐标调整
                                    if (m_LineModelList[gan_Num].Line_Style == 0)
                                    {
                                        #region     图标
                                        //横杆左端点，往右边平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_BeginPoint)
                                        {
                                            new_niljY.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15 + 5);
                                            new_niljY.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5);
                                            m_LoadModel.Load_Length = 0;

                                            rePlacePt.X = m_LoadModel.Load_pt.X + 5;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y;

                                        }
                                        //横杆右端点，往左平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_EndPoint)
                                        {
                                            new_niljY.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15 - 5);
                                            new_niljY.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5);
                                            m_LoadModel.Load_Length = m_LineModelList[gan_Num].LineLength;

                                            rePlacePt.X = m_LoadModel.Load_pt.X - 5;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y;

                                        }
                                        #endregion

                                        #region     标签（有问题）
                                        if (m_LineModelList[gan_Num].Line_BeginPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBX += 20;
                                        }
                                        if (m_LineModelList[gan_Num].Line_EndPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBX -= 20;
                                        }
                                        #endregion
                                    }
                                    if (m_LineModelList[gan_Num].Line_Style == 1)
                                    {
                                        #region     图标
                                        //横杆左端点，往右边平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_BeginPoint)
                                        {
                                            new_niljY.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15);
                                            new_niljY.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5 - 5);
                                            m_LoadModel.Load_Length = 0;

                                            rePlacePt.X = m_LoadModel.Load_pt.X;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y - 5;

                                        }
                                        //横杆右端点，往左平移5
                                        if (m_LoadModel.Load_pt == m_LineModelList[gan_Num].Line_EndPoint)
                                        {
                                            new_niljY.SetValue(Canvas.LeftProperty, m_LoadModel.Load_pt.X - 15);
                                            new_niljY.SetValue(Canvas.TopProperty, m_LoadModel.Load_pt.Y + 22.5 + 5);
                                            m_LoadModel.Load_Length = m_LineModelList[gan_Num].LineLength;

                                            rePlacePt.X = m_LoadModel.Load_pt.X;
                                            rePlacePt.Y = m_LoadModel.Load_pt.Y + 5;

                                        }
                                        #endregion

                                        #region     标签（有问题）
                                        if (m_LineModelList[gan_Num].Line_BeginPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBY -= 20;
                                        }
                                        if (m_LineModelList[gan_Num].Line_EndPoint == m_HingeJointList[h].J_pt)
                                        {
                                            m_TBY += 20;
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            for (int r = 0; r < m_Rigid_Num; r++)
                            {
                                if (m_RigidJointList[r].J_pt == m_LoadModel.Load_pt)
                                {
                                    ganjian = false;
                                    m_LoadModel.Load_Location = 2;
                                    m_LoadModel.Load_Joint_Num = r;
                                }
                            }
                            if (ganjian == true)
                            {
                                for (int i = 0; i < m_Line_Num; i++)
                                {
                                    if (rePlacePt.X == m_LineModelList[i].Line_BeginPoint.X)
                                    {
                                        if ((rePlacePt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                            && (rePlacePt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                        {
                                            m_LoadModel.Load_Location = 3;
                                            m_LoadModel.Load_Line_Num = i;
                                            m_LoadModel.Load_Length = m_LineModelList[i].Line_BeginPoint.Y - m_LoadModel.Load_pt.Y;
                                        }
                                    }
                                }
                            }

                            #endregion

                            #endregion

                            m_LoadModel.Load_F = 0;
                            m_LoadModel.Load_l = 0;
                            m_LoadModel.Load_q = 0;
                            m_LoadModel.Load_M = double_M;
                            m_LoadModel.Load_Style = 11;

                            tuodong = false;

                            #region     锁定到杆件上
                            for (int i = 0; i < m_Line_Num; i++)
                            {
                                #region     竖杆
                                if ((m_LineModelList[i].Line_BeginPoint.X == m_LoadModel.Load_pt.X)
                                    && (m_LoadModel.Load_pt.Y <= m_LineModelList[i].Line_BeginPoint.Y)
                                    && (m_LoadModel.Load_pt.Y >= m_LineModelList[i].Line_EndPoint.Y))
                                {
                                    tuodong = true;
                                }
                                #endregion
                            }
                            #endregion
                            if (tuodong == true)
                            {
                                m_LoadModelList.Add(m_LoadModel);
                                m_Load_Num = m_Load_Num + 1;

                                #region 力矩标签、图片添加
                                ScaleTransform m_TBscale = new ScaleTransform();
                                m_TBscale.ScaleY = -1;
                                m_TB.RenderTransform = m_TBscale;
                                m_TB.Text = double_M/400/bilichi/bilichi + "";
                                m_TB.Margin = new Thickness(m_TBX, m_TBY, 0, 0);

                                can.Children.Add(new_niljY);
                                can.Children.Add(m_TB);
                                m_Num = m_Num + 1;

                                #endregion
                            }
                            m_LoadModel = new LoadModel();
                            ganjian = true;
                            
                        }
                        if (order != 3)
                        {
                            MessageBox.Show("无效的操作！");
                        }
                        if (order == 2)
                        {
                            MessageBox.Show("请完成约束！");
                        }
                        break;
                    }
                
                
                default:
                    {
                        MessageBox.Show("zz=" + zz);
                        break;
                    }
                    #endregion

                #endregion

                #endregion
            }

        }
        #endregion

        #endregion

        #endregion

        #region     打包

        #region  载荷输入
        private double double_F;
        private double double_l;
        private double double_q;
        private double double_M;

        private void btn_F_ok_Click(object sender, RoutedEventArgs e)
        {
            var textbox_F_test = this.textbox_F.Text;
            try
            {
                double_F = Convert.ToDouble(textbox_F_test);
            }
            catch 
            {
                MessageBox.Show("大小F输入值 非法！");
                double_F = 0;
            }
            MessageBox.Show("F:" + double_F + "");
            double_F = double_F * 20 * bilichi;
        }
        private void btn_q_ok_Click(object sender, RoutedEventArgs e)
        {
            var textbox_l_test = this.textbox_l.Text;
            var textbox_q_test = this.textbox_q.Text;
            try
            {
                double_l = Convert.ToDouble(textbox_l_test);
            }
            catch 
            {
                MessageBox.Show("长度l输入值 非法！");
                double_l = 0;
            }
            try
            {
                double_q = Convert.ToDouble(textbox_q_test);
            }
            catch 
            {
                MessageBox.Show("大小q输入值 非法！");
                double_q = 0;
            }
            MessageBox.Show("l:" + double_l + "" + "\nq:" + double_q + "");
            double_l = double_l * 20 * bilichi;
        }
        private void btn_M_ok_Click(object sender, RoutedEventArgs e)
        {
            var textbox_M_test = this.textbox_M.Text;
            try
            {
                double_M = Convert.ToDouble(textbox_M_test);
            }
            catch
            {
                MessageBox.Show("大小M输入值 非法！");
                double_M = 0;
            }
            MessageBox.Show("M:" + double_M + "");
            double_M = double_M * 400 * bilichi * bilichi;
        }
        
        #endregion

        #region 变量声明
        private double x_area = SystemParameters.WorkArea.Width-200;
        private double y_area = SystemParameters.WorkArea.Height-90;
        /// <summary>
        /// 秩序函数，为了使用户按照一定顺序操作，在操作顺序错误的地方不会破坏数据
        /// 1绘制杆件
        /// 2添加支座
        /// 3施加载荷
        /// 4练习图
        /// 6参考答案图
        /// </summary>
        private int order;
        private DoubleCollection dianhuaxian = new DoubleCollection { 5,2,1,2 };
        private void Paint_ok_Click(object sender, RoutedEventArgs e)
        {
            if(order==0)
            {
                kaishiBt.Background = Brushes.Blue;

                paint = true;
                
                #region     分区线
                m_LineNow = new Line();
                m_LineNow.X1 = x_area / 2;
                m_LineNow.Y1 = y_area / 2;
                m_LineNow.X2 = x_area / 2;
                m_LineNow.Y2 = y_area;
                m_LineNow.StrokeThickness = 3;
                m_LineNow.StrokeDashArray = dianhuaxian;
                m_LineNow.Stroke = Brushes.Red;
                can.Children.Add(m_LineNow);

                m_LineNow = new Line();
                m_LineNow.X1 = 0;
                m_LineNow.Y1 = y_area / 2;
                m_LineNow.X2 = x_area;
                m_LineNow.Y2 = y_area / 2;
                m_LineNow.StrokeThickness = 3;
                m_LineNow.Stroke = Brushes.Red;
                m_LineNow.StrokeDashArray = dianhuaxian;
                can.Children.Add(m_LineNow);

                m_LineNow = new Line();
                m_LineNow.X1 = 0;
                m_LineNow.Y1 = 0;
                m_LineNow.X2 = 0;
                m_LineNow.Y2 = y_area;
                m_LineNow.StrokeThickness = 3;
                m_LineNow.Stroke = Brushes.Red;
                m_LineNow.StrokeDashArray = dianhuaxian;
                can.Children.Add(m_LineNow);

                m_LineNow = new Line();
                m_LineNow.X1 = 0;
                m_LineNow.Y1 = 0;
                m_LineNow.X2 = x_area;
                m_LineNow.Y2 = 0;
                m_LineNow.StrokeThickness = 3;
                m_LineNow.Stroke = Brushes.Red;
                m_LineNow.StrokeDashArray = dianhuaxian;
                can.Children.Add(m_LineNow);

                m_LineNow = new Line();
                m_LineNow.X1 = x_area;
                m_LineNow.Y1 = y_area;
                m_LineNow.X2 = 0;
                m_LineNow.Y2 = y_area;
                m_LineNow.StrokeThickness = 3;
                m_LineNow.Stroke = Brushes.Red;
                m_LineNow.StrokeDashArray = dianhuaxian;
                can.Children.Add(m_LineNow);

                m_LineNow = new Line();
                m_LineNow.X1 = x_area;
                m_LineNow.Y1 = 10;
                m_LineNow.X2 = x_area;
                m_LineNow.Y2 = y_area;
                m_LineNow.StrokeThickness = 3;
                m_LineNow.Stroke = Brushes.Red;
                m_LineNow.StrokeDashArray = dianhuaxian;
                can.Children.Add(m_LineNow);
                #endregion

                #region     标签

                #region     东北大学力学系
                TextBlock f_TB = new TextBlock();
                var f_TBX = x_area / 2 - 60;
                var f_TBY = y_area + 30;
                f_TB.Background = Brushes.White;

                ScaleTransform f_TBscale = new ScaleTransform();
                f_TBscale.ScaleY = -1;
                f_TB.RenderTransform = f_TBscale;
                f_TB.Text = "东北大学力学系";
                f_TB.FontSize = 20;
                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                can.Children.Add(f_TB);
                #endregion

                #region     原图
                TextBlock f_TB1 = new TextBlock();
                var f_TB1X = 0;
                var f_TB1Y = y_area;
                f_TB.Background = Brushes.White;

                ScaleTransform f_TB1scale = new ScaleTransform();
                f_TB1scale.ScaleY = -1;
                f_TB1.RenderTransform = f_TB1scale;
                f_TB1.Text = "原图";
                f_TB1.FontSize = 15;
                f_TB1.Margin = new Thickness(f_TB1X, f_TB1Y, 0, 0);
                can.Children.Add(f_TB1);
                #endregion

                #region     练习图
                TextBlock f_TB2 = new TextBlock();
                var f_TB2X = x_area / 2;
                var f_TB2Y = y_area;
                f_TB2.Background = Brushes.White;

                ScaleTransform f_TB2scale = new ScaleTransform();
                f_TB2scale.ScaleY = -1;
                f_TB2.RenderTransform = f_TB2scale;
                f_TB2.Text = "练习图";
                f_TB2.FontSize = 15;
                f_TB2.Margin = new Thickness(f_TB2X, f_TB2Y, 0, 0);
                can.Children.Add(f_TB2);
                #endregion

                #region     参考答案
                TextBlock f_TB3 = new TextBlock();
                var f_TB3X = 0;
                var f_TB3Y = y_area / 2;
                f_TB3.Background = Brushes.White;

                ScaleTransform f_TB3scale = new ScaleTransform();
                f_TB3scale.ScaleY = -1;
                f_TB3.RenderTransform = f_TB3scale;
                f_TB3.Text = "参考答案";
                f_TB3.FontSize = 15;
                f_TB3.Margin = new Thickness(f_TB3X, f_TB3Y, 0, 0);
                can.Children.Add(f_TB3);
                #endregion
                
                MessageBox.Show("请在画板上左上角区域绘制杆件");

                #endregion

                #region     新建清空
                Button qk = new Button();
                qk.Width = 117;
                qk.Height = 48;
                qk.Content = "清空";
                ScaleTransform f_TBrulerscale = new ScaleTransform();
                f_TBrulerscale.ScaleY = -1;
                qk.RenderTransform = f_TBrulerscale;
                qk.Margin = new Thickness(SystemParameters.WorkArea.Width-320,53, 0, 0);
                can.Children.Add(qk);
                qk.Click += qingkong;
                #endregion
                
                order = 1;
            }
        }
        #endregion 

        #endregion

        #region         弯矩图部分

        #region     系数矩阵

        #region  支座添加完毕，判断刚节点，声明变量

        #region     刚节点
        private KeyPointClass m_KeyPonit = new KeyPointClass();
        private List<KeyPointClass> m_KeyPonitList = new List<KeyPointClass>();
        private JointClass m_RigidJoint = new JointClass();
        private List<JointClass> m_RigidJointList = new List<JointClass>();


        private int m_Rigid_Num;
        private bool rigid = true;
        private bool err = false ;

        #endregion
        private int x_Num = 0;

        private XModelClass x_d = new XModelClass();
        private List<XModelClass> x_dList = new List<XModelClass>();

        private double[,] A;

        private int kelin;

        private int nweishu;
        private double[,] array;
        private double det;

        #endregion

        #region  支座添加完毕，生成系数矩阵
        private void zz_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (order == 2)
                {
                    #region     刚节点
                    for (int i = 0; i < m_Line_Num - 1; i++)
                    {
                        for (int j = i + 1; j < m_Line_Num; j++)
                        {
                            #region   节点情况1
                            if (m_LineModelList[i].Line_BeginPoint == m_LineModelList[j].Line_BeginPoint)
                            {
                                m_KeyPonit.KeyPonit_pt = m_LineModelList[i].Line_BeginPoint;

                                //判断刚节点
                                for (int t = 0; t < m_Hinge_Num; t++)
                                {
                                    if (m_KeyPonit.KeyPonit_pt == m_HingeJointList[t].J_pt)
                                    {
                                        rigid = false;
                                    }
                                }
                                for (int m = 0; m < m_Rigid_Num; m++)
                                {
                                    if (m_KeyPonit.KeyPonit_pt == m_RigidJointList[m].J_pt)
                                    {
                                        rigid = false;
                                    }
                                }
                                if (rigid == true)
                                {
                                    m_RigidJoint.J_pt = m_KeyPonit.KeyPonit_pt;
                                    m_Rigid_Num = m_Rigid_Num + 1;


                                    #region  复刚层数
                                    m_RigidJoint.J_Layer = -1;
                                    for (int n = 0; n < m_Line_Num; n++)
                                    {
                                        if (m_RigidJoint.J_pt == m_LineModelList[n].Line_BeginPoint)
                                        {

                                            m_RigidJoint.J_Layer = m_RigidJoint.J_Layer + 1;

                                        }
                                        else if (m_RigidJoint.J_pt == m_LineModelList[n].Line_EndPoint)
                                        {

                                            m_RigidJoint.J_Layer = m_RigidJoint.J_Layer + 1;

                                        }
                                    }
                                    #endregion

                                    //约束叠加数
                                    yueshu = yueshu + 3 * m_RigidJoint.J_Layer;

                                    m_RigidJointList.Add(m_RigidJoint);
                                    m_RigidJoint = new JointClass();
                                    m_RigidJoint.J_Layer = -1;
                                }
                            }
                            #endregion

                            #region 节点情况2
                            else if (m_LineModelList[i].Line_BeginPoint == m_LineModelList[j].Line_EndPoint)
                            {
                                m_KeyPonit.KeyPonit_pt = m_LineModelList[i].Line_BeginPoint;

                                for (int t = 0; t < m_Hinge_Num; t++)
                                {
                                    if (m_HingeJointList[t].J_pt == m_KeyPonit.KeyPonit_pt)
                                    {
                                        rigid = false;
                                    }
                                }
                                for (int m = 0; m < m_Rigid_Num; m++)
                                {
                                    if (m_KeyPonit.KeyPonit_pt == m_RigidJointList[m].J_pt)
                                    {
                                        rigid = false;
                                    }
                                }
                                if (rigid == true)
                                {
                                    m_RigidJoint.J_pt = m_KeyPonit.KeyPonit_pt;
                                    m_Rigid_Num = m_Rigid_Num + 1;


                                    #region  复刚层数
                                    m_RigidJoint.J_Layer = -1;
                                    for (int n = 0; n < m_Line_Num; n++)
                                    {
                                        if (m_RigidJoint.J_pt == m_LineModelList[n].Line_BeginPoint)
                                        {

                                            m_RigidJoint.J_Layer = m_RigidJoint.J_Layer + 1;

                                        }
                                        else if (m_RigidJoint.J_pt == m_LineModelList[n].Line_EndPoint)
                                        {

                                            m_RigidJoint.J_Layer = m_RigidJoint.J_Layer + 1;

                                        }
                                    }
                                    #endregion

                                    //约束叠加数
                                    yueshu = yueshu + 3 * m_RigidJoint.J_Layer;

                                    m_RigidJointList.Add(m_RigidJoint);
                                    m_RigidJoint = new JointClass();
                                    m_RigidJoint.J_Layer = -1;
                                }

                            }
                            #endregion

                            #region   节点情况3
                            else if (m_LineModelList[i].Line_EndPoint == m_LineModelList[j].Line_BeginPoint)
                            {
                                m_KeyPonit.KeyPonit_pt = m_LineModelList[i].Line_EndPoint;

                                for (int t = 0; t < m_Hinge_Num; t++)
                                {
                                    if (m_HingeJointList[t].J_pt == m_KeyPonit.KeyPonit_pt)
                                    {
                                        rigid = false;
                                    }
                                }
                                for (int m = 0; m < m_Rigid_Num; m++)
                                {
                                    if (m_KeyPonit.KeyPonit_pt == m_RigidJointList[m].J_pt)
                                    {
                                        rigid = false;
                                    }
                                }
                                if (rigid == true)
                                {
                                    m_RigidJoint.J_pt = m_KeyPonit.KeyPonit_pt;
                                    m_Rigid_Num = m_Rigid_Num + 1;



                                    #region  复刚层数
                                    m_RigidJoint.J_Layer = -1;
                                    for (int n = 0; n < m_Line_Num; n++)
                                    {
                                        if (m_RigidJoint.J_pt == m_LineModelList[n].Line_BeginPoint)
                                        {

                                            m_RigidJoint.J_Layer = m_RigidJoint.J_Layer + 1;

                                        }
                                        else if (m_RigidJoint.J_pt == m_LineModelList[n].Line_EndPoint)
                                        {

                                            m_RigidJoint.J_Layer = m_RigidJoint.J_Layer + 1;

                                        }
                                    }
                                    #endregion

                                    //约束叠加数
                                    yueshu = yueshu + 3 * m_RigidJoint.J_Layer;

                                    m_RigidJointList.Add(m_RigidJoint);
                                    m_RigidJoint = new JointClass();
                                    m_RigidJoint.J_Layer = -1;
                                }

                            }
                            #endregion

                            #region 节点情况4
                            else if (m_LineModelList[i].Line_EndPoint == m_LineModelList[j].Line_EndPoint)
                            {
                                m_KeyPonit.KeyPonit_pt = m_LineModelList[i].Line_EndPoint;

                                for (int t = 0; t < m_Hinge_Num; t++)
                                {
                                    if (m_HingeJointList[t].J_pt == m_KeyPonit.KeyPonit_pt)
                                    {
                                        rigid = false;
                                    }
                                }
                                for (int m = 0; m < m_Rigid_Num; m++)
                                {
                                    if (m_KeyPonit.KeyPonit_pt == m_RigidJointList[m].J_pt)
                                    {
                                        rigid = false;
                                    }
                                }
                                if (rigid == true)
                                {
                                    m_RigidJoint.J_pt = m_KeyPonit.KeyPonit_pt;



                                    m_Rigid_Num = m_Rigid_Num + 1;

                                    #region  复刚层数
                                    m_RigidJoint.J_Layer = -1;
                                    for (int n = 0; n < m_Line_Num; n++)
                                    {
                                        if (m_RigidJoint.J_pt == m_LineModelList[n].Line_BeginPoint)
                                        {
                                            m_RigidJoint.J_Layer = m_RigidJoint.J_Layer + 1;
                                        }                                
                                        if (m_RigidJoint.J_pt == m_LineModelList[n].Line_EndPoint)
                                        {
                                            m_RigidJoint.J_Layer = m_RigidJoint.J_Layer + 1;
                                        }
                                    }
                                    #endregion

                                    //约束叠加数
                                    yueshu = yueshu + 3 * m_RigidJoint.J_Layer;

                                    m_RigidJointList.Add(m_RigidJoint);
                                    m_RigidJoint = new JointClass();
                                    m_RigidJoint.J_Layer = -1;
                                }

                            }
                            #endregion

                        }
                    }
                    #endregion

                    #region         支座未知量
                    for (int n = 0; n < m_Zhizuo_Num; n++)
                    {
                        for (int i = 0; i < m_Line_Num; i++)
                        {
                            if (m_ZhizuoList[n].Zhizuo_panduan == 0)
                            {
                                if (m_ZhizuoList[n].Z_Line_Num == i)
                                {

                                    #region     固定支座
                                    if (m_ZhizuoList[n].Zhizuo_Style == 1)
                                    {
                                        //Style 1 水平X；
                                        //2 竖直Y；
                                        //3 力偶

                                        //水平
                                        x_d.X_pt = m_ZhizuoList[n].Zhizuo_pt;
                                        x_d.X = 0;
                                        x_d.X_Dis = m_ZhizuoList[n].Z_Length;
                                        x_d.X_Line_Num = i;
                                        x_d.X_Style = 1;
                                        x_Num++;
                                        x_dList.Add(x_d);
                                        x_d = new XModelClass();

                                        //竖直
                                        x_d.X_pt = m_ZhizuoList[n].Zhizuo_pt;
                                        x_d.X = 0;
                                        x_d.X_Dis = m_ZhizuoList[n].Z_Length;
                                        x_d.X_Line_Num = i;
                                        x_d.X_Style = 2;
                                        x_Num++;
                                        x_dList.Add(x_d);
                                        x_d = new XModelClass();
                                        m_ZhizuoList[n].Zhizuo_panduan = 1;
                                    }
                                    #endregion

                                    #region     活动纵支座
                                    if (m_ZhizuoList[n].Zhizuo_Style == 2)
                                    {
                                        //Style 1 水平X；
                                        //2 竖直Y；
                                        //3 力偶



                                        //竖直
                                        x_d.X_pt = m_ZhizuoList[n].Zhizuo_pt;
                                        x_d.X = 0;
                                        x_d.X_Dis = m_ZhizuoList[n].Z_Length;
                                        x_d.X_Line_Num = i;
                                        x_d.X_Style = 2;
                                        x_Num++;
                                        x_dList.Add(x_d);
                                        x_d = new XModelClass();

                                        m_ZhizuoList[n].Zhizuo_panduan = 1;
                                    }
                                    #endregion

                                    #region     活动横支座
                                    if (m_ZhizuoList[n].Zhizuo_Style == 3)
                                    {
                                        //Style 1 水平X；
                                        //2 竖直Y；
                                        //3 力偶

                                        //水平
                                        x_d.X_pt = m_ZhizuoList[n].Zhizuo_pt;
                                        x_d.X = 0;
                                        x_d.X_Dis = m_ZhizuoList[n].Z_Length;
                                        x_d.X_Line_Num = i;
                                        x_d.X_Style = 1;
                                        x_Num++;
                                        x_dList.Add(x_d);
                                        x_d = new XModelClass();

                                        m_ZhizuoList[n].Zhizuo_panduan = 1;
                                    }
                                    #endregion

                                    #region     固定端
                                    if (m_ZhizuoList[n].Zhizuo_Style == 4)
                                    {
                                        //Style 1 水平X；
                                        //2 竖直Y；
                                        //3 力偶

                                        //水平
                                        x_d.X_pt = m_ZhizuoList[n].Zhizuo_pt;
                                        x_d.X = 0;
                                        x_d.X_Dis = m_ZhizuoList[n].Z_Length;
                                        x_d.X_Line_Num = i;
                                        x_d.X_Style = 1;
                                        x_Num++;
                                        x_dList.Add(x_d);
                                        x_d = new XModelClass();

                                        //竖直
                                        x_d.X_pt = m_ZhizuoList[n].Zhizuo_pt;
                                        x_d.X = 0;
                                        x_d.X_Dis = m_ZhizuoList[n].Z_Length;
                                        x_d.X_Line_Num = i;
                                        x_d.X_Style = 2;
                                        x_Num++;
                                        x_dList.Add(x_d);
                                        x_d = new XModelClass();

                                        //力偶
                                        x_d.X_pt = m_ZhizuoList[n].Zhizuo_pt;
                                        x_d.X = 0;
                                        x_d.X_Dis = m_ZhizuoList[n].Z_Length;
                                        x_d.X_Line_Num = i;
                                        x_d.X_Style = 3;
                                        x_Num++;
                                        x_dList.Add(x_d);
                                        x_d = new XModelClass();

                                        m_ZhizuoList[n].Zhizuo_panduan = 1;
                                    }
                                    #endregion

                                    #region     滑动横支座
                                    if (m_ZhizuoList[n].Zhizuo_Style == 5)
                                    {
                                        //Style 1 水平X；
                                        //2 竖直Y；
                                        //3 力偶

                                        //水平
                                        x_d.X_pt = m_ZhizuoList[n].Zhizuo_pt;
                                        x_d.X = 0;
                                        x_d.X_Dis = m_ZhizuoList[n].Z_Length;
                                        x_d.X_Line_Num = i;
                                        x_d.X_Style = 1;
                                        x_Num++;
                                        x_dList.Add(x_d);
                                        x_d = new XModelClass();

                                        //力偶
                                        x_d.X_pt = m_ZhizuoList[n].Zhizuo_pt;
                                        x_d.X = 0;
                                        x_d.X_Dis = m_ZhizuoList[n].Z_Length;
                                        x_d.X_Line_Num = i;
                                        x_d.X_Style = 3;
                                        x_Num++;
                                        x_dList.Add(x_d);
                                        x_d = new XModelClass();

                                        m_ZhizuoList[n].Zhizuo_panduan = 1;
                                    }
                                    #endregion

                                    #region     滑动纵支座
                                    if (m_ZhizuoList[n].Zhizuo_Style == 6)
                                    {
                                        //Style 1 水平X；
                                        //2 竖直Y；
                                        //3 力偶

                                        //水平
                                        x_d.X_pt = m_ZhizuoList[n].Zhizuo_pt;
                                        x_d.X = 0;
                                        x_d.X_Dis = m_ZhizuoList[n].Z_Length;
                                        x_d.X_Line_Num = i;
                                        x_d.X_Style = 2;
                                        x_Num++;
                                        x_dList.Add(x_d);
                                        x_d = new XModelClass();

                                        //力偶
                                        x_d.X_pt = m_ZhizuoList[n].Zhizuo_pt;
                                        x_d.X = 0;
                                        x_d.X_Dis = m_ZhizuoList[n].Z_Length;
                                        x_d.X_Line_Num = i;
                                        x_d.X_Style = 3;
                                        x_Num++;
                                        x_dList.Add(x_d);
                                        x_d = new XModelClass();

                                        m_ZhizuoList[n].Zhizuo_panduan = 1;
                                    }
                                    #endregion


                                }
                            }
                        }
                    }
                    #endregion

                    #region     铰节点未知量
                    for (int h = 0; h < m_Hinge_Num; h++)
                    {
                        for (int i = 0; i < m_Line_Num; i++)
                        {
                            if (m_LineModelList[i].Line_BeginPoint == m_HingeJointList[h].J_pt)
                            {
                                //Style 1 水平X；
                                //2 竖直Y；
                                //3 力偶
                                x_d.X_pt = m_HingeJointList[h].J_pt;
                                x_d.X_Dis = 0;
                                x_d.X_Line_Num = i;
                                x_d.X_Style = 1;
                                x_d.X = 0;
                                x_d.X_Joint_Style = 1;
                                x_d.X_Joint_Num = h;
                                x_Num++;
                                x_dList.Add(x_d);
                                x_d = new XModelClass();

                                x_d.X_pt = m_HingeJointList[h].J_pt;
                                x_d.X_Dis = 0;
                                x_d.X_Line_Num = i;
                                x_d.X_Style = 2;
                                x_d.X = 0;
                                x_d.X_Joint_Style = 1;
                                x_d.X_Joint_Num = h;
                                x_Num++;
                                x_dList.Add(x_d);
                                x_d = new XModelClass();
                                kelin++;
                            }
                            else if (m_LineModelList[i].Line_EndPoint == m_HingeJointList[h].J_pt)
                            {
                                //Style 1 水平X；
                                //2 竖直Y；
                                //3 力偶
                                x_d.X_pt = m_HingeJointList[h].J_pt;
                                x_d.X_Dis = m_LineModelList[i].LineLength;
                                x_d.X_Line_Num = i;
                                x_d.X_Style = 1;
                                x_d.X = 0;
                                x_d.X_Joint_Style = 1;
                                x_d.X_Joint_Num = h;
                                x_Num++;
                                x_dList.Add(x_d);
                                x_d = new XModelClass();

                                x_d.X_pt = m_HingeJointList[h].J_pt;
                                x_d.X_Dis = m_LineModelList[i].LineLength;
                                x_d.X_Line_Num = i;
                                x_d.X_Style = 2;
                                x_d.X = 0;
                                x_d.X_Joint_Style = 1;
                                x_d.X_Joint_Num = h;
                                x_Num++;
                                x_dList.Add(x_d);
                                x_d = new XModelClass();
                            }
                        }
                    }

                    #endregion

                    #region         刚节点未知量

                    for (int r = 0; r < m_Rigid_Num; r++)
                    {
                        for (int i = 0; i < m_Line_Num; i++)
                        {
                            if (m_LineModelList[i].Line_BeginPoint == m_RigidJointList[r].J_pt)
                            {
                                x_d.X_pt = m_RigidJointList[r].J_pt;
                                x_d.X_Dis = 0;
                                x_d.X_Line_Num = i;
                                x_d.X_Style = 1;
                                x_d.X = 0;
                                x_d.X_Joint_Style = 2;
                                x_d.X_Joint_Num = r;
                                x_Num++;
                                x_dList.Add(x_d);
                                x_d = new XModelClass();

                                x_d.X_pt = m_RigidJointList[r].J_pt;
                                x_d.X_Dis = 0;
                                x_d.X_Line_Num = i;
                                x_d.X_Style = 2;
                                x_d.X = 0;
                                x_d.X_Joint_Style = 2;
                                x_d.X_Joint_Num = r;
                                x_Num++;
                                x_dList.Add(x_d);
                                x_d = new XModelClass();

                                x_d.X_pt = m_RigidJointList[r].J_pt;
                                x_d.X_Dis = 0;
                                x_d.X_Line_Num = i;
                                x_d.X_Style = 3;
                                x_d.X = 0;
                                x_d.X_Joint_Style = 2;
                                x_d.X_Joint_Num = r;
                                x_Num++;
                                x_dList.Add(x_d);
                                x_d = new XModelClass();
                            }
                            if (m_LineModelList[i].Line_EndPoint == m_RigidJointList[r].J_pt)
                            {
                                x_d.X_pt = m_RigidJointList[r].J_pt;
                                x_d.X_Dis = m_LineModelList[i].LineLength;
                                x_d.X_Line_Num = i;
                                x_d.X_Style = 1;
                                x_d.X = 0;
                                x_d.X_Joint_Style = 2;
                                x_d.X_Joint_Num = r;
                                x_Num++;
                                x_dList.Add(x_d);
                                x_d = new XModelClass();

                                x_d.X_pt = m_RigidJointList[r].J_pt;
                                x_d.X_Dis = m_LineModelList[i].LineLength;
                                x_d.X_Line_Num = i;
                                x_d.X_Style = 2;
                                x_d.X = 0;
                                x_d.X_Joint_Style = 2;
                                x_d.X_Joint_Num = r;
                                x_Num++;
                                x_dList.Add(x_d);
                                x_d = new XModelClass();

                                x_d.X_pt = m_RigidJointList[r].J_pt;
                                x_d.X_Dis = m_LineModelList[i].LineLength;
                                x_d.X_Line_Num = i;
                                x_d.X_Style = 3;
                                x_d.X = 0;
                                x_d.X_Joint_Style = 2;
                                x_d.X_Joint_Num = r;
                                x_Num++;
                                x_dList.Add(x_d);
                                x_d = new XModelClass();
                            }
                        }

                    }
                    #endregion

                    #region     自由度判断

                    if (3 * m_Line_Num > yueshu)
                    {
                        MessageBox.Show("自由度大于零，可动机构。请继续添加约束或清空后重新绘制结构！");
                        err = true;
                    }
                    if (3 * m_Line_Num < yueshu)
                    {
                        MessageBox.Show("自由度小于零，超静定结构。请清空后重新绘制结构！");
                        this.menuTabctrl.SelectedItem = this.menuTabctrl.Items[0];
                        err = true;
                    }
                    if (3 * m_Line_Num == yueshu)
                    {
                        err = false;
                    }
                    #endregion

                    #region     静定判断

                    if (err == false)
                    {
                        #region     系数矩阵

                        #region   系数矩阵建立
                        A = new double[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * m_Rigid_Num, 3 * m_Line_Num + 2 * m_Hinge_Num + 3 * m_Rigid_Num];

                        #endregion
                        
                        #region 杆件和铰节点部分

                        for (int i = 0; i < m_Line_Num; i++)
                        {

                            for (int b = 0; b < x_Num; b++)
                            {
                                //如果第b个未知量在第i根杆件上
                                if (x_dList[b].X_Line_Num == i)
                                {
                                    #region     判断铰节点、刚节点是否与未知量位置重叠
                                    //铰节点
                                    for (int h = 0; h < m_Hinge_Num; h++)
                                    {
                                        if (x_dList[b].X_Joint_Style == 0)
                                        {
                                            if (x_dList[b].X_pt == m_HingeJointList[h].J_pt)
                                            {
                                                x_dList[b].X_zhizuo = 1;
                                                x_dList[b].X_Joint_Num = h;
                                            }
                                        }
                                    }

                                    //刚节点
                                    for (int r = 0; r < m_Rigid_Num; r++)
                                    {
                                        if (x_dList[b].X_Joint_Style == 0)
                                        {
                                            if (x_dList[b].X_pt == m_RigidJointList[r].J_pt)
                                            {
                                                x_dList[b].X_zhizuo = 2;
                                                x_dList[b].X_Joint_Num = r;
                                            }
                                        }

                                    }
                                    #endregion

                                    #region         支座
                                    //如果未知量是X方向的未知量
                                    if (x_dList[b].X_Style == 1)
                                    {
                                        // 水平杆件
                                        if (m_LineModelList[i].Line_Style == 0)
                                        {
                                            if (x_dList[b].X_zhizuo == 0)
                                            {
                                                A[3 * i, b] = 1;
                                            }

                                        }
                                        //竖直杆件
                                        if (m_LineModelList[i].Line_Style == 1)
                                        {
                                            if (x_dList[b].X_zhizuo == 0)
                                            {
                                                A[3 * i, b] = 1;
                                                A[3 * i + 2, b] = x_dList[b].X_Dis;
                                            }
                                        }
                                    }

                                    //如果未知量是Y方向的未知量
                                    if (x_dList[b].X_Style == 2)
                                    {
                                        // 水平杆件
                                        if (m_LineModelList[i].Line_Style == 0)
                                        {
                                            if (x_dList[b].X_zhizuo == 0)
                                            {
                                                A[3 * i + 1, b] = 1;
                                                A[3 * i + 2, b] = x_dList[b].X_Dis;
                                            }

                                        }
                                        //竖直杆件
                                        if (m_LineModelList[i].Line_Style == 1)
                                        {
                                            if (x_dList[b].X_zhizuo == 0)
                                            {
                                                A[3 * i + 1, b] = 1;
                                            }

                                        }
                                    }

                                    //如果未知量是力偶M的未知量
                                    if (x_dList[b].X_Style == 3)
                                    {
                                        if (x_dList[b].X_zhizuo == 0)
                                        {
                                            A[3 * i + 2, b] = 1;
                                            kelin++;
                                        }
                                    }
                                    #endregion

                                }
                            }
                            
                            #region     铰节点

                            for (int b = 0; b < x_Num; b++)
                            {
                                if (x_dList[b].X_Joint_Style == 0)
                                {
                                    if (x_dList[b].X_zhizuo == 1)
                                    {
                                        int h = x_dList[b].X_Joint_Num;
                                        if (x_dList[b].X_Style == 1)
                                        {
                                            A[3 * m_Line_Num + 2 * h, b] = -1;
                                        }
                                        if (x_dList[b].X_Style == 2)
                                        {
                                            A[3 * m_Line_Num + 2 * h + 1, b] = -1;
                                        }
                                        if (x_dList[b].X_Style == 3)
                                        {
                                            A[3 * m_Line_Num + 2 * h + 2, b] = -1;
                                        }
                                    }
                                }

                                if (x_dList[b].X_Joint_Style == 1)
                                {
                                    int h = x_dList[b].X_Joint_Num;
                                    if (x_dList[b].X_Style == 1)
                                    {
                                        A[3 * m_Line_Num + 2 * h, b] = -1;
                                    }
                                    if (x_dList[b].X_Style == 2)
                                    {
                                        A[3 * m_Line_Num + 2 * h + 1, b] = -1;
                                    }
                                }
                            }
                        
                            #endregion
                        }

                        #endregion

                        #region    刚节点

                        for (int b = 0; b < x_Num; b++)
                        {
                            #region     支座在刚节点处
                            if (x_dList[b].X_Joint_Style == 0)
                            {
                                if (x_dList[b].X_zhizuo == 2)
                                {
                                    int r = x_dList[b].X_Joint_Num;
                                    if (x_dList[b].X_Style == 1)
                                    {
                                        A[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r, b] = 1;
                                    }
                                    if (x_dList[b].X_Style == 2)
                                    {
                                        A[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 1, b] = 1;
                                    }
                                    if (x_dList[b].X_Style == 3)
                                    {
                                        A[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2, b] = 1;
                                    }
                                }
                            }
                            #endregion

                            #region     刚节点处
                            if (x_dList[b].X_Joint_Style == 2)
                            {
                                int r = x_dList[b].X_Joint_Num;
                                if (x_dList[b].X_Style == 1)
                                {
                                    A[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r, b] = -1;
                                }
                                if (x_dList[b].X_Style == 2)
                                {
                                    A[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 1, b] = -1;
                                }
                                if (x_dList[b].X_Style == 3)
                                {
                                    A[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2, b] = -1;
                                }
                            }
                            #endregion
                        }
                        #endregion
                        
                        #endregion

                        #region     判断是否静定
                        nweishu = 3 * m_Line_Num + 2 * m_Hinge_Num + 3 * m_Rigid_Num;
                        array = new double[nweishu, nweishu];
                        for (int i = 0; i < nweishu; i++)
                        {
                            for (int j = 0; j < nweishu; j++)
                            {
                                array[i, j] = A[i, j];
                            }
                        }

                        Matrix<double> mA = DenseMatrix.OfArray(array);
                        det = mA.Determinant();

                        if (det == 0)
                        {
                            MessageBox.Show("非静定结构，请清空后重新绘制结构！");
                            this.menuTabctrl.SelectedItem = this.menuTabctrl.Items[0];
                            err = true;
                        }
                        if (det != 0)
                        {
                            MessageBox.Show("静定结构，可以继续添加载荷。");
                            this.menuTabctrl.SelectedItem = this.menuTabctrl.Items[2];
                            err = false;
                        }
                        #endregion
                    }
                    #endregion

                    order = 3;
                }
                if (order != 2)
                {
                    if (order != 3)
                        MessageBox.Show("无效的操作！");
                }
            }
            catch
            {
                MessageBox.Show("原结构存在错误，未能成功生成系数矩阵");
            }
        }
        #endregion 

        #endregion

        #region     右端项和弯矩图数据
        double[,] b;
        double[] vB;
        int tbr = 1;
        double xishu = 1;
        private QuXianClass quxian3 = new QuXianClass();
        private List<QuXianClass> quxian3List = new List<QuXianClass>();

        private void btn_sure_Click(object sender, RoutedEventArgs e)
        {
            if (order == 3)
            {
                b = new double[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * m_Rigid_Num, 1];
                vB = new double[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * m_Rigid_Num];
                try
                {
                    #region     将载荷转换成右端项
                    for (int d = 0; d < m_Load_Num; d++)
                    {
                        #region     载荷在铰节点上
                        if (m_LoadModelList[d].Load_Location == 1)
                        {
                            #region     上下集中力
                            if (m_LoadModelList[d].Load_Style == 1)
                            {
                                int h = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * h + 1, 0] = b[3 * m_Line_Num + 2 * h + 1, 0] - m_LoadModelList[d].Load_F;
                                vB[3 * m_Line_Num + 2 * h + 1] = vB[3 * m_Line_Num + 2 * h + 1] - m_LoadModelList[d].Load_F;
                            }
                            if (m_LoadModelList[d].Load_Style == 2)
                            {
                                int h = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * h + 1, 0] = b[3 * m_Line_Num + 2 * h + 1, 0] - m_LoadModelList[d].Load_F;
                                vB[3 * m_Line_Num + 2 * h + 1] = vB[3 * m_Line_Num + 2 * h + 1] - m_LoadModelList[d].Load_F;

                            }
                            #endregion

                            #region     左右集中力
                            if (m_LoadModelList[d].Load_Style == 3)
                            {
                                int h = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * h, 0] = b[3 * m_Line_Num + 2 * h, 0] - m_LoadModelList[d].Load_F;
                                vB[3 * m_Line_Num + 2 * h] = vB[3 * m_Line_Num + 2 * h] - m_LoadModelList[d].Load_F;

                            }

                            if (m_LoadModelList[d].Load_Style == 4)
                            {
                                int h = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * h, 0] = b[3 * m_Line_Num + 2 * h, 0] - m_LoadModelList[d].Load_F;
                                vB[3 * m_Line_Num + 2 * h] = vB[3 * m_Line_Num + 2 * h] - m_LoadModelList[d].Load_F;

                            }
                            #endregion

                            #region     力偶
                            if (m_LoadModelList[d].Load_Style == 9)
                            {
                                int h = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * h + 2, 0] = b[3 * m_Line_Num + 2 * h + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * m_Line_Num + 2 * h + 2] = vB[3 * m_Line_Num + 2 * h + 2] - m_LoadModelList[d].Load_M;

                            }
                            if (m_LoadModelList[d].Load_Style == 10)
                            {
                                int h = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * h + 2, 0] = b[3 * m_Line_Num + 2 * h + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * m_Line_Num + 2 * h + 2] = vB[3 * m_Line_Num + 2 * h + 2] - m_LoadModelList[d].Load_M;

                            }
                            if (m_LoadModelList[d].Load_Style == 11)
                            {
                                int h = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * h + 2, 0] = b[3 * m_Line_Num + 2 * h + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * m_Line_Num + 2 * h + 2] = vB[3 * m_Line_Num + 2 * h + 2] - m_LoadModelList[d].Load_M;

                            }
                            if (m_LoadModelList[d].Load_Style == 12)
                            {
                                int h = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * h + 2, 0] = b[3 * m_Line_Num + 2 * h + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * m_Line_Num + 2 * h + 2] = vB[3 * m_Line_Num + 2 * h + 2] - m_LoadModelList[d].Load_M;

                            }
                            #endregion
                        }
                        #endregion

                        #region     载荷在刚节点上
                        if (m_LoadModelList[d].Load_Location == 2)
                        {
                            #region     上下集中力
                            if (m_LoadModelList[d].Load_Style == 1)
                            {
                                int r = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 1, 0] = b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 1, 0] - m_LoadModelList[d].Load_F;
                                vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 1] = vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 1] - m_LoadModelList[d].Load_F;

                            }
                            if (m_LoadModelList[d].Load_Style == 2)
                            {
                                int r = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 1, 0] = b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 1, 0] - m_LoadModelList[d].Load_F;
                                vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 1] = vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 1] - m_LoadModelList[d].Load_F;

                            }
                            #endregion

                            #region     左右集中力
                            if (m_LoadModelList[d].Load_Style == 3)
                            {
                                int r = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r, 0] = b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r, 0] - m_LoadModelList[d].Load_F;
                                vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r] = vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r] - m_LoadModelList[d].Load_F;

                            }

                            if (m_LoadModelList[d].Load_Style == 4)
                            {
                                int r = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r, 0] = b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r, 0] - m_LoadModelList[d].Load_F;
                                vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r] = vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r] - m_LoadModelList[d].Load_F;

                            }
                            #endregion

                            #region     力偶
                            if (m_LoadModelList[d].Load_Style == 9)
                            {
                                int r = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2, 0] = b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2] = vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2] - m_LoadModelList[d].Load_M;

                            }
                            if (m_LoadModelList[d].Load_Style == 10)
                            {
                                int r = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2, 0] = b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2] = vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2] - m_LoadModelList[d].Load_M;

                            }
                            if (m_LoadModelList[d].Load_Style == 11)
                            {
                                int r = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2, 0] = b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2] = vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2] - m_LoadModelList[d].Load_M;

                            }
                            if (m_LoadModelList[d].Load_Style == 12)
                            {
                                int r = m_LoadModelList[d].Load_Joint_Num;
                                b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2, 0] = b[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2] = vB[3 * m_Line_Num + 2 * m_Hinge_Num + 3 * r + 2] - m_LoadModelList[d].Load_M;

                            }
                            #endregion
                        }
                        #endregion

                        #region     载荷在杆件上
                        if (m_LoadModelList[d].Load_Location == 3)
                        {
                            #region     上下集中力
                            if (m_LoadModelList[d].Load_Style == 1)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i + 1, 0] = b[3 * i + 1, 0] - m_LoadModelList[d].Load_F;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_F * m_LoadModelList[d].Load_Length;
                                vB[3 * i + 1] = vB[3 * i + 1] - m_LoadModelList[d].Load_F;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_F * m_LoadModelList[d].Load_Length;

                            }
                            if (m_LoadModelList[d].Load_Style == 2)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i + 1, 0] = b[3 * i + 1, 0] - m_LoadModelList[d].Load_F;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_F * m_LoadModelList[d].Load_Length;
                                vB[3 * i + 1] = vB[3 * i + 1] - m_LoadModelList[d].Load_F;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_F * m_LoadModelList[d].Load_Length;

                            }
                            #endregion

                            #region     左右集中力
                            if (m_LoadModelList[d].Load_Style == 3)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i, 0] = b[3 * i, 0] - m_LoadModelList[d].Load_F;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_F * m_LoadModelList[d].Load_Length;
                                vB[3 * i] = vB[3 * i] - m_LoadModelList[d].Load_F;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_F * m_LoadModelList[d].Load_Length;

                            }

                            if (m_LoadModelList[d].Load_Style == 4)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i, 0] = b[3 * i, 0] - m_LoadModelList[d].Load_F;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_F * m_LoadModelList[d].Load_Length;
                                vB[3 * i] = vB[3 * i] - m_LoadModelList[d].Load_F;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_F * m_LoadModelList[d].Load_Length;

                            }
                            #endregion

                            #region     左右均布载荷
                            if (m_LoadModelList[d].Load_Style == 5)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i, 0] = b[3 * i, 0] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q * m_LoadModelList[d].Load_Length;
                                vB[3 * i] = vB[3 * i] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q * m_LoadModelList[d].Load_Length;

                            }
                            if (m_LoadModelList[d].Load_Style == 6)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i, 0] = b[3 * i, 0] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q * m_LoadModelList[d].Load_Length;
                                vB[3 * i] = vB[3 * i] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q * m_LoadModelList[d].Load_Length;

                            }
                            #endregion

                            #region     上下均布载荷
                            if (m_LoadModelList[d].Load_Style == 7)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i + 1, 0] = b[3 * i + 1, 0] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q * m_LoadModelList[d].Load_Length;
                                vB[3 * i + 1] = vB[3 * i + 1] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q * m_LoadModelList[d].Load_Length;

                            }
                            if (m_LoadModelList[d].Load_Style == 8)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i + 1, 0] = b[3 * i + 1, 0] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q * m_LoadModelList[d].Load_Length;
                                vB[3 * i + 1] = vB[3 * i + 1] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_l * m_LoadModelList[d].Load_q * m_LoadModelList[d].Load_Length;

                            }
                            #endregion

                            #region     力偶
                            if (m_LoadModelList[d].Load_Style == 9)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_M;
                            }
                            if (m_LoadModelList[d].Load_Style == 10)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_M;
                            }
                            if (m_LoadModelList[d].Load_Style == 11)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_M;
                            }
                            if (m_LoadModelList[d].Load_Style == 12)
                            {
                                int i = m_LoadModelList[d].Load_Line_Num;
                                b[3 * i + 2, 0] = b[3 * i + 2, 0] - m_LoadModelList[d].Load_M;
                                vB[3 * i + 2] = vB[3 * i + 2] - m_LoadModelList[d].Load_M;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    this.menuTabctrl.SelectedItem = this.menuTabctrl.Items[3];
                }
                catch
                {
                    MessageBox.Show("程序错误，未能将载荷转换成右端项");
                }

                try
                {
                    #region     弯矩图数据

                    #region     支座反力

                    n = x_Num;
                    Di = new MatrixCls(n, n);
                    Ci = new double[n, n];
                    Ci = Di.Detail;
                    var matrixA = DenseMatrix.OfArray(array);
                    var vectorB = new DenseVector(vB);
                    var resultX = matrixA.LU().Solve(vectorB);

                    for (int z = 0; z < n; z++)
                    {
                        dxList.Add(resultX[z]);
                    }

                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            Ci[i, j] = A[i, j];
                        }
                    }
                    #endregion

                    #region     将未知量赋值
                    for (int i = 0; i < x_Num; i++)
                    {
                        x_dList[i].X = dxList[i];
                    }
                    #endregion

                    #region     将支座反力输入载荷list
                    for (int i = 0; i < x_Num; i++)
                    {
                        #region     在杆件上

                        n_LoadModel.jiemian = 3;
                        n_LoadModel.Load_pt = x_dList[i].X_pt;
                        n_LoadModel.Load_Line_Num = x_dList[i].X_Line_Num;
                        n_LoadModel.Load_Length = x_dList[i].X_Dis;
                        n_LoadModel.Load_Location = 3;
                        n_Load_Num++;

                        #region     X方向力
                        if (x_dList[i].X_Style == 1)
                        {
                            n_LoadModel.Load_Style = 3;
                            n_LoadModel.Load_F = x_dList[i].X;
                            n_LoadModelList.Add(n_LoadModel);
                            n_LoadModel = new LoadModel();

                        }
                        #endregion

                        #region     Y方向力
                        if (x_dList[i].X_Style == 2)
                        {
                            n_LoadModel.Load_Style = 1;
                            n_LoadModel.Load_F = x_dList[i].X;
                            n_LoadModelList.Add(n_LoadModel);
                            n_LoadModel = new LoadModel();
                        }
                        #endregion

                        #region     集中力偶
                        if (x_dList[i].X_Style == 3)
                        {
                            n_LoadModel.Load_M = x_dList[i].X;
                            int j = n_LoadModel.Load_Line_Num;
                            if (m_LineModelList[j].Line_Style == 0)
                            {
                                n_LoadModel.Load_Style = 9;
                                n_LoadModelList.Add(n_LoadModel);
                                n_LoadModel = new LoadModel();
                            }
                            if (m_LineModelList[j].Line_Style == 1)
                            {
                                n_LoadModel.Load_Style = 11;
                                n_LoadModelList.Add(n_LoadModel);
                                n_LoadModel = new LoadModel();
                            }
                        }
                        #endregion

                        #endregion
                    }
                    #endregion

                    #region     只分布在杆件上的载荷输入list
                    for (int i = 0; i < m_Load_Num; i++)
                    {
                        if (m_LoadModelList[i].Load_Location == 3)
                        {
                            #region     集中力与集中力偶
                            if (m_LoadModelList[i].Load_q == 0)
                            {
                                if (m_LoadModelList[i].Load_M == 0)
                                {
                                    n_LoadModel.jiemian = 3;
                                    n_LoadModel = m_LoadModelList[i];
                                    n_LoadModel.Load_Bend = n_LoadModel.Load_pt;
                                    n_LoadModelList.Add(n_LoadModel);
                                    n_LoadModel = new LoadModel();
                                    n_Load_Num++;
                                }
                                if (m_LoadModelList[i].Load_M != 0)
                                {
                                    n_LoadModel = new LoadModel();
                                    n_LoadModel.jiemian = 3;
                                    n_LoadModel.Load_Bend = m_LoadModelList[i].Load_pt;
                                    n_LoadModel.Load_Length = m_LoadModelList[i].Load_Length;
                                    n_LoadModel.Load_Line_Num = m_LoadModelList[i].Load_Line_Num;
                                    n_LoadModel.Load_Location = m_LoadModelList[i].Load_Location;
                                    n_LoadModel.Load_M = 0;
                                    n_LoadModel.Load_pt = m_LoadModelList[i].Load_pt;
                                    n_LoadModel.Load_Style = m_LoadModelList[i].Load_Style;
                                    n_LoadModelList.Add(n_LoadModel);

                                    n_Load_Num++;
                                    n_LoadModel = new LoadModel();
                                    n_LoadModel.jiemian = 3;
                                    n_LoadModel.Load_Bend = m_LoadModelList[i].Load_pt;
                                    n_LoadModel.Load_Length = m_LoadModelList[i].Load_Length - 0.01;
                                    n_LoadModel.Load_Line_Num = m_LoadModelList[i].Load_Line_Num;
                                    n_LoadModel.Load_Location = m_LoadModelList[i].Load_Location;
                                    n_LoadModel.Load_M = m_LoadModelList[i].Load_M;
                                    n_LoadModel.Load_pt = m_LoadModelList[i].Load_pt;
                                    n_LoadModel.Load_Style = m_LoadModelList[i].Load_Style;
                                    if (n_LoadModel.Load_Style == 9)
                                    {
                                        n_LoadModel.Load_pt.X -= 0.1;
                                    }
                                    if (n_LoadModel.Load_Style == 11)
                                    {
                                        n_LoadModel.Load_pt.Y += 0.1;
                                    }
                                    n_LoadModel.Load_Bend = n_LoadModel.Load_pt;
                                    n_LoadModelList.Add(n_LoadModel);
                                    n_Load_Num++;
                                }
                            }
                            #endregion

                            #region     均布载荷的处理
                            if (m_LoadModelList[i].Load_q != 0)
                            {
                                #region     处理均布载荷需要使用的变量
                                double q = m_LoadModelList[i].Load_q;
                                double junbu = m_LoadModelList[i].Load_l / 2;
                                int line_Num = m_LoadModelList[i].Load_Line_Num;
                                double lenth = m_LoadModelList[i].Load_Length - m_LoadModelList[i].Load_l / 2;
                                double x = m_LoadModelList[i].Load_pt.X;
                                double y = m_LoadModelList[i].Load_pt.Y;

                                #endregion

                                #region     横杆上的均布载荷
                                if ((m_LoadModelList[i].Load_Style == 7) || (m_LoadModelList[i].Load_Style == 8))
                                {
                                    #region     第一个
                                    n_LoadModel = new LoadModel();
                                    n_LoadModel.jiemian = 4;
                                    n_LoadModel.Load_F = q;
                                    n_LoadModel.Load_Style = 1;
                                    n_LoadModel.Load_Line_Num = line_Num;
                                    n_LoadModel.Load_Length = lenth;
                                    n_LoadModel.Load_pt.X = x;
                                    n_LoadModel.Load_pt.Y = y;

                                    //存储
                                    n_LoadModelList.Add(n_LoadModel);
                                    n_Load_Num++;
                                    #endregion

                                    #region     循环
                                    for (int j = 1; j < junbu; j++)
                                    {
                                        n_LoadModel = new LoadModel();
                                        n_LoadModel.Load_F = 2 * q;
                                        n_LoadModel.Load_Style = 1;
                                        n_LoadModel.Load_Line_Num = line_Num;
                                        n_LoadModel.Load_Length = lenth + 2 * j;
                                        n_LoadModel.Load_pt.X = x + j * 2;
                                        n_LoadModel.Load_pt.Y = y;

                                        //存储
                                        n_LoadModelList.Add(n_LoadModel);
                                        n_LoadModel = new LoadModel();
                                        n_Load_Num++;
                                    }
                                    #endregion

                                    #region     最后一个
                                    n_LoadModel = new LoadModel();
                                    n_LoadModel.jiemian = 5;
                                    n_LoadModel.Load_F = q;
                                    n_LoadModel.Load_Style = 1;
                                    n_LoadModel.Load_Line_Num = line_Num;
                                    n_LoadModel.Load_Length = lenth + 2 * junbu;
                                    n_LoadModel.Load_pt.X = x + 2 * junbu;
                                    n_LoadModel.Load_pt.Y = y;
                                    n_LoadModelList.Add(n_LoadModel);
                                    n_Load_Num++;
                                    #endregion

                                }
                                #endregion

                                #region     竖杆上的均布载荷
                                if ((m_LoadModelList[i].Load_Style == 5) || (m_LoadModelList[i].Load_Style == 6))
                                {
                                    #region     第一个
                                    n_LoadModel = new LoadModel();
                                    n_LoadModel.jiemian = 4;
                                    n_LoadModel.Load_F = q;
                                    n_LoadModel.Load_Style = 3;
                                    n_LoadModel.Load_Line_Num = line_Num;
                                    n_LoadModel.Load_Length = lenth;
                                    n_LoadModel.Load_pt.X = x;
                                    n_LoadModel.Load_pt.Y = y;

                                    //存储
                                    n_LoadModelList.Add(n_LoadModel);
                                    n_Load_Num++;
                                    #endregion

                                    #region     循环
                                    for (int j = 1; j < junbu; j++)
                                    {
                                        n_LoadModel = new LoadModel();
                                        n_LoadModel.Load_F = 2 * q;
                                        n_LoadModel.Load_Style = 3;
                                        n_LoadModel.Load_Line_Num = line_Num;
                                        n_LoadModel.Load_Length = lenth + 2 * j;
                                        n_LoadModel.Load_pt.X = x;
                                        n_LoadModel.Load_pt.Y = y - 2 * j;

                                        //存储
                                        n_LoadModelList.Add(n_LoadModel);
                                        n_LoadModel = new LoadModel();
                                        n_Load_Num++;
                                    }
                                    #endregion

                                    #region     最后一个
                                    n_LoadModel = new LoadModel();
                                    n_LoadModel.jiemian = 5;
                                    n_LoadModel.Load_F = q;
                                    n_LoadModel.Load_Style = 3;
                                    n_LoadModel.Load_Line_Num = line_Num;
                                    n_LoadModel.Load_Length = lenth + 2 * junbu;
                                    n_LoadModel.Load_pt.X = x;
                                    n_LoadModel.Load_pt.Y = y - 2 * junbu;
                                    n_LoadModelList.Add(n_LoadModel);
                                    n_Load_Num++;
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region     数据准备

                    #region     将所有的类载荷分配到每根杆件
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        n_LineLoad = new LoadListClass();
                        n_LineLoad_Num = 0;
                        for (int j = 0; j < n_Load_Num; j++)
                        {
                            if (n_LoadModelList[j].Load_Line_Num == i)
                            {
                                n_LineLoad.LoadList.Add(n_LoadModelList[j]);
                                n_LineLoad_Num++;
                            }
                        }
                        n_LineLoad.load_Num = n_LineLoad_Num;
                        n_LineLoadList.Add(n_LineLoad);
                    }
                    #endregion

                    #region      给每根杆件里的载荷排序
                    //杆件循环
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        //载荷第一次循环
                        for (int j = 0; j < n_LineLoadList[i].load_Num - 1; j++)
                        {
                            //载荷第二次循环
                            for (int k = j + 1; k < n_LineLoadList[i].load_Num; k++)
                            {
                                if (n_LineLoadList[i].LoadList[j].Load_Length
                                    > n_LineLoadList[i].LoadList[k].Load_Length)
                                {
                                    loadTemp = n_LineLoadList[i].LoadList[j];
                                    n_LineLoadList[i].LoadList[j] = n_LineLoadList[i].LoadList[k];
                                    n_LineLoadList[i].LoadList[k] = loadTemp;
                                }
                            }
                        }
                    }
                    #endregion


                    #endregion

                    #endregion

                    #region     特殊点

                    #region     节点

                    #region     铰节点
                    for (int i = 0; i < m_Hinge_Num; i++)
                    {
                        for (int j = 0; j < m_Line_Num; j++)
                        {
                            #region     杆件起始点与铰节点重合
                            if (m_LineModelList[j].Line_BeginPoint == m_HingeJointList[i].J_pt)
                            {
                                prac++;
                                #region     圆点公共信息
                                Ellipse endEllipse1 = new Ellipse();
                                Ellipse endEllipse2 = new Ellipse();
                                Ellipse endEllipse3 = new Ellipse();
                                endEllipse1.Height = 4;
                                endEllipse1.Width = 4;
                                endEllipse1.Fill = Brushes.Red;

                                endEllipse2.Height = 4;
                                endEllipse2.Width = 4;
                                endEllipse2.Fill = Brushes.Red;

                                endEllipse3.Height = 4;
                                endEllipse3.Width = 4;
                                endEllipse3.Fill = Brushes.Red;
                                #endregion

                                #region     特殊点公共信息
                                prac1 = new PracticeClass();
                                prac1.prac_pt.X = m_HingeJointList[i].J_pt.X + x_area / 2;
                                prac1.prac_pt.Y = m_HingeJointList[i].J_pt.Y;
                                prac1.prac_Style = 1;
                                

                                prac3 = new PracticeClass();
                                prac3.prac_pt.X = m_HingeJointList[i].J_pt.X + x_area / 4;
                                prac3.prac_pt.Y = m_HingeJointList[i].J_pt.Y - y_area / 2;
                                prac3.prac_Style = 1;

                                prac1.line_Num = j;
                                prac1.prac_Ep = prac1.prac_pt;
                                prac1.dis = 0;
                                
                                prac3.line_Num = j;
                                prac3.prac_Ep = prac3.prac_pt;
                                prac3.dis = 0;
                                #endregion

                                #region     圆点平移
                                if (m_LineModelList[j].Line_Style == 0)
                                {
                                    //右移2
                                    endEllipse1.SetValue(Canvas.LeftProperty, m_HingeJointList[i].J_pt.X - 2 + x_area / 2 + 2);
                                    endEllipse1.SetValue(Canvas.TopProperty, m_HingeJointList[i].J_pt.Y - 2);
                                    prac1.prac_Ep.X += 2;
                                    
                                    endEllipse3.SetValue(Canvas.LeftProperty, m_HingeJointList[i].J_pt.X - 2 + x_area / 4 + 2);
                                    endEllipse3.SetValue(Canvas.TopProperty, m_HingeJointList[i].J_pt.Y - 2 - y_area / 2);
                                    prac3.prac_Ep.X += 2;
                                }
                                if (m_LineModelList[j].Line_Style == 1)
                                {
                                    //下移2
                                    endEllipse1.SetValue(Canvas.LeftProperty, m_HingeJointList[i].J_pt.X - 2 + x_area);
                                    endEllipse1.SetValue(Canvas.TopProperty, m_HingeJointList[i].J_pt.Y - 2 - 2);
                                    prac1.prac_Ep.Y -= 2;
                                    
                                    endEllipse3.SetValue(Canvas.LeftProperty, m_HingeJointList[i].J_pt.X - 2 + x_area /4);
                                    endEllipse3.SetValue(Canvas.TopProperty, m_HingeJointList[i].J_pt.Y - 2 - y_area / 2 - 2);
                                    prac3.prac_Ep.Y -= 2;
                                }
                                #endregion
                                prac1.prac_Ellip = endEllipse1;
                                prac1List.Add(prac1);

                                prac3.prac_Ellip = endEllipse3;
                                prac3List.Add(prac3);
                            }
                            #endregion

                            #region     杆件结束点与铰节点重合
                            if (m_LineModelList[j].Line_EndPoint == m_HingeJointList[i].J_pt)
                            {
                                prac++;
                                #region     圆点公共信息
                                Ellipse endEllipse1 = new Ellipse();
                                Ellipse endEllipse2 = new Ellipse();
                                Ellipse endEllipse3 = new Ellipse();
                                endEllipse1.Height = 4;
                                endEllipse1.Width = 4;
                                endEllipse1.Fill = Brushes.Red;

                                endEllipse2.Height = 4;
                                endEllipse2.Width = 4;
                                endEllipse2.Fill = Brushes.Red;

                                endEllipse3.Height = 4;
                                endEllipse3.Width = 4;
                                endEllipse3.Fill = Brushes.Red;
                                #endregion

                                #region     特殊点公共信息
                                prac1 = new PracticeClass();
                                prac1.prac_pt.X = m_HingeJointList[i].J_pt.X + x_area / 2;
                                prac1.prac_pt.Y = m_HingeJointList[i].J_pt.Y;
                                prac1.prac_Style = 1;
                               
                                prac3 = new PracticeClass();
                                prac3.prac_pt.X = m_HingeJointList[i].J_pt.X + x_area / 4;
                                prac3.prac_pt.Y = m_HingeJointList[i].J_pt.Y - y_area / 2;
                                prac3.prac_Style = 1;

                                prac1.line_Num = j;
                                prac1.prac_Ep = prac1.prac_pt;
                                prac1.dis = m_LineModelList[j].LineLength;
                                
                                prac3.line_Num = j;
                                prac3.prac_Ep = prac3.prac_pt;
                                prac3.dis = m_LineModelList[j].LineLength;
                                #endregion

                                #region     圆点平移
                                if (m_LineModelList[j].Line_Style == 0)
                                {
                                    //左移2
                                    endEllipse1.SetValue(Canvas.LeftProperty, m_HingeJointList[i].J_pt.X - 2 + x_area / 2 - 2);
                                    endEllipse1.SetValue(Canvas.TopProperty, m_HingeJointList[i].J_pt.Y - 2);
                                    prac1.prac_Ep.X -= 2;
                                    
                                    endEllipse3.SetValue(Canvas.LeftProperty, m_HingeJointList[i].J_pt.X - 2 + x_area / 4 - 2);
                                    endEllipse3.SetValue(Canvas.TopProperty, m_HingeJointList[i].J_pt.Y - 2 - y_area / 2);
                                    prac3.prac_Ep.X -= 2;
                                }
                                if (m_LineModelList[j].Line_Style == 1)
                                {
                                    //上移2
                                    endEllipse1.SetValue(Canvas.LeftProperty, m_HingeJointList[i].J_pt.X - 2 + x_area / 2);
                                    endEllipse1.SetValue(Canvas.TopProperty, m_HingeJointList[i].J_pt.Y - 2 + 2);
                                    prac1.prac_Ep.Y += 2;

                                    endEllipse3.SetValue(Canvas.LeftProperty, m_HingeJointList[i].J_pt.X - 2 + x_area / 4);
                                    endEllipse3.SetValue(Canvas.TopProperty, m_HingeJointList[i].J_pt.Y - 2 - y_area / 2 + 2);
                                    prac3.prac_Ep.Y += 2;
                                }
                                #endregion
                                prac1.prac_Ellip = endEllipse1;
                                prac1List.Add(prac1);
                                

                                prac3.prac_Ellip = endEllipse3;
                                prac3List.Add(prac3);
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region     刚节点
                    for (int i = 0; i < m_Rigid_Num; i++)
                    {
                        for (int j = 0; j < m_Line_Num; j++)
                        {
                            #region     杆件起始点与刚节点重合
                            if (m_LineModelList[j].Line_BeginPoint == m_RigidJointList[i].J_pt)
                            {
                                prac++;
                                #region     圆点公共信息
                                Ellipse endEllipse1 = new Ellipse();
                                Ellipse endEllipse2 = new Ellipse();
                                Ellipse endEllipse3 = new Ellipse();
                                endEllipse1.Height = 4;
                                endEllipse1.Width = 4;
                                endEllipse1.Fill = Brushes.Red;

                                endEllipse2.Height = 4;
                                endEllipse2.Width = 4;
                                endEllipse2.Fill = Brushes.Red;

                                endEllipse3.Height = 4;
                                endEllipse3.Width = 4;
                                endEllipse3.Fill = Brushes.Red;
                                #endregion

                                #region     特殊点公共信息
                                prac1 = new PracticeClass();
                                prac1.prac_pt.X = m_RigidJointList[i].J_pt.X + x_area / 2;
                                prac1.prac_pt.Y = m_RigidJointList[i].J_pt.Y;
                                prac1.prac_Style = 1;

                                prac3 = new PracticeClass();
                                prac3.prac_pt.X = m_RigidJointList[i].J_pt.X + x_area / 4;
                                prac3.prac_pt.Y = m_RigidJointList[i].J_pt.Y - y_area / 2;
                                prac3.prac_Style = 1;

                                prac1.line_Num = j;
                                prac1.prac_Ep = prac1.prac_pt;
                                prac1.dis = 0;

                                prac3.line_Num = j;
                                prac3.prac_Ep = prac3.prac_pt;
                                prac3.dis = 0;
                                #endregion

                                #region     圆点平移
                                if (m_LineModelList[j].Line_Style == 0)
                                {
                                    //右移2
                                    endEllipse1.SetValue(Canvas.LeftProperty, m_RigidJointList[i].J_pt.X - 2 + x_area / 2 + 2);
                                    endEllipse1.SetValue(Canvas.TopProperty, m_RigidJointList[i].J_pt.Y - 2);
                                    prac1.prac_Ep.X += 2;

                                    endEllipse3.SetValue(Canvas.LeftProperty, m_RigidJointList[i].J_pt.X - 2 + x_area / 4 + 2);
                                    endEllipse3.SetValue(Canvas.TopProperty, m_RigidJointList[i].J_pt.Y - 2 - y_area / 2);
                                    prac3.prac_Ep.X += 2;
                                }
                                if (m_LineModelList[j].Line_Style == 1)
                                {
                                    //下移2
                                    endEllipse1.SetValue(Canvas.LeftProperty, m_RigidJointList[i].J_pt.X - 2 + x_area / 2);
                                    endEllipse1.SetValue(Canvas.TopProperty, m_RigidJointList[i].J_pt.Y - 2 - 2);
                                    prac1.prac_Ep.Y -= 2;
                                    
                                    endEllipse3.SetValue(Canvas.LeftProperty, m_RigidJointList[i].J_pt.X - 2 + x_area / 4);
                                    endEllipse3.SetValue(Canvas.TopProperty, m_RigidJointList[i].J_pt.Y - 2 - y_area / 2 - 2);
                                    prac3.prac_Ep.Y -= 2;
                                }
                                #endregion
                                prac1.prac_Ellip = endEllipse1;
                                prac1List.Add(prac1);
                                
                                prac3.prac_Ellip = endEllipse3;
                                prac3List.Add(prac3);
                            }
                            #endregion

                            #region     杆件结束点与刚节点重合
                            if (m_LineModelList[j].Line_EndPoint == m_RigidJointList[i].J_pt)
                            {
                                prac++;
                                #region     圆点公共信息
                                Ellipse endEllipse1 = new Ellipse();
                                Ellipse endEllipse3 = new Ellipse();
                                endEllipse1.Height = 4;
                                endEllipse1.Width = 4;
                                endEllipse1.Fill = Brushes.Red;

                                endEllipse3.Height = 4;
                                endEllipse3.Width = 4;
                                endEllipse3.Fill = Brushes.Red;
                                #endregion

                                #region     特殊点公共信息
                                prac1 = new PracticeClass();
                                prac1.prac_pt.X = m_RigidJointList[i].J_pt.X + x_area / 2;
                                prac1.prac_pt.Y = m_RigidJointList[i].J_pt.Y;
                                prac1.prac_Style = 1;

                                prac3 = new PracticeClass();
                                prac3.prac_pt.X = m_RigidJointList[i].J_pt.X + x_area / 4;
                                prac3.prac_pt.Y = m_RigidJointList[i].J_pt.Y - y_area / 2;
                                prac3.prac_Style = 1;

                                prac1.line_Num = j;
                                prac1.prac_Ep = prac1.prac_pt;
                                prac1.dis = m_LineModelList[j].LineLength - 0.001;
                                
                                prac3.line_Num = j;
                                prac3.prac_Ep = prac3.prac_pt;
                                prac3.dis = m_LineModelList[j].LineLength - 0.001;
                                #endregion

                                #region     圆点平移
                                if (m_LineModelList[j].Line_Style == 0)
                                {
                                    //左移2
                                    endEllipse1.SetValue(Canvas.LeftProperty, m_RigidJointList[i].J_pt.X - 2 + x_area / 2 - 2);
                                    endEllipse1.SetValue(Canvas.TopProperty, m_RigidJointList[i].J_pt.Y - 2);
                                    prac1.prac_Ep.X -= 2;
                                    
                                    endEllipse3.SetValue(Canvas.LeftProperty, m_RigidJointList[i].J_pt.X - 2 + x_area / 4 - 2);
                                    endEllipse3.SetValue(Canvas.TopProperty, m_RigidJointList[i].J_pt.Y - 2 - y_area / 2);
                                    prac3.prac_Ep.X -= 2;
                                }
                                if (m_LineModelList[j].Line_Style == 1)
                                {
                                    //上移2
                                    endEllipse1.SetValue(Canvas.LeftProperty, m_RigidJointList[i].J_pt.X - 2 + x_area / 2);
                                    endEllipse1.SetValue(Canvas.TopProperty, m_RigidJointList[i].J_pt.Y - 2 + 2);
                                    prac1.prac_Ep.Y += 2;
                                    
                                    endEllipse3.SetValue(Canvas.LeftProperty, m_RigidJointList[i].J_pt.X - 2 + x_area / 4);
                                    endEllipse3.SetValue(Canvas.TopProperty, m_RigidJointList[i].J_pt.Y - 2 - y_area / 2 + 2);
                                    prac3.prac_Ep.Y += 2;
                                }
                                #endregion
                                prac1.prac_Ellip = endEllipse1;
                                prac1List.Add(prac1);

                                prac3.prac_Ellip = endEllipse3;
                                prac3List.Add(prac3);
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #endregion

                    #region     支座

                    for (int i = 0; i < m_Zhizuo_Num; i++)
                    {
                        chonghe = true;
                        for (int j = 0; j < prac; j++)
                        {
                            if ((m_ZhizuoList[i].Zhizuo_pt.X + x_area / 2 == prac1List[j].prac_pt.X)
                                && (m_ZhizuoList[i].Zhizuo_pt.Y == prac1List[j].prac_pt.Y))
                            {
                                chonghe = false;
                            }
                        }
                        if (chonghe == true)
                        {
                            #region     练习图1
                            Ellipse endEllipse1 = new Ellipse();
                            endEllipse1.Height = 4;
                            endEllipse1.Width = 4;
                            endEllipse1.Fill = Brushes.Red;
                            endEllipse1.SetValue(Canvas.LeftProperty, m_ZhizuoList[i].Zhizuo_pt.X - 2 + x_area / 2);
                            endEllipse1.SetValue(Canvas.TopProperty, m_ZhizuoList[i].Zhizuo_pt.Y - 2);

                            prac++;
                            prac1 = new PracticeClass();
                            prac1.prac_pt.X = m_ZhizuoList[i].Zhizuo_pt.X + x_area / 2;
                            prac1.prac_pt.Y = m_ZhizuoList[i].Zhizuo_pt.Y;
                            prac1.prac_Style = 1;
                            prac1.dis = m_ZhizuoList[i].Z_Length;
                            prac1.line_Num = m_ZhizuoList[i].Z_Line_Num;
                            prac1.prac_Ep = prac1.prac_pt;
                            prac1.prac_Ellip = endEllipse1;
                            prac1List.Add(prac1);
                            #endregion
                            
                            #region     参考答案图
                            Ellipse endEllipse3 = new Ellipse();
                            endEllipse3.Height = 4;
                            endEllipse3.Width = 4;
                            endEllipse3.Fill = Brushes.Red;
                            endEllipse3.SetValue(Canvas.LeftProperty, m_ZhizuoList[i].Zhizuo_pt.X - 2 + x_area / 4);
                            endEllipse3.SetValue(Canvas.TopProperty, m_ZhizuoList[i].Zhizuo_pt.Y - 2 - y_area / 2);

                            prac3 = new PracticeClass();
                            prac3.prac_pt.X = m_ZhizuoList[i].Zhizuo_pt.X + x_area / 4;
                            prac3.prac_pt.Y = m_ZhizuoList[i].Zhizuo_pt.Y - y_area / 2;
                            prac3.prac_Style = 1;
                            prac3.dis = m_ZhizuoList[i].Z_Length;
                            prac3.line_Num = m_ZhizuoList[i].Z_Line_Num;
                            prac3.prac_Ep = prac3.prac_pt;
                            prac3.prac_Ellip = endEllipse3;
                            prac3List.Add(prac3);
                            #endregion
                        }

                    }
                    #endregion

                    #region     施加载荷的点
                    for (int i = 0; i < m_Load_Num; i++)
                    {
                        #region     集中力
                        if ((m_LoadModelList[i].Load_Style == 1) || (m_LoadModelList[i].Load_Style == 3))
                        {
                            chonghe = true;
                            for (int j = 0; j < prac; j++)
                            {
                                if ((m_LoadModelList[i].Load_pt.X + x_area / 2 == prac1List[j].prac_pt.X)
                                    && (m_LoadModelList[i].Load_pt.Y == prac1List[j].prac_pt.Y))
                                {
                                    chonghe = false;
                                }
                            }
                            if (chonghe == true)
                            {
                                #region     绘图1
                                Ellipse endEllipse1 = new Ellipse();
                                endEllipse1.Height = 4;
                                endEllipse1.Width = 4;
                                endEllipse1.Fill = Brushes.Red;
                                endEllipse1.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area / 2);
                                endEllipse1.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2);

                                prac++;
                                prac1 = new PracticeClass();
                                prac1.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area / 2;
                                prac1.prac_pt.Y = m_LoadModelList[i].Load_pt.Y;
                                prac1.prac_Style = 1;
                                prac1.dis = m_LoadModelList[i].Load_Length;
                                prac1.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac1.prac_Ep = prac1.prac_pt;
                                prac1.prac_Ellip = endEllipse1;
                                prac1List.Add(prac1);
                                #endregion
                                
                                #region     参考答案图
                                Ellipse endEllipse3 = new Ellipse();
                                endEllipse3.Height = 4;
                                endEllipse3.Width = 4;
                                endEllipse3.Fill = Brushes.Red;
                                endEllipse3.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area /4);
                                endEllipse3.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - y_area / 2 - 2);

                                prac3 = new PracticeClass();
                                prac3.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area / 4;
                                prac3.prac_pt.Y = m_LoadModelList[i].Load_pt.Y - y_area / 2;
                                prac3.prac_Style = 1;
                                prac3.dis = m_LoadModelList[i].Load_Length;
                                prac3.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac3.prac_Ep = prac3.prac_pt;
                                prac3.prac_Ellip = endEllipse3;
                                prac3List.Add(prac3);
                                #endregion

                            }
                        }
                        #endregion

                        #region 集中力偶

                        #region     横杆
                        else if (m_LoadModelList[i].Load_Style == 9)
                        {
                            #region     左端

                            #region     练习图1
                            Ellipse endEllipse1 = new Ellipse();
                            endEllipse1.Height = 4;
                            endEllipse1.Width = 4;
                            endEllipse1.Fill = Brushes.Red;
                            endEllipse1.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 6 + x_area / 2);
                            endEllipse1.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2);

                            prac++;
                            prac1 = new PracticeClass();
                            prac1.prac_pt.X = m_LoadModelList[i].Load_pt.X - 0.1 + x_area / 2;
                            prac1.prac_pt.Y = m_LoadModelList[i].Load_pt.Y;
                            prac1.prac_Style = 1;
                            prac1.dis = m_LoadModelList[i].Load_Length - 0.1;
                            prac1.line_Num = m_LoadModelList[i].Load_Line_Num;
                            prac1.prac_Ep.X = prac1.prac_pt.X - 4;
                            prac1.prac_Ep.Y = prac1.prac_pt.Y;
                            prac1.prac_Ellip = endEllipse1;
                            prac1List.Add(prac1);
                            #endregion
                            
                            #region     参考答案图
                            Ellipse endEllipse3 = new Ellipse();
                            endEllipse3.Height = 4;
                            endEllipse3.Width = 4;
                            endEllipse3.Fill = Brushes.Red;
                            endEllipse3.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 6 + x_area / 4);
                            endEllipse3.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2 - y_area / 2);

                            prac3 = new PracticeClass();
                            prac3.prac_pt.X = m_LoadModelList[i].Load_pt.X - 0.1 + x_area / 4;
                            prac3.prac_pt.Y = m_LoadModelList[i].Load_pt.Y - y_area / 2;
                            prac3.prac_Style = 1;
                            prac3.dis = m_LoadModelList[i].Load_Length - 0.1;
                            prac3.line_Num = m_LoadModelList[i].Load_Line_Num;
                            prac3.prac_Ep.X = prac3.prac_pt.X - 4;
                            prac3.prac_Ep.Y = prac3.prac_pt.Y;
                            prac3.prac_Ellip = endEllipse3;
                            prac3List.Add(prac3);
                            #endregion

                            #endregion

                            #region     右端
                            chonghe = true;
                            for (int j = 0; j < prac; j++)
                            {
                                if ((m_LoadModelList[i].Load_pt.X + x_area / 2 == prac1List[j].prac_pt.X)
                                    && (m_LoadModelList[i].Load_pt.Y == prac1List[j].prac_pt.Y))
                                {
                                    chonghe = false;
                                }
                            }
                            if (chonghe == true)
                            {
                                #region     练习图1
                                Ellipse endEp1 = new Ellipse();
                                endEp1.Height = 4;
                                endEp1.Width = 4;
                                endEp1.Fill = Brushes.Red;
                                endEp1.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area / 2);
                                endEp1.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2);

                                prac++;
                                prac1 = new PracticeClass();
                                prac1.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area / 2;
                                prac1.prac_pt.Y = m_LoadModelList[i].Load_pt.Y;
                                prac1.prac_Style = 1;
                                prac1.dis = m_LoadModelList[i].Load_Length;
                                prac1.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac1.prac_Ep = prac1.prac_pt;
                                prac1.prac_Ellip = endEp1;
                                prac1List.Add(prac1);
                                #endregion

                                #region     参考答案
                                Ellipse endEp3 = new Ellipse();
                                endEp3.Height = 4;
                                endEp3.Width = 4;
                                endEp3.Fill = Brushes.Red;
                                endEp3.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area / 4);
                                endEp3.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2 - y_area / 2);

                                prac3 = new PracticeClass();
                                prac3.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area / 4;
                                prac3.prac_pt.Y = m_LoadModelList[i].Load_pt.Y - y_area / 2;
                                prac3.prac_Style = 1;
                                prac3.dis = m_LoadModelList[i].Load_Length;
                                prac3.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac3.prac_Ep = prac3.prac_pt;
                                prac3.prac_Ellip = endEp3;
                                prac3List.Add(prac3);
                                #endregion
                            }
                            #endregion

                        }
                        #endregion

                        #region     竖杆
                        else if (m_LoadModelList[i].Load_Style == 11)
                        {

                            #region     下端
                            chonghe = true;
                            for (int j = 0; j < prac; j++)
                            {
                                if ((m_LoadModelList[i].Load_pt.X + x_area / 2 == prac1List[j].prac_pt.X)
                                    && (m_LoadModelList[i].Load_pt.Y == prac1List[j].prac_pt.Y))
                                {
                                    chonghe = false;
                                }
                            }
                            if (chonghe == true)
                            {
                                prac++;

                                #region     练习图1
                                Ellipse endEllipse1 = new Ellipse();
                                endEllipse1.Height = 4;
                                endEllipse1.Width = 4;
                                endEllipse1.Fill = Brushes.Red;
                                endEllipse1.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area / 2);
                                endEllipse1.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2 - 4);

                                prac1 = new PracticeClass();
                                prac1.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area / 2;
                                prac1.prac_pt.Y = m_LoadModelList[i].Load_pt.Y;
                                prac1.prac_Style = 1;
                                prac1.dis = m_LoadModelList[i].Load_Length;
                                prac1.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac1.prac_Ep = prac1.prac_pt;
                                prac1.prac_Ellip = endEllipse1;
                                prac1List.Add(prac1);
                                #endregion
                                
                                #region     参考答案
                                Ellipse endEllipse3 = new Ellipse();
                                endEllipse3.Height = 4;
                                endEllipse3.Width = 4;
                                endEllipse3.Fill = Brushes.Red;
                                endEllipse3.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area / 4);
                                endEllipse3.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2 -4 - y_area / 2);

                                prac3 = new PracticeClass();
                                prac3.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area / 4;
                                prac3.prac_pt.Y = m_LoadModelList[i].Load_pt.Y - y_area / 2;
                                prac3.prac_Style = 1;
                                prac3.dis = m_LoadModelList[i].Load_Length;
                                prac3.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac3.prac_Ep = prac3.prac_pt;
                                prac3.prac_Ellip = endEllipse3;
                                prac3List.Add(prac3);
                                #endregion
                            }
                            #endregion

                            #region     上端

                            #region     练习图1
                            Ellipse endEp1 = new Ellipse();
                            endEp1.Height = 4;
                            endEp1.Width = 4;
                            endEp1.Fill = Brushes.Red;
                            endEp1.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area / 2);
                            endEp1.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2 );

                            prac++;
                            prac1 = new PracticeClass();
                            prac1.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area / 2;
                            prac1.prac_pt.Y = m_LoadModelList[i].Load_pt.Y + 0.1;
                            prac1.prac_Style = 1;
                            prac1.dis = m_LoadModelList[i].Load_Length - 0.1;
                            prac1.line_Num = m_LoadModelList[i].Load_Line_Num;
                            prac1.prac_Ep.X = prac1.prac_pt.X;
                            prac1.prac_Ep.Y = prac1.prac_pt.Y + 4;
                            prac1.prac_Ellip = endEp1;
                            prac1List.Add(prac1);
                            #endregion
                            
                            #region     参考答案图
                            Ellipse endEp3 = new Ellipse();
                            endEp3.Height = 4;
                            endEp3.Width = 4;
                            endEp3.Fill = Brushes.Red;
                            endEp3.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area / 4);
                            endEp3.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2 - y_area / 2);

                            prac3 = new PracticeClass();
                            prac3.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area /4;
                            prac3.prac_pt.Y = m_LoadModelList[i].Load_pt.Y + 0.1 - y_area / 2;
                            prac3.prac_Style = 1;
                            prac3.dis = m_LoadModelList[i].Load_Length - 0.1;
                            prac3.line_Num = m_LoadModelList[i].Load_Line_Num;
                            prac3.prac_Ep.X = prac3.prac_pt.X;
                            prac3.prac_Ep.Y = prac3.prac_pt.Y + 4;
                            prac3.prac_Ellip = endEp3;
                            prac3List.Add(prac3);
                            #endregion

                            #endregion

                        }
                        #endregion

                        #endregion

                        #region     均布载荷

                        #region     竖杆
                        if (m_LoadModelList[i].Load_Style == 5)
                        {
                            #region     上端
                            chonghe = true;
                            for (int j = 0; j < prac; j++)
                            {
                                if ((m_LoadModelList[i].Load_pt.X + x_area / 2 == prac1List[j].prac_pt.X)
                                    && (m_LoadModelList[i].Load_pt.Y == prac1List[j].prac_pt.Y))
                                {
                                    chonghe = false;
                                    chonghedian = j;
                                }
                            }
                            if (chonghe == false)
                            {
                                prac1List[chonghedian].prac_Style = 2;
                                prac3List[chonghedian].prac_Style = 2;
                            }
                            if (chonghe == true)
                            {
                                #region     练习图1
                                Ellipse endEllipse1 = new Ellipse();
                                endEllipse1.Height = 4;
                                endEllipse1.Width = 4;
                                endEllipse1.Fill = Brushes.Red;
                                endEllipse1.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area / 2);
                                endEllipse1.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2);

                                prac++;
                                prac1 = new PracticeClass();
                                prac1.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area / 2;
                                prac1.prac_pt.Y = m_LoadModelList[i].Load_pt.Y;
                                prac1.prac_Style = 2;
                                prac1.dis = m_LoadModelList[i].Load_Length - m_LoadModelList[i].Load_l / 2;
                                prac1.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac1.prac_Ep = prac1.prac_pt;
                                prac1.prac_Ellip = endEllipse1;
                                prac1List.Add(prac1);
                                #endregion
                                
                                #region     参考答案图
                                Ellipse endEllipse3 = new Ellipse();
                                endEllipse3.Height = 4;
                                endEllipse3.Width = 4;
                                endEllipse3.Fill = Brushes.Red;
                                endEllipse3.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area / 4);
                                endEllipse3.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2 - y_area / 2);

                                prac3 = new PracticeClass();
                                prac3.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area /4;
                                prac3.prac_pt.Y = m_LoadModelList[i].Load_pt.Y - y_area / 2;
                                prac3.prac_Style = 2;
                                prac3.dis = m_LoadModelList[i].Load_Length - m_LoadModelList[i].Load_l / 2;
                                prac3.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac3.prac_Ep = prac3.prac_pt;
                                prac3.prac_Ellip = endEllipse3;
                                prac3List.Add(prac3);
                                #endregion
                            }
                            #endregion

                            #region     下端
                            chonghe = true;
                            for (int j = 0; j < prac; j++)
                            {
                                if ((m_LoadModelList[i].Load_pt.X + x_area / 2 == prac1List[j].prac_pt.X)
                                    && (m_LoadModelList[i].Load_pt.Y - m_LoadModelList[i].Load_l == prac1List[j].prac_pt.Y))
                                {
                                    chonghe = false;
                                    chonghedian = j;
                                }
                            }
                            if (chonghe == false)
                            {
                                prac1List[chonghedian].prac_Style = 3;
                                prac3List[chonghedian].prac_Style = 3;
                            }
                            if (chonghe == true)
                            {
                                #region     练习图1
                                Ellipse endEllipse1 = new Ellipse();
                                endEllipse1.Height = 4;
                                endEllipse1.Width = 4;
                                endEllipse1.Fill = Brushes.Red;
                                endEllipse1.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area / 2);
                                endEllipse1.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - m_LoadModelList[i].Load_l - 2);

                                prac++;
                                prac1 = new PracticeClass();
                                prac1.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area / 2;
                                prac1.prac_pt.Y = m_LoadModelList[i].Load_pt.Y - m_LoadModelList[i].Load_l;
                                prac1.prac_Style = 3;
                                prac1.dis = m_LoadModelList[i].Load_Length + m_LoadModelList[i].Load_l / 2;
                                prac1.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac1.prac_Ep = prac1.prac_pt;
                                prac1.prac_Ellip = endEllipse1;
                                prac1List.Add(prac1);
                                #endregion
                                
                                #region     参考答案图
                                Ellipse endEllipse3 = new Ellipse();
                                endEllipse3.Height = 4;
                                endEllipse3.Width = 4;
                                endEllipse3.Fill = Brushes.Red;
                                endEllipse3.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area /4);
                                endEllipse3.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - m_LoadModelList[i].Load_l - 2 - y_area / 2);

                                prac3 = new PracticeClass();
                                prac3.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area /4;
                                prac3.prac_pt.Y = m_LoadModelList[i].Load_pt.Y - y_area / 2 - m_LoadModelList[i].Load_l;
                                prac3.prac_Style = 3;
                                prac3.dis = m_LoadModelList[i].Load_Length + m_LoadModelList[i].Load_l / 2;
                                prac3.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac3.prac_Ep = prac3.prac_pt;
                                prac3.prac_Ellip = endEllipse3;
                                prac3List.Add(prac3);
                                #endregion
                            }
                            #endregion

                        }
                        #endregion

                        #region     横杆
                        if (m_LoadModelList[i].Load_Style == 7)
                        {
                            #region     左端
                            chonghe = true;
                            for (int j = 0; j < prac; j++)
                            {
                                if ((m_LoadModelList[i].Load_pt.X + x_area / 2 == prac1List[j].prac_pt.X)
                                    && (m_LoadModelList[i].Load_pt.Y == prac1List[j].prac_pt.Y))
                                {
                                    chonghe = false;
                                    chonghedian = j;
                                }
                            }
                            if (chonghe == false)
                            {
                                prac1List[chonghedian].prac_Style = 2;
                                prac3List[chonghedian].prac_Style = 2;
                            }
                            if (chonghe == true)
                            {
                                #region     练习图1
                                Ellipse endEllipse1 = new Ellipse();
                                endEllipse1.Height = 4;
                                endEllipse1.Width = 4;
                                endEllipse1.Fill = Brushes.Red;
                                endEllipse1.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area / 2);
                                endEllipse1.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2);

                                prac++;
                                prac1 = new PracticeClass();
                                prac1.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area / 2;
                                prac1.prac_pt.Y = m_LoadModelList[i].Load_pt.Y;
                                prac1.prac_Style = 2;
                                prac1.dis = m_LoadModelList[i].Load_Length - m_LoadModelList[i].Load_l / 2;
                                prac1.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac1.prac_Ep = prac1.prac_pt;
                                prac1.prac_Ellip = endEllipse1;
                                prac1List.Add(prac1);
                                #endregion
                                
                                #region     参考答案图
                                Ellipse endEllipse3 = new Ellipse();
                                endEllipse3.Height = 4;
                                endEllipse3.Width = 4;
                                endEllipse3.Fill = Brushes.Red;
                                endEllipse3.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + x_area /4);
                                endEllipse3.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2 - y_area / 2);

                                prac3 = new PracticeClass();
                                prac3.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area /4;
                                prac3.prac_pt.Y = m_LoadModelList[i].Load_pt.Y - y_area / 2;
                                prac3.prac_Style = 2;
                                prac3.dis = m_LoadModelList[i].Load_Length - m_LoadModelList[i].Load_l / 2;
                                prac3.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac3.prac_Ep = prac3.prac_pt;
                                prac3.prac_Ellip = endEllipse3;
                                prac3List.Add(prac3);
                                #endregion
                            }
                            #endregion

                            #region     右端
                            chonghe = true;
                            for (int j = 0; j < prac; j++)
                            {
                                if ((m_LoadModelList[i].Load_pt.X + x_area / 2 + m_LoadModelList[i].Load_l == prac1List[j].prac_pt.X)
                                    && (m_LoadModelList[i].Load_pt.Y == prac1List[j].prac_pt.Y))
                                {
                                    chonghe = false;
                                    chonghedian = j;
                                }
                            }
                            if (chonghe == false)
                            {
                                prac1List[chonghedian].prac_Style = 3;
                                prac3List[chonghedian].prac_Style = 3;

                            }
                            if (chonghe == true)
                            {
                                #region     练习图1
                                Ellipse endEllipse1 = new Ellipse();
                                endEllipse1.Height = 4;
                                endEllipse1.Width = 4;
                                endEllipse1.Fill = Brushes.Red;
                                endEllipse1.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X - 2 + m_LoadModelList[i].Load_l + x_area / 2);
                                endEllipse1.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2);

                                prac++;
                                prac1 = new PracticeClass();
                                prac1.prac_pt.X = m_LoadModelList[i].Load_pt.X + m_LoadModelList[i].Load_l + x_area / 2;
                                prac1.prac_pt.Y = m_LoadModelList[i].Load_pt.Y;
                                prac1.prac_Style = 3;
                                prac1.dis = m_LoadModelList[i].Load_Length + m_LoadModelList[i].Load_l / 2;
                                prac1.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac1.prac_Ep = prac1.prac_pt;
                                prac1.prac_Ellip = endEllipse1;
                                prac1List.Add(prac1);
                                #endregion
                                
                                #region     参考答案图
                                Ellipse endEllipse3 = new Ellipse();
                                endEllipse3.Height = 4;
                                endEllipse3.Width = 4;
                                endEllipse3.Fill = Brushes.Red;
                                endEllipse3.SetValue(Canvas.LeftProperty, m_LoadModelList[i].Load_pt.X + m_LoadModelList[i].Load_l - 2 + x_area / 4);
                                endEllipse3.SetValue(Canvas.TopProperty, m_LoadModelList[i].Load_pt.Y - 2 - y_area / 2);

                                prac3 = new PracticeClass();
                                prac3.prac_pt.X = m_LoadModelList[i].Load_pt.X + x_area /4 + m_LoadModelList[i].Load_l;
                                prac3.prac_pt.Y = m_LoadModelList[i].Load_pt.Y - y_area / 2;
                                prac3.prac_Style = 3;
                                prac3.dis = m_LoadModelList[i].Load_Length + m_LoadModelList[i].Load_l / 2;
                                prac3.line_Num = m_LoadModelList[i].Load_Line_Num;
                                prac3.prac_Ep = prac3.prac_pt;
                                prac3.prac_Ellip = endEllipse3;
                                prac3List.Add(prac3);
                                #endregion
                            }
                            #endregion
                        }
                        #endregion

                        #endregion

                    }
                    #endregion

                    #region     分配到杆件上

                    #region     练习图1
                    for (int j = 0; j < m_Line_Num; j++)
                    {
                        lian1List = new PracListClass();
                        lian1List.pra_Num = 0;

                        for (int i = 0; i < prac; i++)
                        {
                            if (prac1List[i].line_Num == j)
                            {
                                pracTemp = prac1List[i];
                                lian1List.pra_Num++;
                                lian1List.praList.Add(pracTemp);
                            }
                        }
                        lian1ListList.Add(lian1List);
                    }
                    #endregion
                    
                    #region     参考图
                    for (int j = 0; j < m_Line_Num; j++)
                    {
                        lian3List = new PracListClass();
                        lian3List.pra_Num = 0;
                        for (int i = 0; i < prac; i++)
                        {
                            if (prac3List[i].line_Num == j)
                            {
                                pracTemp = prac3List[i];
                                lian3List.pra_Num++;
                                lian3List.praList.Add(pracTemp);
                            }
                        }
                        lian3ListList.Add(lian3List);
                    }
                    #endregion

                    #endregion

                    #region     排序

                    #region     练习图1
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        for (int j = 0; j < lian1ListList[i].pra_Num - 1; j++)
                        {
                            for (int k = j + 1; k < lian1ListList[i].pra_Num; k++)
                            {
                                if (lian1ListList[i].praList[j].dis > lian1ListList[i].praList[k].dis)
                                {
                                    pracTemp = lian1ListList[i].praList[j];
                                    lian1ListList[i].praList[j] = lian1ListList[i].praList[k];
                                    lian1ListList[i].praList[k] = pracTemp;
                                }
                            }
                        }
                    }
                        
                    #endregion

                    #region     参考图
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        for (int j = 0; j < lian3ListList[i].pra_Num - 1; j++)
                        {
                            for (int k = j + 1; k < lian3ListList[i].pra_Num; k++)
                            {
                                if (lian3ListList[i].praList[j].dis > lian3ListList[i].praList[k].dis)
                                {
                                    pracTemp = lian3ListList[i].praList[j];

                                    lian3ListList[i].praList[j] = lian3ListList[i].praList[k];
                                    lian3ListList[i].praList[k] = pracTemp;
                                }
                            }
                        }
                    }
                    #endregion

                    #endregion

                    #endregion

                    #region     参考答案弯矩图信息记录

                    #region     初始化
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        for (int j = 0; j < lian3ListList[i].pra_Num; j++)
                        {
                            lian3ListList[i].praList[j].prac_bend = lian3ListList[i].praList[j].prac_pt;
                        }
                    }
                    #endregion

                    #region     计算弯矩值
                    ruler = 1;
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        #region     横杆
                        if (m_LineModelList[i].Line_Style == 0)
                        {
                            for (int j = 0; j < lian3ListList[i].pra_Num; j++)
                            {
                                wanju1 = 0;
                                for (int k = 0; k < n_LineLoadList[i].load_Num; k++)
                                {
                                    if (n_LineLoadList[i].LoadList[k].Load_Length <= lian3ListList[i].praList[j].dis)
                                    {
                                        if ((n_LineLoadList[i].LoadList[k].Load_Style == 1)
                                            || (n_LineLoadList[i].LoadList[k].Load_Style == 9))
                                        {
                                            wanju1 += n_LineLoadList[i].LoadList[k].Load_M;
                                            wanjuF = n_LineLoadList[i].LoadList[k].Load_F;
                                            wanjuLength = lian3ListList[i].praList[j].dis - n_LineLoadList[i].LoadList[k].Load_Length;
                                            wanju1 -= wanjuF * wanjuLength;
                                        }
                                    }
                                }
                                if (wanju1 > ruler)
                                {
                                    ruler = wanju1;
                                }
                                if ((-wanju1) > ruler)
                                {
                                    ruler = -wanju1;
                                }
                                lian3ListList[i].praList[j].wanju = wanju1;
                            }
                        }
                        #endregion

                        #region     竖杆
                        if (m_LineModelList[i].Line_Style == 1)
                        {
                            for (int j = 0; j < lian3ListList[i].pra_Num; j++)
                            {
                                wanju1 = 0;
                                for (int k = 0; k < n_LineLoadList[i].load_Num; k++)
                                {
                                    if (n_LineLoadList[i].LoadList[k].Load_Length <= lian3ListList[i].praList[j].dis)
                                    {
                                        if ((n_LineLoadList[i].LoadList[k].Load_Style == 3)
                                            || (n_LineLoadList[i].LoadList[k].Load_Style == 11))
                                        {
                                            wanju1 += n_LineLoadList[i].LoadList[k].Load_M;
                                            wanjuF = n_LineLoadList[i].LoadList[k].Load_F;
                                            wanjuLength = lian3ListList[i].praList[j].dis - n_LineLoadList[i].LoadList[k].Load_Length;
                                            wanju1 -= wanjuF * wanjuLength;
                                        }
                                    }
                                }
                                if (wanju1 > ruler)
                                {
                                    ruler = wanju1;
                                }
                                if ((-wanju1) > ruler)
                                {
                                    ruler = -wanju1;
                                }
                                lian3ListList[i].praList[j].wanju = wanju1;
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region     分布载荷部分曲线数据存储

                    #region  初始化
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        for (int j = 0; j < n_LineLoadList[i].load_Num; j++)
                        {
                            n_LineLoadList[i].LoadList[j].Load_Bend.X = n_LineLoadList[i].LoadList[j].Load_pt.X + x_area / 4;
                            n_LineLoadList[i].LoadList[j].Load_Bend.Y = n_LineLoadList[i].LoadList[j].Load_pt.Y - y_area / 2;
                        }
                    }
                    #endregion

                    #region     记录弯矩、计算比例尺
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        //23为寻找均布载荷起始点的状态
                        tension = 23;
                        for (int j = 0; j < n_LineLoadList[i].load_Num; j++)
                        {
                            //找到均布载荷起始点，跳到状态24
                            if (tension == 23)
                            {
                                if (n_LineLoadList[i].LoadList[j].jiemian == 4)
                                {
                                    tension = 24;
                                }
                            }
                            //24为算均布载荷部分弯矩值大小的状态、找到均布载荷终点跳到23状态
                            if (tension == 24)
                            {
                                //均布载荷部分弯矩值计算
                                #region     横杆
                                if (m_LineModelList[i].Line_Style == 0)
                                {
                                    double mm = 0;
                                    for (int k = 0; k <= j; k++)
                                    {
                                        if ((n_LineLoadList[i].LoadList[k].Load_Style == 1)
                                            || (n_LineLoadList[i].LoadList[k].Load_Style == 2))
                                        {
                                            wanjuF = n_LineLoadList[i].LoadList[k].Load_F;
                                            wanjuLength = n_LineLoadList[i].LoadList[j].Load_Length - n_LineLoadList[i].LoadList[k].Load_Length;
                                            mm -= wanjuLength * wanjuF;

                                        }
                                        if ((n_LineLoadList[i].LoadList[k].Load_Style == 9)
                                            || (n_LineLoadList[i].LoadList[k].Load_Style == 10))
                                        {
                                            mm += n_LineLoadList[i].LoadList[k].Load_M;
                                        }
                                    }
                                    if (mm > ruler)
                                    {
                                        ruler = mm;
                                    }
                                    if ((-mm) > ruler)
                                    {
                                        ruler = -mm;
                                    }
                                    n_LineLoadList[i].LoadList[j].MM = mm;
                                }
                                #endregion

                                #region     竖杆
                                if (m_LineModelList[i].Line_Style == 1)
                                {
                                    double mm = 0;
                                    for (int k = 0; k < j; k++)
                                    {
                                        if ((n_LineLoadList[i].LoadList[k].Load_Style == 3)
                                            || (n_LineLoadList[i].LoadList[k].Load_Style == 4))
                                        {
                                            wanjuF = n_LineLoadList[i].LoadList[k].Load_F;
                                            wanjuLength = n_LineLoadList[i].LoadList[j].Load_Length - n_LineLoadList[i].LoadList[k].Load_Length;
                                            mm -= wanjuLength * wanjuF;
                                        }
                                        if ((n_LineLoadList[i].LoadList[k].Load_Style == 11)
                                            || (n_LineLoadList[i].LoadList[k].Load_Style == 12))
                                        {
                                            mm += n_LineLoadList[i].LoadList[k].Load_M;
                                        }
                                        if (mm > ruler)
                                        {
                                            ruler = mm;
                                        }
                                        if ((-mm) > ruler)
                                        {
                                            ruler = -mm;
                                        }
                                        n_LineLoadList[i].LoadList[j].MM = mm;
                                    }
                                }
                                #endregion

                                if (n_LineLoadList[i].LoadList[j].jiemian == 5)
                                {
                                    tension = 23;
                                }
                            }
                        }

                    }
                    #endregion

                    #endregion

                    #region     按比例尺缩小弯矩值
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        for (int j = 0; j < lian3ListList[i].pra_Num; j++)
                        {
                            lian3ListList[i].praList[j].wan = lian3ListList[i].praList[j].wanju * 60 / ruler;
                            if (m_LineModelList[i].Line_Style == 0)
                            {
                                lian3ListList[i].praList[j].prac_bend.Y += lian3ListList[i].praList[j].wan;
                            }
                            if (m_LineModelList[i].Line_Style == 1)
                            {
                                lian3ListList[i].praList[j].prac_bend.X += lian3ListList[i].praList[j].wan;
                            }
                        }

                    }
                    #endregion

                    #endregion

                    #region     比例尺标签

                    xishu = Math.Log10(ruler);
                    for (int i = 4; i <= xishu; i++)
                    {
                        tbr *= 10;
                    }
                    
                    #endregion

                    #region     直线
                    wanjuline.Clear();
                    wjline_Num = 0;
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        #region     第一根直线
                        m_LineNow = new Line();
                        m_LineNow.X1 = m_LineModelList[i].Line_BeginPoint.X + x_area / 4;
                        m_LineNow.Y1 = m_LineModelList[i].Line_BeginPoint.Y - y_area / 2;
                        m_LineNow.X2 = lian3ListList[i].praList[0].prac_bend.X;
                        m_LineNow.Y2 = lian3ListList[i].praList[0].prac_bend.Y;
                        m_LineNow.Stroke = Brushes.Red;
                        m_LineNow.StrokeThickness = 1;
                        wanjuline.Add(m_LineNow);
                        wjline_Num++;
                        tension = 21;
                        #endregion

                        #region     直线
                        for (int j = 0; j < lian3ListList[i].pra_Num - 1; j++)
                        {
                            #region     实线状态21,找到均布载荷起始点跳到状态22
                            if (tension == 21)
                            {
                                if (lian3ListList[i].praList[j].prac_Style != 2)
                                {
                                    m_LineNow = new Line();
                                    m_LineNow.X1 = lian3ListList[i].praList[j].prac_bend.X;
                                    m_LineNow.Y1 = lian3ListList[i].praList[j].prac_bend.Y;
                                    m_LineNow.X2 = lian3ListList[i].praList[j + 1].prac_bend.X;
                                    m_LineNow.Y2 = lian3ListList[i].praList[j + 1].prac_bend.Y;
                                    m_LineNow.Stroke = Brushes.Red;
                                    m_LineNow.StrokeThickness = 1;
                                    wanjuline.Add(m_LineNow);
                                    wjline_Num++;
                                }
                                //均布载荷左端点
                                if (lian3ListList[i].praList[j].prac_Style == 2)
                                {
                                    tension = 22;
                                }

                            }
                            #endregion

                            #region     虚线和竖线的信息
                            if (tension == 22)
                            {
                                if (lian3ListList[i].praList[j].prac_Style != 3)
                                {
                                    #region    画虚线
                                    m_LineNow = new Line();
                                    m_LineNow.X1 = lian3ListList[i].praList[j].prac_bend.X;
                                    m_LineNow.Y1 = lian3ListList[i].praList[j].prac_bend.Y;
                                    m_LineNow.X2 = lian3ListList[i].praList[j + 1].prac_bend.X;
                                    m_LineNow.Y2 = lian3ListList[i].praList[j + 1].prac_bend.Y;
                                    m_LineNow.Stroke = Brushes.Red;
                                    m_LineNow.StrokeThickness = 1;
                                    m_LineNow.StrokeDashArray = xuxian;
                                    wanjuline.Add(m_LineNow);
                                    wjline_Num++;
                                    #endregion

                                    shu = new ShuXianClass();
                                    shu.line_Num = i;
                                    shu.xu_pt.X = (lian3ListList[i].praList[j].prac_bend.X + lian3ListList[i].praList[j + 1].prac_bend.X) / 2;
                                    shu.xu_pt.Y = (lian3ListList[i].praList[j].prac_bend.Y + lian3ListList[i].praList[j + 1].prac_bend.Y) / 2;
                                    double mTemp = (lian3ListList[i].praList[j].wanju + lian3ListList[i].praList[j + 1].wanju) / 2;
                                    double XX = (lian3ListList[i].praList[j].dis + lian3ListList[i].praList[j + 1].dis) / 2;
                                    double MTemp = 0;

                                    #region     均布载荷打分信息准备部分
                                    quxian3.left_pt = lian3ListList[i].praList[j].prac_bend;
                                    quxian3.right_pt = lian3ListList[i].praList[j + 1].prac_bend;
                                    quxian3.mid_pt.X = (quxian3.left_pt.X + quxian3.right_pt.X) / 2;
                                    quxian3.mid_pt.Y = (quxian3.left_pt.Y + quxian3.right_pt.Y) / 2;
                                    quxian3.bend_pt = quxian3.mid_pt;
                                    #endregion

                                    #region    横杆
                                    if (m_LineModelList[i].Line_Style == 0)
                                    {
                                        for (int k = 0; k < n_LineLoadList[i].load_Num; k++)
                                        {
                                            if (n_LineLoadList[i].LoadList[k].Load_Length <= XX)
                                            {
                                                if ((n_LineLoadList[i].LoadList[k].Load_Style == 1)
                                                    || (n_LineLoadList[i].LoadList[k].Load_Style == 2)
                                                    || (n_LineLoadList[i].LoadList[k].Load_Style == 9)
                                                    || (n_LineLoadList[i].LoadList[k].Load_Style == 10))
                                                {
                                                    MTemp += n_LineLoadList[i].LoadList[k].Load_M;
                                                    double lengTemp = XX - n_LineLoadList[i].LoadList[k].Load_Length;
                                                    double FTemp = n_LineLoadList[i].LoadList[k].Load_F;
                                                    MTemp -= lengTemp * FTemp;
                                                }
                                            }
                                        }
                                        shu.shu_M = MTemp - mTemp;
                                        shu.qu_pt.X = shu.xu_pt.X;
                                        shu.qu_pt.Y = shu.xu_pt.Y + shu.shu_M * 60 / ruler;
                                        quxian3.qu_M = shu.shu_M;
                                        quxian3.bend_pt = shu.qu_pt;
                                    }
                                    #endregion

                                    #region     竖杆
                                    if (m_LineModelList[i].Line_Style == 1)
                                    {
                                        for (int k = 0; k < n_LineLoadList[i].load_Num; k++)
                                        {
                                            if (n_LineLoadList[i].LoadList[k].Load_Length <= XX)
                                            {
                                                if ((n_LineLoadList[i].LoadList[k].Load_Style == 3)
                                                    || (n_LineLoadList[i].LoadList[k].Load_Style == 4)
                                                    || (n_LineLoadList[i].LoadList[k].Load_Style == 11)
                                                    || (n_LineLoadList[i].LoadList[k].Load_Style == 12))
                                                {
                                                    MTemp += n_LineLoadList[i].LoadList[k].Load_M;
                                                    double lengTemp = XX - n_LineLoadList[i].LoadList[k].Load_Length;
                                                    double FTemp = n_LineLoadList[i].LoadList[k].Load_F;
                                                    MTemp -= lengTemp * FTemp;
                                                }
                                            }
                                        }
                                        shu.shu_M = MTemp - mTemp;
                                        shu.qu_pt.X = shu.xu_pt.X + shu.shu_M * 60 / ruler;
                                        shu.qu_pt.Y = shu.xu_pt.Y;
                                        quxian3.bend_pt = shu.qu_pt;
                                        quxian3.qu_M = shu.shu_M;
                                    }
                                    #endregion

                                    quxian3List.Add(quxian3);
                                    quxian3 = new QuXianClass();
                                    shuList.shu_num++;
                                    shuList.shuList.Add(shu);

                                }
                                if (lian3ListList[i].praList[j].prac_Style == 3)
                                {
                                    tension = 21;
                                    j--;
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region     最后一根直线
                        if (tension == 21)
                        {
                            int nn = lian3ListList[i].pra_Num - 1;
                            m_LineNow = new Line();
                            m_LineNow.X1 = lian3ListList[i].praList[nn].prac_bend.X;
                            m_LineNow.Y1 = lian3ListList[i].praList[nn].prac_bend.Y;
                            m_LineNow.X2 = m_LineModelList[i].Line_EndPoint.X + x_area / 4;
                            m_LineNow.Y2 = m_LineModelList[i].Line_EndPoint.Y - y_area / 2;
                            m_LineNow.Stroke = Brushes.Red;
                            m_LineNow.StrokeThickness = 1;
                            wanjuline.Add(m_LineNow);
                            wjline_Num++;
                        }
                        #endregion

                        shuListList.Add(shuList);
                        shuList = new ShuXianListClass();

                    }
                    #endregion
                }
                catch
                {
                    MessageBox.Show("程序错误，未能成功计算弯矩图");
                }

                order = 4;
            }
            if (order != 3)
            {
                if (order != 4)
                    MessageBox.Show("无效的操作!");
            }
            if (order == 4)
                MessageBox.Show("请在练习图区域绘制练习图");
        }
        #endregion

        #region     参考答案图

        #region     解线性方程组声明变量
        private MatrixCls Di;
        private int n;
        private double[,] Ci;
        private List<double> dxList = new List<double>();
        #endregion

        #region     新建list存储杆件受到的载荷所需变量

        private LoadModel n_LoadModel = new LoadModel();
        private List<LoadModel> n_LoadModelList = new List<LoadModel>();
        private int n_Load_Num;
        #endregion

        #region     载荷分配到
        private LoadListClass n_LineLoad = new LoadListClass();
        private List<LoadListClass> n_LineLoadList = new List<LoadListClass>();
        private int n_LineLoad_Num;
        //载荷转换项
        private LoadModel loadTemp = new LoadModel();
        #endregion

        private List<Line> wanjuline = new List<Line>();
        private int wjline_Num;

        private void bending_Click(object sender, RoutedEventArgs e)
        {
            #region     杆件
            for (int i = 0; i < m_Line_Num; i++)
            {
                m_LineNow = new Line();
                m_LineNow.X1 = m_LineModelList[i].Line_BeginPoint.X + x_area / 4;
                m_LineNow.Y1 = m_LineModelList[i].Line_BeginPoint.Y - y_area / 2;
                m_LineNow.X2 = m_LineModelList[i].Line_EndPoint.X + x_area / 4;
                m_LineNow.Y2 = m_LineModelList[i].Line_EndPoint.Y - y_area / 2;
                m_LineNow.Stroke = Brushes.Black;
                m_LineNow.StrokeThickness = 1;
                can.Children.Add(m_LineNow);
            }
            #endregion

            #region     支座
            for (int i = 0; i < rec; i++)
            {
                can.Children.Add(rec3List[i]);
            }
            #endregion

            #region     弯矩图直线和虚线
            for (int i = 0; i < wjline_Num; i++)
            {
                can.Children.Add(wanjuline[i]);
            }
            #endregion

            #region     竖线
            for (int i = 0; i < m_Line_Num; i++)
            {
                for (int j = 0; j < shuListList[i].shu_num; j++)
                {
                    m_LineNow = new Line();
                    m_LineNow.X1 = shuListList[i].shuList[j].xu_pt.X;
                    m_LineNow.Y1 = shuListList[i].shuList[j].xu_pt.Y;
                    m_LineNow.X2 = shuListList[i].shuList[j].qu_pt.X;
                    m_LineNow.Y2 = shuListList[i].shuList[j].qu_pt.Y;
                    m_LineNow.Stroke = Brushes.Green;
                    m_LineNow.StrokeThickness = 1;
                    can.Children.Add(m_LineNow);

                    m_LineNow = new Line();
                    m_LineNow.X1 = (shuListList[i].shuList[j].xu_pt.X + shuListList[i].shuList[j].qu_pt.X) / 2;
                    m_LineNow.Y1 = (shuListList[i].shuList[j].xu_pt.Y + shuListList[i].shuList[j].qu_pt.Y) / 2;
                    m_LineNow.X2 = (shuListList[i].shuList[j].xu_pt.X + shuListList[i].shuList[j].qu_pt.X) / 2 + 40;
                    m_LineNow.Y2 = (shuListList[i].shuList[j].xu_pt.Y + shuListList[i].shuList[j].qu_pt.Y) / 2 + 40;
                    m_LineNow.Stroke = Brushes.Green;
                    m_LineNow.StrokeThickness = 1;
                    can.Children.Add(m_LineNow);

                    #region     竖线标签
                    TextBlock TBqu = new TextBlock();
                    var TBquX = shuListList[i].shuList[j].qu_pt.X - 10 + 40;
                    var TBquY = shuListList[i].shuList[j].qu_pt.Y + 20 + 40;
                    ScaleTransform TBquscale = new ScaleTransform();
                    TBquscale.ScaleY = -1;
                    TBqu.RenderTransform = TBquscale;
                    double shum = shuListList[i].shuList[j].shu_M  / 400/bilichi/bilichi;
                    string str1 = "";
                    if (shuListList[i].shuList[j].shu_M > 0)
                    {
                        str1 = shum.ToString("f2");
                    }
                    if (shuListList[i].shuList[j].shu_M < 0)
                    {
                        double abcd = -shum;
                        str1 = abcd.ToString("f2");
                    }
                    TBqu.Text = "" + str1;
                    TBqu.Margin = new Thickness(TBquX, TBquY, 0, 0);
                    can.Children.Add(TBqu);
                    #endregion
                }
            }
            #endregion

            #region     标签
            for (int i = 0; i < m_Line_Num; i++)
            {
                #region  横杆
                if (m_LineModelList[i].Line_Style == 0)
                {
                    for (int j = 0; j < lian3ListList[i].pra_Num; j++)
                    {
                        double er = 0;
                        double k = 0;
                        if (lian3ListList[i].praList[j].wanju == 0)
                        {
                            er = lian1ListList[i].praList[j].wanju -
                                lian3ListList[i].praList[j].wanju;

                        }
                        if (lian3ListList[i].praList[j].wanju != 0)
                        {
                            er = (lian1ListList[i].praList[j].wanju -
                                lian3ListList[i].praList[j].wanju)
                            / lian3ListList[i].praList[j].wanju;

                        }
                        if ((er < 0.05) && (er > (-0.05)))
                        {
                            k = lian1ListList[i].praList[j].wanju;
                        }
                        else
                        {
                            k = lian3ListList[i].praList[j].wanju;
                        }
                        if (lian3ListList[i].praList[j].wanju > 5)
                        {
                            TextBlock TB_m = new TextBlock();
                            var f_TBX = lian3ListList[i].praList[j].prac_bend.X - 5;
                            var f_TBY = lian3ListList[i].praList[j].prac_bend.Y + 20 + 5;
                            ScaleTransform f_TBscale = new ScaleTransform();
                            f_TBscale.ScaleY = -1;
                            TB_m.RenderTransform = f_TBscale;
                            TB_m.Foreground = Brushes.Blue;
                            string wanjutp = String.Format("{0:N0}", k/400/bilichi/bilichi);
                            TB_m.Text = "" + wanjutp;
                            TB_m.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                            can.Children.Add(TB_m);

                        }
                        if (lian3ListList[i].praList[j].wan < 0)
                        {
                            TextBlock TB_m = new TextBlock();
                            var f_TBX = lian3ListList[i].praList[j].prac_bend.X - 5;
                            var f_TBY = lian3ListList[i].praList[j].prac_bend.Y - 5 + 5;
                            ScaleTransform f_TBscale = new ScaleTransform();
                            f_TBscale.ScaleY = -1;
                            TB_m.RenderTransform = f_TBscale;
                            TB_m.Foreground = Brushes.Blue;
                            string wanjutp = String.Format("{0:N0}", -k/ 400/bilichi/bilichi);
                            TB_m.Text = "" + wanjutp;
                            TB_m.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                            can.Children.Add(TB_m);
                        }
                    }
                }

                #endregion

                #region  竖杆
                if (m_LineModelList[i].Line_Style == 1)
                {
                    for (int j = 0; j < lian3ListList[i].pra_Num; j++)
                    {
                        double er = 0;
                        double k = 0;
                        if (lian3ListList[i].praList[j].wanju == 0)
                        {
                            er = lian1ListList[i].praList[j].wanju -
                                lian3ListList[i].praList[j].wanju;

                        }
                        if (lian3ListList[i].praList[j].wanju != 0)
                        {
                            er = (lian1ListList[i].praList[j].wanju -
                                lian3ListList[i].praList[j].wanju)
                            / lian3ListList[i].praList[j].wanju;

                        }
                        if ((er < 0.05) && (er > (-0.05)))
                        {
                            k = lian1ListList[i].praList[j].wanju;
                        }
                        else
                        {
                            k = lian3ListList[i].praList[j].wanju;
                        }
                        if (lian3ListList[i].praList[j].wanju > 5)
                        {
                            TextBlock TB_m = new TextBlock();
                            var f_TBX = lian3ListList[i].praList[j].prac_bend.X - 5;
                            var f_TBY = lian3ListList[i].praList[j].prac_bend.Y + 20 + 5;
                            ScaleTransform f_TBscale = new ScaleTransform();
                            f_TBscale.ScaleY = -1;
                            TB_m.RenderTransform = f_TBscale;
                            TB_m.Foreground = Brushes.Green;
                            string wanjutp = String.Format("{0:N0}", k / 400/bilichi/bilichi);
                            TB_m.Text = "" + wanjutp;
                            TB_m.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                            can.Children.Add(TB_m);
                        }
                        if (lian3ListList[i].praList[j].wan < 0)
                        {
                            TextBlock TB_m = new TextBlock();
                            var f_TBX = lian3ListList[i].praList[j].prac_bend.X - 5;
                            var f_TBY = lian3ListList[i].praList[j].prac_bend.Y - 5 + 5;
                            ScaleTransform f_TBscale = new ScaleTransform();
                            f_TBscale.ScaleY = -1;
                            TB_m.RenderTransform = f_TBscale;
                            TB_m.Foreground = Brushes.Green;
                            string wanjutp = String.Format("{0:N0}", -k / 400/bilichi/bilichi);
                            TB_m.Text = "" + wanjutp;
                            TB_m.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                            can.Children.Add(TB_m);
                        }
                    }
                }

                #endregion
            }
            #endregion

            #region     曲线点集
            for (int i = 0; i < m_Line_Num; i++)
            {
                //寻找均布载荷起点状态
                tension = 25;
                for (int j = 0; j < n_LineLoadList[i].load_Num - 1; j++)
                {
                    if (tension == 25)
                    {
                        if (n_LineLoadList[i].LoadList[j].jiemian == 4)
                        {
                            tension = 26;
                        }
                    }
                    if (tension == 26)
                    {
                        if (m_LineModelList[i].Line_Style == 0)
                        {
                            n_LineLoadList[i].LoadList[j].Load_Bend.Y +=
                                n_LineLoadList[i].LoadList[j].MM * 60 / ruler;
                        }
                        if (m_LineModelList[i].Line_Style == 1)
                        {
                            n_LineLoadList[i].LoadList[j].Load_Bend.X +=
                                n_LineLoadList[i].LoadList[j].MM * 60 / ruler;
                        }

                        if (n_LineLoadList[i].LoadList[j].jiemian == 5)
                        {
                            tension = 25;
                        }
                    }
                }
            }
            #endregion

            #region     曲线绘制
            for (int i = 0; i < m_Line_Num; i++)
            {
                //寻找均布载荷起点状态
                tension = 25;
                for (int j = 0; j < n_LineLoadList[i].load_Num - 1; j++)
                {
                    if (tension == 25)
                    {
                        if (n_LineLoadList[i].LoadList[j].jiemian == 4)
                        {
                            tension = 26;
                        }
                    }
                    if (tension == 26)
                    {
                        if (n_LineLoadList[i].LoadList[j].jiemian != 5)
                        {
                            m_LineNow = new Line();

                            m_LineNow.X1 = n_LineLoadList[i].LoadList[j].Load_Bend.X;
                            m_LineNow.Y1 = n_LineLoadList[i].LoadList[j].Load_Bend.Y;
                            m_LineNow.X2 = n_LineLoadList[i].LoadList[j + 1].Load_Bend.X;
                            m_LineNow.Y2 = n_LineLoadList[i].LoadList[j + 1].Load_Bend.Y;
                            m_LineNow.Stroke = Brushes.Red;
                            m_LineNow.StrokeThickness = 1;
                            can.Children.Add(m_LineNow);
                        }
                        if (n_LineLoadList[i].LoadList[j].jiemian == 5)
                        {
                            tension = 25;
                        }
                    }
                }
            }
            #endregion

        }
        #endregion
        
        #endregion
        
        #region     练习图
        
        #region     练习部分

        private int chonghedian;

        #region 练习部分--结构显示

        private void practice_Click(object sender, RoutedEventArgs e)
        {
            if(order==4)
            {
                try
                {
                    TextBlock TB_ruler = new TextBlock();
                    var f_TBrulerX = x_area -500;
                    var f_TBrulerY = y_area;
                    ScaleTransform f_TBrulerscale = new ScaleTransform();
                    f_TBrulerscale.ScaleY = -1;
                    TB_ruler.RenderTransform = f_TBrulerscale;
                    TB_ruler.Foreground = Brushes.Green;
                    TB_ruler.Background = Brushes.White;
                    TB_ruler.FontSize = 20;
                    TB_ruler.Text = "提示：弯矩图上侧为正、下侧为负;右侧为正、左侧为负";
                    TB_ruler.Margin = new Thickness(f_TBrulerX, f_TBrulerY, 0, 0);
                    can.Children.Add(TB_ruler);

                    #region     杆件部分
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        m_LineNow = new Line();
                        m_LineNow.X1 = m_LineModelList[i].Line_BeginPoint.X + x_area / 2;
                        m_LineNow.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                        m_LineNow.X2 = m_LineModelList[i].Line_EndPoint.X + x_area / 2;
                        m_LineNow.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                        m_LineNow.Stroke = Brushes.Black;
                        m_LineNow.StrokeThickness = 1;
                        can.Children.Add(m_LineNow);

                        mLine1List.Add(m_LineNow);
                    }
                    #endregion

                    #region     支座
                    for (int i = 0; i < rec; i++)
                    {
                        can.Children.Add(rec1List[i]);
                    }
                    #endregion

                    #region     绘制出关键点
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        for (int j = 0; j < lian1ListList[i].pra_Num; j++)
                        {
                            can.Children.Add(lian1ListList[i].praList[j].prac_Ellip);
                            lian1ListList[i].praList[j].prac_bend = lian1ListList[i].praList[j].prac_pt;
                        }
                    }
                    #endregion
                }
                catch
                {
                    MessageBox.Show("练习图结构绘制失败");
                }

                order = 5;
            }
        }
        
        #endregion
        
        #endregion

        #region     绘图部分

        #region     均布载荷第一次绘图
        private QuXianClass quxian = new QuXianClass();
        private List<QuXianClass> quxianList = new List<QuXianClass>();
        private QuXianClass quxian2 = new QuXianClass();
        private List<QuXianClass> quxian2List = new List<QuXianClass>();
        private int qu_Num;

        private void caotu1_Click(object sender, RoutedEventArgs e)
        {
            if (order == 5)
            {
                try
                {
                    bool doit = true;
                    if (doit == true)
                    {
                        #region     线条部分
                        if (choose1 != 0)
                        {
                            //擦除红线
                            can.Children.Remove(choList[choose1 - 1]);

                            //生成黑线
                            can.Children.Add(huifu1[choose1 - 1]);
                        }
                        #endregion

                        #region     核心部分
                        for (int i = 0; i < m_Line_Num; i++)
                        {
                            m_LineNow = new Line();
                            m_LineNow.X1 = m_LineModelList[i].Line_BeginPoint.X + x_area / 2;
                            m_LineNow.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                            m_LineNow.X2 = lian1ListList[i].praList[0].prac_bend.X;
                            m_LineNow.Y2 = lian1ListList[i].praList[0].prac_bend.Y;
                            m_LineNow.Stroke = Brushes.Blue;
                            m_LineNow.StrokeThickness = 1;
                            can.Children.Add(m_LineNow);
                            for (int j = 0; j < lian1ListList[i].pra_Num - 1; j++)
                            {
                                #region     状态1找到左端点后跳至状态2
                                if (tension == 1)
                                {
                                    if (lian1ListList[i].praList[j].prac_Style != 2)
                                    {
                                        m_LineNow = new Line();
                                        m_LineNow.X1 = lian1ListList[i].praList[j].prac_bend.X;
                                        m_LineNow.Y1 = lian1ListList[i].praList[j].prac_bend.Y;
                                        m_LineNow.X2 = lian1ListList[i].praList[j + 1].prac_bend.X;
                                        m_LineNow.Y2 = lian1ListList[i].praList[j + 1].prac_bend.Y;
                                        m_LineNow.Stroke = Brushes.Blue;
                                        m_LineNow.StrokeThickness = 1;
                                        can.Children.Add(m_LineNow);
                                    }
                                    //均布载荷左端点
                                    if (lian1ListList[i].praList[j].prac_Style == 2)
                                    {
                                        if (lian1ListList[i].praList[j].prac_Style == 2)
                                        {
                                            tension = 2;
                                        }
                                    }
                                }
                                #endregion

                                #region     状态2找到右端点后跳至状态1
                                if (tension == 2)
                                {
                                    if (lian1ListList[i].praList[j].prac_Style != 3)
                                    {
                                        #region     画虚线
                                        m_LineNow = new Line();
                                        m_LineNow.X1 = lian1ListList[i].praList[j].prac_bend.X;
                                        m_LineNow.Y1 = lian1ListList[i].praList[j].prac_bend.Y;
                                        m_LineNow.X2 = lian1ListList[i].praList[j + 1].prac_bend.X;
                                        m_LineNow.Y2 = lian1ListList[i].praList[j + 1].prac_bend.Y;
                                        m_LineNow.Stroke = Brushes.Blue;
                                        m_LineNow.StrokeThickness = 1;
                                        m_LineNow.StrokeDashArray = xuxian;
                                        can.Children.Add(m_LineNow);
                                        #endregion

                                        #region     记录曲线数据
                                        quxian = new QuXianClass();
                                        qu_Num++;
                                        quxian.left_pt = lian1ListList[i].praList[j].prac_bend;
                                        quxian.right_pt = lian1ListList[i].praList[j + 1].prac_bend;
                                        quxian.mid_pt.X = (quxian.left_pt.X + quxian.right_pt.X) / 2;
                                        quxian.mid_pt.Y = (quxian.left_pt.Y + quxian.right_pt.Y) / 2;
                                        quxian.bend_pt = quxian.mid_pt;
                                        if (lian1ListList[i].praList[j].prac_pt.Y == lian1ListList[i].praList[j + 1].prac_pt.Y)
                                        {
                                            quxian.qu_Style = 1;
                                        }
                                        if (lian1ListList[i].praList[j].prac_pt.X == lian1ListList[i].praList[j + 1].prac_pt.X)
                                        {
                                            quxian.qu_Style = 2;
                                        }
                                        quxianList.Add(quxian);
                                        #endregion

                                        #region     中点圆点标记
                                        elli1 = new Ellipse();
                                        elli1.Height = 4;
                                        elli1.Width = 4;
                                        elli1.Fill = Brushes.Red;
                                        elli1.SetValue(Canvas.LeftProperty, quxian.mid_pt.X - 2);
                                        elli1.SetValue(Canvas.TopProperty, quxian.mid_pt.Y - 2);
                                        can.Children.Add(elli1);
                                        #endregion
                                    }
                                    if (lian1ListList[i].praList[j].prac_Style == 3)
                                    {
                                        tension = 1;
                                        j--;
                                    }

                                }
                                #endregion
                            }
                            int mmm = lian1ListList[i].pra_Num;
                            m_LineNow = new Line();
                            m_LineNow.X1 = lian1ListList[i].praList[mmm - 1].prac_bend.X;
                            m_LineNow.Y1 = lian1ListList[i].praList[mmm - 1].prac_bend.Y;
                            m_LineNow.X2 = m_LineModelList[i].Line_EndPoint.X + x_area / 2;
                            m_LineNow.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                            m_LineNow.Stroke = Brushes.Blue;
                            m_LineNow.StrokeThickness = 1;
                            can.Children.Add(m_LineNow);
                        }
                        #endregion

                        doit = true;
                    }

                    tension = 3;
                    order = 6;

                }
                catch
                {
                    MessageBox.Show("练习图绘制弯矩图失败");
                }
            }
        }
        #endregion

        #region     均布载荷第二次绘图
        private double q_qu, l_qu, num_qu;
        private PracticeClass qu_pr = new PracticeClass();
        private PracListClass qu_prac = new PracListClass();
        private List<PracListClass> qu_pracList = new List<PracListClass>();

        private PracticeClass qu2_pr = new PracticeClass();
        private PracListClass qu2_prac = new PracListClass();
        private List<PracListClass> qu2_pracList = new List<PracListClass>();
        private double qu_a, qu_b, qu_X, qu_Y;

        #region 练习图
        private void draw1_ok_Click(object sender, RoutedEventArgs e)
        {
            if (order == 6)
            {
                try
                {
                    bool doit = true;
                    if (doit == true)
                    {
                        tension = 10;
                        for (int i = 0; i < qu_Num; i++)
                        {
                            #region     竖线部分
                            m_LineNow = new Line();
                            m_LineNow.X1 = quxianList[i].mid_pt.X;
                            m_LineNow.Y1 = quxianList[i].mid_pt.Y;
                            m_LineNow.X2 = quxianList[i].bend_pt.X;
                            m_LineNow.Y2 = quxianList[i].bend_pt.Y;
                            m_LineNow.Stroke = Brushes.Blue;
                            m_LineNow.StrokeThickness = 1;
                            m_LineNow.StrokeDashArray = xuxian;
                            can.Children.Add(m_LineNow);
                            #endregion

                            #region     曲线部分数据
                            qu_prac = new PracListClass();
                            if (quxianList[i].qu_Style == 1)
                            {
                                l_qu = quxianList[i].right_pt.X - quxianList[i].left_pt.X;
                                q_qu = quxianList[i].qu_wan * 8 / l_qu / l_qu;
                                num_qu = l_qu / 2 + 1;
                                qu_X = quxianList[i].left_pt.X;
                                qu_Y = quxianList[i].left_pt.Y;
                                qu_a = (-q_qu / 2);
                                qu_b = (quxianList[i].right_pt.Y - qu_Y + 4 * quxianList[i].qu_wan) / l_qu;
                                for (int j = 0; j < num_qu; j++)
                                {
                                    qu_pr = new PracticeClass();
                                    qu_pr.dis = 2 * j;
                                    qu_pr.prac_bend.X = qu_X + 2 * j;
                                    qu_pr.prac_bend.Y = qu_Y + qu_a * 2 * j * 2 * j + qu_b * 2 * j;
                                    qu_prac.pra_Num++;
                                    qu_prac.praList.Add(qu_pr);
                                }
                            }
                            if (quxianList[i].qu_Style == 2)
                            {
                                l_qu = quxianList[i].left_pt.Y - quxianList[i].right_pt.Y;
                                q_qu = quxianList[i].qu_wan * 8 / l_qu / l_qu;
                                num_qu = l_qu / 2 + 1;
                                qu_X = quxianList[i].left_pt.X;
                                qu_Y = quxianList[i].left_pt.Y;
                                qu_a = (-q_qu / 2);
                                qu_b = (quxianList[i].right_pt.X - qu_X + 4 * quxianList[i].qu_wan) / l_qu;
                                for (int j = 0; j < num_qu; j++)
                                {
                                    qu_pr = new PracticeClass();
                                    qu_pr.dis = 2 * j;
                                    qu_pr.prac_bend.X = qu_X + qu_a * 2 * j * 2 * j + qu_b * 2 * j;
                                    qu_pr.prac_bend.Y = qu_Y - 2 * j;
                                    qu_prac.pra_Num++;
                                    qu_prac.praList.Add(qu_pr);
                                }
                            }
                            qu_pracList.Add(qu_prac);
                            #endregion

                            #region     曲线绘图
                            for (int j = 0; j < qu_prac.pra_Num - 1; j++)
                            {
                                m_LineNow = new Line();
                                m_LineNow.X1 = qu_prac.praList[j].prac_bend.X;
                                m_LineNow.Y1 = qu_prac.praList[j].prac_bend.Y;
                                m_LineNow.X2 = qu_prac.praList[j + 1].prac_bend.X;
                                m_LineNow.Y2 = qu_prac.praList[j + 1].prac_bend.Y;
                                m_LineNow.Stroke = Brushes.Blue;
                                m_LineNow.StrokeThickness = 1;
                                can.Children.Add(m_LineNow);
                            }
                            #endregion
                        }
                        doit = false;
                    }
                }
                catch
                {
                    MessageBox.Show("练习图弯矩图绘制失败");
                }
            }
            else
            {
                MessageBox.Show("请先单击区段叠加");
            }
        }
        #endregion
        
        #endregion

        #endregion

        #region     拾取杆件状态
        private void chose1_Click(object sender, RoutedEventArgs e)
        {
            tension = 0;
        }
        #endregion
         
        #region 支反力
        private void Reaction_Click(object sender, RoutedEventArgs e)
        {
            reaction reaction1 = new reaction(m_ZhizuoList, x_dList, bilichi);
            reaction1.Show();
        }
        #endregion
        
        #endregion

        #region     帮助

        /// <summary>
        /// 练习帮助
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void help1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("点击“开始绘制杆件”，在原图（屏幕左上部）区域绘制杆件，绘制完成后单击“完成绘制”，根据提示进行下一步");
        }

        private void help2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("拖拽支座到原图的杆件上，实现对杆件的约束，约束完成后单击“完成约束”，根据提示进行下一步");
        }

        private void help3_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("输入载荷的属性（大小和长度，图中1格表示实际长度1），单击“确认输入”，然后将合适方向的载荷拖拽到原图的杆件上，实现载荷的施加，施加完成后单击“完成”，根据提示进行下一步");
        }

        private void help4_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("先输入弯矩值大小，点击“弯矩值确认”按钮，再点击练习图上的对应的关键点，完成所有关键点后，点击“绘制弯矩图”等绘制出直线段相连的弯矩图。假如外部载荷中有均布载荷，则先点击“区段叠加”，绘制虚线区段，在均布载荷部分的弯矩图上点击中点，输入弯矩值大小，点击“绘制弯矩图”");
        }
        
        #endregion
        
        #region 打分

        private int sum1;
        private double degree1, get1;

        private void score1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool doit = true;
                if (doit == true)
                {

                    #region     打分

                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        for (int j = 0; j < lian1ListList[i].pra_Num; j++)
                        {
                            if (Math.Abs(lian1ListList[i].praList[j].wanju) < 0.001)
                            {
                                lian1ListList[i].praList[j].wanju = 0;
                            }
                            if (Math.Abs(lian3ListList[i].praList[j].wanju) <0.001)
                            {
                                lian3ListList[i].praList[j].wanju = 0;
                            }
                        }
                    }

                    #region     初始化
                    sum1 = 0;
                    get1 = 0;
                    bool false1 = false;
                    bool false2 = false;
                    bool false3 = false;
                    bool false4 = false;
                    bool false5 = false;
                    bool false6 = false;
                    bool false7 = false;
                    List<Point> free = new List<Point>();

                    #endregion
                    
                    double k_sum = 0;
                    for (int k = 0; k < m_Rigid_Num; k++)
                    {

                        for (int kk = 0; kk < m_Line_Num; kk++)
                        {
                            if (m_LineModelList[kk].Line_BeginPoint == m_RigidJointList[k].J_pt)
                            {
                                k_sum += lian1ListList[kk].praList[0].wanju;
                            }
                            if (m_LineModelList[kk].Line_EndPoint == m_RigidJointList[k].J_pt)
                            {
                                int k_num = lian1ListList[kk].pra_Num - 1;
                                k_sum -= lian1ListList[kk].praList[k_num].wanju;
                            }
                        }
                    }

                    #region     错误1
                    if (k_sum != 0)
                    {
                        false1 = true;
                    }
                    #endregion

                    #region 错误3-6
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        for (int j = 0; j < lian1ListList[i].pra_Num; j++)
                        {
                            sum1++;
                            double er = 0;

                            if (lian1ListList[i].praList[j].wanju == 0)
                            {
                                #region     错误3

                                #region     固定端弯矩值为零
                                for (int k = 0; k < m_Zhizuo_Num; k++)
                                {
                                    if ((m_ZhizuoList[k].Zhizuo_pt.X + x_area /4 == lian3ListList[i].praList[j].prac_pt.X)
                                        && (m_ZhizuoList[k].Zhizuo_pt.Y - y_area / 2 == lian3ListList[i].praList[j].prac_pt.Y))
                                    {
                                        if (m_ZhizuoList[k].Zhizuo_Style == 4 || m_ZhizuoList[k].Zhizuo_Style == 5 || m_ZhizuoList[k].Zhizuo_Style == 6)
                                        {
                                            false3 = true;
                                        }
                                    }
                                }
                                #endregion

                                #region     刚节点弯矩值为零
                                for (int k = 0; k < m_Rigid_Num; k++)
                                {
                                    if ((m_RigidJointList[k].J_pt.X + x_area /4 == lian3ListList[i].praList[j].prac_pt.X)
                                        && (m_RigidJointList[k].J_pt.Y - y_area / 2 == lian3ListList[i].praList[j].prac_pt.Y))
                                    {
                                        false3 = true;
                                    }
                                }
                                #endregion
                                
                                #endregion
                            }

                            for (int k = 0; k < m_Hinge_Num; k++)
                            {
                                if ((m_HingeJointList[k].J_pt.X == Math.Round(lian3ListList[i].praList[j].prac_pt.X - x_area / 4))
                                    && (m_HingeJointList[k].J_pt.Y == Math.Round(lian3ListList[i].praList[j].prac_pt.Y + y_area / 2)))
                                {
                                    int daan =(int) Math.Round(lian3ListList[i].praList[j].wanju);
                                    int shuru =(int) Math.Round(lian1ListList[i].praList[j].wanju);
                                    if (shuru == 0 && daan != 0) 
                                    {
                                        false4 = true;
                                    }

                                    if (shuru != 0 && daan == 0) 
                                    {
                                        false5 = true;
                                    }

                                    if (shuru != 0 && daan != 0 && shuru != daan) 
                                    {
                                        false4 = true;
                                    }
                                }
                            }
                            
                            if (lian1ListList[i].praList[j].wanju != 0)
                            {
                                #region     错误6
                                for (int k = 0; k < m_Zhizuo_Num; k++)
                                {
                                    if ((m_ZhizuoList[k].Zhizuo_pt.X == lian3ListList[i].praList[j].prac_pt.X - x_area /4)
                                        && (m_ZhizuoList[k].Zhizuo_pt.Y == lian3ListList[i].praList[j].prac_pt.Y + y_area / 2))
                                    {
                                        if (m_ZhizuoList[k].Zhizuo_Style == 1 || m_ZhizuoList[k].Zhizuo_Style == 2 || m_ZhizuoList[k].Zhizuo_Style == 3)
                                        {
                                            false6 = false;
                                        }
                                    }
                                }
                                #endregion
                            }

                            if (lian3ListList[i].praList[j].wanju == 0)
                            {
                                er = lian1ListList[i].praList[j].wanju -
                                    lian3ListList[i].praList[j].wanju;

                            }
                            if (lian3ListList[i].praList[j].wanju != 0)
                            {
                                er = (lian1ListList[i].praList[j].wanju -
                                    lian3ListList[i].praList[j].wanju)
                                / lian3ListList[i].praList[j].wanju;
                                
                            }
                            if ((er < 0.05) && (er > (-0.05)))
                            {
                                get1++;

                            }
                            #region     打叉
                            if ((er > 0.05) || (er < (-0.05)))
                            {
                                m_LineNow = new Line();
                                m_LineNow.Stroke = Brushes.Red;
                                m_LineNow.StrokeThickness = 1;
                                m_LineNow.X1 = lian1ListList[i].praList[j].prac_bend.X - 10;
                                m_LineNow.Y1 = lian1ListList[i].praList[j].prac_bend.Y + 10;
                                m_LineNow.X2 = lian1ListList[i].praList[j].prac_bend.X + 10;
                                m_LineNow.Y2 = lian1ListList[i].praList[j].prac_bend.Y - 10;
                                can.Children.Add(m_LineNow);

                                m_LineNow = new Line();
                                m_LineNow.Stroke = Brushes.Red;
                                m_LineNow.StrokeThickness = 1;
                                m_LineNow.X1 = lian1ListList[i].praList[j].prac_bend.X + 10;
                                m_LineNow.Y1 = lian1ListList[i].praList[j].prac_bend.Y + 10;
                                m_LineNow.X2 = lian1ListList[i].praList[j].prac_bend.X - 10;
                                m_LineNow.Y2 = lian1ListList[i].praList[j].prac_bend.Y - 10;
                                can.Children.Add(m_LineNow);
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 错误7
                    //备选自由端
                    for (int i = 0; i < m_Line_Num; i++)              
                    {
                        free.Add(m_LineModelList[i].Line_BeginPoint);
                        free.Add(m_LineModelList[i].Line_EndPoint);
                    }
                    //去除重复点
                    for (int i = 0; i < free.Count; i++)             
                    {
                        bool tag = false;
                        for (int j = i + 1; j < free.Count; j++) 
                        {
                            if (free[i].X == free[j].X && free[i].Y == free[j].Y)
                            {
                                tag = true;
                                free.RemoveAt(j);
                            }

                        }
                        if (tag == true)
                        {
                            free.RemoveAt(i);
                            tag = false;
                        }
                    }
                    //去除支座点
                    for (int i = 0; i < free.Count; i++)              
                    {
                        for (int j = 0; j < m_Zhizuo_Num; j++)
                        {
                            if (free[i].X == m_ZhizuoList[j].Zhizuo_pt.X && free[i].Y == m_ZhizuoList[j].Zhizuo_pt.Y)
                            {
                                free.RemoveAt(i);
                            }
                        }
                    }
                    //查找关键点
                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        for (int j = 0; j < lian1ListList[i].pra_Num; j++)
                        {
                            for (int k = 0; k < free.Count; k++)
                            {
                                if ((free[k].X == Math.Round(lian3ListList[i].praList[j].prac_pt.X - x_area / 4))
                                    && (free[k].Y == Math.Round(lian3ListList[i].praList[j].prac_pt.Y + y_area / 2)))
                                {
                                    int daan = (int)Math.Round(lian3ListList[i].praList[j].wanju);
                                    int shuru = (int)Math.Round(lian1ListList[i].praList[j].wanju);
                                    if (shuru == 0 && daan != 0)
                                    {
                                        false7 = true;
                                    }
                                }
                            }

                        }
                    }

                    #endregion

                    #region     错误2：二次绘图
                    for (int j = 0; j < qu_Num; j++)
                    {
                        for (int i = 0; i < qu_Num; i++)
                        {
                            if (((quxian3List[i].left_pt.Y + y_area / 2) <= quxianList[j].left_pt.Y + 5)
                                && ((quxian3List[i].right_pt.Y + y_area / 2) <= quxianList[j].right_pt.Y + 5)
                                && ((quxian3List[i].left_pt.Y + y_area / 2) >= quxianList[j].left_pt.Y - 5)
                                && ((quxian3List[i].right_pt.Y + y_area / 2) >= quxianList[j].right_pt.Y - 5))
                            {
                                #region     错误2
                                if (quxianList[j].qu_M * quxian3List[i].qu_M < 0)
                                {
                                    false2 = true;
                                }
                                #endregion

                                double err = (quxianList[j].qu_M - quxian3List[i].qu_M) / quxian3List[i].qu_M;
                                sum1++;
                                if ((err < 0.05) && (err > 0 - .05))
                                {
                                    get1++;
                                }
                                if ((err > 0.05) || (err < -0.05))
                                {
                                    m_LineNow = new Line();
                                    m_LineNow.StrokeThickness = 1;
                                    m_LineNow.Stroke = Brushes.Red;
                                    m_LineNow.X1 = quxianList[j].bend_pt.X - 10;
                                    m_LineNow.Y1 = quxianList[j].bend_pt.Y + 10;
                                    m_LineNow.X2 = quxianList[j].bend_pt.X + 10;
                                    m_LineNow.Y2 = quxianList[j].bend_pt.Y - 10;
                                    can.Children.Add(m_LineNow);

                                    m_LineNow = new Line();
                                    m_LineNow.StrokeThickness = 1;
                                    m_LineNow.Stroke = Brushes.Red;
                                    m_LineNow.X1 = quxianList[j].bend_pt.X - 10;
                                    m_LineNow.Y1 = quxianList[j].bend_pt.Y - 10;
                                    m_LineNow.X2 = quxianList[j].bend_pt.X + 10;
                                    m_LineNow.Y2 = quxianList[j].bend_pt.Y + 10;
                                    can.Children.Add(m_LineNow);
                                }

                            }

                            if ((quxian3List[i].left_pt.X+x_area / 4 <= quxianList[j].left_pt.X + 5)
                                && (quxian3List[i].right_pt.X+x_area / 4 <= quxianList[j].right_pt.X + 5)
                                && (quxian3List[i].left_pt.X+x_area / 4 >= quxianList[j].left_pt.X - 5)
                                && (quxian3List[i].right_pt.X+x_area / 4 >= quxianList[j].right_pt.X - 5))
                            {
                                #region     错误2
                                if (quxianList[j].qu_M * quxian3List[i].qu_M < 0)
                                {
                                    false2 = true;
                                }
                                #endregion

                                double err = (quxianList[j].qu_M - quxian3List[i].qu_M) / quxian3List[i].qu_M;
                                sum1++;
                                if ((err < 0.05) && (err > -0.05))
                                {
                                    get1++;
                                }
                                if ((err > 0.05) || (err < -0.05))
                                {
                                    m_LineNow = new Line();
                                    m_LineNow.StrokeThickness = 1;
                                    m_LineNow.Stroke = Brushes.Red;
                                    m_LineNow.X1 = quxianList[j].bend_pt.X - 10;
                                    m_LineNow.Y1 = quxianList[j].bend_pt.Y + 10;
                                    m_LineNow.X2 = quxianList[j].bend_pt.X + 10;
                                    m_LineNow.Y2 = quxianList[j].bend_pt.Y - 10;
                                    can.Children.Add(m_LineNow);

                                    m_LineNow = new Line();
                                    m_LineNow.StrokeThickness = 1;
                                    m_LineNow.Stroke = Brushes.Red;
                                    m_LineNow.X1 = quxianList[j].bend_pt.X - 10;
                                    m_LineNow.Y1 = quxianList[j].bend_pt.Y - 10;
                                    m_LineNow.X2 = quxianList[j].bend_pt.X + 10;
                                    m_LineNow.Y2 = quxianList[j].bend_pt.Y + 10;
                                    can.Children.Add(m_LineNow);
                                }
                            }
                        }
                    }
                    #endregion

                    degree1 = get1 * 1.0 / sum1 * 100;
                    MessageBox.Show("您本次的得分为;" + Math.Round(degree1, 1));

                    #endregion

                    #region     后处理

                    double x_area1 = x_area - 240;

                    if (Math.Round(degree1, 1) != 100)
                    {
                        #region     错误1
                        if (false1 == true)
                        {
                            TextBlock tb11 = new TextBlock();
                            tb11.Text = "错误1：刚节点不平衡";
                            tb11.FontSize = 16;
                            var tb11X = x_area1 - 150;
                            var tb11Y = y_area / 2 + 75;
                            ScaleTransform f_TBrulerscale = new ScaleTransform();
                            f_TBrulerscale.ScaleY = -1;
                            tb11.RenderTransform = f_TBrulerscale;
                            tb11.Background = Brushes.Yellow;
                            tb11.Margin = new Thickness(tb11X, tb11Y, 0, 0);
                            can.Children.Add(tb11);
                        }
                        #endregion

                        #region     错误2
                        if (false2 == true)
                        {
                            TextBlock tb12 = new TextBlock();
                            tb12.Text = "错误2：抛物线凹凸方向不对";
                            tb12.FontSize = 16;
                            var tb12X = x_area1 - 150;
                            var tb12Y = y_area / 2 + 50;
                            ScaleTransform f_TBrulerscale = new ScaleTransform();
                            f_TBrulerscale.ScaleY = -1;
                            tb12.RenderTransform = f_TBrulerscale;
                            tb12.Background = Brushes.Yellow;
                            tb12.Margin = new Thickness(tb12X, tb12Y, 0, 0);
                            can.Children.Add(tb12);
                        }
                        #endregion

                        #region     错误3
                        if (false3 == true)
                        {
                            TextBlock tb13 = new TextBlock();
                            tb13.Text = "错误3：刚节点或固定端弯矩一般不为零";
                            tb13.FontSize = 16;
                            var tb13X = x_area1 - 150;
                            var tb13Y = y_area / 2 + 25;
                            ScaleTransform f_TBrulerscale = new ScaleTransform();
                            f_TBrulerscale.ScaleY = -1;
                            tb13.RenderTransform = f_TBrulerscale;
                            tb13.Background = Brushes.Yellow;
                            tb13.Margin = new Thickness(tb13X, tb13Y, 0, 0);
                            can.Children.Add(tb13);
                        }
                        #endregion

                        #region     错误4
                        if (false4 == true)
                        {
                            TextBlock tb14 = new TextBlock();
                            tb14.Text = "错误4：铰节点有集中力偶作用时弯矩值等于外力偶值";
                            tb14.FontSize = 16;
                            var tb14X = x_area1 - 150;
                            var tb14Y = y_area / 2;
                            ScaleTransform f_TBrulerscale = new ScaleTransform();
                            f_TBrulerscale.ScaleY = -1;
                            tb14.RenderTransform = f_TBrulerscale;
                            tb14.Background = Brushes.Yellow;
                            tb14.Margin = new Thickness(tb14X, tb14Y, 0, 0);
                            can.Children.Add(tb14);
                        }
                        #endregion

                        #region     错误5
                        if (false5 == true)
                        {
                            TextBlock tb15 = new TextBlock();
                            tb15.Text = "错误5：铰节点无集中力偶作用时弯矩应为零";
                            tb15.FontSize = 16;
                            var tb15X = x_area1 - 150;
                            var tb15Y = y_area / 2 - 25;
                            ScaleTransform f_TBrulerscale = new ScaleTransform();
                            f_TBrulerscale.ScaleY = -1;
                            tb15.RenderTransform = f_TBrulerscale;
                            tb15.Background = Brushes.Yellow;
                            tb15.Margin = new Thickness(tb15X, tb15Y, 0, 0);
                            can.Children.Add(tb15);
                        }
                        #endregion

                        #region     错误6
                        if (false6 == true)
                        {
                            TextBlock tb16 = new TextBlock();
                            tb16.Text = "错误6：铰支座无集中力偶作用时弯矩应为零";
                            tb16.FontSize = 16;
                            var tb16X = x_area1 - 150;
                            var tb16Y = y_area / 2 - 50;
                            ScaleTransform f_TBrulerscale = new ScaleTransform();
                            f_TBrulerscale.ScaleY = -1;
                            tb16.RenderTransform = f_TBrulerscale;
                            tb16.Background = Brushes.Yellow;
                            tb16.Margin = new Thickness(tb16X, tb16Y, 0, 0);
                            can.Children.Add(tb16);
                        }
                        #endregion

                        #region     错误7
                        if (false7 == true)
                        {
                            TextBlock tb17 = new TextBlock();
                            tb17.Text = "错误7：自由端有集中力偶作用时弯矩不为零";
                            tb17.FontSize = 16;
                            var tb17X = x_area1 - 150;
                            var tb17Y = y_area / 2 - 75;
                            ScaleTransform f_TBrulerscale = new ScaleTransform();
                            f_TBrulerscale.ScaleY = -1;
                            tb17.RenderTransform = f_TBrulerscale;
                            tb17.Background = Brushes.Yellow;
                            tb17.Margin = new Thickness(tb17X, tb17Y, 0, 0);
                            can.Children.Add(tb17);
                        }
                        #endregion

                    }
                    #endregion

                    doit = false;
                }
                if (degree1 != 100)
                {
                    MessageBox.Show("请单击“参考答案”按钮查看正确答案");
                }
                if (degree1 == 100)
                {
                    order = 6;
                    MessageBox.Show("恭喜你完成练习");
                }
            }
            catch
            {
                MessageBox.Show("练习图评分错误");
            }
}

        #endregion

    }
}