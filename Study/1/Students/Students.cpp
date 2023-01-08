#include "stdafx.h"
#include "Students.h"

using namespace std;

Students::Students()
{
}


Students::~Students()
{
}
// Нахождение количества студентов, у которых параметр date меньше, чем birthDate
int Students::GetYoung(MyDate date)
{
	int num = students.size();
	int count = 0;
	for (int i = 0; i < num; ++i)
	{
		if (students[i].birthDate.Less(date))
		{
			count++;
		}
	}
	return count;
}

// Чтение данных из файла в формате с разделителем ";"
bool Students::ReadFromFile(const string FName)
{
	ifstream FRead(FName);
	bool ret = true;
	int bufsize = 256;
	char *buff = new char[bufsize]; // буфер промежуточного хранения считываемого из файла текста

	Student student;
	//int num = 0;

	try
	{
		if (!FRead.is_open()) // если файл не открыт
		{
			return false;
		}
		else
		{
			while (FRead.getline(buff, bufsize))
			{
				//	   FRead >> buff;
				string s = buff;
				student.GetData(s);
				students.push_back(student);
			}
			FRead.close(); // закрываем файл
		}
	}
	catch (...)
	{
		ret = false;
	}
	delete buff;
}
// Запись данных в файл в формате с разделителем ";"
bool Students::WriteToFile(const string FName)
{
	ofstream FWrite(FName);
	bool ret = true;
	int bufsize = 256;
	char *buff = new char[bufsize]; // буфер промежуточного хранения считываемого из файла текста

	Student student;
	//int num = 0;

	try
	{
		if (!FWrite.is_open()) // если файл не открыт
		{
			return false;
		}
		else
		{
			int num = students.size();
			for (int i = 0; i < num; ++i)
				FWrite << students[i].ToCSV() << endl;
			FWrite.close(); // закрываем файл
		}
	}
	catch (...)
	{
		ret = false;
	}
	return ret;
}

// Добавление студента в список
bool Students::AddNew(Student student)
{
	bool ret = true;
	try
	{
		students.push_back(student);
	}
	catch (...)
	{
		ret = false;
	}
	return ret;
}