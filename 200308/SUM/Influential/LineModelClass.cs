using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Influential
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
        /// 起点
        /// </summary>
        public Point Line_EndPoint;

        /// <summary>
        /// 线段具体信息
        /// </summary>
        public Line LineInfo;

        ///<summary>
        ///斜率
        ///</summary>
        public double LineK=10000000;

        ///<summary>
        ///是否画过
        ///</summary>
        public bool isDrew = false;

        ///<summary>
        ///杆上已知点数
        ///</summary>
        public int LineKnewPtNum = 0;

        ///<summary>
        ///杆上已知点集
        ///</summary>
        public List<Point> LineKnewPtList=new List<Point> ();

        ///<summary>
        ///是否是被选梁
        ///</summary>
        public bool isSelectBeam = false;

        ///<summary>
        ///是否是简支梁
        ///</summary>
        public bool isSSBeam = false;

        
    }
}
