using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;


namespace Influential
{
    /// <summary>
    /// Window3.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        #region 变量
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

        #endregion


        //分辨率比
        double ResolutionRatio = 0;

        #region 界面触发事件

        #region 界面初始化部分（背景设置）
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
        Point pt = new Point();
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            pt = e.GetPosition(can);
            pt.X = Math.Round(pt.X / 20) * 20;
            pt.Y = Math.Round(pt.Y / 20) * 20;
            this.textBlock1.Text = (pt.X + "," + pt.Y);
        }
        #endregion

        #region 直线部分
        //开始绘制Button
        private bool isKaishu = false;
        double y = 0;
        private void kaishiBt_Click(object sender, RoutedEventArgs e)
        {
            isKaishu = true;
            kaishiBt.Background = Brushes.LightBlue;
            //虚线分界
            double x = SystemParameters.WorkArea.Width;
            y = (SystemParameters.WorkArea.Height) / 2;
            double Hx = SystemParameters.PrimaryScreenWidth;
            double Hy = SystemParameters.PrimaryScreenHeight;
            ResolutionRatio = (Hx / Hy) / (1366.0 / 768.0);
            Line dottedLine = new Line();
            dottedLine.X1 = 0;
            dottedLine.X2 = x;
            dottedLine.Y1 = y;
            dottedLine.Y2 = y;
            dottedLine.Stroke = Brushes.DarkBlue;
            dottedLine.StrokeThickness = 3;
            dottedLine.StrokeDashArray = new DoubleCollection() { 2, 3 };
            can.Children.Add(dottedLine);

            TextBlock tBlock = new TextBlock();
            tBlock.Margin = new Thickness(10, 2 * y - 80, 600, 600);
            tBlock.Width = 75;
            tBlock.Background = Brushes.DarkBlue;
            tBlock.Foreground = Brushes.White;
            tBlock.Text = "练习区";
            tBlock.FontSize = 18;
            tBlock.TextAlignment = TextAlignment.Center;
            ScaleTransform scale = new ScaleTransform();
            scale.ScaleY = -1;
            tBlock.RenderTransform = scale;
            can.Children.Add(tBlock);

            TextBlock tBlock2 = new TextBlock();
            tBlock2.Margin = new Thickness(10, y - 20, 600, 600);
            tBlock2.Width = 75;
            tBlock2.Background = Brushes.DarkBlue;
            tBlock2.Foreground = Brushes.White;
            tBlock2.Text = "正确结果";
            tBlock2.FontSize = 18;
            tBlock2.TextAlignment = TextAlignment.Center;
            tBlock2.RenderTransform = scale;
            can.Children.Add(tBlock2);
        }
        /// <summary>
        /// 鼠标左键点击 canvas函数 获取鼠标绘制线段的起点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void get_startPt(object sender, MouseButtonEventArgs e)
        {
            if (isKaishu == true && pt.Y > 420)
            {
                //初始化 Line 
                m_LineNow = new Line();
                //获取开始点
                startPoint = e.GetPosition(can);
                startPoint.X = (Math.Round(startPoint.X / 20) * 20);
                startPoint.Y = (Math.Round(startPoint.Y / 20) * 20);
                //设置开始点坐标
                m_LineNow.X1 = (Math.Round(startPoint.X / 20)) * 20;
                m_LineNow.Y1 = (Math.Round(startPoint.Y / 20)) * 20;

                m_LineNow.X2 = (Math.Round(startPoint.X / 20)) * 20;
                m_LineNow.Y2 = (Math.Round(startPoint.Y / 20)) * 20;
                m_LineNow.Name = "L" + (m_Line_Num + 1);
                m_LineModel.Line_BeginPoint = startPoint;
                //圆点加粗
                Ellipse startEllipse = new Ellipse();
                startEllipse.Height = 4;
                startEllipse.Width = 4;
                startEllipse.Fill = Brushes.Black;
                startEllipse.SetValue(Canvas.LeftProperty, startPoint.X - 2);
                startEllipse.SetValue(Canvas.TopProperty, startPoint.Y - 2);

                can.Children.Add(m_LineNow);
                can.Children.Add(startEllipse);
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// 鼠标在canvas上移动事件 通过移动路径绘制线段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void move(object sender, MouseEventArgs e)
        {
            if (isKaishu == true && pt.Y > 420)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point point = e.GetPosition(can);

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
                        //直线画在网格上
                        if (Math.Abs(point.Y - startPoint.Y) > Math.Abs(point.X - startPoint.X))//竖线//
                        {
                            m_LineNow.X2 = m_LineNow.X1;
                            m_LineNow.Y2 = (Math.Round(point.Y / 20)) * 20;
                        }
                        else //横线
                        {
                            m_LineNow.X2 = (Math.Round(point.X / 20)) * 20;
                            m_LineNow.Y2 = m_LineNow.Y1;
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 鼠标左键抬起函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        Point endPoint = new Point();
        TextBlock LineText = new TextBlock();
        private void can_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isKaishu == true && pt.Y > 420)
            {
                //获取线段终点
                endPoint = new Point();
                endPoint = e.GetPosition(can);
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

                //标线号
                LineText = new TextBlock();
                var LineTextX = (endPoint.X + startPoint.X) / 2;
                var LineTextY = (endPoint.Y + startPoint.Y) / 2 + 30;
                LineText.Text = "(" + Convert.ToString(m_Line_Num + 1) + ")";
                LineText.Margin = new Thickness(LineTextX, LineTextY, 0, 0);
                ScaleTransform LTscale = new ScaleTransform();
                LTscale.ScaleY = -1;
                LineText.RenderTransform = LTscale;
                can.Children.Add(LineText);

                //计算长度
                var length = Math.Sqrt(Math.Pow(m_LineNow.X1 - m_LineNow.X2, 2) + Math.Pow(m_LineNow.Y1 - m_LineNow.Y2, 2));
                m_LineModel.LineLength = length;

                //  建立对应Line模型
                m_LineModel.LineInfo = m_LineNow;

                //命名
                m_LineModel.LineName = m_LineNow.Name;
                //线段数量加一
                m_Line_Num++;

                //将当前线段保存到List
                m_LineModelList.Add(m_LineModel);
                m_LineModel = new LineModelClass();
                return;
            }
            else
            {
                return;
            }
        }

        //完成绘制button
        private void savedraw_click(object sender, RoutedEventArgs e)
        {
            // LineSave();
            isKaishu = false;
            kaishiBt.Background = Brushes.LightGray;
            this.menuTabctrl.SelectedItem = this.menuTabctrl.Items[1];//跳转第2页
        }
        #endregion

        #region 清空函数
        /// <summary>
        /// 清空当前显示部分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void qingkong(object sender, RoutedEventArgs e)
        {
            comboList.Clear();
            zzComboBox.ItemsSource = null;
            zzCombomess();

            ClearInfluenceLine_FL();//清空，初始化
            BottomSectionClear();
            UserSectionClear();

            comboList2.Clear();
            zzComboBox2.ItemsSource = null;
            zzCombomess2();
            ClearInfluenceLine_FLO();
            BottomSectionClear();
            UserSectionClear();

            ClearInfluenceLine_WJ();
            BottomSectionClear();
            UserSectionClear();

            ClearInfluenceLine_JL();
            BottomSectionClear();
            UserSectionClear();

            this.menuTabctrl.SelectedItem = this.menuTabctrl.Items[0];//跳转第1页
            m_LineModelList.Clear();
            pointList.Clear();
            can.Children.Clear();
            zzList.Clear();
            zz_PointList.Clear();
            BottomZZRectList.Clear();
            BottomZZTBList.Clear();

            ResolutionRatio = 0;

            h1 = 0; h2 = 0; h3 = 0; h4 = 0; h5 = 0; h6 = 0; h7 = 0; h8 = 0; h9 = 0; h10 = 0; h11 = 0;
            m_Line_Num = 0;
            zzNum = 0;
            zyd = 0; zydD = 0; nj = 0; nz = 0;
            NowZydList.Clear();
            NowZyd = new zydAClass();
            NowzydNum = 0;

            comboList.Clear();
            zzComboBox.ItemsSource = null;
            JiaoList.Clear();
            comboList2.Clear();
            zzComboBox2.ItemsSource = null;

            ClearInfluenceLine_FL();
            ClearInfluenceLine_FLO();
            ClearInfluenceLine_WJ();
            ClearInfluenceLine_JL();

            BottomSectionClear();
            UserSectionClear();


        }
        #endregion

        #region 支座部分
        //当前支座类型
        private int zz = 0;
        //支座点列表
        private List<Point> zz_PointList = new List<Point>();
        private Point zzPt = new Point();
        //支座类
        private zzClass m_zzClass = new zzClass();
        private List<zzClass> zzList = new List<zzClass>();
        private int zzNum = 0;

        /// <summary>
        /// 鼠标移过位置函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rect_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle m_Rectangle = (Rectangle)sender;
            var m_Tag = m_Rectangle.Tag;
            zz = Convert.ToInt32(m_Tag);
            //根据种类初始化drap函数
            switch (zz)
            {
                case 1:
                    {
                        //固定铰支座
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.gudzzRect);
                        DragDrop.DoDragDrop(this.gudzzRect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "固定铰支座";
                        break;
                    }
                case 2:
                    {
                        //活动铰支座
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.huodzz_Rect);
                        DragDrop.DoDragDrop(this.huodzz_Rect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "活动铰支座";
                        break;
                    }
                case 3:
                    {
                        //左固定端
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.zuogdd_Rect);
                        DragDrop.DoDragDrop(this.zuogdd_Rect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "左固定端";
                        break;
                    }
                case 4:
                    {
                        //右固定端
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.yougdd_Rect);
                        DragDrop.DoDragDrop(this.yougdd_Rect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "右固定端";
                        break;
                    }
                case 5:
                    {
                        //滑动支座X
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.huadzzX_Rect);
                        DragDrop.DoDragDrop(this.huadzzX_Rect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "滑动支座X";
                        break;
                    }
                case 6:
                    {
                        //滑动支座Y
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.huadzzY_Rect);
                        DragDrop.DoDragDrop(this.huadzzY_Rect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "滑动支座Y";
                        break;
                    }
                case 7:
                    {
                        //铰链
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.jiaolian_Rect);
                        DragDrop.DoDragDrop(this.jiaolian_Rect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "铰链";
                        break;
                    }
                case 8:
                    {
                        //滑动支座-X
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.huadzzX2_Rect);
                        DragDrop.DoDragDrop(this.huadzzX2_Rect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "滑动支座-X";
                        break;
                    }
                case 9:
                    {
                        //滑动支座-Y
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.huadzzY2_Rect);
                        DragDrop.DoDragDrop(this.huadzzY2_Rect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "滑动支座-Y";
                        break;
                    }
                case 10:
                    {
                        //活动铰支座X
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.huodzz2_Rect);
                        DragDrop.DoDragDrop(this.huodzz2_Rect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "活动铰支座X";
                        break;
                    }
                case 11:
                    {
                        //活动铰支座-X
                        DataObject gudzz_data = new DataObject(typeof(Rectangle), this.huodzz3_Rect);
                        DragDrop.DoDragDrop(this.huodzz3_Rect, gudzz_data, DragDropEffects.Copy);
                        text_zz.Text = "活动铰支座-X";
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

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
            //铰链
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //滑动支座-X
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //滑动支座-Y
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //活动铰支座X
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            //活动铰支座-X
            if (!e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
        }
        //drop支座及支座相关属性定义
        List<Rectangle> zzRectList = new List<Rectangle>();
        #region 变量
        Rectangle new_gudzz = new Rectangle();
        Rectangle new_huodzz = new Rectangle();
        Rectangle new_zuogdd = new Rectangle();
        Rectangle new_yougdd = new Rectangle();
        Rectangle new_huadzzX = new Rectangle();
        Rectangle new_huadzzY = new Rectangle();
        Rectangle new_jiaolian = new Rectangle();
        Rectangle new_huadzzX2 = new Rectangle();
        Rectangle new_huadzzY2 = new Rectangle();
        Rectangle new_huodzz2 = new Rectangle();
        Rectangle new_huodzz3 = new Rectangle();
        Point p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11;
        TextBlock zzTB1 = new TextBlock();
        TextBlock zzTB2 = new TextBlock();
        TextBlock zzTB3 = new TextBlock();
        TextBlock zzTB4 = new TextBlock();
        TextBlock zzTB5 = new TextBlock();
        TextBlock zzTB6 = new TextBlock();
        TextBlock zzTB7 = new TextBlock();
        TextBlock zzTB8 = new TextBlock();
        TextBlock zzTB9 = new TextBlock();
        TextBlock zzTB10 = new TextBlock();
        TextBlock zzTB11 = new TextBlock();
        #endregion
        private void can_Drop(object sender, DragEventArgs e)
        {
            switch (zz)
            {
                #region 固定支座
                case 1://固定支座
                    {
                        Rectangle gudzz_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                        new_gudzz = new Rectangle();
                        new_gudzz.Height = gudzz_dataobj.RenderSize.Height;
                        new_gudzz.Width = gudzz_dataobj.RenderSize.Width;
                        new_gudzz.Fill = gudzz_dataobj.Fill;
                        new_gudzz.Stroke = gudzz_dataobj.Stroke;
                        new_gudzz.StrokeThickness = gudzz_dataobj.StrokeThickness;
                        new_gudzz.Tag = gudzz_dataobj.Tag;

                        p1 = new Point();
                        zzPt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                        zzPt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                        p1 = zzPt;
                        new_gudzz.SetValue(Canvas.LeftProperty, zzPt.X - 20);
                        new_gudzz.SetValue(Canvas.TopProperty, zzPt.Y);

                        BottomZZRect = new Rectangle();
                        BottomZZRect.Height = new_gudzz.Height;
                        BottomZZRect.Width = new_gudzz.Width;
                        BottomZZRect.Fill = new_gudzz.Fill;
                        BottomZZRect.Stroke = new_gudzz.Stroke;
                        BottomZZRect.StrokeThickness = new_gudzz.StrokeThickness;
                        BottomZZRect.Tag = new_gudzz.Tag;
                        BottomZZRect.SetValue(Canvas.LeftProperty, zzPt.X - 20);
                        BottomZZRect.SetValue(Canvas.TopProperty, zzPt.Y - 400 * ResolutionRatio);

                        ScaleTransform gudzz_scale = new ScaleTransform();
                        gudzz_scale.ScaleY = -1;
                        new_gudzz.RenderTransform = gudzz_scale;

                        //添加信息进支座类
                        m_zzClass.zzZYD = 2;
                        m_zzClass.zzID = zzNum;
                        m_zzClass.zzInfo = new_gudzz;
                        m_zzClass.zzTag = new_gudzz.Tag;
                        m_zzClass.zzName = "固定铰支座" + (zzNum + 1);
                        m_zzClass.zzPoint = zzPt;
                        for (int i1 = 0; i1 < m_LineModelList.Count; i1++)
                        {
                            if (m_zzClass.zzPoint.X == m_LineModelList[i1].Line_EndPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i1].Line_EndPoint.Y)
                            {
                                m_zzClass.zzGan = i1;
                                can.Children.Add(new_gudzz);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);
                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h1 = h1 + 1;//固定铰支座数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB1 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB1.Text = Convert.ToString(zzNum);
                                zzTB1.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB1.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB1.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB1);
                                break;
                            }
                            else if (m_LineModelList.Count > 1 && i1 + 1 < m_LineModelList.Count
                                && m_zzClass.zzPoint.X == m_LineModelList[i1].Line_EndPoint.X
                                && m_zzClass.zzPoint.X == m_LineModelList[i1 + 1].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i1].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i1;
                                can.Children.Add(new_gudzz);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h1 = h1 + 1;//固定铰支座数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB1 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB1.Text = Convert.ToString(zzNum);
                                zzTB1.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB1.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB1.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB1);
                                break;
                            }
                            else if (m_zzClass.zzPoint.X == m_LineModelList[i1].Line_BeginPoint.X
                                 && m_zzClass.zzPoint.Y == m_LineModelList[i1].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i1;
                                can.Children.Add(new_gudzz);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h1 = h1 + 1;//固定铰支座数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB1 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB1.Text = Convert.ToString(zzNum);
                                zzTB1.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB1.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB1.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB1);
                                break;
                            }
                            else if (m_LineModelList[i1].Line_BeginPoint.X < m_zzClass.zzPoint.X
                                && m_zzClass.zzPoint.X < m_LineModelList[i1].Line_EndPoint.X
                                && m_LineModelList[i1].Line_BeginPoint.Y == m_zzClass.zzPoint.Y)
                            {
                                m_zzClass.zzGan = i1;
                                can.Children.Add(new_gudzz);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h1 = h1 + 1;//固定铰支座数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB1 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB1.Text = Convert.ToString(zzNum);
                                zzTB1.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB1.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB1.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB1);
                                break;
                            }
                        }
                        break;
                    }
                #endregion

                #region 活动铰支座
                case 2: //活动铰支座
                    {
                        Rectangle huodzz_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                        new_huodzz = new Rectangle();
                        new_huodzz.Height = huodzz_dataobj.RenderSize.Height;
                        new_huodzz.Width = huodzz_dataobj.RenderSize.Width;
                        new_huodzz.Fill = huodzz_dataobj.Fill;
                        new_huodzz.Stroke = huodzz_dataobj.Stroke;
                        new_huodzz.StrokeThickness = huodzz_dataobj.StrokeThickness;
                        new_huodzz.Tag = huodzz_dataobj.Tag;

                        p2 = new Point();
                        zzPt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                        zzPt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                        p2 = zzPt;
                        new_huodzz.SetValue(Canvas.LeftProperty, zzPt.X - 20);
                        new_huodzz.SetValue(Canvas.TopProperty, zzPt.Y);

                        BottomZZRect = new Rectangle();
                        BottomZZRect.Height = new_huodzz.Height;
                        BottomZZRect.Width = new_huodzz.Width;
                        BottomZZRect.Fill = new_huodzz.Fill;
                        BottomZZRect.Stroke = new_huodzz.Stroke;
                        BottomZZRect.StrokeThickness = new_huodzz.StrokeThickness;
                        BottomZZRect.Tag = new_huodzz.Tag;
                        BottomZZRect.SetValue(Canvas.LeftProperty, zzPt.X - 20);
                        BottomZZRect.SetValue(Canvas.TopProperty, zzPt.Y - 400 * ResolutionRatio);

                        ScaleTransform huodzz_scale = new ScaleTransform();
                        huodzz_scale.ScaleY = -1;
                        new_huodzz.RenderTransform = huodzz_scale;

                        //添加信息进支座类
                        m_zzClass.zzZYD = 1;
                        m_zzClass.zzID = zzNum;
                        m_zzClass.zzInfo = new_huodzz;
                        m_zzClass.zzTag = new_huodzz.Tag;
                        m_zzClass.zzName = "活动铰支座" + (zzNum + 1);
                        m_zzClass.zzPoint = zzPt;
                        for (int i2 = 0; i2 < m_LineModelList.Count; i2++)
                        {
                            if (m_zzClass.zzPoint.X == m_LineModelList[i2].Line_EndPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i2].Line_EndPoint.Y)
                            {
                                m_zzClass.zzGan = i2;
                                can.Children.Add(new_huodzz);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h2 = h2 + 1;//活动铰支座数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB2 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB2.Text = Convert.ToString(zzNum);
                                zzTB2.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB2.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB2.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB2);

                                break;
                            }
                            else if (m_LineModelList.Count > 1 && i2 + 1 < m_LineModelList.Count
                                && m_zzClass.zzPoint.X == m_LineModelList[i2].Line_EndPoint.X
                                && m_zzClass.zzPoint.X == m_LineModelList[i2 + 1].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i2].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i2;
                                can.Children.Add(new_huodzz);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h2 = h2 + 1;//活动铰支座数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB2 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB2.Text = Convert.ToString(zzNum);
                                zzTB2.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB2.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB2.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB2);
                                break;
                            }
                            else if (m_zzClass.zzPoint.X == m_LineModelList[i2].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i2].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i2;
                                can.Children.Add(new_huodzz);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h2 = h2 + 1;//活动铰支座数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB2 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB2.Text = Convert.ToString(zzNum);
                                zzTB2.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB2.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB2.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB2);
                                break;
                            }
                            else if (m_LineModelList[i2].Line_BeginPoint.X < m_zzClass.zzPoint.X
                                && m_zzClass.zzPoint.X < m_LineModelList[i2].Line_EndPoint.X
                                && m_LineModelList[i2].Line_BeginPoint.Y == m_zzClass.zzPoint.Y)
                            {
                                m_zzClass.zzGan = i2;
                                can.Children.Add(new_huodzz);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h2 = h2 + 1;//活动铰支座数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB2 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB2.Text = Convert.ToString(zzNum);
                                zzTB2.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB2.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB2.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB2);
                                break;
                            }
                        }
                        break;
                    }
                #endregion

                #region 左固定端
                case 3://左固定端
                    {
                        Rectangle zuogdd_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                        new_zuogdd = new Rectangle();
                        new_zuogdd.Height = zuogdd_dataobj.RenderSize.Height;
                        new_zuogdd.Width = zuogdd_dataobj.RenderSize.Width;
                        new_zuogdd.Fill = zuogdd_dataobj.Fill;
                        new_zuogdd.Stroke = zuogdd_dataobj.Stroke;
                        new_zuogdd.StrokeThickness = zuogdd_dataobj.StrokeThickness;
                        new_zuogdd.Tag = zuogdd_dataobj.Tag;

                        p3 = new Point();
                        zzPt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                        zzPt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                        p3 = zzPt;
                        new_zuogdd.SetValue(Canvas.LeftProperty, zzPt.X - 40);
                        new_zuogdd.SetValue(Canvas.TopProperty, zzPt.Y + 20);

                        BottomZZRect = new Rectangle();
                        BottomZZRect.Height = new_zuogdd.Height;
                        BottomZZRect.Width = new_zuogdd.Width;
                        BottomZZRect.Fill = new_zuogdd.Fill;
                        BottomZZRect.Stroke = new_zuogdd.Stroke;
                        BottomZZRect.StrokeThickness = new_zuogdd.StrokeThickness;
                        BottomZZRect.Tag = new_zuogdd.Tag;
                        BottomZZRect.SetValue(Canvas.LeftProperty, zzPt.X - 40);
                        BottomZZRect.SetValue(Canvas.TopProperty, zzPt.Y - 380 * ResolutionRatio);

                        ScaleTransform zuogdd_scale = new ScaleTransform();
                        zuogdd_scale.ScaleY = -1;
                        new_zuogdd.RenderTransform = zuogdd_scale;

                        //添加信息进支座类
                        m_zzClass.zzZYD = 3;
                        m_zzClass.zzID = zzNum;
                        m_zzClass.zzInfo = zuogdd_dataobj;
                        m_zzClass.zzTag = new_zuogdd.Tag;
                        m_zzClass.zzName = "左固定端" + (zzNum + 1);
                        m_zzClass.zzPoint = zzPt;
                        for (int i3 = 0; i3 < m_LineModelList.Count; i3++)
                        {
                            if (m_zzClass.zzPoint.X == m_LineModelList[i3].Line_EndPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i3].Line_EndPoint.Y)
                            {
                                m_zzClass.zzGan = i3;
                                can.Children.Add(new_zuogdd);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h3 = h3 + 1;//左固定端数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB3 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB3.Text = Convert.ToString(zzNum);
                                zzTB3.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB3.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB3.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB3);
                                break;
                            }
                            else if (m_LineModelList.Count > 1 && i3 + 1 < m_LineModelList.Count
                                && m_zzClass.zzPoint.X == m_LineModelList[i3].Line_EndPoint.X
                                && m_zzClass.zzPoint.X == m_LineModelList[i3 + 1].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i3].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i3;
                                can.Children.Add(new_zuogdd);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h3 = h3 + 1;//左固定端数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB3 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB3.Text = Convert.ToString(zzNum);
                                zzTB3.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB3.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB3.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB3);
                                break;
                            }
                            else if (m_zzClass.zzPoint.X == m_LineModelList[i3].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i3].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i3;
                                can.Children.Add(new_zuogdd);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h3 = h3 + 1;//左固定端数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB3 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB3.Text = Convert.ToString(zzNum);
                                zzTB3.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB3.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB3.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB3);
                                break;
                            }
                            else if (m_LineModelList[i3].Line_BeginPoint.X < m_zzClass.zzPoint.X
                                && m_zzClass.zzPoint.X < m_LineModelList[i3].Line_EndPoint.X
                                && m_LineModelList[i3].Line_BeginPoint.Y == m_zzClass.zzPoint.Y)
                            {
                                m_zzClass.zzGan = i3;
                                can.Children.Add(new_zuogdd);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h3 = h3 + 1;//左固定端数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB3 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB3.Text = Convert.ToString(zzNum);
                                zzTB3.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB3.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB3.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB3);
                                break;
                            }
                        }
                        break;
                    }
                #endregion

                #region 右固定端
                case 4://右固定端
                    {
                        Rectangle yougdd_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                        new_yougdd = new Rectangle();
                        new_yougdd.Height = yougdd_dataobj.RenderSize.Height;
                        new_yougdd.Width = yougdd_dataobj.RenderSize.Width;
                        new_yougdd.Fill = yougdd_dataobj.Fill;
                        new_yougdd.Stroke = yougdd_dataobj.Stroke;
                        new_yougdd.StrokeThickness = yougdd_dataobj.StrokeThickness;

                        p4 = new Point();
                        zzPt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                        zzPt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                        p4 = zzPt;
                        new_yougdd.SetValue(Canvas.LeftProperty, zzPt.X);
                        new_yougdd.SetValue(Canvas.TopProperty, zzPt.Y + 20);
                        new_yougdd.Tag = yougdd_dataobj.Tag;

                        BottomZZRect = new Rectangle();
                        BottomZZRect.Height = new_yougdd.Height;
                        BottomZZRect.Width = new_yougdd.Width;
                        BottomZZRect.Fill = new_yougdd.Fill;
                        BottomZZRect.Stroke = new_yougdd.Stroke;
                        BottomZZRect.StrokeThickness = new_yougdd.StrokeThickness;
                        BottomZZRect.Tag = new_yougdd.Tag;
                        BottomZZRect.SetValue(Canvas.LeftProperty, zzPt.X);
                        BottomZZRect.SetValue(Canvas.TopProperty, zzPt.Y - 380 * ResolutionRatio);

                        ScaleTransform yougdd_scale = new ScaleTransform();
                        yougdd_scale.ScaleY = -1;
                        new_yougdd.RenderTransform = yougdd_scale;

                        //添加信息进支座类
                        m_zzClass.zzZYD = 3;
                        m_zzClass.zzID = zzNum;
                        m_zzClass.zzInfo = yougdd_dataobj;
                        m_zzClass.zzTag = new_yougdd.Tag;
                        m_zzClass.zzName = "右固定端" + (zzNum + 1);
                        m_zzClass.zzPoint = zzPt;
                        for (int i4 = 0; i4 < m_LineModelList.Count; i4++)
                        {
                            if (m_zzClass.zzPoint.X == m_LineModelList[i4].Line_EndPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i4].Line_EndPoint.Y)
                            {
                                m_zzClass.zzGan = i4;
                                can.Children.Add(new_yougdd);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h4 = h4 + 1;//右固定端数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB4 = new TextBlock();
                                var zzTBX = zzPt.X - 10;
                                var zzTBY = zzPt.Y + 15;
                                zzTB4.Text = Convert.ToString(zzNum);
                                zzTB4.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB4.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X - 10, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB4.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB4);
                                break;
                            }
                            else if (m_LineModelList.Count > 1 && i4 + 1 < m_LineModelList.Count
                                && m_zzClass.zzPoint.X == m_LineModelList[i4].Line_EndPoint.X
                                && m_zzClass.zzPoint.X == m_LineModelList[i4 + 1].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i4].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i4;
                                can.Children.Add(new_yougdd);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h4 = h4 + 1;//右固定端数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB4 = new TextBlock();
                                var zzTBX = zzPt.X - 10;
                                var zzTBY = zzPt.Y + 15;
                                zzTB4.Text = Convert.ToString(zzNum);
                                zzTB4.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB4.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X - 10, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB4.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB4);
                                break;
                            }
                            else if (m_zzClass.zzPoint.X == m_LineModelList[i4].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i4].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i4;
                                can.Children.Add(new_yougdd);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h4 = h4 + 1;//右固定端数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB4 = new TextBlock();
                                var zzTBX = zzPt.X - 10;
                                var zzTBY = zzPt.Y + 15;
                                zzTB4.Text = Convert.ToString(zzNum);
                                zzTB4.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB4.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X - 10, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB4.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB4);
                                break;
                            }
                            else if (m_LineModelList[i4].Line_BeginPoint.X < m_zzClass.zzPoint.X
                                && m_zzClass.zzPoint.X < m_LineModelList[i4].Line_EndPoint.X
                                && m_LineModelList[i4].Line_BeginPoint.Y == m_zzClass.zzPoint.Y)
                            {
                                m_zzClass.zzGan = i4;
                                can.Children.Add(new_yougdd);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h4 = h4 + 1;//右固定端数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB4 = new TextBlock();
                                var zzTBX = zzPt.X - 10;
                                var zzTBY = zzPt.Y + 15;
                                zzTB4.Text = Convert.ToString(zzNum);
                                zzTB4.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB4.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X - 10, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB4.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB4);
                                break;
                            }
                        }
                        break;
                    }
                #endregion

                #region 滑动支座X
                case 5://滑动支座X
                    {
                        Rectangle huadzzX_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                        new_huadzzX = new Rectangle();
                        new_huadzzX.Height = huadzzX_dataobj.RenderSize.Height;
                        new_huadzzX.Width = huadzzX_dataobj.RenderSize.Width;
                        new_huadzzX.Fill = huadzzX_dataobj.Fill;
                        new_huadzzX.Stroke = huadzzX_dataobj.Stroke;
                        new_huadzzX.StrokeThickness = huadzzX_dataobj.StrokeThickness;
                        new_huadzzX.Tag = huadzzX_dataobj.Tag;

                        p5 = new Point();
                        zzPt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                        zzPt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                        p5 = zzPt;
                        new_huadzzX.SetValue(Canvas.LeftProperty, zzPt.X - 40);
                        new_huadzzX.SetValue(Canvas.TopProperty, zzPt.Y + 20);

                        BottomZZRect = new Rectangle();
                        BottomZZRect.Height = new_huadzzX.Height;
                        BottomZZRect.Width = new_huadzzX.Width;
                        BottomZZRect.Fill = new_huadzzX.Fill;
                        BottomZZRect.Stroke = new_huadzzX.Stroke;
                        BottomZZRect.StrokeThickness = new_huadzzX.StrokeThickness;
                        BottomZZRect.Tag = new_huadzzX.Tag;
                        BottomZZRect.SetValue(Canvas.LeftProperty, zzPt.X - 40);
                        BottomZZRect.SetValue(Canvas.TopProperty, zzPt.Y - 380 * ResolutionRatio);

                        ScaleTransform huadzzX_scale = new ScaleTransform();
                        huadzzX_scale.ScaleY = -1;
                        new_huadzzX.RenderTransform = huadzzX_scale;

                        //添加信息进支座类
                        m_zzClass.zzZYD = 2;
                        m_zzClass.zzID = zzNum;
                        m_zzClass.zzInfo = huadzzX_dataobj;
                        m_zzClass.zzTag = new_huadzzX.Tag;
                        m_zzClass.zzName = "定向支座X" + (zzNum + 1);
                        m_zzClass.zzPoint = zzPt;
                        for (int i5 = 0; i5 < m_LineModelList.Count; i5++)
                        {
                            if (m_zzClass.zzPoint.X == m_LineModelList[i5].Line_EndPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i5].Line_EndPoint.Y)
                            {
                                m_zzClass.zzGan = i5;
                                can.Children.Add(new_huadzzX);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h5 = h5 + 1;//滑动支座X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB5 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB5.Text = Convert.ToString(zzNum);
                                zzTB5.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB5.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB5.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB5);
                                break;
                            }
                            else if (m_LineModelList.Count > 1 && i5 + 1 < m_LineModelList.Count
                                && m_zzClass.zzPoint.X == m_LineModelList[i5].Line_EndPoint.X
                                && m_zzClass.zzPoint.X == m_LineModelList[i5 + 1].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i5].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i5;
                                can.Children.Add(new_huadzzX);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h5 = h5 + 1;//滑动支座X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB5 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB5.Text = Convert.ToString(zzNum);
                                zzTB5.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB5.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB5.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB5);
                                break;
                            }
                            else if (m_zzClass.zzPoint.X == m_LineModelList[i5].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i5].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i5;
                                can.Children.Add(new_huadzzX);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h5 = h5 + 1;//滑动支座X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB5 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB5.Text = Convert.ToString(zzNum);
                                zzTB5.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB5.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB5.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB5);
                                break;
                            }
                            else if (m_LineModelList[i5].Line_BeginPoint.X < m_zzClass.zzPoint.X
                                && m_zzClass.zzPoint.X < m_LineModelList[i5].Line_EndPoint.X
                                && m_LineModelList[i5].Line_BeginPoint.Y == m_zzClass.zzPoint.Y)
                            {
                                m_zzClass.zzGan = i5;
                                can.Children.Add(new_huadzzX);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h5 = h5 + 1;//滑动支座X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB5 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB5.Text = Convert.ToString(zzNum);
                                zzTB5.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB5.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB5.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB5);
                                break;
                            }
                        }
                        break;
                    }
                #endregion

                #region 滑动支座Y
                case 6://滑动支座Y
                    {
                        Rectangle huadzzY_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                        new_huadzzY = new Rectangle();
                        new_huadzzY.Height = huadzzY_dataobj.RenderSize.Height;
                        new_huadzzY.Width = huadzzY_dataobj.RenderSize.Width;
                        new_huadzzY.Fill = huadzzY_dataobj.Fill;
                        new_huadzzY.Stroke = huadzzY_dataobj.Stroke;
                        new_huadzzY.StrokeThickness = huadzzY_dataobj.StrokeThickness;
                        new_huadzzY.Tag = huadzzY_dataobj.Tag;

                        p6 = new Point();
                        zzPt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                        zzPt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                        p6 = zzPt;
                        new_huadzzY.SetValue(Canvas.LeftProperty, zzPt.X - 20);
                        new_huadzzY.SetValue(Canvas.TopProperty, zzPt.Y);

                        BottomZZRect = new Rectangle();
                        BottomZZRect.Height = new_huadzzY.Height;
                        BottomZZRect.Width = new_huadzzY.Width;
                        BottomZZRect.Fill = new_huadzzY.Fill;
                        BottomZZRect.Stroke = new_huadzzY.Stroke;
                        BottomZZRect.StrokeThickness = new_huadzzY.StrokeThickness;
                        BottomZZRect.Tag = new_huadzzY.Tag;
                        BottomZZRect.SetValue(Canvas.LeftProperty, zzPt.X - 20);
                        BottomZZRect.SetValue(Canvas.TopProperty, zzPt.Y - 400 * ResolutionRatio);

                        ScaleTransform huadzzY_scale = new ScaleTransform();
                        huadzzY_scale.ScaleY = -1;
                        new_huadzzY.RenderTransform = huadzzY_scale;

                        //添加信息进支座类
                        m_zzClass.zzZYD = 2;
                        m_zzClass.zzID = zzNum;
                        m_zzClass.zzInfo = huadzzY_dataobj;
                        m_zzClass.zzTag = new_huadzzY.Tag;
                        m_zzClass.zzName = "定向支座Y" + (zzNum + 1);
                        m_zzClass.zzPoint = zzPt;
                        for (int i6 = 0; i6 < m_LineModelList.Count; i6++)
                        {
                            if (m_zzClass.zzPoint.X == m_LineModelList[i6].Line_EndPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i6].Line_EndPoint.Y)
                            {
                                m_zzClass.zzGan = i6;
                                can.Children.Add(new_huadzzY);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h6 = h6 + 1;//滑动支座Y数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB6 = new TextBlock();
                                var zzTBX = zzPt.X + 5;
                                var zzTBY = zzPt.Y + 15;
                                zzTB6.Text = Convert.ToString(zzNum);
                                zzTB6.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB6.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X + 5, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB6.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB6);
                                break;
                            }
                            else if (m_LineModelList.Count > 1 && i6 + 1 < m_LineModelList.Count
                                && m_zzClass.zzPoint.X == m_LineModelList[i6].Line_EndPoint.X
                                && m_zzClass.zzPoint.X == m_LineModelList[i6 + 1].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i6].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i6;
                                can.Children.Add(new_huadzzY);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h6 = h6 + 1;//滑动支座Y数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB6 = new TextBlock();
                                var zzTBX = zzPt.X + 5;
                                var zzTBY = zzPt.Y + 15;
                                zzTB6.Text = Convert.ToString(zzNum);
                                zzTB6.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB6.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X + 5, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB6.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB6);
                                break;
                            }
                            else if (m_zzClass.zzPoint.X == m_LineModelList[i6].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i6].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i6;
                                can.Children.Add(new_huadzzY);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h6 = h6 + 1;//滑动支座Y数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB6 = new TextBlock();
                                var zzTBX = zzPt.X + 5;
                                var zzTBY = zzPt.Y + 15;
                                zzTB6.Text = Convert.ToString(zzNum);
                                zzTB6.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB6.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X + 5, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB6.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB6);
                                break;
                            }
                            else if (m_LineModelList[i6].Line_BeginPoint.X < m_zzClass.zzPoint.X
                                && m_zzClass.zzPoint.X < m_LineModelList[i6].Line_EndPoint.X
                                && m_LineModelList[i6].Line_BeginPoint.Y == m_zzClass.zzPoint.Y)
                            {
                                m_zzClass.zzGan = i6;
                                can.Children.Add(new_huadzzY);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h6 = h6 + 1;//滑动支座Y数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB6 = new TextBlock();
                                var zzTBX = zzPt.X + 5;
                                var zzTBY = zzPt.Y + 15;
                                zzTB6.Text = Convert.ToString(zzNum);
                                zzTB6.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB6.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X + 5, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB6.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB6);
                                break;
                            }
                        }
                        break;
                    }
                #endregion

                #region 铰链
                case 7://铰链
                    {
                        Rectangle jiaolian_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                        new_jiaolian = new Rectangle();
                        new_jiaolian.Height = jiaolian_dataobj.RenderSize.Height;
                        new_jiaolian.Width = jiaolian_dataobj.RenderSize.Width;
                        new_jiaolian.Fill = jiaolian_dataobj.Fill;
                        new_jiaolian.Stroke = jiaolian_dataobj.Stroke;
                        new_jiaolian.StrokeThickness = jiaolian_dataobj.StrokeThickness;
                        new_jiaolian.Tag = jiaolian_dataobj.Tag;

                        p7 = new Point();
                        zzPt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                        zzPt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                        p7 = zzPt;
                        new_jiaolian.SetValue(Canvas.LeftProperty, zzPt.X - 20);
                        new_jiaolian.SetValue(Canvas.TopProperty, zzPt.Y + 20);

                        BottomZZRect = new Rectangle();
                        BottomZZRect.Height = new_jiaolian.Height;
                        BottomZZRect.Width = new_jiaolian.Width;
                        BottomZZRect.Fill = new_jiaolian.Fill;
                        BottomZZRect.Stroke = new_jiaolian.Stroke;
                        BottomZZRect.StrokeThickness = new_jiaolian.StrokeThickness;
                        BottomZZRect.Tag = new_jiaolian.Tag;
                        BottomZZRect.SetValue(Canvas.LeftProperty, zzPt.X - 20);
                        BottomZZRect.SetValue(Canvas.TopProperty, zzPt.Y - 380 * ResolutionRatio);

                        ScaleTransform jiaolian_scale = new ScaleTransform();
                        jiaolian_scale.ScaleY = -1;
                        new_jiaolian.RenderTransform = jiaolian_scale;

                        //添加信息进支座类
                        m_zzClass.zzZYD = 2;
                        m_zzClass.zzID = zzNum;
                        m_zzClass.zzInfo = jiaolian_dataobj;
                        m_zzClass.zzTag = new_jiaolian.Tag;
                        m_zzClass.zzName = "铰链" + (zzNum + 1);
                        m_zzClass.zzPoint = zzPt;
                        for (int i7 = 0; i7 < m_LineModelList.Count; i7++)
                        {
                            if (m_zzClass.zzPoint.X == m_LineModelList[i7].Line_EndPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i7].Line_EndPoint.Y)
                            {
                                m_zzClass.zzGan = i7;
                                can.Children.Add(new_jiaolian);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                JiaoList.Add(m_zzClass);//加入铰链List
                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h7 = h7 + 1;//铰链数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB7 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB7.Text = Convert.ToString(zzNum);
                                zzTB7.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB7.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB7.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB7);
                                break;
                            }
                            else if (m_LineModelList.Count > 1 && i7 + 1 < m_LineModelList.Count
                                && m_zzClass.zzPoint.X == m_LineModelList[i7].Line_EndPoint.X
                                && m_zzClass.zzPoint.X == m_LineModelList[i7 + 1].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i7].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i7;
                                can.Children.Add(new_jiaolian);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                JiaoList.Add(m_zzClass);//加入铰链List
                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h7 = h7 + 1;//铰链数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB7 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB7.Text = Convert.ToString(zzNum);
                                zzTB7.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB7.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB7.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB7);
                                break;
                            }
                            else if (m_zzClass.zzPoint.X == m_LineModelList[i7].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i7].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i7;
                                can.Children.Add(new_jiaolian);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                JiaoList.Add(m_zzClass);//加入铰链List
                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h7 = h7 + 1;//铰链数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB7 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB7.Text = Convert.ToString(zzNum);
                                zzTB7.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB7.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB7.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB7);
                                break;
                            }
                            else if (m_LineModelList[i7].Line_BeginPoint.X < m_zzClass.zzPoint.X
                                && m_zzClass.zzPoint.X < m_LineModelList[i7].Line_EndPoint.X
                                && m_LineModelList[i7].Line_BeginPoint.Y == m_zzClass.zzPoint.Y)
                            {
                                m_zzClass.zzGan = i7;
                                can.Children.Add(new_jiaolian);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                JiaoList.Add(m_zzClass);//加入铰链List
                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h7 = h7 + 1;//铰链数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB7 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB7.Text = Convert.ToString(zzNum);
                                zzTB7.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB7.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB7.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB7);
                                break;
                            }
                        }
                        break;
                    }
                #endregion

                #region 滑动支座-X
                case 8://滑动支座-X
                    {
                        Rectangle huadzzX2_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                        new_huadzzX2 = new Rectangle();
                        new_huadzzX2.Height = huadzzX2_dataobj.RenderSize.Height;
                        new_huadzzX2.Width = huadzzX2_dataobj.RenderSize.Width;
                        new_huadzzX2.Fill = huadzzX2_dataobj.Fill;
                        new_huadzzX2.Stroke = huadzzX2_dataobj.Stroke;
                        new_huadzzX2.StrokeThickness = huadzzX2_dataobj.StrokeThickness;
                        new_huadzzX2.Tag = huadzzX2_dataobj.Tag;

                        p7 = new Point();
                        zzPt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                        zzPt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                        p7 = zzPt;
                        new_huadzzX2.SetValue(Canvas.LeftProperty, zzPt.X);
                        new_huadzzX2.SetValue(Canvas.TopProperty, zzPt.Y + 20);

                        BottomZZRect = new Rectangle();
                        BottomZZRect.Height = new_huadzzX2.Height;
                        BottomZZRect.Width = new_huadzzX2.Width;
                        BottomZZRect.Fill = new_huadzzX2.Fill;
                        BottomZZRect.Stroke = new_huadzzX2.Stroke;
                        BottomZZRect.StrokeThickness = new_huadzzX2.StrokeThickness;
                        BottomZZRect.Tag = new_huadzzX2.Tag;
                        BottomZZRect.SetValue(Canvas.LeftProperty, zzPt.X);
                        BottomZZRect.SetValue(Canvas.TopProperty, zzPt.Y - 380 * ResolutionRatio);

                        ScaleTransform huadzzX2_scale = new ScaleTransform();
                        huadzzX2_scale.ScaleY = -1;
                        new_huadzzX2.RenderTransform = huadzzX2_scale;

                        //添加信息进支座类
                        m_zzClass.zzZYD = 2;
                        m_zzClass.zzID = zzNum;
                        m_zzClass.zzInfo = huadzzX2_dataobj;
                        m_zzClass.zzTag = new_huadzzX2.Tag;
                        m_zzClass.zzName = "定向支座-X" + (zzNum + 1);
                        m_zzClass.zzPoint = zzPt;
                        for (int i8 = 0; i8 < m_LineModelList.Count; i8++)
                        {
                            if (m_zzClass.zzPoint.X == m_LineModelList[i8].Line_EndPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i8].Line_EndPoint.Y)
                            {
                                m_zzClass.zzGan = i8;
                                can.Children.Add(new_huadzzX2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h8 = h8 + 1;//滑动支座-X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB8 = new TextBlock();
                                var zzTBX = zzPt.X - 10;
                                var zzTBY = zzPt.Y + 15;
                                zzTB8.Text = Convert.ToString(zzNum);
                                zzTB8.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB8.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X - 10, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB8.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB8);
                                break;
                            }
                            else if (m_LineModelList.Count > 1 && i8 + 1 < m_LineModelList.Count
                                && m_zzClass.zzPoint.X == m_LineModelList[i8].Line_EndPoint.X
                                && m_zzClass.zzPoint.X == m_LineModelList[i8 + 1].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i8].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i8;
                                can.Children.Add(new_huadzzX2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h8 = h8 + 1;//滑动支座-X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB8 = new TextBlock();
                                var zzTBX = zzPt.X - 10;
                                var zzTBY = zzPt.Y + 15;
                                zzTB8.Text = Convert.ToString(zzNum);
                                zzTB8.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB8.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X - 10, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB8.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB8);
                                break;
                            }
                            else if (m_zzClass.zzPoint.X == m_LineModelList[i8].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i8].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i8;
                                can.Children.Add(new_huadzzX2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h8 = h8 + 1;//滑动支座-X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB8 = new TextBlock();
                                var zzTBX = zzPt.X - 10;
                                var zzTBY = zzPt.Y + 15;
                                zzTB8.Text = Convert.ToString(zzNum);
                                zzTB8.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB8.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X - 10, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB8.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB8);
                                break;
                            }
                            else if (m_LineModelList[i8].Line_BeginPoint.X < m_zzClass.zzPoint.X
                                && m_zzClass.zzPoint.X < m_LineModelList[i8].Line_EndPoint.X
                                && m_LineModelList[i8].Line_BeginPoint.Y == m_zzClass.zzPoint.Y)
                            {
                                m_zzClass.zzGan = i8;
                                can.Children.Add(new_huadzzX2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h8 = h8 + 1;//滑动支座-X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB8 = new TextBlock();
                                var zzTBX = zzPt.X - 10;
                                var zzTBY = zzPt.Y + 15;
                                zzTB8.Text = Convert.ToString(zzNum);
                                zzTB8.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB8.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X - 10, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB8.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB8);
                                break;
                            }
                        }
                        break;
                    }
                #endregion

                #region 滑动支座-Y
                case 9://滑动支座-Y
                    {
                        Rectangle huadzzY2_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                        new_huadzzY2 = new Rectangle();
                        new_huadzzY2.Height = huadzzY2_dataobj.RenderSize.Height;
                        new_huadzzY2.Width = huadzzY2_dataobj.RenderSize.Width;
                        new_huadzzY2.Fill = huadzzY2_dataobj.Fill;
                        new_huadzzY2.Stroke = huadzzY2_dataobj.Stroke;
                        new_huadzzY2.StrokeThickness = huadzzY2_dataobj.StrokeThickness;
                        new_huadzzY2.Tag = huadzzY2_dataobj.Tag;

                        p9 = new Point();
                        zzPt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                        zzPt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                        p9 = zzPt;
                        new_huadzzY2.SetValue(Canvas.LeftProperty, zzPt.X - 20);
                        new_huadzzY2.SetValue(Canvas.TopProperty, zzPt.Y + 40);

                        BottomZZRect = new Rectangle();
                        BottomZZRect.Height = new_huadzzY2.Height;
                        BottomZZRect.Width = new_huadzzY2.Width;
                        BottomZZRect.Fill = new_huadzzY2.Fill;
                        BottomZZRect.Stroke = new_huadzzY2.Stroke;
                        BottomZZRect.StrokeThickness = new_huadzzY2.StrokeThickness;
                        BottomZZRect.Tag = new_huadzzY2.Tag;
                        BottomZZRect.SetValue(Canvas.LeftProperty, zzPt.X - 20);
                        BottomZZRect.SetValue(Canvas.TopProperty, zzPt.Y - 360 * ResolutionRatio);

                        ScaleTransform huadzzY2_scale = new ScaleTransform();
                        huadzzY2_scale.ScaleY = -1;
                        new_huadzzY2.RenderTransform = huadzzY2_scale;

                        //添加信息进支座类
                        m_zzClass.zzZYD = 2;
                        m_zzClass.zzID = zzNum;
                        m_zzClass.zzInfo = huadzzY2_dataobj;
                        m_zzClass.zzTag = new_huadzzY2.Tag;
                        m_zzClass.zzName = "定向支座-Y" + (zzNum + 1);
                        m_zzClass.zzPoint = zzPt;
                        for (int i9 = 0; i9 < m_LineModelList.Count; i9++)
                        {
                            if (m_zzClass.zzPoint.X == m_LineModelList[i9].Line_EndPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i9].Line_EndPoint.Y)
                            {
                                m_zzClass.zzGan = i9;
                                can.Children.Add(new_huadzzY2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h9 = h9 + 1;//滑动支座-Y数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB9 = new TextBlock();
                                var zzTBX = zzPt.X + 15;
                                var zzTBY = zzPt.Y - 5;
                                zzTB9.Text = Convert.ToString(zzNum);
                                zzTB9.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB9.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X + 15, zzPt.Y - 395 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB9.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB9);
                                break;
                            }
                            else if (m_LineModelList.Count > 1 && i9 + 1 < m_LineModelList.Count
                                && m_zzClass.zzPoint.X == m_LineModelList[i9].Line_EndPoint.X
                                && m_zzClass.zzPoint.X == m_LineModelList[i9 + 1].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i9].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i9;
                                can.Children.Add(new_huadzzY2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h9 = h9 + 1;//滑动支座-Y数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB9 = new TextBlock();
                                var zzTBX = zzPt.X + 15;
                                var zzTBY = zzPt.Y - 5;
                                zzTB9.Text = Convert.ToString(zzNum);
                                zzTB9.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB9.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X + 15, zzPt.Y - 395 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB9.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB9);
                                break;
                            }
                            else if (m_zzClass.zzPoint.X == m_LineModelList[i9].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i9].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i9;
                                can.Children.Add(new_huadzzY2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h9 = h9 + 1;//滑动支座-Y数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB9 = new TextBlock();
                                var zzTBX = zzPt.X + 15;
                                var zzTBY = zzPt.Y - 5;
                                zzTB9.Text = Convert.ToString(zzNum);
                                zzTB9.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB9.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X + 15, zzPt.Y - 395 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB9.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB9);
                                break;
                            }
                            else if (m_LineModelList[i9].Line_BeginPoint.X < m_zzClass.zzPoint.X
                                && m_zzClass.zzPoint.X < m_LineModelList[i9].Line_EndPoint.X
                                && m_LineModelList[i9].Line_BeginPoint.Y == m_zzClass.zzPoint.Y)
                            {
                                m_zzClass.zzGan = i9;
                                can.Children.Add(new_huadzzY2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h9 = h9 + 1;//滑动支座-Y数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB9 = new TextBlock();
                                var zzTBX = zzPt.X + 15;
                                var zzTBY = zzPt.Y - 5;
                                zzTB9.Text = Convert.ToString(zzNum);
                                zzTB9.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB9.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X + 15, zzPt.Y - 395 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB9.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB9);
                                break;
                            }
                        }
                        break;
                    }
                #endregion

                #region 活动铰支座X
                case 10://活动铰支座X
                    {
                        Rectangle huodzz2_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                        new_huodzz2 = new Rectangle();
                        new_huodzz2.Height = huodzz2_dataobj.RenderSize.Height;
                        new_huodzz2.Width = huodzz2_dataobj.RenderSize.Width;
                        new_huodzz2.Fill = huodzz2_dataobj.Fill;
                        new_huodzz2.Stroke = huodzz2_dataobj.Stroke;
                        new_huodzz2.StrokeThickness = huodzz2_dataobj.StrokeThickness;
                        new_huodzz2.Tag = huodzz2_dataobj.Tag;

                        p10 = new Point();
                        zzPt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                        zzPt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                        p10 = zzPt;
                        new_huodzz2.SetValue(Canvas.LeftProperty, zzPt.X - 40);
                        new_huodzz2.SetValue(Canvas.TopProperty, zzPt.Y + 20);

                        BottomZZRect = new Rectangle();
                        BottomZZRect.Height = new_huodzz2.Height;
                        BottomZZRect.Width = new_huodzz2.Width;
                        BottomZZRect.Fill = new_huodzz2.Fill;
                        BottomZZRect.Stroke = new_huodzz2.Stroke;
                        BottomZZRect.StrokeThickness = new_huodzz2.StrokeThickness;
                        BottomZZRect.Tag = new_huodzz2.Tag;
                        BottomZZRect.SetValue(Canvas.LeftProperty, zzPt.X - 40);
                        BottomZZRect.SetValue(Canvas.TopProperty, zzPt.Y - 380 * ResolutionRatio);

                        ScaleTransform huodzz2_scale = new ScaleTransform();
                        huodzz2_scale.ScaleY = -1;
                        new_huodzz2.RenderTransform = huodzz2_scale;

                        //添加信息进支座类
                        m_zzClass.zzZYD = 1;
                        m_zzClass.zzID = zzNum;
                        m_zzClass.zzInfo = huodzz2_dataobj;
                        m_zzClass.zzTag = new_huodzz2.Tag;
                        m_zzClass.zzName = "活动铰支座X" + (zzNum + 1);
                        m_zzClass.zzPoint = zzPt;
                        for (int i10 = 0; i10 < m_LineModelList.Count; i10++)
                        {
                            if (m_zzClass.zzPoint.X == m_LineModelList[i10].Line_EndPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i10].Line_EndPoint.Y)
                            {
                                m_zzClass.zzGan = i10;
                                can.Children.Add(new_huodzz2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h10 = h10 + 1;//活动铰支座X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB10 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB10.Text = Convert.ToString(zzNum);
                                zzTB10.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB10.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB10.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB10);
                                break;
                            }
                            else if (m_LineModelList.Count > 1 && i10 + 1 < m_LineModelList.Count
                                && m_zzClass.zzPoint.X == m_LineModelList[i10].Line_EndPoint.X
                                && m_zzClass.zzPoint.X == m_LineModelList[i10 + 1].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i10].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i10;
                                can.Children.Add(new_huodzz2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h10 = h10 + 1;//活动铰支座X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB10 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB10.Text = Convert.ToString(zzNum);
                                zzTB10.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB10.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB10.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB10);
                                break;
                            }
                            else if (m_zzClass.zzPoint.X == m_LineModelList[i10].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i10].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i10;
                                can.Children.Add(new_huodzz2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h10 = h10 + 1;//活动铰支座X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB10 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB10.Text = Convert.ToString(zzNum);
                                zzTB10.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB10.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB10.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB10);
                                break;
                            }
                            else if (m_LineModelList[i10].Line_BeginPoint.X < m_zzClass.zzPoint.X
                                && m_zzClass.zzPoint.X < m_LineModelList[i10].Line_EndPoint.X
                                && m_LineModelList[i10].Line_BeginPoint.Y == m_zzClass.zzPoint.Y)
                            {
                                m_zzClass.zzGan = i10;
                                can.Children.Add(new_huodzz2);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h10 = h10 + 1;//活动铰支座X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB10 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 15;
                                zzTB10.Text = Convert.ToString(zzNum);
                                zzTB10.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB10.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 385 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB10.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB10);
                                break;
                            }
                        }
                        break;
                    }
                #endregion

                #region 活动铰支座-X
                case 11://活动铰支座-X
                    {
                        Rectangle huodzz3_dataobj = e.Data.GetData(typeof(Rectangle)) as Rectangle;
                        new_huodzz3 = new Rectangle();
                        new_huodzz3.Height = huodzz3_dataobj.RenderSize.Height;
                        new_huodzz3.Width = huodzz3_dataobj.RenderSize.Width;
                        new_huodzz3.Fill = huodzz3_dataobj.Fill;
                        new_huodzz3.Stroke = huodzz3_dataobj.Stroke;
                        new_huodzz3.StrokeThickness = huodzz3_dataobj.StrokeThickness;
                        new_huodzz3.Tag = huodzz3_dataobj.Tag;

                        p11 = new Point();
                        zzPt.X = Math.Round(e.GetPosition(can).X / 20) * 20;
                        zzPt.Y = Math.Round(e.GetPosition(can).Y / 20) * 20;
                        p11 = zzPt;
                        new_huodzz3.SetValue(Canvas.LeftProperty, zzPt.X);
                        new_huodzz3.SetValue(Canvas.TopProperty, zzPt.Y + 20);

                        BottomZZRect = new Rectangle();
                        BottomZZRect.Height = new_huodzz3.Height;
                        BottomZZRect.Width = new_huodzz3.Width;
                        BottomZZRect.Fill = new_huodzz3.Fill;
                        BottomZZRect.Stroke = new_huodzz3.Stroke;
                        BottomZZRect.StrokeThickness = new_huodzz3.StrokeThickness;
                        BottomZZRect.Tag = new_huodzz3.Tag;
                        BottomZZRect.SetValue(Canvas.LeftProperty, zzPt.X);
                        BottomZZRect.SetValue(Canvas.TopProperty, zzPt.Y - 380 * ResolutionRatio);

                        ScaleTransform huodzz3_scale = new ScaleTransform();
                        huodzz3_scale.ScaleY = -1;
                        new_huodzz3.RenderTransform = huodzz3_scale;

                        //添加信息进支座类
                        m_zzClass.zzZYD = 1;
                        m_zzClass.zzID = zzNum;
                        m_zzClass.zzInfo = huodzz3_dataobj;
                        m_zzClass.zzTag = new_huodzz3.Tag;
                        m_zzClass.zzName = "活动铰支座-X" + (zzNum + 1);
                        m_zzClass.zzPoint = zzPt;
                        for (int i11 = 0; i11 < m_LineModelList.Count; i11++)
                        {
                            if (m_zzClass.zzPoint.X == m_LineModelList[i11].Line_EndPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i11].Line_EndPoint.Y)
                            {
                                m_zzClass.zzGan = i11;
                                can.Children.Add(new_huodzz3);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h11 = h11 + 1;//活动铰支座-X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB11 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 18;
                                zzTB11.Text = Convert.ToString(zzNum);
                                zzTB11.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB11.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 382 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB11.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB11);
                                break;
                            }
                            else if (m_LineModelList.Count > 1 && i11 + 1 < m_LineModelList.Count
                                && m_zzClass.zzPoint.X == m_LineModelList[i11].Line_EndPoint.X
                                && m_zzClass.zzPoint.X == m_LineModelList[i11 + 1].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i11].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i11;
                                can.Children.Add(new_huodzz3);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h11 = h11 + 1;//活动铰支座-X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB11 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 18;
                                zzTB11.Text = Convert.ToString(zzNum);
                                zzTB11.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB11.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 382 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB11.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB11);
                                break;
                            }
                            else if (m_zzClass.zzPoint.X == m_LineModelList[i11].Line_BeginPoint.X
                                && m_zzClass.zzPoint.Y == m_LineModelList[i11].Line_BeginPoint.Y)
                            {
                                m_zzClass.zzGan = i11;
                                can.Children.Add(new_huodzz3);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h11 = h11 + 1;//活动铰支座-X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB11 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 18;
                                zzTB11.Text = Convert.ToString(zzNum);
                                zzTB11.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB11.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 382 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB11.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB11);
                                break;
                            }
                            else if (m_LineModelList[i11].Line_BeginPoint.X < m_zzClass.zzPoint.X
                                && m_zzClass.zzPoint.X < m_LineModelList[i11].Line_EndPoint.X
                                && m_LineModelList[i11].Line_BeginPoint.Y == m_zzClass.zzPoint.Y)
                            {
                                m_zzClass.zzGan = i11;
                                can.Children.Add(new_huodzz3);
                                zzList.Add(m_zzClass);
                                BottomZZRectList.Add(BottomZZRect);

                                m_zzClass = new zzClass();
                                zz_PointList.Add(zzPt);
                                zzCombomess();
                                zzCombomess2();

                                h11 = h11 + 1;//活动铰支座-X数
                                zzNum = zzNum + 1;
                                zz = 0;
                                //标支座号
                                zzTB11 = new TextBlock();
                                var zzTBX = zzPt.X;
                                var zzTBY = zzPt.Y + 18;
                                zzTB11.Text = Convert.ToString(zzNum);
                                zzTB11.Margin = new Thickness(zzTBX, zzTBY, 0, 0);

                                BottomZZTB = new TextBlock();
                                BottomZZTB.Text = Convert.ToString(zzTB11.Text);
                                BottomZZTB.Margin = new Thickness(zzPt.X, zzPt.Y - 382 * ResolutionRatio, 0, 0);
                                BottomZZTB.RenderTransform = BottomScale;
                                BottomZZTBList.Add(BottomZZTB);

                                ScaleTransform zzTBScale = new ScaleTransform();
                                zzTBScale.ScaleY = -1;
                                zzTB11.RenderTransform = zzTBScale;
                                can.Children.Add(zzTB11);
                                break;
                            }
                        }
                        break;
                    }
                #endregion

                default:
                    {
                        MessageBox.Show("zz=" + zz);
                        break;
                    }
            }
        }
        #endregion

        #region 自由度计算

        #region 变量（自由度）
        //固定支座数
        private int h1 = 0;
        //活动铰支座数
        private int h2 = 0;
        //左固定端数
        private int h3 = 0;
        //右固定端数
        private int h4 = 0;
        //滑动支座X数
        private int h5 = 0;
        //滑动支座Y数
        private int h6 = 0;
        //铰链数
        private int h7 = 0;
        //滑动支座-X数
        private int h8 = 0;
        //滑动支座-Y数
        private int h9 = 0;
        //活动铰支座X数
        private int h10 = 0;
        //活动铰支座-X数
        private int h11 = 0;
        //自由度
        private int zyd = 0;
        //系数矩阵
        private double[,] zydA;
        //系数矩阵
        private MatrixCls zydM;
        //矩阵行列式值
        private double zydD = 0;
        //调用自由度计算方法
        private int kTag = 0;
        private int nj = 0;
        //目前所在自由度
        private int nz = 0;
        #endregion

        //自由度计算方法
        protected void Calculate_Zyd()
        {
            zyd = 3 * m_Line_Num - (2 * h1 + h2 + 3 * h3 + 3 * h4 + 2 * h5 + 2 * h6 + 2 * h7 + 2 * h8 + 2 * h9 + h10 + h11);
            if (zyd == 0)
            {
                zydM.Detail = zydA;
                zydD = MatrixCls.MatrixDet(zydM);
            }
            if (zyd == 0 && zydD != 0)
            {
                MessageBoxResult result = MessageBox.Show("此为静定结构", "提示");
                if (result == MessageBoxResult.OK)//跳转第4页
                {
                    this.menuTabctrl.SelectedItem = this.menuTabctrl.Items[2];
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("该结构不是静定结构，请重新绘制。", "警告");
                if (result == MessageBoxResult.OK)//清空
                {
                    this.menuTabctrl.SelectedItem = this.menuTabctrl.Items[0];//跳转第1页
                    m_LineModelList.Clear();
                    pointList.Clear();
                    can.Children.Clear();
                    zzList.Clear();
                    zz_PointList.Clear();
                    BottomZZRectList.Clear();
                    BottomZZTBList.Clear();

                    h1 = 0; h2 = 0; h3 = 0; h4 = 0; h5 = 0; h6 = 0; h7 = 0; h8 = 0; h9 = 0; h10 = 0; h11 = 0;
                    m_Line_Num = 0;
                    zzNum = 0;
                    zyd = 0; zydD = 0; nj = 0; nz = 0;
                    NowZydList.Clear();
                    NowZyd = new zydAClass();
                    NowzydNum = 0;

                    comboList.Clear();
                    zzComboBox.ItemsSource = null;
                    JiaoList.Clear();
                    comboList2.Clear();
                    zzComboBox2.ItemsSource = null;

                    ClearInfluenceLine_FL();
                    ClearInfluenceLine_FLO();
                    ClearInfluenceLine_WJ();
                    ClearInfluenceLine_JL();

                    BottomSectionClear();
                    UserSectionClear();
                }
            }
        }

        //生成系数矩阵
        private void bt_zyd_Click(object sender, RoutedEventArgs e)
        {
            Build_zydA();
            zydA = new double[3 * m_Line_Num, 3 * m_Line_Num];
            zydM = new MatrixCls(3 * m_Line_Num, 3 * m_Line_Num);
            for (int izyd = 0; izyd < NowZydList.Count; izyd++)
            {
                zydA[NowZydList[izyd].zydRow, NowZydList[izyd].zydCol] = NowZydList[izyd].inputValue;
            }
            Calculate_Zyd();
        }

        zydAClass NowZyd = new zydAClass();
        List<zydAClass> NowZydList = new List<zydAClass>();
        int NowzydNum = 0;
        private void Build_zydA()
        {
            zzList.Sort((z1, z2) => z1.zzPoint.X.CompareTo(z2.zzPoint.X));
            int lieX = 0;
            int lieY = 0;
            int lieM = 0;
            for (int ix = 0; ix < zzList.Count; ix++)
            {
                for (int jy = 0; jy < m_LineModelList.Count; jy++)
                {
                    if (m_LineModelList[jy].Line_BeginPoint.X <= zzList[ix].zzPoint.X && zzList[ix].zzPoint.X <= m_LineModelList[jy].Line_EndPoint.X)
                    {
                        switch (Convert.ToInt32(zzList[ix].zzTag))
                        {
                            case 1:
                                {
                                    NowZyd.zydName = zzList[ix].zzName + "X";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    if (zzList[ix].zzPoint.X == m_LineModelList[jy].Line_BeginPoint.X)
                                    {
                                        NowZyd.inputValue = 1;
                                    }
                                    else
                                    {
                                        NowZyd.inputValue = -1;
                                    }
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;

                                    NowZyd.zydName = zzList[ix].zzName + "Y";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 1;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();

                                    NowZyd.zydName = zzList[ix].zzName + "M";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 2;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1 * NowZyd.zydLength;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;
                                    break;
                                }
                            case 2:
                                {
                                    NowZyd.zydName = zzList[ix].zzName + "Y";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 1;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();

                                    NowZyd.zydName = zzList[ix].zzName + "M";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 2;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1 * NowZyd.zydLength;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;
                                    break;
                                }
                            case 3:
                                {
                                    NowZyd.zydName = zzList[ix].zzName + "X";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;

                                    NowZyd.zydName = zzList[ix].zzName + "Y";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 1;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;

                                    NowZyd.zydName = zzList[ix].zzName + "M";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 2;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;
                                    break;
                                }
                            case 4:
                                {
                                    NowZyd.zydName = zzList[ix].zzName + "X";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = -1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;

                                    NowZyd.zydName = zzList[ix].zzName + "Y";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 1;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;

                                    NowZyd.zydName = zzList[ix].zzName + "M";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 2;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;
                                    break;
                                }
                            case 5:
                                {
                                    NowZyd.zydName = zzList[ix].zzName + "X";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;

                                    NowZyd.zydName = zzList[ix].zzName + "M";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 2;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;
                                    break;
                                }
                            case 6:
                                {
                                    NowZyd.zydName = zzList[ix].zzName + "Y";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 1;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();

                                    NowZyd.zydName = zzList[ix].zzName + "YM";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 2;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1 * NowZyd.zydLength;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;

                                    NowZyd.zydName = zzList[ix].zzName + "M";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 2;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;
                                    break;
                                }
                            case 7:
                                {
                                    if (zzList[ix].zzPoint.X == m_LineModelList[jy].Line_EndPoint.X)
                                    {
                                        NowZyd.zydName = zzList[ix].zzName + "X";
                                        NowZyd.zydCol = NowzydNum;
                                        lieX = NowZyd.zydCol;
                                        NowZyd.zydRow = 3 * jy;
                                        NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                        NowZyd.zydTag = zzList[ix].zzTag;
                                        NowZyd.inputValue = -1;
                                        NowZydList.Add(NowZyd);
                                        NowZyd = new zydAClass();
                                        NowzydNum++;

                                        NowZyd.zydName = zzList[ix].zzName + "Y";
                                        NowZyd.zydCol = NowzydNum;
                                        lieY = NowZyd.zydCol;
                                        NowZyd.zydRow = 3 * jy + 1;
                                        NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                        NowZyd.zydTag = zzList[ix].zzTag;
                                        NowZyd.inputValue = 1;
                                        NowZydList.Add(NowZyd);
                                        NowZyd = new zydAClass();

                                        NowZyd.zydName = zzList[ix].zzName + "M";
                                        NowZyd.zydCol = NowzydNum;
                                        lieM = NowZyd.zydCol;
                                        NowZyd.zydRow = 3 * jy + 2;
                                        NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                        NowZyd.zydTag = zzList[ix].zzTag;
                                        NowZyd.inputValue = 1 * NowZyd.zydLength;
                                        NowZydList.Add(NowZyd);
                                        NowZyd = new zydAClass();
                                        NowzydNum++;
                                    }
                                    if (zzList[ix].zzPoint.X == m_LineModelList[jy].Line_BeginPoint.X)
                                    {
                                        NowZyd.zydName = zzList[ix].zzName + "X";
                                        NowZyd.zydCol = lieX;
                                        NowZyd.zydRow = 3 * jy;
                                        NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                        NowZyd.zydTag = zzList[ix].zzTag;
                                        NowZyd.inputValue = 1;
                                        NowZydList.Add(NowZyd);
                                        NowZyd = new zydAClass();

                                        NowZyd.zydName = zzList[ix].zzName + "Y";
                                        NowZyd.zydCol = lieY;
                                        NowZyd.zydRow = 3 * jy + 1;
                                        NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                        NowZyd.zydTag = zzList[ix].zzTag;
                                        NowZyd.inputValue = -1;
                                        NowZydList.Add(NowZyd);
                                        NowZyd = new zydAClass();

                                        NowZyd.zydName = zzList[ix].zzName + "M";
                                        NowZyd.zydCol = lieM;
                                        NowZyd.zydRow = 3 * jy + 2;
                                        NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                        NowZyd.zydTag = zzList[ix].zzTag;
                                        NowZyd.inputValue = -1 * NowZyd.zydLength;
                                        NowZydList.Add(NowZyd);
                                        NowZyd = new zydAClass();
                                    }
                                    break;
                                }
                            case 8:
                                {
                                    NowZyd.zydName = zzList[ix].zzName + "X";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = -1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;


                                    NowZyd.zydName = zzList[ix].zzName + "M";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 2;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;
                                    break;
                                }
                            case 9:
                                {
                                    NowZyd.zydName = zzList[ix].zzName + "Y";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 1;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = -1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();

                                    NowZyd.zydName = zzList[ix].zzName + "YM";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 2;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = -1 * NowZyd.zydLength;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;

                                    NowZyd.zydName = zzList[ix].zzName + "M";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy + 2;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;
                                    break;
                                }
                            case 10:
                                {
                                    NowZyd.zydName = zzList[ix].zzName + "X";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = 1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;
                                    break;
                                }
                            case 11:
                                {
                                    NowZyd.zydName = zzList[ix].zzName + "X";
                                    NowZyd.zydCol = NowzydNum;
                                    NowZyd.zydRow = 3 * jy;
                                    NowZyd.zydLength = zzList[ix].zzPoint.X - m_LineModelList[jy].Line_BeginPoint.X;
                                    NowZyd.zydTag = zzList[ix].zzTag;
                                    NowZyd.inputValue = -1;
                                    NowZydList.Add(NowZyd);
                                    NowZyd = new zydAClass();
                                    NowzydNum++;
                                    break;
                                }
                        }
                    }
                }
            }
        }
        #endregion

        #endregion

        #region 反力影响线部分清空函数
        private void testBt_Click(object sender, RoutedEventArgs e)
        {
            comboList.Clear();
            zzComboBox.ItemsSource = null;
            zzCombomess();

            ClearInfluenceLine_FL();//清空，初始化
            BottomSectionClear();
            UserSectionClear();

            comboList2.Clear();
            zzComboBox2.ItemsSource = null;
            zzCombomess2();
            ClearInfluenceLine_FLO();
            BottomSectionClear();
            UserSectionClear();

            ClearInfluenceLine_WJ();
            BottomSectionClear();
            UserSectionClear();

            ClearInfluenceLine_JL();
            BottomSectionClear();
            UserSectionClear();

        }
        private void ClearInfluenceLine_FL()//反力清空函数
        {
            fhID = -1;
            zzComboBox.IsEnabled = true;
            foreach (zzClass zz in comboList)
            {
                zz.isselected = false;
            }
            NcomboList = new List<zzClass>();
            selectPt = new Point();
            influLine = new Line();
            knewPt = new Point();
            foreach (LineModelClass L1 in m_LineModelList)
            {
                L1.LineK = 10000000;
                L1.isDrew = false;
                L1.LineKnewPtNum = 0;
                L1.LineKnewPtList = new List<Point>();
                L1.isSelectBeam = false;
                L1.isSSBeam = false;
            }

            foreach (Image i in FLChangeImageList)
            {
                can.Children.Remove(i);
            }
            FLChangeImageList.Clear();
            FLChangeImage = new Image();

            can.Children.Remove(VirtualDText);
            influPt1 = new Point();
            influPt2 = new Point();
            influPtList = new List<Point>();
            foreach (Line L2 in InfluLineList)
            {
                can.Children.Remove(L2);
            }
            InfluLineList.Clear();

            foreach (Line tp1 in tempLineList)
            {
                can.Children.Remove(tp1);
            }
            tempLine = new Line();
            tempLineList.Clear();

            InfluClass = new LineModelClass();
            foreach (Line L3 in BottomInfluLineList1)
            {
                can.Children.Remove(L3);
            }
            BottomInfluLineList1.Clear();
            can.Children.Remove(NoticeTB1);
            InfluClassList.Clear();

            foreach (Line L in ShowinfluLineList)
            {
                can.Children.Remove(L);
            }
            ShowinfluLineList.Clear();
            ShowinfluLine = new Line();
            ShowinfluPtList.Clear();

            ratio1 = 0;
        }
        #endregion

        #region 支座反力ComboBox选择
        //支座ComboBox
        List<zzClass> comboList = new List<zzClass>();
        List<zzClass> JiaoList = new List<zzClass>();//铰链List

        private void zzCombomess()//支座反力下拉列表内容
        {
            //删除铰链
            for (int ic1 = 0; ic1 < zzList.Count; ic1++)
            {
                var intTag = Convert.ToInt32(zzList[ic1].zzTag);
                if (intTag == 1 || intTag == 2 || intTag == 3 || intTag == 4 || intTag == 6 || intTag == 9)
                {
                    comboList.Add(zzList[ic1]);
                    comboList = comboList.Distinct().ToList();
                }
            }
            //添加选项
            zzComboBox.ItemsSource = comboList;
            zzComboBox.SelectedValuePath = "zzID";
            zzComboBox.DisplayMemberPath = "zzName";
        }
        #endregion

        #region 支座反力影响线计算函数
        //返回的ID
        private int fhID = -1;
        TextBlock NoticeTB1 = new TextBlock();

        Image FLChangeImage = new Image();
        List<Image> FLChangeImageList = new List<Image>();
        private void zzComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//选择反馈
        {
            can.Children.Remove(NoticeTB1);
            fhID = zzComboBox.SelectedIndex;
            if (fhID > -1)
            {
                comboList[fhID].isselected = true;
                for (int if1 = 0; if1 < comboList.Count; if1++)
                {
                    if (comboList[if1].isselected == false)
                    {
                        zzComboBox.IsEnabled = false;
                    }
                }

                //添加力正方向
                #region 添加力正方向
                if (Convert.ToInt32(comboList[fhID].zzTag) == 1
                    || Convert.ToInt32(comboList[fhID].zzTag) == 2)//铰支座
                {
                    Image WhiteImage = new Image();
                    WhiteImage.Source = new BitmapImage(new Uri(@"Images/white.png", UriKind.Relative));
                    WhiteImage.Width = 40;
                    WhiteImage.Height = 40;
                    WhiteImage.SetValue(Canvas.LeftProperty, comboList[fhID].zzPoint.X - 20);
                    WhiteImage.SetValue(Canvas.TopProperty, comboList[fhID].zzPoint.Y - 40);
                    FLChangeImageList.Add(WhiteImage);
                    can.Children.Add(WhiteImage);

                    FLChangeImage = new Image();
                    FLChangeImage.Source = new BitmapImage(new Uri(@"Images/arrow.png", UriKind.Relative));
                    FLChangeImage.Width = 40;
                    FLChangeImage.Height = 40;
                    FLChangeImage.SetValue(Canvas.LeftProperty, comboList[fhID].zzPoint.X - 20);
                    FLChangeImage.SetValue(Canvas.TopProperty, comboList[fhID].zzPoint.Y);
                    ScaleTransform scl = new ScaleTransform();
                    scl.ScaleY = -1;
                    FLChangeImage.RenderTransform = scl;
                    FLChangeImageList.Add(FLChangeImage);
                    can.Children.Add(FLChangeImage);


                }
                if (Convert.ToInt32(comboList[fhID].zzTag) == 3)//左固定端
                {
                    ScaleTransform scl = new ScaleTransform();

                    FLChangeImage = new Image();
                    FLChangeImage.Source = new BitmapImage(new Uri(@"Images/WhuadzzX.jpg", UriKind.Relative));
                    FLChangeImage.Width = 40;
                    FLChangeImage.Height = 40;
                    FLChangeImage.SetValue(Canvas.LeftProperty, comboList[fhID].zzPoint.X - 40);
                    FLChangeImage.SetValue(Canvas.TopProperty, comboList[fhID].zzPoint.Y + 20);
                    FLChangeImage.RenderTransform = scl;
                    FLChangeImageList.Add(FLChangeImage);
                    can.Children.Add(FLChangeImage);

                    FLChangeImage = new Image();
                    FLChangeImage.Source = new BitmapImage(new Uri(@"Images/arrow.png", UriKind.Relative));
                    FLChangeImage.Width = 40;
                    FLChangeImage.Height = 40;
                    FLChangeImage.SetValue(Canvas.LeftProperty, comboList[fhID].zzPoint.X - 20);
                    FLChangeImage.SetValue(Canvas.TopProperty, comboList[fhID].zzPoint.Y - 20);
                    scl.ScaleY = -1;
                    FLChangeImage.RenderTransform = scl;
                    FLChangeImageList.Add(FLChangeImage);
                    can.Children.Add(FLChangeImage);

                }

                if (Convert.ToInt32(comboList[fhID].zzTag) == 4)//右固定端
                {
                    FLChangeImage = new Image();
                    FLChangeImage.Source = new BitmapImage(new Uri(@"Images/arrow.png", UriKind.Relative));
                    FLChangeImage.Width = 40;
                    FLChangeImage.Height = 40;
                    FLChangeImage.SetValue(Canvas.LeftProperty, comboList[fhID].zzPoint.X - 20);
                    FLChangeImage.SetValue(Canvas.TopProperty, comboList[fhID].zzPoint.Y - 20);
                    ScaleTransform scl = new ScaleTransform();
                    scl.ScaleY = -1;
                    FLChangeImage.RenderTransform = scl;
                    FLChangeImageList.Add(FLChangeImage);
                    can.Children.Add(FLChangeImage);

                    FLChangeImage = new Image();
                    FLChangeImage.Source = new BitmapImage(new Uri(@"Images/Whuadzz-X.jpg", UriKind.Relative));
                    FLChangeImage.Width = 40;
                    FLChangeImage.Height = 40;
                    FLChangeImage.SetValue(Canvas.LeftProperty, comboList[fhID].zzPoint.X);
                    FLChangeImage.SetValue(Canvas.TopProperty, comboList[fhID].zzPoint.Y + 20);
                    FLChangeImage.RenderTransform = scl;
                    FLChangeImageList.Add(FLChangeImage);
                    can.Children.Add(FLChangeImage);
                }

                if (Convert.ToInt32(comboList[fhID].zzTag) == 6)//定向支座Y
                {
                }
                if (Convert.ToInt32(comboList[fhID].zzTag) == 9)//定向支座-Y
                {
                }

                #endregion
                InfluenceLine_Cal();
                MarkPoint();

                NoticeTB1 = new TextBlock();
                NoticeTB1.Text = "当前求" + comboList[fhID].zzName + "的支座反力影响线";
                NoticeTB1.Margin = new Thickness(340, y, 0, 0);
                NoticeTB1.FontSize = 16;
                ScaleTransform scale = new ScaleTransform();
                scale.ScaleY = -1;
                NoticeTB1.RenderTransform = scale;
                can.Children.Add(NoticeTB1);
            }
        }

        private List<zzClass> NcomboList = new List<zzClass>();
        Point selectPt;//被选支座点
        private Line influLine;//影响线   
        private Point knewPt;//已知点
        TextBlock VirtualDText = new TextBlock();

        private void InfluenceLine_Cal()//影响线计算主函数
        {
            NcomboList = new List<zzClass>(zzList);
            foreach (zzClass jiao in JiaoList)//加入铰链
            {
                NcomboList.Add(jiao);
            }
            //删去被选支座，在新List里循环
            for (int if2 = 0; if2 < NcomboList.Count; if2++)
            {
                if (NcomboList[if2].isselected == true)
                {
                    NcomboList.RemoveAt(if2);
                }
            }
            //被选支座处虚位移
            if (Convert.ToInt32(comboList[fhID].zzTag) == 3
                || Convert.ToInt32(comboList[fhID].zzTag) == 4)//被选支座为固定端
            {
                selectPt.X = comboList[fhID].zzPoint.X;
                selectPt.Y = comboList[fhID].zzPoint.Y + 20;
                m_LineModelList[comboList[fhID].zzGan].LineK = 0;//斜率
                m_LineModelList[comboList[fhID].zzGan].LineKnewPtList.Add(selectPt);//加入此杆已知点list
            }
            else if (Convert.ToInt32(comboList[fhID].zzTag) == 6
                || Convert.ToInt32(comboList[fhID].zzTag) == 9)//被选支座为定向支座Y
            {
                selectPt.X = comboList[fhID].zzPoint.X;
                selectPt.Y = comboList[fhID].zzPoint.Y + 20;
                m_LineModelList[comboList[fhID].zzGan].LineK = 0;//斜率
                m_LineModelList[comboList[fhID].zzGan].LineKnewPtList.Add(selectPt);//加入此杆已知点list
            }
            else
            {
                selectPt.X = comboList[fhID].zzPoint.X;
                selectPt.Y = comboList[fhID].zzPoint.Y + 20;
                m_LineModelList[comboList[fhID].zzGan].LineKnewPtList.Add(selectPt);//加入此杆已知点list
            }

            //标虚位移为1
            VirtualDText = new TextBlock();
            var TextX = selectPt.X;
            var TextY = selectPt.Y + 20;
            VirtualDText.Text = "1";
            VirtualDText.Margin = new Thickness(TextX, TextY, 0, 0);
            ScaleTransform VDScale = new ScaleTransform();
            VDScale.ScaleY = -1;
            VirtualDText.RenderTransform = VDScale;
            VirtualDText.Background = Brushes.DarkGreen;
            VirtualDText.Foreground = Brushes.White;
            //确定支座杆号
            ConfirminfluzzGan();
            //建立已知点集
            BuildPtList();
            //收集影响线点list元素
            CollectInfluPt();
            //绘制影响线
            DrawInfluenceLine();
        }

        #region 确定支座杆号
        private void ConfirminfluzzGan()//确定支座杆号
        {
            for (int mf1 = 0; mf1 < NcomboList.Count; mf1++)//循环支座
            {
                for (int nf1 = 0; nf1 < m_LineModelList.Count; nf1++)//循环杆
                {
                    if (m_LineModelList[nf1].Line_BeginPoint.X <= NcomboList[mf1].zzPoint.X
                        && NcomboList[mf1].zzPoint.X <= m_LineModelList[nf1].Line_EndPoint.X)
                    {
                        NcomboList[mf1].influzzGan = nf1;
                        break;
                    }
                }
            }
        }
        #endregion

        #region 建立已知点集
        private void BuildPtList()//建立已知点集
        {
            for (int mf2 = 0; mf2 < NcomboList.Count; mf2++)//循环支座
            {
                for (int nf2 = 0; nf2 < m_LineModelList.Count; nf2++)
                {
                    if (NcomboList[mf2].isselected == false && NcomboList[mf2].influzzGan == nf2)//如果不是被选支座且在杆上
                    {
                        if (Convert.ToInt32(NcomboList[mf2].zzTag) == 1
                             || Convert.ToInt32(NcomboList[mf2].zzTag) == 2)//固定铰支座、活动铰支座
                        {
                            knewPt = new Point(NcomboList[mf2].zzPoint.X, NcomboList[mf2].zzPoint.Y);
                            m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList.Add(knewPt);
                            m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList = m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList.Distinct().ToList();
                        }
                        else if (Convert.ToInt32(NcomboList[mf2].zzTag) == 3
                            || Convert.ToInt32(NcomboList[mf2].zzTag) == 4)//固定端
                        {
                            knewPt = new Point(NcomboList[mf2].zzPoint.X, NcomboList[mf2].zzPoint.Y);
                            m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList.Add(knewPt);
                            m_LineModelList[NcomboList[mf2].influzzGan].LineK = 0;//斜率
                            m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList = m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList.Distinct().ToList();
                        }
                        else if (Convert.ToInt32(NcomboList[mf2].zzTag) == 6
                             || Convert.ToInt32(NcomboList[mf2].zzTag) == 9)//定向支座Y
                        {
                            knewPt = new Point(NcomboList[mf2].zzPoint.X, NcomboList[mf2].zzPoint.Y);
                            m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList.Add(knewPt);
                            m_LineModelList[NcomboList[mf2].influzzGan].LineK = 0;//斜率
                            m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList = m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList.Distinct().ToList();
                        }
                        else if (Convert.ToInt32(NcomboList[mf2].zzTag) == 5
                             || Convert.ToInt32(NcomboList[mf2].zzTag) == 8)//定向支座X
                        {
                            knewPt = new Point(NcomboList[mf2].zzPoint.X, NcomboList[mf2].zzPoint.Y);
                            // m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList.Add(knewPt);
                            m_LineModelList[NcomboList[mf2].influzzGan].LineK = 0;//斜率
                            m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList = m_LineModelList[NcomboList[mf2].influzzGan].LineKnewPtList.Distinct().ToList();
                        }
                    }
                }
            }
        }
        #endregion

        Point influPt1;//影响线起点
        Point influPt2;//影响线终点
        List<Point> influPtList = new List<Point>();//影响线点list
        #region 收集影响线点list元素
        private void CollectInfluPt()
        {
            for (int nf3 = 0; nf3 < m_LineModelList.Count; nf3++)//循环杆
            {
                if (nf3 <= 50)
                {

                    if (m_LineModelList[nf3].isDrew == false)
                    {
                        for (int p = 0; p < m_LineModelList[nf3].LineKnewPtList.Count; p++)//循环已知点list
                        {
                            if (m_LineModelList[nf3].LineKnewPtList.Count >= 2)//同杆上已知点数为2
                            {
                                m_LineModelList[nf3].LineK = (m_LineModelList[nf3].LineKnewPtList[1].Y - m_LineModelList[nf3].LineKnewPtList[0].Y) / (m_LineModelList[nf3].LineKnewPtList[1].X - m_LineModelList[nf3].LineKnewPtList[0].X);
                            }
                            if (m_LineModelList[nf3].LineKnewPtList.Count >= 1)//同杆上已知点数为1
                            {
                                if (m_LineModelList[nf3].LineK != 10000000)//斜率已知
                                {
                                    if (m_LineModelList[nf3].LineKnewPtList[p].X > m_LineModelList[nf3].Line_BeginPoint.X
                                        && m_LineModelList[nf3].LineKnewPtList[p].X < m_LineModelList[nf3].Line_EndPoint.X)//如果已知点不在杆端
                                    {
                                        influPt1.X = m_LineModelList[nf3].Line_BeginPoint.X;
                                        influPt1.Y = m_LineModelList[nf3].LineKnewPtList[0].Y - m_LineModelList[nf3].LineK * (m_LineModelList[nf3].LineKnewPtList[0].X - m_LineModelList[nf3].Line_BeginPoint.X);
                                        influPt2.X = m_LineModelList[nf3].Line_EndPoint.X;
                                        influPt2.Y = m_LineModelList[nf3].LineKnewPtList[0].Y - m_LineModelList[nf3].LineK * (m_LineModelList[nf3].LineKnewPtList[0].X - m_LineModelList[nf3].Line_EndPoint.X);
                                        influPtList.Add(influPt1);
                                        influPtList.Add(influPt2);//加入影响线点list
                                        m_LineModelList[nf3].isDrew = true;
                                        foreach (LineModelClass line in m_LineModelList)
                                        {
                                            if (influPt1.X >= line.Line_BeginPoint.X && influPt1.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt1);
                                            }
                                            if (influPt2.X >= line.Line_BeginPoint.X && influPt2.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt2);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        break;
                                    }
                                    else if (m_LineModelList[nf3].LineKnewPtList[p].X == m_LineModelList[nf3].Line_BeginPoint.X)//已知点在杆端起点
                                    {
                                        influPt1 = m_LineModelList[nf3].LineKnewPtList[p];
                                        influPt2.X = m_LineModelList[nf3].Line_EndPoint.X;
                                        influPt2.Y = m_LineModelList[nf3].LineKnewPtList[p].Y - m_LineModelList[nf3].LineK * (m_LineModelList[nf3].LineKnewPtList[p].X - m_LineModelList[nf3].Line_EndPoint.X);
                                        influPtList.Add(influPt1);
                                        influPtList.Add(influPt2);
                                        m_LineModelList[nf3].isDrew = true;
                                        foreach (LineModelClass line in m_LineModelList)
                                        {
                                            if (influPt1.X >= line.Line_BeginPoint.X && influPt1.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt1);
                                            }
                                            if (influPt2.X >= line.Line_BeginPoint.X && influPt2.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt2);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        break;
                                    }
                                    else if (m_LineModelList[nf3].LineKnewPtList[p].X == m_LineModelList[nf3].Line_EndPoint.X)//已知点在杆端终点
                                    {
                                        influPt1.X = m_LineModelList[nf3].Line_BeginPoint.X;
                                        influPt1.Y = m_LineModelList[nf3].LineKnewPtList[p].Y - m_LineModelList[nf3].LineK * (m_LineModelList[nf3].LineKnewPtList[p].X - m_LineModelList[nf3].Line_BeginPoint.X);
                                        influPt2 = m_LineModelList[nf3].LineKnewPtList[p];
                                        influPtList.Add(influPt1);
                                        influPtList.Add(influPt2);
                                        m_LineModelList[nf3].isDrew = true;
                                        foreach (LineModelClass line in m_LineModelList)
                                        {
                                            if (influPt1.X >= line.Line_BeginPoint.X && influPt1.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt1);
                                            }
                                            if (influPt2.X >= line.Line_BeginPoint.X && influPt2.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt2);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("结果溢出!");
                }
            }
            for (int af1 = 0; af1 < m_LineModelList.Count; af1++)
            {
                if (m_LineModelList[af1].isDrew == false)
                {
                    CollectInfluPt();
                }
            }
        }
        #endregion

        double ratio1 = 0;

        #region 绘制影响线
        List<Line> InfluLineList = new List<Line>();
        LineModelClass InfluClass = new LineModelClass();
        List<LineModelClass> InfluClassList = new List<LineModelClass>();
        Line tempLine = new Line();
        List<Line> tempLineList = new List<Line>();

        List<Point> ShowinfluPtList = new List<Point>();
        Line ShowinfluLine = new Line();
        List<Line> ShowinfluLineList = new List<Line>();
        private void DrawInfluenceLine()
        {
            influPtList = influPtList.Distinct().ToList();//去除重复点
            #region 归一化
            for (int jf1 = 0; jf1 < influPtList.Count; jf1++)
            {
                if (Math.Abs(influPtList[jf1].Y - m_LineModelList[0].Line_BeginPoint.Y) > 80)
                {
                    ratio1 = 80 / (influPtList[jf1].Y - m_LineModelList[0].Line_BeginPoint.Y);
                }
            }
            ratio1 = Math.Abs(ratio1);
            List<Point> tempList = new List<Point>();
            ShowinfluPtList = new List<Point>(influPtList);
            if (ratio1 != 0)
            {
                for (int kf1 = 0; kf1 < ShowinfluPtList.Count; kf1++)
                {
                    Point tempPt = new Point();
                    tempPt.X = ShowinfluPtList[kf1].X;
                    tempPt.Y = (ShowinfluPtList[kf1].Y - m_LineModelList[0].Line_BeginPoint.Y) * ratio1 + m_LineModelList[0].Line_BeginPoint.Y;
                    tempList.Add(tempPt);
                }
                ShowinfluPtList.Clear();
                foreach (Point P in tempList)
                {
                    ShowinfluPtList.Add(P);
                }
            }
            for (int sf1 = 0; sf1 < ShowinfluPtList.Count - 1; sf1++)
            {
                ShowinfluPtList.Distinct().ToList();
                ShowinfluPtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                ShowinfluLine = new Line();
                ShowinfluLine.Stroke = Brushes.Green;
                ShowinfluLine.StrokeThickness = 1;
                ShowinfluLine.X1 = ShowinfluPtList[sf1].X;
                ShowinfluLine.Y1 = ShowinfluPtList[sf1].Y;
                ShowinfluLine.X2 = ShowinfluPtList[sf1 + 1].X;
                ShowinfluLine.Y2 = ShowinfluPtList[sf1 + 1].Y;
                ShowinfluLineList.Add(ShowinfluLine);
                //can.Children.Add(ShowinfluLine);
            }
            #endregion

            for (int if3 = 0; if3 < influPtList.Count - 1; if3++)
            {
                influPtList = influPtList.Distinct().ToList();
                influPtList.Sort((P1, P2) => P1.X.CompareTo(P2.X));
                influLine = new Line();
                influLine.Stroke = Brushes.Green;
                influLine.StrokeThickness = 1;
                influLine.X1 = influPtList[if3].X;
                influLine.Y1 = influPtList[if3].Y;
                influLine.X2 = influPtList[if3 + 1].X;
                influLine.Y2 = influPtList[if3 + 1].Y;

                InfluClass.Line_BeginPoint.X = influLine.X1;
                InfluClass.Line_BeginPoint.Y = influLine.Y1;
                InfluClass.Line_EndPoint.X = influLine.X2;
                InfluClass.Line_EndPoint.Y = influLine.Y2;
                InfluClass.LineLength = InfluClass.Line_EndPoint.Y - InfluClass.Line_BeginPoint.Y;
                InfluClass.LineK = (InfluClass.Line_EndPoint.Y - InfluClass.Line_BeginPoint.Y) / (InfluClass.Line_EndPoint.X - InfluClass.Line_BeginPoint.X);
                InfluClassList.Add(InfluClass);
                InfluClass = new LineModelClass();

                InfluLineList.Add(influLine);
            }
            //闭合影响线
            m_LineModelList.Sort((l1, l2) => l1.Line_BeginPoint.X.CompareTo(l2.Line_BeginPoint.X));
            if (ShowinfluPtList[0].Y != m_LineModelList[0].Line_BeginPoint.Y)
            {
                tempLine = new Line();
                tempLine.Stroke = Brushes.Green;
                tempLine.StrokeThickness = 1;
                tempLine.X1 = ShowinfluPtList[0].X;
                tempLine.Y1 = ShowinfluPtList[0].Y;
                tempLine.X2 = m_LineModelList[0].Line_BeginPoint.X;
                tempLine.Y2 = m_LineModelList[0].Line_BeginPoint.Y;
                tempLineList.Add(tempLine);
            }
            if (ShowinfluPtList[ShowinfluPtList.Count - 1].Y != m_LineModelList[m_LineModelList.Count - 1].Line_EndPoint.Y)
            {
                tempLine = new Line();
                tempLine.Stroke = Brushes.Green;
                tempLine.StrokeThickness = 1;
                tempLine.X1 = ShowinfluPtList[ShowinfluPtList.Count - 1].X;
                tempLine.Y1 = ShowinfluPtList[ShowinfluPtList.Count - 1].Y;
                tempLine.X2 = m_LineModelList[m_LineModelList.Count - 1].Line_EndPoint.X;
                tempLine.Y2 = m_LineModelList[m_LineModelList.Count - 1].Line_EndPoint.Y;
                tempLineList.Add(tempLine);
            }
        }
        #endregion

        #endregion

        #region 支座反力偶影响线计算

        #region 支座反力偶ComboBox选择
        List<zzClass> comboList2 = new List<zzClass>();
        private void zzCombomess2()
        {
            //只加入固定端，定向支座
            for (int ic2 = 0; ic2 < zzList.Count; ic2++)
            {
                var iTag = Convert.ToInt32(zzList[ic2].zzTag);
                if (iTag == 3 || iTag == 4 || iTag == 5 || iTag == 8 || iTag == 6 || iTag == 9)
                {
                    comboList2.Add(zzList[ic2]);
                    comboList2 = comboList2.Distinct().ToList();
                }
            }
            zzComboBox2.ItemsSource = comboList2;
            zzComboBox2.SelectedValuePath = "zzID";
            zzComboBox2.DisplayMemberPath = "zzName";
        }
        #endregion

        #region 支座反力偶影响线计算函数

        //返回ID2
        private int fhID2 = -1;
        TextBlock NoticeTB2 = new TextBlock();

        Image FLOChangeImage = new Image();
        List<Image> FLOChangeImageList = new List<Image>();

        Image FLOImage = new Image();
        List<Image> FLOImageList = new List<Image>();

        Image WarrowImage = new Image();
        List<Image> WarrowImageList = new List<Image>();
        private void zzComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fhID2 = zzComboBox2.SelectedIndex;
            if (fhID2 > -1)
            {
                comboList2[fhID2].isselected = true;
                for (int io1 = 0; io1 < comboList2.Count; io1++)
                {
                    if (comboList2[io1].isselected == false)
                    {
                        zzComboBox2.IsEnabled = false;
                    }
                }
                //添加力正方向
                #region 添加力正方向
                if (Convert.ToInt32(comboList2[fhID2].zzTag) == 3)//左固定端
                {
                    FLOChangeImage = new Image();
                    FLOChangeImage.Source = new BitmapImage(new Uri(@"Images/jiaolian.png", UriKind.Relative));
                    FLOChangeImage.Width = 40;
                    FLOChangeImage.Height = 40;
                    FLOChangeImage.SetValue(Canvas.LeftProperty, comboList2[fhID2].zzPoint.X - 20);
                    FLOChangeImage.SetValue(Canvas.TopProperty, comboList2[fhID2].zzPoint.Y + 20);
                    ScaleTransform scl1 = new ScaleTransform();
                    scl1.ScaleY = -1;
                    FLOChangeImage.RenderTransform = scl1;
                    FLOChangeImageList.Add(FLOChangeImage);
                    can.Children.Add(FLOChangeImage);

                    WarrowImage = new Image();
                    WarrowImage.Source = new BitmapImage(new Uri(@"Images/Warrow.png", UriKind.Relative));
                    WarrowImage.Width = 40;
                    WarrowImage.Height = 40;
                    WarrowImage.SetValue(Canvas.LeftProperty, comboList2[fhID2].zzPoint.X - 20);
                    WarrowImage.SetValue(Canvas.TopProperty, comboList2[fhID2].zzPoint.Y + 20);
                    ScaleTransform scl = new ScaleTransform();
                    scl.ScaleY = -1;
                    scl.ScaleX = -1;
                    WarrowImage.RenderTransform = scl;
                    WarrowImageList.Add(WarrowImage);
                    can.Children.Add(WarrowImage);
                }

                else if (Convert.ToInt32(comboList2[fhID2].zzTag) == 4)//右固定端
                {
                    FLOChangeImage = new Image();
                    FLOChangeImage.Source = new BitmapImage(new Uri(@"Images/jiaolian.png", UriKind.Relative));
                    FLOChangeImage.Width = 40;
                    FLOChangeImage.Height = 40;
                    FLOChangeImage.SetValue(Canvas.LeftProperty, comboList2[fhID2].zzPoint.X - 20);
                    FLOChangeImage.SetValue(Canvas.TopProperty, comboList2[fhID2].zzPoint.Y + 20);
                    ScaleTransform scl = new ScaleTransform();
                    scl.ScaleY = -1;
                    FLOChangeImage.RenderTransform = scl;
                    FLOChangeImageList.Add(FLOChangeImage);
                    can.Children.Add(FLOChangeImage);

                    WarrowImage = new Image();
                    WarrowImage.Source = new BitmapImage(new Uri(@"Images/Warrow.png", UriKind.Relative));
                    WarrowImage.Width = 40;
                    WarrowImage.Height = 40;
                    WarrowImage.SetValue(Canvas.LeftProperty, comboList2[fhID2].zzPoint.X + 20);
                    WarrowImage.SetValue(Canvas.TopProperty, comboList2[fhID2].zzPoint.Y + 20);
                    ScaleTransform scl2 = new ScaleTransform();
                    scl2.ScaleY = -1;
                    WarrowImage.RenderTransform = scl2;
                    WarrowImageList.Add(WarrowImage);
                    can.Children.Add(WarrowImage);
                }

                else if (Convert.ToInt32(comboList2[fhID2].zzTag) == 5)//左定向支座
                {

                    string WhuodzzXPath = @"Images/WhuodzzX.jpg";
                    ScaleTransform scl = new ScaleTransform();

                    using (BinaryReader loader = new BinaryReader(File.Open(WhuodzzXPath, FileMode.Open)))
                    {
                        #region 图片二进制

                        FileInfo fd1 = new FileInfo(WhuodzzXPath);
                        int length1 = (int)fd1.Length;
                        byte[] by1 = new byte[length1];
                        by1 = loader.ReadBytes((int)fd1.Length);
                        loader.Dispose();
                        loader.Close();

                        BitmapImage bim = new BitmapImage();
                        bim.BeginInit();
                        bim.StreamSource = new MemoryStream(by1);
                        bim.EndInit();
                        #endregion

                        FLOImage = new Image();
                        FLOImage.Source = bim;
                        FLOImage.Width = 40;
                        FLOImage.Height = 40;
                        FLOImage.SetValue(Canvas.LeftProperty, comboList2[fhID2].zzPoint.X - 40);
                        FLOImage.SetValue(Canvas.TopProperty, comboList2[fhID2].zzPoint.Y + 20);
                        scl.ScaleY = -1;
                        FLOImage.RenderTransform = scl;
                        FLOImageList.Add(FLOImage);
                        can.Children.Add(FLOImage);

                    }

                    WarrowImage = new Image();
                    WarrowImage.Source = new BitmapImage(new Uri(@"Images/Warrow.png", UriKind.Relative));
                    WarrowImage.Width = 40;
                    WarrowImage.Height = 40;
                    WarrowImage.SetValue(Canvas.LeftProperty, comboList2[fhID2].zzPoint.X - 40);
                    WarrowImage.SetValue(Canvas.TopProperty, comboList2[fhID2].zzPoint.Y + 20);
                    ScaleTransform scl1 = new ScaleTransform();
                    scl1.ScaleX = -1;
                    scl1.ScaleY = -1;
                    WarrowImage.RenderTransform = scl1;
                    WarrowImageList.Add(WarrowImage);
                    can.Children.Add(WarrowImage);
                }
                else if (Convert.ToInt32(comboList2[fhID2].zzTag) == 8)//右定向支座
                {
                    string Whuodzz2Path = @"Images/Whuodzz-X.jpg";
                    ScaleTransform scl = new ScaleTransform();

                    using (BinaryReader loader = new BinaryReader(File.Open(Whuodzz2Path, FileMode.Open)))
                    {
                        #region 图片二进制

                        FileInfo fd1 = new FileInfo(Whuodzz2Path);
                        int length1 = (int)fd1.Length;
                        byte[] by1 = new byte[length1];
                        by1 = loader.ReadBytes((int)fd1.Length);
                        loader.Dispose();
                        loader.Close();

                        BitmapImage bim = new BitmapImage();
                        bim.BeginInit();
                        bim.StreamSource = new MemoryStream(by1);
                        bim.EndInit();
                        #endregion

                        FLOImage = new Image();
                        FLOImage.Source = bim;
                        FLOImage.Width = 40;
                        FLOImage.Height = 40;
                        FLOImage.SetValue(Canvas.LeftProperty, comboList2[fhID2].zzPoint.X);
                        FLOImage.SetValue(Canvas.TopProperty, comboList2[fhID2].zzPoint.Y + 20);
                        scl.ScaleY = -1;
                        FLOImage.RenderTransform = scl;
                        FLOImageList.Add(FLOImage);
                        can.Children.Add(FLOImage);

                    }

                    WarrowImage = new Image();
                    WarrowImage.Source = new BitmapImage(new Uri(@"Images/Warrow.png", UriKind.Relative));
                    WarrowImage.Width = 40;
                    WarrowImage.Height = 40;
                    WarrowImage.SetValue(Canvas.LeftProperty, comboList2[fhID2].zzPoint.X + 40);
                    WarrowImage.SetValue(Canvas.TopProperty, comboList2[fhID2].zzPoint.Y + 20);
                    ScaleTransform scl2 = new ScaleTransform();
                    scl2.ScaleY = -1;
                    WarrowImage.RenderTransform = scl2;
                    WarrowImageList.Add(WarrowImage);
                    can.Children.Add(WarrowImage);
                }
                else if (Convert.ToInt32(comboList2[fhID2].zzTag) == 6
                   || Convert.ToInt32(NcomboList2[fhID2].zzTag) == 9)//定向支座Y
                {
                    string Whuodzz2Path = @"Images/Whuodzz.jpg";
                    ScaleTransform scl = new ScaleTransform();

                    using (BinaryReader loader = new BinaryReader(File.Open(Whuodzz2Path, FileMode.Open)))
                    {
                        #region 图片二进制

                        FileInfo fd1 = new FileInfo(Whuodzz2Path);
                        int length1 = (int)fd1.Length;
                        byte[] by1 = new byte[length1];
                        by1 = loader.ReadBytes((int)fd1.Length);
                        loader.Dispose();
                        loader.Close();

                        BitmapImage bim = new BitmapImage();
                        bim.BeginInit();
                        bim.StreamSource = new MemoryStream(by1);
                        bim.EndInit();
                        #endregion

                        FLOImage = new Image();
                        FLOImage.Source = bim;
                        FLOImage.Width = 40;
                        FLOImage.Height = 40;
                        FLOImage.SetValue(Canvas.LeftProperty, comboList2[fhID2].zzPoint.X - 20);
                        FLOImage.SetValue(Canvas.TopProperty, comboList2[fhID2].zzPoint.Y);
                        scl.ScaleY = -1;
                        FLOImage.RenderTransform = scl;
                        FLOImageList.Add(FLOImage);
                        can.Children.Add(FLOImage);

                    }

                    WarrowImage = new Image();
                    WarrowImage.Source = new BitmapImage(new Uri(@"Images/Warrow.png", UriKind.Relative));
                    WarrowImage.Width = 40;
                    WarrowImage.Height = 40;
                    if (Math.Abs(comboList2[fhID2].zzPoint.X - m_LineModelList[comboList2[fhID2].zzGan].Line_BeginPoint.X) < Math.Abs(comboList2[fhID2].zzPoint.X - m_LineModelList[comboList2[fhID2].zzGan].Line_EndPoint.X))
                    {
                        WarrowImage.SetValue(Canvas.LeftProperty, comboList2[fhID2].zzPoint.X - 20);
                        WarrowImage.SetValue(Canvas.TopProperty, comboList2[fhID2].zzPoint.Y + 20);
                        ScaleTransform scl2 = new ScaleTransform();
                        scl2.ScaleX = -1;
                        scl2.ScaleY = -1;
                        WarrowImage.RenderTransform = scl2;
                    }
                    else if (Math.Abs(comboList2[fhID2].zzPoint.X - m_LineModelList[comboList2[fhID2].zzGan].Line_BeginPoint.X) > Math.Abs(comboList2[fhID2].zzPoint.X - m_LineModelList[comboList2[fhID2].zzGan].Line_EndPoint.X))
                    {
                        WarrowImage.SetValue(Canvas.LeftProperty, comboList2[fhID2].zzPoint.X + 20);
                        WarrowImage.SetValue(Canvas.TopProperty, comboList2[fhID2].zzPoint.Y + 20);
                        ScaleTransform scl2 = new ScaleTransform();
                        scl2.ScaleY = -1;
                        WarrowImage.RenderTransform = scl2;
                    }
                    WarrowImageList.Add(WarrowImage);
                    can.Children.Add(WarrowImage);
                }

                #endregion

                InfluenceLine2_Cal();
                MarkPoint();

                NoticeTB2 = new TextBlock();
                NoticeTB2.Text = "当前求" + comboList2[fhID2].zzName + "的支座反力偶影响线";
                NoticeTB2.Margin = new Thickness(340, y, 0, 0);
                NoticeTB2.FontSize = 16;
                ScaleTransform scale = new ScaleTransform();
                scale.ScaleY = -1;
                NoticeTB2.RenderTransform = scale;
                can.Children.Add(NoticeTB2);
            }
        }

        private List<zzClass> NcomboList2 = new List<zzClass>();
        Point selectPt2;//被选支座点
        private Line influLine2;//影响线   
        private Point knewPt2;//已知点

        private void InfluenceLine2_Cal()//弯矩影响线计算主函数
        {
            NcomboList2 = new List<zzClass>(zzList);
            foreach (zzClass jiao in JiaoList)//加入铰链
            {
                NcomboList2.Add(jiao);
            }
            //删去被选支座，在新List里循环
            for (int io2 = 0; io2 < NcomboList2.Count; io2++)
            {
                if (NcomboList2[io2].isselected == true)
                {
                    NcomboList2.RemoveAt(io2);
                }
            }
            //被选支座处虚位移
            if (Convert.ToInt32(comboList2[fhID2].zzTag) == 3)//被选支座为固定端
            {
                selectPt2.X = comboList2[fhID2].zzPoint.X;
                selectPt2.Y = comboList2[fhID2].zzPoint.Y;
                m_LineModelList[comboList2[fhID2].zzGan].LineK = -1;//斜率
                m_LineModelList[comboList2[fhID2].zzGan].LineKnewPtList.Add(selectPt2);//加入此杆已知点list
            }
            else if (Convert.ToInt32(comboList2[fhID2].zzTag) == 4)//被选支座为固定端
            {
                selectPt2.X = comboList2[fhID2].zzPoint.X;
                selectPt2.Y = comboList2[fhID2].zzPoint.Y;
                m_LineModelList[comboList2[fhID2].zzGan].LineK = 1;//斜率
                m_LineModelList[comboList2[fhID2].zzGan].LineKnewPtList.Add(selectPt2);//加入此杆已知点list
            }
            else if (Convert.ToInt32(comboList2[fhID2].zzTag) == 5)//被选支座为定向支座X
            {
                m_LineModelList[comboList2[fhID2].zzGan].LineK = 1;//斜率
            }
            else if (Convert.ToInt32(comboList2[fhID2].zzTag) == 8)//被选支座为定向支座-X
            {
                m_LineModelList[comboList2[fhID2].zzGan].LineK = -1;//斜率
            }
            else if (Convert.ToInt32(comboList2[fhID2].zzTag) == 6
                || Convert.ToInt32(comboList2[fhID2].zzTag) == 9)//被选支座为定向支座Y，-Y
            {
                selectPt2.X = comboList2[fhID2].zzPoint.X;
                selectPt2.Y = comboList2[fhID2].zzPoint.Y;
                m_LineModelList[comboList2[fhID2].zzGan].LineK = 1;//斜率
                m_LineModelList[comboList2[fhID2].zzGan].LineKnewPtList.Add(selectPt2);//加入此杆已知点list
                /*
                if (Math.Abs (comboList2[fhID2].zzPoint.X-m_LineModelList[comboList2[fhID2].zzGan].Line_BeginPoint .X )<Math.Abs(comboList2[fhID2].zzPoint.X-m_LineModelList[comboList2[fhID2].zzGan].Line_EndPoint.X ))
                {
                    m_LineModelList[comboList2[fhID2].zzGan].LineK = 1;//斜率
                }
                else if (Math.Abs(comboList2[fhID2].zzPoint.X - m_LineModelList[comboList2[fhID2].zzGan].Line_BeginPoint.X) >Math.Abs(comboList2[fhID2].zzPoint.X - m_LineModelList[comboList2[fhID2].zzGan].Line_EndPoint.X))
                {
                    m_LineModelList[comboList2[fhID2].zzGan].LineK = -1;//斜率
                }
                */
            }
            //确定支座杆号
            ConfirminfluzzGan2();
            //建立已知点集
            BuildPtList2();
            //判断同杆有多少个已知点
            CollectInfluPt2();
            //绘制影响线
            DrawInfluenceLine2();
        }

        #region 确定支座杆号2
        private void ConfirminfluzzGan2()//确定支座杆号
        {
            for (int mo1 = 0; mo1 < NcomboList2.Count; mo1++)//循环支座
            {
                for (int no1 = 0; no1 < m_LineModelList.Count; no1++)//循环杆
                {
                    if (m_LineModelList[no1].Line_BeginPoint.X <= NcomboList2[mo1].zzPoint.X
                        && NcomboList2[mo1].zzPoint.X <= m_LineModelList[no1].Line_EndPoint.X)
                    {
                        NcomboList2[mo1].influzzGan = no1;
                        break;
                    }
                }
            }
        }
        #endregion

        #region 建立已知点集2
        private void BuildPtList2()//建立已知点集
        {
            for (int mo2 = 0; mo2 < NcomboList2.Count; mo2++)//循环支座
            {
                for (int no2 = 0; no2 < m_LineModelList.Count; no2++)
                {
                    if (NcomboList2[mo2].isselected == false && NcomboList2[mo2].influzzGan == no2)//如果不是被选支座且在杆上
                    {
                        if (Convert.ToInt32(NcomboList2[mo2].zzTag) == 1
                             || Convert.ToInt32(NcomboList2[mo2].zzTag) == 2)//固定铰支座、活动铰支座
                        {
                            knewPt2 = new Point(NcomboList2[mo2].zzPoint.X, NcomboList2[mo2].zzPoint.Y);
                            m_LineModelList[NcomboList2[mo2].influzzGan].LineKnewPtList.Add(knewPt2);
                            m_LineModelList[NcomboList2[mo2].influzzGan].LineKnewPtList = m_LineModelList[NcomboList2[mo2].influzzGan].LineKnewPtList.Distinct().ToList();
                        }
                        else if (Convert.ToInt32(NcomboList2[mo2].zzTag) == 3
                            || Convert.ToInt32(NcomboList2[mo2].zzTag) == 4)//固定端
                        {
                            knewPt2 = new Point(NcomboList2[mo2].zzPoint.X, NcomboList2[mo2].zzPoint.Y);
                            m_LineModelList[NcomboList2[mo2].influzzGan].LineKnewPtList.Add(knewPt2);
                            m_LineModelList[NcomboList2[mo2].influzzGan].LineK = 0;//斜率
                            m_LineModelList[NcomboList2[mo2].influzzGan].LineKnewPtList = m_LineModelList[NcomboList2[mo2].influzzGan].LineKnewPtList.Distinct().ToList();
                        }
                        else if (Convert.ToInt32(NcomboList2[mo2].zzTag) == 6
                             || Convert.ToInt32(NcomboList2[mo2].zzTag) == 9)//定向支座Y
                        {
                            knewPt2 = new Point(NcomboList2[mo2].zzPoint.X, NcomboList2[mo2].zzPoint.Y);
                            m_LineModelList[NcomboList2[mo2].influzzGan].LineKnewPtList.Add(knewPt2);
                            m_LineModelList[NcomboList2[mo2].influzzGan].LineK = 0;//斜率
                            m_LineModelList[NcomboList2[mo2].influzzGan].LineKnewPtList = m_LineModelList[NcomboList2[mo2].influzzGan].LineKnewPtList.Distinct().ToList();
                        }
                    }
                }
            }
        }
        #endregion

        Point influPt12;//影响线起点
        Point influPt22;//影响线终点
        List<Point> influPtList2 = new List<Point>();//影响线点list

        #region 收集影响线点list元素2
        private void CollectInfluPt2()
        {
            m_LineModelList.Sort((l1, l2) => l1.Line_BeginPoint.X.CompareTo(l2.Line_BeginPoint.X));
            for (int no3 = 0; no3 < m_LineModelList.Count; no3++)//循环杆
            {
                if (no3 <= 50)
                {
                    if (m_LineModelList[no3].isDrew == false)
                    {
                        for (int p1 = 0; p1 < m_LineModelList[no3].LineKnewPtList.Count; p1++)//循环已知点list
                        {
                            if (m_LineModelList[no3].LineKnewPtList.Count >= 2)//同杆上已知点数为2
                            {
                                m_LineModelList[no3].LineK = (m_LineModelList[no3].LineKnewPtList[1].Y - m_LineModelList[no3].LineKnewPtList[0].Y) / (m_LineModelList[no3].LineKnewPtList[1].X - m_LineModelList[no3].LineKnewPtList[0].X);
                            }
                            if (m_LineModelList[no3].LineKnewPtList.Count >= 1)//同杆上已知点数为1
                            {
                                if (m_LineModelList[no3].LineK != 10000000)//斜率已知
                                {
                                    if (m_LineModelList[no3].LineKnewPtList[p1].X > m_LineModelList[no3].Line_BeginPoint.X
                                        && m_LineModelList[no3].LineKnewPtList[p1].X < m_LineModelList[no3].Line_EndPoint.X)//如果已知点不在杆端
                                    {
                                        influPt12.X = m_LineModelList[no3].Line_BeginPoint.X;
                                        influPt12.Y = m_LineModelList[no3].LineKnewPtList[0].Y - m_LineModelList[no3].LineK * (m_LineModelList[no3].LineKnewPtList[0].X - m_LineModelList[no3].Line_BeginPoint.X);
                                        influPt22.X = m_LineModelList[no3].Line_EndPoint.X;
                                        influPt22.Y = m_LineModelList[no3].LineKnewPtList[0].Y - m_LineModelList[no3].LineK * (m_LineModelList[no3].LineKnewPtList[0].X - m_LineModelList[no3].Line_EndPoint.X);
                                        influPtList2.Add(influPt12);
                                        influPtList2.Add(influPt22);//加入影响线点list
                                        m_LineModelList[no3].isDrew = true;
                                        foreach (LineModelClass line in m_LineModelList)
                                        {
                                            if (influPt12.X >= line.Line_BeginPoint.X && influPt12.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt12);
                                            }
                                            if (influPt22.X >= line.Line_BeginPoint.X && influPt22.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt22);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        break;
                                    }
                                    else if (m_LineModelList[no3].LineKnewPtList[p1].X == m_LineModelList[no3].Line_BeginPoint.X)//已知点在杆端起点
                                    {
                                        influPt12 = m_LineModelList[no3].LineKnewPtList[p1];
                                        influPt22.X = m_LineModelList[no3].Line_EndPoint.X;
                                        influPt22.Y = m_LineModelList[no3].LineKnewPtList[p1].Y - m_LineModelList[no3].LineK * (m_LineModelList[no3].LineKnewPtList[p1].X - m_LineModelList[no3].Line_EndPoint.X);
                                        influPtList2.Add(influPt12);
                                        influPtList2.Add(influPt22);
                                        m_LineModelList[no3].isDrew = true;
                                        foreach (LineModelClass line in m_LineModelList)
                                        {
                                            if (influPt12.X >= line.Line_BeginPoint.X && influPt12.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt12);
                                            }
                                            if (influPt22.X >= line.Line_BeginPoint.X && influPt22.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt22);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        break;
                                    }
                                    else if (m_LineModelList[no3].LineKnewPtList[p1].X == m_LineModelList[no3].Line_EndPoint.X)//已知点在杆端终点
                                    {
                                        influPt12.X = m_LineModelList[no3].Line_BeginPoint.X;
                                        influPt12.Y = m_LineModelList[no3].LineKnewPtList[p1].Y - m_LineModelList[no3].LineK * (m_LineModelList[no3].LineKnewPtList[p1].X - m_LineModelList[no3].Line_BeginPoint.X);
                                        influPt22 = m_LineModelList[no3].LineKnewPtList[p1];
                                        influPtList2.Add(influPt12);
                                        influPtList2.Add(influPt22);
                                        m_LineModelList[no3].isDrew = true;
                                        foreach (LineModelClass line in m_LineModelList)
                                        {
                                            if (influPt12.X >= line.Line_BeginPoint.X && influPt12.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt12);
                                            }
                                            if (influPt22.X >= line.Line_BeginPoint.X && influPt22.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt22);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("结果溢出!");
                }
            }
            for (int mo3 = 0; mo3 < m_LineModelList.Count; mo3++)
            {
                if (m_LineModelList[mo3].isDrew == false)
                {
                    CollectInfluPt2();
                    break;
                }
            }
        }
        #endregion

        #region 绘制影响线2
        List<Line> InfluLineList2 = new List<Line>();
        Line tempLine2 = new Line();
        List<Line> tempLine2List = new List<Line>();

        LineModelClass InfluClass2 = new LineModelClass();
        List<LineModelClass> InfluClassList2 = new List<LineModelClass>();

        List<Point> ShowinfluPtList2 = new List<Point>();
        Line ShowinfluLine2 = new Line();
        List<Line> ShowinfluLineList2 = new List<Line>();

        double ratio2 = 0;

        private void DrawInfluenceLine2()
        {
            influPtList2 = influPtList2.Distinct().ToList();//去除重复点
            #region 归一化
            for (int jo1 = 0; jo1 < influPtList2.Count; jo1++)
            {
                if (Math.Abs(influPtList2[jo1].Y - m_LineModelList[0].Line_BeginPoint.Y) > 80)
                {
                    ratio2 = 80 / (influPtList2[jo1].Y - m_LineModelList[0].Line_BeginPoint.Y);
                }
            }
            ratio2 = Math.Abs(ratio2);
            List<Point> tempList = new List<Point>();
            ShowinfluPtList2 = new List<Point>(influPtList2);
            if (ratio2 != 0)
            {
                for (int ko1 = 0; ko1 < ShowinfluPtList2.Count; ko1++)
                {
                    Point tempPt = new Point();
                    tempPt.X = ShowinfluPtList2[ko1].X;
                    tempPt.Y = (ShowinfluPtList2[ko1].Y - m_LineModelList[0].Line_BeginPoint.Y) * ratio2 + m_LineModelList[0].Line_BeginPoint.Y;
                    tempList.Add(tempPt);
                }
                ShowinfluPtList2.Clear();
                foreach (Point P in tempList)
                {
                    ShowinfluPtList2.Add(P);
                }
            }
            for (int so1 = 0; so1 < ShowinfluPtList2.Count - 1; so1++)
            {
                ShowinfluPtList2.Distinct().ToList();
                ShowinfluPtList2.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                ShowinfluLine2 = new Line();
                ShowinfluLine2.Stroke = Brushes.Green;
                ShowinfluLine2.StrokeThickness = 1;
                ShowinfluLine2.X1 = ShowinfluPtList2[so1].X;
                ShowinfluLine2.Y1 = ShowinfluPtList2[so1].Y;
                ShowinfluLine2.X2 = ShowinfluPtList2[so1 + 1].X;
                ShowinfluLine2.Y2 = ShowinfluPtList2[so1 + 1].Y;
                ShowinfluLineList2.Add(ShowinfluLine2);
                //can.Children.Add(ShowinfluLine);
            }
            #endregion
            for (int io3 = 0; io3 < influPtList2.Count - 1; io3++)
            {
                influPtList2 = influPtList2.Distinct().ToList();
                influPtList2.Sort((P1, P2) => P1.X.CompareTo(P2.X));
                influLine2 = new Line();
                influLine2.Stroke = Brushes.Green;
                influLine2.StrokeThickness = 1;
                influLine2.X1 = influPtList2[io3].X;
                influLine2.Y1 = influPtList2[io3].Y;
                influLine2.X2 = influPtList2[io3 + 1].X;
                influLine2.Y2 = influPtList2[io3 + 1].Y;
                InfluLineList2.Add(influLine2);

                InfluClass2.Line_BeginPoint.X = influLine2.X1;
                InfluClass2.Line_BeginPoint.Y = influLine2.Y1;
                InfluClass2.Line_EndPoint.X = influLine2.X2;
                InfluClass2.Line_EndPoint.Y = influLine2.Y2;
                InfluClass2.LineLength = InfluClass2.Line_EndPoint.Y - InfluClass2.Line_BeginPoint.Y;
                InfluClass2.LineK = (InfluClass2.Line_EndPoint.Y - InfluClass2.Line_BeginPoint.Y) / (InfluClass2.Line_EndPoint.X - InfluClass2.Line_BeginPoint.X);
                InfluClassList2.Add(InfluClass2);
                InfluClass2 = new LineModelClass();
            }
            //闭合影响线
            m_LineModelList.Sort((l1, l2) => l1.Line_BeginPoint.X.CompareTo(l2.Line_BeginPoint.X));
            if (ShowinfluPtList2[0].Y != m_LineModelList[0].Line_BeginPoint.Y)
            {
                tempLine2 = new Line();
                tempLine2.Stroke = Brushes.Green;
                tempLine2.StrokeThickness = 1;
                tempLine2.X1 = ShowinfluPtList2[0].X;
                tempLine2.Y1 = ShowinfluPtList2[0].Y;
                tempLine2.X2 = m_LineModelList[0].Line_BeginPoint.X;
                tempLine2.Y2 = m_LineModelList[0].Line_BeginPoint.Y;
                tempLine2List.Add(tempLine2);
            }
            if (ShowinfluPtList2[ShowinfluPtList2.Count - 1].Y != m_LineModelList[m_LineModelList.Count - 1].Line_EndPoint.Y)
            {
                tempLine2 = new Line();
                tempLine2.Stroke = Brushes.Green;
                tempLine2.StrokeThickness = 1;
                tempLine2.X1 = ShowinfluPtList2[ShowinfluPtList2.Count - 1].X;
                tempLine2.Y1 = ShowinfluPtList2[ShowinfluPtList2.Count - 1].Y;
                tempLine2.X2 = m_LineModelList[m_LineModelList.Count - 1].Line_EndPoint.X;
                tempLine2.Y2 = m_LineModelList[m_LineModelList.Count - 1].Line_EndPoint.Y;
                tempLine2List.Add(tempLine2);

            }
        }
        #endregion

        #endregion
        #endregion

        #region 支座反力偶清空函数
        private void FloBt_Click(object sender, RoutedEventArgs e)
        {
            comboList.Clear();
            zzComboBox.ItemsSource = null;
            zzCombomess();

            ClearInfluenceLine_FL();//清空，初始化
            BottomSectionClear();
            UserSectionClear();

            comboList2.Clear();
            zzComboBox2.ItemsSource = null;
            zzCombomess2();
            ClearInfluenceLine_FLO();
            BottomSectionClear();
            UserSectionClear();

            ClearInfluenceLine_WJ();
            BottomSectionClear();
            UserSectionClear();

            ClearInfluenceLine_JL();
            BottomSectionClear();
            UserSectionClear();

        }
        private void ClearInfluenceLine_FLO()//支座反力偶影响线清空函数
        {
            zzComboBox2.IsEnabled = true;
            fhID2 = -1;
            zzComboBox2.IsEnabled = true;
            NcomboList2.Clear();
            selectPt2 = new Point();
            influLine2 = new Line();
            knewPt2 = new Point();
            foreach (LineModelClass L1 in m_LineModelList)
            {
                L1.LineK = 10000000;
                L1.isDrew = false;
                L1.LineKnewPtNum = 0;
                L1.LineKnewPtList = new List<Point>();
                L1.isSelectBeam = false;
                L1.isSSBeam = false;
                influPt12 = new Point();
                influPt22 = new Point();
                influPtList2 = new List<Point>();
            }
            foreach (Image i in FLOChangeImageList)
            {
                can.Children.Remove(i);
            }
            FLOChangeImageList.Clear();
            FLOChangeImage = new Image();

            foreach (Image i1 in FLOImageList)
            {
                can.Children.Remove(i1);
            }
            FLOImageList.Clear();
            FLOImage = new Image();

            foreach (Image i2 in WarrowImageList)
            {
                can.Children.Remove(i2);
            }
            WarrowImageList.Clear();
            WarrowImage = new Image();

            foreach (Line L2 in InfluLineList2)
            {
                can.Children.Remove(L2);
            }
            InfluLineList2.Clear();

            foreach (Line tp2 in tempLine2List)
            {
                can.Children.Remove(tp2);
            }
            tempLine2 = new Line();
            tempLine2List.Clear();

            foreach (Line L3 in BottomInfluLineList2)
            {
                can.Children.Remove(L3);
            }
            BottomInfluLine2 = new Line();
            BottomInfluLineList2.Clear();

            can.Children.Remove(NoticeTB2);
            InfluClass2 = new LineModelClass();
            InfluClassList2.Clear();
            foreach (Line L in ShowinfluLineList2)
            {
                can.Children.Remove(L);
            }
            ShowinfluLineList2.Clear();
            ShowinfluPtList2.Clear();
            ShowinfluLine2 = new Line();

            ratio2 = 0;

        }

        #endregion

        #region 内力影响线

        #region 界面操作
        private bool isWanJuBt = false;
        private bool isJianLiBt = false;

        private void WanJuBt_Click(object sender, RoutedEventArgs e)
        {
            isWanJuBt = true;
            if (isWanJuBt == true)
            {

                JianLiBt.Background = Brushes.LightBlue;
            }
        }

        private void JianLiBt_Click(object sender, RoutedEventArgs e)
        {
            isJianLiBt = true;
            if (isJianLiBt == true)
            {
                JianLiBt.Background = Brushes.LightBlue;
            }
        }

        Point JLSelectPt = new Point();//剪力选择点
        Point WJSelectPt = new Point();//弯矩选择点
        Image JLImage = new Image();
        Image WJImage0 = new Image();
        List<Image> WJImageList = new List<Image>();
        List<Image> JLImageList = new List<Image>();

        private void can_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (InfluKindsTabctrl.SelectedItem == this.InfluKindsTabctrl.Items[1])//选中剪力影响线选项时
            {
                if (isJianLiBt == true)
                {
                    if (InternalForceTabctrl.SelectedItem == this.InternalForceTabctrl.Items[1])
                    {
                        for (int in1 = 0; in1 < m_LineModelList.Count; in1++)
                        {
                            if (pt.X > m_LineModelList[in1].Line_BeginPoint.X && in1 < m_LineModelList[in1].Line_EndPoint.X && pt.Y == m_LineModelList[0].Line_BeginPoint.Y)
                            {
                                JLSelectPt = pt;
                                JLImage = new Image();
                                JLImage.Source = new BitmapImage(new Uri(@"Images/JianLi.png", UriKind.Relative));
                                JLImage.Width = 30;
                                JLImage.Height = 40;
                                JLImage.SetValue(Canvas.LeftProperty, pt.X - 15);
                                JLImage.SetValue(Canvas.TopProperty, pt.Y + 20);
                                ScaleTransform JLImageScale = new ScaleTransform();
                                JLImageScale.ScaleY = -1;
                                JLImage.RenderTransform = JLImageScale;
                                JLImageList.Add(JLImage);
                                can.Children.Add(JLImage);
                                JLInfluenceLine_Cal();
                                MarkPoint();
                            }
                        }
                    }
                }
                else if (isWanJuBt == true)
                {
                    if (InternalForceTabctrl.SelectedItem == this.InternalForceTabctrl.Items[0])//选中弯矩影响线选项时
                    {
                        for (int in2 = 0; in2 < m_LineModelList.Count; in2++)
                        {
                            if (pt.X > m_LineModelList[in2].Line_BeginPoint.X && in2 < m_LineModelList[in2].Line_EndPoint.X && pt.Y == m_LineModelList[0].Line_BeginPoint.Y)
                            {
                                WJSelectPt = pt;
                                WJImage0 = new Image();
                                WJImage0.Source = new BitmapImage(new Uri(@"Images/WanJuJiaoLian.png", UriKind.Relative));
                                WJImage0.Width = 40;
                                WJImage0.Height = 40;
                                WJImage0.SetValue(Canvas.LeftProperty, pt.X - 20);
                                WJImage0.SetValue(Canvas.TopProperty, pt.Y + 20);
                                ScaleTransform WJImage0Scale = new ScaleTransform();
                                WJImage0Scale.ScaleY = -1;
                                WJImage0.RenderTransform = WJImage0Scale;
                                WJImageList.Add(WJImage0);
                                can.Children.Add(WJImage0);
                                WJInfluenceLine_Cal();
                                MarkPoint();
                            }
                        }
                    }
                }
            }
        }
        TextBlock NoticeTB3 = new TextBlock();
        TextBlock NoticeTB4 = new TextBlock();
        List<TextBlock> NoticeTB3List = new List<TextBlock>();
        List<TextBlock> NoticeTB4List = new List<TextBlock>();

        private void can_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isWanJuBt = false;
            isJianLiBt = false;
            JianLiBt.Background = Brushes.LightGray;
            WanJuBt.Background = Brushes.LightGray;
            //显示信息
            CurrentSelectTB.Text = "";
            CurrentSelectTB2.Text = "";
            CurrentSelectTB3.Text = "";
            CurrentSelectTB4.Text = "";
            for (int an1 = 0; an1 < m_LineModelList.Count; an1++)
            {
                if (WJSelectPt.X >= m_LineModelList[an1].Line_BeginPoint.X && WJSelectPt.X <= m_LineModelList[an1].Line_EndPoint.X && WJSelectPt.Y == m_LineModelList[an1].Line_BeginPoint.Y)
                {
                    CurrentSelectTB.Text = "第（" + (an1 + 1) + " ) 杆";
                    CurrentSelectTB2.Text = Convert.ToString((WJSelectPt.X - m_LineModelList[an1].Line_BeginPoint.X) / 20);
                    NoticeTB4 = new TextBlock();
                    NoticeTB4.Text = "当前求第（" + (an1 + 1) + " ) 杆的弯矩影响线";
                    NoticeTB4.Margin = new Thickness(340, y, 0, 0);
                    NoticeTB4.FontSize = 16;
                    ScaleTransform scale = new ScaleTransform();
                    scale.ScaleY = -1;
                    NoticeTB4.RenderTransform = scale;
                    NoticeTB4List.Add(NoticeTB4);
                    can.Children.Add(NoticeTB4);
                }
                if (JLSelectPt.X >= m_LineModelList[an1].Line_BeginPoint.X && JLSelectPt.X <= m_LineModelList[an1].Line_EndPoint.X && JLSelectPt.Y == m_LineModelList[an1].Line_BeginPoint.Y)
                {
                    CurrentSelectTB3.Text = "第（" + (an1 + 1) + " ) 杆";
                    CurrentSelectTB4.Text = Convert.ToString((JLSelectPt.X - m_LineModelList[an1].Line_BeginPoint.X) / 20);
                    NoticeTB3 = new TextBlock();
                    NoticeTB3.Text = "当前求第（" + (an1 + 1) + "）杆的剪力影响线";
                    NoticeTB3.Margin = new Thickness(340, y, 0, 0);
                    NoticeTB3.FontSize = 16;
                    ScaleTransform scale = new ScaleTransform();
                    scale.ScaleY = -1;
                    NoticeTB3.RenderTransform = scale;
                    NoticeTB3List.Add(NoticeTB3);
                    can.Children.Add(NoticeTB3);
                }
            }
        }

        #endregion

        #region 剪力影响线
        private void JLInfluenceLine_Cal()//剪力影响线计算主函数
        {
            if (JLNewLineList.Count == 0)
            {
                ComfirmSpecialBeam();
                BuildNewLineList();
                ConfirminfluzzGan_JL();
                BuildPtList_JL();
                CollectInfluPt_JL();
                DrawInfluenceLine_JL();
            }
        }

        #region 特殊情况判断
        List<zzClass> JiaoZZList = new List<zzClass>();
        Point SpecialPt1 = new Point(0, 0);
        Point SpecialPt2 = new Point(0, 0);
        Point SpecialPt3 = new Point(0, 0);
        double SpecialK = 10000000;
        double JiaoZZLength = 0;
        int jj = 0;
        private void ComfirmSpecialBeam()//判断特殊情况
        {
            int Gan = -1;
            int thisJ = -1;
            for (int ij1 = 0; ij1 < m_LineModelList.Count; ij1++)
            {
                if (JLSelectPt.X > m_LineModelList[ij1].Line_BeginPoint.X
                    && JLSelectPt.X < m_LineModelList[ij1].Line_EndPoint.X)
                {
                    Gan = ij1;
                }
            }
            for (int jj1 = 0; jj1 < zzList.Count; jj1++)
            {
                if (Convert.ToInt32(zzList[jj1].zzTag) == 7)//铰链
                {
                    if (zzList[jj1].zzPoint.X == m_LineModelList[Gan].Line_BeginPoint.X && zzList[jj1].zzPoint.X < JLSelectPt.X)//在起点
                    {
                        SpecialPt1.X = m_LineModelList[Gan].Line_BeginPoint.X;
                    }
                    if (zzList[jj1].zzPoint.X == m_LineModelList[Gan].Line_EndPoint.X && zzList[jj1].zzPoint.X > JLSelectPt.X)//在终点
                    {
                        SpecialPt2.X = m_LineModelList[Gan].Line_EndPoint.X;
                    }
                }
                if (Convert.ToInt32(zzList[jj1].zzTag) == 1
                        || Convert.ToInt32(zzList[jj1].zzTag) == 2)//铰支座
                {
                    if (SpecialPt1.X != 0)
                    {
                        if (zzList[jj1].zzPoint.X >= m_LineModelList[Gan].Line_BeginPoint.X
                         && zzList[jj1].zzPoint.X <= m_LineModelList[Gan].Line_EndPoint.X)// && SpecialPt3.X >JLSelectPt.X )//支座介于杆两端间
                        {
                            SpecialPt3.X = zzList[jj1].zzPoint.X;
                            thisJ = jj1;
                        }
                    }
                    if (SpecialPt2.X != 0)
                    {
                        if (zzList[jj1].zzPoint.X >= m_LineModelList[Gan].Line_BeginPoint.X
                         && zzList[jj1].zzPoint.X <= m_LineModelList[Gan].Line_EndPoint.X)//&& SpecialPt3.X < JLSelectPt.X)//支座介于杆两端间
                        {
                            SpecialPt3.X = zzList[jj1].zzPoint.X;
                            thisJ = jj1;
                        }
                    }
                    JiaoZZList.Add(zzList[jj1]);
                }
            }
            for (int kj1 = 0; kj1 < JiaoZZList.Count - 1; kj1++)
            {
                JiaoZZList.Sort((z1, z2) => z1.zzPoint.X.CompareTo(z2.zzPoint.X));
                if (JiaoZZList.Count >= 2)
                {
                    if (JiaoZZList[kj1].zzPoint.X >= m_LineModelList[Gan].Line_BeginPoint.X
                        && JiaoZZList[kj1].zzPoint.X <= m_LineModelList[Gan].Line_EndPoint.X
                        && JiaoZZList[kj1 + 1].zzPoint.X >= m_LineModelList[Gan].Line_BeginPoint.X
                        && JiaoZZList[kj1 + 1].zzPoint.X <= m_LineModelList[Gan].Line_EndPoint.X)
                    {
                        if (JLSelectPt.X > JiaoZZList[kj1].zzPoint.X && JLSelectPt.X < JiaoZZList[kj1 + 1].zzPoint.X)
                        {
                            m_LineModelList[Gan].isSSBeam = true;//是简支梁
                            JiaoZZLength = JiaoZZList[kj1 + 1].zzPoint.X - JiaoZZList[kj1].zzPoint.X;
                        }
                    }
                }
            }
            if (m_LineModelList[Gan].isSSBeam == true)
            {
                LeftSectionPt.X = JLSelectPt.X - 0.01;
                LeftSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y - 20 * (LeftSectionPt.X - m_LineModelList[Gan].Line_BeginPoint.X) / JiaoZZLength;
                RightSectionPt.X = JLSelectPt.X + 0.01;
                RightSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y + 20 * (m_LineModelList[Gan].Line_EndPoint.X - RightSectionPt.X) / JiaoZZLength;
                SpecialK = (LeftSectionPt.Y - m_LineModelList[Gan].Line_BeginPoint.Y) / (LeftSectionPt.X - m_LineModelList[Gan].Line_BeginPoint.X);
            }
            else if (SpecialPt3.X == 0 && SpecialPt2.X != 0 && SpecialPt1.X != 0)//x3=0,x2!=x0,x1!=0
            {
                LeftSectionPt.X = JLSelectPt.X - 0.01;
                LeftSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y - 20 * (LeftSectionPt.X - m_LineModelList[Gan].Line_BeginPoint.X) / (SpecialPt2.X - SpecialPt1.X);
                RightSectionPt.X = JLSelectPt.X + 0.01;
                RightSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y + 20 * (m_LineModelList[Gan].Line_EndPoint.X - RightSectionPt.X) / (SpecialPt2.X - SpecialPt1.X);
                SpecialK = (LeftSectionPt.Y - m_LineModelList[Gan].Line_BeginPoint.Y) / (LeftSectionPt.X - m_LineModelList[Gan].Line_BeginPoint.X);
                m_LineModelList[Gan].isSelectBeam = true;
            }
            else if (SpecialPt2.X == 0 && SpecialPt1.X != 0 && SpecialPt3.X != 0)//x2=0,x1!=0,x3!=0
            {
                LeftSectionPt.X = JLSelectPt.X - 0.01;
                LeftSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y - 20 * (LeftSectionPt.X - m_LineModelList[Gan].Line_BeginPoint.X) / (SpecialPt3.X - SpecialPt1.X);
                RightSectionPt.X = JLSelectPt.X + 0.01;
                RightSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y + 20 * (zzList[thisJ].zzPoint.X - RightSectionPt.X) / (SpecialPt3.X - SpecialPt1.X);
                SpecialK = (LeftSectionPt.Y - m_LineModelList[Gan].Line_BeginPoint.Y) / (LeftSectionPt.X - m_LineModelList[Gan].Line_BeginPoint.X);
                m_LineModelList[Gan].isSelectBeam = true;
                jj = 1;
            }
            else if (SpecialPt1.X == 0 && SpecialPt2.X != 0 && SpecialPt3.X != 0)//x1=0,x2!=0,x3!=0
            {
                LeftSectionPt.X = JLSelectPt.X - 0.01;
                LeftSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y - 20 * (LeftSectionPt.X - zzList[thisJ].zzPoint.X) / (SpecialPt2.X - SpecialPt3.X);
                RightSectionPt.X = JLSelectPt.X + 0.01;
                RightSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y + 20 * (m_LineModelList[Gan].Line_EndPoint.X - RightSectionPt.X) / (SpecialPt2.X - SpecialPt3.X);
                SpecialK = (LeftSectionPt.Y - zzList[thisJ].zzPoint.Y) / (LeftSectionPt.X - zzList[thisJ].zzPoint.X);
                m_LineModelList[Gan].isSelectBeam = true;
                jj = 1;
            }
            else if (SpecialPt2.X != 0 && SpecialPt1.X == SpecialPt3.X && SpecialPt1.X != 0 && SpecialPt3.X != 0)//x2!=0,x1=x3
            {
                LeftSectionPt.X = JLSelectPt.X - 0.01;
                LeftSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y - 20 * (LeftSectionPt.X - zzList[thisJ].zzPoint.X) / (SpecialPt2.X - SpecialPt1.X);
                RightSectionPt.X = JLSelectPt.X + 0.01;
                RightSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y + 20 * (m_LineModelList[Gan].Line_EndPoint.X - RightSectionPt.X) / (SpecialPt2.X - SpecialPt1.X);
                SpecialK = (LeftSectionPt.Y - zzList[thisJ].zzPoint.Y) / (LeftSectionPt.X - zzList[thisJ].zzPoint.X);
                m_LineModelList[Gan].isSelectBeam = true;
                jj = 1;
            }
            else if (SpecialPt1.X != 0 && SpecialPt2.X == SpecialPt3.X && SpecialPt2.X != 0 && SpecialPt3.X != 0)//x1!=0,x2=x3
            {
                LeftSectionPt.X = JLSelectPt.X - 0.01;
                LeftSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y - 20 * (LeftSectionPt.X - m_LineModelList[Gan].Line_BeginPoint.X) / (SpecialPt2.X - SpecialPt1.X);
                RightSectionPt.X = JLSelectPt.X + 0.01;
                RightSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y + 20 * (zzList[thisJ].zzPoint.X - RightSectionPt.X) / (SpecialPt2.X - SpecialPt1.X);
                SpecialK = (LeftSectionPt.Y - m_LineModelList[Gan].Line_BeginPoint.Y) / (LeftSectionPt.X - m_LineModelList[Gan].Line_BeginPoint.X);
                m_LineModelList[Gan].isSelectBeam = true;
                jj = 1;
            }
            else if (SpecialPt1.X != 0 && SpecialPt2.X != 0 && SpecialPt3.X != 0)//x1!=0,x2!=0,x3!=0
            {
                if (SpecialPt2.X != SpecialPt3.X || SpecialPt1.X != SpecialPt3.X)//x2!=x3 || x1!=x3
                {
                    if (JLSelectPt.X > SpecialPt1.X && JLSelectPt.X < SpecialPt3.X)//x1<x0<x3
                    {
                        LeftSectionPt.X = JLSelectPt.X - 0.01;
                        LeftSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y - 20 * (LeftSectionPt.X - m_LineModelList[Gan].Line_BeginPoint.X) / (SpecialPt3.X - SpecialPt1.X);
                        RightSectionPt.X = JLSelectPt.X + 0.01;
                        RightSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y + 20 * (zzList[thisJ].zzPoint.X - RightSectionPt.X) / (SpecialPt3.X - SpecialPt1.X);
                        SpecialK = (LeftSectionPt.Y - m_LineModelList[Gan].Line_BeginPoint.Y) / (LeftSectionPt.X - m_LineModelList[Gan].Line_BeginPoint.X);
                        m_LineModelList[Gan].isSelectBeam = true;
                        jj = 1;
                    }
                    if (JLSelectPt.X > SpecialPt3.X && JLSelectPt.X < SpecialPt2.X)//x3<x0<x2
                    {
                        LeftSectionPt.X = JLSelectPt.X - 0.01;
                        LeftSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y - 20 * (LeftSectionPt.X - zzList[thisJ].zzPoint.X) / (SpecialPt2.X - SpecialPt3.X);
                        RightSectionPt.X = JLSelectPt.X + 0.01;
                        RightSectionPt.Y = m_LineModelList[Gan].Line_BeginPoint.Y + 20 * (m_LineModelList[Gan].Line_EndPoint.X - RightSectionPt.X) / (SpecialPt2.X - SpecialPt3.X);
                        SpecialK = (LeftSectionPt.Y - zzList[thisJ].zzPoint.Y) / (LeftSectionPt.X - zzList[thisJ].zzPoint.X);
                        m_LineModelList[Gan].isSelectBeam = true;
                        jj = 1;
                    }
                }
            }
            else
            {
                LeftSectionPt.X = JLSelectPt.X - 0.01;
                RightSectionPt.X = JLSelectPt.X + 0.01;
                m_LineModelList[Gan].isSelectBeam = true;
                jj = 0;
            }
        }
        #endregion

        #region 选择点处断开，建立新的杆List
        List<Point> JLNewLinePtList = new List<Point>();
        LineModelClass JLNewLine = new LineModelClass();
        List<LineModelClass> JLNewLineList = new List<LineModelClass>();
        Point LeftSectionPt = new Point();
        Point RightSectionPt = new Point();//左右截面点
        int newGan = -1;
        private void BuildNewLineList()
        {
            //建立新点 list
            JLNewLinePtList.Add(LeftSectionPt);
            JLNewLinePtList.Add(RightSectionPt);
            foreach (LineModelClass L1 in m_LineModelList)
            {
                JLNewLinePtList.Add(L1.Line_BeginPoint);
                JLNewLinePtList.Add(L1.Line_EndPoint);
                JLNewLinePtList = JLNewLinePtList.Distinct().ToList();
            }
            JLNewLinePtList.Sort((P1, P2) => P1.X.CompareTo(P2.X));//排序
            //新杆list
            for (int ij2 = 0; ij2 < JLNewLinePtList.Count - 1; ij2++)
            {
                JLNewLine.Line_BeginPoint = JLNewLinePtList[ij2];
                JLNewLine.Line_EndPoint = JLNewLinePtList[ij2 + 1];
                JLNewLine.LineLength = JLNewLinePtList[ij2 + 1].X - JLNewLinePtList[ij2].X;
                JLNewLine.LineK = 10000000;
                JLNewLine.isDrew = false;
                JLNewLine.LineKnewPtNum = 0;
                JLNewLineList.Add(JLNewLine);
                JLNewLine = new LineModelClass();
            }
            for (int jj2 = 0; jj2 < JLNewLineList.Count; jj2++)
            {
                if (JLNewLineList[jj2].LineLength <= 0.02)
                {
                    JLNewLineList.RemoveAt(jj2);
                }
                if (JLNewLineList[jj2].Line_BeginPoint.X < LeftSectionPt.X && LeftSectionPt.X <= JLNewLineList[jj2].Line_EndPoint.X)
                {
                    JLNewLineList[jj2].isSelectBeam = true;
                    newGan = jj2;
                }
                if (JLNewLineList[jj2].Line_BeginPoint.X <= RightSectionPt.X && RightSectionPt.X < JLNewLineList[jj2].Line_EndPoint.X)
                {
                    JLNewLineList[jj2].isSelectBeam = true;
                }
            }
        }
        #endregion

        #region 确定支座杆号（剪力）
        List<zzClass> JLzzList = new List<zzClass>();
        private void ConfirminfluzzGan_JL()
        {
            JLzzList = new List<zzClass>(zzList);
            for (int mj1 = 0; mj1 < JLzzList.Count; mj1++)//循环支座
            {
                for (int nj1 = 0; nj1 < JLNewLineList.Count; nj1++)//循环杆
                {
                    if (JLNewLineList[nj1].Line_BeginPoint.X <= JLzzList[mj1].zzPoint.X
                        && JLzzList[mj1].zzPoint.X <= JLNewLineList[nj1].Line_EndPoint.X)
                    {
                        JLzzList[mj1].influzzGan = nj1;
                        break;
                    }
                }
            }
        }
        #endregion

        #region 建立已知点集(剪力)
        Point knewPt3 = new Point();
        private void BuildPtList_JL()//建立已知点集(剪力)
        {
            for (int mj2 = 0; mj2 < JLzzList.Count; mj2++)//循环支座
            {
                if (Convert.ToInt32(JLzzList[mj2].zzTag) == 1
                         || Convert.ToInt32(JLzzList[mj2].zzTag) == 2)//固定铰支座、活动铰支座
                {
                    knewPt3 = new Point(JLzzList[mj2].zzPoint.X, JLzzList[mj2].zzPoint.Y);
                    JLNewLineList[JLzzList[mj2].influzzGan].LineKnewPtList.Add(knewPt3);
                    JLNewLineList[JLzzList[mj2].influzzGan].LineKnewPtList = JLNewLineList[JLzzList[mj2].influzzGan].LineKnewPtList.Distinct().ToList();
                }
                else if (Convert.ToInt32(JLzzList[mj2].zzTag) == 3
                        || Convert.ToInt32(JLzzList[mj2].zzTag) == 4)//固定端
                {
                    knewPt3 = new Point(JLzzList[mj2].zzPoint.X, JLzzList[mj2].zzPoint.Y);
                    JLNewLineList[JLzzList[mj2].influzzGan].LineKnewPtList.Add(knewPt3);
                    JLNewLineList[JLzzList[mj2].influzzGan].LineK = 0;//斜率
                    JLNewLineList[JLzzList[mj2].influzzGan].LineKnewPtList = JLNewLineList[JLzzList[mj2].influzzGan].LineKnewPtList.Distinct().ToList();
                }
                else if (Convert.ToInt32(JLzzList[mj2].zzTag) == 6
                         || Convert.ToInt32(JLzzList[mj2].zzTag) == 9)//定向支座Y
                {
                    knewPt3 = new Point(JLzzList[mj2].zzPoint.X, JLzzList[mj2].zzPoint.Y);
                    JLNewLineList[JLzzList[mj2].influzzGan].LineKnewPtList.Add(knewPt3);
                    JLNewLineList[JLzzList[mj2].influzzGan].LineK = 0;//斜率
                    JLNewLineList[JLzzList[mj2].influzzGan].LineKnewPtList = JLNewLineList[JLzzList[mj2].influzzGan].LineKnewPtList.Distinct().ToList();
                }
                else if (Convert.ToInt32(JLzzList[mj2].zzTag) == 5
                         || Convert.ToInt32(JLzzList[mj2].zzTag) == 8)//定向支座X
                {
                    JLNewLineList[JLzzList[mj2].influzzGan].LineK = 0;//斜率
                    JLNewLineList[JLzzList[mj2].influzzGan].LineKnewPtList = JLNewLineList[JLzzList[mj2].influzzGan].LineKnewPtList.Distinct().ToList();
                }
            }
            //已知点集加入截面点
            for (int ij3 = 0; ij3 < JLNewLineList.Count; ij3++)
            {
                if (LeftSectionPt.X > JLNewLineList[ij3].Line_BeginPoint.X && LeftSectionPt.X <= JLNewLineList[ij3].Line_EndPoint.X && LeftSectionPt.Y != 0 && jj == 0)
                {
                    JLNewLineList[ij3].LineKnewPtList.Add(LeftSectionPt);
                    JLNewLineList[ij3].LineK = SpecialK;
                }
                if (RightSectionPt.X >= JLNewLineList[ij3].Line_BeginPoint.X && RightSectionPt.X < JLNewLineList[ij3].Line_EndPoint.X && RightSectionPt.Y != 0 && jj == 0)
                {
                    JLNewLineList[ij3].LineKnewPtList.Add(RightSectionPt);
                    JLNewLineList[ij3].LineK = SpecialK;
                }
            }
        }
        #endregion

        #region 收集影响线点list元素(剪力)
        Point influPt3 = new Point();
        Point influPt4 = new Point();
        List<Point> influPtList3 = new List<Point>();
        int CalCount = 0;
        double A = 0;
        double B = 0;
        private void CollectInfluPt_JL()
        {
            JLNewLineList.Sort((L1, L2) => L1.Line_BeginPoint.X.CompareTo(L2.Line_BeginPoint.X));
            for (int nj2 = 0; nj2 < JLNewLineList.Count; nj2++)//循环杆
            {
                if (nj2 <= 50)
                {
                    if (JLNewLineList[nj2].isDrew == false)//此杆未画影响线
                    {
                        for (int p3 = 0; p3 < JLNewLineList[nj2].LineKnewPtList.Count; p3++)//循环已知点list
                        {
                            //被选杆左右段斜率相等
                            if (JLNewLineList[newGan].LineK != 10000000)
                            {
                                JLNewLineList[newGan + 1].LineK = JLNewLineList[newGan].LineK;
                            }
                            else if (JLNewLineList[newGan + 1].LineK != 10000000)
                            {
                                JLNewLineList[newGan].LineK = JLNewLineList[newGan + 1].LineK;
                            }
                            if (JLNewLineList[nj2].LineKnewPtList.Count >= 2)//同杆上已知点数为2
                            {
                                JLNewLineList[nj2].LineK = (JLNewLineList[nj2].LineKnewPtList[1].Y - JLNewLineList[nj2].LineKnewPtList[0].Y) / (JLNewLineList[nj2].LineKnewPtList[1].X - JLNewLineList[nj2].LineKnewPtList[0].X);
                                if (nj2 == newGan)
                                {
                                    JLNewLineList[newGan + 1].LineK = JLNewLineList[newGan].LineK;
                                }
                                if (nj2 == newGan + 1)
                                {
                                    JLNewLineList[newGan].LineK = JLNewLineList[newGan + 1].LineK;
                                }
                            }
                            if (JLNewLineList[nj2].LineKnewPtList.Count >= 1)//同杆上已知点数为1
                            {
                                if (JLNewLineList[nj2].LineK != 10000000)//斜率已知
                                {
                                    if (JLNewLineList[nj2].LineKnewPtList[p3].X > JLNewLineList[nj2].Line_BeginPoint.X
                                        && JLNewLineList[nj2].LineKnewPtList[p3].X < JLNewLineList[nj2].Line_EndPoint.X)//如果已知点不在杆端
                                    {
                                        influPt3.X = JLNewLineList[nj2].Line_BeginPoint.X;
                                        influPt3.Y = JLNewLineList[nj2].LineKnewPtList[0].Y - JLNewLineList[nj2].LineK * (JLNewLineList[nj2].LineKnewPtList[0].X - JLNewLineList[nj2].Line_BeginPoint.X);
                                        influPt4.X = JLNewLineList[nj2].Line_EndPoint.X;
                                        influPt4.Y = JLNewLineList[nj2].LineKnewPtList[0].Y - JLNewLineList[nj2].LineK * (JLNewLineList[nj2].LineKnewPtList[0].X - JLNewLineList[nj2].Line_EndPoint.X);
                                        influPtList3.Add(influPt3);
                                        influPtList3.Add(influPt4);//加入影响线点list
                                        JLNewLineList[nj2].isDrew = true;
                                        A = influPt3.Y;
                                        B = influPt4.Y;
                                        if (nj2 == newGan)
                                        {
                                            int samePtNum = 0;
                                            Point tPt = new Point();
                                            tPt.X = JLNewLineList[nj2 + 1].Line_BeginPoint.X;
                                            tPt.Y = 20 + B;
                                            for (int ij4 = 0; ij4 < JLNewLineList[nj2 + 1].LineKnewPtList.Count; ij4++)
                                            {
                                                if (tPt.X == JLNewLineList[nj2 + 1].LineKnewPtList[ij4].X)
                                                {
                                                    samePtNum++;
                                                }
                                            }
                                            if (samePtNum == 0)
                                            {
                                                JLNewLineList[nj2 + 1].LineKnewPtList.Add(tPt);
                                                samePtNum = 0;
                                            }
                                            JLNewLineList[nj2 + 1].LineKnewPtList = JLNewLineList[nj2 + 1].LineKnewPtList.Distinct().ToList();
                                        }
                                        if (nj2 == newGan + 1)
                                        {
                                            int samePtNum = 0;
                                            Point tPt = new Point();
                                            tPt.X = JLNewLineList[nj2 - 1].Line_BeginPoint.X;
                                            tPt.Y = A - 20;
                                            for (int ij5 = 0; ij5 < JLNewLineList[nj2 - 1].LineKnewPtList.Count; ij5++)
                                            {
                                                if (tPt.X == JLNewLineList[nj2 - 1].LineKnewPtList[ij5].X)
                                                {
                                                    samePtNum++;
                                                }
                                            }
                                            if (samePtNum == 0)
                                            {
                                                JLNewLineList[nj2 - 1].LineKnewPtList.Add(tPt);
                                                samePtNum = 0;
                                            }
                                            JLNewLineList[nj2 - 1].LineKnewPtList = JLNewLineList[nj2 - 1].LineKnewPtList.Distinct().ToList();
                                        }
                                        foreach (LineModelClass line in JLNewLineList)
                                        {
                                            if (influPt3.X >= line.Line_BeginPoint.X && influPt3.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt3);
                                            }
                                            if (influPt4.X >= line.Line_BeginPoint.X && influPt4.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt4);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        CalCount++;
                                        break;
                                    }
                                    else if (JLNewLineList[nj2].LineKnewPtList[p3].X == JLNewLineList[nj2].Line_BeginPoint.X)//已知点在杆端起点
                                    {
                                        influPt3 = JLNewLineList[nj2].LineKnewPtList[p3];
                                        influPt4.X = JLNewLineList[nj2].Line_EndPoint.X;
                                        influPt4.Y = JLNewLineList[nj2].LineKnewPtList[p3].Y - JLNewLineList[nj2].LineK * (JLNewLineList[nj2].LineKnewPtList[p3].X - JLNewLineList[nj2].Line_EndPoint.X);
                                        influPtList3.Add(influPt3);
                                        influPtList3.Add(influPt4);
                                        A = influPt3.Y;
                                        B = influPt4.Y;
                                        JLNewLineList[nj2].isDrew = true;
                                        if (nj2 == newGan)
                                        {
                                            int samePtNum = 0;
                                            Point tPt = new Point();
                                            tPt.X = JLNewLineList[nj2 + 1].Line_BeginPoint.X;
                                            tPt.Y = 20 + B;
                                            for (int ij6 = 0; ij6 < JLNewLineList[nj2 + 1].LineKnewPtList.Count; ij6++)
                                            {
                                                if (tPt.X == JLNewLineList[nj2 + 1].LineKnewPtList[ij6].X)
                                                {
                                                    samePtNum++;
                                                }
                                            }
                                            if (samePtNum == 0)
                                            {
                                                JLNewLineList[nj2 + 1].LineKnewPtList.Add(tPt);
                                                samePtNum = 0;
                                            }
                                            JLNewLineList[nj2 + 1].LineKnewPtList = JLNewLineList[nj2 + 1].LineKnewPtList.Distinct().ToList();
                                        }
                                        if (nj2 == newGan + 1)
                                        {
                                            int samePtNum = 0;
                                            Point tPt = new Point();
                                            tPt.X = JLNewLineList[nj2 - 1].Line_BeginPoint.X;
                                            tPt.Y = A - 20;
                                            for (int ij7 = 0; ij7 < JLNewLineList[nj2 - 1].LineKnewPtList.Count; ij7++)
                                            {
                                                if (tPt.X == JLNewLineList[nj2 - 1].LineKnewPtList[ij7].X)
                                                {
                                                    samePtNum++;
                                                }
                                            }
                                            if (samePtNum == 0)
                                            {
                                                JLNewLineList[nj2 - 1].LineKnewPtList.Add(tPt);
                                                samePtNum = 0;
                                            }
                                            JLNewLineList[nj2 - 1].LineKnewPtList = JLNewLineList[nj2 - 1].LineKnewPtList.Distinct().ToList();
                                        }
                                        foreach (LineModelClass line in JLNewLineList)
                                        {
                                            if (influPt3.X >= line.Line_BeginPoint.X && influPt3.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt3);
                                            }
                                            if (influPt4.X >= line.Line_BeginPoint.X && influPt4.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt4);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        CalCount++;
                                        break;
                                    }
                                    else if (JLNewLineList[nj2].LineKnewPtList[p3].X == JLNewLineList[nj2].Line_EndPoint.X)//已知点在杆端终点
                                    {
                                        influPt3.X = JLNewLineList[nj2].Line_BeginPoint.X;
                                        influPt3.Y = JLNewLineList[nj2].LineKnewPtList[p3].Y - JLNewLineList[nj2].LineK * (JLNewLineList[nj2].LineKnewPtList[p3].X - JLNewLineList[nj2].Line_BeginPoint.X);
                                        influPt4 = JLNewLineList[nj2].LineKnewPtList[p3];
                                        influPtList3.Add(influPt3);
                                        influPtList3.Add(influPt4);
                                        A = influPt3.Y;
                                        B = influPt4.Y;
                                        JLNewLineList[nj2].isDrew = true;
                                        if (nj2 == newGan)
                                        {
                                            int samePtNum = 0;
                                            Point tPt = new Point();
                                            tPt.X = JLNewLineList[nj2 + 1].Line_BeginPoint.X;
                                            tPt.Y = 20 + B;
                                            for (int ij8 = 0; ij8 < JLNewLineList[nj2 + 1].LineKnewPtList.Count; ij8++)
                                            {
                                                if (tPt.X == JLNewLineList[nj2 + 1].LineKnewPtList[ij8].X)
                                                {
                                                    samePtNum++;
                                                }
                                            }
                                            if (samePtNum == 0)
                                            {
                                                JLNewLineList[nj2 + 1].LineKnewPtList.Add(tPt);
                                                samePtNum = 0;
                                            }
                                            JLNewLineList[nj2 + 1].LineKnewPtList = JLNewLineList[nj2 + 1].LineKnewPtList.Distinct().ToList();
                                        }
                                        if (nj2 == newGan + 1)
                                        {
                                            int samePtNum = 0;
                                            Point tPt = new Point();
                                            tPt.X = JLNewLineList[nj2 - 1].Line_BeginPoint.X;
                                            tPt.Y = A - 20;
                                            for (int ij9 = 0; ij9 < JLNewLineList[nj2 - 1].LineKnewPtList.Count; ij9++)
                                            {
                                                if (tPt.X == JLNewLineList[nj2 - 1].LineKnewPtList[ij9].X)
                                                {
                                                    samePtNum++;
                                                }
                                            }
                                            if (samePtNum == 0)
                                            {
                                                JLNewLineList[nj2 - 1].LineKnewPtList.Add(tPt);
                                                samePtNum = 0;
                                            }
                                            JLNewLineList[nj2 - 1].LineKnewPtList = JLNewLineList[nj2 - 1].LineKnewPtList.Distinct().ToList();
                                        }
                                        foreach (LineModelClass line in JLNewLineList)
                                        {
                                            if (influPt3.X >= line.Line_BeginPoint.X && influPt3.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt3);
                                            }
                                            if (influPt4.X >= line.Line_BeginPoint.X && influPt4.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt4);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        CalCount++;
                                        break;
                                    }
                                }
                                else
                                {
                                    CalCount++;
                                    if (CalCount > JLNewLineList.Count)
                                    {
                                        for (int ij10 = 0; ij10 < JLNewLineList.Count; ij10++)
                                        {
                                            if (jj == 1)
                                            {
                                                JLNewLineList[newGan].LineKnewPtList.Add(LeftSectionPt);
                                                JLNewLineList[newGan].LineKnewPtList = JLNewLineList[newGan].LineKnewPtList.Distinct().ToList();
                                                JLNewLineList[newGan + 1].LineKnewPtList.Add(RightSectionPt);
                                                JLNewLineList[newGan + 1].LineKnewPtList = JLNewLineList[newGan + 1].LineKnewPtList.Distinct().ToList();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("结果溢出!");
                }
            }
            for (int jj3 = 0; jj3 < JLNewLineList.Count; jj3++)
            {
                if (JLNewLineList[jj3].isDrew == false)
                {
                    CollectInfluPt_JL();
                    break;
                }
            }
        }
        #endregion

        #region 绘制影响线（剪力）
        private Line influLine3 = new Line();
        Line tempLine3 = new Line();

        List<Line> tempLine3List = new List<Line>();
        List<Line> InfluLineList3 = new List<Line>();

        Point JLLeftSectionPt = new Point();
        Point JLRightSectionPt = new Point();

        LineModelClass InfluClass3 = new LineModelClass();
        List<LineModelClass> InfluClassList3 = new List<LineModelClass>();

        List<Point> ShowinfluPtList3 = new List<Point>();
        Line ShowinfluLine3 = new Line();
        List<Line> ShowinfluLineList3 = new List<Line>();

        double ratio3 = 0;

        private void DrawInfluenceLine_JL()
        {
            influPtList3 = influPtList3.Distinct().ToList();//去除重复点
            #region 归一化
            for (int jj4 = 0; jj4 < influPtList3.Count; jj4++)
            {
                if (Math.Abs(influPtList3[jj4].Y - JLNewLineList[0].Line_BeginPoint.Y) > 80)
                {
                    ratio3 = 80 / (influPtList3[jj4].Y - JLNewLineList[0].Line_BeginPoint.Y);
                }
            }
            ratio3 = Math.Abs(ratio3);
            List<Point> tempList = new List<Point>();
            ShowinfluPtList3 = new List<Point>(influPtList3);
            if (ratio3 != 0)
            {
                for (int kj2 = 0; kj2 < ShowinfluPtList3.Count; kj2++)
                {
                    Point tempPt = new Point();
                    tempPt.X = ShowinfluPtList3[kj2].X;
                    tempPt.Y = (ShowinfluPtList3[kj2].Y - JLNewLineList[0].Line_BeginPoint.Y) * ratio3 + JLNewLineList[0].Line_BeginPoint.Y;
                    tempList.Add(tempPt);
                    // ShowinfluPtList3.RemoveAt(k);
                    // k = 0;
                }
                ShowinfluPtList3.Clear();
                foreach (Point P in tempList)
                {
                    ShowinfluPtList3.Add(P);
                }
            }
            for (int sj1 = 0; sj1 < ShowinfluPtList3.Count - 1; sj1++)
            {
                ShowinfluPtList3.Distinct().ToList();
                ShowinfluPtList3.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                ShowinfluLine3 = new Line();
                ShowinfluLine3.Stroke = Brushes.Green;
                ShowinfluLine3.StrokeThickness = 1;
                ShowinfluLine3.X1 = ShowinfluPtList3[sj1].X;
                ShowinfluLine3.Y1 = ShowinfluPtList3[sj1].Y;
                ShowinfluLine3.X2 = ShowinfluPtList3[sj1 + 1].X;
                ShowinfluLine3.Y2 = ShowinfluPtList3[sj1 + 1].Y;
                ShowinfluLineList3.Add(ShowinfluLine3);
                //can.Children.Add(ShowinfluLine);
            }
            #endregion

            for (int ij11 = 0; ij11 < influPtList3.Count - 1; ij11++)
            {
                influPtList3 = influPtList3.Distinct().ToList();
                influPtList3.Sort((P1, P2) => P1.X.CompareTo(P2.X));
                influLine3 = new Line();
                influLine3.Stroke = Brushes.Green;
                influLine3.StrokeThickness = 1;
                influLine3.X1 = influPtList3[ij11].X;
                influLine3.Y1 = influPtList3[ij11].Y;
                influLine3.X2 = influPtList3[ij11 + 1].X;
                influLine3.Y2 = influPtList3[ij11 + 1].Y;
                InfluLineList3.Add(influLine3);

                InfluClass3.Line_BeginPoint.X = influLine3.X1;
                InfluClass3.Line_BeginPoint.Y = influLine3.Y1;
                InfluClass3.Line_EndPoint.X = influLine3.X2;
                InfluClass3.Line_EndPoint.Y = influLine3.Y2;
                InfluClass3.LineLength = InfluClass3.Line_EndPoint.Y - InfluClass3.Line_BeginPoint.Y;
                InfluClass3.LineK = (InfluClass3.Line_EndPoint.Y - InfluClass3.Line_BeginPoint.Y) / (InfluClass3.Line_EndPoint.X - InfluClass3.Line_BeginPoint.X);
                InfluClassList3.Add(InfluClass3);
                InfluClass3 = new LineModelClass();
            }
            //闭合影响线
            JLNewLineList.Sort((l1, l2) => l1.Line_BeginPoint.X.CompareTo(l2.Line_BeginPoint.X));
            if (ShowinfluPtList3[0].Y != JLNewLineList[0].Line_BeginPoint.Y)
            {
                tempLine3 = new Line();
                tempLine3.Stroke = Brushes.Green;
                tempLine3.StrokeThickness = 1;
                tempLine3.X1 = ShowinfluPtList3[0].X;
                tempLine3.Y1 = ShowinfluPtList3[0].Y;
                tempLine3.X2 = JLNewLineList[0].Line_BeginPoint.X;
                tempLine3.Y2 = JLNewLineList[0].Line_BeginPoint.Y;
                tempLine3List.Add(tempLine3);
                // can.Children.Add(tempLine3);
            }
            if (ShowinfluPtList3[ShowinfluPtList3.Count - 1].Y != JLNewLineList[JLNewLineList.Count - 1].Line_EndPoint.Y)
            {
                tempLine3 = new Line();
                tempLine3.Stroke = Brushes.Green;
                tempLine3.StrokeThickness = 1;
                tempLine3.X1 = ShowinfluPtList3[ShowinfluPtList3.Count - 1].X;
                tempLine3.Y1 = ShowinfluPtList3[ShowinfluPtList3.Count - 1].Y;
                tempLine3.X2 = JLNewLineList[JLNewLineList.Count - 1].Line_EndPoint.X;
                tempLine3.Y2 = JLNewLineList[JLNewLineList.Count - 1].Line_EndPoint.Y;
                tempLine3List.Add(tempLine3);

                //can.Children.Add(tempLine3);
            }
            for (int aj1 = 0; aj1 < influPtList3.Count; aj1++)
            {
                if (influPtList3[aj1].X == JLSelectPt.X - 0.01)
                {
                    JLLeftSectionPt.X = influPtList3[aj1].X;
                    JLLeftSectionPt.Y = influPtList3[aj1].Y;
                }
                if (influPtList3[aj1].X == JLSelectPt.X + 0.01)
                {
                    JLRightSectionPt.X = influPtList3[aj1].X;
                    JLRightSectionPt.Y = influPtList3[aj1].Y;
                }
            }
            for (int bj1 = 0; bj1 < InfluClassList3.Count; bj1++)
            {
                if (InfluClassList3[bj1].Line_BeginPoint.X < JLSelectPt.X - 0.01 && JLSelectPt.X - 0.01 <= InfluClassList3[bj1].Line_EndPoint.X)
                {
                    InfluClassList3[bj1].isSelectBeam = true;
                }
                if (InfluClassList3[bj1].Line_BeginPoint.X <= JLSelectPt.X + 0.01 && JLSelectPt.X + 0.01 < InfluClassList3[bj1].Line_EndPoint.X)
                {
                    InfluClassList3[bj1].isSelectBeam = true;
                }
            }
        }
        #endregion

        #endregion

        #region 剪力影响线清空函数
        private void JianLiResetBt_Click(object sender, RoutedEventArgs e)
        {
            comboList.Clear();
            zzComboBox.ItemsSource = null;
            zzCombomess();

            ClearInfluenceLine_FL();//清空，初始化
            BottomSectionClear();
            UserSectionClear();

            comboList2.Clear();
            zzComboBox2.ItemsSource = null;
            zzCombomess2();
            ClearInfluenceLine_FLO();
            BottomSectionClear();
            UserSectionClear();

            ClearInfluenceLine_WJ();
            BottomSectionClear();
            UserSectionClear();

            ClearInfluenceLine_JL();
            BottomSectionClear();
            UserSectionClear();
        }
        private void ClearInfluenceLine_JL()//剪力影响线清空函数
        {
            isJianLiBt = false;
            JLSelectPt = new Point();
            foreach (Image I in JLImageList)
            {
                can.Children.Remove(I);
            }
            JLImageList.Clear();
            CurrentSelectTB3.Text = "";
            CurrentSelectTB4.Text = "";
            JiaoZZList.Clear();
            SpecialPt1 = new Point(0, 0);
            SpecialPt2 = new Point(0, 0);
            SpecialPt3 = new Point(0, 0);
            SpecialK = 10000000;
            JiaoZZLength = 0;
            jj = 0;
            JLNewLinePtList.Clear();
            JLNewLine = new LineModelClass();
            JLNewLineList.Clear();
            LeftSectionPt = new Point();
            RightSectionPt = new Point();
            newGan = -1;
            JLzzList.Clear();
            knewPt3 = new Point();
            influPt3 = new Point();
            influPt4 = new Point();
            influPtList3.Clear();
            CalCount = 0;
            A = 0;
            B = 0;
            influLine3 = new Line();
            foreach (Line L1 in InfluLineList3)
            {
                can.Children.Remove(L1);
            }

            foreach (Line tp3 in tempLine3List)
            {
                can.Children.Remove(tp3);
            }
            tempLine3 = new Line();
            tempLine3List.Clear();
            InfluLineList3.Clear();
            foreach (Line L2 in BottomInfluLineList3)
            {
                can.Children.Remove(L2);
            }
            BottomInfluLine3 = new Line();
            BottomInfluLineList3.Clear();
            can.Children.Remove(BottomTempLine3);
            BottomTempLine3 = new Line();
            foreach (TextBlock T3 in BottomVDTextList3)
            {
                can.Children.Remove(T3);
            }
            BottomVDTextList3.Clear();
            can.Children.Remove(BottomJLImage);

            foreach (TextBlock tb3 in NoticeTB3List)
            {
                can.Children.Remove(tb3);
            }
            JLLeftSectionPt = new Point();
            JLRightSectionPt = new Point();
            InfluClass3 = new LineModelClass();
            InfluClassList3.Clear();

            foreach (Line L in ShowinfluLineList3)
            {
                can.Children.Remove(L);
            }
            ShowinfluLineList3.Clear();
            ShowinfluPtList3.Clear();
            ShowinfluLine3 = new Line();

            ratio3 = 0;
        }

        #endregion

        #region 弯矩影响线
        private void WJInfluenceLine_Cal()//弯矩影响线计算主函数
        {
            if (WJNewLineList.Count == 0)
            {
                ComfirmSpecialBeam2();
                BuildNewLineLis2();
                ConfirminfluzzGan_WJ();
                BuildPtList_WJ();
                CollectInfluPt_WJ();
                DrawInfluenceLine_WJ();
            }
        }

        #region 特殊情况判断（弯矩）
        List<zzClass> JiaoZZList2 = new List<zzClass>();
        Point SpecialPt12 = new Point(0, 0);
        Point SpecialPt22 = new Point(0, 0);
        Point SpecialPt32 = new Point(0, 0);
        int jj2 = 0;
        double JiaoZZLength2 = 0;
        Point ExtraPt = new Point();
        private void ComfirmSpecialBeam2()
        {
            int Gan2 = -1;
            int thiisJ2 = -1;
            for (int iw1 = 0; iw1 < m_LineModelList.Count; iw1++)
            {
                if (WJSelectPt.X > m_LineModelList[iw1].Line_BeginPoint.X
                    && WJSelectPt.X < m_LineModelList[iw1].Line_EndPoint.X)
                {
                    Gan2 = iw1;
                }
            }
            for (int jw1 = 0; jw1 < zzList.Count; jw1++)
            {
                if (Convert.ToInt32(zzList[jw1].zzTag) == 7)//铰链
                {
                    if (zzList[jw1].zzPoint.X == m_LineModelList[Gan2].Line_BeginPoint.X && zzList[jw1].zzPoint.X < WJSelectPt.X)//在起点
                    {
                        SpecialPt12.X = m_LineModelList[Gan2].Line_BeginPoint.X;
                    }
                    if (zzList[jw1].zzPoint.X == m_LineModelList[Gan2].Line_EndPoint.X && zzList[jw1].zzPoint.X > WJSelectPt.X)//在终点
                    {
                        SpecialPt22.X = m_LineModelList[Gan2].Line_EndPoint.X;
                    }
                }
                if (Convert.ToInt32(zzList[jw1].zzTag) == 1
                    || Convert.ToInt32(zzList[jw1].zzTag) == 2)//铰支座
                {
                    if (SpecialPt12.X != 0)
                    {
                        if (zzList[jw1].zzPoint.X >= m_LineModelList[Gan2].Line_BeginPoint.X
                            && zzList[jw1].zzPoint.X <= m_LineModelList[Gan2].Line_EndPoint.X)//支座介于杆两端间
                        {
                            SpecialPt32.X = zzList[jw1].zzPoint.X;
                            thiisJ2 = jw1;
                        }
                    }
                    if (SpecialPt22.X != 0)
                    {
                        if (zzList[jw1].zzPoint.X >= m_LineModelList[Gan2].Line_BeginPoint.X
                            && zzList[jw1].zzPoint.X <= m_LineModelList[Gan2].Line_EndPoint.X)//支座介于杆两端间
                        {
                            SpecialPt32.X = zzList[jw1].zzPoint.X;
                            thiisJ2 = jw1;
                        }
                    }
                    JiaoZZList2.Add(zzList[jw1]);
                }
            }
            for (int kw1 = 0; kw1 < JiaoZZList2.Count - 1; kw1++)
            {
                JiaoZZList2.Sort((z1, z2) => z1.zzPoint.X.CompareTo(z2.zzPoint.X));
                if (JiaoZZList2.Count >= 2)
                {
                    if (JiaoZZList2[kw1].zzPoint.X >= m_LineModelList[Gan2].Line_BeginPoint.X
                        && JiaoZZList2[kw1].zzPoint.X <= m_LineModelList[Gan2].Line_EndPoint.X
                        && JiaoZZList2[kw1 + 1].zzPoint.X >= m_LineModelList[Gan2].Line_BeginPoint.X
                        && JiaoZZList2[kw1 + 1].zzPoint.X <= m_LineModelList[Gan2].Line_EndPoint.X)
                    {
                        if (WJSelectPt.X > JiaoZZList2[kw1].zzPoint.X && WJSelectPt.X < JiaoZZList2[kw1 + 1].zzPoint.X)
                        {
                            m_LineModelList[Gan2].isSSBeam = true;//是简支梁
                            JiaoZZLength2 = JiaoZZList2[kw1 + 1].zzPoint.X - JiaoZZList2[kw1].zzPoint.X;
                        }
                    }
                }
            }
            if (m_LineModelList[Gan2].isSSBeam == true)
            {
                ExtraPt.X = WJSelectPt.X;
                ExtraPt.Y = ((ExtraPt.X - m_LineModelList[Gan2].Line_BeginPoint.X) * (m_LineModelList[Gan2].Line_EndPoint.X - ExtraPt.X) / JiaoZZLength2) + m_LineModelList[Gan2].Line_BeginPoint.Y;
                jj2 = 2;
            }
            else if (SpecialPt32.X == 0 && SpecialPt22.X != 0 && SpecialPt12.X != 0)//x3=0,x2!=x0,x1!=0
            {
                ExtraPt.X = WJSelectPt.X;
                ExtraPt.Y = ((ExtraPt.X - m_LineModelList[Gan2].Line_BeginPoint.X) * (m_LineModelList[Gan2].Line_EndPoint.X - ExtraPt.X) / (SpecialPt22.X - SpecialPt12.X)) + m_LineModelList[Gan2].Line_BeginPoint.Y;
                jj2 = 2;
            }
            else if (SpecialPt22.X == 0 && SpecialPt12.X != 0 && SpecialPt32.X != 0)//x2=0,x1!=0,x3!=0
            {
                ExtraPt.X = WJSelectPt.X;
                ExtraPt.Y = ((ExtraPt.X - m_LineModelList[Gan2].Line_BeginPoint.X) * (m_LineModelList[Gan2].Line_EndPoint.X - ExtraPt.X) / (SpecialPt32.X - SpecialPt12.X)) + m_LineModelList[Gan2].Line_BeginPoint.Y;
                m_LineModelList[Gan2].isSelectBeam = true;
                jj2 = 1;
            }
            else if (SpecialPt12.X == 0 && SpecialPt22.X != 0 && SpecialPt32.X != 0)//x1=0,x2!=0,x3!=0
            {
                ExtraPt.X = WJSelectPt.X;
                ExtraPt.Y = ((ExtraPt.X - m_LineModelList[Gan2].Line_BeginPoint.X) * (m_LineModelList[Gan2].Line_EndPoint.X - ExtraPt.X) / (SpecialPt22.X - SpecialPt32.X));
                m_LineModelList[Gan2].isSelectBeam = true;
                jj2 = 1;
            }
            else if (SpecialPt22.X != 0 && SpecialPt12.X == SpecialPt32.X && SpecialPt12.X != 0 && SpecialPt32.X != 0)//x2!=0,x1=x3
            {
                ExtraPt.X = WJSelectPt.X;
                ExtraPt.Y = ((ExtraPt.X - m_LineModelList[Gan2].Line_BeginPoint.X) * (m_LineModelList[Gan2].Line_EndPoint.X - ExtraPt.X) / (SpecialPt22.X - SpecialPt12.X)) + m_LineModelList[Gan2].Line_BeginPoint.Y;
                m_LineModelList[Gan2].isSelectBeam = true;
                jj2 = 1;
            }
            else if (SpecialPt12.X != 0 && SpecialPt22.X == SpecialPt32.X && SpecialPt22.X != 0 && SpecialPt32.X != 0)//x1!=0,x2=x3
            {
                ExtraPt.X = WJSelectPt.X;
                ExtraPt.Y = ((ExtraPt.X - m_LineModelList[Gan2].Line_BeginPoint.X) * (m_LineModelList[Gan2].Line_EndPoint.X - ExtraPt.X) / (SpecialPt22.X - SpecialPt12.X)) + m_LineModelList[Gan2].Line_BeginPoint.Y;
                m_LineModelList[Gan2].isSelectBeam = true;
                jj2 = 1;
            }
            else if (SpecialPt12.X != 0 && SpecialPt22.X != 0 && SpecialPt32.X != 0)//x1!=0,x2!=0,x3!=0
            {
                if (SpecialPt22.X != SpecialPt32.X || SpecialPt12.X != SpecialPt32.X)//x2!=x3 || x1!=x3
                {
                    if (WJSelectPt.X > SpecialPt12.X && WJSelectPt.X < SpecialPt32.X)//x1<x0<x3
                    {
                        ExtraPt.X = WJSelectPt.X;
                        ExtraPt.Y = ((ExtraPt.X - m_LineModelList[Gan2].Line_BeginPoint.X) * (m_LineModelList[Gan2].Line_EndPoint.X - ExtraPt.X) / (SpecialPt32.X - SpecialPt12.X)) + m_LineModelList[Gan2].Line_BeginPoint.Y;
                        m_LineModelList[Gan2].isSelectBeam = true;
                        jj2 = 1;
                    }
                    if (WJSelectPt.X > SpecialPt32.X && WJSelectPt.X < SpecialPt22.X)//x3<x0<x2
                    {
                        ExtraPt.X = WJSelectPt.X;
                        ExtraPt.Y = ((ExtraPt.X - m_LineModelList[Gan2].Line_BeginPoint.X) * (m_LineModelList[Gan2].Line_EndPoint.X - ExtraPt.X) / (SpecialPt22.X - SpecialPt32.X)) + m_LineModelList[Gan2].Line_BeginPoint.Y;
                        m_LineModelList[Gan2].isSelectBeam = true;
                        jj2 = 1;
                    }
                }
            }
            else
            {
                ExtraPt.X = WJSelectPt.X;
                for (int aw1 = 0; aw1 < zzList.Count - 1; aw1++)
                {
                    if (ExtraPt.X > zzList[aw1].zzPoint.X && ExtraPt.X < zzList[aw1 + 1].zzPoint.X)
                    {
                        ExtraPt.Y = ((ExtraPt.X - zzList[aw1].zzPoint.X) * (zzList[aw1 + 1].zzPoint.X - ExtraPt.X) / (zzList[aw1 + 1].zzPoint.X - zzList[aw1].zzPoint.X)) + m_LineModelList[Gan2].Line_BeginPoint.Y;
                    }
                }
                m_LineModelList[Gan2].isSelectBeam = true;
                jj2 = 0;
            }
        }
        #endregion

        #region 选择点处断开，建立新的杆List（弯矩）
        List<Point> WJNewLinePtList = new List<Point>();
        LineModelClass WJNewLine = new LineModelClass();
        List<LineModelClass> WJNewLineList = new List<LineModelClass>();
        int newGan2 = 0;
        private void BuildNewLineLis2()
        {
            WJNewLinePtList.Add(WJSelectPt);
            //建立新点 list
            foreach (LineModelClass L1 in m_LineModelList)
            {
                WJNewLinePtList.Add(L1.Line_BeginPoint);
                WJNewLinePtList.Add(L1.Line_EndPoint);
                WJNewLinePtList = WJNewLinePtList.Distinct().ToList();
            }
            WJNewLinePtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));//排序
            //新杆list
            for (int iw2 = 0; iw2 < WJNewLinePtList.Count - 1; iw2++)
            {
                WJNewLine.Line_BeginPoint = WJNewLinePtList[iw2];
                WJNewLine.Line_EndPoint = WJNewLinePtList[iw2 + 1];
                WJNewLine.LineLength = WJNewLinePtList[iw2 + 1].X - WJNewLinePtList[iw2].X;
                WJNewLine.LineK = 10000000;
                WJNewLine.isDrew = false;
                WJNewLine.LineKnewPtNum = 0;
                WJNewLineList.Add(WJNewLine);
                WJNewLine = new LineModelClass();
            }
            for (int jw2 = 0; jw2 < WJNewLineList.Count; jw2++)
            {
                if (WJNewLineList[jw2].Line_BeginPoint.X < WJSelectPt.X && WJSelectPt.X <= WJNewLineList[jw2].Line_EndPoint.X)
                {
                    WJNewLineList[jw2].isSelectBeam = true;
                    newGan2 = jw2;
                }
                if (WJNewLineList[jw2].Line_BeginPoint.X <= WJSelectPt.X && WJSelectPt.X < WJNewLineList[jw2].Line_EndPoint.X)
                {
                    WJNewLineList[jw2].isSelectBeam = true;
                }
            }
            for (int iw3 = 0; iw3 < WJNewLineList.Count; iw3++)
            {
                if (jj2 == 2)
                {
                    WJNewLineList[newGan2].LineKnewPtList.Add(ExtraPt);
                    WJNewLineList[newGan2].LineKnewPtList = WJNewLineList[newGan2].LineKnewPtList.Distinct().ToList();
                    WJNewLineList[newGan2 + 1].LineKnewPtList.Add(ExtraPt);
                    WJNewLineList[newGan2 + 1].LineKnewPtList = WJNewLineList[newGan2 + 1].LineKnewPtList.Distinct().ToList();
                }
            }
        }

        #endregion

        #region 确定支座杆号（弯矩）
        List<zzClass> WJzzList = new List<zzClass>();
        private void ConfirminfluzzGan_WJ()
        {
            WJzzList = new List<zzClass>(zzList);
            for (int mw1 = 0; mw1 < WJzzList.Count; mw1++)//循环支座
            {
                for (int nw1 = 0; nw1 < WJNewLineList.Count; nw1++)//循环杆
                {
                    if (WJNewLineList[nw1].Line_BeginPoint.X <= WJzzList[mw1].zzPoint.X
                        && WJzzList[mw1].zzPoint.X <= WJNewLineList[nw1].Line_EndPoint.X)
                    {
                        WJzzList[mw1].influzzGan = nw1;
                        break;
                    }
                }
            }
        }
        #endregion

        #region 建立已知点集(弯矩)
        Point knewPt4 = new Point();
        private void BuildPtList_WJ()//建立已知点集(剪力)
        {
            for (int mw2 = 0; mw2 < WJzzList.Count; mw2++)//循环支座
            {
                if (Convert.ToInt32(WJzzList[mw2].zzTag) == 1
                     || Convert.ToInt32(WJzzList[mw2].zzTag) == 2)//固定铰支座、活动铰支座
                {
                    knewPt4 = new Point(WJzzList[mw2].zzPoint.X, WJzzList[mw2].zzPoint.Y);
                    WJNewLineList[WJzzList[mw2].influzzGan].LineKnewPtList.Add(knewPt4);
                    WJNewLineList[WJzzList[mw2].influzzGan].LineKnewPtList = WJNewLineList[WJzzList[mw2].influzzGan].LineKnewPtList.Distinct().ToList();
                }
                else if (Convert.ToInt32(WJzzList[mw2].zzTag) == 3
                    || Convert.ToInt32(WJzzList[mw2].zzTag) == 4)//固定端
                {
                    knewPt4 = new Point(WJzzList[mw2].zzPoint.X, WJzzList[mw2].zzPoint.Y);
                    WJNewLineList[WJzzList[mw2].influzzGan].LineKnewPtList.Add(knewPt4);
                    WJNewLineList[WJzzList[mw2].influzzGan].LineK = 0;//斜率
                    WJNewLineList[WJzzList[mw2].influzzGan].LineKnewPtList = WJNewLineList[WJzzList[mw2].influzzGan].LineKnewPtList.Distinct().ToList();
                }
                else if (Convert.ToInt32(WJzzList[mw2].zzTag) == 6
                     || Convert.ToInt32(WJzzList[mw2].zzTag) == 9)//定向支座Y
                {
                    knewPt4 = new Point(WJzzList[mw2].zzPoint.X, WJzzList[mw2].zzPoint.Y);
                    WJNewLineList[WJzzList[mw2].influzzGan].LineKnewPtList.Add(knewPt4);
                    WJNewLineList[WJzzList[mw2].influzzGan].LineK = 0;//斜率
                    WJNewLineList[WJzzList[mw2].influzzGan].LineKnewPtList = WJNewLineList[WJzzList[mw2].influzzGan].LineKnewPtList.Distinct().ToList();
                }
                else if (Convert.ToInt32(WJzzList[mw2].zzTag) == 5
                     || Convert.ToInt32(WJzzList[mw2].zzTag) == 8)//定向支座X
                {
                    WJNewLineList[WJzzList[mw2].influzzGan].LineK = 0;//斜率
                    WJNewLineList[WJzzList[mw2].influzzGan].LineKnewPtList = WJNewLineList[WJzzList[mw2].influzzGan].LineKnewPtList.Distinct().ToList();
                }
            }
        }
        #endregion

        #region 收集影响线点list元素(弯矩)
        Point influPt5 = new Point();
        Point influPt6 = new Point();
        List<Point> influPtList4 = new List<Point>();
        int CalCount2 = 0;
        private void CollectInfluPt_WJ()
        {
            WJNewLineList.Sort((L1, L2) => L1.Line_BeginPoint.X.CompareTo(L2.Line_BeginPoint.X));
            for (int nw2 = 0; nw2 < WJNewLineList.Count; nw2++)//循环杆
            {
                if (nw2 <= 50)
                {
                    //斜率赋值
                    if (WJNewLineList[newGan2].LineK == 0 && WJNewLineList[newGan2 + 1].LineK == 10000000)
                    {
                        WJNewLineList[newGan2 + 1].LineK = -1;
                    }
                    if (WJNewLineList[newGan2 + 1].LineK == 0 && WJNewLineList[newGan2].LineK == 10000000)
                    {
                        WJNewLineList[newGan2].LineK = 1;
                    }
                    if (WJNewLineList[nw2].isDrew == false)//此杆未画影响线
                    {
                        for (int p4 = 0; p4 < WJNewLineList[nw2].LineKnewPtList.Count; p4++)//循环已知点list
                        {
                            if (WJNewLineList[nw2].LineKnewPtList.Count >= 2)//同杆上已知点数为2
                            {
                                WJNewLineList[nw2].LineK = (WJNewLineList[nw2].LineKnewPtList[1].Y - WJNewLineList[nw2].LineKnewPtList[0].Y) / (WJNewLineList[nw2].LineKnewPtList[1].X - WJNewLineList[nw2].LineKnewPtList[0].X);
                            }
                            if (WJNewLineList[nw2].LineKnewPtList.Count >= 1)//同杆上已知点数为1
                            {
                                if (WJNewLineList[nw2].LineK != 10000000)//斜率已知
                                {
                                    if (WJNewLineList[nw2].LineKnewPtList[p4].X > WJNewLineList[nw2].Line_BeginPoint.X
                                        && WJNewLineList[nw2].LineKnewPtList[p4].X < WJNewLineList[nw2].Line_EndPoint.X)//如果已知点不在杆端
                                    {
                                        influPt5.X = WJNewLineList[nw2].Line_BeginPoint.X;
                                        influPt5.Y = WJNewLineList[nw2].LineKnewPtList[0].Y - WJNewLineList[nw2].LineK * (WJNewLineList[nw2].LineKnewPtList[0].X - WJNewLineList[nw2].Line_BeginPoint.X);
                                        influPt6.X = WJNewLineList[nw2].Line_EndPoint.X;
                                        influPt6.Y = WJNewLineList[nw2].LineKnewPtList[0].Y - WJNewLineList[nw2].LineK * (WJNewLineList[nw2].LineKnewPtList[0].X - WJNewLineList[nw2].Line_EndPoint.X);
                                        influPtList4.Add(influPt5);
                                        influPtList4.Add(influPt6);//加入影响线点list
                                        WJNewLineList[nw2].isDrew = true;
                                        foreach (LineModelClass line in WJNewLineList)
                                        {
                                            if (influPt5.X >= line.Line_BeginPoint.X && influPt5.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt5);
                                            }
                                            if (influPt6.X >= line.Line_BeginPoint.X && influPt6.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt6);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        CalCount2++;
                                        break;
                                    }
                                    else if (WJNewLineList[nw2].LineKnewPtList[p4].X == WJNewLineList[nw2].Line_BeginPoint.X)//已知点在杆端起点
                                    {
                                        influPt5 = WJNewLineList[nw2].LineKnewPtList[p4];
                                        influPt6.X = WJNewLineList[nw2].Line_EndPoint.X;
                                        influPt6.Y = WJNewLineList[nw2].LineKnewPtList[p4].Y - WJNewLineList[nw2].LineK * (WJNewLineList[nw2].LineKnewPtList[p4].X - WJNewLineList[nw2].Line_EndPoint.X);
                                        influPtList4.Add(influPt5);
                                        influPtList4.Add(influPt6);
                                        WJNewLineList[nw2].isDrew = true;
                                        foreach (LineModelClass line in WJNewLineList)
                                        {
                                            if (influPt5.X >= line.Line_BeginPoint.X && influPt5.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt5);
                                            }
                                            if (influPt6.X >= line.Line_BeginPoint.X && influPt6.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt6);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        CalCount2++;
                                        break;
                                    }
                                    else if (WJNewLineList[nw2].LineKnewPtList[p4].X == WJNewLineList[nw2].Line_EndPoint.X)//已知点在杆端终点
                                    {
                                        influPt5.X = WJNewLineList[nw2].Line_BeginPoint.X;
                                        influPt5.Y = WJNewLineList[nw2].LineKnewPtList[p4].Y - WJNewLineList[nw2].LineK * (WJNewLineList[nw2].LineKnewPtList[p4].X - WJNewLineList[nw2].Line_BeginPoint.X);
                                        influPt6 = WJNewLineList[nw2].LineKnewPtList[p4];
                                        influPtList4.Add(influPt5);
                                        influPtList4.Add(influPt6);
                                        WJNewLineList[nw2].isDrew = true;
                                        foreach (LineModelClass line in WJNewLineList)
                                        {
                                            if (influPt5.X >= line.Line_BeginPoint.X && influPt5.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt5);
                                            }
                                            if (influPt6.X >= line.Line_BeginPoint.X && influPt6.X <= line.Line_EndPoint.X)
                                            {
                                                line.LineKnewPtList.Add(influPt6);
                                            }
                                            line.LineKnewPtList = line.LineKnewPtList.Distinct().ToList();
                                        }
                                        CalCount2++;
                                        break;
                                    }
                                }
                                else
                                {
                                    CalCount2++;
                                    if (CalCount2 > WJNewLineList.Count)
                                    {
                                        for (int iw4 = 0; iw4 < WJNewLineList.Count; iw4++)
                                        {
                                            if (jj2 == 1)
                                            {
                                                WJNewLineList[newGan2].LineKnewPtList.Add(ExtraPt);
                                                WJNewLineList[newGan2].LineKnewPtList = WJNewLineList[newGan2].LineKnewPtList.Distinct().ToList();
                                                WJNewLineList[newGan2 + 1].LineKnewPtList.Add(ExtraPt);
                                                WJNewLineList[newGan2 + 1].LineKnewPtList = WJNewLineList[newGan2 + 1].LineKnewPtList.Distinct().ToList();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("结果溢出!");
                }
            }
            for (int jw3 = 0; jw3 < WJNewLineList.Count; jw3++)
            {
                if (WJNewLineList[jw3].isDrew == false)
                {
                    CollectInfluPt_WJ();
                    break;
                }
            }
        }
        #endregion

        #region 绘制影响线（弯矩）
        private Line influLine4 = new Line();
        List<Line> InfluLineList4 = new List<Line>();

        Line tempLine4 = new Line();
        List<Line> tempLine4List = new List<Line>();

        Point WJLeftSectionPt = new Point();
        Point WJRightSectionPt = new Point();
        Point WJTextPoint = new Point();

        LineModelClass InfluClass4 = new LineModelClass();
        List<LineModelClass> InfluClassList4 = new List<LineModelClass>();

        List<Point> ShowinfluPtList4 = new List<Point>();
        Line ShowinfluLine4 = new Line();
        List<Line> ShowinfluLineList4 = new List<Line>();

        double ratio4 = 0;

        private void DrawInfluenceLine_WJ()
        {
            influPtList4 = influPtList4.Distinct().ToList();//去除重复点
            #region 归一化
            for (int jw4 = 0; jw4 < influPtList4.Count; jw4++)
            {
                if (Math.Abs(influPtList4[jw4].Y - WJNewLineList[0].Line_BeginPoint.Y) > 80)
                {
                    ratio4 = 80 / (influPtList4[jw4].Y - WJNewLineList[0].Line_BeginPoint.Y);
                }
            }
            ratio4 = Math.Abs(ratio4);
            List<Point> tempList = new List<Point>();
            ShowinfluPtList4 = new List<Point>(influPtList4);
            if (ratio4 != 0)
            {
                for (int kw2 = 0; kw2 < ShowinfluPtList4.Count; kw2++)
                {
                    Point tempPt = new Point();
                    tempPt.X = ShowinfluPtList4[kw2].X;
                    tempPt.Y = (ShowinfluPtList4[kw2].Y - WJNewLineList[0].Line_BeginPoint.Y) * ratio4 + WJNewLineList[0].Line_BeginPoint.Y;
                    tempList.Add(tempPt);
                    //  ShowinfluPtList4.RemoveAt(k);
                    // k = 0;
                }
                ShowinfluPtList4.Clear();
                foreach (Point P in tempList)
                {
                    ShowinfluPtList4.Add(P);
                }
            }
            for (int sw4 = 0; sw4 < ShowinfluPtList4.Count - 1; sw4++)
            {
                ShowinfluPtList4.Distinct().ToList();
                ShowinfluPtList4.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                ShowinfluLine4 = new Line();
                ShowinfluLine4.Stroke = Brushes.Green;
                ShowinfluLine4.StrokeThickness = 1;
                ShowinfluLine4.X1 = ShowinfluPtList4[sw4].X;
                ShowinfluLine4.Y1 = ShowinfluPtList4[sw4].Y;
                ShowinfluLine4.X2 = ShowinfluPtList4[sw4 + 1].X;
                ShowinfluLine4.Y2 = ShowinfluPtList4[sw4 + 1].Y;
                ShowinfluLineList4.Add(ShowinfluLine4);
                //can.Children.Add(ShowinfluLine);
            }
            #endregion

            for (int iw5 = 0; iw5 < influPtList4.Count - 1; iw5++)
            {
                influPtList4 = influPtList4.Distinct().ToList();
                influPtList4.Sort((P1, P2) => P1.X.CompareTo(P2.X));
                influLine4 = new Line();
                influLine4.Stroke = Brushes.Green;
                influLine4.StrokeThickness = 1;
                influLine4.X1 = influPtList4[iw5].X;
                influLine4.Y1 = influPtList4[iw5].Y;
                influLine4.X2 = influPtList4[iw5 + 1].X;
                influLine4.Y2 = influPtList4[iw5 + 1].Y;
                InfluLineList4.Add(influLine4);

                InfluClass4.Line_BeginPoint.X = influLine4.X1;
                InfluClass4.Line_BeginPoint.Y = influLine4.Y1;
                InfluClass4.Line_EndPoint.X = influLine4.X2;
                InfluClass4.Line_EndPoint.Y = influLine4.Y2;
                InfluClass4.LineLength = InfluClass4.Line_EndPoint.Y - InfluClass4.Line_BeginPoint.Y;
                InfluClass4.LineK = (InfluClass4.Line_EndPoint.Y - InfluClass4.Line_BeginPoint.Y) / (InfluClass4.Line_EndPoint.X - InfluClass4.Line_BeginPoint.X);
                InfluClassList4.Add(InfluClass4);
                InfluClass4 = new LineModelClass();

                // can.Children.Add(influLine4);
            }
            //闭合影响线
            WJNewLineList.Sort((l1, l2) => l1.Line_BeginPoint.X.CompareTo(l2.Line_BeginPoint.X));
            if (ShowinfluPtList4[0].Y != WJNewLineList[0].Line_BeginPoint.Y)
            {
                tempLine4.Stroke = Brushes.Green;
                tempLine4.StrokeThickness = 1;
                tempLine4.X1 = ShowinfluPtList4[0].X;
                tempLine4.Y1 = ShowinfluPtList4[0].Y;
                tempLine4.X2 = WJNewLineList[0].Line_BeginPoint.X;
                tempLine4.Y2 = WJNewLineList[0].Line_BeginPoint.Y;
                tempLine4List.Add(tempLine4);
                // can.Children.Add(tempLine4);
            }
            if (ShowinfluPtList4[ShowinfluPtList4.Count - 1].Y != WJNewLineList[WJNewLineList.Count - 1].Line_EndPoint.Y)
            {
                tempLine4 = new Line();
                tempLine4.Stroke = Brushes.Green;
                tempLine4.StrokeThickness = 1;
                tempLine4.X1 = ShowinfluPtList4[ShowinfluPtList4.Count - 1].X;
                tempLine4.Y1 = ShowinfluPtList4[ShowinfluPtList4.Count - 1].Y;
                tempLine4.X2 = WJNewLineList[WJNewLineList.Count - 1].Line_EndPoint.X;
                tempLine4.Y2 = WJNewLineList[WJNewLineList.Count - 1].Line_EndPoint.Y;
                tempLine4List.Add(tempLine4);

                //can.Children.Add(tempLine4);
            }
            for (int aw2 = 0; aw2 < influPtList4.Count; aw2++)
            {
                if (influPtList4[aw2].X == WJSelectPt.X)
                {
                    WJLeftSectionPt.X = influPtList4[aw2].X - 0.01;
                    WJLeftSectionPt.Y = influPtList4[aw2].Y;
                    WJRightSectionPt.X = influPtList4[aw2].X + 0.01;
                    WJRightSectionPt.Y = influPtList4[aw2].Y;
                    WJTextPoint.X = influPtList4[aw2].X;
                    WJTextPoint.Y = influPtList4[aw2].Y;
                }
            }
        }
        #endregion

        #endregion

        #region 弯矩影响线清空函数
        private void WanJuResetBt_Click(object sender, RoutedEventArgs e)
        {
            comboList.Clear();
            zzComboBox.ItemsSource = null;
            zzCombomess();

            ClearInfluenceLine_FL();//清空，初始化
            BottomSectionClear();
            UserSectionClear();

            comboList2.Clear();
            zzComboBox2.ItemsSource = null;
            zzCombomess2();
            ClearInfluenceLine_FLO();
            BottomSectionClear();
            UserSectionClear();

            ClearInfluenceLine_WJ();
            BottomSectionClear();
            UserSectionClear();

            ClearInfluenceLine_JL();
            BottomSectionClear();
            UserSectionClear();
        }

        private void ClearInfluenceLine_WJ()
        {
            JiaoZZList2.Clear();
            SpecialPt12 = new Point(0, 0);
            SpecialPt22 = new Point(0, 0);
            SpecialPt32 = new Point(0, 0);
            jj2 = 0;
            JiaoZZLength2 = 0;
            ExtraPt = new Point();
            isWanJuBt = false;
            WJSelectPt = new Point();
            foreach (Image A in WJImageList)
            {
                can.Children.Remove(A);
            }
            WJImageList.Clear();
            CurrentSelectTB.Text = "";
            CurrentSelectTB2.Text = "";
            WJNewLinePtList.Clear();
            WJNewLine = new LineModelClass();
            WJNewLineList.Clear();
            newGan2 = -1;
            WJzzList.Clear();
            knewPt4 = new Point();
            influPt5 = new Point();
            influPt6 = new Point();
            influPtList4.Clear();
            CalCount2 = 0;
            influLine4 = new Line();
            foreach (Line L1 in InfluLineList4)
            {
                can.Children.Remove(L1);
            }
            InfluLineList4.Clear();

            foreach (Line tp4 in tempLine4List)
            {
                can.Children.Remove(tp4);
            }
            tempLine4 = new Line();
            tempLine4List.Clear();
            foreach (Line L2 in BottomInfluLineList4)
            {
                can.Children.Remove(L2);
            }
            BottomInfluLineList4.Clear();

            foreach (Line tp44 in BottomTempLine4List)
            {
                can.Children.Remove(tp44);
            }
            BottomTempLine4List.Clear();
            foreach (TextBlock T4 in BottomVDTextList4)
            {
                can.Children.Remove(T4);
            }
            BottomVDTextList4.Clear();
            can.Children.Remove(BottomWJImage);

            foreach (TextBlock tb4 in NoticeTB4List)
            {
                can.Children.Remove(tb4);
            }
            WJLeftSectionPt = new Point();
            WJRightSectionPt = new Point();
            WJTextPoint = new Point();
            InfluClassList4.Clear();
            InfluClass4 = new LineModelClass();

            foreach (Line L in ShowinfluLineList4)
            {
                can.Children.Remove(L);
            }
            ShowinfluLineList4.Clear();
            ShowinfluPtList4.Clear();
            ShowinfluLine4 = new Line();

            ratio4 = 0;
        }
        #endregion

        #endregion

        #region Bottom Section
        #region 变量
        ScaleTransform BottomScale = new ScaleTransform();
        //直线
        Line BottomLine1 = new Line();
        List<Line> BottomLineList1 = new List<Line>();

        LineModelClass BottomLine = new LineModelClass();
        List<LineModelClass> BottomLineList = new List<LineModelClass>();

        Ellipse BottomEllipse = new Ellipse();
        List<Ellipse> BottomEllipseList = new List<Ellipse>();
        Ellipse BottomEllipse1 = new Ellipse();

        TextBlock BottomLineText = new TextBlock();
        List<TextBlock> BottomLineTextList = new List<TextBlock>();
        //支座
        Rectangle BottomZZRect = new Rectangle();
        List<Rectangle> BottomZZRectList = new List<Rectangle>();

        zzClass BottomZZ = new zzClass();
        List<zzClass> BottomZZList = new List<zzClass>();

        TextBlock BottomZZTB = new TextBlock();
        List<TextBlock> BottomZZTBList = new List<TextBlock>();
        //支座反力
        Line BottomInfluLine1 = new Line();
        List<Line> BottomInfluLineList1 = new List<Line>();

        Line BottomTempLine1 = new Line();
        List<Line> BottomTempLine1List = new List<Line>();

        TextBlock BottomVDText1 = new TextBlock();
        List<TextBlock> BottomVDTextList1 = new List<TextBlock>();
        //支座反力偶
        Line BottomInfluLine2 = new Line();
        List<Line> BottomInfluLineList2 = new List<Line>();

        Line BottomTempLine2 = new Line();
        List<Line> BottomTempLine2List = new List<Line>();

        TextBlock BottomVDText2 = new TextBlock();
        List<TextBlock> BottomVDTextList2 = new List<TextBlock>();

        //剪力
        Line BottomInfluLine3 = new Line();
        List<Line> BottomInfluLineList3 = new List<Line>();

        Line BottomTempLine3 = new Line();
        List<Line> BottomTempLine3List = new List<Line>();

        TextBlock BottomVDText3 = new TextBlock();
        List<TextBlock> BottomVDTextList3 = new List<TextBlock>();

        Image BottomJLImage = new Image();
        //弯矩
        Line BottomInfluLine4 = new Line();
        List<Line> BottomInfluLineList4 = new List<Line>();

        Line BottomTempLine4 = new Line();
        List<Line> BottomTempLine4List = new List<Line>();

        TextBlock BottomVDText4 = new TextBlock();
        List<TextBlock> BottomVDTextList4 = new List<TextBlock>();
        Image BottomWJImage = new Image();

        #endregion
        private void CorrectAnswer_Click(object sender, RoutedEventArgs e)//结果检查按钮
        {
            if (CheckNum <= 0)
            {
                MessageBox.Show("请先尝试绘制影响线后再查看正确结果！");
            }
            if (CheckNum > 0)
            {
                MoveBottom();
            }
            CheckNum = 0;
        }

        List<Rectangle> BottomZZRectList_1 = new List<Rectangle>();
        List<TextBlock> BottomZZTBList_1 = new List<TextBlock>();

        private void MoveBottom()
        {
            BottomScale.ScaleY = -1;
            BottomLineList.Clear();
            #region 直线
            for (int ib1 = 0; ib1 < m_LineModelList.Count; ib1++)//储存直线
            {
                BottomLine.Line_BeginPoint.X = m_LineModelList[ib1].Line_BeginPoint.X;
                BottomLine.Line_BeginPoint.Y = m_LineModelList[ib1].Line_BeginPoint.Y - 400 * ResolutionRatio;
                BottomLine.Line_EndPoint.X = m_LineModelList[ib1].Line_EndPoint.X;
                BottomLine.Line_EndPoint.Y = m_LineModelList[ib1].Line_EndPoint.Y - 400 * ResolutionRatio;
                BottomLine1 = new Line();
                BottomLine1.X1 = BottomLine.Line_BeginPoint.X;
                BottomLine1.Y1 = BottomLine.Line_BeginPoint.Y;
                BottomLine1.X2 = BottomLine.Line_EndPoint.X;
                BottomLine1.Y2 = BottomLine.Line_EndPoint.Y;
                BottomLine1.Stroke = Brushes.Black;
                BottomLine1.StrokeThickness = 1;
                BottomLineList.Add(BottomLine);
                BottomLineList1.Add(BottomLine1);
                can.Children.Add(BottomLine1);
                BottomLine = new LineModelClass();
            }
            //各种标注
            //起点圆点加粗
            BottomEllipse = new Ellipse();
            BottomEllipse.Height = 4;
            BottomEllipse.Width = 4;
            BottomEllipse.Fill = Brushes.Black;
            BottomEllipse.SetValue(Canvas.LeftProperty, startPoint.X - 2);
            BottomEllipse.SetValue(Canvas.TopProperty, startPoint.Y - 402 * ResolutionRatio);
            BottomEllipseList.Add(BottomEllipse);
            can.Children.Add(BottomEllipse);

            //终点圆点加粗
            BottomEllipse1 = new Ellipse();
            BottomEllipse1.Height = 4;
            BottomEllipse1.Width = 4;
            BottomEllipse1.Fill = Brushes.Black;
            BottomEllipse1.SetValue(Canvas.LeftProperty, endPoint.X - 2);
            BottomEllipse1.SetValue(Canvas.TopProperty, endPoint.Y - 402 * ResolutionRatio);
            BottomEllipseList.Add(BottomEllipse1);
            can.Children.Add(BottomEllipse1);

            //标线号
            double LineTextX = 0;
            double LineTextY = 0;

            for (int ii = 0; ii < m_LineModelList.Count; ii++)
            {
                LineTextX = (m_LineModelList[ii].Line_EndPoint.X + m_LineModelList[ii].Line_BeginPoint.X) / 2;
                LineTextY = (m_LineModelList[ii].Line_EndPoint.Y + m_LineModelList[ii].Line_BeginPoint.Y) / 2 + -470 * ResolutionRatio;
                BottomLineText = new TextBlock();
                BottomLineText.Text = "(" + (ii + 1) + ")";
                BottomLineText.Margin = new Thickness(LineTextX, LineTextY, 0, 0);
                ScaleTransform LTscale = new ScaleTransform();
                LTscale.ScaleY = -1;
                BottomLineText.RenderTransform = LTscale;
                BottomLineTextList.Add(BottomLineText);
                can.Children.Add(BottomLineText);
            }

            #endregion
            BottomZZRectList_1.Clear();
            BottomZZTBList_1.Clear();
            BottomZZRectList_1 = new List<Rectangle>(BottomZZRectList);
            BottomZZTBList_1 = new List<TextBlock>(BottomZZTBList);

            #region 支座
            for (int ab1 = 0; ab1 < BottomZZRectList_1.Count; ab1++)
            {
                BottomZZRectList_1[ab1].RenderTransform = BottomScale;
                can.Children.Add(BottomZZRectList_1[ab1]);
            }
            for (int a1 = 0; a1 < BottomZZTBList_1.Count; a1++)
            {
                BottomZZTBList_1[a1].RenderTransform = BottomScale;
                can.Children.Add(BottomZZTBList_1[a1]);
            }
            #endregion

            #region 支座反力影响线
            for (int bb1 = 0; bb1 < ShowinfluLineList.Count; bb1++)
            {
                BottomInfluLine1 = new Line();
                BottomInfluLine1.Stroke = Brushes.Green;
                BottomInfluLine1.StrokeThickness = 1;
                BottomInfluLine1.X1 = ShowinfluLineList[bb1].X1;
                BottomInfluLine1.Y1 = ShowinfluLineList[bb1].Y1 - 400 * ResolutionRatio;
                BottomInfluLine1.X2 = ShowinfluLineList[bb1].X2;
                BottomInfluLine1.Y2 = ShowinfluLineList[bb1].Y2 - 400 * ResolutionRatio;
                BottomInfluLineList1.Add(BottomInfluLine1);
                can.Children.Add(BottomInfluLine1);
            }
            for (int tp1 = 0; tp1 < tempLineList.Count; tp1++)
            {
                BottomTempLine1 = new Line();
                BottomTempLine1.Stroke = Brushes.Green;
                BottomTempLine1.StrokeThickness = 1;
                BottomTempLine1.X1 = tempLineList[tp1].X1;
                BottomTempLine1.Y1 = tempLineList[tp1].Y1 - 400 * ResolutionRatio;
                BottomTempLine1.X2 = tempLineList[tp1].X2;
                BottomTempLine1.Y2 = tempLineList[tp1].Y2 - 400 * ResolutionRatio;
                can.Children.Add(BottomTempLine1);
                BottomTempLine1List.Add(BottomTempLine1);

            }
            
            double deta0 = 0;
            for (int ab2 = 0; ab2 < influPtList.Count; ab2++)
            {
                deta0 = (influPtList[ab2].Y - m_LineModelList[0].Line_BeginPoint.Y) / 20;
                if (deta0 > 0)
                {
                    for (int aa1 = 0; aa1 < ShowinfluPtList.Count; aa1++)
                    {
                        if (influPtList[ab2].X == ShowinfluPtList[aa1].X)
                        {
                            BottomVDText1 = new TextBlock();
                            BottomVDText1.Text = Convert.ToString(Math.Round(deta0, 2));
                            BottomVDText1.Margin = new Thickness(ShowinfluPtList[aa1].X - 10, ShowinfluPtList[aa1].Y - 380 * ResolutionRatio, 0, 0);
                            BottomVDText1.RenderTransform = BottomScale;
                            BottomVDText1.Background = Brushes.DarkGreen;
                            BottomVDText1.Foreground = Brushes.White;
                            can.Children.Add(BottomVDText1);
                            BottomVDTextList1.Add(BottomVDText1);
                        }
                    }
                }
                if (deta0 < 0)
                {
                    for (int aa2 = 0; aa2 < ShowinfluPtList.Count; aa2++)
                    {
                        if (influPtList[ab2].X == ShowinfluPtList[aa2].X)
                        {
                            BottomVDText1 = new TextBlock();
                            BottomVDText1.Text = Convert.ToString(-Math.Round(deta0, 2));
                            BottomVDText1.Margin = new Thickness(ShowinfluPtList[aa2].X - 10, ShowinfluPtList[aa2].Y - 420 * ResolutionRatio, 0, 0);
                            BottomVDText1.RenderTransform = BottomScale;
                            BottomVDText1.Background = Brushes.DarkGreen;
                            BottomVDText1.Foreground = Brushes.White;
                            can.Children.Add(BottomVDText1);
                            BottomVDTextList1.Add(BottomVDText1);
                        }
                    }
                }
            }

            #endregion

            #region 支座反力偶影响线
            for (int c = 0; c < ShowinfluLineList2.Count; c++)
            {
                BottomInfluLine2 = new Line();
                BottomInfluLine2.Stroke = Brushes.Green;
                BottomInfluLine2.StrokeThickness = 1;
                BottomInfluLine2.X1 = ShowinfluLineList2[c].X1;
                BottomInfluLine2.Y1 = ShowinfluLineList2[c].Y1 - 400 * ResolutionRatio;
                BottomInfluLine2.X2 = ShowinfluLineList2[c].X2;
                BottomInfluLine2.Y2 = ShowinfluLineList2[c].Y2 - 400 * ResolutionRatio;
                BottomInfluLineList2.Add(BottomInfluLine2);
                can.Children.Add(BottomInfluLine2);
            }
            for (int tp2 = 0; tp2 < tempLine2List.Count; tp2++)
            {
                BottomTempLine2 = new Line();
                BottomTempLine2.Stroke = Brushes.Green;
                BottomTempLine2.StrokeThickness = 1;
                BottomTempLine2.X1 = tempLine2List[tp2].X1;
                BottomTempLine2.Y1 = tempLine2List[tp2].Y1 - 400 * ResolutionRatio;
                BottomTempLine2.X2 = tempLine2List[tp2].X2;
                BottomTempLine2.Y2 = tempLine2List[tp2].Y2 - 400 * ResolutionRatio;
                can.Children.Add(BottomTempLine2);
                BottomTempLine2List.Add(BottomTempLine2);
            }

            double deta1 = 0;
            for (int ab3 = 0; ab3 < influPtList2.Count; ab3++)
            {
                deta1 = (influPtList2[ab3].Y - m_LineModelList[0].Line_BeginPoint.Y) / 20;
                if (deta1 > 0)
                {
                    for (int bb1 = 0; bb1 < ShowinfluPtList2.Count; bb1++)
                    {
                        if (influPtList2[ab3].X == ShowinfluPtList2[bb1].X)
                        {
                            BottomVDText2 = new TextBlock();
                            BottomVDText2.Text = Convert.ToString(Math.Round(deta1, 2));
                            BottomVDText2.Margin = new Thickness(ShowinfluPtList2[bb1].X - 10, ShowinfluPtList2[bb1].Y - 380 * ResolutionRatio, 0, 0);
                            BottomVDText2.RenderTransform = BottomScale;
                            BottomVDText2.Background = Brushes.DarkGreen;
                            BottomVDText2.Foreground = Brushes.White;
                            can.Children.Add(BottomVDText2);
                            BottomVDTextList2.Add(BottomVDText2);
                        }
                    }
                }
                if (deta1 < 0)
                {
                    for (int bb2 = 0; bb2 < ShowinfluPtList2.Count; bb2++)
                    {
                        if (influPtList2[ab3].X == ShowinfluPtList2[bb2].X)
                        {
                            BottomVDText2 = new TextBlock();
                            BottomVDText2.Text = Convert.ToString(-Math.Round(deta1, 2));
                            BottomVDText2.Margin = new Thickness(ShowinfluPtList2[bb2].X - 10, ShowinfluPtList2[bb2].Y - 420 * ResolutionRatio, 0, 0);
                            BottomVDText2.RenderTransform = BottomScale;
                            BottomVDText2.Background = Brushes.DarkGreen;
                            BottomVDText2.Foreground = Brushes.White;
                            can.Children.Add(BottomVDText2);
                            BottomVDTextList2.Add(BottomVDText2);
                        }
                    }
                }
            }

            #endregion

            #region 剪力影响线
            for (int d = 0; d < ShowinfluLineList3.Count; d++)
            {
                BottomInfluLine3 = new Line();
                BottomInfluLine3.Stroke = Brushes.Green;
                BottomInfluLine3.StrokeThickness = 1;
                BottomInfluLine3.X1 = ShowinfluLineList3[d].X1;
                BottomInfluLine3.Y1 = ShowinfluLineList3[d].Y1 - 400 * ResolutionRatio;
                BottomInfluLine3.X2 = ShowinfluLineList3[d].X2;
                BottomInfluLine3.Y2 = ShowinfluLineList3[d].Y2 - 400 * ResolutionRatio;
                BottomInfluLineList3.Add(BottomInfluLine3);
                can.Children.Add(BottomInfluLine3);
            }

            for (int tp3 = 0; tp3 < tempLine3List.Count; tp3++)
            {
                BottomTempLine3 = new Line();
                BottomTempLine3.Stroke = Brushes.Green;
                BottomTempLine3.StrokeThickness = 1;
                BottomTempLine3.X1 = tempLine3List[tp3].X1;
                BottomTempLine3.Y1 = tempLine3List[tp3].Y1 - 400 * ResolutionRatio;
                BottomTempLine3.X2 = tempLine3List[tp3].X2;
                BottomTempLine3.Y2 = tempLine3List[tp3].Y2 - 400 * ResolutionRatio;
                can.Children.Add(BottomTempLine3);
                BottomTempLine3List.Add(BottomTempLine3);
            }

            double deta2 = 0;
            for (int ab4 = 0; ab4 < influPtList3.Count; ab4++)
            {
                deta2 = (influPtList3[ab4].Y - m_LineModelList[0].Line_BeginPoint.Y) / 20;
                if (deta2 > 0)
                {
                    for (int cc1 = 0; cc1 < ShowinfluPtList3.Count; cc1++)
                    {
                        if (influPtList3[ab4].X == ShowinfluPtList3[cc1].X)
                        {
                            BottomVDText3 = new TextBlock();
                            BottomVDText3.Text = Convert.ToString(Math.Round(deta2, 2));
                            BottomVDText3.Margin = new Thickness(ShowinfluPtList3[cc1].X - 10, ShowinfluPtList3[cc1].Y - 380 * ResolutionRatio, 0, 0);
                            BottomVDText3.RenderTransform = BottomScale;
                            BottomVDText3.Background = Brushes.DarkGreen;
                            BottomVDText3.Foreground = Brushes.White;
                            can.Children.Add(BottomVDText3);
                            BottomVDTextList3.Add(BottomVDText3);
                        }
                    }
                }
                if (deta2 < 0)
                {
                    for (int cc2 = 0; cc2 < ShowinfluPtList3.Count; cc2++)
                    {
                        if (influPtList3[ab4].X == ShowinfluPtList3[cc2].X)
                        {
                            BottomVDText3 = new TextBlock();
                            BottomVDText3.Text = Convert.ToString(-Math.Round(deta2, 2));
                            BottomVDText3.Margin = new Thickness(ShowinfluPtList3[cc2].X - 10, ShowinfluPtList3[cc2].Y - 420 * ResolutionRatio, 0, 0);
                            BottomVDText3.RenderTransform = BottomScale;
                            BottomVDText3.Background = Brushes.DarkGreen;
                            BottomVDText3.Foreground = Brushes.White;
                            can.Children.Add(BottomVDText3);
                            BottomVDTextList3.Add(BottomVDText3);
                        }
                    }
                }
            }
            BottomJLImage = new Image();
            BottomJLImage.Source = JLImage.Source;
            BottomJLImage.Width = JLImage.Width;
            BottomJLImage.Height = JLImage.Height;
            BottomJLImage.SetValue(Canvas.LeftProperty, JLSelectPt.X - 20);
            BottomJLImage.SetValue(Canvas.TopProperty, JLSelectPt.Y - 380 * ResolutionRatio);
            BottomJLImage.RenderTransform = BottomScale;
            can.Children.Add(BottomJLImage);

            #endregion

            #region 弯矩影响线
            for (int e = 0; e < ShowinfluLineList4.Count; e++)
            {
                BottomInfluLine4 = new Line();
                BottomInfluLine4.Stroke = Brushes.Green;
                BottomInfluLine4.StrokeThickness = 1;
                BottomInfluLine4.X1 = ShowinfluLineList4[e].X1;
                BottomInfluLine4.Y1 = ShowinfluLineList4[e].Y1 - 400 * ResolutionRatio;
                BottomInfluLine4.X2 = ShowinfluLineList4[e].X2;
                BottomInfluLine4.Y2 = ShowinfluLineList4[e].Y2 - 400 * ResolutionRatio;
                BottomInfluLineList4.Add(BottomInfluLine4);
                can.Children.Add(BottomInfluLine4);
            }
            if (InfluLineList4.Count > 0)
            {
                for (int tp4 = 0; tp4 < tempLine4List.Count; tp4++)
                {
                    BottomTempLine4 = new Line();
                    BottomTempLine4.Stroke = Brushes.Green;
                    BottomTempLine4.StrokeThickness = 1;
                    BottomTempLine4.X1 = tempLine4List[tp4].X1;
                    BottomTempLine4.Y1 = tempLine4List[tp4].Y1 - 400 * ResolutionRatio;
                    BottomTempLine4.X2 = tempLine4List[tp4].X2;
                    BottomTempLine4.Y2 = tempLine4List[tp4].Y2 - 400 * ResolutionRatio;
                    can.Children.Add(BottomTempLine4);
                    BottomTempLine4List.Add(BottomTempLine4);
                }

                double deta3 = 0;
                for (int ab5 = 0; ab5 < influPtList4.Count; ab5++)
                {
                    deta3 = (influPtList4[ab5].Y - m_LineModelList[0].Line_BeginPoint.Y) / 20;
                    if (deta3 > 0)
                    {
                        for (int dd1 = 0; dd1 < ShowinfluPtList4.Count; dd1++)
                        {
                            if (influPtList4[ab5].X == ShowinfluPtList4[dd1].X)
                            {
                                BottomVDText4 = new TextBlock();
                                BottomVDText4.Text = Convert.ToString(Math.Round(deta3, 2));
                                BottomVDText4.Margin = new Thickness(ShowinfluPtList4[dd1].X - 10, ShowinfluPtList4[dd1].Y - 380 * ResolutionRatio, 0, 0);
                                BottomVDText4.RenderTransform = BottomScale;
                                BottomVDText4.Background = Brushes.DarkGreen;
                                BottomVDText4.Foreground = Brushes.White;
                                can.Children.Add(BottomVDText4);
                                BottomVDTextList4.Add(BottomVDText4);
                            }
                        }
                    }
                    if (deta3 < 0)
                    {
                        for (int dd2 = 0; dd2 < ShowinfluPtList4.Count; dd2++)
                        {
                            if (influPtList4[ab5].X == ShowinfluPtList4[dd2].X)
                            {
                                BottomVDText4 = new TextBlock();
                                BottomVDText4.Text = Convert.ToString(-Math.Round(deta3, 2));
                                BottomVDText4.Margin = new Thickness(ShowinfluPtList4[dd2].X - 10, ShowinfluPtList4[dd2].Y - 420 * ResolutionRatio, 0, 0);
                                BottomVDText4.RenderTransform = BottomScale;
                                BottomVDText4.Background = Brushes.DarkGreen;
                                BottomVDText4.Foreground = Brushes.White;
                                can.Children.Add(BottomVDText4);
                                BottomVDTextList4.Add(BottomVDText4);
                            }
                        }
                    }
                    else
                    {

                    }
                }
                BottomWJImage = new Image();
                BottomWJImage.Source = WJImage0.Source;
                BottomWJImage.Width = WJImage0.Width;
                BottomWJImage.Height = WJImage0.Height;
                BottomWJImage.SetValue(Canvas.LeftProperty, WJSelectPt.X - 20);
                BottomWJImage.SetValue(Canvas.TopProperty, WJSelectPt.Y - 380 * ResolutionRatio);
                BottomWJImage.RenderTransform = BottomScale;
                can.Children.Add(BottomWJImage);
            }

            #endregion
        }

        #endregion

        #region BottomSection清空函数
        private void BottomSectionClear()
        {
            foreach (Line a in BottomLineList1)
            {
                can.Children.Remove(a);
            }
            BottomLineList1.Clear();
            BottomLineList.Clear();
            BottomLine1 = new Line();
            BottomLine = new LineModelClass();
            foreach (Ellipse b in BottomEllipseList)
            {
                can.Children.Remove(b);
            }
            BottomEllipse = new Ellipse();
            BottomEllipseList.Clear();
            foreach (TextBlock c in BottomLineTextList)
            {
                can.Children.Remove(c);
            }
            BottomLineTextList.Clear();

            foreach (Rectangle d in BottomZZRectList_1)
            {
                can.Children.Remove(d);
            }
            BottomZZRectList_1.Clear();
            BottomZZRect = new Rectangle();
            BottomZZ = new zzClass();
            BottomZZList.Clear();
            foreach (TextBlock e in BottomZZTBList_1)
            {
                can.Children.Remove(e);
            }
            BottomZZTB = new TextBlock();
            BottomZZTBList_1.Clear();
            foreach (Line f in BottomInfluLineList1)
            {
                can.Children.Remove(f);
            }
            BottomInfluLine1 = new Line();
            BottomInfluLineList1.Clear();

            foreach (Line tp1 in BottomTempLine1List)
            {
                can.Children.Remove(tp1);
            }
            BottomTempLine1List.Clear();
            foreach (TextBlock T in BottomVDTextList1)
            {
                can.Children.Remove(T);
            }
            BottomVDTextList1.Clear();
            foreach (Line g in BottomInfluLineList2)
            {
                can.Children.Remove(g);
            }
            BottomInfluLine2 = new Line();
            BottomInfluLineList2.Clear();

            foreach (Line tp2 in BottomTempLine2List)
            {
                can.Children.Remove(tp2);
            }
            BottomTempLine2List.Clear();
            can.Children.Remove(BottomVDText2);
            foreach (TextBlock T2 in BottomVDTextList2)
            {
                can.Children.Remove(T2);
            }
            BottomVDTextList2.Clear();
            foreach (Line h in BottomInfluLineList3)
            {
                can.Children.Remove(h);
            }
            BottomInfluLine3 = new Line();
            BottomInfluLineList3.Clear();

            foreach (Line tp3 in BottomTempLine3List)
            {
                can.Children.Remove(tp3);
            }
            BottomTempLine3List.Clear();
            can.Children.Remove(BottomJLImage);

            foreach (Line i in BottomInfluLineList4)
            {
                can.Children.Remove(i);
            }
            BottomInfluLine4 = new Line();
            BottomInfluLineList4.Clear();

            foreach (Line tp4 in BottomTempLine4List)
            {
                can.Children.Remove(tp4);
            }
            BottomTempLine4List.Clear();
            can.Children.Remove(BottomVDText4);
            can.Children.Remove(BottomWJImage);
        }

        #endregion

        #region 输入模块

        #region 标注关键点
        Point keyPt1 = new Point();
        Point keyPt2 = new Point();
        Point keyPt3 = new Point();
        Point keyPt4 = new Point();
        List<Point> RedPointList = new List<Point>();
        List<Ellipse> RedEllipseList = new List<Ellipse>();
        Ellipse redEllipse = new Ellipse();

        TextBlock KeyPtTB = new TextBlock();
        List<TextBlock> KeyPtTBList = new List<TextBlock>();
        private void MarkPoint()
        {
            for (int am1 = 0; am1 < m_LineModelList.Count; am1++)
            {
                RedPointList.Add(m_LineModelList[am1].Line_BeginPoint);
                RedPointList.Add(m_LineModelList[am1].Line_EndPoint);
            }
            for (int bm1 = 0; bm1 < zzList.Count; bm1++)
            {
                RedPointList.Add(zzList[bm1].zzPoint);
            }
            if (WJSelectPt.X != 0)
            {
                keyPt1.X = WJSelectPt.X - 5;
                keyPt1.Y = WJSelectPt.Y;
                keyPt2.X = WJSelectPt.X + 5;
                keyPt2.Y = WJSelectPt.Y;
                RedPointList.Add(keyPt1);
                RedPointList.Add(keyPt2);
            }
            if (JLSelectPt.X != 0)
            {
                keyPt3.X = JLSelectPt.X - 5;
                keyPt3.Y = JLSelectPt.Y;
                keyPt4.X = JLSelectPt.X + 5;
                keyPt4.Y = JLSelectPt.Y;
                RedPointList.Add(keyPt3);
                RedPointList.Add(keyPt4);
            }
            RedPointList = RedPointList.Distinct().ToList();
            for (int dm1 = 0; dm1 < m_LineModelList.Count; dm1++)
            {
                KeyPtTB = new TextBlock();
                KeyPtTB.Text = m_LineModelList[dm1].LineName + "=" + Convert.ToString(m_LineModelList[dm1].LineLength / 20);
                var half = m_LineModelList[dm1].Line_BeginPoint.X + (m_LineModelList[dm1].Line_EndPoint.X - m_LineModelList[dm1].Line_BeginPoint.X) / 2;
                KeyPtTB.Margin = new Thickness(half, RedPointList[dm1].Y - 40, 0, 0);
                ScaleTransform scale = new ScaleTransform();
                scale.ScaleY = -1;
                KeyPtTB.RenderTransform = scale;
                KeyPtTBList.Add(KeyPtTB);
                can.Children.Add(KeyPtTB);
            }
            for (int cm1 = 0; cm1 < RedPointList.Count; cm1++)
            {
                redEllipse = new Ellipse();
                redEllipse.Height = 5;
                redEllipse.Width = 5;
                redEllipse.Fill = Brushes.Red;
                redEllipse.SetValue(Canvas.LeftProperty, RedPointList[cm1].X - 2.5);
                redEllipse.SetValue(Canvas.TopProperty, RedPointList[cm1].Y - 2.5);
                redEllipse.MouseLeftButtonDown += e1_MouseLeftButtonDown;
                RedEllipseList.Add(redEllipse);
                can.Children.Add(redEllipse);
            }
        }
        #endregion

        #region 关键点点击事件
        TextBlock InputPtTB = new TextBlock();
        List<TextBlock> InputPtTBList = new List<TextBlock>();
        Ellipse InputPtEllipse = new Ellipse();
        List<Ellipse> InputPtEllipseList = new List<Ellipse>();
        //用于显示的数据
        List<Point> ShowUserPtList = new List<Point>();
        Line ShowUserLine = new Line();
        List<Line> ShowUserLineList = new List<Line>();

        private void e1_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            Window4 win4 = new Window4();
            win4.ShowDialog();
            InputPt = new Point();
            InputPt.X = pt.X;
            InputPt.Y = 20 * Convert.ToDouble(win4.KeyPtYTB.Text) + m_LineModelList[0].Line_BeginPoint.Y;
            InputPtList.Add(InputPt);
            Point SUPt = new Point();
            SUPt.X = InputPt.X;
            if (ShowinfluLineList.Count != 0)//支座反力影响线
            {
                if (ratio1 == 0)
                {
                    SUPt.Y = 20 * Convert.ToDouble(win4.KeyPtYTB.Text) + m_LineModelList[0].Line_BeginPoint.Y;
                }
                else
                {
                    SUPt.Y = (20 * Convert.ToDouble(win4.KeyPtYTB.Text) + m_LineModelList[0].Line_BeginPoint.Y) * ratio1 - m_LineModelList[0].Line_BeginPoint.Y * (ratio1 - 1);
                }
            }
            if (ShowinfluLineList2.Count != 0)//支座反力偶影响线
            {
                if (ratio2 == 0)
                {
                    SUPt.Y = 20 * Convert.ToDouble(win4.KeyPtYTB.Text) + m_LineModelList[0].Line_BeginPoint.Y;
                }
                else
                {
                    SUPt.Y = (20 * Convert.ToDouble(win4.KeyPtYTB.Text) + m_LineModelList[0].Line_BeginPoint.Y) * ratio2 - m_LineModelList[0].Line_BeginPoint.Y * (ratio2 - 1);
                }
            }
            if (ShowinfluLineList3.Count != 0)//剪力影响线
            {
                if (ratio3 == 0)
                {
                    SUPt.Y = 20 * Convert.ToDouble(win4.KeyPtYTB.Text) + m_LineModelList[0].Line_BeginPoint.Y;
                }
                else
                {
                    SUPt.Y = (20 * Convert.ToDouble(win4.KeyPtYTB.Text) + m_LineModelList[0].Line_BeginPoint.Y) * ratio3 - m_LineModelList[0].Line_BeginPoint.Y * (ratio3 - 1);
                }
            }
            if (ShowinfluLineList4.Count != 0)//弯矩影响线
            {
                if (ratio4 == 0)
                {
                    SUPt.Y = 20 * Convert.ToDouble(win4.KeyPtYTB.Text) + m_LineModelList[0].Line_BeginPoint.Y;
                }
                else
                {
                    SUPt.Y = (20 * Convert.ToDouble(win4.KeyPtYTB.Text) + m_LineModelList[0].Line_BeginPoint.Y) * ratio4 - m_LineModelList[0].Line_BeginPoint.Y * (ratio4 - 1);
                }
            }

            ShowUserPtList.Add(SUPt);


            InputPtEllipse = new Ellipse();
            InputPtEllipse.Height = 4;
            InputPtEllipse.Width = 4;
            InputPtEllipse.Fill = Brushes.Blue;
            InputPtEllipse.SetValue(Canvas.LeftProperty, SUPt.X - 2);
            InputPtEllipse.SetValue(Canvas.TopProperty, SUPt.Y - 2);
            InputPtEllipseList.Add(InputPtEllipse);
            can.Children.Add(InputPtEllipse);


            InputPtTB = new TextBlock();
            InputPtTB.Text = win4.KeyPtYTB.Text;
            InputPtTB.Foreground = Brushes.White;
            InputPtTB.Background = Brushes.Blue;
            if (Convert.ToDouble(InputPtTB.Text) < 0)
            {
                InputPtTB.Margin = new Thickness(SUPt.X, SUPt.Y - 40, 0, 0);
                ScaleTransform scale = new ScaleTransform();
                scale.ScaleY = -1;
                InputPtTB.RenderTransform = scale;
                InputPtTBList.Add(InputPtTB);
                can.Children.Add(InputPtTB);
            }
            if (Convert.ToDouble(InputPtTB.Text) > 0)
            {
                InputPtTB.Margin = new Thickness(SUPt.X, SUPt.Y + 40, 0, 0);
                ScaleTransform scale = new ScaleTransform();
                scale.ScaleY = -1;
                InputPtTB.RenderTransform = scale;
                InputPtTBList.Add(InputPtTB);
                can.Children.Add(InputPtTB);
            }
        }
        #endregion

        #region 记录输入点,绘制影响线（用户）
        Point InputPt = new Point();
        List<Point> InputPtList = new List<Point>();
        Line UserInfluLine1 = new Line();
        List<Line> UserInfluLineList1 = new List<Line>();
        List<LineModelClass> UserInfluClassList1 = new List<LineModelClass>();
        LineModelClass UserInfluClass1 = new LineModelClass();

        Line tempLine5 = new Line();
        List<Line> tempLine5List = new List<Line>();

        private void UserInfluLine()
        {
            for (int as1 = 0; as1 < InputPtList.Count; as1++)//未输入点默认在杆上
            {
                for (int is1 = 0; is1 < RedPointList.Count; is1++)
                {
                    if (RedPointList[is1].X == InputPtList[as1].X)
                    {
                        if (RedPointList[is1].Y == m_LineModelList[0].Line_BeginPoint.Y)
                        {
                            RedPointList.RemoveAt(is1);
                        }
                    }
                }
            }

            for (int js1 = 0; js1 < RedPointList.Count; js1++)
            {
                if (WJSelectPt.X != 0)
                {
                    if (RedPointList[js1].X == WJSelectPt.X - 5)
                    {
                        RedPointList.RemoveAt(js1);
                        js1 = 0;
                    }
                    if (RedPointList[js1].X == WJSelectPt.X + 5)
                    {
                        RedPointList.RemoveAt(js1);
                        js1 = 0;
                    }
                }
                if (JLSelectPt.X != 0)
                {
                    if (RedPointList[js1].X == JLSelectPt.X - 5)
                    {
                        RedPointList.RemoveAt(js1);
                        js1 = 0;
                    }
                    if (RedPointList[js1].X == JLSelectPt.X + 5)
                    {
                        RedPointList.RemoveAt(js1);
                        js1 = 0;
                    }
                }
            }

            for (int bs1 = 0; bs1 < RedPointList.Count; bs1++)
            {
                InputPtList.Add(RedPointList[bs1]);
            }
            InputPtList = InputPtList.Distinct().ToList();

            InputPtList.Sort((p1, p2) => p1.Y.CompareTo(p2.Y));
            InputPtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));
            for (int cs1 = 0; cs1 < InputPtList.Count - 1; cs1++)//用户绘制的影响线
            {
                UserInfluLine1 = new Line();
                UserInfluLine1.Stroke = Brushes.Blue;
                UserInfluLine1.StrokeThickness = 1;
                UserInfluLine1.X1 = InputPtList[cs1].X;
                UserInfluLine1.Y1 = InputPtList[cs1].Y;
                UserInfluLine1.X2 = InputPtList[cs1 + 1].X;
                UserInfluLine1.Y2 = InputPtList[cs1 + 1].Y;

                UserInfluClass1.Line_BeginPoint.X = UserInfluLine1.X1;
                UserInfluClass1.Line_BeginPoint.Y = UserInfluLine1.Y1;
                UserInfluClass1.Line_EndPoint.X = UserInfluLine1.X2;
                UserInfluClass1.Line_EndPoint.Y = UserInfluLine1.Y2;
                UserInfluClass1.LineK = (UserInfluClass1.Line_EndPoint.Y - UserInfluClass1.Line_BeginPoint.Y) / (UserInfluClass1.Line_EndPoint.X - UserInfluClass1.Line_BeginPoint.X);
                UserInfluClass1.LineLength = UserInfluClass1.Line_EndPoint.Y - UserInfluClass1.Line_BeginPoint.Y;

                UserInfluClassList1.Add(UserInfluClass1);
                UserInfluClass1 = new LineModelClass();

                UserInfluLineList1.Add(UserInfluLine1);
            }
            //显示用
            for (int bs1 = 0; bs1 < RedPointList.Count; bs1++)
            {
                ShowUserPtList.Add(RedPointList[bs1]);
            }
            ShowUserPtList = ShowUserPtList.Distinct().ToList();
            ShowUserPtList.Sort((p1, p2) => (-p1.Y).CompareTo(-p2.Y));
            ShowUserPtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));
            for (int cs1 = 0; cs1 < ShowUserPtList.Count - 1; cs1++)//用户绘制的影响线
            {
                ShowUserLine = new Line();
                ShowUserLine.Stroke = Brushes.Blue;
                ShowUserLine.StrokeThickness = 1;
                ShowUserLine.X1 = ShowUserPtList[cs1].X;
                ShowUserLine.Y1 = ShowUserPtList[cs1].Y;
                ShowUserLine.X2 = ShowUserPtList[cs1 + 1].X;
                ShowUserLine.Y2 = ShowUserPtList[cs1 + 1].Y;

                ShowUserLineList.Add(ShowUserLine);
                can.Children.Add(ShowUserLine);
            }
            //闭合影响线
            if (ShowUserPtList[0].Y != m_LineModelList[0].Line_BeginPoint.Y)
            {
                tempLine5 = new Line();
                tempLine5.Stroke = Brushes.Blue;
                tempLine5.StrokeThickness = 1;
                tempLine5.X1 = ShowUserPtList[0].X;
                tempLine5.Y1 = ShowUserPtList[0].Y;
                tempLine5.X2 = m_LineModelList[0].Line_BeginPoint.X;
                tempLine5.Y2 = m_LineModelList[0].Line_BeginPoint.Y;
                tempLine5List.Add(tempLine5);
                can.Children.Add(tempLine5);
            }
            if (ShowUserPtList[ShowUserPtList.Count - 1].Y != m_LineModelList[m_LineModelList.Count - 1].Line_EndPoint.Y)
            {
                tempLine5 = new Line();
                tempLine5.Stroke = Brushes.Blue;
                tempLine5.StrokeThickness = 1;
                tempLine5.X1 = ShowUserPtList[ShowUserPtList.Count - 1].X;
                tempLine5.Y1 = ShowUserPtList[ShowUserPtList.Count - 1].Y;
                tempLine5.X2 = m_LineModelList[m_LineModelList.Count - 1].Line_EndPoint.X;
                tempLine5.Y2 = m_LineModelList[m_LineModelList.Count - 1].Line_EndPoint.Y;
                tempLine5List.Add(tempLine5);
                can.Children.Add(tempLine5);
            }

            if (InfluClassList3.Count > 0)
            {
                for (int bs2 = 0; bs2 < UserInfluClassList1.Count; bs2++)
                {
                    if (UserInfluClassList1[bs2].Line_BeginPoint.X < JLSelectPt.X - 0.01 && JLSelectPt.X - 0.01 <= UserInfluClassList1[bs2].Line_EndPoint.X)
                    {
                        UserInfluClassList1[bs2].isSelectBeam = true;
                    }
                    if (UserInfluClassList1[bs2].Line_BeginPoint.X <= JLSelectPt.X + 0.01 && JLSelectPt.X + 0.01 < UserInfluClassList1[bs2].Line_EndPoint.X)
                    {
                        UserInfluClassList1[bs2].isSelectBeam = true;
                    }
                }
            }
        }

        #endregion

        #region 结果判定
        int CheckNum = 0;
        TextBlock WrongNotice1 = new TextBlock();
        List<TextBlock> WrongNoticeList1 = new List<TextBlock>();

        TextBlock WrongNotice2 = new TextBlock();
        List<TextBlock> WrongNoticeList2 = new List<TextBlock>();

        TextBlock WrongNotice3 = new TextBlock();
        List<TextBlock> WrongNoticeList3 = new List<TextBlock>();
        Line WrongLine3 = new Line();

        List<Line> WrongLineList3 = new List<Line>();

        TextBlock WrongNotice4 = new TextBlock();
        List<TextBlock> WrongNoticeList4 = new List<TextBlock>();
        Line WrongLine4 = new Line();
        List<Line> WrongLineList4 = new List<Line>();

        int FLNum = 0, FLONum = 0, JLNum = 0, WJNum = 0;

        Image crossImage = new Image();
        List<Image> crossImageList = new List<Image>();
        private void CheckAnswerBt_Click(object sender, RoutedEventArgs e)
        {
            CheckNum = 0;
            if (InputPtList.Count == 0)
            {
                MessageBox.Show("请输入关键点竖标！");
            }
            if (InputPtList.Count > 0)
            {
                InputPtList = InputPtList.Distinct().ToList();
                InputPtList.Sort((p1, p2) => p1.Y.CompareTo(p2.Y));
                InputPtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                UserInfluLine();

                if ((InfluClassList.Count > 0 && FLNum == 0) || (InfluClassList2.Count > 0 && FLNum == 0))
                {
                    List<Point> CorrectPtList = new List<Point>();
                    int degree = 0;
                    CorrectPtList.Clear();

                    for (int i = 0; i < m_LineModelList.Count; i++)
                    {
                        for (int j = 0; j < m_LineModelList[i].LineKnewPtList.Count; j++)
                        {
                            CorrectPtList.Add(m_LineModelList[i].LineKnewPtList[j]);
                        }
                    }
                    CorrectPtList = CorrectPtList.Distinct().ToList();
                    CorrectPtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                    ShowUserPtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                    for (int i = 0; i < CorrectPtList.Count; i++)
                    {
                        if (CorrectPtList[i].Y <= ShowUserPtList[i].Y + 10 && CorrectPtList[i].Y >= ShowUserPtList[i].Y - 10)
                        {
                            degree++;
                        }
                    }
                    MessageBox.Show("您的得分为：" + (100 * degree / CorrectPtList.Count).ToString());
                }

                if ((InfluClassList3.Count > 0 && FLNum == 0))
                {
                    List<Point> CorrectPtList = new List<Point>();
                    int degree = 0;
                    CorrectPtList.Clear();
                    Point point = new Point();
                    for (int i = 0; i < JLNewLineList.Count; i++)
                    {
                        for (int j = 0; j < JLNewLineList[i].LineKnewPtList.Count; j++)
                        {
                            point.X = Math.Round(JLNewLineList[i].LineKnewPtList[j].X);
                            point.Y = JLNewLineList[i].LineKnewPtList[j].Y;
                            CorrectPtList.Add(point);
                        }
                    }
                    CorrectPtList = CorrectPtList.Distinct().ToList();
                    CorrectPtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                    ShowUserPtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                    for (int i = 0; i < ShowUserPtList.Count; i++)
                    {
                        if (CorrectPtList[i].Y <= ShowUserPtList[i].Y + 5 && CorrectPtList[i].Y >= ShowUserPtList[i].Y - 5)
                        {
                            degree++;
                        }
                    }
                    MessageBox.Show("您的得分为：" + (100 * degree / ShowUserPtList.Count).ToString());
                }

                if ((InfluClassList4.Count > 0 && FLNum == 0))
                {
                    List<Point> CorrectPtList = new List<Point>();
                    int degree = 0;
                    CorrectPtList.Clear();
                    for (int i = 0; i < WJNewLineList.Count; i++)
                    {
                        for (int j = 0; j < WJNewLineList[i].LineKnewPtList.Count; j++)
                        {
                            CorrectPtList.Add(WJNewLineList[i].LineKnewPtList[j]);
                        }
                    }
                    CorrectPtList = CorrectPtList.Distinct().ToList();
                    CorrectPtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                    ShowUserPtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                    for (int i = 0; i < ShowUserPtList.Count; i++)
                    {
                        if (CorrectPtList[i].Y <= ShowUserPtList[i].Y + 10 && CorrectPtList[i].Y >= ShowUserPtList[i].Y - 10)
                        {
                            degree++;
                        }
                    }
                    MessageBox.Show("您的得分为：" + (100 * degree / ShowUserPtList.Count).ToString());
                }


                //结果判定
                #region 方向相反

                #region 支座反力影响线
                if (InfluClassList.Count > 0 && FLNum == 0)//支座反力影响线
                {
                    for (int ar1 = 0; ar1 < InputPtList.Count; ar1++)
                    {
                        if (selectPt.X == InputPtList[ar1].X)
                        {
                            if (selectPt.Y >= m_LineModelList[0].Line_BeginPoint.Y
                                && InputPtList[ar1].Y < m_LineModelList[0].Line_BeginPoint.Y)
                            {
                                WrongNotice1 = new TextBlock();
                                WrongNotice1.Text = "错误提示：方向错误！";
                                WrongNotice1.FontSize = 18;
                                WrongNotice1.Background = Brushes.Yellow;
                                WrongNotice1.FontWeight = FontWeights.Bold;
                                WrongNotice1.Margin = new Thickness(840 * ResolutionRatio, 340 * ResolutionRatio, 0, 0);
                                WrongNotice1.TextWrapping = TextWrapping.WrapWithOverflow;
                                WrongNotice1.Width = 100;
                                ScaleTransform scale = new ScaleTransform();
                                scale.ScaleY = -1;
                                WrongNotice1.RenderTransform = scale;
                                WrongNoticeList1.Add(WrongNotice1);
                                can.Children.Add(WrongNotice1);


                                FLNum++;
                            }
                            if (selectPt.Y <= m_LineModelList[0].Line_BeginPoint.Y
                                && InputPtList[ar1].Y > m_LineModelList[0].Line_BeginPoint.Y)
                            {
                                WrongNotice1 = new TextBlock();
                                WrongNotice1.Text = "错误提示：方向错误！";
                                WrongNotice1.FontSize = 18;
                                WrongNotice1.Background = Brushes.Yellow;
                                WrongNotice1.FontWeight = FontWeights.Bold;
                                WrongNotice1.Margin = new Thickness(840 * ResolutionRatio, 340 * ResolutionRatio, 0, 0);
                                WrongNotice1.TextWrapping = TextWrapping.WrapWithOverflow;
                                WrongNotice1.Width = 100;
                                ScaleTransform scale = new ScaleTransform();
                                scale.ScaleY = -1;
                                WrongNotice1.RenderTransform = scale;
                                WrongNoticeList1.Add(WrongNotice1);
                                can.Children.Add(WrongNotice1);

                                FLNum++;
                            }
                        }
                    }
                }
                #endregion

                #region 支座反力偶
                if (InfluClassList2.Count > 0 && FLONum == 0)//支座反力偶
                {
                    for (int br1 = 0; br1 < UserInfluClassList1.Count; br1++)
                    {
                        if (selectPt2.X == UserInfluClassList1[br1].Line_BeginPoint.X)
                        {
                            if (UserInfluClassList1[br1].LineK > 0)
                            {
                                WrongNotice1 = new TextBlock();
                                WrongNotice1.Text = "错误提示：方向错误！";
                                WrongNotice1.FontSize = 18;
                                WrongNotice1.Background = Brushes.Yellow;
                                WrongNotice1.FontWeight = FontWeights.Bold;
                                WrongNotice1.Margin = new Thickness(840 * ResolutionRatio, 340 * ResolutionRatio, 0, 0);
                                WrongNotice1.TextWrapping = TextWrapping.WrapWithOverflow;
                                WrongNotice1.Width = 100;
                                ScaleTransform scale = new ScaleTransform();
                                scale.ScaleY = -1;
                                WrongNotice1.RenderTransform = scale;
                                WrongNoticeList1.Add(WrongNotice1);
                                can.Children.Add(WrongNotice1);

                                FLONum++;
                            }
                        }
                    }
                }
                #endregion

                #region 剪力影响线
                if (InfluClassList3.Count > 0 && JLNum == 0)//剪力影响线
                {
                    for (int jl = 0; jl < InfluClassList3.Count; jl++)
                    {
                        if (InfluClassList3[jl].isSelectBeam == true)
                        {
                            InputPtList.Sort((p1, p2) => p1.X.CompareTo(p2.X));
                            for (int b3 = 0; b3 < InputPtList.Count - 1; b3++)
                            {
                                if (InputPtList[b3].X == InfluClassList3[jl].Line_EndPoint.X)
                                {
                                    if (InputPtList[b3].Y - m_LineModelList[0].Line_EndPoint.Y > 0)
                                    {
                                        WrongNotice1 = new TextBlock();
                                        WrongNotice1.Text = "错误提示：方向错误！";
                                        WrongNotice1.FontSize = 18;
                                        WrongNotice1.Background = Brushes.Yellow;
                                        WrongNotice1.FontWeight = FontWeights.Bold;
                                        WrongNotice1.Margin = new Thickness(840 * ResolutionRatio, 340 * ResolutionRatio, 0, 0);
                                        WrongNotice1.TextWrapping = TextWrapping.WrapWithOverflow;
                                        WrongNotice1.Width = 100;
                                        ScaleTransform scale = new ScaleTransform();
                                        scale.ScaleY = -1;
                                        WrongNotice1.RenderTransform = scale;
                                        WrongNoticeList1.Add(WrongNotice1);
                                        can.Children.Add(WrongNotice1);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 弯矩影响线
                if (InfluClassList4.Count > 0 && WJNum == 0)//弯矩影响线
                {
                    for (int b4 = 0; b4 < InputPtList.Count; b4++)
                    {
                        if (InputPtList[b4].X == WJSelectPt.X)
                        {
                            if (InputPtList[b4].Y - m_LineModelList[0].Line_EndPoint.Y < 0)
                            {
                                WrongNotice1 = new TextBlock();
                                WrongNotice1.Text = "错误提示：方向错误！";
                                WrongNotice1.FontSize = 18;
                                WrongNotice1.Background = Brushes.Yellow;
                                WrongNotice1.FontWeight = FontWeights.Bold;
                                WrongNotice1.Margin = new Thickness(840 * ResolutionRatio, 340 * ResolutionRatio, 0, 0);
                                WrongNotice1.TextWrapping = TextWrapping.WrapWithOverflow;
                                WrongNotice1.Width = 100;
                                ScaleTransform scale = new ScaleTransform();
                                scale.ScaleY = -1;
                                WrongNotice1.RenderTransform = scale;
                                WrongNoticeList1.Add(WrongNotice1);
                                can.Children.Add(WrongNotice1);
                            }
                        }
                    }
                    for (int w = 0; w < UserInfluClassList1.Count; w++)
                    {
                        if (UserInfluClassList1[w].Line_BeginPoint.X == WJSelectPt.X)
                        {
                            if (UserInfluClassList1[w].LineK > 0)
                            {
                                WrongNotice1 = new TextBlock();
                                WrongNotice1.Text = "错误提示：方向错误！";
                                WrongNotice1.FontSize = 18;
                                WrongNotice1.Background = Brushes.Yellow;
                                WrongNotice1.FontWeight = FontWeights.Bold;
                                WrongNotice1.Margin = new Thickness(840 * ResolutionRatio, 340 * ResolutionRatio, 0, 0);
                                WrongNotice1.TextWrapping = TextWrapping.WrapWithOverflow;
                                WrongNotice1.Width = 100;
                                ScaleTransform scale = new ScaleTransform();
                                scale.ScaleY = -1;
                                WrongNotice1.RenderTransform = scale;
                                WrongNoticeList1.Add(WrongNotice1);
                                can.Children.Add(WrongNotice1);
                            }
                        }
                    }
                }
                #endregion
                #endregion

                #region 剪力影响线，两侧不平行
                if (InfluClassList3.Count > 0 && JLNum == 0)//剪力
                {
                    List<LineModelClass> tempK = new List<LineModelClass>();
                    InfluClassList3.Sort((l1, l2) => l1.Line_BeginPoint.X.CompareTo(l2.Line_BeginPoint.X));
                    UserInfluClassList1.Sort((l3, l4) => l3.Line_BeginPoint.X.CompareTo(l4.Line_BeginPoint.X));
                    for (int l = 0; l < UserInfluClassList1.Count; l++)
                    {
                        if (UserInfluClassList1[l].isSelectBeam == true)
                        {
                            tempK.Add(UserInfluClassList1[l]);
                        }
                    }
                    if (tempK[0].LineK != tempK[1].LineK)
                    {
                        WrongNotice3 = new TextBlock();
                        WrongNotice3.Text = "错误提示：被选截面左右斜率不相等！";
                        WrongNotice3.FontSize = 18;
                        WrongNotice3.Background = Brushes.Yellow;
                        WrongNotice3.FontWeight = FontWeights.Bold;
                        WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 340 * ResolutionRatio, 0, 0);
                        WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                        WrongNotice3.Width = 100;
                        ScaleTransform scale = new ScaleTransform();
                        scale.ScaleY = -1;
                        WrongNotice3.RenderTransform = scale;
                        WrongNoticeList3.Add(WrongNotice3);
                        can.Children.Add(WrongNotice3);
                    }
                }
                #endregion

                #region 约束条件
                #region 支座反力影响线
                if (InfluClassList.Count > 0)
                {
                    for (int z = 0; z < zzList.Count; z++)
                    {
                        if (zzList[z].zzPoint.X != comboList[fhID].zzPoint.X)//不是被选支座
                        {
                            #region 铰支座
                            if (Convert.ToInt32(zzList[z].zzTag) == 1
                                || Convert.ToInt32(zzList[z].zzTag) == 2)//铰支座
                            {
                                for (int z2 = 0; z2 < InputPtList.Count; z2++)
                                {
                                    if (zzList[z].zzPoint.X == InputPtList[z2].X)
                                    {
                                        if (zzList[z].zzPoint.Y != InputPtList[z2].Y)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);

                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 固定端
                            if (Convert.ToInt32(zzList[z].zzTag) == 3)//固定端
                            {
                                for (int z3 = 0; z3 < UserInfluClassList1.Count; z3++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z3].Line_BeginPoint.X)
                                    {
                                        if (UserInfluClassList1[z3].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 固定端2
                            if (Convert.ToInt32(zzList[z].zzTag) == 4)//固定端
                            {
                                for (int z4 = 0; z4 < UserInfluClassList1.Count; z4++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z4].Line_EndPoint.X)
                                    {
                                        if (UserInfluClassList1[z4].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 定向支座
                            if (Convert.ToInt32(zzList[z].zzTag) == 6)//定向支座
                            {
                                for (int z5 = 0; z5 < UserInfluClassList1.Count; z5++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z5].Line_BeginPoint.X
                                        || zzList[z].zzPoint.X == UserInfluClassList1[z5].Line_EndPoint.X)
                                    {
                                        if (UserInfluClassList1[z5].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 定向支座2
                            if (Convert.ToInt32(zzList[z].zzTag) == 9)//定向支座
                            {
                                for (int z6 = 0; z6 < UserInfluClassList1.Count; z6++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z6].Line_BeginPoint.X
                                        || zzList[z].zzPoint.X == UserInfluClassList1[z6].Line_EndPoint.X)
                                    {
                                        if (UserInfluClassList1[z6].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 定向支座3
                            if (Convert.ToInt32(zzList[z].zzTag) == 5)//定向支座
                            {
                                for (int z7 = 0; z7 < UserInfluClassList1.Count; z7++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z7].Line_BeginPoint.X)
                                    {
                                        if (UserInfluClassList1[z7].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 定向支座4
                            if (Convert.ToInt32(zzList[z].zzTag) == 8)//定向支座
                            {
                                for (int z8 = 0; z8 < UserInfluClassList1.Count; z8++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z8].Line_EndPoint.X)
                                    {
                                        if (UserInfluClassList1[z8].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        if (zzList[z].zzPoint.X == comboList[fhID].zzPoint.X)//是被选支座
                        {
                            #region 固定端
                            if (Convert.ToInt32(zzList[z].zzTag) == 3)
                            {
                                for (int z33 = 0; z33 < UserInfluClassList1.Count; z33++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z33].Line_BeginPoint.X)
                                    {
                                        if (UserInfluClassList1[z33].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 固定端2
                            if (Convert.ToInt32(zzList[z].zzTag) == 4)
                            {
                                for (int z34 = 0; z34 < UserInfluClassList1.Count; z34++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z34].Line_EndPoint.X)
                                    {
                                        if (UserInfluClassList1[z34].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 定向支座3
                            if (Convert.ToInt32(zzList[z].zzTag) == 5)
                            {
                                for (int z35 = 0; z35 < UserInfluClassList1.Count; z35++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z35].Line_BeginPoint.X)
                                    {
                                        if (UserInfluClassList1[z35].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 定向支座4
                            if (Convert.ToInt32(zzList[z].zzTag) == 8)
                            {
                                for (int z36 = 0; z36 < UserInfluClassList1.Count; z36++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z36].Line_EndPoint.X)
                                    {
                                        if (UserInfluClassList1[z36].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 定向支座
                            if (Convert.ToInt32(zzList[z].zzTag) == 6)//定向支座
                            {
                                for (int z37 = 0; z37 < UserInfluClassList1.Count; z37++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z37].Line_BeginPoint.X
                                        || zzList[z].zzPoint.X == UserInfluClassList1[z37].Line_EndPoint.X)
                                    {
                                        if (UserInfluClassList1[z37].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }

                                #endregion

                                #region 定向支座2
                                if (Convert.ToInt32(zzList[z].zzTag) == 9)//定向支座
                                {
                                    for (int z38 = 0; z38 < UserInfluClassList1.Count; z38++)
                                    {
                                        if (zzList[z].zzPoint.X == UserInfluClassList1[z38].Line_BeginPoint.X
                                            || zzList[z].zzPoint.X == UserInfluClassList1[z38].Line_EndPoint.X)
                                        {
                                            if (UserInfluClassList1[z38].LineK != 0)
                                            {
                                                WrongNotice3 = new TextBlock();
                                                WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                                WrongNotice3.FontSize = 18;
                                                WrongNotice3.Background = Brushes.Yellow;
                                                WrongNotice3.FontWeight = FontWeights.Bold;
                                                WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                                WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                                WrongNotice3.Width = 100;
                                                ScaleTransform scale = new ScaleTransform();
                                                scale.ScaleY = -1;
                                                WrongNotice3.RenderTransform = scale;
                                                WrongNoticeList3.Add(WrongNotice3);
                                                can.Children.Add(WrongNotice3);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                #endregion

                #region 支座反力偶
                if (InfluClassList2.Count > 0)//支座反力偶
                {
                    for (int z = 0; z < zzList.Count; z++)
                    {
                        if (zzList[z].zzPoint.X != comboList2[fhID2].zzPoint.X)//不是被选支座
                        {
                            #region 铰支座
                            if (Convert.ToInt32(zzList[z].zzTag) == 1
                                || Convert.ToInt32(zzList[z].zzTag) == 2)//铰支座
                            {
                                for (int z2 = 0; z2 < InputPtList.Count; z2++)
                                {
                                    if (zzList[z].zzPoint.X == InputPtList[z2].X)
                                    {
                                        if (zzList[z].zzPoint.Y != InputPtList[z2].Y)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 固定端
                            if (Convert.ToInt32(zzList[z].zzTag) == 3)//固定端
                            {
                                for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_BeginPoint.X)
                                    {
                                        if (UserInfluClassList1[z2].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 固定端2
                            if (Convert.ToInt32(zzList[z].zzTag) == 4)//固定端
                            {
                                for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_EndPoint.X)
                                    {
                                        if (UserInfluClassList1[z2].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 定向支座
                            if (Convert.ToInt32(zzList[z].zzTag) == 6)//定向支座
                            {
                                for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_BeginPoint.X
                                        || zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_EndPoint.X)
                                    {
                                        if (UserInfluClassList1[z2].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 定向支座2
                            if (Convert.ToInt32(zzList[z].zzTag) == 9)//定向支座
                            {
                                for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_BeginPoint.X
                                        || zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_EndPoint.X)
                                    {
                                        if (UserInfluClassList1[z2].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 定向支座3
                            if (Convert.ToInt32(zzList[z].zzTag) == 5)//定向支座
                            {
                                for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_BeginPoint.X)
                                    {
                                        if (UserInfluClassList1[z2].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 定向支座4
                            if (Convert.ToInt32(zzList[z].zzTag) == 8)//定向支座
                            {
                                for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_EndPoint.X)
                                    {
                                        if (UserInfluClassList1[z2].LineK != 0)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        if (zzList[z].zzPoint.X == comboList2[fhID2].zzPoint.X)//是被选支座
                        {
                            #region 铰支座
                            if (Convert.ToInt32(zzList[z].zzTag) == 1
                                || Convert.ToInt32(zzList[z].zzTag) == 2)//铰支座
                            {
                                for (int z2 = 0; z2 < InputPtList.Count; z2++)
                                {
                                    if (zzList[z].zzPoint.X == InputPtList[z2].X)
                                    {
                                        if (zzList[z].zzPoint.Y != InputPtList[z2].Y)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 固定端
                            if (Convert.ToInt32(zzList[z].zzTag) == 3)//固定端
                            {
                                for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_BeginPoint.X)
                                    {
                                        if (zzList[z].zzPoint.Y != UserInfluClassList1[z2].Line_BeginPoint.Y)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 固定端2
                            if (Convert.ToInt32(zzList[z].zzTag) == 4)//固定端
                            {
                                for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                                {
                                    if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_EndPoint.X)
                                    {
                                        if (zzList[z].zzPoint.Y != UserInfluClassList1[z2].Line_EndPoint.Y)
                                        {
                                            WrongNotice3 = new TextBlock();
                                            WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                            WrongNotice3.FontSize = 18;
                                            WrongNotice3.Background = Brushes.Yellow;
                                            WrongNotice3.FontWeight = FontWeights.Bold;
                                            WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                            WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                            WrongNotice3.Width = 100;
                                            ScaleTransform scale = new ScaleTransform();
                                            scale.ScaleY = -1;
                                            WrongNotice3.RenderTransform = scale;
                                            WrongNoticeList3.Add(WrongNotice3);
                                            can.Children.Add(WrongNotice3);
                                        }
                                    }
                                }
                            }
                            #endregion

                        }
                    }
                }
                #endregion

                #region 弯矩、剪力
                if (InfluClassList3.Count > 0 || InfluClassList4.Count > 0)
                {
                    for (int z = 0; z < zzList.Count; z++)
                    {
                        #region 铰支座
                        if (Convert.ToInt32(zzList[z].zzTag) == 1
                            || Convert.ToInt32(zzList[z].zzTag) == 2)//铰支座
                        {
                            for (int z2 = 0; z2 < InputPtList.Count; z2++)
                            {
                                if (zzList[z].zzPoint.X == InputPtList[z2].X)
                                {
                                    if (zzList[z].zzPoint.Y != InputPtList[z2].Y)
                                    {
                                        WrongNotice3 = new TextBlock();
                                        WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                        WrongNotice3.FontSize = 18;
                                        WrongNotice3.Background = Brushes.Yellow;
                                        WrongNotice3.FontWeight = FontWeights.Bold;
                                        WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                        WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                        WrongNotice3.Width = 100;
                                        ScaleTransform scale = new ScaleTransform();
                                        scale.ScaleY = -1;
                                        WrongNotice3.RenderTransform = scale;
                                        WrongNoticeList3.Add(WrongNotice3);
                                        can.Children.Add(WrongNotice3);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 固定端
                        if (Convert.ToInt32(zzList[z].zzTag) == 3)//固定端
                        {
                            for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                            {
                                if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_BeginPoint.X)
                                {
                                    if (UserInfluClassList1[z2].LineK != 0)
                                    {
                                        WrongNotice3 = new TextBlock();
                                        WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                        WrongNotice3.FontSize = 18;
                                        WrongNotice3.Background = Brushes.Yellow;
                                        WrongNotice3.FontWeight = FontWeights.Bold;
                                        WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                        WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                        WrongNotice3.Width = 100;
                                        ScaleTransform scale = new ScaleTransform();
                                        scale.ScaleY = -1;
                                        WrongNotice3.RenderTransform = scale;
                                        WrongNoticeList3.Add(WrongNotice3);
                                        can.Children.Add(WrongNotice3);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 固定端2
                        if (Convert.ToInt32(zzList[z].zzTag) == 4)//固定端
                        {
                            for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                            {
                                if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_EndPoint.X)
                                {
                                    if (UserInfluClassList1[z2].LineK != 0)
                                    {
                                        WrongNotice3 = new TextBlock();
                                        WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                        WrongNotice3.FontSize = 18;
                                        WrongNotice3.Background = Brushes.Yellow;
                                        WrongNotice3.FontWeight = FontWeights.Bold;
                                        WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                        WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                        WrongNotice3.Width = 100;
                                        ScaleTransform scale = new ScaleTransform();
                                        scale.ScaleY = -1;
                                        WrongNotice3.RenderTransform = scale;
                                        WrongNoticeList3.Add(WrongNotice3);
                                        can.Children.Add(WrongNotice3);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 定向支座
                        if (Convert.ToInt32(zzList[z].zzTag) == 6)//定向支座
                        {
                            for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                            {
                                if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_BeginPoint.X
                                    || zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_EndPoint.X)
                                {
                                    if (UserInfluClassList1[z2].LineK != 0)
                                    {
                                        WrongNotice3 = new TextBlock();
                                        WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                        WrongNotice3.FontSize = 18;
                                        WrongNotice3.Background = Brushes.Yellow;
                                        WrongNotice3.FontWeight = FontWeights.Bold;
                                        WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                        WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                        WrongNotice3.Width = 100;
                                        ScaleTransform scale = new ScaleTransform();
                                        scale.ScaleY = -1;
                                        WrongNotice3.RenderTransform = scale;
                                        WrongNoticeList3.Add(WrongNotice3);
                                        can.Children.Add(WrongNotice3);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 定向支座2
                        if (Convert.ToInt32(zzList[z].zzTag) == 9)//定向支座
                        {
                            for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                            {
                                if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_BeginPoint.X
                                    || zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_EndPoint.X)
                                {
                                    if (UserInfluClassList1[z2].LineK != 0)
                                    {
                                        WrongNotice3 = new TextBlock();
                                        WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                        WrongNotice3.FontSize = 18;
                                        WrongNotice3.Background = Brushes.Yellow;
                                        WrongNotice3.FontWeight = FontWeights.Bold;
                                        WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                        WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                        WrongNotice3.Width = 100;
                                        ScaleTransform scale = new ScaleTransform();
                                        scale.ScaleY = -1;
                                        WrongNotice3.RenderTransform = scale;
                                        WrongNoticeList3.Add(WrongNotice3);
                                        can.Children.Add(WrongNotice3);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 定向支座3
                        if (Convert.ToInt32(zzList[z].zzTag) == 5)//定向支座
                        {
                            for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                            {
                                if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_BeginPoint.X)
                                {
                                    if (UserInfluClassList1[z2].LineK != 0)
                                    {
                                        WrongNotice3 = new TextBlock();
                                        WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                        WrongNotice3.FontSize = 18;
                                        WrongNotice3.Background = Brushes.Yellow;
                                        WrongNotice3.FontWeight = FontWeights.Bold;
                                        WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                        WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                        WrongNotice3.Width = 100;
                                        ScaleTransform scale = new ScaleTransform();
                                        scale.ScaleY = -1;
                                        WrongNotice3.RenderTransform = scale;
                                        WrongNoticeList3.Add(WrongNotice3);
                                        can.Children.Add(WrongNotice3);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 定向支座4
                        if (Convert.ToInt32(zzList[z].zzTag) == 8)//定向支座
                        {
                            for (int z2 = 0; z2 < UserInfluClassList1.Count; z2++)
                            {
                                if (zzList[z].zzPoint.X == UserInfluClassList1[z2].Line_EndPoint.X)
                                {
                                    if (UserInfluClassList1[z2].LineK != 0)
                                    {
                                        WrongNotice3 = new TextBlock();
                                        WrongNotice3.Text = "错误提示：" + zzList[z].zzName + "处不满足约束条件！";
                                        WrongNotice3.FontSize = 18;
                                        WrongNotice3.Background = Brushes.Yellow;
                                        WrongNotice3.FontWeight = FontWeights.Bold;
                                        WrongNotice3.Margin = new Thickness(840 * ResolutionRatio, 150 * ResolutionRatio, 0, 0);
                                        WrongNotice3.TextWrapping = TextWrapping.WrapWithOverflow;
                                        WrongNotice3.Width = 100;
                                        ScaleTransform scale = new ScaleTransform();
                                        scale.ScaleY = -1;
                                        WrongNotice3.RenderTransform = scale;
                                        WrongNoticeList3.Add(WrongNotice3);
                                        can.Children.Add(WrongNotice3);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion
                #endregion

                #region 数值不正确
                #region 支座反力影响线
                if (InfluClassList.Count > 0)//支座反力影响线
                {
                    for (int k = 0; k < InfluClassList.Count; k++)
                    {
                        for (int m = 0; m < UserInfluClassList1.Count; m++)
                        {
                            if (InfluClassList[k].Line_BeginPoint.X == UserInfluClassList1[m].Line_BeginPoint.X)
                            {
                                if (Math.Abs(UserInfluClassList1[m].Line_BeginPoint.Y - InfluClassList[k].Line_BeginPoint.Y) > Math.Abs(0.2))
                                {
                                    WrongNotice4 = new TextBlock();
                                    WrongNotice4.Text = "错误提示：数值不正确！";
                                    WrongNotice4.FontSize = 18;
                                    WrongNotice4.Background = Brushes.Yellow;
                                    WrongNotice4.FontWeight = FontWeights.Bold;
                                    WrongNotice4.Margin = new Thickness(840 * ResolutionRatio, 50 * ResolutionRatio, 0, 0);
                                    WrongNotice4.TextWrapping = TextWrapping.WrapWithOverflow;
                                    WrongNotice4.Width = 100;
                                    ScaleTransform scale = new ScaleTransform();
                                    scale.ScaleY = -1;
                                    WrongNotice4.RenderTransform = scale;
                                    WrongNoticeList4.Add(WrongNotice4);
                                    can.Children.Add(WrongNotice4);
                                }
                            }
                            if (InfluClassList[k].Line_EndPoint.X == UserInfluClassList1[m].Line_EndPoint.X)
                            {
                                if (Math.Abs(UserInfluClassList1[m].Line_EndPoint.Y - InfluClassList[k].Line_EndPoint.Y) > Math.Abs(0.2))
                                {
                                    WrongNotice4 = new TextBlock();
                                    WrongNotice4.Text = "错误提示：数值不正确！";
                                    WrongNotice4.FontSize = 18;
                                    WrongNotice4.Background = Brushes.Yellow;
                                    WrongNotice4.FontWeight = FontWeights.Bold;
                                    WrongNotice4.Margin = new Thickness(840 * ResolutionRatio, 50 * ResolutionRatio, 0, 0);
                                    WrongNotice4.TextWrapping = TextWrapping.WrapWithOverflow;
                                    WrongNotice4.Width = 100;
                                    ScaleTransform scale = new ScaleTransform();
                                    scale.ScaleY = -1;
                                    WrongNotice4.RenderTransform = scale;
                                    WrongNoticeList4.Add(WrongNotice4);
                                    can.Children.Add(WrongNotice4);
                                }
                            }

                        }
                    }
                }
                #endregion

                #region 支座反力偶
                if (InfluClassList2.Count > 0)//支座反力偶
                {
                    for (int k2 = 0; k2 < InfluClassList2.Count; k2++)
                    {
                        for (int m2 = 0; m2 < UserInfluClassList1.Count; m2++)
                        {
                            if (InfluClassList2[k2].Line_BeginPoint.X == UserInfluClassList1[m2].Line_BeginPoint.X)
                            {
                                if (Math.Abs(UserInfluClassList1[m2].Line_BeginPoint.Y - InfluClassList2[k2].Line_BeginPoint.Y) > Math.Abs(0.2))
                                {
                                    WrongNotice4 = new TextBlock();
                                    WrongNotice4.Text = "错误提示：数值不正确！";
                                    WrongNotice4.FontSize = 18;
                                    WrongNotice4.Background = Brushes.Yellow;
                                    WrongNotice4.FontWeight = FontWeights.Bold;
                                    WrongNotice4.Margin = new Thickness(840 * ResolutionRatio, 50 * ResolutionRatio, 0, 0);
                                    WrongNotice4.TextWrapping = TextWrapping.WrapWithOverflow;
                                    WrongNotice4.Width = 100;
                                    ScaleTransform scale = new ScaleTransform();
                                    scale.ScaleY = -1;
                                    WrongNotice4.RenderTransform = scale;
                                    WrongNoticeList4.Add(WrongNotice4);
                                    can.Children.Add(WrongNotice4);
                                }
                            }
                            if (InfluClassList2[k2].Line_EndPoint.X == UserInfluClassList1[m2].Line_EndPoint.X)
                            {
                                if (Math.Abs(UserInfluClassList1[m2].Line_EndPoint.Y - InfluClassList2[k2].Line_EndPoint.Y) > Math.Abs(0.2))
                                {
                                    WrongNotice4 = new TextBlock();
                                    WrongNotice4.Text = "错误提示：数值不正确！";
                                    WrongNotice4.FontSize = 18;
                                    WrongNotice4.Background = Brushes.Yellow;
                                    WrongNotice4.FontWeight = FontWeights.Bold;
                                    WrongNotice4.Margin = new Thickness(840 * ResolutionRatio, 50 * ResolutionRatio, 0, 0);
                                    WrongNotice4.TextWrapping = TextWrapping.WrapWithOverflow;
                                    WrongNotice4.Width = 100;
                                    ScaleTransform scale = new ScaleTransform();
                                    scale.ScaleY = -1;
                                    WrongNotice4.RenderTransform = scale;
                                    WrongNoticeList4.Add(WrongNotice4);
                                    can.Children.Add(WrongNotice4);
                                }
                            }

                        }
                    }
                }
                #endregion

                #region 剪力影响线
                if (InfluClassList3.Count > 0 && JLNum == 0)//剪力影响线
                {
                    for (int k3 = 0; k3 < InfluClassList3.Count; k3++)
                    {
                        for (int m3 = 0; m3 < UserInfluClassList1.Count; m3++)
                        {
                            if (InfluClassList3[k3].Line_BeginPoint.X == UserInfluClassList1[m3].Line_BeginPoint.X)
                            {
                                if (Math.Abs(UserInfluClassList1[m3].Line_BeginPoint.Y - InfluClassList3[k3].Line_BeginPoint.Y) > Math.Abs(0.2))
                                {
                                    WrongNotice4 = new TextBlock();
                                    WrongNotice4.Text = "错误提示：数值不正确！";
                                    WrongNotice4.FontSize = 18;
                                    WrongNotice4.Background = Brushes.Yellow;
                                    WrongNotice4.FontWeight = FontWeights.Bold;
                                    WrongNotice4.Margin = new Thickness(840 * ResolutionRatio, 50 * ResolutionRatio, 0, 0);
                                    WrongNotice4.TextWrapping = TextWrapping.WrapWithOverflow;
                                    WrongNotice4.Width = 100;
                                    ScaleTransform scale = new ScaleTransform();
                                    scale.ScaleY = -1;
                                    WrongNotice4.RenderTransform = scale;
                                    WrongNoticeList4.Add(WrongNotice4);
                                    can.Children.Add(WrongNotice4);
                                }
                            }
                            if (InfluClassList3[k3].Line_EndPoint.X == UserInfluClassList1[m3].Line_EndPoint.X)
                            {
                                if (Math.Abs(UserInfluClassList1[m3].Line_EndPoint.Y - InfluClassList3[k3].Line_EndPoint.Y) > Math.Abs(0.2))
                                {
                                    WrongNotice4 = new TextBlock();
                                    WrongNotice4.Text = "错误提示：数值不正确！";
                                    WrongNotice4.FontSize = 18;
                                    WrongNotice4.Background = Brushes.Yellow;
                                    WrongNotice4.FontWeight = FontWeights.Bold;
                                    WrongNotice4.Margin = new Thickness(840 * ResolutionRatio, 50 * ResolutionRatio, 0, 0);
                                    WrongNotice4.TextWrapping = TextWrapping.WrapWithOverflow;
                                    WrongNotice4.Width = 100;
                                    ScaleTransform scale = new ScaleTransform();
                                    scale.ScaleY = -1;
                                    WrongNotice4.RenderTransform = scale;
                                    WrongNoticeList4.Add(WrongNotice4);
                                    can.Children.Add(WrongNotice4);
                                }
                            }

                        }
                    }
                }
                #endregion

                #region 弯矩影响线
                if (InfluClassList4.Count > 0 && WJNum == 0)//弯矩影响线
                {
                    for (int k4 = 0; k4 < InfluClassList4.Count; k4++)
                    {
                        for (int m4 = 0; m4 < UserInfluClassList1.Count; m4++)
                        {
                            if (InfluClassList4[k4].Line_BeginPoint.X == UserInfluClassList1[m4].Line_BeginPoint.X)
                            {
                                if (Math.Abs(UserInfluClassList1[m4].Line_BeginPoint.Y - InfluClassList4[k4].Line_BeginPoint.Y) > Math.Abs(0.2))
                                {
                                    WrongNotice4 = new TextBlock();
                                    WrongNotice4.Text = "错误提示：数值不正确！";
                                    WrongNotice4.FontSize = 18;
                                    WrongNotice4.Background = Brushes.Yellow;
                                    WrongNotice4.FontWeight = FontWeights.Bold;
                                    WrongNotice4.Margin = new Thickness(840 * ResolutionRatio, 50 * ResolutionRatio, 0, 0);
                                    WrongNotice4.TextWrapping = TextWrapping.WrapWithOverflow;
                                    WrongNotice4.Width = 100;
                                    ScaleTransform scale = new ScaleTransform();
                                    scale.ScaleY = -1;
                                    WrongNotice4.RenderTransform = scale;
                                    WrongNoticeList4.Add(WrongNotice4);
                                    can.Children.Add(WrongNotice4);
                                }
                            }
                            if (InfluClassList4[k4].Line_EndPoint.X == UserInfluClassList1[m4].Line_EndPoint.X)
                            {
                                if (Math.Abs(UserInfluClassList1[m4].Line_EndPoint.Y - InfluClassList4[k4].Line_EndPoint.Y) > Math.Abs(0.2))
                                {
                                    WrongNotice4 = new TextBlock();
                                    WrongNotice4.Text = "错误提示：数值不正确！";
                                    WrongNotice4.FontSize = 18;
                                    WrongNotice4.Background = Brushes.Yellow;
                                    WrongNotice4.FontWeight = FontWeights.Bold;
                                    WrongNotice4.Margin = new Thickness(840 * ResolutionRatio, 50 * ResolutionRatio, 0, 0);
                                    WrongNotice4.TextWrapping = TextWrapping.WrapWithOverflow;
                                    WrongNotice4.Width = 100;
                                    ScaleTransform scale = new ScaleTransform();
                                    scale.ScaleY = -1;
                                    WrongNotice4.RenderTransform = scale;
                                    WrongNoticeList4.Add(WrongNotice4);
                                    can.Children.Add(WrongNotice4);
                                }
                            }

                        }
                    }
                }
                #endregion
                #endregion

                #region 弯矩影响线，所选截面两侧斜率相对转向错误
                if (InfluClassList4.Count > 0 && WJNum == 0)//弯矩影响线
                {
                    for (int zx = 0; zx < UserInfluClassList1.Count; zx++)
                    {
                        if (UserInfluClassList1[zx].Line_EndPoint.X == WJSelectPt.X)//截面左侧
                        {
                            for (int zx1 = 0; zx1 < UserInfluClassList1.Count; zx1++)
                            {
                                if (UserInfluClassList1[zx1].Line_BeginPoint.X == WJSelectPt.X)//截面右侧
                                {
                                    if ((UserInfluClassList1[zx].LineK - UserInfluClassList1[zx1].LineK) <= 0)//两侧k值差为负
                                    {
                                        WrongNotice4 = new TextBlock();
                                        WrongNotice4.Text = "所选截面两侧相对转动方向错误！";
                                        WrongNotice4.FontSize = 18;
                                        WrongNotice4.Background = Brushes.Yellow;
                                        WrongNotice4.FontWeight = FontWeights.Bold;
                                        WrongNotice4.Margin = new Thickness(840 * ResolutionRatio, 50 * ResolutionRatio, 0, 0);
                                        WrongNotice4.TextWrapping = TextWrapping.WrapWithOverflow;
                                        WrongNotice4.Width = 100;
                                        ScaleTransform scale = new ScaleTransform();
                                        scale.ScaleY = -1;
                                        WrongNotice4.RenderTransform = scale;
                                        WrongNoticeList4.Add(WrongNotice4);
                                        can.Children.Add(WrongNotice4);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                CheckNum++;
            }
        }
        #endregion

        #endregion

        #region 交互模块清空
        private void UserSectionClear()
        {
            keyPt1 = new Point();
            keyPt2 = new Point();
            keyPt3 = new Point();
            keyPt4 = new Point();
            foreach (Ellipse a in RedEllipseList)
            {
                can.Children.Remove(a);
            }
            RedPointList.Clear();
            RedEllipseList.Clear();
            foreach (TextBlock b in KeyPtTBList)
            {
                can.Children.Remove(b);
            }
            KeyPtTBList.Clear();
            foreach (TextBlock c in InputPtTBList)
            {
                can.Children.Remove(c);
            }
            InputPtTBList.Clear();
            foreach (Ellipse d in InputPtEllipseList)
            {
                can.Children.Remove(d);
            }
            InputPtEllipseList.Clear();
            foreach (Line e in UserInfluLineList1)
            {
                can.Children.Remove(e);
            }

            InputPtList.Clear();
            UserInfluLineList1.Clear();
            UserInfluClassList1.Clear();
            foreach (Line f in tempLine5List)
            {
                can.Children.Remove(f);
            }
            foreach (TextBlock h in WrongNoticeList1)
            {
                can.Children.Remove(h);
            }
            WrongNoticeList1.Clear();
            foreach (TextBlock j in WrongNoticeList2)
            {
                can.Children.Remove(j);
            }
            WrongNoticeList2.Clear();
            foreach (Line k in WrongLineList3)
            {
                can.Children.Remove(k);
            }
            WrongLineList3.Clear();
            foreach (TextBlock o in WrongNoticeList3)
            {
                can.Children.Remove(o);
            }
            WrongNoticeList3.Clear();

            FLNum = 0; FLONum = 0; JLNum = 0; WJNum = 0;
            foreach (Line k in WrongLineList4)
            {
                can.Children.Remove(k);
            }
            WrongLineList4.Clear();
            foreach (TextBlock o in WrongNoticeList4)
            {
                can.Children.Remove(o);
            }
            WrongNoticeList4.Clear();

            foreach (Image CI in crossImageList)
            {
                can.Children.Remove(CI);
            }
            crossImageList.Clear();
            crossImage = new Image();

            foreach (Line SL in ShowUserLineList)
            {
                can.Children.Remove(SL);
            }
            ShowUserLineList.Clear();
            ShowUserLine = new Line();
            ShowUserPtList.Clear();
        }
        #endregion
    }
}