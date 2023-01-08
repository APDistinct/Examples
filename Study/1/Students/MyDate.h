#pragma once

class MyDate
{
public:
	MyDate(int day, int month, int year);
	MyDate();
	~MyDate();
	int Year;  // Год
	int Month; // Месяц
	int Day;   // День
	bool Grate(MyDate d2);
	bool Less(MyDate d2);
	bool Equal(MyDate d2);
};

