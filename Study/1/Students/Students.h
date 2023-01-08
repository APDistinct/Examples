#pragma once
#include <vector>
#include <fstream>
#include <iostream>
#include "Student.h"

class Students
{
public:
	Students();
	~Students();
	bool ReadFromFile(const string FName);
	bool WriteToFile(const string FName);
	int GetYoung(MyDate date);
	bool AddNew(Student student);
private:
	vector<Student> students;
};

