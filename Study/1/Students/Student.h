#pragma once
#include <string>
#include "MyDate.h"

using namespace std;
class Student
{
public:
	Student();
	~Student();
	MyDate birthDate; // ���� ��������
	string code;  // ����������� ���
	string depart;   // ���������
	string group;    // ������
	string surname;  // �������
	string name;     // ���
	string patronymic; // ��������
	int kind;  // ��� ��������. 0 - �������, 1 �������
	double pay; // ����� �������
	void GetData(string str);
	string ToCSV();
	
};

