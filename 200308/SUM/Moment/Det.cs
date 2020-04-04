using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication3
{
    public class MatrixCls
    {
        double[,] A;
        //m行n列
        int m, n;
        public MatrixCls(int am, int an)
        {
            m = am;
            n = an;
            A = new double[m, n];
        }

        public MatrixCls()
        {
            // TODO: Complete member initialization
        }
        public int getM
        {
            get
            {
                return m;
            }
        }
        public int getN
        {
            get
            {
                return n;
            }
        }
        public double[,] Detail
        {
            get
            {
                return A;
            }
            set
            {
                A = value;
            }
        }
        /// <summary>
        /// 对应行列式的代数余子式矩阵
        /// </summary>
        /// <param name="Ma"></param>
        /// <returns></returns>
        public static MatrixCls MatrixSpa(MatrixCls Ma, int ai, int aj)
        {
            int m = Ma.getM;
            int n = Ma.getN;
            if (m != n)
            {
                Exception myException = new Exception("数组维数不匹配");
                throw myException;
            }
            int n2 = n - 1;
            MatrixCls Mc = new MatrixCls(n2, n2);
            double[,] a = Ma.Detail;
            double[,] b = Mc.Detail;
            //左上
            for (int i = 0; i < ai; i++)
                for (int j = 0; j < aj; j++)
                {
                    b[i, j] = a[i, j];
                }
            //右下
            for (int i = ai; i < n2; i++)
                for (int j = aj; j < n2; j++)
                {
                    b[i, j] = a[i + 1, j + 1];
                }
            //右上
            for (int i = 0; i < ai; i++)
                for (int j = aj; j < n2; j++)
                {
                    b[i, j] = a[i, j + 1];
                }
            //左下
            for (int i = ai; i < n2; i++)
                for (int j = 0; j < aj; j++)
                {
                    b[i, j] = a[i + 1, j];
                }
            //符号位
            if ((ai + aj) % 2 != 0)
            {
                for (int i = 0; i < n2; i++)
                    b[i, 0] = -b[i, 0];
            }
            return Mc;
        }
        /// <summary>
        /// 矩阵的行列式
        /// </summary>
        /// <param name="Ma"></param>
        /// <returns></returns>
        public static double MatrixDet(MatrixCls Ma)
        {
            int m = Ma.getM;
            int n = Ma.getN;
            if (m != n)
            {
                Exception myException = new Exception("数组维数不匹配");
                throw myException;
            }
            double[,] a = Ma.Detail;
            if (n == 1) return a[0, 0];
            double D = 0;
            for (int i = 0; i < n; i++)
            {
                D += a[1, i] * MatrixDet(MatrixSpa(Ma, 1, i));
            }
            return D;
        }
    }
}
