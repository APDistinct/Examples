#pragma once
#include <string>
#include "MyDate.h"

using namespace std;
class Student
{
public:
	Student();
	~Student();
	MyDate birthDate; // Дата рождения
	string code;  // Персональнй код
	string depart;   // Факультет
	string group;    // Группа
	string surname;  // Фамилия
	string name;     // Имя
	string patronymic; // Отчество
	int kind;  // Вид обучения. 0 - обычное, 1 платное
	double pay; // Сумма платежа
	void GetData(string str);
	string ToCSV();
	
};

