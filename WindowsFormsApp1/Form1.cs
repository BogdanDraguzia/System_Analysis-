
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ZedGraph;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        const int N = 3;
        const double e = 0.00001;
        bool Approximate = false;
        Stream XStream;
        Stream OStream;
        Graphics gPanel;

        List<Stream> YStream = new List<Stream>();

        double[][,] X = new
            double[N][,];

        double[][,] ZG;

        double[][] Y,
            MinXG,
            MaxXG,
            YG;

        double[] MinYG, MaxYG;
        int[] dG;
        double[][][][] PolyCoef;
        double l, zt;
        GraphPane myPane;

        public Form1()
        {
            InitializeComponent();
            openFileDialog2.Multiselect = true;
            gPanel = panel1.CreateGraphics();
            myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = "Результати";
            myPane.XAxis.Title.Text = "X ";
            myPane.YAxis.Title.Text = "Y ";
        }

        public class Polynom
        {
            double[] A;

            public Polynom(double[] a)
            {
                A = (double[]) a.Clone();
            }

            public double Method(double x)
            {
                double res = 0;
                for (int i = 0; i < A.Length; i++)
                {
                    double t = A[i];
                    for (int j = 0; j < i; j++) t *= x;
                    res += t;
                }

                return res;
            }
        }

        double Chebishev(int P, double x)
        {
            if (P > 4) P = 4;
            if (P == 0) return 1;
            if (P == 1) return x;
            return 2 * x * Chebishev(P - 1, x) - Chebishev(P - 2, x);
        }

        double SChebishev(int P, double x)
        {
            if (P > 4) P = 4;
            if (P == 0) return 1;
            return Chebishev(P, 2 * x - 1);
        }

        double Lagger(int P, double x)
        {
            if (P > 4) P = 4;
            if (P == 0) return 1;
            if (P == 1)
                return 1 - x;
            return (2 * (P - 1) + 1 - x) * Lagger(P - 1, x) - (P - 1) * (P - 1) * Lagger(P - 2, x);
        }

        double Legander(int P, double x)
        {
            if (P > 4) P = 4;
            if (P == 0) return 1;
            if (P == 1) return x;
            return ((2 * (P - 1) + 1) * x * Legander(P - 1, x) - (P - 1) * Legander(P - 2, x)) /
                   P;
        }

        double Hermith(int P,
            double x)
        {
            if (P > 4) P = 4;
            if (P == 0) return 1;
            if (P == 1) return x;
            return x * Hermith(P - 1, x) - (P - 1) * Hermith(P - 2, x);
        }

        double[] PChebishev(int P)
        {
            if (P > 4) P = 4;
            double[] ans = new double[P + 1];
            if (P == 0)
            {
                ans[0] = 1;
                return ans;
            }

            if (P == 1)
            {
                ans[0] = 0;
                ans[1] = 1;
                return ans;
            }

            if (P == 2)
            {
                ans[0] = -1;
                ans[2] = 2;
                return ans;
            }

            if (P == 3)
            {
                ans[3] = 4;
                ans[1] = -3;
                return ans;
            }

            if (P == 4)
            {
                ans[4] = 8;
                ans[2] = -8;
                ans[0] = 1;
                return ans;
            }

            double[] t1 = PChebishev(P - 1);
            double[] t2 = PChebishev(P - 2);
            ans[0] = -t2[0];
            for (int i = 1; i < P - 1; i++)
                ans[i] = 2 * t1[i - 1] - t2[i];
            ans[P - 1] = 2 * t1[P - 2];
            ans[P] = 2 * t1[P - 1];
            return ans;
        }

        double[] PSChebishev(int P)
        {
            if (P > 4) P = 4;
            double[] ans = new double[P + 1];
            if (P == 0)
            {
                ans[0] = 1;
                return ans;
            }

            if (P == 1)
            {
                ans[0] = 0;
                ans[1] = 1;
                return ans;
            }

            if (P == 2)
            {
                ans[0] = -1;
                ans[2] = 2;
                return ans;
            }

            if (P == 3)
            {
                ans[3] = 4;
                ans[1] = -3;
                return ans;
            }

            if (P == 4)
            {
                ans[4] = 8;
                ans[2] = -8;
                ans[0] = 1;
                return ans;
            }

            double[] t1 = PChebishev(P);
            double[] fibo = new double[] {1};
            double[] lfibo = new double[] {1};
            ans[0] = t1[0];
            for (int i = 1; i <= P; i++)
            {
                lfibo = fibo;
                fibo = new double[fibo.Length + 1];
                fibo[0] = fibo[lfibo.Length] = 1;
                for (int j = 1; j < i; j++)
                    fibo[j] = lfibo[j - 1] + lfibo[j];
                double k;
                if ((i % 2) == 0)
                    k = 1;
                else k = -1;

                for (int j = 0; j <= i; j++)
                {
                    ans[j] += k * t1[i] * fibo[j];
                    k *= -2;
                }
            }

            return ans;
        }

        double[] PLagger(int P)
        {
            if (P > 4) P = 4;
            double[] ans = new double[P + 1];
            if (P == 0)
            {
                ans[0] = 1;
                return ans;
            }

            if (P == 1)
            {
                ans[0] = 1;
                ans[1] = -1;
                return ans;
            }

            if (P == 2)
            {
                ans[0] = 1;
                ans[1] = -2;
                ans[2] = 0.5;
                return ans;
            }

            if (P == 3)
            {
                ans[0] = 1;
                ans[1] = -3;
                ans[2] = 3 / 2.0;
                ans[3] = -1 / 6.0;
                return ans;
            }

            double[] t1 = PLagger(P - 1);
            double[] t2 = PLagger(P - 2);
            for (int i = 0; i <= P - 1; i++)
            {
                ans[i] += (2 * (P - 1) + 1) * t1[i];
                ans[i + 1] -= t1[i];
            }

            for (int i = 0; i <= P - 2; i++)
            {
                ans[i] -= ((P - 1) * (P - 1) * t2[i]);
            }

            return ans;
        }

        double[] PLegander(int P)
        {
            if (P > 4) P = 4;
            double[] ans = new double[P + 1];
            if (P == 0)
            {
                ans[0] = 1;
                return ans;
            }

            if (P == 1)
            {
                ans[0]
                    = 0;
                ans[1] = 1;
                return ans;
            }

            double[] t1 = PLegander(P - 1);
            double[] t2 = PLegander(P - 2);
            for (int i = 0; i <= P - 1; i++)
            {
                ans[i + 1] += ((2 * (P - 1) + 1) * t1[i]) / P;
            }

            for (int i = 0; i <= P - 2; i++)
            {
                ans[i] -= ((P - 1) * t2[i]) / P;
            }

            return ans;
        }

        double[] PHermith(int P)
        {
            if (P > 4) P = 4;
            double[] ans = new double[P + 1];
            if (P == 0)
            {
                ans[0] = 1;
                return ans;
            }

            if (P == 1)
            {
                ans[0] = 0;
                ans[1] = 1;
                return ans;
            }

            double[] t1 = PHermith(P - 1);
            double[] t2 = PHermith(P - 2);
            for (int i = 0; i <= P - 1; i++)
            {
                ans[i + 1] += t1[i];
            }

            for (int i = 0; i <= P - 2; i++)
            {
                ans[i] -= (P - 1) * t2[i];
            }

            return ans;
        }


        private double[] minus(double[] a, double[] b) //todo
        {
            double[] t = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                t[i] = a[i] - b[i];
            }

            return t;
        }

        private double norm(double[] a)
        {
            return Math.Sqrt(a.Sum(a_i => a_i * a_i));
        }

        public delegate double
            DelegateF(double[] x);

        private double[] GM(double[] x0, Func<double[], double> F, LinearEnvelope L)
        {
            double[] xt = new double[x0.Length];
            xt = (double[]) x0.Clone();
            xt[0] += 1;
            double[] t = new double[x0.Length];
            double a, lares, tres;
            double dy;
            while (norm(minus(xt, x0)) > e)
            {
                x0 = (double[]) xt.Clone();

                for (int i = 0; i < x0.Length; i++)
                {
                    t = (double[]) xt.Clone();
                    t[i] += e;
                    dy = (F(t) - F(xt)) / e;
                    a = 1;
                    for (int j = 0; j < t.Length; j++)
                        t[j] = 0;
                    t[i] = dy;
                    tres = F(minus(xt, t));
                    do
                    {
                        lares = tres;
                        a /= 2;
                        t[i] = a * dy;
                        tres = F(minus(xt, t));
                    } while (tres < lares);

                    a *= 2;
                    for (int j = 0; j < t.Length; j++)
                        t[j] = 0;
                    t[i] = a * dy;
                    xt = minus(xt, t);
                }
            }

            return (double[]) xt.Clone();
        }

        private void xload_Click(object sender, EventArgs e)
        {
            var dr = openFileDialog1.ShowDialog();
            if ( dr == DialogResult.OK)
            {
                try
                {
                    if ((XStream = openFileDialog1.OpenFile()) !=null)
                    {
                        Xinput.Text = openFileDialog1.SafeFileName;
                        l = 0.78;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        public class LinearEnvelope
        {
            private double[] A;
            private double[] B;
            private double[,] M;
            private double[] K;
            private int n;

            public
                LinearEnvelope(double[] a,double[] b, int m)
            {
                A =(double[]) a.Clone();
                B =(double[]) b.Clone();
                n = m;
                M = new double[m,m];
                K = new double[m];
                for (int i = 0; i< m;i++)
                for (int j =0; j < m; j++)
                {
                    M[i, j] =0;
                    for (int k = 0;k < A.Length / m; k++)
                        M[i,j] += A[k * m + i] * A[k*m + j];
                }

                for (int i = 0; i < m; i++)
                {
                    K[i] = 0;
                    for (int k = 0; k < A.Length / m; k++)
                        K[i] -= 2 * A[k*m + i] * B[k];
                }
            }

            public double Method(double[] x)
            {
                double res = 0;
                for (int i = 0; i< x.Length; i++)
                    for (int j =0; j < x.Length; j++)
                        res += M[i, j] * x[i] * x[j];
                //res /= 2;
                for (int j = 0; j< x.Length; j++)
                    res += K[j] *x[j];
                return res;
            }

            public double MethodMax(double[] x)
            {
                double res = 0;
                double t = 0;
                for (int i = 0;
                    i
                    < A.Length / n;
                    i++)
                {
                    t = 0;
                    for (int j =
                            0;
                        j < n;
                        j++)
                        t += A[i * n + j]
                             * x[j];
                    t -= B[i];
                    if
                        (Math.Abs(t) > res)
                        res =
                            Math.Abs(t);
                }

                return res;
            }

            public double Lambd(double[] x, double[] v)
            {
                double[] z = new double[v.Length];
                for (int i = 0; i < z.Length; i++)
                {
                    z[i] = 0;
                    for (int j = 0; j < z.Length; j++)
                        z[i] += M[i, j] * x[j];
                    z[i] *= 2;
                    z[i] += K[i];
                }

                double res = 0,t = 0;
                for (int i = 0; i < z.Length; i++)
                    res += z[i] *v[i];
                for (int i = 0; i< x.Length;i++)
                    for (int j =0; j < x.Length; j++)
                        t += M[i,j] * v[i] * v[j];
                t *= 2;
                return res / t;
            }

            public double[] grad(double[] x)
            {
                double[] z = new double[x.Length];
                for (int i = 0; i < z.Length; i++)
                {
                    z[i] = 0;
                    for (int j = 0; j < z.Length; j++)
                        z[i] += M[i, j] * x[j];
                    // z[i] *= 2;
                    z[i] += K[i];
                }

                return z;
            }

            static double[] LSMethod(double[][] A, double[] b)
            {
                return Mult(
                        Mult(
                            Inverse(
                                Mult(Transpose(A), A)),
                            Transpose(A))
                        , b);
            }

            static double[][] Transpose(double[][] A)
            {
                double[][] TrA = new double[A[0].Length][];
                for (int i = 0; i < A[0].Length; i++)
                {
                    TrA[i] = new double[A.Length];
                }

                for (int i = 0; i < A.Length; i++)
                {
                    for (int j = 0; j < A[i].Length; j++)
                    {
                        TrA[j][i] = A[i][j];
                    }
                }

                return TrA;
            }

            static double[][] Mult(double[][] A, double[][] B)
            {
                double[][] C = new double[A.Length][];
                for (int i = 0; i < A.Length; i++)
                {
                    C[i] = new double[B[i].Length];
                }

                for (int i = 0; i < A.Length; i++)
                {
                    for (int j = 0; j < B[0].Length; j++)
                    {
                        for (int k = 0; k < B.Length; k++)
                        {
                            C[i][j] += A[i][k] * B[k][j];
                        }
                    }
                }
                return C;
            }

            static double[] Mult(double[][] A, double[] B)
            {
                double[] C = new double[A.Length];
                for (int i = 0; i < A.Length; i++)
                {
                    for (int j = 0; j < B.Length; j++)
                    {
                        C[i] += A[i][j] * B[j];
                    }
                }

                return C;
            }

            static void Print(double[][] A)
            {
                Console.WriteLine("***");
                foreach (var item in A)
                {
                    foreach (var it in item)
                    {
                        Console.Write(it + " ");
                    }
                    Console.WriteLine();
                }
            }

            static void Print(double[] A)
            {
                Console.WriteLine("***");
                foreach (var item in A)
                {
                    Console.WriteLine(item);
                }
            }

            static double[][] Inverse(double[][] A)
            {
                int n = A.Length;
                double[] e;
                double[][] x = new double[n][];
                for (int i = 0; i < n; i++)
                {
                    x[i] = new double[A[i].Length];
                }

                /*
                * solve will
                contain the vector solution for
                the LUP decomposition as we solve
                * for each
                vector of x. We will combine the
                solutions into the double[][]
                array x.
                */
                double[] solve = new double[A.Length];
                int[] P = new int[A.Length];
                double[][] LU = new double[A.Length][];
                for (int i = 0; i < A.Length; i++)
                {
                    LU[i] = new double[A[i].Length];
                    for (int j = 0; j < LU[i].Length; j++)
                    {
                        LU[i][j] = A[i][j];
                    }
                }

                LUDecompose(ref LU, ref P);
                /*
                * Solve AX = e
                for each column ei of the
                identity matrix using LUP
                decomposition
                */
                for (int i = 0; i < n; i++)
                {
                    e = new double[A[i].Length];
                    e[i] = 1;
                    LUSolve(ref e, ref solve, P, LU);
                    for (int j = 0; j < solve.Length; j++)
                    {
                        x[j][i] = solve[j];
                    }
                }

                return x;
            }

            static void LUDecompose(ref double[][] lu, ref int[] indx)
            {
                int i, imax = 0, j, k, n = lu.GetLength(0);
                double big, temp;
                double[] vv = new double[n];
                for (i = 0;i < n; i++)
                {
                    big = 0.0;
                    for (j = 0; j < n; j++)
                        if ((temp = Math.Abs(lu[i][j])) > big)
                            big = temp;
                    if (Math.Abs(big) < 0.000001)
                        throw new
                            Exception("singular matrix");
                    vv[i] = 1.0 / big;
                }

                for (k = 0; k < n; k++)
                {
                    big = 0.0;
                    for (i = k; i < n; i++)
                    {
                        temp = vv[i] * Math.Abs(lu[i][k]);
                        if (temp > big)
                        {
                            big = temp;
                            imax = i;
                        }
                    }

                    if (k != imax)
                    {
                        for (j = 0; j < n; j++)
                        {
                            temp = lu[imax][j];
                            lu[imax][j] = lu[k][j];
                            lu[k][j] = temp;
                        }

                        vv[imax] = vv[k];
                    }

                    indx[k] = imax;
                    for (i = k + 1; i < n; i++)
                    {
                        temp = lu[i][k] /= lu[k][k];
                        for (j = k + 1; j < n; j++)
                            lu[i][j] -= temp * lu[k][j];
                    }
                }
            }

            static void LUSolve(ref double[] b, ref double[] x, int[] indx, double[][] lu)
            {
                if (b.Length != lu.GetLength(0) || x.Length != lu.GetLength(0))
                    throw new Exception("vector dimension problem");
                int n = lu.GetLength(0);
                int i,ii = 0, ip,j;
                double sum = 0;
                for (i = 0;i < n; i++) x[i] = b[i];
                for (i = 0; i < n; i++)
                {
                    ip = indx[i];
                    sum = x[ip];
                    x[ip] = x[i];
                    if (ii != 0)
                        for (j = ii - 1; j < i; j++)
                            sum -= lu[i][j] * x[j];
                    else if (sum!= 0.0)
                        ii = i +1;
                    x[i] = sum;
                }

                for (i = n - 1; i >= 0; i--)
                {
                    sum = x[i];
                    for (j = i + 1; j < n; j++)
                        sum -= lu[i][j] * x[j];
                    x[i] = sum / lu[i][i];
                }
            }
        }

        private void outfilebutton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Текстові файли (*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((OStream = saveFileDialog1.OpenFile()) != null)
                {
                    outfile.Text = saveFileDialog1.FileName;
                    OStream.Close();
                }
            }
        }

        private void addbutton_Click(object sender, EventArgs e)
        {
            var dr = openFileDialog2.ShowDialog();

            if (dr == DialogResult.OK)
            {
                try
                {  
                    foreach (string fileName in openFileDialog2.FileNames)
                    {
                        YStream.Add(File.Open(fileName, FileMode.Open, FileAccess.Read));
                        Yinput.Text += fileName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void clearbutton_Click(object sender, EventArgs e)
        {
            YStream.Clear();
            Yinput.Text = null;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (PolinoType.SelectedIndex < 0)
                PolinoType.SelectedIndex = 0;
            textBox1.Text = null;
            progressBar1.Value = 0;
            progressBar1.Refresh();
            // Error Checking
            if (Xinput.Text == null || outfile.Text == null)
            {
                MessageBox.Show("Введіть вхідні файли");
                return;
            }

            if (YStream.Count != dimy.Value)
            {
                MessageBox.Show("Введіть вхідні файли");
                return;
            }

            OStream = saveFileDialog1.OpenFile();

            // Initializing
            int n = Convert.ToInt32(Range.Value); //n - РАЗМЕР ВЫБОРКИ 
            int[] dimensions_X = new int[N];
            dimensions_X[0] = Convert.ToInt32(dim1.Value);
            X[0] = new double[n, dimensions_X[0]];
            dimensions_X[1] = Convert.ToInt32(dim2.Value);
            X[1] = new double[n, dimensions_X[1]];
            dimensions_X[2] = Convert.ToInt32(dim3.Value);
            X[2] = new double[n, dimensions_X[2]];
            dG = dimensions_X;
            int dy = Convert.ToInt32(dimy.Value);
            Y = new double[dy][];
            for (int i = 0; i < dy; i++) Y[i] = new double[n];
            progressBar1.Value = 5;
            progressBar1.Refresh();
            PolyCoef = new double[dy][][][];
            for (int m = 0; m < dy; m++)
                PolyCoef[m] = new double[N][][];

            // Reading
            StreamReader Xr = new StreamReader(XStream);
            StreamReader[] Yr = new StreamReader[dy];
            for (int i = 0; i < dy; i++)
                Yr[i] = new StreamReader(YStream[i]);
            string t;
            double[] arr;
            try
            {
                t = Xr.ReadToEnd();
                arr = t.Split(' ', '\t', '\r', '\n').Where(s => !string.IsNullOrEmpty(s))
                    .Select(double.Parse).ToArray();
            }
            catch
            {
                MessageBox.Show("Невірний формат файлу Вхідних Даних");
                return;
            }


            //Сначала заполняется столбец X11, затем столбец Х12 и т.д.
            for (int i = 0, k = 0; i < n; i++)//для каждого x от 1 до размера выборки
            {
                
                for (int l = 0; l < N; l++) //каждому иксу от 1 до 3 
                    for (int j = 0; j < dimensions_X[l]; j++, k++) // каждому (иксу)x_l от 1 до размерности вектора x_l
                        X[l][i, j] = arr[k];
            }

            //Сначала заполняется первый столбец игреков, потом второй, и т.д. - Каждый У в отдельном файле
            for (int i = 0; i < dy; i++)
            {
                try
                {
                    t = Yr[i].ReadToEnd();
                    arr = t.Split(' ', '\t', '\r', '\n').Where(item =>!string.IsNullOrEmpty(item))
                        .Select(double.Parse).ToArray();
                }
                catch
                {
                    MessageBox.Show(@"Невірний формат файлу Y[" + (i + 1) + @"]");
                    return;
                }

                for (int j = 0; j < n; j++)
                    Y[i][j] = arr[j];
            }

            progressBar1.Value = 10;
            progressBar1.Refresh();

            // Begin
            //MIN & MAX
            for (int i = 0; i < Convert.ToInt16(Rankx1.Value) - 1; i++)
                l += 0.106 / Math.Pow(2, i);

            zt = (1 - l) * 2;
            Result.Text += "Мінімальні та максимальні значення векторів:\r\n";
            Result.Refresh();
            StreamWriter Out = new StreamWriter(OStream);
            double[][] MinX = new double[N][];
            for (int i = 0; i < N; i++)
                MinX[i] = new double[dimensions_X[i]];

            double[][] MaxX = new double[N][];
            for (int i = 0; i < N; i++)
                MaxX[i] = new double[dimensions_X[i]];
            double[] MinY = new double[dy];
            double[] MaxY = new double[dy];
            MaxYG = MaxY;
            MinYG = MinY;
            MaxXG = MaxX;
            MinXG = MinX;

            for (int i = 0; i < N; i++)
            for (int j = 0; j < dimensions_X[i]; j++)
                MinX[i][j] = MaxX[i][j] = X[i][0, j];

            for (int i = 0;i < N; i++)
            {
                for (int j = 0; j< dimensions_X[i]; j++)
                {
                    for (int q0 =0; q0 < n; q0++)
                    {
                        if
                            (MinX[i][j] > X[i][q0, j])
                            MinX[i][j] = X[i][q0, j];
                        if
                            (MaxX[i][j] < X[i][q0, j])
                            MaxX[i][j] = X[i][q0, j];
                    }
                }
            }

            for (int i = 0; i < dy; i++)
                MinY[i] = MaxY[i] = Y[i][0];
            for (int i = 0; i < dy; i++)
            {
                foreach (double x in Y[i])
                    if (MinY[i] > x)
                        MinY[i] = x;
                foreach (double x in Y[i])
                    if (MaxY[i] < x)
                        MaxY[i] = x;
            }

            for (int i = 0; i < dy; i++)
                Result.Text += "MinY" + (i + 1) + " = " + MinY[i] + "\tMaxY" + (i + 1) + " = " + MaxY[i] + "\r\n";

            progressBar1.Value = 15;
            progressBar1.Refresh();


            // Normalizing
            Result.Refresh();
            double[][,] Z = new double[N][,];
            double[][] ZY = new double[dy][];
            YG = new double[dy][];
            for (int i = 0; i < N; i++)
            {
                Z[i] = new double[n, dimensions_X[i]];
                for (int k = 0; k < n; k++)
                for (int w = 0; w < dimensions_X[i]; w++)
                    Z[i][k, w] = (X[i][k, w] - MinX[i][w]) / (MaxX[i][w] - MinX[i][w]);
            }

            for (int i = 0; i < dy; i++)
            {
                ZY[i] = new double[n];
                for (int k = 0; k < n; k++)
                    ZY[i][k] = (Y[i][k] - MinY[i]) / (MaxY[i] - MinY[i]);
                YG[i] = (double[]) ZY[i].Clone();
            }

            for (int i = 0; i < dy; i++)
            for (int k = 0; k < n; k++)
                YG[i][k] *= l;
            ZG = Z;
            progressBar1.Value = 20;
            progressBar1.Refresh();

            // Bq0
            Result.Refresh();
            Result.Text += "\r\n";
            double[] B = new double[n];
            double Maxy, Miny;
            for (int q0 = 0; q0 < n; q0++)
            {
                Miny = ZY[0][q0];
                Maxy = ZY[0][q0];
                for (int i = 1; i < dy; i++)
                {
                    if (ZY[i][q0] > Maxy) Maxy = ZY[i][q0];
                    if (ZY[i][q0] < Miny) Miny = ZY[i][q0];
                }

                B[q0] = (Maxy + Miny) / 2.0;
            }

            Result.Update();
            progressBar1.Value = 30;
            progressBar1.Refresh();

            // Lambda Search
            Result.Refresh();
            int[] P = new int[N];
            P[0] = Convert.ToInt32(Rankx1.Value);
            P[1] = Convert.ToInt32(Rankx2.Value);
            P[2] = Convert.ToInt32(Rankx3.Value);

            double[][,] lamb = new double[N][,];
            for (int i = 0; i < N; i++)
                lamb[i] = new double[dimensions_X[i], P[i] + 1];
            Func<int, double, double> f = SChebishev;
            if
                (PolinoType.SelectedIndex == 1)
                f = Legander;
            if
                (PolinoType.SelectedIndex == 2)
                f = Lagger;
            if
                (PolinoType.SelectedIndex == 3)
                f = Hermith;

            if (!checkBox1.Checked)
            {
                double[] megalamb = new double[(P[0] + 1) * dimensions_X[0] + (P[1] + 1) * dimensions_X[1] + (P[2] + 1) * dimensions_X[2]];
                double[] T = new double[((P[0] + 1) * dimensions_X[0] + (P[1] + 1) * dimensions_X[1] + (P[2] + 1) * dimensions_X[2]) * n];
                int k = 0;
                for (int q0 = 0; q0 < n; q0++)
                for (int i = 0; i < N; i++)
                for (int j = 0; j < dimensions_X[i]; j++)
                for (int p = 0; p <= P[i]; p++)
                {
                    T[k] = f(p, Z[i][q0, j]);
                    k++;
                }

                LinearEnvelope L = new LinearEnvelope(T, B, (P[0] + 1) * dimensions_X[0] + (P[1] + 1) * dimensions_X[1] + (P[2] + 1) * dimensions_X[2]);
                megalamb = GM(megalamb, L.Method, L);
                k = 0;
                for (int i = 0; i < N; i++)
                for (int j = 0; j < dimensions_X[i]; j++)
                for (int p = 0; p <= P[i]; p++, k++)
                    lamb[i][j, p] = megalamb[k];
            }
            else
                for (int i = 0; i < N; i++)
                {
                    double[] megalamb = new double[(P[i] + 1) * dimensions_X[i]];
                    double[] T = new double[(P[i] + 1) * dimensions_X[i] * n];
                    int k = 0;
                    for (int q0 = 0; q0 < n; q0++)
                    for (int j = 0; j < dimensions_X[i]; j++)
                    for (int p = 0; p <= P[i]; p++)
                    {
                        T[k] = f(p, Z[i][q0, j]);
                        k++;
                    }

                    LinearEnvelope L = new LinearEnvelope(T, B, (P[i] + 1) * dimensions_X[i]);
                    megalamb = GM(megalamb, L.Method, L);
                    k = 0;
                    for (int j = 0; j < dimensions_X[i]; j++)
                    for (int p = 0; p <= P[i]; p++, k++)
                        lamb[i][j, p] = megalamb[k];
                }

            Result.Text += "\r\n===============Lambdas============== = \r\n";
            for (int i = 0; i < N; i++)
            {
                Result.Text += "\tLambda" + (i + 1) + ":\r\n";
                for (int j = 0; j < dimensions_X[i]; j++)
                {
                    for (int p = 0; p <= P[i]; p++)
                        Result.Text += "\t" + lamb[i][j, p].ToString("F4");
                    Result.Text += "\r\n";
                }
            }

            double dispt = 0,
                disp = 0;
            for (int q0 = 0; q0 < n; q0++)
            {
                dispt = 0;
                for (int i = 0; i < N; i++)
                for (int j = 0; j < dimensions_X[i]; j++)
                for (int p = 0; p <= P[i]; p++)
                    dispt += lamb[i][j, p] * f(p, Z[i][q0, j]);
                if (Math.Abs(dispt - B[q0]) > disp)
                    disp = Math.Abs(dispt - B[q0]);
            }

            disp *= zt;
            Result.Text += "\r\nDisprepancy:\t" + disp.ToString("F4") + "\r\n";
            progressBar1.Value = 40;
            progressBar1.Refresh();

            // Psi
            Result.Text += "\r\nPsi:\r\n";
            Result.Text += "in T form(where T is Your Approximation Polynom) \r\n";
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < dimensions_X[i]; j++)
                {
                    Result.Text += "Psi" + (i + 1) + "," + (j + 1) + " = ";
                    for (int p = 0; p <= P[i]; p++)
                    {
                        Result.Text += " + " + lamb[i][j, p].ToString("F4") + " * T" + p + "(x)\t";
                    }

                    Result.Text += "\r\n";
                }
            }

            Result.Text += "\r\nin Polynom form\r\n";
            Func<int, double[]> Pf = PSChebishev;
            if (PolinoType.SelectedIndex == 1)
                Pf = PLegander;
            if (PolinoType.SelectedIndex == 2)
                Pf = PLagger;
            if (PolinoType.SelectedIndex == 3)
                Pf = PHermith;
            double[][][] coef = new double[N][][];
            for (int i = 0; i < N; i++)
                coef[i] = new double[dimensions_X[i]][];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < dimensions_X[i]; j++)
                {
                    coef[i][j] = new double[Math.Max(Math.Max(P[0], P[1]), P[2]) + 1];
                    double[] tmp = new double[Math.Max(Math.Max(P[0], P[1]), P[2]) + 1];
                    Result.Text += "Psi" + (i + 1) + "," + (j + 1) + " = \t";
                    for (int p = 0; p <= P[i]; p++)
                    {
                        tmp = Pf(p);
                        for (int k = 0; k < tmp.Length; k++)
                            coef[i][j][k] += tmp[k] * lamb[i][j, p];
                    }

                    for (int k = coef[i][j].Length - 1; k >= 2; k--)
                        Result.Text += " + " + coef[i][j][k].ToString("F4") + "* x ^ " + k + "\t";
                    if (coef[i][j].Length > 1)
                        Result.Text += " + " + coef[i][j][1].ToString("F4") + "* x\t";
                    Result.Text += " + " + coef[i][j][0].ToString("F4") + "\r\n";
                }
            }

            progressBar1.Value = 50;
            progressBar1.Refresh();

            // F Search
            double[][][] a = new double[dy][][];
            for (int i = 0; i < dy; i++) a[i] = new double[N][];
            for (int i = 0; i < dy; i++)
            for (int j = 0; j < N; j++)
                a[i][j] = new double[dimensions_X[j]];

            for (int m = 0; m < dy; m++)
            {
                for (int i = 0; i < N; i++)
                {
                    double[] megalamb = new double[dimensions_X[i]];
                    double[] T = new double[dimensions_X[i] * n];
                    int k = 0;
                    for (int q0 = 0; q0 < n; q0++)
                    for (int j = 0; j < dimensions_X[i]; j++)
                    {
                        T[k] = new Polynom(coef[i][j]).Method(Z[i][q0, j]);
                        k++;
                    }

                    LinearEnvelope L = new LinearEnvelope(T, ZY[m], dimensions_X[i]);
                    megalamb = GM(megalamb, L.Method, L);
                    for (int j = 0; j < dimensions_X[i]; j++)
                        a[m][i][j] = megalamb[j];
                }
            }

            Result.Text += "\r\n Matrix of || a ||:\r\n";
            for (int m = 0; m < dy; m++)
            {
                Result.Text += "Matrix ||a" + (m + 1) + "||:\r\n";
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < dimensions_X[i]; j++)
                    {
                        Result.Text += "\t" + a[m][i][j].ToString("F4");
                    }

                    Result.Text += "\r\n";
                }
            }

            Result.Text += "\r\n";
            dispt = 0;
            disp = 0;
            for (int m = 0; m < dy; m++)
            for (int i = 0; i < N; i++)
            {
                for (int q0 = 0; q0 < n; q0++)
                {
                    dispt = 0;
                    for (int j = 0; j < dimensions_X[i]; j++)
                        dispt += a[m][i][j] * (new Polynom(coef[i][j]).Method(Z[i][q0, j]));
                    if (Math.Abs(dispt - ZY[m][q0]) > disp)
                        disp = Math.Abs(dispt - ZY[m][q0]);
                }
            }

            disp *= zt;
            Result.Text += "\r\nDisprepancy:\t" + disp.ToString("F4") + "\r\n";
            progressBar1.Value = 60;
            progressBar1.Refresh();


            // F
            Result.Text += "\r\n F:\r\n";
            Result.Text += "in Psi form \r\n";
            for (int m = 0; m < dy; m++)
            {
                for (int i = 0; i < N; i++)
                {
                    Result.Text += "F" + (m + 1) + "," + (i + 1) + " = ";
                    for (int j = 0; j < dimensions_X[i]; j++)
                    {
                        Result.Text += " + " + a[m][i][j].ToString("F4") + " *Psi" + (i + 1) + "," + (j + 1) +
                                       "(X" + (i + 1) + "," + (j + 1) + "[q0])\t";
                    }

                    Result.Text += "\r\n";
                }
            }

            Result.Text += "\r\nin Polynom form\r\n";
            for (int m = 0; m < dy; m++)
            {
                for (int i = 0; i < N; i++)
                {
                    Result.Text += "\r\nF" + (m + 1) + "," + (i + 1) + " = ";
                    for (int j = 0; j < dimensions_X[i]; j++)
                    {
                        for (int k = coef[i][j].Length - 1; k >= 2; k--)
                            Result.Text += " + " + (coef[i][j][k] * a[m][i][j]).ToString("F4") + " *x"
                                           + (i + 1) + "" + (j + 1) + " ^ " + k + "\t";
                        if
                            (coef[i][j].Length > 1)
                            Result.Text += " + " + (coef[i][j][1] * a[m][i][j]).ToString("F4") + " * x"
                                           + (i + 1) + "" + (j + 1) + "\t";
                        Result.Text += " + " + (coef[i][j][0] * a[m][i][j]).ToString("F4") +
                                       "\r\n\t\t";
                    }
                }
            }

            progressBar1.Value = 70;
            progressBar1.Refresh();


            // Search Ф
            double[][] c = new double[dy][];
            for (int m = 0; m < dy; m++) c[m] = new double[N];
            for (int m = 0; m < dy; m++)
            {
                double[] T = new double[N * n];
                for (int q0 = 0; q0 < n; q0++)
                {
                    int k = 0;
                    for (int i = 0; i < N; i++)
                    {
                        double tmp = 0;
                        for (int j = 0; j < dimensions_X[i]; j++)
                            tmp += (new Polynom(coef[i][j]).Method(Z[i][q0, j])) * a[m][i][j];
                        T[k] = tmp;
                        k++;
                    }
                }

                LinearEnvelope L = new LinearEnvelope(T, ZY[m], N);
                c[m] = GM(c[m], L.Method, L);
            }

            Result.Text += "\r\n Matrix of || c ||:\r\n";
            for (int m = 0; m < dy; m++)
            {
                for (int i = 0; i < N; i++)
                    Result.Text += "\t" + c[m][i].ToString("F4");
                Result.Text += "\r\n";
            }

            Result.Text += "\r\n";
            dispt = 0;
            disp = 0;
            for (int m = 0; m < dy; m++)
            for (int q0 = 0; q0 < n; q0++)
            {
                dispt = 0;
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < dimensions_X[i]; j++)
                        dispt += a[m][i][j] * (new Polynom(coef[i][j]).Method(Z[i][q0, j]));
                    dispt *= c[m][i];
                }

                if (Math.Abs(dispt - ZY[m][q0]) > disp)
                    disp = Math.Abs(dispt - ZY[m][q0]);
            }

            disp *= zt;
            Result.Text += "\r\n Disprepancy:\t" + disp.ToString("F4") + "\r\n";
            progressBar1.Value = 80;
            progressBar1.Refresh();


            //Ф
            Result.Text += "\r\n Фi:\r\n";
            Result.Text += "in F form \r\n";
            for (int m = 0; m < dy; m++)
            {
                Result.Text += "Ф" + (m + 1) + " = \t";
                for (int i = 0; i < N; i++)
                    Result.Text += " + " + c[m][i].ToString("F4")
                                         + " * F" + (m + 1) + "," + (i + 1) + "(X" + (i + 1) + "[q0])\t";
                Result.Text += "\r\n";
            }

            Result.Text += "\r\nin Polynom form\r\n";
            for (int m = 0; m < dy; m++)
            {
                Result.Text += "Ф" + (m + 1) + " = \t";
                for (int i = 0; i < N; i++)
                {
                    PolyCoef[m][i] = new double[dimensions_X[i]][];
                    for (int j = 0; j < dimensions_X[i]; j++)
                    {
                        PolyCoef[m][i][j] = new double[Math.Max(Math.Max(P[0], P[1]), P[2]) + 1];
                        for (int k = coef[i][j].Length - 1; k >= 0; k--)
                            PolyCoef[m][i][j][k] = coef[i][j][k] * a[m][i][j] * c[m][i];
                        for (int k = coef[i][j].Length - 1; k >= 2; k--)
                            Result.Text += " + " + (coef[i][j][k] * a[m][i][j] *
                                                    c[m][i]).ToString("F4") + " * x"
                                           + (i + 1) + "" + (j + 1) + "^" +
                                           k + "\t";
                        if (coef[i][j].Length > 1)
                            Result.Text += " + " +
                                           (coef[i][j][1] * a[m][i][j] *
                                            c[m][i]).ToString("F4") + " * x"
                                           + (i + 1) + "" + (j + 1) + "\t";
                        Result.Text += " + " +
                                       (coef[i][j][0] * a[m][i][j] *
                                        c[m][i]).ToString("F4") +
                                       "\t\r\n\t\t";
                    }
                }
            }

            Result.Text += "\r\nin T form (where T is Your Approximation Polynom) \r\n";
            for (int m = 0; m < dy; m++)
            {
                Result.Text += "\r\nФ" + (m + 1) + " = \t";
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < dimensions_X[i]; j++)
                    {
                        for (int p = 0; p <= P[i]; p++)
                        {
                            Result.Text += " + " + (lamb[i][j, p] * a[m][i][j] * c[m][i]).ToString("F4") + " * T"
                                           + p + "(X" + (i + 1) + "," + (j + 1) + "[q0])\t";
                        }

                        Result.Text += "\r\n\t\t";
                    }
                }
            }

            progressBar1.Value = 90;
            progressBar1.Refresh();


            

            //ReNorm
            Result.Text += "\r\n Ф without Normilizing for different Yi:\r\n";
            for (int m = 0; m < dy; m++)
            {
                Result.Text += "in F form \r\n";
                Result.Text += "\r\n Ф" + (m + 1) + " = \r\n";
                for (int i = 0; i < N; i++)
                    Result.Text += " + " + (c[m][i] * (MaxY[m] - MinY[m])).ToString("F4") + " * F"
                                   + (m + 1) + "," + (i + 1) +
                                   "(X[q0])\t";
                Result.Text += "+ " + MinY[m].ToString("F4") + "\r\n";
                Result.Text += "\r\nin Polynom form\r\n";
                Result.Text += "\r\n Ф" + (m + 1) + " = \r\n";
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < dimensions_X[i]; j++)
                    {
                        for (int k = coef[i][j].Length - 1; k >= 2; k--)
                            Result.Text += " + "
                                           + (coef[i][j][k] * a[m][i][j] * c[m][i] * (MaxY[m] - MinY[m]))
                                           .ToString("F4")
                                           + " * x"
                                           + (i + 1) + "" + (j + 1) + "^" +
                                           k + "\t";
                        if (coef[i][j].Length > 1)
                            Result.Text += " + " + (coef[i][j][1] * a[m][i][j] *
                                                    c[m][i] * (MaxY[m] - MinY[m])).ToString("F4") + " * x"
                                           + (i + 1) + "" + (j + 1) + "\t";
                        Result.Text += " + " + (coef[i][j][0] * a[m][i][j] *
                                                c[m][i] * (MaxY[m] -
                                                           MinY[m])).ToString("F4") +
                                       "\t\r\n\t\t";
                    }
                }

                Result.Text += " + " + MinY[m] + "\r\n";
                Result.Text += "\r\nin T form (where T is Your Approximation Polynom) \r\n";
                Result.Text += "\r\nФ = \t";
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < dimensions_X[i]; j++)
                    {
                        for (int p = 0; p <= P[i]; p++)
                        {
                            Result.Text += " + " + (lamb[i][j, p] * a[m][i][j] *
                                                    c[m][i] * (MaxY[m] -
                                                               MinY[m])).ToString("F4") + " * T"
                                           + p + "(X" + (i + 1) + "," + (j +
                                                                         1) + "[q0])\t";
                        }

                        Result.Text += "\r\n\t\t";
                    }
                }

                Result.Text += "+ " + MinY[m] + "\r\n";
            }

            // Max Dif
            double[] diff = new double[dy];
            for (int m = 0; m < dy; m++)
            {
                double tmp = 0;
                for (int q0 = 0; q0 < n; q0++)
                {
                    tmp = 0;
                    for (int i = 0; i < N; i++)
                        for (int j = 0; j < dimensions_X[i]; j++)
                            tmp += (zt * a[m][i][j] * c[m][i]) * (new Polynom(coef[i][j]).Method(Z[i][q0, j]));
                    if (Math.Abs(tmp - ZY[m][q0]) > diff[m])
                        diff[m] = Math.Abs(tmp - ZY[m][q0]);
                }
            }

            textBox1.Text = "";
            for (int m = 0;m < dy; m++)
            {
                textBox1.Text += "max on Y" + (m + 1) + ": " +
                                 (Math.Abs(zt * diff[m]) /**(MaxY[m] - MinY[m])*/).ToString("F4") +
                                 "\r\n";
            }

            Approximate = true;
            Draw1(Convert.ToInt32(Rankx1.Value - 1));
            XStream.Close();
            XStream =openFileDialog1.OpenFile();
            foreach (Stream s in YStream) s.Close();
            YStream.Clear();
            foreach (string fileName in openFileDialog2.FileNames)
                YStream.Add(File.Open(fileName,FileMode.Open, FileAccess.Read));
            Out.Write(Result.Text);
            Out.Close();
            OStream.Close();
            progressBar1.Value = 100;
            progressBar1.Refresh();
        }

        void Draw(int m)
        {
            gPanel.Clear(Color.White);
            Pen p = new Pen(Color.Black, 2);
            int zeroy = panel1.Height - 3;
            int zerox = 3;
            int W = panel1.Width - 6;
            int H = panel1.Height - 6;
            int n = (int) Range.Value;
    
            // variables for coordinate grid system 
            int Num = 10;
            int deltaX = (int) Math.Round(panel1.Width / (double) (Num));
            int deltaY =
                (int) Math.Round(panel1.Height / (double) Num);
            Font arialFont = new Font("Arial", 6);
            Brush blackBrush = new SolidBrush(Color.Black);
            gPanel.DrawLine(p, new Point(zerox, zeroy), new Point(panel1.Width - 3, zeroy));
            gPanel.DrawLine(p,new Point(panel1.Width - 3, zeroy), new Point(panel1.Width - 6, zeroy + 3));
            gPanel.DrawLine(p, new Point(panel1.Width - 3, zeroy), new Point(panel1.Width - 6, zeroy - 3));
            gPanel.DrawLine(p,new Point(3, panel1.Height - 3), new Point(3, 3));
            gPanel.DrawLine(p, new Point(3, 3), new Point(0, 6));
            gPanel.DrawLine(p, new Point(3, 3), new Point(6, 6));
            gPanel.DrawString("0", arialFont, blackBrush, new PointF(zerox, zeroy - 15), new StringFormat());
            
            //gPanel.DrawString(n.ToString(),arialFont, blackBrush, newPointF(zerox + W - 20, zeroy -15), new StringFormat());
            //gPanel.DrawString(Math.Round(MaxYG[m]).ToString(), arialFont, blackBrush, new PointF(zerox, zeroy - H + 5), new StringFormat());
            p = new Pen(Color.Black, 2);
            double xVal = n / Convert.ToDouble(Num - 1); double yVal = MaxYG[m] / Num;
            for (int i = 1;i <=Num;i++)
            {
                //panel1.Width -3
                gPanel.DrawLine(p, new Point(zerox + (i * deltaX), zeroy - 3), 
                    new Point(zerox + i * deltaX, zeroy + 3));
                gPanel.DrawString(Math.Round(xVal * i).ToString(), arialFont, 
                    blackBrush, new PointF(zerox + (i* deltaX) - 5, 
                        zeroy - 15), 
                    new StringFormat());
                gPanel.DrawLine(p, new Point(zerox - 3, zeroy - i * deltaY), new Point(zerox + 3, 
                    zeroy - i * deltaY));
                gPanel.DrawString( Math.Round(yVal * i).ToString(CultureInfo.CurrentCulture),
                    arialFont, blackBrush, 
                    new Point(zerox + 3, zeroy - i * deltaY),
                    new StringFormat());
                
                //gPanel.DrawLine(p, new Point(zerox, zeroy), new Point(panel1.Width - 3, zeroy));
            }

            p = new Pen(Color.Red, 1);
            gPanel.DrawLine(p, new Point(W - 100, zeroy - H + 30), new Point(W - 100 + 30, zeroy - H + 30));
            gPanel.DrawString("Функція", arialFont, 
                new SolidBrush(Color.Red), 
                new PointF(W - 100 + 33,
                    zeroy - H +20),
                new StringFormat());
            for (int q0 = 0; q0 < n - 1; q0++)
            {
                gPanel.DrawLine(p, new Point(zerox + q0 * (W / n),
                    zeroy - (int) (H * (Y[m][q0] / MaxYG[m]))),
                    new Point(zerox + (q0 + 1) * (W / n),
                    zeroy - (int) (H * (Y[m][q0 + 1] / MaxYG[m]))));
            }

            p = new Pen(Color.Green, 1);
            gPanel.DrawLine(p, new Point(W - 100, zeroy - H + 40), 
                new Point(W - 100 + 30, zeroy - H + 40));
            gPanel.DrawString("Апроксимація", arialFont,
                new SolidBrush(Color.Green), 
                new PointF(W - 100 + 33, zeroy - H +30),
                new StringFormat());
            double t = YG[m][0];
            int T;
            int Tprev;
            for (int i = 0; i < N; i++)
            for (int j = 0;j < dG[i]; j++)
            for (int k = 0; k < PolyCoef[m][i][j].Length; k++)
                t += zt * PolyCoef[m][i][j][k] * Math.Pow(ZG[i][0, j], k);

            T = (int) (H * (((MaxYG[m] - MinYG[m]) * t + MinYG[m]) / MaxYG[m]));
            for (int q0 = 1; q0 < n; q0++)
            {
                t = YG[m][q0];
                for (int i = 0; i < N; i++)
                for (int j = 0; j < dG[i]; j++)
                for (int k = 0; k < PolyCoef[m][i][j].Length; k++)
                    t += zt * PolyCoef[m][i][j][k] * Math.Pow(ZG[i][q0, j], k);
                Tprev = T;
                T = (int) (H * (((MaxYG[m] - MinYG[m]) * t + MinYG[m]) / MaxYG[m]));
                gPanel.DrawLine(p, new Point(zerox + (q0 - 1) * (W / n),
                    zeroy - Tprev), new Point(zerox + q0 * (W / n), zeroy - T));
            }
        }

      
       

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
        }

        void Draw1(int m)
        {
            myPane.CurveList.Clear();
            PointPairList listPointsOne = new PointPairList();
            PointPairList listPointsTwo = new PointPairList();
            LineItem myCurveOne;
            LineItem myCurve2;
            myPane.Title.Text = "Results Y_" + m.ToString();
            double _x, _y;
            int n = (int) Range.Value;
            double t = YG[m][0];
            double T;
            double Tprev;
            for (int q0 = 0; q0 < n; q0++)
            {
                _x = q0;
                _y = Y[m][q0];
                listPointsOne.Add(_x, _y);
            }

            //aproximation
            for (int i = 0; i < N; i++)
            for (int j = 0; j < dG[i]; j++)
            for (int k = 0; k < PolyCoef[m][i][j].Length; k++)
                t += zt * PolyCoef[m][i][j][k] * Math.Pow(ZG[i][0, j], k);

            T = ((MaxYG[m] - MinYG[m]) * t + MinYG[m]);
            for (int q0 = 1; q0 < n; q0++)
            {
                t = YG[m][q0];
                for (int i = 0; i < N; i++)
                for (int j = 0; j < dG[i]; j++)
                for (int k = 0; k < PolyCoef[m][i][j].Length; k++)
                    t += zt * PolyCoef[m][i][j][k] * Math.Pow(ZG[i][q0, j], k);
                Tprev = T;
                _x = q0 - 1;
                _y = Tprev;
                listPointsTwo.Add(_x, _y);
                T = ((MaxYG[m] - MinYG[m]) * t + MinYG[m]);
            }

            myCurveOne = myPane.AddCurve(null, listPointsOne, Color.Red, SymbolType.None);
            myCurve2 = myPane.AddCurve(null, listPointsTwo, Color.Green, SymbolType.None);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void dimy_ValueChanged(object sender, EventArgs e)
        {
            Rankx1.Maximum = dimy.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (Approximate)
                Draw1(Convert.ToInt32(Rankx1.Value - 1));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string helpText = @"1. Оберіть вхідний файл (X)
                                2. Оберіть файл для виводу результатів (файл вихідних даних)
                                3. Оберіть по одному або множинним вибором файли Y.
                                4. Натисніть кнопку ""Виконати""";
            MessageBox.Show(helpText, "Help Box!");
        }


       
    }
}

//tPoly & Polynom(int type, int n, double a, double den)

//{
//            tPoly term(1.0 / den, a / den);
//            tPoly accum(1.0);
//            tPoly pattern =
//                Polynom(type, n);
//            tPoly res(0.0);
//            for (int p = 0;
//                p <=
//                n;
//                p++)
//            {
//                res = res +
//                      pattern[p] * accum;
//                accum = accum *
//                        term;
//            }

//            return res;
//        } 
//    }
