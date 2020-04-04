using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes ;
using System.Windows;
using System.Windows.Documents;

namespace Influential
{
    public  class zzClass
    {  
        /// <summary> 
        /// 支座信息
        /// </summary>        
        public  zzClass ()
        {

        }      
        ///<summary>
        ///支座名称
        ///</summary>        
        public string zzName { get; set; }
        ///<summary>
        ///支座Tag
        ///</summary>
        public object zzTag;
        ///<summary>
        ///支座点
        ///</summary>
        public Point zzPoint;
        ///<summary>
        ///支座对应杆
        ///</summary>
        public int zzGan;
        ///<summary>
        ///具体信息
        ///</summary>
        public Rectangle zzInfo;
        ///<summary>
        ///选择ID
        ///</summary>
        public int zzID;
        ///<summary>
        ///支座自由度
        ///</summary>
        public int zzZYD;
        ///<summary>
        ///是否被选中
        ///</summary>
        public bool isselected = false;

        ///<summary>
        ///影响线计算时的支座杆号
        ///</summary>
        public int influzzGan;
 
    }
}
