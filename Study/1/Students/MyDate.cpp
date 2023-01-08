#include "stdafx.h"
#include "MyDate.h"


MyDate::MyDate(int day, int month, int year)
{
	Day = day;
	Month = month;
	Year = year;
}
// ������� d2 ������, ��� ��������
bool MyDate::Grate(MyDate d2)
	{
		bool ret = (Year < d2.Year) || (Year == d2.Year) && (Month < d2.Month) || (Year == d2.Year) && (Month == d2.Month) && (Day < d2.Day);
		return ret;
	}

// ������� d2 ������, ��� ��������
bool MyDate::Less(MyDate d2)
	{
		bool ret = !Grate(d2) && !Equal(d2);
		return ret;
	}
// ������� d2 ����� ��������
bool MyDate::Equal(MyDate d2)
	{
		bool ret = (Year == d2.Year) && (Month == d2.Month) && (Day == d2.Day);
		return ret;
	}

MyDate::MyDate()
{
}

MyDate::~MyDate()
{
}
