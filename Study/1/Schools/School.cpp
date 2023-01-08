#include "stdafx.h"
#include "School.h"

using namespace std;

School::School()
{
}


School::~School()
{
}

int School::countGoldMedals(int year)
{
	int num = YearInfo.size();
	int sum = 0;
	// Перебор по годам
	for (int i = 0; i < num; ++i)
	{
		if(YearInfo[i].Year == year)
		  sum += YearInfo[i].CountGoldMedal;  // Количество золотых медалей
	}
	return sum;
}

int School::countAll(int year)
{
	int num = YearInfo.size();
	int sum = 0;
	// Перебор по годам
	for (int i = 0; i < num; ++i)
	{
		if (YearInfo[i].Year == year)
			sum += YearInfo[i].CountAll;  // Количество выпускников
	}
	return sum;
}

int School::countHigher(int year)
{
	int num = YearInfo.size();
	int sum = 0;
	// Перебор по годам
	for (int i = 0; i < num; ++i)
	{
		if (YearInfo[i].Year == year)
			sum += YearInfo[i].CountHigher;  // Количество выпускников поступивших в ВУЗы
	}
	return sum;
}

void School::GetData(string str)
{
	//int nbuf = 256;	
	//char s1[256], s2[256], s3[256], s4[256], s5[256], s6[256];

	vector<string> vec;
	std::string::size_type found0 = -1;
	std::string::size_type found1 = str.find_first_of(";");
	while (found1 != std::string::npos)
	{
		string ss = str.substr(found0 + 1, found1 - found0 - 1);
		vec.push_back(ss);
		found0 = found1;
		found1 = str.find_first_of(";", found0 + 1);
	}	
	code = vec[0];
	Info info;
	info.Year = stoi(vec[1]);
	info.CountGoldMedal = stoi(vec[2]);
	info.CountSilverMedal = stoi(vec[3]);
	info.CountAll = stoi(vec[4]);
	info.CountHigher = stoi(vec[5]);
	info.CountMiddle = stoi(vec[6]);
	YearInfo.push_back(info);
}

void School::ToCSV(vector<string> &v)
{
	//string code;
	//SchoolKind kind;  // Вид заведения
	//vector<Info> YearInfo;
	/*int Year;
	int CountAll;
	int CountHigher;
	int CountMiddle;
	int CountGoldMedal;
	int CountSilverMedal;
*/
	char *str = new char[256];
	int num = YearInfo.size();
	for (int i = 0; i < num; ++i)
	{
		string s = code + ";";
		sprintf_s(str, 256, "%d", YearInfo[i].Year);
		s = s + str + ";";
		sprintf_s(str, 256, "%d", YearInfo[i].CountAll);
		s = s + str + ";";
		sprintf_s(str, 256, "%d", YearInfo[i].CountGoldMedal);
		s = s + str + ";";
		sprintf_s(str, 256, "%d", YearInfo[i].CountSilverMedal);
		s = s + str + ";";
		sprintf_s(str, 256, "%d", YearInfo[i].CountHigher);
		s = s + str + ";";
		sprintf_s(str, 256, "%d", YearInfo[i].CountMiddle);
		s = s + str + ";";
		v.push_back(s);
	}
	return ;
}
