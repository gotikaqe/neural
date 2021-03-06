﻿using System;
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
        bool partia = true;

        neural_worker[] xxx = new neural_worker[20];
        neural_worker[] ooo = new neural_worker[20];


        public Form1()
        {
            for (int i = 0; i < 20; i++)
            {
                xxx[i] = new neural_worker();
                ooo[i] = new neural_worker();
            }
            if (File.Exists("save_x.txt")) upload();
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

        private void sort_by_good()
        {
            neural_worker tmp;
            for (int i = 0; i < xxx.Length; i++)
            {
                for (int k = i + 1; k < xxx.Length; k++)
                {
                    if (xxx[i].usefull < xxx[k].usefull)
                    {
                        tmp = xxx[i];
                        xxx[i] = xxx[k];
                        xxx[k] = tmp;
                    }
                    if (ooo[i].usefull < ooo[k].usefull)
                    {
                        tmp = ooo[i];
                        ooo[i] = ooo[k];
                        ooo[k] = tmp;
                    }
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int generations = 20;      //Количество поколений
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
                for (int k = 0; k<20; k++)
                {
                    winner = 0;
                    nul_pole();
					hodov = 0;
                    while (hodov <= 899)
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
                        hodov++;
                        if ((i == generations-1) && (k == 0)) hellomf.WriteLine(tmp_y.ToString() + " " + z.ToString());
                        if (WinCheck.IfWin(pole, tmp_y, z))
                        {
                            winner = 2;
                            break;
                        }
                    }
                    if ((winner == 0) || (winner == 2))
                    {
                        xxx[k].good(hodov, false);
                        ooo[k].good(hodov, true);
                    } else if (winner == 1)
                    {
                        xxx[k].good(hodov, true);
                        ooo[k].good(hodov, false);
                    } 
                }

                sort_by_good();
                for (int k = 10; k < 20; k++)
                    xxx[k].slij(xxx[k - 10], xxx[k - 9]);
                for (int k = 10; k < 20; k++)
                    ooo[k].slij(ooo[k - 10], ooo[k - 9]);
                show_pole(winner);
                progressBar1.Value += 1;
            }
            hellomf.Close();
        }

        private void save_now()
        {
            //public double[,] start_w = new double[900, 100];
            //public double[,,] w = new double[2, 100, 100];
            //public double[,] end_w = new double[100, 900];
            StreamWriter save_x = new StreamWriter(@"save_x.txt");
            StreamWriter save_o = new StreamWriter(@"save_o.txt");

            for (int i = 0; i < 20; i++)
            {
                for (int k = 0; k < 900; k++)
                    for (int z = 0; z < 100; z++)
                        save_x.WriteLine(xxx[i].start_w[k, z]);
                for (int k = 0; k < 2; k++)
                    for (int z = 0; z < 100; z++)
                        for (int j = 0; j < 100; j++)
                            save_x.WriteLine(xxx[i].w[k, z, j]);
                for (int k = 0; k < 100; k++)
                    for (int z = 0; z < 900; z++)
                        save_x.WriteLine(xxx[i].end_w[k, z]);
            }
            for (int i = 0; i < 20; i++)
            {
                for (int k = 0; k < 900; k++)
                    for (int z = 0; z < 100; z++)
                        save_o.WriteLine(ooo[i].start_w[k, z]);
                for (int k = 0; k < 2; k++)
                    for (int z = 0; z < 100; z++)
                        for (int j = 0; j < 100; j++)
                            save_o.WriteLine(ooo[i].w[k, z, j]);
                for (int k = 0; k < 100; k++)
                    for (int z = 0; z < 900; z++)
                        save_o.WriteLine(ooo[i].end_w[k, z]);
            }
            save_x.Close();
            save_o.Close();

        }
        private void upload()
        {
            StreamReader save_x = new StreamReader(@"save_x.txt");
            StreamReader save_o = new StreamReader(@"save_o.txt");

            for (int i = 0; i < 20; i++)
            {
                for (int k = 0; k < 900; k++)
                    for (int z = 0; z < 100; z++)
                        xxx[i].start_w[k, z] = Convert.ToDouble(save_x.ReadLine());
                for (int k = 0; k < 2; k++)
                    for (int z = 0; z < 100; z++)
                        for (int j = 0; j < 100; j++)
                            xxx[i].w[k, z, j] = Convert.ToDouble(save_x.ReadLine());
                for (int k = 0; k < 100; k++)
                    for (int z = 0; z < 900; z++)
                        xxx[i].end_w[k, z] = Convert.ToDouble(save_x.ReadLine());
            }
            for (int i = 0; i < 20; i++)
            {
                for (int k = 0; k < 900; k++)
                    for (int z = 0; z < 100; z++)
                        ooo[i].start_w[k, z] = Convert.ToDouble(save_o.ReadLine());
                for (int k = 0; k < 2; k++)
                    for (int z = 0; z < 100; z++)
                        for (int j = 0; j < 100; j++)
                            ooo[i].w[k, z, j] = Convert.ToDouble(save_o.ReadLine());
                for (int k = 0; k < 100; k++)
                    for (int z = 0; z < 900; z++)
                        ooo[i].end_w[k, z] = Convert.ToDouble(save_o.ReadLine());
            }
            save_x.Close();
            save_o.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            save_now();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (partia == true) { nul_pole(); partia = false; }
            int x, y;
            x = Convert.ToInt32(textBox2.Text);
            y = Convert.ToInt32(textBox3.Text);
            pole[x, y] = 1;
            show_pole(0);
            if (WinCheck.IfWin(pole, x, y)) partia = true;
            int tmp_y = ooo[0].hod(pole);
            int tmp_x = IntDiv30(tmp_y);
            int z = tmp_y - tmp_x * 30;
            pole[tmp_x, z] = 2;
            show_pole(0);
            if (WinCheck.IfWin(pole, tmp_x, z)) partia = true;
        }
    }
}
