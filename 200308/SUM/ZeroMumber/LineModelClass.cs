using System;
using System.Windows;
using System.Windows.Shapes;

namespace ZeroMumber
{
    class LineModelClass
    {
        /// <summary>
        /// 
        /// 初始化函数
        /// </summary>
        public LineModelClass()
        {
        }

        /// <summary>
        /// 角度
        /// </summary>
        public double Line_Angle;

        /// <summary>
        /// 起点
        /// </summary>
        public Point Line_BeginPoint;

        /// <summary>
        /// 终点
        /// </summary>
        public Point Line_EndPoint;

        ///<summury>
        ///类型：普通false 竖直true
        ///</summury>
        public Boolean Line_Style;

        ///<summury>
        ///节点1
        ///</summury>
        public int Line_Joint1;

        ///<summury>
        ///节点2
        ///</summury>
        public int Line_Joint2;

        /// <summary>
        /// 线段具体信息
        /// </summary>
        public Line Line_Info;

        /// <summary>
        /// 杆件是否为零杆
        /// </summary>
        public bool Line_IsZero;
    }
}
