using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace WpfApplication3
{
    public class ZhizuoModelClass
    {
       
            public ZhizuoModelClass()
            {

            }
            /// <summary>
            /// 支座类型
            /// </summary>
            public int Zhizuo_Style;
            /// <summary>
            /// 支座位置
            /// </summary>
            public Point Zhizuo_pt;

            /// <summary>
            /// 当一个支座在节点上时，防止同一个支座在列未知量时候被重复列出
            /// </summary>
            public int Zhizuo_panduan;

            /// <summary>
            /// 支座杆件号
            /// </summary>
            public int Z_Line_Num;

            /// <summary>
            /// 支座距离杆端距离
            /// </summary>
            public double Z_Length;

            /// <summary>
            /// 判断支座是否在杆件上:1是杆件上、2是节点上
            /// </summary>
            public int ganjian;

            /// <summary>
            /// 判断是否在杆件或者节点上
            /// </summary>
            
           
        }
    
}
