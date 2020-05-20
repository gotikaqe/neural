using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace neuro_xox_v2
{
    class neural_worker
    {
        private double k1 = 0.1;
        public double[,] neurons = new double[3, 100];
        public double[,] start_w = new double[900, 100];
        public double[,,] w = new double[2, 100, 100];
        public double[,] end_w = new double[100, 900];
        public double[] neurons_end = new double[900];

        public int usefull = 0;

        public neural_worker()                                //Конструктор класса. Задает рандомные веса.
        {
            Random rr = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i<900; i++)
                for (int k = 0; k<100; k++)
                {
                    start_w[i, k] = rr.NextDouble();
                    end_w[k, i] = rr.NextDouble();
                }
            for (int i = 0; i < 2; i++)
                for (int k = 0; k < 100; k++)
                    for (int z = 0; z < 100; z++)
                        w[i, k, z] = rr.NextDouble();

        }


        public int hod(int[,] pole)                                 //Нейросеть выбирает, куда ей сходить. Поле подается в формате двумерного массива
        {
            for (int i = 0; i < 100; i++)
                neurons[0, i] = f_act(schet(i, pole));
            for (int i = 1; i < 3; i++)
                for (int k = 0; k < 100; k++)
                    neurons[i, k] = f_act(sum_hidden(i, k));

            for (int i = 0; i < 900; i++)
                neurons_end[i] = f_act(sum_end(i));

            double tmp = 0;
            int place = 0;
            for (int i = 0; i < 900; i++) {
                int temp = (int)Math.Truncate((double)(i / 30));
                if ((neurons_end[i] >= tmp) && (pole[temp, i-temp*30] == 0)) place = i;
            }

            return place;


        }


        private double schet(int current_neuron, int[,] pole1)  //Функция суммы произведения весов от входного слоя к скрытому с входными данными
        {
            double tmp = 0;
            for (int i = 0; i < 30; i++)
                for (int k = 0; k < 30; k++)
                    tmp += pole1[i, k] * start_w[i * 30 + k, current_neuron];
            return tmp;
        }

        private double sum_hidden(int a, int b)             //Функция суммы на скрытом слое
        {
            double tmp = 0;
            for (int i = 0; i < 99; i++)
                tmp += neurons[a - 1, i] * w[a - 1, i, b];
            tmp += 1 * w[a - 1, 99, b];
            return tmp;

        }
        private double sum_end(int current)             //Функция суммы для выходных нейронов
        {
            double tmp = 0;
            for (int i = 0; i < 100; i++)
                tmp += neurons[2, i] * end_w[i, current];
            return tmp;
        }


        public void slij(neural_worker a, neural_worker b)
        {
            Random rr = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 900; i++)
                for (int k = 0; k < 100; k++)
                {
                    if (a.start_w[i, k] < b.start_w[i, k]) start_w[i, k] = b.start_w[i, k] - k1;
                    else start_w[i, k] = b.start_w[i, k] + k1;
                    if (rr.Next() % 250 == 0) start_w[i, k] /= 2;
                }
            for (int i = 0; i<2; i++)
                for (int k = 0; k < 100; k++)
                    for (int z = 0; z < 100; z++) { 
                        if (a.w[i, k, z] > b.w[i, k, z]) w[i, k, z] = b.w[i, k, z] + k1;
                        else w[i, k, z] = b.w[i, k, z] - k1;
                        if (rr.Next() % 600 == 0) w[i, k, z] /= 2;
                    }

            for (int i = 0; i<100; i++)
                for (int k = 0; k<900; k++)
                {
                    if (a.end_w[i, k] > b.end_w[i, k]) end_w[i, k] = b.end_w[i, k] + k1;
                    else end_w[i, k] = b.end_w[i, k] - k1;
                    if (rr.Next() % 250 == 0) end_w[i, k] /= 2;
                }

        }
        public void good(int hodov, bool win)
        {
            usefull = Convert.ToInt32(win) * (900 - hodov) - hodov;
        }


        private double f_act(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }


    }
}
