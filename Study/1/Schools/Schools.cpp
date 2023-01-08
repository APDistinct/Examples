#include "stdafx.h"
#include "Schools.h"


Schools::Schools()
{
}


Schools::~Schools()
{
}

int Schools::countAll(string code, int year)
{
	int sum = 0;
	
	int num = schools.size();
	bool find = false;
	int index = 0;
	// ������� ���� ������� ���������
	for (int i = 0; i < num; ++i)
	{
		find = schools[i].code == code;
		if (find)
		{
			index = i;  // �������
			break;
		}
	}
	if (find)
	{
		// ������� - �������
		sum = schools[index].countAll(year);		
	}
	return sum;
}

int Schools::countHigher(string code, int year)
{
	int sum = 0;

	int num = schools.size();
	bool find = false;
	int index = 0;
	// ������� ���� ������� ���������
	for (int i = 0; i < num; ++i)
	{
		find = schools[i].code == code;
		if (find)
		{
			index = i;  // �������
			break;
		}
	}
	if (find)
	{
		// ������� - �������
		sum = schools[index].countHigher(year);
	}
	return sum;
}

int Schools::countGoldMedals(int year)
{
	int num = schools.size();
	int sum = 0;
	// ������� ���� ������� ���������
	for (int i = 0; i < num; ++i)
	{
		sum += schools[i].countGoldMedals(year);  // �� ������� �������� ��������� ���������� �����
	}
	return sum;
}

bool Schools::ReadFromFile(const string FName)
{
	ifstream FRead(FName);
	bool ret = true;
	int bufsize = 256;
	char *buff = new char[bufsize]; // ����� �������������� �������� ������������ �� ����� ������

	
	//int num = 0;

	try
	{
		if (!FRead.is_open()) // ���� ���� �� ������
		{
			return false;
		}
		else
		{			
			while (FRead.getline(buff, bufsize))  // ������ ������ �� �����
			{				
				string str = buff;  // ������� � string
				std::string::size_type found0 = -1;
				std::string::size_type found1 = str.find_first_of(";");
				//� ����� ����� ��������� ������
				string code = str.substr(found0 + 1, found1 - found0 - 1);

				//����� ����� � ����� �����. ���� ��� - ��������
				int num = schools.size();
				bool find = false;
				School school;
				int index = 0;
				for (int i = 0; i < num; ++i)
				{
					find = schools[i].code == code;
					if (find)
					{
						index = i;  // �������
						break;						
					}
				}
				if (!find)  
				{
					// �� ������� - ���������
				
					school.code = code;
					schools.push_back(school);
					index = schools.size()-1;  // ����� - ���������
				}

				schools[index].GetData(str);  // ������ ������
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
bool Schools::WriteToFile(const string FName)
{
	ofstream FWrite(FName);
	bool ret = true;
	int bufsize = 256;
	char *buff = new char[bufsize]; // ����� �������������� �������� ������������ �� ����� ������

	School school;
	//int num = 0;

	try
	{
		if (!FWrite.is_open()) // ���� ���� �� ������
		{
			return false;
		}
		else
		{
			int num = schools.size();
			vector<string> text;
			// ������� ���� ������� ���������
			for (int i = 0; i < num; ++i)
			{
				schools[i].ToCSV(text);  // �� ������� �������� ��������� ���������� �����
			}
			num = text.size();
			// ��� ���������� ������ �� ���� ������
			for (int i = 0; i < num; ++i)
			{
				FWrite << text[i] << endl;
			}
			FWrite.close(); // ��������� ����
		}
	}
	catch (...)
	{
		ret = false;
	}
	return ret;
}