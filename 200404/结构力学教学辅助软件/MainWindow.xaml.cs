using System.Windows;


namespace 结构力学教学辅助软件
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

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Influential.Window1 window1 = new Influential.Window1();
            window1.ShowDialog();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            WpfApplication3.Window3 window2 = new WpfApplication3.Window3();
            window2.ShowDialog();
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            ZeroMumber.MainWindow window3 = new ZeroMumber.MainWindow();
            window3.ShowDialog();
        }
    }
}
