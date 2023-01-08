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
	int Year;  // ���
	int CountAll; // ���������� �����������
	int CountGoldMedal;  // ���������� ������� �������
	int CountSilverMedal;  // ���������� ���������� �������
	int CountHigher;  // ���������� ����������� � ������ ���������
	int CountMiddle;  // ���������� ����������� � ������� ���������	
};

class School
{
public:
	School();
	~School();
	string code;  // ��� ���������
	SchoolKind kind;  // ��� ���������
	vector<Info> YearInfo;	// ���������� � �������� �� �����
	void GetData(string str);  // ��������� ������ �� ������
	void ToCSV(vector<string> &v);  // ������������ ������� ����� �� ������� ������
	int countGoldMedals(int year); // ���������� ������� ����������, �������� - ���
	int countAll(int year);  // ���������� �����������, �������� - ���
	int countHigher(int year);  // ���������� �����������, ����������� � ����, �������� - ���
	//Person director;

};

