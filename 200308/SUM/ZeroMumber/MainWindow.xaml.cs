using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ZeroMumber
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region     初始化

        #region     清空

        /// <summary>
        /// 清空当前显示部分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void qingkong(object sender, RoutedEventArgs e)
        {
            order = 0;

            #region     列表清空

            //杆件列表清空
            m_LineModelList.Clear();
            //结点列表清空
            m_JointModelList.Clear();
            //载荷列表清空
            m_LoadModelList.Clear();
            //点列表清空
            pointList.Clear();
            // 画板清空
            can.Children.Clear();

            #endregion

            #region     初始值清空

            m_Line_Num = 0;
            m_Joint_Num = 0;
            m_Load_Num = 0;
            yueshu = 0;
            jiaozhizuo = 0;
            lianganX = 0;
            lianganY = 0;

            #endregion

            startPoint = new Point();
            m_LineNow = new Line();
            m_LineModel = new LineModelClass();
            m_JointModel = new JointModelClass();
            this.menuTabctrl.SelectedItem = this.menuTabctrl.Items[0];

            Paint = false;
            Zhizuo = false;
            Choose = false;
            Load = false;
        }

        #endregion

        #region 变量声明

        #region     杆件变量

        //网格背景 brush//
        private DrawingBrush _gridBrush;

        /// <summary>
        /// 当前绘制的Line的端点
        /// </summary>
        private Point startPoint;
        private Point endPoint;
        private Point transPoint;

        //当前划线信息
        private Line m_LineNow = new Line();

        /// <summary>
        /// 当前绘制的Line模型
        /// </summary>
        private LineModelClass m_LineModel = new LineModelClass();

        /// <summary>
        /// 当前绘制的所有Line集合
        /// </summary>
        private List<LineModelClass> m_LineModelList = new List<LineModelClass>();

        /// <summary>
        /// 鼠标划线汇集点的集合
        /// </summary>
        private List<Point> pointList = new List<Point>();

        /// <summary>
        /// 当前Line从0开始
        /// </summary>
        public int m_Line_Num = 0;

        #endregion

        #region 节点变量

        /// <summary>
        /// 当前生成的Joint模型
        /// </summary>
        private JointModelClass m_JointModel = new JointModelClass();

        /// <summary>
        /// 当前生成的所有Joint集合
        /// </summary>
        private List<JointModelClass> m_JointModelList = new List<JointModelClass>();

        /// <summary>
        /// 当前Joint数 从0开始
        /// </summary>
        public int m_Joint_Num = 0;

        #endregion

        #region 约束变量

        int yueshu;
        int jiaozhizuo;
        int lianganX;
        int lianganY;

        #endregion 

        #region     载荷变量

        //当前划线信息
        private Line m_LoadNow = new Line();

        /// <summary>
        /// 当前绘制的Load模型
        /// </summary>
        private LoadModelClass m_LoadModel = new LoadModelClass();

        /// <summary>
        /// 当前绘制的所有Load集合
        /// </summary>
        private List<LoadModelClass> m_LoadModelList = new List<LoadModelClass>();

        /// <summary>
        /// 当前Load从0开始
        /// </summary>
        public int m_Load_Num = 0;

        #endregion

        #region     界面触发函数变量声明

        private bool Paint = false;
        private bool Zhizuo = false;
        private bool Load = false;
        private bool Choose = false;
        private int Zhizuo_Style = 0;

        #endregion

        #region 程序控制函数变量声明

        private double x_area = SystemParameters.WorkArea.Width - 200;
        private double y_area = SystemParameters.WorkArea.Height - 90;

        TextBlock f_TB = new TextBlock();
        TextBlock f_TC = new TextBlock();
        TextBlock f_TD = new TextBlock();

        Line m_lineB = new Line();
        Line m_lineC = new Line();

        /// <summary>
        /// 秩序函数，交互界面每个button对应一个秩序
        /// </summary>
        private int order = 0;
        private DoubleCollection dianhuaxian = new DoubleCollection { 5, 2, 1, 2 };
        /// <summary>
        /// 自动生成节点时，判断该节点是否已经存在
        /// </summary>
        private bool Flag = false;

        #endregion

        #endregion

        #endregion

        #region 界面触发函数

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

        #endregion

        #region 鼠标按下事件

        /// <summary>
        /// 鼠标左键点击canvas函数 获取鼠标绘制线段的起点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void can_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            startPoint = e.GetPosition(can);

            if (Paint == true)
            {
                //初始化 Line 
                m_LineNow = new Line();

                if ((startPoint.X < x_area) && (startPoint.Y < y_area))
                {
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
                    can.Children.Add(m_LineNow);

                    f_TB = new TextBlock();
                    can.Children.Add(f_TB);
                    f_TD = new TextBlock();
                    can.Children.Add(f_TD);

                    m_lineB = new Line();
                    m_lineB.X1 = m_LineNow.X1;
                    m_lineB.Y1 = m_LineNow.Y1;
                    m_lineB.X2 = m_LineNow.X1;
                    m_lineB.Y2 = m_LineNow.Y1;
                    m_lineB.Stroke = Brushes.Red;
                    m_lineB.StrokeThickness = 1;
                    can.Children.Add(m_lineB);

                    m_lineC = new Line();
                    m_lineC.Y1 = m_LineNow.Y1;
                    m_lineC.Stroke = Brushes.Red;
                    m_lineC.StrokeThickness = 1;
                    can.Children.Add(m_lineC);

                }
            }

            if (Load == true)
            {
                m_LoadNow = new Line();

                #region 建立零载荷量

                if (m_LoadModelList.Count == 0)
                {
                    m_LoadModel.Load_X = 0;
                    m_LoadModel.Load_Y = 0;
                    m_LoadModelList.Add(m_LoadModel);
                    m_LoadModel = new LoadModelClass();
                }

                #endregion

                for (int i = 0; i < m_Joint_Num; i++)
                {
                    if (m_JointModelList[i].Joint_Point.X - 6 < startPoint.X && m_JointModelList[i].Joint_Point.X + 6 > startPoint.X && m_JointModelList[i].Joint_Point.Y - 6 < startPoint.Y && m_JointModelList[i].Joint_Point.Y + 6 > startPoint.Y)
                    {
                        for (int j = 0; j < m_Load_Num + 1; j++)
                        {
                            if (m_LoadModelList[j].Load_Joint == i && (m_LoadModelList[j].Load_X != 0 || m_LoadModelList[j].Load_Y != 0))
                            {
                                MessageBox.Show("该节点已存在外载荷，请勿重复添加");
                            }
                            else
                            {
                                m_LoadNow.X1 = (Math.Round(startPoint.X / 20) * 20);
                                m_LoadNow.Y1 = (Math.Round(startPoint.Y / 20) * 20);
                                m_LoadModel.Load_Joint = i;
                            }
                        }
                    }
                }
                can.Children.Add(m_LoadNow);
                f_TB = new TextBlock();
                can.Children.Add(f_TB);
                f_TC = new TextBlock();
                can.Children.Add(f_TC);
                f_TD = new TextBlock();
                can.Children.Add(f_TD);

                m_lineB = new Line();
                m_lineB.X1 = m_LoadNow.X1;
                m_lineB.Y1 = m_LoadNow.Y1;
                m_lineB.X2 = m_LoadNow.X1;
                m_lineB.Y2 = m_LoadNow.Y1;
                m_lineB.Stroke = Brushes.Red;
                m_lineB.StrokeThickness = 1;
                can.Children.Add(m_lineB);

                m_lineC = new Line();
                m_lineC.Y1 = m_LoadNow.Y1;
                m_lineC.Stroke = Brushes.Red;
                m_lineC.StrokeThickness = 1;
                can.Children.Add(m_lineC);

            }

            if (Choose == true)
            {
                Line newLine = new Line();
                int line1 = 0;
                int line2 = 0;
                int Joint1 = 0;
                int Joint2 = 0;
                int Load = 0;

                for (int i = 0; i < m_Line_Num; i++)
                {
                    if (m_LineModelList[i].Line_Style == false)
                    {
                        if (m_LineModelList[i].Line_BeginPoint.X < startPoint.X && m_LineModelList[i].Line_EndPoint.X > startPoint.X)
                        {
                            if ((startPoint.X - m_LineModelList[i].Line_BeginPoint.X) * m_LineModelList[i].Line_Angle + m_LineModelList[i].Line_BeginPoint.Y - 4 < startPoint.Y
                                && startPoint.Y < (startPoint.X - m_LineModelList[i].Line_BeginPoint.X) * m_LineModelList[i].Line_Angle + m_LineModelList[i].Line_BeginPoint.Y + 4)
                            {
                                if (m_LineModelList[i].Line_IsZero == true)
                                {
                                    MessageBox.Show("已经是零杆");
                                }

                                if (m_LineModelList[i].Line_IsZero == false)
                                {
                                    switch (m_JointModelList[m_LineModelList[i].Line_Joint1].Joint_Count)
                                    {
                                        case 1:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;

                                            #endregion

                                            #region 判断零杆

                                            if (m_JointModelList[Joint1].Joint_Load == 0 ||
                                                (m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_Y == 0 && m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_X == 0))
                                            {
                                                m_LineModelList[i].Line_IsZero = true;
                                            }

                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true ||
                                                (m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_Y == 0 && m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_X == 0))
                                            {
                                                m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;

                                                for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Red;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);

                                                MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                            }

                                            #endregion

                                            break;

                                        case 2:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;
                                            if (i == m_JointModelList[Joint1].Joint_Line[0])
                                            {
                                                line1 = m_JointModelList[Joint1].Joint_Line[1];
                                            }
                                            if (i == m_JointModelList[Joint1].Joint_Line[1])
                                            {
                                                line1 = m_JointModelList[Joint1].Joint_Line[0];
                                            }
                                            Load = m_JointModelList[Joint1].Joint_Load;

                                            #endregion

                                            #region 判断零杆

                                            if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                            {
                                                if (!(m_LineModelList[line1].Line_Style == true && m_LineModelList[i].Line_Style == true) &&
                                                    !(m_LineModelList[line1].Line_Style == false && m_LineModelList[i].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[i].Line_Angle))
                                                {
                                                    m_LineModelList[i].Line_IsZero = true;
                                                }
                                            }

                                            if (Load != 0 && (m_LoadModelList[Load].Load_X != 0 || m_LoadModelList[Load].Load_Y != 0))
                                            {
                                                if (!(m_LineModelList[line1].Line_Style == true && m_LineModelList[i].Line_Style == true) &&
                                                    !(m_LineModelList[line1].Line_Style == false && m_LineModelList[i].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[i].Line_Angle))
                                                {

                                                    if (m_LoadModelList[Load].Load_Style == true)
                                                    {
                                                        if (m_LineModelList[line1].Line_Style == true)
                                                        {
                                                            m_LineModelList[i].Line_IsZero = true;
                                                        }
                                                    }

                                                    if (m_LoadModelList[Load].Load_Style == false && m_LineModelList[line1].Line_Style == false)
                                                    {
                                                        if (m_LineModelList[line1].Line_Angle == m_LoadModelList[Load].Load_Angle)
                                                        {
                                                            m_LineModelList[i].Line_IsZero = true;
                                                        }

                                                    }

                                                }

                                            }

                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true)
                                            {
                                                m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                m_JointModelList[Joint1].Joint_Line[0] = line1;

                                                for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Red;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);

                                                MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                            }

                                            #endregion

                                            break;

                                        case 3:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;
                                            if (i == m_JointModelList[Joint1].Joint_Line[0])
                                            {
                                                line1 = m_JointModelList[Joint1].Joint_Line[1];
                                                line2 = m_JointModelList[Joint1].Joint_Line[2];
                                            }
                                            if (i == m_JointModelList[Joint1].Joint_Line[1])
                                            {
                                                line1 = m_JointModelList[Joint1].Joint_Line[0];
                                                line2 = m_JointModelList[Joint1].Joint_Line[2];
                                            }
                                            if (i == m_JointModelList[Joint1].Joint_Line[2])
                                            {
                                                line1 = m_JointModelList[Joint1].Joint_Line[0];
                                                line2 = m_JointModelList[Joint1].Joint_Line[1];
                                            }
                                            Load = m_JointModelList[Joint1].Joint_Load;

                                            #endregion

                                            #region 判断零杆
                                            if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                            {
                                                if ((m_LineModelList[line1].Line_Style == false && m_LineModelList[line2].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[line2].Line_Angle) ||
                                                    (m_LineModelList[line1].Line_Style == true && m_LineModelList[line2].Line_Style == true))
                                                {
                                                    m_LineModelList[i].Line_IsZero = true;
                                                }
                                            }
                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true)
                                            {
                                                m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                m_JointModelList[Joint1].Joint_Line[0] = line1;
                                                m_JointModelList[Joint1].Joint_Line[1] = line2;

                                                for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Red;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);

                                                MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                            }

                                            #endregion

                                            break;

                                        default:

                                            break;
                                    }

                                    if (m_LineModelList[i].Line_IsZero == false)
                                    {
                                        switch (m_JointModelList[m_LineModelList[i].Line_Joint2].Joint_Count)
                                        {
                                            case 1:

                                                #region 读取结点数据

                                                Joint1 = m_LineModelList[i].Line_Joint1;
                                                Joint2 = m_LineModelList[i].Line_Joint2;

                                                #endregion

                                                #region 判断零杆

                                                if (m_JointModelList[Joint2].Joint_Load == 0 ||
                                                    (m_LoadModelList[m_JointModelList[Joint2].Joint_Load].Load_Y == 0 && m_LoadModelList[m_JointModelList[Joint2].Joint_Load].Load_X == 0))
                                                {
                                                    m_LineModelList[i].Line_IsZero = true;
                                                }

                                                #endregion

                                                #region 判断结果反馈

                                                if (m_LineModelList[i].Line_IsZero == true)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;

                                                    for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                    {
                                                        if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                            for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                            {
                                                                m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                            }
                                                        }
                                                    }

                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.White;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);
                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.Red;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);

                                                    MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                                }

                                                if (m_LineModelList[i].Line_IsZero == false)
                                                {
                                                    MessageBox.Show("当前条件下，杆件" + (i + 1) + "不是零杆");
                                                }

                                                #endregion

                                                break;

                                            case 2:

                                                #region 读取结点数据

                                                Joint1 = m_LineModelList[i].Line_Joint1;
                                                Joint2 = m_LineModelList[i].Line_Joint2;
                                                if (i == m_JointModelList[Joint2].Joint_Line[0])
                                                {
                                                    line1 = m_JointModelList[Joint2].Joint_Line[1];
                                                }
                                                if (i == m_JointModelList[Joint2].Joint_Line[1])
                                                {
                                                    line1 = m_JointModelList[Joint2].Joint_Line[0];
                                                }
                                                Load = m_JointModelList[Joint2].Joint_Load;

                                                #endregion

                                                #region 判断零杆

                                                if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                                {
                                                    if (!(m_LineModelList[line1].Line_Style == true && m_LineModelList[i].Line_Style == true) &&
                                                        !(m_LineModelList[line1].Line_Style == false && m_LineModelList[i].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[i].Line_Angle))
                                                    {
                                                        m_LineModelList[i].Line_IsZero = true;
                                                    }
                                                }

                                                if (Load != 0 && (m_LoadModelList[Load].Load_X != 0 || m_LoadModelList[Load].Load_Y != 0))
                                                {
                                                    if (!(m_LineModelList[line1].Line_Style == true && m_LineModelList[i].Line_Style == true) &&
                                                        !(m_LineModelList[line1].Line_Style == false && m_LineModelList[i].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[i].Line_Angle))
                                                    {
                                                        if (m_LoadModelList[Load].Load_Style == true)
                                                        {
                                                            if (m_LineModelList[line1].Line_Style == true)
                                                            {
                                                                m_LineModelList[i].Line_IsZero = true;
                                                            }
                                                        }

                                                        if (m_LoadModelList[Load].Load_Style == false && m_LineModelList[line1].Line_Style == false)
                                                        {
                                                            if (m_LineModelList[line1].Line_Angle == m_LoadModelList[Load].Load_Angle)
                                                            {
                                                                m_LineModelList[i].Line_IsZero = true;
                                                            }
                                                        }

                                                    }

                                                }

                                                #endregion

                                                #region 判断结果反馈

                                                if (m_LineModelList[i].Line_IsZero == true)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                    m_JointModelList[Joint2].Joint_Line[0] = line1;

                                                    for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                    {
                                                        if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                            for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                            {
                                                                m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                            }
                                                        }
                                                    }

                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.White;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);
                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.Red;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);

                                                    MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                                }

                                                if (m_LineModelList[i].Line_IsZero == false)
                                                {
                                                    MessageBox.Show("当前条件下，杆件" + (i + 1) + "不是零杆");
                                                }

                                                #endregion

                                                break;

                                            case 3:

                                                #region 读取结点数据

                                                Joint1 = m_LineModelList[i].Line_Joint1;
                                                Joint2 = m_LineModelList[i].Line_Joint2;
                                                if (i == m_JointModelList[Joint2].Joint_Line[0])
                                                {
                                                    line1 = m_JointModelList[Joint2].Joint_Line[1];
                                                    line2 = m_JointModelList[Joint2].Joint_Line[2];
                                                }
                                                if (i == m_JointModelList[Joint2].Joint_Line[1])
                                                {
                                                    line1 = m_JointModelList[Joint2].Joint_Line[0];
                                                    line2 = m_JointModelList[Joint2].Joint_Line[2];
                                                }
                                                if (i == m_JointModelList[Joint2].Joint_Line[2])
                                                {
                                                    line1 = m_JointModelList[Joint2].Joint_Line[0];
                                                    line2 = m_JointModelList[Joint2].Joint_Line[1];
                                                }
                                                Load = m_JointModelList[Joint2].Joint_Load;

                                                #endregion

                                                #region 判断零杆
                                                if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                                {
                                                    if ((m_LineModelList[line1].Line_Style == false && m_LineModelList[line2].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[line2].Line_Angle) ||
                                                        (m_LineModelList[line1].Line_Style == true && m_LineModelList[line2].Line_Style == true))
                                                    {
                                                        m_LineModelList[i].Line_IsZero = true;
                                                    }
                                                }
                                                #endregion

                                                #region 判断结果反馈

                                                if (m_LineModelList[i].Line_IsZero == true)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                    m_JointModelList[Joint2].Joint_Line[0] = line1;
                                                    m_JointModelList[Joint2].Joint_Line[1] = line2;

                                                    for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                    {
                                                        if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                            for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                            {
                                                                m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                            }
                                                        }
                                                    }

                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.White;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);
                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.Red;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);

                                                    MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                                }

                                                if (m_LineModelList[i].Line_IsZero == false)
                                                {
                                                    MessageBox.Show("当前条件下，杆件" + (i + 1) + "不是零杆");
                                                }

                                                #endregion

                                                break;

                                            default:

                                                if (m_LineModelList[i].Line_IsZero == false)
                                                {
                                                    MessageBox.Show("当前条件下，杆件" + (i + 1) + "不是零杆");
                                                }

                                                break;
                                        }
                                    }
                                }

                            }
                        }
                    }

                    if (m_LineModelList[i].Line_Style == true)
                    {
                        if (m_LineModelList[i].Line_BeginPoint.Y < startPoint.Y && startPoint.Y < m_LineModelList[i].Line_EndPoint.Y)
                        {
                            if (m_LineModelList[i].Line_BeginPoint.X - 4 < startPoint.X && startPoint.X < m_LineModelList[i].Line_BeginPoint.X + 4)
                            {
                                if (m_LineModelList[i].Line_IsZero == true)
                                {
                                    MessageBox.Show("已经是零杆");
                                }

                                if (m_LineModelList[i].Line_IsZero == false)
                                {
                                    switch (m_JointModelList[m_LineModelList[i].Line_Joint1].Joint_Count)
                                    {
                                        case 1:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;

                                            #endregion

                                            #region 判断零杆

                                            if (m_JointModelList[Joint1].Joint_Load == 0 ||
                                                (m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_Y == 0 && m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_X == 0))
                                            {
                                                m_LineModelList[i].Line_IsZero = true;
                                            }

                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true)
                                            {
                                                m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;

                                                for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Red;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);

                                                MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                            }

                                            #endregion

                                            break;

                                        case 2:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;
                                            if (i == m_JointModelList[Joint1].Joint_Line[0])
                                            {
                                                line1 = m_JointModelList[Joint1].Joint_Line[1];
                                            }
                                            if (i == m_JointModelList[Joint1].Joint_Line[1])
                                            {
                                                line1 = m_JointModelList[Joint1].Joint_Line[0];
                                            }
                                            Load = m_JointModelList[Joint1].Joint_Load;

                                            #endregion

                                            #region 判断零杆

                                            if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                            {
                                                if (m_LineModelList[line1].Line_Style == false)
                                                {
                                                    m_LineModelList[i].Line_IsZero = true;
                                                }
                                            }

                                            if (Load != 0 && (m_LoadModelList[Load].Load_X != 0 || m_LoadModelList[Load].Load_Y != 0))
                                            {
                                                if (m_LineModelList[line1].Line_Style == false)
                                                {
                                                    if (m_LoadModelList[Load].Load_Style == false)
                                                    {
                                                        if (m_LineModelList[line1].Line_Angle == m_LoadModelList[Load].Load_Angle)
                                                        {
                                                            m_LineModelList[i].Line_IsZero = true;
                                                        }
                                                    }
                                                }

                                            }

                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true)
                                            {
                                                m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                m_JointModelList[Joint1].Joint_Line[0] = line1;

                                                for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Red;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);

                                                MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                            }

                                            #endregion

                                            break;

                                        case 3:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;

                                            if (i == m_JointModelList[Joint1].Joint_Line[0])
                                            {
                                                line1 = m_JointModelList[Joint1].Joint_Line[1];
                                                line2 = m_JointModelList[Joint1].Joint_Line[2];
                                            }
                                            if (i == m_JointModelList[Joint1].Joint_Line[1])
                                            {
                                                line1 = m_JointModelList[Joint1].Joint_Line[0];
                                                line2 = m_JointModelList[Joint1].Joint_Line[2];
                                            }
                                            if (i == m_JointModelList[Joint1].Joint_Line[2])
                                            {
                                                line1 = m_JointModelList[Joint1].Joint_Line[0];
                                                line2 = m_JointModelList[Joint1].Joint_Line[1];
                                            }
                                            Load = m_JointModelList[Joint1].Joint_Load;

                                            #endregion

                                            #region 判断零杆
                                            if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                            {
                                                if ((m_LineModelList[line1].Line_Style == false && m_LineModelList[line2].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[line2].Line_Angle) ||
                                                    (m_LineModelList[line1].Line_Style == true && m_LineModelList[line2].Line_Style == true))
                                                {
                                                    m_LineModelList[i].Line_IsZero = true;
                                                }
                                            }
                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true)
                                            {
                                                m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                m_JointModelList[Joint1].Joint_Line[0] = line1;
                                                m_JointModelList[Joint1].Joint_Line[1] = line2;

                                                for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Red;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);

                                                MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                            }

                                            #endregion

                                            break;

                                        default:

                                            break;
                                    }

                                    if (m_LineModelList[i].Line_IsZero == false)
                                    {
                                        switch (m_JointModelList[m_LineModelList[i].Line_Joint2].Joint_Count)
                                        {
                                            case 1:

                                                #region 读取结点数据

                                                Joint1 = m_LineModelList[i].Line_Joint1;
                                                Joint2 = m_LineModelList[i].Line_Joint2;

                                                #endregion

                                                #region 判断零杆

                                                if (m_JointModelList[Joint2].Joint_Load == 0 ||
                                                    (m_LoadModelList[m_JointModelList[Joint2].Joint_Load].Load_Y == 0 && m_LoadModelList[m_JointModelList[Joint2].Joint_Load].Load_X == 0))
                                                {
                                                    m_LineModelList[i].Line_IsZero = true;
                                                }

                                                #endregion

                                                #region 判断结果反馈

                                                if (m_LineModelList[i].Line_IsZero == true)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;

                                                    for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                    {
                                                        if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                            for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                            {
                                                                m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                            }
                                                        }
                                                    }

                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.White;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);
                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.Red;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);

                                                    MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                                }

                                                if (m_LineModelList[i].Line_IsZero == false)
                                                {
                                                    MessageBox.Show("当前条件下，杆件" + (i + 1) + "不是零杆");
                                                }

                                                #endregion

                                                break;

                                            case 2:

                                                #region 读取结点数据

                                                Joint1 = m_LineModelList[i].Line_Joint1;
                                                Joint2 = m_LineModelList[i].Line_Joint2;
                                                if (i == m_JointModelList[Joint2].Joint_Line[0])
                                                {
                                                    line1 = m_JointModelList[Joint2].Joint_Line[1];
                                                }
                                                if (i == m_JointModelList[Joint2].Joint_Line[1])
                                                {
                                                    line1 = m_JointModelList[Joint2].Joint_Line[0];
                                                }
                                                Load = m_JointModelList[Joint2].Joint_Load;

                                                #endregion

                                                #region 判断零杆

                                                if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                                {
                                                    if (m_LineModelList[line1].Line_Style == false)
                                                    {
                                                        m_LineModelList[i].Line_IsZero = true;
                                                    }
                                                }

                                                if (Load != 0 && (m_LoadModelList[Load].Load_X != 0 || m_LoadModelList[Load].Load_Y != 0))
                                                {
                                                    if (m_LineModelList[line1].Line_Style == false)
                                                    {
                                                        if (m_LoadModelList[Load].Load_Style == false)
                                                        {
                                                            if (m_LineModelList[line1].Line_Angle == m_LoadModelList[Load].Load_Angle)
                                                            {
                                                                m_LineModelList[i].Line_IsZero = true;
                                                            }
                                                        }
                                                    }

                                                }

                                                #endregion

                                                #region 判断结果反馈

                                                if (m_LineModelList[i].Line_IsZero == true)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                    m_JointModelList[Joint2].Joint_Line[0] = line1;

                                                    for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                    {
                                                        if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                            for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                            {
                                                                m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                            }
                                                        }
                                                    }

                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.White;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);
                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.Red;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);

                                                    MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                                }

                                                if (m_LineModelList[i].Line_IsZero == false)
                                                {
                                                    MessageBox.Show("当前条件下，杆件" + (i + 1) + "不是零杆");
                                                }

                                                #endregion

                                                break;

                                            case 3:

                                                #region 读取结点数据

                                                Joint1 = m_LineModelList[i].Line_Joint1;
                                                Joint2 = m_LineModelList[i].Line_Joint2;
                                                if (i == m_JointModelList[Joint2].Joint_Line[0])
                                                {
                                                    line1 = m_JointModelList[Joint2].Joint_Line[1];
                                                    line2 = m_JointModelList[Joint2].Joint_Line[2];
                                                }
                                                if (i == m_JointModelList[Joint2].Joint_Line[1])
                                                {
                                                    line1 = m_JointModelList[Joint2].Joint_Line[0];
                                                    line2 = m_JointModelList[Joint2].Joint_Line[2];
                                                }
                                                if (i == m_JointModelList[Joint2].Joint_Line[2])
                                                {
                                                    line1 = m_JointModelList[Joint2].Joint_Line[0];
                                                    line2 = m_JointModelList[Joint2].Joint_Line[1];
                                                }
                                                Load = m_JointModelList[Joint2].Joint_Load;

                                                #endregion

                                                #region 判断零杆
                                                if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                                {
                                                    if ((m_LineModelList[line1].Line_Style == false && m_LineModelList[line2].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[line2].Line_Angle) ||
                                                        (m_LineModelList[line1].Line_Style == true && m_LineModelList[line2].Line_Style == true))
                                                    {
                                                        m_LineModelList[i].Line_IsZero = true;
                                                    }
                                                }
                                                #endregion

                                                #region 判断结果反馈

                                                if (m_LineModelList[i].Line_IsZero == true)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                    m_JointModelList[Joint2].Joint_Line[0] = line1;
                                                    m_JointModelList[Joint2].Joint_Line[1] = line2;

                                                    for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                    {
                                                        if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                            for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                            {
                                                                m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                            }
                                                        }
                                                    }

                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.White;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);
                                                    newLine = new Line();
                                                    newLine.Stroke = Brushes.Red;
                                                    newLine.StrokeThickness = 1;
                                                    newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                    newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                    newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                    newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                    can.Children.Add(newLine);

                                                    MessageBox.Show("杆件" + (i + 1) + "是零杆");
                                                }

                                                if (m_LineModelList[i].Line_IsZero == false)
                                                {
                                                    MessageBox.Show("当前条件下，杆件" + (i + 1) + "不是零杆");
                                                }

                                                #endregion

                                                break;

                                            default:

                                                if (m_LineModelList[i].Line_IsZero == false)
                                                {
                                                    MessageBox.Show("当前条件下，杆件" + (i + 1) + "不是零杆");
                                                }

                                                break;
                                        }
                                    }

                                }
                            }
                        }
                    }

                }
            }

        }

        #endregion 

        #region     鼠标移动事件

        /// <summary>
        /// 鼠标在canvas上移动事件 通过移动路径绘制线段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void move(object sender, MouseEventArgs e)
        {
            if (Paint == true)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point point = e.GetPosition(can);
                    if ((point.X < x_area) && (point.Y < y_area))
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

                            m_LineNow.X2 = (Math.Round(point.X / 20)) * 20;
                            m_LineNow.Y2 = (Math.Round(point.Y / 20)) * 20;

                            #region     加标签


                            var f_TBX = (m_LineNow.X2 + m_LineNow.X1) / 2;
                            var f_TBY = m_LineNow.Y1;
                            ScaleTransform f_TBscale = new ScaleTransform();
                            f_TBscale.ScaleY = -1;
                            f_TB.RenderTransform = f_TBscale;
                            f_TB.Foreground = Brushes.Blue;
                            f_TB.Text = (m_LineNow.X2 - m_LineNow.X1) / 20 + "格";
                            f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);

                            var f_TDX = m_LineNow.X2;
                            var f_TDY = (m_LineNow.Y2 + m_LineNow.Y1) / 2;
                            ScaleTransform f_TDscale = new ScaleTransform();
                            f_TDscale.ScaleY = -1;
                            f_TD.RenderTransform = f_TDscale;
                            f_TD.Foreground = Brushes.Blue;
                            f_TD.Text = (m_LineNow.Y2 - m_LineNow.Y1) / 20 + "格";
                            f_TD.Margin = new Thickness(f_TDX, f_TDY, 0, 0);

                            m_lineB.X2 = m_LineNow.X2;

                            m_lineC.X1 = m_LineNow.X2;
                            m_lineC.X2 = m_LineNow.X2;
                            m_lineC.Y2 = m_LineNow.Y2;

                            #endregion

                        }
                    }
                }
            }

            if (Load == true)
            {
                if (m_LoadNow.X1 != 0 && m_LoadNow.Y1 != 0)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        Point point = e.GetPosition(can);

                        if ((point.X < x_area) && (point.Y < y_area))
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
                                m_LoadNow.Stroke = Brushes.Red;
                                m_LoadNow.StrokeThickness = 1;

                                if (count < 2)
                                    return;

                                m_LoadNow.X2 = (Math.Round(point.X / 20)) * 20;
                                m_LoadNow.Y2 = (Math.Round(point.Y / 20)) * 20;

                                #region     加标签


                                var f_TBX = (m_LoadNow.X2 + m_LoadNow.X1) / 2;
                                var f_TBY = m_LoadNow.Y1;
                                ScaleTransform f_TBscale = new ScaleTransform();
                                f_TBscale.ScaleY = -1;
                                f_TB.RenderTransform = f_TBscale;
                                f_TB.Foreground = Brushes.Blue;
                                f_TB.Text = (m_LoadNow.X2 - m_LoadNow.X1) / 20 + "格";
                                f_TB.Margin = new Thickness(f_TBX, f_TBY, 0, 0);

                                var f_TDX = m_LoadNow.X2;
                                var f_TDY = (m_LoadNow.Y2 + m_LoadNow.Y1) / 2;
                                ScaleTransform f_TDscale = new ScaleTransform();
                                f_TDscale.ScaleY = -1;
                                f_TD.RenderTransform = f_TDscale;
                                f_TD.Foreground = Brushes.Blue;
                                f_TD.Text = (m_LoadNow.Y2 - m_LoadNow.Y1) / 20 + "格";
                                f_TD.Margin = new Thickness(f_TDX, f_TDY, 0, 0);

                                m_lineB.X2 = m_LoadNow.X2;

                                m_lineC.X1 = m_LoadNow.X2;
                                m_lineC.X2 = m_LoadNow.X2;
                                m_lineC.Y2 = m_LoadNow.Y2;

                                #endregion

                            }

                            #region 辅助线

                            for (int i = 0; i < m_Joint_Num; i++)
                            {
                                if (m_JointModelList[i].Joint_Point.X - 6 < startPoint.X && m_JointModelList[i].Joint_Point.X + 6 > startPoint.X && m_JointModelList[i].Joint_Point.Y - 6 < startPoint.Y && m_JointModelList[i].Joint_Point.Y + 6 > startPoint.Y)
                                {
                                    for (int j = 0; j < m_JointModelList[i].Joint_Count; j++)
                                    {
                                        int m_line;

                                        if ((m_LoadNow.X2 - m_LoadNow.X1) != 0)
                                        {

                                            if (m_LineModelList[m_JointModelList[i].Joint_Line[j]].Line_Style == true && (((m_LoadNow.Y2 - m_LoadNow.Y1) / (m_LoadNow.X2 - m_LoadNow.X1)) > 4 || ((m_LoadNow.Y2 - m_LoadNow.Y1) / (m_LoadNow.X2 - m_LoadNow.X1)) < -4))
                                            {
                                                m_line = m_JointModelList[i].Joint_Line[j];
                                                ScaleTransform f_TCscale = new ScaleTransform();
                                                f_TCscale.ScaleY = -1;
                                                f_TC.RenderTransform = f_TCscale;
                                                f_TC.Foreground = Brushes.Red;
                                                f_TC.Text = "杆件" + (m_line + 1).ToString() + "横:0" + "纵:" + ((m_LineModelList[m_line].Line_EndPoint.Y - m_LineModelList[m_line].Line_BeginPoint.Y) / 20).ToString() + "格";
                                                f_TC.Margin = new Thickness((m_LineModelList[m_line].Line_EndPoint.X + m_LineModelList[m_line].Line_BeginPoint.X) / 2, (m_LineModelList[m_line].Line_EndPoint.Y + m_LineModelList[m_line].Line_BeginPoint.Y) / 2, 0, 0);
                                            }

                                            if (m_LineModelList[m_JointModelList[i].Joint_Line[j]].Line_Style == false && ((m_LineModelList[m_JointModelList[i].Joint_Line[j]].Line_Angle < ((m_LoadNow.Y2 - m_LoadNow.Y1 + 40) / (m_LoadNow.X2 - m_LoadNow.X1))
                                                && ((m_LoadNow.Y2 - m_LoadNow.Y1 - 40) / (m_LoadNow.X2 - m_LoadNow.X1)) < m_LineModelList[m_JointModelList[i].Joint_Line[j]].Line_Angle)))
                                            {
                                                m_line = m_JointModelList[i].Joint_Line[j];
                                                ScaleTransform f_TCscale = new ScaleTransform();
                                                f_TCscale.ScaleY = -1;
                                                f_TC.RenderTransform = f_TCscale;
                                                f_TC.Foreground = Brushes.Red;
                                                f_TC.Text = "杆件" + (m_line + 1).ToString() + "横:" + ((m_LineModelList[m_line].Line_EndPoint.X - m_LineModelList[m_line].Line_BeginPoint.X) / 20).ToString() + "格,纵:" + ((m_LineModelList[m_line].Line_EndPoint.Y - m_LineModelList[m_line].Line_BeginPoint.Y) / 20).ToString() + "格";
                                                f_TC.Margin = new Thickness((m_LineModelList[m_line].Line_EndPoint.X + m_LineModelList[m_line].Line_BeginPoint.X) / 2, (m_LineModelList[m_line].Line_EndPoint.Y + m_LineModelList[m_line].Line_BeginPoint.Y) / 2, 0, 0);
                                            }

                                            if (m_LineModelList[m_JointModelList[i].Joint_Line[j]].Line_Style == false && ((m_LineModelList[m_JointModelList[i].Joint_Line[j]].Line_Angle > ((m_LoadNow.Y2 - m_LoadNow.Y1 + 40) / (m_LoadNow.X2 - m_LoadNow.X1))
                                                && ((m_LoadNow.Y2 - m_LoadNow.Y1 - 40) / (m_LoadNow.X2 - m_LoadNow.X1)) > m_LineModelList[m_JointModelList[i].Joint_Line[j]].Line_Angle)))
                                            {
                                                m_line = m_JointModelList[i].Joint_Line[j];
                                                ScaleTransform f_TCscale = new ScaleTransform();
                                                f_TCscale.ScaleY = -1;
                                                f_TC.RenderTransform = f_TCscale;
                                                f_TC.Foreground = Brushes.Red;
                                                f_TC.Text = "杆件:" + (m_line + 1).ToString() + "横向:" + ((m_LineModelList[m_line].Line_EndPoint.X - m_LineModelList[m_line].Line_BeginPoint.X) / 20).ToString() + "格,纵向:" + ((m_LineModelList[m_line].Line_EndPoint.Y - m_LineModelList[m_line].Line_BeginPoint.Y) / 20).ToString() + "格";
                                                f_TC.Margin = new Thickness((m_LineModelList[m_line].Line_EndPoint.X + m_LineModelList[m_line].Line_BeginPoint.X) / 2, (m_LineModelList[m_line].Line_EndPoint.Y + m_LineModelList[m_line].Line_BeginPoint.Y) / 2, 0, 0);
                                            }

                                        }
                                    }
                                }
                            }

                            #endregion

                        }
                    }
                }
            }
        }

        #endregion

        #region     鼠标抬起事件

        /// <summary>
        /// 鼠标左键抬起函数，直线终点、保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void can_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            endPoint = e.GetPosition(can);

            if (Paint == true)
            {
                if ((endPoint.X < x_area) && (endPoint.Y < y_area))
                {
                    endPoint.X = m_LineNow.X2;
                    endPoint.Y = m_LineNow.Y2;
                    m_LineModel.Line_EndPoint = endPoint;

                    f_TB.Visibility = Visibility.Collapsed;
                    f_TD.Visibility = Visibility.Collapsed;

                    m_lineB.X2 = m_lineB.X1;
                    m_lineB.Y2 = m_lineB.Y1;
                    m_lineC.X2 = m_lineC.X1;
                    m_lineC.Y2 = m_lineC.Y1;

                    Ellipse endEllipse = new Ellipse();
                    endEllipse.Height = 4;
                    endEllipse.Width = 4;
                    endEllipse.Fill = Brushes.Black;
                    endEllipse.SetValue(Canvas.LeftProperty, endPoint.X - 2);
                    endEllipse.SetValue(Canvas.TopProperty, endPoint.Y - 2);
                    can.Children.Add(endEllipse);

                    #region 直线类型
                    m_LineModel.Line_Style = false;

                    if (m_LineModel.Line_BeginPoint.X == m_LineModel.Line_EndPoint.X)
                    {
                        m_LineModel.Line_Style = true;
                    }
                    #endregion

                    #region 调整直线坐标
                    if (m_LineModel.Line_Style == false)
                    {
                        if (m_LineModel.Line_BeginPoint.X > m_LineModel.Line_EndPoint.X)
                        {
                            transPoint = m_LineModel.Line_BeginPoint;
                            m_LineModel.Line_BeginPoint = m_LineModel.Line_EndPoint;
                            m_LineModel.Line_EndPoint = transPoint;
                        }
                    }
                    if (m_LineModel.Line_Style == true)
                    {
                        if (m_LineModel.Line_BeginPoint.Y > m_LineModel.Line_EndPoint.Y)
                        {
                            transPoint = m_LineModel.Line_BeginPoint;
                            m_LineModel.Line_BeginPoint = m_LineModel.Line_EndPoint;
                            m_LineModel.Line_EndPoint = transPoint;
                        }
                    }
                    #endregion 

                    #region  计算角度，竖直杆不参与计算，默认为零
                    if (m_LineModel.Line_Style == false)
                    {
                        var angle = (m_LineModel.Line_EndPoint.Y - m_LineModel.Line_BeginPoint.Y) / (m_LineModel.Line_EndPoint.X - m_LineModel.Line_BeginPoint.X);
                        m_LineModel.Line_Angle = angle;
                    }
                    #endregion

                    #region 建立保存对应Line模型

                    m_LineModel.Line_IsZero = false;

                    m_LineModel.Line_Info = m_LineNow;

                    if (m_LineModel.Line_BeginPoint.X != m_LineModel.Line_EndPoint.X || m_LineModel.Line_BeginPoint.Y != m_LineModel.Line_EndPoint.Y)
                    {
                        m_Line_Num++;

                        m_LineModelList.Add(m_LineModel);

                        #region     加标签

                        TextBlock f_TBline = new TextBlock();
                        var f_TBX = (m_LineModel.Line_BeginPoint.X + m_LineModel.Line_EndPoint.X) / 2;
                        var f_TBY = (m_LineModel.Line_BeginPoint.Y + m_LineModel.Line_EndPoint.Y) / 2 + 10;
                        ScaleTransform f_TBscale = new ScaleTransform();
                        f_TBscale.ScaleY = -1;
                        f_TBline.RenderTransform = f_TBscale;
                        f_TBline.Foreground = Brushes.Blue;
                        f_TBline.Text = "(" + m_Line_Num + ")";
                        f_TBline.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                        can.Children.Add(f_TBline);

                        #endregion

                        m_LineModel = new LineModelClass();
                    }

                    #endregion
                }
            }

            if (Zhizuo == true)
            {
                for (int i = 0; i < m_Joint_Num; i++)
                {
                    if (m_JointModelList[i].Joint_Point.X - 6 < endPoint.X && m_JointModelList[i].Joint_Point.X + 6 > endPoint.X && m_JointModelList[i].Joint_Point.Y - 6 < endPoint.Y && m_JointModelList[i].Joint_Point.Y + 6 > endPoint.Y)
                    {
                        if (Zhizuo_Style == 1)
                        {
                            if (m_JointModelList[i].Joint_Zhizuo == 0)
                            {
                                m_JointModelList[i].Joint_Zhizuo = 1;
                            }
                            if (m_JointModelList[i].Joint_Zhizuo == 2)
                            {
                                m_JointModelList[i].Joint_Zhizuo = 3;
                            }
                            Line Zhizuoline = new Line();
                            Zhizuoline.X1 = m_JointModelList[i].Joint_Point.X - 4;
                            Zhizuoline.Y1 = m_JointModelList[i].Joint_Point.Y;
                            Zhizuoline.X2 = m_JointModelList[i].Joint_Point.X - 29;
                            Zhizuoline.Y2 = m_JointModelList[i].Joint_Point.Y;
                            Zhizuoline.StrokeThickness = 2;
                            Zhizuoline.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline);

                            Ellipse ZhizuoEllipse = new Ellipse();
                            ZhizuoEllipse.Height = 8;
                            ZhizuoEllipse.Width = 8;
                            ZhizuoEllipse.Fill = Brushes.Black;
                            ZhizuoEllipse.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 33);
                            ZhizuoEllipse.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 4);
                            can.Children.Add(ZhizuoEllipse);
                            Ellipse ZhizuoEllipse2 = new Ellipse();
                            ZhizuoEllipse2.Height = 6;
                            ZhizuoEllipse2.Width = 6;
                            ZhizuoEllipse2.Fill = Brushes.White;
                            ZhizuoEllipse2.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 32);
                            ZhizuoEllipse2.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 3);
                            can.Children.Add(ZhizuoEllipse2);
                            Line Zhizuoline1 = new Line();
                            Zhizuoline1.X1 = m_JointModelList[i].Joint_Point.X - 33;
                            Zhizuoline1.Y1 = m_JointModelList[i].Joint_Point.Y - 20;
                            Zhizuoline1.X2 = m_JointModelList[i].Joint_Point.X - 33;
                            Zhizuoline1.Y2 = m_JointModelList[i].Joint_Point.Y + 20;
                            Zhizuoline1.StrokeThickness = 2;
                            Zhizuoline1.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline1);
                            #region 人类的本质就是复读机
                            Line Zhizuoline2 = new Line();
                            Zhizuoline2.X1 = m_JointModelList[i].Joint_Point.X - 33;
                            Zhizuoline2.Y1 = m_JointModelList[i].Joint_Point.Y + 10;
                            Zhizuoline2.X2 = m_JointModelList[i].Joint_Point.X - 43;
                            Zhizuoline2.Y2 = m_JointModelList[i].Joint_Point.Y + 20;
                            Zhizuoline2.StrokeThickness = 1;
                            Zhizuoline2.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline2);
                            Line Zhizuoline3 = new Line();
                            Zhizuoline3.X1 = m_JointModelList[i].Joint_Point.X - 33;
                            Zhizuoline3.Y1 = m_JointModelList[i].Joint_Point.Y ;
                            Zhizuoline3.X2 = m_JointModelList[i].Joint_Point.X - 43;
                            Zhizuoline3.Y2 = m_JointModelList[i].Joint_Point.Y + 10;
                            Zhizuoline3.StrokeThickness = 1;
                            Zhizuoline3.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline3);
                            Line Zhizuoline4 = new Line();
                            Zhizuoline4.X1 = m_JointModelList[i].Joint_Point.X - 33;
                            Zhizuoline4.Y1 = m_JointModelList[i].Joint_Point.Y - 10;
                            Zhizuoline4.X2 = m_JointModelList[i].Joint_Point.X - 43;
                            Zhizuoline4.Y2 = m_JointModelList[i].Joint_Point.Y ;
                            Zhizuoline4.StrokeThickness = 1;
                            Zhizuoline4.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline4);
                            Line Zhizuoline5 = new Line();
                            Zhizuoline5.X1 = m_JointModelList[i].Joint_Point.X - 33;
                            Zhizuoline5.Y1 = m_JointModelList[i].Joint_Point.Y - 20;
                            Zhizuoline5.X2 = m_JointModelList[i].Joint_Point.X - 43;
                            Zhizuoline5.Y2 = m_JointModelList[i].Joint_Point.Y - 10;
                            Zhizuoline5.StrokeThickness = 1;
                            Zhizuoline5.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline5);
                            #endregion
                        }

                        if (Zhizuo_Style == 2)
                        {
                            if (m_JointModelList[i].Joint_Zhizuo == 0)
                            {
                                m_JointModelList[i].Joint_Zhizuo = 2;
                            }
                            if (m_JointModelList[i].Joint_Zhizuo == 1)
                            {
                                m_JointModelList[i].Joint_Zhizuo = 3;
                            }
                            Line Zhizuoline = new Line();
                            Zhizuoline.X1 = m_JointModelList[i].Joint_Point.X;
                            Zhizuoline.Y1 = m_JointModelList[i].Joint_Point.Y - 4;
                            Zhizuoline.X2 = m_JointModelList[i].Joint_Point.X;
                            Zhizuoline.Y2 = m_JointModelList[i].Joint_Point.Y - 29;
                            Zhizuoline.StrokeThickness = 2;
                            Zhizuoline.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline);

                            Ellipse ZhizuoEllipse = new Ellipse();
                            ZhizuoEllipse.Height = 8;
                            ZhizuoEllipse.Width = 8;
                            ZhizuoEllipse.Fill = Brushes.Black;
                            ZhizuoEllipse.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 4);
                            ZhizuoEllipse.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 33);
                            can.Children.Add(ZhizuoEllipse);
                            Ellipse ZhizuoEllipse2 = new Ellipse();
                            ZhizuoEllipse2.Height = 6;
                            ZhizuoEllipse2.Width = 6;
                            ZhizuoEllipse2.Fill = Brushes.White;
                            ZhizuoEllipse2.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 3);
                            ZhizuoEllipse2.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 32);
                            can.Children.Add(ZhizuoEllipse2);
                            Line Zhizuoline1 = new Line();
                            Zhizuoline1.X1 = m_JointModelList[i].Joint_Point.X - 20;
                            Zhizuoline1.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                            Zhizuoline1.X2 = m_JointModelList[i].Joint_Point.X + 20;
                            Zhizuoline1.Y2 = m_JointModelList[i].Joint_Point.Y - 33;
                            Zhizuoline1.StrokeThickness = 2;
                            Zhizuoline1.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline1);
                            #region 人类的本质就是复读机
                            Line Zhizuoline2 = new Line();
                            Zhizuoline2.X1 = m_JointModelList[i].Joint_Point.X + 10;
                            Zhizuoline2.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                            Zhizuoline2.X2 = m_JointModelList[i].Joint_Point.X + 20;
                            Zhizuoline2.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                            Zhizuoline2.StrokeThickness = 1;
                            Zhizuoline2.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline2);
                            Line Zhizuoline3 = new Line();
                            Zhizuoline3.X1 = m_JointModelList[i].Joint_Point.X;
                            Zhizuoline3.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                            Zhizuoline3.X2 = m_JointModelList[i].Joint_Point.X + 10;
                            Zhizuoline3.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                            Zhizuoline3.StrokeThickness = 1;
                            Zhizuoline3.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline3);
                            Line Zhizuoline4 = new Line();
                            Zhizuoline4.X1 = m_JointModelList[i].Joint_Point.X - 10;
                            Zhizuoline4.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                            Zhizuoline4.X2 = m_JointModelList[i].Joint_Point.X;
                            Zhizuoline4.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                            Zhizuoline4.StrokeThickness = 1;
                            Zhizuoline4.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline4);
                            Line Zhizuoline5 = new Line();
                            Zhizuoline5.X1 = m_JointModelList[i].Joint_Point.X - 20;
                            Zhizuoline5.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                            Zhizuoline5.X2 = m_JointModelList[i].Joint_Point.X - 10;
                            Zhizuoline5.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                            Zhizuoline5.StrokeThickness = 1;
                            Zhizuoline5.Stroke = Brushes.Black;
                            can.Children.Add(Zhizuoline5);
                            #endregion
                        }
                    }
                }
            }

            if (Load == true)
            {
                if ((endPoint.X < x_area) && (endPoint.Y < y_area))
                {
                    endPoint.X = m_LoadNow.X2;
                    endPoint.Y = m_LoadNow.Y2;

                    f_TB.Visibility = Visibility.Collapsed;
                    f_TC.Visibility = Visibility.Collapsed;
                    f_TD.Visibility = Visibility.Collapsed;

                    m_lineB.X2 = m_lineB.X1;
                    m_lineB.Y2 = m_lineB.Y1;
                    m_lineC.X2 = m_lineC.X1;
                    m_lineC.Y2 = m_lineC.Y1;

                    #region 载荷类型

                    m_LoadModel.Load_Style = false;
                    m_LoadModel.Load_Sign = true;

                    if (m_LoadNow.X1 == m_LoadNow.X2)
                    {
                        m_LoadModel.Load_Style = true;
                    }
                    #endregion

                    #region 判断载荷正负

                    if (m_LoadNow.Y1 > m_LoadNow.Y2)
                    {
                        m_LoadModel.Load_Sign = false;
                    }
                    if (m_LoadNow.Y1 == m_LoadNow.Y2)
                    {
                        if (m_LoadNow.X1 > m_LoadNow.X2)
                        {
                            m_LoadModel.Load_Sign = false;
                        }
                    }
                    #endregion

                    #region 计算载荷分量

                    var length = Math.Sqrt(Math.Pow(m_LoadNow.X1 - m_LoadNow.X2, 2) + Math.Pow(m_LoadNow.Y1 - m_LoadNow.Y2, 2));
                    m_LoadModel.Load_X = Math.Abs((m_LoadNow.X1 - m_LoadNow.X2) / length);
                    m_LoadModel.Load_Y = Math.Abs((m_LoadNow.Y1 - m_LoadNow.Y2) / length);

                    #endregion

                    #region  计算角度，竖直力不参与计算，其角度默认为零，注意与水平力区分
                    if (m_LoadModel.Load_Style == false)
                    {
                        var angle = (m_LoadNow.Y2 - m_LoadNow.Y1) / (m_LoadNow.X2 - m_LoadNow.X1);
                        m_LoadModel.Load_Angle = angle;
                    }
                    #endregion

                    #region 建立保存对应Load模型

                    if (m_LoadNow.X1 != m_LoadNow.X2 || m_LoadNow.Y1 != m_LoadNow.Y2)
                    {
                        if (m_LoadModel.Load_X != 0 || m_LoadModel.Load_Y != 0)
                        {
                            m_Load_Num++;

                            m_LoadModelList.Add(m_LoadModel);

                            #region     画箭头

                            if (m_LoadModel.Load_Style == false)
                            {
                                if (m_LoadModel.Load_Angle > 0 || m_LoadModel.Load_Angle == 0)
                                {
                                    if (m_LoadModel.Load_Angle < 1)
                                    {
                                        if (m_LoadModel.Load_Sign == true)
                                        {
                                            Line line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 - 10;
                                            line.Y2 = m_LoadNow.Y2 - 10 * (m_LoadModel.Load_Angle);
                                            TransformGroup transformGroup = new TransformGroup();
                                            RotateTransform rotateTransform = new RotateTransform(20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                            line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 - 10;
                                            line.Y2 = m_LoadNow.Y2 - 10 * (m_LoadModel.Load_Angle);
                                            transformGroup = new TransformGroup();
                                            rotateTransform = new RotateTransform(-20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                        }

                                        if (m_LoadModel.Load_Sign == false)
                                        {
                                            Line line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 + 10;
                                            line.Y2 = m_LoadNow.Y2 + 10 * (m_LoadModel.Load_Angle);
                                            TransformGroup transformGroup = new TransformGroup();
                                            RotateTransform rotateTransform = new RotateTransform(20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                            line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 + 10;
                                            line.Y2 = m_LoadNow.Y2 + 10 * (m_LoadModel.Load_Angle);
                                            transformGroup = new TransformGroup();
                                            rotateTransform = new RotateTransform(-20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                        }
                                    }

                                    if (m_LoadModel.Load_Angle > 1 || m_LoadModel.Load_Angle == 1)
                                    {
                                        if (m_LoadModel.Load_Sign == true)
                                        {
                                            Line line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 - 10 / (m_LoadModel.Load_Angle);
                                            line.Y2 = m_LoadNow.Y2 - 10;
                                            TransformGroup transformGroup = new TransformGroup();
                                            RotateTransform rotateTransform = new RotateTransform(20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                            line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 - 10 / (m_LoadModel.Load_Angle);
                                            line.Y2 = m_LoadNow.Y2 - 10;
                                            transformGroup = new TransformGroup();
                                            rotateTransform = new RotateTransform(-20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                        }

                                        if (m_LoadModel.Load_Sign == false)
                                        {
                                            Line line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 + 10 / (m_LoadModel.Load_Angle);
                                            line.Y2 = m_LoadNow.Y2 + 10;
                                            TransformGroup transformGroup = new TransformGroup();
                                            RotateTransform rotateTransform = new RotateTransform(20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                            line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 + 10 / (m_LoadModel.Load_Angle);
                                            line.Y2 = m_LoadNow.Y2 + 10;
                                            transformGroup = new TransformGroup();
                                            rotateTransform = new RotateTransform(-20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                        }
                                    }
                                }

                                if (m_LoadModel.Load_Angle < 0)
                                {
                                    if (m_LoadModel.Load_Angle > -1)
                                    {
                                        if (m_LoadModel.Load_Sign == true)
                                        {
                                            Line line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 + 10;
                                            line.Y2 = m_LoadNow.Y2 + 10 * (m_LoadModel.Load_Angle);
                                            TransformGroup transformGroup = new TransformGroup();
                                            RotateTransform rotateTransform = new RotateTransform(20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                            line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 + 10;
                                            line.Y2 = m_LoadNow.Y2 + 10 * (m_LoadModel.Load_Angle);
                                            transformGroup = new TransformGroup();
                                            rotateTransform = new RotateTransform(-20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                        }

                                        if (m_LoadModel.Load_Sign == false)
                                        {
                                            Line line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 - 10;
                                            line.Y2 = m_LoadNow.Y2 - 10 * (m_LoadModel.Load_Angle);
                                            TransformGroup transformGroup = new TransformGroup();
                                            RotateTransform rotateTransform = new RotateTransform(20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                            line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 - 10;
                                            line.Y2 = m_LoadNow.Y2 - 10 * (m_LoadModel.Load_Angle);
                                            transformGroup = new TransformGroup();
                                            rotateTransform = new RotateTransform(-20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                        }
                                    }

                                    if (m_LoadModel.Load_Angle < -1 || m_LoadModel.Load_Angle == -1)
                                    {
                                        if (m_LoadModel.Load_Sign == true)
                                        {
                                            Line line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 - 10 / (m_LoadModel.Load_Angle);
                                            line.Y2 = m_LoadNow.Y2 - 10;
                                            TransformGroup transformGroup = new TransformGroup();
                                            RotateTransform rotateTransform = new RotateTransform(20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                            line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 - 10 / (m_LoadModel.Load_Angle);
                                            line.Y2 = m_LoadNow.Y2 - 10;
                                            transformGroup = new TransformGroup();
                                            rotateTransform = new RotateTransform(-20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                        }

                                        if (m_LoadModel.Load_Sign == false)
                                        {
                                            Line line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 + 10 / (m_LoadModel.Load_Angle);
                                            line.Y2 = m_LoadNow.Y2 + 10;
                                            TransformGroup transformGroup = new TransformGroup();
                                            RotateTransform rotateTransform = new RotateTransform(20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                            line = new Line();
                                            line.Stroke = Brushes.Red;
                                            line.StrokeThickness = 1;
                                            line.X1 = m_LoadNow.X2;
                                            line.Y1 = m_LoadNow.Y2;
                                            line.X2 = m_LoadNow.X2 + 10 / (m_LoadModel.Load_Angle);
                                            line.Y2 = m_LoadNow.Y2 + 10;
                                            transformGroup = new TransformGroup();
                                            rotateTransform = new RotateTransform(-20, line.X1, line.Y1);
                                            transformGroup.Children.Add(rotateTransform);
                                            line.RenderTransform = transformGroup;
                                            can.Children.Add(line);
                                        }
                                    }
                                }
                            }

                            if (m_LoadModel.Load_Style == true && m_LoadModel.Load_Y != 0)
                            {
                                if (m_LoadModel.Load_Sign == true)
                                {
                                    Line line = new Line();
                                    line.Stroke = Brushes.Red;
                                    line.StrokeThickness = 1;
                                    line.X1 = m_LoadNow.X2;
                                    line.Y1 = m_LoadNow.Y2;
                                    line.X2 = m_LoadNow.X2;
                                    line.Y2 = m_LoadNow.Y2 - 10;
                                    TransformGroup transformGroup = new TransformGroup();
                                    RotateTransform rotateTransform = new RotateTransform(20, line.X1, line.Y1);
                                    transformGroup.Children.Add(rotateTransform);
                                    line.RenderTransform = transformGroup;
                                    can.Children.Add(line);
                                    line = new Line();
                                    line.Stroke = Brushes.Red;
                                    line.StrokeThickness = 1;
                                    line.X1 = m_LoadNow.X2;
                                    line.Y1 = m_LoadNow.Y2;
                                    line.X2 = m_LoadNow.X2;
                                    line.Y2 = m_LoadNow.Y2 - 10;
                                    transformGroup = new TransformGroup();
                                    rotateTransform = new RotateTransform(-20, line.X1, line.Y1);
                                    transformGroup.Children.Add(rotateTransform);
                                    line.RenderTransform = transformGroup;
                                    can.Children.Add(line); ;
                                }

                                if (m_LoadModel.Load_Sign == false)
                                {
                                    Line line = new Line();
                                    line.Stroke = Brushes.Red;
                                    line.StrokeThickness = 1;
                                    line.X1 = m_LoadNow.X2;
                                    line.Y1 = m_LoadNow.Y2;
                                    line.X2 = m_LoadNow.X2;
                                    line.Y2 = m_LoadNow.Y2 + 10;
                                    TransformGroup transformGroup = new TransformGroup();
                                    RotateTransform rotateTransform = new RotateTransform(20, line.X1, line.Y1);
                                    transformGroup.Children.Add(rotateTransform);
                                    line.RenderTransform = transformGroup;
                                    can.Children.Add(line);
                                    line = new Line();
                                    line.Stroke = Brushes.Red;
                                    line.StrokeThickness = 1;
                                    line.X1 = m_LoadNow.X2;
                                    line.Y1 = m_LoadNow.Y2;
                                    line.X2 = m_LoadNow.X2;
                                    line.Y2 = m_LoadNow.Y2 + 10;
                                    transformGroup = new TransformGroup();
                                    rotateTransform = new RotateTransform(-20, line.X1, line.Y1);
                                    transformGroup.Children.Add(rotateTransform);
                                    line.RenderTransform = transformGroup;
                                    can.Children.Add(line); ;
                                }
                            }

                            #endregion

                            #region     加标签

                            TextBlock f_TBline = new TextBlock();
                            var f_TBX = m_LoadNow.X2;
                            var f_TBY = m_LoadNow.Y2 + 20;
                            ScaleTransform f_TBscale = new ScaleTransform();
                            f_TBscale.ScaleY = -1;
                            f_TBline.RenderTransform = f_TBscale;
                            f_TBline.Foreground = Brushes.Blue;
                            f_TBline.Text = "F" + m_Load_Num;
                            f_TBline.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                            can.Children.Add(f_TBline);

                            #endregion

                            m_LoadModel = new LoadModelClass();

                        }
                    }

                    #endregion
                }
            }

        }

        #endregion

        #region 添加支座函数

        private void Rect_X(object sender, MouseEventArgs e)
        {
            if (Zhizuo == false)
            {
                MessageBox.Show("请先点击添加支座按钮");
            }
            Zhizuo_Style = 1;
        }

        private void Rect_Y(object sender, MouseEventArgs e)
        {
            if (Zhizuo == false)
            {
                MessageBox.Show("请先点击添加支座按钮");
            }
            Zhizuo_Style = 2;
        }

        #endregion

        #endregion

        #region     程序控制函数

        #region order 0 — 11 开始绘制桁架

        private void Paint_ok_Click(object sender, RoutedEventArgs e)
        {
            if (order == 0)
            {
                Paint = true;

                #region     分区线

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

                MessageBox.Show("请在画板上绘制杆件");

                #endregion

                #region     新建清空
                Button qk = new Button();
                qk.Width = 117;
                qk.Height = 48;
                qk.Content = "清空";
                ScaleTransform f_TBrulerscale = new ScaleTransform();
                f_TBrulerscale.ScaleY = -1;
                qk.RenderTransform = f_TBrulerscale;

                qk.Margin = new Thickness(1053, 53, 0, 0);
                can.Children.Add(qk);
                qk.Click += qingkong;
                #endregion

                order = 11;
            }
            if (order == 15)
            {
                MessageBox.Show("若要新建桁架，请先单击清空按钮");
            }
        }

        #endregion

        #region order 11 — 12 完成绘制 添加节点

        private void SavePaint_Click(object sender, RoutedEventArgs e)
        {
            if (order == 11)
            {
                Paint = false;

                #region 杆件节点生成

                for (int i = 0; i < m_Line_Num; i++)
                {
                    Flag = false;
                    for (int j = 0; j < m_Joint_Num; j++)
                    {
                        if (m_JointModelList[j].Joint_Point.X == m_LineModelList[i].Line_BeginPoint.X && m_JointModelList[j].Joint_Point.Y == m_LineModelList[i].Line_BeginPoint.Y)
                        {
                            m_JointModelList[j].Joint_Count++;
                            m_JointModelList[j].Joint_Line[(m_JointModelList[j].Joint_Count - 1)] = i;
                            Flag = true;
                            break;
                        }
                    }
                    if (Flag == false)
                    {
                        m_Joint_Num++;

                        m_JointModel.Joint_Num = m_Joint_Num;
                        m_JointModel.Joint_Count = 1;
                        m_JointModel.Joint_Line[0] = i;
                        m_JointModel.Joint_Point.X = m_LineModelList[i].Line_BeginPoint.X;
                        m_JointModel.Joint_Point.Y = m_LineModelList[i].Line_BeginPoint.Y;
                        m_JointModel.Joint_Zhizuo = 0;

                        m_JointModelList.Add(m_JointModel);
                        m_JointModel = new JointModelClass();
                    }
                }

                for (int i = 0; i < m_Line_Num; i++)
                {
                    Flag = false;
                    for (int j = 0; j < m_Joint_Num; j++)
                    {
                        if (m_JointModelList[j].Joint_Point.X == m_LineModelList[i].Line_EndPoint.X && m_JointModelList[j].Joint_Point.Y == m_LineModelList[i].Line_EndPoint.Y)
                        {
                            m_JointModelList[j].Joint_Count++;
                            m_JointModelList[j].Joint_Line[(m_JointModelList[j].Joint_Count - 1)] = i;
                            Flag = true;
                            break;
                        }
                    }
                    if (Flag == false)
                    {
                        m_Joint_Num++;

                        m_JointModel.Joint_Num = m_Joint_Num;
                        m_JointModel.Joint_Count = 1;
                        m_JointModel.Joint_Line[0] = i;
                        m_JointModel.Joint_Point.X = m_LineModelList[i].Line_EndPoint.X;
                        m_JointModel.Joint_Point.Y = m_LineModelList[i].Line_EndPoint.Y;
                        m_JointModel.Joint_Zhizuo = 0;

                        m_JointModelList.Add(m_JointModel);
                        m_JointModel = new JointModelClass();
                    }
                }

                for (int i = 0; i < m_Line_Num; i++)
                {
                    for (int j = 0; j < m_Joint_Num; j++)
                    {
                        if (m_LineModelList[i].Line_BeginPoint.X == m_JointModelList[j].Joint_Point.X && m_LineModelList[i].Line_BeginPoint.Y == m_JointModelList[j].Joint_Point.Y)
                        {
                            m_LineModelList[i].Line_Joint1 = j;
                        }
                    }
                }

                for (int i = 0; i < m_Line_Num; i++)
                {
                    for (int j = 0; j < m_Joint_Num; j++)
                    {
                        if (m_LineModelList[i].Line_EndPoint.X == m_JointModelList[j].Joint_Point.X && m_LineModelList[i].Line_EndPoint.Y == m_JointModelList[j].Joint_Point.Y)
                        {
                            m_LineModelList[i].Line_Joint2 = j;
                        }
                    }
                }

                #endregion

                #region 添加节点图片
                for (int i = 0; i < m_Joint_Num; i++)
                {
                    Ellipse JointEllipse = new Ellipse();
                    JointEllipse.Height = 8;
                    JointEllipse.Width = 8;
                    JointEllipse.Fill = Brushes.Black;
                    JointEllipse.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 4);
                    JointEllipse.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 4);
                    can.Children.Add(JointEllipse);
                    Ellipse JointEllipse2 = new Ellipse();
                    JointEllipse2.Height = 6;
                    JointEllipse2.Width = 6;
                    JointEllipse2.Fill = Brushes.White;
                    JointEllipse2.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 3);
                    JointEllipse2.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 3);
                    can.Children.Add(JointEllipse2);
                }
                #endregion

                order = 12;
                MessageBox.Show("请添加支座");
            }
        }

        #endregion

        #region order 12 — 13 开始添加支座

        private void Zhizuo_ok_Click(object sender, RoutedEventArgs e)
        {
            if (order == 12)
            {
                Zhizuo = true;
                order = 13;
            }
            if (order == 11)
            {
                MessageBox.Show("请先完成绘制");
            }
        }

        #endregion

        #region order 13 — 14 完成添加 验证静定

        private void SaveZhizuo_Click(object sender, RoutedEventArgs e)
        {
            if (order == 13)
            {
                Zhizuo = false;

                for (int i = 0; i < m_Joint_Num; i++)
                {
                    if (m_JointModelList[i].Joint_Zhizuo == 3)
                    {
                        jiaozhizuo++;
                        yueshu = yueshu + 2;
                    }
                    if (m_JointModelList[i].Joint_Zhizuo == 1)
                    {
                        lianganX++;
                        yueshu = yueshu + 1;
                    }
                    if (m_JointModelList[i].Joint_Zhizuo == 2)
                    {
                        lianganY++;
                        yueshu = yueshu + 1;
                    }
                }
                if (m_Joint_Num * 2 - m_Line_Num - yueshu == 0)
                {
                    MessageBox.Show("静定结构，可以保存桁架或者进行练习");
                }
                if (m_Joint_Num * 2 - m_Line_Num - yueshu < 0)
                {
                    MessageBox.Show("自由度小于零，超静定结构，请清空后重新绘制");
                }
                if (m_Joint_Num * 2 - m_Line_Num - yueshu > 0)
                {
                    MessageBox.Show("自由度大于零，可动机构，请清空后重新绘制");
                }
                order = 14;
            }
        }

        #endregion

        #region order 14 — 0 保存桁架

        private void Export(object sender, RoutedEventArgs e)
        {
            if (order == 13)
            {
                MessageBox.Show("请先完成支座添加");
            }
            if (order == 14 || order == 21)
            {
                XmlDocument m_hengjiaList = new XmlDocument();

                XmlElement m_hengjia = m_hengjiaList.CreateElement("hengjia");

                m_hengjia.SetAttribute("单位", "东大力学系");

                m_hengjiaList.AppendChild(m_hengjia);

                #region 保存杆件信息

                XmlElement m_LineList = m_hengjiaList.CreateElement("LineList");

                m_LineList.SetAttribute("东北大学", "邵天溢");

                m_hengjia.AppendChild(m_LineList);

                XmlNode root = m_hengjia.SelectSingleNode("LineList");

                for (int i = 0; i < m_Line_Num; i++)
                {
                    XmlElement m_Line = m_hengjiaList.CreateElement("Line" + i);

                    XmlElement m_Line_Angle = m_hengjiaList.CreateElement("Line_Angle");
                    m_Line_Angle.InnerText = m_LineModelList[i].Line_Angle.ToString();
                    m_Line.AppendChild(m_Line_Angle);

                    XmlElement m_Line_BeginPoint_X = m_hengjiaList.CreateElement("Line_BeginPoint_X");
                    m_Line_BeginPoint_X.InnerText = m_LineModelList[i].Line_BeginPoint.X.ToString();
                    m_Line.AppendChild(m_Line_BeginPoint_X);

                    XmlElement m_Line_BeginPoint_Y = m_hengjiaList.CreateElement("Line_BeginPoint_Y");
                    m_Line_BeginPoint_Y.InnerText = m_LineModelList[i].Line_BeginPoint.Y.ToString();
                    m_Line.AppendChild(m_Line_BeginPoint_Y);

                    XmlElement m_Line_EndPoint_X = m_hengjiaList.CreateElement("Line_EndPoint_X");
                    m_Line_EndPoint_X.InnerText = m_LineModelList[i].Line_EndPoint.X.ToString();
                    m_Line.AppendChild(m_Line_EndPoint_X);

                    XmlElement m_Line_EndPoint_Y = m_hengjiaList.CreateElement("Line_EndPoint_Y");
                    m_Line_EndPoint_Y.InnerText = m_LineModelList[i].Line_EndPoint.Y.ToString();
                    m_Line.AppendChild(m_Line_EndPoint_Y);

                    XmlElement m_Line_Style = m_hengjiaList.CreateElement("Line_Style");
                    m_Line_Style.InnerText = m_LineModelList[i].Line_Style.ToString();
                    m_Line.AppendChild(m_Line_Style);

                    XmlElement m_Line_Joint1 = m_hengjiaList.CreateElement("Line_Joint1");
                    m_Line_Joint1.InnerText = m_LineModelList[i].Line_Joint1.ToString();
                    m_Line.AppendChild(m_Line_Joint1);

                    XmlElement m_Line_Joint2 = m_hengjiaList.CreateElement("Line_Joint2");
                    m_Line_Joint2.InnerText = m_LineModelList[i].Line_Joint2.ToString();
                    m_Line.AppendChild(m_Line_Joint2);

                    XmlElement m_Line_IsZero = m_hengjiaList.CreateElement("Line_IsZero");
                    m_Line_IsZero.InnerText = m_LineModelList[i].Line_IsZero.ToString();
                    m_Line.AppendChild(m_Line_IsZero);

                    root.AppendChild(m_Line);
                }

                #endregion

                #region 保存节点信息

                XmlElement m_JointList = m_hengjiaList.CreateElement("JointList");

                m_hengjia.AppendChild(m_JointList);

                root = m_hengjia.SelectSingleNode("JointList");

                for (int i = 0; i < m_Joint_Num; i++)
                {
                    XmlElement m_Joint = m_hengjiaList.CreateElement("Joint" + i);

                    XmlElement m_Joint_Num = m_hengjiaList.CreateElement("Joint_Num");
                    m_Joint_Num.InnerText = m_JointModelList[i].Joint_Num.ToString();
                    m_Joint.AppendChild(m_Joint_Num);

                    XmlElement m_Joint_Count = m_hengjiaList.CreateElement("Joint_Count");
                    m_Joint_Count.InnerText = m_JointModelList[i].Joint_Count.ToString();
                    m_Joint.AppendChild(m_Joint_Count);

                    XmlElement m_Joint_Load = m_hengjiaList.CreateElement("Joint_Load");
                    m_Joint_Load.InnerText = m_JointModelList[i].Joint_Load.ToString();
                    m_Joint.AppendChild(m_Joint_Load);

                    XmlElement m_Joint_Point_X = m_hengjiaList.CreateElement("Joint_Point_X");
                    m_Joint_Point_X.InnerText = m_JointModelList[i].Joint_Point.X.ToString();
                    m_Joint.AppendChild(m_Joint_Point_X);

                    XmlElement m_Joint_Point_Y = m_hengjiaList.CreateElement("Joint_Point_Y");
                    m_Joint_Point_Y.InnerText = m_JointModelList[i].Joint_Point.Y.ToString();
                    m_Joint.AppendChild(m_Joint_Point_Y);

                    XmlElement m_Joint_Zhizuo = m_hengjiaList.CreateElement("Joint_Zhizuo");
                    m_Joint_Zhizuo.InnerText = m_JointModelList[i].Joint_Zhizuo.ToString();
                    m_Joint.AppendChild(m_Joint_Zhizuo);

                    for (int j = 0; j < m_JointModelList[i].Joint_Count; j++)
                    {
                        XmlElement m_Joint_Line = m_hengjiaList.CreateElement("Joint_Line" + j);
                        m_Joint_Line.InnerText = m_JointModelList[i].Joint_Line[j].ToString();
                        m_Joint.AppendChild(m_Joint_Line);
                    }

                    root.AppendChild(m_Joint);
                }

                #endregion

                #region 保存对话框

                string Writer_path;

                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();

                saveFileDialog.Filter = "xml files|*.xml|All files|*.*";
                saveFileDialog.FileName = "hengjia1.xml";
                saveFileDialog.Title = "保存桁架（东北大学力学系）";

                System.Windows.Forms.DialogResult result = saveFileDialog.ShowDialog();

                Writer_path = saveFileDialog.FileName;

                if (Writer_path == "hengjia1.xml")
                {
                    MessageBox.Show("保存失败请重试");
                }
                else
                {
                    m_hengjiaList.Save(Writer_path);

                    MessageBox.Show("桁架保存成功");
                }

                order = 15;

                #endregion

            }
        }

        #endregion

        #region order 0 — 21 读取桁架

        private void Import(object sender, RoutedEventArgs e)
        {

            if (order != 0)
            {
                MessageBox.Show("请先单击右下角清空按钮，清空程序");
            }

            if (order == 0)
            {

                #region 读取对话框

                string Reader_path;

                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog.Title = "读取桁架（东北大学力学系）";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.ShowDialog();
                Reader_path = openFileDialog.FileName;

                #endregion

                try
                {
                    XmlDocument m_hengjiaList = new XmlDocument();
                    m_hengjiaList.Load(Reader_path);
                    XmlNode root = m_hengjiaList.SelectSingleNode("hengjia");

                    #region 读取杆件信息

                    XmlNodeList root_LineList = root.SelectSingleNode("LineList").ChildNodes;
                    m_Line_Num = 0;
                    foreach (XmlNode root_Line in root_LineList)
                    {
                        LineModelClass m_LineModel = new LineModelClass();

                        XmlNode m_LineList = root_Line.SelectSingleNode("Line_Angle");
                        m_LineModel.Line_Angle = Convert.ToDouble(m_LineList.InnerText);

                        m_LineList = root_Line.SelectSingleNode("Line_BeginPoint_X");
                        m_LineModel.Line_BeginPoint.X = Convert.ToInt32(m_LineList.InnerText);

                        m_LineList = root_Line.SelectSingleNode("Line_BeginPoint_Y");
                        m_LineModel.Line_BeginPoint.Y = Convert.ToInt32(m_LineList.InnerText);

                        m_LineList = root_Line.SelectSingleNode("Line_EndPoint_X");
                        m_LineModel.Line_EndPoint.X = Convert.ToInt32(m_LineList.InnerText);

                        m_LineList = root_Line.SelectSingleNode("Line_EndPoint_Y");
                        m_LineModel.Line_EndPoint.Y = Convert.ToInt32(m_LineList.InnerText);

                        m_LineList = root_Line.SelectSingleNode("Line_Style");
                        m_LineModel.Line_Style = Convert.ToBoolean(m_LineList.InnerText);

                        m_LineList = root_Line.SelectSingleNode("Line_IsZero");
                        m_LineModel.Line_IsZero = Convert.ToBoolean(m_LineList.InnerText);

                        m_LineList = root_Line.SelectSingleNode("Line_Joint1");
                        m_LineModel.Line_Joint1 = Convert.ToInt32(m_LineList.InnerText);

                        m_LineList = root_Line.SelectSingleNode("Line_Joint2");
                        m_LineModel.Line_Joint2 = Convert.ToInt32(m_LineList.InnerText);

                        m_LineModelList.Add(m_LineModel);

                        m_Line_Num++;
                    }

                    #endregion

                    #region 读取节点信息

                    XmlNodeList root_JointList = root.SelectSingleNode("JointList").ChildNodes;
                    m_Joint_Num = 0;

                    foreach (XmlNode root_Joint in root_JointList)
                    {
                        JointModelClass m_JointModel = new JointModelClass();

                        XmlNode m_JointList = root_Joint.SelectSingleNode("Joint_Num");
                        m_JointModel.Joint_Num = Convert.ToInt32(m_JointList.InnerText);

                        m_JointList = root_Joint.SelectSingleNode("Joint_Count");
                        m_JointModel.Joint_Count = Convert.ToInt32(m_JointList.InnerText);

                        m_JointList = root_Joint.SelectSingleNode("Joint_Point_X");
                        m_JointModel.Joint_Point.X = Convert.ToInt32(m_JointList.InnerText);

                        m_JointList = root_Joint.SelectSingleNode("Joint_Point_Y");
                        m_JointModel.Joint_Point.Y = Convert.ToInt32(m_JointList.InnerText);

                        m_JointList = root_Joint.SelectSingleNode("Joint_Load");
                        m_JointModel.Joint_Load = Convert.ToInt32(m_JointList.InnerText);

                        m_JointList = root_Joint.SelectSingleNode("Joint_Zhizuo");
                        m_JointModel.Joint_Zhizuo = Convert.ToInt32(m_JointList.InnerText);


                        for (int i = 0; i < m_JointModel.Joint_Count; i++)
                        {
                            m_JointList = root_Joint.SelectSingleNode("Joint_Line" + i);
                            m_JointModel.Joint_Line[i] = Convert.ToInt32(m_JointList.InnerText);
                        }

                        m_JointModelList.Add(m_JointModel);

                        m_Joint_Num++;
                    }

                    #endregion

                    #region 重新绘图

                    #region 重新载入画布

                    #region     分区线

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

                    #endregion

                    #region 重新绘制杆件

                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        m_LineNow = new Line();

                        m_LineNow.Stroke = Brushes.Black;
                        m_LineNow.StrokeThickness = 1;
                        m_LineNow.X1 = m_LineModelList[i].Line_BeginPoint.X;
                        m_LineNow.X2 = m_LineModelList[i].Line_EndPoint.X;
                        m_LineNow.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                        m_LineNow.Y2 = m_LineModelList[i].Line_EndPoint.Y;

                        #region     加标签

                        TextBlock f_TBline = new TextBlock();
                        f_TBX = (m_LineModelList[i].Line_BeginPoint.X + m_LineModelList[i].Line_EndPoint.X) / 2;
                        f_TBY = (m_LineModelList[i].Line_BeginPoint.Y + m_LineModelList[i].Line_EndPoint.Y) / 2 + 10;
                        f_TBscale = new ScaleTransform();
                        f_TBscale.ScaleY = -1;
                        f_TBline.RenderTransform = f_TBscale;
                        f_TBline.Foreground = Brushes.Blue;
                        f_TBline.Text = "(" + (i + 1) + ")";
                        f_TBline.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                        can.Children.Add(f_TBline);

                        #endregion

                        can.Children.Add(m_LineNow);
                    }

                    #endregion

                    #region 重新绘制结点

                    for (int i = 0; i < m_Joint_Num; i++)
                    {
                        Ellipse JointEllipse = new Ellipse();
                        JointEllipse.Height = 8;
                        JointEllipse.Width = 8;
                        JointEllipse.Fill = Brushes.Black;
                        JointEllipse.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 4);
                        JointEllipse.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 4);
                        can.Children.Add(JointEllipse);
                        Ellipse JointEllipse2 = new Ellipse();
                        JointEllipse2.Height = 6;
                        JointEllipse2.Width = 6;
                        JointEllipse2.Fill = Brushes.White;
                        JointEllipse2.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 3);
                        JointEllipse2.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 3);
                        can.Children.Add(JointEllipse2);
                    }

                    #endregion

                    #region 重新绘制支座

                    for (int i = 0; i < m_Joint_Num; i++)
                    {
                        if (m_JointModelList[i].Joint_Zhizuo == 1)
                        {
                            BitmapImage Zhizuo_Root = new BitmapImage(new Uri("/Image/ZhizuoX.png", UriKind.Relative));
                            Image Zhizuo = new Image();
                            Zhizuo.Source = Zhizuo_Root;
                            Zhizuo.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 40);
                            Zhizuo.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 22);
                            can.Children.Add(Zhizuo);
                        }
                        if (m_JointModelList[i].Joint_Zhizuo == 2)
                        {
                            BitmapImage Zhizuo_Root = new BitmapImage(new Uri("/Image/ZhizuoY.png", UriKind.Relative));
                            Image Zhizuo = new Image();
                            Zhizuo.Source = Zhizuo_Root;
                            Zhizuo.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 22);
                            Zhizuo.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y + 5);
                            ScaleTransform Zhizuo_scale = new ScaleTransform();
                            Zhizuo_scale.ScaleY = -1;
                            Zhizuo.RenderTransform = Zhizuo_scale;
                            can.Children.Add(Zhizuo);
                        }
                        if (m_JointModelList[i].Joint_Zhizuo == 3)
                        {
                            BitmapImage Zhizuo_Root = new BitmapImage(new Uri("/Image/ZhizuoX.png", UriKind.Relative));
                            Image Zhizuo = new Image();
                            Zhizuo.Source = Zhizuo_Root;
                            Zhizuo.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 40);
                            Zhizuo.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 22);
                            can.Children.Add(Zhizuo);
                            Zhizuo_Root = new BitmapImage(new Uri("/Image/ZhizuoY.png", UriKind.Relative));
                            Zhizuo = new Image();
                            Zhizuo.Source = Zhizuo_Root;
                            Zhizuo.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 22);
                            Zhizuo.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y + 5);
                            ScaleTransform Zhizuo_scale = new ScaleTransform();
                            Zhizuo_scale.ScaleY = -1;
                            Zhizuo.RenderTransform = Zhizuo_scale;
                            can.Children.Add(Zhizuo);
                        }
                    }

                    #endregion

                    #region     新建清空
                    Button qk = new Button();
                    qk.Width = 117;
                    qk.Height = 48;
                    qk.Content = "清空";
                    ScaleTransform f_TBrulerscale = new ScaleTransform();
                    f_TBrulerscale.ScaleY = -1;
                    qk.RenderTransform = f_TBrulerscale;
                    qk.Margin = new Thickness(1053, 53, 0, 0);
                    can.Children.Add(qk);
                    qk.Click += qingkong;
                    #endregion

                    #endregion

                    MessageBox.Show("桁架读取成功，可以添加载荷进行练习");

                    order = 21;
                }
                catch
                {
                    MessageBox.Show("未能找到桁架文件，请尝试重新输入路径");
                }
            }

        }

        #endregion

        #region order 21 — 22 添加载荷

        private void LoadLoad(object sender, RoutedEventArgs e)
        {
            if (order == 13)
            {
                MessageBox.Show("请先完成支座添加");
            }
            if (order == 21 || order == 14 || order == 15)
            {
                Load = true;
                order = 22;
            }
        }

        #endregion

        #region  order 22 — 23 完成添加 计算支座反力

        private void SaveLoad(object sender, RoutedEventArgs e)
        {
            if (order == 22)
            {
                double[,] A = new double[100, 100];
                double[] B = new double[100];
                double[] X = new double[100];
                Load = false;

                #region 载荷添加到结点上

                for (int i = 1; i < m_Load_Num + 1; i++)
                {
                    m_JointModelList[m_LoadModelList[i].Load_Joint].Joint_Load = i;
                }

                #endregion

                #region 系数准备

                int b = m_Line_Num;

                for (int i = 0, a = 0; i < m_Joint_Num; i++, a = a + 2)
                {
                    int n = m_JointModelList[i].Joint_Load;

                    for (int j = 0; j < m_JointModelList[i].Joint_Count; j++)
                    {
                        int k = m_JointModelList[i].Joint_Line[j];

                        if (m_LineModelList[k].Line_Style == true)
                        {
                            if (m_LineModelList[k].Line_BeginPoint.Y == m_JointModelList[i].Joint_Point.Y)
                            {
                                A[a, k] = 0;
                                A[a + 1, k] = 1;
                            }
                            else
                            {
                                A[a, k] = 0;
                                A[a + 1, k] = -1;
                            }
                        }

                        if (m_LineModelList[k].Line_Style == false)
                        {
                            if (m_LineModelList[k].Line_BeginPoint.X == m_JointModelList[i].Joint_Point.X)
                            {
                                A[a, k] = Math.Cos(Math.Atan(m_LineModelList[k].Line_Angle));
                                A[a + 1, k] = Math.Sin(Math.Atan(m_LineModelList[k].Line_Angle));
                            }
                            else
                            {
                                A[a, k] = (-1) * Math.Cos(Math.Atan(m_LineModelList[k].Line_Angle));
                                A[a + 1, k] = (-1) * Math.Sin(Math.Atan(m_LineModelList[k].Line_Angle));
                            }
                        }

                    }

                    if (m_LoadModelList[n].Load_Style == true)
                    {
                        if (m_LoadModelList[n].Load_Sign == true)
                        {
                            B[a] = 0;
                            B[a + 1] = m_LoadModelList[n].Load_Y;
                        }

                        if (m_LoadModelList[n].Load_Sign == false)
                        {
                            B[a] = 0;
                            B[a + 1] = (-1) * m_LoadModelList[n].Load_Y;
                        }
                    }

                    if (m_LoadModelList[n].Load_Style == false)
                    {
                        if (m_LoadModelList[n].Load_Sign == true)
                        {
                            if (m_LoadModelList[n].Load_Angle >= 0)
                            {
                                B[a] = m_LoadModelList[n].Load_X;
                                B[a + 1] = m_LoadModelList[n].Load_Y;
                            }
                            if (m_LoadModelList[n].Load_Angle < 0)
                            {
                                B[a] = (-1) * m_LoadModelList[n].Load_X;
                                B[a + 1] = m_LoadModelList[n].Load_Y;
                            }
                        }

                        if (m_LoadModelList[n].Load_Sign == false)
                        {
                            if (m_LoadModelList[n].Load_Angle >= 0)
                            {
                                B[a] = (-1) * m_LoadModelList[n].Load_X;
                                B[a + 1] = (-1) * m_LoadModelList[n].Load_Y;
                            }
                            if (m_LoadModelList[n].Load_Angle < 0)
                            {
                                B[a] = m_LoadModelList[n].Load_X;
                                B[a + 1] = (-1) * m_LoadModelList[n].Load_Y;
                            }
                        }
                    }

                    if (m_JointModelList[i].Joint_Zhizuo == 1)
                    {
                        A[a, b] = 1;
                        A[a + 1, b] = 0;
                        b++;
                    }

                    if (m_JointModelList[i].Joint_Zhizuo == 2)
                    {
                        A[a, b] = 0;
                        A[a + 1, b] = 1;
                        b++;
                    }

                    if (m_JointModelList[i].Joint_Zhizuo == 3)
                    {
                        A[a, b] = 1;
                        b++;
                        A[a + 1, b] = 1;
                        b++;
                    }

                }

                #endregion

                #region 计算外载荷值

                var matrixA = new DenseMatrix(2 * m_Joint_Num);
                var vectorB = new DenseVector(2 * m_Joint_Num);

                for (int i = 0; i < 2 * m_Joint_Num; i++)
                {
                    for (int j = 0; j < 2 * m_Joint_Num; j++)
                    {
                        matrixA[i, j] = A[i, j];
                    }
                }
                for (int i = 0; i < 2 * m_Joint_Num; i++)
                {
                    vectorB[i] = B[i];
                }

                var resultX = matrixA.LU().Solve(vectorB);

                for (int i = 0; i < 2 * m_Joint_Num; i++)
                {
                    X[i] = resultX[i];
                }

                #endregion

                #region 支座反力

                for (int i = 0, a = 0; i < m_Joint_Num; i++)
                {
                    if (m_JointModelList[i].Joint_Zhizuo == 1)
                    {
                        m_LoadModel = new LoadModelClass();
                        m_Load_Num++;
                        m_LoadModel.Load_Style = false;
                        m_LoadModel.Load_Joint = i;
                        if (Math.Abs(X[m_Line_Num + a]) > 0.0001)
                        {
                            m_LoadModel.Load_X = X[m_Line_Num + a];
                        }
                        else
                        {
                            m_LoadModel.Load_X = 0;
                        }

                        if (m_LoadModel.Load_X < 0)
                        {
                            m_LoadModel.Load_Sign = false;
                        }
                        if (m_LoadModel.Load_X > 0)
                        {
                            m_LoadModel.Load_Sign = true;
                        }
                        m_LoadModel.Load_X = Math.Abs(m_LoadModel.Load_X);
                        m_LoadModel.Load_Y = 0;
                        m_LoadModelList.Add(m_LoadModel);
                        m_JointModelList[i].Joint_Load = m_Load_Num;
                        a++;
                    }

                    if (m_JointModelList[i].Joint_Zhizuo == 2)
                    {
                        m_LoadModel = new LoadModelClass();
                        m_Load_Num++;
                        m_LoadModel.Load_Style = true;
                        m_LoadModel.Load_Joint = i;
                        if (Math.Abs(X[m_Line_Num + a]) > 0.0001)
                        {
                            m_LoadModel.Load_Y = X[m_Line_Num + a];
                        }
                        else { m_LoadModel.Load_Y = 0; }

                        if (m_LoadModel.Load_Y < 0)
                        {
                            m_LoadModel.Load_Sign = false;
                        }
                        if (m_LoadModel.Load_Y > 0)
                        {
                            m_LoadModel.Load_Sign = true;
                        }
                        m_LoadModel.Load_Y = Math.Abs(m_LoadModel.Load_Y);
                        m_LoadModel.Load_X = 0;
                        m_LoadModelList.Add(m_LoadModel);
                        m_JointModelList[i].Joint_Load = m_Load_Num;
                        a++;
                    }

                    if (m_JointModelList[i].Joint_Zhizuo == 3)
                    {

                        m_LoadModel = new LoadModelClass();
                        m_Load_Num++;
                        m_LoadModel.Load_Style = false;
                        m_LoadModel.Load_Joint = i;

                        if (Math.Abs (X[m_Line_Num + a]) > 0.0001)
                        {
                            m_LoadModel.Load_X = X[m_Line_Num + a];
                        }
                        else { m_LoadModel.Load_X = 0; }

                        a++;

                        if (Math.Abs(X[m_Line_Num + a]) > 0.0001)
                        {
                            m_LoadModel.Load_Y = X[m_Line_Num + a];
                        }
                        else { m_LoadModel.Load_Y = 0; }

                        if (m_LoadModel.Load_X == 0)
                        {
                            m_LoadModel.Load_Style = true;
                        }

                        if (m_LoadModel.Load_X != 0)
                        {
                            m_LoadModel.Load_Angle = m_LoadModel.Load_Y / m_LoadModel.Load_X;
                        }

                        if (m_LoadModel.Load_Y < 0)
                        {
                            m_LoadModel.Load_Sign = false;
                        }

                        if (m_LoadModel.Load_Y > 0)
                        {
                            m_LoadModel.Load_Sign = true;
                        }

                        if (m_LoadModel.Load_Y == 0)
                        {
                            if (m_LoadModel.Load_X > 0)
                            {
                                m_LoadModel.Load_Sign = true;
                            }

                            if (m_LoadModel.Load_X < 0)
                            {
                                m_LoadModel.Load_Sign = false;
                            }
                        }

                        m_LoadModel.Load_Y = Math.Abs(m_LoadModel.Load_Y);

                        m_LoadModel.Load_X = Math.Abs(m_LoadModel.Load_X);

                        m_LoadModelList.Add(m_LoadModel);
                        m_JointModelList[i].Joint_Load = m_Load_Num;
                        a++;
                    }
                }

                #endregion

                order = 23;

            }
        }

        #endregion

        #region order 23 — 24 判断零杆

        private void Start(object sender, RoutedEventArgs e)
        {
            if (order == 22)
            {
                MessageBox.Show("请先完成载荷添加");
            }
            if (order == 23)
            {
                Choose = true;
                order = 24;
            }
        }

        #endregion

        #region order 24 — 25 完成判断

        private void Over(object sender, RoutedEventArgs e)
        {
            if (order == 22)
            {
                MessageBox.Show("请先完成载荷添加");
            }
            if (order == 24)
            {
                Choose = false;
                int count = 0;
                bool flag = false;
                Line newLine = new Line();
                int line1 = 0;
                int line2 = 0;
                int Joint1 = 0;
                int Joint2 = 0;
                int Load = 0;

                while (flag == false)
                {
                    flag = true;

                    for (int i = 0; i < m_Line_Num; i++)
                    {
                        if (m_LineModelList[i].Line_Style == false)
                        {
                            if (m_LineModelList[i].Line_IsZero == false)
                            {
                                switch (m_JointModelList[m_LineModelList[i].Line_Joint1].Joint_Count)
                                {
                                    case 1:

                                        #region 读取结点数据

                                        Joint1 = m_LineModelList[i].Line_Joint1;
                                        Joint2 = m_LineModelList[i].Line_Joint2;

                                        #endregion

                                        #region 判断零杆

                                        if (m_JointModelList[Joint1].Joint_Load == 0 ||
                                            (m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_Y == 0 && m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_X == 0))
                                        {
                                            m_LineModelList[i].Line_IsZero = true;
                                        }

                                        #endregion

                                        #region 判断结果反馈

                                        if (m_LineModelList[i].Line_IsZero == true ||
                                            (m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_Y == 0 && m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_X == 0))
                                        {
                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;

                                            for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                            {
                                                if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                    for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                    }
                                                }
                                            }

                                            newLine = new Line();
                                            newLine.Stroke = Brushes.White;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            newLine = new Line();
                                            newLine.Stroke = Brushes.Green;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            count++;
                                            flag = false;
                                        }

                                        #endregion

                                        break;

                                    case 2:

                                        #region 读取结点数据

                                        Joint1 = m_LineModelList[i].Line_Joint1;
                                        Joint2 = m_LineModelList[i].Line_Joint2;
                                        if (i == m_JointModelList[Joint1].Joint_Line[0])
                                        {
                                            line1 = m_JointModelList[Joint1].Joint_Line[1];
                                        }
                                        if (i == m_JointModelList[Joint1].Joint_Line[1])
                                        {
                                            line1 = m_JointModelList[Joint1].Joint_Line[0];
                                        }
                                        Load = m_JointModelList[Joint1].Joint_Load;

                                        #endregion

                                        #region 判断零杆

                                        if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                        {
                                            if (!(m_LineModelList[line1].Line_Style == true && m_LineModelList[i].Line_Style == true) &&
                                                !(m_LineModelList[line1].Line_Style == false && m_LineModelList[i].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[i].Line_Angle))
                                            {
                                                m_LineModelList[i].Line_IsZero = true;
                                            }
                                        }

                                        if (Load != 0 && (m_LoadModelList[Load].Load_X != 0 || m_LoadModelList[Load].Load_Y != 0))
                                        {
                                            if (!(m_LineModelList[line1].Line_Style == true && m_LineModelList[i].Line_Style == true) &&
                                                !(m_LineModelList[line1].Line_Style == false && m_LineModelList[i].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[i].Line_Angle))
                                            {

                                                if (m_LoadModelList[Load].Load_Style == true)
                                                {
                                                    if (m_LineModelList[line1].Line_Style == true)
                                                    {
                                                        m_LineModelList[i].Line_IsZero = true;
                                                    }
                                                }

                                                if (m_LoadModelList[Load].Load_Style == false && m_LineModelList[line1].Line_Style == false)
                                                {
                                                    if (m_LineModelList[line1].Line_Angle == m_LoadModelList[Load].Load_Angle)
                                                    {
                                                        m_LineModelList[i].Line_IsZero = true;
                                                    }

                                                }

                                            }

                                        }

                                        #endregion

                                        #region 判断结果反馈

                                        if (m_LineModelList[i].Line_IsZero == true)
                                        {
                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                            m_JointModelList[Joint1].Joint_Line[0] = line1;

                                            for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                            {
                                                if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                    for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                    }
                                                }
                                            }

                                            newLine = new Line();
                                            newLine.Stroke = Brushes.White;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            newLine = new Line();
                                            newLine.Stroke = Brushes.Green;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            count++;
                                            flag = false;
                                        }

                                        #endregion

                                        break;

                                    case 3:

                                        #region 读取结点数据

                                        Joint1 = m_LineModelList[i].Line_Joint1;
                                        Joint2 = m_LineModelList[i].Line_Joint2;
                                        if (i == m_JointModelList[Joint1].Joint_Line[0])
                                        {
                                            line1 = m_JointModelList[Joint1].Joint_Line[1];
                                            line2 = m_JointModelList[Joint1].Joint_Line[2];
                                        }
                                        if (i == m_JointModelList[Joint1].Joint_Line[1])
                                        {
                                            line1 = m_JointModelList[Joint1].Joint_Line[0];
                                            line2 = m_JointModelList[Joint1].Joint_Line[2];
                                        }
                                        if (i == m_JointModelList[Joint1].Joint_Line[2])
                                        {
                                            line1 = m_JointModelList[Joint1].Joint_Line[0];
                                            line2 = m_JointModelList[Joint1].Joint_Line[1];
                                        }
                                        Load = m_JointModelList[Joint1].Joint_Load;

                                        #endregion

                                        #region 判断零杆
                                        if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                        {
                                            if ((m_LineModelList[line1].Line_Style == false && m_LineModelList[line2].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[line2].Line_Angle) ||
                                                (m_LineModelList[line1].Line_Style == true && m_LineModelList[line2].Line_Style == true))
                                            {
                                                m_LineModelList[i].Line_IsZero = true;
                                            }
                                        }
                                        #endregion

                                        #region 判断结果反馈

                                        if (m_LineModelList[i].Line_IsZero == true)
                                        {
                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                            m_JointModelList[Joint1].Joint_Line[0] = line1;
                                            m_JointModelList[Joint1].Joint_Line[1] = line2;

                                            for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                            {
                                                if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                    for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                    }
                                                }
                                            }

                                            newLine = new Line();
                                            newLine.Stroke = Brushes.White;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            newLine = new Line();
                                            newLine.Stroke = Brushes.Green;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            count++;
                                            flag = false;
                                        }

                                        #endregion

                                        break;

                                    default:

                                        break;
                                }

                                if (m_LineModelList[i].Line_IsZero == false)
                                {
                                    switch (m_JointModelList[m_LineModelList[i].Line_Joint2].Joint_Count)
                                    {
                                        case 1:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;

                                            #endregion

                                            #region 判断零杆

                                            if (m_JointModelList[Joint2].Joint_Load == 0 ||
                                                (m_LoadModelList[m_JointModelList[Joint2].Joint_Load].Load_Y == 0 && m_LoadModelList[m_JointModelList[Joint2].Joint_Load].Load_X == 0))
                                            {
                                                m_LineModelList[i].Line_IsZero = true;
                                            }

                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true)
                                            {
                                                m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;

                                                for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Green;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                count++;
                                                flag = false;
                                            }

                                            #endregion

                                            break;

                                        case 2:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;
                                            if (i == m_JointModelList[Joint2].Joint_Line[0])
                                            {
                                                line1 = m_JointModelList[Joint2].Joint_Line[1];
                                            }
                                            if (i == m_JointModelList[Joint2].Joint_Line[1])
                                            {
                                                line1 = m_JointModelList[Joint2].Joint_Line[0];
                                            }
                                            Load = m_JointModelList[Joint2].Joint_Load;

                                            #endregion

                                            #region 判断零杆

                                            if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                            {
                                                if (!(m_LineModelList[line1].Line_Style == true && m_LineModelList[i].Line_Style == true) &&
                                                    !(m_LineModelList[line1].Line_Style == false && m_LineModelList[i].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[i].Line_Angle))
                                                {
                                                    m_LineModelList[i].Line_IsZero = true;
                                                }
                                            }

                                            if (Load != 0 && (m_LoadModelList[Load].Load_X != 0 || m_LoadModelList[Load].Load_Y != 0))
                                            {
                                                if (!(m_LineModelList[line1].Line_Style == true && m_LineModelList[i].Line_Style == true) &&
                                                    !(m_LineModelList[line1].Line_Style == false && m_LineModelList[i].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[i].Line_Angle))
                                                {
                                                    if (m_LoadModelList[Load].Load_Style == true)
                                                    {
                                                        if (m_LineModelList[line1].Line_Style == true)
                                                        {
                                                            m_LineModelList[i].Line_IsZero = true;
                                                        }
                                                    }

                                                    if (m_LoadModelList[Load].Load_Style == false && m_LineModelList[line1].Line_Style == false)
                                                    {
                                                        if (m_LineModelList[line1].Line_Angle == m_LoadModelList[Load].Load_Angle)
                                                        {
                                                            m_LineModelList[i].Line_IsZero = true;
                                                        }
                                                    }

                                                }

                                            }

                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true)
                                            {
                                                m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                m_JointModelList[Joint2].Joint_Line[0] = line1;

                                                for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Green;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                count++;
                                                flag = false;
                                            }

                                            #endregion

                                            break;

                                        case 3:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;
                                            if (i == m_JointModelList[Joint2].Joint_Line[0])
                                            {
                                                line1 = m_JointModelList[Joint2].Joint_Line[1];
                                                line2 = m_JointModelList[Joint2].Joint_Line[2];
                                            }
                                            if (i == m_JointModelList[Joint2].Joint_Line[1])
                                            {
                                                line1 = m_JointModelList[Joint2].Joint_Line[0];
                                                line2 = m_JointModelList[Joint2].Joint_Line[2];
                                            }
                                            if (i == m_JointModelList[Joint2].Joint_Line[2])
                                            {
                                                line1 = m_JointModelList[Joint2].Joint_Line[0];
                                                line2 = m_JointModelList[Joint2].Joint_Line[1];
                                            }
                                            Load = m_JointModelList[Joint2].Joint_Load;

                                            #endregion

                                            #region 判断零杆
                                            if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                            {
                                                if ((m_LineModelList[line1].Line_Style == false && m_LineModelList[line2].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[line2].Line_Angle) ||
                                                    (m_LineModelList[line1].Line_Style == true && m_LineModelList[line2].Line_Style == true))
                                                {
                                                    m_LineModelList[i].Line_IsZero = true;
                                                }
                                            }
                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true)
                                            {
                                                m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                m_JointModelList[Joint2].Joint_Line[0] = line1;
                                                m_JointModelList[Joint2].Joint_Line[1] = line2;

                                                for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Green;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                count++;
                                                flag = false;
                                            }

                                            #endregion

                                            break;

                                        default:

                                            break;
                                    }


                                }
                            }
                        }

                        if (m_LineModelList[i].Line_Style == true)
                        {
                            if (m_LineModelList[i].Line_IsZero == false)
                            {
                                switch (m_JointModelList[m_LineModelList[i].Line_Joint1].Joint_Count)
                                {
                                    case 1:

                                        #region 读取结点数据

                                        Joint1 = m_LineModelList[i].Line_Joint1;
                                        Joint2 = m_LineModelList[i].Line_Joint2;

                                        #endregion

                                        #region 判断零杆

                                        if (m_JointModelList[Joint1].Joint_Load == 0 ||
                                            (m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_Y == 0 && m_LoadModelList[m_JointModelList[Joint1].Joint_Load].Load_X == 0))
                                        {
                                            m_LineModelList[i].Line_IsZero = true;
                                        }

                                        #endregion

                                        #region 判断结果反馈

                                        if (m_LineModelList[i].Line_IsZero == true)
                                        {
                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;

                                            for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                            {
                                                if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                    for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                    }
                                                }
                                            }

                                            newLine = new Line();
                                            newLine.Stroke = Brushes.White;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            newLine = new Line();
                                            newLine.Stroke = Brushes.Green;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            count++;
                                            flag = false;
                                        }

                                        #endregion

                                        break;

                                    case 2:

                                        #region 读取结点数据

                                        Joint1 = m_LineModelList[i].Line_Joint1;
                                        Joint2 = m_LineModelList[i].Line_Joint2;
                                        if (i == m_JointModelList[Joint1].Joint_Line[0])
                                        {
                                            line1 = m_JointModelList[Joint1].Joint_Line[1];
                                        }
                                        if (i == m_JointModelList[Joint1].Joint_Line[1])
                                        {
                                            line1 = m_JointModelList[Joint1].Joint_Line[0];
                                        }
                                        Load = m_JointModelList[Joint1].Joint_Load;

                                        #endregion

                                        #region 判断零杆

                                        if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                        {
                                            if (m_LineModelList[line1].Line_Style == false)
                                            {
                                                m_LineModelList[i].Line_IsZero = true;
                                            }
                                        }

                                        if (Load != 0 && (m_LoadModelList[Load].Load_X != 0 || m_LoadModelList[Load].Load_Y != 0))
                                        {
                                            if (m_LineModelList[line1].Line_Style == false)
                                            {
                                                if (m_LoadModelList[Load].Load_Style == false)
                                                {
                                                    if (m_LineModelList[line1].Line_Angle == m_LoadModelList[Load].Load_Angle)
                                                    {
                                                        m_LineModelList[i].Line_IsZero = true;
                                                    }
                                                }
                                            }

                                        }

                                        #endregion

                                        #region 判断结果反馈

                                        if (m_LineModelList[i].Line_IsZero == true)
                                        {
                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                            m_JointModelList[Joint1].Joint_Line[0] = line1;

                                            for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                            {
                                                if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                    for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                    }
                                                }
                                            }

                                            newLine = new Line();
                                            newLine.Stroke = Brushes.White;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            newLine = new Line();
                                            newLine.Stroke = Brushes.Green;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            count++;
                                            flag = false;
                                        }

                                        #endregion

                                        break;

                                    case 3:

                                        #region 读取结点数据

                                        Joint1 = m_LineModelList[i].Line_Joint1;
                                        Joint2 = m_LineModelList[i].Line_Joint2;

                                        if (i == m_JointModelList[Joint1].Joint_Line[0])
                                        {
                                            line1 = m_JointModelList[Joint1].Joint_Line[1];
                                            line2 = m_JointModelList[Joint1].Joint_Line[2];
                                        }
                                        if (i == m_JointModelList[Joint1].Joint_Line[1])
                                        {
                                            line1 = m_JointModelList[Joint1].Joint_Line[0];
                                            line2 = m_JointModelList[Joint1].Joint_Line[2];
                                        }
                                        if (i == m_JointModelList[Joint1].Joint_Line[2])
                                        {
                                            line1 = m_JointModelList[Joint1].Joint_Line[0];
                                            line2 = m_JointModelList[Joint1].Joint_Line[1];
                                        }
                                        Load = m_JointModelList[Joint1].Joint_Load;

                                        #endregion

                                        #region 判断零杆
                                        if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                        {
                                            if ((m_LineModelList[line1].Line_Style == false && m_LineModelList[line2].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[line2].Line_Angle) ||
                                                (m_LineModelList[line1].Line_Style == true && m_LineModelList[line2].Line_Style == true))
                                            {
                                                m_LineModelList[i].Line_IsZero = true;
                                            }
                                        }
                                        #endregion

                                        #region 判断结果反馈

                                        if (m_LineModelList[i].Line_IsZero == true)
                                        {
                                            m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                            m_JointModelList[Joint1].Joint_Line[0] = line1;
                                            m_JointModelList[Joint1].Joint_Line[1] = line2;

                                            for (int j = 0; j < m_JointModelList[Joint2].Joint_Count; j++)
                                            {
                                                if (m_JointModelList[Joint2].Joint_Line[j] == i)
                                                {
                                                    m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                    for (int k = j; k < m_JointModelList[Joint2].Joint_Count; k++)
                                                    {
                                                        m_JointModelList[Joint2].Joint_Line[k] = m_JointModelList[Joint2].Joint_Line[k + 1];
                                                    }
                                                }
                                            }

                                            newLine = new Line();
                                            newLine.Stroke = Brushes.White;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            newLine = new Line();
                                            newLine.Stroke = Brushes.Green;
                                            newLine.StrokeThickness = 1;
                                            newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                            newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                            newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                            newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                            can.Children.Add(newLine);
                                            count++;
                                            flag = false;
                                        }

                                        #endregion

                                        break;

                                    default:

                                        break;
                                }

                                if (m_LineModelList[i].Line_IsZero == false)
                                {
                                    switch (m_JointModelList[m_LineModelList[i].Line_Joint2].Joint_Count)
                                    {
                                        case 1:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;

                                            #endregion

                                            #region 判断零杆

                                            if (m_JointModelList[Joint2].Joint_Load == 0 ||
                                                (m_LoadModelList[m_JointModelList[Joint2].Joint_Load].Load_Y == 0 && m_LoadModelList[m_JointModelList[Joint2].Joint_Load].Load_X == 0))
                                            {
                                                m_LineModelList[i].Line_IsZero = true;
                                            }

                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true)
                                            {
                                                m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;

                                                for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Green;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                count++;
                                                flag = false;
                                            }

                                            #endregion

                                            break;

                                        case 2:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;
                                            if (i == m_JointModelList[Joint2].Joint_Line[0])
                                            {
                                                line1 = m_JointModelList[Joint2].Joint_Line[1];
                                            }
                                            if (i == m_JointModelList[Joint2].Joint_Line[1])
                                            {
                                                line1 = m_JointModelList[Joint2].Joint_Line[0];
                                            }
                                            Load = m_JointModelList[Joint2].Joint_Load;

                                            #endregion

                                            #region 判断零杆

                                            if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                            {
                                                if (m_LineModelList[line1].Line_Style == false)
                                                {
                                                    m_LineModelList[i].Line_IsZero = true;
                                                }
                                            }

                                            if (Load != 0 && (m_LoadModelList[Load].Load_X != 0 || m_LoadModelList[Load].Load_Y != 0))
                                            {
                                                if (m_LineModelList[line1].Line_Style == false)
                                                {
                                                    if (m_LoadModelList[Load].Load_Style == false)
                                                    {
                                                        if (m_LineModelList[line1].Line_Angle == m_LoadModelList[Load].Load_Angle)
                                                        {
                                                            m_LineModelList[i].Line_IsZero = true;
                                                        }
                                                    }
                                                }

                                            }

                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true)
                                            {
                                                m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                m_JointModelList[Joint2].Joint_Line[0] = line1;

                                                for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Green;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                count++;
                                                flag = false;
                                            }

                                            #endregion

                                            break;

                                        case 3:

                                            #region 读取结点数据

                                            Joint1 = m_LineModelList[i].Line_Joint1;
                                            Joint2 = m_LineModelList[i].Line_Joint2;
                                            if (i == m_JointModelList[Joint2].Joint_Line[0])
                                            {
                                                line1 = m_JointModelList[Joint2].Joint_Line[1];
                                                line2 = m_JointModelList[Joint2].Joint_Line[2];
                                            }
                                            if (i == m_JointModelList[Joint2].Joint_Line[1])
                                            {
                                                line1 = m_JointModelList[Joint2].Joint_Line[0];
                                                line2 = m_JointModelList[Joint2].Joint_Line[2];
                                            }
                                            if (i == m_JointModelList[Joint2].Joint_Line[2])
                                            {
                                                line1 = m_JointModelList[Joint2].Joint_Line[0];
                                                line2 = m_JointModelList[Joint2].Joint_Line[1];
                                            }
                                            Load = m_JointModelList[Joint2].Joint_Load;

                                            #endregion

                                            #region 判断零杆
                                            if (Load == 0 || (m_LoadModelList[Load].Load_X == 0 && m_LoadModelList[Load].Load_Y == 0))
                                            {
                                                if ((m_LineModelList[line1].Line_Style == false && m_LineModelList[line2].Line_Style == false && m_LineModelList[line1].Line_Angle == m_LineModelList[line2].Line_Angle) ||
                                                    (m_LineModelList[line1].Line_Style == true && m_LineModelList[line2].Line_Style == true))
                                                {
                                                    m_LineModelList[i].Line_IsZero = true;
                                                }
                                            }
                                            #endregion

                                            #region 判断结果反馈

                                            if (m_LineModelList[i].Line_IsZero == true)
                                            {
                                                m_JointModelList[Joint2].Joint_Count = m_JointModelList[Joint2].Joint_Count - 1;
                                                m_JointModelList[Joint2].Joint_Line[0] = line1;
                                                m_JointModelList[Joint2].Joint_Line[1] = line2;

                                                for (int j = 0; j < m_JointModelList[Joint1].Joint_Count; j++)
                                                {
                                                    if (m_JointModelList[Joint1].Joint_Line[j] == i)
                                                    {
                                                        m_JointModelList[Joint1].Joint_Count = m_JointModelList[Joint1].Joint_Count - 1;
                                                        for (int k = j; k < m_JointModelList[Joint1].Joint_Count; k++)
                                                        {
                                                            m_JointModelList[Joint1].Joint_Line[k] = m_JointModelList[Joint1].Joint_Line[k + 1];
                                                        }
                                                    }
                                                }

                                                newLine = new Line();
                                                newLine.Stroke = Brushes.White;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                newLine = new Line();
                                                newLine.Stroke = Brushes.Green;
                                                newLine.StrokeThickness = 1;
                                                newLine.X1 = m_LineModelList[i].Line_BeginPoint.X;
                                                newLine.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                                                newLine.X2 = m_LineModelList[i].Line_EndPoint.X;
                                                newLine.Y2 = m_LineModelList[i].Line_EndPoint.Y;
                                                can.Children.Add(newLine);
                                                count++;
                                                flag = false;
                                            }

                                            #endregion

                                            break;

                                        default:

                                            break;
                                    }
                                }
                            }
                        }

                    }
                }

                if (count == 0)
                {
                    MessageBox.Show("已找出当前载荷下的所有零杆，恭喜您");
                }
                else
                {
                    MessageBox.Show("当前载荷下仍有" + count.ToString() + "个零杆未找出，已用绿线标出，请继续努力");
                }

                order = 25;
            }
        }

        #endregion

        #region order 24 — 0 清除载荷

        private void Erase(object sender, RoutedEventArgs e)
        {
            if (order == 24 || order == 23 || order == 22 || order == 25)
            {
                Choose = false;

                #region 载荷初始化

                for (int i = 0; i < m_Line_Num; i++)
                {
                    if (m_LineModelList[i].Line_IsZero == true)
                    {
                        m_JointModelList[m_LineModelList[i].Line_Joint1].Joint_Count = m_JointModelList[m_LineModelList[i].Line_Joint1].Joint_Count + 1;
                        m_JointModelList[m_LineModelList[i].Line_Joint1].Joint_Line[m_JointModelList[m_LineModelList[i].Line_Joint1].Joint_Count - 1] = i;
                        m_JointModelList[m_LineModelList[i].Line_Joint2].Joint_Count = m_JointModelList[m_LineModelList[i].Line_Joint2].Joint_Count + 1;
                        m_JointModelList[m_LineModelList[i].Line_Joint2].Joint_Line[m_JointModelList[m_LineModelList[i].Line_Joint2].Joint_Count - 1] = i;
                    }
                }

                m_LoadModel = new LoadModelClass();
                m_LoadModelList.Clear();

                for (int i = 0; i < m_Joint_Num; i++)
                {
                    m_JointModelList[i].Joint_Load = 0;
                }

                for (int i = 0; i < m_Line_Num; i++)
                {
                    m_LineModelList[i].Line_IsZero = false;
                }

                m_Load_Num = 0;

                #endregion

                #region 界面初始化

                can.Children.Clear();

                #region 重新载入画布

                #region     分区线

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

                #endregion

                #region 重新绘制杆件

                for (int i = 0; i < m_Line_Num; i++)
                {
                    m_LineNow = new Line();

                    m_LineNow.Stroke = Brushes.Black;
                    m_LineNow.StrokeThickness = 1;
                    m_LineNow.X1 = m_LineModelList[i].Line_BeginPoint.X;
                    m_LineNow.X2 = m_LineModelList[i].Line_EndPoint.X;
                    m_LineNow.Y1 = m_LineModelList[i].Line_BeginPoint.Y;
                    m_LineNow.Y2 = m_LineModelList[i].Line_EndPoint.Y;

                    #region     加标签

                    TextBlock f_TBline = new TextBlock();
                    f_TBX = (m_LineModelList[i].Line_BeginPoint.X + m_LineModelList[i].Line_EndPoint.X) / 2;
                    f_TBY = (m_LineModelList[i].Line_BeginPoint.Y + m_LineModelList[i].Line_EndPoint.Y) / 2 + 10;
                    f_TBscale = new ScaleTransform();
                    f_TBscale.ScaleY = -1;
                    f_TBline.RenderTransform = f_TBscale;
                    f_TBline.Foreground = Brushes.Blue;
                    f_TBline.Text = "(" + (i + 1) + ")";
                    f_TBline.Margin = new Thickness(f_TBX, f_TBY, 0, 0);
                    can.Children.Add(f_TBline);

                    #endregion

                    can.Children.Add(m_LineNow);


                }

                #endregion

                #region 重新绘制结点

                for (int i = 0; i < m_Joint_Num; i++)
                {
                    Ellipse JointEllipse = new Ellipse();
                    JointEllipse.Height = 8;
                    JointEllipse.Width = 8;
                    JointEllipse.Fill = Brushes.Black;
                    JointEllipse.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 4);
                    JointEllipse.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 4);
                    can.Children.Add(JointEllipse);
                    Ellipse JointEllipse2 = new Ellipse();
                    JointEllipse2.Height = 6;
                    JointEllipse2.Width = 6;
                    JointEllipse2.Fill = Brushes.White;
                    JointEllipse2.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 3);
                    JointEllipse2.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 3);
                    can.Children.Add(JointEllipse2);
                }

                #endregion

                #region     新建清空
                Button qk = new Button();
                qk.Width = 117;
                qk.Height = 48;
                qk.Content = "清空";
                ScaleTransform f_TBrulerscale = new ScaleTransform();
                f_TBrulerscale.ScaleY = -1;
                qk.RenderTransform = f_TBrulerscale;
                qk.Margin = new Thickness(1053, 53, 0, 0);
                can.Children.Add(qk);
                qk.Click += qingkong;
                #endregion

                #region 重新绘制支座

                for (int i = 0; i < m_Joint_Num; i++)
                {
                    if (m_JointModelList[i].Joint_Zhizuo == 1)
                    {
                        Line Zhizuoline = new Line();
                        Zhizuoline.X1 = m_JointModelList[i].Joint_Point.X - 4;
                        Zhizuoline.Y1 = m_JointModelList[i].Joint_Point.Y;
                        Zhizuoline.X2 = m_JointModelList[i].Joint_Point.X - 29;
                        Zhizuoline.Y2 = m_JointModelList[i].Joint_Point.Y;
                        Zhizuoline.StrokeThickness = 2;
                        Zhizuoline.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline);

                        Ellipse ZhizuoEllipse = new Ellipse();
                        ZhizuoEllipse.Height = 8;
                        ZhizuoEllipse.Width = 8;
                        ZhizuoEllipse.Fill = Brushes.Black;
                        ZhizuoEllipse.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 33);
                        ZhizuoEllipse.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 4);
                        can.Children.Add(ZhizuoEllipse);
                        Ellipse ZhizuoEllipse2 = new Ellipse();
                        ZhizuoEllipse2.Height = 6;
                        ZhizuoEllipse2.Width = 6;
                        ZhizuoEllipse2.Fill = Brushes.White;
                        ZhizuoEllipse2.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 32);
                        ZhizuoEllipse2.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 3);
                        can.Children.Add(ZhizuoEllipse2);
                        Line Zhizuoline1 = new Line();
                        Zhizuoline1.X1 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline1.Y1 = m_JointModelList[i].Joint_Point.Y - 20;
                        Zhizuoline1.X2 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline1.Y2 = m_JointModelList[i].Joint_Point.Y + 20;
                        Zhizuoline1.StrokeThickness = 2;
                        Zhizuoline1.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline1);
                        #region 人类的本质就是复读机
                        Line Zhizuoline2 = new Line();
                        Zhizuoline2.X1 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline2.Y1 = m_JointModelList[i].Joint_Point.Y + 10;
                        Zhizuoline2.X2 = m_JointModelList[i].Joint_Point.X - 43;
                        Zhizuoline2.Y2 = m_JointModelList[i].Joint_Point.Y + 20;
                        Zhizuoline2.StrokeThickness = 1;
                        Zhizuoline2.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline2);
                        Line Zhizuoline3 = new Line();
                        Zhizuoline3.X1 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline3.Y1 = m_JointModelList[i].Joint_Point.Y;
                        Zhizuoline3.X2 = m_JointModelList[i].Joint_Point.X - 43;
                        Zhizuoline3.Y2 = m_JointModelList[i].Joint_Point.Y + 10;
                        Zhizuoline3.StrokeThickness = 1;
                        Zhizuoline3.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline3);
                        Line Zhizuoline4 = new Line();
                        Zhizuoline4.X1 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline4.Y1 = m_JointModelList[i].Joint_Point.Y - 10;
                        Zhizuoline4.X2 = m_JointModelList[i].Joint_Point.X - 43;
                        Zhizuoline4.Y2 = m_JointModelList[i].Joint_Point.Y;
                        Zhizuoline4.StrokeThickness = 1;
                        Zhizuoline4.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline4);
                        Line Zhizuoline5 = new Line();
                        Zhizuoline5.X1 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline5.Y1 = m_JointModelList[i].Joint_Point.Y - 20;
                        Zhizuoline5.X2 = m_JointModelList[i].Joint_Point.X - 43;
                        Zhizuoline5.Y2 = m_JointModelList[i].Joint_Point.Y - 10;
                        Zhizuoline5.StrokeThickness = 1;
                        Zhizuoline5.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline5);
                        #endregion
                    }
                    if (m_JointModelList[i].Joint_Zhizuo == 2)
                    {
                        Line Zhizuoline = new Line();
                        Zhizuoline.X1 = m_JointModelList[i].Joint_Point.X;
                        Zhizuoline.Y1 = m_JointModelList[i].Joint_Point.Y - 4;
                        Zhizuoline.X2 = m_JointModelList[i].Joint_Point.X;
                        Zhizuoline.Y2 = m_JointModelList[i].Joint_Point.Y - 29;
                        Zhizuoline.StrokeThickness = 2;
                        Zhizuoline.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline);

                        Ellipse ZhizuoEllipse = new Ellipse();
                        ZhizuoEllipse.Height = 8;
                        ZhizuoEllipse.Width = 8;
                        ZhizuoEllipse.Fill = Brushes.Black;
                        ZhizuoEllipse.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 4);
                        ZhizuoEllipse.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 33);
                        can.Children.Add(ZhizuoEllipse);
                        Ellipse ZhizuoEllipse2 = new Ellipse();
                        ZhizuoEllipse2.Height = 6;
                        ZhizuoEllipse2.Width = 6;
                        ZhizuoEllipse2.Fill = Brushes.White;
                        ZhizuoEllipse2.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 3);
                        ZhizuoEllipse2.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 32);
                        can.Children.Add(ZhizuoEllipse2);
                        Line Zhizuoline1 = new Line();
                        Zhizuoline1.X1 = m_JointModelList[i].Joint_Point.X - 20;
                        Zhizuoline1.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline1.X2 = m_JointModelList[i].Joint_Point.X + 20;
                        Zhizuoline1.Y2 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline1.StrokeThickness = 2;
                        Zhizuoline1.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline1);
                        #region 人类的本质就是复读机
                        Line Zhizuoline2 = new Line();
                        Zhizuoline2.X1 = m_JointModelList[i].Joint_Point.X + 10;
                        Zhizuoline2.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline2.X2 = m_JointModelList[i].Joint_Point.X + 20;
                        Zhizuoline2.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                        Zhizuoline2.StrokeThickness = 1;
                        Zhizuoline2.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline2);
                        Line Zhizuoline3 = new Line();
                        Zhizuoline3.X1 = m_JointModelList[i].Joint_Point.X;
                        Zhizuoline3.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline3.X2 = m_JointModelList[i].Joint_Point.X + 10;
                        Zhizuoline3.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                        Zhizuoline3.StrokeThickness = 1;
                        Zhizuoline3.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline3);
                        Line Zhizuoline4 = new Line();
                        Zhizuoline4.X1 = m_JointModelList[i].Joint_Point.X - 10;
                        Zhizuoline4.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline4.X2 = m_JointModelList[i].Joint_Point.X;
                        Zhizuoline4.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                        Zhizuoline4.StrokeThickness = 1;
                        Zhizuoline4.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline4);
                        Line Zhizuoline5 = new Line();
                        Zhizuoline5.X1 = m_JointModelList[i].Joint_Point.X - 20;
                        Zhizuoline5.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline5.X2 = m_JointModelList[i].Joint_Point.X - 10;
                        Zhizuoline5.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                        Zhizuoline5.StrokeThickness = 1;
                        Zhizuoline5.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline5);
                        #endregion
                    }
                    if (m_JointModelList[i].Joint_Zhizuo == 3)
                    {
                        Line Zhizuoline = new Line();
                        Zhizuoline.X1 = m_JointModelList[i].Joint_Point.X - 4;
                        Zhizuoline.Y1 = m_JointModelList[i].Joint_Point.Y;
                        Zhizuoline.X2 = m_JointModelList[i].Joint_Point.X - 29;
                        Zhizuoline.Y2 = m_JointModelList[i].Joint_Point.Y;
                        Zhizuoline.StrokeThickness = 2;
                        Zhizuoline.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline);

                        Ellipse ZhizuoEllipse = new Ellipse();
                        ZhizuoEllipse.Height = 8;
                        ZhizuoEllipse.Width = 8;
                        ZhizuoEllipse.Fill = Brushes.Black;
                        ZhizuoEllipse.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 33);
                        ZhizuoEllipse.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 4);
                        can.Children.Add(ZhizuoEllipse);
                        Ellipse ZhizuoEllipse2 = new Ellipse();
                        ZhizuoEllipse2.Height = 6;
                        ZhizuoEllipse2.Width = 6;
                        ZhizuoEllipse2.Fill = Brushes.White;
                        ZhizuoEllipse2.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 32);
                        ZhizuoEllipse2.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 3);
                        can.Children.Add(ZhizuoEllipse2);
                        Line Zhizuoline1 = new Line();
                        Zhizuoline1.X1 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline1.Y1 = m_JointModelList[i].Joint_Point.Y - 20;
                        Zhizuoline1.X2 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline1.Y2 = m_JointModelList[i].Joint_Point.Y + 20;
                        Zhizuoline1.StrokeThickness = 2;
                        Zhizuoline1.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline1);
                        #region 人类的本质就是复读机
                        Line Zhizuoline2 = new Line();
                        Zhizuoline2.X1 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline2.Y1 = m_JointModelList[i].Joint_Point.Y + 10;
                        Zhizuoline2.X2 = m_JointModelList[i].Joint_Point.X - 43;
                        Zhizuoline2.Y2 = m_JointModelList[i].Joint_Point.Y + 20;
                        Zhizuoline2.StrokeThickness = 1;
                        Zhizuoline2.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline2);
                        Line Zhizuoline3 = new Line();
                        Zhizuoline3.X1 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline3.Y1 = m_JointModelList[i].Joint_Point.Y;
                        Zhizuoline3.X2 = m_JointModelList[i].Joint_Point.X - 43;
                        Zhizuoline3.Y2 = m_JointModelList[i].Joint_Point.Y + 10;
                        Zhizuoline3.StrokeThickness = 1;
                        Zhizuoline3.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline3);
                        Line Zhizuoline4 = new Line();
                        Zhizuoline4.X1 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline4.Y1 = m_JointModelList[i].Joint_Point.Y - 10;
                        Zhizuoline4.X2 = m_JointModelList[i].Joint_Point.X - 43;
                        Zhizuoline4.Y2 = m_JointModelList[i].Joint_Point.Y;
                        Zhizuoline4.StrokeThickness = 1;
                        Zhizuoline4.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline4);
                        Line Zhizuoline5 = new Line();
                        Zhizuoline5.X1 = m_JointModelList[i].Joint_Point.X - 33;
                        Zhizuoline5.Y1 = m_JointModelList[i].Joint_Point.Y - 20;
                        Zhizuoline5.X2 = m_JointModelList[i].Joint_Point.X - 43;
                        Zhizuoline5.Y2 = m_JointModelList[i].Joint_Point.Y - 10;
                        Zhizuoline5.StrokeThickness = 1;
                        Zhizuoline5.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline5);
                        #endregion
                        Line Zhizuoline10 = new Line();
                        Zhizuoline10.X1 = m_JointModelList[i].Joint_Point.X;
                        Zhizuoline10.Y1 = m_JointModelList[i].Joint_Point.Y - 4;
                        Zhizuoline10.X2 = m_JointModelList[i].Joint_Point.X;
                        Zhizuoline10.Y2 = m_JointModelList[i].Joint_Point.Y - 29;
                        Zhizuoline10.StrokeThickness = 2;
                        Zhizuoline10.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline10);

                        Ellipse ZhizuoEllipse11 = new Ellipse();
                        ZhizuoEllipse11.Height = 8;
                        ZhizuoEllipse11.Width = 8;
                        ZhizuoEllipse11.Fill = Brushes.Black;
                        ZhizuoEllipse11.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 4);
                        ZhizuoEllipse11.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 33);
                        can.Children.Add(ZhizuoEllipse11);
                        Ellipse ZhizuoEllipse12 = new Ellipse();
                        ZhizuoEllipse12.Height = 6;
                        ZhizuoEllipse12.Width = 6;
                        ZhizuoEllipse12.Fill = Brushes.White;
                        ZhizuoEllipse12.SetValue(Canvas.LeftProperty, m_JointModelList[i].Joint_Point.X - 3);
                        ZhizuoEllipse12.SetValue(Canvas.TopProperty, m_JointModelList[i].Joint_Point.Y - 32);
                        can.Children.Add(ZhizuoEllipse12);
                        Line Zhizuoline11 = new Line();
                        Zhizuoline11.X1 = m_JointModelList[i].Joint_Point.X - 20;
                        Zhizuoline11.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline11.X2 = m_JointModelList[i].Joint_Point.X + 20;
                        Zhizuoline11.Y2 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline11.StrokeThickness = 2;
                        Zhizuoline11.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline11);
                        #region 人类的本质就是复读机
                        Line Zhizuoline12 = new Line();
                        Zhizuoline12.X1 = m_JointModelList[i].Joint_Point.X + 10;
                        Zhizuoline12.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline12.X2 = m_JointModelList[i].Joint_Point.X + 20;
                        Zhizuoline12.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                        Zhizuoline12.StrokeThickness = 1;
                        Zhizuoline12.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline12);
                        Line Zhizuoline13 = new Line();
                        Zhizuoline13.X1 = m_JointModelList[i].Joint_Point.X;
                        Zhizuoline13.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline13.X2 = m_JointModelList[i].Joint_Point.X + 10;
                        Zhizuoline13.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                        Zhizuoline13.StrokeThickness = 1;
                        Zhizuoline13.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline13);
                        Line Zhizuoline14 = new Line();
                        Zhizuoline14.X1 = m_JointModelList[i].Joint_Point.X - 10;
                        Zhizuoline14.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline14.X2 = m_JointModelList[i].Joint_Point.X;
                        Zhizuoline14.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                        Zhizuoline14.StrokeThickness = 1;
                        Zhizuoline14.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline14);
                        Line Zhizuoline15 = new Line();
                        Zhizuoline15.X1 = m_JointModelList[i].Joint_Point.X - 20;
                        Zhizuoline15.Y1 = m_JointModelList[i].Joint_Point.Y - 33;
                        Zhizuoline15.X2 = m_JointModelList[i].Joint_Point.X - 10;
                        Zhizuoline15.Y2 = m_JointModelList[i].Joint_Point.Y - 43;
                        Zhizuoline15.StrokeThickness = 1;
                        Zhizuoline15.Stroke = Brushes.Black;
                        can.Children.Add(Zhizuoline15);
                        #endregion
                    }
                }

                #endregion

                #endregion

                MessageBox.Show("请重新添加载荷");

                order = 21;
            }
            else
            {
                MessageBox.Show("原图中无载荷");
            }
        }


        #endregion

        #endregion

    }
}
