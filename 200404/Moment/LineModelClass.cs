using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace WpfApplication3
{
    public class LineModelClass
    {
        /// <summary>
        /// 
        /// 初始化函数
        /// </summary>
        public LineModelClass()
        {
        }


        /// <summary>
        /// 线名称
        /// </summary>
        public string LineName;
        /// <summary>
        /// 线长
        /// </summary>
        public double LineLength;

        /// <summary>
        /// 起点
        /// </summary>
        public Point Line_BeginPoint;

        /// <summary>
        /// 终点
        /// </summary>
        public Point Line_EndPoint;

        ///<summury>
        ///水平竖直：水平0,竖直1
        ///</summury>
        public int Line_Style;

        /// <summary>
        /// 线段具体信息
        /// </summary>
        public Line LineInfo;

        public int m_LineNum;
    }
}
