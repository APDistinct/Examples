#include "stdafx.h"
#include "Students.h"

using namespace std;

Students::Students()
{
}


Students::~Students()
{
}
// ���������� ���������� ���������, � ������� �������� date ������, ��� birthDate
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

// ������ ������ �� ����� � ������� � ������������ ";"
bool Students::ReadFromFile(const string FName)
{
	ifstream FRead(FName);
	bool ret = true;
	int bufsize = 256;
	char *buff = new char[bufsize]; // ����� �������������� �������� ������������ �� ����� ������

	Student student;
	//int num = 0;

	try
	{
		if (!FRead.is_open()) // ���� ���� �� ������
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
			FRead.close(); // ��������� ����
		}
	}
	catch (...)
	{
		ret = false;
	}
	delete buff;
}
// ������ ������ � ���� � ������� � ������������ ";"
bool Students::WriteToFile(const string FName)
{
	ofstream FWrite(FName);
	bool ret = true;
	int bufsize = 256;
	char *buff = new char[bufsize]; // ����� �������������� �������� ������������ �� ����� ������

	Student student;
	//int num = 0;

	try
	{
		if (!FWrite.is_open()) // ���� ���� �� ������
		{
			return false;
		}
		else
		{
			int num = students.size();
			for (int i = 0; i < num; ++i)
				FWrite << students[i].ToCSV() << endl;
			FWrite.close(); // ��������� ����
		}
	}
	catch (...)
	{
		ret = false;
	}
	return ret;
}

// ���������� �������� � ������
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