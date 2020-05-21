using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace neuro_xox_v2
{
    public partial class Form1 : Form
    {
        string path = @"C:\Users\Admin\Desktop\save.txt";
        //string path = @"C:\Users\Depard42\Desktop\save.txt";
        int[,] pole = new int[30, 30];

        neural_worker[] xxx = new neural_worker[20];
        neural_worker[] ooo = new neural_worker[20];


        public Form1()
        {
            InitializeComponent();
        }
        public int IntDiv30(int x)
        {
            return Convert.ToInt32(Math.Truncate((double)(x / 30)));
        }
        private void show_pole(int ab)
        {
            textBox1.Text = null;
            for (int i = 0; i < 30; i++)
            {
                for (int k = 0; k < 30; k++)
                    textBox1.Text += pole[i, k] + " ";
                textBox1.Text += Environment.NewLine;

            }
            textBox1.Text += ab.ToString();
            int z=0, z1=0;
            for (int i = 0; i < 30; i++)
                for (int k = 0; k < 30; k++)
                {
                    if (pole[i, k] == 1) z++;
                    else if (pole[i, k] == 2) z1++;
                }
            textBox1.Text += "\r\n" + z.ToString() + "   " + z1.ToString();
        }

        private void sort_by_good(neural_worker[] temp)
        {
            neural_worker tmp;
            //int xix = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                tmp = temp[i];
                for (int k = 0; k < temp.Length; k++)
                    if (tmp.usefull <= temp[k].usefull)
                    {
                        temp[i] = temp[k];
                        temp[k] = tmp;
                    }
            }
        }

        private void nul_pole()
        {
            for (int i = 0; i < 30; i++)
                for (int k = 0; k < 30; k++)
                    pole[i, k] = 0;
        }

		private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i<20; i++)
            {
                xxx[i] = new neural_worker();
                ooo[i] = new neural_worker();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int generations = 500;      //Количество поколений
            progressBar1.Value = 0;
            progressBar1.Maximum = generations;
            StreamWriter hellomf = new StreamWriter(@path);
			int hodov = 0;
			int tmp_x = 0;
			int tmp_y = 0;
            int winner = 0;
            int z = 0;
            for (int i = 0; i < generations; i++)
            {
                for (int k = 0; k<5; k++)
                {
                    winner = 0;
                    nul_pole();
					hodov = 0;
                    while (hodov <= 898)
					{
                        tmp_y = xxx[k].hod(pole);
                        tmp_x = IntDiv30(tmp_y);
                        z = tmp_y - tmp_x * 30;
                        pole[tmp_x, z] = 1;
                        if ((i == generations-1) && (k == 0)) hellomf.WriteLine(tmp_x.ToString() + " " + z.ToString());
                        hodov++;
                        if (WinCheck.IfWin(pole, tmp_x, z))
                        {
                            winner = 1;
                            break;
                        }
                        tmp_x = ooo[k].hod(pole);
                        tmp_y = IntDiv30(tmp_x);
                        z = tmp_x - tmp_y * 30;
                        pole[tmp_y, z]=2;
                        if ((i == generations-1) && (k == 0)) hellomf.WriteLine(tmp_y.ToString() + " " + z.ToString());
                        if (WinCheck.IfWin(pole, tmp_y, z))
                        {
                            winner = 2;
                            break;
                        }
                        hodov++;
                    }
                    if (winner == 0)
                    {
                        tmp_y = ooo[k].hod(pole);
                        tmp_x = IntDiv30(tmp_y);
                        z = tmp_y - tmp_x * 30;
                        pole[tmp_x, z] = 2;
                        if ((i == generations-1) && (k == 0)) hellomf.WriteLine(tmp_y.ToString() + " " + z.ToString());
                        if (!WinCheck.IfWin(pole, tmp_x, z))
                        {
                            xxx[k].good(hodov, false);
                            ooo[k].good(-hodov, true);
                        }
                    } else if (winner == 1)
                    {
                        xxx[k].good(hodov, true);
                        ooo[k].good(-hodov, false);
                    } else
                    {
                        xxx[k].good(hodov, false);
                        ooo[k].good(-hodov, true);
                    }
                }

                sort_by_good(xxx);
                sort_by_good(ooo);
                for (int k = 3; k < 5; k++)
                    xxx[k].slij(xxx[k - 3], xxx[k - 2]);
                for (int k = 3; k < 5; k++)
                    ooo[k].slij(ooo[k - 3], ooo[k - 2]);
                show_pole(winner);
                progressBar1.Value += 1;
            }
            hellomf.Close();
        }
    }
}
