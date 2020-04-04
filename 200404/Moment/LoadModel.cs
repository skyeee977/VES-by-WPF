using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace WpfApplication3
{
    public class LoadModel
    {
        ///<summary>
        ///初始化函数
        ///</summary>
        public LoadModel()
        {

        }

        /// <summary>
        /// 载荷类型：1竖直集中力、3水平集中力、5竖杆均布载荷、7横杆均布载荷、9横杆集中力偶、11竖杆集中力偶
        /// </summary>
        public int Load_Style;

        /// <summary>
        /// 对应位置的弯矩值大小
        /// </summary>
        public double MM;

        /// <summary>
        /// 等于9意味着是弯矩点在曲线上
        /// </summary>
        public int jbzh;

        /// <summary>
        ///对应弯矩值的点 
        /// </summary>
        public Point Load_Bend;

        ///<summary>
        ///3表示普通截面，4表示均布载荷起始点、5表示均布载荷终点、0表示均布载荷衍生出来的小载荷
        /// </summary>
        public int jiemian;

        /// <summary>
        /// 载荷位置：铰节点1、刚节点2、杆件3
        /// </summary>
        public int Load_Location;
        
        /// <summary>
        /// 载荷所在杆件序号
        /// </summary>
        public int Load_Line_Num;
        
        /// <summary>
        /// 载荷所在节点序号
        /// </summary>
        public int Load_Joint_Num;
        
        /// <summary>
        /// 载荷所在点与起始点的距离
        /// </summary>
        public double Load_Length;

        /// <summary>
        /// 载荷位置
        /// </summary>
        public Point Load_pt;

        /// <summary>
        /// 集中力
        /// </summary>
        public double Load_F;
        
        ///<summary>
        ///均布载荷长度
        ///</summary>
        public double Load_l;

        ///<summary>
        ///均布载荷大小
        ///</summary>
        public double Load_q;

        ///<summary>
        ///力矩
        ///</summary>
        public double Load_M;
    }
}
