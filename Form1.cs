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

namespace neuro_xox_v2
{
    public partial class Form1 : Form
    {

        int[,] pole = new int[30, 30];

        neural_worker[] xxx = new neural_worker[20];
        neural_worker[] ooo = new neural_worker[20];


        public Form1()
        {
            InitializeComponent();
        }

        private void sort_by_good(neural_worker[] temp)
        {
            neural_worker tmp = new neural_worker();
            int xix = 0;
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
			int hodov = 0;
			int tmp_x = 0;
			int tmp_y = 0;
            int winner = 0;
            for (int i = 0; i<1000; i++)
            {
                for (int k = 0; k<20; k++)
                {
                    winner = 0;
                    nul_pole();
					hodov = 0;
                    while (hodov <=898)
					{
						tmp_x = Convert.ToInt32(Math.Truncate((double)(xxx[k].hod(pole)/30)));
						pole[tmp_x, xxx[k].hod(pole) - tmp_x*30] = 1;
                        hodov++;
                        if (WinCheck.IfWin(pole, tmp_x, xxx[k].hod(pole) - tmp_x*30) == 1)
                        {
                            winner = 1;
                            break;
                        }
                        tmp_y = Convert.ToInt32(Math.Truncate((double)(ooo[k].hod(pole) / 30)));
                        int z = ooo[k].hod(pole) - tmp_y * 30;
                        pole[tmp_y, ooo[k].hod(pole) - tmp_y*30]=2;
                        if (WinCheck.IfWin(pole, tmp_y, z)==2)
                        {
                            winner = 2;
                            break;
                        }
                        hodov++;
					}
                    if (winner == 0)
                    {
                        tmp_x = Convert.ToInt32(Math.Truncate((double)(xxx[k].hod(pole) / 30)));
                        pole[tmp_x, xxx[k].hod(pole) - tmp_x*30] = 1;
                        if (WinCheck.IfWin(pole, tmp_x, xxx[k].hod(pole) - tmp_x*30) == 0)
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
                for (int k = 10; k < 20; k++)
                    xxx[k].slij(xxx[k - 10], xxx[k - 9]);
                for (int k = 10; k < 20; k++)
                    ooo[k].slij(ooo[k - 10], ooo[k - 9]);

            }
        }
    }
}
