// Students.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <conio.h>  
#include "Student.h"
#include "Students.h"

int main()
{
	setlocale(LC_ALL, "rus");	
	Students sts;
	sts.ReadFromFile("s-list.txt");
	cout << "Введите год ";
	int year;
	cin >> year;
	MyDate *d = new MyDate(1, 1, year);
	int col = sts.GetYoung(*d);
	cout << "Количество студентов, родившихся после 1.1." << year << "  " << col << endl;
	delete d;
	Student st;
	string ss = "023578;Бицадзе;Автандил;Иванович; 11.03.1923;первый факультет;2012-бис;0;0;";
	st.GetData(ss);
	if (sts.AddNew(st))
	{
		cout << "В список студентов добавлен " << st.surname << " " << st.name << " " << st.patronymic << endl;
	}
	else
	{
		cout << "В список студентов не удалось добавить " << st.surname << " " << st.name << " " << st.patronymic << endl;
	}
	sts.WriteToFile("s-list.out");
	_getch();
    return 0;
}

