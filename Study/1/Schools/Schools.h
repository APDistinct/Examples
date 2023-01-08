#pragma once
#include "School.h"
class Schools
{
public:
	Schools();
	~Schools();
	bool ReadFromFile(const string FName);
	bool WriteToFile(const string FName);
	int countGoldMedals(int year);
	int countAll(string code, int year);
	int countHigher(string code, int year);
private:
	vector<School> schools;
};

