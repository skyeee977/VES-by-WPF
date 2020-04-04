using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroMumber
{
    class LoadModelClass
    {
        ///<summary>
        ///初始化函数
        ///</summary>
        public LoadModelClass()
        {

        }
        /// <summary>
        /// 载荷类型：普通集中力false、竖直集中力true
        /// </summary>
        public bool Load_Style;

        /// <summary>
        /// 载荷作用节点号
        /// </summary>
        public int Load_Joint;

        /// <summary>
        /// 载荷施加角度
        /// </summary>
        public double Load_Angle;

        /// <summary>
        /// 载荷正负，正向true，负向false
        /// </summary>
        public bool Load_Sign;

        /// <summary>
        /// 无符号载荷分量，X向
        /// </summary>
        public double Load_X;

        /// <summary>
        /// 无符号载荷分量，Y向
        /// </summary>
        public double Load_Y;
    }
}
