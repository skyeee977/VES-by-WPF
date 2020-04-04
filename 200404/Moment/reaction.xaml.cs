using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication3
{
    /// <summary>
    /// reaction.xaml 的交互逻辑
    /// </summary>
    public partial class reaction : Window
    {
        public reaction(List<ZhizuoModelClass> m_ZhizuoList, List<XModelClass> x_dList, double bilichi)
        {
            InitializeComponent();
            for (int i = 0; i < x_dList.Count; i++)
            {
                for (int k = 0; k < m_ZhizuoList.Count; k++)
                {
                    if ((m_ZhizuoList[k].Zhizuo_pt.X == x_dList[i].X_pt.X)
                        && (m_ZhizuoList[k].Zhizuo_pt.Y == x_dList[i].X_pt.Y))
                    {
                        if (x_dList[i].X_Style == 1)
                        {
                            string dir;
                            if (x_dList[i].X >= 0)
                            {
                                dir = "向右";
                            }
                            else { dir = "向左"; }

                            textblock.Text += "支座" + (k + 1) + "水平方向反力为：" + dir + Math.Abs(x_dList[i].X / 20 / bilichi) + '\n';
                        }

                        if (x_dList[i].X_Style == 2)
                        {
                            string dir;
                            if (x_dList[i].X >= 0)
                            {
                                dir = "向上";
                            }
                            else { dir = "向下"; }
                            textblock.Text += "支座" + (k + 1) + "竖直方向反力为：" + dir + Math.Abs(x_dList[i].X / 20 / bilichi) + '\n';
                        }

                        if (x_dList[i].X_Style == 3)
                        {
                            string dir;
                            if (x_dList[i].X >= 0)
                            {
                                dir = "逆时针方向";
                            }
                            else { dir = "顺时针方向"; }
                            textblock.Text += "支座" + (k + 1) + "的支座反力偶为：" + dir + Math.Abs(x_dList[i].X / 400 / bilichi / bilichi) + '\n';
                        }
                    }
                }
            }
        }
    }
}
