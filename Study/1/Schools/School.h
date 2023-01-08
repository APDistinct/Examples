#pragma once
#include <vector>
#include <fstream>
#include <iostream>
#include <string>

using namespace std;

enum SchoolKind
{
	school=0, lyceum, gymnasium
}; 

class Info
{
public:
	int Year;  // Год
	int CountAll; // Количество выпускников
	int CountGoldMedal;  // Количество золотых медалей
	int CountSilverMedal;  // Количество серебряных медалей
	int CountHigher;  // Количество поступивших в высшие заведения
	int CountMiddle;  // Количество поступивших в средние заведения	
};

class School
{
public:
	School();
	~School();
	string code;  // Код заведения
	SchoolKind kind;  // Вид заведения
	vector<Info> YearInfo;	// Информация о выпусках по годам
	void GetData(string str);  // Получение данных из строки
	void ToCSV(vector<string> &v);  // Формирование массива строк по массиву данных
	int countGoldMedals(int year); // Количество золотых медалистов, параметр - год
	int countAll(int year);  // Количество выпускников, параметр - год
	int countHigher(int year);  // Количество выпускников, поступивших в ВУЗы, параметр - год
	//Person director;

};

