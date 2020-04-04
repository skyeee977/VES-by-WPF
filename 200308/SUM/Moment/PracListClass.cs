using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication3
{
    public class PracListClass
    {
        public PracListClass()
        {

        }
        ///<summary>
        ///每根杆件上关键点信息列表
        /// </summary>
        public List<PracticeClass> praList = new List<PracticeClass>();

        ///<summary>
        ///列表里关键点的个数
        /// </summary>
        public int pra_Num;
    }
}
