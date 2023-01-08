// Schools.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <conio.h>  
#include "School.h"
#include "Schools.h"

int main()
{
	setlocale(LC_ALL, "rus");
	//School st;
	//string ss = "023526;Иванов;  Вася; Иванович; 11.03.2000;первый факультет;2012-бис;0;0;";
	//st.GetData(ss);
	Schools sts;
	sts.ReadFromFile("Schools.txt");
	cout << "Введите год ";
	int year;
	cin >> year;
	int col = sts.countGoldMedals(year);
	cout << "Количество золотых медалистов в " << year << " году  " << col << endl;
	string code = "001s01";
	int numAll = sts.countAll(code, year);
	cout << "Количество выпускников в  " << year << " году в школе " << code << "  " << numAll << endl;
	int numH = sts.countHigher(code, year);
	cout << "Количество выпускников в " << year << "  году в школе " << code << " поступивших в ВУЗы " << numH << endl;
	if (numAll != 0)
	{
		double fraction = 1.0*numH / numAll * 100;
		cout << "Процент - " << fraction << "%" << endl;
	}
	sts.WriteToFile("Schools.out");
	_getch();
    return 0;
}

