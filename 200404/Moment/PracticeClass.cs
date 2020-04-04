using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;


namespace WpfApplication3
{
    public class PracticeClass
    {
        public PracticeClass()
        {

        }
        ///<summary>
        ///记录弯矩关键点的位置
        ///</summary>
        public Point prac_pt;

        ///<summary>
        ///弯矩关键点所在杆件号
        /// </summary>
        public int line_Num;

        ///<summary>
        ///弯矩关键点与起点距离
        /// </summary>
        public double dis;

        ///<summary>
        ///记录点的种类（区分均布载荷）
        ///</summary>
        public int prac_Style;

        ///<summary>
        ///记录点对应的圆形图标的位置
        ///</summary>
        public Point prac_Ep;

        ///<summary>
        ///对应的圆形图标
        /// </summary>
        public Ellipse prac_Ellip;

        ///<summary>
        ///对应弯矩值点
        ///</summary>
        public Point prac_bend;

        /// <summary>
        /// 对应弯矩值大小
        /// </summary>
        public double wanju;

        /// <summary>
        /// 按照比例尺缩小的弯矩值
        /// </summary>
        public double wan;
    }
}
