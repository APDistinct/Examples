#include "stdafx.h"
#include "MyDate.h"


MyDate::MyDate(int day, int month, int year)
{
	Day = day;
	Month = month;
	Year = year;
}
// Парамтр d2 больше, чем значение
bool MyDate::Grate(MyDate d2)
	{
		bool ret = (Year < d2.Year) || (Year == d2.Year) && (Month < d2.Month) || (Year == d2.Year) && (Month == d2.Month) && (Day < d2.Day);
		return ret;
	}

// Парамтр d2 меньше, чем значение
bool MyDate::Less(MyDate d2)
	{
		bool ret = !Grate(d2) && !Equal(d2);
		return ret;
	}
// Парамтр d2 равен значению
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
