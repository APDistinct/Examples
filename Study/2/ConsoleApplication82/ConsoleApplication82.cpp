// ConsoleApplication82.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <vector>
#include <fstream>
#include <iostream>
#include <string>
#include <conio.h>  

using namespace std;

typedef vector<string> v_file;  // ������ ����� 

// ��������� ���������� � ��������� ���������
bool is_eq(string strtemp)
{	
	//if (strtemp.length() == 1)
	//	return true;
	return(strtemp[0] == strtemp[strtemp.length() - 1]);
}

// �������� ��������� ���������
bool do_work(string FName, v_file &mas)
{
	ifstream FRead(FName);  // ���� ��� ������	
	bool ret = true;
	string buff;  // ����� ��� ������ ������ �� �����

	try
	{
		if (!FRead.is_open()) // ���� ���� �� ������
		{	
			return false;
		}
		else
		{
			int strnum = 1;  // ����� ������� ������
			string delimeter = " ";  // ����������� ����

			//���� - ���� �� ����� ��������� ������
			while (getline(FRead, buff))   // ������ ������ �� ����� � �����
			{
				cout << strnum << ":  "  << buff << std::endl;  // ����� ������ - ����� � ����������
				string strtemp;  // ��������������� ������ 
				std::string::size_type pos0 = 0;   // ������� � ������
				std::string::size_type pos1;       // ������� � ������
				//  ���� ���� � ������ ������� ��������� ��������� �����������
				while ((pos1 = buff.find_first_of(delimeter, pos0)) != std::string::npos)
				{
					strtemp = buff.substr(pos0, pos1 - pos0);  // ����� �������� ������ �� ���������� �� ������� �������
					if (is_eq(strtemp))
					{
						// ��������� ��������� �����
						mas.push_back("����� " + strtemp + " ������ " + to_string(strnum) + "  ������� "+ to_string((int)pos0));
					}
					pos0 = pos1 + 1;  // ������� ��������� �� ������ �������������� ����� ������
				}
				strtemp = buff.substr(pos0);  // ��������� ��, ��� �������� �� ��������� ���������� ������������
				if (is_eq(strtemp))
				{
					// ��������� ��������� �����
					mas.push_back("����� " + strtemp + " ������ " + to_string(strnum) + "  ������� " + to_string((int)pos0));
				}
				strnum++;  // ������� ������������ �����
			}
			FRead.close(); // ��������� ����			
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
		cout << mas[i] << std::endl;  // ����� ���������� � �����
		_getch();  // ��� ������� �������
	}

}
int main()
{
	v_file mas; // ������ ����� ��� ���������� �������� � ��������� ������
	string filename;
	setlocale(LC_ALL, "rus");
	cout << "������� ��� �����  ";
	cin >> filename;	
	if (do_work(filename, mas))
	{
		int len = mas.size();  // ����� � ������� - ���������� ��������� ����
		cout << std::endl << "���������� ������ � ����� " << filename << std::endl;
		cout << "����� ������� " << len << " ����" <<std::endl;
		show_text(mas);
	}
	else
	{
		cout << "������ � ������  ";
	}
	cout << "����� ���������  ";
	_getch();
	return 0;
}

