using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsControlLibrary1
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Influential.Window1 win1 = new Influential.Window1();
            win1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WpfApplication3.Window3 win3 = new WpfApplication3.Window3();
            win3.ShowDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ZeroMumber.MainWindow win5 = new ZeroMumber.MainWindow();
            win5.ShowDialog();
        }

    }
}
