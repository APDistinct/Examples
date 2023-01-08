#include "stdafx.h"
#include <vector>
#include "Student.h"


Student::Student()
{
	kind = 0;
}


Student::~Student()
{
}

// Чтение данных по одному студенту из строки с разделителем ";"
void Student::GetData(string str)
{
	vector<string> vec;
	std::string::size_type found0 = -1;
	std::string::size_type found1 = str.find_first_of(";");
	while (found1 != std::string::npos)
	{
		string ss = str.substr(found0+1, found1 - found0-1);
		vec.push_back(ss);		
		found0 = found1;
		found1 = str.find_first_of(";", found0 + 1);
	}

	code = vec[0];
	surname = vec[1];
	name = vec[2];
	patronymic = vec[3];
	depart = vec[5];
	group = vec[6];
	string dateB = vec[4];
	found1 = dateB.find_first_of(".", 0);
	birthDate.Day = stoi(dateB.substr(0, found1));

	found0 = found1;
	found1 = dateB.find_first_of(".", found1+1);
	birthDate.Month = stoi(dateB.substr(found0+1, found1 - found0-1));

	found0 = found1;
	found1 = dateB.find_first_of(".", found1 + 1);
	birthDate.Year = stoi(dateB.substr(found0+1, found1 - found0-1));
	kind = stoi(vec[7]);
	pay = stof(vec[8]);
}

// Запись данных по одному студенту в строку с разделителем ";"
string Student::ToCSV()
{
	char *str = new char[256];
	string s = "";
	s += code + ";";
	s += surname + ";";
	s += name + ";";
	s += patronymic + ";";
	sprintf_s(str, 256, "%02d.%02d.%4d", birthDate.Day, birthDate.Month, birthDate.Year);
	s = s + str + ";";
	s += depart + ";";
	s += group + ";";	
	sprintf_s(str, 256, "%d", kind);
	s = s + str + ";";
	sprintf_s(str, 256, "%f", pay);
	s = s + str + ";";

	return s;
}
