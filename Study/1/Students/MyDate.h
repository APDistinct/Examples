#pragma once

class MyDate
{
public:
	MyDate(int day, int month, int year);
	MyDate();
	~MyDate();
	int Year;  // ���
	int Month; // �����
	int Day;   // ����
	bool Grate(MyDate d2);
	bool Less(MyDate d2);
	bool Equal(MyDate d2);
};

