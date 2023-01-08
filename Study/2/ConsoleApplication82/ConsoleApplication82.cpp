// ConsoleApplication82.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <vector>
#include <fstream>
#include <iostream>
#include <string>
#include <conio.h>  

using namespace std;

typedef vector<string> v_file;  // Вектор строк 

// Равенство начального и конечного элементов
bool is_eq(string strtemp)
{	
	//if (strtemp.length() == 1)
	//	return true;
	return(strtemp[0] == strtemp[strtemp.length() - 1]);
}

// Основная процедура обработки
bool do_work(string FName, v_file &mas)
{
	ifstream FRead(FName);  // Файл для чтения	
	bool ret = true;
	string buff;  // Буфер для чтения строки из файла

	try
	{
		if (!FRead.is_open()) // если файл не открыт
		{	
			return false;
		}
		else
		{
			int strnum = 1;  // Номер текущей строки
			string delimeter = " ";  // Разделитель слов

			//Цикл - пока из файла прочитана строка
			while (getline(FRead, buff))   // Читаем строку из файла в буфер
			{
				cout << strnum << ":  "  << buff << std::endl;  // Вывод строки - номер и содержание
				string strtemp;  // Вспомогательная строка 
				std::string::size_type pos0 = 0;   // Позиция в строке
				std::string::size_type pos1;       // Позиция в строке
				//  Цикл пока в строке найдено следующее вхождение разделителя
				while ((pos1 = buff.find_first_of(delimeter, pos0)) != std::string::npos)
				{
					strtemp = buff.substr(pos0, pos1 - pos0);  // Часть исходной строки от предыдущей до текущей позиции
					if (is_eq(strtemp))
					{
						// Фиксируем найденное слово
						mas.push_back("Слово " + strtemp + " строка " + to_string(strnum) + "  позиция "+ to_string((int)pos0));
					}
					pos0 = pos1 + 1;  // Смещаем указатель на начало необработанной части строки
				}
				strtemp = buff.substr(pos0);  // Добавляем то, что осталось за последним вхождением разделителем
				if (is_eq(strtemp))
				{
					// Фиксируем найденное слово
					mas.push_back("Слово " + strtemp + " строка " + to_string(strnum) + "  позиция " + to_string((int)pos0));
				}
				strnum++;  // Счётчик обработанных строк
			}
			FRead.close(); // закрываем файл			
		}
	}
	catch (...)
	{
		ret = false;
	}
	return ret;
}

void show_text(v_file mas)
{
	int len = mas.size();  
	for (int i = 0; i < len; ++i)
	{
		cout << mas[i] << std::endl;  // Вывод информации о слове
		_getch();  // Ждём нажатия клавиши
	}

}
int main()
{
	v_file mas; // Вектор строк для сохранения описания о найденных словах
	string filename;
	setlocale(LC_ALL, "rus");
	cout << "Введите имя файла  ";
	cin >> filename;	
	if (do_work(filename, mas))
	{
		int len = mas.size();  // Строк в массиве - количество найденных слов
		cout << std::endl << "Результаты поиска в файле " << filename << std::endl;
		cout << "Всего найдено " << len << " слов" <<std::endl;
		show_text(mas);
	}
	else
	{
		cout << "Ошибки в работе  ";
	}
	cout << "Конец просмотра  ";
	_getch();
	return 0;
}

