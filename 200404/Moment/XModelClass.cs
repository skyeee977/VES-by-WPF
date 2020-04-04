using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace WpfApplication3
{
    public class XModelClass
    {
        public XModelClass()
        {

        }
        
        /// <summary>
        /// X的值
        /// </summary>
        public double X;
        
        /// <summary>
        /// X的种类：1是x方向力；2是y方向力；3是M
        /// </summary>
        public int X_Style;
        
        /// <summary>
        /// X的杆件号
        /// </summary>
        public int X_Line_Num;
        
        /// <summary>
        ///X的作用点距离杆件的 Begin_Point的距离大小; 
        /// </summary>
        public double X_Dis;

        
        /// <summary>
        ///X作用点 
        /// </summary>
        public Point X_pt;
        
        /// <summary>
        ///节点信息，0不在节点上，1铰节点，2刚节点 
        /// </summary>
        public int X_Joint_Style;
        
        /// <summary>
        ///X所在的节点号 
        /// </summary>
        public int X_Joint_Num;
        
        /// <summary>
        ///X的支座号 
        /// </summary>
        public int X_zhizuo;
    }
}
