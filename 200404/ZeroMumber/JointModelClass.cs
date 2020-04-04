using System;
using System.Windows;
using System.Windows.Shapes;

namespace ZeroMumber
{
    class JointModelClass
    {
        /// <summary> 
        /// 初始化函数
        /// </summary>
        public JointModelClass()
        {
        }

        ///<summary>
        ///铰节点编号
        ///</summary>
        public int Joint_Num;

        ///<summary>
        ///铰节点连接杆件数
        ///</summary>
        public int Joint_Count;

        ///<summary>
        ///铰节点所连接杆件序号
        ///</summary>
        public int[] Joint_Line = new int[10];

        ///<summary>
        ///铰节点位置
        ///</summary>
        public Point Joint_Point;

        ///<summary>
        ///铰节点上施加载荷编号,不受载荷为零
        ///</summary>
        public int Joint_Load;

        ///<summary>
        ///铰节点上的支座类型，0无支座，1为X向，2为Y向，3为均有
        ///</summary>
        public int Joint_Zhizuo;
    }
}
