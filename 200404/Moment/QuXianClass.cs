using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication3
{
    public class QuXianClass
    {
        public QuXianClass()
        {

        }
        /// <summary>
        /// 左端点或者上端点
        /// </summary>
        public Point left_pt;

        /// <summary>
        /// 中间点
        /// </summary>
        public Point mid_pt;

        /// <summary>
        /// 右端点或者下端点
        /// </summary>
        public Point right_pt;

        /// <summary>
        /// 曲线的中点
        /// </summary>
        public Point bend_pt;

        /// <summary>
        /// 1表示横杆上均布载荷的曲线，2表示竖杆
        /// </summary>
        public int qu_Style;

        /// <summary>
        /// 曲线中弯矩值大小
        /// </summary>
        public double qu_M;

        /// <summary>
        /// 按照比例尺缩小后的弯矩值
        /// </summary>
        public double qu_wan;
    }
}
