﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuro_xox_v2
{
    class WinCheck
    {
		public static int min(int a, int b) //Функция нахождения минимума
		{
			if (a < b) return a;
			return b;
		}


		public static int max(int a, int b) //Функция нахождения максимума
		{
			if (a > b) return a;
			return b;
		}


		public static bool IfWin(int[,] array, int x, int y)
		{
			int current = array[x, y]; //Номер игрока
			int rows = array.GetUpperBound(0) + 1; //Кол-во строк поля
			int columns = array.GetUpperBound(1) + 1; //Кол-во столбцов поля
			int line = 0; //Переменная для хранения длины непрерывного ряда
			bool ifline = false; //Переменная для проверки того продолжается ли ряд

			for (int i = max(x - 4, 0); i < min(x + 4, columns); i++) //Подсчёт ряда по горизонтали
			{
				if (ifline) //Если до этого начался ряд
				{
					if (array[i, y] != current) //Но данная клетка не заполнена нашим игроком
					{
						if (line >= 5) return true; //Проверяем длину закончевшегося ряда на выиграшность
						line = 0; //В противном случае обнуляем длину ряда
						ifline = false; //И говорим, что ряд не начат
					}
					else line++; //Если же ряд продолжается инкрементируем его длину
				}
				else if (array[i, y] == current) //Если ряд не начат, проверяем не начинается ли он сейчас
				{
					line = 1; //В таком случае ставим длину ряда равную единице
					ifline = true; //И сообщаем о начале ряда
				}
			}
			if (line >= 5) return true; //Проверяем выигрыш, так как если ряд находится в самом конце, его окончание не будет обрабатываться в цикле.
			line = 0; //Обнуляем переменные
			ifline = false;

			for (int i = max(y - 4, 0); i < min(y + 4, rows); i++) //Подсчёт ряда по вертикали
			{
				if (ifline) //Если до этого начался ряд
				{
					if (array[x, i] != current) //Но данная клетка не заполнена нашим игроком
					{
						if (line >= 5) return true; //Проверяем длину закончевшегося ряда на выиграшность
						line = 0; //В противном случае обнуляем длину ряда
						ifline = false; //И говорим, что ряд не начат
					}
					else line++; //Если же ряд продолжается инкрементируем его длину
				}
				else if (array[x, i] == current) //Если ряд не начат, проверяем не начинается ли он сейчас
				{
					line = 1; //В таком случае ставим длину ряда равную единице
					ifline = true; //И сообщаем о начале ряда
				}
			}
			if (line >= 5) return true; //Проверяем выигрыш, так как если ряд находится в самом конце, его окончание не будет обрабатываться в цикле.
			line = 0; //Обнуляем переменные
			ifline = false;

			for (int i = -min(4, min(x, y)); i < min(5, min(rows - y, columns - x)); i++) //Подсчёт главной диагонали
			{
				if (ifline) //Если до этого начался ряд
				{
					if (array[x + i, y + i] != current) //Но данная клетка не заполнена нашим игроком
					{
						if (line >= 5) return true; //Проверяем длину закончевшегося ряда на выиграшность
						line = 0; //В противном случае обнуляем длину ряда
						ifline = false; //И говорим, что ряд не начат
					}
					else line++; //Если же ряд продолжается инкрементируем его длину
				}
				else if (array[x + i, y + i] == current) //Если ряд не начат, проверяем не начинается ли он сейчас
				{
					line = 1; //В таком случае ставим длину ряда равную единице
					ifline = true; //И сообщаем о начале ряда
				}
			}
			if (line >= 5) return true; //Проверяем выигрыш, так как если ряд находится в самом конце, его окончание не будет обрабатываться в цикле.
			line = 0; //Обнуляем переменные
			ifline = false;

			for (int i = -min(4, min(x, columns - y)-1); i < min(5, min(rows - x, y)); i++) //Подсчёт побочной диагонали
			{
				if (ifline) //Если до этого начался ряд
				{
					if (array[x + i, y - i] != current) //Но данная клетка не заполнена нашим игроком
					{
						if (line >= 5) return true; //Проверяем длину закончевшегося ряда на выиграшность
						line = 0; //В противном случае обнуляем длину ряда
						ifline = false; //И говорим, что ряд не начат
					}
					else line++; //Если же ряд продолжается инкрементируем его длину
				}
				else if (array[x + i, y - i] == current) //Если ряд не начат, проверяем не начинается ли он сейчас
				{
					line = 1; //В таком случае ставим длину ряда равную единице
					ifline = true; //И сообщаем о начале ряда
				}
			}
			if (line >= 5) return true; //Проверяем выигрыш, так как если ряд находится в самом конце, его окончание не будет обрабатываться в цикле.

			return false; //Если выполнение функции дошло до этой строки - никто не выиграл за этот ход.
		}
	}
}
